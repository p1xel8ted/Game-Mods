// Decompiled with JetBrains decompiler
// Type: CritterBaseBird
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterBaseBird : BaseMonoBehaviour
{
  public static List<CritterBaseBird> Birds = new List<CritterBaseBird>();
  public CritterBaseBird.State CurrentState;
  private float RandomDelay;
  public Vector3 FlyOutPosition;
  private float Progress;
  public Vector3 TargetPosition;
  private Health health;
  private Vector3 PrevPosition;
  private float Angle;
  private float Duration;
  private float FlipTimer;
  public SkeletonAnimation bird;
  private EventInstance loopingSoundInstance;
  private bool createdLoop;

  public bool ManualControl { get; set; }

  public Vector2 FlipTimerIntervals { get; set; } = new Vector2(3f, 8f);

  public float EatChance { get; set; } = 0.5f;

  private void OnEnable()
  {
    CritterBaseBird.Birds.Add(this);
    this.Start();
  }

  private void OnDisable() => CritterBaseBird.Birds.Remove(this);

  private void OnBecameInvisible()
  {
    if (this.ManualControl)
      return;
    this.bird.enabled = false;
  }

  public void OnBecameVisible()
  {
    if (this.ManualControl)
      return;
    this.bird.enabled = true;
  }

  private void Start()
  {
    this.TargetPosition = this.transform.position;
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.health = this.GetComponent<Health>();
    this.NewFlyOutPosition((float) UnityEngine.Random.Range(0, 360));
    this.RandomDelay = UnityEngine.Random.Range(0.0f, 240f) / 2f;
    this.Duration = UnityEngine.Random.Range(120f, 360f) / 2f;
    this.transform.position = this.FlyOutPosition;
    this.bird.state.SetAnimation(0, "FLY", true);
    this.FlipTimer = UnityEngine.Random.Range(0.5f, 2f);
    this.bird.enabled = false;
  }

  private void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);

  private void OnNewPhaseStarted()
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

  private void CheckForPlayer()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) > 2.0)
      return;
    this.NewFlyOutPosition(Utils.GetAngle(PlayerFarming.Instance.transform.position, this.transform.position));
    this.FlyOut();
  }

  private void NewFlyOutPosition(float Angle)
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

  private void Update()
  {
    if ((UnityEngine.Object) this.bird == (UnityEngine.Object) null || this.bird.state == null)
      return;
    this.PrevPosition = this.transform.position;
    switch (this.CurrentState)
    {
      case CritterBaseBird.State.WaitingToArrive:
        this.health.enabled = false;
        if ((double) (this.RandomDelay -= Time.deltaTime) < 0.0 && !this.ManualControl)
        {
          this.bird.enabled = true;
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
    this.Angle = Mathf.Repeat(Utils.GetAngle(this.PrevPosition, this.transform.position), 360f);
    this.transform.localScale = new Vector3((double) this.Angle <= 90.0 || (double) this.Angle >= 270.0 ? -1f : 1f, 1f, 1f);
  }

  public enum State
  {
    WaitingToArrive,
    FlyingIn,
    Idle,
    FlyingOut,
  }
}
