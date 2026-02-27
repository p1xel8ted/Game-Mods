// Decompiled with JetBrains decompiler
// Type: FormationFighter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FormationFighter : TaskDoer
{
  public float KnockBackAmount = 0.25f;
  public float PreAttackDuration = 1f;
  public float PostAttackDuration = 1f;
  public float DefendingDuration = 2f;
  [HideInInspector]
  public float Timer;
  public int AttackPosition;
  public float DetectEnemyRange = 5f;
  public float LoseEnemyRange = 8f;
  public float AttackRange = 0.5f;
  public float SeperationRadius = 1f;
  public List<Sprite> ChunksToSpawn;
  public int thisChecked;
  public bool BreakAttacksOnHit;
  public SimpleSpineAnimator _simpleSpineAnimator;
  public SimpleSpineFlash _simpleSpineFlash;
  public static List<FormationFighter> fighters = new List<FormationFighter>();
  public Task_Combat CombatTask;
  [HideInInspector]
  public float DefendTimer;
  [HideInInspector]
  public float HitTimer;

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid/death", this.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    int num = -1;
    if (this.ChunksToSpawn.Count <= 0)
      return;
    while (++num < 10)
      Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(Attacker.transform.position, this.transform.position) + (float) UnityEngine.Random.Range(-20, 20), this.ChunksToSpawn);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.SetTask();
    FormationFighter.fighters.Add(this);
    if (!(bool) (UnityEngine.Object) this.health)
      return;
    this.health.invincible = false;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    FormationFighter.fighters.Remove(this);
    if (this.CombatTask != null)
      this.CombatTask.ClearTarget();
    this.CombatTask = (Task_Combat) null;
  }

  public void GraveSpawn()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.GraveSpawnRoutine());
  }

  public IEnumerator GraveSpawnRoutine()
  {
    FormationFighter formationFighter = this;
    formationFighter.CurrentTask.ClearTask();
    formationFighter.CurrentTask = (Task) null;
    formationFighter.health.invincible = true;
    formationFighter.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    formationFighter.simpleSpineAnimator.Animate("grave-spawn", 0, false, 0.0f);
    formationFighter.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    formationFighter.health.invincible = false;
    formationFighter.SetTask();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid/gethit", this.transform.position);
    this.simpleSpineAnimator.FlashRedTint();
    if (AttackType != Health.AttackTypes.Heavy && !FromBehind)
    {
      this.knockBackVX = -this.KnockBackAmount * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = -this.KnockBackAmount * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
    }
    this.simpleSpineAnimator.Animate("hurt-eyes", 1, false);
    base.OnHit(Attacker, AttackLocation, AttackType);
  }

  public SimpleSpineAnimator simpleSpineAnimator
  {
    set => this._simpleSpineAnimator = value;
    get
    {
      if ((UnityEngine.Object) this._simpleSpineAnimator == (UnityEngine.Object) null)
        this._simpleSpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
      return this._simpleSpineAnimator;
    }
  }

  public SimpleSpineFlash simpleSpineFlash
  {
    set => this._simpleSpineFlash = value;
    get
    {
      if ((UnityEngine.Object) this._simpleSpineFlash == (UnityEngine.Object) null)
        this._simpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
      return this._simpleSpineFlash;
    }
  }

  public virtual void SetTask()
  {
    this.AddNewTask(Task_Type.COMBAT, false);
    this.CombatTask = this.CurrentTask as Task_Combat;
    this.CombatTask.Init(this.DetectEnemyRange, this.AttackRange, this.LoseEnemyRange, this.PreAttackDuration, this.PostAttackDuration, this.DefendingDuration, (TaskDoer) this);
  }

  public override void OnDestroy() => base.OnDestroy();

  public virtual void SpecialStates()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.SignPostAttack && this.CurrentTask != null)
      this.simpleSpineFlash.FlashWhite(this.CurrentTask.Timer / 0.5f);
    else
      this.simpleSpineFlash.FlashWhite(false);
    if (this.state.CURRENT_STATE == StateMachine.State.HitLeft && (double) (this.HitTimer += Time.deltaTime) >= 0.40000000596046448)
    {
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.HitTimer = 0.0f;
    }
    if (this.state.CURRENT_STATE != StateMachine.State.HitRight || (double) (this.HitTimer += Time.deltaTime) < 0.40000000596046448)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.HitTimer = 0.0f;
  }

  public override void Update()
  {
    this.SpecialStates();
    base.Update();
    if (this.CurrentTask != null)
      this.CurrentTask.TaskUpdate();
    if (!this.SeperateObject)
      return;
    this.Seperate(this.SeperationRadius);
  }
}
