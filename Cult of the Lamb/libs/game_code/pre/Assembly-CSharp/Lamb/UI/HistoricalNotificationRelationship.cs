// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationRelationship : 
  HistoricalNotificationBase<FinalizedRelationshipNotification>
{
  [SerializeField]
  private SkeletonGraphic _followerSpineA;
  [SerializeField]
  private SkeletonGraphic _followerSpineB;

  protected override void ConfigureImpl(
    FinalizedRelationshipNotification finalizedNotification)
  {
    this._followerSpineA.ConfigureFollowerSkin(finalizedNotification.followerInfoSnapshotA);
    this._followerSpineA.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(finalizedNotification.FollowerAnimationA), true);
    this._followerSpineB.ConfigureFollowerSkin(finalizedNotification.followerInfoSnapshotB);
    this._followerSpineB.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(finalizedNotification.FollowerAnimationB), true);
  }

  protected override string GetLocalizedDescription(
    FinalizedRelationshipNotification finalizedNotification)
  {
    return string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object) finalizedNotification.followerInfoSnapshotA.Name, (object) finalizedNotification.followerInfoSnapshotB.Name);
  }
}
