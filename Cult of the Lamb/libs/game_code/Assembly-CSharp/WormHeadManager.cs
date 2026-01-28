// Decompiled with JetBrains decompiler
// Type: WormHeadManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class WormHeadManager : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string Head;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string HeadFacingUp;
  [SerializeField]
  public Vector2 upAngle = new Vector2(45f, 135f);
  public bool FacingUp;
  public StateMachine state;

  public void Start() => this.state = this.GetComponentInParent<StateMachine>();

  public void LateUpdate()
  {
    if ((double) this.state.facingAngle >= (double) this.upAngle.x && (double) this.state.facingAngle <= (double) this.upAngle.y)
    {
      if (this.FacingUp)
        return;
      this.Spine.skeleton.SetSkin(this.HeadFacingUp);
      this.Spine.skeleton.SetSlotsToSetupPose();
      this.FacingUp = true;
    }
    else
    {
      if (!this.FacingUp)
        return;
      this.Spine.skeleton.SetSkin(this.Head);
      this.Spine.skeleton.SetSlotsToSetupPose();
      this.FacingUp = false;
    }
  }
}
