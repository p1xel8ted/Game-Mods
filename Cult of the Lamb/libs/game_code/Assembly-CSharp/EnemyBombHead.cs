// Decompiled with JetBrains decompiler
// Type: EnemyBombHead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyBombHead : UnitObject
{
  public float ExplosionDelay = 0.5f;
  public float SeperationRadius = 0.5f;
  public SimpleSpineAnimator simpleSpineAnimator;
  public SpriteRenderer sprite;
  public const string SHADER_COLOR_NAME = "_Color";
  public Rigidbody2D rb2D;
  public GameObject TargetObject;
  public float Range = 6f;
  public float KnockbackSpeed = 1f;
  public float ExplosionRadius = 1f;
  public List<GameObject> ToSpawn = new List<GameObject>();
  public bool ExplodeOnDeath = true;
  public bool ExplodeToAttack = true;
  public static List<EnemySlime> Slimes = new List<EnemySlime>();
  public float AttackSpeed;
  public Coroutine ChasePlayerCoroutine;
  public float StartSpeed = 0.4f;
  public float Angle;
  public float WhiteFade;
  public List<Collider2D> collider2DList;
  public Collider2D DamageCollider;
  public Health EnemyHealth;

  public void Start()
  {
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb2D = this.GetComponent<Rigidbody2D>();
    this.SeperateObject = true;
  }

  public override void OnEnable() => base.OnEnable();

  public override void OnDisable() => base.OnDisable();

  public IEnumerator WaitForTarget()
  {
    EnemyBombHead enemyBombHead = this;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      enemyBombHead.Seperate(enemyBombHead.SeperationRadius);
      yield return (object) null;
    }
    while ((UnityEngine.Object) enemyBombHead.TargetObject == (UnityEngine.Object) null)
    {
      enemyBombHead.TargetObject = PlayerFarming.FindClosestPlayerGameObject(enemyBombHead.transform.position);
      yield return (object) null;
    }
    while ((double) enemyBombHead.MagnitudeFindDistanceBetween(enemyBombHead.TargetObject.transform.position, enemyBombHead.transform.position) > (double) enemyBombHead.Range * (double) enemyBombHead.Range)
      yield return (object) null;
    enemyBombHead.ChasePlayerCoroutine = enemyBombHead.StartCoroutine((IEnumerator) enemyBombHead.ChasePlayer());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.ClearPaths();
    if (this.ChasePlayerCoroutine != null)
      this.StopCoroutine(this.ChasePlayerCoroutine);
    this.StartCoroutine((IEnumerator) this.AddForce());
    BiomeConstants.Instance.EmitHitVFX(AttackLocation - Vector3.back * 0.5f, Quaternion.identity.z, "HitFX_Weak");
  }

  public IEnumerator AddForce()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBombHead enemyBombHead = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyBombHead.ChasePlayerCoroutine = enemyBombHead.StartCoroutine((IEnumerator) enemyBombHead.ChasePlayer());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyBombHead.simpleSpineAnimator.FlashFillRed();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Dieing)
      return;
    this.Angle = Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f);
    CameraManager.shakeCamera(this.ExplodeOnDeath ? 0.5f : 0.2f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.ClearPaths();
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ExplodeAndSpawn());
  }

  public IEnumerator ExplodeAndSpawn()
  {
    EnemyBombHead enemyBombHead = this;
    yield return (object) new WaitForSeconds(0.05f);
    float angle = enemyBombHead.Angle;
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      float num2 = Utils.GetAngle(allUnit.transform.position, enemyBombHead.transform.position) * ((float) Math.PI / 180f);
      if ((UnityEngine.Object) allUnit != (UnityEngine.Object) enemyBombHead.health && allUnit.team != Health.Team.PlayerTeam && (double) Mathf.Abs(angle - num2) < 1.5707963705062866)
      {
        float distanceBetween = enemyBombHead.MagnitudeFindDistanceBetween(enemyBombHead.transform.position, allUnit.transform.position);
        if ((double) distanceBetween < (double) num1 * (double) num1 && (double) distanceBetween < 10.0)
        {
          health = allUnit;
          num1 = Mathf.Sqrt(distanceBetween);
        }
      }
    }
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      enemyBombHead.Angle = Utils.GetAngle(health.transform.position, enemyBombHead.transform.position) * ((float) Math.PI / 180f);
    enemyBombHead.knockBackVX = (float) (-(double) enemyBombHead.KnockbackSpeed * 1.5) * Mathf.Cos(enemyBombHead.Angle);
    enemyBombHead.knockBackVY = (float) (-(double) enemyBombHead.KnockbackSpeed * 1.5) * Mathf.Sin(enemyBombHead.Angle);
    enemyBombHead.state.CURRENT_STATE = StateMachine.State.Dieing;
    if (enemyBombHead.ExplodeOnDeath)
    {
      while ((double) (enemyBombHead.state.Timer += Time.deltaTime) < 0.5)
      {
        if (Time.frameCount % 5 == 0)
          enemyBombHead.simpleSpineAnimator.FlashWhite(enemyBombHead.simpleSpineAnimator.isFillWhite = !enemyBombHead.simpleSpineAnimator.isFillWhite);
        yield return (object) null;
      }
      Explosion.CreateExplosion(enemyBombHead.transform.position, Health.Team.PlayerTeam, enemyBombHead.health, enemyBombHead.ExplosionRadius, 10f);
    }
    enemyBombHead.SpawnChildren();
    UnityEngine.Object.Destroy((UnityEngine.Object) enemyBombHead.gameObject);
  }

  public void SpawnChildren()
  {
    int index = -1;
    while (++index < this.ToSpawn.Count)
    {
      StateMachine component = UnityEngine.Object.Instantiate<GameObject>(this.ToSpawn[index], this.transform.position, Quaternion.identity, this.transform.parent).GetComponent<StateMachine>();
      component.facingAngle = this.state.facingAngle + 90f + (float) (360 / this.ToSpawn.Count * index);
      Debug.Log((object) component.facingAngle);
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemyBombHead enemyBombHead = this;
    enemyBombHead.state.CURRENT_STATE = StateMachine.State.Idle;
    float RepathTimer = 0.0f;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyBombHead.TargetObject == (UnityEngine.Object) null)
      {
        enemyBombHead.StartCoroutine((IEnumerator) enemyBombHead.WaitForTarget());
        break;
      }
      if (enemyBombHead.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack)
        enemyBombHead.Seperate(enemyBombHead.SeperationRadius);
      switch (enemyBombHead.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector2.Distance((Vector2) enemyBombHead.transform.position, (Vector2) enemyBombHead.TargetObject.transform.position) < (double) enemyBombHead.Range)
          {
            enemyBombHead.givePath(enemyBombHead.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.Moving:
          if (Time.frameCount % 5 == 0)
            enemyBombHead.simpleSpineAnimator.FlashWhite(enemyBombHead.simpleSpineAnimator.isFillWhite = !enemyBombHead.simpleSpineAnimator.isFillWhite);
          if ((double) Vector2.Distance((Vector2) enemyBombHead.transform.position, (Vector2) enemyBombHead.TargetObject.transform.position) < 1.0)
          {
            enemyBombHead.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          if ((double) (RepathTimer += Time.deltaTime) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            enemyBombHead.givePath(enemyBombHead.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemyBombHead.state.facingAngle = Utils.GetAngle(enemyBombHead.transform.position, enemyBombHead.TargetObject.transform.position);
          if (Time.frameCount % 5 == 0)
            enemyBombHead.simpleSpineAnimator.FlashWhite(enemyBombHead.simpleSpineAnimator.isFillWhite = !enemyBombHead.simpleSpineAnimator.isFillWhite);
          if ((double) (enemyBombHead.state.Timer += Time.deltaTime) >= (double) enemyBombHead.ExplosionDelay)
          {
            enemyBombHead.simpleSpineAnimator.FlashWhite(false);
            if (enemyBombHead.ExplodeToAttack)
            {
              Explosion.CreateExplosion(enemyBombHead.transform.position, enemyBombHead.health.team, enemyBombHead.health, enemyBombHead.ExplosionRadius, 1f);
              enemyBombHead.SpawnChildren();
              UnityEngine.Object.Destroy((UnityEngine.Object) enemyBombHead.gameObject);
              break;
            }
            enemyBombHead.state.facingAngle = Utils.GetAngle(enemyBombHead.transform.position, enemyBombHead.TargetObject.transform.position);
            CameraManager.shakeCamera(0.2f, enemyBombHead.state.facingAngle);
            enemyBombHead.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemyBombHead.SeperateObject = false;
            enemyBombHead.AttackSpeed = 0.75f;
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) enemyBombHead.AttackSpeed > 0.0)
          {
            enemyBombHead.WhiteFade = Mathf.Lerp(1f, 0.0f, (float) (1.0 - (double) enemyBombHead.AttackSpeed / 0.75));
            enemyBombHead.simpleSpineAnimator.FillColor(Color.white, enemyBombHead.WhiteFade);
            enemyBombHead.collider2DList = new List<Collider2D>();
            enemyBombHead.DamageCollider.GetContacts((List<Collider2D>) enemyBombHead.collider2DList);
            foreach (Collider2D collider2D in enemyBombHead.collider2DList)
            {
              enemyBombHead.EnemyHealth = collider2D.gameObject.GetComponent<Health>();
              if ((UnityEngine.Object) enemyBombHead.EnemyHealth != (UnityEngine.Object) null && enemyBombHead.EnemyHealth.team != enemyBombHead.health.team)
                enemyBombHead.EnemyHealth.DealDamage(1f, enemyBombHead.gameObject, Vector3.Lerp(enemyBombHead.transform.position, enemyBombHead.EnemyHealth.transform.position, 0.7f));
            }
            enemyBombHead.AttackSpeed -= 3f * Time.deltaTime;
            enemyBombHead.speed = enemyBombHead.AttackSpeed;
          }
          if ((double) (enemyBombHead.state.Timer += Time.deltaTime) >= 1.0)
          {
            enemyBombHead.SeperateObject = true;
            enemyBombHead.state.CURRENT_STATE = StateMachine.State.Idle;
            enemyBombHead.sprite.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.0f));
            break;
          }
          break;
      }
      yield return (object) null;
    }
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }
}
