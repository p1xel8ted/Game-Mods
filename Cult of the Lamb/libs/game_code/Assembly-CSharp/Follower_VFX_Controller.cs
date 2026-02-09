// Decompiled with JetBrains decompiler
// Type: Follower_VFX_Controller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Follower_VFX_Controller : BaseMonoBehaviour
{
  public GameObject promoteFX;
  public SkeletonAnimation spine;
  public SimpleSpineAnimator simpleSpineAnimator;
  public int followerStartingLayer;
  public int playerStartingLayer;

  public void Start()
  {
  }

  public void PromoteMeVFX()
  {
    this.promoteFX.SetActive(true);
    this.followerStartingLayer = this.spine.gameObject.layer;
    this.playerStartingLayer = PlayerFarming.Instance.gameObject.layer;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.PromoteVFX());
  }

  public IEnumerator PromoteVFX()
  {
    yield return (object) new WaitForSeconds(1f);
    this.spine.gameObject.layer = LayerMask.NameToLayer("VFX");
    PlayerFarming.Instance.gameObject.layer = LayerMask.NameToLayer("VFX");
    BiomeConstants.Instance.ImpactFrameForIn();
    this.simpleSpineAnimator.FlashWhite(true);
    yield return (object) new WaitForSeconds(2f);
    this.simpleSpineAnimator.FlashWhite(false);
    BiomeConstants.Instance.ImpactFrameForOut();
    this.spine.gameObject.layer = this.followerStartingLayer;
    PlayerFarming.Instance.gameObject.layer = this.playerStartingLayer;
  }

  public void Update()
  {
  }
}
