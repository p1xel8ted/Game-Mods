// Decompiled with JetBrains decompiler
// Type: EnemySpiderShooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderShooter : EnemySpider
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  [SerializeField]
  public float shootAnticipation;
  [SerializeField]
  public float shootDuration;
  [SerializeField]
  public float shootCooldown;
  [SerializeField]
  public float minDistanceToShoot;
  [SerializeField]
  public Vector2 timeBetweenShooting;
  public float shootTimestamp;
  public ProjectilePattern projectilePattern;

  public void Start() => this.projectilePattern = this.GetComponentInChildren<ProjectilePattern>();

  public override void Update()
  {
    base.Update();
    if (!this.ShouldShoot())
      return;
    this.Shoot();
  }

  public bool ShouldShoot()
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

  public override bool ShouldAttack() => false;

  public void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE());

  public IEnumerator ShootIE()
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
    while ((double) t < (double) enemySpiderShooter.shootAnticipation)
    {
      float amt = t / enemySpiderShooter.shootAnticipation;
      enemySpiderShooter.SimpleSpineFlash.FlashWhite(amt);
      t += Time.deltaTime * enemySpiderShooter.Spine.timeScale;
      yield return (object) null;
    }
    enemySpiderShooter.SimpleSpineFlash.FlashWhite(false);
    enemySpiderShooter.LookAtTarget();
    enemySpiderShooter.SetAnimation(enemySpiderShooter.shootAnimation);
    enemySpiderShooter.AddAnimation(enemySpiderShooter.IdleAnimation, true);
    enemySpiderShooter.state.CURRENT_STATE = StateMachine.State.Attacking;
    AudioManager.Instance.PlayOneShot(enemySpiderShooter.attackSfx, enemySpiderShooter.transform.position);
    enemySpiderShooter.projectilePattern.Shoot(0.1f, (bool) (Object) enemySpiderShooter.TargetEnemy ? enemySpiderShooter.TargetEnemy.gameObject : (GameObject) null, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderShooter.Spine.timeScale) < (double) enemySpiderShooter.shootCooldown)
      yield return (object) null;
    enemySpiderShooter.TargetEnemy = (Health) null;
    enemySpiderShooter.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiderShooter.shootTimestamp = GameManager.GetInstance().CurrentTime + Random.Range(enemySpiderShooter.timeBetweenShooting.x, enemySpiderShooter.timeBetweenShooting.y);
    enemySpiderShooter.updateDirection = true;
  }
}
