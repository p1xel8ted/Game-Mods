// Decompiled with JetBrains decompiler
// Type: EnemySpiderMortar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderMortar : EnemySpider
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnimation;
  [SerializeField]
  private EnemyBomb shootPrefab;
  [SerializeField]
  private float shootAnticipation;
  [SerializeField]
  private float shootDuration;
  [SerializeField]
  private float shootCooldown;
  [SerializeField]
  private float minDistanceToShoot;
  [SerializeField]
  private float timeBetweenShooting;
  [SerializeField]
  private bool shootAtTarget;
  private float shootTimestamp;

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
    while ((double) t < (double) enemySpiderMortar.shootAnticipation / (double) enemySpiderMortar.Spine.timeScale)
    {
      float amt = t / enemySpiderMortar.shootAnticipation;
      enemySpiderMortar.SimpleSpineFlash.FlashWhite(amt);
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemySpiderMortar.SimpleSpineFlash.FlashWhite(false);
    enemySpiderMortar.SetAnimation(enemySpiderMortar.shootAnimation);
    enemySpiderMortar.AddAnimation(enemySpiderMortar.IdleAnimation, true);
    enemySpiderMortar.state.CURRENT_STATE = StateMachine.State.Attacking;
    Vector3 position = !enemySpiderMortar.shootAtTarget || !((Object) enemySpiderMortar.TargetEnemy != (Object) null) ? (Vector3) Random.insideUnitCircle * 5f : enemySpiderMortar.TargetEnemy.transform.position;
    Object.Instantiate<EnemyBomb>(enemySpiderMortar.shootPrefab, position, Quaternion.identity, enemySpiderMortar.transform.parent).Play(enemySpiderMortar.transform.position, enemySpiderMortar.shootDuration);
    AudioManager.Instance.PlayOneShot(enemySpiderMortar.attackSfx, enemySpiderMortar.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemySpiderMortar.gameObject);
    yield return (object) new WaitForSeconds(enemySpiderMortar.shootCooldown / enemySpiderMortar.Spine.timeScale);
    enemySpiderMortar.TargetEnemy = (Health) null;
    enemySpiderMortar.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiderMortar.shootTimestamp = GameManager.GetInstance().CurrentTime + enemySpiderMortar.timeBetweenShooting;
    enemySpiderMortar.updateDirection = true;
  }
}
