// Decompiled with JetBrains decompiler
// Type: CritterBee
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterBee : BaseMonoBehaviour
{
  public static List<CritterBee> Bees = new List<CritterBee>();
  public bool IsFireFly;
  public static List<CritterBee> FireFlys = new List<CritterBee>();
  public bool IsButterFly;
  public static List<CritterBee> ButterFlys = new List<CritterBee>();
  private Vector3? StartingPosition;
  private Vector3 TargetPosition;
  public float MaximumRange = 5f;
  public float Speed = 0.03f;
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float timestamp;
  public int RanDirection;
  private float Angle;
  public float DirectionChangeDelay;
  private Vector3 NewPosition;
  public bool AvoidPlayer;
  private Vector3 AvoidPlayerDirection = Vector3.zero;
  public float AvoidPlayerDistance = 1f;
  public float AvoidPlayerSafetyDistance = 5f;
  public float AvoidPlayerAccelerationSpeed = 10f;
  public float AvoidPlayerCooldownSpeed = 10f;
  public float AvoidSpeed = 5f;
  public bool AvoidWall;
  public LayerMask LayersToCheck;
  public float RayDistance;
  private Vector3 AvoidWallDirection = Vector3.zero;
  public float AvoidWallSpeed = 5f;
  public SpriteRenderer spriteRenderer;
  public float BaseHeight = 1f;
  public float WobbleHeight = 0.5f;
  public float VerticalWobbleSpeed = 5f;
  public float VerticalNoiseFrequency = 1f;
  public float VerticalNoiseAmplitude = 1f;
  private float VerticalWobble;
  private Vector3 NewHeightPosition;
  public bool StopAndAnimate;
  public Vector2 TimeBetweenAnimations = new Vector2(3f, 4f);
  public Vector2 AnimationDuration = new Vector2(2f, 3f);
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AnimationName;
  public float StopAndAnimateTimer;
  public bool StartRandomPositionWithinMaxRange;
  public bool HideAtnight = true;
  public bool HideAtday;
  public bool CanMove;
  private CritterBee.State CurrentState;
  private float FlyInDuration;
  public float FlyInSpeed = 0.5f;
  public float FlyOutSpeed = 1f;
  public Vector3 FlyOutPosition = new Vector3(0.0f, 0.5f, 0.5f);
  private float RandomOffset;
  private float changeDirectionTimestamp;
  private Vector3 PrevPosition;
  public bool OverrideAnimation;
  public Sprite Frame1;
  public Sprite Frame2;
  public List<Sprite> OverrideFrame1;
  public List<Sprite> OverrideFrame2;
  public int OverrideIndex;

  private void OnEnable()
  {
    if (this.IsFireFly)
      CritterBee.FireFlys.Add(this);
    else if (this.name.IndexOf("Butter") != -1)
    {
      this.IsButterFly = true;
      CritterBee.ButterFlys.Add(this);
    }
    else
      CritterBee.Bees.Add(this);
  }

  private void OnDisable()
  {
    if (this.IsFireFly)
      CritterBee.FireFlys.Remove(this);
    else if (this.IsButterFly)
      CritterBee.ButterFlys.Remove(this);
    else
      CritterBee.Bees.Remove(this);
  }

  private void Start() => this.Setup(this.transform.position);

  public void Setup(Vector3 _TargetPosition)
  {
    this.CanMove = false;
    this.transform.position = _TargetPosition;
    this.TargetPosition = _TargetPosition;
    Debug.Log((object) "Setup critters");
    if (this.StartRandomPositionWithinMaxRange)
      this.transform.position = this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * this.MaximumRange;
    this.timestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.RanDirection = (double) UnityEngine.Random.value < 0.5 ? -1 : 1;
    this.VerticalWobble = (float) UnityEngine.Random.Range(0, 360);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    if (this.OverrideAnimation)
      this.OverrideIndex = UnityEngine.Random.Range(0, this.OverrideFrame1.Count);
    this.CanMove = true;
  }

  private void OnNewPhaseStarted()
  {
    if (!this.HideAtnight && !this.HideAtday || !this.CanMove)
      return;
    if (this.HideAtnight)
    {
      if (TimeManager.CurrentPhase == DayPhase.Night)
        this.FlyAway();
      else if (this.CurrentState == CritterBee.State.FlyingOut)
        this.FlyIn();
    }
    if (!this.HideAtday)
      return;
    if (TimeManager.CurrentPhase != DayPhase.Night)
    {
      this.FlyAway();
    }
    else
    {
      if (this.CurrentState != CritterBee.State.FlyingOut)
        return;
      this.FlyIn();
    }
  }

  private void FlyIn()
  {
    this.FlyInDuration = 0.0f;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.CurrentState = CritterBee.State.FlyingIn;
  }

  private void FlyAway()
  {
    this.FlyInDuration = 0.0f;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 2f);
    this.CurrentState = CritterBee.State.FlyingOut;
  }

  private void Update()
  {
    if (!this.CanMove)
      return;
    float turningSpeed = this.turningSpeed;
    this.Angle = Mathf.LerpAngle(this.Angle, Utils.GetAngle(this.transform.position, this.TargetPosition), Time.deltaTime * turningSpeed);
    if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) this.angleNoiseAmplitude > 0.0 && (double) this.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(this.TargetPosition, this.transform.position) < (double) this.MaximumRange)
      this.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(this.timestamp) * this.angleNoiseFrequency, 0.0f) - 0.5f) * this.angleNoiseAmplitude * (float) this.RanDirection;
    else if ((double) Vector3.Distance(this.TargetPosition, this.transform.position) >= (double) this.MaximumRange)
      this.Angle = Utils.GetAngle(this.transform.position, this.TargetPosition);
    this.NewPosition = new Vector3(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f))) * Time.deltaTime;
    if (this.AvoidPlayer && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      float num = Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position);
      if ((double) num < (double) this.AvoidPlayerDistance)
        this.AvoidPlayerDirection = Vector3.Normalize((this.transform.position - PlayerFarming.Instance.transform.position) with
        {
          z = 0.0f
        }) * this.AvoidSpeed * 0.0166666675f;
      else if ((double) num > (double) this.AvoidPlayerSafetyDistance)
        this.AvoidPlayerDirection = Vector3.Lerp(this.AvoidPlayerDirection, Vector3.zero, this.AvoidPlayerCooldownSpeed * Time.deltaTime);
    }
    if (this.AvoidWall && (bool) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (this.transform.position - this.NewPosition).normalized, this.RayDistance, (int) this.LayersToCheck))
      this.AvoidWallDirection = Vector3.Normalize((Vector3) Utils.DegreeToVector2(Mathf.Repeat(Utils.GetAngle(this.transform.position, this.NewPosition) + UnityEngine.Random.Range(-35f, 35f), 360f)) with
      {
        z = 0.0f
      }) * this.AvoidWallSpeed * 0.0166666675f;
    this.PrevPosition = this.transform.position;
    if ((double) Time.timeScale != 0.0)
      this.transform.position = this.transform.position + this.NewPosition + this.AvoidPlayerDirection + this.AvoidWallDirection;
    this.Angle = Mathf.Repeat(Utils.GetAngle(this.PrevPosition, this.transform.position), 360f);
    if ((double) this.Angle != 0.0 && (double) Time.time > (double) this.changeDirectionTimestamp)
    {
      this.transform.localScale = new Vector3((double) this.Angle <= 90.0 || (double) this.Angle >= 270.0 ? -1f : 1f, 1f, 1f);
      this.changeDirectionTimestamp = Time.time + this.DirectionChangeDelay;
    }
    switch (this.CurrentState)
    {
      case CritterBee.State.FlyingIn:
        if ((double) this.FlyInDuration < 1.0 && (double) (this.RandomOffset -= Time.deltaTime) < 0.0)
          this.NewHeightPosition = Vector3.Lerp(this.FlyOutPosition, new Vector3(0.0f, 0.5f, this.BaseHeight + this.WobbleHeight * Mathf.Cos(this.VerticalWobble)), Mathf.SmoothStep(0.0f, 1f, this.FlyInDuration += Time.deltaTime * this.FlyInSpeed));
        if ((double) this.FlyInDuration >= 1.0)
        {
          this.CurrentState = CritterBee.State.Idle;
          break;
        }
        break;
      case CritterBee.State.Idle:
        this.NewHeightPosition = new Vector3(0.0f, 0.5f, this.BaseHeight + this.WobbleHeight * Mathf.Cos(this.VerticalWobble += Time.deltaTime * this.VerticalWobbleSpeed));
        this.NewHeightPosition.z += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(this.timestamp) * this.VerticalNoiseFrequency, 0.0f) - 0.5f) * this.VerticalNoiseAmplitude;
        break;
      case CritterBee.State.FlyingOut:
        if ((double) this.FlyInDuration < 1.0 && (double) (this.RandomOffset -= Time.deltaTime) < 0.0)
          this.NewHeightPosition = Vector3.Lerp(new Vector3(0.0f, 0.5f, this.BaseHeight + this.WobbleHeight * Mathf.Cos(this.VerticalWobble)), this.FlyOutPosition, Mathf.SmoothStep(0.0f, 1f, this.FlyInDuration += Time.deltaTime * this.FlyOutSpeed));
        if ((double) this.FlyInDuration >= 1.0)
        {
          this.gameObject.Recycle();
          break;
        }
        break;
    }
    if (!((UnityEngine.Object) this.spriteRenderer != (UnityEngine.Object) null))
      return;
    this.spriteRenderer.transform.localPosition = this.NewHeightPosition;
  }

  public void SetSpeedWithAcceleration(float speed)
  {
    this.enabled = true;
    this.CanMove = true;
    DOTween.To((DOGetter<float>) (() => this.Speed), (DOSetter<float>) (x => this.Speed = x), speed, 1.5f);
  }

  public void SetAvoidPlayer(bool avoidPlayer) => this.AvoidPlayer = avoidPlayer;

  private void LateUpdate()
  {
    if ((UnityEngine.Object) this.spriteRenderer == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.spriteRenderer.sprite == (UnityEngine.Object) this.Frame1)
    {
      this.spriteRenderer.sprite = this.OverrideFrame1[this.OverrideIndex];
    }
    else
    {
      if (!((UnityEngine.Object) this.spriteRenderer.sprite == (UnityEngine.Object) this.Frame2))
        return;
      this.spriteRenderer.sprite = this.OverrideFrame2[this.OverrideIndex];
    }
  }

  private void OnDrawGizmos()
  {
    if (!Application.isPlaying)
      Utils.DrawCircleXY(this.transform.position, this.MaximumRange, Color.yellow);
    else
      Utils.DrawCircleXY(this.TargetPosition, this.MaximumRange, Color.yellow);
  }

  private enum State
  {
    FlyingIn,
    Idle,
    FlyingOut,
  }
}
