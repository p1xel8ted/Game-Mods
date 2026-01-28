// Decompiled with JetBrains decompiler
// Type: TwitchMessages
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class TwitchMessages
{
  public static List<TwitchMessages.MessageData_Send> queuedMessages = new List<TwitchMessages.MessageData_Send>();
  public static float timeSinceLastShownMessage;
  public const float timeBetweenMessages = 30f;
  public static bool Deactivated = false;

  public static List<string> Read_Messages => DataManager.Instance.ReadTwitchMessages;

  public static void Initialise()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchMessages.TwitchRequest_OnSocketReceived);
    TwitchRequest.OnSocketReceived += new TwitchRequest.SocketResponse(TwitchMessages.TwitchRequest_OnSocketReceived);
  }

  public static void TwitchRequest_OnSocketReceived(string key, string data)
  {
    if (!(key == "channelPoints.redemption"))
      return;
    try
    {
      TwitchMessages.MessageData_Send messageDataSend = JsonUtility.FromJson<TwitchMessages.MessageData_Send>(data);
      if (!(messageDataSend.type == "FOLLOWER_MESSAGE"))
        return;
      TwitchMessages.queuedMessages.Add(messageDataSend);
    }
    catch (Exception ex)
    {
    }
  }

  public static void Update()
  {
    if (PlayerFarming.Location != FollowerLocation.Base || (double) Time.time <= (double) TwitchMessages.timeSinceLastShownMessage || TwitchMessages.queuedMessages.Count <= 0 || !DataManager.Instance.TwitchSettings.TwitchMessagesEnabled || TwitchMessages.Deactivated)
      return;
    TwitchMessages.timeSinceLastShownMessage = Time.time + 30f;
    if (TwitchMessages.queuedMessages[TwitchMessages.queuedMessages.Count - 1] != null)
    {
      TwitchMessages.MessageData_Send queuedMessage = TwitchMessages.queuedMessages[TwitchMessages.queuedMessages.Count - 1];
      Follower followerByViewerId = FollowerManager.FindFollowerByViewerID(queuedMessage.user_id.ToString());
      if ((UnityEngine.Object) followerByViewerId != (UnityEngine.Object) null)
        followerByViewerId.ShowBarkMessage(queuedMessage.user_input);
      TwitchMessages.Read_Messages.Add(queuedMessage.user_input + queuedMessage.user_name);
      TwitchMessages.queuedMessages.Remove(queuedMessage);
    }
    else
      TwitchMessages.queuedMessages.RemoveAt(TwitchMessages.queuedMessages.Count - 1);
  }

  public static void Abort()
  {
    TwitchRequest.OnSocketReceived -= new TwitchRequest.SocketResponse(TwitchMessages.TwitchRequest_OnSocketReceived);
  }

  [Serializable]
  public class MessageData_Send
  {
    public string type;
    public string user_input;
    public int user_id;
    public string user_name;
  }
}
