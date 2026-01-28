// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DynamicNotificationInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class DynamicNotificationInfoCard : UIInfoCardBase<DynamicNotificationData>
{
  [SerializeField]
  public TextMeshProUGUI _notificationTitle;
  [SerializeField]
  public TextMeshProUGUI _notificationDescription;
  [SerializeField]
  public TextMeshProUGUI _icon;

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
      case NotificationCentre.NotificationType.Dynamic_Injured:
        this._icon.text = "<sprite name=\"icon_Injured\">";
        this._notificationTitle.text = LocalizationManager.GetTranslation("UI/Injured");
        this._notificationDescription.text = string.Format(LocalizationManager.GetTranslation("Tutorial UI/Injured/Info1"), (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
      case NotificationCentre.NotificationType.Dynamic_Drunk:
        this._icon.text = "<sprite name=\"icon_Drunk\">";
        this._notificationTitle.text = LocalizationManager.GetTranslation("UI/Befuddled");
        this._notificationDescription.text = string.Format(LocalizationManager.GetTranslation("Tutorial UI/Befuddled/Info1"), (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
      case NotificationCentre.NotificationType.Dynamic_Freezing:
        this._icon.text = "<sprite name=\"icon_Freezing\">";
        this._notificationTitle.text = LocalizationManager.GetTranslation("UI/Freezing");
        this._notificationDescription.text = string.Format(LocalizationManager.GetTranslation("Tutorial UI/Freezing/Info1"), (object) config.TotalCount, (object) DataManager.Instance.Followers.Count);
        break;
    }
  }
}
