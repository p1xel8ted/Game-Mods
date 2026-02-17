// Decompiled with JetBrains decompiler
// Type: RoomInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class RoomInfo : BaseMonoBehaviour
{
  public AudioClip Music;
  [HideInInspector]
  public string ID;
  public static List<RoomInfo> RoomInfos = new List<RoomInfo>();
  public bool ForceClan;

  public void Init() => RoomInfo.RoomInfos.Add(this);

  public void OnDestroy() => RoomInfo.RoomInfos.Remove(this);

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
