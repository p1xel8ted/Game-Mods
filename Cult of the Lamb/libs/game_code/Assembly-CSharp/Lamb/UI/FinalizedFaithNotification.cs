// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedFaithNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine.Serialization;

#nullable disable
namespace Lamb.UI;

[MessagePackObject(false)]
[Serializable]
public class FinalizedFaithNotification : FinalizedNotification
{
  [Key(3)]
  public float FaithDelta;
  [Key(4)]
  [FormerlySerializedAs("FollowerInfo")]
  public FollowerInfoSnapshot followerInfoSnapshot;
}
