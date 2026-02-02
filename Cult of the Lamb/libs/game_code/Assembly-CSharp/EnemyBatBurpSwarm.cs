// Decompiled with JetBrains decompiler
// Type: EnemyBatBurpSwarm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyBatBurpSwarm : UnitObject
{
  [SerializeField]
  public GameObject projectilePrefab;
  [SerializeField]
  public float anticipation;
  [SerializeField]
  public Vector2 attackCoolDownDuration;
  [SerializeField]
  public float radius;
  [SerializeField]
  public int beamAmount = 8;
  [SerializeField]
  public float beamTimeBetween;
  [SerializeField]
  public Vector2 beamSetTargetTime;
  [SerializeField]
  public int circleAmount = 8;
  [SerializeField]
  public float circleTimeBetween;
  [SerializeField]
  public Vector2 circleSetTargetTime;
  [SerializeField]
  public int scatterAmount = 8;
  [SerializeField]
  public float scatterTimeBetween;
  [SerializeField]
  public Vector2 scatterSetTargetTime;
  [SerializeField]
  public SpriteRenderer spriteRenderer;
  public float attackTimestamp = -1f;
  public bool attacking;
  public List<Projectile> activeProjectiles = new List<Projectile>();
  public EnemyBatBurpSwarm.AttackType previousAttackType;
  public PlayerFarming targetPlayerFarming;

  public override void Update()
  {
    base.Update();
    if (this.ShouldAttack())
      this.Attack();
    if (!GameManager.RoomActive || (double) this.attackTimestamp != -1.0)
      return;
    this.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.attackCoolDownDuration.x, this.attackCoolDownDuration.y);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.attacking = false;
    this.spriteRenderer.color = Color.white;
  }

  public void Attack()
  {
    this.targetPlayerFarming = PlayerFarming.FindClosestPlayer(this.transform.position);
    EnemyBatBurpSwarm.AttackType attackType;
    do
    {
      attackType = (EnemyBatBurpSwarm.AttackType) UnityEngine.Random.Range(0, Enum.GetNames(typeof (EnemyBatBurpSwarm.AttackType)).Length);
    }
    while (attackType == this.previousAttackType);
    switch (attackType)
    {
      case EnemyBatBurpSwarm.AttackType.Beam:
        this.SingleBeam();
        break;
      case EnemyBatBurpSwarm.AttackType.Circle:
        this.Circle();
        break;
      case EnemyBatBurpSwarm.AttackType.Scatter:
        this.Scatter();
        break;
    }
    this.previousAttackType = attackType;
  }

  public bool ShouldAttack()
  {
    if (this.attacking)
      return false;
    float? currentTime = GameManager.GetInstance()?.CurrentTime;
    float attackTimestamp = this.attackTimestamp;
    return (double) currentTime.GetValueOrDefault() > (double) attackTimestamp & currentTime.HasValue;
  }

  public void SingleBeam() => this.StartCoroutine((IEnumerator) this.SingleBeamIE());

  public IEnumerator SingleBeamIE()
  {
    EnemyBatBurpSwarm enemyBatBurpSwarm = this;
    enemyBatBurpSwarm.attacking = true;
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(3f, 1.5f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce));
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(1.75f, 2.25f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce));
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(2f, 2f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine));
    yield return (object) new WaitForSeconds(enemyBatBurpSwarm.anticipation);
    for (int i = 0; i < enemyBatBurpSwarm.beamAmount; ++i)
    {
      enemyBatBurpSwarm.StartCoroutine((IEnumerator) enemyBatBurpSwarm.SpawnProjectile(enemyBatBurpSwarm.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * enemyBatBurpSwarm.radius), Utils.GetAngle(enemyBatBurpSwarm.transform.position, enemyBatBurpSwarm.targetPlayerFarming.transform.position), UnityEngine.Random.Range(enemyBatBurpSwarm.beamSetTargetTime.x, enemyBatBurpSwarm.beamSetTargetTime.y)));
      if ((double) enemyBatBurpSwarm.beamTimeBetween != 0.0)
        yield return (object) new WaitForSeconds(enemyBatBurpSwarm.beamTimeBetween);
    }
    enemyBatBurpSwarm.attacking = false;
    enemyBatBurpSwarm.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyBatBurpSwarm.attackCoolDownDuration.x, enemyBatBurpSwarm.attackCoolDownDuration.y);
  }

  public void Circle() => this.StartCoroutine((IEnumerator) this.CircleIE());

  public IEnumerator CircleIE()
  {
    EnemyBatBurpSwarm enemyBatBurpSwarm = this;
    enemyBatBurpSwarm.attacking = true;
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(3f, 1.5f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce));
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(1.75f, 2.25f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce));
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(2f, 2f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine));
    yield return (object) new WaitForSeconds(enemyBatBurpSwarm.anticipation);
    List<float> shootAngles = new List<float>(enemyBatBurpSwarm.circleAmount);
    for (int index = 0; index < enemyBatBurpSwarm.circleAmount; ++index)
      shootAngles.Add(360f / (float) enemyBatBurpSwarm.circleAmount * (float) index);
    float initAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shootAngles.Count; ++i)
    {
      enemyBatBurpSwarm.StartCoroutine((IEnumerator) enemyBatBurpSwarm.SpawnProjectile(enemyBatBurpSwarm.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * enemyBatBurpSwarm.radius), initAngle + shootAngles[i], UnityEngine.Random.Range(enemyBatBurpSwarm.circleSetTargetTime.x, enemyBatBurpSwarm.circleSetTargetTime.y)));
      if ((double) enemyBatBurpSwarm.circleTimeBetween != 0.0)
        yield return (object) new WaitForSeconds(enemyBatBurpSwarm.circleTimeBetween);
    }
    enemyBatBurpSwarm.attacking = false;
    enemyBatBurpSwarm.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyBatBurpSwarm.attackCoolDownDuration.x, enemyBatBurpSwarm.attackCoolDownDuration.y);
  }

  public void Scatter() => this.StartCoroutine((IEnumerator) this.ScatterIE());

  public IEnumerator ScatterIE()
  {
    EnemyBatBurpSwarm enemyBatBurpSwarm = this;
    enemyBatBurpSwarm.attacking = true;
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(3f, 1.5f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce));
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(1.75f, 2.25f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce));
    s.Append((Tween) enemyBatBurpSwarm.spriteRenderer.transform.DOScale(new Vector3(2f, 2f, 2f), enemyBatBurpSwarm.anticipation / 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine));
    yield return (object) new WaitForSeconds(enemyBatBurpSwarm.anticipation);
    for (int i = 0; i < enemyBatBurpSwarm.scatterAmount; ++i)
    {
      enemyBatBurpSwarm.StartCoroutine((IEnumerator) enemyBatBurpSwarm.SpawnProjectile(enemyBatBurpSwarm.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * enemyBatBurpSwarm.radius), UnityEngine.Random.Range(0.0f, 360f), UnityEngine.Random.Range(enemyBatBurpSwarm.scatterSetTargetTime.x, enemyBatBurpSwarm.scatterSetTargetTime.y)));
      if ((double) enemyBatBurpSwarm.scatterTimeBetween != 0.0)
        yield return (object) new WaitForSeconds(enemyBatBurpSwarm.scatterTimeBetween);
    }
    enemyBatBurpSwarm.attacking = false;
    enemyBatBurpSwarm.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyBatBurpSwarm.attackCoolDownDuration.x, enemyBatBurpSwarm.attackCoolDownDuration.y);
  }

  public IEnumerator SpawnProjectile(Vector3 spawnPosition, float angle, float setTargetTime)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBatBurpSwarm enemyBatBurpSwarm = this;
    Projectile projectile;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      projectile.SetTarget((Health) PlayerFarming.FindClosestPlayer(spawnPosition).health);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    projectile = UnityEngine.Object.Instantiate<GameObject>(enemyBatBurpSwarm.projectilePrefab, enemyBatBurpSwarm.transform.parent).GetComponent<Projectile>();
    projectile.transform.position = spawnPosition;
    projectile.Angle = angle;
    projectile.team = enemyBatBurpSwarm.health.team;
    projectile.Speed += UnityEngine.Random.Range(-0.5f, 0.5f);
    projectile.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    projectile.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    projectile.LifeTime += UnityEngine.Random.Range(0.0f, 0.3f);
    projectile.Owner = enemyBatBurpSwarm.health;
    enemyBatBurpSwarm.activeProjectiles.Add(projectile);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(setTargetTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.spriteRenderer.color = Color.red;
    this.spriteRenderer.DOColor(Color.white, 0.25f);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = this.activeProjectiles.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.activeProjectiles[index] != (UnityEngine.Object) null)
        this.activeProjectiles[index].health.DealDamage(100f, this.gameObject, this.transform.position);
    }
  }

  public enum AttackType
  {
    Beam,
    Circle,
    Scatter,
  }
}
