// Decompiled with JetBrains decompiler
// Type: Prison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Prison : BaseMonoBehaviour
{
  public static List<Prison> Prisons = new List<Prison>();
  public Structure Structure;
  public Transform PrisonerLocation;
  public GameObject PrisonerExitLocation;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  private void OnEnable() => Prison.Prisons.Add(this);

  private void OnDisable() => Prison.Prisons.Remove(this);

  public static bool HasAvailablePrisons()
  {
    foreach (Prison prison in Prison.Prisons)
    {
      if (prison.StructureInfo.FollowerID == -1)
        return true;
    }
    return false;
  }

  public static List<Follower> ImprisonableFollowers()
  {
    List<Follower> followerList = new List<Follower>((IEnumerable<Follower>) FollowerManager.FollowersAtLocation(FollowerLocation.Base));
    for (int index = followerList.Count - 1; index >= 0; --index)
    {
      int id = followerList[index].Brain.Info.ID;
      if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(id) || DataManager.Instance.Followers_OnMissionary_IDs.Contains(id) || DataManager.Instance.Followers_Demons_IDs.Contains(id))
        followerList.RemoveAt(index);
    }
    return followerList;
  }
}
