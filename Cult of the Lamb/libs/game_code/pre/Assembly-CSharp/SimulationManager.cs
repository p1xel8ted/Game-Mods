// Decompiled with JetBrains decompiler
// Type: SimulationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class SimulationManager : BaseMonoBehaviour
{
  private static bool UpdatedThisFrame = false;
  public const float DEFAULT_SKIP_SPEED = 100f;
  public static bool ShowFollowerDebugInfo = false;
  private static bool _showFollowerDebugInfo;
  public static bool ShowStructureDebugInfo = false;
  private static bool _showStructureDebugInfo;
  private static bool _showDetailedLocationDebugInfo = false;
  public static Dictionary<int, bool> ShowDetailedFollowerDebugInfo = new Dictionary<int, bool>();
  public static Dictionary<int, bool> ShowDetailedStructureDebugInfo = new Dictionary<int, bool>();
  public float SimulationSpeed = 1f;
  private static bool _isPaused = false;
  private static float _gameMinutesToSkip = 0.0f;
  private static float _currentSkipSpeed = 100f;
  private static SimulationManager.SkipDelegate _shouldEndSkipDelegate;
  private static System.Action _onSkipComplete;
  private static List<FollowerLocation> _skipLocations = new List<FollowerLocation>();
  private static SimulationManager.DropdownData _followerLocationDropdown = new SimulationManager.DropdownData();
  private static SimulationManager.DropdownData _structureTypeDropdown = new SimulationManager.DropdownData();
  private static SimulationManager.DropdownData _structureLocationDropdown = new SimulationManager.DropdownData();

  public static bool IsPaused => SimulationManager._isPaused;

  private void Update()
  {
    if (SimulationManager.UpdatedThisFrame)
      return;
    TrinketManager.UpdateCooldowns(Time.deltaTime);
    if ((double) SimulationManager._gameMinutesToSkip > 0.0)
    {
      float deltaGameTime = (float) ((double) Time.deltaTime * (double) SimulationManager._currentSkipSpeed / 480.0 * 1200.0);
      if ((double) deltaGameTime > (double) SimulationManager._gameMinutesToSkip)
        deltaGameTime = SimulationManager._gameMinutesToSkip;
      TimeManager.Simulate(deltaGameTime);
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
    else
    {
      if (SimulationManager._isPaused)
        return;
      TimeManager.Simulate((float) ((double) Time.deltaTime * (double) this.SimulationSpeed / 480.0 * 1200.0));
      SimulationManager.UpdatedThisFrame = true;
    }
  }

  private void LateUpdate() => SimulationManager.UpdatedThisFrame = false;

  public static void Pause() => SimulationManager._isPaused = true;

  public static void UnPause() => SimulationManager._isPaused = false;

  private static float GetSkipSpeed(float desiredDuration, float gameMinutesToSkip)
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
    SimulationManager.Skip(100f, (SimulationManager.SkipDelegate) null, TimeManager.TimeRemainingUntilPhase(phase) - 1f, onComplete);
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
    SimulationManager.Skip(100f, check, maxGameMinutesToSkip, onComplete);
  }

  private static void Skip(
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

  private void OnGUI()
  {
    if (PerformanceTest.ReduceGUI)
      return;
    if (Event.current.type == EventType.Layout)
    {
      if (!SimulationManager._showFollowerDebugInfo && SimulationManager.ShowFollowerDebugInfo)
        SimulationManager._followerLocationDropdown.selectedIndex = (int) PlayerFarming.Location;
      SimulationManager._showFollowerDebugInfo = SimulationManager.ShowFollowerDebugInfo;
      SimulationManager._showStructureDebugInfo = SimulationManager.ShowStructureDebugInfo;
    }
    GUILayout.BeginArea(new Rect(935f, 200f, 1000f, 1000f));
    GUILayout.BeginVertical();
    if (SimulationManager._showFollowerDebugInfo)
    {
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(SimulationManager._showDetailedLocationDebugInfo ? "-" : "+", GUILayout.Width(50f)))
        SimulationManager._showDetailedLocationDebugInfo = !SimulationManager._showDetailedLocationDebugInfo;
      StringBuilder stringBuilder1 = new StringBuilder();
      IEnumerable<FollowerLocation> values1 = LocationManager.LocationsInState(LocationState.Active);
      stringBuilder1.AppendLine($"Player Location: {PlayerFarming.Location}, Prev: {PlayerFarming.LastLocation}");
      stringBuilder1.AppendLine("Active Locations: " + string.Join<FollowerLocation>(", ", values1));
      if (SimulationManager._showDetailedLocationDebugInfo)
      {
        IEnumerable<FollowerLocation> values2 = LocationManager.LocationsInState(LocationState.Inactive);
        stringBuilder1.AppendLine("Inactive Locations: " + string.Join<FollowerLocation>(", ", values2));
        IEnumerable<FollowerLocation> values3 = LocationManager.LocationsInState(LocationState.Unloaded);
        stringBuilder1.AppendLine("Unloaded Locations: " + string.Join<FollowerLocation>(", ", values3));
      }
      GUILayout.Label(stringBuilder1.ToString());
      GUILayout.EndHorizontal();
      Array values4 = Enum.GetValues(typeof (FollowerLocation));
      FollowerLocation[] values5 = Enum.GetValues(typeof (FollowerLocation)) as FollowerLocation[];
      if (!this.GUIDropdown(SimulationManager._followerLocationDropdown, values4, "Home Location", "Here", (System.Action) (() => this.SetLocationToHere(SimulationManager._followerLocationDropdown))))
      {
        FollowerLocation location = values5[SimulationManager._followerLocationDropdown.selectedIndex];
        GUILayout.Label("Override Task: " + TimeManager.GetOverrideTaskString());
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HomeLocation == location)
          {
            GUILayout.BeginHorizontal();
            bool flag = false;
            SimulationManager.ShowDetailedFollowerDebugInfo.TryGetValue(allBrain.Info.ID, out flag);
            if (GUILayout.Button(flag ? "-" : "+", GUILayout.Width(25f)))
              SimulationManager.ShowDetailedFollowerDebugInfo[allBrain.Info.ID] = !flag;
            StringBuilder stringBuilder2 = new StringBuilder();
            Thought cursedState;
            if (allBrain.Location == allBrain.DesiredLocation)
            {
              StringBuilder stringBuilder3 = stringBuilder2;
              object[] objArray = new object[5]
              {
                (object) allBrain.Info.Name,
                (object) allBrain.Info.ID,
                null,
                null,
                null
              };
              cursedState = allBrain.Info.CursedState;
              objArray[2] = (object) cursedState.ToString();
              objArray[3] = allBrain.CurrentTask == null ? (object) "(null)" : (object) allBrain.CurrentTask.ToDebugString();
              objArray[4] = (object) allBrain.Location;
              string str = string.Format("{0} {1} cursed state: {2}: {3} ({4})", objArray);
              stringBuilder3.AppendLine(str);
            }
            else
            {
              StringBuilder stringBuilder4 = stringBuilder2;
              object[] objArray = new object[6]
              {
                (object) allBrain.Info.Name,
                (object) allBrain.Info.ID,
                null,
                null,
                null,
                null
              };
              cursedState = allBrain.Info.CursedState;
              objArray[2] = (object) cursedState.ToString();
              objArray[3] = allBrain.CurrentTask == null ? (object) "(null)" : (object) allBrain.CurrentTask.ToDebugString();
              objArray[4] = (object) allBrain.Location;
              objArray[5] = (object) allBrain.DesiredLocation;
              string str = string.Format("{0} {1} cursed state: {2}: {3} ({4}, {5})", objArray);
              stringBuilder4.AppendLine(str);
            }
            Follower follower = this.FindFollower(allBrain);
            int num = SimulationManager.CountSimFollowers(allBrain);
            if (num > 1)
              stringBuilder2.AppendLine($"<color=red><b>DUPLICATE SIM FOLLOWERS FOUND ({num}), TELL MATT!</b></color>");
            if ((UnityEngine.Object) follower != (UnityEngine.Object) null && !follower.IsPaused && num > 0)
              stringBuilder2.AppendLine("<color=red><b>FOLLOWER AND SIM FOLLOWERS FOUND, TELL MATT!</b></color>");
            if (flag)
            {
              stringBuilder2.AppendLine($"State: {allBrain.CurrentState?.Type}");
              stringBuilder2.AppendLine($"Hunger: {allBrain.Stats.Satiation:F2}, Rest: {allBrain.Stats.Rest:F2}, Happiness: {allBrain.Stats.Happiness:F2}, Bathroom: {allBrain.Stats.Bathroom:F2} -> {allBrain.Stats.TargetBathroom:F2}");
              stringBuilder2.AppendLine($"Illness: {allBrain.Stats.Illness:F2}, Adoration: {allBrain.Stats.Adoration}");
              Dwelling.DwellingAndSlot dwellingAndSlot = allBrain.GetDwellingAndSlot();
              if (dwellingAndSlot != null)
                stringBuilder2.AppendLine($"Dwelling ID: {dwellingAndSlot.ID}, Slot: {dwellingAndSlot.dwellingslot}, Level: {dwellingAndSlot.dwellingLevel}");
              else
                stringBuilder2.AppendLine("Dwelling: Homeless");
              if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
              {
                string name1 = follower.Spine.state.GetCurrent(0)?.Animation.Name;
                string name2 = follower.Spine.state.GetCurrent(1)?.Animation.Name;
                stringBuilder2.AppendLine($"FaceAnim: {name1}, BodyAnim: {name2}");
                SimpleSpineAnimator.SpineChartacterAnimationData animationData1 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
                SimpleSpineAnimator.SpineChartacterAnimationData animationData2 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
                stringBuilder2.AppendLine($"State: {follower.State.CURRENT_STATE}; Anims, Move: {animationData1.Animation.Animation.Name}, Idle: {animationData2.Animation.Animation.Name}");
              }
            }
            GUILayout.Label(stringBuilder2.ToString());
            GUILayout.EndHorizontal();
          }
        }
        StringBuilder stringBuilder5 = new StringBuilder();
        GUILayout.Label("Real Followers At Location:");
        foreach (Follower follower in FollowerManager.FollowersAtLocation(location))
          stringBuilder5.Append($"{follower.Brain.Info.Name} {follower.Brain.Info.ID}, ");
        GUILayout.Label(stringBuilder5.ToString());
        stringBuilder5.Clear();
        GUILayout.Label("Sim Followers At Location:");
        foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(location))
          stringBuilder5.Append($"{simFollower.Brain.Info.Name} {simFollower.Brain.Info.ID}, ");
        GUILayout.Label(stringBuilder5.ToString());
      }
    }
    if (SimulationManager._showStructureDebugInfo)
    {
      GUILayout.Label("Structures Debug:");
      Array values6 = Enum.GetValues(typeof (StructureBrain.TYPES));
      StructureBrain.TYPES[] values7 = Enum.GetValues(typeof (StructureBrain.TYPES)) as StructureBrain.TYPES[];
      if (!this.GUIDropdown(SimulationManager._structureTypeDropdown, values6, "Type"))
      {
        Array values8 = Enum.GetValues(typeof (FollowerLocation));
        FollowerLocation[] values9 = Enum.GetValues(typeof (FollowerLocation)) as FollowerLocation[];
        if (!this.GUIDropdown(SimulationManager._structureLocationDropdown, values8, "Location", "Here", (System.Action) (() => this.SetLocationToHere(SimulationManager._structureLocationDropdown))))
        {
          StructureBrain.TYPES type = values7[SimulationManager._structureTypeDropdown.selectedIndex];
          FollowerLocation location = values9[SimulationManager._structureLocationDropdown.selectedIndex];
          List<StructureBrain> structureBrainList = location != FollowerLocation.None ? StructureManager.GetAllStructuresOfType(location, type) : StructureManager.GetAllStructuresOfType(type);
          GUILayout.Label($"Count: {structureBrainList.Count}");
          int num = Mathf.Min(10, structureBrainList.Count);
          for (int index = 0; index < num; ++index)
          {
            StructureBrain structureBrain = structureBrainList[index];
            GUILayout.BeginHorizontal();
            bool flag = false;
            SimulationManager.ShowDetailedStructureDebugInfo.TryGetValue(structureBrain.Data.ID, out flag);
            if (GUILayout.Button(flag ? "-" : "+", GUILayout.Width(25f)))
              SimulationManager.ShowDetailedStructureDebugInfo[structureBrain.Data.ID] = !flag;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ID: {structureBrain.Data.ID}; Type: {structureBrain.Data.Type}");
            if (flag)
              structureBrain.ToDebugString(sb);
            GUILayout.Label(sb.ToString());
            GUILayout.EndHorizontal();
          }
        }
      }
    }
    GUILayout.EndVertical();
    GUILayout.EndArea();
  }

  public static int CountSimFollowers(FollowerBrain brain)
  {
    int num = 0;
    for (int location = 0; location < 84; ++location)
    {
      foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation((FollowerLocation) location))
      {
        if (simFollower.Brain == brain && !simFollower.Retired)
          ++num;
      }
    }
    return num;
  }

  private Follower FindFollower(FollowerBrain brain)
  {
    Follower follower1 = (Follower) null;
    for (int location = 0; location < 84; ++location)
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

  private bool GUIDropdown(
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

  private void SetLocationToHere(
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

  private class DropdownData
  {
    public bool show;
    public int selectedIndex;
    public Vector2 scrollViewVector;
  }
}
