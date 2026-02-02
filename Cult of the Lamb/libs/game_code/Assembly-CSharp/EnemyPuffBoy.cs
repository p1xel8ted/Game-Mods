// Decompiled with JetBrains decompiler
// Type: EnemyPuffBoy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyPuffBoy : UnitObject
{
  public float SeperationRadius = 0.5f;
  public GameObject TargetObject;
  public float Range = 6f;
  public float KnockbackSpeed = 0.6f;
  public Collider2D DamageCollider;
  public List<Collider2D> collider2DList;
  public Health EnemyHealth;
  public SimpleSpineAnimator simpleSpineAnimator;
  public GameObject SpawnSlime;
  public GameObject SlimeChild;
  public float SpawnDelay;
  public Coroutine ChasePlayerCoroutine;
  public float StartSpeed = 0.4f;

  public void Start()
  {
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.SeperateObject = true;
  }

  public override void OnEnable() => base.OnEnable();

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    this.StopAllCoroutines();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyPuffBoy enemyPuffBoy = this;
    while ((UnityEngine.Object) enemyPuffBoy.TargetObject == (UnityEngine.Object) null)
    {
      enemyPuffBoy.TargetObject = PlayerFarming.FindClosestPlayerGameObject(enemyPuffBoy.transform.position);
      yield return (object) null;
    }
    while ((double) Vector3.Distance(enemyPuffBoy.TargetObject.transform.position, enemyPuffBoy.transform.position) > (double) enemyPuffBoy.Range)
      yield return (object) null;
    enemyPuffBoy.ChasePlayerCoroutine = enemyPuffBoy.StartCoroutine((IEnumerator) enemyPuffBoy.ChasePlayer());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (AttackType == Health.AttackTypes.Projectile)
    {
      this.simpleSpineAnimator.FlashFillRed();
      this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
      GameManager.GetInstance().HitStop();
      this.simpleSpineAnimator.Animate("hurt", 0, false);
      this.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    }
    else
    {
      this.simpleSpineAnimator.FlashFillRed();
      CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
      if (this.state.CURRENT_STATE == StateMachine.State.SignPostAttack)
        return;
      this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      this.simpleSpineAnimator.Animate("attack", 0, false);
      this.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    }
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
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  public IEnumerator ChasePlayer()
  {
    EnemyPuffBoy enemyPuffBoy = this;
    enemyPuffBoy.state.CURRENT_STATE = StateMachine.State.Idle;
    float RepathTimer = 0.0f;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyPuffBoy.TargetObject == (UnityEngine.Object) null)
      {
        enemyPuffBoy.StartCoroutine((IEnumerator) enemyPuffBoy.WaitForTarget());
        break;
      }
      enemyPuffBoy.Seperate(enemyPuffBoy.SeperationRadius);
      switch (enemyPuffBoy.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector2.Distance((Vector2) enemyPuffBoy.transform.position, (Vector2) enemyPuffBoy.TargetObject.transform.position) < (double) enemyPuffBoy.Range)
          {
            enemyPuffBoy.givePath(enemyPuffBoy.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.Moving:
          if ((double) Vector2.Distance((Vector2) enemyPuffBoy.transform.position, (Vector2) enemyPuffBoy.TargetObject.transform.position) < 1.0)
          {
            enemyPuffBoy.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            enemyPuffBoy.simpleSpineAnimator.Animate("attack", 0, false);
            enemyPuffBoy.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
            break;
          }
          if (DataManager.Instance.PLAYER_ARROW_AMMO <= 0 && (UnityEngine.Object) enemyPuffBoy.SlimeChild == (UnityEngine.Object) null && (double) (enemyPuffBoy.SpawnDelay -= Time.deltaTime) < 0.0)
          {
            enemyPuffBoy.SlimeChild = UnityEngine.Object.Instantiate<GameObject>(enemyPuffBoy.SpawnSlime, enemyPuffBoy.transform.position, Quaternion.identity, enemyPuffBoy.transform.parent);
            enemyPuffBoy.SpawnDelay = 3f;
          }
          if ((double) (RepathTimer += Time.deltaTime) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            enemyPuffBoy.givePath(enemyPuffBoy.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemyPuffBoy.state.facingAngle = Utils.GetAngle(enemyPuffBoy.transform.position, enemyPuffBoy.TargetObject.transform.position);
          if (Time.frameCount % 5 == 0)
            enemyPuffBoy.simpleSpineAnimator.FlashWhite(!enemyPuffBoy.simpleSpineAnimator.isFillWhite);
          if ((double) (enemyPuffBoy.state.Timer += Time.deltaTime) >= 0.25)
          {
            enemyPuffBoy.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemyPuffBoy.simpleSpineAnimator.FlashWhite(false);
            CameraManager.shakeCamera(0.2f, Utils.GetAngle(enemyPuffBoy.transform.position, enemyPuffBoy.TargetObject.transform.position));
            enemyPuffBoy.collider2DList = new List<Collider2D>();
            enemyPuffBoy.DamageCollider.GetContacts((List<Collider2D>) enemyPuffBoy.collider2DList);
            using (List<Collider2D>.Enumerator enumerator = enemyPuffBoy.collider2DList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Collider2D current = enumerator.Current;
                enemyPuffBoy.EnemyHealth = current.gameObject.GetComponent<Health>();
                if ((UnityEngine.Object) enemyPuffBoy.EnemyHealth != (UnityEngine.Object) null && enemyPuffBoy.EnemyHealth.team != enemyPuffBoy.health.team)
                  enemyPuffBoy.EnemyHealth.DealDamage(1f, enemyPuffBoy.gameObject, Vector3.Lerp(enemyPuffBoy.transform.position, enemyPuffBoy.EnemyHealth.transform.position, 0.7f));
              }
              break;
            }
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) (enemyPuffBoy.state.Timer += Time.deltaTime) >= 2.0)
          {
            enemyPuffBoy.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
      }
      yield return (object) null;
    }
  }
}
