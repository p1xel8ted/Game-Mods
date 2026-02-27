// Decompiled with JetBrains decompiler
// Type: CritterJellyfish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CritterJellyfish : UnitObject
{
  private float DangerDistance = 2f;
  public CircleCollider2D CircleCollider;
  private Animator animator;
  private float Timer;
  private float TargetAngle;
  private float vz;
  public GameObject Shadow;

  private void Start()
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
        this.transform.position = this.transform.position + new Vector3(this.vx, this.vy, this.vz);
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

  private void LookForDanger()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DangerDistance && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        if ((UnityEngine.Object) this.animator != (UnityEngine.Object) null)
          this.animator.SetTrigger("FLY");
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
        this.transform.localScale = new Vector3((double) this.TargetAngle >= 90.0 || (double) this.TargetAngle <= -90.0 ? 1f : -1f, 1f, 1f);
        this.StartCoroutine((IEnumerator) this.DisableCollider());
      }
    }
  }

  private IEnumerator DisableCollider()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CircleCollider.enabled = false;
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.DangerDistance, Color.white);
  }
}
