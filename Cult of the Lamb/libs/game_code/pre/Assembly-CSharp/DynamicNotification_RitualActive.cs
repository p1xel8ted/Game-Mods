// Decompiled with JetBrains decompiler
// Type: DynamicNotification_RitualActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DynamicNotification_RitualActive : DynamicNotificationData
{
  private NotificationCentre.NotificationType _type;

  public override NotificationCentre.NotificationType Type => this._type;

  public override bool IsEmpty
  {
    get
    {
      if (this._type == NotificationCentre.NotificationType.RitualHoliday)
        return !FollowerBrainStats.IsHoliday;
      if (this._type == NotificationCentre.NotificationType.RitualWorkThroughNight)
        return !FollowerBrainStats.IsWorkThroughTheNight;
      if (this._type == NotificationCentre.NotificationType.RitualFast)
        return !FollowerBrainStats.Fasting;
      if (this._type == NotificationCentre.NotificationType.RitualFishing)
        return !FollowerBrainStats.IsFishing;
      if (this._type == NotificationCentre.NotificationType.RitualBrainwashing)
        return !FollowerBrainStats.BrainWashed;
      if (this._type == NotificationCentre.NotificationType.RitualEnlightenment)
        return !FollowerBrainStats.IsEnlightened;
      return this._type != NotificationCentre.NotificationType.RitualHalloween || !FollowerBrainStats.IsBloodMoon;
    }
  }

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float num = 0.0f;
      if (this._type == NotificationCentre.NotificationType.RitualHoliday)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastHolidayDeclared) / 1200.0);
      else if (this._type == NotificationCentre.NotificationType.RitualWorkThroughNight)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastWorkThroughTheNight) / 3600.0);
      else if (this._type == NotificationCentre.NotificationType.RitualFast)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFastDeclared) / 3600.0);
      else if (this._type == NotificationCentre.NotificationType.RitualFishing)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFishingDeclared) / 3600.0);
      else if (this._type == NotificationCentre.NotificationType.RitualBrainwashing)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastBrainwashed) / 3600.0);
      else if (this._type == NotificationCentre.NotificationType.RitualEnlightenment)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastEnlightenment) / 3600.0);
      else if (this._type == NotificationCentre.NotificationType.RitualHalloween)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastHalloween) / 3600.0);
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

  public DynamicNotification_RitualActive(UpgradeSystem.Type type)
  {
    switch (type)
    {
      case UpgradeSystem.Type.Ritual_Enlightenment:
        this._type = NotificationCentre.NotificationType.RitualEnlightenment;
        break;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        this._type = NotificationCentre.NotificationType.RitualWorkThroughNight;
        break;
      case UpgradeSystem.Type.Ritual_Holiday:
        this._type = NotificationCentre.NotificationType.RitualHoliday;
        break;
      case UpgradeSystem.Type.Ritual_Fast:
        this._type = NotificationCentre.NotificationType.RitualFast;
        break;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        this._type = NotificationCentre.NotificationType.RitualFishing;
        break;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        this._type = NotificationCentre.NotificationType.RitualBrainwashing;
        break;
      case UpgradeSystem.Type.Ritual_Halloween:
        this._type = NotificationCentre.NotificationType.RitualHalloween;
        break;
    }
  }
}
