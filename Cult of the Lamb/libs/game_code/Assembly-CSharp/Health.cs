// Decompiled with JetBrains decompiler
// Type: Health
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using MMRoomGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

#nullable disable
public class Health : BaseMonoBehaviour
{
  public const float chanceToSpreadBurn = 0.6f;
  [HideInInspector]
  public StateMachine state;
  public bool Unaware;
  [HideInInspector]
  public bool InStealthCover;
  public bool SlowMoOnkill;
  public bool HasShield;
  public bool IsDeflecting;
  public bool WasJustParried;
  public bool IgnoreProjectiles;
  public bool CanIncreaseDamageMultiplier = true;
  [HideInInspector]
  public MinionProtector protector;
  public bool Dormant;
  public Health.CheatMode GodMode;
  public UnityEvent OnHitCallback;
  public UnityEvent OnDieCallback;
  public Health.Team team;
  public static List<Health> allUnits = new List<Health>();
  public static List<Health> neutralTeam = new List<Health>();
  public static List<Health> playerTeam = new List<Health>();
  public static List<Health> team2 = new List<Health>();
  public static List<Health> dormant = new List<Health>();
  public static List<Health> dangerousAnimals = new List<Health>();
  public static List<Health> killAll = new List<Health>();
  [SerializeField]
  public float _totalHP = 1f;
  [SerializeField]
  public float _HP;
  [SerializeField]
  public float _BlueHearts;
  [SerializeField]
  public float _BlackHearts;
  [SerializeField]
  public float _FireHearts;
  [SerializeField]
  public float _IceHearts;
  [SerializeField]
  [FormerlySerializedAs("_SpiritHearts")]
  public float _TotalSpiritHearts;
  [SerializeField]
  public float _SpiritHearts;
  public bool projectileTrapInitialized;
  public IProjectileTrap _projectileTrap;
  public bool ArmoredFront;
  public bool invincible;
  public bool isPlayer;
  public bool isPlayerAlly;
  public List<GameObject> attackers = new List<GameObject>();
  public List<GameObject> followers = new List<GameObject>();
  public Task_Combat[] AttackPositions = new Task_Combat[4];
  public bool InanimateObject;
  public bool InanimateObjectEffect = true;
  public float ArrowAttackVulnerability = 1f;
  public float MeleeAttackVulnerability = 1f;
  public float DamageModifier = 1f;
  public bool untouchable;
  public bool DestroyOnDeath = true;
  public bool DontCombo;
  public int hitsWithShield;
  public bool CanBePoisoned = true;
  public bool CanBeIced = true;
  public bool CanBeCharmed = true;
  public bool CanBeElectrified = true;
  public bool CanBeBurned = true;
  public bool CanBeTurnedIntoCritter = true;
  public bool CanBeKilledByTarot = true;
  [CompilerGenerated]
  public bool \u003CIsImmuneToAllStasis\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsHidden\u003Ek__BackingField;
  public bool ImmuneToDiseasedHearts;
  public bool ImmuneToTraps;
  public bool ImmuneToPlayer;
  public bool ImpactOnHit = true;
  public Color ImpactOnHitColor = Color.red;
  public float ImpactOnHitScale = 1f;
  public bool BloodOnHit;
  public bool BloodOnDie;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  public int BloodOnDieParticlesAmount = 5;
  public float BloodOnDieParticlesMultiplyer = 1f;
  public bool EmitGroundSmashDecal;
  public bool ScreenshakeOnHit = true;
  public bool ScreenshakeOnDie = true;
  public float ScreenShakeOnDieIntensity = 2f;
  public bool spawnParticles;
  public BiomeConstants.TypeOfParticle typeOfParticle;
  public bool SmokeOnDie;
  public string EffectBurningStartSFX = "event:/dlc/combat/effect_fire_burning_start";
  public string EffectBurningStopSFX = "event:/dlc/combat/effect_fire_burning_stop";
  public string EffectBurningDamageSFX = "event:/dlc/combat/fire_damage_tick";
  public string EffectBurningLoopSFX = "event:/dlc/player/effect_fire_burning_loop";
  public string EffectPlayerBurnReminderSFX = "event:/dlc/player/effect_fire_burning_damage_tick";
  public string EffectFreezeStartSFX = "event:/dlc/combat/effect_ice_freeze_start";
  public string EffectFreezeStopSFX = "event:/dlc/combat/effect_ice_freeze_stop";
  public string EffectFreezeLoopSFX = "event:/dlc/combat/effect_ice_freeze_loop";
  public EventInstance FreezeLoopInstance;
  public bool OnHitBlockAttacker;
  public GameObject AttackerToBlock;
  [HideInInspector]
  public Health.DealDamageEvent damageEventQueue;
  public static float DealDamageForgivenessWindow = 0.04f;
  public float autoAimAttractionFactor = 1f;
  [CompilerGenerated]
  public bool \u003CCanBeFreezedInCustscene\u003Ek__BackingField = true;
  public Health.IMPACT_SFX ImpactSoundToPlay;
  public Health.DEATH_SFX DeathSoundToPlay;
  public ShowHPBar showHpBar;
  public CircleCollider2D circleCollider2D;
  public PlayerFarming playerFarming;
  public bool IgnoreLocationHPBuff;
  public bool BlackSoulOnHit;
  public bool CanSpawnTarotSinOnDie = true;
  public Vector3 Velocity;
  [CompilerGenerated]
  public float \u003CpoisonTickDuration\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CenemyPoisonDamage\u003Ek__BackingField = 1f;
  public int poisonCounter = -1;
  [CompilerGenerated]
  public float \u003CpoisonTimer\u003Ek__BackingField;
  public float enemyPoisonDuration = 5f;
  public float enemyPoisonTimestamp = -1f;
  public GameObject poisonAttacker;
  [CompilerGenerated]
  public ParticleSystem \u003CpoisonedParticles\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CPoisonImmune\u003Ek__BackingField;
  public EnemyStasisTicker enemyPoisonTicker;
  public EventInstance PoisonLoopInstance;
  public bool createdPoisonLoop;
  public int charmCounter = -1;
  [CompilerGenerated]
  public float \u003CcharmTimer\u003Ek__BackingField;
  public float enemyCharmDuration = 5f;
  public float enemyCharmTimestamp = -1f;
  public float enemyLastCharmTimestamp = -1f;
  [CompilerGenerated]
  public ParticleSystem \u003CcharmParticles\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCharmImmune\u003Ek__BackingField;
  public EnemyStasisTicker enemyCharmTicker;
  public EventInstance CharmLoopInstance;
  public bool createdCharmLoop;
  public int iceCounter = -1;
  [CompilerGenerated]
  public float \u003CiceTimer\u003Ek__BackingField;
  public float enemyIceDuration = 3f;
  public float enemyIceTimestamp = -1f;
  [CompilerGenerated]
  public ParticleSystem \u003CiceParticles\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIceImmune\u003Ek__BackingField;
  public EnemyStasisTicker enemyIceTicker;
  public EventInstance IceLoopInstance;
  public bool createdIceLoop;
  public bool timeFrozen;
  public static bool isGlobalTimeFreeze = false;
  public SkeletonAnimation[] cachedSpineComponentsForFreeze;
  public int electrifiedCounter = -1;
  [CompilerGenerated]
  public float \u003CelectrifiedTimer\u003Ek__BackingField;
  public float enemyElectrifiedDuration = 1.5f;
  public float enemyElectrifiedTimestamp = -1f;
  public float enemyLastElectrifiedTimestamp = -1f;
  [CompilerGenerated]
  public ParticleSystem \u003CelectrifiedParticles\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CElectrifiedImmune\u003Ek__BackingField;
  public EnemyStasisTicker enemyElectrifiedTicker;
  public EventInstance ElectrifiedLoopInstance;
  public bool createdElectrifiedLoop;
  [CompilerGenerated]
  public float \u003CelectrifiedTickDuration\u003Ek__BackingField = 0.3f;
  [CompilerGenerated]
  public float \u003CenemyElectrifiedDamage\u003Ek__BackingField = 0.3f;
  public GameObject electrifiedAttacker;
  public PlayerWeapon playerWeapon;
  [CompilerGenerated]
  public float \u003CburnTickDuration\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CenemyBurnDamage\u003Ek__BackingField = 5f;
  public int burnCounter = -1;
  [CompilerGenerated]
  public float \u003CburnTimer\u003Ek__BackingField;
  public float enemyBurnTimestamp = -1f;
  public GameObject burnAttacker;
  [SerializeField]
  public ParticleSystem burnParticles;
  [CompilerGenerated]
  public ParticleSystem \u003CBurnParticles\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CBurnImmune\u003Ek__BackingField;
  public EnemyStasisTicker enemyBurnTicker;
  public EventInstance BurnLoopInstance;
  public bool createdBurnLoop;
  public float playerBurnStartSFXTimer;

  public event Health.DieAction OnDie;

  public event Health.DieAction OnDieEarly;

  public static event Health.DieAllAction OnDieAny;

  public event Health.HitAction OnHit;

  public event Health.HitAction OnHitEarly;

  public event Health.HitAction OnPoisonedHit;

  public event Health.HitAction OnPenetrationHit;

  public event Health.HitAction OnBurnHit;

  public event Health.HitAction OnHitForceBossHUD;

  public event System.Action OnDamageNegated;

  public event Health.HealthEvent OnDamaged;

  public event Health.HealEvent OnInitHP;

  public virtual float CurrentHP
  {
    get
    {
      return this.HP + this.BlueHearts + this.BlackHearts + this.SpiritHearts + this.FireHearts + this.IceHearts;
    }
  }

  public virtual float totalHP
  {
    get => this._totalHP;
    set => this._totalHP = value;
  }

  public virtual float HP
  {
    get => this._HP;
    set => this._HP = value;
  }

  public virtual float BlueHearts
  {
    get => this._BlueHearts;
    set => this._BlueHearts = value;
  }

  public virtual float BlackHearts
  {
    get => this._BlackHearts;
    set => this._BlackHearts = value;
  }

  public virtual float FireHearts
  {
    get => this._FireHearts;
    set => this._FireHearts = value;
  }

  public virtual float IceHearts
  {
    get => this._IceHearts;
    set => this._IceHearts = value;
  }

  public virtual float TotalSpiritHearts
  {
    get => this._TotalSpiritHearts;
    set => this._TotalSpiritHearts = value;
  }

  public virtual float SpiritHearts
  {
    get => this._SpiritHearts;
    set => this._SpiritHearts = value;
  }

  public IProjectileTrap ProjectileTrap
  {
    get
    {
      if (!this.projectileTrapInitialized)
      {
        this._projectileTrap = this.GetComponent<IProjectileTrap>();
        this.projectileTrapInitialized = true;
      }
      return this._projectileTrap;
    }
  }

  public bool IsImmuneToAllStasis
  {
    get => this.\u003CIsImmuneToAllStasis\u003Ek__BackingField;
    set => this.\u003CIsImmuneToAllStasis\u003Ek__BackingField = value;
  }

  public bool IsHidden
  {
    get => this.\u003CIsHidden\u003Ek__BackingField;
    set => this.\u003CIsHidden\u003Ek__BackingField = value;
  }

  public bool CanBeFreezedInCustscene
  {
    set => this.\u003CCanBeFreezedInCustscene\u003Ek__BackingField = value;
    get => this.\u003CCanBeFreezedInCustscene\u003Ek__BackingField;
  }

  public event Health.StasisEvent OnStasisCleared;

  public event Health.StasisEvent OnPoisoned;

  public event Health.StasisEvent OnIced;

  public event Health.StasisEvent OnFreezeTime;

  public event Health.StasisEvent OnFreezeTimeCleared;

  public event Health.StasisEvent OnAddCharm;

  public event Health.StasisEvent OnCharmed;

  public event Health.StasisEvent OnElectrified;

  public event Health.StasisEvent OnBurned;

  public event Health.StasisEvent OnBurnedFailed;

  public CircleCollider2D CircleCollider2D
  {
    get
    {
      if ((UnityEngine.Object) this.circleCollider2D == (UnityEngine.Object) null)
        this.circleCollider2D = this.GetComponent<CircleCollider2D>();
      return this.circleCollider2D;
    }
  }

  public void Awake()
  {
    if ((bool) (UnityEngine.Object) this.playerFarming && (double) Time.time < (double) this.playerFarming.playerWasHidden + 1.0)
      return;
    if (!this.isPlayer)
      this.InitHP();
    this.ClearPoison();
    this.ClearIce();
    this.ClearCharm();
    this.ClearElectrified();
    if (!this.isPlayer)
      return;
    BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(this.ClearBurnParticles);
  }

  public virtual void OnEnable()
  {
    this.SetTeam(this.team);
    if (!Health.allUnits.Contains(this))
      Health.allUnits.Add(this);
    this.state = this.GetComponent<StateMachine>();
    this.showHpBar = this.GetComponent<ShowHPBar>();
    if ((UnityEngine.Object) this.AttackerToBlock == (UnityEngine.Object) null)
      this.AttackerToBlock = this.gameObject;
    if (this.isPlayer)
      this.playerWeapon = this.GetComponent<PlayerWeapon>();
    this.ClearPoison();
    this.ClearIce();
    this.ClearCharm();
    this.ClearElectrified();
  }

  public void SetTeam(Health.Team teamTarget)
  {
    Health.neutralTeam.Remove(this);
    Health.playerTeam.Remove(this);
    Health.team2.Remove(this);
    Health.dangerousAnimals.Remove(this);
    Health.killAll.Remove(this);
    Health.dormant.Remove(this);
    this.team = teamTarget;
    switch (this.team)
    {
      case Health.Team.Neutral:
        if (Health.neutralTeam.Contains(this))
          break;
        Health.neutralTeam.Add(this);
        break;
      case Health.Team.Team2:
        if (!Health.team2.Contains(this))
          Health.team2.Add(this);
        if (!PlayerRelic.TimeFrozen)
          break;
        PlayerFarming.Instance.playerRelic.AddFrozenEnemy(this);
        break;
      case Health.Team.DangerousAnimals:
        Health.dangerousAnimals.Add(this);
        break;
      case Health.Team.KillAll:
        Health.killAll.Add(this);
        break;
      default:
        Health.playerTeam.Add(this);
        this.playerFarming = this.GetComponent<PlayerFarming>();
        break;
    }
  }

