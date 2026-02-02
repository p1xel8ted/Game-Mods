// Decompiled with JetBrains decompiler
// Type: SpiderHeadManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public EnemySpider spider;

  public void Awake() => this.spider = this.GetComponentInParent<EnemySpider>();

  public void LateUpdate()
  {
    if ((Object) this.spider == (Object) null || this.spider.DisableForces && !this.spider.Attacking || (Object) PlayerFarming.Instance == (Object) null)
      return;
    Vector3 zero = Vector3.zero;
    this.SetDirection((double) (!this.spider.Attacking ? (this.spider.pathToFollow == null || this.spider.pathToFollow.Count <= 0 ? PlayerFarming.Instance.transform.position : this.spider.pathToFollow[this.spider.pathToFollow.Count - 1]) : this.spider.AttackingTargetPosition).y > (double) this.spider.transform.position.y);
  }

  public void SetDirection(bool up)
  {
    this.Spine.skeleton.SetSkin(up ? this.HeadFacingUp : this.Head);
    this.Spine.skeleton.SetSlotsToSetupPose();
  }
}
