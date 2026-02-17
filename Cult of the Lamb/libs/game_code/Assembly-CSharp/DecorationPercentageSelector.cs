// Decompiled with JetBrains decompiler
// Type: DecorationPercentageSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DecorationPercentageSelector : MonoBehaviour
{
  [SerializeField]
  public List<GameObject> targetObjects;

  public void Start()
  {
    if (this.targetObjects.Count <= 0)
      return;
    this.EnableDecorations();
  }

  public void EnableDecorations()
  {
    float num = (float) (GameManager.CurrentDungeonLayer - 1) / 3f;
    for (int index = 0; index < this.targetObjects.Count; ++index)
    {
      if ((Object) this.targetObjects[index] != (Object) null)
        this.targetObjects[index].gameObject.SetActive((double) num > (double) Random.value);
    }
  }

  public void GetChildren()
  {
    this.targetObjects.Clear();
    for (int index = 0; index < this.transform.childCount; ++index)
      this.targetObjects.Add(this.transform.GetChild(index).gameObject);
  }
}