  public virtual void InitHP()
  {
    if (!this.isPlayer)
    {
      this.totalHP /= DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.HalfHealth, 2f, 1f);
      this.totalHP *= DifficultyManager.GetEnemyHealthMultiplier();
      this.totalHP *= DataManager.Instance.EnemyHealthMultiplier;
      if (this.team == Health.Team.Team2 && !this.IgnoreLocationHPBuff)
      {
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_1:
            this.totalHP += 0.0f;
            if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
            {
              this.totalHP += 4f;
              break;
            }
            break;
          case FollowerLocation.Dungeon1_2:
            this.totalHP += 2f;
            if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2))
            {
              this.totalHP += 4f;
              break;
            }
            break;
          case FollowerLocation.Dungeon1_3:
            this.totalHP += 4f;
            if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
            {
              this.totalHP += 4f;
              break;
            }
            break;
          case FollowerLocation.Dungeon1_4:
            this.totalHP += 6f;
            if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
            {
              this.totalHP += 4f;
              break;
            }
            break;
          case FollowerLocation.Dungeon1_5:
            this.totalHP += 3f;
            if (DataManager.Instance.BeatenWolf)
            {
              this.totalHP += 5f;
              break;
            }
            break;
          case FollowerLocation.Dungeon1_6:
            this.totalHP += 3f;
            if (DataManager.Instance.BeatenYngya)
            {
              this.totalHP += 5f;
              break;
            }
            break;
        }
        if (GameManager.DungeonEndlessLevel > 0 && GameManager.SandboxDungeonEnabled)
        {
          Debug.Log((object) ("GameManager.DungeonEndlessLevel: " + GameManager.DungeonEndlessLevel.ToString()));
          MiniBossController componentInParent = this.GetComponentInParent<MiniBossController>();
          if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.EnemiesToTrack.Count > 0 && (UnityEngine.Object) componentInParent.EnemiesToTrack[0] == (UnityEngine.Object) this)
            this.totalHP += (float) Mathf.Min(15 * (GameManager.DungeonEndlessLevel - 2), 150);
          else
            this.totalHP += (float) Mathf.Min(GameManager.DungeonEndlessLevel - 2, 8);
        }
        UnitObject component = this.GetComponent<UnitObject>();
        if ((bool) (UnityEngine.Object) component)
        {
          switch (component.EnemyType)
          {
            case Enemy.WormBoss:
            case Enemy.FrogBoss:
            case Enemy.JellyBoss:
            case Enemy.SpiderBoss:
              if (DataManager.Instance.playerDeathsInARowFightingLeader >= 2)
              {
                this.totalHP *= 0.85f;
                break;
              }
              break;
          }
        }
      }
      this.HP = this.totalHP;
    }
    Health.HealEvent onInitHp = this.OnInitHP;
    if (onInitHp == null)
      return;
    onInitHp();
  }

  public virtual void OnDisable()
  {
    if (this.isPlayer)
      AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
    AudioManager.Instance.StopLoop(this.BurnLoopInstance);
    AudioManager.Instance.StopLoop(this.FreezeLoopInstance);
    if (Health.playerTeam.Contains(this))
      Health.playerTeam.Remove(this);
    if (Health.neutralTeam.Contains(this))
      Health.neutralTeam.Remove(this);
    if (Health.team2.Contains(this))
      Health.team2.Remove(this);
    if (Health.dangerousAnimals.Contains(this))
      Health.dangerousAnimals.Remove(this);
    if (Health.killAll.Contains(this))
      Health.killAll.Remove(this);
    Health.allUnits.Remove(this);
  }

  public static void DamageAllEnemies(float damage, Health.DamageAllEnemiesType damageType)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) PlayerFarming.Instance.health.DamageAllEnemiesIE(damage, damageType));
  }

  public IEnumerator DamageAllEnemiesIE(float damage, Health.DamageAllEnemiesType damageType)
  {
    Health health1 = this;
    foreach (Health health2 in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if (!((UnityEngine.Object) health2 == (UnityEngine.Object) null) && !(bool) (UnityEngine.Object) health2.GetComponentInParent<Projectile>() && !health2.ImmuneToDiseasedHearts)
      {
        switch (damageType)
        {
          case Health.DamageAllEnemiesType.BlackHeart:
            BiomeConstants.Instance.ShowHeartEffect(health2.transform, Vector3.up, Health.HeartEffects.Black);
            continue;
          case Health.DamageAllEnemiesType.DeathsDoor:
            BiomeConstants.Instance.ShowTarotCardDamage(health2.transform, Vector3.up * 1.5f);
            continue;
          case Health.DamageAllEnemiesType.DamagePerFollower:
            BiomeConstants.Instance.ShowDamageTextIcon(health2.transform, Vector3.up, damage);
            continue;
          case Health.DamageAllEnemiesType.TradeOff:
            BiomeConstants.Instance.ShowTarotCardDamage(health2.transform, Vector3.up * 1.5f);
            continue;
          default:
            continue;
        }
      }
    }
    yield return (object) new WaitForSeconds(1f);
    while (PlayerFarming.AnyPlayerGotoAndStopping() || !GameManager.RoomActive)
      yield return (object) null;
    foreach (Health health3 in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if ((UnityEngine.Object) health3 != (UnityEngine.Object) null)
        health3.DealDamage(damage, health1.gameObject, health1.transform.position, AttackType: Health.AttackTypes.NoKnockBack);
    }
  }

  public void ApplyHeartEffects(Health.HeartEffects heartEffectType)
  {
    foreach (Health health in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if (!((UnityEngine.Object) health == (UnityEngine.Object) null) && !(bool) (UnityEngine.Object) health.GetComponentInParent<Projectile>() && !health.ImmuneToDiseasedHearts)
      {
        BiomeConstants.Instance.ShowHeartEffect(health.transform, Vector3.up, heartEffectType);
        switch (heartEffectType)
        {
          case Health.HeartEffects.Fire:
            health.AddBurn(this.gameObject, UnityEngine.Random.Range(7f, 12f));
            continue;
          case Health.HeartEffects.Ice:
            health.AddIce(10f);
            continue;
          default:
            continue;
        }
      }
    }
  }

  public void Kill() => this.DealDamage(this.CurrentHP, this.gameObject, this.transform.position);

  public virtual bool DealDamage(
    float Damage,
    GameObject Attacker,
    Vector3 AttackLocation,
    bool BreakBlocking = false,
    Health.AttackTypes AttackType = Health.AttackTypes.Melee,
    bool dealDamageImmediately = false,
    Health.AttackFlags AttackFlags = (Health.AttackFlags) 0)
  {
    Health health = (Health) null;
    PlayerFarming attackerPlayerFarming = (PlayerFarming) null;
    if ((bool) (UnityEngine.Object) Attacker)
    {
      health = Attacker.GetComponent<Health>();
      attackerPlayerFarming = Attacker.GetComponent<PlayerFarming>();
    }
    if ((UnityEngine.Object) attackerPlayerFarming == (UnityEngine.Object) null)
    {
      GameObject spellOwner = Health.GetSpellOwner(Attacker);
      if ((UnityEngine.Object) spellOwner != (UnityEngine.Object) null)
        attackerPlayerFarming = spellOwner.GetComponent<PlayerFarming>();
    }
    if (!this.enabled || this.invincible || this.untouchable || this.GodMode == Health.CheatMode.God || (UnityEngine.Object) this.state != (UnityEngine.Object) null && !dealDamageImmediately && this.state.CURRENT_STATE == StateMachine.State.Dodging || (UnityEngine.Object) this.state != (UnityEngine.Object) null && this.state.CURRENT_STATE == StateMachine.State.InActive || (UnityEngine.Object) health == (UnityEngine.Object) this && !this.isPlayer && this.IsCharmedBuffer || (UnityEngine.Object) health != (UnityEngine.Object) null && health.isPlayer && this.ImmuneToPlayer || this.isPlayer && (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && AttackFlags.HasFlag((Enum) Health.AttackFlags.Trap) && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, this.playerFarming) || this.isPlayer && (this.state.CURRENT_STATE == StateMachine.State.CustomAnimation || PlayerFarming.Instance.GoToAndStopping))
      return false;
    Vector3 position1 = this.transform.position;
    AudioManager instance1 = AudioManager.Instance;
    if (this.isPlayer && !DataManager.Instance.ShownDodgeTutorial && DataManager.Instance.ShownDodgeTutorialCount < 3)
    {
      UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/UI/UI Control Prompt Dodge") as GameObject, GameObject.FindWithTag("Canvas").transform).GetComponent<UIDodgePromptTutorial>().Play(Attacker, this.GetComponent<PlayerFarming>());
      return false;
    }
    if (this.team == Health.Team.Team2 && PlayerFarming.playersCount > 1 && TrinketManager.HasTrinket(TarotCards.Card.CoopGoodTiming) && (double) Mathf.Abs(PlayerFarming.players[0].playerWeapon.TimeOfAttack - PlayerFarming.players[1].playerWeapon.TimeOfAttack) < 0.10000000149011612)
      AttackFlags |= Health.AttackFlags.Crit;
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
      this.Velocity = AttackLocation - Attacker.transform.position;
    if (this.isPlayer)
    {
      if (dealDamageImmediately)
      {
        MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) this);
        this.damageEventQueue = (Health.DealDamageEvent) null;
      }
      if (!dealDamageImmediately)
      {
        if (this.damageEventQueue != null)
          return false;
        this.damageEventQueue = new Health.DealDamageEvent(Time.unscaledTime, Damage, Attacker, AttackLocation, BreakBlocking, AttackType, AttackFlags);
        return true;
      }
      PlayerWeapon.EquippedWeaponsInfo currentWeaponInfo = this.playerFarming.CurrentWeaponInfo;
      bool flag1 = false;
      bool flag2 = this.ChanceToNegateDamage(currentWeaponInfo.NegateDamageChance + TrinketManager.GetNegateDamageChance(this.playerFarming)) || flag1;
      bool flag3 = TrinketManager.CanNegateDamage(this.playerFarming) && this.state.CURRENT_STATE == StateMachine.State.Heal || flag2;
      PlayerWeapon component1 = this.GetComponent<PlayerWeapon>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.ChargedNegation)
      {
        flag3 = true;
        component1.ChargedNegation = false;
      }
      if (flag3)
      {
        if (TrinketManager.HasTrinket(TarotCards.Card.MutatedNegateHit, this.playerFarming))
        {
          if ((UnityEngine.Object) health == (UnityEngine.Object) null)
          {
            GameObject spellOwner = Health.GetSpellOwner(Attacker);
            if ((UnityEngine.Object) spellOwner != (UnityEngine.Object) null)
              health = spellOwner.GetComponent<Health>();
          }
          if ((UnityEngine.Object) health != (UnityEngine.Object) null)
            health.DealDamage(component1.GetAverageWeaponDamage(this.playerFarming.currentWeapon, this.playerFarming.currentWeaponLevel), this.gameObject, AttackLocation);
        }
        AudioManager.Instance.PlayOneShot("event:/dlc/combat/defended_impact", this.transform.position);
        System.Action onDamageNegated = this.OnDamageNegated;
        if (onDamageNegated != null)
          onDamageNegated();
        this.NegatedDamage(AttackLocation);
        return false;
      }
      if ((double) this.HP == 1.0)
      {
        float num = DifficultyManager.GetChanceOfNegatingDeath() + (PlayerWeapon.FirstTimeUsingWeapon ? 0.2f : 0.0f);
        if ((double) UnityEngine.Random.Range(0.0f, 1f) <= (double) num)
          return false;
      }
      this.playerFarming.GetBlackSoul(TrinketManager.GetBlackSoulsOnDamaged(this.playerFarming), false);
      if (TrinketManager.DropBlackGoopOnDamaged(this.playerFarming))
        TrapGoop.CreateGoop(position1, 5, 0.5f, this.playerFarming.gameObject, GenerateRoom.Instance.transform);
      if (TrinketManager.DropBombOnDamaged(this.playerFarming) && !TrinketManager.IsOnCooldown(TarotCards.Card.BombOnDamaged, this.playerFarming))
      {
        Bomb.CreateBomb(position1, this, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
        TrinketManager.TriggerCooldown(TarotCards.Card.BombOnDamaged, this.playerFarming);
      }
      if (TrinketManager.DropTentacleOnDamaged(this.playerFarming) && !TrinketManager.IsOnCooldown(TarotCards.Card.TentacleOnDamaged, this.playerFarming))
      {
        float Duration = 10f;
        CurseData curseData = EquipmentManager.GetCurseData(EquipmentType.TENTACLE_TAROT_REF);
        Tentacle t = UnityEngine.Object.Instantiate<GameObject>(curseData.Prefab, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent, true).GetComponent<Tentacle>();
        t.transform.position = position1;
        t.GetComponent<Health>().enabled = false;
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(curseData.SecondaryPrefab, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
        gameObject.transform.position = position1 - Vector3.right;
        gameObject.GetComponent<FX_CrackController>().duration = Duration + 0.5f;
        float damage = curseData.Damage;
        t.Play(0.0f, Duration, damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming), this.team, false, 0, true, true);
        TrinketManager.TriggerCooldown(TarotCards.Card.TentacleOnDamaged, this.playerFarming);
        CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.25f);
        AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.gameObject);
        AudioManager.Instance.PlayOneShot("event:/followers/break_free", this.gameObject);
        BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.stone, t.transform.position, Vector3.one, 5);
        GameManager.GetInstance().WaitForSeconds(Duration + 0.5f, (System.Action) (() =>
        {
          if (!((UnityEngine.Object) t != (UnityEngine.Object) null))
            return;
          AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.gameObject);
          AudioManager.Instance.PlayOneShot("event:/followers/break_free", this.gameObject);
          CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.2f);
          BiomeConstants.Instance.EmitSmokeExplosionVFX(t.transform.position);
          BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.stone, t.transform.position, Vector3.one, 10);
        }));
      }
      if (TrinketManager.StrikeLightningOnDamaged(this.playerFarming))
      {
        ISpellOwning component2 = Attacker.GetComponent<ISpellOwning>();
        health = (Health) null;
        if (component2 != null)
        {
          GameObject owner = component2.GetOwner();
          if ((UnityEngine.Object) owner != (UnityEngine.Object) null)
          {
            health = owner.GetComponent<Health>();
            if ((UnityEngine.Object) health == (UnityEngine.Object) this)
              health = (Health) null;
          }
        }
        else
          health = Attacker.GetComponent<Health>();
        if ((UnityEngine.Object) health != (UnityEngine.Object) null && !health.IsHidden)
          new LightningStrikeAbility(1).Play(this.gameObject, Health.Team.Team2, 8f, this.playerFarming, true, new List<Health>()
          {
            health
          }, "");
      }
      if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedBombsAndHealth, this.playerFarming) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedBombsAndHealth, this.playerFarming))
      {
        int ofCorruptedBombs = TrinketManager.GetAmountOfCorruptedBombs(this.playerFarming);
        for (int index = 0; index < ofCorruptedBombs; ++index)
          Bomb.CreateBomb(this.transform.position, (Health) null, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
      }
      if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedTradeOff, this.playerFarming) && !TrinketManager.IsCorruptedPositiveEffectNegated(TarotCards.Card.CorruptedTradeOff, this.playerFarming))
        this.StartCoroutine((IEnumerator) this.DamageAllEnemiesIE((float) TrinketManager.GetDamageTradeOff(this.playerFarming) + DataManager.GetWeaponDamageMultiplier(this.playerFarming.currentWeaponLevel) * 3f, Health.DamageAllEnemiesType.TradeOff));
      if (TrinketManager.HasTrinket(TarotCards.Card.EasyMoney, this.playerFarming) && DataManager.Instance.PlayerFleece != 8 && (double) UnityEngine.Random.value < (double) TrinketManager.ChanceToDropWoolOnHit(this.playerFarming))
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.WOOL, 1, this.transform.position + Vector3.back, 0.0f);
        AudioManager.Instance.PlayOneShot("event:/dlc/tarot/shearingblade_trigger", this.transform.position);
      }
      if (TrinketManager.HasTrinket(TarotCards.Card.MutatedFreezeOnHit, this.playerFarming) && (double) UnityEngine.Random.value < 0.25)
        BiomeConstants.Instance.FreezeTime();
      if (TrinketManager.HasTrinket(TarotCards.Card.HitKillEnemy, this.playerFarming))
      {
        Health.team2.Shuffle<Health>();
        for (int index = Health.team2.Count - 1; index >= 0; --index)
        {
          UnitObject component3 = Health.team2[index].GetComponent<UnitObject>();
          if ((!((UnityEngine.Object) component3 != (UnityEngine.Object) null) || !component3.IsBoss) && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<BossIntro>() == (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<DeathCatController>() == (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<WolfArmPiece>() == (UnityEngine.Object) null && Health.team2[index].CanBeKilledByTarot)
          {
            this.StartCoroutine((IEnumerator) Health.\u003CDealDamage\u003Eg__DamageEnemy\u007C228_1(Health.team2[index]));
            break;
          }
        }
      }
      PlayerFleeceManager.ResetDamageModifier();
      if (PlayerFleeceManager.FleeceCausesPoisonOnHit())
        this.AddPoison((GameObject) null, Mathf.Max(2f, DifficultyManager.GetInvincibleTimeMultiplier()));
    }
    if (this.OnHitEarly != null)
      this.OnHitEarly(Attacker, AttackLocation, AttackType);
    if (this.invincible)
      return false;
    Health.HealthEvent onDamaged = this.OnDamaged;
    if (onDamaged != null)
      onDamaged(Attacker, AttackLocation, Damage, AttackType, AttackFlags);
    Damage *= this.DamageModifier;
    if (this.isPlayer)
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_5:
          if (!BiomeGenerator.Instance.OnboardingDungeon5)
            break;
          goto case FollowerLocation.LambTown;
        case FollowerLocation.LambTown:
          Damage = 0.0f;
          break;
      }
    }
    bool flag4 = false;
    if ((bool) (UnityEngine.Object) attackerPlayerFarming)
    {
      WeaponData weaponData = EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon);
      if ((UnityEngine.Object) weaponData != (UnityEngine.Object) null && weaponData.EquipmentType == EquipmentType.Sword_Ratau)
      {
        flag4 = true;
        AudioManager.Instance.PlayOneShot("event:/material/wood_impact", position1);
      }
    }
    if (!flag4)
    {
      switch (this.ImpactSoundToPlay)
      {
        case Health.IMPACT_SFX.IMPACT_BLUNT:
          AudioManager.Instance.PlayOneShot("event:/enemy/impact_blunt", position1);
          break;
        case Health.IMPACT_SFX.IMPACT_NORMAL:
          AudioManager.Instance.PlayOneShot("event:/enemy/impact_normal", position1);
          break;
        case Health.IMPACT_SFX.IMPACT_SQUISHY:
          AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", position1);
          break;
        case Health.IMPACT_SFX.HIT_SMALL:
          AudioManager.Instance.PlayOneShot("event:/enemy/gethit_small", position1);
          break;
        case Health.IMPACT_SFX.HIT_MEDIUM:
          AudioManager.Instance.PlayOneShot("event:/enemy/gethit_medium", position1);
          break;
        case Health.IMPACT_SFX.HIT_LARGE:
          AudioManager.Instance.PlayOneShot("event:/enemy/gethit_large", position1);
          break;
      }
    }
    if (AttackType == Health.AttackTypes.Projectile)
      Damage *= this.ArrowAttackVulnerability;
    if (AttackType == Health.AttackTypes.Melee)
      Damage *= this.MeleeAttackVulnerability;
    float angle1 = Utils.GetAngle(position1, AttackLocation);
    if (this.HasShield)
    {
      if ((UnityEngine.Object) attackerPlayerFarming != (UnityEngine.Object) null)
        ++this.hitsWithShield;
      if (this.hitsWithShield == 3)
      {
        System.Action onTutorialShown = BiomeGenerator.OnTutorialShown;
        if (onTutorialShown != null)
          onTutorialShown();
      }
      this.HandleShieldBreak(AttackFlags, angle1);
      Damage *= 0.2f;
    }
    if ((bool) (UnityEngine.Object) this.protector)
    {
      Attacker.GetComponent<UnitObject>();
      if ((UnityEngine.Object) this.showHpBar != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.showHpBar.hpBar && (bool) (UnityEngine.Object) this.showHpBar.hpBar.groupIndicator)
      {
        this.showHpBar.hpBar.groupIndicator.transform.localScale = Vector3.one;
        this.showHpBar.hpBar.groupIndicator.transform.DOKill();
        this.showHpBar.hpBar.groupIndicator.transform.DOPunchScale(Vector3.one * 1.25f, 0.25f);
      }
      Damage *= this.protector.damageMultiplier;
    }
    if ((double) Damage > 0.0)
    {
      if (this.isPlayer)
      {
        DataManager.Instance.PlayerDamageReceived += Damage;
        DataManager.Instance.PlayerDamageReceivedThisRun += Damage;
      }
      else if (this.team == Health.Team.Team2)
      {
        DataManager.Instance.PlayerDamageDealtThisRun += Damage;
        DataManager.Instance.PlayerDamageDealt += Damage;
        if ((bool) (UnityEngine.Object) attackerPlayerFarming && (UnityEngine.Object) attackerPlayerFarming.playerRelic.CurrentRelic != (UnityEngine.Object) null && !AttackFlags.HasFlag((Enum) Health.AttackFlags.DoesntChargeRelics))
          attackerPlayerFarming.playerRelic.IncreaseChargedAmount(Damage);
      }
      else if (this.team == Health.Team.Neutral && (bool) (UnityEngine.Object) attackerPlayerFarming && (bool) (UnityEngine.Object) attackerPlayerFarming.playerRelic)
        attackerPlayerFarming.playerRelic.IncreaseChargedAmount(Mathf.Min(Damage / 10f, 1f));
    }
    if (!BreakBlocking && (UnityEngine.Object) this.state != (UnityEngine.Object) null && this.state.CURRENT_STATE == StateMachine.State.Defending)
    {
      BiomeConstants.Instance.HitFX_Blocked.Spawn(AttackLocation, Quaternion.identity);
      Damage = 0.0f;
    }
    if (this.team == Health.Team.PlayerTeam && !this.IsCharmedEnemy)
    {
      float num = DungeonModifier.HasNegativeModifier(DungeonNegativeModifier.DoubleDamage, 2f, 1f) + PlayerFleeceManager.GetDamageReceivedMultiplier();
      if (this.isPlayer)
      {
        if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedTradeOff, this.playerFarming) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedTradeOff, this.playerFarming))
          num *= (float) TrinketManager.GetEnemyDamageMultiplier(this.playerFarming);
        if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedPoisonCoins, this.playerFarming) && (AttackFlags.HasFlag((Enum) Health.AttackFlags.Poison) || AttackType == Health.AttackTypes.Poison) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedPoisonCoins, this.playerFarming))
          num *= (float) TrinketManager.GetEnemyPoisonDamageMultiplier(this.playerFarming);
      }
      Damage *= Mathf.Clamp(num, 1f, 2f);
    }
    if (this.BlackSoulOnHit && (UnityEngine.Object) Attacker != (UnityEngine.Object) null && (bool) (UnityEngine.Object) attackerPlayerFarming && (AttackType == Health.AttackTypes.Melee || AttackType == Health.AttackTypes.Bullet) && this.team == Health.Team.Team2 && !this.gameObject.CompareTag("Projectile"))
    {
      BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(UnityEngine.Random.Range(1f, 2f) * TrinketManager.GetBlackSoulsMultiplier(attackerPlayerFarming)), position1, true, true);
      if ((bool) (UnityEngine.Object) blackSoul)
        blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(2f, 4f));
    }
    this.HandleFervourOnHit(attackerPlayerFarming, AttackType, position1);
    if ((double) this.IceHearts > 0.0 && (double) Damage > 0.0)
    {
      float iceHearts = this.IceHearts;
      this.IceHearts -= Damage;
      Damage -= iceHearts;
      if ((double) this.IceHearts < 0.0)
        this.IceHearts = 0.0f;
      this.ApplyHeartEffects(Health.HeartEffects.Ice);
    }
    if ((double) this.FireHearts > 0.0 && (double) Damage > 0.0)
    {
      float fireHearts = this.FireHearts;
      this.FireHearts -= Damage;
      Damage -= fireHearts;
      if ((double) this.FireHearts < 0.0)
        this.FireHearts = 0.0f;
      this.ApplyHeartEffects(Health.HeartEffects.Fire);
    }
    if ((double) this.BlackHearts > 0.0 && (double) Damage > 0.0)
    {
      float blackHearts = this.BlackHearts;
      this.BlackHearts -= Damage;
      Damage -= blackHearts;
      if ((double) this.BlackHearts < 0.0)
        this.BlackHearts = 0.0f;
      int level = 0;
      if (this.isPlayer)
        level = this.GetComponent<PlayerWeapon>().CurrentWeaponLevel;
      this.StartCoroutine((IEnumerator) this.DamageAllEnemiesIE((float) (1.25 + (double) DataManager.GetWeaponDamageMultiplier(level) * 3.0), Health.DamageAllEnemiesType.BlackHeart));
    }
    if ((double) this.BlueHearts > 0.0 && (double) Damage > 0.0)
    {
      float blueHearts = this.BlueHearts;
      this.BlueHearts -= Damage;
      Damage -= blueHearts;
      if ((double) this.BlueHearts < 0.0)
        this.BlueHearts = 0.0f;
    }
    if ((double) this.SpiritHearts > 0.0 && (double) Damage > 0.0)
    {
      float spiritHearts = this.SpiritHearts;
      this.SpiritHearts -= Damage;
      Damage -= spiritHearts;
      if ((double) this.SpiritHearts < 0.0)
        this.SpiritHearts = 0.0f;
    }
    if (this.GodMode == Health.CheatMode.Demigod)
      Damage = 0.0f;
    if (AttackFlags.HasFlag((Enum) Health.AttackFlags.NonLethal))
      Damage = Mathf.Max(0.0f, this.HP - 0.1f);
    if ((double) Damage > 0.0)
      this.HP -= Damage;
    if (this.team != Health.Team.Neutral && !this.IsPoisoned)
    {
      bool flag5 = false;
      float num1 = 0.0f;
      float num2 = 0.0f;
      bool flag6 = false;
      if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Poison))
      {
        flag6 = true;
        flag5 = true;
        if (AttackType == Health.AttackTypes.Projectile && (bool) (UnityEngine.Object) health && health.isPlayer && (bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming)
          num1 = 0.2f * (float) attackerPlayerFarming.currentCurseLevel;
      }
      if ((AttackType == Health.AttackTypes.Melee || AttackType == Health.AttackTypes.Heavy || AttackType == Health.AttackTypes.Bullet) && (bool) (UnityEngine.Object) attackerPlayerFarming && attackerPlayerFarming.currentWeapon != EquipmentType.None && (UnityEngine.Object) EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon) != (UnityEngine.Object) null && EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon).ContainsAttachmentType(AttachmentEffect.Poison) && !this.isPlayer && !this.isPlayerAlly && (UnityEngine.Object) health != (UnityEngine.Object) null && health.isPlayer)
      {
        float num3 = UnityEngine.Random.value;
        if ((bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming && attackerPlayerFarming.currentWeapon != EquipmentType.None)
        {
          float poisonChance = EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon).GetAttachment(AttachmentEffect.Poison).poisonChance;
          if (flag6 || (double) num3 <= (double) poisonChance)
          {
            flag5 = true;
            num2 = 0.2f * (float) attackerPlayerFarming.currentWeaponLevel;
          }
        }
      }
      this.enemyPoisonDamage = Mathf.Max(this.enemyPoisonDamage, num2, num1);
      if (flag5)
        this.AddPoison(Attacker);
    }
    if (this.team != Health.Team.Neutral && !this.IsBurned)
    {
      bool flag7 = false;
      float num4 = 0.0f;
      float num5 = 0.0f;
      bool flag8 = false;
      if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Burn) || AttackFlags == Health.AttackFlags.Burn)
      {
        flag8 = true;
        flag7 = true;
        if (AttackType == Health.AttackTypes.Projectile && (bool) (UnityEngine.Object) health && health.isPlayer && (bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming)
          num5 = 0.2f * (float) attackerPlayerFarming.currentCurseLevel;
      }
      if ((AttackType == Health.AttackTypes.Melee || AttackType == Health.AttackTypes.Heavy || AttackType == Health.AttackTypes.Bullet) && (bool) (UnityEngine.Object) attackerPlayerFarming && attackerPlayerFarming.currentWeapon != EquipmentType.None && (UnityEngine.Object) EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon) != (UnityEngine.Object) null && EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon).ContainsAttachmentType(AttachmentEffect.Burn) && !this.isPlayer && !this.isPlayerAlly && (UnityEngine.Object) health != (UnityEngine.Object) null && health.isPlayer)
      {
        float num6 = UnityEngine.Random.value;
        if ((bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming && attackerPlayerFarming.currentWeapon != EquipmentType.None)
        {
          float burnChance = EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon).GetAttachment(AttachmentEffect.Burn).burnChance;
          if (flag8 || (double) num6 <= (double) burnChance)
          {
            flag7 = true;
            num5 = 0.2f * (float) attackerPlayerFarming.currentWeaponLevel;
          }
        }
      }
      this.enemyBurnDamage = Mathf.Max(this.enemyBurnDamage, num5, num4);
      if (flag7)
        this.AddBurn(Attacker.gameObject);
    }
    this.HP = Mathf.Clamp(this.HP, 0.0f, float.MaxValue);
    bool flag9 = (double) this.CurrentHP <= 0.0 && this.GodMode != Health.CheatMode.Immortal;
    if (flag9)
    {
      Health.DieAction onDieEarly = this.OnDieEarly;
      if (onDieEarly != null)
        onDieEarly(Attacker, AttackLocation, this, AttackType, AttackFlags);
    }
    if (this.invincible)
      return false;
    if (!flag9)
    {
      bool FromBehind = false;
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && (UnityEngine.Object) Attacker != (UnityEngine.Object) null && (UnityEngine.Object) this.transform != (UnityEngine.Object) null)
        FromBehind = (double) Mathf.Abs(this.state.facingAngle - Utils.GetAngle(position1, Attacker.transform.position) % 360f) >= 150.0;
      if (AttackType == Health.AttackTypes.Poison || AttackType == Health.AttackTypes.Electrified)
      {
        this.showHpBar?.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
        UIBossHUD.Instance?.OnBossHit(Attacker, AttackLocation, AttackType, FromBehind);
        Health.HitAction onPoisonedHit = this.OnPoisonedHit;
        if (onPoisonedHit != null)
          onPoisonedHit(Attacker, AttackLocation, Health.AttackTypes.Poison, FromBehind);
      }
      else if (AttackType == Health.AttackTypes.Burn)
      {
        this.showHpBar?.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
        UIBossHUD.Instance?.OnBossHit(Attacker, AttackLocation, AttackType, FromBehind);
        Health.HitAction onBurnHit = this.OnBurnHit;
        if (onBurnHit != null)
          onBurnHit(Attacker, AttackLocation, Health.AttackTypes.Burn, FromBehind);
      }
      if (AttackType != Health.AttackTypes.Poison && AttackType != Health.AttackTypes.Electrified && AttackType != Health.AttackTypes.Burn || this.isPlayer)
      {
        Health.HitAction onHit = this.OnHit;
        if (onHit != null)
          onHit(Attacker, AttackLocation, AttackType, FromBehind);
        this.OnHitCallback?.Invoke();
      }
      if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Penetration))
      {
        Health.HitAction onPenetrationHit = this.OnPenetrationHit;
        if (onPenetrationHit != null)
          onPenetrationHit(Attacker, AttackLocation, AttackType, FromBehind);
      }
    }
    else
    {
      bool FromBehind = false;
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && (UnityEngine.Object) Attacker != (UnityEngine.Object) null && (UnityEngine.Object) this.transform != (UnityEngine.Object) null)
        FromBehind = (double) Mathf.Abs(this.state.facingAngle - Utils.GetAngle(position1, Attacker.transform.position) % 360f) >= 150.0;
      Health.HitAction onHitForceBossHud = this.OnHitForceBossHUD;
      if (onHitForceBossHud != null)
        onHitForceBossHud(Attacker, AttackLocation, AttackType, FromBehind);
    }
    Vector3 vector3_1 = position1 - AttackLocation;
    float angle2 = Utils.GetAngle(position1, AttackLocation);
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
    {
      StateMachine component = Attacker.GetComponent<StateMachine>();
      if (!this.isPlayer)
      {
        if (this.InanimateObject)
        {
          if (this.InanimateObjectEffect)
            BiomeConstants.Instance.EmitHitVFX(position1 + Vector3.back, Quaternion.identity.z, "HitFX_Weak");
        }
        else if (this.ImpactOnHit && this.team != Health.Team.Neutral && (bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming)
        {
          if (!AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit) || this.InanimateObject)
          {
            BiomeConstants.Instance.PlayerEmitHitImpactEffect(position1 + Vector3.back * 0.5f, (UnityEngine.Object) component != (UnityEngine.Object) null ? component.facingAngle : angle2, false, this.ImpactOnHitColor, this.ImpactOnHitScale, AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit), attackerPlayerFarming);
          }
          else
          {
            GameManager.GetInstance().HitStop(0.2f);
            BiomeConstants instance2 = BiomeConstants.Instance;
            Vector3 Position = position1 + Vector3.back * 0.5f;
            double Angle = (UnityEngine.Object) component != (UnityEngine.Object) null ? (double) component.facingAngle : (double) angle2;
            bool flag10 = AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit);
            PlayerFarming playerFarming1 = attackerPlayerFarming;
            Color color = new Color();
            int num = flag10 ? 1 : 0;
            PlayerFarming playerFarming2 = playerFarming1;
            instance2.PlayerEmitHitImpactEffect(Position, (float) Angle, color: color, crit: num != 0, playerFarming: playerFarming2);
          }
        }
      }
      else if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit))
      {
        if (!flag9)
        {
          BiomeConstants instance3 = BiomeConstants.Instance;
          Vector3 Position = position1 + Vector3.back * 0.5f;
          double Angle = (UnityEngine.Object) component != (UnityEngine.Object) null ? (double) component.facingAngle : (double) angle2;
          bool flag11 = AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit);
          Color color = new Color();
          int num = flag11 ? 1 : 0;
          instance3.PlayerEmitHitImpactEffect(Position, (float) Angle, color: color, crit: num != 0);
        }
        else
        {
          BiomeConstants instance4 = BiomeConstants.Instance;
          Vector3 Position = position1 + Vector3.back * 0.5f;
          double Angle = (UnityEngine.Object) component != (UnityEngine.Object) null ? (double) component.facingAngle : (double) angle2;
          bool flag12 = AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit);
          Color color = new Color();
          int num = flag12 ? 1 : 0;
          instance4.PlayerEmitHitImpactEffect(Position, (float) Angle, false, color, crit: num != 0);
        }
      }
      if (this.team != Health.Team.Neutral)
      {
        if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Charm))
          this.AddCharm();
        else if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Ice))
          this.AddIce();
        else if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Electrified) && !flag9)
          this.AddElectrified(Attacker);
      }
    }
    if (this.BloodOnHit)
    {
      Vector3 vector3_2 = new Vector3(position1.x, position1.y, 0.0f);
      if (!this.gameObject.CompareTag("Player") && !flag9)
      {
        if (AttackType == Health.AttackTypes.Heavy)
        {
          BiomeConstants.Instance.EmitBloodSplatter(vector3_2, vector3_1.normalized, this.bloodColor);
          BiomeConstants.Instance.EmitBloodImpact(vector3_2 + Vector3.back * 0.5f, angle2, "black");
          string[] strArray = new string[2]
          {
            "BloodImpact_Large_0",
            "BloodImpact_Large_1"
          };
          int index = UnityEngine.Random.Range(0, strArray.Length - 1);
          if (strArray[index] != null)
            BiomeConstants.Instance.EmitBloodImpact(vector3_2 + Vector3.back * 0.5f, angle2, "black", strArray[index]);
        }
        else
        {
          BiomeConstants.Instance.EmitBloodSplatter(vector3_2, vector3_1.normalized, this.bloodColor);
          string[] strArray = new string[3]
          {
            "BloodImpact_0",
            "BloodImpact_1",
            "BloodImpact_2"
          };
          int index = UnityEngine.Random.Range(0, strArray.Length - 1);
          if (strArray[index] != null)
            BiomeConstants.Instance.EmitBloodImpact(vector3_2 + Vector3.back * 0.5f, angle2, "black", strArray[index]);
        }
      }
      else if (AttackType == Health.AttackTypes.Heavy)
      {
        if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
        {
          if (this.state.CURRENT_STATE != StateMachine.State.Flying)
            BiomeConstants.Instance.EmitBloodSplatterGroundParticles(vector3_2, this.Velocity, this.bloodColor);
        }
        else
          BiomeConstants.Instance.EmitBloodSplatterGroundParticles(vector3_2, this.Velocity, this.bloodColor);
        BiomeConstants.Instance.EmitBloodImpact(vector3_2 + Vector3.back * 0.5f, angle2, "black");
        string[] strArray = new string[2]
        {
          "BloodImpact_Large_0",
          "BloodImpact_Large_1"
        };
        int index = UnityEngine.Random.Range(0, strArray.Length - 1);
        if (strArray[index] != null)
          BiomeConstants.Instance.EmitBloodImpact(vector3_2 + Vector3.back * 0.5f, angle2, "black", strArray[index], false);
      }
      else
      {
        if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
        {
          if (this.state.CURRENT_STATE != StateMachine.State.Flying)
            BiomeConstants.Instance.EmitBloodSplatterGroundParticles(vector3_2, this.Velocity, this.bloodColor);
        }
        else
          BiomeConstants.Instance.EmitBloodSplatterGroundParticles(vector3_2, this.Velocity, this.bloodColor);
        BiomeConstants.Instance.EmitBloodSplatter(vector3_2, vector3_1.normalized, this.bloodColor);
        string[] strArray = new string[3]
        {
          "BloodImpact_0",
          "BloodImpact_1",
          "BloodImpact_2"
        };
        int index = UnityEngine.Random.Range(0, strArray.Length - 1);
        if (strArray[index] != null)
          BiomeConstants.Instance.EmitBloodImpact(vector3_2 + Vector3.back * 0.5f, angle2, "black", strArray[index], false);
      }
    }
    if (this.ScreenshakeOnHit && !(this.ScreenshakeOnDie & flag9))
      CameraManager.shakeCamera(this.ScreenShakeOnDieIntensity / 3f);
    if (this.isPlayer)
    {
      if ((double) this.CurrentHP <= 1.0 && TrinketManager.HasTrinket(TarotCards.Card.DeathsDoor, this.playerFarming))
        this.StartCoroutine((IEnumerator) this.DamageAllEnemiesIE((float) TrinketManager.GetDamageAllEnemiesAmount(TarotCards.Card.DeathsDoor, this.playerFarming) + DataManager.GetWeaponDamageMultiplier(this.playerFarming.currentWeaponLevel) * 3f, Health.DamageAllEnemiesType.DeathsDoor));
      if ((double) this.CurrentHP <= 1.0 && TrinketManager.HasTrinket(TarotCards.Card.LastChance, this.playerFarming))
      {
        if ((double) this.CurrentHP == 1.0)
        {
          this.playerFarming.CustomAnimationWithCallback("resurrect", false, (System.Action) (() =>
          {
            this.state.LockStateChanges = false;
            this.state.CURRENT_STATE = StateMachine.State.Idle;
          }));
          this.state.LockStateChanges = true;
        }
        this.IceHearts += (float) TrinketManager.GetIceHeartsToHealOnRevive(this.playerFarming);
        TrinketManager.RemoveTrinket(TarotCards.Card.LastChance, this.playerFarming);
        BiomeConstants.Instance.ShowDestroyTarot(this.transform);
        Vector3 position2 = this.playerFarming.CameraBone.transform.position;
        BiomeConstants.Instance.EmitHeartPickUpVFX(position2, 0.0f, "black", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/dlc/tarot/frostsbite_trigger", position2);
      }
    }
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && this.team == Health.Team.Team2 && AttackType == Health.AttackTypes.Projectile && (bool) (UnityEngine.Object) Attacker.GetComponentInParent<MegaSlash>() && (bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming)
    {
      if (attackerPlayerFarming.currentCurse == EquipmentType.MegaSlash_Ice && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.MegaSlash_Ice).Chance)
        this.AddIce();
      else if (attackerPlayerFarming.currentCurse == EquipmentType.MegaSlash_Charm && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.MegaSlash_Charm).Chance)
        this.AddCharm();
    }
    if (flag9)
    {
      if (this.team == Health.Team.Team2)
      {
        ++DataManager.Instance.PlayerKillsOnRun;
        ++DataManager.Instance.KillsInGame;
        if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && (bool) (UnityEngine.Object) health && (bool) (UnityEngine.Object) attackerPlayerFarming && (UnityEngine.Object) Attacker != (UnityEngine.Object) null && ((bool) (UnityEngine.Object) attackerPlayerFarming || (bool) (UnityEngine.Object) Attacker.GetComponentInParent<MegaSlash>()))
        {
          if ((AttackType == Health.AttackTypes.Melee || AttackType == Health.AttackTypes.Bullet) && EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon).ContainsAttachmentType(AttachmentEffect.Necromancy) && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetWeaponData(attackerPlayerFarming.currentWeapon).GetAttachment(AttachmentEffect.Necromancy).necromancyChance || AttackType == Health.AttackTypes.Projectile && attackerPlayerFarming.currentCurse == EquipmentType.MegaSlash_Necromancy)
            ProjectileGhost.SpawnGhost(position1, 0.0f, 1f + DataManager.GetWeaponDamageMultiplier(attackerPlayerFarming.currentWeaponLevel), 1f);
          if (AttackType == Health.AttackTypes.Melee && (bool) (UnityEngine.Object) attackerPlayerFarming.CurrentWeaponInfo.WeaponData.GetAttachment(AttachmentEffect.NeighbouringDamage))
          {
            Health targetEnemy = UnitObject.GetClosestTarget(this.transform, Health.Team.Team2);
            if ((UnityEngine.Object) targetEnemy != (UnityEngine.Object) null)
              SoulCustomTarget.Create(targetEnemy.gameObject, this.transform.position, Color.red, (System.Action) (() =>
              {
                if (!((UnityEngine.Object) targetEnemy != (UnityEngine.Object) null))
                  return;
                targetEnemy.DealDamage(attackerPlayerFarming.playerWeapon.GetAverageWeaponDamage(attackerPlayerFarming.currentWeapon, attackerPlayerFarming.currentWeaponLevel), attackerPlayerFarming.gameObject, targetEnemy.transform.position);
              }), 0.75f);
          }
        }
      }
      switch (this.DeathSoundToPlay)
      {
        case Health.DEATH_SFX.DEATH_SMALL:
          AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_small", position1);
          break;
        case Health.DEATH_SFX.DEATH_MEDIUM:
          AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_medium", position1);
          break;
        case Health.DEATH_SFX.DEATH_LARGE:
          AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_large", position1);
          break;
      }
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      if (DungeonModifier.HasNegativeModifier(DungeonNegativeModifier.DropPoison) && this.team == Health.Team.Team2)
        TrapPoison.CreatePoison(position1, 5, 0.5f, this.transform.parent);
      if (this.SmokeOnDie && !this.isPlayer)
        BiomeConstants.Instance.EmitSmokeExplosionVFX(position1 + Vector3.back * 0.5f);
      if (this.BloodOnDie)
      {
        if (!this.isPlayer)
          BiomeConstants.Instance.EmitBloodSplatter(position1, vector3_1.normalized, this.bloodColor);
        if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
        {
          if (this.state.CURRENT_STATE != StateMachine.State.Flying)
            BiomeConstants.Instance.EmitBloodSplatterGroundParticles(position1, this.Velocity, this.bloodColor);
        }
        else
          BiomeConstants.Instance.EmitBloodSplatterGroundParticles(position1, this.Velocity, this.bloodColor);
        BiomeConstants.Instance.EmitBloodImpact(position1 + Vector3.back * 0.5f, angle2, "black", useDeltaTime: !this.isPlayer);
        string[] strArray = new string[2]
        {
          "BloodImpact_Large_0",
          "BloodImpact_Large_1"
        };
        int index = UnityEngine.Random.Range(0, 1);
        BiomeConstants.Instance.EmitBloodImpact(position1 + Vector3.back * 0.5f, angle2, "black", strArray[index], !this.isPlayer);
      }
      if (this.spawnParticles)
        BiomeConstants.Instance.EmitParticleChunk(this.typeOfParticle, position1, this.Velocity, 6);
      if (this.ScreenshakeOnDie)
        CameraManager.shakeCamera(this.ScreenShakeOnDieIntensity);
      if (this.EmitGroundSmashDecal)
        BiomeConstants.Instance.EmitGroundSmashVFXParticles(new Vector3(position1.x, position1.y, 0.0f));
      this.HP = 0.0f;
      this.HandleBuffs(Attacker);
      this.ClearElectrified();
      if (this.OnDie != null)
        this.OnDie(Attacker, AttackLocation, this, AttackType, AttackFlags);
      if (Health.OnDieAny != null)
        Health.OnDieAny(this);
      if (this.OnDieCallback != null)
        this.OnDieCallback.Invoke();
      if (this.DestroyOnDeath)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else
      {
        this.enabled = false;
        if ((bool) (UnityEngine.Object) this.state && !(this is HealthPlayer))
          this.state.CURRENT_STATE = StateMachine.State.Dead;
      }
    }
    return true;
  }

  public void HandleShieldBreak(Health.AttackFlags AttackFlags, float Angle)
  {
    if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Penetration))
      BiomeConstants.Instance.EmitBlockImpact(this.transform.position, Angle, this.transform, "Break");
    else
      BiomeConstants.Instance.EmitBlockImpact(this.transform.position, Angle, this.transform);
  }

  public void DisplayHPBar()
  {
    this.showHpBar?.OnHit((GameObject) null, Vector3.zero, Health.AttackTypes.Melee, false);
  }

  public void HandleFervourOnHit(
    PlayerFarming attackingPlayer,
    Health.AttackTypes attackTypes,
    Vector3 position)
  {
    if (!((UnityEngine.Object) attackingPlayer != (UnityEngine.Object) null) || attackTypes != Health.AttackTypes.Melee || this.team != Health.Team.Team2 && this.team != Health.Team.KillAll && !this.IsCharmedEnemy || (double) UnityEngine.Random.value > (double) attackingPlayer.CurrentWeaponInfo.FervourOnHitChance)
      return;
    int num = UnityEngine.Random.Range(3, 6);
    for (int index = 0; index < num; ++index)
    {
      BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(UnityEngine.Random.Range(1f, 2f) * TrinketManager.GetBlackSoulsMultiplier(attackingPlayer)), position, true, true);
      if ((bool) (UnityEngine.Object) blackSoul)
        blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(2f, 4f));
    }
  }

  public void HandleBuffs(GameObject Attacker)
  {
    Health component1 = Attacker.GetComponent<Health>();
    bool flag = false;
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && (UnityEngine.Object) component1 == (UnityEngine.Object) null)
      flag = (UnityEngine.Object) Attacker.GetComponent<Demon>() != (UnityEngine.Object) null;
    if (this.team != Health.Team.Team2 && !this.IsCharmedEnemy)
      return;
    if ((bool) (UnityEngine.Object) component1 && component1.team == Health.Team.PlayerTeam && !component1.isPlayerAlly && !component1.IsCharmedEnemy)
    {
      PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(Attacker);
      PlayerWeapon.EquippedWeaponsInfo currentWeaponInfo = farmingComponent.CurrentWeaponInfo;
      if (!farmingComponent.IsKnockedOut)
        this.HandleHeartTrinketBuffs((Health) farmingComponent.health, currentWeaponInfo.HealChance);
      this.HandleDamageModifierBuff();
      this.HandleSinChanceDrop(farmingComponent);
    }
    else
    {
      if (!((UnityEngine.Object) Attacker != (UnityEngine.Object) null))
        return;
      GameObject spellOwner = Health.GetSpellOwner(Attacker);
      Explosion component2 = Attacker.GetComponent<Explosion>();
      if ((UnityEngine.Object) spellOwner != (UnityEngine.Object) null)
      {
        PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(spellOwner);
        PlayerWeapon.EquippedWeaponsInfo currentWeaponInfo = farmingComponent.CurrentWeaponInfo;
        if (!farmingComponent.IsKnockedOut)
          this.HandleHeartTrinketBuffs((Health) farmingComponent.health, currentWeaponInfo.HealChance);
        this.HandleDamageModifierBuff();
        this.HandleSinChanceDrop(farmingComponent);
      }
      else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.GetTeam() == Health.Team.PlayerTeam)
      {
        this.HandleDamageModifierBuff();
      }
      else
      {
        if (!flag && (!(bool) (UnityEngine.Object) component1 || !component1.isPlayerAlly && !component1.IsCharmedEnemy))
          return;
        this.HandleDamageModifierBuff();
      }
    }
  }

  public void HandleHeartTrinketBuffs(Health attackerHealth, float additionalHealChance = 0.0f)
  {
    if (!attackerHealth.isPlayer)
      return;
    float healChance = TrinketManager.GetHealChance(attackerHealth.playerFarming);
    attackerHealth.ChanceToHeal(additionalHealChance + healChance, 1f);
    if ((double) attackerHealth.BlueHearts >= 4.0)
      return;
    attackerHealth.ChanceToGainBlueHeart(TrinketManager.GetChanceOfGainingBlueHeart(attackerHealth.playerFarming));
  }

  public void HandleDamageModifierBuff()
  {
    if (!this.CanIncreaseDamageMultiplier)
      return;
    PlayerFleeceManager.IncrementDamageModifier();
  }

  public void HandleSinChanceDrop(PlayerFarming playerFarming)
  {
    if (!TrinketManager.HasTrinket(TarotCards.Card.Sin, playerFarming) || (double) UnityEngine.Random.value >= (double) TrinketManager.GetSinChance(playerFarming) || !this.CanSpawnTarotSinOnDie)
      return;
    PickUp sinPickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1, this.transform.position);
    sinPickUp.CanBePickedUp = false;
    sinPickUp.MagnetToPlayer = false;
    sinPickUp.child.transform.DOLocalMove(new Vector3(0.0f, 0.5f, -1.2f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutElastic).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => sinPickUp.child.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      sinPickUp.CanBePickedUp = true;
      sinPickUp.MagnetToPlayer = true;
    }))));
  }

  public void NegatedDamage(Vector3 attackPosition)
  {
    BiomeConstants.Instance.EmitBlockImpact(this.transform.position, Utils.GetAngle(this.transform.position, attackPosition));
  }

  public static GameObject GetSpellOwner(GameObject attacker)
  {
    ISpellOwning spellOwning = (ISpellOwning) null;
    if ((bool) (UnityEngine.Object) attacker)
      spellOwning = attacker.GetComponent<ISpellOwning>();
    if (spellOwning == null && (UnityEngine.Object) attacker != (UnityEngine.Object) null)
      spellOwning = attacker.GetComponentInParent<ISpellOwning>();
    return spellOwning?.GetOwner();
  }

  public static void DamageAtPosition(
    Health.Team hostTeam,
    Vector3 postion,
    float radius,
    float damage)
  {
    switch (hostTeam)
    {
      case Health.Team.PlayerTeam:
        for (int index = Health.team2.Count - 1; index >= 0; --index)
        {
          if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (double) Vector3.Distance(Health.team2[index].transform.position, postion) <= (double) radius)
            Health.team2[index].DealDamage(damage, Health.team2[index].gameObject, postion);
        }
        break;
      case Health.Team.Team2:
        for (int index = Health.playerTeam.Count - 1; index >= 0; --index)
        {
          if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null && (double) Vector3.Distance(Health.playerTeam[index].transform.position, postion) <= (double) radius)
            Health.playerTeam[index].DealDamage(damage, Health.playerTeam[index].gameObject, postion);
        }
        break;
    }
  }

  public void ForgiveRecentDamage()
  {
    Health.DealDamageEvent damageEventQueue = this.damageEventQueue;
    this.damageEventQueue = (Health.DealDamageEvent) null;
  }

  public bool IsAttackerInDamageEventQueue(GameObject attacker)
  {
    return this.damageEventQueue != null && !((UnityEngine.Object) this.damageEventQueue.Attacker == (UnityEngine.Object) null) && (UnityEngine.Object) this.damageEventQueue.Attacker == (UnityEngine.Object) attacker;
  }

  public virtual void Update()
  {
    if (this.damageEventQueue != null && (double) GameManager.GetInstance().UnscaledTimeSince(this.damageEventQueue.UnscaledTimestamp) >= (double) Health.DealDamageForgivenessWindow)
    {
      if ((UnityEngine.Object) this.damageEventQueue.Attacker != (UnityEngine.Object) null)
        this.DealDamage(this.damageEventQueue.Damage, this.damageEventQueue.Attacker, this.damageEventQueue.AttackLocation, this.damageEventQueue.BreakBlocking, this.damageEventQueue.AttackType, true, this.damageEventQueue.AttackFlags);
      else
        this.damageEventQueue = (Health.DealDamageEvent) null;
    }
    this.WasJustParried = false;
    this.PoisonCalculate(this.playerFarming);
    this.CharmCalculate();
    this.IceCalculate();
    this.ElectrifiedCalculate();
    this.BurnCalculate(this.playerFarming);
    if (!this.IsBurned || (double) UnityEngine.Random.value > 0.60000002384185791)
      return;
    this.SpreadBurn();
  }

  public void PoisonCalculate(PlayerFarming playerFarming)
  {
    if ((double) this.CurrentHP > 0.0 && this.poisonCounter > 0)
    {
      this.poisonTimer += Time.deltaTime;
      float num1 = this.poisonTimer / this.poisonTickDuration;
      if (!this.createdPoisonLoop && this.isPlayer)
      {
        this.PoisonLoopInstance = AudioManager.Instance.CreateLoop("event:/player/poison_loop", this.gameObject, true);
        int num2 = (int) this.PoisonLoopInstance.setParameterByName("parameter:/poison", num1);
        this.createdPoisonLoop = true;
      }
      if ((double) num1 > 1.0)
      {
        if (this.isPlayer)
        {
          AudioManager.Instance.PlayOneShot("event:/player/poisoned", this.gameObject);
          AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
          this.createdPoisonLoop = false;
        }
        if (this.isPlayer && playerFarming.state.CURRENT_STATE == StateMachine.State.Dodging)
        {
          AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
          this.createdPoisonLoop = false;
        }
        else
        {
          AudioManager.Instance.PlayOneShot("event:/player/poison_damage", this.gameObject);
          if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null)
            this.DealDamage(this.isPlayer ? this.playerPoisonDamage : this.enemyPoisonDamage, this.poisonAttacker, this.transform.position, AttackType: Health.AttackTypes.Poison, dealDamageImmediately: true);
          SimpleSpineFlash[] componentsInChildren = this.gameObject.GetComponentsInChildren<SimpleSpineFlash>();
          if (componentsInChildren.Length != 0)
          {
            foreach (SimpleSpineFlash simpleSpineFlash in componentsInChildren)
              simpleSpineFlash.FlashFillGreen();
          }
          else
            this.GetComponentInChildren<SimpleSpineAnimator>()?.FlashFillGreen();
        }
        this.poisonTimer = 0.0f;
      }
      else if ((double) num1 <= 0.0 && this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
        this.createdPoisonLoop = false;
      }
      if ((double) this.enemyPoisonTimestamp == -1.0 || (double) Time.time <= (double) this.enemyPoisonTimestamp || this.team == Health.Team.PlayerTeam)
        return;
      this.poisonCounter = 0;
    }
    else
    {
      if ((double) this.CurrentHP <= 0.0 || this.poisonCounter != 0)
        return;
      if (this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
        this.createdPoisonLoop = false;
      }
      this.poisonTimer -= Time.deltaTime;
      if ((double) this.poisonTimer > 0.0)
        return;
      this.ClearPoison();
    }
  }

  public void IceCalculate()
  {
    if (this.timeFrozen)
      this.enemyIceTimestamp += Time.deltaTime;
    if ((double) this.HP > 0.0 && this.iceCounter > 0)
    {
      if (!this.createdIceLoop && this.isPlayer)
        this.createdIceLoop = true;
      if ((double) this.enemyIceTimestamp == -1.0 || (double) Time.time <= (double) this.enemyIceTimestamp || this.team == Health.Team.PlayerTeam)
        return;
      this.iceCounter = 0;
    }
    else
    {
      if ((double) this.HP <= 0.0 || this.iceCounter != 0)
        return;
      if (this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.IceLoopInstance);
        this.createdIceLoop = false;
      }
      this.ClearIce();
    }
  }

  public void CharmCalculate()
  {
    if ((double) this.HP > 0.0 && this.charmCounter > 0)
    {
      this.charmTimer += Time.deltaTime;
      if (!this.createdCharmLoop && this.isPlayer)
        this.createdCharmLoop = true;
      if (((double) this.enemyCharmTimestamp == -1.0 || (double) Time.time <= (double) this.enemyCharmTimestamp) && Health.team2.Count > 1)
        return;
      this.charmCounter = 0;
      this.enemyLastCharmTimestamp = Time.time;
    }
    else
    {
      if ((double) this.HP <= 0.0 || this.charmCounter != 0)
        return;
      if (this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.CharmLoopInstance);
        this.createdCharmLoop = false;
      }
      this.charmTimer -= Time.deltaTime;
      if ((double) this.charmTimer > 0.0)
        return;
      this.ClearCharm();
    }
  }

  public void ElectrifiedCalculate()
  {
    if ((double) this.CurrentHP > 0.0 && this.electrifiedCounter > 0)
    {
      this.electrifiedTimer += Time.deltaTime;
      double num = (double) this.electrifiedTimer / (double) this.electrifiedTickDuration;
      if (!this.createdElectrifiedLoop && this.isPlayer)
        this.createdElectrifiedLoop = true;
      if (num > 1.0)
      {
        if (this.isPlayer)
          this.createdElectrifiedLoop = false;
        if (!this.isPlayer || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Dodging)
        {
          if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null)
            this.DealDamage(this.isPlayer ? this.playerElectrifiedDamage : this.enemyElectrifiedDamage, this.electrifiedAttacker, this.transform.position, AttackType: Health.AttackTypes.Electrified, dealDamageImmediately: true);
          SimpleSpineFlash[] componentsInChildren = this.gameObject.GetComponentsInChildren<SimpleSpineFlash>();
          if (componentsInChildren.Length != 0)
          {
            foreach (SimpleSpineFlash simpleSpineFlash in componentsInChildren)
              simpleSpineFlash.FlashFillBlack(true);
          }
          else
            this.GetComponentInChildren<SimpleSpineAnimator>()?.FlashFillBlack(true);
        }
        this.electrifiedTimer = 0.0f;
      }
      if ((double) this.enemyElectrifiedTimestamp == -1.0 || (double) Time.time <= (double) this.enemyElectrifiedTimestamp || this.team == Health.Team.PlayerTeam)
        return;
      this.electrifiedCounter = 0;
    }
    else
    {
      if ((double) this.CurrentHP <= 0.0 || this.electrifiedCounter != 0)
        return;
      if (this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.ElectrifiedLoopInstance);
        this.createdElectrifiedLoop = false;
      }
      this.electrifiedTimer -= Time.deltaTime;
      if ((double) this.electrifiedTimer > 0.0)
        return;
      this.ClearElectrified();
    }
  }

  public void BurnCalculate(PlayerFarming playerFarming)
  {
    if ((double) this.CurrentHP > 0.0 && this.burnCounter > 0)
    {
      this.burnTimer += Time.deltaTime;
      float num1 = this.burnTimer / this.burnTickDuration;
      if (!this.createdBurnLoop && this.isPlayer)
      {
        this.BurnLoopInstance = AudioManager.Instance.CreateLoop(this.EffectBurningLoopSFX, this.gameObject, true);
        int num2 = (int) this.BurnLoopInstance.setParameterByName("parameter:/poison", num1);
        this.createdBurnLoop = true;
      }
      if ((double) num1 > 1.0)
      {
        if (this.isPlayer)
        {
          AudioManager.Instance.PlayOneShot(this.EffectPlayerBurnReminderSFX, this.gameObject);
          AudioManager.Instance.StopLoop(this.BurnLoopInstance);
          this.createdBurnLoop = false;
        }
        if (this.isPlayer && playerFarming.state.CURRENT_STATE == StateMachine.State.Dodging)
        {
          AudioManager.Instance.StopLoop(this.BurnLoopInstance);
          this.createdBurnLoop = false;
        }
        else
        {
          if (this.isPlayer)
            AudioManager.Instance.PlayOneShot("event:/dlc/player/effect_fire_burning_damage_tick", this.gameObject);
          else
            AudioManager.Instance.PlayOneShot(this.EffectBurningDamageSFX, this.gameObject);
          if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null)
          {
            if (!((UnityEngine.Object) this.burnAttacker != (UnityEngine.Object) null))
              this.burnAttacker = this.gameObject;
            this.DealDamage(this.isPlayer ? this.playerBurnDamage : this.enemyBurnDamage, this.burnAttacker, this.transform.position, AttackType: Health.AttackTypes.Burn, dealDamageImmediately: true);
          }
          SimpleSpineFlash[] componentsInChildren = this.gameObject.GetComponentsInChildren<SimpleSpineFlash>();
          if (componentsInChildren.Length != 0)
          {
            foreach (SimpleSpineFlash simpleSpineFlash in componentsInChildren)
              simpleSpineFlash.FlashFillRed();
          }
          else
            this.GetComponentInChildren<SimpleSpineAnimator>()?.FlashFillRed();
        }
        this.burnTimer = 0.0f;
      }
      else if ((double) num1 <= 0.0 && this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.BurnLoopInstance);
        this.createdBurnLoop = false;
      }
      if ((double) this.enemyBurnTimestamp != -1.0 && (double) Time.time > (double) this.enemyBurnTimestamp && this.team != Health.Team.PlayerTeam)
        this.burnCounter = 0;
    }
    else if ((double) this.CurrentHP > 0.0 && this.burnCounter == 0)
    {
      if (this.isPlayer)
      {
        AudioManager.Instance.StopLoop(this.BurnLoopInstance);
        this.createdBurnLoop = false;
      }
      this.burnTimer -= Time.deltaTime;
      if ((double) this.burnTimer <= 0.0)
        this.ClearBurn();
    }
    this.playerBurnStartSFXTimer -= Time.deltaTime;
  }

  public void SpreadBurn()
  {
    if (this.team != Health.Team.Team2)
      return;
    for (int index = 0; index < Health.team2.Count; ++index)
    {
      Health health = Health.team2[index];
      if ((bool) (UnityEngine.Object) health && (UnityEngine.Object) health != (UnityEngine.Object) this && !health.IsBurned && (double) Vector3.Distance(this.transform.position, health.transform.position) <= 0.5)
        health.AddBurn((UnityEngine.Object) this.burnAttacker != (UnityEngine.Object) null ? this.burnAttacker : PlayerFarming.Instance.gameObject);
    }
  }

  public void OnDestroy()
  {
    this.ClearElectrified();
    if (this.isPlayer)
    {
      BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(this.ClearBurnParticles);
    }
    else
    {
      if (!((UnityEngine.Object) this.BurnParticles != (UnityEngine.Object) null))
        return;
      this.BurnParticles.transform.localScale = Vector3.one;
      this.BurnParticles.Recycle<ParticleSystem>();
      this.BurnParticles = (ParticleSystem) null;
    }
  }

  public virtual void Heal(float healing)
  {
    if ((double) this.HP < (double) this.totalHP && (double) healing > 0.0)
    {
      float num = this.totalHP - this.HP;
      this.HP += healing;
      healing -= num;
      if ((double) this.HP > (double) this.totalHP)
        this.HP = this.totalHP;
    }
    if ((double) this.SpiritHearts >= (double) this.TotalSpiritHearts || (double) healing <= 0.0)
      return;
    float num1 = this.TotalSpiritHearts - this.SpiritHearts;
    this.SpiritHearts += healing;
    healing -= num1;
    if ((double) this.SpiritHearts <= (double) this.TotalSpiritHearts)
      return;
    this.SpiritHearts = this.TotalSpiritHearts;
  }

  public void FullHeal() => this.Heal(this.totalHP + this.TotalSpiritHearts);

  public void Revive(float healing)
  {
    if ((double) this.HP > 0.0 || (double) healing <= 0.0)
      return;
    this.Heal(healing);
    Interaction_Chest.Instance?.ReviveSpawned(this);
  }

  public void DestroyNextFrame()
  {
    this.StartCoroutine((IEnumerator) this.DestroyNextFrameRoutine());
  }

  public bool ChanceToNegateDamage(float chance)
  {
    return (double) UnityEngine.Random.Range(0.0f, 1f) <= (double) chance;
  }

  public void ChanceToHeal(float chance, float healAmount)
  {
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > (double) chance)
      return;
    this.Heal(healAmount);
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_small");
  }

  public void ChanceToGainBlueHeart(float chance)
  {
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > (double) chance)
      return;
    this.BlueHearts += (float) TrinketManager.GetHealthAmountMultiplier(this.playerFarming);
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_big");
  }

  public IEnumerator SpawnParticle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Health health = this;
    Transform transform1;
    Vector3 LastPos;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      health.Velocity = (transform1.position - LastPos) * 50f;
      BiomeConstants.Instance.EmitParticleChunk(health.typeOfParticle, transform1.position, health.Velocity, 6);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    transform1 = health.transform;
    LastPos = transform1.position;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator emitGroundBloodParticles()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Health health = this;
    Vector3 LastPos;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      health.Velocity = (health.transform.position - LastPos) * 50f;
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(health.transform.position, health.Velocity, health.bloodColor);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    LastPos = health.transform.position;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DestroyNextFrameRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Health health = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) health.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public bool IsAilemented => this.IsPoisoned || this.IsCharmed || this.IsIced;

  public virtual float poisonTickDuration
  {
    get => this.\u003CpoisonTickDuration\u003Ek__BackingField;
    set => this.\u003CpoisonTickDuration\u003Ek__BackingField = value;
  }

  public virtual float playerPoisonDamage => 1f;

  public virtual float enemyPoisonDamage
  {
    get => this.\u003CenemyPoisonDamage\u003Ek__BackingField;
    set => this.\u003CenemyPoisonDamage\u003Ek__BackingField = value;
  }

  public float poisonTimer
  {
    get => this.\u003CpoisonTimer\u003Ek__BackingField;
    set => this.\u003CpoisonTimer\u003Ek__BackingField = value;
  }

  public bool IsPoisoned => this.poisonCounter > 0;

  public ParticleSystem poisonedParticles
  {
    get => this.\u003CpoisonedParticles\u003Ek__BackingField;
    set => this.\u003CpoisonedParticles\u003Ek__BackingField = value;
  }

  public bool PoisonImmune
  {
    get => this.\u003CPoisonImmune\u003Ek__BackingField;
    set => this.\u003CPoisonImmune\u003Ek__BackingField = value;
  }

  public void AddPoison(GameObject attacker, float duration = -1f)
  {
    if (this.IsAilemented)
      return;
    if (this.PoisonImmune || this.isPlayer && TrinketManager.IsPoisonImmune(this.playerFarming) || !this.CanBePoisoned || (double) this.CurrentHP <= 0.0)
    {
      this.ClearPoison();
    }
    else
    {
      if (this.poisonCounter == -1)
      {
        this.poisonCounter = 0;
        this.poisonTimer = 0.0f;
        if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
        {
          UnitObject component = this.GetComponent<UnitObject>();
          this.GetComponent<UnitObject>().SpeedMultiplier = 0.65f;
          foreach (SimpleSpineFlash componentsInChild in component.GetComponentsInChildren<SimpleSpineFlash>())
            componentsInChild.Tint(Color.green);
        }
        Health.StasisEvent onPoisoned = this.OnPoisoned;
        if (onPoisoned != null)
          onPoisoned();
      }
      ++this.poisonCounter;
      this.poisonAttacker = attacker;
      this.PlayPoisonedParticles();
      this.enemyPoisonTimestamp = Time.time + this.enemyPoisonDuration;
      if ((double) duration != -1.0)
      {
        if (this.isPlayer)
          this.StartCoroutine((IEnumerator) this.ClearPoisonAfterTime(duration));
        else
          this.enemyPoisonTimestamp = Time.time + duration;
      }
      if (this.team != Health.Team.Team2 || !((UnityEngine.Object) this.enemyPoisonTicker == (UnityEngine.Object) null) || !(bool) (UnityEngine.Object) this.showHpBar)
        return;
      EnemyStasisTicker.Instantiate(this, new Vector2(this.showHpBar.StasisXOffset, this.showHpBar.zOffset), Health.AttackTypes.Poison, (Action<EnemyStasisTicker>) (r => this.enemyPoisonTicker = r));
    }
  }

  public IEnumerator ClearPoisonAfterTime(float t)
  {
    yield return (object) new WaitForSeconds(t);
    this.ClearPoison();
  }

  public void RemovePoison()
  {
    this.poisonCounter = Mathf.Clamp(this.poisonCounter - 1, 0, int.MaxValue);
    if (this.isPlayer || !(bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
      return;
    this.GetComponent<UnitObject>().SpeedMultiplier = 1f;
  }

  public void ClearPoison()
  {
    if (this.poisonCounter == -1)
      return;
    AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
    this.poisonTimer = 0.0f;
    this.poisonCounter = -1;
    this.poisonAttacker = (GameObject) null;
    this.StopPoisonedParticles();
    if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
    {
      foreach (SimpleSpineFlash componentsInChild in this.GetComponent<UnitObject>().GetComponentsInChildren<SimpleSpineFlash>())
        componentsInChild.Tint(Color.white);
    }
    if ((UnityEngine.Object) this.enemyPoisonTicker != (UnityEngine.Object) null)
    {
      this.enemyPoisonTicker.Hide();
      this.enemyPoisonTicker = (EnemyStasisTicker) null;
    }
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared == null)
      return;
    onStasisCleared();
  }

  public void PlayPoisonedParticles()
  {
    if ((UnityEngine.Object) this.poisonedParticles == (UnityEngine.Object) null)
      return;
    this.poisonedParticles.Play();
  }

  public void StopPoisonedParticles()
  {
    if ((UnityEngine.Object) this.poisonedParticles == (UnityEngine.Object) null)
      return;
    this.poisonedParticles.Stop();
  }

  public float charmTimer
  {
    get => this.\u003CcharmTimer\u003Ek__BackingField;
    set => this.\u003CcharmTimer\u003Ek__BackingField = value;
  }

  public bool IsCharmed => this.charmCounter >= 0;

  public bool IsCharmedEnemy
  {
    get => this.team == Health.Team.PlayerTeam && this.IsCharmed && !this.isPlayerAlly;
  }

  public bool IsCharmedBuffer
  {
    get
    {
      return this.charmCounter >= 0 || (double) Time.time - (double) this.enemyLastCharmTimestamp < 2.0;
    }
  }

  public ParticleSystem charmParticles
  {
    get => this.\u003CcharmParticles\u003Ek__BackingField;
    set => this.\u003CcharmParticles\u003Ek__BackingField = value;
  }

  public bool CharmImmune
  {
    get => this.\u003CCharmImmune\u003Ek__BackingField;
    set => this.\u003CCharmImmune\u003Ek__BackingField = value;
  }

  public void AddCharm()
  {
    if (this.IsAilemented || this.IsImmuneToAllStasis)
      return;
    if (this.CharmImmune || !this.CanBeCharmed || (double) this.CurrentHP <= 0.0)
    {
      this.ClearCharm();
    }
    else
    {
      if (this.charmCounter == -1)
      {
        this.charmCounter = 0;
        this.charmTimer = 0.0f;
        if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
        {
          foreach (SimpleSpineFlash componentsInChild in this.GetComponent<UnitObject>().GetComponentsInChildren<SimpleSpineFlash>())
            componentsInChild.Tint(Color.yellow);
        }
        AudioManager.Instance.PlayOneShot("event:/player/Curses/enemy_charmed", this.transform.position);
        Health.StasisEvent onCharmed = this.OnCharmed;
        if (onCharmed != null)
          onCharmed();
      }
      ++this.charmCounter;
      this.PlayCharmParticles();
      this.enemyCharmTimestamp = Time.time + this.enemyCharmDuration;
      if (this.team == Health.Team.Team2 && (UnityEngine.Object) this.enemyCharmTicker == (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.showHpBar)
        EnemyStasisTicker.Instantiate(this, new Vector2(this.showHpBar.StasisXOffset, this.showHpBar.zOffset), Health.AttackTypes.Charm, (Action<EnemyStasisTicker>) (r => this.enemyCharmTicker = r));
      this.team = Health.Team.PlayerTeam;
      Health.StasisEvent onAddCharm = this.OnAddCharm;
      if (onAddCharm == null)
        return;
      onAddCharm();
    }
  }

  public void ClearCharm()
  {
    if (this.charmCounter == -1)
      return;
    this.charmCounter = -1;
    this.StopCharmParticles();
    this.team = Health.Team.Team2;
    if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
    {
      foreach (SimpleSpineFlash componentsInChild in this.GetComponent<UnitObject>().GetComponentsInChildren<SimpleSpineFlash>())
        componentsInChild.Tint(Color.white);
    }
    if ((UnityEngine.Object) this.enemyCharmTicker != (UnityEngine.Object) null)
    {
      this.enemyCharmTicker.Hide();
      this.enemyCharmTicker = (EnemyStasisTicker) null;
    }
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared == null)
      return;
    onStasisCleared();
  }

  public void PlayCharmParticles()
  {
    if ((UnityEngine.Object) this.charmParticles == (UnityEngine.Object) null)
      return;
    this.charmParticles.Play();
  }

  public void StopCharmParticles()
  {
    if ((UnityEngine.Object) this.charmParticles == (UnityEngine.Object) null)
      return;
    this.charmParticles.Stop();
  }

  public float iceTimer
  {
    get => this.\u003CiceTimer\u003Ek__BackingField;
    set => this.\u003CiceTimer\u003Ek__BackingField = value;
  }

  public bool IsIced => this.iceCounter > 0;

  public ParticleSystem iceParticles
  {
    get => this.\u003CiceParticles\u003Ek__BackingField;
    set => this.\u003CiceParticles\u003Ek__BackingField = value;
  }

  public bool IceImmune
  {
    get => this.\u003CIceImmune\u003Ek__BackingField;
    set => this.\u003CIceImmune\u003Ek__BackingField = value;
  }

  public void AddIce(float duration = -1f)
  {
    if (this.IsAilemented || this.IsImmuneToAllStasis)
      return;
    if (this.IceImmune || this.isPlayer && TrinketManager.IsIceImmune(this.playerFarming) || !this.CanBeIced || (double) this.CurrentHP <= 0.0)
    {
      this.ClearIce();
    }
    else
    {
      if (this.iceCounter == -1)
      {
        this.iceCounter = 0;
        this.iceTimer = 0.0f;
        UnitObject component = this.GetComponent<UnitObject>();
        if (!this.isPlayer && (UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          AudioManager.Instance.PlayOneShot(this.EffectFreezeStartSFX, this.gameObject);
          this.FreezeLoopInstance = AudioManager.Instance.CreateLoop(this.EffectFreezeLoopSFX, this.gameObject, true);
          if (!this.timeFrozen)
            this.ApplyIceSpeed();
          this.OnFreezeTimeCleared += new Health.StasisEvent(this.ApplyIceSpeed);
          foreach (SimpleSpineFlash componentsInChild in component.GetComponentsInChildren<SimpleSpineFlash>())
            componentsInChild.Tint(Color.cyan);
          if ((bool) (UnityEngine.Object) component.rb)
            component.rb.drag *= 2f;
        }
        Health.StasisEvent onIced = this.OnIced;
        if (onIced != null)
          onIced();
      }
      ++this.iceCounter;
      this.PlayIceParticles();
      this.enemyIceTimestamp = Time.time + this.enemyIceDuration;
      if ((double) duration != -1.0)
        this.enemyIceTimestamp = Time.time + duration;
      if (this.team != Health.Team.Team2 || !((UnityEngine.Object) this.enemyIceTicker == (UnityEngine.Object) null) || !(bool) (UnityEngine.Object) this.showHpBar)
        return;
      EnemyStasisTicker.Instantiate(this, new Vector2(this.showHpBar.StasisXOffset, this.showHpBar.zOffset), Health.AttackTypes.Ice, (Action<EnemyStasisTicker>) (r => this.enemyIceTicker = r));
    }
  }

  public void ClearIce()
  {
    if (this.iceCounter == -1)
      return;
    this.iceCounter = -1;
    this.StopIceParticles();
    UnitObject component = this.GetComponent<UnitObject>();
    if (!this.isPlayer && (UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (!this.timeFrozen)
      {
        component.SpeedMultiplier = 1f;
        foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>())
          componentsInChild.timeScale = 1f;
      }
      foreach (SimpleSpineFlash componentsInChild in component.GetComponentsInChildren<SimpleSpineFlash>())
        componentsInChild.Tint(Color.white);
      if ((bool) (UnityEngine.Object) component.rb)
        component.rb.drag /= 2f;
    }
    if ((UnityEngine.Object) this.enemyIceTicker != (UnityEngine.Object) null)
    {
      this.enemyIceTicker.Hide();
      this.enemyIceTicker = (EnemyStasisTicker) null;
    }
    AudioManager.Instance.PlayOneShot(this.EffectFreezeStopSFX, this.gameObject);
    AudioManager.Instance.StopLoop(this.FreezeLoopInstance);
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared != null)
      onStasisCleared();
    this.OnFreezeTimeCleared -= new Health.StasisEvent(this.ApplyIceSpeed);
  }

  public void PlayIceParticles()
  {
    if ((UnityEngine.Object) this.iceParticles == (UnityEngine.Object) null)
      return;
    this.iceParticles.Play();
  }

  public void StopIceParticles()
  {
    if ((UnityEngine.Object) this.iceParticles == (UnityEngine.Object) null)
      return;
    this.iceParticles.Stop();
  }

  public void ApplyIceSpeed()
  {
    UnitObject component = this.GetComponent<UnitObject>();
    component.SpeedMultiplier = 0.1f;
    foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>())
      componentsInChild.timeScale = 0.25f;
  }

  public bool TimeFrozen
  {
    get => this.timeFrozen;
    set => this.timeFrozen = value;
  }

  public void AddFreezeTime(float duration = -1f)
  {
    UnitObject component1 = this.GetComponent<UnitObject>();
    if (!this.isPlayer && !this.timeFrozen)
    {
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.SpeedMultiplier = 0.0001f;
        this.cachedSpineComponentsForFreeze = component1.GetComponentsInChildren<SkeletonAnimation>(true);
        foreach (SkeletonAnimation skeletonAnimation in this.cachedSpineComponentsForFreeze)
        {
          if ((bool) (UnityEngine.Object) skeletonAnimation)
            skeletonAnimation.timeScale = 0.0001f;
        }
        if ((bool) (UnityEngine.Object) component1.rb)
          component1.rb.drag *= 100f;
        component1.health.invincible = false;
        this.timeFrozen = true;
      }
      else
      {
        ITimeFreezable component2 = this.GetComponent<ITimeFreezable>();
        if (component2 != null && (UnityEngine.Object) component2.SkeletonAnimation != (UnityEngine.Object) null)
          component2.SkeletonAnimation.timeScale = 0.0001f;
        this.timeFrozen = true;
      }
    }
    Health.StasisEvent onFreezeTime = this.OnFreezeTime;
    if (onFreezeTime == null)
      return;
    onFreezeTime();
  }

  public void ClearFreezeTime()
  {
    UnitObject component1 = this.GetComponent<UnitObject>();
    if (!this.isPlayer && this.timeFrozen)
    {
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.SpeedMultiplier = 1f;
        foreach (SkeletonAnimation skeletonAnimation in this.cachedSpineComponentsForFreeze)
        {
          if ((bool) (UnityEngine.Object) skeletonAnimation)
            skeletonAnimation.timeScale = 1f;
        }
        if ((bool) (UnityEngine.Object) component1.rb)
          component1.rb.drag /= 100f;
        this.timeFrozen = false;
      }
      else
      {
        ITimeFreezable component2 = this.GetComponent<ITimeFreezable>();
        if (component2 != null && (UnityEngine.Object) component2.SkeletonAnimation != (UnityEngine.Object) null)
          component2.SkeletonAnimation.timeScale = 1f;
        this.timeFrozen = false;
      }
    }
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared != null)
      onStasisCleared();
    Health.StasisEvent freezeTimeCleared = this.OnFreezeTimeCleared;
    if (freezeTimeCleared == null)
      return;
    freezeTimeCleared();
  }

  public void HandleFrozenTime()
  {
    if (PlayerRelic.TimeFrozen)
    {
      if (this.timeFrozen)
        return;
      this.AddFreezeTime();
    }
    else
    {
      if (!this.timeFrozen)
        return;
      this.ClearFreezeTime();
    }
  }

  public float electrifiedTimer
  {
    get => this.\u003CelectrifiedTimer\u003Ek__BackingField;
    set => this.\u003CelectrifiedTimer\u003Ek__BackingField = value;
  }

  public bool IsElectrified => this.electrifiedCounter > 0;

  public bool IsElectrifiedBuffer
  {
    get
    {
      return this.electrifiedCounter > 0 || (double) Time.time - (double) this.enemyLastElectrifiedTimestamp < 2.0;
    }
  }

  public ParticleSystem electrifiedParticles
  {
    get => this.\u003CelectrifiedParticles\u003Ek__BackingField;
    set => this.\u003CelectrifiedParticles\u003Ek__BackingField = value;
  }

  public bool ElectrifiedImmune
  {
    get => this.\u003CElectrifiedImmune\u003Ek__BackingField;
    set => this.\u003CElectrifiedImmune\u003Ek__BackingField = value;
  }

  public virtual float electrifiedTickDuration
  {
    get => this.\u003CelectrifiedTickDuration\u003Ek__BackingField;
    set => this.\u003CelectrifiedTickDuration\u003Ek__BackingField = value;
  }

  public virtual float playerElectrifiedDamage => 1f;

  public virtual float enemyElectrifiedDamage
  {
    get => this.\u003CenemyElectrifiedDamage\u003Ek__BackingField;
    set => this.\u003CenemyElectrifiedDamage\u003Ek__BackingField = value;
  }

  public void AddElectrified(GameObject attacker)
  {
    if (this.IsAilemented || this.IsImmuneToAllStasis)
      return;
    if (this.ElectrifiedImmune || !this.CanBeElectrified || (double) this.CurrentHP <= 0.0)
    {
      this.ClearElectrified();
    }
    else
    {
      if (this.electrifiedCounter == -1)
      {
        this.electrifiedCounter = 0;
        this.electrifiedTimer = 0.0f;
        this.electrifiedAttacker = attacker;
        if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
        {
          UnitObject component = this.GetComponent<UnitObject>();
          component.SpeedMultiplier = 0.1f;
          foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>())
            componentsInChild.timeScale = 0.25f;
          if ((bool) (UnityEngine.Object) component.rb)
            component.rb.drag *= 1.75f;
        }
        AudioManager.Instance.PlayOneShot("event:/player/Curses/enemy_charmed", this.transform.position);
        Health.StasisEvent onElectrified = this.OnElectrified;
        if (onElectrified != null)
          onElectrified();
      }
      ++this.electrifiedCounter;
      this.PlayElectrifiedParticles();
      this.enemyElectrifiedTimestamp = Time.time + this.enemyElectrifiedDuration;
    }
  }

  public void ClearElectrified()
  {
    if (this.electrifiedCounter == -1)
      return;
    this.electrifiedCounter = -1;
    this.StopElectrifiedParticles();
    if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
    {
      UnitObject component = this.GetComponent<UnitObject>();
      foreach (SimpleSpineFlash componentsInChild in component.GetComponentsInChildren<SimpleSpineFlash>())
        componentsInChild.Tint(Color.white);
      component.SpeedMultiplier = 1f;
      foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>(true))
        componentsInChild.timeScale = 1f;
      if ((bool) (UnityEngine.Object) component.rb)
        component.rb.drag /= 1.75f;
    }
    if ((UnityEngine.Object) this.enemyElectrifiedTicker != (UnityEngine.Object) null)
    {
      this.enemyElectrifiedTicker.Hide();
      this.enemyElectrifiedTicker = (EnemyStasisTicker) null;
    }
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared == null)
      return;
    onStasisCleared();
  }

  public void PlayElectrifiedParticles()
  {
    if ((UnityEngine.Object) this.electrifiedParticles == (UnityEngine.Object) null)
      Addressables_wrapper.InstantiateAsync((object) "Assets/Art/Prefabs/CircleAura_Electrified.prefab", this.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        if (!((UnityEngine.Object) this != (UnityEngine.Object) null) || !this.enabled || !((UnityEngine.Object) obj.Result != (UnityEngine.Object) null))
          return;
        this.electrifiedParticles = obj.Result.GetComponent<ParticleSystem>();
        CircleCollider2D component = this.GetComponent<CircleCollider2D>();
        this.electrifiedParticles.transform.localScale *= (UnityEngine.Object) component != (UnityEngine.Object) null ? 1f + component.radius : 1f;
        this.electrifiedParticles.Play();
      }));
    else
      this.electrifiedParticles.Play();
  }

  public void StopElectrifiedParticles()
  {
    if ((UnityEngine.Object) this.electrifiedParticles == (UnityEngine.Object) null)
      return;
    this.electrifiedParticles.Stop();
  }

  public virtual float burnTickDuration
  {
    get => this.\u003CburnTickDuration\u003Ek__BackingField;
    set => this.\u003CburnTickDuration\u003Ek__BackingField = value;
  }

  public virtual float playerBurnDamage => 1f;

  public virtual float enemyBurnDamage
  {
    get => this.\u003CenemyBurnDamage\u003Ek__BackingField;
    set => this.\u003CenemyBurnDamage\u003Ek__BackingField = value;
  }

  public float burnTimer
  {
    get => this.\u003CburnTimer\u003Ek__BackingField;
    set => this.\u003CburnTimer\u003Ek__BackingField = value;
  }

  public float enemyBurnDuration => (float) UnityEngine.Random.Range(3, 6);

  public bool IsBurned => this.burnCounter > 0;

  public ParticleSystem BurnParticles
  {
    get => this.\u003CBurnParticles\u003Ek__BackingField;
    set => this.\u003CBurnParticles\u003Ek__BackingField = value;
  }

  public bool BurnImmune
  {
    get => this.\u003CBurnImmune\u003Ek__BackingField;
    set => this.\u003CBurnImmune\u003Ek__BackingField = value;
  }

  public void AddBurn(GameObject attacker, float duration = -1f, Health.AttackFlags attackFlags = (Health.AttackFlags) 0)
  {
    if (this.IsAilemented || attackFlags.HasFlag((Enum) Health.AttackFlags.Trap) && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, this.playerFarming))
      return;
    if (this.BurnImmune || this.isPlayer && TrinketManager.IsBurnImmune(this.playerFarming) || !this.CanBeBurned || this.isPlayer && this.playerFarming.IsKnockedOut || (double) this.CurrentHP <= 0.0)
    {
      Health.StasisEvent onBurnedFailed = this.OnBurnedFailed;
      if (onBurnedFailed != null)
        onBurnedFailed();
      this.ClearBurn();
    }
    else
    {
      if (this.burnCounter == -1)
      {
        this.burnCounter = 0;
        this.burnTimer = 0.0f;
        if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
        {
          UnitObject component = this.GetComponent<UnitObject>();
          this.GetComponent<UnitObject>().SpeedMultiplier = 1.2f;
          foreach (SimpleSpineFlash componentsInChild in component.GetComponentsInChildren<SimpleSpineFlash>())
            componentsInChild.Tint(Color.red);
        }
        Health.StasisEvent onBurned = this.OnBurned;
        if (onBurned != null)
          onBurned();
      }
      ++this.burnCounter;
      this.burnAttacker = attacker;
      this.PlayBurnParticles();
      if (!this.isPlayer)
      {
        AudioManager.Instance.PlayOneShot(this.EffectBurningStartSFX, this.gameObject);
      }
      else
      {
        if ((double) this.playerBurnStartSFXTimer <= 0.0)
        {
          this.playerBurnStartSFXTimer = 0.5f;
          AudioManager.Instance.PlayOneShot("event:/dlc/combat/fire_damage_tick", this.gameObject);
        }
        this.createdBurnLoop = true;
        this.BurnLoopInstance = AudioManager.Instance.CreateLoop(this.EffectBurningLoopSFX, this.gameObject, true);
        int num = (int) this.BurnLoopInstance.setParameterByName("parameter:/poison", 0.2f);
      }
      this.enemyBurnTimestamp = Time.time + this.enemyBurnDuration;
      if ((double) duration != -1.0)
      {
        if (this.isPlayer)
          this.StartCoroutine((IEnumerator) this.ClearBurnAfterTime(duration));
        else
          this.enemyBurnTimestamp = Time.time + duration;
      }
      if (this.team != Health.Team.Team2 || !((UnityEngine.Object) this.enemyBurnTicker == (UnityEngine.Object) null) || !(bool) (UnityEngine.Object) this.showHpBar)
        return;
      EnemyStasisTicker.Instantiate(this, new Vector2(this.showHpBar.StasisXOffset, this.showHpBar.zOffset), Health.AttackTypes.Burn, (Action<EnemyStasisTicker>) (r => this.enemyBurnTicker = r));
    }
  }

  public IEnumerator ClearBurnAfterTime(float t)
  {
    yield return (object) new WaitForSeconds(t);
    this.ClearBurn();
  }

  public void RemoveBurn()
  {
    this.burnCounter = Mathf.Clamp(this.burnCounter - 1, 0, int.MaxValue);
    if (this.isPlayer || !(bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
      return;
    this.GetComponent<UnitObject>().SpeedMultiplier = 1f;
  }

  public void ClearBurn()
  {
    if (this.burnCounter == -1)
      return;
    AudioManager.Instance.StopLoop(this.BurnLoopInstance);
    this.burnTimer = 0.0f;
    this.burnCounter = -1;
    this.burnAttacker = (GameObject) null;
    this.StopBurnParticles();
    if (!this.isPlayer)
      AudioManager.Instance.PlayOneShot(this.EffectBurningStopSFX, this.gameObject);
    if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
    {
      foreach (SimpleSpineFlash componentsInChild in this.GetComponent<UnitObject>().GetComponentsInChildren<SimpleSpineFlash>())
        componentsInChild.Tint(Color.white);
    }
    if ((UnityEngine.Object) this.enemyBurnTicker != (UnityEngine.Object) null)
    {
      this.enemyBurnTicker.Hide();
      this.enemyBurnTicker = (EnemyStasisTicker) null;
    }
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared == null)
      return;
    onStasisCleared();
  }

  public void PlayBurnParticles()
  {
    this.StopBurnParticles();
    if ((UnityEngine.Object) this.burnParticles != (UnityEngine.Object) null)
    {
      this.BurnParticles = this.burnParticles;
      this.BurnParticles.Play();
    }
    else
      this.BurnParticles = BiomeConstants.Instance.EmitBurnVFX(this.transform);
  }

  public void ClearBurnParticles()
  {
    if (!((UnityEngine.Object) this.BurnParticles != (UnityEngine.Object) null))
      return;
    this.BurnParticles.Clear();
  }

  public void SetDrawBurnParticles(bool state)
  {
    if (!((UnityEngine.Object) this.BurnParticles != (UnityEngine.Object) null))
      return;
    this.BurnParticles.emission.enabled = state;
  }

  public void StopBurnParticles()
  {
    if ((UnityEngine.Object) this.BurnParticles == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.burnParticles != (UnityEngine.Object) null)
    {
      foreach (ParticleSystem componentsInChild in this.BurnParticles.GetComponentsInChildren<ParticleSystem>())
      {
        if ((double) componentsInChild.main.startLifetime.constant > 3.0)
          componentsInChild.Clear();
        componentsInChild.Stop();
      }
    }
    else
    {
      this.BurnParticles.transform.localScale = Vector3.one;
      this.BurnParticles.Recycle<ParticleSystem>();
      this.BurnParticles = (ParticleSystem) null;
    }
  }

  public void ApplyStasisImmunity()
  {
    this.IsImmuneToAllStasis = true;
    this.ClearAllStasisEffects();
  }

  public void ClearAllStasisEffects()
  {
    this.ClearCharm();
    this.ClearIce();
    this.ClearElectrified();
    this.ClearBurn();
  }

  public void ClearStasisImmunity() => this.IsImmuneToAllStasis = false;

  public static void AddToTeam(Health health, Health.Team newTeam)
  {
    if (health.team == Health.Team.Team2 && newTeam == Health.Team.PlayerTeam)
      health.ClearFreezeTime();
    Health.RemoveFromTeam(health, health.team);
    health.team = newTeam;
    switch (newTeam)
    {
      case Health.Team.Neutral:
        if (Health.neutralTeam.Contains(health))
          break;
        Health.neutralTeam.Add(health);
        break;
      case Health.Team.PlayerTeam:
        if (Health.playerTeam.Contains(health))
          break;
        Health.playerTeam.Add(health);
        break;
      case Health.Team.Team2:
        if (Health.team2.Contains(health))
          break;
        Health.team2.Add(health);
        break;
      case Health.Team.DangerousAnimals:
        if (Health.dangerousAnimals.Contains(health))
          break;
        Health.dangerousAnimals.Add(health);
        break;
      case Health.Team.KillAll:
        if (Health.killAll.Contains(health))
          break;
        Health.killAll.Add(health);
        break;
    }
  }

  public static void RemoveFromTeam(Health health, Health.Team team)
  {
    switch (team)
    {
      case Health.Team.Neutral:
        Health.neutralTeam.Remove(health);
        break;
      case Health.Team.PlayerTeam:
        Health.playerTeam.Remove(health);
        break;
      case Health.Team.Team2:
        Health.team2.Remove(health);
        break;
      case Health.Team.DangerousAnimals:
        Health.dangerousAnimals.Remove(health);
        break;
      case Health.Team.KillAll:
        Health.killAll.Remove(health);
        break;
    }
  }

  [CompilerGenerated]
  public static IEnumerator \u003CDealDamage\u003Eg__DamageEnemy\u007C228_1(Health health)
  {
    health.invincible = false;
    BiomeConstants.Instance.ShowTarotCardDamage(health.transform, Vector3.up * 1.5f);
    yield return (object) new WaitForSeconds(0.75f);
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      health.DealDamage(health.HP, health.gameObject, health.transform.position, dealDamageImmediately: true, AttackFlags: Health.AttackFlags.ForceKill);
  }

  [CompilerGenerated]
  public void \u003CAddPoison\u003Eb__293_0(EnemyStasisTicker r) => this.enemyPoisonTicker = r;

  [CompilerGenerated]
  public void \u003CAddCharm\u003Eb__324_0(EnemyStasisTicker r) => this.enemyCharmTicker = r;

  [CompilerGenerated]
  public void \u003CAddIce\u003Eb__348_0(EnemyStasisTicker r) => this.enemyIceTicker = r;

  [CompilerGenerated]
  public void \u003CPlayElectrifiedParticles\u003Eb__399_0(AsyncOperationHandle<GameObject> obj)
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null) || !this.enabled || !((UnityEngine.Object) obj.Result != (UnityEngine.Object) null))
      return;
    this.electrifiedParticles = obj.Result.GetComponent<ParticleSystem>();
    CircleCollider2D component = this.GetComponent<CircleCollider2D>();
    this.electrifiedParticles.transform.localScale *= (UnityEngine.Object) component != (UnityEngine.Object) null ? 1f + component.radius : 1f;
    this.electrifiedParticles.Play();
  }

  [CompilerGenerated]
  public void \u003CAddBurn\u003Eb__435_0(EnemyStasisTicker r) => this.enemyBurnTicker = r;

  public enum CheatMode
  {
    None,
    Immortal,
    Demigod,
    God,
  }

  public delegate void DieAction(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags);

  public delegate void DieAllAction(Health Victim);

  public delegate void HitAction(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false);

  public delegate void HealthEvent(
    GameObject attacker,
    Vector3 attackLocation,
    float damage,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlag);

  public delegate void HealEvent();

  public enum Team
  {
    Neutral,
    PlayerTeam,
    Team2,
    DangerousAnimals,
    KillAll,
  }

  public class DealDamageEvent
  {
    public float UnscaledTimestamp;
    public float Damage;
    public GameObject Attacker;
    public Vector3 AttackLocation;
    public bool BreakBlocking;
    public Health.AttackTypes AttackType;
    public Health.AttackFlags AttackFlags;

    public DealDamageEvent(
      float unscaledTimestamp,
      float damage,
      GameObject attacker,
      Vector3 attackLocation,
      bool breakBlocking,
      Health.AttackTypes attackType,
      Health.AttackFlags AttackFlags = (Health.AttackFlags) 0)
    {
      this.UnscaledTimestamp = unscaledTimestamp;
      this.Damage = damage;
      this.Attacker = attacker;
      this.AttackLocation = attackLocation;
      this.BreakBlocking = breakBlocking;
      this.AttackType = attackType;
      this.AttackFlags = AttackFlags;
    }
  }

  public enum IMPACT_SFX
  {
    NONE,
    IMPACT_BLUNT,
    IMPACT_NORMAL,
    IMPACT_SQUISHY,
    HIT_SMALL,
    HIT_MEDIUM,
    HIT_LARGE,
  }

  public enum DEATH_SFX
  {
    NONE,
    DEATH_SMALL,
    DEATH_MEDIUM,
    DEATH_LARGE,
  }

  public delegate void StasisEvent();

  public enum AttackTypes
  {
    Melee,
    Heavy,
    Projectile,
    Poison,
    NoKnockBack,
    Ice,
    Charm,
    NoHitStop,
    Electrified,
    NoReaction,
    Bullet,
    Burn,
  }

  [Flags]
  public enum AttackFlags
  {
    Crit = 1,
    Skull = 2,
    Poison = 4,
    Ice = 8,
    Charm = 16, // 0x00000010
    DoesntChargeRelics = 32, // 0x00000020
    Electrified = 64, // 0x00000040
    Penetration = 128, // 0x00000080
    NonLethal = 256, // 0x00000100
    Burn = 512, // 0x00000200
    Trap = 1024, // 0x00000400
    ForceKill = 2048, // 0x00000800
  }

  public enum DamageAllEnemiesType
  {
    BlackHeart,
    DeathsDoor,
    Manipulation,
    DamagePerFollower,
    TradeOff,
  }

  public enum HeartEffects
  {
    Black,
    Fire,
    Ice,
  }
}
