// Decompiled with JetBrains decompiler
// Type: WormHeadManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Vector2 upAngle = new Vector2(45f, 135f);
  private bool FacingUp;
  private StateMachine state;

  private void Start() => this.state = this.GetComponentInParent<StateMachine>();

  private void LateUpdate()
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
