// Decompiled with JetBrains decompiler
// Type: RandomInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RandomInstantiator : BaseMonoBehaviour
{
  public GameObject[] GameObjects;
  public RandomInstantiator.SelectionMode selectionMode;
  public bool pickingMultiple;
  [Range(0.0f, 100f)]
  public int chanceToEnable;

  private void Start()
  {
    switch (this.selectionMode)
    {
      case RandomInstantiator.SelectionMode.RandomChance:
        if (this.GameObjects == null || this.GameObjects.Length == 0)
          break;
        for (int index = 0; index < this.GameObjects.Length; ++index)
        {
          if (Random.Range(0, 100) <= this.chanceToEnable)
            ObjectPool.Spawn(this.GameObjects[index].gameObject, this.transform);
        }
        break;
      case RandomInstantiator.SelectionMode.RandomSingle:
        if (this.GameObjects == null || this.GameObjects.Length == 0)
          break;
        int index1 = Random.Range(0, this.GameObjects.Length);
        if (!((Object) this.GameObjects[index1] != (Object) null))
          break;
        ObjectPool.Spawn(this.GameObjects[index1].gameObject, this.transform);
        break;
    }
  }

  public enum SelectionMode
  {
    RandomChance,
    RandomSingle,
  }
}
