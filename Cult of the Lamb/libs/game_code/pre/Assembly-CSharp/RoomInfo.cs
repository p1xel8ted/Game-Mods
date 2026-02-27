// Decompiled with JetBrains decompiler
// Type: RoomInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class RoomInfo : BaseMonoBehaviour
{
  public AudioClip Music;
  [HideInInspector]
  public string ID;
  private static List<RoomInfo> RoomInfos = new List<RoomInfo>();
  public bool ForceClan;

  public void Init() => RoomInfo.RoomInfos.Add(this);

  private void OnDestroy() => RoomInfo.RoomInfos.Remove(this);

  public static RoomInfo GetByID(string ID)
  {
    foreach (RoomInfo roomInfo in RoomInfo.RoomInfos)
    {
      if (roomInfo.ID == ID)
        return roomInfo;
    }
    return (RoomInfo) null;
  }
}
