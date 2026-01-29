// Decompiled with JetBrains decompiler
// Type: IslandGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IslandGroup : BaseMonoBehaviour
{
  public List<GroupProbability> Groups;

  public void OnEnable()
  {
    if ((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null)
      RoomManager.Instance.OnInitEnemies += new RoomManager.InitEnemiesAction(this.InitEnemies);
    else
      this.InitEnemies();
  }

  public void OnDisable()
  {
    RoomManager.Instance.OnInitEnemies -= new RoomManager.InitEnemiesAction(this.InitEnemies);
  }

  public void InitEnemies()
  {
    if (RoomManager.r.IslandChoice == -1)
    {
      int[] weights = new int[this.Groups.Count];
      int index = -1;
      while (++index < this.Groups.Count)
        weights[index] = this.Groups[index].Probability;
      RoomManager.r.IslandChoice = Utils.GetRandomWeightedIndex(weights);
    }
    int index1 = -1;
    while (++index1 < this.Groups.Count)
      this.Groups[index1].GroupObject.SetActive(false);
    this.Groups[RoomManager.r.IslandChoice].GroupObject.SetActive(true);
    foreach (EnemyGroupsChance componentsInChild in this.Groups[RoomManager.r.IslandChoice].GroupObject.GetComponentsInChildren<EnemyGroupsChance>())
      componentsInChild.InitEnemies();
  }

  public void GetGroups()
  {
    this.Groups = new List<GroupProbability>();
    int index = -1;
    while (++index < this.transform.childCount)
      this.Groups.Add(new GroupProbability(this.transform.GetChild(index).gameObject));
  }

  public void ResetWeights()
  {
    foreach (GroupProbability group in this.Groups)
      group.Probability = 50;
  }

  public void Organise()
  {
    int index = -1;
    while (++index < this.Groups.Count)
    {
      Transform transform1 = this.Groups[index].GroupObject.transform;
      Transform transform2 = transform1.Find("Islands");
      if ((UnityEngine.Object) transform2 == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Islands";
        transform2 = gameObject.transform;
      }
      Transform transform3 = transform1.Find("Enemies");
      if ((UnityEngine.Object) transform3 == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Enemies";
        transform3 = gameObject.transform;
      }
      Transform transform4 = transform1.Find("Traps");
      if ((UnityEngine.Object) transform4 == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Traps";
        transform4 = gameObject.transform;
      }
      Transform transform5 = transform1.Find("Resources");
      if ((UnityEngine.Object) transform5 == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Resources";
        transform5 = gameObject.transform;
      }
      Transform transform6 = transform1.Find("Blocks");
      if ((UnityEngine.Object) transform6 == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Blocks";
        transform6 = gameObject.transform;
      }
      IEnumerator enumerator = (IEnumerator) transform1.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          if (current.name.Contains("Trap"))
            current.parent = transform4;
          else if (current.name.Contains("Enemy"))
            current.parent = transform3;
          else if (current.name.Contains("Resource"))
            current.parent = transform5;
          else if (current.name.Contains("Block"))
            current.parent = transform6;
          else if (current.name.Contains("Island"))
            current.parent = transform2;
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
  }
}
