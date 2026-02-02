// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeFakeTwitchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeFakeTwitchManager
{
  public const string _LOG_HEADER = "<color=#754BE7>[FAKE TWITCH]</color>";
  public static FlockadeFakeTwitchManager _instance;
  public FlockadeFakeTwitchManager.Poll _currentPoll;

  public static FlockadeFakeTwitchManager Instance
  {
    get
    {
      return FlockadeFakeTwitchManager._instance ?? (FlockadeFakeTwitchManager._instance = new FlockadeFakeTwitchManager());
    }
  }

  public static FlockadeFakeTwitchManager EBS_API
  {
    get
    {
      return FlockadeFakeTwitchManager._instance ?? (FlockadeFakeTwitchManager._instance = new FlockadeFakeTwitchManager());
    }
  }

  public Task StartPollAsync(
    FlockadeFakeTwitchManager.StartPollRequest startPollRequest)
  {
    this._currentPoll = new FlockadeFakeTwitchManager.Poll(startPollRequest.Type.ActualInstance, startPollRequest.Options);
    FlockadeFakeTwitchManager.ClearConsole();
    Debug.Log((object) $"{"<color=#754BE7>[FAKE TWITCH]</color>"} Starting poll of type {this._currentPoll.Type}. You can vote using the cheat console.");
    Debug.Log((object) ("<color=#754BE7>[FAKE TWITCH]</color> " + startPollRequest.Prompt));
    foreach (FlockadeFakeTwitchManager.PollOption option in this._currentPoll.Options)
      Debug.Log((object) $"<color=#754BE7>[FAKE TWITCH]</color> {option.Label} (cheat: FLOCKADESIMULATETWITCHVOTE_{option.ID})");
    Debug.unityLogger.logEnabled = false;
    return Task.CompletedTask;
  }

  public Task<FlockadeFakeTwitchManager.Poll> EndPollAsync()
  {
    FlockadeFakeTwitchManager.Poll currentPoll = this._currentPoll;
    this._currentPoll = (FlockadeFakeTwitchManager.Poll) null;
    Debug.unityLogger.logEnabled = true;
    return Task.FromResult<FlockadeFakeTwitchManager.Poll>(currentPoll);
  }

  public void SimulateVoteFor(string optionID)
  {
    FlockadeFakeTwitchManager.PollOption pollOption = this._currentPoll?.Options.Find((Predicate<FlockadeFakeTwitchManager.PollOption>) (option => option.ID == optionID));
    if (pollOption == null)
      return;
    ++pollOption.Votes;
  }

  public static void ClearConsole()
  {
  }

  public enum PollType
  {
  }

  public class Poll
  {
    [CompilerGenerated]
    public List<FlockadeFakeTwitchManager.PollOption> \u003COptions\u003Ek__BackingField;
    [CompilerGenerated]
    public FlockadeFakeTwitchManager.PollType \u003CType\u003Ek__BackingField;

    public Poll(
      FlockadeFakeTwitchManager.PollType type,
      List<FlockadeFakeTwitchManager.PollOption> options)
    {
      this.\u003COptions\u003Ek__BackingField = options;
      this.\u003CType\u003Ek__BackingField = type;
    }

    public List<FlockadeFakeTwitchManager.PollOption> Options
    {
      get => this.\u003COptions\u003Ek__BackingField;
    }

    public FlockadeFakeTwitchManager.PollType Type => this.\u003CType\u003Ek__BackingField;
  }

  public class PollOption
  {
    [CompilerGenerated]
    public string \u003CID\u003Ek__BackingField;
    [CompilerGenerated]
    public string \u003CLabel\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CVotes\u003Ek__BackingField;

    public PollOption(string id, string label)
    {
      this.\u003CID\u003Ek__BackingField = id;
      this.\u003CLabel\u003Ek__BackingField = label;
    }

    public string ID => this.\u003CID\u003Ek__BackingField;

    public string Label => this.\u003CLabel\u003Ek__BackingField;

    public int Votes
    {
      get => this.\u003CVotes\u003Ek__BackingField;
      set => this.\u003CVotes\u003Ek__BackingField = value;
    }
  }

  public class StartPollRequest
  {
    [CompilerGenerated]
    public string \u003CPrompt\u003Ek__BackingField;
    [CompilerGenerated]
    public FlockadeFakeTwitchManager.StartPollRequestType \u003CType\u003Ek__BackingField;
    [CompilerGenerated]
    public List<FlockadeFakeTwitchManager.PollOption> \u003COptions\u003Ek__BackingField;

    public StartPollRequest(
      FlockadeFakeTwitchManager.StartPollRequestType type,
      string prompt,
      object metadata,
      List<FlockadeFakeTwitchManager.PollOption> options)
    {
      this.\u003CPrompt\u003Ek__BackingField = prompt;
      this.\u003CType\u003Ek__BackingField = type;
      this.\u003COptions\u003Ek__BackingField = options;
    }

    public string Prompt => this.\u003CPrompt\u003Ek__BackingField;

    public FlockadeFakeTwitchManager.StartPollRequestType Type
    {
      get => this.\u003CType\u003Ek__BackingField;
    }

    public List<FlockadeFakeTwitchManager.PollOption> Options
    {
      get => this.\u003COptions\u003Ek__BackingField;
    }
  }

  public class StartPollRequestType
  {
    [CompilerGenerated]
    public FlockadeFakeTwitchManager.PollType \u003CActualInstance\u003Ek__BackingField;

    public StartPollRequestType(FlockadeFakeTwitchManager.PollType actualInstance)
    {
      this.\u003CActualInstance\u003Ek__BackingField = actualInstance;
    }

    public FlockadeFakeTwitchManager.PollType ActualInstance
    {
      get => this.\u003CActualInstance\u003Ek__BackingField;
    }
  }
}
