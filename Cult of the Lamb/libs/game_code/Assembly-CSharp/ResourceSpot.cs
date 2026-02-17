// Decompiled with JetBrains decompiler
// Type: ResourceSpot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ResourceSpot : BaseMonoBehaviour
{
  public int _current;

  public int Current
  {
    get => this._current;
    set
    {
      this._current = value;
      if (this._current > this.transform.childCount - 1)
        this._current = 0;
      if (this._current >= 0)
        return;
      this._current = this.transform.childCount - 1;
    }
  }

  public void OnEnable()
  {
    if (Application.isEditor && !Application.isPlaying)
    {
      this.Current = 0;
    }
    else
    {
      Object.Destroy((Object) this.gameObject);
      int num = Random.Range(0, 3);
      StructuresData structuresData = (StructuresData) null;
      switch (num)
      {
        case 0:
          structuresData = StructuresData.GetInfoByType(StructureBrain.TYPES.TREE, (double) Random.value < 0.5 ? 1 : 2);
          break;
        case 1:
          structuresData = StructuresData.GetInfoByType(StructureBrain.TYPES.ROCK, 0);
          break;
        case 2:
          structuresData = StructuresData.GetInfoByType(StructureBrain.TYPES.COTTON_BUSH, 0);
          break;
      }
      Object.Instantiate<GameObject>(Resources.Load(structuresData.PrefabPath) as GameObject, this.transform.parent, true).transform.position = this.transform.position;
      GameManager.RecalculatePaths();
    }
  }

  public void Next()
  {
    ++this.Current;
    int index = -1;
    while (++index < this.transform.childCount)
      this.transform.GetChild(index).gameObject.SetActive(index == this.Current);
  }

  public void Previous()
  {
    --this.Current;
    int index = -1;
    while (++index < this.transform.childCount)
      this.transform.GetChild(index).gameObject.SetActive(index == this.Current);
  }
}
