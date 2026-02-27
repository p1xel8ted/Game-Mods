// Decompiled with JetBrains decompiler
// Type: Tentacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Tentacle : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  public SpriteRenderer WarningCircle;
  private Health health;
  private StateMachine.State CurrentState;
  private CircleCollider2D CircleCollider2D;
  private const string SHADER_COLOR_NAME = "_Color";
  private bool DoWarning = true;
  private int Order;
  private bool PlaySound;
  private float damage;
  public Health.AttackFlags AttackFlags;
  public static List<Health> TotalDamagedEnemies = new List<Health>();
  public CircleCollider2D DamageCollider;
  public Collider2D UnitLayerCollider;
  private Health EnemyHealth;
  private List<Health> DamagedEnemies = new List<Health>();

  private void OnEnable()
  {
    this.CircleCollider2D = this.GetComponent<CircleCollider2D>();
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.invincible = true;
  }

  private void OnDisable()
  {
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
    bool continousDamage = false)
  {
    this.health.team = Team;
    this.DoWarning = DoWarning;
    this.Order = Order;
    this.PlaySound = PlaySound;
    this.damage = damage;
    if ((bool) (Object) this.Spine)
      this.Spine.gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.Attack(Delay, Duration, continousDamage));
  }

  public void OnDie(
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

  private IEnumerator Die()
  {
    Tentacle tentacle = this;
    tentacle.CurrentState = StateMachine.State.Dieing;
    if ((bool) (Object) tentacle.Spine)
      tentacle.Spine.AnimationState.SetAnimation(0, "out", false);
    yield return (object) new WaitForSeconds(0.6f);
    Object.Destroy((Object) tentacle.gameObject);
  }

  private IEnumerator Attack(float Delay, float Duration, bool continousDamage)
  {
    Tentacle tentacle = this;
    tentacle.UnitLayerCollider.enabled = false;
    tentacle.DamageCollider.enabled = false;
    float Scale = 0.0f;
    tentacle.WarningCircle.transform.localScale = Vector3.zero;
    if (tentacle.DoWarning)
    {
      float WarningDelay = 0.5f;
      while ((double) (WarningDelay -= Time.deltaTime) > 0.0)
      {
        Scale += Time.deltaTime;
        tentacle.WarningCircle.transform.localScale = Vector3.one * Scale;
        if (Time.frameCount % 5 == 0)
          tentacle.WarningCircle.material.SetColor("_Color", tentacle.WarningCircle.material.GetColor("_Color") == Color.red ? Color.white : Color.red);
        yield return (object) null;
      }
      while ((double) (Delay -= Time.deltaTime) > 0.0)
      {
        if (Time.frameCount % 5 == 0)
          tentacle.WarningCircle.material.SetColor("_Color", tentacle.WarningCircle.material.GetColor("_Color") == Color.red ? Color.white : Color.red);
        yield return (object) null;
      }
    }
    else
      yield return (object) new WaitForSeconds(Delay);
    if ((bool) (Object) tentacle.Spine)
    {
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
      while ((double) (IntroDuration -= Time.deltaTime) > 0.0)
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
        t += Time.deltaTime;
        resetEnemiesTimer += Time.deltaTime;
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
        tentacle.Spine.AnimationState.SetAnimation(0, "out", false);
      yield return (object) new WaitForSeconds(0.9f);
      foreach (Health damagedEnemy in tentacle.DamagedEnemies)
        Tentacle.TotalDamagedEnemies.Remove(damagedEnemy);
      Object.Destroy((Object) tentacle.gameObject);
    }
  }

  private void DealDamage()
  {
    foreach (Component component in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.CircleCollider2D.radius))
    {
      this.EnemyHealth = component.gameObject.GetComponent<Health>();
      if ((Object) this.EnemyHealth != (Object) null && this.EnemyHealth.team != this.health.team && !Tentacle.TotalDamagedEnemies.Contains(this.EnemyHealth))
      {
        this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f), AttackType: Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
        this.DamagedEnemies.Add(this.EnemyHealth);
        Tentacle.TotalDamagedEnemies.Add(this.EnemyHealth);
      }
    }
  }
}
