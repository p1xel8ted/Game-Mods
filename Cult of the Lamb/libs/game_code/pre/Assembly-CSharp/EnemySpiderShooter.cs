// Decompiled with JetBrains decompiler
// Type: EnemySpiderShooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderShooter : EnemySpider
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnimation;
  [SerializeField]
  private float shootAnticipation;
  [SerializeField]
  private float shootDuration;
  [SerializeField]
  private float shootCooldown;
  [SerializeField]
  private float minDistanceToShoot;
  [SerializeField]
  private Vector2 timeBetweenShooting;
  private float shootTimestamp;
  private ProjectilePattern projectilePattern;

  private void Start() => this.projectilePattern = this.GetComponentInChildren<ProjectilePattern>();

  public override void Update()
  {
    base.Update();
    if (!this.ShouldShoot())
      return;
    this.Shoot();
  }

  private bool ShouldShoot()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Idle)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float shootTimestamp = this.shootTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) shootTimestamp & currentTime.HasValue && (bool) (Object) this.TargetEnemy && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > (double) this.minDistanceToShoot)
        return GameManager.RoomActive;
    }
    return false;
  }

  protected override bool ShouldAttack() => false;

  private void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE());

  private IEnumerator ShootIE()
  {
    EnemySpiderShooter enemySpiderShooter = this;
    enemySpiderShooter.updateDirection = false;
    enemySpiderShooter.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemySpiderShooter.ClearPaths();
    enemySpiderShooter.SetAnimation(enemySpiderShooter.shootAnticipationAnimation);
    enemySpiderShooter.LookAtTarget();
    AudioManager.Instance.PlayOneShot(enemySpiderShooter.warningSfx, enemySpiderShooter.transform.position);
    yield return (object) new WaitForEndOfFrame();
    float t = 0.0f;
    while ((double) t < (double) enemySpiderShooter.shootAnticipation / (double) enemySpiderShooter.Spine.timeScale)
    {
      float amt = t / enemySpiderShooter.shootAnticipation;
      enemySpiderShooter.SimpleSpineFlash.FlashWhite(amt);
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemySpiderShooter.SimpleSpineFlash.FlashWhite(false);
    enemySpiderShooter.LookAtTarget();
    enemySpiderShooter.SetAnimation(enemySpiderShooter.shootAnimation);
    enemySpiderShooter.AddAnimation(enemySpiderShooter.IdleAnimation, true);
    enemySpiderShooter.state.CURRENT_STATE = StateMachine.State.Attacking;
    AudioManager.Instance.PlayOneShot(enemySpiderShooter.attackSfx, enemySpiderShooter.transform.position);
    enemySpiderShooter.projectilePattern.Shoot(0.1f, (bool) (Object) enemySpiderShooter.TargetEnemy ? enemySpiderShooter.TargetEnemy.gameObject : (GameObject) null, enemySpiderShooter.transform);
    yield return (object) new WaitForSeconds(enemySpiderShooter.shootCooldown / enemySpiderShooter.Spine.timeScale);
    enemySpiderShooter.TargetEnemy = (Health) null;
    enemySpiderShooter.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiderShooter.shootTimestamp = GameManager.GetInstance().CurrentTime + Random.Range(enemySpiderShooter.timeBetweenShooting.x, enemySpiderShooter.timeBetweenShooting.y);
    enemySpiderShooter.updateDirection = true;
  }
}
