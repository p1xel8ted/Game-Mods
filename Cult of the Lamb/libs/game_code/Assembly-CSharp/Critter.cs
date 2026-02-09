// Decompiled with JetBrains decompiler
// Type: Critter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Critter : UnitObject
{
  public float DangerDistance = 3f;
  public float Timer;
  public float TargetAngle;
  public float WalkSpeed = 0.02f;
  public float RunSpeed = 0.07f;
  public float IgnorePlayer;
  public bool FleeNearEnemies = true;
  public bool EatGrass;
  public bool WonderAround = true;

  public void Start() => this.Timer = (float) UnityEngine.Random.Range(1, 5);

  public override void Update()
  {
    base.Update();
    this.WonderFreely();
  }

  public void WonderFreely()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UsePathing = false;
        if (this.WonderAround)
        {
          if ((double) (this.Timer -= Time.deltaTime) < 0.0)
          {
            this.Timer = (float) UnityEngine.Random.Range(1, 5);
            this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
            this.state.CURRENT_STATE = StateMachine.State.Moving;
          }
          else
            this.LookForDanger();
        }
        if (!this.EatGrass || (double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
        this.Timer = (float) UnityEngine.Random.Range(4, 6);
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
        break;
      case StateMachine.State.Moving:
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (12.0 * (double) GameManager.DeltaTime));
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          if (this.EatGrass)
          {
            this.Timer = (float) UnityEngine.Random.Range(4, 6);
            this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
            break;
          }
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Fleeing:
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null || (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > 5.0)
        {
          this.maxSpeed = this.WalkSpeed;
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        if ((double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > 3.0 || (double) (this.IgnorePlayer -= Time.deltaTime) >= 0.0)
          break;
        this.TargetAngle = Utils.GetAngle(this.TargetEnemy.transform.position, this.transform.position);
        break;
      case StateMachine.State.CustomAction0:
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          this.Timer = (float) UnityEngine.Random.Range(1, 3);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.LookForDanger();
        break;
    }
  }

  public void LookForDanger()
  {
    if (!this.FleeNearEnemies)
      return;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DangerDistance && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetEnemy = allUnit;
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        this.maxSpeed = this.RunSpeed;
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
      }
    }
  }

  public void OnCollisionStay2D(Collision2D collision)
  {
    this.TargetAngle = this.state.facingAngle + 90f;
    this.IgnorePlayer = 2f;
  }

  public override void OnDestroy() => base.OnDestroy();
}
