// Decompiled with JetBrains decompiler
// Type: ShopKeeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public string _cantAffordAnimation = "cant-afford";
  [SerializeField]
  public string _buyAnimation = "buy";
  [SerializeField]
  public string _talkAnimation = "talk";
  [SerializeField]
  public string _normalAnimation = "animation";
  public Spine.Animation cantAffordAnimation;
  public Spine.Animation canAffordAnimation;
  public SkeletonAnimation skeletonAnimation;
  [SerializeField]
  public GameObject BoughtItemBark;
  [SerializeField]
  public GameObject NormalBark;
  [SerializeField]
  public GameObject CantAffordBark;
  [EventRef]
  public string saleVO;
  [EventRef]
  public string cantAffordVO;
  [EventRef]
  public string saleSfx = "event:/shop/buy";
  [EventRef]
  public string cantAffordSfx = "event:/ui/negative_feedback";

  public void Start()
  {
    this.cantAffordAnimation = this.ShopKeeperSpine.skeleton.Data.FindAnimation(this._cantAffordAnimation);
    this.canAffordAnimation = this.ShopKeeperSpine.skeleton.Data.FindAnimation(this._buyAnimation);
    this.skeletonAnimation = this.shopKeeper.GetComponent<SkeletonAnimation>();
  }

  public void Update()
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

  public void UpdateBark(GameObject bark)
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
