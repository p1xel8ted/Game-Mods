// Decompiled with JetBrains decompiler
// Type: CritterSpider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CritterSpider : UnitObject
{
  public static List<CritterSpider> Spiders = new List<CritterSpider>();
  public float Timer;
  public float TargetAngle;
  public float WalkSpeed = 0.02f;
  public float RunSpeed = 0.07f;
  public float IgnorePlayer;
  public Animator animator;
  public SimpleInventory Inventory;
  public bool isSleeping;
  [CompilerGenerated]
  public bool \u003CIsPet\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCanTeleport\u003Ek__BackingField = true;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimName = "animation";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string runAnimName = "run";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string sleepAnimName = "sleep";
  public PickUp TargetPickUp;
  [CompilerGenerated]
  public GameObject \u003CTargetHost\u003Ek__BackingField;
  public bool Stealing;

  public bool IsPet
  {
    get => this.\u003CIsPet\u003Ek__BackingField;
    set => this.\u003CIsPet\u003Ek__BackingField = value;
  }

  public bool CanTeleport
  {
    get => this.\u003CCanTeleport\u003Ek__BackingField;
    set => this.\u003CCanTeleport\u003Ek__BackingField = value;
  }

  public bool IsSleeping
  {
    get => this.isSleeping;
    set
    {
      this.isSleeping = value;
      this.Timer = 10f;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.IsPet)
      CritterSpider.Spiders.Add(this);
    this.Start();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.IsPet)
      return;
    CritterSpider.Spiders.Remove(this);
  }

  public void Initialise()
  {
    if (!this.IsPet)
      return;
    CritterSpider.Spiders.Remove(this);
  }

  public void Start()
  {
    this.Timer = (float) UnityEngine.Random.Range(1, 5);
    this.animator = this.GetComponentInChildren<Animator>(true);
    this.Inventory = this.GetComponentInChildren<SimpleInventory>();
  }

  public GameObject TargetHost
  {
    get => this.\u003CTargetHost\u003Ek__BackingField;
    set => this.\u003CTargetHost\u003Ek__BackingField = value;
  }

  public override void Update()
  {
    if (PlayerRelic.TimeFrozen)
      return;
    base.Update();
    if ((bool) (UnityEngine.Object) this.TargetHost)
      this.FollowerHost();
    else if (!this.Stealing)
      this.WonderFreely();
    else
      this.StealPickUp();
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.Spine.skeleton.ScaleX = (double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle > 270.0 ? -1f : 1f;
  }

  public void StealPickUp()
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
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.timeScale = 1f;
          this.animator.speed = 1f;
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          this.pathToFollow = new List<Vector3>();
          this.pathToFollow.Add(this.TargetPickUp.transform.position);
          this.currentWaypoint = 0;
          this.EndOfPath = this.EndOfPath + new System.Action(this.ArriveAtPickUp);
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, this.idleAnimName, true);
          this.animator.SetTrigger("WALK");
          break;
        case StateMachine.State.Moving:
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.timeScale = 1f;
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
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.timeScale = 1f;
          this.animator.speed = 1.7f;
          this.maxSpeed = this.RunSpeed;
          this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
          break;
      }
    }
  }

  public void ArriveAtPickUp()
  {
    if ((UnityEngine.Object) this.TargetPickUp != (UnityEngine.Object) null && !this.TargetPickUp.Activated)
    {
      this.Inventory.GiveItem(this.TargetPickUp.type);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.TargetPickUp.gameObject);
      this.state.CURRENT_STATE = StateMachine.State.Fleeing;
      this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
      this.UsePathing = false;
      if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
        this.Spine.AnimationState.SetAnimation(0, this.runAnimName, true);
      this.animator.SetTrigger("WALK");
    }
    else
    {
      if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
        this.Spine.AnimationState.SetAnimation(0, this.idleAnimName, true);
      this.animator.SetTrigger("IDLE");
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.Stealing = false;
    }
    this.EndOfPath = this.EndOfPath - new System.Action(this.ArriveAtPickUp);
  }

  public void WonderFreely()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UsePathing = false;
        this.animator.speed = 1f;
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.timeScale = 1f;
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
            this.Spine.AnimationState.SetAnimation(0, this.runAnimName, true);
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Moving:
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.timeScale = 1f;
        this.animator.speed = 1f;
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, this.idleAnimName, true);
          this.animator.SetTrigger("IDLE");
          this.Timer = (float) UnityEngine.Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Fleeing:
        this.animator.speed = 1.5f;
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.timeScale = 1.5f;
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null || (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > 5.0)
        {
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, this.idleAnimName, true);
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

  public void FollowerHost()
  {
    if ((double) Vector3.Distance(this.transform.position, this.TargetHost.transform.position) > 5.0)
    {
      if (this.CanTeleport)
        this.transform.position = this.TargetHost.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
    }
    else
      this.CanTeleport = true;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if (this.IsSleeping)
        {
          if ((double) (this.TargetHost.transform.position + Vector3.down * 0.3f + Vector3.left * 0.1f - this.transform.position).magnitude > 0.5)
            break;
          this.Spine.AnimationState.SetAnimation(0, this.sleepAnimName, true);
          this.state.CURRENT_STATE = StateMachine.State.Sleeping;
          break;
        }
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.timeScale = 1f;
        this.UsePathing = true;
        this.animator.speed = 1f;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        this.pathToFollow = new List<Vector3>();
        this.pathToFollow.Add(this.TargetHost.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
        this.currentWaypoint = 0;
        this.EndOfPath = this.EndOfPath + new System.Action(this.ArriveAtPickUp);
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.AnimationState.SetAnimation(0, this.runAnimName, true);
        this.animator.SetTrigger("WALK");
        break;
      case StateMachine.State.Moving:
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.timeScale = 1f;
        this.animator.speed = 1f;
        this.maxSpeed = this.RunSpeed;
        if ((double) (this.Timer += Time.deltaTime) <= 2.5)
          break;
        this.Timer = 0.0f;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        if (this.IsSleeping)
        {
          Vector3 vector3 = this.TargetHost.transform.position + Vector3.down * 0.3f + Vector3.left * 0.1f;
          Vector3 to = vector3 - this.transform.position;
          if ((double) to.magnitude <= 0.5)
          {
            this.Spine.AnimationState.SetAnimation(0, this.sleepAnimName, true);
            this.state.CURRENT_STATE = StateMachine.State.Sleeping;
          }
          else
          {
            this.Timer = 0.0f;
            this.currentWaypoint = 0;
            this.state.facingAngle = Vector2.Angle(Vector2.right, (Vector2) to);
            this.pathToFollow = new List<Vector3>();
            this.pathToFollow.Add(vector3);
            this.currentWaypoint = 0;
          }
        }
        else
        {
          Vector3 vector3 = this.TargetHost.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
          this.pathToFollow = new List<Vector3>();
          this.pathToFollow.Add(vector3);
          this.currentWaypoint = 0;
          this.state.facingAngle = Vector2.Angle(Vector2.right, (Vector2) (vector3 - this.transform.position));
        }
        if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
          break;
        this.Spine.skeleton.ScaleX = (double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle > 270.0 ? -1f : 1f;
        break;
      case StateMachine.State.Fleeing:
        this.animator.speed = 1.7f;
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
          this.Spine.timeScale = 1.5f;
        this.maxSpeed = this.RunSpeed;
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
          break;
        this.Spine.skeleton.ScaleX = (double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle > 270.0 ? -1f : 1f;
        break;
      case StateMachine.State.Sleeping:
        this.state.CURRENT_STATE = StateMachine.State.Sleeping;
        this.ClearPaths();
        break;
    }
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
        {
          if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
            this.Spine.AnimationState.SetAnimation(0, this.runAnimName, true);
          this.animator.SetTrigger("WALK");
        }
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
