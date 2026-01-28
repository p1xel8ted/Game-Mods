// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationFollower : 
  HistoricalNotificationBase<FinalizedFollowerNotification>
{
  [SerializeField]
  public SkeletonGraphic _followerSpine;

  public override void ConfigureImpl(
    FinalizedFollowerNotification finalizedNotification)
  {
    this._followerSpine.ConfigureFollowerSkin(finalizedNotification.followerInfoSnapshot);
    this._followerSpine.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(finalizedNotification.Animation), true);
  }

  public override string GetLocalizedDescription(
    FinalizedFollowerNotification finalizedNotification)
  {
    return finalizedNotification.followerInfoSnapshot != null ? string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object) finalizedNotification.followerInfoSnapshot.Name) : "";
  }
}
