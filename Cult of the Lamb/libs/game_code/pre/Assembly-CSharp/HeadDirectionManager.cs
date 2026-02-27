// Decompiled with JetBrains decompiler
// Type: HeadDirectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class HeadDirectionManager : BaseMonoBehaviour
{
  public HeadDirectionManager.Mode CurrentMode;
  public SkeletonAnimation Spine;
  private StateMachine state;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string North;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Horizontal;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Default;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string South;
  public float UpdateValue;

  private void Start() => this.state = this.GetComponent<StateMachine>();

  private void LateUpdate()
  {
    if ((Object) this.Spine == (Object) null)
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
    this.UpdateValue = Mathf.Repeat(this.UpdateValue, 360f);
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
