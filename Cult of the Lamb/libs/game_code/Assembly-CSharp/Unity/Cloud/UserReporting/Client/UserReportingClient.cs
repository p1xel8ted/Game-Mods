// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Client.UserReportingClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace Unity.Cloud.UserReporting.Client;

public class UserReportingClient
{
  public Dictionary<string, UserReportMetric> clientMetrics;
  public Dictionary<string, string> currentMeasureMetadata;
  public Dictionary<string, UserReportMetric> currentMetrics;
  public List<Action> currentSynchronizedActions;
  public List<UserReportNamedValue> deviceMetadata;
  public CyclicalList<UserReportEvent> events;
  public int frameNumber;
  public bool isMeasureBoundary;
  public int measureFrames;
  public CyclicalList<UserReportMeasure> measures;
  public CyclicalList<UserReportScreenshot> screenshots;
  public int screenshotsSaved;
  public int screenshotsTaken;
  public List<Action> synchronizedActions;
  public Stopwatch updateStopwatch;
  [CompilerGenerated]
  public UserReportingClientConfiguration \u003CConfiguration\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CEndpoint\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsConnectedToLogger\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsSelfReporting\u003Ek__BackingField;
  [CompilerGenerated]
  public IUserReportingPlatform \u003CPlatform\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CProjectIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CSendEventsToAnalytics\u003Ek__BackingField;

