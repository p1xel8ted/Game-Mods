// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedFollowerNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine.Serialization;

#nullable disable
namespace Lamb.UI;

[MessagePackObject(false)]
[Serializable]
public class FinalizedFollowerNotification : FinalizedNotification
{
  [Key(3)]
  [FormerlySerializedAs("FollowerInfo")]
  public FollowerInfoSnapshot followerInfoSnapshot;
  [Key(4)]
  public NotificationFollower.Animation Animation;
}
