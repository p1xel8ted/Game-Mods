// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DynamicNotificationInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class DynamicNotificationInfoCard : UIInfoCardBase<DynamicNotificationData>
{
  [SerializeField]
  private TextMeshProUGUI _notificationTitle;
  [SerializeField]
  private TextMeshProUGUI _notificationDescription;
  [SerializeField]
  private TextMeshProUGUI _icon;

  public override void Configure(DynamicNotificationData config)
  {
    switch (config.Type)
    {
      case NotificationCentre.NotificationType.Exhausted:
        this._icon.text = "<sprite name=\"icon_Sleep\">";
        this._notificationTitle.text = ScriptLocalization.UI_DynamicNotification_Exhausted.Title;
        this._notificationDescription.text = string.Format(ScriptLocalization.UI_DynamicNotification_Exhausted.Description, (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
      case NotificationCentre.NotificationType.Dynamic_Starving:
        this._icon.text = "\uF623";
        this._notificationTitle.text = ScriptLocalization.UI_DynamicNotification_FollowersStarving.Title;
        this._notificationDescription.text = string.Format(ScriptLocalization.UI_DynamicNotification_FollowersStarving.Description, (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
      case NotificationCentre.NotificationType.Dynamic_Homeless:
        this._icon.text = "\uF236";
        this._notificationTitle.text = ScriptLocalization.UI_DynamicNotification_NotEnoughBeds.Title;
        this._notificationDescription.text = string.Format(ScriptLocalization.UI_DynamicNotification_NotEnoughBeds.Description, (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
      case NotificationCentre.NotificationType.Dynamic_Sick:
        this._icon.text = "<sprite name=\"icon_Sickness\">";
        this._notificationTitle.text = ScriptLocalization.UI_DynamicNotification_Illness.Title;
        this._notificationDescription.text = string.Format(ScriptLocalization.UI_DynamicNotification_Illness.Description, (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
      case NotificationCentre.NotificationType.Dynamic_Dissenter:
        this._icon.text = "<sprite name=\"icon_Faith\">";
        this._notificationTitle.text = ScriptLocalization.Tutorial_UI.Dissenter;
        this._notificationDescription.text = ScriptLocalization.Tutorial_UI_Dissenter.Info1;
        break;
    }
  }
}
