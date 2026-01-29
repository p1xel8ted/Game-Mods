// Decompiled with JetBrains decompiler
// Type: AnalyticsLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using Unify;
using UnityEngine;

#nullable disable
public static class AnalyticsLogger
{
  public const string GoogleFormsURL = "https://script.google.com/macros/s/AKfycbx1xvIf12lQNf4noLtiQmVZUoSlbARAX2zO5RwC3_GK57YxW6yLs-9WKN3Au1On9aFTog/exec";

  public static void LogEvent(
    AnalyticsLogger.EventType eventType,
    string value1,
    string value2,
    string value3,
    string value4)
  {
  }

  public static IEnumerator SendEvent(
    AnalyticsLogger.EventType eventType,
    string value1,
    string value2,
    string value3,
    string value4)
  {
    yield return (object) null;
  }

  public enum EventType
  {
    None,
    Deaths,
    CrusadeResults,
    NewDay,
  }

  [Serializable]
  public class AnalyticsEvent
  {
    public string SteamUserName;
    public string gameDay;
    public string eventType;
    public string value1;
    public string value2;
    public string value3;
    public string value4;
    public string playerId;

    public AnalyticsEvent(
      AnalyticsLogger.EventType eventType,
      string value1,
      string value2,
      string value3,
      string value4)
    {
      Debug.Log((object) $"NEW EVENT {eventType.ToString()} {value1} {value2} {value3} {value4}");
      this.SteamUserName = SessionManager.GetSessionOwner().nickName.ToString();
      this.gameDay = TimeManager.CurrentDay.ToString();
      this.eventType = eventType.ToString();
      this.value1 = value1;
      this.value2 = value2;
      this.value3 = value3;
      this.value4 = value4;
      this.playerId = SystemInfo.deviceUniqueIdentifier;
    }
  }
}
