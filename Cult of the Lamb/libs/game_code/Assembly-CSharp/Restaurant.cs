// Decompiled with JetBrains decompiler
// Type: Restaurant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Restaurant : BaseMonoBehaviour
{
  public static List<Restaurant> AllRestaurants = new List<Restaurant>();
  public static List<Restaurant> FreeRestaurants = new List<Restaurant>();
  public List<Transform> Positions = new List<Transform>();
  public List<GameObject> Workers = new List<GameObject>();

  public void OnEnable()
  {
    for (int index = 0; index < this.Positions.Count; ++index)
      this.Workers.Add((GameObject) null);
    Restaurant.AllRestaurants.Add(this);
    Restaurant.FreeRestaurants.Add(this);
  }

  public void OnDisable()
  {
    this.Workers = new List<GameObject>();
    Restaurant.AllRestaurants.Remove(this);
    Restaurant.FreeRestaurants.Remove(this);
  }

  public static Transform GetFreeRestaurant(GameObject gameObject)
  {
    if (Restaurant.FreeRestaurants.Count < 1)
      return (Transform) null;
    int index1 = Random.Range(0, Restaurant.FreeRestaurants.Count);
    Restaurant freeRestaurant = Restaurant.FreeRestaurants[index1];
    for (int index2 = 0; index2 < freeRestaurant.Positions.Count; ++index2)
    {
      if ((Object) freeRestaurant.Workers[index2] == (Object) null)
      {
        freeRestaurant.Workers[index2] = gameObject;
        if (index2 == freeRestaurant.Positions.Count - 1)
          Restaurant.FreeRestaurants.Remove(freeRestaurant);
        return freeRestaurant.Positions[index2];
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
    if (Restaurant.FreeRestaurants.Contains(this))
      return;
    Restaurant.FreeRestaurants.Add(this);
  }
}
