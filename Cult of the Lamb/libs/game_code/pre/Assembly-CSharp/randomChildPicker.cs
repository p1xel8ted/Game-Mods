// Decompiled with JetBrains decompiler
// Type: randomChildPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class randomChildPicker : BaseMonoBehaviour
{
  public GameObject[] GameObjects;
  public randomChildPicker.SelectionMode selectionMode;
  public bool pickingMultiple;
  [Range(0.0f, 100f)]
  public int chanceToEnable;
  public Vector2Int MinMax = new Vector2Int(5, 8);

  private void GetChildren()
  {
    this.GameObjects = new GameObject[this.transform.childCount];
    int index = -1;
    while (++index < this.transform.childCount)
      this.GameObjects[index] = this.transform.GetChild(index).gameObject;
  }

  public void SetAllActive()
  {
    foreach (GameObject gameObject in this.GameObjects)
      gameObject.SetActive(true);
  }

  private void Start()
  {
    switch (this.selectionMode)
    {
      case randomChildPicker.SelectionMode.RandomChance:
        if (this.GameObjects == null || this.GameObjects.Length == 0)
          break;
        for (int index = 0; index < this.GameObjects.Length; ++index)
        {
          if (Random.Range(0, 100) <= this.chanceToEnable)
            this.GameObjects[index].SetActive(true);
          else
            this.GameObjects[index].SetActive(false);
        }
        break;
      case randomChildPicker.SelectionMode.RandomSingle:
        if (this.GameObjects == null || this.GameObjects.Length == 0)
          break;
        for (int index = 0; index < this.GameObjects.Length; ++index)
        {
          if ((Object) this.GameObjects[index] != (Object) null)
            this.GameObjects[index].SetActive(false);
        }
        int index1 = Random.Range(0, this.GameObjects.Length);
        if (!((Object) this.GameObjects[index1] != (Object) null))
          break;
        this.GameObjects[index1].SetActive(true);
        break;
      case randomChildPicker.SelectionMode.RandomRange:
        List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) this.GameObjects);
        int num = Mathf.Min(Random.Range(this.MinMax.x, this.MinMax.y), gameObjectList.Count);
        foreach (GameObject gameObject in gameObjectList)
          gameObject.SetActive(false);
        for (; num > 0; --num)
        {
          int index2 = Random.Range(0, gameObjectList.Count);
          gameObjectList[index2].SetActive(true);
          gameObjectList.RemoveAt(index2);
        }
        break;
    }
  }

  public enum SelectionMode
  {
    RandomChance,
    RandomSingle,
    RandomRange,
  }
}
