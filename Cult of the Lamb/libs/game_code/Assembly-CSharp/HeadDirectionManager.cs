// Decompiled with JetBrains decompiler
// Type: HeadDirectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class HeadDirectionManager : BaseMonoBehaviour
{
  public HeadDirectionManager.Mode CurrentMode;
  public SkeletonAnimation Spine;
  public StateMachine state;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string North;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Horizontal;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Default;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string South;
  public float UpdateValue;

  public void Start() => this.state = this.GetComponent<StateMachine>();

  public void LateUpdate()
  {
    if ((Object) this.Spine == (Object) null || (double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    switch (this.CurrentMode)
    {
      case HeadDirectionManager.Mode.LookAngle:
        this.UpdateValue = this.state.LookAngle;
        break;
      case HeadDirectionManager.Mode.FacingAngle:
        this.UpdateValue = this.state.facingAngle;
        break;
    }
    this.UpdateValue = Utils.Repeat(this.UpdateValue, 360f);
    if ((double) this.UpdateValue <= 22.5 || (double) this.UpdateValue >= 337.5)
    {
      if (!(this.Spine.AnimationName != this.Horizontal))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Horizontal, true);
    }
    else if ((double) this.UpdateValue > 22.5 && (double) this.UpdateValue <= 157.5)
    {
      if (!(this.Spine.AnimationName != this.North))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.North, true);
    }
    else if ((double) this.UpdateValue > 157.5 && (double) this.UpdateValue <= 202.5)
    {
      if (!(this.Spine.AnimationName != this.Horizontal))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Horizontal, true);
    }
    else if ((double) this.UpdateValue > 202.5 && (double) this.UpdateValue <= 247.5)
    {
      if (!(this.Spine.AnimationName != this.Default))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Default, true);
    }
    else if ((double) this.UpdateValue > 247.5 && (double) this.UpdateValue <= 292.5)
    {
      if (!(this.Spine.AnimationName != this.South))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.South, true);
    }
    else
    {
      if ((double) this.UpdateValue <= 292.5 || (double) this.UpdateValue > 337.5 || !(this.Spine.AnimationName != this.Default))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Default, true);
    }
  }

  public void ForceUpdate()
  {
    if ((Object) this.state == (Object) null)
      this.state = this.GetComponent<StateMachine>();
    switch (this.CurrentMode)
    {
      case HeadDirectionManager.Mode.LookAngle:
        this.UpdateValue = this.state.LookAngle;
        break;
      case HeadDirectionManager.Mode.FacingAngle:
        this.UpdateValue = this.state.facingAngle;
        break;
    }
    this.UpdateValue = Utils.Repeat(this.UpdateValue, 360f);
    if ((double) this.UpdateValue <= 22.5 || (double) this.UpdateValue >= 337.5)
    {
      if (!(this.Spine.AnimationName != this.Horizontal))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Horizontal, true);
    }
    else if ((double) this.UpdateValue > 22.5 && (double) this.UpdateValue <= 157.5)
    {
      if (!(this.Spine.AnimationName != this.North))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.North, true);
    }
    else if ((double) this.UpdateValue > 157.5 && (double) this.UpdateValue <= 202.5)
    {
      if (!(this.Spine.AnimationName != this.Horizontal))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Horizontal, true);
    }
    else if ((double) this.UpdateValue > 202.5 && (double) this.UpdateValue <= 247.5)
    {
      if (!(this.Spine.AnimationName != this.Default))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Default, true);
    }
    else if ((double) this.UpdateValue > 247.5 && (double) this.UpdateValue <= 292.5)
    {
      if (!(this.Spine.AnimationName != this.South))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.South, true);
    }
    else
    {
      if ((double) this.UpdateValue <= 292.5 || (double) this.UpdateValue > 337.5 || !(this.Spine.AnimationName != this.Default))
        return;
      this.Spine.AnimationState.SetAnimation(1, this.Default, true);
    }
  }

  public enum Mode
  {
    LookAngle,
    FacingAngle,
  }
}
