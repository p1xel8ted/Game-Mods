// Decompiled with JetBrains decompiler
// Type: EnemyBatBurp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyBatBurp : EnemyBat
{
  public int ShotsToFire = 10;
  public float DetectEnemyRange = 10f;
  public GameObject projectilePrefab;
  protected float LookAngle;
  protected float lastBurpedFliesTimestamp;
  protected float minTimeBetweenBurps = 7f;
  protected float chargingTimestamp;
  public float chargingDuration = 0.5f;
  [SerializeField]
  private bool projectileTrail;
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  private LayerMask wallMask;
  [SerializeField]
  private float projectileSpeed;
  [SerializeField]
  private float projectileAcceleration;
  [SerializeField]
  private float projectileMoveSpeed;
  [SerializeField]
  private float distanceBetweenProjectiles;
  [SerializeField]
  private float timeBetweenProjectileTrails;
  private float lastProjectileTrailTime;
  private bool fleeing;
  protected List<Projectile> activeProjectiles = new List<Projectile>();

  protected override IEnumerator ActiveRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      float turningSpeed = enemyBatBurp.turningSpeed;
      if (!enemyBatBurp.ChasingPlayer)
      {
        enemyBatBurp.state.LookAngle = enemyBatBurp.state.facingAngle;
        if (GameManager.RoomActive && (UnityEngine.Object) enemyBatBurp.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.GetClosestTarget().transform.position) < (double) enemyBatBurp.noticePlayerDistance)
        {
          if (!enemyBatBurp.NoticedPlayer)
          {
            if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
              AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
            enemyBatBurp.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
            enemyBatBurp.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
            enemyBatBurp.NoticedPlayer = true;
          }
          enemyBatBurp.maxSpeed = enemyBatBurp.ChaseSpeed;
          enemyBatBurp.ChasingPlayer = true;
        }
      }
      else
      {
        if (!enemyBatBurp.fleeing)
        {
          if ((UnityEngine.Object) enemyBatBurp.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.GetClosestTarget().transform.position) > 12.0)
          {
            enemyBatBurp.TargetPosition = enemyBatBurp.StartingPosition.Value;
            enemyBatBurp.maxSpeed = enemyBatBurp.IdleSpeed;
            enemyBatBurp.ChasingPlayer = false;
          }
          else
            enemyBatBurp.TargetPosition = enemyBatBurp.GetClosestTarget().transform.position;
        }
        enemyBatBurp.state.LookAngle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
        if ((double) (enemyBatBurp.AttackCoolDown -= Time.deltaTime) < 0.0)
        {
          if (!enemyBatBurp.ShouldStartCharging())
          {
            if (!enemyBatBurp.ShouldTrail())
            {
              if (enemyBatBurp.ShouldAttack())
                goto label_18;
            }
            else
              goto label_16;
          }
          else
            break;
        }
      }
      enemyBatBurp.Angle = Mathf.LerpAngle(enemyBatBurp.Angle, Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition), Time.deltaTime * turningSpeed);
      if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) enemyBatBurp.angleNoiseAmplitude > 0.0 && (double) enemyBatBurp.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(enemyBatBurp.TargetPosition, enemyBatBurp.transform.position) < (double) enemyBatBurp.MaximumRange)
        enemyBatBurp.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(enemyBatBurp.timestamp) * enemyBatBurp.angleNoiseFrequency, 0.0f) - 0.5f) * enemyBatBurp.angleNoiseAmplitude * (float) enemyBatBurp.RanDirection;
      if (!enemyBatBurp.useAcceleration)
        enemyBatBurp.speed = enemyBatBurp.maxSpeed * enemyBatBurp.SpeedMultiplier;
      enemyBatBurp.state.facingAngle = enemyBatBurp.Angle;
      yield return (object) null;
    }
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ChargingRoutine());
    yield break;
label_16:
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ProjectileTrailRoutine());
    yield break;
