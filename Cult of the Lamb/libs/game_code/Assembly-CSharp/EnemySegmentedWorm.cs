// Decompiled with JetBrains decompiler
// Type: EnemySegmentedWorm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using MMRoomGeneration;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemySegmentedWorm : UnitObject
{
  [SerializeField]
  public float headHealth = 5f;
  [SerializeField]
  public float noTailHealth = 1f;
  public ColliderEvents damageColliderEvents;
  public GameObject trapLavaPrefab;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SpineSkin("", "SkeletonData", true, false, false)]
  public string HeadSkin;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HeadAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string TailAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MoveAnimation;
  [EventRef]
  public string MoveSFX = "event:/dlc/dungeon06/enemy/segmentedworm/move";
  [EventRef]
  public string TransformSFX = "event:/dlc/dungeon06/enemy/segmentedworm/mv_split";
  [EventRef]
  public string TransformFinalSFX = "event:/dlc/dungeon06/enemy/segmentedworm/mv_finalsegment";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/gethit";
  public EnemySegmentedWormManager wormManager;
  public EnemySegmentedWorm.SegmentedWormStateMachine logicStateMachine;
  public EnemySegmentedWorm followTarget;
  public EnemySegmentedWorm currentHead;
  public float lavalLifetime = 10f;
  public int moveCount;
  public int movesBeforeDirectionChange;
  [CompilerGenerated]
  public float \u003CCurrentMoveForce\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CCurrentMoveFrequency\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CCurrentTelegraphTime\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CCurrentSplitStunnedDuration\u003Ek__BackingField;
  public bool isHead;
  public int Children;
  public List<EnemySegmentedWorm> TailSegments = new List<EnemySegmentedWorm>();

  public override float timeStopMultiplier => 0.0f;

  public float CurrentMoveForce
  {
    get => this.\u003CCurrentMoveForce\u003Ek__BackingField;
    set => this.\u003CCurrentMoveForce\u003Ek__BackingField = value;
  }

  public float CurrentMoveFrequency
  {
    get => this.\u003CCurrentMoveFrequency\u003Ek__BackingField;
    set => this.\u003CCurrentMoveFrequency\u003Ek__BackingField = value;
  }

  public float CurrentTelegraphTime
  {
    get => this.\u003CCurrentTelegraphTime\u003Ek__BackingField;
    set => this.\u003CCurrentTelegraphTime\u003Ek__BackingField = value;
  }

  public float CurrentSplitStunnedDuration
  {
    get => this.\u003CCurrentSplitStunnedDuration\u003Ek__BackingField;
    set => this.\u003CCurrentSplitStunnedDuration\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    this.wormManager = this.GetComponentInParent<EnemySegmentedWormManager>();
    this.SetupStateMachine();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(true);
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, this.HeadAnimation, true);
  }

  public new virtual void Update()
  {
    base.Update();
    this.logicStateMachine.Update();
  }

  public void SetAsHead(bool playSFX, bool isNewlySplit)
  {
    if (isNewlySplit)
      this.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadStunnedState(this.CurrentSplitStunnedDuration));
    else
      this.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadIdleState());
    if (this.isHead)
      return;
    this.isHead = true;
    this.Spine.AnimationState.SetAnimation(0, this.HeadAnimation, true);
    this.Spine.skeleton.SetSkin(this.HeadSkin);
    this.followTarget = (EnemySegmentedWorm) null;
    if (playSFX && !string.IsNullOrEmpty(this.TransformSFX))
      AudioManager.Instance.PlayOneShot(this.TransformSFX, this.transform.position);
    this.health.totalHP = this.headHealth;
    this.health.HP = this.health.totalHP;
    this.GetComponent<ShowHPBar>().enabled = true;
  }

  public void SetChildrenCountOnSplit(int count)
  {
    this.Children = count;
    if (this.Children != 0 || string.IsNullOrEmpty(this.TransformFinalSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.TransformFinalSFX, this.transform.position);
    this.health.HP = this.noTailHealth;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    Vector3 vector3 = new Vector3(this.transform.position.x, this.transform.position.y, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform.position.z : this.transform.position.z);
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropPoison))
    {
      TrapPoison.CreatePoison(vector3, 5, 0.1f, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/player/poison_damage", vector3);
    }
    if (AttackType != Health.AttackTypes.Projectile && AttackType != Health.AttackTypes.Poison && AttackType != Health.AttackTypes.Burn && AttackType != Health.AttackTypes.NoHitStop)
      GameManager.GetInstance().HitStop(0.05f * this.timeStopMultiplier);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.gameObject);
    this.SimpleSpineFlash.FlashFillRed();
    if (this.IsHead())
    {
      this.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadStunnedState(this.wormManager.SingleKnockbackDuration));
      CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.3f, false);
      foreach (EnemySegmentedWorm tailSegment in this.TailSegments)
      {
        if ((UnityEngine.Object) tailSegment != (UnityEngine.Object) null)
        {
          this.SimpleSpineFlash.FlashFillRed();
          float angle = Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f);
          tailSegment.DoKnockBack(angle, this.wormManager.SingleKnockbackModifier, this.wormManager.SingleKnockbackDuration, false);
        }
      }
      this.DoKnockBack(Attacker, this.wormManager.SingleKnockbackModifier, this.wormManager.SingleKnockbackDuration, false);
    }
    else
    {
      if (!((UnityEngine.Object) this.currentHead != (UnityEngine.Object) null) || !(this.currentHead.logicStateMachine.GetCurrentState().GetType() != typeof (EnemySegmentedWorm.HeadStunnedState)))
        return;
      this.currentHead.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    }
  }

  public bool IsHead()
  {
    return this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemySegmentedWorm.HeadIdleState) || this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemySegmentedWorm.HeadTelegraphState) || this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemySegmentedWorm.HeadMoveState) || this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemySegmentedWorm.HeadStunnedState);
  }

  public void SetAsTail(EnemySegmentedWorm followTarget, EnemySegmentedWorm head)
  {
    this.followTarget = followTarget;
    this.currentHead = head;
    this.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.TailFollowState());
    this.Spine.AnimationState.SetAnimation(0, this.TailAnimation, true);
  }

  public void SetupStateMachine()
  {
    this.logicStateMachine = new EnemySegmentedWorm.SegmentedWormStateMachine();
    this.logicStateMachine.SetParent(this);
    this.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.IdleState());
  }

  public void SetTailStunned()
  {
    foreach (EnemySegmentedWorm tailSegment in this.TailSegments)
    {
      if ((UnityEngine.Object) tailSegment != (UnityEngine.Object) null)
        tailSegment.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.TailStunnedState());
    }
  }

  public void SetTailFollow()
  {
    foreach (EnemySegmentedWorm tailSegment in this.TailSegments)
    {
      if ((UnityEngine.Object) tailSegment != (UnityEngine.Object) null)
        tailSegment.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.TailFollowState());
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.IsHead())
    {
      if (!string.IsNullOrEmpty(this.DeathVO))
        AudioManager.Instance.PlayOneShot(this.DeathVO, this.gameObject);
      if (this.TailSegments.Count > 0)
      {
        foreach (EnemySegmentedWorm tailSegment in this.TailSegments)
        {
          if ((UnityEngine.Object) tailSegment != (UnityEngine.Object) null)
            tailSegment.health.DealDamage(999f, Attacker, AttackLocation, AttackType: AttackType, AttackFlags: AttackFlags);
        }
      }
    }
    TrapLava.CreateLava(this.trapLavaPrefab, this.transform.position, this.wormManager.transform.parent, this.health, this.lavalLifetime);
    GameManager.GetInstance().WaitForSeconds(0.0f, new System.Action(this.wormManager.RecalculateSegments));
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public class SegmentedWormStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemySegmentedWorm \u003CParent\u003Ek__BackingField;

    public EnemySegmentedWorm Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemySegmentedWorm parent) => this.Parent = parent;
  }

  public class IdleState : SimpleState
  {
    public override void OnEnter()
    {
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }

  public class HeadIdleState : SimpleState
  {
    public EnemySegmentedWorm parent;
    public float moveTimer;
    public float frequency;

    public override void OnEnter()
    {
      this.parent = ((EnemySegmentedWorm.SegmentedWormStateMachine) this.parentStateMachine).Parent;
      this.frequency = this.parent.CurrentMoveFrequency;
      this.moveTimer = this.frequency;
    }

    public override void Update()
    {
      if ((double) (this.moveTimer += Time.deltaTime * this.parent.Spine.timeScale) < (double) this.frequency)
        return;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadTelegraphState());
    }

    public override void OnExit()
    {
    }
  }

  public class HeadStunnedState : SimpleState
  {
    public EnemySegmentedWorm parent;
    public float duration;
    public float stunnedTimer;

    public HeadStunnedState(float duration) => this.duration = duration;

    public override void OnEnter()
    {
      this.parent = ((EnemySegmentedWorm.SegmentedWormStateMachine) this.parentStateMachine).Parent;
      this.stunnedTimer = 0.0f;
      this.parent.SetTailStunned();
    }

    public override void Update()
    {
      if ((double) (this.stunnedTimer += Time.deltaTime * this.parent.Spine.timeScale) < (double) this.duration)
        return;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadIdleState());
      this.parent.SetTailFollow();
    }

    public override void OnExit()
    {
    }
  }

  public class TailStunnedState : SimpleState
  {
    public EnemySegmentedWorm parent;

    public override void OnEnter()
    {
      this.parent = ((EnemySegmentedWorm.SegmentedWormStateMachine) this.parentStateMachine).Parent;
      this.parent.transform.DOKill();
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }

  public class HeadTelegraphState : SimpleState
  {
    public EnemySegmentedWorm parent;
    public float telegraphTimer;
    public Vector3 startScale;
    public float telegraphTime;

    public override void OnEnter()
    {
      this.parent = ((EnemySegmentedWorm.SegmentedWormStateMachine) this.parentStateMachine).Parent;
      this.startScale = this.parent.transform.localScale;
    }

    public override void Update()
    {
      this.telegraphTime = this.parent.CurrentTelegraphTime;
      if (this.parent.Children <= 0)
        this.telegraphTime = this.parent.wormManager.SingleTelegraphTime;
      else if (this.parent.Children < 5)
        this.telegraphTime -= (float) this.parent.Children * 0.05f;
      this.telegraphTimer += Time.deltaTime * this.parent.Spine.timeScale;
      this.parent.transform.localScale = Vector3.Lerp(this.startScale, this.parent.wormManager.TelegraphScale, this.telegraphTimer / this.telegraphTime);
      if ((double) this.telegraphTimer < (double) this.telegraphTime)
        return;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadMoveState());
    }

    public override void OnExit() => this.parent.transform.localScale = this.startScale;
  }

  public class HeadMoveState : SimpleState
  {
    public EnemySegmentedWorm parent;
    public float directionRand = 1f;
    public float rayDistance = 2f;
    public float moveTimer;
    public float telegraphTimer;

    public override void OnEnter()
    {
      this.parent = ((EnemySegmentedWorm.SegmentedWormStateMachine) this.parentStateMachine).Parent;
      Vector2 normalized = new Vector2(Mathf.Cos(this.parent.state.LookAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.parent.state.LookAngle * ((float) Math.PI / 180f))).normalized;
      if ((bool) Physics2D.Raycast((Vector2) this.parent.transform.position, normalized, this.rayDistance, (int) this.parent.layerToCheck) || this.parent.moveCount >= this.parent.movesBeforeDirectionChange)
      {
        this.parent.moveCount = 0;
        this.parent.movesBeforeDirectionChange = UnityEngine.Random.Range(this.parent.wormManager.DirectionChangeCountRange.x, this.parent.wormManager.DirectionChangeCountRange.y);
        this.directionRand *= -1f;
        float angle1 = Utils.GetAngle(Vector3.zero, (Vector3) normalized);
        float angle2 = angle1;
        int num1 = 360 / this.parent.wormManager.TurnAngle;
        if ((UnityEngine.Object) this.parent.GetClosestTarget() != (UnityEngine.Object) null && (double) UnityEngine.Random.value <= (double) this.parent.wormManager.ChanceToMoveTowardsPlayer)
        {
          for (int index = 0; index < num1; ++index)
          {
            Health closestTarget = this.parent.GetClosestTarget();
            if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
            {
              this.parent.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadIdleState());
              break;
            }
            float degree = Utils.Repeat(Utils.GetAngle(this.parent.transform.position, closestTarget.transform.position) + (float) (index * this.parent.wormManager.TurnAngle) * this.directionRand, 360f);
            if ((UnityEngine.Object) Physics2D.Raycast((Vector2) this.parent.transform.position, Utils.DegreeToVector2(degree).normalized, this.rayDistance, (int) this.parent.layerToCheck).collider == (UnityEngine.Object) null)
            {
              angle2 = degree;
              break;
            }
          }
        }
        else
        {
          for (int index = 1; index < num1; ++index)
          {
            float num2 = Utils.Repeat(angle1 + (float) (index * this.parent.wormManager.TurnAngle) * this.directionRand, 360f);
            if ((UnityEngine.Object) Physics2D.Raycast((Vector2) this.parent.transform.position, Utils.DegreeToVector2(num2).normalized, this.rayDistance, (int) this.parent.layerToCheck).collider == (UnityEngine.Object) null)
            {
              angle2 = Utils.SmoothAngle(angle1, num2, 0.1f);
              break;
            }
          }
        }
        this.parent.LookAtAngle(angle2);
      }
      ++this.parent.moveCount;
      this.parent.DisableForces = true;
      float f = this.parent.state.LookAngle * ((float) Math.PI / 180f);
      this.parent.rb.AddForce(new Vector2(this.parent.CurrentMoveForce * Mathf.Cos(f), this.parent.CurrentMoveForce * Mathf.Sin(f)));
      if (this.parent.Children <= 0)
      {
        this.parent.Spine.transform.DOLocalMoveY(0.5f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
        this.parent.Spine.transform.DOLocalMoveY(0.0f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.1f);
      }
      if (!string.IsNullOrEmpty(this.parent.MoveSFX))
        AudioManager.Instance.PlayOneShot(this.parent.MoveSFX, this.parent.transform.position);
      this.parent.logicStateMachine.SetState((SimpleState) new EnemySegmentedWorm.HeadIdleState());
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }

  public class TailFollowState : SimpleState
  {
    public EnemySegmentedWorm parent;
    public float undulateSpeed = 0.5f;
    public float undulateDistance = 0.1f;
    public float moveLerpTime = 0.1f;
    public float moveDelay = 0.2f;
    public float moveTimer;

    public override void OnEnter()
    {
      this.parent = ((EnemySegmentedWorm.SegmentedWormStateMachine) this.parentStateMachine).Parent;
    }

    public override void Update()
    {
      if (!((UnityEngine.Object) this.parent.followTarget != (UnityEngine.Object) null))
        return;
      Vector3 position = this.parent.followTarget.transform.position;
      Vector3 normalized = (this.parent.transform.position - position).normalized;
      if ((double) Vector3.Distance(position, this.parent.transform.position) <= (double) this.parent.wormManager.TailDistance || (double) (this.moveTimer += Time.deltaTime * this.parent.Spine.timeScale) <= (double) this.moveDelay)
        return;
      this.parent.transform.DOMove(position + normalized * this.parent.wormManager.TailDistance, this.moveLerpTime);
    }

    public override void OnExit()
    {
    }
  }
}
