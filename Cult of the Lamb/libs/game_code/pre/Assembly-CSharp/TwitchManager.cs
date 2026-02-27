// Decompiled with JetBrains decompiler
// Type: TwitchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TwitchManager : MonoSingleton<TwitchManager>
{
  public const string FOLLOWER_CREATED = "FOLLOWER_CREATED";
  public const string FOLLOWER_RAFFLE_WINNER = "FOLLOWER_RAFFLE_WINNER";
  public const string TOTEM_CONTRIBUTION = "TOTEM_CONTRIBUTION";

  public static string SecretKey
  {
    get => DataManager.Instance.TwitchSecretKey;
    set => DataManager.Instance.TwitchSecretKey = value;
  }

  public static string ChannelID
  {
    get => DataManager.Instance.ChannelID;
    set => DataManager.Instance.ChannelID = value;
  }

  public static string ChannelName
  {
    get => DataManager.Instance.ChannelName;
    set => DataManager.Instance.ChannelName = value;
  }

  public static event TwitchManager.NotificationResponse OnNotificationReceived;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void InitializeTwitchManager()
  {
    GameObject gameObject = new GameObject();
    gameObject.name = "Twitch Manager";
    gameObject.AddComponent<TwitchManager>();
  }

  public static void UpdateEvents()
  {
    if (!TwitchAuthentication.IsAuthenticated)
      return;
    TwitchHelpHinder.Update();
    TwitchTotem.Update();
    TwitchFollowers.Update();
  }

  public static void LocationChanged(FollowerLocation location)
  {
    if (!TwitchAuthentication.IsAuthenticated)
      return;
    TwitchHelpHinder.LocationChanged(location);
  }

  public static void NotificationReceived(string viewerDisplayName, string notificationType)
  {
    TwitchManager.NotificationResponse notificationReceived = TwitchManager.OnNotificationReceived;
    if (notificationReceived == null)
      return;
    notificationReceived(viewerDisplayName, notificationType);
  }

  public static void Abort()
  {
    if (!TwitchAuthentication.IsAuthenticated)
      return;
    TwitchFollowers.Abort();
    TwitchHelpHinder.Abort();
    TwitchTotem.Abort();
    TwitchRequest.Abort();
  }

  public delegate void NotificationResponse(string viewerDisplayName, string notificationType);
}
