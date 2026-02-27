// Decompiled with JetBrains decompiler
// Type: CritterJellyfish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CritterJellyfish : UnitObject
{
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string swimAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string scareAnimation;
  public float DangerDistance = 2f;
  public CircleCollider2D CircleCollider;
  public Animator animator;
  public float Timer;
  public float TargetAngle;
  public float vz;
  public GameObject Shadow;

  public void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    if ((UnityEngine.Object) this.CircleCollider == (UnityEngine.Object) null)
      this.CircleCollider = this.GetComponent<CircleCollider2D>();
    this.animator = this.GetComponentInChildren<Animator>();
    this.Timer = (float) UnityEngine.Random.Range(2, 4);
    if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, 1f, 1f);
    this.DangerDistance = 2.5f;
  }

  public void SetFleeing()
  {
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
      this.state = this.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.Fleeing;
  }

  public override void Update()
  {
    this.health.HandleFrozenTime();
    if (PlayerRelic.TimeFrozen)
      return;
    base.Update();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.LookForDanger();
        break;
      case StateMachine.State.Fleeing:
        this.vx = 0.3f * Mathf.Cos(this.TargetAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vy = 0.3f * Mathf.Sin(this.TargetAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vz -= 0.015f * Mathf.Sin(Time.deltaTime);
        this.transform.position = this.transform.position + new Vector3(this.vx, this.vy, (double) Time.deltaTime == 0.0 ? 0.0f : this.vz);
        break;
      case StateMachine.State.CustomAction0:
        this.LookForDanger();
        if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.Timer = (float) UnityEngine.Random.Range(2, 4);
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        if ((UnityEngine.Object) this.animator != (UnityEngine.Object) null)
          this.animator.SetTrigger("IDLE");
        if ((double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5)
          break;
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, 1f, 1f);
        break;
    }
  }

  public void LookForDanger()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DangerDistance && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        this.Spine.AnimationState.SetAnimation(0, this.scareAnimation, false);
        this.Spine.AnimationState.AddAnimation(0, this.swimAnimation, true, 0.0f);
        if ((UnityEngine.Object) this.animator != (UnityEngine.Object) null)
          this.animator.SetTrigger("FLY");
        AudioManager.Instance.PlayOneShot("event:/enemy/fly_spawn", this.gameObject);
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
        this.transform.localScale = new Vector3((double) this.TargetAngle >= 90.0 || (double) this.TargetAngle <= -90.0 ? 1f : -1f, 1f, 1f);
        this.StartCoroutine(this.DisableCollider());
      }
    }
  }

  public IEnumerator DisableCollider()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CircleCollider.enabled = false;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.DangerDistance, Color.white);
  }
}
