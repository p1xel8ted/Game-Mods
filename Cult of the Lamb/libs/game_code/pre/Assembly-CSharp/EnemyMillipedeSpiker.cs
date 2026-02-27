// Decompiled with JetBrains decompiler
// Type: EnemyMillipedeSpiker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyMillipedeSpiker : EnemyMillipede
{
  [SerializeField]
  private float anticipation;
  [SerializeField]
  private float attackDistance;
  [SerializeField]
  private float cooldown;
  [SerializeField]
  private bool stopMovingOnAttack;
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnimation;
  protected bool attacking;

  public override void OnEnable()
  {
    base.OnEnable();
    this.attacking = false;
  }

  public override void Update()
  {
    if (this.CanAttack())
      this.StartCoroutine((IEnumerator) this.Attack());
    base.Update();
  }

  protected bool CanAttack()
  {
    return !this.attacking && (bool) (Object) this.GetClosestTarget() && (double) Vector3.Distance(this.GetCenterPosition(), this.GetClosestTarget().transform.position) < (double) this.attackDistance;
  }

  private IEnumerator Attack()
  {
    EnemyMillipedeSpiker enemyMillipedeSpiker = this;
    enemyMillipedeSpiker.attacking = true;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/warning", enemyMillipedeSpiker.gameObject);
    enemyMillipedeSpiker.SetAnimation(enemyMillipedeSpiker.attackAnticipationAnimation, true);
    yield return (object) new WaitForEndOfFrame();
    if (enemyMillipedeSpiker.stopMovingOnAttack)
    {
      enemyMillipedeSpiker.moveVX = 0.0f;
      enemyMillipedeSpiker.moveVY = 0.0f;
    }
    float t = 0.0f;
    while ((double) t < (double) enemyMillipedeSpiker.anticipation)
    {
      float amt = t / enemyMillipedeSpiker.anticipation;
      foreach (SimpleSpineFlash flash in enemyMillipedeSpiker.flashes)
        flash.FlashWhite(amt);
      t += Time.deltaTime;
      yield return (object) null;
    }
    foreach (SimpleSpineFlash flash in enemyMillipedeSpiker.flashes)
      flash.FlashWhite(false);
    enemyMillipedeSpiker.SetAnimation(enemyMillipedeSpiker.attackAnimation);
    enemyMillipedeSpiker.AddAnimation(enemyMillipedeSpiker.idleAnimation, true);
    enemyMillipedeSpiker.EnableDamageColliders();
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/attack", enemyMillipedeSpiker.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", enemyMillipedeSpiker.gameObject);
    yield return (object) new WaitForSeconds(enemyMillipedeSpiker.cooldown);
    enemyMillipedeSpiker.attacking = false;
  }
}
