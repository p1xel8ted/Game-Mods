// Decompiled with JetBrains decompiler
// Type: EnemyGroupsChance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class EnemyGroupsChance : BaseMonoBehaviour
{
  public List<GroupProbability> Groups;

  public void OnEnable()
  {
  }

  public void InitEnemies()
  {
    if (RoomManager.r.EnemyChoice == -1)
    {
      int[] weights = new int[this.Groups.Count];
      int index = -1;
      while (++index < this.Groups.Count)
        weights[index] = this.Groups[index].Probability;
      RoomManager.r.EnemyChoice = Utils.GetRandomWeightedIndex(weights);
    }
    int index1 = -1;
    while (++index1 < this.Groups.Count)
      this.Groups[index1].GroupObject.SetActive(false);
    this.Groups[RoomManager.r.EnemyChoice].GroupObject.SetActive(true);
  }

  public void GetGroups()
  {
    this.Groups = new List<GroupProbability>();
    int index = -1;
    while (++index < this.transform.childCount)
    {
      this.Groups.Add(new GroupProbability(this.transform.GetChild(index).gameObject));
      this.transform.GetChild(index).name = "Enemies " + (index + 1).ToString();
    }
  }

  public void ResetWeights()
  {
    foreach (GroupProbability group in this.Groups)
      group.Probability = 50;
  }
}
