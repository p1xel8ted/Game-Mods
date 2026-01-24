// Decompiled with JetBrains decompiler
// Type: TwitchHelpHinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Map;
using MMBiomeGeneration;
using Org.OpenAPITools.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public static class TwitchHelpHinder
{
  public static bool Active = false;
  public static Vector2 TimeBetweenEvent = new Vector2(DataManager.Instance.TwitchSettings.HelpHinderFrequency * 0.75f, DataManager.Instance.TwitchSettings.HelpHinderFrequency);
  [CompilerGenerated]
  public static float \u003CTimer\u003Ek__BackingField;
  [CompilerGenerated]
  public static int \u003CVotingPhaseDuration\u003Ek__BackingField = 15;
  public const string COMPLETED = "COMPLETED";
  public const string VOTING = "HH_VOTING";
  public const string OUTCOME_VOTING = "OUTCOME_VOTING";
  public const string HELP = "HELP";
  public const string HINDER = "HINDER";
  public const int MaxChoices = 3;
  public static TwitchHelpHinder.HHOption[] currentHelpOptions;
  public static TwitchHelpHinder.HHOption[] currentHinderOptions;
  public static bool initialLoad = true;
  public static bool Deactivated = false;
  public const float kMinHelpHinderMinutes = 20f;
  public const float kMaxHelpHinderMinutes = 50f;
  public static Coroutine underDoorsCoroutine;

  public static bool Available
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TwitchNextHHEvent;
    }
  }

  public static float Timer
  {
    get => TwitchHelpHinder.\u003CTimer\u003Ek__BackingField;
    set => TwitchHelpHinder.\u003CTimer\u003Ek__BackingField = value;
  }

  public static int VotingPhaseDuration
  {
    get => TwitchHelpHinder.\u003CVotingPhaseDuration\u003Ek__BackingField;
    set => TwitchHelpHinder.\u003CVotingPhaseDuration\u003Ek__BackingField = value;
  }

  public static void Initialise() => TwitchHelpHinder.Abort();

  public static void TriggerEvent()
  {
    if (TwitchHelpHinder.Deactivated || !DataManager.Instance.TwitchSettings.HelpHinderEnabled || !TwitchAuthentication.IsAuthenticated)
      return;
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchHelpHinder.HHEventIE());
  }

  public static IEnumerator HHEventIE()
  {
    bool flag = GameManager.IsDungeon(PlayerFarming.Location);
    TwitchHelpHinder.Timer = 0.0f;
    TwitchHelpHinder.SetNextEventTime();
    List<WorldManipulatorManager.Manipulations> helpManipulations = flag ? WorldManipulatorManager.GetPossibleDungeonPositiveManipulations() : WorldManipulatorManager.GetPossibleBasePositiveManipulations();
    if (helpManipulations.Count >= 3)
    {
      helpManipulations.RemoveRange(3, helpManipulations.Count - 3);
      List<WorldManipulatorManager.Manipulations> hinderManipulations = flag ? WorldManipulatorManager.GetPossibleDungeonNegativeManipulations() : WorldManipulatorManager.GetPossibleBaseNegativeManipulations();
      if (hinderManipulations.Count >= 3)
      {
        hinderManipulations.RemoveRange(3, hinderManipulations.Count - 3);
        TwitchHelpHinder.currentHelpOptions = new TwitchHelpHinder.HHOption[helpManipulations.Count];
        TwitchHelpHinder.currentHinderOptions = new TwitchHelpHinder.HHOption[hinderManipulations.Count];
        for (int index = 0; index < TwitchHelpHinder.currentHelpOptions.Length; ++index)
          TwitchHelpHinder.currentHelpOptions[index] = new TwitchHelpHinder.HHOption()
          {
            label = WorldManipulatorManager.GetLocalisation(helpManipulations[index]),
            value = helpManipulations[index].ToString()
          };
        for (int index = 0; index < TwitchHelpHinder.currentHinderOptions.Length; ++index)
          TwitchHelpHinder.currentHinderOptions[index] = new TwitchHelpHinder.HHOption()
          {
            label = WorldManipulatorManager.GetLocalisation(hinderManipulations[index]),
            value = hinderManipulations[index].ToString()
          };
        string prompt = LocalizationManager.GetTranslation("UI/Twitch/HH/Title");
        List<PollOption> options1 = new List<PollOption>()
        {
          new PollOption("HELP", LocalizationManager.GetTranslation("UI/Twitch/HH/Good")),
          new PollOption("HINDER", LocalizationManager.GetTranslation("UI/Twitch/HH/Bad"))
        };
        StartPollRequest startPollRequest1 = new StartPollRequest(new StartPollRequestType(PollType.HELPHINDER), prompt, (object) new TwitchHelpHinder.HHMetadata()
        {
          timer = TwitchHelpHinder.VotingPhaseDuration
        }, options1);
        NotificationCentre.Instance.PlayHelpHinderNotification("UI/Twitch/HH/DecidingFate");
        BaseGoopDoor.DoorUp("UI/Twitch/HH/DecidingFate");
        BaseGoopDoor.LockDoor = true;
        GameManager.GetInstance().WaitForSeconds(35f, (System.Action) (() =>
        {
          BaseGoopDoor.LockDoor = false;
          BaseGoopDoor.DoorDown();
          TwitchHelpHinder.Active = false;
        }));
        TwitchHelpHinder.Active = true;
        TwitchRequest.EBS_API.StartPollAsync(startPollRequest1, new CancellationToken());
        Debug.Log((object) "POLL SET UP");
        yield return (object) new WaitForSecondsRealtime((float) TwitchHelpHinder.VotingPhaseDuration);
        Task<Poll> poll = TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
        if (poll == null)
        {
          BaseGoopDoor.LockDoor = false;
          BaseGoopDoor.DoorDown();
          TwitchHelpHinder.Active = false;
        }
        else
        {
          float timer = 0.0f;
          yield return (object) new WaitUntil((Func<bool>) (() => poll.IsCompleted || (double) (timer += Time.unscaledDeltaTime) > 10.0));
          if (!poll.IsCompleted || poll.Result.Options.Count < 2)
          {
            BaseGoopDoor.LockDoor = false;
            BaseGoopDoor.DoorDown();
            TwitchHelpHinder.Active = false;
          }
          else
          {
            poll.Result.Options.Shuffle<PollOption>();
            PollOption pollOption = poll.Result.Options[0].Votes > poll.Result.Options[1].Votes ? poll.Result.Options[0] : poll.Result.Options[1];
            List<PollOption> options2 = new List<PollOption>();
            if (pollOption.Id == "HELP")
            {
              prompt = LocalizationManager.GetTranslation("UI/Twitch/HH/GoodVictor");
              for (int index = 0; index < 3; ++index)
                options2.Add(new PollOption(helpManipulations[index].ToString(), WorldManipulatorManager.GetLocalisation(helpManipulations[index])));
            }
            else if (pollOption.Id == "HINDER")
            {
              prompt = LocalizationManager.GetTranslation("UI/Twitch/HH/BadVictor");
              for (int index = 0; index < 3; ++index)
                options2.Add(new PollOption(hinderManipulations[index].ToString(), WorldManipulatorManager.GetLocalisation(hinderManipulations[index])));
            }
            StartPollRequest startPollRequest2 = new StartPollRequest(new StartPollRequestType(PollType.HELPHINDEROUTCOME), prompt, (object) new TwitchHelpHinder.HHChoiceMetadata()
            {
              type = pollOption.Id
            }, options2);
            TwitchRequest.EBS_API.StartPollAsync(startPollRequest2, new CancellationToken());
            yield return (object) new WaitForSecondsRealtime((float) TwitchHelpHinder.VotingPhaseDuration);
            poll = TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
            if (poll == null)
            {
              BaseGoopDoor.LockDoor = false;
              BaseGoopDoor.DoorDown();
              TwitchHelpHinder.Active = false;
            }
            else
            {
              timer = 0.0f;
              yield return (object) new WaitUntil((Func<bool>) (() => poll.IsCompleted || (double) (timer += Time.unscaledDeltaTime) > 10.0));
              if (!poll.IsCompleted || poll.Result.Options.Count < 3)
              {
                BaseGoopDoor.LockDoor = false;
                BaseGoopDoor.DoorDown();
                TwitchHelpHinder.Active = false;
              }
              else
              {
                NotificationHelpHinder.CloseNotification();
                TwitchHelpHinder.Active = false;
                TwitchHelpHinder.Timer = 0.0f;
                poll.Result.Options.Shuffle<PollOption>();
                PollOption option = poll.Result.Options[0];
                for (int index = 1; index < poll.Result.Options.Count; ++index)
                {
                  if (poll.Result.Options[index].Votes > option.Votes)
                    option = poll.Result.Options[index];
                }
                WorldManipulatorManager.Manipulations result;
                if (Enum.TryParse<WorldManipulatorManager.Manipulations>(option.Id, out result))
                  WorldManipulatorManager.TriggerManipulation(result, twitch: true);
                BaseGoopDoor.LockDoor = false;
                BaseGoopDoor.DoorDown();
              }
            }
          }
        }
      }
    }
  }

  public static void Update()
  {
    if (!TwitchHelpHinder.Active)
      return;
    TwitchHelpHinder.Timer += Time.unscaledDeltaTime;
  }

  public static void LocationChanged(FollowerLocation location)
  {
    if (!DataManager.Instance.OnboardingFinished)
      return;
    if ((double) DataManager.Instance.TwitchNextHHEvent == -1.0 || TwitchHelpHinder.initialLoad)
      TwitchHelpHinder.SetNextEventTime();
    if (TwitchHelpHinder.Active)
    {
      TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
      TwitchHelpHinder.Active = false;
    }
    TwitchHelpHinder.initialLoad = false;
    if (TwitchHelpHinder.underDoorsCoroutine != null)
    {
      MonoSingleton<TwitchManager>.Instance.StopCoroutine(TwitchHelpHinder.underDoorsCoroutine);
      TwitchHelpHinder.underDoorsCoroutine = (Coroutine) null;
    }
    if (TwitchHelpHinder.Available && !TwitchHelpHinder.Active && location == FollowerLocation.Base)
    {
      TwitchHelpHinder.underDoorsCoroutine = MonoSingleton<TwitchManager>.Instance.StartCoroutine((IEnumerator) TwitchHelpHinder.WaitUntilUnderDoors(new System.Action(TwitchHelpHinder.TriggerEvent)));
      BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
    }
    if (GameManager.IsDungeon(location))
      BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
    else
      BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
  }

  public static IEnumerator WaitUntilUnderDoors(System.Action callback)
  {
    while ((UnityEngine.Object) BaseGoopDoor.MainDoor == (UnityEngine.Object) null || (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while ((UnityEngine.Object) BaseGoopDoor.MainDoor != (UnityEngine.Object) null && (double) BaseGoopDoor.MainDoor.transform.position.y - 2.5 < (double) PlayerFarming.Instance.transform.position.y)
      yield return (object) null;
    TwitchHelpHinder.underDoorsCoroutine = (Coroutine) null;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      System.Action action = callback;
      if (action != null)
        action();
    }
  }

  public static void OnChangedRoom()
  {
    if (!TwitchHelpHinder.Available || TwitchHelpHinder.Active || !((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null) || MapManager.Instance.CurrentNode == null || MapManager.Instance.CurrentNode.nodeType == Map.NodeType.MiniBossFloor || UnityEngine.Random.Range(0, 100) >= 30)
      return;
    TwitchHelpHinder.TriggerEvent();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
  }

  public static void SetNextEventTime()
  {
    DataManager.Instance.TwitchNextHHEvent = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(TwitchHelpHinder.TimeBetweenEvent.x, TwitchHelpHinder.TimeBetweenEvent.y) * 60f;
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(TwitchHelpHinder.OnChangedRoom);
  }

  public static void Abort()
  {
    if (TwitchAuthentication.IsAuthenticated)
      TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
    TwitchHelpHinder.initialLoad = false;
  }

  [Serializable]
  public class HHOption
  {
    public string label;
    public string value;
  }

  [Serializable]
  public struct HHChoiceMetadata
  {
    public string type;
  }

  [Serializable]
  public struct HHMetadata
  {
    public int timer;
  }
}
