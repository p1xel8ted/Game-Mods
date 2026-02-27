// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Client.NullUserReportingPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Unity.Cloud.UserReporting.Client;

public class NullUserReportingPlatform : IUserReportingPlatform
{
  public T DeserializeJson<T>(string json) => default (T);

  public IDictionary<string, string> GetDeviceMetadata()
  {
    return (IDictionary<string, string>) new Dictionary<string, string>();
  }

  public void ModifyUserReport(UserReport userReport)
  {
  }

  public void OnEndOfFrame(UserReportingClient client)
  {
  }

  public void Post(
    string endpoint,
    string contentType,
    byte[] content,
    Action<float, float> progressCallback,
    Action<bool, byte[]> callback)
  {
    progressCallback(1f, 1f);
    callback(true, content);
  }

  public void RunTask(Func<object> task, Action<object> callback) => callback(task());

  public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
  {
  }

  public string SerializeJson(object instance) => string.Empty;

  public void TakeScreenshot(
    int frameNumber,
    int maximumWidth,
    int maximumHeight,
    object source,
    Action<int, byte[]> callback)
  {
    callback(frameNumber, new byte[0]);
  }

  public void Update(UserReportingClient client)
  {
  }
}
