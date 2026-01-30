// Decompiled with JetBrains decompiler
// Type: SimulationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class SimulationManager : BaseMonoBehaviour
{
  public static bool UpdatedThisFrame = false;
  public const float DEFAULT_SKIP_SPEED = 500f;
  public static bool ShowFollowerDebugInfo = false;
  public static bool _showFollowerDebugInfo;
  public static bool ShowStructureDebugInfo = false;
  public static bool _showStructureDebugInfo;
  public static bool _showDetailedLocationDebugInfo = false;
  public static Dictionary<int, bool> ShowDetailedFollowerDebugInfo = new Dictionary<int, bool>();
  public static Dictionary<int, bool> ShowDetailedStructureDebugInfo = new Dictionary<int, bool>();
  public float SimulationSpeed = 1f;
  public static bool _isPaused = false;
  public static SimulationManager.OnPause onPause;
  public static SimulationManager.OnUnPause onUnPause;
  public static float _gameMinutesToSkip = 0.0f;
  public static float _currentSkipSpeed = 500f;
  public static SimulationManager.SkipDelegate _shouldEndSkipDelegate;
  public static System.Action _onSkipComplete;
  public static List<FollowerLocation> _skipLocations = new List<FollowerLocation>();
  public Structures_Furnace furnace;
  public float previousFurnaceFuelNormalised;
  public float timeBetweenFurnaceNotification;
  public float dayBetweenFurnaceNotification;
  public static SimulationManager.DropdownData _followerLocationDropdown = new SimulationManager.DropdownData();
  public static SimulationManager.DropdownData _structureTypeDropdown = new SimulationManager.DropdownData();
  public static SimulationManager.DropdownData _structureLocationDropdown = new SimulationManager.DropdownData();

  public static bool IsPaused => SimulationManager._isPaused;

  public float WinterBurnSpeed
  {
    get
    {
      switch (SeasonsManager.WinterSeverity)
      {
        case 0:
          return 0.8f;
        case 1:
          return 0.8f;
        case 2:
          return 0.9f;
        case 3:
          return 1f;
        case 4:
          return 1.1f;
        case 5:
          return 1.2f;
        default:
          return 1f;
      }
    }
  }

  public void Update()
  {
    if (SimulationManager.UpdatedThisFrame)
      return;
    TrinketManager.UpdateCooldowns(Time.deltaTime);
    float deltaGameTime = (float) ((double) Time.deltaTime * (double) SimulationManager._currentSkipSpeed / 480.0 * 1200.0);
    if ((double) SimulationManager._gameMinutesToSkip > 0.0)
    {
      if ((double) deltaGameTime > (double) SimulationManager._gameMinutesToSkip)
        deltaGameTime = SimulationManager._gameMinutesToSkip;
      TimeManager.Simulate(deltaGameTime, true);
      SimulationManager._gameMinutesToSkip -= TimeManager.DeltaGameTime;
      if (SimulationManager._shouldEndSkipDelegate != null && SimulationManager._shouldEndSkipDelegate())
        SimulationManager._gameMinutesToSkip = 0.0f;
      if ((double) SimulationManager._gameMinutesToSkip <= 0.0)
      {
        foreach (FollowerLocation skipLocation in SimulationManager._skipLocations)
          LocationManager.ActivateLocation(skipLocation);
        SimulationManager._skipLocations.Clear();
        SimulationManager._shouldEndSkipDelegate = (SimulationManager.SkipDelegate) null;
        System.Action onSkipComplete = SimulationManager._onSkipComplete;
        SimulationManager._onSkipComplete = (System.Action) null;
        if (onSkipComplete != null)
          onSkipComplete();
      }
      SimulationManager.UpdatedThisFrame = true;
    }
    else if (!SimulationManager._isPaused)
    {
      deltaGameTime = (float) ((double) Time.deltaTime * (double) this.SimulationSpeed / 480.0 * 1200.0);
      TimeManager.Simulate(deltaGameTime);
      SimulationManager.UpdatedThisFrame = true;
    }
    if (SimulationManager._isPaused)
      return;
    if (this.furnace != null && !DataManager.Instance.HasFurnace)
      this.furnace = (Structures_Furnace) null;
    else if (this.furnace == null && DataManager.Instance.HasFurnace)
    {
      this.furnace = StructureManager.GetAllStructuresOfType<Structures_Furnace>().FirstOrDefault<Structures_Furnace>();
      this.previousFurnaceFuelNormalised = WarmthBar.WarmthNormalized;
    }
    else
    {
      if (this.furnace == null || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
        return;
      if (!FollowerBrainStats.LockedWarmth)
        this.furnace.UpdateFuel(Mathf.RoundToInt(deltaGameTime * 187f * this.WinterBurnSpeed));
      float warmthNormalized = WarmthBar.WarmthNormalized;
      if ((double) this.previousFurnaceFuelNormalised != 3.4028234663852886E+38 && (double) this.previousFurnaceFuelNormalised <= 0.0 && (double) warmthNormalized > 0.0)
        this.dayBetweenFurnaceNotification = (float) (TimeManager.CurrentDay - 1);
      if ((double) this.previousFurnaceFuelNormalised != 3.4028234663852886E+38 && (double) warmthNormalized > 0.0 && (double) this.previousFurnaceFuelNormalised > 0.25 && (double) warmthNormalized <= 0.25 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) Time.time > (double) this.timeBetweenFurnaceNotification)
      {
        NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.Furnace_Low);
        this.timeBetweenFurnaceNotification = Time.time + 1f;
      }
      else if ((double) this.previousFurnaceFuelNormalised != 3.4028234663852886E+38 && (double) this.previousFurnaceFuelNormalised >= 0.0 && (double) warmthNormalized <= 0.0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) TimeManager.CurrentDay > (double) this.dayBetweenFurnaceNotification && (double) Time.time > (double) this.timeBetweenFurnaceNotification)
      {
        NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.Furnace_Empty);
        this.dayBetweenFurnaceNotification = (float) (TimeManager.CurrentDay + 1);
      }
      this.previousFurnaceFuelNormalised = WarmthBar.WarmthNormalized;
    }
  }

  public void LateUpdate() => SimulationManager.UpdatedThisFrame = false;

  public static void Pause()
  {
    SimulationManager._isPaused = true;
    SimulationManager.OnPause onPause = SimulationManager.onPause;
    if (onPause == null)
      return;
    onPause();
  }

  public static void UnPause()
  {
    SimulationManager._isPaused = false;
    SimulationManager.OnUnPause onUnPause = SimulationManager.onUnPause;
    if (onUnPause == null)
      return;
    onUnPause();
  }

  public static float GetSkipSpeed(float desiredDuration, float gameMinutesToSkip)
  {
    return (float) ((double) gameMinutesToSkip / 1200.0 * 480.0) / desiredDuration;
  }

  public static void SkipToPhase(float desiredDuration, DayPhase phase, System.Action onComplete)
  {
    float num = TimeManager.TimeRemainingUntilPhase(phase) - 1f;
    SimulationManager.Skip(SimulationManager.GetSkipSpeed(desiredDuration, num), (SimulationManager.SkipDelegate) null, num, onComplete);
  }

  public static void SkipToPhase(DayPhase phase, System.Action onComplete)
  {
    SimulationManager.Skip(500f, (SimulationManager.SkipDelegate) null, TimeManager.TimeRemainingUntilPhase(phase) - 1f, onComplete);
  }

  public static void SkipWithDuration(
    float desiredDuration,
    SimulationManager.SkipDelegate check,
    float maxGameMinutesToSkip,
    System.Action onComplete)
  {
    SimulationManager.Skip(SimulationManager.GetSkipSpeed(desiredDuration, maxGameMinutesToSkip), check, maxGameMinutesToSkip, onComplete);
  }

  public static void Skip(
    SimulationManager.SkipDelegate check,
    float maxGameMinutesToSkip,
    System.Action onComplete)
  {
    SimulationManager.Skip(500f, check, maxGameMinutesToSkip, onComplete);
  }

  public static void Skip(
    float skipSpeed,
    SimulationManager.SkipDelegate check,
    float maxGameMinutesToSkip,
    System.Action onComplete)
  {
    SimulationManager._skipLocations.AddRange(LocationManager.LocationsInState(LocationState.Active));
    foreach (FollowerLocation skipLocation in SimulationManager._skipLocations)
      LocationManager.DeactivateLocation(skipLocation);
    SimulationManager._shouldEndSkipDelegate = check;
    SimulationManager._gameMinutesToSkip = maxGameMinutesToSkip;
    SimulationManager._currentSkipSpeed = skipSpeed;
    SimulationManager._onSkipComplete = onComplete;
  }

  public static int CountSimFollowers(FollowerBrain brain)
  {
    int num = 0;
    for (int location = 0; location < 98; ++location)
    {
      foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation((FollowerLocation) location))
      {
        if (simFollower.Brain == brain && !simFollower.Retired)
          ++num;
      }
    }
    return num;
  }

  public Follower FindFollower(FollowerBrain brain)
  {
    Follower follower1 = (Follower) null;
    for (int location = 0; location < 98; ++location)
    {
      foreach (Follower follower2 in FollowerManager.FollowersAtLocation((FollowerLocation) location))
      {
        if (follower2.Brain == brain)
        {
          follower1 = follower2;
          goto label_9;
        }
      }
    }
label_9:
    return follower1;
  }

  public bool GUIDropdown(
    SimulationManager.DropdownData data,
    Array values,
    string label = null,
    string secondaryButtonLabel = null,
    System.Action onSecondaryButton = null)
  {
    GUILayout.BeginHorizontal(GUILayout.Width(250f));
    if (!string.IsNullOrEmpty(label))
      GUILayout.Label(label + ":");
    if (GUILayout.Button(values.GetValue(data.selectedIndex).ToString()))
      data.show = !data.show;
    if (secondaryButtonLabel != null && GUILayout.Button(secondaryButtonLabel) && onSecondaryButton != null)
      onSecondaryButton();
    GUILayout.EndHorizontal();
    Rect lastRect = GUILayoutUtility.GetLastRect() with
    {
      height = 300f
    };
    if (data.show)
    {
      data.scrollViewVector = GUI.BeginScrollView(new Rect(lastRect.x, lastRect.y + 25f, lastRect.width, lastRect.height), data.scrollViewVector, new Rect(0.0f, 0.0f, lastRect.width, Mathf.Max(lastRect.height, (float) (values.Length * 25))));
      GUI.Box(new Rect(0.0f, 0.0f, lastRect.width, Mathf.Max(lastRect.height, (float) (values.Length * 25))), "");
      for (int index = 0; index < values.Length; ++index)
      {
        if (GUI.Button(new Rect(0.0f, (float) (index * 25), lastRect.width, 25f), values.GetValue(index).ToString()))
        {
          data.show = false;
          data.selectedIndex = index;
        }
      }
      GUI.EndScrollView();
    }
    return data.show;
  }

  public void SetLocationToHere(
    SimulationManager.DropdownData locationDropdownData)
  {
    FollowerLocation[] values = Enum.GetValues(typeof (FollowerLocation)) as FollowerLocation[];
    for (int index = 0; index < values.Length; ++index)
    {
      if (values[index] == PlayerFarming.Location)
      {
        locationDropdownData.selectedIndex = index;
        break;
      }
    }
  }

  public delegate bool SkipDelegate();

  public delegate void OnPause();

  public delegate void OnUnPause();

  public class DropdownData
  {
    public bool show;
    public int selectedIndex;
    public Vector2 scrollViewVector;
  }
}
