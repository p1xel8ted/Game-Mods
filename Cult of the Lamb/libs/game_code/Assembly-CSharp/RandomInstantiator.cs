// Decompiled with JetBrains decompiler
// Type: RandomInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RandomInstantiator : BaseMonoBehaviour
{
  public GameObject[] GameObjects;
  public RandomInstantiator.SelectionMode selectionMode;
  public bool pickingMultiple;
  [Range(0.0f, 100f)]
  public int chanceToEnable;

  public void Start()
  {
    switch (this.selectionMode)
    {
      case RandomInstantiator.SelectionMode.RandomChance:
        if (this.GameObjects == null || this.GameObjects.Length == 0)
          break;
        for (int index = 0; index < this.GameObjects.Length; ++index)
        {
          if (Random.Range(0, 100) <= this.chanceToEnable && this.CanItemBeSpawned(this.GameObjects[index]))
            ObjectPool.Spawn(this.GameObjects[index].gameObject, this.transform);
        }
        break;
      case RandomInstantiator.SelectionMode.RandomSingle:
        if (this.GameObjects == null || this.GameObjects.Length == 0)
          break;
        int index1 = Random.Range(0, this.GameObjects.Length);
        if (!((Object) this.GameObjects[index1] != (Object) null) || !this.CanItemBeSpawned(this.GameObjects[index1]))
          break;
        ObjectPool.Spawn(this.GameObjects[index1].gameObject, this.transform);
        break;
    }
  }

  public bool CanItemBeSpawned(GameObject item)
  {
    return !InventoryItem.IsHeart(InventoryItem.GetInventoryItemTypeOf(item)) || !PlayerFleeceManager.FleecePreventsHealthPickups();
  }

  public enum SelectionMode
  {
    RandomChance,
    RandomSingle,
  }
}
