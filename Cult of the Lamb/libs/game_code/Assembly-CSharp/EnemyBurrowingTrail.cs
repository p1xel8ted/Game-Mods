// Decompiled with JetBrains decompiler
// Type: EnemyBurrowingTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyBurrowingTrail : BaseMonoBehaviour
{
  [SerializeField]
  public float damageColliderDelay = 0.3f;
  [SerializeField]
  public Collider2D damageCollider;

  public Collider2D DamageCollider => this.damageCollider;

  public void Start()
  {
    SkeletonAnimation componentInChildren = this.gameObject.GetComponentInChildren<SkeletonAnimation>(true);
    if (!((Object) componentInChildren != (Object) null) || !((Object) SkeletonAnimationLODGlobalManager.Instance != (Object) null))
      return;
    SkeletonAnimationLODGlobalManager.Instance.DisableCulling(componentInChildren.transform, componentInChildren);
  }

  public void OnEnable()
  {
    this.damageCollider.enabled = false;
    this.StartCoroutine((IEnumerator) this.EnableColliderIE());
  }

  public void OnDisable()
  {
    this.StopAllCoroutines();
    this.damageCollider.enabled = true;
  }

  public IEnumerator EnableColliderIE()
  {
    yield return (object) new WaitForSeconds(this.damageColliderDelay);
    this.damageCollider.enabled = true;
    float timer = 0.0f;
    while ((double) (timer += Time.deltaTime) <= 0.5)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      yield return (object) null;
    }
    this.damageCollider.enabled = false;
  }
}
