// Decompiled with JetBrains decompiler
// Type: Demon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Demon : MonoBehaviour
{
  protected int followerID;
  private FollowerInfo followerInfo;

  public int Level
  {
    get
    {
      if (this.followerInfo == null)
        this.followerInfo = FollowerInfo.GetInfoByID(this.followerID);
      return this.followerInfo == null ? 1 : this.followerInfo.XPLevel;
    }
  }

  public virtual void Init(int followerID) => this.followerID = followerID;

  public static float GetDemonLeftovers()
  {
    float demonLeftovers = 0.0f;
    for (int index = 0; index < DataManager.Instance.Followers_Demons_IDs.Count; ++index)
    {
      if (DataManager.Instance.Followers_Demons_Types[index] == 2)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]);
        demonLeftovers += infoById.XPLevel > 1 ? 0.1f * (float) infoById.XPLevel : 0.0f;
      }
    }
    return demonLeftovers;
  }
}
