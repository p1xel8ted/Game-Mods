// Decompiled with JetBrains decompiler
// Type: DropPoisonOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DropPoisonOnDeath : BaseMonoBehaviour
{
  [SerializeField]
  private GameObject poisonPrefab;
  [SerializeField]
  private DropPoisonOnDeath.SpawnType spawnType;
  [SerializeField]
  private int amount;
  [SerializeField]
  private float radius;
  private Health health;

  private void Awake()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDie(
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

  private void OnDisable()
  {
    if (this.spawnType != DropPoisonOnDeath.SpawnType.OnDisable)
      return;
    this.SpawnPoison();
  }

  private void SpawnPoison()
  {
    for (int index = 0; index < this.amount; ++index)
      UnityEngine.Object.Instantiate<GameObject>(this.poisonPrefab, this.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * this.radius), Quaternion.identity, this.transform.parent);
  }

  [Serializable]
  private enum SpawnType
  {
    OnDeath,
    OnDisable,
  }
}
