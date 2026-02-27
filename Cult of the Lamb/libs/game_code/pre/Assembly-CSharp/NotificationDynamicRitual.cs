// Decompiled with JetBrains decompiler
// Type: NotificationDynamicRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationDynamicRitual : NotificationDynamicBase
{
  [SerializeField]
  protected Image _icon;
  [Header("Icons")]
  [SerializeField]
  private Sprite _holidayIcon;
  [SerializeField]
  private Sprite _workThroughNight;
  [SerializeField]
  private Sprite _fasterBuilding;
  [SerializeField]
  private Sprite _fasting;
  [SerializeField]
  private Sprite _fishing;
  [SerializeField]
  private Sprite _brainwashing;
  [SerializeField]
  private Sprite _enlightenment;
  [SerializeField]
  private Sprite _bloodMoon;

  public UpgradeSystem.Type Ritual { private set; get; }

  public override Color FullColour => StaticColors.GreenColor;

  public override Color EmptyColour => StaticColors.OrangeColor;

  public override void Configure(DynamicNotificationData data)
  {
    if (data is DynamicNotification_RitualActive notificationRitualActive)
    {
      switch (notificationRitualActive.Type)
      {
        case NotificationCentre.NotificationType.RitualHoliday:
          this.Ritual = UpgradeSystem.Type.Ritual_Holiday;
          break;
        case NotificationCentre.NotificationType.RitualWorkThroughNight:
          this.Ritual = UpgradeSystem.Type.Ritual_WorkThroughNight;
          break;
        case NotificationCentre.NotificationType.RitualFasterBuilding:
          this.Ritual = UpgradeSystem.Type.Ritual_FasterBuilding;
          break;
        case NotificationCentre.NotificationType.RitualFast:
          this.Ritual = UpgradeSystem.Type.Ritual_Fast;
          break;
        case NotificationCentre.NotificationType.RitualFishing:
          this.Ritual = UpgradeSystem.Type.Ritual_FishingRitual;
          break;
        case NotificationCentre.NotificationType.RitualBrainwashing:
          this.Ritual = UpgradeSystem.Type.Ritual_Brainwashing;
          break;
        case NotificationCentre.NotificationType.RitualEnlightenment:
          this.Ritual = UpgradeSystem.Type.Ritual_Enlightenment;
          break;
        case NotificationCentre.NotificationType.RitualHalloween:
          this.Ritual = UpgradeSystem.Type.Ritual_Halloween;
          break;
      }
    }
    base.Configure(data);
  }

  protected override void UpdateIcon()
  {
    if (!(this.Data is DynamicNotification_RitualActive data))
      return;
    switch (data.Type)
    {
      case NotificationCentre.NotificationType.RitualHoliday:
        this._icon.sprite = this._holidayIcon;
        break;
      case NotificationCentre.NotificationType.RitualWorkThroughNight:
        this._icon.sprite = this._workThroughNight;
        break;
      case NotificationCentre.NotificationType.RitualFasterBuilding:
        this._icon.sprite = this._fasterBuilding;
        break;
      case NotificationCentre.NotificationType.RitualFast:
        this._icon.sprite = this._fasting;
        break;
      case NotificationCentre.NotificationType.RitualFishing:
        this._icon.sprite = this._fishing;
        break;
      case NotificationCentre.NotificationType.RitualBrainwashing:
        this._icon.sprite = this._brainwashing;
        break;
      case NotificationCentre.NotificationType.RitualEnlightenment:
        this._icon.sprite = this._enlightenment;
        break;
      case NotificationCentre.NotificationType.RitualHalloween:
        this._icon.sprite = this._bloodMoon;
        break;
    }
  }
}
