// Decompiled with JetBrains decompiler
// Type: Structures_WeatherVane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_WeatherVane : StructureBrain
{
  public override void OnAdded()
  {
    base.OnAdded();
    DataManager.Instance.HasWeatherVane = true;
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    DataManager.Instance.HasWeatherVane = false;
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
  }

  public void OnNewDay()
  {
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Spring || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS < 0.89999997615814209 && TimeManager.CurrentDay + 1 <= SeasonsManager.SeasonTimestamp || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 1.0 || !DataManager.Instance.WinterLoopEnabled)
      return;
    NotificationCentre.Instance.PlayGenericNotification($"Notifications/Winter/Coming/{DataManager.Instance.NextWinterServerity}", NotificationBase.Flair.Winter);
  }
}
