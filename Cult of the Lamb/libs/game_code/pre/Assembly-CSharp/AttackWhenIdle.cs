// Decompiled with JetBrains decompiler
// Type: AttackWhenIdle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (StateMachine))]
[RequireComponent(typeof (Health))]
public class AttackWhenIdle : BaseMonoBehaviour
{
  public Bullet bullet;
  public float Damage = 1f;
  public float AttackRange = 5f;
  private StateMachine state;
  private Health target;
  private Health health;
  public bool ShootThroughWalls;
  public LayerMask layerToCheck;
  private int currentBurst;
  public int bursts = 1;
  public int burstsInterval = 1;
  public int attackAnticipation = 30;
  public int attackDuration = 60;
  public int attackInterval = 120;
  private int timer;
  private float testDistance;
  private float targetDistance;

  private void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.timer = this.attackInterval;
  }

  private void Update()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if (++this.timer <= this.attackInterval)
          break;
        this.target = (Health) null;
        this.targetDistance = float.MaxValue;
        foreach (Health allUnit in Health.allUnits)
        {
          this.testDistance = Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position);
          if (allUnit.team != this.health.team && (double) this.testDistance < (double) this.AttackRange && (double) this.testDistance < (double) this.targetDistance && (this.ShootThroughWalls || !this.ShootThroughWalls && this.CheckLineOfSight(allUnit.transform.position)))
          {
            this.target = allUnit;
            this.targetDistance = this.testDistance;
          }
        }
        if (!((Object) this.target != (Object) null))
          break;
        this.currentBurst = 0;
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.target.transform.position);
        this.state.CURRENT_STATE = StateMachine.State.Attacking;
        this.timer = 0;
        break;
      case StateMachine.State.Attacking:
        if (this.timer == this.attackAnticipation + (this.bursts == 1 ? 0 : this.currentBurst * this.burstsInterval) && (this.bursts == 1 || this.bursts != 1 && this.currentBurst < this.bursts))
        {
          if ((Object) this.target != (Object) null)
          {
            Bullet bullet = Object.Instantiate<Bullet>(this.bullet, this.transform.position, Quaternion.identity, this.transform.parent);
            bullet.target = this.target;
            bullet.Damage = this.Damage;
            SimpleAnimator componentInChildren = this.GetComponentInChildren<SimpleAnimator>();
            if ((Object) componentInChildren != (Object) null)
              componentInChildren.SetScale(0.5f, 1.2f);
          }
          ++this.currentBurst;
        }
        if (++this.timer <= this.attackDuration)
          break;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.timer = 0;
        break;
    }
  }

  private bool CheckLineOfSight(Vector3 pointToCheck)
  {
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), this.testDistance, (int) this.layerToCheck);
    if ((Object) raycastHit2D.collider != (Object) null)
    {
      Debug.DrawRay(this.transform.position, (raycastHit2D.collider.transform.position - this.transform.position) * Vector2.Distance((Vector2) this.transform.position, (Vector2) raycastHit2D.collider.transform.position), Color.yellow);
      return false;
    }
    Debug.DrawRay(this.transform.position, pointToCheck - this.transform.position, Color.green);
    return true;
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.AttackRange, Color.red);
  }
}
