// Decompiled with JetBrains decompiler
// Type: TwitchFollowers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class TwitchFollowers
{
  public static bool Active;
  public static bool WaitingForCreation;
  private static bool Request_Active;
  public static TwitchFollowers.RaffleData CurrentData;
  public const string CREATED = "CREATED";
  public const string READY_FOR_CREATION = "READY_FOR_CREATION";
  public const string INTRO = "INTRO";
  public const string SKIN_SELECTION = "SKIN_SELECTION";
  public const string COLOR_SELECTION = "COLOR_SELECTION";
  public const string VARIATION_SELECTION = "VARIATION_SELECTION";
  public static bool Deactivated;
  private static float timer;
  private const float interval = 1f;

  public static event TwitchFollowers.RaffleResponse RaffleUpdated;

  public static event TwitchFollowers.FollowerResponse FollowerCreated;

  public static event TwitchFollowers.FollowerResponse FollowerCreationProgress;

  public static void Update()
  {
    if (TwitchFollowers.Active)
    {
      if (TwitchFollowers.Request_Active || (double) Time.unscaledTime <= (double) TwitchFollowers.timer)
        return;
      TwitchFollowers.Request_Active = true;
      TwitchFollowers.GetRaffle((TwitchFollowers.RaffleResponse) ((response, data) =>
      {
        if (data != null)
        {
          if (TwitchFollowers.Active)
          {
            TwitchFollowers.RaffleResponse raffleUpdated = TwitchFollowers.RaffleUpdated;
            if (raffleUpdated != null)
              raffleUpdated(response, data);
          }
          TwitchFollowers.CurrentData = data;
        }
        TwitchFollowers.Request_Active = false;
      }));
      TwitchFollowers.timer = Time.unscaledTime + 1f;
    }
    else
    {
      if (!TwitchFollowers.WaitingForCreation || TwitchFollowers.Request_Active || (double) Time.unscaledTime <= (double) TwitchFollowers.timer)
        return;
      TwitchFollowers.Request_Active = true;
      TwitchFollowers.GetFollowersAll((TwitchFollowers.FollowerAllResponse) (data =>
      {
        if (TwitchFollowers.WaitingForCreation && TwitchFollowers.CurrentData != null && data != null && data.Length != 0)
        {
          foreach (TwitchFollowers.ViewerFollowerData data1 in data)
          {
            if (data1 != null)
            {
              string str = data1.viewer_id + data1.created_at;
              string createdAt = TwitchFollowers.CurrentData.created_at;
              if (data1.created_at == createdAt)
              {
                if (data1.status == "CREATED" && !DataManager.Instance.TwitchFollowerViewerIDs.Contains(str))
                {
                  TwitchFollowers.FollowerResponse followerCreated = TwitchFollowers.FollowerCreated;
                  if (followerCreated != null)
                    followerCreated(data1);
                }
                else if (data1.status != "CREATED")
                {
                  TwitchFollowers.FollowerResponse creationProgress = TwitchFollowers.FollowerCreationProgress;
                  if (creationProgress != null)
                    creationProgress(data1);
                }
              }
            }
          }
        }
        TwitchFollowers.Request_Active = false;
      }));
      TwitchFollowers.timer = Time.unscaledTime + 1f;
    }
  }

  public static void GetRaffle(TwitchFollowers.RaffleResponse raffleResponse)
  {
    TwitchRequest.Request(TwitchRequest.uri + "followers/raffle", (TwitchRequest.RequestResponse) ((response, result) =>
    {
      if (response == TwitchRequest.ResponseType.Success)
      {
        try
        {
          TwitchFollowers.RaffleData data = JsonUtility.FromJson<TwitchFollowers.RaffleData>(result);
          TwitchFollowers.RaffleResponse raffleResponse1 = raffleResponse;
          if (raffleResponse1 == null)
            return;
          raffleResponse1(response, data);
        }
        catch (Exception ex)
        {
          Debug.Log((object) result);
          TwitchFollowers.RaffleResponse raffleResponse2 = raffleResponse;
          if (raffleResponse2 == null)
            return;
          raffleResponse2(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
        }
      }
      else
      {
        TwitchFollowers.RaffleResponse raffleResponse3 = raffleResponse;
        if (raffleResponse3 == null)
          return;
        raffleResponse3(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
      }
    }), TwitchRequest.RequestType.GET, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void StartRaffle(TwitchFollowers.RaffleResponse raffleResponse)
  {
    TwitchFollowers.Request_Active = false;
    TwitchFollowers.Active = false;
    TwitchRequest.Request(TwitchRequest.uri + "followers/raffle", (TwitchRequest.RequestResponse) ((response, result) =>
    {
      if (response == TwitchRequest.ResponseType.Success)
      {
        TwitchFollowers.Active = true;
        try
        {
          TwitchFollowers.RaffleData data = JsonUtility.FromJson<TwitchFollowers.RaffleData>(result);
          TwitchFollowers.RaffleResponse raffleResponse1 = raffleResponse;
          if (raffleResponse1 == null)
            return;
          raffleResponse1(response, data);
        }
        catch (Exception ex)
        {
          Debug.Log((object) result);
          TwitchFollowers.RaffleResponse raffleResponse2 = raffleResponse;
          if (raffleResponse2 == null)
            return;
          raffleResponse2(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
        }
      }
      else
      {
        TwitchFollowers.RaffleResponse raffleResponse3 = raffleResponse;
        if (raffleResponse3 == null)
          return;
        raffleResponse3(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
      }
    }), TwitchRequest.RequestType.POST, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void EndRaffle(TwitchFollowers.RaffleResponse raffleResponse)
  {
    TwitchFollowers.Active = false;
    TwitchFollowers.Request_Active = false;
    TwitchFollowers.WaitingForCreation = true;
    TwitchRequest.Request(TwitchRequest.uri + "followers/raffle/end", (TwitchRequest.RequestResponse) ((response, result) =>
    {
      if (response == TwitchRequest.ResponseType.Success)
      {
        try
        {
          TwitchFollowers.RaffleData data = JsonUtility.FromJson<TwitchFollowers.RaffleData>(result);
          TwitchFollowers.RaffleResponse raffleResponse1 = raffleResponse;
          if (raffleResponse1 != null)
            raffleResponse1(response, data);
          TwitchFollowers.RaffleResponse raffleUpdated = TwitchFollowers.RaffleUpdated;
          if (raffleUpdated == null)
            return;
          raffleUpdated(response, data);
        }
        catch (Exception ex)
        {
          Debug.Log((object) result);
          TwitchFollowers.RaffleResponse raffleResponse2 = raffleResponse;
          if (raffleResponse2 != null)
            raffleResponse2(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
          TwitchFollowers.RaffleResponse raffleUpdated = TwitchFollowers.RaffleUpdated;
          if (raffleUpdated == null)
            return;
          raffleUpdated(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
        }
      }
      else
      {
        TwitchFollowers.RaffleResponse raffleResponse3 = raffleResponse;
        if (raffleResponse3 == null)
          return;
        raffleResponse3(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
      }
    }), TwitchRequest.RequestType.POST, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void GetFollowerVariations()
  {
    TwitchRequest.Request(TwitchRequest.uri + "followers/variations", (TwitchRequest.RequestResponse) ((response, result) => { }), TwitchRequest.RequestType.GET, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void SetFollowerVariations(List<string> followerNames)
  {
    TwitchFollowers.UnlockedSkinsData unlockedSkinsData = new TwitchFollowers.UnlockedSkinsData();
    unlockedSkinsData.enabled_skin_names = new string[followerNames.Count];
    for (int index = 0; index < followerNames.Count; ++index)
      unlockedSkinsData.enabled_skin_names[index] = followerNames[index];
    TwitchRequest.Request(TwitchRequest.uri + "channel", (TwitchRequest.RequestResponse) ((response, result) => { }), TwitchRequest.RequestType.PATCH, JsonUtility.ToJson((object) unlockedSkinsData), new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void GetFollowersAll(TwitchFollowers.FollowerAllResponse response)
  {
    TwitchRequest.Request(TwitchRequest.uri + "followers", (TwitchRequest.RequestResponse) ((res, result) =>
    {
      if (res != TwitchRequest.ResponseType.Success)
        return;
      string str = result;
      List<string> stringList = new List<string>();
      stringList.Add("");
      for (int index = 0; index < str.Length; ++index)
      {
        if (str[index] != '[')
        {
          if (str[index] == ',' && str[index - 1] == '}' && str[index + 1] == '{')
            stringList.Insert(0, "");
          else if (str[index] != ']')
            stringList[0] += str[index].ToString();
          else
            break;
        }
      }
      TwitchFollowers.ViewerFollowerData[] data = new TwitchFollowers.ViewerFollowerData[stringList.Count];
      for (int index = 0; index < data.Length; ++index)
        data[index] = JsonUtility.FromJson<TwitchFollowers.ViewerFollowerData>(stringList[index]);
      TwitchFollowers.FollowerAllResponse followerAllResponse = response;
      if (followerAllResponse == null)
        return;
      followerAllResponse(data);
    }), TwitchRequest.RequestType.GET, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void WaitingForCreationCancelled() => TwitchFollowers.WaitingForCreation = false;

  public static void Abort() => TwitchFollowers.EndRaffle((TwitchFollowers.RaffleResponse) null);

  [Serializable]
  public class RaffleData
  {
    public long channel_id;
    public int winning_viewer_id;
    public string created_at;
    public string updated_at;
    public int participants;
    public TwitchFollowers.ViewerFollowerData created_follower;
  }

  [Serializable]
  public class ViewerFollowerData
  {
    public long channel_id;
    public string viewer_id;
    public string viewer_display_name;
    public string status;
    public TwitchFollowers.FollowerData customisations;
    public string premium_bits_transaction_id;
    public string created_at;
    public float updated_at;
    public string customisation_step;
    public string recent_chat_message;
    public string id;
    public string save_id;
  }

  [Serializable]
  public class FollowerData
  {
    public string skin_name;
    public TwitchFollowers.ColorData color;
  }

  [Serializable]
  public class ColorData
  {
    public int colorOptionIndex;
    public string HEAD_SKIN_TOP;
    public string HEAD_SKIN_BTM;
    public string ARM_LEFT_SKIN;
    public string ARM_RIGHT_SKIN;
    public string LEG_LEFT_SKIN;
    public string LEG_RIGHT_SKIN;
    public string MARKINGS;
  }

  [Serializable]
  public class UnlockedSkinsData
  {
    public string[] enabled_skin_names;
  }

  public delegate void RaffleResponse(
    TwitchRequest.ResponseType response,
    TwitchFollowers.RaffleData data);

  public delegate void FollowerAllResponse(TwitchFollowers.ViewerFollowerData[] data);

  public delegate void FollowerResponse(TwitchFollowers.ViewerFollowerData data);
}
