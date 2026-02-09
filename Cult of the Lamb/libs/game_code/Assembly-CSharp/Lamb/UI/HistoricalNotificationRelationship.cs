// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationRelationship : 
  HistoricalNotificationBase<FinalizedRelationshipNotification>
{
  [SerializeField]
  public SkeletonGraphic _followerSpineA;
  [SerializeField]
  public SkeletonGraphic _followerSpineB;

  public override void ConfigureImpl(
    FinalizedRelationshipNotification finalizedNotification)
  {
    this._followerSpineA.ConfigureFollowerSkin(finalizedNotification.followerInfoSnapshotA);
    this._followerSpineA.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(finalizedNotification.FollowerAnimationA), true);
    this._followerSpineB.ConfigureFollowerSkin(finalizedNotification.followerInfoSnapshotB);
    this._followerSpineB.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(finalizedNotification.FollowerAnimationB), true);
  }

  public override string GetLocalizedDescription(
    FinalizedRelationshipNotification finalizedNotification)
  {
    return string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object) finalizedNotification.followerInfoSnapshotA.Name, (object) finalizedNotification.followerInfoSnapshotB.Name);
  }
}
