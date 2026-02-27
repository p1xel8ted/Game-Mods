// Decompiled with JetBrains decompiler
// Type: SpiderHeadManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class SpiderHeadManager : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string Head;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string HeadFacingUp;
  private EnemySpider spider;

  private void Awake() => this.spider = this.GetComponentInParent<EnemySpider>();

  private void LateUpdate()
  {
    if ((Object) this.spider == (Object) null || this.spider.DisableForces && !this.spider.Attacking || (Object) PlayerFarming.Instance == (Object) null)
      return;
    Vector3 zero = Vector3.zero;
    this.SetDirection((double) (!this.spider.Attacking ? (this.spider.pathToFollow == null || this.spider.pathToFollow.Count <= 0 ? PlayerFarming.Instance.transform.position : this.spider.pathToFollow[this.spider.pathToFollow.Count - 1]) : this.spider.AttackingTargetPosition).y > (double) this.spider.transform.position.y);
  }

  private void SetDirection(bool up)
  {
    this.Spine.skeleton.SetSkin(up ? this.HeadFacingUp : this.Head);
    this.Spine.skeleton.SetSlotsToSetupPose();
  }
}
