// Decompiled with JetBrains decompiler
// Type: EnemyMillipedeSpiker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyMillipedeSpiker : EnemyMillipede
{
  [SerializeField]
  public float anticipation;
  [SerializeField]
  public float attackDistance;
  [SerializeField]
  public float cooldown;
  [SerializeField]
  public bool stopMovingOnAttack;
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackAnimation;
  public bool attacking;

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

  public bool CanAttack()
  {
    return !this.attacking && (bool) (Object) this.GetClosestTarget() && (double) Vector3.Distance(this.GetCenterPosition(), this.GetClosestTarget().transform.position) < (double) this.attackDistance;
  }

  public IEnumerator Attack()
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
      t += Time.deltaTime * enemyMillipedeSpiker.Spine.timeScale;
      yield return (object) null;
    }
    foreach (SimpleSpineFlash flash in enemyMillipedeSpiker.flashes)
      flash.FlashWhite(false);
    enemyMillipedeSpiker.SetAnimation(enemyMillipedeSpiker.attackAnimation);
    enemyMillipedeSpiker.AddAnimation(enemyMillipedeSpiker.idleAnimation, true);
    enemyMillipedeSpiker.EnableDamageColliders();
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/attack", enemyMillipedeSpiker.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", enemyMillipedeSpiker.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyMillipedeSpiker.Spine.timeScale) < (double) enemyMillipedeSpiker.cooldown)
      yield return (object) null;
    enemyMillipedeSpiker.attacking = false;
  }
}