  public UserReportingClient(
    string endpoint,
    string projectIdentifier,
    IUserReportingPlatform platform,
    UserReportingClientConfiguration configuration)
  {
    this.Endpoint = endpoint;
    this.ProjectIdentifier = projectIdentifier;
    this.Platform = platform;
    this.Configuration = configuration;
    this.Configuration.FramesPerMeasure = this.Configuration.FramesPerMeasure > 0 ? this.Configuration.FramesPerMeasure : 1;
    this.Configuration.MaximumEventCount = this.Configuration.MaximumEventCount > 0 ? this.Configuration.MaximumEventCount : 1;
    this.Configuration.MaximumMeasureCount = this.Configuration.MaximumMeasureCount > 0 ? this.Configuration.MaximumMeasureCount : 1;
    this.Configuration.MaximumScreenshotCount = this.Configuration.MaximumScreenshotCount > 0 ? this.Configuration.MaximumScreenshotCount : 1;
    this.clientMetrics = new Dictionary<string, UserReportMetric>();
    this.currentMeasureMetadata = new Dictionary<string, string>();
    this.currentMetrics = new Dictionary<string, UserReportMetric>();
    this.events = new CyclicalList<UserReportEvent>(configuration.MaximumEventCount);
    this.measures = new CyclicalList<UserReportMeasure>(configuration.MaximumMeasureCount);
    this.screenshots = new CyclicalList<UserReportScreenshot>(configuration.MaximumScreenshotCount);
    this.deviceMetadata = new List<UserReportNamedValue>();
    foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.Platform.GetDeviceMetadata())
      this.AddDeviceMetadata(keyValuePair.Key, keyValuePair.Value);
    this.AddDeviceMetadata("UserReportingClientVersion", "2.0");
    this.synchronizedActions = new List<Action>();
    this.currentSynchronizedActions = new List<Action>();
    this.updateStopwatch = new Stopwatch();
    this.IsConnectedToLogger = true;
  }

  public UserReportingClientConfiguration Configuration
  {
    get => this.\u003CConfiguration\u003Ek__BackingField;
    set => this.\u003CConfiguration\u003Ek__BackingField = value;
  }

  public string Endpoint
  {
    get => this.\u003CEndpoint\u003Ek__BackingField;
    set => this.\u003CEndpoint\u003Ek__BackingField = value;
  }

  public bool IsConnectedToLogger
  {
    get => this.\u003CIsConnectedToLogger\u003Ek__BackingField;
    set => this.\u003CIsConnectedToLogger\u003Ek__BackingField = value;
  }

  public bool IsSelfReporting
  {
    get => this.\u003CIsSelfReporting\u003Ek__BackingField;
    set => this.\u003CIsSelfReporting\u003Ek__BackingField = value;
  }

  public IUserReportingPlatform Platform
  {
    get => this.\u003CPlatform\u003Ek__BackingField;
    set => this.\u003CPlatform\u003Ek__BackingField = value;
  }

  public string ProjectIdentifier
  {
    get => this.\u003CProjectIdentifier\u003Ek__BackingField;
    set => this.\u003CProjectIdentifier\u003Ek__BackingField = value;
  }

  public bool SendEventsToAnalytics
  {
    get => this.\u003CSendEventsToAnalytics\u003Ek__BackingField;
    set => this.\u003CSendEventsToAnalytics\u003Ek__BackingField = value;
  }

  public void AddDeviceMetadata(string name, string value)
  {
    lock (this.deviceMetadata)
      this.deviceMetadata.Add(new UserReportNamedValue()
      {
        Name = name,
        Value = value
      });
  }

  public void AddMeasureMetadata(string name, string value)
  {
    if (this.currentMeasureMetadata.ContainsKey(name))
      this.currentMeasureMetadata[name] = value;
    else
      this.currentMeasureMetadata.Add(name, value);
  }

  public void ClearScreenshots()
  {
    lock (this.screenshots)
      this.screenshots.Clear();
  }

  public void CreateUserReport(Action<UserReport> callback)
  {
    this.LogEvent(UserReportEventLevel.Info, "Creating user report.");
    this.WaitForPerforation(this.screenshotsTaken, (Action) (() => this.Platform.RunTask((Func<object>) (() =>
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      UserReport userReport = new UserReport();
      userReport.ProjectIdentifier = this.ProjectIdentifier;
      lock (this.deviceMetadata)
        userReport.DeviceMetadata = this.deviceMetadata.ToList<UserReportNamedValue>();
      lock (this.events)
        userReport.Events = this.events.ToList<UserReportEvent>();
      lock (this.measures)
        userReport.Measures = this.measures.ToList<UserReportMeasure>();
      lock (this.screenshots)
        userReport.Screenshots = this.screenshots.ToList<UserReportScreenshot>();
      userReport.Complete();
      this.Platform.ModifyUserReport(userReport);
      stopwatch.Stop();
      this.SampleClientMetric("UserReportingClient.CreateUserReport.Task", (double) stopwatch.ElapsedMilliseconds);
      foreach (KeyValuePair<string, UserReportMetric> clientMetric in this.clientMetrics)
        userReport.ClientMetrics.Add(clientMetric.Value);
      return (object) userReport;
    }), (Action<object>) (result => callback(result as UserReport)))));
  }

  public string GetEndpoint() => this.Endpoint == null ? "https://localhost" : this.Endpoint.Trim();

  public void LogEvent(UserReportEventLevel level, string message)
  {
    this.LogEvent(level, message, (string) null, (Exception) null);
  }

  public void LogEvent(UserReportEventLevel level, string message, string stackTrace)
  {
    this.LogEvent(level, message, stackTrace, (Exception) null);
  }

  public void LogEvent(
    UserReportEventLevel level,
    string message,
    string stackTrace,
    Exception exception)
  {
    lock (this.events)
    {
      UserReportEvent userReportEvent = new UserReportEvent();
      userReportEvent.Level = level;
      userReportEvent.Message = message;
      userReportEvent.FrameNumber = this.frameNumber;
      userReportEvent.StackTrace = stackTrace;
      userReportEvent.Timestamp = DateTime.UtcNow;
      if (exception != null)
        userReportEvent.Exception = new SerializableException(exception);
      this.events.Add(userReportEvent);
    }
  }

  public void LogException(Exception exception)
  {
    this.LogEvent(UserReportEventLevel.Error, (string) null, (string) null, exception);
  }

  public void SampleClientMetric(string name, double value)
  {
    if (double.IsInfinity(value) || double.IsNaN(value))
      return;
    if (!this.clientMetrics.ContainsKey(name))
      this.clientMetrics.Add(name, new UserReportMetric()
      {
        Name = name
      });
    UserReportMetric clientMetric = this.clientMetrics[name];
    clientMetric.Sample(value);
    this.clientMetrics[name] = clientMetric;
    if (!this.IsSelfReporting)
      return;
    this.SampleMetric(name, value);
  }

  public void SampleMetric(string name, double value)
  {
    if (this.Configuration.MetricsGatheringMode == MetricsGatheringMode.Disabled || double.IsInfinity(value) || double.IsNaN(value))
      return;
    if (!this.currentMetrics.ContainsKey(name))
      this.currentMetrics.Add(name, new UserReportMetric()
      {
        Name = name
      });
    UserReportMetric currentMetric = this.currentMetrics[name];
    currentMetric.Sample(value);
    this.currentMetrics[name] = currentMetric;
  }

  public void SaveUserReportToDisk(UserReport userReport)
  {
    this.LogEvent(UserReportEventLevel.Info, "Saving user report to disk.");
    File.WriteAllText("UserReport.json", this.Platform.SerializeJson((object) userReport));
  }

  public void SendUserReport(UserReport userReport, Action<bool, UserReport> callback)
  {
    this.SendUserReport(userReport, (Action<float, float>) null, callback);
  }

  public void SendUserReport(
    UserReport userReport,
    Action<float, float> progressCallback,
    Action<bool, UserReport> callback)
  {
    try
    {
      if (userReport == null)
        return;
      if (userReport.Identifier != null)
        this.LogEvent(UserReportEventLevel.Warning, "Identifier cannot be set on the client side. The value provided was discarded.");
      else if (userReport.ContentLength != 0L)
        this.LogEvent(UserReportEventLevel.Warning, "ContentLength cannot be set on the client side. The value provided was discarded.");
      else if (userReport.ReceivedOn != new DateTime())
        this.LogEvent(UserReportEventLevel.Warning, "ReceivedOn cannot be set on the client side. The value provided was discarded.");
      else if (userReport.ExpiresOn != new DateTime())
      {
        this.LogEvent(UserReportEventLevel.Warning, "ExpiresOn cannot be set on the client side. The value provided was discarded.");
      }
      else
      {
        this.LogEvent(UserReportEventLevel.Info, "Sending user report.");
        byte[] bytes = Encoding.UTF8.GetBytes(this.Platform.SerializeJson((object) userReport));
        this.Platform.Post(string.Format($"{this.GetEndpoint()}/api/userreporting"), "application/json", bytes, (Action<float, float>) ((uploadProgress, downloadProgress) =>
        {
          if (progressCallback == null)
            return;
          progressCallback(uploadProgress, downloadProgress);
        }), (Action<bool, byte[]>) ((success, result) => this.synchronizedActions.Add((Action) (() =>
        {
          if (success)
          {
            try
            {
              UserReport userReport1 = this.Platform.DeserializeJson<UserReport>(Encoding.UTF8.GetString(result));
              if (userReport1 != null)
              {
                if (this.SendEventsToAnalytics)
                  this.Platform.SendAnalyticsEvent("UserReportingClient.SendUserReport", new Dictionary<string, object>()
                  {
                    {
                      "UserReportIdentifier",
                      (object) userReport.Identifier
                    }
                  });
                callback(success, userReport1);
              }
              else
                callback(false, (UserReport) null);
            }
            catch (Exception ex)
            {
              this.LogEvent(UserReportEventLevel.Error, $"Sending user report failed: {ex.ToString()}");
              callback(false, (UserReport) null);
            }
          }
          else
          {
            this.LogEvent(UserReportEventLevel.Error, "Sending user report failed.");
            callback(false, (UserReport) null);
          }
        }))));
      }
    }
    catch (Exception ex)
    {
      this.LogEvent(UserReportEventLevel.Error, $"Sending user report failed: {ex.ToString()}");
      callback(false, (UserReport) null);
    }
  }

  public void TakeScreenshot(
    int maximumWidth,
    int maximumHeight,
    Action<UserReportScreenshot> callback)
  {
    this.TakeScreenshotFromSource(maximumWidth, maximumHeight, (object) null, callback);
  }

  public void TakeScreenshotFromSource(
    int maximumWidth,
    int maximumHeight,
    object source,
    Action<UserReportScreenshot> callback)
  {
    this.LogEvent(UserReportEventLevel.Info, "Taking screenshot.");
    ++this.screenshotsTaken;
    this.Platform.TakeScreenshot(this.frameNumber, maximumWidth, maximumHeight, source, (Action<int, byte[]>) ((passedFrameNumber, data) => this.synchronizedActions.Add((Action) (() =>
    {
      lock (this.screenshots)
      {
        UserReportScreenshot reportScreenshot = new UserReportScreenshot();
        reportScreenshot.FrameNumber = passedFrameNumber;
        reportScreenshot.DataBase64 = Convert.ToBase64String(data);
        this.screenshots.Add(reportScreenshot);
        ++this.screenshotsSaved;
        callback(reportScreenshot);
      }
    }))));
  }

  public void Update()
  {
    this.updateStopwatch.Reset();
    this.updateStopwatch.Start();
    this.Platform.Update(this);
    if (this.Configuration.MetricsGatheringMode != MetricsGatheringMode.Disabled)
    {
      this.isMeasureBoundary = false;
      int framesPerMeasure = this.Configuration.FramesPerMeasure;
      if (this.measureFrames >= framesPerMeasure)
      {
        lock (this.measures)
        {
          UserReportMeasure userReportMeasure = new UserReportMeasure();
          userReportMeasure.StartFrameNumber = this.frameNumber - framesPerMeasure;
          userReportMeasure.EndFrameNumber = this.frameNumber - 1;
          UserReportMeasure nextEviction = this.measures.GetNextEviction();
          if (nextEviction.Metrics != null)
          {
            userReportMeasure.Metadata = nextEviction.Metadata;
            userReportMeasure.Metrics = nextEviction.Metrics;
          }
          else
          {
            userReportMeasure.Metadata = new List<UserReportNamedValue>();
            userReportMeasure.Metrics = new List<UserReportMetric>();
          }
          userReportMeasure.Metadata.Clear();
          userReportMeasure.Metrics.Clear();
          foreach (KeyValuePair<string, string> keyValuePair in this.currentMeasureMetadata)
            userReportMeasure.Metadata.Add(new UserReportNamedValue()
            {
              Name = keyValuePair.Key,
              Value = keyValuePair.Value
            });
          foreach (KeyValuePair<string, UserReportMetric> currentMetric in this.currentMetrics)
            userReportMeasure.Metrics.Add(currentMetric.Value);
          this.currentMetrics.Clear();
          this.measures.Add(userReportMeasure);
          this.measureFrames = 0;
          this.isMeasureBoundary = true;
        }
      }
      ++this.measureFrames;
    }
    else
      this.isMeasureBoundary = true;
    foreach (Action synchronizedAction in this.synchronizedActions)
      this.currentSynchronizedActions.Add(synchronizedAction);
    this.synchronizedActions.Clear();
    foreach (Action synchronizedAction in this.currentSynchronizedActions)
      synchronizedAction();
    this.currentSynchronizedActions.Clear();
    ++this.frameNumber;
    this.updateStopwatch.Stop();
    this.SampleClientMetric("UserReportingClient.Update", (double) this.updateStopwatch.ElapsedMilliseconds);
  }

  public void UpdateOnEndOfFrame()
  {
    this.updateStopwatch.Reset();
    this.updateStopwatch.Start();
    this.Platform.OnEndOfFrame(this);
    this.updateStopwatch.Stop();
    this.SampleClientMetric("UserReportingClient.UpdateOnEndOfFrame", (double) this.updateStopwatch.ElapsedMilliseconds);
  }

  public void WaitForPerforation(int currentScreenshotsTaken, Action callback)
  {
    if (this.screenshotsSaved >= currentScreenshotsTaken && this.isMeasureBoundary)
      callback();
    else
      this.synchronizedActions.Add((Action) (() => this.WaitForPerforation(currentScreenshotsTaken, callback)));
  }
}
