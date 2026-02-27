// Decompiled with JetBrains decompiler
// Type: PlayerWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class PlayerWeapon : BaseMonoBehaviour
{
  public PlayerWeapon.EquippedWeaponsInfo CurrentWeapon;
  public int CurrentWeaponLevel;
  public SkeletonAnimation skeletonAnimation;
  private PlayerFarming playerFarming;
  private PlayerController playerController;
  private StateMachine state;
  private Health health;
  private float ResetCombo = 0.2f;
  private const float ResetComboDuration = 0.2f;
  private int StealthDamageMultiplierIncrease = 4;
  private PlayerArrows playerArrows;
  private Health.Team? overrideTeam;
  private static float minHeavyAttackChargeBeforeSecondAttack = 0.2f;
  public GameObject HeavyAttackTarget;
  [SerializeField]
  private CriticalTimer criticalTimer;
  public static System.Action WeaponDamaged;
  public static System.Action WeaponBroken;
  private float criticalHitChargeDuration = 25f;
  private bool _critHitCharged;
  public static bool FirstTimeUsingWeapon = false;
  private Coroutine attackRoutine;
  public static PlayerWeapon.AttackSwipeDirections AttackSwipeDirection;
  private bool invokedDamage;
  public bool ForceWeapons;
  public static PlayerWeapon.WeaponEvent OnWeaponChanged;
  public PlayerWeapon.AttackState CurrentAttackState;
  private int CurrentCombo;
  private bool HoldingForHeavyAttack;
  private bool StealthSneakAttack;
  private bool CanChangeDirection = true;
  private float StoreFacing = -1f;
  private float aimTimer;
  public PlayerVFXManager playerVFX;
  private PlayerWeapon.SpecialsData CurrentSpecial;
  public List<PlayerWeapon.SpecialsData> Specials;

  public static event PlayerWeapon.DoSpecialAction OnSpecial;

  public static float CriticalHitTimer { get; set; }

  public bool CriticalHitCharged
  {
    get
    {
      bool criticalHitCharged = (double) PlayerWeapon.CriticalHitTimer >= (double) this.criticalHitChargeDuration;
      if (!this._critHitCharged & criticalHitCharged)
      {
        this._critHitCharged = true;
        this.criticalHitChargeDuration = UnityEngine.Random.Range(20f, 30f);
      }
      return criticalHitCharged;
    }
  }

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    this.state = this.GetComponent<StateMachine>();
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.playerController = this.GetComponent<PlayerController>();
    this.playerArrows = this.GetComponent<PlayerArrows>();
    this.criticalTimer.gameObject.SetActive(false);
    this.SetWeapon(DataManager.Instance.CurrentWeapon, DataManager.Instance.CurrentWeaponLevel);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.SetSpecial(0);
    DataManager.Instance.PLAYER_SPECIAL_AMMO = (float) DataManager.Instance.Followers.Count;
    this.HeavyAttackTarget.SetActive(false);
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.StopAllCoroutines();
    GameManager.SetTimeScale(1f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }

  private void OnDestroy()
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    PlayerWeapon.FirstTimeUsingWeapon = false;
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    string name = e.Data.Name;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
    {
      case 369890212:
        if (!(name == "invincibility_OFF"))
          break;
        this.health.invincible = false;
        break;
      case 520586699:
        if (!(name == "S2 - Deal Damage 1"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(0.0f, 0, 1f, this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType, 1f);
        break;
      case 537364318:
        if (!(name == "S2 - Deal Damage 2"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe((float) (90 + ((double) this.state.facingAngle <= -90.0 || (double) this.state.facingAngle >= 90.0 ? 180 : 0)), 0, 1f, this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType, 1f);
        break;
      case 554141937:
        if (!(name == "S2 - Deal Damage 3"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(180f, 0, 1f, this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType, 1f);
        break;
      case 570919556:
        if (!(name == "S2 - Deal Damage 4"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe((float) (270 + ((double) this.state.facingAngle <= -90.0 || (double) this.state.facingAngle >= 90.0 ? 180 : 0)), 0, 1f, this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType, 1f);
        break;
      case 587697175:
        if (!(name == "S2 - Deal Damage 5"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(this.state.facingAngle, 0, 1f, this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType, 1f);
        break;
      case 764380392:
        if (!(name == "Swipe-Down"))
          break;
        PlayerWeapon.AttackSwipeDirection = PlayerWeapon.AttackSwipeDirections.Down;
        break;
      case 1045760954:
        if (!(name == "Attack Deal Damage"))
          break;
        AudioManager.Instance.PlayOneShot(this.CurrentWeapon.WeaponData.PerformActionSound, this.gameObject);
        switch (EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).PrimaryEquipmentType)
        {
          case EquipmentType.Sword:
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
            MMVibrate.Rumble(0.166666672f, 0.0333333351f, 0.33f, (MonoBehaviour) this);
            break;
          case EquipmentType.Axe:
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
            MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this);
            break;
          case EquipmentType.Hammer:
            PlayerFarming.Instance.HideWeaponChargeBars();
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
            BiomeConstants.Instance.EmitHammerEffects(this.transform.position, this.state.facingAngle);
            MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this);
            break;
          case EquipmentType.Dagger:
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_small", this.gameObject);
            MMVibrate.Rumble(0.0833333358f, 0.025f, 0.25f, (MonoBehaviour) this);
            break;
          default:
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
            MMVibrate.Rumble(0.166666672f, 0.0333333351f, 0.33f, (MonoBehaviour) this);
            break;
        }
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        CameraManager.shakeCamera(this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].CameraShake, this.state.facingAngle);
        Swipe component1 = UnityEngine.Object.Instantiate<GameObject>(this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].SwipeObject, this.transform, true).GetComponent<Swipe>();
        Vector3 vector3 = this.transform.position + new Vector3(this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.CurrentWeapon.RangeMultiplier * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.CurrentWeapon.RangeMultiplier * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f);
        Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
        if (TrinketManager.HasTrinket(TarotCards.Card.Skull))
          attackFlags |= Health.AttackFlags.Skull;
        if (TrinketManager.HasTrinket(TarotCards.Card.Spider))
          attackFlags |= Health.AttackFlags.Poison;
        float damage1 = this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].Damage;
        Vector3 Position = vector3;
        double facingAngle = (double) this.state.facingAngle;
        int num1 = (int) this.overrideTeam ?? (int) this.health.team;
        Health health = this.health;
        Action<Health, Health.AttackTypes> CallBack = new Action<Health, Health.AttackTypes>(this.HitEnemyCallBack);
        double Radius = (double) this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].RangeRadius * (double) this.CurrentWeapon.RangeMultiplier;
        double damage2 = (double) PlayerWeapon.GetDamage(damage1, DataManager.Instance.CurrentWeaponLevel);
        double critChance = (double) this.GetCritChance();
        int attackType = (int) this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType;
        int num2 = (int) attackFlags;
        component1.Init(Position, (float) facingAngle, (Health.Team) num1, health, CallBack, (float) Radius, (float) damage2, (float) critChance, (Health.AttackTypes) attackType, (Health.AttackFlags) num2);
        break;
      case 1118366532:
        if (!(name == "Attack Can Break"))
          break;
        this.CurrentAttackState = PlayerWeapon.AttackState.CanBreak;
        break;
      case 1306905498:
        if (!(name == "Swipe-DownRight"))
          break;
        PlayerWeapon.AttackSwipeDirection = PlayerWeapon.AttackSwipeDirections.DownRight;
        break;
      case 1538731393:
        if (!(name == "Swipe-Up"))
          break;
        PlayerWeapon.AttackSwipeDirection = PlayerWeapon.AttackSwipeDirections.Up;
        break;
      case 1920427890:
        if (!(name == "invincibility_ON"))
          break;
        this.health.invincible = true;
        break;
      case 2054819097:
        if (!(name == "Attack Has Finished"))
          break;
        this.CurrentAttackState = PlayerWeapon.AttackState.Finish;
        break;
      case 2847789413:
        if (!(name == "S1 - Deal Damage"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        foreach (Component component2 in Physics2D.OverlapCircleAll((Vector2) (this.transform.position + new Vector3(1f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 1f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f)), 2f))
        {
          Health component3 = component2.gameObject.GetComponent<Health>();
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && (UnityEngine.Object) component3 != (UnityEngine.Object) this.health && component3.team != (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team))
          {
            component3.DealDamage(5f, this.gameObject, Vector3.Lerp(this.transform.position, component3.transform.position, 0.8f));
            CameraManager.shakeCamera(0.6f, Utils.GetAngle(this.transform.position, component3.transform.position));
          }
        }
        break;
      case 2890057927:
        if (!(name == "Update Angle"))
          break;
        if (this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].CanChangeDirectionDuringAttack)
          this.state.facingAngle = this.playerController.forceDir = this.StoreFacing;
        this.CanChangeDirection = false;
        break;
      case 4136000111:
        if (!(name == "attack-charge1-hit"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(this.state.facingAngle, 0, 1.5f, this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].AttackType, 2f);
        CameraManager.shakeCamera(0.4f);
        break;
    }
  }

  public void DoSlowMo(bool setZoom = true)
  {
    this.StartCoroutine((IEnumerator) this.SlowMo(setZoom));
  }

  private IEnumerator SlowMo(bool setZoom = true)
  {
    if (setZoom)
      GameManager.GetInstance().CameraSetZoom(6f);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.699999988079071)
    {
      GameManager.SetTimeScale(0.2f);
      yield return (object) null;
    }
    GameManager.SetTimeScale(1f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }

  private void CreateSwipe(
    float Angle,
    int Combo,
    float BaseDamage,
    Health.AttackTypes AttackType,
    float Radius)
  {
    Addressables.InstantiateAsync((object) $"Assets/Prefabs/Enemies/Weapons/PlayerSwipe{Combo}.prefab", this.transform.position + new Vector3(Radius * 0.5f * Mathf.Cos(Angle * ((float) Math.PI / 180f)), Radius * 0.5f * Mathf.Sin(Angle * ((float) Math.PI / 180f)), -0.5f), Quaternion.identity, this.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<Swipe>().Init(this.transform.position + new Vector3(Radius * 0.5f * Mathf.Cos(Angle * ((float) Math.PI / 180f)), Radius * 0.5f * Mathf.Sin(Angle * ((float) Math.PI / 180f)), -0.5f), Angle, (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team), this.health, new Action<Health, Health.AttackTypes>(this.HitEnemyCallBack), Radius, PlayerWeapon.GetDamage(BaseDamage, DataManager.Instance.CurrentWeaponLevel), this.GetCritChance(), AttackType));
  }

  public static float GetDamage(float BaseDamage, int WeaponLevel)
  {
    float num = 1f + TrinketManager.GetWeaponDamageMultiplerIncrease() + (0.2f * (float) DataManager.Instance.PLAYER_DAMAGE_LEVEL + DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL) + PlayerFleeceManager.GetWeaponDamageMultiplier() + DataManager.GetWeaponDamageMultiplier(WeaponLevel);
    return BaseDamage * num * DifficultyManager.GetPlayerDamageMultiplier();
  }

  public Sprite GetCurrentIcon(EquipmentType weaponType)
  {
    return EquipmentManager.GetWeaponData(weaponType).UISprite;
  }

  public Sprite GetSprite(EquipmentType weaponType)
  {
    return EquipmentManager.GetWeaponData(weaponType).WorldSprite;
  }

  private float GetCritChance() => 0.0f + TrinketManager.GetWeaponCritChanceIncrease();

  private void HitEnemyCallBack(Health h, Health.AttackTypes AttackType)
  {
    if (h.team == Health.Team.Team2)
    {
      this.playerController.forceDir = this.state.facingAngle = Utils.GetAngle(this.transform.position, h.transform.position);
      if ((double) h.HP <= 0.0)
      {
        if (h.SlowMoOnkill)
          this.StartCoroutine((IEnumerator) this.SlowMo());
        if (DataManager.Instance.PLAYER_ARROW_AMMO <= 0)
          this.playerArrows.RestockAllArrows();
        CameraManager.shakeCamera(0.6f, Utils.GetAngle(this.transform.position, h.gameObject.transform.position));
      }
    }
    switch (EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).PrimaryEquipmentType)
    {
      case EquipmentType.Sword:
        MMVibrate.Rumble(0.5f, 0.1f, 0.33f, (MonoBehaviour) this);
        break;
      case EquipmentType.Axe:
        MMVibrate.Rumble(0.5f, 0.15f, 0.5f, (MonoBehaviour) this);
        break;
      case EquipmentType.Dagger:
        MMVibrate.Rumble(0.25f, 0.075f, 0.25f, (MonoBehaviour) this);
        break;
      default:
        MMVibrate.Rumble(0.5f, 0.1f, 0.33f, (MonoBehaviour) this);
        break;
    }
    if (h.HasShield && AttackType != Health.AttackTypes.Projectile)
    {
      this.state.facingAngle = Utils.GetAngle(this.transform.position, h.transform.position);
      this.playerController.CancelLunge(0.0f);
      this.playerController.unitObject.knockBackVX = (h.WasJustParried ? -1f : -0.75f) * Mathf.Cos(Utils.GetAngle(this.transform.position, h.transform.position) * ((float) Math.PI / 180f));
      this.playerController.unitObject.knockBackVY = (h.WasJustParried ? -1f : -0.75f) * Mathf.Sin(Utils.GetAngle(this.transform.position, h.transform.position) * ((float) Math.PI / 180f));
    }
    if (h.DontCombo)
      this.playerController.CancelLunge(h.team == Health.Team.Team2 ? this.CurrentWeapon.WeaponData.Combos[this.CurrentCombo].HitKnockback : 0.0f);
    if (!h.OnHitBlockAttacker && !h.WasJustParried)
      return;
    if ((UnityEngine.Object) h.AttackerToBlock == (UnityEngine.Object) null)
      Debug.LogWarning((object) $"Player tried to block an attack from {(object) h.gameObject}but AttackerToBlock was null. Have you set the AttackerToBlock on the Health prefab in the Inspector?", (UnityEngine.Object) h.gameObject);
    else
      this.playerController.BlockAttacker(h.AttackerToBlock);
  }

  private void Update()
  {
    if (!LocationManager.LocationIsDungeon(PlayerFarming.Location) && !this.ForceWeapons || !DataManager.Instance.EnabledSword || DataManager.Instance.CurrentWeapon == EquipmentType.None)
      return;
    if (this.state.CURRENT_STATE != StateMachine.State.Attacking || this.CurrentWeapon.WeaponData.PrimaryEquipmentType != EquipmentType.Hammer || (double) Time.timeScale <= 0.0 || LetterBox.IsPlaying || MMConversation.CURRENT_CONVERSATION != null && !MMConversation.isBark)
      PlayerFarming.Instance.HideWeaponChargeBars();
    if ((double) Time.timeScale <= 0.0 || this.playerFarming.GoToAndStopping)
      return;
    if (this.state.CURRENT_STATE != StateMachine.State.Attacking)
    {
      if (this.CurrentCombo > 0 && (double) (this.ResetCombo -= Time.deltaTime) < 0.0)
      {
        this.ResetCombo = 0.2f;
        this.CurrentCombo = 0;
      }
    }
    else
      this.ResetCombo = 0.2f;
    if ((UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) null && Interactor.CurrentInteraction.HasSecondaryInteraction && Interactor.CurrentInteraction.SecondaryAction == 2)
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
      case StateMachine.State.Dodging:
      case StateMachine.State.Stealth:
        if (InputManager.Gameplay.GetAttackButtonDown())
        {
          if (this.attackRoutine != null)
            this.StopCoroutine(this.attackRoutine);
          this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine());
          break;
        }
        break;
    }
    if (!this.criticalTimer.gameObject.activeSelf)
      return;
    this.criticalTimer.UpdateCharging(PlayerWeapon.CriticalHitTimer / this.criticalHitChargeDuration);
    PlayerWeapon.CriticalHitTimer += Time.deltaTime;
  }

  public void DoAttack()
  {
    if (this.attackRoutine != null)
      this.StopCoroutine(this.attackRoutine);
    this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine());
  }

  public void ManualHit(Health.Team? overrideTeam = null)
  {
    this.overrideTeam = overrideTeam;
    if (this.attackRoutine != null)
      this.StopCoroutine(this.attackRoutine);
    this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine());
  }

  public void SetWeapon(EquipmentType weaponType, int WeaponLevel)
  {
    this.CurrentWeapon = new PlayerWeapon.EquippedWeaponsInfo();
    this.CurrentWeapon.WeaponType = weaponType;
    this.CurrentWeapon.WeaponData = EquipmentManager.GetWeaponData(weaponType);
    if ((UnityEngine.Object) this.criticalTimer != (UnityEngine.Object) null)
      this.criticalTimer.gameObject.SetActive((bool) (UnityEngine.Object) this.CurrentWeapon.WeaponData.GetAttachment(AttachmentEffect.Critical));
    if (DataManager.Instance.CurrentWeapon != weaponType)
      PlayerWeapon.CriticalHitTimer = 0.0f;
    DataManager.Instance.CurrentWeapon = weaponType;
    DataManager.Instance.CurrentWeaponLevel = WeaponLevel;
    this.DoAttachmentEffect(AttachmentState.Constant);
    this.CurrentWeaponLevel = WeaponLevel;
    this.CurrentCombo = 0;
    PlayerWeapon.WeaponEvent onWeaponChanged = PlayerWeapon.OnWeaponChanged;
    if (onWeaponChanged != null)
      onWeaponChanged(DataManager.Instance.CurrentWeapon, DataManager.Instance.CurrentWeaponLevel);
    PlayerFarming.Instance.SetSkin();
  }

  public PlayerWeapon.EquippedWeaponsInfo GetCurrentWeapon() => this.CurrentWeapon;

  public void StopAttackRoutine()
  {
    if (this.attackRoutine == null)
      return;
    this.StopCoroutine(this.attackRoutine);
  }

  private IEnumerator DoAttackRoutine()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.playerVFX.stopEmitChargingParticles();
    playerWeapon.CanChangeDirection = true;
    if (playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].CanChangeDirectionDuringAttack)
      playerWeapon.StoreFacing = playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir;
    playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
    playerWeapon.aimTimer = 0.0f;
    if (playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator)
      playerWeapon.playerFarming.ShowWeaponChargeBars();
    playerWeapon.HoldingForHeavyAttack = true;
    playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].Animation, false).MixDuration = 0.0f;
    playerWeapon.StealthSneakAttack = playerWeapon.state.CURRENT_STATE == StateMachine.State.Stealth;
    float num1 = playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging ? 1.25f : 1f;
    if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
      playerWeapon.playerController.speed = playerWeapon.playerController.DodgeSpeed * 1.2f;
    playerWeapon.playerController.Lunge(playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].LungeDuration * num1, playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].LungeSpeed * num1);
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (DataManager.Instance.SpawnPoisonOnAttack)
      TrapPoison.CreatePoison(playerWeapon.transform.position, 2, 0.2f, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
    if (TrinketManager.HasTrinket(TarotCards.Card.HandsOfRage) && !TrinketManager.IsOnCooldown(TarotCards.Card.HandsOfRage))
    {
      playerWeapon.playerFarming.playerSpells.AimAngle = playerWeapon.state.LookAngle;
      playerWeapon.playerFarming.playerSpells.Spell_Fireball(EquipmentType.Fireball, knockback: false);
      TrinketManager.TriggerCooldown(TarotCards.Card.HandsOfRage);
    }
    bool QueueAttack = false;
    float QueueAttackTimer = 0.0f;
    playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackStart);
    float num2 = playerWeapon.CurrentWeapon.AttackRateMultiplier + TrinketManager.GetAttackRateMultiplier();
    playerWeapon.skeletonAnimation.timeScale = num2;
    if (playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator)
      AudioManager.Instance.PlayOneShot("event:/weapon/melee_charge", playerWeapon.gameObject);
    while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
    {
      if (!InputManager.Gameplay.GetAttackButtonHeld())
        playerWeapon.HoldingForHeavyAttack = false;
      if (playerWeapon.CanChangeDirection && playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].CanChangeDirectionDuringAttack && ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) > (double) PlayerController.MinInputForMovement))
        playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(), InputManager.Gameplay.GetVerticalAxis()));
      if (playerWeapon.CanChangeDirection && playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].CanFreelyChangeDirection && ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) > (double) PlayerController.MinInputForMovement))
        playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(), InputManager.Gameplay.GetVerticalAxis()));
      if (InputManager.General.MouseInputActive && playerWeapon.state.CURRENT_STATE == StateMachine.State.Attacking)
      {
        float angle = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position), (Vector3) InputManager.General.GetMousePosition());
        playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = angle;
      }
      if (playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator && (double) Time.timeScale > 0.0)
      {
        playerWeapon.aimTimer += Time.deltaTime;
        float facingAngle = playerWeapon.state.facingAngle;
        playerWeapon.playerFarming.SetWeaponAimingRecticuleScaleAndRotation(0, Vector3.one, new Vector3(0.0f, 0.0f, facingAngle));
      }
      if ((double) (QueueAttackTimer += Time.deltaTime) > 0.10000000149011612 && playerWeapon.CurrentCombo < playerWeapon.CurrentWeapon.WeaponData.Combos.Count && playerWeapon.CurrentWeapon.WeaponData.Combos[playerWeapon.CurrentCombo].CanQueueNextAttack && InputManager.Gameplay.GetAttackButtonDown())
        QueueAttack = true;
      yield return (object) null;
    }
    playerWeapon.overrideTeam = new Health.Team?();
    playerWeapon.CurrentCombo = (int) Mathf.Repeat((float) (playerWeapon.CurrentCombo + 1), (float) playerWeapon.CurrentWeapon.WeaponData.Combos.Count);
    while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.CanBreak)
    {
      if (!InputManager.Gameplay.GetAttackButtonHeld())
        playerWeapon.HoldingForHeavyAttack = false;
      playerWeapon.skeletonAnimation.timeScale = 1f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) > (double) PlayerController.MinInputForMovement)
      {
        if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Attacking)
        {
          System.Action crownReturnSubtle = PlayerFarming.OnCrownReturnSubtle;
          if (crownReturnSubtle != null)
            crownReturnSubtle();
          playerWeapon.state.CURRENT_STATE = StateMachine.State.Moving;
        }
        playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackEnd);
        yield break;
      }
      if ((QueueAttack || InputManager.Gameplay.GetAttackButtonDown()) && (LocationManager.LocationIsDungeon(PlayerFarming.Location) || playerWeapon.ForceWeapons))
      {
        playerWeapon.StopAllCoroutines();
        GameManager.SetTimeScale(1f);
        GameManager.GetInstance().CameraResetTargetZoom();
        playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackEnd);
        if (playerWeapon.attackRoutine != null)
          playerWeapon.StopCoroutine(playerWeapon.attackRoutine);
        playerWeapon.attackRoutine = playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAttackRoutine());
        yield break;
      }
      yield return (object) null;
    }
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private void DoAttachmentEffect(AttachmentState attachmentState)
  {
    if (attachmentState == AttachmentState.Constant)
      this.CurrentWeapon.ResetMultipliers();
    foreach (WeaponAttachmentData weaponAttachmentData in this.GetAttachmentsWithState(DataManager.Instance.CurrentWeapon, attachmentState))
    {
      if (weaponAttachmentData.IsAttachmentActive())
      {
        Vector3 position = this.transform.position + new Vector3(weaponAttachmentData.ExplosionOffset * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), weaponAttachmentData.ExplosionOffset * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f);
        switch (weaponAttachmentData.Effect)
        {
          case AttachmentEffect.Explode:
            Explosion.CreateExplosion(position, Health.Team.PlayerTeam, this.health, weaponAttachmentData.ExplosionRadius, weaponAttachmentData.ExplosionDamage);
            continue;
          case AttachmentEffect.Dash:
            this.playerController.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.playerController.xDir, this.playerController.yDir));
            this.playerController.speed = weaponAttachmentData.DashSpeed;
            continue;
          case AttachmentEffect.Damage:
            this.CurrentWeapon.WeaponDamageMultiplier += weaponAttachmentData.DamageMultiplierIncrement;
            continue;
          case AttachmentEffect.Critical:
            this.CurrentWeapon.CriticalChance += weaponAttachmentData.CriticalMultiplierIncrement;
            continue;
          case AttachmentEffect.Range:
            this.CurrentWeapon.RangeMultiplier += weaponAttachmentData.RangeIncrement;
            continue;
          case AttachmentEffect.AttackRate:
            this.CurrentWeapon.AttackRateMultiplier += weaponAttachmentData.AttackRateIncrement;
            continue;
          case AttachmentEffect.MovementSpeed:
            this.CurrentWeapon.MovementSpeedMultiplier += weaponAttachmentData.MovementSpeedIncrement;
            continue;
          case AttachmentEffect.IncreasedXPDrop:
            this.CurrentWeapon.XPDropMultiplier += weaponAttachmentData.xpDropIncrement;
            continue;
          case AttachmentEffect.HealChance:
            this.CurrentWeapon.HealChance += weaponAttachmentData.healChanceIncrement;
            this.CurrentWeapon.HealAmount += weaponAttachmentData.healAmount;
            continue;
          case AttachmentEffect.NegateDamageChance:
            this.CurrentWeapon.NegateDamageChance += weaponAttachmentData.negateDamageChanceIncrement;
            continue;
          case AttachmentEffect.Poison:
            this.CurrentWeapon.PoisonChance += weaponAttachmentData.poisonChance;
            continue;
          default:
            continue;
        }
      }
    }
  }

  public float GetAverageWeaponDamage(EquipmentType weaponType, int WeaponLevel)
  {
    if (weaponType == EquipmentType.None)
      return 1f;
    WeaponData weaponData = EquipmentManager.GetWeaponData(weaponType);
    if ((UnityEngine.Object) weaponData == (UnityEngine.Object) null || weaponData.EquipmentType == EquipmentType.None)
      return 0.0f;
    float num = 0.0f;
    foreach (PlayerWeapon.WeaponCombos combo in weaponData.Combos)
      num += PlayerWeapon.GetDamage(combo.Damage, WeaponLevel);
    return (float) Math.Round((double) num / (double) weaponData.Combos.Count * 100.0 / 100.0, 1);
  }

  public float GetWeaponSpeed(EquipmentType weaponType)
  {
    if (weaponType == EquipmentType.None)
      return 1f;
    WeaponData weaponData = EquipmentManager.GetWeaponData(weaponType);
    return !((UnityEngine.Object) weaponData != (UnityEngine.Object) null) ? 1f : weaponData.Speed;
  }

  public List<WeaponAttachmentData> GetAttachmentsWithState(
    EquipmentType weaponType,
    AttachmentState state)
  {
    WeaponData weaponData = EquipmentManager.GetWeaponData(weaponType);
    List<WeaponAttachmentData> attachmentsWithState = new List<WeaponAttachmentData>();
    if ((UnityEngine.Object) weaponData != (UnityEngine.Object) null && weaponData.Attachments != null)
    {
      foreach (WeaponAttachmentData attachment in weaponData.Attachments)
      {
        if (attachment.State == state)
          attachmentsWithState.Add(attachment);
      }
    }
    return attachmentsWithState;
  }

  private void SetSpecial(int Num)
  {
    this.CurrentSpecial = this.Specials[Num];
    DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET = this.CurrentSpecial.Target;
  }

  public delegate void DoSpecialAction();

  public enum AttackSwipeDirections
  {
    Down,
    DownRight,
    Up,
  }

  public delegate void WeaponEvent(EquipmentType Weapon, int Level);

  public enum AttackState
  {
    Begin,
    CanBreak,
    Finish,
  }

  [Serializable]
  public class EquippedWeaponsInfo
  {
    public EquipmentType WeaponType;
    public WeaponData WeaponData;

    public float WeaponDamageMultiplier { get; set; } = 1f;

    public float CriticalChance { get; set; }

    public float RangeMultiplier { get; set; } = 1f;

    public float AttackRateMultiplier { get; set; } = 1f;

    public float MovementSpeedMultiplier { get; set; } = 1f;

    public float XPDropMultiplier { get; set; } = 1f;

    public float NegateDamageChance { get; set; }

    public float HealChance { get; set; }

    public float HealAmount { get; set; } = 1f;

    public float PoisonChance { get; set; }

    public void ResetMultipliers()
    {
      this.WeaponDamageMultiplier = 1f;
      this.RangeMultiplier = 1f;
      this.AttackRateMultiplier = 1f;
      this.MovementSpeedMultiplier = 1f;
      this.XPDropMultiplier = 1f;
      this.CriticalChance = 0.0f;
      this.NegateDamageChance = 0.0f;
      this.HealChance = 0.0f;
      this.HealAmount = 1f;
      this.PoisonChance = 0.0f;
    }
  }

  [Serializable]
  public class WeaponCombos
  {
    public float CameraShake;
    public float Damage;
    public float RangeRadius = 1f;
    public string Animation;
    public GameObject SwipeObject;
    public bool CanQueueNextAttack = true;
    public bool CanChangeDirectionDuringAttack = true;
    public bool CanFreelyChangeDirection;
    public bool ShowDirectionIndicator;
    public float LungeSpeed = 20f;
    public float LungeDuration = 0.15f;
    public float HitKnockback;
    public Health.AttackTypes AttackType;
  }

  [Serializable]
  public class SpecialsData
  {
    public string Name;
    [SpineAnimation("", "skeletonAnimation", true, false)]
    public string NorthAnimation;
    [SpineAnimation("", "skeletonAnimation", true, false)]
    public string HorizontalAnimation;
    [SpineAnimation("", "skeletonAnimation", true, false)]
    public string SouthAnimation;
    public float Target;
  }
}
