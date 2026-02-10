// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Client.IUserReportingPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Unity.Cloud.UserReporting.Client;

public interface IUserReportingPlatform
{
  T DeserializeJson<T>(string json);

  IDictionary<string, string> GetDeviceMetadata();

  void ModifyUserReport(UserReport userReport);

  void OnEndOfFrame(UserReportingClient client);

  void Post(
    string endpoint,
    string contentType,
    byte[] content,
    Action<float, float> progressCallback,
    Action<bool, byte[]> callback);

  void RunTask(Func<object> task, Action<object> callback);

  void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData);

  string SerializeJson(object instance);

  void TakeScreenshot(
    int frameNumber,
    int maximumWidth,
    int maximumHeight,
    object source,
    Action<int, byte[]> callback);

  void Update(UserReportingClient client);
}
