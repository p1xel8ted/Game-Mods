// Decompiled with JetBrains decompiler
// Type: EntertainmentPlace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EntertainmentPlace : BaseMonoBehaviour
{
  public static List<EntertainmentPlace> AllEntertainmentPlaces = new List<EntertainmentPlace>();
  public static List<EntertainmentPlace> FreeEntertainmentPlaces = new List<EntertainmentPlace>();
  public List<Transform> Positions = new List<Transform>();
  public List<GameObject> Workers = new List<GameObject>();

  public void OnEnable()
  {
    for (int index = 0; index < this.Positions.Count; ++index)
      this.Workers.Add((GameObject) null);
    EntertainmentPlace.AllEntertainmentPlaces.Add(this);
    EntertainmentPlace.FreeEntertainmentPlaces.Add(this);
  }

  public void OnDisable()
  {
    this.Workers = new List<GameObject>();
    EntertainmentPlace.AllEntertainmentPlaces.Remove(this);
    EntertainmentPlace.FreeEntertainmentPlaces.Remove(this);
  }

  public static Transform GetFreeEntertainmentPlace(GameObject gameObject)
  {
    if (EntertainmentPlace.FreeEntertainmentPlaces.Count < 1)
      return (Transform) null;
    int index1 = Random.Range(0, EntertainmentPlace.FreeEntertainmentPlaces.Count);
    EntertainmentPlace entertainmentPlace = EntertainmentPlace.FreeEntertainmentPlaces[index1];
    for (int index2 = 0; index2 < entertainmentPlace.Positions.Count; ++index2)
    {
      if ((Object) entertainmentPlace.Workers[index2] == (Object) null)
      {
        entertainmentPlace.Workers[index2] = gameObject;
        if (index2 == entertainmentPlace.Positions.Count - 1)
          EntertainmentPlace.FreeEntertainmentPlaces.Remove(entertainmentPlace);
        return entertainmentPlace.Positions[index2];
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
    if (EntertainmentPlace.FreeEntertainmentPlaces.Contains(this))
      return;
    EntertainmentPlace.FreeEntertainmentPlaces.Add(this);
  }
}
