// Decompiled with JetBrains decompiler
// Type: CritterBaseBird
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CritterBaseBird : BaseMonoBehaviour
{
  public static List<CritterBaseBird> Birds = new List<CritterBaseBird>();
  public CritterBaseBird.State CurrentState;
  public float RandomDelay;
  public Vector3 FlyOutPosition;
  public float Progress;
  public Vector3 TargetPosition;
  public Health health;
  public Vector3 PrevPosition;
  public float Angle;
  public float Duration;
  public float FlipTimer;
  public SkeletonAnimation bird;
  [HideInInspector]
  public SkeletonAnimationLODManager skeletonAnimationLODManager;
  [CompilerGenerated]
  public bool \u003CManualControl\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector2 \u003CFlipTimerIntervals\u003Ek__BackingField = new Vector2(3f, 8f);
  [CompilerGenerated]
  public float \u003CEatChance\u003Ek__BackingField = 0.5f;
  public EventInstance loopingSoundInstance;
  public bool createdLoop;

  public bool ManualControl
  {
    get => this.\u003CManualControl\u003Ek__BackingField;
    set => this.\u003CManualControl\u003Ek__BackingField = value;
  }

  public Vector2 FlipTimerIntervals
  {
    get => this.\u003CFlipTimerIntervals\u003Ek__BackingField;
    set => this.\u003CFlipTimerIntervals\u003Ek__BackingField = value;
  }

  public float EatChance
  {
    get => this.\u003CEatChance\u003Ek__BackingField;
    set => this.\u003CEatChance\u003Ek__BackingField = value;
  }

  public void OnEnable()
  {
    CritterBaseBird.Birds.Add(this);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public void OnDisable()
  {
    CritterBaseBird.Birds.Remove(this);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  public void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);

  public void OnBecameInvisible()
  {
    if (this.ManualControl || !((UnityEngine.Object) this.skeletonAnimationLODManager != (UnityEngine.Object) null))
      return;
    this.skeletonAnimationLODManager.DoUpdate = false;
  }

  public void OnBecameVisible()
  {
    if (this.ManualControl || !((UnityEngine.Object) this.skeletonAnimationLODManager != (UnityEngine.Object) null))
      return;
    this.skeletonAnimationLODManager.DoUpdate = true;
  }

  public void Awake()
  {
    if (!((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null))
      return;
    this.skeletonAnimationLODManager = SkeletonAnimationLODGlobalManager.Instance.AddCustomLOD(this.transform, this.bird);
  }

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.NewFlyOutPosition((float) UnityEngine.Random.Range(0, 360));
  }

  public void PrepareForSpawn()
  {
    this.CurrentState = CritterBaseBird.State.WaitingToArrive;
    this.Progress = 0.0f;
    this.TargetPosition.z = 0.0f;
    this.NewFlyOutPosition((float) UnityEngine.Random.Range(0, 360));
    this.RandomDelay = UnityEngine.Random.Range(0.0f, 240f) / 2f;
    this.Duration = Mathf.Max(UnityEngine.Random.Range(120f, 360f) / 2f, 3f);
    this.transform.position = this.TargetPosition + this.FlyOutPosition;
    this.bird.state.SetAnimation(0, "FLY", true);
    this.FlipTimer = UnityEngine.Random.Range(0.5f, 2f);
    if (!((UnityEngine.Object) this.skeletonAnimationLODManager != (UnityEngine.Object) null))
      return;
    this.skeletonAnimationLODManager.DoUpdate = false;
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawLine(this.transform.position, this.TargetPosition);
    Utils.DrawCircleXY(this.TargetPosition, 0.5f, Color.red);
  }

  public void OnNewPhaseStarted()
  {
    if (TimeManager.CurrentPhase != DayPhase.Night || this.ManualControl)
      return;
    switch (this.CurrentState)
    {
      case CritterBaseBird.State.WaitingToArrive:
        TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
        if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
          break;
        this.gameObject.Recycle();
        break;
      case CritterBaseBird.State.FlyingIn:
      case CritterBaseBird.State.Idle:
        this.RandomDelay = UnityEngine.Random.Range(0.0f, 3f);
        this.NewFlyOutPosition((float) UnityEngine.Random.Range(0, 360));
        this.FlyOut();
        break;
    }
  }

  public void CheckForPlayer()
  {
    if (!this.health.enabled || (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) Vector3.Distance(this.transform.position, player.transform.position) <= 2.0)
      {
        this.NewFlyOutPosition(Utils.GetAngle(player.transform.position, this.transform.position));
        this.FlyOut();
      }
    }
  }

  public void NewFlyOutPosition(float Angle)
  {
    float num = UnityEngine.Random.Range(6f, 10f);
    Angle *= (float) Math.PI / 180f;
    this.FlyOutPosition = new Vector3(num * Mathf.Cos(Angle), num * Mathf.Sin(Angle), -12f);
  }

  public void FlyOut()
  {
    if ((UnityEngine.Object) this.bird == (UnityEngine.Object) null || this.bird.state == null)
      return;
    AudioManager.Instance.PlayOneShot("event:/birds/bird_fly_away", this.gameObject);
    this.bird.state.SetAnimation(0, "FLY", true);
    this.Progress = 0.0f;
    this.CurrentState = CritterBaseBird.State.FlyingOut;
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.bird == (UnityEngine.Object) null)
    {
      if (this.bird.state != null)
        return;
      this.bird.gameObject.SetActive(false);
    }
    else
    {
      this.bird.gameObject.SetActive(this.CurrentState != 0);
      this.PrevPosition = this.transform.position;
      switch (this.CurrentState)
      {
        case CritterBaseBird.State.WaitingToArrive:
          this.health.enabled = false;
          if ((double) (this.RandomDelay -= Time.deltaTime) < 0.0 && !this.ManualControl)
          {
            this.bird.gameObject.SetActive(true);
            if ((UnityEngine.Object) this.skeletonAnimationLODManager != (UnityEngine.Object) null)
              this.skeletonAnimationLODManager.DoUpdate = true;
            this.CurrentState = CritterBaseBird.State.FlyingIn;
            break;
          }
          break;
        case CritterBaseBird.State.FlyingIn:
          if ((double) this.Progress < 1.0)
          {
            this.transform.position = Vector3.Lerp(this.TargetPosition + this.FlyOutPosition, this.TargetPosition, this.Progress += Time.deltaTime * 0.5f);
            this.health.enabled = (double) this.Progress > 0.800000011920929;
            break;
          }
          this.bird.state.SetAnimation(0, "IDLE", true);
          this.CurrentState = CritterBaseBird.State.Idle;
          break;
        case CritterBaseBird.State.Idle:
          if ((double) (this.FlipTimer -= Time.deltaTime) < 0.0)
          {
            this.FlipTimer = UnityEngine.Random.Range(this.FlipTimerIntervals.x, this.FlipTimerIntervals.y);
            if ((double) UnityEngine.Random.value <= 0.5)
              this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, 1f, 1f);
            if ((double) UnityEngine.Random.value >= (double) this.EatChance)
              this.bird.state.SetAnimation(0, "IDLE", true);
            else
              this.bird.state.SetAnimation(0, "EAT", true);
          }
          if ((double) (this.Duration -= Time.deltaTime) < 0.0 && !this.ManualControl)
          {
            this.FlyOut();
            break;
          }
          this.CheckForPlayer();
          break;
        case CritterBaseBird.State.FlyingOut:
          if ((double) this.Progress < 1.0)
          {
            this.transform.position = Vector3.Lerp(this.TargetPosition, this.TargetPosition + this.FlyOutPosition, Mathf.SmoothStep(0.0f, 1f, this.Progress += Time.deltaTime * 0.6f));
            this.health.enabled = (double) this.Progress <= 0.20000000298023224;
            break;
          }
          TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
          this.gameObject.Recycle();
          break;
      }
      if (!(this.transform.position != this.PrevPosition) || this.CurrentState == CritterBaseBird.State.Idle)
        return;
      this.Angle = Utils.GetAngle(this.PrevPosition, this.transform.position);
      this.transform.localScale = new Vector3((double) this.Angle <= 90.0 || (double) this.Angle >= 270.0 ? -1f : 1f, 1f, 1f);
    }
  }

  public enum State
  {
    WaitingToArrive,
    FlyingIn,
    Idle,
    FlyingOut,
  }
}
