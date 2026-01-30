// Decompiled with JetBrains decompiler
// Type: Interaction_WolfBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Interaction_WolfBase : Interaction
{
  public static List<Interaction_WolfBase> Wolfs = new List<Interaction_WolfBase>();
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  public UnitObject unitObject;
  public Interaction_Ranchable target;
  public Interaction_WolfTrap trap;
  public float repathTimer = 1f;
  public bool spanked;
  public bool isScared;
  public Vector3 previousPosition = Vector3.zero;
  public LayerMask islandMask;
  public LayerMask fenceMask;
  [CompilerGenerated]
  public Interaction_WolfBase.State \u003CCurrentState\u003Ek__BackingField;
  public StateMachine stateMachine;
  public float attackTimer = -1f;
  public const float TIME_TILL_ATTACK = 5f;
  public float followerCheckerTimer = 2f;
  public int spankedCounter;
  public static int WolfFled = 0;
  public static int WolfCount = 0;
  public static int WolfTarget = 0;
  public static int WolfDied = 0;
  public static Interaction_WolfBase.WolfEvent OnWolvesBegan;
  public static Interaction_WolfBase.WolfEvent OnWolvesSucceeded;
  public static Interaction_WolfBase.WolfEvent OnWolvesFailed;
  public static Interaction_WolfBase.WolfEvent OnWolfFled;
  public static Interaction_WolfBase.WolfEvent OnWolfDied;
  public bool contributedToCounter;
  public EventInstance howlEventInstance;
  public bool IsFleein;
  public static int PlayerCombo = 1;
  public EventInstance fleeLoopingSFX;

  public SkeletonAnimation Spine => this.spine;

  public UnitObject UnitObject => this.unitObject;

  public Interaction_WolfBase.State CurrentState
  {
    get => this.\u003CCurrentState\u003Ek__BackingField;
    set => this.\u003CCurrentState\u003Ek__BackingField = value;
  }

  public static bool WolvesActive => Interaction_WolfBase.WolfTarget != -1;

  public string RunAnimation => !this.IsFleein ? "run" : "charge_attack";

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_WolfBase.Wolfs.Add(this);
    this.HasSecondaryInteraction = true;
    this.SecondaryInteractable = true;
    this.Interactable = false;
  }

  public override void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.fleeLoopingSFX);
    AudioManager.Instance.StopOneShotInstanceEarly(this.howlEventInstance, STOP_MODE.IMMEDIATE);
    base.OnDisable();
    Interaction_WolfBase.Wolfs.Remove(this);
  }

  public void Awake()
  {
    this.stateMachine = this.GetComponent<StateMachine>();
    this.unitObject = this.GetComponent<UnitObject>();
    this.unitObject.EndOfPath += new System.Action(this.OnEndPath);
    this.islandMask = (LayerMask) ((int) this.islandMask | 1 << LayerMask.NameToLayer("Island"));
    this.fenceMask = (LayerMask) ((int) this.fenceMask | 1 << LayerMask.NameToLayer("Only Collider With Enemies"));
  }

  public static void SpawnWolf(
    Vector3 position,
    Interaction_Ranchable target,
    bool givePath,
    System.Action<Interaction_WolfBase> callback)
  {
    if (Interaction_WolfBase.WolfCount >= Interaction_WolfBase.WolfTarget)
      return;
    ++Interaction_WolfBase.WolfCount;
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Structures/Ranch/Wolf Base Enemy.prefab", position, Quaternion.identity, BaseLocationManager.Instance.StructureLayer).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Interaction_WolfBase component = obj.Result.GetComponent<Interaction_WolfBase>();
      component.transform.position = position + (Vector3) (UnityEngine.Random.insideUnitCircle * 2f);
      component.target = target;
      if (givePath)
        component.GivePath();
      System.Action<Interaction_WolfBase> action = callback;
      if (action == null)
        return;
      action(component);
    });
  }

  public void GivePath()
  {
    if (((UnityEngine.Object) this.target == (UnityEngine.Object) null || this.target.CurrentState == Interaction_Ranchable.State.Dead) && this.CurrentState != Interaction_WolfBase.State.Fleeing)
    {
      this.stateMachine.facingAngle = this.stateMachine.LookAngle = Mathf.Repeat(this.stateMachine.facingAngle + 180f, 360f);
      this.Flee();
    }
    else
    {
      this.unitObject.givePath(this.target.transform.position, this.target.gameObject);
      this.CurrentState = Interaction_WolfBase.State.Preying;
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this.unitObject != (UnityEngine.Object) null)
      this.unitObject.EndOfPath -= new System.Action(this.OnEndPath);
    if ((UnityEngine.Object) this.trap != (UnityEngine.Object) null)
      this.trap.Structure.Brain.ReservedForTask = false;
    AudioManager.Instance.StopLoop(this.fleeLoopingSFX);
    AudioManager.Instance.StopOneShotInstanceEarly(this.howlEventInstance, STOP_MODE.IMMEDIATE);
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = this.SecondaryInteractable ? LocalizationManager.GetTranslation("Interactions/Spank") : "";
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    ++this.spankedCounter;
    this.Spank(this.playerFarming.transform.position);
  }

  public void Spank(Vector3 fromPosition)
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      this.target.Hits = 0;
    AudioManager.Instance.StopOneShotInstanceEarly(this.howlEventInstance, STOP_MODE.IMMEDIATE);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    this.playerFarming.TimedAction(0.266666681f, (System.Action) (() => { }), "attack-combo" + Interaction_WolfBase.PlayerCombo.ToString());
    ++Interaction_WolfBase.PlayerCombo;
    if (Interaction_WolfBase.PlayerCombo > 3)
      Interaction_WolfBase.PlayerCombo = 1;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/spank", this.transform.position);
    float angle = Utils.GetAngle(fromPosition, this.transform.position);
    this.stateMachine.facingAngle = this.stateMachine.LookAngle = Utils.GetAngle(this.transform.position, fromPosition);
    this.unitObject.DoKnockBack(angle * ((float) Math.PI / 180f), 1f, 0.5f);
    this.simpleSpineFlash.FlashFillRed();
    this.spanked = true;
    BiomeConstants.Instance.PlayerEmitHitImpactEffect(this.transform.position, angle);
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, angle, "black");
    if (this.spankedCounter >= 3)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      Interaction_WolfBase.WolfEvent onWolfDied = Interaction_WolfBase.OnWolfDied;
      if (onWolfDied != null)
        onWolfDied();
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT_MORSEL, UnityEngine.Random.Range(2, 5), this.transform.position);
      this.ContributeToCounter();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
      this.KnockBack();
  }

  public void ForcePosition(Vector3 pos)
  {
    this.transform.position = pos;
    Vector3 vector3_1 = this.target.transform.position - this.transform.position;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) vector3_1.normalized, vector3_1.magnitude, (int) this.islandMask).collider == (UnityEngine.Object) null)
      this.unitObject.UsePathing = true;
    Vector3 vector3_2 = this.GetTargetFencePosition();
    this.trap = this.GetTargetTrap(vector3_2);
    if ((UnityEngine.Object) this.trap != (UnityEngine.Object) null)
      vector3_2 = this.trap.transform.position;
    this.unitObject.givePath(vector3_2);
  }

  public void KnockBack()
  {
    this.CurrentState = Interaction_WolfBase.State.Animating;
    this.unitObject.ClearPaths();
    this.StartCoroutine((IEnumerator) this.KnockBackRoutine());
  }

  public IEnumerator KnockBackRoutine()
  {
    this.unitObject.ClearPaths();
    this.unitObject.UsePathing = false;
    this.spine.AnimationState.SetAnimation(0, "knockback", false);
    yield return (object) new WaitForSeconds(0.6f);
    this.unitObject.maxSpeed = 0.0f;
    this.spine.AnimationState.SetAnimation(0, "knockback-reset", false);
    this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.9f);
    this.unitObject.UsePathing = true;
    this.Flee();
  }

  public void Flee()
  {
    if (this.CurrentState != Interaction_WolfBase.State.Fleeing)
    {
      this.ContributeToCounter();
      AudioManager.Instance.StopOneShotInstanceEarly(this.howlEventInstance, STOP_MODE.IMMEDIATE);
      AudioManager.Instance.StopLoop(this.fleeLoopingSFX);
      this.fleeLoopingSFX = AudioManager.Instance.CreateLoop("event:/dlc/env/dog/dog_basic_mv_run_away_loop", this.gameObject, true);
    }
    this.unitObject.UsePathing = false;
    this.unitObject.maxSpeed = 0.1f;
    this.unitObject.ClearPaths();
    this.unitObject.givePath(new Vector3((double) this.stateMachine.facingAngle < 0.0 || (double) this.stateMachine.facingAngle >= 180.0 ? 35f : -35f, this.transform.position.y), forceIgnoreAStar: true);
    this.CurrentState = Interaction_WolfBase.State.Fleeing;
    if (!((UnityEngine.Object) this.trap != (UnityEngine.Object) null))
      return;
    this.trap.Structure.Brain.ReservedForTask = false;
    this.trap = (Interaction_WolfTrap) null;
  }

  public void ContributeToCounter()
  {
    if (!this.contributedToCounter)
    {
      ++Interaction_WolfBase.WolfFled;
      Interaction_WolfBase.WolfEvent onWolfFled = Interaction_WolfBase.OnWolfFled;
      if (onWolfFled != null)
        onWolfFled();
      this.CheckWolvesSuccess();
      this.HandleNextWolf();
    }
    this.contributedToCounter = true;
  }

  public void SetScared() => this.isScared = true;

  public override void Update()
  {
    base.Update();
    if (this.CurrentState == Interaction_WolfBase.State.Animating)
      return;
    Vector3 normalized1 = (this.transform.position - this.previousPosition).normalized;
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.CurrentState != Interaction_WolfBase.State.Fleeing && this.previousPosition != Vector3.zero && !this.unitObject.UsePathing && (bool) Physics2D.Raycast((Vector2) (this.transform.position - normalized1), (Vector2) -normalized1, 2f, (int) this.islandMask))
    {
      this.unitObject.ClearPaths();
      this.unitObject.UsePathing = true;
      this.repathTimer = 0.0f;
      if (this.target.CurrentState == Interaction_Ranchable.State.Default || this.target.CurrentState == Interaction_Ranchable.State.InsideHutch || this.target.CurrentState == Interaction_Ranchable.State.Sleeping || this.target.CurrentState == Interaction_Ranchable.State.Animating)
      {
        Vector3 vector3 = this.GetTargetFencePosition();
        this.trap = this.GetTargetTrap(vector3);
        if ((UnityEngine.Object) this.trap != (UnityEngine.Object) null)
          vector3 = this.trap.transform.position;
        this.unitObject.givePath(vector3);
        this.CurrentState = Interaction_WolfBase.State.Preying;
      }
    }
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.target.ReservedByPlayer)
      this.target = (Interaction_Ranchable) null;
    if (this.CurrentState == Interaction_WolfBase.State.Preying && (UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      this.repathTimer -= Time.deltaTime;
      if ((double) this.repathTimer <= 0.0)
      {
        Vector3 vector3 = this.GetTargetFencePosition();
        this.trap = this.GetTargetTrap(vector3);
        if ((UnityEngine.Object) this.trap != (UnityEngine.Object) null)
          vector3 = this.trap.transform.position;
        if ((double) Vector3.Distance(vector3, this.transform.position) > 0.25)
          this.unitObject.givePath(vector3);
        this.repathTimer = 1f;
      }
      if ((double) this.attackTimer != -1.0 && this.CurrentState != Interaction_WolfBase.State.Fleeing)
      {
        this.attackTimer += Time.deltaTime;
        if ((double) this.attackTimer > 5.0)
        {
          this.attackTimer = -1f;
          this.repathTimer = 0.0f;
          this.CurrentState = Interaction_WolfBase.State.Animating;
          Vector3 normalized2 = (this.target.transform.position - this.transform.position).normalized;
          AudioManager.Instance.StopOneShotInstanceEarly(this.howlEventInstance, STOP_MODE.IMMEDIATE);
          AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/vocals_shared/dog_basic_small/warning", this.transform.position);
          this.spine.AnimationState.SetAnimation(0, "jump_anticipation", false);
          this.spine.AnimationState.AddAnimation(0, "jump", false, 0.0f);
          GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
          {
            if (this.CurrentState != Interaction_WolfBase.State.Animating)
              return;
            AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/dog_basic_mv_fence_jump_launch", this.transform.position);
          }));
          this.transform.DOMove(this.transform.position + normalized2 * 2f, 0.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1f);
          GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
          {
            if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
              this.spine.AnimationState.SetAnimation(0, "jump_land", false);
            GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
            {
              if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
              {
                this.spine.AnimationState.SetAnimation(0, this.RunAnimation, true);
                this.spine.transform.localPosition = Vector3.zero;
              }
              if (this.CurrentState == Interaction_WolfBase.State.Fleeing)
                return;
              this.CurrentState = Interaction_WolfBase.State.Attacking;
            }));
          }));
        }
        else if (this.spine.AnimationState.GetCurrent(0) == null || this.spine.AnimationState.GetCurrent(0).Animation.Name != "howl")
        {
          this.spine.AnimationState.SetAnimation(0, "howl", false);
          this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          AudioManager.Instance.StopOneShotInstanceEarly(this.howlEventInstance, STOP_MODE.IMMEDIATE);
          this.howlEventInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/dlc/dungeon05/enemy/vocals_shared/dog_basic_small/howl", this.transform);
        }
      }
      if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.target.CurrentState == Interaction_Ranchable.State.BreakingOut)
      {
        this.CurrentState = Interaction_WolfBase.State.Attacking;
        this.repathTimer = 0.0f;
      }
    }
    else if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.CurrentState != Interaction_WolfBase.State.Fleeing && this.target.CurrentState != Interaction_Ranchable.State.Dead)
    {
      float num = Vector3.Distance(this.target.transform.position, this.transform.position);
      this.repathTimer -= Time.deltaTime;
      if ((double) this.repathTimer <= 0.0)
      {
        this.repathTimer = 1f;
        if ((double) num < 2.0)
          this.repathTimer = 0.25f;
        this.unitObject.givePath(this.target.transform.position, this.target.gameObject);
      }
      if ((double) num < 0.5 && !this.target.UnitObject.DisableForces)
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/dog_basic_attack_bite", this.transform.position);
        if (this.target.Damage(this.gameObject))
        {
          Interaction_WolfBase.WolfEvent onWolvesFailed = Interaction_WolfBase.OnWolvesFailed;
          if (onWolvesFailed != null)
            onWolvesFailed();
          Interaction_WolfBase.ResetWolvesEnounterData();
          foreach (Interaction_WolfBase wolf in Interaction_WolfBase.Wolfs)
          {
            if ((UnityEngine.Object) wolf != (UnityEngine.Object) this)
              wolf.Flee();
          }
        }
        this.unitObject.ClearPaths();
        this.repathTimer = 1f;
      }
    }
    else if (((UnityEngine.Object) this.target == (UnityEngine.Object) null || this.target.CurrentState == Interaction_Ranchable.State.Dead) && this.CurrentState != Interaction_WolfBase.State.Fleeing)
    {
      this.stateMachine.facingAngle = this.stateMachine.LookAngle = Mathf.Repeat(this.stateMachine.facingAngle + 180f, 360f);
      this.Flee();
    }
    if (this.CurrentState == Interaction_WolfBase.State.Fleeing && ((double) this.transform.position.x < -35.0 || (double) this.transform.position.x > 35.0))
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if (this.previousPosition == this.transform.position && this.spine.AnimationState.GetCurrent(0).Animation.Name != "idle" && this.spine.AnimationState.GetCurrent(0).Animation.Name != "howl")
      this.spine.AnimationState.SetAnimation(0, "idle", true);
    else if (this.previousPosition != this.transform.position && this.spine.AnimationState.GetCurrent(0).Animation.Name != this.RunAnimation)
      this.spine.AnimationState.SetAnimation(0, this.RunAnimation, true);
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.CurrentState != Interaction_WolfBase.State.Fleeing)
      this.stateMachine.facingAngle = Utils.GetAngle(this.transform.position, this.target.transform.position);
    else if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.CurrentState == Interaction_WolfBase.State.Fleeing)
      this.stateMachine.facingAngle = Utils.GetAngle(this.target.transform.position, this.transform.position);
    if ((double) (this.followerCheckerTimer -= Time.deltaTime) <= 0.0)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (follower.Brain.CurrentTaskType != FollowerTaskType.Drunk && !follower.Brain.HasTrait(FollowerTrait.TraitType.WolfHater) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID, excludeHotTub: true) && (double) Vector3.Distance(this.transform.position, follower.transform.position) < 2.0)
          follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_RunAway(this.gameObject));
      }
      this.followerCheckerTimer = 2f;
    }
    this.previousPosition = this.transform.position;
  }

  public void OnEndPath()
  {
    if (this.CurrentState == Interaction_WolfBase.State.Preying)
      this.attackTimer = 0.0f;
    else
      this.repathTimer = 0.0f;
    if (!((UnityEngine.Object) this.trap != (UnityEngine.Object) null))
      return;
    if (!this.contributedToCounter)
    {
      ++Interaction_WolfBase.WolfDied;
      Interaction_WolfBase.WolfEvent onWolfDied = Interaction_WolfBase.OnWolfDied;
      if (onWolfDied != null)
        onWolfDied();
      this.CheckWolvesSuccess();
      this.HandleNextWolf();
    }
    this.contributedToCounter = true;
    this.CurrentState = Interaction_WolfBase.State.Animating;
    this.spine.AnimationState.SetAnimation(0, "trapped", true);
    this.trap.TrappedWolf(2.2f);
    this.SecondaryInteractable = false;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/trap_hop", this.transform.position);
    GameManager.GetInstance().WaitForSeconds(2.2f, (System.Action) (() =>
    {
      if (!((UnityEngine.Object) this.trap != (UnityEngine.Object) null))
        return;
      AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/death_poof", this.transform.position);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
  }

  public Vector3 GetTargetFencePosition()
  {
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (this.target.transform.position - this.transform.position).normalized, 100f, (int) this.fenceMask);
    if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
      return Vector3.zero;
    RanchFence component = raycastHit2D.collider.GetComponent<RanchFence>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.Connected ? (Vector3) raycastHit2D.point + (this.transform.position - (Vector3) raycastHit2D.point).normalized * 0.25f : this.GetTargetFencePosition((Vector3) raycastHit2D.point);
  }

  public Vector3 GetTargetFencePosition(Vector3 fromPos)
  {
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) fromPos, (Vector2) (this.target.transform.position - this.transform.position).normalized, 100f, (int) this.fenceMask);
    if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
      return fromPos;
    RanchFence component = raycastHit2D.collider.GetComponent<RanchFence>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.Connected ? (Vector3) raycastHit2D.point + (this.transform.position - (Vector3) raycastHit2D.point).normalized * 0.25f : this.GetTargetFencePosition((Vector3) raycastHit2D.point + (this.target.transform.position - this.transform.position).normalized * 1f);
  }

  public Interaction_WolfTrap GetTargetTrap(Vector3 position)
  {
    if ((UnityEngine.Object) this.trap != (UnityEngine.Object) null)
      return this.trap;
    foreach (Interaction_WolfTrap trap in Interaction_WolfTrap.Traps)
    {
      if (trap.Structure.Brain != null && !trap.Structure.Brain.Data.HasBird && trap.Structure.Brain.Data.Inventory.Count > 0 && trap.Structure.Brain.Data.Inventory[0].type != 86 && !trap.Structure.Brain.ReservedForTask && (double) Vector3.Distance(position, trap.transform.position) < 7.0)
      {
        RaycastHit2D raycastHit2D = Physics2D.Linecast((Vector2) this.transform.position, (Vector2) trap.transform.position, (int) this.fenceMask);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          RanchFence component = raycastHit2D.collider.GetComponent<RanchFence>();
          if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component.Connected)
          {
            trap.Structure.Brain.ReservedForTask = true;
            return trap;
          }
        }
        else
        {
          trap.Structure.Brain.ReservedForTask = true;
          return trap;
        }
      }
    }
    return (Interaction_WolfTrap) null;
  }

  public static Interaction_WolfBase GetClosestWolf(Vector3 position)
  {
    Interaction_WolfBase closestWolf = (Interaction_WolfBase) null;
    float num1 = float.PositiveInfinity;
    foreach (Interaction_WolfBase wolf in Interaction_WolfBase.Wolfs)
    {
      float num2 = Vector2.Distance((Vector2) position, (Vector2) wolf.transform.position);
      if ((double) num2 < (double) num1)
      {
        closestWolf = wolf;
        num1 = num2;
      }
    }
    return closestWolf;
  }

  public void HandleNextWolf()
  {
    if (Interaction_WolfBase.WolfCount >= Interaction_WolfBase.WolfTarget || Interaction_WolfBase.WolfFled == -1 || Interaction_WolfBase.WolfDied == -1 || !((UnityEngine.Object) this.target != (UnityEngine.Object) null))
      return;
    Interaction_WolfBase.SpawnWolf(new Vector3((double) UnityEngine.Random.value < 0.5 ? PlacementRegion.X_Constraints.x - 10f : PlacementRegion.X_Constraints.y + 10f, (UnityEngine.Object) this.target != (UnityEngine.Object) null ? this.target.transform.position.y : this.transform.position.y), this.target, true, (System.Action<Interaction_WolfBase>) (newWolf => { }));
  }

  public void CheckWolvesSuccess()
  {
    if (Interaction_WolfBase.WolfFled + Interaction_WolfBase.WolfDied < Interaction_WolfBase.WolfTarget || Interaction_WolfBase.WolfFled == -1 || Interaction_WolfBase.WolfDied == -1 || Interaction_WolfBase.WolfTarget == -1)
      return;
    Interaction_WolfBase.WolfEvent onWolvesSucceeded = Interaction_WolfBase.OnWolvesSucceeded;
    if (onWolvesSucceeded != null)
      onWolvesSucceeded();
    foreach (Interaction_WolfBase wolf in Interaction_WolfBase.Wolfs)
    {
      if ((UnityEngine.Object) wolf != (UnityEngine.Object) this)
        wolf.Flee();
    }
  }

  public static void ResetWolvesEnounterData()
  {
    Interaction_WolfBase.WolfTarget = -1;
    Interaction_WolfBase.WolfDied = -1;
    Interaction_WolfBase.WolfFled = -1;
  }

  public bool IsWithinScreenView()
  {
    Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(this.transform.position);
    return (double) screenPoint.x > 0.0 & (double) screenPoint.x < (double) Screen.width && (double) screenPoint.y > 0.0 && (double) screenPoint.y < (double) (Screen.height - 100);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__60_0()
  {
    if (this.CurrentState != Interaction_WolfBase.State.Animating)
      return;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/dog_basic_mv_fence_jump_launch", this.transform.position);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__60_1()
  {
    if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
      this.spine.AnimationState.SetAnimation(0, "jump_land", false);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
      {
        this.spine.AnimationState.SetAnimation(0, this.RunAnimation, true);
        this.spine.transform.localPosition = Vector3.zero;
      }
      if (this.CurrentState == Interaction_WolfBase.State.Fleeing)
        return;
      this.CurrentState = Interaction_WolfBase.State.Attacking;
    }));
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__60_2()
  {
    if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
    {
      this.spine.AnimationState.SetAnimation(0, this.RunAnimation, true);
      this.spine.transform.localPosition = Vector3.zero;
    }
    if (this.CurrentState == Interaction_WolfBase.State.Fleeing)
      return;
    this.CurrentState = Interaction_WolfBase.State.Attacking;
  }

  [CompilerGenerated]
  public void \u003COnEndPath\u003Eb__61_0()
  {
    if (!((UnityEngine.Object) this.trap != (UnityEngine.Object) null))
      return;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/death_poof", this.transform.position);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public enum State
  {
    Preying,
    Attacking,
    Fleeing,
    Animating,
  }

  public delegate void WolfEvent();
}
