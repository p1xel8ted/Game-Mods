// Decompiled with JetBrains decompiler
// Type: EnemyLightningTeleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class EnemyLightningTeleporter : UnitObject
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash simpleSpineFlash;
  public SpriteRenderer Shadow;
  public ParticleSystem teleportEffect;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string TeleportAnimation;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string TeleportAwayVO = "event:/enemy/teleport_away";
  [EventRef]
  public string TeleportAppearVO = "event:/enemy/teleport_appear";
  public float TeleportDelayMin = 3f;
  public float TeleportDelayMax = 5f;
  public float TeleportFleeRadius = 2.5f;
  public float TeleportFleeDelayMax = 2f;
  public float TeleportMinDistance = 4f;
  public float TeleportMaxDistance = 7f;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool shorterTeleportSearch;
  public int shorterTeleportSteps = 12;
  public float FleeRange = 1f;
  public float KnockbackSpeed = 0.2f;
  public float LightningEnemyDamage = 4f;
  public float LightningExpansionSpeed = 5f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public EnemyLightningTeleporter.LightningTeleporterStateMachine logicStateMachine;
  public GameObject TargetObject;
  public CircleCollider2D CircleCollider;
  public float StartSpeed = 0.4f;
  public string LoopedSoundSFX;
  public EventInstance LoopedSound;
  public float teleportDelay = 1f;
  public float teleportBuffer;
  public float fleeDelay = 1f;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public override void Awake()
  {
    base.Awake();
    this.SetupStateMachine();
  }

  public void SetupStateMachine()
  {
    this.logicStateMachine = new EnemyLightningTeleporter.LightningTeleporterStateMachine();
    this.logicStateMachine.SetParent(this);
    this.logicStateMachine.SetState((SimpleState) new EnemyLightningTeleporter.IdleState());
  }

  public void Start()
  {
    this.SeperateObject = true;
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.CircleCollider = this.GetComponent<CircleCollider2D>();
  }

  public new virtual void Update()
  {
    base.Update();
    this.logicStateMachine.Update();
    this.teleportBuffer -= Time.deltaTime * this.Spine.timeScale;
  }

  public void ResetTeleportBuffer() => this.teleportBuffer = this.teleportDelay;

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Teleport"))
      return;
    this.Teleport();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    if (this.LoopedSoundSFX.IsNullOrEmpty())
      return;
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.LoopedSoundSFX.IsNullOrEmpty())
      return;
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.simpleSpineFlash.FlashWhite(false);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.LoopedSoundSFX.IsNullOrEmpty())
      AudioManager.Instance.StopLoop(this.LoopedSound);
    if (this.state.CURRENT_STATE == StateMachine.State.Dieing)
      return;
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public void Teleport()
  {
    this.CircleCollider.enabled = true;
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      this.TargetObject = PlayerFarming.FindClosestPlayerGameObject(this.transform.position);
    if (this.shorterTeleportSearch)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = UnityEngine.Random.Range(this.TeleportMinDistance, this.TeleportMaxDistance);
      for (int index = 0; index < this.shorterTeleportSteps; ++index)
      {
        Vector3 vector3 = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.TargetObject.transform.position, this.CircleCastRadius, (Vector2) Vector3.Normalize(vector3 - this.TargetObject.transform.position), distance, (int) this.layerToCheck);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(this.TargetObject.transform.position, (Vector3) raycastHit2D.centroid) > (double) this.TeleportMinDistance)
          {
            if (this.ShowDebug)
            {
              this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
              this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
            }
            this.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.TargetObject.transform.position - vector3) * this.CircleCastOffset;
            break;
          }
          f += (float) (360.0 / (double) this.shorterTeleportSteps * (Math.PI / 180.0));
        }
        else
        {
          if (this.ShowDebug)
          {
            this.EndPoints.Add(new Vector3(vector3.x, vector3.y));
            this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
          }
          this.transform.position = vector3;
          break;
        }
      }
    }
    else
    {
      float num = 100f;
      while ((double) --num > 0.0)
      {
        float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
        float distance = UnityEngine.Random.Range(this.TeleportMinDistance, this.TeleportMaxDistance);
        Vector3 vector3 = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.TargetObject.transform.position, this.CircleCastRadius, (Vector2) Vector3.Normalize(vector3 - this.TargetObject.transform.position), distance, (int) this.layerToCheck);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(this.TargetObject.transform.position, (Vector3) raycastHit2D.centroid) > (double) this.TeleportMinDistance)
          {
            if (this.ShowDebug)
            {
              this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
              this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
            }
            this.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.TargetObject.transform.position - vector3) * this.CircleCastOffset;
            break;
          }
        }
        else
        {
          if (this.ShowDebug)
          {
            this.EndPoints.Add(new Vector3(vector3.x, vector3.y));
            this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
          }
          this.transform.position = vector3;
          break;
        }
      }
    }
    this.logicStateMachine.SetState((SimpleState) new EnemyLightningTeleporter.TeleportAppearState());
  }

  public void DoLightningExplosion(Vector3 position)
  {
    LightningRingExplosion.CreateExplosion(position, this.health.team, this.health, this.LightningExpansionSpeed, 1f, this.LightningEnemyDamage);
  }

  public void OnDrawGizmos()
  {
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.CircleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.CircleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  public class LightningTeleporterStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemyLightningTeleporter \u003CParent\u003Ek__BackingField;

    public EnemyLightningTeleporter Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemyLightningTeleporter parent) => this.Parent = parent;
  }

  public class IdleState : SimpleState
  {
    public EnemyLightningTeleporter parent;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningTeleporter.LightningTeleporterStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      if (!this.parent.LoopedSoundSFX.IsNullOrEmpty())
        this.parent.LoopedSound = AudioManager.Instance.CreateLoop(this.parent.LoopedSoundSFX, this.parent.Spine.gameObject, true);
      if ((double) this.parent.teleportDelay > 0.0)
        return;
      this.parent.teleportDelay = UnityEngine.Random.Range(this.parent.TeleportDelayMin, this.parent.TeleportDelayMax);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if ((double) this.parent.teleportBuffer > 0.0)
        return;
      this.parent.teleportDelay -= Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.parent.teleportDelay > 0.0 && (double) PlayerFarming.GetClosestPlayerDist(this.parent.transform.position) >= (double) this.parent.FleeRange)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyLightningTeleporter.TeleportState());
    }

    public override void OnExit()
    {
    }
  }

  public class TeleportState : SimpleState
  {
    public EnemyLightningTeleporter parent;
    public bool hasStartedTeleporting;
    public float teleportDelay = 0.15f;
    public float moveToNextStateTime = 0.8f;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningTeleporter.LightningTeleporterStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Teleporting;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress > (double) this.teleportDelay && !this.hasStartedTeleporting)
      {
        this.hasStartedTeleporting = true;
        this.parent.Shadow.enabled = false;
        AudioManager.Instance.PlayOneShot(this.parent.TeleportAwayVO, this.parent.gameObject);
        this.parent.teleportEffect.Play();
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.TeleportAnimation, false);
        this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
        this.parent.CircleCollider.enabled = false;
      }
      if ((double) this.progress <= 0.800000011920929)
        return;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemyLightningTeleporter.TeleportAppearState());
    }

    public override void OnExit()
    {
    }
  }

  public class TeleportAppearState : SimpleState
  {
    public EnemyLightningTeleporter parent;
    public bool hasAttacked;
    public float timeBeforeLightning = 0.5f;
    public float timeBeforeIdle = 0.6f;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningTeleporter.LightningTeleporterStateMachine) this.parentStateMachine).Parent;
      this.parent.Shadow.enabled = true;
      this.parent.LookAtTarget();
      this.parent.teleportEffect.Play();
      AudioManager.Instance.PlayOneShot(this.parent.TeleportAppearVO, this.parent.gameObject);
      this.parent.ResetTeleportBuffer();
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if (!this.hasAttacked && (double) this.progress > (double) this.timeBeforeLightning)
      {
        this.hasAttacked = true;
        this.DoLightingAttack();
      }
      if ((double) this.progress <= (double) this.timeBeforeIdle)
        return;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemyLightningTeleporter.IdleState());
    }

    public void DoLightingAttack()
    {
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.AttackAnimation, false);
      AudioManager.Instance.PlayOneShot(this.parent.AttackVO, this.parent.gameObject);
      this.parent.DoLightningExplosion(this.parent.transform.position);
    }

    public override void OnExit()
    {
    }
  }
}
