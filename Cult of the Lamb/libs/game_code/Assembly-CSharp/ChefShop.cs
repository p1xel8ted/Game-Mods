// Decompiled with JetBrains decompiler
// Type: ChefShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChefShop : MonoBehaviour
{
  [SerializeField]
  public GameObject snailWarningConvo_0;
  [SerializeField]
  public GameObject snailWarningConvo_1;
  [SerializeField]
  public GameObject snailWarningConvo_2;
  [SerializeField]
  public GameObject snailDoublePrices;
  [SerializeField]
  public shopKeeperManager shopKeeperManager_0;
  [SerializeField]
  public shopKeeperManager shopKeeperManager_1;
  [SerializeField]
  public Health snailhealth;
  [SerializeField]
  public Health shopKeeperHealth;
  [SerializeField]
  public AnimateOnCollision _animateOnCollision;
  [SerializeField]
  public List<MonoBehaviour> chefComponentsToEnable;
  [SerializeField]
  public List<MonoBehaviour> chefComponentsToDisable;
  [SerializeField]
  public List<MonoBehaviour> chefComponentsToDisableDead;
  [SerializeField]
  public SkeletonAnimation shopKeep;
  [SerializeField]
  public spineChangeAnimationSimple[] worms;
  public bool touchedSnail;
  public int warnings;
  public bool fightStarted;
  public float attackedTimestamp;

  public void Start()
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
      this.fightStarted = true;
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

  public IEnumerator EnragedIE()
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
    chefShop.fightStarted = true;
  }

  public void ChefDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.Shop);
  }

  public void OnEnable()
  {
    if (this.shopKeeperHealth.enabled)
      this.shopKeeperHealth.OnDie += new Health.DieAction(this.ChefDied);
    this.checkMusic();
  }

  public void OnDisable()
  {
    this.shopKeeperHealth.OnDie -= new Health.DieAction(this.ChefDied);
    this.ResetChefState();
  }

  public void ResetChefState()
  {
    if (!this.fightStarted || !((Object) PlayerFarming.Instance != (Object) null) || PlayerFarming.Instance.health.state.CURRENT_STATE != StateMachine.State.GameOver || DataManager.Instance.ShopKeeperChefState != 2)
      return;
    DataManager.Instance.ShopKeeperChefState = 1;
  }

  public void checkMusic()
  {
    if (DataManager.Instance.ShopKeeperChefState == 1)
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
    else
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.Shop);
  }

  public void OnDestroy() => this.snailhealth.OnHit -= new Health.HitAction(this.HitSnail);

  public void TouchSnail()
  {
    if (this.touchedSnail)
      return;
    this.touchedSnail = true;
    this.snailWarningConvo_0.SetActive(true);
    ++this.warnings;
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.2f);
  }

  public void HitSnail(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((double) Time.time < (double) this.attackedTimestamp)
      return;
    GameObject player = Attacker;
    if ((Object) Attacker != (Object) null && (Object) Attacker.GetComponent<PlayerFarming>() == (Object) null)
      player = Health.GetSpellOwner(Attacker);
    this._animateOnCollision.PushPlayer();
    if (this.warnings == 0)
    {
      this.snailWarningConvo_0.SetActive(true);
      this.snailWarningConvo_0.GetComponent<Interaction_SimpleConversation>()?.Play(player);
    }
    else if (this.warnings == 1)
    {
      this.snailWarningConvo_1.SetActive(true);
      this.snailWarningConvo_1.GetComponent<Interaction_SimpleConversation>()?.Play(player);
    }
    else if (this.warnings == 2)
    {
      this.snailWarningConvo_2.SetActive(true);
      this.snailWarningConvo_2.GetComponent<Interaction_SimpleConversation>()?.Play(player);
    }
    else if (this.warnings == 3)
      this.StartCoroutine((IEnumerator) this.EnragedIE());
    this.attackedTimestamp = Time.time + 1.5f;
    ++this.warnings;
  }

  public void DefeatedChef() => DataManager.Instance.ShopKeeperChefState = 2;

  public void UpdateShop()
  {
    this.shopKeeperManager_0.DoublePrices();
    this.shopKeeperManager_1.DoublePrices();
  }
}
