// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Client.IUserReportingPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
