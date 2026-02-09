// Decompiled with JetBrains decompiler
// Type: EnemySlime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySlime : UnitObject
{
  public bool SleepingOnStart;
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
  public bool MoveOnState = true;
  public bool ExplodeOnDeath = true;
  public bool ExplodeToAttack = true;
  public static List<EnemySlime> Slimes = new List<EnemySlime>();
  public float AttackSpeed;
  public Coroutine ChasePlayerCoroutine;
  public float StartSpeed = 0.4f;
  public float WhiteFade;
  public List<Collider2D> collider2DList;
  public Collider2D DamageCollider;
  public Health EnemyHealth;

  public override void Awake()
  {
    base.Awake();
    this.rb2D = this.GetComponent<Rigidbody2D>();
    this.SeperateObject = true;
  }

  public void OnDieAny(Health Victim)
  {
    if (Victim.team != this.health.team || (double) this.MagnitudeFindDistanceBetween(Victim.transform.position, this.transform.position) >= 16.0)
      return;
    this.WarnMe(Victim.transform.position);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemySlime.Slimes.Add(this);
    Health.OnDieAny += new Health.DieAllAction(this.OnDieAny);
    if (!this.SleepingOnStart)
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    else
      this.StartCoroutine((IEnumerator) this.GoToSleep());
  }

  public IEnumerator GoToSleep()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySlime enemySlime = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemySlime.state.CURRENT_STATE = StateMachine.State.Sleeping;
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

  public override void OnDisable()
  {
    base.OnDisable();
    EnemySlime.Slimes.Remove(this);
    Health.OnDieAny -= new Health.DieAllAction(this.OnDieAny);
  }

  public void ScreamToOthers() => this.StartCoroutine((IEnumerator) this.DoScremToOthers());

  public void WarnMe(Vector3 Position)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Sleeping)
      return;
    this.state.facingAngle = Utils.GetAngle(this.transform.position, Position);
    this.ScreamToOthers();
  }

  public IEnumerator DoScremToOthers()
  {
    EnemySlime enemySlime = this;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));
    enemySlime.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForSeconds(0.3f);
    foreach (EnemySlime slime in EnemySlime.Slimes)
    {
      if ((double) enemySlime.MagnitudeFindDistanceBetween(enemySlime.transform.position, slime.transform.position) < 16.0)
        slime.WarnMe(enemySlime.transform.position);
    }
    yield return (object) new WaitForSeconds(1.2f);
    enemySlime.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySlime.ChasePlayerCoroutine = enemySlime.StartCoroutine((IEnumerator) enemySlime.ChasePlayer());
  }

  public IEnumerator WaitForTarget()
  {
    EnemySlime enemySlime = this;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      if (enemySlime.MoveOnState && (double) enemySlime.StartSpeed > 0.0)
      {
        enemySlime.StartSpeed -= 1f * Time.deltaTime;
        enemySlime.speed = enemySlime.StartSpeed;
      }
      enemySlime.Seperate(enemySlime.SeperationRadius);
      yield return (object) null;
    }
    while ((UnityEngine.Object) enemySlime.TargetObject == (UnityEngine.Object) null)
    {
      enemySlime.TargetObject = PlayerFarming.FindClosestPlayerGameObject(enemySlime.transform.position);
      yield return (object) null;
    }
    while ((double) enemySlime.MagnitudeFindDistanceBetween(enemySlime.TargetObject.transform.position, enemySlime.transform.position) > (double) enemySlime.Range * (double) enemySlime.Range)
      yield return (object) null;
    enemySlime.ChasePlayerCoroutine = enemySlime.StartCoroutine((IEnumerator) enemySlime.ChasePlayer());
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
    if (this.state.CURRENT_STATE == StateMachine.State.Sleeping)
    {
      this.simpleSpineAnimator.FlashFillRed();
      this.ScreamToOthers();
    }
    else
    {
      this.ClearPaths();
      if (this.ChasePlayerCoroutine != null)
        this.StopCoroutine(this.ChasePlayerCoroutine);
      this.StartCoroutine((IEnumerator) this.AddForce());
    }
    BiomeConstants.Instance.EmitHitVFX(AttackLocation - Vector3.back * 0.5f, Quaternion.identity.z, "HitFX_Weak");
  }

  public IEnumerator AddForce()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySlime enemySlime = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemySlime.ChasePlayerCoroutine = enemySlime.StartCoroutine((IEnumerator) enemySlime.ChasePlayer());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemySlime.simpleSpineAnimator.FlashFillRed();
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
    if (this.state.CURRENT_STATE != StateMachine.State.Sleeping)
    {
      foreach (EnemySlime slime in EnemySlime.Slimes)
      {
        if ((double) this.MagnitudeFindDistanceBetween(this.transform.position, slime.transform.position) < 16.0)
          slime.WarnMe(this.transform.position);
      }
    }
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    GameObject gameObject = BiomeConstants.Instance.GroundSmash_Medium.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    CameraManager.shakeCamera(this.ExplodeOnDeath ? 0.5f : 0.2f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.ClearPaths();
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ExplodeAndSpawn());
  }

  public IEnumerator ExplodeAndSpawn()
  {
    EnemySlime enemySlime = this;
    enemySlime.state.CURRENT_STATE = StateMachine.State.Dieing;
    if (enemySlime.ExplodeOnDeath)
    {
      while ((double) (enemySlime.state.Timer += Time.deltaTime) < 1.0)
      {
        if (Time.frameCount % 5 == 0)
          enemySlime.simpleSpineAnimator.FlashWhite(enemySlime.simpleSpineAnimator.isFillWhite = !enemySlime.simpleSpineAnimator.isFillWhite);
        yield return (object) null;
      }
      Explosion.CreateExplosion(enemySlime.transform.position, enemySlime.health.team, enemySlime.health, enemySlime.ExplosionRadius, 1f);
    }
    enemySlime.SpawnChildren();
    UnityEngine.Object.Destroy((UnityEngine.Object) enemySlime.gameObject);
  }

  public void SpawnChildren()
  {
    int index = -1;
    while (++index < this.ToSpawn.Count)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ToSpawn[index], this.transform.position, Quaternion.identity, this.transform.parent);
      StateMachine component1 = gameObject.GetComponent<StateMachine>();
      EnemySlime component2 = gameObject.GetComponent<EnemySlime>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.MoveOnState = true;
      component1.facingAngle = this.state.facingAngle + 90f + (float) (360 / this.ToSpawn.Count * index);
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemySlime enemySlime = this;
    enemySlime.state.CURRENT_STATE = StateMachine.State.Idle;
    float RepathTimer = 0.0f;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemySlime.TargetObject == (UnityEngine.Object) null)
      {
        enemySlime.StartCoroutine((IEnumerator) enemySlime.WaitForTarget());
        break;
      }
      if (enemySlime.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack)
        enemySlime.Seperate(enemySlime.SeperationRadius);
      switch (enemySlime.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector2.Distance((Vector2) enemySlime.transform.position, (Vector2) enemySlime.TargetObject.transform.position) < (double) enemySlime.Range)
          {
            enemySlime.givePath(enemySlime.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.Moving:
          if ((double) Vector2.Distance((Vector2) enemySlime.transform.position, (Vector2) enemySlime.TargetObject.transform.position) < (enemySlime.ExplodeToAttack ? 1.0 : 2.5))
          {
            enemySlime.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          if ((double) (RepathTimer += Time.deltaTime) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            enemySlime.givePath(enemySlime.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemySlime.state.facingAngle = Utils.GetAngle(enemySlime.transform.position, enemySlime.TargetObject.transform.position);
          if (Time.frameCount % 5 == 0)
            enemySlime.simpleSpineAnimator.FlashWhite(enemySlime.simpleSpineAnimator.isFillWhite = !enemySlime.simpleSpineAnimator.isFillWhite);
          if ((double) (enemySlime.state.Timer += Time.deltaTime) >= (enemySlime.ExplodeToAttack ? 1.0 : 0.5))
          {
            enemySlime.simpleSpineAnimator.FlashWhite(false);
            if (enemySlime.ExplodeToAttack)
            {
              Explosion.CreateExplosion(enemySlime.transform.position, enemySlime.health.team, enemySlime.health, enemySlime.ExplosionRadius, 1f);
              enemySlime.SpawnChildren();
              UnityEngine.Object.Destroy((UnityEngine.Object) enemySlime.gameObject);
              break;
            }
            enemySlime.state.facingAngle = Utils.GetAngle(enemySlime.transform.position, enemySlime.TargetObject.transform.position);
            CameraManager.shakeCamera(0.2f, enemySlime.state.facingAngle);
            enemySlime.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemySlime.SeperateObject = false;
            enemySlime.AttackSpeed = 0.75f;
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) enemySlime.AttackSpeed > 0.0)
          {
            enemySlime.WhiteFade = Mathf.Lerp(1f, 0.0f, (float) (1.0 - (double) enemySlime.AttackSpeed / 0.75));
            enemySlime.simpleSpineAnimator.FillColor(Color.white, enemySlime.WhiteFade);
            enemySlime.collider2DList = new List<Collider2D>();
            enemySlime.DamageCollider.GetContacts((List<Collider2D>) enemySlime.collider2DList);
            foreach (Collider2D collider2D in enemySlime.collider2DList)
            {
              enemySlime.EnemyHealth = collider2D.gameObject.GetComponent<Health>();
              if ((UnityEngine.Object) enemySlime.EnemyHealth != (UnityEngine.Object) null && enemySlime.EnemyHealth.team != enemySlime.health.team)
                enemySlime.EnemyHealth.DealDamage(1f, enemySlime.gameObject, Vector3.Lerp(enemySlime.transform.position, enemySlime.EnemyHealth.transform.position, 0.7f));
            }
            enemySlime.AttackSpeed -= 3f * Time.deltaTime;
            enemySlime.speed = enemySlime.AttackSpeed;
          }
          if ((double) (enemySlime.state.Timer += Time.deltaTime) >= 1.0)
          {
            enemySlime.SeperateObject = true;
            enemySlime.state.CURRENT_STATE = StateMachine.State.Idle;
            enemySlime.sprite.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.0f));
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
