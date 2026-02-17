// Decompiled with JetBrains decompiler
// Type: EnemySpiderMortar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderMortar : EnemySpider
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  [SerializeField]
  public EnemyBomb shootPrefab;
  [SerializeField]
  public float shootAnticipation;
  [SerializeField]
  public float shootDuration;
  [SerializeField]
  public float shootCooldown;
  [SerializeField]
  public float minDistanceToShoot;
  [SerializeField]
  public float timeBetweenShooting;
  [SerializeField]
  public bool shootAtTarget;
  public float shootTimestamp;

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
    EnemySpiderMortar enemySpiderMortar = this;
    enemySpiderMortar.updateDirection = false;
    enemySpiderMortar.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemySpiderMortar.ClearPaths();
    enemySpiderMortar.SetAnimation(enemySpiderMortar.shootAnticipationAnimation);
    if (enemySpiderMortar.shootAtTarget)
      enemySpiderMortar.LookAtTarget();
    AudioManager.Instance.PlayOneShot(enemySpiderMortar.warningSfx, enemySpiderMortar.transform.position);
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (Object) enemySpiderMortar.Body.Spine)
      enemySpiderMortar.Body.Spine.skeleton.ScaleX = 1f;
    enemySpiderMortar.Body.transform.localScale = new Vector3(enemySpiderMortar.Spine.skeleton.ScaleX, 1f, 1f);
    float t = 0.0f;
    while ((double) t < (double) enemySpiderMortar.shootAnticipation)
    {
      float amt = t / enemySpiderMortar.shootAnticipation;
      enemySpiderMortar.SimpleSpineFlash.FlashWhite(amt);
      t += Time.deltaTime * enemySpiderMortar.Spine.timeScale;
      yield return (object) null;
    }
    enemySpiderMortar.SimpleSpineFlash.FlashWhite(false);
    enemySpiderMortar.SetAnimation(enemySpiderMortar.shootAnimation);
    enemySpiderMortar.AddAnimation(enemySpiderMortar.IdleAnimation, true);
    enemySpiderMortar.state.CURRENT_STATE = StateMachine.State.Attacking;
    Vector3 position = !enemySpiderMortar.shootAtTarget || !((Object) enemySpiderMortar.TargetEnemy != (Object) null) ? (Vector3) Random.insideUnitCircle * 5f : enemySpiderMortar.TargetEnemy.transform.position;
    Object.Instantiate<EnemyBomb>(enemySpiderMortar.shootPrefab, position, Quaternion.identity, enemySpiderMortar.transform.parent).Play(enemySpiderMortar.transform.position, enemySpiderMortar.shootDuration, enemySpiderMortar.health.team);
    AudioManager.Instance.PlayOneShot(enemySpiderMortar.attackSfx, enemySpiderMortar.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemySpiderMortar.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMortar.Spine.timeScale) < (double) enemySpiderMortar.shootCooldown)
      yield return (object) null;
    enemySpiderMortar.TargetEnemy = (Health) null;
    enemySpiderMortar.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiderMortar.shootTimestamp = GameManager.GetInstance().CurrentTime + enemySpiderMortar.timeBetweenShooting;
    enemySpiderMortar.updateDirection = true;
  }
}
