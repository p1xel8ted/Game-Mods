// Decompiled with JetBrains decompiler
// Type: BigSpider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BigSpider : UnitObject
{
  public float Timer;
  public float TargetAngle;
  public float WalkSpeed = 0.02f;
  public float RunSpeed = 0.07f;
  public float IgnorePlayer;
  public MonsterHive MonsterDen;
  public Animator animator;
  public Worshipper worshipper;
  public Worshipper TargetWorshipper;
  public bool Stealing;

  public void Start()
  {
    this.Timer = (float) UnityEngine.Random.Range(1, 5);
    this.animator = this.GetComponentInChildren<Animator>();
  }

  public override void Update()
  {
    base.Update();
    if (!this.Stealing)
      this.WonderFreely();
    else if ((UnityEngine.Object) this.MonsterDen != (UnityEngine.Object) null)
      this.StealPickUp();
    else
      this.Stealing = false;
  }

  public void WonderFreely()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UsePathing = false;
        this.animator.speed = 1f;
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          if ((UnityEngine.Object) this.MonsterDen != (UnityEngine.Object) null && (UnityEngine.Object) this.worshipper == (UnityEngine.Object) null)
          {
            foreach (Worshipper worshipper in Worshipper.worshippers)
            {
              if (!worshipper.BeingCarried && (double) Vector3.Distance(worshipper.transform.position, this.transform.position) < 6.0)
              {
                this.TargetWorshipper = worshipper;
                this.Stealing = true;
                return;
              }
            }
          }
          this.animator.SetTrigger("WALK");
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Moving:
        this.animator.speed = 1f;
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          this.animator.SetTrigger("IDLE");
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Fleeing:
        this.animator.speed = 1.5f;
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null || (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > 5.0)
        {
          this.animator.SetTrigger("IDLE");
          this.maxSpeed = this.WalkSpeed;
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        if ((double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > 3.0 || (double) (this.IgnorePlayer -= Time.deltaTime) >= 0.0)
          break;
        this.TargetAngle = Utils.GetAngle(this.TargetEnemy.transform.position, this.transform.position);
        break;
    }
  }

  public void StealPickUp()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UsePathing = true;
        this.animator.speed = 1f;
        this.givePath((Vector3) AstarPath.active.GetNearest(this.TargetWorshipper.transform.position, UnitObject.constraint).node.position);
        this.animator.SetTrigger("WALK");
        break;
      case StateMachine.State.Moving:
        this.animator.speed = 1f;
        this.maxSpeed = this.RunSpeed;
        if ((UnityEngine.Object) this.TargetWorshipper == (UnityEngine.Object) null)
        {
          this.animator.SetTrigger("IDLE");
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.Stealing = false;
          break;
        }
        if ((double) Vector3.Distance(this.transform.position, this.TargetWorshipper.transform.position) < 0.5)
        {
          this.worshipper = this.TargetWorshipper;
          this.worshipper.PickUp();
          this.ClearPaths();
          this.UsePathing = true;
          this.givePath(this.MonsterDen.HoomanTrap.transform.position);
          this.EndOfPath = this.EndOfPath + new System.Action(this.TrapHooman);
          this.animator.SetTrigger("WALK");
          AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/warning", this.gameObject);
          this.state.CURRENT_STATE = StateMachine.State.Fleeing;
          break;
        }
        if ((double) (this.Timer += Time.deltaTime) <= 1.0)
          break;
        this.Timer = 0.0f;
        this.givePath((Vector3) AstarPath.active.GetNearest(this.TargetWorshipper.transform.position, UnitObject.constraint).node.position);
        break;
      case StateMachine.State.Fleeing:
        if ((UnityEngine.Object) this.worshipper != (UnityEngine.Object) null)
          this.worshipper.transform.position = this.transform.position + new Vector3(0.0f, 0.0f, -0.3f);
        this.animator.speed = 1.7f;
        this.maxSpeed = this.RunSpeed;
        break;
      case StateMachine.State.CustomAction0:
        if ((double) (this.Timer += Time.deltaTime) <= 0.10000000149011612)
          break;
        this.givePath(this.MonsterDen.Den.transform.position);
        this.EndOfPath = this.EndOfPath + new System.Action(this.HideInDen);
        AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/warning", this.gameObject);
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
        this.animator.SetTrigger("WALK");
        break;
    }
  }

  public void TrapHooman()
  {
    this.MonsterDen.HoomanTrap.GetComponent<ScaleBounce>().SquishMe(0.2f, -0.2f);
    this.MonsterDen.worshipper = this.worshipper;
    this.worshipper.CapturedByBigSpider();
    this.worshipper = (Worshipper) null;
    this.EndOfPath = this.EndOfPath - new System.Action(this.TrapHooman);
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    this.animator.SetTrigger("IDLE");
  }

  public void HideInDen()
  {
    this.MonsterDen.Den.GetComponent<ScaleBounce>().SquishMe(0.2f, -0.2f);
    this.Stealing = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    this.EndOfPath = this.EndOfPath - new System.Action(this.HideInDen);
  }

  public void LookForDanger()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < 2.5 && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetEnemy = allUnit;
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        this.maxSpeed = this.RunSpeed;
        if (this.state.CURRENT_STATE == StateMachine.State.Idle)
          this.animator.SetTrigger("WALK");
        AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/warning", this.gameObject);
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
      }
    }
  }

  public void OnCollisionStay2D(Collision2D collision)
  {
    this.TargetAngle = this.state.facingAngle + 90f;
    this.IgnorePlayer = 2f;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.EndOfPath = this.EndOfPath - new System.Action(this.TrapHooman);
    this.EndOfPath = this.EndOfPath - new System.Action(this.HideInDen);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!((UnityEngine.Object) this.worshipper != (UnityEngine.Object) null))
      return;
    this.worshipper.DropMe();
  }
}
