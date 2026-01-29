// Decompiled with JetBrains decompiler
// Type: TwitchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Org.OpenAPITools.Model;
using System;
using System.Threading;
using UnityEngine;

#nullable disable
public class TwitchManager : MonoSingleton<TwitchManager>
{
  public static Action<bool> OnHelpHinderEnabledChanged;
  public static Action<float> OnHelpHinderFrequencyChanged;
  public static Action<bool> OnTotemEnabledChanged;
  public static Action<bool> OnFollowerNamesEnabledChanged;
  public static Action<bool> TwitchMessagesEnabledChanged;
  public const string FOLLOWER_CREATED = "FOLLOWER_CREATED";
  public const string FOLLOWER_RAFFLE_WINNER = "FOLLOWER_RAFFLE_WINNER";
  public const string TOTEM_CONTRIBUTION = "TOTEM_CONTRIBUTION";

  public static string Token
  {
    get => DataManager.Instance.TwitchToken;
    set => DataManager.Instance.TwitchToken = value;
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

  public static bool HelpHinderEnabled
  {
    get => DataManager.Instance.TwitchSettings.HelpHinderEnabled;
    set
    {
      if (DataManager.Instance.TwitchSettings.HelpHinderEnabled == value)
        return;
      Debug.Log((object) $"Twitch Settings - Help/Hinder value changed to {value}".Colour(Color.yellow));
      DataManager.Instance.TwitchSettings.HelpHinderEnabled = value;
      Action<bool> hinderEnabledChanged = TwitchManager.OnHelpHinderEnabledChanged;
      if (hinderEnabledChanged == null)
        return;
      hinderEnabledChanged(value);
    }
  }

  public static float HelpHinderFrequency
  {
    get => DataManager.Instance.TwitchSettings.HelpHinderFrequency;
    set
    {
      if (DataManager.Instance.TwitchSettings.HelpHinderFrequency.Equals(value))
        return;
      Debug.Log((object) $"Twitch Settings - Help/Hinder Frequency value changed to {value}".Colour(Color.yellow));
      DataManager.Instance.TwitchSettings.HelpHinderFrequency = value;
      Action<float> frequencyChanged = TwitchManager.OnHelpHinderFrequencyChanged;
      if (frequencyChanged == null)
        return;
      frequencyChanged(value);
    }
  }

  public static bool TotemEnabled
  {
    get => DataManager.Instance.TwitchSettings.TotemEnabled;
    set
    {
      if (DataManager.Instance.TwitchSettings.TotemEnabled == value)
        return;
      Debug.Log((object) $"Twitch Settings - Totem Enabled value changed to {value}".Colour(Color.yellow));
      DataManager.Instance.TwitchSettings.TotemEnabled = value;
      Action<bool> totemEnabledChanged = TwitchManager.OnTotemEnabledChanged;
      if (totemEnabledChanged == null)
        return;
      totemEnabledChanged(value);
    }
  }

  public static bool FollowerNamesEnabled
  {
    get => DataManager.Instance.TwitchSettings.FollowerNamesEnabled;
    set
    {
      if (DataManager.Instance.TwitchSettings.FollowerNamesEnabled == value)
        return;
      Debug.Log((object) $"Twitch Settings - Show Twitch Follower Names value changed to {value}".Colour(Color.yellow));
      DataManager.Instance.TwitchSettings.FollowerNamesEnabled = value;
      Action<bool> namesEnabledChanged = TwitchManager.OnFollowerNamesEnabledChanged;
      if (namesEnabledChanged == null)
        return;
      namesEnabledChanged(value);
    }
  }

  public static bool MessagesEnabled
  {
    get => DataManager.Instance.TwitchSettings.TwitchMessagesEnabled;
    set
    {
      if (DataManager.Instance.TwitchSettings.TwitchMessagesEnabled == value)
        return;
      Debug.Log((object) $"Twitch Settings - Twitch Messages value changed to {value}".Colour(Color.yellow));
      DataManager.Instance.TwitchSettings.TwitchMessagesEnabled = value;
      Action<bool> messagesEnabledChanged = TwitchManager.TwitchMessagesEnabledChanged;
      if (messagesEnabledChanged == null)
        return;
      messagesEnabledChanged(value);
    }
  }

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
    TwitchMessages.Update();
    TwitchVoting.Update();
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

  public static void SetLanguage(string languageCode)
  {
    if (!TwitchAuthentication.IsAuthenticated)
      return;
    languageCode = languageCode.Replace(languageCode[0], char.ToUpper(languageCode[0]));
    languageCode = languageCode.Replace("-", "");
    SettingsGameData settingsGameData = new SettingsGameData((Org.OpenAPITools.Model.Language) Enum.Parse(typeof (Org.OpenAPITools.Model.Language), languageCode));
    TwitchRequest.DEFAULT_API.UpdateSettingsGameDataAsync(settingsGameData, new CancellationToken());
  }

  public static void Abort()
  {
    if (!TwitchAuthentication.IsAuthenticated)
      return;
    TwitchRequest.Abort();
    TwitchVoting.Abort();
    TwitchFollowers.Abort();
    TwitchHelpHinder.Abort();
    TwitchTotem.Abort();
    TwitchMessages.Abort();
  }

  public delegate void NotificationResponse(string viewerDisplayName, string notificationType);
}
