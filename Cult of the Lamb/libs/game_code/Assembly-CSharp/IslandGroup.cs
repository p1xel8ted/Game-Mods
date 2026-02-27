// Decompiled with JetBrains decompiler
// Type: IslandGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IslandGroup : BaseMonoBehaviour
{
  public List<GroupProbability> Groups;

  public void OnEnable()
  {
    if ((Object) RoomManager.Instance != (Object) null)
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
      if ((Object) transform2 == (Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Islands";
        transform2 = gameObject.transform;
      }
      Transform transform3 = transform1.Find("Enemies");
      if ((Object) transform3 == (Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Enemies";
        transform3 = gameObject.transform;
      }
      Transform transform4 = transform1.Find("Traps");
      if ((Object) transform4 == (Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Traps";
        transform4 = gameObject.transform;
      }
      Transform transform5 = transform1.Find("Resources");
      if ((Object) transform5 == (Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Resources";
        transform5 = gameObject.transform;
      }
      Transform transform6 = transform1.Find("Blocks");
      if ((Object) transform6 == (Object) null)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = transform1;
        gameObject.name = "Blocks";
        transform6 = gameObject.transform;
      }
      foreach (Transform transform7 in transform1.transform)
      {
        if (transform7.name.Contains("Trap"))
          transform7.parent = transform4;
        else if (transform7.name.Contains("Enemy"))
          transform7.parent = transform3;
        else if (transform7.name.Contains("Resource"))
          transform7.parent = transform5;
        else if (transform7.name.Contains("Block"))
          transform7.parent = transform6;
        else if (transform7.name.Contains("Island"))
          transform7.parent = transform2;
      }
    }
  }
}
