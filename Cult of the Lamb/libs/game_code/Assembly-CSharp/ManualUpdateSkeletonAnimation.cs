// Decompiled with JetBrains decompiler
// Type: ManualUpdateSkeletonAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class ManualUpdateSkeletonAnimation : BaseMonoBehaviour
{
  public SkeletonAnimation skeletonAnimation;
  [Range(0.0166666675f, 0.125f)]
  public float timeInterval = 0.0416666679f;
  public float deltaTime;

  public void Start()
  {
    if ((Object) this.skeletonAnimation == (Object) null)
      this.skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    this.skeletonAnimation.Initialize(false);
    this.skeletonAnimation.clearStateOnDisable = false;
    this.skeletonAnimation.enabled = false;
    this.ManualUpdate();
  }

  public void Update()
  {
    this.deltaTime += Time.deltaTime;
    if ((double) this.deltaTime < (double) this.timeInterval)
      return;
    this.ManualUpdate();
  }

  public void ManualUpdate()
  {
    this.skeletonAnimation.Update(this.deltaTime);
    this.skeletonAnimation.LateUpdate();
    this.deltaTime -= this.timeInterval;
  }
}
