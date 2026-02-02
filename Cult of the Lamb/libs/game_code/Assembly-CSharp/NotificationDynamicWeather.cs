// Decompiled with JetBrains decompiler
// Type: NotificationDynamicWeather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationDynamicWeather : NotificationDynamicBase
{
  [SerializeField]
  public Image _icon;
  [Header("Icons")]
  [SerializeField]
  public Sprite _blizzardIcon;
  public SeasonsManager.WeatherEvent WeatherType;

  public override Color FullColour => StaticColors.GreenColor;

  public override Color EmptyColour => StaticColors.OrangeColor;

  public override void Configure(DynamicNotificationData data)
  {
    if (data.Type == NotificationCentre.NotificationType.Blizzard)
      this.WeatherType = SeasonsManager.WeatherEvent.Blizzard;
    base.Configure(data);
  }

  public override void UpdateIcon()
  {
    if (!(this.Data is DynamicNotification_WeatherActive data) || data.Type != NotificationCentre.NotificationType.Blizzard)
      return;
    this._icon.sprite = this._blizzardIcon;
  }
}
