// Decompiled with JetBrains decompiler
// Type: TwitchHelpHinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Map;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class TwitchHelpHinder
{
  public static bool Active = false;
  public static Vector2 TimeBetweenEvent = new Vector2(900f, 1200f);
  public const string COMPLETED = "COMPLETED";
  public const string VOTING = "HH_VOTING";
  public const string OUTCOME_VOTING = "OUTCOME_VOTING";
  public const string HELP = "help";
  public const string HINDER = "hinder";
  public const int MaxChoices = 3;
  public static TwitchHelpHinder.HHData CurrentData;
  private static TwitchHelpHinder.HHOption[] currentHelpOptions;
  private static TwitchHelpHinder.HHOption[] currentHinderOptions;
  private static bool initialLoad = true;
  public static bool Deactivated = false;

  public static bool Available
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TwitchNextHHEvent;
    }
  }

  public static float Timer { get; private set; }

  public static float VotingPhaseDuration { get; private set; } = 30f;

  public static float ChoicePhaseDuration { get; private set; } = 30f;

  public static event TwitchHelpHinder.HHResponse HHStatusChanged;

  public static event TwitchHelpHinder.HHResponse HHUpdated;

  public static void Initialise()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchHelpHinder.TwitchRequest_OnSocketReceived);
    TwitchRequest.OnSocketReceived += new TwitchRequest.SocketResponse(TwitchHelpHinder.TwitchRequest_OnSocketReceived);
  }

  public static void StartHHEvent(bool isDungeon)
  {
    if (TwitchHelpHinder.Deactivated || !TwitchRequest.SocketConnected)
      return;
    TwitchHelpHinder.Active = true;
    TwitchHelpHinder.Timer = 0.0f;
    TwitchHelpHinder.SetNextEventTime();
    List<WorldManipulatorManager.Manipulations> manipulationsList1 = isDungeon ? WorldManipulatorManager.GetPossibleDungeonPositiveManipulations() : WorldManipulatorManager.GetPossibleBasePositiveManipulations();
    if (manipulationsList1.Count < 3)
      return;
    manipulationsList1.RemoveRange(3, manipulationsList1.Count - 3);
    List<WorldManipulatorManager.Manipulations> manipulationsList2 = isDungeon ? WorldManipulatorManager.GetPossibleDungeonNegativeManipulations() : WorldManipulatorManager.GetPossibleBaseNegativeManipulations();
    if (manipulationsList2.Count < 3)
      return;
    manipulationsList2.RemoveRange(3, manipulationsList2.Count - 3);
    TwitchHelpHinder.currentHelpOptions = new TwitchHelpHinder.HHOption[manipulationsList1.Count];
    TwitchHelpHinder.currentHinderOptions = new TwitchHelpHinder.HHOption[manipulationsList2.Count];
    for (int index = 0; index < TwitchHelpHinder.currentHelpOptions.Length; ++index)
      TwitchHelpHinder.currentHelpOptions[index] = new TwitchHelpHinder.HHOption()
      {
        label = WorldManipulatorManager.GetLocalisation(manipulationsList1[index]),
        value = manipulationsList1[index].ToString()
      };
    for (int index = 0; index < TwitchHelpHinder.currentHinderOptions.Length; ++index)
      TwitchHelpHinder.currentHinderOptions[index] = new TwitchHelpHinder.HHOption()
      {
        label = WorldManipulatorManager.GetLocalisation(manipulationsList2[index]),
        value = manipulationsList2[index].ToString()
      };
    TwitchHelpHinder.SendHHEvent(new TwitchHelpHinder.HHData_Send()
    {
      status = "HH_VOTING",
      help_outcome_options = TwitchHelpHinder.currentHelpOptions,
      hinder_outcome_options = TwitchHelpHinder.currentHinderOptions
    }, (System.Action) (() =>
    {
      if ((UnityEngine.Object) BaseGoopDoor.Instance != (UnityEngine.Object) null)
      {
        BaseGoopDoor.Instance.DoorUp("UI/Twitch/HH/DecidingFate");
        BaseGoopDoor.Instance.LockDoor = true;
      }
      NotificationCentre.Instance.PlayHelpHinderNotification("UI/Twitch/HH/DecidingFate");
    }));
    TwitchRequest.Request(TwitchRequest.uri + "timers", (TwitchRequest.RequestResponse) ((response, result) =>
    {
      if (response != TwitchRequest.ResponseType.Success)
        return;
      try
      {
        TwitchHelpHinder.TimerData timerData = JsonUtility.FromJson<TwitchHelpHinder.TimerData>(result);
        TwitchHelpHinder.VotingPhaseDuration = (float) timerData.helpHinder.hhVote;
        TwitchHelpHinder.ChoicePhaseDuration = (float) timerData.helpHinder.outcomeVote;
        MonoSingleton<TwitchManager>.Instance.StartCoroutine((IEnumerator) TwitchHelpHinder.ForceEndEvent((float) (((double) TwitchHelpHinder.VotingPhaseDuration + (double) TwitchHelpHinder.ChoicePhaseDuration) * 1.5)));
      }
      catch (Exception ex)
      {
      }
    }), TwitchRequest.RequestType.GET, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  private static IEnumerator ForceEndEvent(float delay)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    TwitchHelpHinder.EndHHEvent((TwitchHelpHinder.HHData) null);
  }

  public static void EndHHEvent(TwitchHelpHinder.HHData data)
  {
    if (!TwitchHelpHinder.Active)
      return;
    NotificationHelpHinder.CloseNotification();
    TwitchHelpHinder.Active = false;
    TwitchHelpHinder.Timer = 0.0f;
    TwitchHelpHinder.SetNextEventTime();
    if ((UnityEngine.Object) BaseGoopDoor.Instance != (UnityEngine.Object) null)
    {
      BaseGoopDoor.Instance.LockDoor = false;
      BaseGoopDoor.Instance.DoorDown();
    }
    TwitchHelpHinder.CurrentData = data;
    TwitchHelpHinder.HHResponse hhStatusChanged = TwitchHelpHinder.HHStatusChanged;
    if (hhStatusChanged != null)
      hhStatusChanged(data);
    WorldManipulatorManager.Manipulations result;
    if (data == null || !Enum.TryParse<WorldManipulatorManager.Manipulations>(data.outcome_winning_option, out result))
      return;
    WorldManipulatorManager.TriggerManipulation(result, twitch: true);
  }

  public static void SendHHEvent(TwitchHelpHinder.HHData_Send data, System.Action sentCallback = null)
  {
    TwitchRequest.Request(TwitchRequest.uri + "help-or-hinder", (TwitchRequest.RequestResponse) ((res, result) =>
    {
      System.Action action = sentCallback;
      if (action == null)
        return;
      action();
    }), TwitchRequest.RequestType.POST, JsonUtility.ToJson((object) data), new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void Update()
  {
    if (!TwitchHelpHinder.Active)
      return;
    TwitchHelpHinder.Timer += Time.unscaledDeltaTime;
  }

  private static void TwitchRequest_OnSocketReceived(string key, string data)
  {
    if (!(key == "hh.completed"))
      return;
    try
    {
      TwitchHelpHinder.EndHHEvent(JsonUtility.FromJson<TwitchHelpHinder.HHData>(data));
    }
    catch (Exception ex)
    {
    }
  }

  public static void LocationChanged(FollowerLocation location)
  {
    if (!DataManager.Instance.OnboardingFinished)
      return;
    if ((double) DataManager.Instance.TwitchNextHHEvent == -1.0 || TwitchHelpHinder.initialLoad)
      TwitchHelpHinder.SetNextEventTime();
    TwitchHelpHinder.initialLoad = false;
    if (TwitchHelpHinder.Available && !TwitchHelpHinder.Active && location == FollowerLocation.Base)
    {
      TwitchHelpHinder.StartHHEvent(false);
      BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
    }
    if (GameManager.IsDungeon(location))
      BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
    else
      BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
  }

  private static void OnChangedRoom()
  {
    if (!TwitchHelpHinder.Available || TwitchHelpHinder.Active || !((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null) || MapManager.Instance.CurrentNode == null || MapManager.Instance.CurrentNode.nodeType == NodeType.MiniBossFloor || UnityEngine.Random.Range(0, 100) >= 30)
      return;
    TwitchHelpHinder.StartHHEvent(true);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
  }

  public static void SetNextEventTime()
  {
    DataManager.Instance.TwitchNextHHEvent = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(TwitchHelpHinder.TimeBetweenEvent.x, TwitchHelpHinder.TimeBetweenEvent.y);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
  }

  public static void Abort()
  {
    TwitchHelpHinder.EndHHEvent((TwitchHelpHinder.HHData) null);
    TwitchHelpHinder.initialLoad = false;
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchHelpHinder.TwitchRequest_OnSocketReceived);
  }

  [Serializable]
  public class HHData_Send
  {
    public string status;
    public TwitchHelpHinder.HHOption[] help_outcome_options;
    public TwitchHelpHinder.HHOption[] hinder_outcome_options;
  }

  [Serializable]
  public class HHData
  {
    public long channel_id;
    public string status;
    public TwitchHelpHinder.HHOption[] help_outcome_options;
    public TwitchHelpHinder.HHOption[] hinder_outcome_options;
    public TwitchHelpHinder.HHVotes hh_votes;
    public TwitchHelpHinder.HHOptionResult[] outcome_votes;
    public float created_at;
    public float updated_at;
    public string hh_vote_result;
    public string hh_winning_option;
    public string outcome_winning_option;
  }

  [Serializable]
  public class HHOption
  {
    public string label;
    public string value;
  }

  [Serializable]
  public class HHVotes
  {
    public int help;
    public int hinder;
  }

  [Serializable]
  public class HHOptionResult
  {
    public string value;
    public int votes;
  }

  [Serializable]
  public class FollowerTimerData
  {
    public int raffle;
    public int creation;
  }

  [Serializable]
  public class HelpHinderTimerData
  {
    public int hhVote;
    public int outcomeVote;
  }

  [Serializable]
  public class TimerData
  {
    public TwitchHelpHinder.FollowerTimerData followers;
    public TwitchHelpHinder.HelpHinderTimerData helpHinder;
  }

  public delegate void HHResponse(TwitchHelpHinder.HHData data);
}
