// Decompiled with JetBrains decompiler
// Type: CritterSquirrel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class CritterSquirrel : UnitObject
{
  public float DangerDistance = 3.5f;
  private float Timer;
  private float TargetAngle;
  public float WalkSpeed = 0.02f;
  public float RunSpeed = 0.07f;
  private float IgnorePlayer;
  public bool FleeNearEnemies = true;
  public bool EatGrass;
  public bool WonderAround = true;
  public bool FleeIntoGround = true;
  public SkeletonAnimation spine;
  private StateMachine.State _prevState;
  public float EscapeTimer;
  public float FleeTimer = 5f;

  private void Start()
  {
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Timer = (float) Random.Range(1, 5);
    if (!((Object) this.spine != (Object) null) || this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((Object) this.spine != (Object) null) || this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((Object) this.spine != (Object) null) || this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "dig")
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_disappear_underground", this.gameObject);
    }
    else
    {
      if (!(e.Data.Name == "step"))
        return;
      AudioManager.Instance.PlayOneShot("event:/material/footstep_woodland", this.gameObject);
    }
  }

  public override void Update()
  {
    base.Update();
    this.WonderFreely();
  }

  private void WonderFreely()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if (this._prevState != this.state.CURRENT_STATE)
        {
          this.spine.AnimationState.SetAnimation(0, "animation", true);
          this._prevState = this.state.CURRENT_STATE;
        }
        this.UsePathing = false;
        if (!this.WonderAround)
          break;
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          this.Timer = (float) Random.Range(1, 5);
          this.TargetAngle = (float) Random.Range(0, 360);
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Moving:
        if (this._prevState != this.state.CURRENT_STATE)
        {
          if ((Object) this.spine != (Object) null)
            this.spine.AnimationState.SetAnimation(0, "walk", true);
          this._prevState = this.state.CURRENT_STATE;
        }
        this.state.SmoothFacingAngle(this.TargetAngle, 10f);
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          if (this.EatGrass && (double) Random.value < 0.800000011920929)
          {
            this.Timer = (float) Random.Range(2, 4);
            this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
            break;
          }
          this.Timer = (float) Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.Fleeing:
        if (this._prevState != this.state.CURRENT_STATE)
        {
          AudioManager.Instance.PlayOneShot("event:/squirrel/squirrel_vocal", this.gameObject);
          this.spine.AnimationState.SetAnimation(0, "run", true);
          this._prevState = this.state.CURRENT_STATE;
        }
        this.IgnorePlayer -= Time.deltaTime;
        this.state.SmoothFacingAngle(this.TargetAngle, 12f);
        if ((double) (this.FleeTimer -= Time.deltaTime) < 0.0)
        {
          if (!this.FleeIntoGround)
            break;
          this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
          break;
        }
        if ((Object) this.TargetEnemy == (Object) null)
        {
          this.maxSpeed = this.WalkSpeed;
          this.Timer = (float) Random.Range(1, 5);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        if ((double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > 3.0 || (double) this.IgnorePlayer >= 0.0)
          break;
        this.state.facingAngle = this.TargetAngle = Utils.GetAngle(this.TargetEnemy.transform.position, this.transform.position);
        break;
      case StateMachine.State.CustomAction0:
        if (this._prevState != this.state.CURRENT_STATE)
        {
          AudioManager.Instance.PlayOneShot("event:/squirrel/squirell_tssk", this.gameObject);
          this.spine.AnimationState.SetAnimation(0, "eat", true);
          this._prevState = this.state.CURRENT_STATE;
        }
        if ((double) (this.Timer -= Time.deltaTime) < 0.0)
        {
          this.Timer = (float) Random.Range(1, 3);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.LookForDanger();
        break;
      case StateMachine.State.CustomAnimation:
        if (this._prevState != this.state.CURRENT_STATE)
        {
          AudioManager.Instance.PlayOneShot("event:/squirrel/squirrel_vocal", this.gameObject);
          this.spine.AnimationState.SetAnimation(0, "dig", false);
          this._prevState = this.state.CURRENT_STATE;
          this.GetComponent<ShowHPBar>().DestroyHPBar();
        }
        if ((double) (this.EscapeTimer += Time.deltaTime) <= 1.1000000238418579)
          break;
        this.gameObject.Recycle();
        break;
    }
  }

  private void LookForDanger()
  {
    if (!this.FleeNearEnemies)
      return;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DangerDistance && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetEnemy = allUnit;
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        this.maxSpeed = this.RunSpeed;
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
        this.FleeTimer = (float) Random.Range(5, 10);
      }
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.3f, false);
    AudioManager.Instance.PlayOneShot("event:/squirrel/squirrel_vocal", this.gameObject);
    this.spine.GetComponent<SimpleSpineFlash>().FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AudioManager.Instance.PlayOneShot("event:/squirrel/squirrel_vocal", this.gameObject);
    this.gameObject.Recycle();
    ++DataManager.Instance.TotalSquirrelsCaught;
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    this.state.facingAngle = this.TargetAngle = Utils.GetAngle((Vector3) collision.contacts[0].point, this.transform.position);
    this.IgnorePlayer = 0.5f;
  }
}
