// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedRelationshipNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
