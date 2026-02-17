// Decompiled with JetBrains decompiler
// Type: IceWall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class IceWall : MonoBehaviour
{
  public GameObject imageContainer;
  public ColliderEvents colliderEvents;
  public Health health;
  public SkeletonAnimation parentSpine;
  public bool isDamaging;
  public float damageDuration = 0.5f;
  public float colliderTimer;
  public bool ignoreOtherTraps;

  public void InitWall(SkeletonAnimation parentSpine, Health health)
  {
    this.parentSpine = parentSpine;
    this.health = health;
    health.OnDie += new Health.DieAction(this.OnDie);
    this.transform.parent = BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
    this.isDamaging = false;
    this.colliderTimer = 0.0f;
    this.colliderEvents = this.GetComponentInChildren<ColliderEvents>(true);
    this.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnIceWallDamageTriggerEnter);
    this.AnimateIn();
    health.enabled = true;
    health.InitHP();
    this.StartCoroutine((IEnumerator) this.EnableDamageCollider(this.colliderEvents, 0.0f));
  }

  public void Update()
  {
    if (!this.isDamaging || (double) this.colliderTimer >= (double) this.damageDuration)
      return;
    this.colliderTimer += Time.deltaTime * this.parentSpine.timeScale;
    if ((double) this.colliderTimer < (double) this.damageDuration)
      return;
    this.colliderEvents?.SetActive(false);
  }

  public void Kill(GameObject killer)
  {
    if (this.ignoreOtherTraps && killer.gameObject.name.ToLower().Contains("trap"))
      return;
    this.health.DealDamage(1f, killer, killer.transform.position);
    this.gameObject.SetActive(false);
  }

  public void AnimateIn()
  {
    Vector3 position = this.transform.position;
    this.imageContainer.transform.position = new Vector3(position.x, position.y, 2f);
    this.imageContainer.transform.DOMove(new Vector3(position.x, position.y, 0.0f), 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
  }

  public void OnIceWallDamageTriggerEnter(Collider2D collider)
  {
    if (this.ignoreOtherTraps && collider.gameObject.name.ToLower().Contains("trap"))
      return;
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || !((Object) component != (Object) this.health) || component.team == this.health.team && !component.IsCharmedEnemy && collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public IEnumerator EnableDamageCollider(ColliderEvents damageCollider, float initialDelay)
  {
    if ((bool) (Object) damageCollider)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * ((Object) this.parentSpine != (Object) null ? this.parentSpine.timeScale : 1f)) < (double) initialDelay)
        yield return (object) null;
      damageCollider.SetActive(true);
      this.isDamaging = true;
    }
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.colliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnIceWallDamageTriggerEnter);
    this.gameObject.Recycle();
  }
}
