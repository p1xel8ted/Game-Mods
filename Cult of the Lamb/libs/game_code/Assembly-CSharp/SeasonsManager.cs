// Decompiled with JetBrains decompiler
// Type: SeasonsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SeasonsManager
{
  public const float BLIZZARD_CHANCE = 0.025f;
  public static List<float> WEATHER_EVENT_CHANCES = new List<float>()
  {
    0.0f,
    0.025f
  };
  public static float WEATHER_EVENT_DURATION = 480f;
  public static List<float> WINTER_SPECIAL_EVENT_CHANCES = new List<float>()
  {
    0.2f
  };
  public static List<float> WINTER_SEVERITY_COLDNESS = new List<float>()
  {
    -10f,
    -10f,
    -15f,
    -20f,
    -20f
  };
  public const float NIGHT_COLDNESS = 10f;
  public static bool WinterEndingDisabled = false;
  public static Vector2 TIME_BETWEEN_OVERHEATING_FOLLOWER = new Vector2(480f, 960f);
  public static Vector2 TIME_BETWEEN_AFLAMED_STRUCTURE = new Vector2(240f, 720f);
  public static Vector2 TIME_BETWEEN_FREEZING_FOLLOWER = new Vector2(960f, 1440f);
  public static Vector2 TIME_BETWEEN_SNOWED_UNDER_STRUCTURE = new Vector2(192f, 432f);
  public static Vector2 TIME_BETWEEN_LIGHTNING_STRIKED_ENEMY = new Vector2(24f, 240f);
  public const float BLIZZARD_FREEZING_MULTIPLIER = 1.5f;
  public static List<InventoryItem> POSSIBLE_BLIZZARD_OFFERINGS = new List<InventoryItem>()
  {
    new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, UnityEngine.Random.Range(25, 41)),
    new InventoryItem(InventoryItem.ITEM_TYPE.LOG, UnityEngine.Random.Range(15, 31 /*0x1F*/)),
    new InventoryItem(InventoryItem.ITEM_TYPE.STONE, UnityEngine.Random.Range(10, 21)),
    new InventoryItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, UnityEngine.Random.Range(15, 31 /*0x1F*/)),
    new InventoryItem(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, UnityEngine.Random.Range(15, 31 /*0x1F*/))
  };
  public static int lightningStrikesThisBlizzard = 0;
  public static int maxLightningStrikesThisBlizzard = 0;
  public static List<float> lightningStrikeTimes = new List<float>();
  public static Vector2 TIME_BETWEEN_LIGHTNING_STRIKES = new Vector2(360f, 1200f);
  public static Vector2 TIME_BETWEEN_LIGHTNING_STRIKED_FOLLOWER = new Vector2(60f, 240f);
  public static Vector2 TIME_BETWEEN_LIGHTNING_STRIKED_STRUCTURE = new Vector2(48f, 120f);
  public static string WinterFollowersReactSFX = "event:/dlc/env/winter/start_followers";
  public static string WinterSeasonTitleInSFX = "event:/dlc/env/winter/start_title";
  public static string WinterSeasonEndInBaseSFX = "event:/dlc/env/winter/end";
  public static string WinterSeasonEndAnywhereSFX = "event:/dlc/env/winter/end_anywhere";
  public static string WinterSeasonStartAnywhereSFX = "event:/dlc/env/winter/start_anywhere";
  public static Coroutine blizzardRoutine;
  public static bool SetSeasonInstant = false;
  public static bool isInitialized = false;
  public static bool testBlizzards = false;
  public static int lastBlizzardSeedCheck = -1;
  public static int lastBlizzardSeverityCheck = -1;
  public static List<FollowerBrain> targetedFollowers = new List<FollowerBrain>();
  public static bool displayBlizzardName;

  public static bool Active
  {
    get => DataManager.Instance.SeasonsActive;
    set => DataManager.Instance.SeasonsActive = value;
  }

  public static SeasonsManager.Season CurrentSeason
  {
    get => DataManager.Instance.CurrentSeason;
    set => DataManager.Instance.CurrentSeason = value;
  }

  public static SeasonsManager.Season PreviousSeason
  {
    get => DataManager.Instance.PreviousSeason;
    set => DataManager.Instance.PreviousSeason = value;
  }

  public static SeasonsManager.WeatherEvent CurrentWeatherEvent
  {
    get => DataManager.Instance.CurrentWeatherEvent;
    set
    {
      Debug.Log((object) "Setting weather event");
      DataManager.Instance.CurrentWeatherEvent = value;
    }
  }

  public static int WeatherEventID
  {
    get => DataManager.Instance.WeatherEventID;
    set => DataManager.Instance.WeatherEventID = value;
  }

  public static int WeatherEventTriggeredDay
  {
    get => DataManager.Instance.WeatherEventTriggeredDay;
    set => DataManager.Instance.WeatherEventTriggeredDay = value;
  }

  public static float WeatherEventOverTargetTime
  {
    get => DataManager.Instance.WeatherEventOverTime;
    set => DataManager.Instance.WeatherEventOverTime = value;
  }

  public static float WeatherEventDurationTime
  {
    get => DataManager.Instance.WeatherEventDurationTime;
    set => DataManager.Instance.WeatherEventDurationTime = value;
  }

  public static int SeasonSpecialEventTriggeredDay
  {
    get => DataManager.Instance.SeasonSpecialEventTriggeredDay;
    set => DataManager.Instance.SeasonSpecialEventTriggeredDay = value;
  }

  public static event SeasonsManager.TemperatureEvent OnTemperatureChanged;

  public static event SeasonsManager.SeasonEvent OnSeasonChanged;

  public static event SeasonsManager.WeatherTypeEvent OnWeatherBegan;

  public static event SeasonsManager.WeatherTypeEvent OnWeatherEnded;

  public static event SeasonsManager.WeatherTypeEvent OnBlizzardBegan;

  public static event SeasonsManager.WeatherTypeEvent OnBlizzardEnded;

  public static event SeasonsManager.DeafultWeatherEvent OnDefaultWeatherBegan;

  public static event SeasonsManager.DeafultWeatherEvent OnDefaultWeatherEnded;

  public static int SeasonTimestamp
  {
    get => DataManager.Instance.SeasonTimestamp;
    set => DataManager.Instance.SeasonTimestamp = value;
  }

  public static int SEASON_CURRENT_DAY
  {
    get
    {
      return SeasonsManager.GetSeasonDuration(SeasonsManager.CurrentSeason) - (SeasonsManager.SeasonTimestamp - TimeManager.CurrentDay) + 1;
    }
  }

  public static float SEASON_NORMALISED_PROGRESS
  {
    get
    {
      if (!DataManager.Instance.WinterLoopEnabled)
        return 0.0f;
      if (DataManager.Instance.WinterModeActive)
        return 0.5f;
      float num = (float) (SeasonsManager.GetSeasonDuration(SeasonsManager.CurrentSeason) * 1200);
      if (DataManager.Instance.OnboardedLongNights)
        num += 240f;
      return 1f - ((float) (SeasonsManager.SeasonTimestamp * 1200) - (TimeManager.TotalElapsedGameTime - 1200f)) / num;
    }
  }

  public static float WEATHER_EVENT_NORMALISED_PROGRESS
  {
    get
    {
      return (double) SeasonsManager.WeatherEventDurationTime != -1.0 ? (float) (1.0 - ((double) SeasonsManager.WeatherEventOverTargetTime - (double) TimeManager.TotalElapsedGameTime) / (double) SeasonsManager.WeatherEventDurationTime) : 1f;
    }
  }

  public static float MIN_TEMPERATURE
  {
    get => SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard ? 15f : 0.0f;
  }

  public static float MAX_TEMPERATURE
  {
    get => SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Heatwave ? 85f : 100f;
  }

  public static int WinterSeverity
  {
    get => DataManager.Instance.WinterServerity;
    set => DataManager.Instance.WinterServerity = value;
  }

  public static bool NextPhaseIsWeatherEvent
  {
    get => DataManager.Instance.NextPhaseIsWeatherEvent;
    set => DataManager.Instance.NextPhaseIsWeatherEvent = value;
  }

  public static bool GivenBlizzardObjective
  {
    get => DataManager.Instance.GivenBlizzardObjective;
    set => DataManager.Instance.GivenBlizzardObjective = value;
  }

  public static int SPRING_DURATION => 5;

  public static int WINTER_DURATION => 5;

  public static float LastFollowerToStartOverheating
  {
    get => DataManager.Instance.LastFollowerToStartOverheating;
    set => DataManager.Instance.LastFollowerToStartOverheating = value;
  }

  public static float TimeSinceLastAflamedStructure
  {
    get => DataManager.Instance.TimeSinceLastAflamedStructure;
    set => DataManager.Instance.TimeSinceLastAflamedStructure = value;
  }

  public static float LastFollowerToStartFreezing
  {
    get => DataManager.Instance.LastFollowerToStartFreezing;
    set => DataManager.Instance.LastFollowerToStartFreezing = value;
  }

  public static float TimeSinceLastSnowedUnderStructure
  {
    get => DataManager.Instance.TimeSinceLastSnowedUnderStructure;
    set => DataManager.Instance.TimeSinceLastSnowedUnderStructure = value;
  }

  public static bool IsInitialized => SeasonsManager.isInitialized;

  public static void Initialise()
  {
    SeasonsManager.isInitialized = false;
    GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.WaitForPlayer());
    TimeManager.OnNewPhaseStarted += new System.Action(SeasonsManager.OnNewPhaseStarted);
    TimeManager.OnNewDayStarted += new System.Action(SeasonsManager.OnNewDay);
  }

  public static IEnumerator WaitForPlayer()
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    SeasonsManager.HandleWeatherTriggers();
    if (!DataManager.Instance.WinterLoopEnabled)
      SeasonsManager.SetSeason(SeasonsManager.Season.Spring);
    SeasonsManager.isInitialized = true;
  }

  public static SeasonsManager.Season GetSeasonForDay(int day)
  {
    SeasonsManager.Season season = SeasonsManager.CurrentSeason;
    int seasonTimestamp = SeasonsManager.SeasonTimestamp;
    if (day <= seasonTimestamp)
      return season;
    int seasonDuration;
    for (int index = 32 /*0x20*/; day > seasonTimestamp && index-- > 0; seasonTimestamp += seasonDuration)
    {
      season = season == SeasonsManager.Season.Spring ? SeasonsManager.Season.Winter : SeasonsManager.Season.Spring;
      seasonDuration = SeasonsManager.GetSeasonDuration(season);
    }
    return season;
  }

  public static bool IsStartOfSpring(int day)
  {
    int num = SeasonsManager.SPRING_DURATION + SeasonsManager.WINTER_DURATION;
    return day % num == 0;
  }

  public static bool IsStartOfWinter(int day)
  {
    int num = SeasonsManager.SPRING_DURATION + SeasonsManager.WINTER_DURATION;
    return day % num == SeasonsManager.SPRING_DURATION;
  }

  public static bool IsLongNightDay(int day)
  {
    if (SeasonsManager.GetSeasonForDay(day) != SeasonsManager.Season.Winter)
      return false;
    int num = SeasonsManager.SPRING_DURATION + SeasonsManager.WINTER_DURATION;
    return day % num - SeasonsManager.SPRING_DURATION + 1 == 3;
  }

  public static void HandleWeatherTriggers()
  {
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.None)
      SeasonsManager.TriggerWeatherEvent(SeasonsManager.CurrentWeatherEvent);
    else
      SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
    SeasonsManager.SetLongNight();
  }

  public static float SeededRandomRange(int seed, float rangeMin, float rangeMax)
  {
    UnityEngine.Random.State state = UnityEngine.Random.state;
    UnityEngine.Random.InitState(seed);
    double num = (double) UnityEngine.Random.Range(rangeMin, rangeMax);
    UnityEngine.Random.state = state;
    return (float) num;
  }

  public static void SetBlizzardTimes(bool force = false)
  {
    if (!force && SeasonsManager.lastBlizzardSeedCheck == DataManager.Instance.SeasonTimestamp && SeasonsManager.lastBlizzardSeverityCheck == SeasonsManager.WinterSeverity)
      return;
    Debug.Log((object) $"Calculating blizzard times for Winters occured {DataManager.Instance.WintersOccured.ToString()} Winter Severity {SeasonsManager.WinterSeverity.ToString()} Current save {DataManager.Instance.blizzardTimeInCurrentSeason.ToString()} Force? {force.ToString()}");
    SeasonsManager.lastBlizzardSeedCheck = DataManager.Instance.SeasonTimestamp;
    SeasonsManager.lastBlizzardSeverityCheck = SeasonsManager.WinterSeverity;
    int wintersOccured = DataManager.Instance.WintersOccured;
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring)
    {
      DataManager.Instance.RemoveBlizzardsBeforeTimestamp = 0.0f;
      DataManager.Instance.DisableBlizzard1 = false;
      DataManager.Instance.DisableBlizzard2 = false;
      ++wintersOccured;
    }
    float num1 = 0.005f * (float) SeasonsManager.WinterSeverity;
    float num2 = SeasonsManager.SeededRandomRange(wintersOccured, 0.1f, 0.15f + num1);
    float num3 = SeasonsManager.SeededRandomRange(wintersOccured, 0.2f, 0.4f);
    float num4 = num3 + num2;
    DataManager.Instance.blizzardTimeInCurrentSeason = num3;
    DataManager.Instance.blizzardEndTimeInCurrentSeason = num4;
    if (SeasonsManager.WinterSeverity >= 5)
    {
      float num5 = SeasonsManager.SeededRandomRange(wintersOccured, 0.1f, 0.15f + num1);
      float num6 = SeasonsManager.SeededRandomRange(wintersOccured, 0.6f, 0.8f);
      float num7 = num6 + num5;
      DataManager.Instance.blizzardTimeInCurrentSeason2 = num6;
      DataManager.Instance.blizzardEndTimeInCurrentSeason2 = num7;
    }
    else
    {
      DataManager.Instance.blizzardTimeInCurrentSeason2 = num3;
      DataManager.Instance.blizzardEndTimeInCurrentSeason2 = num4;
    }
    bool disableBlizzard1 = DataManager.Instance.DisableBlizzard1;
    bool disableBlizzard2 = DataManager.Instance.DisableBlizzard2;
    Debug.Log((object) $"Blizzard 1 disable {disableBlizzard1.ToString()} Blizzard 2 disable {disableBlizzard2.ToString()}");
    if (disableBlizzard1)
    {
      int num8 = -1;
      DataManager.Instance.blizzardTimeInCurrentSeason = (float) num8;
      DataManager.Instance.blizzardEndTimeInCurrentSeason = (float) num8;
    }
    if (disableBlizzard2)
    {
      int num9 = -1;
      DataManager.Instance.blizzardTimeInCurrentSeason2 = (float) num9;
      DataManager.Instance.blizzardEndTimeInCurrentSeason2 = (float) num9;
    }
    Debug.Log((object) $"Blizzard time reset is {DataManager.Instance.blizzardTimeInCurrentSeason.ToString()} {DataManager.Instance.DisableBlizzard1.ToString()}");
  }

  public static void DisableCurrentActiveBlizzards(bool forceAll = false)
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring)
      return;
    Debug.Log((object) "CHECKING DISABLED BLIZZARDS");
    float normalisedProgress = SeasonsManager.SEASON_NORMALISED_PROGRESS;
    if (forceAll || (double) normalisedProgress >= (double) DataManager.Instance.blizzardTimeInCurrentSeason && (double) normalisedProgress <= (double) DataManager.Instance.blizzardEndTimeInCurrentSeason)
    {
      interaction_FollowerInteraction.preventLightningStrikeTimestamp = Time.time + 20f;
      DataManager.Instance.DisableBlizzard1 = true;
      Debug.Log((object) "DISABLING BLIZZARD 1");
    }
    if (forceAll || (double) normalisedProgress >= (double) DataManager.Instance.blizzardTimeInCurrentSeason2 && (double) normalisedProgress <= (double) DataManager.Instance.blizzardEndTimeInCurrentSeason2)
    {
      interaction_FollowerInteraction.preventLightningStrikeTimestamp = Time.time + 20f;
      DataManager.Instance.DisableBlizzard2 = true;
      Debug.Log((object) "DISABLING BLIZZARD 2");
    }
    SeasonsManager.SetBlizzardTimes(true);
    SeasonsManager.SimpleBlizzardCheck(true);
  }

  public static void SimpleBlizzardCheck(bool debug = false)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || MMConversation.isPlaying)
      return;
    if (debug)
      Debug.Log((object) "break here");
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      if (!DataManager.Instance.OnboardedBlizzards && !SeasonsManager.testBlizzards)
        return;
      if ((double) DataManager.Instance.blizzardTimeInCurrentSeason <= 0.0)
        DataManager.Instance.blizzardEndTimeInCurrentSeason = -1f;
      if ((double) DataManager.Instance.blizzardTimeInCurrentSeason2 <= 0.0)
        DataManager.Instance.blizzardEndTimeInCurrentSeason2 = -1f;
      SeasonsManager.SetBlizzardTimes();
      if ((double) SeasonsManager.SEASON_NORMALISED_PROGRESS > (double) DataManager.Instance.blizzardTimeInCurrentSeason && (double) SeasonsManager.SEASON_NORMALISED_PROGRESS < (double) DataManager.Instance.blizzardEndTimeInCurrentSeason * 0.98000001907348633 || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS > (double) DataManager.Instance.blizzardTimeInCurrentSeason2 && (double) SeasonsManager.SEASON_NORMALISED_PROGRESS < (double) DataManager.Instance.blizzardEndTimeInCurrentSeason2 * 0.98000001907348633)
      {
        if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
          return;
        SeasonsManager.TriggerWeatherEvent(SeasonsManager.WeatherEvent.Blizzard, SeasonsManager.displayBlizzardName);
        SeasonsManager.displayBlizzardName = false;
        HUD_Winter.Instance.UpdateBarFeatures(true);
      }
      else
      {
        if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
          return;
        SeasonsManager.StopWeatherEvent();
      }
    }
    else
    {
      if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
        return;
      SeasonsManager.StopWeatherEvent();
    }
  }

  public static void Deinitialise()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(SeasonsManager.OnNewPhaseStarted);
    TimeManager.OnNewDayStarted -= new System.Action(SeasonsManager.OnNewDay);
    SeasonsManager.SetSeasonInstant = false;
    SeasonsManager.WinterEndingDisabled = false;
    SeasonsManager.blizzardRoutine = (Coroutine) null;
  }

  public static IEnumerator WaitForPlayerReady(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMTransition.IsPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static IEnumerator WaitForPlayerInBase(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    while (PlayerFarming.Location != FollowerLocation.Base && PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom && PlayerFarming.Location != FollowerLocation.Church && PlayerFarming.Location != FollowerLocation.DoorRoom)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static void SetSeason(SeasonsManager.Season season, System.Action callback = null)
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && season != SeasonsManager.Season.Winter && !DataManager.Instance.ShowCultWarmth)
      ++SeasonsManager.SeasonTimestamp;
    else
      GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.WaitForPlayerInBase((System.Action) (() =>
      {
        if (!DataManager.Instance.WinterLoopEnabled && season == SeasonsManager.Season.Winter)
          return;
        DataManager.Instance.AllowSaving = false;
        if (season != SeasonsManager.Season.Winter)
        {
          DataManager.Instance.blizzardTimeInCurrentSeason = -1f;
          DataManager.Instance.blizzardEndTimeInCurrentSeason = -1f;
          DataManager.Instance.blizzardTimeInCurrentSeason2 = -1f;
          DataManager.Instance.blizzardEndTimeInCurrentSeason2 = -1f;
          if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
            SeasonsManager.StopWeatherEvent();
        }
        SeasonsManager.PreviousSeason = SeasonsManager.CurrentSeason;
        SeasonsManager.WeatherEventTriggeredDay = TimeManager.CurrentDay;
        if (SeasonsManager.PreviousSeason == SeasonsManager.Season.Winter && season != SeasonsManager.Season.Winter)
          GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.WaitForPlayerReady((System.Action) (() =>
          {
            if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.WinterOver))
              return;
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.WinterOver);
          })));
        if (season == SeasonsManager.Season.Winter || DataManager.Instance.WinterModeActive)
        {
          AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.winter_starts);
          if (!DataManager.Instance.WinterMaxSeverity)
            DataManager.Instance.NextWinterServerity = DataManager.Instance.WinterServerity + 1;
          if (DataManager.Instance.WinterServerity >= 3)
            --SeasonsManager.WeatherEventTriggeredDay;
          ++DataManager.Instance.WintersOccured;
          SeasonsManager.SeasonTimestamp = TimeManager.CurrentDay + SeasonsManager.GetSeasonDuration(season) - 1;
          SeasonsManager.SeasonEvent onSeasonChanged = SeasonsManager.OnSeasonChanged;
          if (onSeasonChanged != null)
            onSeasonChanged(SeasonsManager.Season.Winter);
          SeasonsManager.CurrentSeason = season;
          DataManager.Instance.AllowSaving = true;
          SeasonsManager.LastFollowerToStartFreezing = 240f;
          SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.Season.Winter);
          AudioManager.Instance.PlayOneShot(SeasonsManager.WinterSeasonStartAnywhereSFX);
          if (DataManager.Instance.WintersOccured > 1)
            NotificationCentre.Instance.PlayGenericNotification($"Notifications/{SeasonsManager.CurrentSeason}/Started", NotificationBase.Flair.Winter);
          System.Action action = callback;
          if (action == null)
            return;
          action();
        }
        else
        {
          SeasonsManager.CurrentSeason = season;
          SeasonsManager.SeasonTimestamp = TimeManager.CurrentDay + SeasonsManager.GetSeasonDuration(season) - 1;
          if (DataManager.Instance.WintersOccured <= 0 && season != SeasonsManager.Season.Winter)
          {
            SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.Season.Winter);
            SeasonsManager.SeasonTimestamp = 0;
          }
          else
          {
            if (DataManager.Instance.WinterLoopEnabled)
            {
              NotificationCentre.Instance.PlayGenericNotification("Notifications/Winter/Ended", NotificationBase.Flair.Winter);
              AudioManager.Instance.PlayOneShot(SeasonsManager.WinterSeasonEndAnywhereSFX);
              if (PlayerFarming.Location == FollowerLocation.Base)
                AudioManager.Instance.PlayOneShot(SeasonsManager.WinterSeasonEndInBaseSFX);
            }
            SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
            SeasonsManager.SeasonEvent onSeasonChanged = SeasonsManager.OnSeasonChanged;
            if (onSeasonChanged != null)
              onSeasonChanged(SeasonsManager.CurrentSeason);
          }
          DataManager.Instance.AllowSaving = true;
        }
      })));
  }

  public static void SetSeasonDefaultWeather(SeasonsManager.Season season)
  {
    if (!SeasonsManager.Active)
      return;
    if (season == SeasonsManager.Season.Winter)
    {
      WeatherSystemController.Instance?.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Heavy);
      SeasonsManager.DeafultWeatherEvent defaultWeatherBegan = SeasonsManager.OnDefaultWeatherBegan;
      if (defaultWeatherBegan == null)
        return;
      defaultWeatherBegan(WeatherSystemController.WeatherType.Snowing);
    }
    else if (DataManager.Instance.OnboardedSeasons && DataManager.Instance.WintersOccured <= 0)
    {
      WeatherSystemController.Instance?.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Light, 0.0f);
      SeasonsManager.DeafultWeatherEvent defaultWeatherBegan = SeasonsManager.OnDefaultWeatherBegan;
      if (defaultWeatherBegan == null)
        return;
      defaultWeatherBegan(WeatherSystemController.WeatherType.Snowing);
    }
    else
    {
      if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      {
        if (BiomeGenerator.Instance.weatherType != WeatherSystemController.WeatherType.None)
        {
          WeatherSystemController.WeatherType? currentWeatherType = WeatherSystemController.Instance?.CurrentWeatherType;
          WeatherSystemController.WeatherType weatherType = BiomeGenerator.Instance.weatherType;
          if (currentWeatherType.GetValueOrDefault() == weatherType & currentWeatherType.HasValue)
            goto label_18;
        }
        SeasonsManager.DeafultWeatherEvent defaultWeatherEnded = SeasonsManager.OnDefaultWeatherEnded;
        if (defaultWeatherEnded != null)
          defaultWeatherEnded(WeatherSystemController.Instance.CurrentWeatherType);
      }
label_18:
      WeatherSystemController.Instance.StopCurrentWeather();
    }
  }

  public static void ActivateSeasons() => SeasonsManager.Active = true;

  public static void Update(float deltaTime)
  {
    if (!SeasonsManager.Active)
      return;
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      if (DataManager.Instance.Followers.Count != 0 && (double) WarmthBar.WarmthNormalized <= 0.0 && (!DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.ColdEnthusiast) || SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard) && (DataManager.Instance.WintersOccured > 1 || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.20000000298023224) && (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.10000000149011612 && SeasonsManager.WinterSeverity > 0 && (double) TimeManager.TotalElapsedGameTime > (double) SeasonsManager.LastFollowerToStartFreezing)
      {
        FollowerBrain freeze = FollowerBrain.RandomAvailableBrainToFreeze();
        if (freeze != null)
        {
          freeze.MakeFreezing();
          float num1 = UnityEngine.Random.Range(SeasonsManager.TIME_BETWEEN_FREEZING_FOLLOWER.x, SeasonsManager.TIME_BETWEEN_FREEZING_FOLLOWER.y);
          if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
            num1 /= 1.5f;
          float num2 = num1 / DifficultyManager.GetTimeBetweenFreezingMultiplier();
          if (freeze.HasTrait(FollowerTrait.TraitType.Mutated))
            num2 /= 2f;
          SeasonsManager.LastFollowerToStartFreezing = TimeManager.TotalElapsedGameTime + num2;
        }
      }
      if ((double) TimeManager.TotalElapsedGameTime > (double) SeasonsManager.TimeSinceLastSnowedUnderStructure && DataManager.Instance.OnboardedSnowedUnder && SeasonsManager.WinterSeverity >= 4)
      {
        List<StructureBrain> ts = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureBrain.AllBrains);
        List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.PROXIMITY_FURNACE, StructureBrain.TYPES.FURNACE_1, StructureBrain.TYPES.FURNACE_2, StructureBrain.TYPES.FURNACE_3);
        ts.Shuffle<StructureBrain>();
        float num3 = UnityEngine.Random.Range(SeasonsManager.TIME_BETWEEN_SNOWED_UNDER_STRUCTURE.x, SeasonsManager.TIME_BETWEEN_SNOWED_UNDER_STRUCTURE.y);
        if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
          num3 /= 1.75f;
        SeasonsManager.TimeSinceLastSnowedUnderStructure += TimeManager.TotalElapsedGameTime + num3;
        foreach (StructureBrain structureBrain1 in ts)
        {
          if (StructureManager.IsCollapsible(structureBrain1.Data.Type) && structureBrain1.Data.Bounds.x >= 2 && !structureBrain1.Data.IsCollapsed && !structureBrain1.Data.IsSnowedUnder)
          {
            bool flag = false;
            foreach (StructureBrain structureBrain2 in structuresOfTypes)
            {
              if (!((UnityEngine.Object) Interaction_DLCFurnace.Instance == (UnityEngine.Object) null) && Interaction_DLCFurnace.Instance.Lit)
              {
                int num4 = structureBrain2.Data.Type == StructureBrain.TYPES.PROXIMITY_FURNACE ? 15 : Interaction_DLCFurnace.GetRangeForType(structureBrain2.Data.Type);
                if ((double) Vector3.Distance(structureBrain2.Data.Position, structureBrain1.Data.Position) < (double) num4)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
            {
              structureBrain1.SnowedUnder();
              break;
            }
            break;
          }
        }
      }
    }
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      if (SeasonsManager.WinterSeverity >= 5 && SeasonsManager.lightningStrikeTimes.Count > 0 && SeasonsManager.lightningStrikesThisBlizzard < SeasonsManager.lightningStrikeTimes.Count && (double) TimeManager.TotalElapsedGameTime >= (double) SeasonsManager.lightningStrikeTimes[SeasonsManager.lightningStrikesThisBlizzard])
        SeasonsManager.TriggerLightningStrike();
      if ((double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastSnowPileSpawned && (UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
      {
        StructureManager.PlaceIceBlock(FollowerLocation.Base, new List<Structures_PlacementRegion>()
        {
          PlacementRegion.Instance.structureBrain
        });
        DataManager.Instance.TimeSinceLastSnowPileSpawned = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(600f, 1200f);
      }
    }
    if (TimeManager.CurrentDay > SeasonsManager.SeasonTimestamp && DataManager.Instance.WintersOccured <= 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring)
      SeasonsManager.SetSeason(SeasonsManager.Season.Winter);
    SeasonsManager.UpdateWeatherEvent();
  }

  public static void OnNewDay() => SeasonsManager.SetLongNight();

  public static void SetLongNight()
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && SeasonsManager.WinterSeverity >= 3)
    {
      if (SeasonsManager.SEASON_CURRENT_DAY == 4)
      {
        if (!DataManager.Instance.LongNightActive)
        {
          if (!DataManager.Instance.OnboardedLongNights && !DataManager.Instance.LongNightActive)
          {
            GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.WaitForPlayerReady((System.Action) (() => HUD_Time.Instance.LongNightSequence())));
          }
          else
          {
            DataManager.Instance.LongNightActive = true;
            HUD_Time.Instance.Init();
          }
        }
      }
      else if (DataManager.Instance.LongNightActive)
      {
        DataManager.Instance.LongNightActive = false;
        if ((UnityEngine.Object) HUD_Time.Instance != (UnityEngine.Object) null)
          HUD_Time.Instance.Init();
      }
    }
    if (!((UnityEngine.Object) HUD_Winter.Instance != (UnityEngine.Object) null))
      return;
    HUD_Winter.Instance.UpdateBarFeatures(true);
  }

  public static void LocationChanged(FollowerLocation location) => SeasonsManager.CheckSeasonOver();

  public static void CheckSeasonOver()
  {
    if (!SeasonsManager.Active || DataManager.Instance.WinterModeActive || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && SeasonsManager.WinterEndingDisabled || PlayerFarming.Location != FollowerLocation.Base || DataManager.Instance.WintersOccured <= 0)
      return;
    GameManager.GetInstance()?.WaitForSeconds(1f, (System.Action) (() =>
    {
      SeasonsManager.Season season = (SeasonsManager.Season) Mathf.Repeat((float) (SeasonsManager.CurrentSeason + 1), (float) (Enum.GetNames(typeof (SeasonsManager.Season)).Length - 1));
      if (TimeManager.CurrentDay <= SeasonsManager.SeasonTimestamp)
        return;
      if (!DataManager.Instance.WinterLoopEnabled)
        season = SeasonsManager.Season.Winter;
      SeasonsManager.SetSeason(season);
    }));
  }

  public static void OnNewPhaseStarted()
  {
    SeasonsManager.CheckSeasonOver();
    SeasonsManager.SetLongNight();
    if (TimeManager.CurrentPhase == DayPhase.Night)
      WarmthBar.ModifyWarmth("Notifications/NightBegan", -10f);
    if (TimeManager.CurrentDay - SeasonsManager.SeasonSpecialEventTriggeredDay < 1 && SeasonsManager.SeasonSpecialEventTriggeredDay != -1)
      return;
    SeasonsManager.TriggerSpecialEvent(SeasonsManager.CurrentSeason);
  }

  public static int GetSeasonDuration(SeasonsManager.Season season)
  {
    if (season == SeasonsManager.Season.Spring)
      return SeasonsManager.SPRING_DURATION;
    return season == SeasonsManager.Season.Winter ? SeasonsManager.WINTER_DURATION : 0;
  }

  public static SeasonsManager.WeatherEvent GetWeatherEventForSeason(SeasonsManager.Season season)
  {
    if (season != SeasonsManager.Season.Winter || !DataManager.Instance.OnboardedBlizzards)
      return SeasonsManager.WeatherEvent.None;
    if (SeasonsManager.NextPhaseIsWeatherEvent)
      return SeasonsManager.WeatherEvent.Blizzard;
    float num = SeasonsManager.WEATHER_EVENT_CHANCES[(int) SeasonsManager.CurrentSeason];
    if (DataManager.Instance.WintersOccured >= 3)
      num += SeasonsManager.WEATHER_EVENT_CHANCES[(int) SeasonsManager.CurrentSeason];
    return (double) UnityEngine.Random.value < (double) num ? SeasonsManager.WeatherEvent.Blizzard : SeasonsManager.WeatherEvent.None;
  }

  public static void UpdateWeatherEvent() => SeasonsManager.SimpleBlizzardCheck();

  public static void TriggerWeatherEvent(SeasonsManager.WeatherEvent weatherEvent, bool displayName = true)
  {
    bool initialTrigger = SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.None && SeasonsManager.WeatherEventTriggeredDay <= TimeManager.CurrentDay;
    if (weatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      SeasonsManager.TriggerBlizzard(initialTrigger);
    SeasonsManager.CurrentWeatherEvent = weatherEvent;
    if (initialTrigger)
    {
      ++SeasonsManager.WeatherEventID;
      SeasonsManager.WeatherEventDurationTime = SeasonsManager.WEATHER_EVENT_DURATION;
      SeasonsManager.WeatherEventOverTargetTime = TimeManager.TotalElapsedGameTime + SeasonsManager.WeatherEventDurationTime;
      SeasonsManager.WeatherEventTriggeredDay = TimeManager.CurrentDay;
      if (displayName)
        HUD_DisplayName.Play($"NAMES/Season/{SeasonsManager.CurrentWeatherEvent}", 3, HUD_DisplayName.Positions.Centre);
      SeasonsManager.WeatherTypeEvent onWeatherBegan = SeasonsManager.OnWeatherBegan;
      if (onWeatherBegan != null)
        onWeatherBegan(SeasonsManager.CurrentWeatherEvent);
      if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      {
        SeasonsManager.WeatherTypeEvent onBlizzardBegan = SeasonsManager.OnBlizzardBegan;
        if (onBlizzardBegan != null)
          onBlizzardBegan(SeasonsManager.WeatherEvent.Blizzard);
      }
    }
    SeasonsManager.NextPhaseIsWeatherEvent = false;
  }

  public static void TriggerSpecialEvent(SeasonsManager.Season season)
  {
    if (season != SeasonsManager.Season.Winter || (double) UnityEngine.Random.value >= (double) SeasonsManager.WINTER_SPECIAL_EVENT_CHANCES[0] || DataManager.Instance.WintersOccured < 4)
      return;
    SeasonsManager.SeasonSpecialEventTriggeredDay = TimeManager.CurrentDay;
  }

  public static void TriggerBlizzard(bool initialTrigger)
  {
    Debug.Log((object) "TRIGGER BLIZZARD!".Colour(Color.yellow));
    if (SeasonsManager.lightningStrikeTimes.Count == 0)
    {
      SeasonsManager.lightningStrikesThisBlizzard = 0;
      SeasonsManager.maxLightningStrikesThisBlizzard = UnityEngine.Random.Range(2, 5);
      SeasonsManager.lightningStrikeTimes.Clear();
      float num1 = (float) (SeasonsManager.GetSeasonDuration(SeasonsManager.CurrentSeason) * 1200);
      if (DataManager.Instance.OnboardedLongNights)
        num1 += 240f;
      float num2 = (float) ((SeasonsManager.SeasonTimestamp + 1) * 1200) - num1;
      float timeInCurrentSeason1 = DataManager.Instance.blizzardTimeInCurrentSeason;
      float timeInCurrentSeason2 = DataManager.Instance.blizzardEndTimeInCurrentSeason;
      float inCurrentSeason2_1 = DataManager.Instance.blizzardTimeInCurrentSeason2;
      float inCurrentSeason2_2 = DataManager.Instance.blizzardEndTimeInCurrentSeason2;
      float normalisedProgress = SeasonsManager.SEASON_NORMALISED_PROGRESS;
      int num3 = (double) normalisedProgress <= (double) timeInCurrentSeason1 ? 0 : ((double) normalisedProgress < (double) timeInCurrentSeason2 ? 1 : 0);
      bool flag = (double) normalisedProgress > (double) inCurrentSeason2_1 && (double) normalisedProgress < (double) inCurrentSeason2_2;
      if (num3 == 0 && !flag)
      {
        float num4 = (float) (0.5 * ((double) timeInCurrentSeason1 + (double) timeInCurrentSeason2));
        float num5 = (float) (0.5 * ((double) inCurrentSeason2_1 + (double) inCurrentSeason2_2));
        flag = (double) Mathf.Abs(normalisedProgress - num5) < (double) Mathf.Abs(normalisedProgress - num4);
      }
      float a;
      float b;
      if (flag)
      {
        a = num2 + inCurrentSeason2_1 * num1;
        b = num2 + inCurrentSeason2_2 * num1;
      }
      else
      {
        a = num2 + timeInCurrentSeason1 * num1;
        b = num2 + timeInCurrentSeason2 * num1;
      }
      if ((double) b <= (double) a)
        b = a + Mathf.Max(120f, 1f);
      for (int index = 0; index < SeasonsManager.maxLightningStrikesThisBlizzard; ++index)
      {
        float num6 = Mathf.Lerp(a, b, (float) index / (float) SeasonsManager.maxLightningStrikesThisBlizzard);
        float num7 = Mathf.Lerp(a, b, (float) (index + 1) / (float) SeasonsManager.maxLightningStrikesThisBlizzard);
        float num8 = UnityEngine.Random.Range(num6 + (float) (((double) num7 - (double) num6) * 0.20000000298023224), num7 - (float) (((double) num7 - (double) num6) * 0.20000000298023224));
        SeasonsManager.lightningStrikeTimes.Add(num8);
      }
      SeasonsManager.lightningStrikeTimes.Sort();
      while (SeasonsManager.lightningStrikesThisBlizzard < SeasonsManager.lightningStrikeTimes.Count && (double) TimeManager.TotalElapsedGameTime >= (double) SeasonsManager.lightningStrikeTimes[SeasonsManager.lightningStrikesThisBlizzard])
        ++SeasonsManager.lightningStrikesThisBlizzard;
    }
    if (initialTrigger)
    {
      SeasonsManager.LastFollowerToStartFreezing = 80f;
      SeasonsManager.TimeSinceLastSnowedUnderStructure = 0.0f;
      DataManager.Instance.BlizzardEventID = DataManager.Instance.WintersOccured;
      DataManager.Instance.CompletedOfferingThisBlizzard = false;
      NotificationCentre.Instance.PlayGenericNotification("Notifications/Blizzard/Started", NotificationBase.Flair.Winter);
      AudioManager.Instance.PlayOneShot("event:/dlc/atmos/blizzard_start", PlayerFarming.Instance.transform.position);
    }
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      Thought[] thoughtArray = new Thought[2]
      {
        Thought.Blizzard_1,
        Thought.Blizzard_2
      };
      allBrain.AddRandomThoughtFromList(thoughtArray);
    }
    if (!DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.WorkThroughBlizzard))
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if ((!FollowerManager.FollowerLocked(allBrain.Info.ID) || allBrain.Info.CursedState == Thought.Child && allBrain.Info.ID != 100000) && allBrain.HasHome && allBrain.Location == FollowerLocation.Base && allBrain.CurrentTaskType != FollowerTaskType.ChangeLocation && !allBrain.CanWork)
          allBrain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep(forced: true));
      }
    }
    if (!LocationManager.IndoorLocations.Contains(PlayerFarming.Location))
    {
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme);
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.playerController.SetSpecialMovingAnimations("blizzard/idle", "blizzard/run-up", "blizzard/run-down", "blizzard/run", "blizzard/run-up-diagonal", "blizzard/run-horizontal", StateMachine.State.Idle_Winter);
        if (player.state.CURRENT_STATE == StateMachine.State.Idle)
          player.state.CURRENT_STATE = StateMachine.State.Idle_Winter;
        else if (player.state.CURRENT_STATE == StateMachine.State.Moving && !player.GoToAndStopping)
          player.state.CURRENT_STATE = StateMachine.State.Moving_Winter;
      }
    }
    else
    {
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme, 0.0f);
      foreach (PlayerFarming player in PlayerFarming.players)
        player.playerController.SetSpecialMovingAnimations("blizzard/idle", "blizzard/run-up", "blizzard/run-down", "blizzard/run", "blizzard/run-up-diagonal", "blizzard/run-horizontal", StateMachine.State.Idle_Winter);
    }
  }

  public static void TriggerLightningStrike()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    FollowerBrain followerBrain = (FollowerBrain) null;
    StructureBrain structure1 = (StructureBrain) null;
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    int num1 = (double) UnityEngine.Random.value < 0.60000002384185791 ? 1 : 0;
    bool flag1 = SeasonsManager.lightningStrikesThisBlizzard < SeasonsManager.maxLightningStrikesThisBlizzard;
    int num2 = flag1 ? 1 : 0;
    if ((num1 & num2) != 0)
    {
      List<FollowerBrain> followerBrainList1 = new List<FollowerBrain>();
      List<FollowerBrain> followerBrainList2 = new List<FollowerBrain>();
      foreach (FollowerInfo follower in DataManager.Instance.Followers)
      {
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
        bool flag2;
        bool flag3;
        bool flag4;
        if (brain != null)
        {
          ref int local1 = ref follower.ID;
          // ISSUE: explicit reference operation
          ref bool local2 = @false;
          flag2 = false;
          ref bool local3 = ref flag2;
          flag3 = false;
          ref bool local4 = ref flag3;
          flag4 = true;
          ref bool local5 = ref flag4;
          // ISSUE: explicit reference operation
          ref bool local6 = @false;
          if (!FollowerManager.FollowerLocked(in local1, in local2, in local3, in local4, in local5, in local6))
            followerBrainList1.Add(brain);
        }
        if (brain != null)
        {
          ref int local7 = ref follower.ID;
          // ISSUE: explicit reference operation
          ref bool local8 = @false;
          flag2 = false;
          ref bool local9 = ref flag2;
          flag3 = false;
          ref bool local10 = ref flag3;
          flag4 = true;
          ref bool local11 = ref flag4;
          // ISSUE: explicit reference operation
          ref bool local12 = @false;
          if (!FollowerManager.FollowerLocked(in local7, in local8, in local9, in local10, in local11, in local12) && brain.HasTrait(FollowerTrait.TraitType.LightningEnthusiast))
            followerBrainList2.Add(brain);
        }
      }
      FollowerBrain.GetAvailableBrainsWithNecklaceTargeted(ref SeasonsManager.targetedFollowers, false);
      followerBrain = SeasonsManager.targetedFollowers.Count <= 0 ? (followerBrainList2.Count <= 0 || (double) UnityEngine.Random.value < 0.5 ? (followerBrainList1.Count > 0 ? followerBrainList1[UnityEngine.Random.Range(0, followerBrainList1.Count)] : (FollowerBrain) null) : followerBrainList2[UnityEngine.Random.Range(0, followerBrainList2.Count)]) : SeasonsManager.targetedFollowers[UnityEngine.Random.Range(0, SeasonsManager.targetedFollowers.Count)];
      SeasonsManager.targetedFollowers.Clear();
    }
    else if (flag1)
      structure1 = StructureManager.GetPossibleLighningStrikedStructure();
    if (followerBrain != null && (double) UnityEngine.Random.value < 0.87999999523162842)
    {
      ++SeasonsManager.lightningStrikesThisBlizzard;
      StructureBrain structure2 = (StructureBrain) followerBrain.IsWithinLightningRodRange();
      if (structure2 != null)
      {
        WeatherSystemController.Instance.TriggerLightningStrike(structure2);
      }
      else
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
          return;
        if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
          HUD_Manager.Instance.SetLightningTarget(new SeasonsManager.LightningTarget(followerById));
        followerById.LightningStrikeIncoming();
      }
    }
    else if (structure1 != null && (double) UnityEngine.Random.value < 0.800000011920929)
    {
      ++SeasonsManager.lightningStrikesThisBlizzard;
      Structure structure3 = (Structure) null;
      foreach (Structure structure4 in Structure.Structures)
      {
        if ((UnityEngine.Object) structure4 != (UnityEngine.Object) null && structure4.Brain == structure1)
        {
          structure3 = structure4;
          break;
        }
      }
      if (!((UnityEngine.Object) structure3 != (UnityEngine.Object) null))
        return;
      if (structure3.Brain.Data.Type == StructureBrain.TYPES.LIGHTNING_ROD)
      {
        WeatherSystemController.Instance.TriggerLightningStrike(structure1);
      }
      else
      {
        if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
          HUD_Manager.Instance.SetLightningTarget(new SeasonsManager.LightningTarget(structure1.Data.Position));
        Interaction component = structure3.GetComponent<Interaction>();
        structure3.gameObject.AddComponent<Interaction_LightningStructure>().Configure(structure3, component);
      }
    }
    else
    {
      if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
        return;
      ++SeasonsManager.lightningStrikesThisBlizzard;
      Structures_LightningRod structure5 = StructureManager.IsWithinLightningRod(PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 6f);
      if (structure5 != null)
        WeatherSystemController.Instance.TriggerLightningStrike((StructureBrain) structure5);
      else
        WeatherSystemController.Instance.TriggerLightningStrike();
    }
  }

  public static void StopWeatherEvent()
  {
    DataManager.Instance.GivenBlizzardObjective = false;
    SeasonsManager.WeatherEventDurationTime = -1f;
    SeasonsManager.WeatherEvent currentWeatherEvent = SeasonsManager.CurrentWeatherEvent;
    SeasonsManager.CurrentWeatherEvent = SeasonsManager.WeatherEvent.None;
    if (!GameManager.IsDungeon(PlayerFarming.Location) && !SeasonsManager.WinterEndingDisabled)
      SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
    if (currentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      SeasonsManager.lightningStrikeTimes.Clear();
      SeasonsManager.lightningStrikesThisBlizzard = 0;
      SeasonsManager.maxLightningStrikesThisBlizzard = 0;
      AudioManager.Instance.PlayOneShot("event:/dlc/atmos/blizzard_stop");
      DataManager.Instance.CompletedOfferingThisBlizzard = false;
      SeasonsManager.WeatherTypeEvent onBlizzardEnded = SeasonsManager.OnBlizzardEnded;
      if (onBlizzardEnded != null)
        onBlizzardEnded(currentWeatherEvent);
    }
    HUD_Winter.Instance.UpdateBarFeatures(true);
    SeasonsManager.WeatherTypeEvent onWeatherEnded = SeasonsManager.OnWeatherEnded;
    if (onWeatherEnded == null)
      return;
    onWeatherEnded(currentWeatherEvent);
  }

  public static string GetSeasonLocalisedTitle(SeasonsManager.Season season, bool localised = true)
  {
    return !localised ? $"NAMES/Season/{season}" : LocalizationManager.GetTranslation($"NAMES/Season/{season}");
  }

  public static string GetWeatherLocalisedTitle(SeasonsManager.WeatherEvent weather, bool localised = true)
  {
    return !localised ? $"NAMES/Weather/{weather}" : LocalizationManager.GetTranslation($"NAMES/Weather/{weather}");
  }

  public static string GetWeatherLocalisedDescription(
    SeasonsManager.WeatherEvent weather,
    bool localised = true)
  {
    return !localised ? $"NAMES/Weather/{weather}/Description" : LocalizationManager.GetTranslation($"NAMES/Weather/{weather}/Description");
  }

  public static IEnumerator FirstWinterIE(SeasonsManager.Season season, System.Action callback)
  {
    SeasonsManager.SeasonTimestamp = TimeManager.CurrentDay + SeasonsManager.GetSeasonDuration(SeasonsManager.Season.Winter) - 1;
    TimeManager.PauseGameTime = false;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeIn());
    Vector3 fromPos = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.SetGlobalOcclusionActive(false);
    BiomeBaseManager.Instance.ActivateRoom();
    Vector3 position = TownCentre.Instance.Centre.position + Vector3.down * 2f;
    List<Follower> availableFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true, excludeFreezing: true) && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockTaskChanges))
      {
        availableFollowers.Add(follower);
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        follower.HideAllFollowerIcons();
        Vector3 normalized = (position - follower.transform.position).normalized;
        if ((double) Vector3.Distance(position, follower.transform.position) < 3.0)
          follower.transform.position += (double) normalized.x < 0.0 ? Vector3.left * 3f : Vector3.right * 3f;
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, (double) UnityEngine.Random.value < 0.5 ? "Snow/look-up-happy" : "Snow/look-up-wonder");
        follower.State.CURRENT_STATE = StateMachine.State.Idle;
        if (FollowerManager.BishopIDs.Contains(follower.Brain.Info.ID))
          follower.Brain.AddThought(Thought.Bishop_FirstWinter);
      }
    }
    AudioManager.Instance.PlayOneShot(SeasonsManager.WinterFollowersReactSFX);
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    yield return (object) new WaitForEndOfFrame();
    availableFollowers.Shuffle<Follower>();
    SeasonsManager.SeasonEvent onSeasonChanged = SeasonsManager.OnSeasonChanged;
    if (onSeasonChanged != null)
      onSeasonChanged(SeasonsManager.Season.Winter);
    if (availableFollowers.Count > 0)
    {
      Follower follower = availableFollowers[UnityEngine.Random.Range(0, availableFollowers.Count)];
      follower.transform.position = position;
      availableFollowers.Remove(follower);
      follower.Spine.AnimationState.SetAnimation(1, (double) UnityEngine.Random.value < 0.5 ? "Snow/look-up-happy" : "Snow/look-up-wonder", false);
      follower.Spine.AnimationState.AddAnimation(1, "Snow/idle", true, 0.0f);
    }
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Dusting, 30f);
    ParticleSystem particleSystem = WeatherSystemController.Instance.GetParticleSystem(WeatherSystemController.WeatherType.Snowing);
    if ((UnityEngine.Object) particleSystem != (UnityEngine.Object) null)
      particleSystem.main.useUnscaledTime = true;
    float t = Shader.GetGlobalFloat("_Snow_Intensity");
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1.75f, 30f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => Shader.SetGlobalFloat("_Snow_Intensity", t)));
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.enabled = false;
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(-1f, -16f, -3.5f));
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.right * 2f, 10f);
    yield return (object) new WaitForSeconds(4f);
    if (availableFollowers.Count >= 2)
    {
      availableFollowers[0].transform.position = new Vector3(-13f, -17f, 0.0f);
      availableFollowers[1].transform.position = new Vector3(-15f, -17f, 0.0f);
      availableFollowers[0].FacePosition(availableFollowers[1].transform.position);
      availableFollowers[1].FacePosition(availableFollowers[0].transform.position);
      availableFollowers[1].Brain.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight(availableFollowers[0], true));
      availableFollowers.Remove(availableFollowers[1]);
      availableFollowers.Remove(availableFollowers[0]);
    }
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(-13f, -19f, -3f));
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.left * 2f, 10f);
    yield return (object) new WaitForSeconds(5.5f);
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(position.x + 3f, position.y, -3f));
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.right * 2f, 10f);
    if (availableFollowers.Count > 0)
    {
      availableFollowers[0].transform.position = new Vector3(position.x + 4f, position.y + 2f, 0.0f);
      availableFollowers[0].Spine.AnimationState.SetAnimation(1, "Snow/snow-angel", true);
      if (availableFollowers.Count > 1)
      {
        availableFollowers[1].transform.position = new Vector3(position.x + 3f, position.y + 3f, 0.0f);
        availableFollowers[1].FacePosition(availableFollowers[0].transform.position);
        availableFollowers[1].Spine.AnimationState.SetAnimation(1, "Snow/idle-tongueout", true);
        if (availableFollowers.Count > 2)
        {
          availableFollowers[2].transform.position = new Vector3(position.x + 4.7f, position.y + 2.75f, 0.0f);
          availableFollowers[2].FacePosition(availableFollowers[0].transform.position);
          availableFollowers[2].Spine.AnimationState.SetAnimation(1, "Snow/idle-tongueout2", true);
        }
      }
    }
    yield return (object) new WaitForSeconds(6f);
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(0.0f, -18f, -6.5f));
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(new Vector3(0.0f, -20f, -11f), 10f);
    SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.Season.Winter);
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    ++DataManager.Instance.WintersOccured;
    if (!DataManager.Instance.DiscoveredLocations.Contains(FollowerLocation.LambTown))
    {
      DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.LambTown);
      DataManager.Instance.VisitedLocations.Add(FollowerLocation.LambTown);
    }
    if (!DataManager.Instance.OnboardedRanchingWolves && Interaction_Ranchable.Ranchables.Count > 0 && DataManager.Instance.RequiresWolvesOnboarded)
      DataManager.Instance.RequiresWolvesOnboarded = false;
    DataManager.Instance.AllowSaving = true;
    AudioManager.Instance.PlayOneShot(SeasonsManager.WinterSeasonTitleInSFX);
    HUD_DisplayName.Play(SeasonsManager.GetSeasonLocalisedTitle(SeasonsManager.Season.Winter, false), Position: HUD_DisplayName.Positions.Centre, blend: HUD_DisplayName.textBlendMode.Winter);
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        string animationName = (double) UnityEngine.Random.value < 0.5 ? "Snow/look-up-happy" : "Snow/look-up-wonder";
        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.Spine.AnimationState.SetAnimation(1, animationName, false);
        follower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 2f);
        follower.Spine.AnimationState.AddAnimation(1, "Snow/idle", true, 0.0f);
      }
    }
    yield return (object) new WaitForSeconds(5f);
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeIn());
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        follower.Brain.CompleteCurrentTask();
        follower.SimpleAnimator.ResetAnimationsToDefaults();
      }
      follower.ShowAllFollowerIcons();
    }
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(fromPos);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    GameManager.SetGlobalOcclusionActive(true);
    BiomeBaseManager.Instance.ActivateDLCShrineRoom();
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeOut());
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public static IEnumerator SecondWinterIE()
  {
    bool isAlreadyWinter = SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter;
    if (!isAlreadyWinter)
      SeasonsManager.SeasonTimestamp = TimeManager.CurrentDay + SeasonsManager.GetSeasonDuration(SeasonsManager.Season.Winter) - 1;
    TimeManager.PauseGameTime = false;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeIn());
    Vector3 fromPos = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.SetGlobalOcclusionActive(false);
    BiomeBaseManager.Instance.ActivateRoom();
    Vector3 position = TownCentre.Instance.Centre.position + Vector3.down * 2f;
    List<Follower> avaiableFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true, excludeFreezing: true))
      {
        avaiableFollowers.Add(follower);
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        follower.HideAllFollowerIcons();
        Vector3 normalized = (position - follower.transform.position).normalized;
        if ((double) Vector3.Distance(position, follower.transform.position) < 3.0)
          follower.transform.position += (double) normalized.x < 0.0 ? Vector3.left * 3f : Vector3.right * 3f;
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/look-up-worried");
        follower.State.CURRENT_STATE = StateMachine.State.Idle;
      }
    }
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    yield return (object) new WaitForEndOfFrame();
    avaiableFollowers.Shuffle<Follower>();
    SeasonsManager.SeasonEvent onSeasonChanged = SeasonsManager.OnSeasonChanged;
    if (onSeasonChanged != null)
      onSeasonChanged(SeasonsManager.Season.Winter);
    if (avaiableFollowers.Count > 0)
    {
      Follower follower = avaiableFollowers[UnityEngine.Random.Range(0, avaiableFollowers.Count)];
      follower.transform.position = position;
      avaiableFollowers.Remove(follower);
      follower.Spine.AnimationState.SetAnimation(1, "Snow/look-up-worried", false);
      follower.Spine.AnimationState.AddAnimation(1, "Snow/idle-sad", true, 0.0f);
    }
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.enabled = false;
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(1f, -20f, -7.5f));
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.left * 2f, 10f);
    float num1 = UnityEngine.Random.Range(0.1f, 0.15f);
    float normalisedProgress = SeasonsManager.SEASON_NORMALISED_PROGRESS;
    float num2 = SeasonsManager.SEASON_NORMALISED_PROGRESS + num1;
    DataManager.Instance.blizzardTimeInCurrentSeason = normalisedProgress;
    DataManager.Instance.blizzardEndTimeInCurrentSeason = num2;
    DataManager.Instance.blizzardTimeInCurrentSeason2 = normalisedProgress;
    DataManager.Instance.blizzardEndTimeInCurrentSeason2 = num2;
    SeasonsManager.TriggerWeatherEvent(SeasonsManager.WeatherEvent.Blizzard, false);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(2f);
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    if (!isAlreadyWinter)
      ++DataManager.Instance.WintersOccured;
    if (!DataManager.Instance.OnboardedRanchingWolves && Interaction_Ranchable.Ranchables.Count > 0 && DataManager.Instance.RequiresWolvesOnboarded)
      DataManager.Instance.RequiresWolvesOnboarded = false;
    DataManager.Instance.AllowSaving = true;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/winter/level_02_firsttime_text");
    HUD_DisplayName.Play(SeasonsManager.GetSeasonLocalisedTitle(SeasonsManager.Season.Winter, false), Position: HUD_DisplayName.Positions.Centre, blend: HUD_DisplayName.textBlendMode.Winter, winterSeverity: SeasonsManager.WinterSeverity);
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        string animationName = "Snow/look-up-worried";
        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.Spine.AnimationState.SetAnimation(1, animationName, false);
        follower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 2f);
        follower.Spine.AnimationState.AddAnimation(1, "Snow/idle-sad", true, 0.0f);
      }
    }
    yield return (object) new WaitForSeconds(5f);
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeIn());
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        follower.Brain.CompleteCurrentTask();
        follower.SimpleAnimator.ResetAnimationsToDefaults();
      }
      follower.ShowAllFollowerIcons();
    }
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(fromPos);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    GameManager.SetGlobalOcclusionActive(true);
    BiomeBaseManager.Instance.ActivateDLCShrineRoom();
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeOut());
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public static IEnumerator FourthWinterIE()
  {
    bool isAlreadyWinter = SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter;
    if (!isAlreadyWinter)
      SeasonsManager.SeasonTimestamp = TimeManager.CurrentDay + SeasonsManager.GetSeasonDuration(SeasonsManager.Season.Winter) - 1;
    TimeManager.PauseGameTime = false;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeIn());
    Vector3 fromPos = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.SetGlobalOcclusionActive(false);
    BiomeBaseManager.Instance.ActivateRoom();
    Vector3 position = TownCentre.Instance.Centre.position + Vector3.down * 2f;
    List<Follower> avaiableFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true, excludeFreezing: true))
      {
        avaiableFollowers.Add(follower);
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        follower.HideAllFollowerIcons();
        Vector3 normalized = (position - follower.transform.position).normalized;
        if ((double) Vector3.Distance(position, follower.transform.position) < 3.0)
          follower.transform.position += (double) normalized.x < 0.0 ? Vector3.left * 3f : Vector3.right * 3f;
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, (double) UnityEngine.Random.value < 0.5 ? "Snow/idle-sad" : "Snow/idle-smile");
        follower.State.CURRENT_STATE = StateMachine.State.Idle;
      }
    }
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    yield return (object) new WaitForEndOfFrame();
    avaiableFollowers.Shuffle<Follower>();
    SeasonsManager.SeasonEvent onSeasonChanged = SeasonsManager.OnSeasonChanged;
    if (onSeasonChanged != null)
      onSeasonChanged(SeasonsManager.Season.Winter);
    if (avaiableFollowers.Count > 0)
    {
      Follower follower = avaiableFollowers[UnityEngine.Random.Range(0, avaiableFollowers.Count)];
      follower.transform.position = position;
      avaiableFollowers.Remove(follower);
      follower.Spine.AnimationState.SetAnimation(1, "Snow/look-up-worried", false);
      follower.Spine.AnimationState.AddAnimation(1, "Snow/idle-sad", true, 0.0f);
    }
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme, 30f);
    ParticleSystem particleSystem = WeatherSystemController.Instance.GetParticleSystem(WeatherSystemController.WeatherType.Snowing);
    if ((UnityEngine.Object) particleSystem != (UnityEngine.Object) null)
      particleSystem.main.useUnscaledTime = true;
    float t = Shader.GetGlobalFloat("_Snow_Intensity");
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1.75f, 30f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => Shader.SetGlobalFloat("_Snow_Intensity", t)));
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.enabled = false;
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(1f, -16f, -3.5f));
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.left * 2f, 10f);
    yield return (object) new WaitForSeconds(4f);
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.OnboardSnowedUnderIE());
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(0.0f, -18f, -6.5f));
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(new Vector3(0.0f, -20f, -11f), 10f);
    SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.Season.Winter);
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    if (!isAlreadyWinter)
      ++DataManager.Instance.WintersOccured;
    DataManager.Instance.AllowSaving = true;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/winter/level_02_firsttime_text");
    HUD_DisplayName.Play(SeasonsManager.GetSeasonLocalisedTitle(SeasonsManager.Season.Winter, false), Position: HUD_DisplayName.Positions.Centre, blend: HUD_DisplayName.textBlendMode.Winter, winterSeverity: SeasonsManager.WinterSeverity);
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        string animationName = (double) UnityEngine.Random.value < 0.5 ? "Snow/look-up-happy" : "Snow/look-up-wonder";
        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.Spine.AnimationState.SetAnimation(1, animationName, false);
        follower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 2f);
        follower.Spine.AnimationState.AddAnimation(1, "Snow/idle", true, 0.0f);
      }
    }
    yield return (object) new WaitForSeconds(5f);
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeIn());
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        follower.Brain.CompleteCurrentTask();
        follower.SimpleAnimator.ResetAnimationsToDefaults();
      }
      follower.ShowAllFollowerIcons();
    }
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(fromPos);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    GameManager.SetGlobalOcclusionActive(true);
    BiomeBaseManager.Instance.ActivateDLCShrineRoom();
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FadeOut());
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public static IEnumerator OnboardSnowedUnderIE()
  {
    DataManager.Instance.OnboardedSnowedUnder = true;
    DataManager.Instance.RequiresSnowedUnderOnboarded = false;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    List<Structure> ts = new List<Structure>((IEnumerable<Structure>) Structure.Structures);
    ts.Shuffle<Structure>();
    SeasonsManager.TimeSinceLastSnowedUnderStructure = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(SeasonsManager.TIME_BETWEEN_SNOWED_UNDER_STRUCTURE.x, SeasonsManager.TIME_BETWEEN_SNOWED_UNDER_STRUCTURE.y);
    Structure targetStructure = (Structure) null;
    foreach (Structure structure in ts)
    {
      if (structure.Brain != null && StructureManager.IsCollapsible(structure.Brain.Data.Type) && structure.Brain.Data.Bounds.x >= 2 && !structure.Brain.Data.IsCollapsed && !structure.Brain.Data.IsSnowedUnder)
      {
        targetStructure = structure;
        break;
      }
    }
    if ((UnityEngine.Object) targetStructure != (UnityEngine.Object) null)
    {
      GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
      GameManager.GetInstance().CamFollowTarget.transform.DOKill();
      GameManager.GetInstance().CamFollowTarget.SnapTo(targetStructure.transform.position + Vector3.left - GameManager.GetInstance().CamFollowTarget.transform.forward * 2f);
      GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.right * 2f, 10f);
      yield return (object) new WaitForSeconds(1f);
      targetStructure.Brain.SnowedUnder();
      yield return (object) new WaitForSeconds(3f);
    }
  }

  public static IEnumerator OnboardBlizzardIE(List<Follower> availableFollowers)
  {
    DataManager.Instance.OnboardedBlizzards = true;
    DataManager.Instance.RequiresBlizzardOnboarded = false;
    SeasonsManager.WeatherEventTriggeredDay = -1;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    availableFollowers.Shuffle<Follower>();
    Structure structure = (Structure) null;
    Follower follower = (Follower) null;
    foreach (Follower availableFollower in availableFollowers)
    {
      if ((UnityEngine.Object) availableFollower != (UnityEngine.Object) null && availableFollower.Brain.Info.CursedState == Thought.None)
      {
        Structures_Bed dwellingStructure = availableFollower.Brain.GetAssignedDwellingStructure();
        if (dwellingStructure != null && !dwellingStructure.Data.Destroyed && !dwellingStructure.Data.IsSnowedUnder)
        {
          structure = Dwelling.GetDwellingByID(dwellingStructure.Data.ID).Structure;
          follower = availableFollower;
          if ((UnityEngine.Object) structure != (UnityEngine.Object) null)
            break;
        }
      }
    }
    if ((UnityEngine.Object) structure != (UnityEngine.Object) null)
    {
      GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
      GameManager.GetInstance().CamFollowTarget.transform.DOKill();
      GameManager.GetInstance().CamFollowTarget.SnapTo(structure.transform.position + Vector3.left - GameManager.GetInstance().CamFollowTarget.transform.forward * 2f);
      GameManager.GetInstance().CamFollowTarget.transform.DOMove(GameManager.GetInstance().CamFollowTarget.transform.position + Vector3.right * 2f, 10f);
      follower.transform.position = structure.transform.position + Vector3.left * 1f + Vector3.down * 1f;
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep(forced: true));
      yield return (object) new WaitForSeconds(5f);
    }
  }

  public static IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static IEnumerator FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static void DEBUG_FORCE_WINTER()
  {
    SeasonsManager.Active = true;
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    SeasonsManager.SetSeason(SeasonsManager.Season.Winter);
    SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
    SeasonsManager.OnSeasonChanged(SeasonsManager.Season.Winter);
  }

  public static void DEBUG_FORCE_SPRING()
  {
    SeasonsManager.Active = true;
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Spring;
    SeasonsManager.SetSeason(SeasonsManager.Season.Spring);
    SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
    SeasonsManager.OnSeasonChanged(SeasonsManager.Season.Spring);
  }

  public static void DEBUG_FORCE_BLIZZARD()
  {
    int num1 = SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter ? 1 : 0;
    if (num1 == 0)
      SeasonsManager.SeasonTimestamp = TimeManager.CurrentDay + SeasonsManager.GetSeasonDuration(SeasonsManager.Season.Winter) - 1;
    SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
    SeasonsManager.SeasonEvent onSeasonChanged = SeasonsManager.OnSeasonChanged;
    if (onSeasonChanged != null)
      onSeasonChanged(SeasonsManager.Season.Winter);
    float num2 = 0.6f;
    float normalisedProgress = SeasonsManager.SEASON_NORMALISED_PROGRESS;
    float num3 = SeasonsManager.SEASON_NORMALISED_PROGRESS + num2;
    DataManager.Instance.blizzardTimeInCurrentSeason = normalisedProgress;
    DataManager.Instance.blizzardEndTimeInCurrentSeason = num3;
    DataManager.Instance.blizzardTimeInCurrentSeason2 = normalisedProgress;
    DataManager.Instance.blizzardEndTimeInCurrentSeason2 = num3;
    SeasonsManager.TriggerWeatherEvent(SeasonsManager.WeatherEvent.Blizzard, false);
    if (num1 != 0)
      return;
    ++DataManager.Instance.WintersOccured;
  }

  public enum Season
  {
    Default = -1, // 0xFFFFFFFF
    Spring = 0,
    Winter = 1,
  }

  public enum WeatherEvent
  {
    None,
    Blizzard,
    Heatwave,
    Typhoon,
  }

  public delegate void TemperatureEvent(float previousTemperature, float currentTemperature);

  public delegate void SeasonEvent(SeasonsManager.Season newSeason);

  public delegate void WeatherTypeEvent(SeasonsManager.WeatherEvent weatherEvent);

  public delegate void DeafultWeatherEvent(WeatherSystemController.WeatherType weatherEvent);

  public class LightningTarget
  {
    public Transform _dynamicTarget;
    public Vector3 _staticPosition;
    public int FollowerID = -1;

    public bool IsDynamic => (UnityEngine.Object) this._dynamicTarget != (UnityEngine.Object) null;

    public Vector3 Position
    {
      get
      {
        return !this.IsDynamic ? this._staticPosition + new Vector3(0.0f, 0.0f, -2f) : this._dynamicTarget.position + new Vector3(0.0f, 0.0f, -1.5f);
      }
    }

    public LightningTarget(Follower target)
    {
      this._dynamicTarget = target.transform;
      this.FollowerID = target.Brain.Info.ID;
      this._staticPosition = Vector3.zero;
    }

    public LightningTarget(Vector3 staticPosition)
    {
      this._dynamicTarget = (Transform) null;
      this._staticPosition = staticPosition;
    }

    public bool IsWithinScreenView()
    {
      Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(this.Position);
      return (double) screenPoint.x > 0.0 & (double) screenPoint.x < (double) Screen.width && (double) screenPoint.y > 0.0 && (double) screenPoint.y < (double) (Screen.height - 100);
    }
  }
}
