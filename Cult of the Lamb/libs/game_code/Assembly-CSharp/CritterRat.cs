// Decompiled with JetBrains decompiler
// Type: CritterRat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterRat : UnitObject
{
  [SerializeField]
  public Interaction_CatchRat Interaction_CatchRat;
  [SerializeField]
  public float DangerDistance = 3.5f;
  [SerializeField]
  public float WalkSpeed = 0.02f;
  [SerializeField]
  public float RunSpeed = 0.07f;
  [SerializeField]
  public bool FleeNearEnemies = true;
  [SerializeField]
  public bool EatGrass;
  [SerializeField]
  public bool WonderAround = true;
  [SerializeField]
  public bool FleeIntoGround = true;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public float EscapeTimer;
  [SerializeField]
  public float FleeTimer = 5f;
  public bool IsCurrent;
  [SerializeField]
  [SpineSkin("", "", true, false, true)]
  public List<string> initialSkins;
  public StateMachine.State _prevState;
  public float Timer;
  public float TargetAngle;
  public float IgnorePlayer;

  public void Start()
  {
    this.spine.Skeleton.SetSkin(this.initialSkins[Random.Range(0, this.initialSkins.Count)]);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Timer = 0.0f;
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

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "dig")
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/rat_burrow_in", this.gameObject);
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
    if (PlayerRelic.TimeFrozen)
      return;
    base.Update();
    this.WonderFreely();
  }

  public void WonderFreely()
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
          AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/rat_vocal", this.gameObject);
          this.spine.AnimationState.SetAnimation(0, "run", true);
          this._prevState = this.state.CURRENT_STATE;
        }
        this.IgnorePlayer -= Time.deltaTime;
        this.state.SmoothFacingAngle(this.TargetAngle, 12f);
        if ((double) (this.FleeTimer -= Time.deltaTime) < 0.0)
        {
          if (!this.FleeIntoGround || this.IsCurrent)
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
          AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/rat_tssk", this.gameObject);
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
          this.health.invincible = true;
        }
        if (this.Interaction_CatchRat.enabled)
        {
          this.Interaction_CatchRat.enabled = false;
          this.Interaction_CatchRat.StopAllCoroutines();
        }
        if ((double) (this.EscapeTimer += Time.deltaTime) <= 1.1000000238418579)
          break;
        this.gameObject.Recycle();
        break;
    }
  }

  public void LookForDanger()
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

  public void OnCollisionStay2D(Collision2D collision)
  {
    this.state.facingAngle = this.TargetAngle = Utils.GetAngle((Vector3) collision.contacts[0].point, this.transform.position);
    this.IgnorePlayer = 0.5f;
  }
}
