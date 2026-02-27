// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.Version2018_3.AsyncUnityUserReportingPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Cloud.UserReporting.Client;
using Unity.Screenshots;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin.Version2018_3;

public class AsyncUnityUserReportingPlatform : IUserReportingPlatform
{
  public List<AsyncUnityUserReportingPlatform.LogMessage> logMessages;
  public List<AsyncUnityUserReportingPlatform.PostOperation> postOperations;
  public List<AsyncUnityUserReportingPlatform.ProfilerSampler> profilerSamplers;
  public ScreenshotManager screenshotManager;
  public List<AsyncUnityUserReportingPlatform.PostOperation> taskOperations;

  public AsyncUnityUserReportingPlatform()
  {
    this.logMessages = new List<AsyncUnityUserReportingPlatform.LogMessage>();
    this.postOperations = new List<AsyncUnityUserReportingPlatform.PostOperation>();
    this.screenshotManager = new ScreenshotManager();
    this.profilerSamplers = new List<AsyncUnityUserReportingPlatform.ProfilerSampler>();
    foreach (KeyValuePair<string, string> samplerName in this.GetSamplerNames())
    {
      Sampler sampler = Sampler.Get(samplerName.Key);
      if (sampler.isValid)
      {
        Recorder recorder = sampler.GetRecorder();
        recorder.enabled = true;
        this.profilerSamplers.Add(new AsyncUnityUserReportingPlatform.ProfilerSampler()
        {
          Name = samplerName.Value,
          Recorder = recorder
        });
      }
    }
    Application.logMessageReceivedThreaded += (Application.LogCallback) ((logString, stackTrace, logType) =>
    {
      lock (this.logMessages)
        this.logMessages.Add(new AsyncUnityUserReportingPlatform.LogMessage()
        {
          LogString = logString,
          StackTrace = stackTrace,
          LogType = logType
        });
    });
  }

  public T DeserializeJson<T>(string json) => Unity.Cloud.UserReporting.Plugin.SimpleJson.SimpleJson.DeserializeObject<T>(json);

  public void OnEndOfFrame(UserReportingClient client) => this.screenshotManager.OnEndOfFrame();

  public void Post(
    string endpoint,
    string contentType,
    byte[] content,
    Action<float, float> progressCallback,
    Action<bool, byte[]> callback)
  {
    UnityWebRequest unityWebRequest = new UnityWebRequest(endpoint, "POST");
    unityWebRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(content);
    unityWebRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
    unityWebRequest.SetRequestHeader("Content-Type", contentType);
    unityWebRequest.SendWebRequest();
    this.postOperations.Add(new AsyncUnityUserReportingPlatform.PostOperation()
    {
      WebRequest = unityWebRequest,
      Callback = callback,
      ProgressCallback = progressCallback
    });
  }

  public void RunTask(Func<object> task, Action<object> callback) => callback(task());

  public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
  {
  }

  public string SerializeJson(object instance) => Unity.Cloud.UserReporting.Plugin.SimpleJson.SimpleJson.SerializeObject(instance);

  public void TakeScreenshot(
    int frameNumber,
    int maximumWidth,
    int maximumHeight,
    object source,
    Action<int, byte[]> callback)
  {
    this.screenshotManager.TakeScreenshot(source, frameNumber, maximumWidth, maximumHeight, callback);
  }

  public void Update(UserReportingClient client)
  {
    lock (this.logMessages)
    {
      foreach (AsyncUnityUserReportingPlatform.LogMessage logMessage in this.logMessages)
      {
        UserReportEventLevel level = UserReportEventLevel.Info;
        if (logMessage.LogType == LogType.Warning)
          level = UserReportEventLevel.Warning;
        else if (logMessage.LogType == LogType.Error)
          level = UserReportEventLevel.Error;
        else if (logMessage.LogType == LogType.Exception)
          level = UserReportEventLevel.Error;
        else if (logMessage.LogType == LogType.Assert)
          level = UserReportEventLevel.Error;
        if (client.IsConnectedToLogger)
          client.LogEvent(level, logMessage.LogString, logMessage.StackTrace);
      }
      this.logMessages.Clear();
    }
    if (client.Configuration.MetricsGatheringMode == MetricsGatheringMode.Automatic)
    {
      this.SampleAutomaticMetrics(client);
      foreach (AsyncUnityUserReportingPlatform.ProfilerSampler profilerSampler in this.profilerSamplers)
        client.SampleMetric(profilerSampler.Name, profilerSampler.GetValue());
    }
    int index = 0;
    while (index < this.postOperations.Count)
    {
      AsyncUnityUserReportingPlatform.PostOperation postOperation = this.postOperations[index];
      if (postOperation.WebRequest.isDone)
      {
        bool flag = postOperation.WebRequest.error != null && postOperation.WebRequest.responseCode != 200L;
        if (flag)
        {
          string message = $"UnityUserReportingPlatform.Post: {postOperation.WebRequest.responseCode} {postOperation.WebRequest.error}";
          Debug.Log((object) message);
          client.LogEvent(UserReportEventLevel.Error, message);
        }
        postOperation.ProgressCallback(1f, 1f);
        postOperation.Callback(!flag, postOperation.WebRequest.downloadHandler.data);
        this.postOperations.Remove(postOperation);
      }
      else
      {
        postOperation.ProgressCallback(postOperation.WebRequest.uploadProgress, postOperation.WebRequest.downloadProgress);
        ++index;
      }
    }
  }

