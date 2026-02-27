// Decompiled with JetBrains decompiler
// Type: EnemyBurrowingTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyBurrowingTrail : BaseMonoBehaviour
{
  public Action<EnemyBurrowingTrail> OnActivate;
  public Action<EnemyBurrowingTrail> OnDeactivate;
  [SerializeField]
  public float damageColliderDelay = 0.3f;
  [SerializeField]
  public Collider2D damageCollider;
  [CompilerGenerated]
  public ColliderEvents \u003CColliderEvents\u003Ek__BackingField;

  public Collider2D DamageCollider => this.damageCollider;

  public ColliderEvents ColliderEvents
  {
    get => this.\u003CColliderEvents\u003Ek__BackingField;
    set => this.\u003CColliderEvents\u003Ek__BackingField = value;
  }

  public void Awake() => this.ColliderEvents = this.GetComponentInChildren<ColliderEvents>();

  public void Start()
  {
    SkeletonAnimation componentInChildren = this.gameObject.GetComponentInChildren<SkeletonAnimation>(true);
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null) || !((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null))
      return;
    SkeletonAnimationLODGlobalManager.Instance.DisableCulling(componentInChildren.transform, componentInChildren);
  }

  public void OnEnable()
  {
    this.damageCollider.enabled = false;
    this.StartCoroutine(this.EnableColliderIE());
  }

  public void OnDisable()
  {
    this.StopAllCoroutines();
    this.damageCollider.enabled = true;
    Action<EnemyBurrowingTrail> onDeactivate = this.OnDeactivate;
    if (onDeactivate == null)
      return;
    onDeactivate(this);
  }

  public IEnumerator EnableColliderIE()
  {
    EnemyBurrowingTrail enemyBurrowingTrail = this;
    yield return (object) new WaitForSeconds(enemyBurrowingTrail.damageColliderDelay);
    enemyBurrowingTrail.damageCollider.enabled = true;
    Action<EnemyBurrowingTrail> onActivate = enemyBurrowingTrail.OnActivate;
    if (onActivate != null)
      onActivate(enemyBurrowingTrail);
    float timer = 0.0f;
    while ((double) (timer += Time.deltaTime) <= 0.5)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      yield return (object) null;
    }
    enemyBurrowingTrail.damageCollider.enabled = false;
  }
}
