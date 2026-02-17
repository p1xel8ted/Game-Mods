// Decompiled with JetBrains decompiler
// Type: Task_Combat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Task_Combat : Task
{
  public float DetectEnemyRange = 5f;
  public float AttackRange = 0.5f;
  public float LoseEnemyRange = 7f;
  public float PreAttackDuration = 0.5f;
  public float PostAttackDuration = 1f;
  [HideInInspector]
  public int thisChecked;
  [HideInInspector]
  public int AttackPosition;
  [HideInInspector]
  public Health EnemyHealth;
  [HideInInspector]
  public Health health;
  [HideInInspector]
  public float DefendingDuration;
  public bool CannotLoseTarget;
  public float posAngle;

  public Task_Combat() => this.Type = Task_Type.COMBAT;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.health = t.gameObject.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void Init(
    float DetectEnemyRange,
    float AttackRange,
    float LoseEnemyRange,
    float PreAttackDuration,
    float PostAttackDuration,
    float DefendingDuration,
    TaskDoer doer)
  {
    this.DetectEnemyRange = DetectEnemyRange;
    this.AttackRange = AttackRange;
    this.LoseEnemyRange = LoseEnemyRange;
    this.PreAttackDuration = PreAttackDuration;
    this.PostAttackDuration = PostAttackDuration;
    this.DefendingDuration = DefendingDuration;
    this.t = doer;
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.ClearTarget();
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public override void TaskUpdate()
  {
    this.thisChecked = 0;
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position) < (double) this.AttackRange)
          {
            this.Timer = 0.0f;
            this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
            this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          this.Timer = 0.0f;
          this.SortAttackPosition();
          this.PathToPosition(this.TargetPosition());
          break;
        case StateMachine.State.Moving:
          if ((double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) this.TargetObject.transform.position) > (double) this.LoseEnemyRange && !this.CannotLoseTarget)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            this.ClearTarget();
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 0.5)
            break;
          this.Timer = 0.0f;
          if (!this.CannotLoseTarget)
          {
            this.ClearTarget();
            this.GetNewTarget();
          }
          this.SortAttackPosition();
          this.PathToPosition(this.TargetPosition());
          break;
        case StateMachine.State.SignPostAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PreAttackDuration)
            break;
          this.Timer = 0.0f;
          if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          this.DoAttack(this.AttackRange);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PostAttackDuration)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (!((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null))
            break;
          this.ClearTarget();
          break;
        case StateMachine.State.SignPostCounterAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PreAttackDuration)
            break;
          this.Timer = 0.0f;
          if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          CameraManager.shakeCamera(0.3f, this.state.facingAngle);
          this.DoAttack(this.AttackRange, StateMachine.State.RecoverFromCounterAttack);
          break;
        case StateMachine.State.RecoverFromCounterAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PostAttackDuration)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (!((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null))
            break;
          this.ClearTarget();
          break;
      }
    }
    else
    {
      this.GetNewTarget();
      if (!((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null))
        return;
      this.ClearTask();
    }
  }

  public override void ClearTask()
  {
    this.ClearTarget();
    base.ClearTask();
  }

  public void ClearTarget()
  {
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
    {
      if (this.TargetObject.GetComponent<Health>().AttackPositions[this.AttackPosition] != null)
        this.TargetObject.GetComponent<Health>().AttackPositions[this.AttackPosition] = (Task_Combat) null;
      this.TargetObject.GetComponent<Health>().attackers.Remove(this.t.gameObject);
    }
    this.TargetObject = (GameObject) null;
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public Vector3 TargetPosition()
  {
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      return this.t.transform.position;
    this.posAngle = (float) (90 * this.AttackPosition);
    return this.TargetObject.transform.position + new Vector3(1f * Mathf.Cos(this.posAngle * ((float) Math.PI / 180f)), 1f * Mathf.Sin(this.posAngle * ((float) Math.PI / 180f)), 0.0f);
  }

  public void GetNewTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.t.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.t.transform.position)))
      {
        float num2 = Vector3.Distance(this.t.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if (!((UnityEngine.Object) health != (UnityEngine.Object) null))
      return;
    health.attackers.Add(this.t.gameObject);
    this.TargetObject = health.gameObject;
    this.EnemyHealth = health;
    this.SortAttackPosition();
  }

  public void SortAttackPosition()
  {
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      return;
    ++this.thisChecked;
    if (this.EnemyHealth.AttackPositions[this.AttackPosition] != null && this.EnemyHealth.AttackPositions[this.AttackPosition] == this)
      this.EnemyHealth.AttackPositions[this.AttackPosition] = (Task_Combat) null;
    List<Vector2> vector2List = new List<Vector2>();
    for (int x = 0; x < 4; ++x)
    {
      float num = (float) (90 * x);
      float y = Vector2.Distance((Vector2) this.t.transform.position, (Vector2) (this.TargetObject.gameObject.transform.position + new Vector3(1f * Mathf.Cos(num * ((float) Math.PI / 180f)), 1f * Mathf.Sin(num * ((float) Math.PI / 180f)), 0.0f)));
      Vector2 vector2 = new Vector2((float) x, y);
      vector2List.Add(vector2);
    }
    vector2List.Sort(new Comparison<Vector2>(Task_Combat.SortByY));
    for (int index = 0; index < 4; ++index)
    {
      if (this.EnemyHealth.AttackPositions[(int) vector2List[index].x] == null)
      {
        this.AttackPosition = (int) vector2List[index].x;
        this.EnemyHealth.AttackPositions[(int) vector2List[index].x] = this;
        break;
      }
      if (this.EnemyHealth.AttackPositions[(int) vector2List[index].x].thisChecked < 4)
      {
        Task_Combat attackPosition1 = this.EnemyHealth.AttackPositions[(int) vector2List[index].x];
        float num = (float) (90 * index);
        Vector3 b = this.TargetObject.transform.position + new Vector3(1f * Mathf.Cos(num * ((float) Math.PI / 180f)), 1f * Mathf.Sin(num * ((float) Math.PI / 180f)), 0.0f);
        if (attackPosition1 != null && (UnityEngine.Object) attackPosition1.t != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) attackPosition1.t.transform.position, (Vector2) b) > (double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) b))
        {
          Task_Combat attackPosition2 = this.EnemyHealth.AttackPositions[(int) vector2List[index].x];
          this.AttackPosition = (int) vector2List[index].x;
          this.EnemyHealth.AttackPositions[(int) vector2List[index].x] = this;
          attackPosition2.SortAttackPosition();
          break;
        }
      }
    }
  }

  public static int SortByY(Vector2 p1, Vector2 p2) => p1.y.CompareTo(p2.y);
}
