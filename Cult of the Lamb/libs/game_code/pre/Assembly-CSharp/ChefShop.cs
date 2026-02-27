// Decompiled with JetBrains decompiler
// Type: ChefShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChefShop : MonoBehaviour
{
  [SerializeField]
  private GameObject snailWarningConvo_0;
  [SerializeField]
  private GameObject snailWarningConvo_1;
  [SerializeField]
  private GameObject snailWarningConvo_2;
  [SerializeField]
  private GameObject snailDoublePrices;
  [SerializeField]
  private shopKeeperManager shopKeeperManager_0;
  [SerializeField]
  private shopKeeperManager shopKeeperManager_1;
  [SerializeField]
  private Health snailhealth;
  [SerializeField]
  private Health shopKeeperHealth;
  [SerializeField]
  private AnimateOnCollision _animateOnCollision;
  [SerializeField]
  private List<MonoBehaviour> chefComponentsToEnable;
  [SerializeField]
  private List<MonoBehaviour> chefComponentsToDisable;
  [SerializeField]
  private List<MonoBehaviour> chefComponentsToDisableDead;
  [SerializeField]
  private SkeletonAnimation shopKeep;
  [SerializeField]
  private spineChangeAnimationSimple[] worms;
  private bool touchedSnail;
  private int warnings;
  private float attackedTimestamp;

  private void Start()
  {
    DataManager.Instance.HasMetChefShop = true;
    if (DataManager.Instance.ShopKeeperChefState == 1)
    {
      this.warnings = 4;
      foreach (Behaviour behaviour in this.chefComponentsToEnable)
        behaviour.enabled = true;
      foreach (Behaviour behaviour in this.chefComponentsToDisable)
        behaviour.enabled = false;
      foreach (spineChangeAnimationSimple worm in this.worms)
        worm.Play();
    }
    else if (DataManager.Instance.ShopKeeperChefState == 2)
    {
      this.warnings = 4;
      foreach (Behaviour behaviour in this.chefComponentsToDisableDead)
        behaviour.enabled = false;
      if (this.shopKeep.AnimationState != null)
        this.shopKeep.AnimationState.SetAnimation(0, "scared", true);
    }
    this.snailhealth.OnHit += new Health.HitAction(this.HitSnail);
  }

  private IEnumerator EnragedIE()
  {
    ChefShop chefShop = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(chefShop.shopKeep.gameObject, 5f);
    yield return (object) new WaitForSeconds(0.75f);
    chefShop.shopKeep.AnimationState.SetAnimation(0, "furious", true);
    AudioManager.Instance.PlayOneShot("event:/dialogue/shop_shrimp_rakshasa/angry_rakshasa", chefShop.shopKeep.gameObject);
    yield return (object) new WaitForSeconds(2.6f);
    GameManager.GetInstance().OnConversationEnd();
    foreach (Behaviour behaviour in chefShop.chefComponentsToEnable)
      behaviour.enabled = true;
    foreach (Behaviour behaviour in chefShop.chefComponentsToDisable)
      behaviour.enabled = false;
    DataManager.Instance.ShopKeeperChefState = 1;
    DataManager.Instance.ShopKeeperChefEnragedDay = TimeManager.CurrentDay + 1;
    chefShop.shopKeeperHealth.OnDie += new Health.DieAction(chefShop.ChefDied);
    chefShop.checkMusic();
    foreach (spineChangeAnimationSimple worm in chefShop.worms)
      worm.Play();
  }

  private void ChefDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.Shop);
  }

  private void OnEnable()
  {
    if (this.shopKeeperHealth.enabled)
      this.shopKeeperHealth.OnDie += new Health.DieAction(this.ChefDied);
    this.checkMusic();
  }

  private void OnDisable() => this.shopKeeperHealth.OnDie -= new Health.DieAction(this.ChefDied);

  private void checkMusic()
  {
    if (DataManager.Instance.ShopKeeperChefState == 1)
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
    else
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.Shop);
  }

  private void OnDestroy() => this.snailhealth.OnHit -= new Health.HitAction(this.HitSnail);

  public void TouchSnail()
  {
    if (this.touchedSnail)
      return;
    this.touchedSnail = true;
    this.snailWarningConvo_0.SetActive(true);
    ++this.warnings;
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.2f);
  }

  private void HitSnail(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((double) Time.time < (double) this.attackedTimestamp)
      return;
    this._animateOnCollision.PushPlayer();
    if (this.warnings == 0)
      this.snailWarningConvo_0.SetActive(true);
    else if (this.warnings == 1)
      this.snailWarningConvo_1.SetActive(true);
    else if (this.warnings == 2)
      this.snailWarningConvo_2.SetActive(true);
    else if (this.warnings == 3)
      this.StartCoroutine((IEnumerator) this.EnragedIE());
    this.attackedTimestamp = Time.time + 1.5f;
    ++this.warnings;
  }

  public void DefeatedChef() => DataManager.Instance.ShopKeeperChefState = 2;

  private void UpdateShop()
  {
    this.shopKeeperManager_0.DoublePrices();
    this.shopKeeperManager_1.DoublePrices();
  }
}
