// Decompiled with JetBrains decompiler
// Type: Health
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using MMRoomGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

#nullable disable
public class Health : BaseMonoBehaviour
{
  [HideInInspector]
  public StateMachine state;
  public bool Unaware;
  [HideInInspector]
  public bool InStealthCover;
  public bool SlowMoOnkill;
  public bool HasShield;
  public bool WasJustParried;
  public bool IgnoreProjectiles;
  public Health.CheatMode GodMode;
  public UnityEvent OnHitCallback;
  public UnityEvent OnDieCallback;
  public Health.Team team;
  public static List<Health> allUnits = new List<Health>();
  public static List<Health> neutralTeam = new List<Health>();
  public static List<Health> playerTeam = new List<Health>();
  public static List<Health> team2 = new List<Health>();
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
  [FormerlySerializedAs("_SpiritHearts")]
  protected float _TotalSpiritHearts;
  [SerializeField]
  protected float _SpiritHearts;
  public bool ArmoredFront;
  public bool invincible;
  public bool isPlayer;
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
  public bool CanBePoisoned = true;
  public bool CanBeIced = true;
  public bool CanBeCharmed = true;
  public bool ImmuneToDiseasedHearts;
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
  public bool OnHitBlockAttacker;
  public GameObject AttackerToBlock;
  [HideInInspector]
  public Health.DealDamageEvent damageEventQueue;
  public static float DealDamageForgivenessWindow = 0.04f;
  public float autoAimAttractionFactor = 1f;
  public Health.IMPACT_SFX ImpactSoundToPlay;
  public Health.DEATH_SFX DeathSoundToPlay;
  public bool IgnoreLocationHPBuff;
  public bool BlackSoulOnHit;
  private Vector3 Velocity;
  protected int poisonCounter = -1;
  protected float enemyPoisonDuration = 5f;
  protected float enemyPoisonTimestamp = -1f;
  private GameObject poisonAttacker;
  private EnemyStasisTicker enemyPoisonTicker;
  private EventInstance PoisonLoopInstance;
  private bool createdPoisonLoop;
  protected int charmCounter = -1;
  protected float enemyCharmDuration = 5f;
  protected float enemyCharmTimestamp = -1f;
  protected float enemyLastCharmTimestamp = -1f;
  private EnemyStasisTicker enemyCharmTicker;
  private EventInstance CharmLoopInstance;
  private bool createdCharmLoop;
  protected int iceCounter = -1;
  protected float enemyIceDuration = 3f;
  protected float enemyIceTimestamp = -1f;
  private EnemyStasisTicker enemyIceTicker;
  private EventInstance IceLoopInstance;
  private bool createdIceLoop;

  public event Health.DieAction OnDie;

  public static event Health.DieAllAction OnDieAny;

  public event Health.HitAction OnHit;

  public event Health.HitAction OnHitEarly;

  public event Health.HitAction OnPoisonedHit;

  public event Health.HealthEvent OnDamaged;

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

  public event Health.StasisEvent OnStasisCleared;

  public event Health.StasisEvent OnPoisoned;

  public event Health.StasisEvent OnIced;

  public event Health.StasisEvent OnCharmed;

  private void Awake()
  {
    this.InitHP();
    this.ClearPoison();
    this.ClearIce();
    this.ClearCharm();
  }

  public virtual void OnEnable()
  {
    switch (this.team)
    {
      case Health.Team.Neutral:
        if (!Health.neutralTeam.Contains(this))
        {
          Health.neutralTeam.Add(this);
          break;
        }
        break;
      case Health.Team.Team2:
        if (!Health.team2.Contains(this))
        {
          Health.team2.Add(this);
          break;
        }
        break;
      case Health.Team.DangerousAnimals:
        Health.dangerousAnimals.Add(this);
        break;
      case Health.Team.KillAll:
        Health.killAll.Add(this);
        break;
      default:
        Health.playerTeam.Add(this);
        break;
    }
    if (!Health.allUnits.Contains(this))
      Health.allUnits.Add(this);
    this.state = this.GetComponent<StateMachine>();
    if ((UnityEngine.Object) this.AttackerToBlock == (UnityEngine.Object) null)
      this.AttackerToBlock = this.gameObject;
    this.ClearPoison();
    this.ClearIce();
    this.ClearCharm();
  }

