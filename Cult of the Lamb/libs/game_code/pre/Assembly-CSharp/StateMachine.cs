// Decompiled with JetBrains decompiler
// Type: StateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class StateMachine : BaseMonoBehaviour
{
  public bool IsPlayer;
  public StateMachine.StateChange OnStateChange;
  [SerializeField]
  private StateMachine.State currentstate;
  public float facingAngle;
  public float LookAngle;
  [HideInInspector]
  public bool isDefending;
  [HideInInspector]
  public float Timer;

  public StateMachine.State CURRENT_STATE
  {
    get => this.currentstate;
    set
    {
      if (this.LockStateChanges)
        return;
      this.Timer = 0.0f;
      if (this.OnStateChange != null)
        this.OnStateChange(value, this.currentstate);
      this.currentstate = value;
      if (!this.IsPlayer)
        return;
      Debug.Log((object) value);
    }
  }

  public bool LockStateChanges { get; set; }

  public void SmoothFacingAngle(float Angle, float Easing)
  {
    this.facingAngle += Mathf.Atan2(Mathf.Sin((float) (((double) Angle - (double) this.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) Angle - (double) this.facingAngle) * (Math.PI / 180.0)))) * 57.29578f / Easing * GameManager.DeltaTime;
  }

  private void LateUpdate()
  {
    this.facingAngle = Mathf.Repeat(this.facingAngle, 360f);
    this.LookAngle = Mathf.Repeat(this.LookAngle, 360f);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos(this.facingAngle * ((float) Math.PI / 180f)), 2f * Mathf.Sin(this.facingAngle * ((float) Math.PI / 180f))), Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos(this.LookAngle * ((float) Math.PI / 180f)), 2f * Mathf.Sin(this.LookAngle * ((float) Math.PI / 180f))), Color.green);
  }

  private void FacingToLook() => this.facingAngle = this.LookAngle;

  private void LookToFacing() => this.LookAngle = this.facingAngle;

  public enum State
  {
    Idle,
    Moving,
    Attacking,
    Defending,
    SignPostAttack,
    RecoverFromAttack,
    AimDodge,
    Dodging,
    Fleeing,
    Inventory,
    Map,
    WeaponSelect,
    CustomAction0,
    InActive,
    RaiseAlarm,
    Casting,
    TimedAction,
    Worshipping,
    Sleeping,
    BeingCarried,
    HitThrown,
    HitLeft,
    HitRight,
    HitRecover,
    Teleporting,
    SignPostCounterAttack,
    RecoverFromCounterAttack,
    Charging,
    Vulnerable,
    Converting,
    Unconverted,
    FoundItem,
    Dieing,
    Dead,
    Building,
    Respawning,
    AwaitRecruit,
    PickedUp,
    SacrificeRecruit,
    Recruited,
    Dancing,
    SpawnIn,
    SpawnOut,
    CrowdWorship,
    Grapple,
    DashAcrossIsland,
    ChargingHeavyAttack,
    Elevator,
    Grabbed,
    CustomAnimation,
    Preach,
    Stealth,
    GameOver,
    KnockBack,
    Aiming,
    Meditate,
    Resurrecting,
    Idle_CarryingBody,
    Moving_CarryingBody,
    Heal,
    Reeling,
    TiedToAltar,
    FinalGameOver,
  }

  public delegate void StateChange(StateMachine.State NewState, StateMachine.State PrevState);
}
