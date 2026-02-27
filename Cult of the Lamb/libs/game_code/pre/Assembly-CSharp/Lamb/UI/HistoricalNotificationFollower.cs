// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationFollower : 
  HistoricalNotificationBase<FinalizedFollowerNotification>
{
  [SerializeField]
  private SkeletonGraphic _followerSpine;

  protected override void ConfigureImpl(
    FinalizedFollowerNotification finalizedNotification)
  {
    this._followerSpine.ConfigureFollower(finalizedNotification.followerInfoSnapshot);
  }

  protected override string GetLocalizedDescription(
    FinalizedFollowerNotification finalizedNotification)
  {
    return finalizedNotification.followerInfoSnapshot != null ? string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object) finalizedNotification.followerInfoSnapshot.Name) : "";
  }
}
