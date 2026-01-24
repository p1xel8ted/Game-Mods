// Decompiled with JetBrains decompiler
// Type: DynamicNotification_RitualActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class DynamicNotification_RitualActive : DynamicNotificationData
{
  public NotificationCentre.NotificationType _type;

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
      if (this._type == NotificationCentre.NotificationType.RitualHalloween)
        return !FollowerBrainStats.IsBloodMoon;
      if (this._type == NotificationCentre.NotificationType.RitualPurge)
        return !FollowerBrainStats.IsPurge;
      if (this._type == NotificationCentre.NotificationType.RitualNudism)
        return !FollowerBrainStats.IsNudism;
      if (this._type == NotificationCentre.NotificationType.RitualRanch_Harvest)
        return !FollowerBrainStats.IsRanchHarvest;
      return this._type != NotificationCentre.NotificationType.RitualRanch_Meat || !FollowerBrainStats.IsRanchMeat;
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
      if (this._type == NotificationCentre.NotificationType.RitualPurge)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastPurgeDeclared) / 1200.0);
      if (this._type == NotificationCentre.NotificationType.RitualNudism)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastNudismDeclared) / 600.0);
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
      else if (this._type == NotificationCentre.NotificationType.RitualRanch_Harvest)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastRanchRitualHarvest) / 1200.0);
      else if (this._type == NotificationCentre.NotificationType.RitualRanch_Meat)
        num = (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastRanchRitualMeat) / 1200.0);
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
      case UpgradeSystem.Type.Ritual_Purge:
        this._type = NotificationCentre.NotificationType.RitualPurge;
        break;
      case UpgradeSystem.Type.Ritual_Nudism:
        this._type = NotificationCentre.NotificationType.RitualNudism;
        break;
      case UpgradeSystem.Type.Ritual_RanchMeat:
        this._type = NotificationCentre.NotificationType.RitualRanch_Meat;
        break;
      case UpgradeSystem.Type.Ritual_RanchHarvest:
        this._type = NotificationCentre.NotificationType.RitualRanch_Harvest;
        break;
    }
  }
}
