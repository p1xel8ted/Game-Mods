// Decompiled with JetBrains decompiler
// Type: EnemyTrapLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyTrapLayer : UnitObject
{
  public float FleeDistance = 4f;
  public float TimeBetweenTrapPlant = 5f;
  public float TrapTriggerDelay = 0.5f;
  public int MaxTraps = 6;
  public TrapBombLightning TrapPrefab;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LayTrapAnimation;
  public EnemyTrapLayer.TrapLayerStateMachine logicStateMachine;
  public float turningArc = 90f;
  public float randomDirection;
  public float trapPlantBuffer;

  public new void Awake()
  {
    base.Awake();
    this.SetupStateMachine();
    this.randomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
  }

  public override void Update()
  {
    base.Update();
    this.logicStateMachine.Update();
    if ((double) this.trapPlantBuffer < 0.0)
      return;
    this.trapPlantBuffer -= Time.deltaTime * this.Spine.timeScale;
  }

  public void SetupStateMachine()
  {
    this.logicStateMachine = new EnemyTrapLayer.TrapLayerStateMachine();
    this.logicStateMachine.SetParent(this);
    this.logicStateMachine.SetState((SimpleState) new EnemyTrapLayer.IdleState());
  }

  public Vector3 GetNewWanderPosition(float travelDistance)
  {
    float num = 100f;
    float radius = 0.2f;
    Vector3 newWanderPosition = Vector3.zero;
    while ((double) --num > 0.0)
    {
      this.randomDirection += UnityEngine.Random.Range(-this.turningArc, this.turningArc) * ((float) Math.PI / 180f);
      Vector3 vector3 = this.transform.position + new Vector3(travelDistance * Mathf.Cos(this.randomDirection), travelDistance * Mathf.Sin(this.randomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), travelDistance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.randomDirection = 180f - this.randomDirection;
      }
      else
      {
        newWanderPosition = vector3;
        break;
      }
    }
    return newWanderPosition;
  }

  public Vector3 GetNewFleePosition(float travelDistance)
  {
    float num = 100f;
    float radius = 0.2f;
    Health closestTarget = this.GetClosestTarget();
    Vector3 newFleePosition = Vector3.zero;
    if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
      this.randomDirection = Utils.GetAngle(closestTarget.transform.position, this.transform.position) * ((float) Math.PI / 180f);
    while ((double) --num > 0.0)
    {
      this.randomDirection += UnityEngine.Random.Range(-this.turningArc, this.turningArc) * ((float) Math.PI / 180f);
      Vector3 vector3 = this.transform.position + new Vector3(travelDistance * Mathf.Cos(this.randomDirection), travelDistance * Mathf.Sin(this.randomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), travelDistance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.randomDirection = 180f - this.randomDirection;
      }
      else
      {
        newFleePosition = vector3;
        break;
      }
    }
    return newFleePosition;
  }

  public void ResetTrapPlantTimer() => this.trapPlantBuffer = this.TimeBetweenTrapPlant;

  public class TrapLayerStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemyTrapLayer \u003CParent\u003Ek__BackingField;

    public EnemyTrapLayer Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemyTrapLayer parent) => this.Parent = parent;
  }

  public class IdleState : SimpleState
  {
    public EnemyTrapLayer parent;
    public Vector2 wanderWaitRange = new Vector2(1f, 3f);
    public float wanderTimer;
    public float timeBeforeWander;

    public override void OnEnter()
    {
      this.parent = ((EnemyTrapLayer.TrapLayerStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.ResetTimeBeforeWander();
    }

    public override void Update()
    {
      if (this.IsPlayerWithinFleeRange(PlayerFarming.GetClosestPlayerDist(this.parent.transform.position)))
      {
        this.parentStateMachine.SetState((SimpleState) new EnemyTrapLayer.RunFromPlayerState());
      }
      else
      {
        this.wanderTimer += Time.deltaTime * this.parent.Spine.timeScale;
        if ((double) this.parent.trapPlantBuffer <= 0.0)
        {
          this.parentStateMachine.SetState((SimpleState) new EnemyTrapLayer.LayTrapState());
        }
        else
        {
          if ((double) this.wanderTimer <= (double) this.timeBeforeWander)
            return;
          this.parentStateMachine.SetState((SimpleState) new EnemyTrapLayer.WanderState());
        }
      }
    }

    public override void OnExit()
    {
    }

    public void ResetTimeBeforeWander()
    {
      this.timeBeforeWander = UnityEngine.Random.Range(this.wanderWaitRange.x, this.wanderWaitRange.y);
      this.wanderTimer = 0.0f;
    }

    public bool IsPlayerWithinFleeRange(float closestPlayerDistance)
    {
      return (double) closestPlayerDistance <= (double) this.parent.FleeDistance;
    }
  }

  public class WanderState : SimpleState
  {
    public EnemyTrapLayer parent;
    public Vector3 wanderTarget;
    public Vector2 wanderRange = new Vector2(1f, 3f);

    public override void OnEnter()
    {
      this.parent = ((EnemyTrapLayer.TrapLayerStateMachine) this.parentStateMachine).Parent;
      this.SetNewWanderTarget();
    }

    public override void Update()
    {
      if (this.parent.state.CURRENT_STATE == StateMachine.State.Moving)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyTrapLayer.IdleState());
    }

    public override void OnExit()
    {
    }

    public void SetNewWanderTarget()
    {
      this.wanderTarget = this.parent.GetNewWanderPosition(UnityEngine.Random.Range(this.wanderRange.x, this.wanderRange.y));
      this.parent.givePath(this.wanderTarget);
      float angle = Utils.GetAngle(this.parent.transform.position, this.wanderTarget);
      this.parent.state.facingAngle = angle;
      this.parent.state.LookAngle = angle;
    }
  }

  public class RunFromPlayerState : SimpleState
  {
    public EnemyTrapLayer parent;
    public Vector3 fleeTarget;
    public Vector2 fleeRange = new Vector2(2f, 4f);

    public override void OnEnter()
    {
      this.parent = ((EnemyTrapLayer.TrapLayerStateMachine) this.parentStateMachine).Parent;
      this.SetNewFleeTarget();
    }

    public override void Update()
    {
      if (this.parent.state.CURRENT_STATE == StateMachine.State.Moving)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyTrapLayer.IdleState());
    }

    public override void OnExit()
    {
    }

    public void SetNewFleeTarget()
    {
      this.fleeTarget = this.parent.GetNewFleePosition(UnityEngine.Random.Range(this.fleeRange.x, this.fleeRange.y));
      this.parent.givePath(this.fleeTarget);
      float angle = Utils.GetAngle(this.parent.transform.position, this.fleeTarget);
      this.parent.state.facingAngle = angle;
      this.parent.state.LookAngle = angle;
    }
  }

  public class LayTrapState : SimpleState
  {
    public EnemyTrapLayer parent;
    public float animationDuration;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((EnemyTrapLayer.TrapLayerStateMachine) this.parentStateMachine).Parent;
      this.parent.ResetTrapPlantTimer();
      this.PlaceMine();
      this.animationDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.LayTrapAnimation).Duration;
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.animationDuration)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyTrapLayer.IdleState());
    }

    public override void OnExit()
    {
    }

    public void PlaceMine()
    {
      UnityEngine.Object.Instantiate<TrapBombLightning>(this.parent.TrapPrefab, this.parent.transform.position, Quaternion.identity);
    }
  }
}
