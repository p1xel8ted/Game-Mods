// Decompiled with JetBrains decompiler
// Type: WorshipPlace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorshipPlace : BaseMonoBehaviour
{
  public static List<WorshipPlace> AllWorshipPlaces = new List<WorshipPlace>();
  public static List<WorshipPlace> FreeWorshipPlaces = new List<WorshipPlace>();
  public List<Transform> Positions = new List<Transform>();
  public List<GameObject> Workers = new List<GameObject>();

  public void Start()
  {
    for (int index = 0; index < this.Positions.Count; ++index)
      this.Workers.Add((GameObject) null);
  }

  public void OnEnable()
  {
    WorshipPlace.AllWorshipPlaces.Add(this);
    WorshipPlace.FreeWorshipPlaces.Add(this);
  }

  public void OnDisable()
  {
    WorshipPlace.AllWorshipPlaces.Remove(this);
    WorshipPlace.FreeWorshipPlaces.Remove(this);
  }

  public static Transform GetFreeWorshipPlace(GameObject gameObject)
  {
    if (WorshipPlace.FreeWorshipPlaces.Count < 1)
      return (Transform) null;
    int index1 = Random.Range(0, WorshipPlace.FreeWorshipPlaces.Count);
    WorshipPlace freeWorshipPlace = WorshipPlace.FreeWorshipPlaces[index1];
    for (int index2 = 0; index2 < freeWorshipPlace.Positions.Count; ++index2)
    {
      if ((Object) freeWorshipPlace.Workers[index2] == (Object) null)
      {
        freeWorshipPlace.Workers[index2] = gameObject;
        if (index2 == freeWorshipPlace.Positions.Count - 1)
          WorshipPlace.FreeWorshipPlaces.Remove(freeWorshipPlace);
        return freeWorshipPlace.Positions[index2];
      }
    }
    return (Transform) null;
  }

  public void RemoveFromPositions(GameObject gameObject)
  {
    int index = this.Workers.IndexOf(gameObject);
    if (index == -1)
      return;
    this.Workers[index] = (GameObject) null;
    if (WorshipPlace.FreeWorshipPlaces.Contains(this))
      return;
    WorshipPlace.FreeWorshipPlaces.Add(this);
  }
}
