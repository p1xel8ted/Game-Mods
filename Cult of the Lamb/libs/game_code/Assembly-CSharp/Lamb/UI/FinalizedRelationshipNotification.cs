// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedRelationshipNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace Lamb.UI;

[MessagePackObject(false)]
[Serializable]
public class FinalizedRelationshipNotification : FinalizedNotification
{
  [Key(3)]
  public FollowerInfoSnapshot followerInfoSnapshotA;
  [Key(4)]
  public FollowerInfoSnapshot followerInfoSnapshotB;
  [Key(5)]
  public NotificationFollower.Animation FollowerAnimationA;
  [Key(6)]
  public NotificationFollower.Animation FollowerAnimationB;
}
