// Decompiled with JetBrains decompiler
// Type: ShopKeeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopKeeper : MonoBehaviour
{
  public SkeletonAnimation ShopKeeperSpine;
  public GameObject shopKeeper;
  [SerializeField]
  private string _cantAffordAnimation = "cant-afford";
  [SerializeField]
  private string _buyAnimation = "buy";
  [SerializeField]
  private string _talkAnimation = "talk";
  [SerializeField]
  private string _normalAnimation = "animation";
  private Spine.Animation cantAffordAnimation;
  private Spine.Animation canAffordAnimation;
  private SkeletonAnimation skeletonAnimation;
  [SerializeField]
  private GameObject BoughtItemBark;
  [SerializeField]
  private GameObject NormalBark;
  [SerializeField]
  private GameObject CantAffordBark;
  [EventRef]
  public string saleVO;
  [EventRef]
  public string cantAffordVO;
  [EventRef]
  public string saleSfx = "event:/shop/buy";
  [EventRef]
  public string cantAffordSfx = "event:/ui/negative_feedback";

  private void Start()
  {
    this.cantAffordAnimation = this.ShopKeeperSpine.skeleton.Data.FindAnimation(this._cantAffordAnimation);
    this.canAffordAnimation = this.ShopKeeperSpine.skeleton.Data.FindAnimation(this._buyAnimation);
    this.skeletonAnimation = this.shopKeeper.GetComponent<SkeletonAnimation>();
  }

  private void Update()
  {
  }

  public IEnumerator cantAfford()
  {
    ShopKeeper shopKeeper = this;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot(shopKeeper.cantAffordSfx, shopKeeper.transform.position);
    AudioManager.Instance.PlayOneShot(shopKeeper.cantAffordVO, shopKeeper.gameObject.transform.position);
    shopKeeper.UpdateBark(shopKeeper.CantAffordBark);
    if ((Object) shopKeeper.ShopKeeperSpine == (Object) null && (Object) shopKeeper.shopKeeper != (Object) null)
      shopKeeper.ShopKeeperSpine = shopKeeper.skeletonAnimation;
    if (!((Object) shopKeeper.ShopKeeperSpine == (Object) null) && shopKeeper.ShopKeeperSpine.gameObject.activeInHierarchy && shopKeeper.cantAffordAnimation != null)
    {
      shopKeeper.ShopKeeperSpine.AnimationState.SetAnimation(0, shopKeeper.cantAffordAnimation, false);
      shopKeeper.ShopKeeperSpine.AnimationState.AddAnimation(0, shopKeeper._normalAnimation, true, 0.0f);
    }
  }

  private void UpdateBark(GameObject bark)
  {
    if ((Object) this.NormalBark != (Object) null)
      this.NormalBark.SetActive(false);
    if ((Object) this.BoughtItemBark != (Object) null)
      this.BoughtItemBark.SetActive(false);
    if ((Object) this.CantAffordBark != (Object) null)
      this.CantAffordBark.SetActive(false);
    bark.SetActive(true);
  }

  public IEnumerator boughtItem()
  {
    ShopKeeper shopKeeper = this;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot(shopKeeper.saleSfx, shopKeeper.transform.position);
    AudioManager.Instance.PlayOneShot(shopKeeper.saleVO, shopKeeper.gameObject.transform.position);
    shopKeeper.UpdateBark(shopKeeper.BoughtItemBark);
    if ((Object) shopKeeper.ShopKeeperSpine == (Object) null && (Object) shopKeeper.shopKeeper != (Object) null && shopKeeper.shopKeeper.activeInHierarchy)
      shopKeeper.ShopKeeperSpine = shopKeeper.skeletonAnimation;
    if (!((Object) shopKeeper.ShopKeeperSpine == (Object) null) && shopKeeper.ShopKeeperSpine.gameObject.activeInHierarchy && shopKeeper.canAffordAnimation != null)
    {
      shopKeeper.ShopKeeperSpine.AnimationState.SetAnimation(0, shopKeeper.canAffordAnimation, false);
      shopKeeper.ShopKeeperSpine.AnimationState.AddAnimation(0, shopKeeper._normalAnimation, true, 0.0f);
      if ((Object) shopKeeper.NormalBark != (Object) null)
        shopKeeper.NormalBark.SetActive(false);
      if ((Object) shopKeeper.BoughtItemBark != (Object) null)
        shopKeeper.BoughtItemBark.SetActive(true);
    }
  }
}
