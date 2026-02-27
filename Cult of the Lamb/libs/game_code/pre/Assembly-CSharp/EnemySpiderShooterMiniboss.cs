// Decompiled with JetBrains decompiler
// Type: EnemySpiderShooterMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderShooterMiniboss : EnemySpider
{
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnimation;
  [Space]
  [SerializeField]
  private EnemySpiderShooterMiniboss.Shot[] shots;
  [SerializeField]
  private float timeBetweenShots;
  [SerializeField]
  private float minShootDistance;
  private float shootTimestamp;
  private int shootIndex;

  public override void Update()
  {
    base.Update();
    if (this.Attacking)
      return;
    float? currentTime = GameManager.GetInstance()?.CurrentTime;
    float shootTimestamp = this.shootTimestamp;
    if (!((double) currentTime.GetValueOrDefault() > (double) shootTimestamp & currentTime.HasValue) || !(bool) (UnityEngine.Object) PlayerFarming.Instance || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= (double) this.minShootDistance)
      return;
    this.StartCoroutine((IEnumerator) this.ShootIE(this.shots[UnityEngine.Random.Range(0, this.shots.Length)]));
  }

  private IEnumerator ShootIE(EnemySpiderShooterMiniboss.Shot shot)
  {
    EnemySpiderShooterMiniboss spiderShooterMiniboss = this;
    ++spiderShooterMiniboss.shootIndex;
    spiderShooterMiniboss.updateDirection = false;
    spiderShooterMiniboss.Attacking = true;
    spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    spiderShooterMiniboss.ClearPaths();
    AudioManager.Instance.PlayOneShot(spiderShooterMiniboss.warningSfx, spiderShooterMiniboss.transform.position);
    for (int i = 0; i < shot.Amount; ++i)
    {
      spiderShooterMiniboss.SetAnimation(spiderShooterMiniboss.shootAnticipationAnimation);
      spiderShooterMiniboss.TargetEnemy = spiderShooterMiniboss.GetClosestTarget();
      spiderShooterMiniboss.LookAtTarget();
      yield return (object) new WaitForEndOfFrame();
      float t = 0.0f;
      while ((double) t < (double) shot.ShootAnticipation / (double) spiderShooterMiniboss.Spine.timeScale)
      {
        float amt = t / shot.ShootAnticipation;
        spiderShooterMiniboss.SimpleSpineFlash.FlashWhite(amt);
        t += Time.deltaTime;
        yield return (object) null;
      }
      spiderShooterMiniboss.SimpleSpineFlash.FlashWhite(false);
      spiderShooterMiniboss.SetAnimation(spiderShooterMiniboss.shootAnimation);
      spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
      spiderShooterMiniboss.StartCoroutine((IEnumerator) shot.ProjectilePattern.ShootIE());
      AudioManager.Instance.PlayOneShot(spiderShooterMiniboss.attackSfx, spiderShooterMiniboss.transform.position);
      spiderShooterMiniboss.TargetEnemy = spiderShooterMiniboss.GetClosestTarget();
      spiderShooterMiniboss.LookAtTarget();
      yield return (object) new WaitForSeconds(shot.TimeBetweenShooting / spiderShooterMiniboss.Spine.timeScale);
    }
    spiderShooterMiniboss.AddAnimation(spiderShooterMiniboss.IdleAnimation, true);
    yield return (object) new WaitForSeconds(shot.ShootCooldown / spiderShooterMiniboss.Spine.timeScale);
    spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    spiderShooterMiniboss.shootTimestamp = GameManager.GetInstance().CurrentTime + spiderShooterMiniboss.timeBetweenShots;
    spiderShooterMiniboss.updateDirection = true;
    spiderShooterMiniboss.Attacking = false;
  }

  protected override bool ShouldAttack()
  {
    return (double) (this.AttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && (bool) (UnityEngine.Object) this.TargetEnemy && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) < (double) this.VisionRange && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer && (double) UnityEngine.Random.Range(0.0f, 1f) > 0.6600000262260437;
  }

  [Serializable]
  private struct Shot
  {
    public int Amount;
    public float ShootAnticipation;
    public float ShootDuration;
    public float ShootCooldown;
    public float TimeBetweenShooting;
    public ProjectilePatternBase ProjectilePattern;
  }
}
