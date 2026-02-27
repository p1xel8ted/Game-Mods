// Decompiled with JetBrains decompiler
// Type: EnemyJellyFish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyJellyFish : UnitObject
{
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float timestamp;
  public float MaximumRange = 5f;
  public float attackPlayerDistance = 2f;
  public float KnockbackModifier = 0.3f;
  public SimpleSpineFlash SimpleSpineFlash;
  public ColliderEvents damageColliderEvents;
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  public int RanDirection = 1;
  public float Angle;
  public float AttackCoolDown;
  public Vector2 AttackCoolDownDuration = new Vector2(1.5f, 2.5f);
  public float SignPostAttackDuration = 0.5f;
  public Health EnemyHealth;

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.StartingPosition.HasValue)
    {
      this.StartingPosition = new Vector3?(this.transform.position);
      this.TargetPosition = this.StartingPosition.Value;
    }
    this.timestamp = !((Object) GameManager.GetInstance() != (Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.turningSpeed += Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += Random.Range(-0.1f, 0.1f);
    this.RanDirection = (double) Random.value < 0.5 ? -1 : 1;
    this.damageColliderEvents.SetActive(false);
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.StartCoroutine(this.ActiveRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((Object) this.damageColliderEvents != (Object) null))
      return;
    this.damageColliderEvents.SetActive(false);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public IEnumerator ActiveRoutine()
  {
    EnemyJellyFish enemyJellyFish = this;
    while (true)
    {
      float turningSpeed = enemyJellyFish.turningSpeed;
      enemyJellyFish.state.LookAngle = enemyJellyFish.state.facingAngle;
      enemyJellyFish.Angle = Mathf.LerpAngle(enemyJellyFish.Angle, Utils.GetAngle(enemyJellyFish.transform.position, enemyJellyFish.TargetPosition), Time.deltaTime * turningSpeed);
      if ((Object) GameManager.GetInstance() != (Object) null && (double) enemyJellyFish.angleNoiseAmplitude > 0.0 && (double) enemyJellyFish.angleNoiseFrequency > 0.0 && (double) enemyJellyFish.MagnitudeFindDistanceBetween(enemyJellyFish.TargetPosition, enemyJellyFish.transform.position) < (double) enemyJellyFish.MaximumRange * (double) enemyJellyFish.MaximumRange)
        enemyJellyFish.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(enemyJellyFish.timestamp) * enemyJellyFish.angleNoiseFrequency, 0.0f) - 0.5f) * enemyJellyFish.angleNoiseAmplitude * (float) enemyJellyFish.RanDirection;
      enemyJellyFish.speed = enemyJellyFish.maxSpeed * enemyJellyFish.SpeedMultiplier;
      enemyJellyFish.state.facingAngle = enemyJellyFish.Angle;
      yield return (object) null;
    }
  }

  public IEnumerator AttackRoutine(bool DestroyOnComplete)
  {
    EnemyJellyFish enemyJellyFish = this;
    float Progress = 0.0f;
    float Duration = enemyJellyFish.SignPostAttackDuration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyJellyFish.SimpleSpineFlash.FlashWhite(Progress / Duration);
      foreach (Health health in Health.team2)
      {
        if ((Object) health != (Object) enemyJellyFish.health && (double) enemyJellyFish.MagnitudeFindDistanceBetween(health.transform.position, enemyJellyFish.transform.position) <= 1.0)
          Progress = Duration;
      }
      yield return (object) null;
    }
    Explosion.CreateExplosion(enemyJellyFish.transform.position + Vector3.back, Health.Team.KillAll, enemyJellyFish.health, 1f, 1f, 5f);
    enemyJellyFish.SimpleSpineFlash.FlashWhite(false);
    yield return (object) new WaitForSeconds(0.2f);
    enemyJellyFish.AttackCoolDown = Random.Range(enemyJellyFish.AttackCoolDownDuration.x, enemyJellyFish.AttackCoolDownDuration.y);
    if (DestroyOnComplete)
      Object.Destroy((Object) enemyJellyFish.gameObject);
    else
      enemyJellyFish.StartCoroutine(enemyJellyFish.ActiveRoutine());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (AttackType != Health.AttackTypes.NoKnockBack)
      this.DoKnockBack(Attacker, this.KnockbackModifier, 0.5f);
    this.AttackCoolDown = Mathf.Min(this.AttackCoolDown, 0.5f);
    this.StartCoroutine(this.HurtRoutine());
    this.SimpleSpineFlash.FlashWhite(false);
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.StopAllCoroutines();
    if (AttackType != Health.AttackTypes.NoKnockBack)
      this.DoKnockBack(Attacker, this.KnockbackModifier, 0.5f);
    this.StartCoroutine(this.AttackRoutine(true));
  }

  public IEnumerator HurtRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellyFish enemyJellyFish = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyJellyFish.DisableForces = false;
      enemyJellyFish.StartingPosition = new Vector3?(enemyJellyFish.transform.position);
      enemyJellyFish.TargetPosition = enemyJellyFish.StartingPosition.Value;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Debug.Log((object) "Start hurt routine");
    enemyJellyFish.damageColliderEvents.SetActive(false);
    enemyJellyFish.ClearPaths();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public void OnDrawGizmos()
  {
    if (this.StartingPosition.HasValue)
      Utils.DrawCircleXY(this.TargetPosition, 0.3f, Color.red);
    if (this.StartingPosition.HasValue)
      Utils.DrawCircleXY(this.TargetPosition, this.MaximumRange, Color.red);
    else
      Utils.DrawCircleXY(this.transform.position, this.MaximumRange, Color.red);
  }
}
