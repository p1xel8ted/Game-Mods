// Decompiled with JetBrains decompiler
// Type: WildLifeDangerous
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WildLifeDangerous : UnitObject
{
  public float LookAround;
  public float FacingAngle;
  public float Timer;
  public SpriteRenderer spriteRenderer;
  public new Health TargetEnemy;
  public GameObject AttackCircle;
  public GameObject VisionCone;
  public float AttackSpeed;
  public float LoseEnemyRange = 999f;

  public void Start()
  {
    this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    this.LookAround = 90f;
    this.AttackCircle.SetActive(false);
    this.VisionCone.SetActive(false);
    this.ChangeState(StateMachine.State.Idle);
  }

  public override void Update()
  {
    base.Update();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.state.facingAngle = this.FacingAngle + 45f * Mathf.Cos(this.LookAround += 0.005f * GameManager.DeltaTime);
        this.speed += (float) ((0.0 - (double) this.speed) / 7.0);
        this.AttackSpeed += (float) ((0.0 - (double) this.AttackSpeed) / 7.0);
        this.LookOutForDanger();
        break;
      case StateMachine.State.Moving:
        if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null)
        {
          this.ChangeState(StateMachine.State.Idle);
          return;
        }
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.TargetEnemy.transform.position) > (double) this.LoseEnemyRange)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.TargetEnemy = (Health) null;
          return;
        }
        if ((double) (this.Timer += Time.deltaTime) > 1.0)
        {
          this.Timer = 0.0f;
          this.givePath(this.TargetEnemy.transform.position);
        }
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.TargetEnemy.transform.position) < 5.0)
        {
          this.ChangeState(StateMachine.State.SignPostAttack);
          break;
        }
        break;
      case StateMachine.State.Attacking:
        if ((double) this.AttackSpeed < 10.0)
          ++this.AttackSpeed;
        if ((double) (this.Timer += Time.deltaTime) > 0.699999988079071)
        {
          this.ChangeState(StateMachine.State.RecoverFromAttack);
          break;
        }
        break;
      case StateMachine.State.SignPostAttack:
        if ((double) this.AttackSpeed > -5.0)
          this.AttackSpeed -= 0.2f;
        if ((double) (this.Timer += Time.deltaTime) > 0.5)
        {
          this.ChangeState(StateMachine.State.Attacking);
          break;
        }
        break;
      case StateMachine.State.RecoverFromAttack:
        this.AttackSpeed += (float) ((0.0 - (double) this.AttackSpeed) / 15.0);
        if ((double) (this.Timer += Time.deltaTime) > 1.0)
        {
          if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null)
          {
            this.ChangeState(StateMachine.State.Idle);
            break;
          }
          this.ChangeState(StateMachine.State.Moving);
          this.givePath(this.TargetEnemy.transform.position);
          break;
        }
        break;
      case StateMachine.State.RaiseAlarm:
        this.AttackSpeed += (float) ((0.0 - (double) this.AttackSpeed) / 7.0);
        if ((double) (this.Timer += Time.deltaTime) > 1.0)
        {
          this.ChangeState(StateMachine.State.Moving);
          this.givePath(this.TargetEnemy.transform.position);
          break;
        }
        break;
    }
    this.vx = this.AttackSpeed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f));
    this.vy = this.AttackSpeed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f));
  }

  public void ChangeState(StateMachine.State newState)
  {
    this.Timer = 0.0f;
    this.AttackCircle.SetActive(false);
    this.VisionCone.SetActive(false);
    switch (newState)
    {
      case StateMachine.State.Idle:
        this.VisionCone.SetActive(true);
        break;
      case StateMachine.State.Moving:
        this.spriteRenderer.color = Color.white;
        break;
      case StateMachine.State.Attacking:
        this.AttackSpeed = 0.0f;
        this.AttackCircle.SetActive(true);
        this.spriteRenderer.color = Color.white;
        break;
      case StateMachine.State.SignPostAttack:
        this.AttackSpeed = 0.0f;
        this.spriteRenderer.color = Color.white;
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetEnemy.transform.position);
        break;
      case StateMachine.State.RecoverFromAttack:
        this.AttackCircle.SetActive(true);
        this.spriteRenderer.color = Color.white;
        break;
      case StateMachine.State.RaiseAlarm:
        this.spriteRenderer.color = Color.white;
        if (this.TargetEnemy.gameObject.CompareTag("Player"))
        {
          GameManager.GetInstance().AddToCamera(this.gameObject);
          break;
        }
        break;
    }
    this.state.CURRENT_STATE = newState;
  }

  public void LookOutForDanger()
  {
    this.spriteRenderer.color = Color.white;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < 5.0 && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.spriteRenderer.color = Color.yellow;
        float angle = Utils.GetAngle(this.transform.position, allUnit.gameObject.transform.position);
        if ((double) angle < (double) this.state.facingAngle + 45.0 && (double) angle > (double) this.state.facingAngle - 45.0 && this.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, 5f))
        {
          this.TargetEnemy = allUnit;
          this.ChangeState(StateMachine.State.RaiseAlarm);
        }
      }
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType);
    if (this.state.CURRENT_STATE != StateMachine.State.Idle)
      return;
    this.TargetEnemy = Attacker.GetComponent<Health>();
    if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null)
    {
      foreach (Health allUnit in Health.allUnits)
      {
        if (allUnit.team != this.health.team && !allUnit.untouchable && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < 15.0)
          this.TargetEnemy = allUnit;
      }
    }
    this.ChangeState(StateMachine.State.RaiseAlarm);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    this.AttackSpeed *= -0.4f;
  }
}
