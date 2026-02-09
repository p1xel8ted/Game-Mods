// Decompiled with JetBrains decompiler
// Type: PlayerSpirit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PlayerSpirit : BaseMonoBehaviour
{
  public float vx;
  public float vy;
  public float Speed;
  public float FacingAngle;
  public float TargetAngle;
  public float MaxSpeed = 5f;
  public float Acceleration = 0.5f;
  public float TurnSpeed = 7f;
  public GameObject CameraBone;
  public Rigidbody2D rb;
  public float DodgeDelay;
  public StateMachine state;
  public ParticleSystem Particles;
  public SkeletonAnimation Spine;
  public static PlayerSpirit Instance;
  public bool _PlayingParticles;
  public bool Begin;
  public float CollisionDelay;

  public bool PlayingParticles
  {
    get => this._PlayingParticles;
    set
    {
      this._PlayingParticles = value;
      if (this._PlayingParticles)
        this.Particles.Play();
      else
        this.Particles.Stop();
    }
  }

  public void Start()
  {
    this.rb = this.gameObject.GetComponent<Rigidbody2D>();
    this.state = this.GetComponent<StateMachine>();
    this.PlayingParticles = false;
    GameObject.FindWithTag("Player Camera Bone");
    GameManager.GetInstance().CameraSetZoom(2f);
    GameManager.GetInstance().AddToCamera(this.CameraBone);
    GameManager.GetInstance().CameraSetTargetZoom(3f);
    Camera.main.GetComponent<CameraFollowTarget>().DisablePlayerLook = true;
    this.Begin = false;
    this.StartCoroutine((IEnumerator) this.SpawnIn());
  }

  public void OnEnable() => PlayerSpirit.Instance = this;

  public void OnDisable()
  {
    if (!((UnityEngine.Object) PlayerSpirit.Instance == (UnityEngine.Object) this))
      return;
    PlayerSpirit.Instance = (PlayerSpirit) null;
  }

  public IEnumerator SpawnIn()
  {
    this.Spine.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    this.Spine.gameObject.SetActive(true);
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.Spine.AnimationState.SetAnimation(0, "spawn-in", false);
    this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(2.3f);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CameraSetTargetZoom(8f);
  }

  public void Update()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.InActive)
      return;
    this.CollisionDelay -= Time.deltaTime;
    if ((double) this.CollisionDelay < 0.0)
      this.DodgeDelay -= Time.deltaTime;
    if (this.state.CURRENT_STATE == StateMachine.State.InActive)
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) <= 0.30000001192092896 && (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) <= 0.30000001192092896)
    {
      if ((double) this.CollisionDelay < 0.0)
        this.Speed += (float) ((0.0 - (double) this.Speed) / 7.0) * GameManager.DeltaTime;
    }
    else
    {
      if ((double) this.Speed < (double) this.MaxSpeed)
        this.Speed += this.Acceleration * GameManager.DeltaTime;
      if ((double) this.CollisionDelay < 0.0)
      {
        this.TargetAngle = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.Gameplay.GetHorizontalAxis(), InputManager.Gameplay.GetVerticalAxis()));
        this.FacingAngle = Utils.SmoothAngle(this.FacingAngle, this.TargetAngle, this.TurnSpeed);
      }
      if (!this.Particles.isPlaying)
        this.Particles.Play();
    }
    if ((double) this.Speed > 2.0)
    {
      if (!this.PlayingParticles)
        this.PlayingParticles = true;
    }
    else if (this.PlayingParticles)
      this.PlayingParticles = false;
    this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
    this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
  }

  public void FixedUpdate()
  {
    this.rb.MovePosition(this.rb.position + new Vector2(this.vx, this.vy) * Time.fixedDeltaTime);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    this.TargetAngle = this.FacingAngle = Utils.GetAngle((Vector3) collision.contacts[0].point, this.transform.position);
    this.Speed = this.MaxSpeed * 1f;
    this.CollisionDelay = 0.1f;
  }
}
