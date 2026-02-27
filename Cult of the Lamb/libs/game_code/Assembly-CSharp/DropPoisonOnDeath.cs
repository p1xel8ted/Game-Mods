// Decompiled with JetBrains decompiler
// Type: DropPoisonOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DropPoisonOnDeath : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject poisonPrefab;
  [SerializeField]
  public DropPoisonOnDeath.SpawnType spawnType;
  [SerializeField]
  public int amount;
  [SerializeField]
  public float radius;
  public Health health;

  public void Awake()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.spawnType != DropPoisonOnDeath.SpawnType.OnDeath)
      return;
    this.SpawnPoison();
  }

  public void OnDisable()
  {
    if (this.spawnType != DropPoisonOnDeath.SpawnType.OnDisable)
      return;
    this.SpawnPoison();
  }

  public void SpawnPoison()
  {
    for (int index = 0; index < this.amount; ++index)
      UnityEngine.Object.Instantiate<GameObject>(this.poisonPrefab, this.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * this.radius), Quaternion.identity, this.transform.parent);
  }

  [Serializable]
  public enum SpawnType
  {
    OnDeath,
    OnDisable,
  }
}
