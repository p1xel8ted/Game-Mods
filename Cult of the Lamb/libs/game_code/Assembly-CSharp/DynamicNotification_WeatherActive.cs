// Decompiled with JetBrains decompiler
// Type: DynamicNotification_WeatherActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class DynamicNotification_WeatherActive : DynamicNotificationData
{
  public NotificationCentre.NotificationType _type;

  public override NotificationCentre.NotificationType Type => this._type;

  public override bool IsEmpty
  {
    get
    {
      return this._type != NotificationCentre.NotificationType.Blizzard || SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard;
    }
  }

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float num = 0.0f;
      if (this._type == NotificationCentre.NotificationType.Blizzard)
        num = SeasonsManager.WEATHER_EVENT_NORMALISED_PROGRESS;
      float currentProgress = 1f - num;
      if ((double) currentProgress <= 0.0)
      {
        System.Action dataChanged = this.DataChanged;
        if (dataChanged != null)
          dataChanged();
      }
      return currentProgress;
    }
  }

  public override float TotalCount => 0.0f;

  public override string SkinName => "";

  public override int SkinColor => 0;

  public DynamicNotification_WeatherActive(NotificationCentre.NotificationType type)
  {
    this._type = type;
  }
}
