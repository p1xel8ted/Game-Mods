// Decompiled with JetBrains decompiler
// Type: FormationFighterShield
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FormationFighterShield : FormationFighter
{
  private float VulnerableTimer;
  private Task_Combat_Shield ShieldTask;
  private UnitObject AttackerUO;

  private void Start() => this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);

  public override void SetTask()
  {
    this.AddNewTask(Task_Type.SHIELD, false);
    this.ShieldTask = this.CurrentTask as Task_Combat_Shield;
    this.ShieldTask.Init(this.DetectEnemyRange, this.AttackRange, this.LoseEnemyRange, this.PreAttackDuration, this.PostAttackDuration, this.DefendingDuration, (TaskDoer) this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    if (this.ShieldTask == null)
      return;
    this.ShieldTask.ClearTarget();
  }

  private void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.health.DamageModifier = 1f;
    if (this.state.CURRENT_STATE != StateMachine.State.Defending && (this.state.CURRENT_STATE == StateMachine.State.Defending || this.state.CURRENT_STATE == StateMachine.State.Vulnerable || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack || this.state.CURRENT_STATE == StateMachine.State.RecoverFromCounterAttack))
      return;
    this.health.DamageModifier = 0.0f;
    GameManager.GetInstance().HitStop();
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(AttackLocation, this.transform.position));
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
    {
      this.AttackerUO = Attacker.GetComponent<UnitObject>();
      if ((UnityEngine.Object) this.AttackerUO != (UnityEngine.Object) null)
      {
        this.AttackerUO.knockBackVX = 0.7f * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
        this.AttackerUO.knockBackVY = 0.7f * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Defending)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Defending;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Defending)
      return;
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override void SpecialStates()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Defending && (double) (this.DefendTimer += Time.deltaTime) >= (double) this.DefendingDuration)
    {
      this.ShieldTask.DoAttack(this.AttackRange, StateMachine.State.RecoverFromCounterAttack);
      this.DefendTimer = 0.0f;
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Vulnerable || (double) (this.VulnerableTimer += Time.deltaTime) < 1.0)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.VulnerableTimer = 0.0f;
  }
}
