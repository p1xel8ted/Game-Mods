// Decompiled with JetBrains decompiler
// Type: Follower_VFX_Controller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Follower_VFX_Controller : BaseMonoBehaviour
{
  public GameObject promoteFX;
  public SkeletonAnimation spine;
  public SimpleSpineAnimator simpleSpineAnimator;
  private int followerStartingLayer;
  private int playerStartingLayer;

  private void Start()
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

  private IEnumerator PromoteVFX()
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

  private void Update()
  {
  }
}
