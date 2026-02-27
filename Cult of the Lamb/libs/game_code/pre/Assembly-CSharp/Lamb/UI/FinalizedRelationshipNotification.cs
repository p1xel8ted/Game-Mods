// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedRelationshipNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Lamb.UI;

[Serializable]
public class FinalizedRelationshipNotification : FinalizedNotification
{
  public FollowerInfoSnapshot followerInfoSnapshotA;
  public FollowerInfoSnapshot followerInfoSnapshotB;
  public NotificationFollower.Animation FollowerAnimationA;
  public NotificationFollower.Animation FollowerAnimationB;
}
