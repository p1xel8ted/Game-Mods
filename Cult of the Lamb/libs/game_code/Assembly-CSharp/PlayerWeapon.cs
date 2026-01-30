// Decompiled with JetBrains decompiler
// Type: PlayerWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class PlayerWeapon : BaseMonoBehaviour
{
  public BlunderAmmo blunderAmmo;
  public GameObject swipeShieldCharge;
  public ThrownAxe thrownAxe;
  [SerializeField]
  public ChainHook chainHook;
  [SerializeField]
  public ChainHook chainHook1;
  [Tooltip("Used only for the thid chain weapon attack.")]
  [Range(0.0f, 180f)]
  [SerializeField]
  public float chainAutoAimAngle = 25f;
  public const float closeChainCheatDamageDistance = 0.2f;
  public const float closeChainCheatDamageRadius = 0.5f;
  public int CurrentWeaponLevel;
  public SkeletonAnimation skeletonAnimation;
  public PlayerController playerController;
  public StateMachine state;
  public Health health;
  public float ResetCombo = 0.2f;
  public const float ResetComboDuration = 0.2f;
  public int StealthDamageMultiplierIncrease = 4;
  public PlayerArrows playerArrows;
  public Health.Team? overrideTeam;
  public static float minHeavyAttackChargeBeforeSecondAttack = 0.2f;
  public GameObject HeavyAttackTarget;
  [SerializeField]
  public GrenadeBullet yngyaChunkGrandeBullet;
  [SerializeField]
  public CriticalTimer criticalTimer;
  [SerializeField]
  public DamageNegationTimer damageNegation;
  [SerializeField]
  public float damageNegationTime = 10f;
  public float damageNegationTimer;
  public static System.Action WeaponDamaged;
  public static System.Action WeaponBroken;
  [CompilerGenerated]
  public static float \u003CCriticalHitTimer\u003Ek__BackingField;
  public float criticalHitChargeDuration = 25f;
  public string invincibleOnHitSFX = "event:/dlc/combat/invulnerable_impact";
  public string chainSwingBasicSFX = "event:/dlc/weapon/chain_swing_basic";
  public string chainSwingLargeSFX = "event:/dlc/weapon/chain_swing_large";
  public string chainGroundImpactBasicSFX = "event:/dlc/weapon/chain_ground_impact_basic";
  public string chainGroundImpactLargeSFX = "event:/dlc/weapon/chain_ground_impact_large";
  public string chainLegendaryProjectileLaunchSFX = "event:/dlc/weapon/chain_legendary_projectile_launch";
  public string chainPullbackSFX = "event:/dlc/weapon/chain_pullback";
  public string chainHeavyAttackStartSFX = "event:/dlc/weapon/chain_relic_start";
  public string chainHeavyAttackJumpSFX = "event:/dlc/weapon/chain_relic_jump";
  public string chainHeavyAttackAimLoopSFX = "event:/dlc/weapon/chain_relic_aim";
  public PlayerFarming _playerFarming;
  public bool _critHitCharged;
  public static bool FirstTimeUsingWeapon = false;
  public Coroutine attackRoutine;
  public static PlayerWeapon.AttackSwipeDirections AttackSwipeDirection;
  public const float GUANTLET_HEAVY_RANGE = 2f;
  public bool invokedDamage;
  public bool ForceWeapons;
  public static PlayerWeapon.WeaponEvent OnWeaponChanged;
  public PlayerWeapon.AttackState CurrentAttackState;
  public int CurrentCombo;
  public bool HoldingForHeavyAttack;
  public bool StealthSneakAttack;
  public bool CanChangeDirection = true;
  public float StoreFacing = -1f;
  public float aimTimer;
  public bool EnableHeavyAttack = true;
  public float MaxHold = 0.3f;
  public bool ChargedNegation;
  public float DaggerAngleOffset = 10f;
  public float DaggerDistance = 0.75f;
  public bool DoHeavyAttack;
  public bool DoingHeavyAttack;
  public bool RepelProjectiles;
  public bool ShowHeavyAim;
  public float HeavyAimSpeed;
  public EventInstance HeavyAttackSound;
  public Coroutine ShieldHoldCoroutine;
  [CompilerGenerated]
  public float \u003CTimeOfAttack\u003Ek__BackingField;
  public ShieldRepel shieldRepel;
  public string lastShieldAnimationString = "";
  public PlayerVFXManager playerVFX;
  public EventInstance loopedSound;
  public float xHeavyAimingScale;
  public EventInstance chargingLoopSound;
  public PlayerWeapon.SpecialsData CurrentSpecial;
  public List<PlayerWeapon.SpecialsData> Specials;

  public static float HeavySwordDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Sword) ? 2.5f * DataManager.Instance.SpecialAttackDamageMultiplier : 2f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyAxeDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Axe) ? 2.5f * DataManager.Instance.SpecialAttackDamageMultiplier : 2f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyShieldDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Shield) ? 2.5f * DataManager.Instance.SpecialAttackDamageMultiplier : 2f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyDaggerDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger) ? 2f * DataManager.Instance.SpecialAttackDamageMultiplier : 1.5f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyGauntletDamageFirst
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Gauntlets) ? 5f * DataManager.Instance.SpecialAttackDamageMultiplier : 4f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyGauntletDamageSubsequent
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Gauntlets) ? 1.5f * DataManager.Instance.SpecialAttackDamageMultiplier : 1f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyHammerDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Sword) ? 5f * DataManager.Instance.SpecialAttackDamageMultiplier : 4f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyBlunderbussDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Blunderbuss) ? 8f * DataManager.Instance.SpecialAttackDamageMultiplier : 6f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public static float HeavyChainDamage
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Chain) ? 3f * DataManager.Instance.SpecialAttackDamageMultiplier : 2.5f * DataManager.Instance.SpecialAttackDamageMultiplier;
    }
  }

  public CriticalTimer CriticalTimer => this.criticalTimer;

  public static event PlayerWeapon.DoSpecialAction OnSpecial;

  public static float CriticalHitTimer
  {
    get => PlayerWeapon.\u003CCriticalHitTimer\u003Ek__BackingField;
    set => PlayerWeapon.\u003CCriticalHitTimer\u003Ek__BackingField = value;
  }

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

  public PlayerFarming playerFarming
  {
    get
    {
      if ((UnityEngine.Object) this._playerFarming == (UnityEngine.Object) null)
        this._playerFarming = this.GetComponent<PlayerFarming>();
      return this._playerFarming;
    }
    set => this._playerFarming = value;
  }

  public int HeavyAttackFervourCost
  {
    get
    {
      float attackCostMultiplier = TrinketManager.GetHeavyAttackCostMultiplier(this.playerFarming);
      int num = 10;
      switch (this.playerFarming.CurrentWeaponInfo.WeaponType)
      {
        case EquipmentType.Axe:
          num = 20;
          break;
        case EquipmentType.Dagger:
          num = 15;
          break;
        case EquipmentType.Blunderbuss:
          num = 20;
          break;
      }
      return (int) ((double) num * (double) attackCostMultiplier);
    }
  }

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.state = this.GetComponent<StateMachine>();
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.playerController = this.GetComponent<PlayerController>();
    this.playerArrows = this.GetComponent<PlayerArrows>();
    this.criticalTimer.gameObject.SetActive(false);
    this.damageNegation.gameObject.SetActive(false);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDamageNegated += new System.Action(this.OnDamageNegated);
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.SetSpecial(0);
    DataManager.Instance.PLAYER_SPECIAL_AMMO = (float) DataManager.Instance.Followers.Count;
    this.HeavyAttackTarget.SetActive(false);
    if (DataManager.Instance.ForcingPlayerWeaponDLC == EquipmentType.None)
      return;
    this.SetWeapon(DataManager.Instance.ForcingPlayerWeaponDLC, DataManager.StartingEquipmentLevel + 1);
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.StopAllCoroutines();
    GameManager.SetTimeScale(1f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }

  public void OnDamageNegated()
  {
    Debug.Log((object) nameof (OnDamageNegated).Colour(Color.red));
    this.damageNegation.transform.DOKill();
    this.damageNegation.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnHit -= new Health.HitAction(this.OnHit);
      this.health.OnDamageNegated -= new System.Action(this.OnDamageNegated);
    }
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    PlayerWeapon.FirstTimeUsingWeapon = false;
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if ((UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData == (UnityEngine.Object) null)
      return;
    string name = e.Data.Name;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
    {
      case 520586699:
        if (!(name == "S2 - Deal Damage 1"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(0.0f, 0, PlayerWeapon.HeavyGauntletDamageSubsequent, this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].AttackType, 1f, Health.AttackFlags.Penetration);
        break;
      case 537364318:
        if (!(name == "S2 - Deal Damage 2"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe((float) (90 + ((double) this.state.facingAngle <= -90.0 || (double) this.state.facingAngle >= 90.0 ? 180 : 0)), 0, PlayerWeapon.HeavyGauntletDamageSubsequent, this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].AttackType, 2f, Health.AttackFlags.Penetration);
        break;
      case 547438326:
        if (!(name == "THROW_DAGGER"))
          break;
        this.ShowHeavyAim = false;
        this.playerFarming.HideHeavyChargeBars();
        this.CanChangeDirection = false;
        int num1 = -1;
        int num2 = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger) ? 6 : 5;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger))
          AudioManager.Instance.PlayOneShot("event:/weapon/dagger_heavy/dagger_glint", this.gameObject);
        while (++num1 <= num2)
        {
          float num3 = UnityEngine.Random.Range(-this.DaggerAngleOffset, this.DaggerAngleOffset);
          ThrownDagger.SpawnThrownDagger(this.transform.position + this.DaggerDistance * new Vector3((float) num1 * Mathf.Cos((float) (((double) this.state.facingAngle + (double) num3) * (Math.PI / 180.0))), (float) num1 * Mathf.Sin((float) (((double) this.state.facingAngle + (double) num3) * (Math.PI / 180.0)))), PlayerWeapon.GetDamage(PlayerWeapon.HeavyDaggerDamage, this.CurrentWeaponLevel, this.playerFarming), (float) num1 * 0.1f, UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger) ? (Sprite) null : this.playerFarming.CurrentWeaponInfo.WeaponData.WorldSprite, this.gameObject);
        }
        this.playerFarming.playerSpells.faithAmmo.UseAmmo((float) this.HeavyAttackFervourCost, false);
        this.playerController.unitObject.DoKnockBack((float) (((double) this.state.facingAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 0.4f, 0.3f);
        break;
      case 554141937:
        if (!(name == "S2 - Deal Damage 3"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(180f, 0, PlayerWeapon.HeavyGauntletDamageSubsequent, this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].AttackType, 2f, Health.AttackFlags.Penetration);
        break;
      case 570919556:
        if (!(name == "S2 - Deal Damage 4"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe((float) (270 + ((double) this.state.facingAngle <= -90.0 || (double) this.state.facingAngle >= 90.0 ? 180 : 0)), 0, PlayerWeapon.HeavyGauntletDamageSubsequent, this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].AttackType, 2f, Health.AttackFlags.Penetration);
        break;
      case 587697175:
        if (!(name == "S2 - Deal Damage 5"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        this.CreateSwipe(this.state.facingAngle, 0, PlayerWeapon.HeavyGauntletDamageSubsequent, this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].AttackType, 2f, Health.AttackFlags.Penetration);
        break;
      case 764380392:
        if (!(name == "Swipe-Down"))
          break;
        PlayerWeapon.AttackSwipeDirection = PlayerWeapon.AttackSwipeDirections.Down;
        break;
      case 1045760954:
        if (!(name == "Attack Deal Damage"))
          break;
        AudioManager.Instance.PlayOneShot(this.playerFarming.CurrentWeaponInfo.WeaponData.PerformActionSound, this.gameObject);
        WeaponData weaponData = EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon);
        switch (weaponData.PrimaryEquipmentType)
        {
          case EquipmentType.Sword:
            if (weaponData.EquipmentType == EquipmentType.Sword_Ratau)
              AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/boomerang/attack_head_throw", this.gameObject);
            else
              AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
            MMVibrate.Rumble(0.166666672f, 0.0333333351f, 0.33f, (MonoBehaviour) this, this.playerFarming);
            break;
          case EquipmentType.Axe:
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
            MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
            break;
          case EquipmentType.Hammer:
            this.playerFarming.HideWeaponChargeBars();
            if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer) && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks))
              AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
            BiomeConstants.Instance.EmitHammerEffects(this.transform.position, this.state.facingAngle);
            Vector3 position = this.transform.position + new Vector3(1.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 1.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
            MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
            if (this.playerFarming.CurrentWeaponInfo.WeaponData.ContainsAttachmentType(AttachmentEffect.ElectricRing))
            {
              WeaponAttachmentData attachment = this.playerFarming.CurrentWeaponInfo.WeaponData.GetAttachment(AttachmentEffect.ElectricRing);
              LightningRingExplosion.CreateExplosion(position, Health.Team.PlayerTeam, this.health, attachment.electricRingSpreadSpeed, 1f, 1f, maxRadiusTarget: attachment.electricRingMaxRadius);
              break;
            }
            break;
          case EquipmentType.Dagger:
            if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger) && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks))
              AudioManager.Instance.PlayOneShot("event:/weapon/metal_small", this.gameObject);
            MMVibrate.Rumble(0.0833333358f, 0.025f, 0.25f, (MonoBehaviour) this, this.playerFarming);
            break;
          case EquipmentType.Chain:
            if (!this.DoingHeavyAttack)
            {
              MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
              this.ChainAttack();
              break;
            }
            break;
          default:
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
            MMVibrate.Rumble(0.166666672f, 0.0333333351f, 0.33f, (MonoBehaviour) this, this.playerFarming);
            break;
        }
        if (EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Chain)
          break;
        this.AttackDealDamage(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].Damage);
        break;
      case 1118366532:
        if (!(name == "Attack Can Break"))
          break;
        this.CurrentAttackState = PlayerWeapon.AttackState.CanBreak;
        break;
      case 1146208096:
        if (!(name == "Hammer Heavy"))
          break;
        AudioManager.Instance.PlayOneShot(this.playerFarming.CurrentWeaponInfo.WeaponData.PerformActionSound, this.gameObject);
        this.playerFarming.HideWeaponChargeBars();
        this.AttackDealDamage(PlayerWeapon.HeavyHammerDamage, Health.AttackFlags.Penetration);
        break;
      case 1252079630:
        int num4 = name == "BLUNDER_ANIMATION" ? 1 : 0;
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
      case 2054819097:
        if (!(name == "Attack Has Finished"))
          break;
        this.CurrentAttackState = PlayerWeapon.AttackState.Finish;
        break;
      case 2419328876:
        if (!(name == "THROW_AXE"))
          break;
        this.ShowHeavyAim = false;
        this.playerFarming.HideHeavyChargeBars();
        this.CanChangeDirection = false;
        this.playerController.unitObject.DoKnockBack((float) (((double) this.state.facingAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 0.3f, 0.3f);
        ThrownAxe.SpawnThrowingAxe(this.playerFarming, this.transform.position, PlayerWeapon.GetDamage(PlayerWeapon.HeavyAxeDamage, this.CurrentWeaponLevel, this.playerFarming), UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Axe) ? (Sprite) null : this.playerFarming.CurrentWeaponInfo.WeaponData.WorldSprite, this.state.facingAngle, callback: (Action<ThrownAxe>) (axe => this.thrownAxe = axe));
        this.playerFarming.playerSpells.faithAmmo.UseAmmo((float) this.HeavyAttackFervourCost, false);
        break;
      case 2847789413:
        if (!(name == "S1 - Deal Damage"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) (this.transform.position + new Vector3(1f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 1f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f)), 2f))
        {
          Health component2 = component1.gameObject.GetComponent<Health>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) this.health && component2.team != (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team))
          {
            component2.DealDamage(PlayerWeapon.HeavyGauntletDamageFirst, this.gameObject, Vector3.Lerp(this.transform.position, component2.transform.position, 0.8f));
            CameraManager.shakeCamera(0.6f, Utils.GetAngle(this.transform.position, component2.transform.position));
          }
        }
        break;
      case 2890057927:
        if (!(name == "Update Angle"))
          break;
        if (this.playerFarming.CurrentWeaponInfo != null && this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].CanChangeDirectionDuringAttack)
          this.state.facingAngle = this.playerController.forceDir = this.StoreFacing;
        this.CanChangeDirection = false;
        break;
      case 4136000111:
        if (!(name == "attack-charge1-hit"))
          break;
        AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
        EquipmentType primaryEquipmentType = EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon).PrimaryEquipmentType;
        this.CreateSwipe(this.state.facingAngle, 0, this.playerFarming.currentWeapon == EquipmentType.Sword_Ratau ? 0.0f : PlayerWeapon.HeavySwordDamage, Health.AttackTypes.Heavy, primaryEquipmentType == EquipmentType.Sword ? 2.5f : 2f, Health.AttackFlags.Penetration);
        this.playerFarming.HideWeaponChargeBars();
        if (primaryEquipmentType == EquipmentType.Hammer)
        {
          if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer) && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks))
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
          else
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
        }
        BiomeConstants.Instance.EmitHammerEffects(this.transform.position, this.state.facingAngle);
        MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
        CameraManager.shakeCamera(0.4f);
        if (this.DoingHeavyAttack)
          this.playerFarming.playerSpells.faithAmmo.UseAmmo((float) this.HeavyAttackFervourCost, false);
        this.DoingHeavyAttack = false;
        this.CanChangeDirection = false;
        break;
    }
  }

  public void LightningStrike(Vector3 pos)
  {
    BiomeConstants.Instance.EmitLightningStrike(pos);
    Explosion.CreateExplosion(pos, Health.Team.Team2, this.health, 1f, PlayerWeapon.GetDamage(2f, this.playerFarming.currentWeaponLevel, this.playerFarming));
  }

  public void AttackDealDamage(float DamageToDeal, Health.AttackFlags additionalFlags = (Health.AttackFlags) 0)
  {
    AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
    CameraManager.shakeCamera(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].CameraShake, this.state.facingAngle);
    Swipe component = UnityEngine.Object.Instantiate<GameObject>(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].SwipeObject, this.transform, true).GetComponent<Swipe>();
    Vector3 position = this.transform.position;
    if (this.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType != EquipmentType.Chain)
      position += new Vector3(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f);
    if (this.CurrentCombo >= 2)
    {
      if (this.playerFarming.CurrentWeaponInfo.WeaponData.ContainsAttachmentType(AttachmentEffect.Lightning))
      {
        Vector3 pos = this.transform.position + (Vector3) Utils.DegreeToVector2(this.state.facingAngle) * 2f;
        this.LightningStrike(pos);
        pos += (Vector3) Utils.DegreeToVector2(this.state.facingAngle);
        for (int index = 0; index < 3; ++index)
          GameManager.GetInstance().WaitForSeconds((float) index + 0.5f, (System.Action) (() =>
          {
            if ((double) UnityEngine.Random.value < 0.5 && (UnityEngine.Object) this.playerFarming.unitObject.GetClosestTarget() != (UnityEngine.Object) null)
              this.LightningStrike(this.playerFarming.unitObject.GetClosestTarget().transform.position);
            else
              this.LightningStrike(pos + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
          }));
      }
      if (this.playerFarming.CurrentWeaponInfo.WeaponData.ContainsAttachmentType(AttachmentEffect.Invincibility) && this.CurrentCombo >= 4)
        this.playerFarming.playerController.MakeUntouchable(1f);
    }
    Health.AttackFlags AttackFlags = (Health.AttackFlags) 0 | additionalFlags;
    if (TrinketManager.HasTrinket(TarotCards.Card.Skull, this.playerFarming))
      AttackFlags |= Health.AttackFlags.Skull;
    if (TrinketManager.HasTrinket(TarotCards.Card.Spider, this.playerFarming))
      AttackFlags |= Health.AttackFlags.Poison;
    float BaseDamage = DamageToDeal;
    component.Init(position, this.state.facingAngle, (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team), this.health, new Action<Health, Health.AttackTypes, Health.AttackFlags, float>(this.HitEnemyCallBack), this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier, PlayerWeapon.GetDamage(BaseDamage, this.playerFarming.currentWeaponLevel, this.playerFarming), this.GetCritChance(), this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].AttackType, AttackFlags);
  }

  public void ChainAttack()
  {
    PlayerWeapon.WeaponCombos combo = this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo];
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    float damage = PlayerWeapon.GetDamage(combo.Damage, this.playerFarming.currentWeaponLevel, this.playerFarming);
    float critChance = this.GetCritChance();
    float speed = this.playerFarming.CurrentWeaponInfo.AttackRateMultiplier + TrinketManager.GetAttackRateMultiplier(this.playerFarming);
    float hookDamageRadius = combo.RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier;
    this.chainHook.SetVisuals(this.playerFarming.CurrentWeaponInfo.WeaponData.EquipmentType);
    this.chainHook1.SetVisuals(this.playerFarming.CurrentWeaponInfo.WeaponData.EquipmentType);
    if (this.CriticalHitCharged || (double) UnityEngine.Random.Range(0.0f, 1f) < (double) critChance)
    {
      damage *= 3f;
      attackFlags |= Health.AttackFlags.Crit;
      PlayerWeapon.CriticalHitTimer = 0.0f;
    }
    if (TrinketManager.HasTrinket(TarotCards.Card.Skull, this.playerFarming))
      attackFlags |= Health.AttackFlags.Skull;
    if (TrinketManager.HasTrinket(TarotCards.Card.Spider, this.playerFarming))
      attackFlags |= Health.AttackFlags.Poison;
    Vector3 facingVector = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    Vector3 facingVectorRight = new Vector3(-facingVector.y, facingVector.x);
    float offsetMultiplier = combo.OffsetMultiplier;
    if (this.CurrentCombo != 2)
    {
      List<Health> healthList = new List<Health>((IEnumerable<Health>) Health.neutralTeam);
      healthList.AddRange((IEnumerable<Health>) Health.team2);
      for (int index = 0; index < healthList.Count; ++index)
      {
        if ((UnityEngine.Object) healthList[index] != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) healthList[index].transform.position, (Vector2) (this.transform.position + facingVector * 0.2f)) <= 0.5)
          healthList[index].DealDamage(damage, this.gameObject, this.transform.position + facingVector * 0.2f);
      }
    }
    switch (this.CurrentCombo)
    {
      case 0:
        EllipseMovement ellipseMovement1 = new EllipseMovement(Vector3.up, Vector3.right, -facingVectorRight * offsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / speed, combo.RadiusMultiplierOverTime);
        this.chainHook.Init(ellipseMovement1, damage, hookDamageRadius, attackFlags, true, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay);
        this.SpawnChainSwipeVFX(this.transform.position - Vector3.forward * 0.3f + ellipseMovement1.CenterOffset - facingVectorRight * (offsetMultiplier - 0.2f), speed);
        AudioManager.Instance.PlayOneShot(this.chainSwingBasicSFX, this.gameObject);
        break;
      case 1:
        EllipseMovement ellipseMovement2 = new EllipseMovement(Vector3.up, Vector3.right, facingVectorRight * offsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / speed, combo.RadiusMultiplierOverTime);
        this.chainHook1.Init(ellipseMovement2, damage, hookDamageRadius, attackFlags, true, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay);
        this.SpawnChainSwipeVFX(this.transform.position - Vector3.forward * 0.3f + ellipseMovement2.CenterOffset + facingVectorRight * (offsetMultiplier - 0.2f), speed);
        AudioManager.Instance.PlayOneShot(this.chainSwingBasicSFX, this.gameObject);
        break;
      case 2:
        this.AutoTargetThirdChainAttackDirection(out facingVector, out facingVectorRight, combo.EllipseRadiusY);
        EllipseMovement ellipseMovement3 = new EllipseMovement(facingVector, Vector3.back, facingVectorRight * offsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, combo.StartAngle, combo.AngleToMove, combo.Duration / speed, combo.RadiusMultiplierOverTime);
        EllipseMovement ellipseMovement4 = new EllipseMovement(facingVector, Vector3.back, -facingVectorRight * offsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, combo.StartAngle, combo.AngleToMove, combo.Duration / speed, combo.RadiusMultiplierOverTime);
        this.chainHook.SetCollidersActive(false);
        this.chainHook1.SetCollidersActive(false);
        this.chainHook.Init(ellipseMovement3, damage, hookDamageRadius, attackFlags, false, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, is3D: true, onComplete: (Action<Vector3>) (hitPosition =>
        {
          this.chainHook.SetCollidersActive(true);
          Vector3 Position = ((hitPosition + this.chainHook1.GetHookPosition()) * 0.5f) with
          {
            z = 0.0f
          };
          BiomeConstants.Instance.EmitHammerEffects(Position, playSFX: false);
          AudioManager.Instance.PlayOneShot(this.chainGroundImpactBasicSFX, this.transform.position);
          MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
          if ((bool) (UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData.GetAttachment(AttachmentEffect.Projectile))
          {
            AudioManager.Instance.PlayOneShot(this.chainLegendaryProjectileLaunchSFX, this.transform.position);
            for (int index = 0; index < UnityEngine.Random.Range(10, 16 /*0x10*/); ++index)
            {
              GrenadeBullet component = ObjectPool.Spawn<GrenadeBullet>(this.yngyaChunkGrandeBullet, this.chainHook.Hook.transform.position + Vector3.back, Quaternion.identity).GetComponent<GrenadeBullet>();
              component.SetOwner(this.gameObject);
              component.Damage = PlayerWeapon.GetDamage(2f, this.playerFarming.currentWeaponLevel, this.playerFarming);
              component.Play(-0.5f, (float) UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(1.5f, 2.5f), UnityEngine.Random.Range(-9f, -7f), Health.Team.PlayerTeam, true);
            }
          }
          if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
            return;
          this.chainHook.Hide(true);
        }), onHideStart: (System.Action) (() => AudioManager.Instance.PlayOneShot(this.chainPullbackSFX, this.transform.position)));
        this.chainHook1.Init(ellipseMovement4, damage, hookDamageRadius, attackFlags, false, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, is3D: true, onComplete: (Action<Vector3>) (hitPosition =>
        {
          this.chainHook1.SetCollidersActive(true);
          if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
            return;
          this.chainHook1.Hide(true);
        }));
        this.SpawnChainSwipeVFX(this.transform.position + ellipseMovement3.CenterOffset, speed);
        AudioManager.Instance.PlayOneShot(this.chainSwingLargeSFX, this.gameObject);
        break;
      default:
        this.chainHook.Init(new EllipseMovement(Vector3.back, Vector3.right, 3f, 3f, this.state.facingAngle - 90f, 180f, 0.2f / speed, combo.RadiusMultiplierOverTime), damage, hookDamageRadius, attackFlags, true, combo.ScaleMultiplierOverTime);
        break;
    }
  }

  public void SpawnChainSwipeVFX(Vector3 position, float speed = 1f)
  {
    this.SpawnChainSwipeVFX(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].SwipeObject, position, speed);
  }

  public void SpawnChainSwipeVFX(GameObject swipeObject, Vector3 position, float speed = 1f)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(swipeObject, this.transform, true);
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.state.facingAngle);
    if ((double) speed == 1.0)
      return;
    foreach (ParticleSystem componentsInChild in gameObject.GetComponentsInChildren<ParticleSystem>())
      componentsInChild.main.simulationSpeed = speed;
  }

  public void AutoTargetThirdChainAttackDirection(
    out Vector3 facingVector,
    out Vector3 facingVectorRight,
    float attackDistance)
  {
    facingVector = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    Vector3 origin = this.transform.position + facingVector * attackDistance;
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Units"));
    double num1 = (double) Mathf.Tan(this.chainAutoAimAngle * ((float) Math.PI / 180f));
    RaycastHit2D[] raycastHit2DArray = Physics2D.CircleCastAll((Vector2) origin, 2f, (Vector2) Vector3.back, 2f, (int) layerMask);
    Health health = (Health) null;
    float num2 = float.PositiveInfinity;
    Vector3 vector3;
    foreach (RaycastHit2D raycastHit2D in raycastHit2DArray)
    {
      Health component = raycastHit2D.collider.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.CurrentHP > 0.0 && component.team == Health.Team.Team2)
      {
        if ((UnityEngine.Object) health == (UnityEngine.Object) null)
        {
          health = component;
          vector3 = component.transform.position - origin;
          num2 = vector3.magnitude;
        }
        else
        {
          vector3 = component.transform.position - origin;
          float magnitude = vector3.magnitude;
          if ((double) magnitude < (double) num2)
          {
            health = component;
            num2 = magnitude;
          }
        }
      }
    }
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
    {
      vector3 = health.transform.position - this.transform.position;
      Vector3 normalized = vector3.normalized;
      float angle = Mathf.Clamp(Vector3.SignedAngle(facingVector, normalized, Vector3.back), -this.chainAutoAimAngle, this.chainAutoAimAngle);
      facingVector = Quaternion.AngleAxis(angle, Vector3.back) * facingVector;
    }
    facingVectorRight = new Vector3(-facingVector.y, facingVector.x);
  }

  public string GetShieldAnimationAngle()
  {
    string shieldAnimationAngle = (double) this.state.facingAngle <= 45.0 || (double) this.state.facingAngle >= 135.0 ? ((double) this.state.facingAngle <= 225.0 || (double) this.state.facingAngle >= 315.0 ? "attack-shield-heavy-horizontal" : ((double) this.state.facingAngle <= 245.0 || (double) this.state.facingAngle >= 295.0 ? "attack-shield-heavy" : "attack-shield-heavy-down")) : ((double) this.state.facingAngle <= 65.0 || (double) this.state.facingAngle >= 115.0 ? "attack-shield-heavy-diagonal-up" : "attack-shield-heavy-up");
    Debug.Log((object) ("Angle is " + shieldAnimationAngle));
    return shieldAnimationAngle;
  }

  public string GetBlunderbussAnimationAngle(bool holding, bool charging, bool heavyShoot)
  {
    return (double) this.state.facingAngle <= 45.0 || (double) this.state.facingAngle >= 135.0 ? ((double) this.state.facingAngle <= 225.0 || (double) this.state.facingAngle >= 315.0 ? (!heavyShoot ? (!charging ? (!holding ? "weapons/blunderbuss/attack-horizontal" : "weapons/blunderbuss/heavy-horizontal-holding") : "weapons/blunderbuss/heavy-horizontal-charge") : "weapons/blunderbuss/heavy-horizontal-shoot") : ((double) this.state.facingAngle <= 245.0 || (double) this.state.facingAngle >= 295.0 ? (!heavyShoot ? (!charging ? (!holding ? "weapons/blunderbuss/attack" : "weapons/blunderbuss/heavy-holding") : "weapons/blunderbuss/heavy-charge") : "weapons/blunderbuss/heavy-shoot") : (!heavyShoot ? (!charging ? (!holding ? "weapons/blunderbuss/attack-down" : "weapons/blunderbuss/heavy-down-holding") : "weapons/blunderbuss/heavy-down-charge") : "weapons/blunderbuss/heavy-down-shoot"))) : ((double) this.state.facingAngle <= 65.0 || (double) this.state.facingAngle >= 115.0 ? (!heavyShoot ? (!charging ? (!holding ? "weapons/blunderbuss/attack-up-diagonal" : "weapons/blunderbuss/heavy-up-diagonal-holding") : "weapons/blunderbuss/heavy-up-diagonal-charge") : "weapons/blunderbuss/heavy-up-diagonal-shoot") : (!heavyShoot ? (!charging ? (!holding ? "weapons/blunderbuss/attack-up" : "weapons/blunderbuss/heavy-up-holding") : "weapons/blunderbuss/heavy-up-charge") : "weapons/blunderbuss/heavy-up-shoot"));
  }

  public bool DoBlunderbussAttack(
    float power,
    Vector3 scale,
    Health.AttackTypes attackType,
    bool heavy)
  {
    this.skeletonAnimation.AnimationState.SetAnimation(0, this.GetBlunderbussAnimationAngle(false, false, heavy), false).MixDuration = 0.0f;
    this.blunderAmmo.Init(this.playerFarming);
    if (!this.blunderAmmo.UseAmmo())
    {
      AudioManager.Instance.PlayOneShot("event:/unlock_building/unlock", this.gameObject);
      this.skeletonAnimation.AnimationState.SetAnimation(0, "weapons/blunderbuss/no-ammo", false).MixDuration = 0.0f;
      this.blunderAmmo.JamBlunderbuss();
      return false;
    }
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    CameraManager.shakeCamera(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].CameraShake, this.state.facingAngle);
    if (this.IsLegendaryWeapon())
      AudioManager.Instance.PlayOneShot("event:/dlc/weapon/blunderbuss_legendary_shoot", this.gameObject);
    else if (heavy)
      AudioManager.Instance.PlayOneShot("event:/weapon/blunderbuss_heavy_shoot", this.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/weapon/blunderbuss_shoot", this.gameObject);
    Vector3 Position = this.transform.position + new Vector3(this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f);
    Health.AttackFlags AttackFlags = (Health.AttackFlags) 0 | attackFlags;
    if (TrinketManager.HasTrinket(TarotCards.Card.Skull, this.playerFarming))
      AttackFlags |= Health.AttackFlags.Skull;
    if (TrinketManager.HasTrinket(TarotCards.Card.Spider, this.playerFarming))
      AttackFlags |= Health.AttackFlags.Poison;
    Swipe component;
    if (heavy)
    {
      AttackFlags |= Health.AttackFlags.Penetration;
      component = UnityEngine.Object.Instantiate<Swipe>(this.blunderAmmo.swipeHeavy).GetComponent<Swipe>();
    }
    else
      component = UnityEngine.Object.Instantiate<Swipe>(this.blunderAmmo.swipe).GetComponent<Swipe>();
    component.destroyAfterDuration = true;
    component.Init(Position, this.state.facingAngle, (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team), this.health, new Action<Health, Health.AttackTypes, Health.AttackFlags, float>(this.BlunderBussEnemyCallBack), this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].RangeRadius * this.playerFarming.CurrentWeaponInfo.RangeMultiplier, 0.0f, this.GetCritChance(), attackType, AttackFlags);
    component.gameObject.transform.localScale = scale;
    if (this.playerFarming.CurrentWeaponInfo.WeaponData.ContainsAttachmentType(AttachmentEffect.Fire))
    {
      this.playerFarming.playerSpells.AimAngle = this.state.facingAngle;
      this.playerFarming.playerSpells.Spell_Fireball(EquipmentType.Fireball_Triple, knockback: false, shooter: this.gameObject, damageMultiplier: 0.15f, playSFX: false);
    }
    return true;
  }

  public void BlunderBussEnemyCallBack(
    Health h,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags,
    float critMultiplier)
  {
    float num = Mathf.Clamp(Vector3.Distance(this.transform.position, h.transform.position), 1.5f, float.MaxValue) * (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Blunderbuss) ? 0.75f : 1f);
    float damage1 = this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].Damage;
    float damage2 = PlayerWeapon.GetDamage(AttackType != Health.AttackTypes.Heavy ? damage1 * 1.2f : PlayerWeapon.HeavyBlunderbussDamage, this.playerFarming.currentWeaponLevel, this.playerFarming);
    if ((double) critMultiplier > 1.0 && AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit))
      damage2 *= critMultiplier;
    float Damage = damage2 / num;
    h.DealDamage(Damage, this.gameObject, Vector3.Lerp(this.transform.position, h.transform.position, 0.8f), AttackType: AttackType, AttackFlags: AttackFlags);
  }

  public void DoSlowMo(bool setZoom = true)
  {
    this.StartCoroutine((IEnumerator) this.SlowMo(setZoom));
  }

  public IEnumerator SlowMo(bool setZoom = true)
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

  public void CreateSwipe(
    float Angle,
    int Combo,
    float BaseDamage,
    Health.AttackTypes AttackType,
    float Radius,
    Health.AttackFlags AttackFlags)
  {
    Addressables_wrapper.InstantiateAsync((object) $"Assets/Prefabs/Enemies/Weapons/PlayerSwipe{Combo}.prefab", this.transform.position + new Vector3(Radius * 0.5f * Mathf.Cos(Angle * ((float) Math.PI / 180f)), Radius * 0.5f * Mathf.Sin(Angle * ((float) Math.PI / 180f)), -0.5f), Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<Swipe>().Init(this.transform.position + new Vector3(Radius * 0.5f * Mathf.Cos(Angle * ((float) Math.PI / 180f)), Radius * 0.5f * Mathf.Sin(Angle * ((float) Math.PI / 180f)), -0.5f), Angle, (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team), this.health, new Action<Health, Health.AttackTypes, Health.AttackFlags, float>(this.HitEnemyCallBack), Radius, PlayerWeapon.GetDamage(BaseDamage, this.playerFarming.currentWeaponLevel, this.playerFarming), this.GetCritChance(), AttackType, AttackFlags)));
  }

  public static float GetDamage(float BaseDamage, int WeaponLevel, PlayerFarming playerFarming)
  {
    float num = (float) ((double) (1f + TrinketManager.GetWeaponDamageMultiplerIncrease(playerFarming) + DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL + PlayerFleeceManager.GetWeaponDamageMultiplier() + DataManager.GetWeaponDamageMultiplier(WeaponLevel)) + ((double) playerFarming.playerRelic.PlayerScaleModifier > 1.0 ? 0.20000000298023224 : 0.0) + (playerFarming.playerRelic.DoubleDamage ? 1.0 : 0.0)) + playerFarming.playerRelic.DamageMultiplier;
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

  public float GetCritChance()
  {
    return 0.0f + TrinketManager.GetWeaponCritChanceIncrease(this.playerFarming);
  }

  public void HitEnemyCallBack(
    Health h,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags,
    float critMultiplier)
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
    switch (EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon).PrimaryEquipmentType)
    {
      case EquipmentType.Sword:
        MMVibrate.Rumble(0.5f, 0.1f, 0.33f, (MonoBehaviour) this, this.playerFarming);
        break;
      case EquipmentType.Axe:
        MMVibrate.Rumble(0.5f, 0.15f, 0.5f, (MonoBehaviour) this, this.playerFarming);
        break;
      case EquipmentType.Dagger:
        MMVibrate.Rumble(0.25f, 0.075f, 0.25f, (MonoBehaviour) this, this.playerFarming);
        break;
      default:
        MMVibrate.Rumble(0.5f, 0.1f, 0.33f, (MonoBehaviour) this, this.playerFarming);
        break;
    }
    if (h.HasShield && AttackType != Health.AttackTypes.Projectile)
    {
      this.state.facingAngle = Utils.GetAngle(this.transform.position, h.transform.position);
      this.playerController.CancelLunge(0.0f);
      this.playerController.unitObject.knockBackVX = (h.WasJustParried ? -1f : -0.75f) * Mathf.Cos(Utils.GetAngle(this.transform.position, h.transform.position) * ((float) Math.PI / 180f));
      this.playerController.unitObject.knockBackVY = (h.WasJustParried ? -1f : -0.75f) * Mathf.Sin(Utils.GetAngle(this.transform.position, h.transform.position) * ((float) Math.PI / 180f));
    }
    if (h.IsDeflecting && AttackType != Health.AttackTypes.Projectile)
    {
      this.state.facingAngle = Utils.GetAngle(this.transform.position, h.transform.position);
      this.playerController.CancelLunge(0.0f);
      this.playerController.unitObject.knockBackVX = (h.WasJustParried ? -1f : -0.75f) * Mathf.Cos(Utils.GetAngle(this.transform.position, h.transform.position) * ((float) Math.PI / 180f));
      this.playerController.unitObject.knockBackVY = (h.WasJustParried ? -1f : -0.75f) * Mathf.Sin(Utils.GetAngle(this.transform.position, h.transform.position) * ((float) Math.PI / 180f));
      BiomeConstants.Instance.EmitBlockElectricImpact(h.transform.position + Vector3.back / 2f, 0.0f, h.transform);
      if (!string.IsNullOrEmpty(this.invincibleOnHitSFX))
        AudioManager.Instance.PlayOneShot(this.invincibleOnHitSFX);
    }
    if (h.DontCombo)
      this.playerController.CancelLunge(h.team == Health.Team.Team2 ? this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[this.CurrentCombo].HitKnockback : 0.0f);
    if (!h.OnHitBlockAttacker && !h.WasJustParried || (UnityEngine.Object) h.AttackerToBlock == (UnityEngine.Object) null)
      return;
    this.playerController.BlockAttacker(h.AttackerToBlock);
  }

  public void Update()
  {
    if (!LocationManager.LocationIsDungeon(PlayerFarming.Location) && !this.ForceWeapons || !DataManager.Instance.EnabledSword || (bool) (UnityEngine.Object) this.playerFarming && this.playerFarming.currentWeapon == EquipmentType.None || (UnityEngine.Object) this.thrownAxe != (UnityEngine.Object) null)
      return;
    if ((this.state.CURRENT_STATE != StateMachine.State.Attacking || (this.DoHeavyAttack || this.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType != EquipmentType.Hammer) && (!this.DoHeavyAttack || this.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType != EquipmentType.Sword) || (double) Time.timeScale <= 0.0 || LetterBox.IsPlaying || MMConversation.CURRENT_CONVERSATION != null && !MMConversation.isBark) && this.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
    {
      AudioManager.Instance.StopLoop(this.HeavyAttackSound);
      if ((bool) (UnityEngine.Object) this.playerFarming)
      {
        this.playerFarming.HideWeaponChargeBars();
        this.playerFarming.HideHeavyChargeBars();
      }
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Attacking && this.ShowHeavyAim)
    {
      this.ShowHeavyAim = false;
      if ((bool) (UnityEngine.Object) this.playerFarming)
        this.playerFarming.HideHeavyChargeBars();
      AudioManager.Instance.StopLoop(this.HeavyAttackSound);
    }
    if ((double) Time.timeScale <= 0.0 || (bool) (UnityEngine.Object) this.playerFarming && this.playerFarming.GoToAndStopping)
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
    if ((UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) null && this.playerFarming.interactor.CurrentInteraction.HasSecondaryInteraction && this.playerFarming.interactor.CurrentInteraction.SecondaryAction == 2)
      return;
    if (this.state.CURRENT_STATE == StateMachine.State.Dodging)
    {
      AudioManager.Instance.StopLoop(this.HeavyAttackSound);
      AudioManager.Instance.StopLoop(this.loopedSound);
    }
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
      case StateMachine.State.Dodging:
      case StateMachine.State.Stealth:
        if (InputManager.Gameplay.GetAttackButtonDown(this.playerFarming))
        {
          if (this.attackRoutine != null)
            this.StopCoroutine(this.attackRoutine);
          this.attackRoutine = this.playerFarming.CurrentWeaponInfo.WeaponType != EquipmentType.Hammer ? this.StartCoroutine((IEnumerator) this.DoAttackRoutine()) : this.StartCoroutine((IEnumerator) this.DoAttackRoutineButtonUp());
        }
        if (this.DoingHeavyAttack)
          Debug.Log((object) "DOING HEAVY ATTACK ALREADY");
        if (InputManager.Gameplay.GetHeavyAttackButtonDown(this.playerFarming) && !this.DoingHeavyAttack && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled && !LetterBox.IsPlaying)
        {
          if (this.playerFarming.playerSpells.faithAmmo.CanAfford((float) this.HeavyAttackFervourCost))
          {
            if (this.attackRoutine != null)
              this.StopCoroutine(this.attackRoutine);
            this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine(true));
            break;
          }
          this.playerFarming.playerSpells.faithAmmo.UseAmmo((float) this.HeavyAttackFervourCost, false);
          break;
        }
        break;
    }
    if (InputManager.Gameplay.GetHeavyAttackButtonDown(this.playerFarming) && !this.DoingHeavyAttack && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled && !LetterBox.IsPlaying && this.state.CURRENT_STATE != StateMachine.State.InActive && this.state.CURRENT_STATE != StateMachine.State.CustomAnimation && this.state.CURRENT_STATE != StateMachine.State.HitThrown && this.state.CURRENT_STATE != StateMachine.State.HitRecover)
    {
      if (this.playerFarming.playerSpells.faithAmmo.CanAfford((float) this.HeavyAttackFervourCost))
      {
        if (this.attackRoutine != null)
          this.StopCoroutine(this.attackRoutine);
        this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine(true));
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/player/Curses/noarrows", this.gameObject);
        this.playerFarming.playerSpells.faithAmmo.UseAmmo((float) this.HeavyAttackFervourCost, false);
      }
    }
    if (this.criticalTimer.gameObject.activeSelf)
    {
      if (this.playerFarming.CurrentWeaponInfo != null && (double) this.playerFarming.CurrentWeaponInfo.CriticalChance > 0.0)
      {
        this.criticalTimer.UpdateCharging(PlayerWeapon.CriticalHitTimer / this.criticalHitChargeDuration);
        PlayerWeapon.CriticalHitTimer += Time.deltaTime;
      }
      if (TrinketManager.HasTrinket(TarotCards.Card.EmptyFervourCritical, this.playerFarming))
      {
        if (Mathf.FloorToInt(this.playerFarming.playerSpells.faithAmmo.Ammo / (float) this.playerFarming.playerSpells.AmmoCost) <= 0)
        {
          this.criticalTimer.UpdateCharging(float.MaxValue);
          PlayerWeapon.CriticalHitTimer = this.criticalHitChargeDuration;
        }
        else if ((double) this.playerFarming.CurrentWeaponInfo.CriticalChance <= 0.0)
        {
          this.criticalTimer.UpdateCharging(0.0f);
          PlayerWeapon.CriticalHitTimer = 0.0f;
        }
      }
    }
    if ((double) this.damageNegationTimer <= 0.0)
      return;
    this.damageNegationTimer -= Time.deltaTime;
    this.damageNegation.UpdateCharging(this.damageNegationTimer / this.damageNegationTime);
    if ((double) this.damageNegationTimer > 0.0)
      return;
    this.ChargedNegation = false;
    this.damageNegation.transform.DOKill();
    this.damageNegation.transform.DOScale(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.damageNegation.gameObject.SetActive(false)));
  }

  public void DoAttack()
  {
    if (this.attackRoutine != null)
      this.StopCoroutine(this.attackRoutine);
    if (this.playerFarming.CurrentWeaponInfo.WeaponType == EquipmentType.Hammer)
      this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutineButtonUp());
    else
      this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine());
  }

  public void ManualHit(Health.Team? overrideTeam = null)
  {
    this.overrideTeam = overrideTeam;
    if (this.attackRoutine != null)
      this.StopCoroutine(this.attackRoutine);
    if (this.playerFarming.CurrentWeaponInfo.WeaponType == EquipmentType.Hammer)
      this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutineButtonUp());
    else
      this.attackRoutine = this.StartCoroutine((IEnumerator) this.DoAttackRoutine());
  }

  public void SetWeapon(EquipmentType weaponType, int WeaponLevel)
  {
    Debug.Log((object) $"WEAPON SET {weaponType.ToString()}  {WeaponLevel.ToString()}");
    if (weaponType == EquipmentType.None)
      return;
    AudioManager.Instance.ToggleFilter("heavy_attack_lvl_2", false);
    if (weaponType == EquipmentType.Axe && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Axe))
      AudioManager.Instance.ToggleFilter("heavy_attack_lvl_2", true);
    if (weaponType == EquipmentType.Sword && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Sword))
      AudioManager.Instance.ToggleFilter("heavy_attack_lvl_2", true);
    if (weaponType == EquipmentType.Gauntlet && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Gauntlets))
      AudioManager.Instance.ToggleFilter("heavy_attack_lvl_2", true);
    if (weaponType == EquipmentType.Dagger && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger))
      AudioManager.Instance.ToggleFilter("heavy_attack_lvl_2", true);
    if (weaponType == EquipmentType.Hammer && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer))
      AudioManager.Instance.ToggleFilter("heavy_attack_lvl_2", true);
    this.playerFarming.CurrentWeaponInfo.WeaponType = weaponType;
    this.playerFarming.CurrentWeaponInfo.WeaponData = EquipmentManager.GetWeaponData(weaponType);
    if (weaponType != EquipmentType.None && (bool) (UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData && this.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType == EquipmentType.Blunderbuss && this.playerFarming.canUseKeyboard)
    {
      this.blunderAmmo.Init(this.playerFarming);
      this.ShowBlunderTutorial();
    }
    else if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
    if (weaponType != EquipmentType.None && (bool) (UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData && this.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType == EquipmentType.Shield)
      this.ShowShieldTutorial();
    if (weaponType == EquipmentType.Blunderbuss_Legendary && !DataManager.Instance.LegendaryBlunderbussPlimboEaterEggTalked)
      DataManager.Instance.LegendaryBlunderbussPlimboEasterEggActive = true;
    if ((UnityEngine.Object) this.criticalTimer != (UnityEngine.Object) null && this.playerFarming.CurrentWeaponInfo != null && (bool) (UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData)
      this.criticalTimer.gameObject.SetActive((bool) (UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData.GetAttachment(AttachmentEffect.Critical) || TrinketManager.HasTrinket(TarotCards.Card.EmptyFervourCritical, this.playerFarming));
    if (this.playerFarming.currentWeapon != weaponType)
      PlayerWeapon.CriticalHitTimer = 0.0f;
    this.ChargedNegation = false;
    if ((UnityEngine.Object) this.damageNegation != (UnityEngine.Object) null)
      this.damageNegation.gameObject.SetActive(false);
    this.playerFarming.currentWeapon = weaponType;
    this.playerFarming.currentWeaponLevel = WeaponLevel;
    this.DoAttachmentEffect(AttachmentState.Constant);
    this.CurrentWeaponLevel = WeaponLevel;
    this.CurrentCombo = 0;
    this.DoHeavyAttack = false;
    this.DoingHeavyAttack = false;
    PlayerWeapon.WeaponEvent onWeaponChanged = PlayerWeapon.OnWeaponChanged;
    if (onWeaponChanged != null)
      onWeaponChanged(this.playerFarming.currentWeapon, this.playerFarming.currentWeaponLevel, this.playerFarming);
    this.playerFarming.SetSkin();
  }

  public void ShowShieldTutorial()
  {
  }

  public void ShowBlunderTutorial()
  {
    if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.BlunderWeapon))
      return;
    MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.BlunderWeapon);
  }

  public void StopAttackRoutine()
  {
    if (this.attackRoutine != null)
      this.StopCoroutine(this.attackRoutine);
    this.DoingHeavyAttack = false;
    this.CurrentAttackState = PlayerWeapon.AttackState.CanBreak;
  }

  public void StopHeavyAttackRoutine()
  {
    if (!this.DoingHeavyAttack)
      return;
    this.StopAttackRoutine();
  }

  public IEnumerator DoAttackRoutineButtonUp(bool ForceHeavyAttack = false)
  {
    PlayerWeapon playerWeapon = this;
    if (!playerWeapon.playerFarming.IsKnockedOut)
    {
      playerWeapon.DoHeavyAttack = ForceHeavyAttack;
      if (!ForceHeavyAttack && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled)
      {
        if (playerWeapon.playerFarming.playerSpells.faithAmmo.CanAfford((float) playerWeapon.HeavyAttackFervourCost))
        {
          float HoldTimer = 0.0f;
          while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming) && (double) HoldTimer < (double) playerWeapon.MaxHold && playerWeapon.playerFarming.state.CURRENT_STATE != StateMachine.State.HitThrown)
          {
            if (playerWeapon.playerFarming.state.CURRENT_STATE != StateMachine.State.Attacking)
            {
              playerWeapon.DoingHeavyAttack = false;
              playerWeapon.HoldingForHeavyAttack = false;
              yield break;
            }
            HoldTimer += Time.deltaTime;
            if ((double) HoldTimer >= (double) playerWeapon.MaxHold)
              playerWeapon.DoHeavyAttack = true;
            else
              yield return (object) null;
          }
        }
        else
          playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost, false);
      }
      playerWeapon.DoingHeavyAttack = playerWeapon.DoHeavyAttack;
      playerWeapon.TimeOfAttack = Time.time;
      playerWeapon.playerVFX.stopEmitChargingParticles();
      playerWeapon.CanChangeDirection = true;
      if (playerWeapon.playerFarming.CurrentWeaponInfo != null && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanChangeDirectionDuringAttack)
        playerWeapon.StoreFacing = playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir;
      playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
      playerWeapon.aimTimer = 0.0f;
      if (!playerWeapon.DoHeavyAttack && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator || playerWeapon.DoHeavyAttack && EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Sword)
      {
        bool flag = EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Sword;
        playerWeapon.playerFarming.ShowWeaponChargeBars(flag ? 2.5f : 1.5f);
      }
      playerWeapon.HoldingForHeavyAttack = true;
      if (playerWeapon.DoHeavyAttack)
      {
        switch (EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType)
        {
          case EquipmentType.Sword:
            if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Sword))
            {
              playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-sword2", false).MixDuration = 0.0f;
              playerWeapon.HeavyAttackSound = AudioManager.Instance.CreateLoop("event:/weapon/sword_heavy/sword_heavy_lvl2", playerWeapon.gameObject, true);
              break;
            }
            playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-sword", false).MixDuration = 0.0f;
            playerWeapon.HeavyAttackSound = AudioManager.Instance.CreateLoop("event:/weapon/sword_heavy/sword_heavy_lvl1", playerWeapon.gameObject, true);
            break;
          case EquipmentType.Axe:
            playerWeapon.DoHeavyAttack = false;
            playerWeapon.StopAllCoroutines();
            playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAxeHeavyAttack());
            yield break;
          case EquipmentType.Hammer:
            playerWeapon.DoHeavyAttack = false;
            playerWeapon.StopAllCoroutines();
            playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoHammerHeavyAttack());
            yield break;
          case EquipmentType.Dagger:
            playerWeapon.HeavyAttackSound = !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger) ? AudioManager.Instance.CreateLoop("event:/weapon/dagger_heavy/dagger_heavy_lvl1", playerWeapon.gameObject, true) : AudioManager.Instance.CreateLoop("event:/weapon/dagger_heavy/dagger_heavy_lvl2", playerWeapon.gameObject, true);
            playerWeapon.HeavyAimSpeed = 0.1f;
            playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-dagger", false).MixDuration = 0.0f;
            playerWeapon.ShowHeavyAim = true;
            playerWeapon.playerFarming.ShowHeavyAttackProjectileChargeBars();
            break;
          case EquipmentType.Gauntlet:
            playerWeapon.CanChangeDirection = false;
            playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost, false);
            if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Gauntlets))
            {
              playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-gauntlet2", false).MixDuration = 0.0f;
              AudioManager.Instance.PlayOneShot("event:/weapon/gauntlet_heavy/gauntlet_lvl2", playerWeapon.gameObject);
              break;
            }
            playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-gauntlet", false).MixDuration = 0.0f;
            break;
          case EquipmentType.Blunderbuss:
            playerWeapon.DoHeavyAttack = false;
            playerWeapon.StopAllCoroutines();
            playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoBlunderHeavyAttack());
            yield break;
          case EquipmentType.Shield:
            playerWeapon.DoHeavyAttack = false;
            playerWeapon.StopAllCoroutines();
            playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoShieldHeavyAttack());
            yield break;
          case EquipmentType.Chain:
            playerWeapon.DoHeavyAttack = false;
            playerWeapon.StopAllCoroutines();
            playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoChainHeavyAttack());
            break;
          default:
            playerWeapon.DoHeavyAttack = false;
            break;
        }
      }
      if (!playerWeapon.DoHeavyAttack)
        playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].Animation, false).MixDuration = 0.0f;
      playerWeapon.StealthSneakAttack = playerWeapon.state.CURRENT_STATE == StateMachine.State.Stealth;
      float num1 = playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging ? 1.25f : 1f;
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
        playerWeapon.playerController.speed = playerWeapon.playerController.DodgeSpeed * 1.2f;
      if (!playerWeapon.DoHeavyAttack && playerWeapon.playerFarming.CurrentWeaponInfo != null)
        playerWeapon.playerController.Lunge(playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].LungeDuration * num1, playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].LungeSpeed * num1);
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
      if (DataManager.Instance.SpawnPoisonOnAttack)
        TrapPoison.CreatePoison(playerWeapon.transform.position, 2, 0.2f, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
      if (TrinketManager.HasTrinket(TarotCards.Card.HandsOfRage, playerWeapon.playerFarming) && !TrinketManager.IsOnCooldown(TarotCards.Card.HandsOfRage, playerWeapon.playerFarming))
      {
        playerWeapon.playerFarming.playerSpells.AimAngle = playerWeapon.state.LookAngle;
        playerWeapon.playerFarming.playerSpells.Spell_Fireball(EquipmentType.Fireball, knockback: false);
        TrinketManager.TriggerCooldown(TarotCards.Card.HandsOfRage, playerWeapon.playerFarming);
      }
      bool QueueAttack = false;
      float QueueAttackTimer = 0.0f;
      float QueueHeavyAtackTimer = 0.0f;
      playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackStart);
      float num2 = playerWeapon.playerFarming.CurrentWeaponInfo.AttackRateMultiplier + TrinketManager.GetAttackRateMultiplier(playerWeapon.playerFarming);
      playerWeapon.skeletonAnimation.timeScale = num2;
      if (playerWeapon.playerFarming.CurrentWeaponInfo != null && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator)
        AudioManager.Instance.PlayOneShot("event:/weapon/melee_charge", playerWeapon.gameObject);
      while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
      {
        if (playerWeapon.playerFarming.state.CURRENT_STATE != StateMachine.State.Attacking)
        {
          playerWeapon.DoingHeavyAttack = false;
          playerWeapon.HoldingForHeavyAttack = false;
          yield break;
        }
        if (!InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
          playerWeapon.HoldingForHeavyAttack = false;
        if (playerWeapon.CanChangeDirection && playerWeapon.playerFarming.CurrentWeaponInfo != null && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanChangeDirectionDuringAttack && ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement))
          playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
        if (playerWeapon.CanChangeDirection && (playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanFreelyChangeDirection || playerWeapon.DoHeavyAttack) && ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement))
          playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
        if (playerWeapon.playerFarming.canUseKeyboard && playerWeapon.CanChangeDirection && InputManager.General.MouseInputActive && playerWeapon.state.CURRENT_STATE == StateMachine.State.Attacking)
        {
          float angle = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position), (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
          playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = angle;
        }
        if ((playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator || playerWeapon.DoHeavyAttack && EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Sword) && (double) Time.timeScale > 0.0)
        {
          playerWeapon.aimTimer += Time.deltaTime;
          float facingAngle = playerWeapon.state.facingAngle;
          playerWeapon.playerFarming.SetWeaponAimingRecticuleScaleAndRotation(0, Vector3.one, new Vector3(0.0f, 0.0f, facingAngle));
        }
        if (playerWeapon.ShowHeavyAim && (double) Time.timeScale > 0.0)
        {
          playerWeapon.aimTimer += Time.deltaTime;
          playerWeapon.playerFarming.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(Mathf.SmoothStep(0.0f, 1f, playerWeapon.aimTimer / playerWeapon.HeavyAimSpeed), 1f, 1f), new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
        }
        if ((double) (QueueAttackTimer += Time.deltaTime) > 0.10000000149011612 && playerWeapon.CurrentCombo < playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos.Count && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanQueueNextAttack && InputManager.Gameplay.GetAttackButtonDown(playerWeapon.playerFarming))
          QueueAttack = true;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled)
        {
          if (!playerWeapon.DoingHeavyAttack && InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
          {
            QueueHeavyAtackTimer += Time.deltaTime;
            if ((double) QueueHeavyAtackTimer > 0.30000001192092896)
              QueueAttack = true;
          }
          else
            QueueHeavyAtackTimer = 0.0f;
        }
        yield return (object) null;
      }
      playerWeapon.overrideTeam = new Health.Team?();
      playerWeapon.CurrentCombo = (int) Utils.Repeat((float) (playerWeapon.CurrentCombo + 1), (float) playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos.Count);
      while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.CanBreak && playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      {
        if (!InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
        {
          AudioManager.Instance.StopLoop(playerWeapon.HeavyAttackSound);
          playerWeapon.HoldingForHeavyAttack = false;
        }
        playerWeapon.skeletonAnimation.timeScale = 1f;
        if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        {
          if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Attacking)
          {
            if (!playerWeapon.DoHeavyAttack || EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType != EquipmentType.Axe)
            {
              System.Action crownReturnSubtle = playerWeapon.playerFarming.OnCrownReturnSubtle;
              if (crownReturnSubtle != null)
                crownReturnSubtle();
            }
            playerWeapon.state.CURRENT_STATE = StateMachine.State.Moving;
          }
          playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackEnd);
          yield break;
        }
        if ((QueueAttack || InputManager.Gameplay.GetAttackButtonDown(playerWeapon.playerFarming)) && (LocationManager.LocationIsDungeon(PlayerFarming.Location) || playerWeapon.ForceWeapons) && (UnityEngine.Object) playerWeapon.thrownAxe == (UnityEngine.Object) null)
        {
          playerWeapon.StopAllCoroutines();
          GameManager.SetTimeScale(1f);
          GameManager.GetInstance().CameraResetTargetZoom();
          playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackEnd);
          if (playerWeapon.attackRoutine != null)
            playerWeapon.StopCoroutine(playerWeapon.attackRoutine);
          playerWeapon.attackRoutine = playerWeapon.playerFarming.CurrentWeaponInfo.WeaponType != EquipmentType.Hammer ? playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAttackRoutine()) : playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAttackRoutineButtonUp());
          yield break;
        }
        yield return (object) null;
      }
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
      playerWeapon.DoingHeavyAttack = false;
    }
  }

  public float TimeOfAttack
  {
    get => this.\u003CTimeOfAttack\u003Ek__BackingField;
    set => this.\u003CTimeOfAttack\u003Ek__BackingField = value;
  }

  public IEnumerator DoAttackRoutine(bool ForceHeavyAttack = false, bool followingHeavyAttack = false)
  {
    PlayerWeapon playerWeapon = this;
    if (!playerWeapon.playerFarming.IsKnockedOut)
    {
      if (playerWeapon.playerFarming.CurrentWeaponInfo == null || playerWeapon.playerFarming.currentWeapon == EquipmentType.None || !(bool) (UnityEngine.Object) playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData || playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos == null || playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos.Count <= playerWeapon.CurrentCombo || playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo] == null)
      {
        Debug.LogWarning((object) "TODO Prevented error temporarily caused by co-op - this should be fixed properly");
      }
      else
      {
        playerWeapon.TimeOfAttack = Time.time;
        playerWeapon.DoHeavyAttack = ForceHeavyAttack;
        playerWeapon.DoingHeavyAttack = playerWeapon.DoHeavyAttack;
        playerWeapon.playerVFX.stopEmitChargingParticles();
        playerWeapon.CanChangeDirection = true;
        if (playerWeapon.playerFarming.CurrentWeaponInfo != null && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanChangeDirectionDuringAttack)
          playerWeapon.StoreFacing = playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir;
        playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
        playerWeapon.aimTimer = 0.0f;
        if (!playerWeapon.DoHeavyAttack && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator || playerWeapon.DoHeavyAttack && EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Sword)
        {
          bool flag = EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Sword;
          playerWeapon.playerFarming.ShowWeaponChargeBars(flag ? 2.5f : 1.5f);
        }
        playerWeapon.HoldingForHeavyAttack = true;
        if (playerWeapon.DoHeavyAttack)
        {
          switch (EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType)
          {
            case EquipmentType.Sword:
              if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Sword))
              {
                playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-sword2", false).MixDuration = 0.0f;
                playerWeapon.HeavyAttackSound = AudioManager.Instance.CreateLoop("event:/weapon/sword_heavy/sword_heavy_lvl2", playerWeapon.gameObject, true);
                break;
              }
              playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-sword", false).MixDuration = 0.0f;
              playerWeapon.HeavyAttackSound = AudioManager.Instance.CreateLoop("event:/weapon/sword_heavy/sword_heavy_lvl1", playerWeapon.gameObject, true);
              break;
            case EquipmentType.Axe:
              playerWeapon.DoHeavyAttack = false;
              playerWeapon.StopAllCoroutines();
              playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAxeHeavyAttack());
              yield break;
            case EquipmentType.Hammer:
              playerWeapon.DoHeavyAttack = false;
              playerWeapon.StopAllCoroutines();
              playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoHammerHeavyAttack());
              yield break;
            case EquipmentType.Dagger:
              playerWeapon.DoHeavyAttack = false;
              playerWeapon.StopAllCoroutines();
              playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoDaggerHeavyAttack());
              yield break;
            case EquipmentType.Gauntlet:
              playerWeapon.CanChangeDirection = false;
              AudioManager.Instance.PlayOneShot("event:/weapon/gauntlet_heavy/gauntlet_scream", playerWeapon.gameObject);
              playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost, false);
              if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Gauntlets))
              {
                playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-gauntlet2", false).MixDuration = 0.0f;
                AudioManager.Instance.PlayOneShot("event:/weapon/gauntlet_heavy/gauntlet_lvl2", playerWeapon.gameObject);
                break;
              }
              playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-gauntlet", false).MixDuration = 0.0f;
              break;
            case EquipmentType.Blunderbuss:
              playerWeapon.DoHeavyAttack = false;
              playerWeapon.StopAllCoroutines();
              playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoBlunderHeavyAttack());
              yield break;
            case EquipmentType.Shield:
              playerWeapon.DoHeavyAttack = false;
              playerWeapon.StopAllCoroutines();
              Debug.Log((object) "SHIELD HEAVY ATTACK TIME 2");
              playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoShieldHeavyAttack());
              yield break;
            case EquipmentType.Chain:
              playerWeapon.DoHeavyAttack = false;
              playerWeapon.StopAllCoroutines();
              playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoChainHeavyAttack());
              yield break;
            default:
              playerWeapon.DoHeavyAttack = false;
              break;
          }
        }
        if (!playerWeapon.DoHeavyAttack)
          playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].Animation, false).MixDuration = 0.0f;
        if (EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Shield)
        {
          if (playerWeapon.ShieldHoldCoroutine != null)
          {
            playerWeapon.StopCoroutine(playerWeapon.ShieldHoldCoroutine);
            playerWeapon.ShieldHoldCoroutine = (Coroutine) null;
          }
          playerWeapon.ShieldHoldCoroutine = playerWeapon.StartCoroutine((IEnumerator) playerWeapon.WaitForShieldBlocking());
          Debug.Log((object) "Shiwlf coroutinge should start here");
        }
        playerWeapon.StealthSneakAttack = playerWeapon.state.CURRENT_STATE == StateMachine.State.Stealth;
        float num1 = playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging ? 1.25f : 1f;
        if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
          playerWeapon.playerController.speed = playerWeapon.playerController.DodgeSpeed * 1.2f;
        if (!playerWeapon.DoHeavyAttack)
          playerWeapon.playerController.Lunge(playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].LungeDuration * num1, playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].LungeSpeed * num1);
        playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
        if (DataManager.Instance.SpawnPoisonOnAttack)
          TrapPoison.CreatePoison(playerWeapon.transform.position, 2, 0.2f, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
        if (TrinketManager.HasTrinket(TarotCards.Card.HandsOfRage, playerWeapon.playerFarming) && !TrinketManager.IsOnCooldown(TarotCards.Card.HandsOfRage, playerWeapon.playerFarming))
        {
          playerWeapon.playerFarming.playerSpells.AimAngle = playerWeapon.state.LookAngle;
          playerWeapon.playerFarming.playerSpells.Spell_Fireball(EquipmentType.Fireball, knockback: false);
          TrinketManager.TriggerCooldown(TarotCards.Card.HandsOfRage, playerWeapon.playerFarming);
        }
        bool QueueAttack = false;
        float QueueAttackTimer = 0.0f;
        bool DoForceHeavyAttack = false;
        float QueueHeavyAtackTimer = 0.0f;
        playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackStart);
        float num2 = playerWeapon.playerFarming.CurrentWeaponInfo.AttackRateMultiplier + TrinketManager.GetAttackRateMultiplier(playerWeapon.playerFarming);
        playerWeapon.skeletonAnimation.timeScale = num2;
        if (playerWeapon.playerFarming.CurrentWeaponInfo != null && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator)
          AudioManager.Instance.PlayOneShot("event:/weapon/melee_charge", playerWeapon.gameObject);
        float RecoilPowerMultiplier = playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].RecoilPowerMultiplier;
        while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
        {
          if (!InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
            playerWeapon.HoldingForHeavyAttack = false;
          if (playerWeapon.CanChangeDirection && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanChangeDirectionDuringAttack && ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement))
            playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
          if (playerWeapon.CanChangeDirection && (playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanFreelyChangeDirection || playerWeapon.DoHeavyAttack) && ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement))
            playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
          if (playerWeapon.playerFarming.canUseKeyboard && playerWeapon.CanChangeDirection && InputManager.General.MouseInputActive && playerWeapon.state.CURRENT_STATE == StateMachine.State.Attacking)
          {
            float angle = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position), (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
            playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = angle;
          }
          if ((playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].ShowDirectionIndicator || playerWeapon.DoHeavyAttack && EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Sword) && (double) Time.timeScale > 0.0)
          {
            playerWeapon.aimTimer += Time.deltaTime;
            float facingAngle = playerWeapon.state.facingAngle;
            playerWeapon.playerFarming.SetWeaponAimingRecticuleScaleAndRotation(0, Vector3.one, new Vector3(0.0f, 0.0f, facingAngle));
          }
          if (playerWeapon.ShowHeavyAim && (double) Time.timeScale > 0.0)
          {
            playerWeapon.aimTimer += Time.deltaTime;
            playerWeapon.playerFarming.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(Mathf.SmoothStep(0.0f, 1f, playerWeapon.aimTimer / playerWeapon.HeavyAimSpeed), 1f, 1f), new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
          }
          if ((double) (QueueAttackTimer += Time.deltaTime) > 0.10000000149011612 && playerWeapon.CurrentCombo < playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos.Count && playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].CanQueueNextAttack && InputManager.Gameplay.GetAttackButtonDown(playerWeapon.playerFarming))
            QueueAttack = true;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled)
          {
            if (!playerWeapon.DoingHeavyAttack && InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
            {
              QueueHeavyAtackTimer += Time.deltaTime;
              if ((double) QueueHeavyAtackTimer > 1.0)
              {
                QueueAttack = true;
                DoForceHeavyAttack = true;
              }
            }
            else
              QueueHeavyAtackTimer = 0.0f;
          }
          if ((double) RecoilPowerMultiplier != 0.0)
          {
            bool flag = true;
            if (EquipmentManager.GetWeaponData(playerWeapon.playerFarming.currentWeapon).PrimaryEquipmentType == EquipmentType.Blunderbuss)
              flag = playerWeapon.DoBlunderbussAttack(1f, Vector3.one, playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].AttackType, false);
            if (flag)
              playerWeapon.playerController.unitObject.DoKnockBack((float) (((double) playerWeapon.state.facingAngle + 180.0) % 360.0 * (Math.PI / 180.0)), RecoilPowerMultiplier, playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos[playerWeapon.CurrentCombo].RecoilDuration);
            RecoilPowerMultiplier = 0.0f;
          }
          yield return (object) null;
        }
        playerWeapon.overrideTeam = new Health.Team?();
        playerWeapon.CurrentCombo = (int) Utils.Repeat((float) (playerWeapon.CurrentCombo + 1), (float) playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.Combos.Count);
        while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.CanBreak)
        {
          if (!playerWeapon.DoingHeavyAttack && playerWeapon.state.CURRENT_STATE != StateMachine.State.Attacking)
            yield break;
          playerWeapon.skeletonAnimation.timeScale = 1f;
          if (playerWeapon.MoveFromCanBreakState())
            yield break;
          if ((QueueAttack || InputManager.Gameplay.GetAttackButtonDown(playerWeapon.playerFarming)) && (LocationManager.LocationIsDungeon(PlayerFarming.Location) || playerWeapon.ForceWeapons) && (UnityEngine.Object) playerWeapon.thrownAxe == (UnityEngine.Object) null)
          {
            playerWeapon.StopAllCoroutines();
            GameManager.SetTimeScale(1f);
            GameManager.GetInstance().CameraResetTargetZoom();
            playerWeapon.DoAttachmentEffect(AttachmentState.OnAttackEnd);
            if (playerWeapon.attackRoutine != null)
              playerWeapon.StopCoroutine(playerWeapon.attackRoutine);
            playerWeapon.attackRoutine = playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAttackRoutine(DoForceHeavyAttack));
            yield break;
          }
          if (!followingHeavyAttack && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && !DataManager.Instance.SpecialAttacksDisabled)
          {
            playerWeapon.HoldingForHeavyAttack = true;
            PlayerWeapon.EquippedWeaponsInfo currentWeaponInfo = playerWeapon.playerFarming.CurrentWeaponInfo;
            float delayTimeBeforeHeavyAttack = Time.realtimeSinceStartup + 0.4f;
            while ((double) Time.realtimeSinceStartup < (double) delayTimeBeforeHeavyAttack)
            {
              if (!InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming) || playerWeapon.CheckForDeath())
              {
                playerWeapon.HoldingForHeavyAttack = false;
                break;
              }
              yield return (object) null;
            }
            if (playerWeapon.HoldingForHeavyAttack)
            {
              playerWeapon.HoldingForHeavyAttack = false;
              if (!followingHeavyAttack && !playerWeapon.CheckForDeath())
              {
                playerWeapon.StopAllCoroutines();
                playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoAttackRoutine(true, true));
                yield break;
              }
            }
          }
          yield return (object) null;
        }
        if (!playerWeapon.CheckForDeath() && playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
          playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
        playerWeapon.DoingHeavyAttack = false;
      }
    }
  }

  public IEnumerator WaitForShieldBlocking()
  {
    PlayerWeapon playerWeapon = this;
    while (InputManager.Gameplay.GetAttackButtonHeld(playerWeapon.playerFarming) && (double) playerWeapon.skeletonAnimation.AnimationState.GetCurrent(0).AnimationTime < (double) playerWeapon.skeletonAnimation.AnimationState.GetCurrent(0).AnimationEnd * 0.25)
      yield return (object) null;
    if (InputManager.Gameplay.GetAttackButtonHeld(playerWeapon.playerFarming))
    {
      playerWeapon.StopAllCoroutines();
      playerWeapon.StartCoroutine((IEnumerator) playerWeapon.DoShieldBlocking());
    }
  }

  public IEnumerator DoShieldBlocking()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    playerWeapon.RepelProjectiles = true;
    playerWeapon.ShowHeavyAim = true;
    playerWeapon.shieldRepel.shieldAmmo.FillAllAmmo();
    playerWeapon.shieldRepel.shieldAmmo.gameObject.SetActive(true);
    playerWeapon.shieldRepel.shieldAmmo.ShowShieldHPBar();
    double time = (double) Time.time;
    playerWeapon.playerFarming.health.invincible = true;
    while (InputManager.Gameplay.GetAttackButtonHeld(playerWeapon.playerFarming))
    {
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.shieldRepel.gameObject.SetActive(false);
        yield break;
      }
      string shieldAnimationAngle = playerWeapon.GetShieldAnimationAngle();
      if (playerWeapon.lastShieldAnimationString != shieldAnimationAngle)
      {
        playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, shieldAnimationAngle, false);
        playerWeapon.lastShieldAnimationString = shieldAnimationAngle;
      }
      playerWeapon.playerController.speed = 0.0f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      playerWeapon.ShieldReflect(1f, playerWeapon.state.facingAngle);
      if (playerWeapon.playerFarming.health.invincible && (double) playerWeapon.shieldRepel.shieldAmmo.getShieldAmmo() < 1.0)
      {
        BiomeConstants.Instance.EmitBlockImpact(playerWeapon.transform.position, 0.0f, animation: "Break");
        playerWeapon.shieldRepel.DestroyShieldEffect();
        break;
      }
      yield return (object) null;
    }
    playerWeapon.shieldRepel.gameObject.SetActive(false);
    playerWeapon.shieldRepel.shieldAmmo.HideShieldHPBar();
    playerWeapon.playerFarming.health.invincible = false;
    System.Action onCrownReturn = playerWeapon.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.RepelProjectiles = false;
  }

  public void ShieldReflect(float distance, float targetAngle, float heavyPower = -1f)
  {
    this.shieldRepel.gameObject.SetActive(true);
    this.shieldRepel.SetAngle(this.state.facingAngle, heavyPower);
    float facingAngle = this.state.facingAngle;
    if ((double) facingAngle < 0.0)
      facingAngle += 360f;
    foreach (Projectile projectile in Projectile.Projectiles)
    {
      if (projectile.team != Health.Team.PlayerTeam && !projectile.IsProjectilesParent)
      {
        if (projectile.destroyOnParry)
          projectile.DestroyProjectile(true);
        else if (!projectile.IsAttachedToProjectileTrap())
        {
          Vector3 position = projectile.transform.position;
          if ((double) (this.transform.position - position).magnitude <= (double) distance && this.shieldRepel.shieldAmmo.UseAmmo())
          {
            float num = Utils.GetAngle(this.transform.position, position) + 180f;
            if ((double) num < 0.0)
              num += 360f;
            Mathf.Abs(num - facingAngle);
            projectile.Angle = (float) ((double) this.state.facingAngle - 5.0 + (double) UnityEngine.Random.value * 10.0);
            if ((double) projectile.angleNoiseFrequency == 0.0)
              projectile.Speed *= 1.5f;
            projectile.KnockedBack = true;
            projectile.team = Health.Team.PlayerTeam;
            projectile.health.team = Health.Team.PlayerTeam;
            if ((double) heavyPower == -1.0)
              this.playerFarming.unitObject.DoKnockBack((float) (((double) this.state.facingAngle + 170.0 + (double) UnityEngine.Random.value * 20.0) % 360.0 * (Math.PI / 180.0)), 0.15f, 0.15f);
            AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
            this.shieldRepel.DamageShieldEffect();
            CameraManager.shakeCamera(3f);
          }
        }
      }
    }
  }

  public bool CheckForDeath()
  {
    return this.state.CURRENT_STATE == StateMachine.State.Dieing || this.state.CURRENT_STATE == StateMachine.State.Dieing;
  }

  public bool MoveFromCanBreakState()
  {
    if (InputManager.Gameplay.GetHeavyAttackButtonHeld(this.playerFarming) || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(this.playerFarming)) <= (double) PlayerController.MinInputForMovement && (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(this.playerFarming)) <= (double) PlayerController.MinInputForMovement)
      return false;
    if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
    {
      if (!this.DoHeavyAttack || EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon).PrimaryEquipmentType != EquipmentType.Axe)
      {
        System.Action crownReturnSubtle = this.playerFarming.OnCrownReturnSubtle;
        if (crownReturnSubtle != null)
          crownReturnSubtle();
      }
      this.state.CURRENT_STATE = StateMachine.State.Moving;
      this.DoingHeavyAttack = false;
    }
    this.DoAttachmentEffect(AttachmentState.OnAttackEnd);
    return true;
  }

  public void DoAttachmentEffect(AttachmentState attachmentState)
  {
    if (attachmentState == AttachmentState.Constant)
      this.playerFarming.CurrentWeaponInfo.ResetMultipliers();
    foreach (WeaponAttachmentData weaponAttachmentData in this.GetAttachmentsWithState(this.playerFarming.currentWeapon, attachmentState))
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
            this.playerFarming.CurrentWeaponInfo.WeaponDamageMultiplier += weaponAttachmentData.DamageMultiplierIncrement;
            continue;
          case AttachmentEffect.Critical:
            this.playerFarming.CurrentWeaponInfo.CriticalChance += weaponAttachmentData.CriticalMultiplierIncrement;
            continue;
          case AttachmentEffect.Range:
            this.playerFarming.CurrentWeaponInfo.RangeMultiplier += weaponAttachmentData.RangeIncrement;
            continue;
          case AttachmentEffect.AttackRate:
            this.playerFarming.CurrentWeaponInfo.AttackRateMultiplier += weaponAttachmentData.AttackRateIncrement;
            continue;
          case AttachmentEffect.MovementSpeed:
            this.playerFarming.CurrentWeaponInfo.MovementSpeedMultiplier += weaponAttachmentData.MovementSpeedIncrement;
            continue;
          case AttachmentEffect.IncreasedXPDrop:
            this.playerFarming.CurrentWeaponInfo.XPDropMultiplier += weaponAttachmentData.xpDropIncrement;
            continue;
          case AttachmentEffect.HealChance:
            this.playerFarming.CurrentWeaponInfo.HealChance += weaponAttachmentData.healChanceIncrement;
            this.playerFarming.CurrentWeaponInfo.HealAmount += weaponAttachmentData.healAmount;
            continue;
          case AttachmentEffect.NegateDamageChance:
            this.playerFarming.CurrentWeaponInfo.NegateDamageChance += weaponAttachmentData.negateDamageChanceIncrement;
            continue;
          case AttachmentEffect.Poison:
            this.playerFarming.CurrentWeaponInfo.PoisonChance += weaponAttachmentData.poisonChance;
            continue;
          case AttachmentEffect.FervourOnHit:
            this.playerFarming.CurrentWeaponInfo.FervourOnHitChance += weaponAttachmentData.fervourOnHitChance;
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
      num += PlayerWeapon.GetDamage(combo.Damage, WeaponLevel, this.playerFarming);
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

  public void CancelHeavyAttackFromDamage()
  {
    this.StopAllCoroutines();
    this.attackRoutine = (Coroutine) null;
    AudioManager.Instance.StopLoop(this.loopedSound);
    this.playerFarming.HideWeaponChargeBars();
    this.ShowHeavyAim = false;
    this.CanChangeDirection = false;
    this.DoingHeavyAttack = false;
    this.HoldingForHeavyAttack = false;
  }

  public IEnumerator DoHammerHeavyAttack()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.loopedSound = AudioManager.Instance.CreateLoop("event:/weapon/hammer_heavy/hammer_unsheath", playerWeapon.gameObject, true);
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer))
    {
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-hammer-charge2", false);
      playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, "attack-heavy-hammer-wait2", true, 0.0f);
    }
    else
    {
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-hammer-charge", false);
      playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, "attack-heavy-hammer-wait", true, 0.0f);
    }
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < 0.5)
    {
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
        yield break;
      }
      playerWeapon.playerController.speed = 0.0f;
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.playerFarming.HideWeaponChargeBars();
        playerWeapon.StopAllCoroutines();
        yield break;
      }
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/weapon/hammer_heavy/hammer_charged", playerWeapon.gameObject);
    playerWeapon.playerFarming.ShowWeaponChargeBars();
    TrackEntry Track = playerWeapon.skeletonAnimation.AnimationState.SetAnimation(1, "attack-charge-walk", true);
    playerWeapon.health.OnHit += new Health.HitAction(playerWeapon.OnHitClearHeavyAttackTrack);
    while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
    {
      while (MonoSingleton<UIManager>.Instance.MenusBlocked)
        yield return (object) null;
      Track.TimeScale = Mathf.Clamp01(playerWeapon.playerController.speed / playerWeapon.playerController.RunSpeed) * 2f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.playerController.forceDir = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      playerWeapon.playerFarming.SetWeaponAimingRecticuleScaleAndRotation(0, Vector3.one, new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
        playerWeapon.health.OnHit -= new Health.HitAction(playerWeapon.OnHitClearHeavyAttackTrack);
        playerWeapon.playerFarming.HideWeaponChargeBars();
        playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
        playerWeapon.ShowHeavyAim = false;
        playerWeapon.CanChangeDirection = false;
        playerWeapon.DoingHeavyAttack = false;
        playerWeapon.HoldingForHeavyAttack = false;
        playerWeapon.StopAllCoroutines();
        yield break;
      }
      playerWeapon.playerFarming.simpleSpineAnimator.FlashMeWhite();
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
    playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
    playerWeapon.health.OnHit -= new Health.HitAction(playerWeapon.OnHitClearHeavyAttackTrack);
    playerWeapon.playerFarming.HideWeaponChargeBars();
    playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
    playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    CameraManager.shakeCamera(0.5f, playerWeapon.state.facingAngle);
    AudioManager.Instance.PlayOneShot("event:/weapon/hammer_heavy/hammer_release_swing", playerWeapon.gameObject);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer))
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-hammer2", false);
    else
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-hammer", false);
    playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost, false);
    while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
    {
      if (playerWeapon.playerFarming.state.CURRENT_STATE != StateMachine.State.Attacking)
      {
        playerWeapon.DoingHeavyAttack = false;
        playerWeapon.HoldingForHeavyAttack = false;
        yield break;
      }
      yield return (object) null;
    }
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
  }

  public IEnumerator DoAxeHeavyAttack()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.loopedSound = AudioManager.Instance.CreateLoop("event:/weapon/hammer_heavy/hammer_unsheath", playerWeapon.gameObject, true);
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Axe))
    {
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-axe-start2", false);
      playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, "attack-heavy-axe-holding2", true, 0.0f);
    }
    else
    {
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-axe-start", false);
      playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, "attack-heavy-axe-holding", true, 0.0f);
    }
    playerWeapon.playerFarming.ShowHeavyAttackProjectileChargeBars();
    float ChargeBarScale = 0.0f;
    while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
    {
      while (MonoSingleton<UIManager>.Instance.MenusBlocked)
        yield return (object) null;
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
        yield break;
      }
      playerWeapon.playerController.speed = 0.0f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      float z = Mathf.Min(ChargeBarScale += Time.deltaTime * 2f, 1f);
      playerWeapon.playerFarming.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(1f, 1f, z), new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
        playerWeapon.playerFarming.HideHeavyChargeBars();
        playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
        playerWeapon.ShowHeavyAim = false;
        playerWeapon.CanChangeDirection = false;
        playerWeapon.StopAllCoroutines();
        yield break;
      }
      playerWeapon.playerFarming.simpleSpineAnimator.FlashMeWhite();
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
    playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
    playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.playerFarming.HideHeavyChargeBars();
    playerWeapon.CanChangeDirection = false;
    playerWeapon.playerController.unitObject.DoKnockBack((float) (((double) playerWeapon.state.facingAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 0.3f, 0.3f);
    playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    CameraManager.shakeCamera(0.5f, playerWeapon.state.facingAngle);
    AudioManager.Instance.PlayOneShot("event:/weapon/axe_heavy/thorw_axe_release", playerWeapon.gameObject);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer))
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-axe-throw2", false);
    else
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-axe-throw", false);
    while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
      yield return (object) null;
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
  }

  public IEnumerator DoBlunderHeavyAttack()
  {
    PlayerWeapon playerWeapon = this;
    AudioManager.Instance.StopLoop(playerWeapon.chargingLoopSound);
    playerWeapon.chargingLoopSound = AudioManager.Instance.CreateLoop("event:/weapon/blunderbuss_heavy_load", playerWeapon.gameObject, true);
    playerWeapon.health.OnHit += new Health.HitAction(playerWeapon.EndBlunderAimingPhaseOnHit);
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    playerWeapon.playerFarming.ShowHeavyAttackProjectileChargeBars();
    float ChargeBarScale = 1f;
    bool showAnimationCharge = true;
    bool skipFirstBlunderAnimationChange = false;
    string lastBlunderAnimationString = "";
    float progress = 0.0f;
    bool sfxPlayed = false;
    while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
    {
      while (MonoSingleton<UIManager>.Instance.MenusBlocked)
        yield return (object) null;
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
        AudioManager.Instance.StopLoop(playerWeapon.HeavyAttackSound);
      }
      if (playerWeapon.PreventAimingNoiseLoop())
        yield break;
      progress += Time.deltaTime;
      if ((double) progress > 0.75)
      {
        if (!sfxPlayed)
        {
          AudioManager.Instance.StopLoop(playerWeapon.chargingLoopSound);
          AudioManager.Instance.PlayOneShot("event:/weapon/blunderbuss_heavy_load_full", playerWeapon.gameObject);
          sfxPlayed = true;
        }
      }
      else
      {
        int num = (int) playerWeapon.chargingLoopSound.setParameterByName("charge", progress / 0.75f);
      }
      GameManager.SetTimeScale(0.4f);
      playerWeapon.playerController.speed = 0.0f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      playerWeapon.playerController.forceDir = 1f;
      playerWeapon.playerController.xDir = 1f;
      if (showAnimationCharge)
      {
        string blunderbussAnimationAngle1 = playerWeapon.GetBlunderbussAnimationAngle(false, true, false);
        if (lastBlunderAnimationString != blunderbussAnimationAngle1)
        {
          playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, blunderbussAnimationAngle1, false);
          string blunderbussAnimationAngle2 = playerWeapon.GetBlunderbussAnimationAngle(true, false, false);
          playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, blunderbussAnimationAngle2, true, 0.0f);
          showAnimationCharge = false;
          lastBlunderAnimationString = blunderbussAnimationAngle1;
          skipFirstBlunderAnimationChange = true;
        }
      }
      else
      {
        string blunderbussAnimationAngle = playerWeapon.GetBlunderbussAnimationAngle(true, false, false);
        if (lastBlunderAnimationString != blunderbussAnimationAngle)
        {
          if (!skipFirstBlunderAnimationChange)
            playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, blunderbussAnimationAngle, true);
          lastBlunderAnimationString = blunderbussAnimationAngle;
          skipFirstBlunderAnimationChange = false;
        }
      }
      playerWeapon.xHeavyAimingScale = Mathf.Min(ChargeBarScale += Time.deltaTime * 2f, 2.5f);
      playerWeapon.playerFarming.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(playerWeapon.xHeavyAimingScale, 1f, 1f / playerWeapon.xHeavyAimingScale), new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
      playerWeapon.playerFarming.simpleSpineAnimator.FlashMeWhite();
      yield return (object) null;
    }
    GameManager.SetTimeScale(1f);
    AudioManager.Instance.StopLoop(playerWeapon.chargingLoopSound);
    playerWeapon.health.OnHit -= new Health.HitAction(playerWeapon.EndBlunderAimingPhaseOnHit);
    playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
    playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.playerFarming.HideHeavyChargeBars();
    playerWeapon.CanChangeDirection = false;
    playerWeapon.playerController.unitObject.DoKnockBack((float) (((double) playerWeapon.state.facingAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 0.3f, 0.3f);
    playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    CameraManager.shakeCamera(0.5f, playerWeapon.state.facingAngle);
    playerWeapon.ShowHeavyAim = false;
    Vector3 scale = new Vector3(playerWeapon.xHeavyAimingScale, 1f, 1f / playerWeapon.xHeavyAimingScale);
    playerWeapon.DoBlunderbussAttack(PlayerWeapon.HeavyBlunderbussDamage, scale, Health.AttackTypes.Heavy, true);
    playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost, false);
    GameManager.SetTimeScale(1f);
    yield return (object) new WaitForSeconds(0.55f);
    while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
      yield return (object) null;
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
  }

  public IEnumerator DoChainHeavyAttack()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    playerWeapon.playerFarming.ShowHeavyAttackProjectileChargeBars();
    float ChargeBarScale = 0.0f;
    PlayerWeapon.WeaponCombos currentComboData = playerWeapon.playerFarming.CurrentWeaponInfo.WeaponData.HeavyAttackCombo;
    playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, currentComboData.Animation, false);
    playerWeapon.loopedSound = AudioManager.Instance.CreateLoop(playerWeapon.chainHeavyAttackAimLoopSFX, playerWeapon.gameObject, true);
    while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
    {
      while (MonoSingleton<UIManager>.Instance.MenusBlocked)
        yield return (object) null;
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
        yield break;
      }
      playerWeapon.playerController.speed = 0.0f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      float z = Mathf.Min(ChargeBarScale += Time.deltaTime * 2f, 1f);
      playerWeapon.playerFarming.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(1f, 1f, z), new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
        playerWeapon.playerFarming.HideHeavyChargeBars();
        playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
        playerWeapon.ShowHeavyAim = false;
        playerWeapon.CanChangeDirection = false;
        playerWeapon.StopAllCoroutines();
        yield break;
      }
      playerWeapon.playerFarming.simpleSpineAnimator.FlashMeWhite();
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
    playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost, false);
    playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
    playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.playerFarming.HideHeavyChargeBars();
    playerWeapon.CanChangeDirection = false;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    CameraManager.shakeCamera(0.5f, playerWeapon.state.facingAngle);
    AudioManager.Instance.PlayOneShot(playerWeapon.chainHeavyAttackStartSFX, playerWeapon.transform.position);
    playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, currentComboData.Animation, false);
    playerWeapon.chainHook.gameObject.SetActive(false);
    playerWeapon.chainHook1.gameObject.SetActive(false);
    playerWeapon.chainHook.SetHeavyAttackVisuals();
    playerWeapon.chainHook1.SetHeavyAttackVisuals();
    Health.AttackFlags attackFlags = Health.AttackFlags.Penetration;
    float chainHookDamage = PlayerWeapon.GetDamage(PlayerWeapon.HeavyChainDamage, playerWeapon.playerFarming.currentWeaponLevel, playerWeapon.playerFarming);
    float critChance = playerWeapon.GetCritChance();
    float num1 = playerWeapon.playerFarming.CurrentWeaponInfo.AttackRateMultiplier + TrinketManager.GetAttackRateMultiplier(playerWeapon.playerFarming);
    float hookRadius = currentComboData.RangeRadius * playerWeapon.playerFarming.CurrentWeaponInfo.RangeMultiplier;
    if (playerWeapon.CriticalHitCharged || (double) UnityEngine.Random.Range(0.0f, 1f) < (double) critChance)
    {
      chainHookDamage *= 3f;
      attackFlags |= Health.AttackFlags.Crit;
      PlayerWeapon.CriticalHitTimer = 0.0f;
    }
    if (TrinketManager.HasTrinket(TarotCards.Card.Skull, playerWeapon.playerFarming))
      attackFlags |= Health.AttackFlags.Skull;
    if (TrinketManager.HasTrinket(TarotCards.Card.Spider, playerWeapon.playerFarming))
      attackFlags |= Health.AttackFlags.Poison;
    Vector3 up = new Vector3(Mathf.Cos(playerWeapon.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(playerWeapon.state.facingAngle * ((float) Math.PI / 180f)));
    Vector3 vector3 = new Vector3(-up.y, up.x);
    float offsetMultiplier = currentComboData.OffsetMultiplier;
    EllipseMovement ellipseMovement = new EllipseMovement(up, Vector3.back, vector3 * offsetMultiplier, currentComboData.EllipseRadiusY, currentComboData.EllipseRadiusX, currentComboData.StartAngle, currentComboData.AngleToMove, currentComboData.Duration / num1, currentComboData.RadiusMultiplierOverTime);
    bool isCompleted = false;
    Vector3 moveToPosition = Vector3.zero;
    playerWeapon.playerController.MakeUntouchable((float) ((double) currentComboData.Duration / (double) num1 + 0.10000000149011612), false);
    playerWeapon.chainHook.Init(ellipseMovement, chainHookDamage, hookRadius, attackFlags, false, currentComboData.ScaleMultiplierOverTime, colliderEnabledDelay: currentComboData.ColliderEnabledDelay, colliderDisabledDelay: currentComboData.ColliderDisabledDelay, onComplete: (Action<Vector3>) (hitPosition =>
    {
      Vector3 Position = hitPosition with { z = 0.0f };
      BiomeConstants.Instance.EmitHammerEffects(Position, 1f, 1.2f, 0.3f, true, 0.8f, false);
      AudioManager.Instance.PlayOneShot(this.chainGroundImpactBasicSFX, hitPosition);
      MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
      isCompleted = true;
      moveToPosition = hitPosition;
    }));
    playerWeapon.chainHook.SetHideOnComplete(false);
    playerWeapon.chainHook.SetAttackType(currentComboData.AttackType);
    yield return (object) new WaitUntil((Func<bool>) (() => isCompleted));
    yield return (object) null;
    playerWeapon.chainHook.SetCollidersActive(false);
    playerWeapon.chainHook.StopUpdatingTime();
    bool isPlayerMoving = true;
    playerWeapon.chainHook.SetHookTargetPosition(moveToPosition);
    float num2 = Vector3.Distance(playerWeapon.transform.position, moveToPosition);
    float attackduration = Mathf.Min(1f / num2, 0.2f);
    playerWeapon.playerController.MakeUntouchable(attackduration + 0.4f, false);
    Vector2 normalized = (Vector2) (moveToPosition - playerWeapon.transform.position).normalized;
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) (1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles"));
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) playerWeapon.transform.position, normalized, 30f, (int) layerMask);
    if ((bool) raycastHit2D)
    {
      float num3 = Vector3.Distance(playerWeapon.transform.position, (Vector3) raycastHit2D.point);
      if ((double) num3 < 0.10000000149011612)
        moveToPosition = (Vector3) raycastHit2D.point;
      else if ((double) num3 < (double) num2)
        moveToPosition = (Vector3) (raycastHit2D.point - normalized);
    }
    if (RoomLockController.IsPositionOutOfRoom(moveToPosition, true))
      moveToPosition = playerWeapon.transform.position;
    yield return (object) new WaitForSeconds(0.3f);
    bool hasReachSmashPrecent = false;
    playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-chain-heavy-fly", true);
    AudioManager.Instance.PlayOneShot(playerWeapon.chainHeavyAttackJumpSFX, playerWeapon.gameObject);
    playerWeapon.transform.DOMove(moveToPosition, attackduration).OnUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if ((double) ((List<Tween>) DOTween.TweensByTarget((object) this.transform))[0].ElapsedPercentage() < 0.40000000596046448 || hasReachSmashPrecent)
        return;
      hasReachSmashPrecent = true;
      this.skeletonAnimation.AnimationState.SetAnimation(0, "attack-chain-heavy-land", false);
    })).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      isPlayerMoving = false;
      this.chainHook.gameObject.SetActive(false);
      Vector3 Position = moveToPosition with { z = 0.0f };
      BiomeConstants.Instance.EmitHammerEffects(Position, 1f, 1.2f, 0.3f, true, 1.3f, false);
      AudioManager.Instance.PlayOneShot(this.chainGroundImpactLargeSFX, this.transform.position);
      MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
      foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, hookRadius * 1.5f))
      {
        Health component2 = component1.gameObject.GetComponent<Health>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) this.health && component2.team != (Health.Team) ((int) this.overrideTeam ?? (int) this.health.team))
        {
          component2.DealDamage(chainHookDamage, this.gameObject, Vector3.Lerp(this.transform.position, component2.transform.position, 0.8f), AttackType: currentComboData.AttackType, AttackFlags: attackFlags);
          CameraManager.shakeCamera(0.6f, Utils.GetAngle(this.transform.position, component2.transform.position));
        }
      }
    }));
    yield return (object) new WaitUntil((Func<bool>) (() => !isPlayerMoving));
    yield return (object) new WaitForSeconds(0.5f);
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
  }

  public void EndBlunderAimingPhaseOnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    Debug.Log((object) "<----------Ending and resetting blunderbuss on hit for re-triggering");
    AudioManager.Instance.StopLoop(this.chargingLoopSound);
    this.health.OnHit -= new Health.HitAction(this.EndBlunderAimingPhaseOnHit);
    this.DoingHeavyAttack = false;
    GameManager.SetTimeScale(1f);
    this.playerFarming.HideHeavyChargeBars();
    this.ShowHeavyAim = false;
  }

  public bool PreventAimingNoiseLoop()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.ChargingHeavyAttack)
      return false;
    this.skeletonAnimation.AnimationState.ClearTrack(1);
    this.playerFarming.HideHeavyChargeBars();
    this.playerFarming.simpleSpineAnimator.FlashWhite(false);
    this.ShowHeavyAim = false;
    this.CanChangeDirection = false;
    GameManager.SetTimeScale(1f);
    this.health.OnHit -= new Health.HitAction(this.EndBlunderAimingPhaseOnHit);
    AudioManager.Instance.StopLoop(this.chargingLoopSound);
    return true;
  }

  public IEnumerator DoShieldHeavyAttack()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.RepelProjectiles = true;
    playerWeapon.shieldRepel.shieldAmmo.FillAllAmmo();
    playerWeapon.shieldRepel.shieldAmmo.gameObject.SetActive(true);
    playerWeapon.shieldRepel.shieldAmmo.ShowShieldHPBar();
    AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
    playerWeapon.loopedSound = AudioManager.Instance.CreateLoop("event:/weapon/hammer_heavy/hammer_unsheath", playerWeapon.gameObject, true);
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-shield-defend", false);
    float shieldChargePower = 0.25f;
    string lastAimingWeaponAnimationString = "";
    while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
    {
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
        yield break;
      playerWeapon.playerController.speed = 0.0f;
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      string shieldAnimationAngle = playerWeapon.GetShieldAnimationAngle();
      if (lastAimingWeaponAnimationString != shieldAnimationAngle)
      {
        playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, shieldAnimationAngle, false);
        lastAimingWeaponAnimationString = shieldAnimationAngle;
      }
      shieldChargePower += 0.1f;
      if ((double) shieldChargePower > 2.5)
        shieldChargePower = 2.5f;
      CameraManager.shakeCamera(shieldChargePower * 0.1f, playerWeapon.state.facingAngle);
      playerWeapon.ShieldReflect(1f, playerWeapon.state.facingAngle, shieldChargePower);
      playerWeapon.playerFarming.simpleSpineAnimator.FlashMeWhite();
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
        playerWeapon.playerFarming.HideHeavyChargeBars();
        playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
        playerWeapon.ShowHeavyAim = false;
        playerWeapon.CanChangeDirection = false;
        playerWeapon.shieldRepel.gameObject.SetActive(false);
        playerWeapon.RepelProjectiles = false;
        playerWeapon.StopAllCoroutines();
        yield break;
      }
      yield return (object) null;
    }
    playerWeapon.shieldRepel.gameObject.SetActive(false);
    AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
    playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
    playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.playerFarming.HideHeavyChargeBars();
    playerWeapon.CanChangeDirection = false;
    playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    AudioManager.Instance.PlayOneShot("event:/weapon/axe_heavy/thorw_axe_release", playerWeapon.gameObject);
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.playerFarming.HideHeavyChargeBars();
    playerWeapon.CanChangeDirection = false;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(playerWeapon.swipeShieldCharge, playerWeapon.transform);
    gameObject.name = "SHIELD CHARGE DAMAGING SWIPE";
    gameObject.SetActive(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 1f);
    Swipe component = gameObject.GetComponent<Swipe>();
    component.enabled = true;
    component.Damage = PlayerWeapon.HeavyShieldDamage;
    component.Duration = 0.75f;
    component.Origin = (Health) playerWeapon.playerFarming.health;
    component.AttackFlags = Health.AttackFlags.Penetration;
    ParticleSystem shieldParticles = gameObject.GetComponent<ParticleSystem>();
    shieldParticles.Play();
    playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, playerWeapon.GetShieldAnimationAngle(), false);
    CameraManager.shakeCamera(shieldChargePower * 2f, playerWeapon.state.facingAngle);
    playerWeapon.playerController.unitObject.DoKnockBack((float) ((double) playerWeapon.state.facingAngle % 360.0 * (Math.PI / 180.0)), shieldChargePower, 0.5f);
    playerWeapon.shieldRepel.shieldAmmo.HideShieldHPBar();
    Debug.Log((object) "Returning crown");
    playerWeapon.playerFarming.CrownBone.transform.position = playerWeapon.playerFarming.transform.position;
    playerWeapon.playerFarming.CrownBone.gameObject.SetActive(false);
    float num = Mathf.Clamp(shieldChargePower * 1.5f, 1.5f, 3f);
    playerWeapon.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerWeapon.HeavyAttackFervourCost * num, false);
    yield return (object) new WaitForSeconds(0.5f);
    playerWeapon.playerFarming.CrownBone.gameObject.SetActive(true);
    System.Action onCrownReturn = playerWeapon.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
    playerWeapon.RepelProjectiles = false;
    shieldParticles.Stop();
  }

  public IEnumerator DoDaggerHeavyAttack()
  {
    PlayerWeapon playerWeapon = this;
    playerWeapon.loopedSound = AudioManager.Instance.CreateLoop("event:/weapon/hammer_heavy/hammer_unsheath", playerWeapon.gameObject, true);
    playerWeapon.state.CURRENT_STATE = StateMachine.State.ChargingHeavyAttack;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Dagger))
    {
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-dagger-charge", false);
      playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, "attack-heavy-dagger-holding", true, 0.0f);
    }
    else
    {
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-dagger-charge", false);
      playerWeapon.skeletonAnimation.AnimationState.AddAnimation(0, "attack-heavy-dagger-holding", true, 0.0f);
    }
    playerWeapon.playerFarming.ShowHeavyAttackProjectileChargeBars();
    float ChargeBarScale = 0.0f;
    while (InputManager.Gameplay.GetHeavyAttackButtonHeld(playerWeapon.playerFarming))
    {
      while (MonoSingleton<UIManager>.Instance.MenusBlocked)
        yield return (object) null;
      if (playerWeapon.state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
        yield break;
      }
      playerWeapon.playerController.speed = 0.0f;
      Debug.Log((object) "A".Colour(Color.red));
      if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)) > (double) PlayerController.MinInputForMovement)
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(playerWeapon.playerFarming), InputManager.Gameplay.GetVerticalAxis(playerWeapon.playerFarming)));
      if (playerWeapon.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerWeapon.transform.position);
        playerWeapon.state.facingAngle = playerWeapon.StoreFacing = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerWeapon.playerFarming));
      }
      float z = Mathf.Min(ChargeBarScale += Time.deltaTime * 2f, 1f);
      playerWeapon.playerFarming.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(1f, 1f, z), new Vector3(0.0f, 0.0f, playerWeapon.state.facingAngle));
      if (playerWeapon.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack)
      {
        playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
        playerWeapon.playerFarming.HideHeavyChargeBars();
        playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
        playerWeapon.ShowHeavyAim = false;
        playerWeapon.CanChangeDirection = false;
        playerWeapon.DoingHeavyAttack = false;
        playerWeapon.StopAllCoroutines();
        yield break;
      }
      playerWeapon.playerFarming.simpleSpineAnimator.FlashMeWhite();
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(playerWeapon.loopedSound);
    playerWeapon.skeletonAnimation.AnimationState.ClearTrack(1);
    playerWeapon.playerFarming.simpleSpineAnimator.FlashWhite(false);
    playerWeapon.ShowHeavyAim = false;
    playerWeapon.playerFarming.HideHeavyChargeBars();
    playerWeapon.CanChangeDirection = false;
    playerWeapon.playerController.unitObject.DoKnockBack((float) (((double) playerWeapon.state.facingAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 0.3f, 0.3f);
    playerWeapon.CurrentAttackState = PlayerWeapon.AttackState.Begin;
    playerWeapon.state.CURRENT_STATE = StateMachine.State.Attacking;
    CameraManager.shakeCamera(0.5f, playerWeapon.state.facingAngle);
    AudioManager.Instance.PlayOneShot("event:/weapon/hammer_heavy/hammer_release_swing", playerWeapon.gameObject);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Hammer))
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-dagger", false);
    else
      playerWeapon.skeletonAnimation.AnimationState.SetAnimation(0, "attack-heavy-dagger", false);
    while (playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
      yield return (object) null;
    if (playerWeapon.state.CURRENT_STATE != StateMachine.State.InActive)
      playerWeapon.state.CURRENT_STATE = StateMachine.State.Idle;
    playerWeapon.DoingHeavyAttack = false;
  }

  public void OnHitClearHeavyAttackTrack(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.health.OnHit -= new Health.HitAction(this.OnHitClearHeavyAttackTrack);
    this.skeletonAnimation.AnimationState.ClearTrack(1);
  }

  public bool IsLegendaryWeapon()
  {
    return EquipmentManager.IsLegendaryWeapon(this.playerFarming.currentWeapon);
  }

  public void SetSpecial(int Num)
  {
    this.CurrentSpecial = this.Specials[Num];
    DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET = this.CurrentSpecial.Target;
  }

  public void OnDrawGizmosSelected()
  {
    if (!((UnityEngine.Object) this.state != (UnityEngine.Object) null))
      return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.transform.position + new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f))) * 0.2f, 0.5f);
  }

  [CompilerGenerated]
  public void \u003CHandleAnimationStateEvent\u003Eb__84_0(ThrownAxe axe) => this.thrownAxe = axe;

  [CompilerGenerated]
  public void \u003CChainAttack\u003Eb__87_0(Vector3 hitPosition)
  {
    this.chainHook.SetCollidersActive(true);
    Vector3 Position = ((hitPosition + this.chainHook1.GetHookPosition()) * 0.5f) with
    {
      z = 0.0f
    };
    BiomeConstants.Instance.EmitHammerEffects(Position, playSFX: false);
    AudioManager.Instance.PlayOneShot(this.chainGroundImpactBasicSFX, this.transform.position);
    MMVibrate.Rumble(0.166666672f, 0.05f, 0.5f, (MonoBehaviour) this, this.playerFarming);
    if ((bool) (UnityEngine.Object) this.playerFarming.CurrentWeaponInfo.WeaponData.GetAttachment(AttachmentEffect.Projectile))
    {
      AudioManager.Instance.PlayOneShot(this.chainLegendaryProjectileLaunchSFX, this.transform.position);
      for (int index = 0; index < UnityEngine.Random.Range(10, 16 /*0x10*/); ++index)
      {
        GrenadeBullet component = ObjectPool.Spawn<GrenadeBullet>(this.yngyaChunkGrandeBullet, this.chainHook.Hook.transform.position + Vector3.back, Quaternion.identity).GetComponent<GrenadeBullet>();
        component.SetOwner(this.gameObject);
        component.Damage = PlayerWeapon.GetDamage(2f, this.playerFarming.currentWeaponLevel, this.playerFarming);
        component.Play(-0.5f, (float) UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(1.5f, 2.5f), UnityEngine.Random.Range(-9f, -7f), Health.Team.PlayerTeam, true);
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
      return;
    this.chainHook.Hide(true);
  }

  [CompilerGenerated]
  public void \u003CChainAttack\u003Eb__87_1()
  {
    AudioManager.Instance.PlayOneShot(this.chainPullbackSFX, this.transform.position);
  }

  [CompilerGenerated]
  public void \u003CChainAttack\u003Eb__87_2(Vector3 hitPosition)
  {
    this.chainHook1.SetCollidersActive(true);
    if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
      return;
    this.chainHook1.Hide(true);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__105_0() => this.damageNegation.gameObject.SetActive(false);

  public delegate void DoSpecialAction();

  public enum AttackSwipeDirections
  {
    Down,
    DownRight,
    Up,
  }

  public delegate void WeaponEvent(EquipmentType Weapon, int Level, PlayerFarming playerFarming);

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
    [CompilerGenerated]
    public float \u003CWeaponDamageMultiplier\u003Ek__BackingField = 1f;
    [CompilerGenerated]
    public float \u003CCriticalChance\u003Ek__BackingField;
    [CompilerGenerated]
    public float \u003CRangeMultiplier\u003Ek__BackingField = 1f;
    [CompilerGenerated]
    public float \u003CAttackRateMultiplier\u003Ek__BackingField = 1f;
    [CompilerGenerated]
    public float \u003CMovementSpeedMultiplier\u003Ek__BackingField = 1f;
    [CompilerGenerated]
    public float \u003CXPDropMultiplier\u003Ek__BackingField = 1f;
    [CompilerGenerated]
    public float \u003CNegateDamageChance\u003Ek__BackingField;
    [CompilerGenerated]
    public float \u003CHealChance\u003Ek__BackingField;
    [CompilerGenerated]
    public float \u003CHealAmount\u003Ek__BackingField = 1f;
    [CompilerGenerated]
    public float \u003CPoisonChance\u003Ek__BackingField;
    [CompilerGenerated]
    public float \u003CFervourOnHitChance\u003Ek__BackingField;

    public float WeaponDamageMultiplier
    {
      get => this.\u003CWeaponDamageMultiplier\u003Ek__BackingField;
      set => this.\u003CWeaponDamageMultiplier\u003Ek__BackingField = value;
    }

    public float CriticalChance
    {
      get => this.\u003CCriticalChance\u003Ek__BackingField;
      set => this.\u003CCriticalChance\u003Ek__BackingField = value;
    }

    public float RangeMultiplier
    {
      get => this.\u003CRangeMultiplier\u003Ek__BackingField;
      set => this.\u003CRangeMultiplier\u003Ek__BackingField = value;
    }

    public float AttackRateMultiplier
    {
      get => this.\u003CAttackRateMultiplier\u003Ek__BackingField;
      set => this.\u003CAttackRateMultiplier\u003Ek__BackingField = value;
    }

    public float MovementSpeedMultiplier
    {
      get => this.\u003CMovementSpeedMultiplier\u003Ek__BackingField;
      set => this.\u003CMovementSpeedMultiplier\u003Ek__BackingField = value;
    }

    public float XPDropMultiplier
    {
      get => this.\u003CXPDropMultiplier\u003Ek__BackingField;
      set => this.\u003CXPDropMultiplier\u003Ek__BackingField = value;
    }

    public float NegateDamageChance
    {
      get => this.\u003CNegateDamageChance\u003Ek__BackingField;
      set => this.\u003CNegateDamageChance\u003Ek__BackingField = value;
    }

    public float HealChance
    {
      get => this.\u003CHealChance\u003Ek__BackingField;
      set => this.\u003CHealChance\u003Ek__BackingField = value;
    }

    public float HealAmount
    {
      get => this.\u003CHealAmount\u003Ek__BackingField;
      set => this.\u003CHealAmount\u003Ek__BackingField = value;
    }

    public float PoisonChance
    {
      get => this.\u003CPoisonChance\u003Ek__BackingField;
      set => this.\u003CPoisonChance\u003Ek__BackingField = value;
    }

    public float FervourOnHitChance
    {
      get => this.\u003CFervourOnHitChance\u003Ek__BackingField;
      set => this.\u003CFervourOnHitChance\u003Ek__BackingField = value;
    }

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
    public float RecoilDuration;
    public float RecoilPowerMultiplier;
    public float HitKnockback;
    public Health.AttackTypes AttackType;
    [Header("Blunderbuss Only")]
    public int BlunderbussBulletCount = 3;
    public float BlunderbussReloadSpeed = 0.8f;
    [Header("Chain Hook Only")]
    public AnimationCurve RadiusMultiplierOverTime;
    public AnimationCurve ScaleMultiplierOverTime;
    public float StartAngle;
    public float AngleToMove;
    public float Duration;
    public float EllipseRadiusX;
    public float EllipseRadiusY;
    public float ColliderEnabledDelay;
    public float ColliderDisabledDelay = float.MaxValue;
    public float OffsetMultiplier = 0.6f;
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
