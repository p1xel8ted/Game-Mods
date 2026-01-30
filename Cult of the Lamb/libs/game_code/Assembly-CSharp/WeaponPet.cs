// Decompiled with JetBrains decompiler
// Type: WeaponPet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using UnityEngine;

#nullable disable
public class WeaponPet : BaseMonoBehaviour
{
  public GameObject Master;
  public StateMachine MasterState;
  public StateMachine state;
  public float TargetAngle;
  public Vector3 MoveVector;
  public float Speed;
  public float vx;
  public float vy;
  public float Bobbing;
  public float SpineVZ;
  public float SpineVY;
  public SpriteRenderer spriteRenderer;
  public SkeletonAnimation spine;
  public Transform ChainPoint;
  public float AttackProgress;
  public Vector3 AttackPosition;
  public float AttackAngle;
  public float AttackWait;
  public float Timer;

  public void Start() => this.state = this.GetComponent<StateMachine>();

  public void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null)
    {
      this.Master = GameObject.FindGameObjectWithTag("Player");
      if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null)
        return;
      this.transform.position = this.Master.transform.position;
      this.MasterState = this.Master.GetComponent<StateMachine>();
    }
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Speed += (float) ((0.0 - (double) this.Speed) / (10.0 * (double) GameManager.DeltaTime));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) > 1.5)
        {
          this.TargetAngle = Utils.GetAngle(this.transform.position, this.Master.transform.position);
          this.state.facingAngle = this.TargetAngle;
          this.state.CURRENT_STATE = StateMachine.State.Moving;
        }
        this.vx = this.Speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vy = this.Speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.transform.position = this.transform.position + new Vector3(this.vx, this.vy);
        break;
      case StateMachine.State.Moving:
        this.TargetAngle = Utils.GetAngle(this.transform.position, this.Master.transform.position);
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        this.Speed += (float) ((4.5 - (double) this.Speed) / (15.0 * (double) GameManager.DeltaTime));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) < 1.5)
          this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.vx = this.Speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vy = this.Speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.transform.position = this.transform.position + new Vector3(this.vx, this.vy);
        break;
      case StateMachine.State.Attacking:
        this.AttackProgress += 0.05f;
        float num = (float) (360.0 * ((double) this.AttackProgress / 1.0)) + this.AttackAngle;
        this.transform.position = this.AttackPosition + new Vector3(2f * Mathf.Cos(num * ((float) Math.PI / 180f)), 2f * Mathf.Sin(num * ((float) Math.PI / 180f)));
        if ((double) this.AttackProgress >= 1.0)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        break;
      case StateMachine.State.SignPostAttack:
        this.AttackProgress += 0.1f;
        this.AttackProgress = 0.0f;
        float f = 180f * this.AttackProgress + this.AttackAngle;
        this.transform.position = this.AttackPosition + new Vector3(1f * Mathf.Cos(f), 1f * Mathf.Sin(f));
        break;
      case StateMachine.State.AimDodge:
        this.vx = 20f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vy = 20f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.transform.position = this.transform.position + new Vector3(this.vx, this.vy);
        if ((double) (this.Timer += Time.deltaTime) > 0.10000000149011612)
        {
          this.state.CURRENT_STATE = StateMachine.State.Dodging;
          break;
        }
        break;
      case StateMachine.State.Dodging:
        this.spine.transform.localPosition = Vector3.Lerp(this.spine.transform.localPosition, new Vector3(0.0f, 0.0f, 0.0f), 25f * Time.deltaTime);
        this.SpineVZ = this.spine.transform.localPosition.z;
        this.SpineVY = this.spine.transform.localPosition.y;
        break;
    }
    this.spine.skeleton.ScaleX = (double) this.Master.transform.position.x > (double) this.transform.position.x ? -1f : 1f;
    this.spine.transform.eulerAngles = new Vector3(-60f, 0.0f, this.vx * -100f);
    if (this.state.CURRENT_STATE == StateMachine.State.AimDodge || this.state.CURRENT_STATE == StateMachine.State.Dodging)
      return;
    this.SpineVZ = Mathf.Lerp(this.SpineVZ, -1.5f, 5f * Time.deltaTime);
    this.SpineVY = Mathf.Lerp(this.SpineVY, 0.5f, 5f * Time.deltaTime);
    this.spine.transform.localPosition = new Vector3(0.0f, this.SpineVY, this.SpineVZ + 0.1f * Mathf.Cos(this.Bobbing += 5f * Time.deltaTime));
  }

  public void DoAttack(Vector3 AttackPosition, float AttackAngle)
  {
    this.AttackProgress = 0.0f;
    this.AttackPosition = AttackPosition;
    this.AttackAngle = AttackAngle;
    this.AttackWait = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Attacking;
  }

  public void PrepareAttack(Vector3 AttackPosition, float AttackAngle)
  {
    this.AttackProgress = 0.0f;
    this.AttackPosition = AttackPosition;
    this.AttackAngle = AttackAngle;
    this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
  }

  public void DoChainDodge(Vector3 StartPosition, float DodgeAngle)
  {
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.AimDodge;
    this.transform.position = StartPosition;
    this.state.facingAngle = DodgeAngle;
  }

  public void EndDodge() => this.state.CURRENT_STATE = StateMachine.State.Idle;
}
