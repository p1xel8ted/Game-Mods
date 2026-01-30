// Decompiled with JetBrains decompiler
// Type: randomChildPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public void GetChildren()
  {
    Debug.Log((object) "Retrieving children...");
    this.GameObjects = new GameObject[this.transform.childCount];
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      GameObject gameObject = this.transform.GetChild(index)?.gameObject;
      if ((Object) gameObject != (Object) null)
      {
        Debug.Log((object) $"Child {index}: {gameObject.name}");
        this.GameObjects[index] = gameObject;
      }
      else
        Debug.LogWarning((object) $"Child {index} is null!");
    }
  }

  public void SetAllActive()
  {
    if (this.GameObjects == null || this.GameObjects.Length == 0)
    {
      Debug.LogWarning((object) "GameObjects array is empty! Call GetChildren first.");
    }
    else
    {
      foreach (GameObject gameObject in this.GameObjects)
      {
        if ((Object) gameObject != (Object) null)
          gameObject.SetActive(true);
      }
    }
  }

  public void Start()
  {
    if (this.GameObjects == null || this.GameObjects.Length == 0)
    {
      Debug.Log((object) "GameObjects array is empty. Automatically calling GetChildren.");
      this.GetChildren();
    }
    switch (this.selectionMode)
    {
      case randomChildPicker.SelectionMode.RandomChance:
        this.HandleRandomChance();
        break;
      case randomChildPicker.SelectionMode.RandomSingle:
        this.HandleRandomSingle();
        break;
      case randomChildPicker.SelectionMode.RandomRange:
        this.HandleRandomRange();
        break;
    }
  }

  public void HandleRandomChance()
  {
    if (this.GameObjects == null)
      return;
    foreach (GameObject gameObject in this.GameObjects)
    {
      if (!((Object) gameObject == (Object) null))
      {
        int num = Random.Range(0, 100);
        gameObject.SetActive(num <= this.chanceToEnable);
      }
    }
  }

  public void HandleRandomSingle()
  {
    if (this.GameObjects == null)
      return;
    foreach (GameObject gameObject in this.GameObjects)
    {
      if ((Object) gameObject != (Object) null)
        gameObject.SetActive(false);
    }
    int index = Random.Range(0, this.GameObjects.Length);
    if (!((Object) this.GameObjects[index] != (Object) null))
      return;
    this.GameObjects[index].SetActive(true);
  }

  public void HandleRandomRange()
  {
    if (this.GameObjects == null)
      return;
    List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) this.GameObjects);
    int num = Mathf.Min(Random.Range(this.MinMax.x, this.MinMax.y), gameObjectList.Count);
    foreach (GameObject gameObject in gameObjectList)
    {
      if ((Object) gameObject != (Object) null)
        gameObject.SetActive(false);
    }
    for (; num > 0 && gameObjectList.Count > 0; --num)
    {
      int index = Random.Range(0, gameObjectList.Count);
      if ((Object) gameObjectList[index] != (Object) null)
      {
        gameObjectList[index].SetActive(true);
        gameObjectList.RemoveAt(index);
      }
    }
  }

  public enum SelectionMode
  {
    RandomChance,
    RandomSingle,
    RandomRange,
  }
}
