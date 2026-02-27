// Decompiled with JetBrains decompiler
// Type: PlayerPrisonerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System;
using UnityEngine;

#nullable disable
public class PlayerPrisonerController : BaseMonoBehaviour
{
  public float Speed;
  public float MaxSpeed = 2f;
  public float Acceleration = 0.5f;
  public float TurnSpeed = 7f;
  public GameObject CameraBone;
  private StateMachine state;
  private UnitObject unitObject;
  private float forceDir;
  private float xDir;
  private float yDir;
  public SkeletonAnimation Spine;
  public static float MinInputForMovement = 0.3f;
  public static PlayerPrisonerController Instance;
  public bool GoToAndStopping;
  private StateMachine.State GoToAndStopCompleteState;

  private void Awake() => PlayerPrisonerController.Instance = this;

  private void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.unitObject = this.GetComponent<UnitObject>();
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "footsteps"))
      return;
    AudioManager.Instance.PlayFootstepPlayer(this.transform.position);
  }

  private void OnDisable()
  {
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void OnEnable()
  {
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
      this.state = this.GetComponent<StateMachine>();
    if (!((UnityEngine.Object) this.unitObject == (UnityEngine.Object) null))
      return;
    this.unitObject = this.GetComponent<UnitObject>();
  }

  public UnitObject GoToAndStop(Vector3 Position, StateMachine.State state = StateMachine.State.Idle, System.Action callback = null)
  {
    this.xDir = this.yDir = this.unitObject.vx = this.unitObject.vy = 0.0f;
    this.GoToAndStopping = true;
    this.unitObject.givePath(Position);
    this.unitObject.EndOfPath += new System.Action(this.EndOfPath);
    this.unitObject.EndOfPath += callback;
    this.unitObject.speed = this.unitObject.maxSpeed;
    this.GoToAndStopCompleteState = state;
    return this.unitObject;
  }

  public void EndOfPath()
  {
    Debug.Log((object) "End go to and stopping");
    this.GoToAndStopping = false;
    this.unitObject.EndOfPath -= new System.Action(this.EndOfPath);
    this.state.CURRENT_STATE = this.GoToAndStopCompleteState;
    this.Speed = 0.0f;
    this.unitObject.vx = this.unitObject.vy = 0.0f;
  }

  private void Update()
  {
    Shader.SetGlobalVector("_PlayerPosition", (Vector4) this.gameObject.transform.position);
    if (!this.GoToAndStopping)
    {
      this.xDir = InputManager.Gameplay.GetHorizontalAxis();
      this.yDir = InputManager.Gameplay.GetVerticalAxis();
      if (this.state.CURRENT_STATE == StateMachine.State.Moving)
        this.Speed *= Mathf.Clamp01(new Vector2(this.xDir, this.yDir).magnitude);
      this.Speed = Mathf.Max(this.Speed, 0.0f);
      this.unitObject.vx = this.Speed * Mathf.Cos(this.forceDir * ((float) Math.PI / 180f));
      this.unitObject.vy = this.Speed * Mathf.Sin(this.forceDir * ((float) Math.PI / 180f));
    }
    else
      this.xDir = this.yDir = 0.0f;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Speed += (float) ((0.0 - (double) this.Speed) / 3.0) * GameManager.DeltaTime;
        if ((double) Mathf.Abs(this.xDir) <= (double) PlayerPrisonerController.MinInputForMovement && (double) Mathf.Abs(this.yDir) <= (double) PlayerPrisonerController.MinInputForMovement)
          break;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        break;
      case StateMachine.State.Moving:
        if (this.GoToAndStopping || (double) Time.timeScale == 0.0)
          break;
        if ((double) Mathf.Abs(this.xDir) <= (double) PlayerPrisonerController.MinInputForMovement && (double) Mathf.Abs(this.yDir) <= (double) PlayerPrisonerController.MinInputForMovement)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
        if ((double) this.unitObject.vx != 0.0 || (double) this.unitObject.vy != 0.0)
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
        this.state.LookAngle = this.state.facingAngle;
        this.Speed += (float) (((double) this.MaxSpeed - (double) this.Speed) / 3.0) * GameManager.DeltaTime;
        break;
      case StateMachine.State.CustomAnimation:
        this.Speed = 0.0f;
        this.xDir = this.yDir = 0.0f;
        break;
    }
  }
}