label_18:
    enemyBatBurp.CurrentAttackNum = 0;
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.AttackRoutine());
  }

  protected override IEnumerator ChargingRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    if (!((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null))
    {
      if (enemyBatBurp.Spine.AnimationState != null)
      {
        enemyBatBurp.Spine.AnimationState.SetAnimation(0, "burpcharge", true);
        if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
          AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
      }
      enemyBatBurp.chargingTimestamp = GameManager.GetInstance().CurrentTime;
      while ((double) GameManager.GetInstance().TimeSince(enemyBatBurp.chargingTimestamp) < (double) enemyBatBurp.chargingDuration)
      {
        enemyBatBurp.SimpleSpineFlash.FlashMeWhite();
        enemyBatBurp.speed = Mathf.Lerp(enemyBatBurp.speed, enemyBatBurp.IdleSpeed, Time.deltaTime * 10f);
        enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
        yield return (object) null;
      }
      enemyBatBurp.SimpleSpineFlash.FlashWhite(false);
      if (enemyBatBurp.Spine.AnimationState != null)
      {
        enemyBatBurp.Spine.AnimationState.SetAnimation(0, "burp", true);
        enemyBatBurp.Spine.AnimationState.AddAnimation(0, enemyBatBurp.IdleAnimation, true, 0.0f);
      }
      yield return (object) enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ShootProjectileRoutine());
      enemyBatBurp.AttackCoolDown = 1f;
      enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ActiveRoutine());
    }
  }

  private IEnumerator ShootProjectileRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    enemyBatBurp.speed = enemyBatBurp.IdleSpeed;
    enemyBatBurp.lastBurpedFliesTimestamp = GameManager.GetInstance().CurrentTime;
    enemyBatBurp.AttackCoolDown = UnityEngine.Random.Range(enemyBatBurp.AttackCoolDownDuration.x, enemyBatBurp.AttackCoolDownDuration.y);
    CameraManager.shakeCamera(0.2f, enemyBatBurp.LookAngle);
    List<float> shootAngles = new List<float>(enemyBatBurp.ShotsToFire);
    for (int index = 0; index < enemyBatBurp.ShotsToFire; ++index)
      shootAngles.Add(360f / (float) enemyBatBurp.ShotsToFire * (float) index);
    shootAngles.Shuffle<float>();
    float initAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shootAngles.Count; ++i)
    {
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(enemyBatBurp.projectilePrefab, enemyBatBurp.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyBatBurp.transform.position;
      component.Angle = initAngle + shootAngles[i];
      component.team = enemyBatBurp.health.team;
      component.Speed += UnityEngine.Random.Range(-0.5f, 0.5f);
      component.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
      component.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
      component.LifeTime += UnityEngine.Random.Range(0.0f, 0.3f);
      component.Owner = enemyBatBurp.health;
      component.SetTarget(PlayerFarming.Health);
      enemyBatBurp.activeProjectiles.Add(component);
      yield return (object) new WaitForSeconds(0.03f);
    }
    yield return (object) new WaitForSeconds(0.3f);
  }

  private bool ShouldTrail()
  {
    return (double) GameManager.GetInstance().TimeSince(this.lastProjectileTrailTime) >= (double) this.timeBetweenProjectileTrails && this.projectileTrail;
  }

  private IEnumerator ProjectileTrailRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    enemyBatBurp.Attacking = true;
    enemyBatBurp.fleeing = true;
    enemyBatBurp.maxSpeed = enemyBatBurp.projectileMoveSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.TargetPosition = enemyBatBurp.GetPositionAwayFromPlayer();
    enemyBatBurp.lastProjectileTrailTime = GameManager.GetInstance().CurrentTime;
    float t = 0.0f;
    while ((double) t < 2.0 && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition) > 1.0)
    {
      t += Time.deltaTime;
      enemyBatBurp.state.facingAngle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
      yield return (object) null;
    }
    enemyBatBurp.KnockbackForceModifier = 0.0f;
    enemyBatBurp.TargetPosition = enemyBatBurp.TargetPosition * -1f;
    if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
    enemyBatBurp.Spine.AnimationState.SetAnimation(0, "attackcharge", false);
    enemyBatBurp.Spine.AnimationState.AddAnimation(0, "attack", true, 0.0f);
    enemyBatBurp.Spine.AnimationState.AddAnimation(0, "Fly", true, 0.0f);
    enemyBatBurp.maxSpeed = 0.0f;
    t = 0.0f;
    while ((double) t < 1.1000000238418579)
    {
      t += Time.deltaTime;
      enemyBatBurp.SimpleSpineFlash.FlashWhite((float) ((double) t / 1.1000000238418579 * 0.75));
      yield return (object) null;
    }
    enemyBatBurp.Spine.timeScale = 0.5f;
    enemyBatBurp.maxSpeed = enemyBatBurp.projectileMoveSpeed * 2f * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.SimpleSpineFlash.FlashWhite(false);
    Vector2 previousSpawnPosition = (Vector2) enemyBatBurp.transform.position;
    t = 0.0f;
    while ((double) t < 2.0 && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition) > 1.0)
    {
      t += Time.deltaTime;
      enemyBatBurp.state.facingAngle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
      if ((double) Vector3.Distance(enemyBatBurp.transform.position, (Vector3) previousSpawnPosition) > (double) enemyBatBurp.distanceBetweenProjectiles)
      {
        for (int index = 0; index < 2; ++index)
        {
          Projectile component = ObjectPool.Spawn(enemyBatBurp.bulletPrefab, enemyBatBurp.transform.parent).GetComponent<Projectile>();
          component.transform.position = enemyBatBurp.transform.position;
          component.Angle = Mathf.Repeat(enemyBatBurp.state.facingAngle + (float) (45 * (index == 0 ? -1 : 1)), 360f);
          component.team = enemyBatBurp.health.team;
          component.Speed = enemyBatBurp.projectileSpeed;
          component.Acceleration = enemyBatBurp.projectileAcceleration;
          component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
          component.Owner = enemyBatBurp.health;
        }
        AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", enemyBatBurp.transform.position);
        previousSpawnPosition = (Vector2) enemyBatBurp.transform.position;
      }
      yield return (object) null;
    }
    enemyBatBurp.Spine.timeScale = 1f;
    enemyBatBurp.maxSpeed = enemyBatBurp.IdleSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.KnockbackForceModifier = 1f;
    enemyBatBurp.Attacking = false;
    enemyBatBurp.AttackCoolDown = 1f;
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ActiveRoutine());
  }

  private Vector3 GetPositionAwayFromPlayer()
  {
    List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(1f, 1f), 100f, (int) this.wallMask));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-1f, 1f), 100f, (int) this.wallMask));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(1f, -1f), 100f, (int) this.wallMask));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-1f, -1f), 100f, (int) this.wallMask));
    RaycastHit2D raycastHit2D1 = raycastHit2DList[0];
    for (int index = 1; index < raycastHit2DList.Count; ++index)
    {
      Vector3 position1 = PlayerFarming.Instance.transform.position;
      RaycastHit2D raycastHit2D2 = raycastHit2DList[index];
      Vector3 point1 = (Vector3) raycastHit2D2.point;
      if ((double) Vector3.Distance(position1, point1) > (double) Vector3.Distance(PlayerFarming.Instance.transform.position, (Vector3) raycastHit2D1.point))
      {
        Vector3 position2 = this.transform.position;
        raycastHit2D2 = raycastHit2DList[index];
        Vector3 point2 = (Vector3) raycastHit2D2.point;
        if ((double) Vector3.Distance(position2, point2) > 2.0)
          raycastHit2D1 = raycastHit2DList[index];
      }
    }
    return (Vector3) (raycastHit2D1.point + ((Vector2) this.transform.position - raycastHit2D1.point).normalized);
  }

  protected override bool ShouldStartCharging()
  {
    return (double) GameManager.GetInstance().TimeSince(this.lastBurpedFliesTimestamp) >= (double) this.minTimeBetweenBurps && this.IsPlayerNearby();
  }

  protected override IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBatBurp enemyBatBurp = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyBatBurp.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyBatBurp.DisableForces = true;
    enemyBatBurp.Angle = Utils.GetAngle(Attacker.transform.position, enemyBatBurp.transform.position) * ((float) Math.PI / 180f);
    enemyBatBurp.Force = (Vector3) new Vector2(500f * Mathf.Cos(enemyBatBurp.Angle), 500f * Mathf.Sin(enemyBatBurp.Angle));
    enemyBatBurp.rb.AddForce((Vector2) enemyBatBurp.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private bool IsPlayerNearby()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange)
        return true;
    }
    return false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = 0; index < this.activeProjectiles.Count; ++index)
    {
      if ((UnityEngine.Object) this.activeProjectiles[index] != (UnityEngine.Object) null && this.activeProjectiles[index].gameObject.activeSelf)
        this.activeProjectiles[index].DestroyWithVFX();
    }
    this.activeProjectiles.Clear();
  }
}
