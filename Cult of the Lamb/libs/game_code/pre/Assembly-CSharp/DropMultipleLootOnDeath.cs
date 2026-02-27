// Decompiled with JetBrains decompiler
// Type: DropMultipleLootOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class DropMultipleLootOnDeath : BaseMonoBehaviour
{
  private Health health;
  public List<DropMultipleLootOnDeath.ItemAndProbability> LootToDrop = new List<DropMultipleLootOnDeath.ItemAndProbability>();
  public bool useGenericForestEnemyLoot;
  public bool IsNaturalResource;
  public Vector2 RandomAmountToDrop = Vector2.one;
  [Range(0.0f, 100f)]
  public float chanceToDropLoot = 100f;

  private void OnEnable()
  {
    if ((UnityEngine.Object) this.health == (UnityEngine.Object) null)
      this.health = this.GetComponent<Health>();
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
      CameraManager.shakeCamera(0.25f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    for (int index = this.LootToDrop.Count - 1; index >= 0; --index)
    {
      if (this.LootToDrop[index].Type == InventoryItem.ITEM_TYPE.MUSHROOM_SMALL && DataManager.Instance.SozoStoryProgress == -1)
      {
        this.LootToDrop.RemoveAt(index);
        break;
      }
    }
    int[] weights = new int[this.LootToDrop.Count];
    int index1 = -1;
    while (++index1 < this.LootToDrop.Count)
      weights[index1] = this.LootToDrop[index1].Probability;
    for (int index2 = 0; (double) index2 < (double) UnityEngine.Random.Range(this.RandomAmountToDrop.x, this.RandomAmountToDrop.y + 1f); ++index2)
    {
      if ((double) UnityEngine.Random.Range(0.0f, 100f) <= (double) this.chanceToDropLoot * (double) DifficultyManager.GetLuckMultiplier())
      {
        InventoryItem.ITEM_TYPE type = this.LootToDrop[Utils.GetRandomWeightedIndex(weights)].Type;
        int quantity = 1;
        if (this.IsNaturalResource)
          quantity = quantity + TrinketManager.GetLootIncreaseModifier(type) + UpgradeSystem.GetForageIncreaseModifier;
        InventoryItem.Spawn(type, quantity, this.transform.position);
      }
    }
  }

  public void spawnLoot()
  {
    int[] weights = new int[this.LootToDrop.Count];
    int index = -1;
    while (++index < this.LootToDrop.Count)
      weights[index] = this.LootToDrop[index].Probability;
    InventoryItem.Spawn(this.LootToDrop[Utils.GetRandomWeightedIndex(weights)].Type, 1, this.transform.position);
  }

  [Serializable]
  public class ItemAndProbability
  {
    public InventoryItem.ITEM_TYPE Type;
    [Range(1f, 100f)]
    public int Probability = 1;

    public ItemAndProbability(InventoryItem.ITEM_TYPE Type, int Probability)
    {
      this.Type = Type;
      this.Probability = Probability;
    }
  }
}
