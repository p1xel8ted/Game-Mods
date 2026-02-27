// Decompiled with JetBrains decompiler
// Type: ManualUpdateSkeletonAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class ManualUpdateSkeletonAnimation : BaseMonoBehaviour
{
  public SkeletonAnimation skeletonAnimation;
  [Range(0.0166666675f, 0.125f)]
  public float timeInterval = 0.0416666679f;
  private float deltaTime;

  private void Start()
  {
    if ((Object) this.skeletonAnimation == (Object) null)
      this.skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    this.skeletonAnimation.Initialize(false);
    this.skeletonAnimation.clearStateOnDisable = false;
    this.skeletonAnimation.enabled = false;
    this.ManualUpdate();
  }

  private void Update()
  {
    this.deltaTime += Time.deltaTime;
    if ((double) this.deltaTime < (double) this.timeInterval)
      return;
    this.ManualUpdate();
  }

  private void ManualUpdate()
  {
    this.skeletonAnimation.Update(this.deltaTime);
    this.skeletonAnimation.LateUpdate();
    this.deltaTime -= this.timeInterval;
  }
}
