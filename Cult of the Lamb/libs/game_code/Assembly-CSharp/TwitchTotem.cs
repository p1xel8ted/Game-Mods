// Decompiled with JetBrains decompiler
// Type: TwitchTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class TwitchTotem
{
  public static bool Request_Active;
  public const int ContributionTarget = 10;
  public static float heartbeatTimestamp;
  public static bool Disabled;

  public static event TwitchTotem.TotemResponse TotemUpdated;

  public static bool TotemUnlockAvailable => TwitchTotem.Contributions >= 10;

  public static int Contributions
  {
    get => DataManager.Instance.TotemContributions;
    set => DataManager.Instance.TotemContributions = value;
  }

  public static bool Deactivated
  {
    get => TwitchTotem.Disabled || !DataManager.Instance.TwitchSettings.TotemEnabled;
  }

  public static void Initialise()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchTotem.TwitchRequest_OnSocketReceived);
    TwitchRequest.OnSocketReceived += new TwitchRequest.SocketResponse(TwitchTotem.TwitchRequest_OnSocketReceived);
  }

  public static void TwitchRequest_OnSocketReceived(string key, string data)
  {
    if (!(key == "channelPoints.redemption"))
      return;
    if (TwitchTotem.Deactivated)
      return;
    try
    {
      TwitchTotem.TotemData totemData = JsonUtility.FromJson<TwitchTotem.TotemData>(data);
      if (!(totemData.type == "TOTEM"))
        return;
      ++TwitchTotem.Contributions;
      TwitchTotem.TotemResponse totemUpdated = TwitchTotem.TotemUpdated;
      if (totemUpdated != null)
        totemUpdated(TwitchTotem.Contributions);
      TwitchManager.NotificationReceived(totemData.user_name, "TOTEM_CONTRIBUTION");
    }
    catch (Exception ex)
    {
    }
  }

  public static void Update()
  {
  }

  public static void TotemRewardClaimed()
  {
    TwitchTotem.Contributions = Mathf.Clamp(TwitchTotem.Contributions - 10, 0, int.MaxValue);
    TwitchTotem.TotemResponse totemUpdated = TwitchTotem.TotemUpdated;
    if (totemUpdated == null)
      return;
    totemUpdated(TwitchTotem.Contributions);
  }

  public static void Abort()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchTotem.TwitchRequest_OnSocketReceived);
  }

  [Serializable]
  public class TotemData
  {
    public string type;
    public string user_input;
    public int user_id;
    public string user_name;
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
