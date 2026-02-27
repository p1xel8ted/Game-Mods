// Decompiled with JetBrains decompiler
// Type: Restaurant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Restaurant : BaseMonoBehaviour
{
  public static List<Restaurant> AllRestaurants = new List<Restaurant>();
  public static List<Restaurant> FreeRestaurants = new List<Restaurant>();
  public List<Transform> Positions = new List<Transform>();
  private List<GameObject> Workers = new List<GameObject>();

  private void OnEnable()
  {
    for (int index = 0; index < this.Positions.Count; ++index)
      this.Workers.Add((GameObject) null);
    Restaurant.AllRestaurants.Add(this);
    Restaurant.FreeRestaurants.Add(this);
  }

  private void OnDisable()
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
