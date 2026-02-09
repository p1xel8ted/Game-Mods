// Decompiled with JetBrains decompiler
// Type: TwitchVoting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Org.OpenAPITools.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public static class TwitchVoting
{
  public static bool Active;
  public static bool Deactivated;
  public static bool Request_Active;
  public static float timer;
  public const float interval = 3f;

  public static event TwitchVoting.VotingEvent OnVotingUpdated;

  public static void Initialise() => TwitchVoting.Abort();

  public static void StartVoting(
    TwitchVoting.VotingType reason,
    List<Follower> followers,
    TwitchVoting.VotingReadyResponse votingReadyResponse)
  {
    List<FollowerInfo> followers1 = new List<FollowerInfo>();
    foreach (Follower follower in followers)
      followers1.Add(follower.Brain._directInfoAccess);
    TwitchVoting.StartVoting(reason, followers1, votingReadyResponse);
  }

  public static void StartVoting(
    TwitchVoting.VotingType reason,
    List<FollowerInfo> followers,
    TwitchVoting.VotingReadyResponse votingReadyResponse)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchVoting.StartVotingIE(reason, followers, votingReadyResponse));
  }

  public static IEnumerator StartVotingIE(
    TwitchVoting.VotingType reason,
    List<FollowerInfo> followers,
    TwitchVoting.VotingReadyResponse votingReadyResponse)
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchVoting.Deactivated)
    {
      TwitchVoting.Active = true;
      TwitchVoting.Request_Active = true;
      string translation = LocalizationManager.GetTranslation("UI/Twitch/Voting/LetChatDecide");
      List<PollOption> pollOptionList = new List<PollOption>();
      for (int index = 0; index < followers.Count; ++index)
        pollOptionList.Add(new PollOption(string.IsNullOrEmpty(followers[index].ViewerID) ? followers[index].ID.ToString() : followers[index].ViewerID, followers[index].Name));
      StartPollRequestType type = new StartPollRequestType(PollType.SELECTFOLLOWER);
      string prompt = translation;
      TwitchVoting.VotingMetadata metadata = new TwitchVoting.VotingMetadata();
      metadata.type = reason.ToString();
      List<PollOption> options = pollOptionList;
      StartPollRequest startPollRequest = new StartPollRequest(type, prompt, (object) metadata, options);
      yield return (object) TwitchRequest.EBS_API.StartPollAsync(startPollRequest, new CancellationToken());
      TwitchVoting.VotingReadyResponse votingReadyResponse1 = votingReadyResponse;
      if (votingReadyResponse1 != null)
        votingReadyResponse1(true);
      TwitchVoting.timer = Time.unscaledTime + 3f;
      TwitchVoting.Request_Active = false;
    }
  }

  public static void EndVoting(TwitchVoting.VotingResponse raffleResponse)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchVoting.EndVotingIE(raffleResponse));
  }

  public static IEnumerator EndVotingIE(TwitchVoting.VotingResponse raffleResponse)
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchVoting.Deactivated)
    {
      TwitchVoting.Active = false;
      Task<Poll> poll = TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
      yield return (object) new WaitUntil((Func<bool>) (() => poll.IsCompleted));
      poll.Result.Options.Shuffle<PollOption>();
      PollOption option = poll.Result.Options[0];
      for (int index = 1; index < poll.Result.Options.Count; ++index)
      {
        if (poll.Result.Options[index].Votes > option.Votes)
          option = poll.Result.Options[index];
      }
      FollowerInfo info = FollowerInfo.GetInfoByViewerID(option.Id, true) ?? FollowerInfo.GetInfoByID(int.Parse(option.Id), true);
      TwitchVoting.VotingResponse votingResponse = raffleResponse;
      if (votingResponse != null)
        votingResponse(info != null ? FollowerBrain.GetOrCreateBrain(info) : (FollowerBrain) null);
    }
  }

  public static void GetVoting(Action<int> callback)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchVoting.GetVotingIE(callback));
  }

  public static IEnumerator GetVotingIE(Action<int> callback)
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchVoting.Deactivated)
    {
      TwitchVoting.Request_Active = true;
      Task<Poll> poll = TwitchRequest.EBS_API.GetPollAsync(new CancellationToken());
      yield return (object) new WaitUntil((Func<bool>) (() => poll.IsCompleted));
      Action<int> action = callback;
      if (action != null)
        action(poll.Result.ViewerVotes.Count);
    }
  }

  public static void Update()
  {
    if (TwitchVoting.Request_Active || (double) Time.unscaledTime <= (double) TwitchVoting.timer || !TwitchVoting.Active)
      return;
    TwitchVoting.GetVoting((Action<int>) (participants =>
    {
      TwitchVoting.VotingEvent onVotingUpdated = TwitchVoting.OnVotingUpdated;
      if (onVotingUpdated != null)
        onVotingUpdated(participants);
      TwitchVoting.Request_Active = false;
    }));
    TwitchVoting.timer = Time.unscaledTime + 3f;
  }

  public static void Abort()
  {
    if (TwitchAuthentication.IsAuthenticated)
      TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
    TwitchVoting.Active = false;
  }

  [Serializable]
  public class VotingData_Result
  {
    public string selected_follower;
  }

  [Serializable]
  public class VotingData_Send
  {
    public string reason;
    public string[] allowed_follower_ids;
  }

  [Serializable]
  public class Voting_Update
  {
    public int amount_of_votes;
  }

  [Serializable]
  public class VotingMetadata
  {
    public string type;
  }

  public enum VotingType
  {
    DEMON,
    PRISON,
    MISSIONARY,
    CONFESSION,
    FOLLOWER_TO_GAIN_XP,
    HEALING_BAY,
    SACRIFICE_TO_NIGHT_FOX,
    SARIFICE_TO_MIDAS,
    RITUAL_ASCEND,
    RITUAL_ENFORCER,
    RITUAL_FIGHT_PIT,
    RITUAL_FUNERAL,
    RITUAL_RESURRECT,
    RITUAL_MARRY,
    RITUAL_SACRIFICE,
    BED,
    SACRIFICE_TO_MIDAS,
    SACRIFICE_TO_DOOR,
    RITUAL_BECOMEDISCIPLE,
    ASSIGN_CLOTHING,
    MATING,
    DRINK,
    RITUAL_CANNIBAL,
    RITUAL_PURGE,
    RITUAL_NUDISM,
    RITUAL_ATONE,
    DRUM_CIRCLE,
    REAP_SOULS,
    DAYCARE,
    KNUCKLEBONES,
    RITUAL_DIVORCE,
    RITUAL_FOLLOWERWEDDING,
    TRAIT_MANIPULATOR,
    VOLCANIC_SPA,
  }

  public delegate void VotingReadyResponse(bool ready);

  public delegate void VotingResponse(FollowerBrain result);

  public delegate void VotingEvent(int totalVotes);
}
