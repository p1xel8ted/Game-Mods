// Decompiled with JetBrains decompiler
// Type: WildLife
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WildLife : UnitObject
{
  public const int V = 1;
  public float Delay;
  public Vector3 TargetPosition;
  public float LookAround;
  public float FacingAngle;
  public float FleeSpeed = 0.08f;
  public float DefaultSpeed = 0.05f;
  public float FleeTimer;
  public float Timer;
  public bool eaten;
  public bool CheckCollisions;
  public static List<WildLife> wildlife = new List<WildLife>();
  public GameObject VisionCone;
  public GameObject EscapeDoor;

  public override void OnEnable()
  {
    base.OnEnable();
    WildLife.wildlife.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    WildLife.wildlife.Remove(this);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.EscapeDoor != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.EscapeDoor.transform.position) <= 1.0)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          this.state.facingAngle = this.FacingAngle + 45f * Mathf.Cos(this.LookAround += 0.005f * GameManager.DeltaTime);
          this.speed += (float) ((0.0 - (double) this.speed) / 7.0);
          if ((double) (this.Delay -= Time.deltaTime) < 0.0)
          {
            this.Delay = (float) UnityEngine.Random.Range(3, 5);
            if (!this.eaten && (double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
            {
              this.CheckCollisions = false;
              this.Timer = 0.0f;
              this.ChangeState(StateMachine.State.CustomAction0);
              this.eaten = true;
            }
            else
            {
              this.NewPosition();
              this.givePath(this.TargetPosition);
              this.eaten = false;
            }
          }
          this.LookOutForDanger();
          break;
        case StateMachine.State.Moving:
          this.FacingAngle = this.state.facingAngle;
          this.LookAround = 90f;
          this.LookOutForDanger();
          break;
        case StateMachine.State.Fleeing:
          if ((double) (this.FleeTimer -= Time.deltaTime) < 0.0)
          {
            this.FleeTimer = 1f;
            this.TargetPosition = this.EscapeDoor.transform.position;
            this.givePath(this.TargetPosition);
            break;
          }
          break;
        case StateMachine.State.CustomAction0:
          if ((double) (this.Timer += Time.deltaTime) > 2.0)
            this.ChangeState(StateMachine.State.Idle);
          if ((double) this.Timer > 0.5 && !this.CheckCollisions)
          {
            this.EatGrass();
            this.CheckCollisions = true;
            break;
          }
          break;
        case StateMachine.State.RaiseAlarm:
          this.speed += (float) ((0.0 - (double) this.speed) / 7.0);
          if ((double) (this.Timer += Time.deltaTime) > 0.5)
          {
            this.givePath(this.TargetPosition);
            this.ChangeState(StateMachine.State.Fleeing);
            break;
          }
          break;
      }
      this.vx = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * GameManager.DeltaTime;
      this.vy = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * GameManager.DeltaTime;
    }
  }

  public void LookOutForDanger()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < 5.0 && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        float angle = Utils.GetAngle(this.transform.position, allUnit.gameObject.transform.position);
        if ((double) angle < (double) this.state.facingAngle + 90.0 && (double) angle > (double) this.state.facingAngle - 90.0)
        {
          this.TargetEnemy = allUnit;
          this.Flee(true);
        }
      }
    }
  }

  public void ChangeState(StateMachine.State newState)
  {
    this.Timer = 0.0f;
    this.maxSpeed = this.DefaultSpeed;
    this.VisionCone.SetActive(false);
    switch (newState)
    {
      case StateMachine.State.Idle:
        this.VisionCone.SetActive(true);
        break;
      case StateMachine.State.Fleeing:
        this.maxSpeed = this.FleeSpeed;
        break;
      case StateMachine.State.RaiseAlarm:
        this.AlertOthers(this.gameObject);
        break;
    }
    this.state.CURRENT_STATE = newState;
  }

  public void Flee(bool FirstToSeeThreat)
  {
    NavigateRooms instance = NavigateRooms.GetInstance();
    if (!((UnityEngine.Object) instance != (UnityEngine.Object) null))
      return;
    this.EscapeDoor = (GameObject) null;
    List<GameObject> gameObjectList = new List<GameObject>();
    float num1 = float.MaxValue;
    if (instance.North.activeSelf)
      gameObjectList.Add(instance.North);
    if (instance.East.activeSelf)
      gameObjectList.Add(instance.East);
    if (instance.South.activeSelf)
      gameObjectList.Add(instance.South);
    if (instance.West.activeSelf)
      gameObjectList.Add(instance.West);
    foreach (GameObject gameObject in gameObjectList)
    {
      float num2 = Vector3.Distance(this.transform.position, gameObject.transform.position);
      if ((double) num2 < (double) num1)
      {
        this.EscapeDoor = gameObject;
        num1 = num2;
      }
    }
    this.TargetPosition = this.EscapeDoor.transform.position;
    if (FirstToSeeThreat)
    {
      this.ChangeState(StateMachine.State.RaiseAlarm);
    }
    else
    {
      this.givePath(this.TargetPosition);
      this.ChangeState(StateMachine.State.Fleeing);
    }
  }

  public void AlertOthers(GameObject Warner)
  {
    foreach (WildLife wildLife in WildLife.wildlife)
    {
      if (wildLife.state.CURRENT_STATE != StateMachine.State.Fleeing && (UnityEngine.Object) wildLife != (UnityEngine.Object) Warner && (double) Vector3.Distance(wildLife.transform.position, Warner.transform.position) < 5.0)
        this.Flee(false);
    }
  }

  public void EatGrass()
  {
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) (this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), 0.0f)), 0.5f))
    {
      if ((UnityEngine.Object) collider2D.gameObject.GetComponent<Grass>() != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) collider2D.gameObject);
    }
  }

  public void NewPosition()
  {
    Vector2 vector2 = UnityEngine.Random.insideUnitCircle * 3f;
    this.TargetPosition = this.transform.position + new Vector3(vector2.x, vector2.y, 0.0f);
    this.TargetPosition = (Vector3) AstarPath.active.GetNearest(this.TargetPosition).node.position;
  }
}
