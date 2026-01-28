// Decompiled with JetBrains decompiler
// Type: NotificationDynamicRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationDynamicRitual : NotificationDynamicBase
{
  [SerializeField]
  public Image _icon;
  [Header("Icons")]
  [SerializeField]
  public Sprite _holidayIcon;
  [SerializeField]
  public Sprite _workThroughNight;
  [SerializeField]
  public Sprite _fasterBuilding;
  [SerializeField]
  public Sprite _fasting;
  [SerializeField]
  public Sprite _fishing;
  [SerializeField]
  public Sprite _brainwashing;
  [SerializeField]
  public Sprite _enlightenment;
  [SerializeField]
  public Sprite _bloodMoon;
  [SerializeField]
  public Sprite _purge;
  [SerializeField]
  public Sprite _nudism;
  [SerializeField]
  public Sprite _ranchHarvest;
  [SerializeField]
  public Sprite _ranchMeat;
  [CompilerGenerated]
  public UpgradeSystem.Type \u003CRitual\u003Ek__BackingField;

  public UpgradeSystem.Type Ritual
  {
    set => this.\u003CRitual\u003Ek__BackingField = value;
    get => this.\u003CRitual\u003Ek__BackingField;
  }

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
        case NotificationCentre.NotificationType.RitualPurge:
          this.Ritual = UpgradeSystem.Type.Ritual_Purge;
          break;
        case NotificationCentre.NotificationType.RitualNudism:
          this.Ritual = UpgradeSystem.Type.Ritual_Nudism;
          break;
        case NotificationCentre.NotificationType.RitualRanch_Meat:
          this.Ritual = UpgradeSystem.Type.Ritual_RanchMeat;
          break;
        case NotificationCentre.NotificationType.RitualRanch_Harvest:
          this.Ritual = UpgradeSystem.Type.Ritual_RanchHarvest;
          break;
      }
    }
    base.Configure(data);
  }

  public override void UpdateIcon()
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
      case NotificationCentre.NotificationType.RitualPurge:
        this._icon.sprite = this._purge;
        break;
      case NotificationCentre.NotificationType.RitualNudism:
        this._icon.sprite = this._nudism;
        break;
      case NotificationCentre.NotificationType.RitualRanch_Meat:
        this._icon.sprite = this._ranchMeat;
        break;
      case NotificationCentre.NotificationType.RitualRanch_Harvest:
        this._icon.sprite = this._ranchHarvest;
        break;
    }
  }
}
