// Decompiled with JetBrains decompiler
// Type: MillipedeBodyPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class MillipedeBodyPart : BaseMonoBehaviour
{
  public static List<MillipedeBodyPart> bodyParts = new List<MillipedeBodyPart>();
  [SerializeField]
  public SkeletonAnimation Spine;
  public EnemyMillipede millipede;
  public Health health;
  public bool dropped;

  public void Awake()
  {
    this.millipede = this.GetComponentInParent<EnemyMillipede>();
    this.health = this.GetComponent<Health>();
    this.health.OnDamaged += new Health.HealthEvent(this.OnDamaged);
  }

  public void OnDestroy()
  {
    if ((bool) (Object) this.health)
      this.health.OnDamaged -= new Health.HealthEvent(this.OnDamaged);
    MillipedeBodyPart.bodyParts.Remove(this);
  }

  public void OnDamaged(
    GameObject attacker,
    Vector3 attackLocation,
    float damage,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlag)
  {
    if (!((Object) this.millipede != (Object) null) || !((Object) attacker != (Object) this.millipede.gameObject) || this.dropped)
      return;
    this.millipede.DamageFromBody(attacker, attackLocation, damage, attackType, attackFlag);
  }

  public void DroppedPart()
  {
    this.transform.localScale = Vector3.one;
    MillipedeBodyPart.bodyParts.Add(this);
    this.dropped = true;
  }

  public void SpawnEnemy(List<AsyncOperationHandle<GameObject>> enemies, Transform parent)
  {
    this.StartCoroutine(this.SpawnIE(enemies, parent));
  }

  public IEnumerator SpawnIE(List<AsyncOperationHandle<GameObject>> enemies, Transform parent)
  {
    MillipedeBodyPart millipedeBodyPart = this;
    yield return (object) millipedeBodyPart.Spine.YieldForAnimation("transform");
    BiomeConstants.Instance.EmitSmokeExplosionVFX(millipedeBodyPart.transform.position);
    for (int index = 0; index < Random.Range(1, 3); ++index)
      ObjectPool.Spawn(enemies[Random.Range(0, enemies.Count)].Result, parent, millipedeBodyPart.transform.position, Quaternion.identity);
    millipedeBodyPart.gameObject.SetActive(false);
  }
}
