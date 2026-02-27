// Decompiled with JetBrains decompiler
// Type: TwitchTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class TwitchTotem
{
  private static bool Request_Active;
  public const int ContributionTarget = 10;
  public static int CurrentContributions;
  private static float heartbeatTimestamp;
  public static bool Deactivated;

  public static event TwitchTotem.TotemResponse TotemUpdated;

  public static bool TotemUnlockAvailable => TwitchTotem.Contributions >= 10;

  public static int Contributions
  {
    get => TwitchTotem.CurrentContributions - 10 * TwitchTotem.TwitchTotemsCompleted;
  }

  public static int TwitchTotemsCompleted
  {
    get => DataManager.Instance.TwitchTotemsCompleted;
    set => DataManager.Instance.TwitchTotemsCompleted = value;
  }

  public static void Initialise()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchTotem.TwitchRequest_OnSocketReceived);
    TwitchRequest.OnSocketReceived += new TwitchRequest.SocketResponse(TwitchTotem.TwitchRequest_OnSocketReceived);
    TwitchTotem.GetTotemStatus();
  }

  public static void GetTotemStatus()
  {
    if (TwitchTotem.Deactivated)
      return;
    TwitchRequest.Request(TwitchRequest.uri + "totem", (TwitchRequest.RequestResponse) ((response, result) =>
    {
      try
      {
        TwitchTotem.CurrentContributions = JsonUtility.FromJson<TwitchTotem.TotemData>(result).contributions;
        TwitchTotem.TotemResponse totemUpdated = TwitchTotem.TotemUpdated;
        if (totemUpdated == null)
          return;
        totemUpdated(TwitchTotem.CurrentContributions);
      }
      catch (Exception ex)
      {
      }
    }), TwitchRequest.RequestType.GET, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  private static void TwitchRequest_OnSocketReceived(string key, string data)
  {
    if (!(key == "notifications.add"))
      return;
    if (TwitchTotem.Deactivated)
      return;
    try
    {
      TwitchTotem.TotemData totemData = JsonUtility.FromJson<TwitchTotem.TotemData>(data);
      TwitchTotem.CurrentContributions = totemData.metadata.totem.contributions;
      TwitchTotem.TotemResponse totemUpdated = TwitchTotem.TotemUpdated;
      if (totemUpdated != null)
        totemUpdated(TwitchTotem.CurrentContributions);
      TwitchManager.NotificationReceived(totemData.metadata.viewer_display_name, "TOTEM_CONTRIBUTION");
    }
    catch (Exception ex)
    {
    }
  }

  public static void Update()
  {
    if (TwitchTotem.Request_Active || (double) Time.unscaledTime <= (double) TwitchTotem.heartbeatTimestamp)
      return;
    TwitchTotem.Request_Active = true;
    TwitchTotem.heartbeatTimestamp = Time.unscaledTime + 300f;
    TwitchRequest.Request(TwitchRequest.uri + "channel/heartbeat", (TwitchRequest.RequestResponse) ((response, result) => TwitchTotem.Request_Active = false), TwitchRequest.RequestType.POST, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void SetNotificationRead(int notificationID)
  {
    TwitchRequest.Request(TwitchRequest.uri + "channel/notifications/read", (TwitchRequest.RequestResponse) ((response, result) => { }), TwitchRequest.RequestType.POST, JsonUtility.ToJson((object) new TwitchTotem.NotificationsReadData()
    {
      notification_ids = new int[1]{ notificationID }
    }), new KeyValuePair<string, string>("x-cotl-debug-twitch-channel-id", TwitchManager.ChannelID));
  }

  public static void TotemRewardClaimed()
  {
    ++TwitchTotem.TwitchTotemsCompleted;
    TwitchTotem.TotemResponse totemUpdated = TwitchTotem.TotemUpdated;
    if (totemUpdated == null)
      return;
    totemUpdated(TwitchTotem.CurrentContributions);
  }

  public static void Abort()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchTotem.TwitchRequest_OnSocketReceived);
  }

  [Serializable]
  public class TotemData
  {
    public long channel_id;
    public string save_id;
    public string type;
    public string message;
    public int contributions;
    public TwitchTotem.MetaData metadata;
  }

  [Serializable]
  public class MetaData
  {
    public string viewer_id;
    public string viewer_display_name;
    public TwitchTotem.TotemMetaData totem;
  }

  [Serializable]
  public class TotemMetaData
  {
    public long channel_id;
    public int contributions;
    public string save_id;
    public string created_at;
  }

  [Serializable]
  public class NotificationsReadData
  {
    public int[] notification_ids;
  }

  public delegate void TotemResponse(int contributions);
}