  public virtual void InitHP()
  {
    if (this.isPlayer)
      return;
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
      }
      if (GameManager.DungeonEndlessLevel > 0)
      {
        MiniBossController componentInParent = this.GetComponentInParent<MiniBossController>();
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.EnemiesToTrack.Count > 0 && (UnityEngine.Object) componentInParent.EnemiesToTrack[0] == (UnityEngine.Object) this)
          this.totalHP += (float) (10 * (GameManager.DungeonEndlessLevel - 1));
        else
          this.totalHP += (float) (5 * (GameManager.DungeonEndlessLevel - 1));
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

  protected virtual void OnDisable()
  {
    if (this.isPlayer)
      AudioManager.Instance.StopLoop(this.PoisonLoopInstance);
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

  private IEnumerator DamageAllEnemiesIE(float damage, Health.DamageAllEnemiesType damageType)
  {
    Health health1 = this;
    foreach (Health health2 in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if (!((UnityEngine.Object) health2 == (UnityEngine.Object) null) && !(bool) (UnityEngine.Object) health2.GetComponentInParent<Projectile>() && !health2.ImmuneToDiseasedHearts)
      {
        switch (damageType)
        {
          case Health.DamageAllEnemiesType.BlackHeart:
            BiomeConstants.Instance.ShowBlackHeartDamage(health2.transform, Vector3.up);
            continue;
          case Health.DamageAllEnemiesType.DeathsDoor:
            BiomeConstants.Instance.ShowTarotCardDamage(health2.transform, Vector3.up);
            continue;
          default:
            continue;
        }
      }
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (Health health3 in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if ((UnityEngine.Object) health3 != (UnityEngine.Object) null)
        health3.DealDamage(damage, health1.gameObject, health1.transform.position, AttackType: Health.AttackTypes.NoKnockBack);
    }
  }

  public virtual bool DealDamage(
    float Damage,
    GameObject Attacker,
    Vector3 AttackLocation,
    bool BreakBlocking = false,
    Health.AttackTypes AttackType = Health.AttackTypes.Melee,
    bool dealDamageImmediately = false,
    Health.AttackFlags AttackFlags = (Health.AttackFlags) 0)
  {
    if (!this.enabled || this.invincible || this.untouchable || this.GodMode == Health.CheatMode.God || (UnityEngine.Object) this.state != (UnityEngine.Object) null && !dealDamageImmediately && (this.state.CURRENT_STATE == StateMachine.State.Dodging || this.state.CURRENT_STATE == StateMachine.State.InActive) || (UnityEngine.Object) Attacker == (UnityEngine.Object) this.gameObject && !this.isPlayer && this.IsCharmedBuffer || this.isPlayer && (this.state.CURRENT_STATE == StateMachine.State.CustomAnimation || PlayerFarming.Instance.GoToAndStopping))
      return false;
    if (this.isPlayer && !DataManager.Instance.ShownDodgeTutorial && DataManager.Instance.ShownDodgeTutorialCount < 3)
    {
      UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/UI/UI Control Prompt Dodge") as GameObject, GameObject.FindWithTag("Canvas").transform).GetComponent<UIDodgePromptTutorial>().Play(Attacker);
      return false;
    }
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
      this.Velocity = AttackLocation - Attacker.transform.position;
    if (this.isPlayer)
    {
      if (dealDamageImmediately)
      {
        MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) this);
        this.damageEventQueue = (Health.DealDamageEvent) null;
      }
      if (!dealDamageImmediately)
      {
        if (this.damageEventQueue != null)
          return false;
        this.damageEventQueue = new Health.DealDamageEvent(Time.unscaledTime, Damage, Attacker, AttackLocation, BreakBlocking, AttackType);
        return true;
      }
      PlayerWeapon.EquippedWeaponsInfo currentWeapon = PlayerFarming.Instance.GetComponent<PlayerWeapon>().GetCurrentWeapon();
      bool flag1 = false;
      bool flag2 = this.ChanceToNegateDamage(currentWeapon.NegateDamageChance + TrinketManager.GetNegateDamageChance()) || flag1;
      if (TrinketManager.CanNegateDamage() && this.state.CURRENT_STATE == StateMachine.State.Heal || flag2)
      {
        this.NegatedDamage(AttackLocation);
        return false;
      }
      if ((double) this.HP == 1.0)
      {
        float num = DifficultyManager.GetChanceOfNegatingDeath() + (PlayerWeapon.FirstTimeUsingWeapon ? 0.2f : 0.0f);
        if ((double) UnityEngine.Random.Range(0.0f, 1f) <= (double) num)
          return false;
      }
      PlayerFarming.Instance.GetBlackSoul(TrinketManager.GetBlackSoulsOnDamaged(), false);
      if (TrinketManager.DropBlackGoopOnDamaged())
        TrapGoop.CreateGoop(this.transform.position, 5, 0.5f, GenerateRoom.Instance.transform);
      PlayerFleeceManager.ResetDamageModifier();
    }
    if (this.OnHitEarly != null)
      this.OnHitEarly(Attacker, AttackLocation, AttackType);
    Health.HealthEvent onDamaged = this.OnDamaged;
    if (onDamaged != null)
      onDamaged(Attacker, AttackLocation, Damage);
    Damage *= this.DamageModifier;
    switch (this.ImpactSoundToPlay)
    {
      case Health.IMPACT_SFX.IMPACT_BLUNT:
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_blunt", this.transform.position);
        break;
      case Health.IMPACT_SFX.IMPACT_NORMAL:
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_normal", this.transform.position);
        break;
      case Health.IMPACT_SFX.IMPACT_SQUISHY:
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.transform.position);
        break;
      case Health.IMPACT_SFX.HIT_SMALL:
        AudioManager.Instance.PlayOneShot("event:/enemy/gethit_small", this.transform.position);
        break;
      case Health.IMPACT_SFX.HIT_MEDIUM:
        AudioManager.Instance.PlayOneShot("event:/enemy/gethit_medium", this.transform.position);
        break;
      case Health.IMPACT_SFX.HIT_LARGE:
        AudioManager.Instance.PlayOneShot("event:/enemy/gethit_large", this.transform.position);
        break;
    }
    if (AttackType == Health.AttackTypes.Projectile)
      Damage *= this.ArrowAttackVulnerability;
    if (AttackType == Health.AttackTypes.Melee)
      Damage *= this.MeleeAttackVulnerability;
    float angle1 = Utils.GetAngle(this.transform.position, AttackLocation);
    if (this.HasShield)
      BiomeConstants.Instance.EmitBlockImpact(this.transform.position, angle1);
    Damage *= !this.HasShield || AttackType == Health.AttackTypes.Heavy ? 1f : 0.1f;
    if ((double) Damage > 0.0)
    {
      if (this.isPlayer)
      {
        DataManager.Instance.PlayerDamageReceived += Damage;
        DataManager.Instance.PlayerDamageReceivedThisRun += Damage;
      }
      else if (this.team == Health.Team.Team2 && (UnityEngine.Object) Attacker == (UnityEngine.Object) PlayerFarming.Instance.gameObject)
      {
        DataManager.Instance.PlayerDamageDealtThisRun += Damage;
        DataManager.Instance.PlayerDamageDealt += Damage;
      }
    }
    if (!BreakBlocking && (UnityEngine.Object) this.state != (UnityEngine.Object) null && this.state.CURRENT_STATE == StateMachine.State.Defending)
    {
      GameObject gameObject = BiomeConstants.Instance.HitFX_Blocked.Spawn();
      gameObject.transform.position = AttackLocation;
      gameObject.transform.rotation = Quaternion.identity;
      Damage = 0.0f;
    }
    if (this.team == Health.Team.PlayerTeam)
    {
      float num = DungeonModifier.HasNegativeModifier(DungeonNegativeModifier.DoubleDamage, 2f, 1f) + PlayerFleeceManager.GetDamageReceivedMultiplier();
      Damage *= Mathf.Clamp(num, 1f, 2f);
    }
    if (this.BlackSoulOnHit && (UnityEngine.Object) Attacker != (UnityEngine.Object) null && (bool) (UnityEngine.Object) Attacker.GetComponent<PlayerFarming>() && AttackType == Health.AttackTypes.Melee && this.team == Health.Team.Team2 && this.gameObject.tag != "Projectile")
    {
      BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(UnityEngine.Random.Range(1f, 2f) * TrinketManager.GetBlackSoulsMultiplier()), this.transform.position, true, true);
      if ((bool) (UnityEngine.Object) blackSoul)
        blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(2f, 4f));
    }
    if ((double) this.BlackHearts > 0.0 && (double) Damage > 0.0)
    {
      float blackHearts = this.BlackHearts;
      this.BlackHearts -= Damage;
      Damage -= blackHearts;
      if ((double) this.BlackHearts < 0.0)
        this.BlackHearts = 0.0f;
      this.StartCoroutine((IEnumerator) this.DamageAllEnemiesIE(1.25f + DataManager.GetWeaponDamageMultiplier(DataManager.Instance.CurrentWeaponLevel), Health.DamageAllEnemiesType.BlackHeart));
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
    if ((double) Damage > 0.0)
      this.HP -= Damage;
    if (this.team != Health.Team.Neutral && !this.IsPoisoned)
    {
      if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Poison))
      {
        if (AttackType == Health.AttackTypes.Projectile)
          this.enemyPoisonDamage = 0.2f * (float) DataManager.Instance.CurrentCurseLevel;
        this.AddPoison(Attacker);
      }
      else if ((AttackType == Health.AttackTypes.Melee || AttackType == Health.AttackTypes.Heavy) && (UnityEngine.Object) EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon) != (UnityEngine.Object) null && EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).ContainsAttachmentType(AttachmentEffect.Poison) && !this.isPlayer && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).GetAttachment(AttachmentEffect.Poison).poisonChance)
      {
        this.enemyPoisonDamage = 0.2f * (float) DataManager.Instance.CurrentWeaponLevel;
        this.AddPoison(Attacker);
      }
    }
    this.HP = Mathf.Clamp(this.HP, 0.0f, float.MaxValue);
    bool flag = (double) this.HP + (double) this.BlueHearts + (double) this.SpiritHearts + (double) this.BlackHearts <= 0.0 && this.GodMode != Health.CheatMode.Immortal;
    if (!flag)
    {
      bool FromBehind = false;
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && (UnityEngine.Object) Attacker != (UnityEngine.Object) null && (UnityEngine.Object) this.transform != (UnityEngine.Object) null)
        FromBehind = (double) Mathf.Abs(this.state.facingAngle - Utils.GetAngle(this.transform.position, Attacker.transform.position) % 360f) >= 150.0;
      if (AttackType == Health.AttackTypes.Poison)
      {
        this.GetComponent<ShowHPBar>()?.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
        UIBossHUD.Instance?.OnBossHit(Attacker, AttackLocation, AttackType, FromBehind);
        Health.HitAction onPoisonedHit = this.OnPoisonedHit;
        if (onPoisonedHit != null)
          onPoisonedHit(Attacker, AttackLocation, Health.AttackTypes.Poison, FromBehind);
      }
      if (AttackType != Health.AttackTypes.Poison || this.isPlayer)
      {
        Health.HitAction onHit = this.OnHit;
        if (onHit != null)
          onHit(Attacker, AttackLocation, AttackType, FromBehind);
        this.OnHitCallback?.Invoke();
      }
    }
    if (AttackType == Health.AttackTypes.Heavy && this.HasShield)
      this.HasShield = false;
    Vector3 vector3_1 = this.transform.position - AttackLocation;
    float angle2 = Utils.GetAngle(this.transform.position, AttackLocation);
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
    {
      StateMachine component = Attacker.GetComponent<StateMachine>();
      if (this.gameObject.tag != "Player")
      {
        if (this.InanimateObject)
        {
          if (this.InanimateObjectEffect)
            BiomeConstants.Instance.EmitHitVFX(this.transform.position + Vector3.back, Quaternion.identity.z, "HitFX_Weak");
        }
        else if (this.ImpactOnHit && this.team != Health.Team.Neutral)
        {
          if (!AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit) || this.InanimateObject)
          {
            BiomeConstants.Instance.PlayerEmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, (UnityEngine.Object) component != (UnityEngine.Object) null ? component.facingAngle : angle2, false, this.ImpactOnHitColor, this.ImpactOnHitScale, AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit));
          }
          else
          {
            GameManager.GetInstance().HitStop(0.2f);
            BiomeConstants.Instance.PlayerEmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, (UnityEngine.Object) component != (UnityEngine.Object) null ? component.facingAngle : angle2, crit: AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit));
          }
        }
      }
      else if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit))
      {
        if (!flag)
          BiomeConstants.Instance.PlayerEmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, (UnityEngine.Object) component != (UnityEngine.Object) null ? component.facingAngle : angle2, crit: AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit));
        else
          BiomeConstants.Instance.PlayerEmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, (UnityEngine.Object) component != (UnityEngine.Object) null ? component.facingAngle : angle2, false, crit: AttackFlags.HasFlag((Enum) Health.AttackFlags.Crit));
      }
      if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Charm))
        this.AddCharm();
      else if (AttackFlags.HasFlag((Enum) Health.AttackFlags.Ice))
        this.AddIce();
    }
    if (this.BloodOnHit)
    {
      Vector3 vector3_2 = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
      if (this.gameObject.tag != "Player" && !flag)
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
    if (this.ScreenshakeOnHit && !(this.ScreenshakeOnDie & flag))
      CameraManager.shakeCamera(this.ScreenShakeOnDieIntensity / 3f);
    if (this.isPlayer && (double) PlayerFarming.Instance.health.HP + (double) PlayerFarming.Instance.health.BlueHearts + (double) PlayerFarming.Instance.health.BlackHearts + (double) PlayerFarming.Instance.health.SpiritHearts <= 1.0 && TrinketManager.HasTrinket(TarotCards.Card.DeathsDoor))
      this.StartCoroutine((IEnumerator) this.DamageAllEnemiesIE((float) TrinketManager.GetDamageAllEnemiesAmount(TarotCards.Card.DeathsDoor) + DataManager.GetWeaponDamageMultiplier(DataManager.Instance.CurrentWeaponLevel), Health.DamageAllEnemiesType.DeathsDoor));
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && this.team == Health.Team.Team2 && AttackType == Health.AttackTypes.Projectile && (bool) (UnityEngine.Object) Attacker.GetComponentInParent<MegaSlash>())
    {
      if (DataManager.Instance.CurrentCurse == EquipmentType.MegaSlash_Ice && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.MegaSlash_Ice).Chance)
        this.AddIce();
      else if (DataManager.Instance.CurrentCurse == EquipmentType.MegaSlash_Charm && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.MegaSlash_Charm).Chance)
        this.AddCharm();
    }
    if (flag)
    {
      if (this.team == Health.Team.Team2)
      {
        ++DataManager.Instance.PlayerKillsOnRun;
        ++DataManager.Instance.KillsInGame;
        if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && ((bool) (UnityEngine.Object) Attacker.GetComponentInParent<PlayerFarming>() || (bool) (UnityEngine.Object) Attacker.GetComponentInParent<MegaSlash>()) && (AttackType == Health.AttackTypes.Melee && EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).ContainsAttachmentType(AttachmentEffect.Necromancy) && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).GetAttachment(AttachmentEffect.Necromancy).necromancyChance || AttackType == Health.AttackTypes.Projectile && DataManager.Instance.CurrentCurse == EquipmentType.MegaSlash_Necromancy))
          ProjectileGhost.SpawnGhost(this.transform.position, 1f + DataManager.GetWeaponDamageMultiplier(DataManager.Instance.CurrentWeaponLevel));
      }
      switch (this.DeathSoundToPlay)
      {
        case Health.DEATH_SFX.DEATH_SMALL:
          AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_small", this.transform.position);
          break;
        case Health.DEATH_SFX.DEATH_MEDIUM:
          AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_medium", this.transform.position);
          break;
        case Health.DEATH_SFX.DEATH_LARGE:
          AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_large", this.transform.position);
          break;
      }
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      if (DungeonModifier.HasNegativeModifier(DungeonNegativeModifier.DropPoison) && this.team == Health.Team.Team2)
        TrapPoison.CreatePoison(this.transform.position, 5, 0.5f, this.transform.parent);
      if (this.SmokeOnDie && !this.isPlayer)
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
      if (this.BloodOnDie)
      {
        if (!this.isPlayer)
          BiomeConstants.Instance.EmitBloodSplatter(this.transform.position, vector3_1.normalized, this.bloodColor);
        BiomeConstants.Instance.EmitBloodSplatterGroundParticles(this.transform.position, this.Velocity, this.bloodColor);
        BiomeConstants.Instance.EmitBloodImpact(this.transform.position + Vector3.back * 0.5f, angle2, "black", useDeltaTime: !this.isPlayer);
        string[] strArray = new string[2]
        {
          "BloodImpact_Large_0",
          "BloodImpact_Large_1"
        };
        int index = UnityEngine.Random.Range(0, 1);
        BiomeConstants.Instance.EmitBloodImpact(this.transform.position + Vector3.back * 0.5f, angle2, "black", strArray[index], !this.isPlayer);
      }
      if (this.spawnParticles)
        BiomeConstants.Instance.EmitParticleChunk(this.typeOfParticle, this.transform.position, this.Velocity, 6);
      if (this.ScreenshakeOnDie)
        CameraManager.shakeCamera(this.ScreenShakeOnDieIntensity);
      if (this.EmitGroundSmashDecal)
        BiomeConstants.Instance.EmitGroundSmashVFXParticles(new Vector3(this.transform.position.x, this.transform.position.y, 0.0f));
      this.HP = 0.0f;
      Health health = (Health) null;
      if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
        health = Attacker.GetComponent<Health>();
      if ((bool) (UnityEngine.Object) health && health.team == Health.Team.PlayerTeam && this.team == Health.Team.Team2)
      {
        PlayerWeapon.EquippedWeaponsInfo currentWeapon = PlayerFarming.Instance.GetComponent<PlayerWeapon>().GetCurrentWeapon();
        health.ChanceToHeal(currentWeapon.HealChance + TrinketManager.GetHealChance(), 1f);
        if ((double) PlayerFarming.Instance.health.BlueHearts < 4.0)
          health.ChanceToGainBlueHeart(TrinketManager.GetChanceOfGainingBlueHeart());
        PlayerFleeceManager.IncrementDamageModifier();
        if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && AttackType == Health.AttackTypes.Projectile && (bool) (UnityEngine.Object) Attacker.GetComponentInParent<MegaSlash>() && DataManager.Instance.CurrentCurse == EquipmentType.MegaSlash_Ice && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.MegaSlash_Ice).Chance)
          this.AddIce();
      }
      else if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null && ((bool) (UnityEngine.Object) Attacker.GetComponentInParent<MegaSlash>() || (bool) (UnityEngine.Object) Attacker.GetComponentInParent<BlastPush>()) && this.team == Health.Team.Team2)
        PlayerFleeceManager.IncrementDamageModifier();
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

  private void NegatedDamage(Vector3 attackPosition)
  {
    BiomeConstants.Instance.EmitBlockImpact(this.transform.position, Utils.GetAngle(this.transform.position, attackPosition));
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

  protected virtual void Update()
  {
    if (this.damageEventQueue != null && (double) GameManager.GetInstance().UnscaledTimeSince(this.damageEventQueue.UnscaledTimestamp) >= (double) Health.DealDamageForgivenessWindow)
    {
      if ((UnityEngine.Object) this.damageEventQueue.Attacker != (UnityEngine.Object) null)
        this.DealDamage(this.damageEventQueue.Damage, this.damageEventQueue.Attacker, this.damageEventQueue.AttackLocation, this.damageEventQueue.BreakBlocking, this.damageEventQueue.AttackType, true);
      else
        this.damageEventQueue = (Health.DealDamageEvent) null;
    }
    this.WasJustParried = false;
    this.PoisonCalculate();
    this.CharmCalculate();
    this.IceCalculate();
  }

  private void PoisonCalculate()
  {
    if ((double) this.HP + (double) this.BlueHearts + (double) this.SpiritHearts + (double) this.BlackHearts > 0.0 && this.poisonCounter > 0)
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
        this.poisonTimer = 0.0f;
      }
      if ((double) this.enemyPoisonTimestamp == -1.0 || (double) Time.time <= (double) this.enemyPoisonTimestamp || this.team == Health.Team.PlayerTeam)
        return;
      this.poisonCounter = 0;
    }
    else
    {
      if ((double) this.HP + (double) this.BlueHearts + (double) this.BlackHearts + (double) this.SpiritHearts <= 0.0 || this.poisonCounter != 0)
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

  private void IceCalculate()
  {
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

  private void CharmCalculate()
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
    this.BlueHearts += (float) TrinketManager.GetHealthAmountMultiplier();
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_big");
  }

  private IEnumerator SpawnParticle()
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

  private IEnumerator emitGroundBloodParticles()
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

  private IEnumerator DestroyNextFrameRoutine()
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

  public virtual float poisonTickDuration { get; set; } = 1f;

  protected virtual float playerPoisonDamage => 1f;

  public virtual float enemyPoisonDamage { get; set; } = 0.3f;

  public float poisonTimer { get; set; }

  public bool IsPoisoned => this.poisonCounter > 0;

  public ParticleSystem poisonedParticles { get; set; }

  public bool PoisonImmune { get; set; }

  public void AddPoison(GameObject attacker)
  {
    if (this.IsAilemented)
      return;
    if (this.PoisonImmune || this.isPlayer && TrinketManager.IsPoisonImmune() || !this.CanBePoisoned)
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
      if (this.team != Health.Team.Team2 || !((UnityEngine.Object) this.enemyPoisonTicker == (UnityEngine.Object) null))
        return;
      ShowHPBar component1 = this.GetComponent<ShowHPBar>();
      if (!(bool) (UnityEngine.Object) component1)
        return;
      EnemyStasisTicker.Instantiate(this, new Vector2(component1.StasisXOffset, component1.zOffset), Health.AttackTypes.Poison, (Action<EnemyStasisTicker>) (r => this.enemyPoisonTicker = r));
    }
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

  public float charmTimer { get; set; }

  public bool IsCharmed => this.charmCounter > 0;

  public bool IsCharmedBuffer
  {
    get
    {
      return this.charmCounter > 0 || (double) Time.time - (double) this.enemyLastCharmTimestamp < 2.0;
    }
  }

  public ParticleSystem charmParticles { get; set; }

  public bool CharmImmune { get; set; }

  public void AddCharm()
  {
    if (this.IsAilemented)
      return;
    if (this.CharmImmune || !this.CanBeCharmed)
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
      if (this.team == Health.Team.Team2 && (UnityEngine.Object) this.enemyCharmTicker == (UnityEngine.Object) null)
      {
        ShowHPBar component = this.GetComponent<ShowHPBar>();
        if ((bool) (UnityEngine.Object) component)
          EnemyStasisTicker.Instantiate(this, new Vector2(component.StasisXOffset, component.zOffset), Health.AttackTypes.Charm, (Action<EnemyStasisTicker>) (r => this.enemyCharmTicker = r));
      }
      this.team = Health.Team.PlayerTeam;
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

  public float iceTimer { get; set; }

  public bool IsIced => this.iceCounter > 0;

  public ParticleSystem iceParticles { get; set; }

  public bool IceImmune { get; set; }

  public void AddIce()
  {
    if (this.IsAilemented)
      return;
    if (this.IceImmune || !this.CanBeIced)
    {
      this.ClearIce();
    }
    else
    {
      if (this.iceCounter == -1)
      {
        this.iceCounter = 0;
        this.iceTimer = 0.0f;
        if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
        {
          UnitObject component = this.GetComponent<UnitObject>();
          component.SpeedMultiplier = 0.1f;
          foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>())
            componentsInChild.timeScale = 0.25f;
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
      if (this.team != Health.Team.Team2 || !((UnityEngine.Object) this.enemyIceTicker == (UnityEngine.Object) null))
        return;
      ShowHPBar component1 = this.GetComponent<ShowHPBar>();
      if (!(bool) (UnityEngine.Object) component1)
        return;
      EnemyStasisTicker.Instantiate(this, new Vector2(component1.StasisXOffset, component1.zOffset), Health.AttackTypes.Ice, (Action<EnemyStasisTicker>) (r => this.enemyIceTicker = r));
    }
  }

  public void ClearIce()
  {
    if (this.iceCounter == -1)
      return;
    this.iceCounter = -1;
    this.StopIceParticles();
    if (!this.isPlayer && (bool) (UnityEngine.Object) this.GetComponent<UnitObject>())
    {
      UnitObject component = this.GetComponent<UnitObject>();
      component.SpeedMultiplier = 1f;
      foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>())
        componentsInChild.timeScale = 1f;
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
    Health.StasisEvent onStasisCleared = this.OnStasisCleared;
    if (onStasisCleared == null)
      return;
    onStasisCleared();
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

  public delegate void HealthEvent(GameObject attacker, Vector3 attackLocation, float damage);

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

    public DealDamageEvent(
      float unscaledTimestamp,
      float damage,
      GameObject attacker,
      Vector3 attackLocation,
      bool breakBlocking,
      Health.AttackTypes attackType)
    {
      this.UnscaledTimestamp = unscaledTimestamp;
      this.Damage = damage;
      this.Attacker = attacker;
      this.AttackLocation = attackLocation;
      this.BreakBlocking = breakBlocking;
      this.AttackType = attackType;
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
  }

  [Flags]
  public enum AttackFlags
  {
    Crit = 1,
    Skull = 2,
    Poison = 4,
    Ice = 8,
    Charm = 16, // 0x00000010
  }

  public enum DamageAllEnemiesType
  {
    BlackHeart,
    DeathsDoor,
    Manipulation,
  }
}