  public virtual IDictionary<string, string> GetDeviceMetadata()
  {
    Dictionary<string, string> deviceMetadata = new Dictionary<string, string>();
    deviceMetadata.Add("BuildGUID", Application.buildGUID);
    deviceMetadata.Add("DeviceModel", SystemInfo.deviceModel);
    deviceMetadata.Add("DeviceType", SystemInfo.deviceType.ToString());
    deviceMetadata.Add("DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
    deviceMetadata.Add("DPI", Screen.dpi.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    deviceMetadata.Add("GraphicsDeviceName", SystemInfo.graphicsDeviceName);
    deviceMetadata.Add("GraphicsDeviceType", SystemInfo.graphicsDeviceType.ToString());
    deviceMetadata.Add("GraphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
    deviceMetadata.Add("GraphicsDeviceVersion", SystemInfo.graphicsDeviceVersion);
    deviceMetadata.Add("GraphicsMemorySize", SystemInfo.graphicsMemorySize.ToString());
    deviceMetadata.Add("InstallerName", Application.installerName);
    deviceMetadata.Add("InstallMode", Application.installMode.ToString());
    deviceMetadata.Add("IsEditor", Application.isEditor.ToString());
    deviceMetadata.Add("IsFullScreen", Screen.fullScreen.ToString());
    deviceMetadata.Add("OperatingSystem", SystemInfo.operatingSystem);
    deviceMetadata.Add("OperatingSystemFamily", SystemInfo.operatingSystemFamily.ToString());
    deviceMetadata.Add("Orientation", Screen.orientation.ToString());
    deviceMetadata.Add("Platform", Application.platform.ToString());
    try
    {
      deviceMetadata.Add("QualityLevel", QualitySettings.names[QualitySettings.GetQualityLevel()]);
    }
    catch
    {
    }
    deviceMetadata.Add("ResolutionWidth", Screen.currentResolution.width.ToString());
    Dictionary<string, string> dictionary1 = deviceMetadata;
    int num = Screen.currentResolution.height;
    string str1 = num.ToString();
    dictionary1.Add("ResolutionHeight", str1);
    Dictionary<string, string> dictionary2 = deviceMetadata;
    num = Screen.currentResolution.refreshRate;
    string str2 = num.ToString();
    dictionary2.Add("ResolutionRefreshRate", str2);
    deviceMetadata.Add("SystemLanguage", Application.systemLanguage.ToString());
    Dictionary<string, string> dictionary3 = deviceMetadata;
    num = SystemInfo.systemMemorySize;
    string str3 = num.ToString();
    dictionary3.Add("SystemMemorySize", str3);
    Dictionary<string, string> dictionary4 = deviceMetadata;
    num = Application.targetFrameRate;
    string str4 = num.ToString();
    dictionary4.Add("TargetFrameRate", str4);
    deviceMetadata.Add("UnityVersion", Application.unityVersion);
    deviceMetadata.Add("Version", Application.version);
    deviceMetadata.Add("Source", "Unity");
    System.Type type = this.GetType();
    deviceMetadata.Add("IUserReportingPlatform", type.Name);
    return (IDictionary<string, string>) deviceMetadata;
  }

  public virtual Dictionary<string, string> GetSamplerNames()
  {
    return new Dictionary<string, string>()
    {
      {
        "AudioManager.FixedUpdate",
        "AudioManager.FixedUpdateInMilliseconds"
      },
      {
        "AudioManager.Update",
        "AudioManager.UpdateInMilliseconds"
      },
      {
        "LateBehaviourUpdate",
        "Behaviors.LateUpdateInMilliseconds"
      },
      {
        "BehaviourUpdate",
        "Behaviors.UpdateInMilliseconds"
      },
      {
        "Camera.Render",
        "Camera.RenderInMilliseconds"
      },
      {
        "Overhead",
        "Engine.OverheadInMilliseconds"
      },
      {
        "WaitForRenderJobs",
        "Engine.WaitForRenderJobsInMilliseconds"
      },
      {
        "WaitForTargetFPS",
        "Engine.WaitForTargetFPSInMilliseconds"
      },
      {
        "GUI.Repaint",
        "GUI.RepaintInMilliseconds"
      },
      {
        "Network.Update",
        "Network.UpdateInMilliseconds"
      },
      {
        "ParticleSystem.EndUpdateAll",
        "ParticleSystem.EndUpdateAllInMilliseconds"
      },
      {
        "ParticleSystem.Update",
        "ParticleSystem.UpdateInMilliseconds"
      },
      {
        "Physics.FetchResults",
        "Physics.FetchResultsInMilliseconds"
      },
      {
        "Physics.Processing",
        "Physics.ProcessingInMilliseconds"
      },
      {
        "Physics.ProcessReports",
        "Physics.ProcessReportsInMilliseconds"
      },
      {
        "Physics.Simulate",
        "Physics.SimulateInMilliseconds"
      },
      {
        "Physics.UpdateBodies",
        "Physics.UpdateBodiesInMilliseconds"
      },
      {
        "Physics.Interpolation",
        "Physics.InterpolationInMilliseconds"
      },
      {
        "Physics2D.DynamicUpdate",
        "Physics2D.DynamicUpdateInMilliseconds"
      },
      {
        "Physics2D.FixedUpdate",
        "Physics2D.FixedUpdateInMilliseconds"
      }
    };
  }

  public virtual void ModifyUserReport(UserReport userReport)
  {
    Scene activeScene = SceneManager.GetActiveScene();
    userReport.DeviceMetadata.Add(new UserReportNamedValue("ActiveSceneName", activeScene.name));
    Camera main = Camera.main;
    if (!((UnityEngine.Object) main != (UnityEngine.Object) null))
      return;
    userReport.DeviceMetadata.Add(new UserReportNamedValue("MainCameraName", main.name));
    List<UserReportNamedValue> deviceMetadata1 = userReport.DeviceMetadata;
    Vector3 vector3 = main.transform.position;
    UserReportNamedValue reportNamedValue1 = new UserReportNamedValue("MainCameraPosition", vector3.ToString());
    deviceMetadata1.Add(reportNamedValue1);
    List<UserReportNamedValue> deviceMetadata2 = userReport.DeviceMetadata;
    vector3 = main.transform.forward;
    UserReportNamedValue reportNamedValue2 = new UserReportNamedValue("MainCameraForward", vector3.ToString());
    deviceMetadata2.Add(reportNamedValue2);
    RaycastHit hitInfo;
    if (!Physics.Raycast(main.transform.position, main.transform.forward, out hitInfo))
      return;
    GameObject gameObject = hitInfo.transform.gameObject;
    List<UserReportNamedValue> deviceMetadata3 = userReport.DeviceMetadata;
    vector3 = hitInfo.point;
    UserReportNamedValue reportNamedValue3 = new UserReportNamedValue("LookingAt", vector3.ToString());
    deviceMetadata3.Add(reportNamedValue3);
    userReport.DeviceMetadata.Add(new UserReportNamedValue("LookingAtGameObject", gameObject.name));
    List<UserReportNamedValue> deviceMetadata4 = userReport.DeviceMetadata;
    vector3 = gameObject.transform.position;
    UserReportNamedValue reportNamedValue4 = new UserReportNamedValue("LookingAtGameObjectPosition", vector3.ToString());
    deviceMetadata4.Add(reportNamedValue4);
  }

  public virtual void SampleAutomaticMetrics(UserReportingClient client)
  {
    client.SampleMetric("Graphics.FramesPerSecond", 1.0 / (double) Time.deltaTime);
    client.SampleMetric("Memory.MonoUsedSizeInBytes", (double) Profiler.GetMonoUsedSizeLong());
    client.SampleMetric("Memory.TotalAllocatedMemoryInBytes", (double) Profiler.GetTotalAllocatedMemoryLong());
    client.SampleMetric("Memory.TotalReservedMemoryInBytes", (double) Profiler.GetTotalReservedMemoryLong());
    client.SampleMetric("Memory.TotalUnusedReservedMemoryInBytes", (double) Profiler.GetTotalUnusedReservedMemoryLong());
    client.SampleMetric("Battery.BatteryLevelInPercent", (double) SystemInfo.batteryLevel);
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__3_0(string logString, string stackTrace, LogType logType)
  {
    lock (this.logMessages)
      this.logMessages.Add(new AsyncUnityUserReportingPlatform.LogMessage()
      {
        LogString = logString,
        StackTrace = stackTrace,
        LogType = logType
      });
  }

  public struct LogMessage
  {
    public string LogString;
    public LogType LogType;
    public string StackTrace;
  }

  public class PostOperation
  {
    [CompilerGenerated]
    public Action<bool, byte[]> \u003CCallback\u003Ek__BackingField;
    [CompilerGenerated]
    public Action<float, float> \u003CProgressCallback\u003Ek__BackingField;
    [CompilerGenerated]
    public UnityWebRequest \u003CWebRequest\u003Ek__BackingField;

    public Action<bool, byte[]> Callback
    {
      get => this.\u003CCallback\u003Ek__BackingField;
      set => this.\u003CCallback\u003Ek__BackingField = value;
    }

    public Action<float, float> ProgressCallback
    {
      get => this.\u003CProgressCallback\u003Ek__BackingField;
      set => this.\u003CProgressCallback\u003Ek__BackingField = value;
    }

    public UnityWebRequest WebRequest
    {
      get => this.\u003CWebRequest\u003Ek__BackingField;
      set => this.\u003CWebRequest\u003Ek__BackingField = value;
    }
  }

  public struct ProfilerSampler
  {
    public string Name;
    public Recorder Recorder;

    public double GetValue()
    {
      return this.Recorder == null ? 0.0 : (double) this.Recorder.elapsedNanoseconds / 1000000.0;
    }
  }
}
