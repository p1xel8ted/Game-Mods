// Decompiled with JetBrains decompiler
// Type: Tentacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Tentacle : UnitObject, ISpellOwning
{
  public SkeletonAnimation Spine;
  public SpriteRenderer WarningCircle;
  public GameObject owner;
  public StateMachine.State CurrentState;
  public CircleCollider2D CircleCollider2D;
  public const string SHADER_COLOR_NAME = "_Color";
  public bool DoWarning = true;
  public int Order;
  public bool PlaySound;
  public float damage;
  public Health.AttackFlags AttackFlags;
  [CompilerGenerated]
  public bool \u003CShootsProjectiles\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CTimeBetweenProjectiles\u003Ek__BackingField = 8f;
  public float shootTimestamp;
  public string customTriggerSFX;
  public string customExitSFX;
  public string customLoopSFX;
  public bool playLoop;
  public bool playExitSFX;
  public static List<Health> TotalDamagedEnemies = new List<Health>();
  public float timeBeforeNextShot;
  public CircleCollider2D DamageCollider;
  public Collider2D UnitLayerCollider;
  public Health EnemyHealth;
  public EventInstance loop;
  public List<Health> DamagedEnemies = new List<Health>();

  public bool ShootsProjectiles
  {
    get => this.\u003CShootsProjectiles\u003Ek__BackingField;
    set => this.\u003CShootsProjectiles\u003Ek__BackingField = value;
  }

  public float TimeBetweenProjectiles
  {
    get => this.\u003CTimeBetweenProjectiles\u003Ek__BackingField;
    set => this.\u003CTimeBetweenProjectiles\u003Ek__BackingField = value;
  }

  public new void OnEnable()
  {
    this.CircleCollider2D = this.GetComponent<CircleCollider2D>();
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.invincible = true;
    this.shootTimestamp = Time.time;
  }

  public new void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.loop);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    Object.Destroy((Object) this.gameObject);
  }

  public void Play(
    float Delay,
    float Duration,
    float damage,
    Health.Team Team,
    bool DoWarning,
    int Order,
    bool PlaySound,
    bool continousDamage = false,
    string customTriggerSFX = "",
    string customLoopSFX = "",
    string customExitSFX = "",
    bool playExitSFX = true,
    bool playLoop = true)
  {
    this.health.team = Team;
    this.health.SetTeam(Team);
    this.DoWarning = DoWarning;
    this.Order = Order;
    this.PlaySound = PlaySound;
    this.damage = damage;
    this.customTriggerSFX = customTriggerSFX;
    this.customExitSFX = customExitSFX;
    this.customLoopSFX = customLoopSFX;
    this.playLoop = playLoop;
    this.playExitSFX = playExitSFX;
    if ((bool) (Object) this.Spine)
      this.Spine.gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.Attack(Delay, Duration, continousDamage));
  }

  public new void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.CurrentState == StateMachine.State.Dieing)
      return;
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Die());
    this.CircleCollider2D.enabled = false;
  }

  public new void Update()
  {
    if ((double) (this.timeBeforeNextShot += Time.deltaTime * ((Object) this.Spine != (Object) null ? this.Spine.timeScale : 1f)) < (double) this.TimeBetweenProjectiles || !this.ShootsProjectiles)
      return;
    this.StartCoroutine((IEnumerator) this.FireSpiralProjectiles());
    this.timeBeforeNextShot = 0.0f;
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", this.gameObject);
    if (!(bool) (Object) this.Spine)
      return;
    AudioManager.Instance.PlayOneShot(string.IsNullOrEmpty(this.customTriggerSFX) ? "event:/relics/tentacle_slime" : this.customTriggerSFX, this.Spine.gameObject);
    this.Spine.AnimationState.SetAnimation(0, "attack", false).Delay = 0.0f;
    this.Spine.AnimationState.AddAnimation(0, "idle" + (Random.Range(0, 2) == 0 ? "2" : ""), true, 0.0f);
  }

  public IEnumerator FireSpiralProjectiles()
  {
    Tentacle tentacle = this;
    PlayerFarming playerFarming = PlayerFarming.GetPlayerFarmingComponent(tentacle.owner);
    int TotalProjectiles = 8;
    int i = -1;
    while (++i < TotalProjectiles)
    {
      Projectile.CreatePlayerProjectiles(1, tentacle.health, tentacle.transform.position, "Assets/Prefabs/Enemies/Weapons/ArrowPlayer.prefab", 16f, PlayerWeapon.GetDamage(0.5f, playerFarming.currentWeaponLevel, playerFarming), 360f / (float) TotalProjectiles * (float) i, attackFlags: tentacle.AttackFlags);
      float t = 0.0f;
      while ((double) (t += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) <= 0.10000000149011612)
        yield return (object) null;
    }
  }

  public IEnumerator Die()
  {
    Tentacle tentacle = this;
    tentacle.CurrentState = StateMachine.State.Dieing;
    if ((bool) (Object) tentacle.Spine)
    {
      TrackEntry trackEntry = tentacle.Spine.AnimationState.SetAnimation(0, "out", false);
      if (tentacle.playExitSFX)
        AudioManager.Instance.PlayOneShot(string.IsNullOrEmpty(tentacle.customExitSFX) ? "event:/relics/tentacle_exit" : tentacle.customExitSFX, tentacle.gameObject);
      yield return (object) new WaitForSeconds(trackEntry.Animation.Duration);
      tentacle.Spine.gameObject.SetActive(false);
    }
    float t = 0.0f;
    while ((double) (t += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) <= 0.60000002384185791)
      yield return (object) null;
    Object.Destroy((Object) tentacle.gameObject);
  }

  public IEnumerator Attack(float Delay, float Duration, bool continousDamage)
  {
    Tentacle tentacle = this;
    tentacle.UnitLayerCollider.enabled = false;
    tentacle.DamageCollider.enabled = false;
    float Scale = 0.0f;
    tentacle.WarningCircle.transform.localScale = Vector3.zero;
    float flashTickTimer;
    if (tentacle.DoWarning)
    {
      float WarningDelay = 0.5f;
      flashTickTimer = 0.0f;
      while ((double) (WarningDelay -= Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) > 0.0)
      {
        Scale += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f);
        tentacle.WarningCircle.transform.localScale = Vector3.one * Scale;
        if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
        {
          tentacle.WarningCircle.material.SetColor("_Color", tentacle.WarningCircle.material.GetColor("_Color") == Color.red ? Color.white : Color.red);
          flashTickTimer = 0.0f;
        }
        flashTickTimer += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f);
        yield return (object) null;
      }
      while ((double) (Delay -= Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) > 0.0)
      {
        if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
        {
          tentacle.WarningCircle.material.SetColor("_Color", tentacle.WarningCircle.material.GetColor("_Color") == Color.red ? Color.white : Color.red);
          flashTickTimer = 0.0f;
        }
        flashTickTimer += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f);
        yield return (object) null;
      }
    }
    else
    {
      flashTickTimer = 0.0f;
      while ((double) (flashTickTimer += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) < (double) Delay)
        yield return (object) null;
    }
    if ((bool) (Object) tentacle.Spine)
    {
      if (tentacle.playLoop)
        tentacle.loop = AudioManager.Instance.CreateLoop(string.IsNullOrEmpty(tentacle.customLoopSFX) ? "event:/relics/cursed_fire" : tentacle.customLoopSFX, tentacle.gameObject, true);
      tentacle.Spine.gameObject.SetActive(true);
      tentacle.Spine.AnimationState.SetAnimation(0, "intro", false);
      tentacle.Spine.AnimationState.AddAnimation(0, "idle" + (Random.Range(0, 2) == 0 ? "2" : ""), true, 0.0f);
      tentacle.Spine.AnimationState.TimeScale = 2f;
    }
    tentacle.health.invincible = false;
    if ((double) Duration != -1.0)
    {
      CameraManager.shakeCamera(0.2f, (float) Random.Range(0, 360));
      float IntroDuration = 0.2f;
      tentacle.DamageCollider.enabled = true;
      tentacle.DamagedEnemies = new List<Health>();
      while ((double) (IntroDuration -= Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) > 0.0)
      {
        Scale -= Time.deltaTime * 2f;
        if ((double) Scale >= 0.0)
          tentacle.WarningCircle.transform.localScale = Vector3.one * Scale;
        else
          tentacle.WarningCircle.gameObject.SetActive(false);
        yield return (object) null;
      }
      if ((bool) (Object) tentacle.Spine)
        tentacle.Spine.AnimationState.TimeScale = 1f;
      tentacle.UnitLayerCollider.enabled = true;
      tentacle.DealDamage();
      float t = 0.0f;
      float resetEnemiesTimer = 0.0f;
      while ((double) t < (double) Duration)
      {
        t += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f);
        resetEnemiesTimer += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f);
        if (continousDamage)
        {
          if ((double) resetEnemiesTimer > 1.0)
          {
            foreach (Health damagedEnemy in tentacle.DamagedEnemies)
              Tentacle.TotalDamagedEnemies.Remove(damagedEnemy);
            tentacle.DamagedEnemies.Clear();
            resetEnemiesTimer = 0.0f;
          }
          tentacle.DealDamage();
        }
        yield return (object) null;
      }
      tentacle.UnitLayerCollider.enabled = false;
      tentacle.DamageCollider.enabled = false;
      if ((bool) (Object) tentacle.Spine)
      {
        AudioManager.Instance.StopLoop(tentacle.loop);
        if (tentacle.playExitSFX)
          AudioManager.Instance.PlayOneShot("event:/relics/tentacle_exit", tentacle.gameObject);
        yield return (object) new WaitForSeconds(tentacle.Spine.AnimationState.SetAnimation(0, "out", false).Animation.Duration);
        tentacle.Spine.gameObject.SetActive(false);
      }
      float cleanupDelay = 0.0f;
      while ((double) (cleanupDelay += Time.deltaTime * ((Object) tentacle.Spine != (Object) null ? tentacle.Spine.timeScale : 1f)) < 0.89999997615814209)
        yield return (object) null;
      foreach (Health damagedEnemy in tentacle.DamagedEnemies)
        Tentacle.TotalDamagedEnemies.Remove(damagedEnemy);
      Object.Destroy((Object) tentacle.gameObject);
    }
  }

  public void DealDamage()
  {
    foreach (Component component in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.CircleCollider2D.radius))
    {
      this.EnemyHealth = component.gameObject.GetComponent<Health>();
      if ((Object) this.EnemyHealth != (Object) null && this.EnemyHealth.team != this.health.team && !Tentacle.TotalDamagedEnemies.Contains(this.EnemyHealth))
      {
        if ((bool) (Object) this.Spine && (this.EnemyHealth.team == Health.Team.Team2 || this.EnemyHealth.team == Health.Team.Neutral))
        {
          AudioManager.Instance.PlayOneShot("event:/relics/tentacle_slime", this.Spine.gameObject);
          this.Spine.AnimationState.SetAnimation(0, "attack", false);
          this.Spine.AnimationState.AddAnimation(0, "idle" + (Random.Range(0, 2) == 0 ? "2" : ""), true, 0.0f);
        }
        this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f), AttackType: Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
        this.DamagedEnemies.Add(this.EnemyHealth);
        Tentacle.TotalDamagedEnemies.Add(this.EnemyHealth);
      }
    }
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;
}
