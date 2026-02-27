// Decompiled with JetBrains decompiler
// Type: Task_Bat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Task_Bat : Task
{
  public Health health;
  public Health EnemyHealth;
  public bool CannotLoseTarget;
  public float DetectEnemyRange;
  public float AttackRange;
  public float LoseEnemyRange;
  public float PreAttackDuration;
  public float PostAttackDuration;
  public float MinimumRange;
  public float DefaultMinimumRange;
  public float AttackDelay;
  public MeshRenderer meshRenderer;
  public MaterialPropertyBlock block;
  public int fillAlpha;
  public int fillColor;
  public float FillAlpha;
  public GameObject TrailRenderer;
  public ColliderEvents damageColliderEvents;
  public List<Collider2D> collider2DList;
  public float AttackAngle;
  public float AttackSpeed = 20f;
  public float AttackVel;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.health = t.gameObject.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.meshRenderer = t.GetComponentInChildren<MeshRenderer>();
    this.block = new MaterialPropertyBlock();
    this.meshRenderer.SetPropertyBlock(this.block);
    this.fillAlpha = Shader.PropertyToID("_FillAlpha");
    this.fillColor = Shader.PropertyToID("_FillColor");
    this.block.SetFloat(this.fillAlpha, this.FillAlpha = 0.0f);
    this.block.SetColor(this.fillColor, Color.white);
    this.meshRenderer.SetPropertyBlock(this.block);
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.FillAlpha = 0.0f;
    this.block.SetFloat(this.fillAlpha, this.FillAlpha);
    this.meshRenderer.SetPropertyBlock(this.block);
  }

  public void Init(
    float DetectEnemyRange,
    float AttackRange,
    float LoseEnemyRange,
    float PreAttackDuration,
    float PostAttackDuration,
    float MinimumRange,
    GameObject TrailRenderer,
    ColliderEvents damageColliderEvents)
  {
    this.DetectEnemyRange = DetectEnemyRange;
    this.AttackRange = AttackRange;
    this.LoseEnemyRange = LoseEnemyRange;
    this.PreAttackDuration = PreAttackDuration;
    this.PostAttackDuration = PostAttackDuration;
    this.DefaultMinimumRange = this.MinimumRange = MinimumRange;
    this.TrailRenderer = TrailRenderer;
    this.damageColliderEvents = damageColliderEvents;
    if (!((Object) damageColliderEvents != (Object) null))
      return;
    damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public Vector3 TargetPosition()
  {
    return (Object) this.TargetObject == (Object) null ? Vector3.zero : this.TargetObject.transform.position;
  }

  public override void TaskUpdate()
  {
    this.AttackDelay -= Time.deltaTime;
    if ((Object) this.TargetObject != (Object) null)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          this.damageColliderEvents.SetActive(false);
          float num1 = Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position);
          if ((double) this.AttackDelay >= 0.0)
            break;
          if ((double) num1 < (double) this.AttackRange)
          {
            this.Timer = 0.0f;
            this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
            this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetPosition());
          this.MinimumRange = this.DefaultMinimumRange + (float) Random.Range(-2, 2);
          break;
        case StateMachine.State.Moving:
          this.damageColliderEvents.SetActive(false);
          float num2 = Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position);
          if ((double) num2 > (double) this.LoseEnemyRange && !this.CannotLoseTarget || (double) num2 < (double) this.MinimumRange)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            if (this.CannotLoseTarget)
              break;
            this.ClearTarget();
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          if (!this.CannotLoseTarget)
          {
            this.ClearTarget();
            this.GetNewTarget();
          }
          this.PathToPosition(this.TargetPosition());
          break;
        case StateMachine.State.SignPostAttack:
          this.damageColliderEvents.SetActive(false);
          this.Timer += Time.deltaTime;
          if ((double) this.Timer > (double) this.PreAttackDuration / 2.0)
          {
            this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
            if ((double) this.t.speed > -2.0)
              this.t.speed -= 0.01f;
          }
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PreAttackDuration)
            break;
          this.Timer = 0.0f;
          if ((Object) this.TargetObject == (Object) null)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          this.DoAttack(this.AttackRange, StateMachine.State.RecoverFromAttack);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) this.AttackVel > 0.0)
            this.AttackVel -= 0.1f * GameManager.DeltaTime;
          this.t.speed = this.AttackVel * Time.deltaTime;
          this.damageColliderEvents.SetActive(true);
          if ((double) this.FillAlpha > 0.0)
          {
            this.FillAlpha -= 3f * Time.deltaTime;
            this.block.SetFloat(this.fillAlpha, this.FillAlpha);
            this.meshRenderer.SetPropertyBlock(this.block);
          }
          if ((double) (this.Timer += Time.deltaTime) <= 0.40000000596046448)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (!((Object) this.TargetObject == (Object) null))
            break;
          this.ClearTarget();
          break;
      }
    }
    else
    {
      this.damageColliderEvents.SetActive(false);
      this.GetNewTarget();
      if (!((Object) this.TargetObject == (Object) null))
        return;
      this.ClearTask();
    }
  }

  public override void DoAttack(float AttackRange, StateMachine.State NextState = StateMachine.State.RecoverFromAttack)
  {
    this.block.SetFloat(this.fillAlpha, this.FillAlpha = 1f);
    if (this.meshRenderer.isVisible)
      CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position));
    this.meshRenderer.SetPropertyBlock(this.block);
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = NextState;
    this.AttackAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
    this.AttackDelay = 2f;
    this.AttackVel = this.AttackSpeed;
  }

  public void GetNewTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.t.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.t.transform.position)))
      {
        float num2 = Vector3.Distance(this.t.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if (!((Object) health != (Object) null))
      return;
    this.TargetObject = health.gameObject;
    this.EnemyHealth = health;
    this.EnemyHealth.attackers.Add(this.t.gameObject);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.ClearTarget();
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    if (!((Object) this.damageColliderEvents != (Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void ClearTask()
  {
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.ClearTarget();
    base.ClearTask();
  }

  public void ClearTarget()
  {
    if ((Object) this.EnemyHealth != (Object) null)
      this.EnemyHealth.attackers.Remove(this.t.gameObject);
    this.TargetObject = (GameObject) null;
    this.EnemyHealth = (Health) null;
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.t.gameObject, Vector3.Lerp(this.t.transform.position, component.transform.position, 0.7f));
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack || collision.gameObject.layer != LayerMask.NameToLayer("Obstacles") && collision.gameObject.layer != LayerMask.NameToLayer("Island"))
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.t.speed = 0.0f;
    this.block.SetFloat(this.fillAlpha, this.FillAlpha = 0.0f);
    this.meshRenderer.SetPropertyBlock(this.block);
  }
}
