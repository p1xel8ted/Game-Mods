// Decompiled with JetBrains decompiler
// Type: CritterSpider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterSpider : UnitObject
{
  public static List<CritterSpider> Spiders = new List<CritterSpider>();
  private float Timer;
  private float TargetAngle;
  public float WalkSpeed = 0.02f;
  public float RunSpeed = 0.07f;
  private float IgnorePlayer;
  private Animator animator;
  public SimpleInventory Inventory;
  public SkeletonAnimation Spine;
  private PickUp TargetPickUp;
  private bool Stealing;

  public override void OnEnable()
  {
    base.OnEnable();
    CritterSpider.Spiders.Add(this);
    this.Start();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    CritterSpider.Spiders.Remove(this);
  }

  private void Start()
  {
    this.Timer = (float) UnityEngine.Random.Range(1, 5);
    this.animator = this.GetComponentInChildren<Animator>();
    this.Inventory = this.GetComponentInChildren<SimpleInventory>();
  }

  public GameObject TargetHost { get; set; }

  public override void Update()
  {
    base.Update();
    if ((bool) (UnityEngine.Object) this.TargetHost)
      this.FollowerHost();
    else if (!this.Stealing)
      this.WonderFreely();
    else
      this.StealPickUp();
  }

  private void StealPickUp()
  {
    if ((UnityEngine.Object) this.TargetPickUp == (UnityEngine.Object) null)
    {
      this.Stealing = false;
    }
    else
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          this.UsePathing = true;
          this.animator.speed = 1f;
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          this.pathToFollow = new List<Vector3>();
          this.pathToFollow.Add(this.TargetPickUp.transform.position);
          this.currentWaypoint = 0;
          this.EndOfPath = this.EndOfPath + new System.Action(this.ArriveAtPickUp);
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, "run", true);
          this.animator.SetTrigger("WALK");
          break;
        case StateMachine.State.Moving:
          this.animator.speed = 1f;
          this.maxSpeed = this.RunSpeed;
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          this.pathToFollow = new List<Vector3>();
          this.pathToFollow.Add(this.TargetPickUp.transform.position);
          this.currentWaypoint = 0;
          break;
        case StateMachine.State.Fleeing:
          this.animator.speed = 1.7f;
          this.maxSpeed = this.RunSpeed;
          this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
          break;
      }
    }
  }

  private void ArriveAtPickUp()
  {
    if ((UnityEngine.Object) this.TargetPickUp != (UnityEngine.Object) null && !this.TargetPickUp.Activated)
    {
      this.Inventory.GiveItem(this.TargetPickUp.type);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.TargetPickUp.gameObject);
      this.state.CURRENT_STATE = StateMachine.State.Fleeing;
      this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
      this.UsePathing = false;
      if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
        this.Spine.AnimationState.SetAnimation(0, "run", true);
      this.animator.SetTrigger("WALK");
    }
    else
    {
      if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
        this.Spine.AnimationState.SetAnimation(0, "animation", true);
      this.animator.SetTrigger("IDLE");
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.Stealing = false;
    }
    this.EndOfPath = this.EndOfPath - new System.Action(this.ArriveAtPickUp);
  }

  private void WonderFreely()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UsePathing = false;
        this.animator.speed = 1f;
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          if (this.Inventory.GetItemType() == InventoryItem.ITEM_TYPE.NONE)
          {
            foreach (PickUp pickUp in PickUp.PickUps)
            {
              if (pickUp.CanBeStolenByCritter)
              {
                this.TargetPickUp = pickUp;
                this.Stealing = true;
                this.TargetPickUp.TargetedByCritter();
                return;
              }
            }
          }
          this.animator.SetTrigger("WALK");
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, "run", true);
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
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, "animation", true);
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
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, "animation", true);
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

  private void FollowerHost()
  {
    if ((double) Vector3.Distance(this.transform.position, this.TargetHost.transform.position) > 5.0)
      this.transform.position = this.TargetHost.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UsePathing = true;
        this.animator.speed = 1f;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        this.pathToFollow = new List<Vector3>();
        this.pathToFollow.Add(this.TargetHost.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
        this.currentWaypoint = 0;
        this.EndOfPath = this.EndOfPath + new System.Action(this.ArriveAtPickUp);
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.AnimationState.SetAnimation(0, "run", true);
        this.animator.SetTrigger("WALK");
        break;
      case StateMachine.State.Moving:
        this.animator.speed = 1f;
        this.maxSpeed = this.RunSpeed;
        if ((double) (this.Timer += Time.deltaTime) <= 2.5)
          break;
        this.Timer = 0.0f;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        this.pathToFollow = new List<Vector3>();
        this.pathToFollow.Add(this.TargetHost.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
        this.currentWaypoint = 0;
        break;
      case StateMachine.State.Fleeing:
        this.animator.speed = 1.7f;
        this.maxSpeed = this.RunSpeed;
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        break;
    }
  }

  private void LookForDanger()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < 2.5 && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetEnemy = allUnit;
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        this.maxSpeed = this.RunSpeed;
        if (this.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, "run", true);
          this.animator.SetTrigger("WALK");
        }
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
      }
    }
  }

  private void OnCollisionStay2D(Collision2D collision)
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
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.Inventory.DropItem();
    this.EndOfPath = this.EndOfPath - new System.Action(this.ArriveAtPickUp);
    this.gameObject.Recycle();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.EndOfPath = this.EndOfPath - new System.Action(this.ArriveAtPickUp);
  }
}
