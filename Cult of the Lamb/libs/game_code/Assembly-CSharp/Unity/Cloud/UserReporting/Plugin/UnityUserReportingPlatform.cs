// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.UnityUserReportingPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Cloud.UserReporting.Client;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin;

public class UnityUserReportingPlatform : IUserReportingPlatform
{
  public List<UnityUserReportingPlatform.LogMessage> logMessages;
  public List<UnityUserReportingPlatform.PostOperation> postOperations;
  public List<UnityUserReportingPlatform.ProfilerSampler> profilerSamplers;
  public List<UnityUserReportingPlatform.ScreenshotOperation> screenshotOperations;
  public Stopwatch screenshotStopwatch;
  public List<UnityUserReportingPlatform.PostOperation> taskOperations;

  public UnityUserReportingPlatform()
  {
    this.logMessages = new List<UnityUserReportingPlatform.LogMessage>();
    this.postOperations = new List<UnityUserReportingPlatform.PostOperation>();
    this.screenshotOperations = new List<UnityUserReportingPlatform.ScreenshotOperation>();
    this.screenshotStopwatch = new Stopwatch();
    this.profilerSamplers = new List<UnityUserReportingPlatform.ProfilerSampler>();
    foreach (KeyValuePair<string, string> samplerName in this.GetSamplerNames())
    {
      Sampler sampler = Sampler.Get(samplerName.Key);
      if (sampler.isValid)
      {
        Recorder recorder = sampler.GetRecorder();
        recorder.enabled = true;
        this.profilerSamplers.Add(new UnityUserReportingPlatform.ProfilerSampler()
        {
          Name = samplerName.Value,
          Recorder = recorder
        });
      }
    }
    Application.logMessageReceivedThreaded += (Application.LogCallback) ((logString, stackTrace, logType) =>
    {
      lock (this.logMessages)
        this.logMessages.Add(new UnityUserReportingPlatform.LogMessage()
        {
          LogString = logString,
          StackTrace = stackTrace,
          LogType = logType
        });
    });
  }

  public T DeserializeJson<T>(string json) => Unity.Cloud.UserReporting.Plugin.SimpleJson.SimpleJson.DeserializeObject<T>(json);

  public void OnEndOfFrame(UserReportingClient client)
  {
    int index = 0;
    while (index < this.screenshotOperations.Count)
    {
      UnityUserReportingPlatform.ScreenshotOperation screenshotOperation = this.screenshotOperations[index];
      if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.Render && screenshotOperation.WaitFrames < 1)
      {
        Camera source = screenshotOperation.Source as Camera;
        if ((UnityEngine.Object) source != (UnityEngine.Object) null)
        {
          this.screenshotStopwatch.Reset();
          this.screenshotStopwatch.Start();
          RenderTexture renderTexture = new RenderTexture(screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, 24);
          RenderTexture targetTexture = source.targetTexture;
          source.targetTexture = renderTexture;
          source.Render();
          source.targetTexture = targetTexture;
          this.screenshotStopwatch.Stop();
          client.SampleClientMetric("Screenshot.Render", (double) this.screenshotStopwatch.ElapsedMilliseconds);
          screenshotOperation.Source = (object) renderTexture;
          screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.ReadPixels;
          screenshotOperation.WaitFrames = 15;
          ++index;
          continue;
        }
        screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.ReadPixels;
      }
      if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.ReadPixels && screenshotOperation.WaitFrames < 1)
      {
        this.screenshotStopwatch.Reset();
        this.screenshotStopwatch.Start();
        RenderTexture source = screenshotOperation.Source as RenderTexture;
        if ((UnityEngine.Object) source != (UnityEngine.Object) null)
        {
          RenderTexture active = RenderTexture.active;
          RenderTexture.active = source;
          screenshotOperation.Texture = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
          screenshotOperation.Texture.ReadPixels(new Rect(0.0f, 0.0f, (float) source.width, (float) source.height), 0, 0);
          screenshotOperation.Texture.Apply();
          RenderTexture.active = active;
        }
        else
        {
          screenshotOperation.Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);
          screenshotOperation.Texture.ReadPixels(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), 0, 0);
          screenshotOperation.Texture.Apply();
        }
        this.screenshotStopwatch.Stop();
        client.SampleClientMetric("Screenshot.ReadPixels", (double) this.screenshotStopwatch.ElapsedMilliseconds);
        screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.GetPixels;
        screenshotOperation.WaitFrames = 15;
        ++index;
      }
      else if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.GetPixels && screenshotOperation.WaitFrames < 1)
      {
        this.screenshotStopwatch.Reset();
        this.screenshotStopwatch.Start();
        int num1 = screenshotOperation.MaximumWidth > 32 /*0x20*/ ? screenshotOperation.MaximumWidth : 32 /*0x20*/;
        int num2 = screenshotOperation.MaximumHeight > 32 /*0x20*/ ? screenshotOperation.MaximumHeight : 32 /*0x20*/;
        int width = screenshotOperation.Texture.width;
        int height = screenshotOperation.Texture.height;
        int miplevel = 0;
        while (width > num1 || height > num2)
        {
          width /= 2;
          height /= 2;
          ++miplevel;
        }
        screenshotOperation.TextureResized = new Texture2D(width, height);
        screenshotOperation.TextureResized.SetPixels(screenshotOperation.Texture.GetPixels(miplevel));
        screenshotOperation.TextureResized.Apply();
        this.screenshotStopwatch.Stop();
        client.SampleClientMetric("Screenshot.GetPixels", (double) this.screenshotStopwatch.ElapsedMilliseconds);
        screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.EncodeToPNG;
        screenshotOperation.WaitFrames = 15;
        ++index;
      }
      else if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.EncodeToPNG && screenshotOperation.WaitFrames < 1)
      {
        this.screenshotStopwatch.Reset();
        this.screenshotStopwatch.Start();
        screenshotOperation.PngData = screenshotOperation.TextureResized.EncodeToJPG();
        this.screenshotStopwatch.Stop();
        client.SampleClientMetric("Screenshot.EncodeToPNG", (double) this.screenshotStopwatch.ElapsedMilliseconds);
        screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.Done;
        ++index;
      }
      else
      {
        if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.Done && screenshotOperation.WaitFrames < 1)
        {
          screenshotOperation.Callback(screenshotOperation.FrameNumber, screenshotOperation.PngData);
          UnityEngine.Object.Destroy((UnityEngine.Object) screenshotOperation.Texture);
          UnityEngine.Object.Destroy((UnityEngine.Object) screenshotOperation.TextureResized);
          this.screenshotOperations.Remove(screenshotOperation);
        }
        --screenshotOperation.WaitFrames;
      }
    }
  }

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
    this.postOperations.Add(new UnityUserReportingPlatform.PostOperation()
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
    this.screenshotOperations.Add(new UnityUserReportingPlatform.ScreenshotOperation()
    {
      FrameNumber = frameNumber,
      MaximumWidth = maximumWidth,
      MaximumHeight = maximumHeight,
      Source = source,
      Callback = callback,
      UnityFrame = Time.frameCount
    });
  }

  public void Update(UserReportingClient client)
  {
    lock (this.logMessages)
    {
      foreach (UnityUserReportingPlatform.LogMessage logMessage in this.logMessages)
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
      foreach (UnityUserReportingPlatform.ProfilerSampler profilerSampler in this.profilerSamplers)
        client.SampleMetric(profilerSampler.Name, profilerSampler.GetValue());
    }
    int index = 0;
    while (index < this.postOperations.Count)
    {
      UnityUserReportingPlatform.PostOperation postOperation = this.postOperations[index];
      if (postOperation.WebRequest.isDone)
      {
        bool flag = postOperation.WebRequest.error != null && postOperation.WebRequest.responseCode != 200L;
        if (flag)
        {
          string message = $"UnityUserReportingPlatform.Post: {postOperation.WebRequest.responseCode} {postOperation.WebRequest.error}";
          UnityEngine.Debug.Log((object) message);
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
  public void \u003C\u002Ector\u003Eb__5_0(string logString, string stackTrace, LogType logType)
  {
    lock (this.logMessages)
      this.logMessages.Add(new UnityUserReportingPlatform.LogMessage()
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

  public class ScreenshotOperation
  {
    [CompilerGenerated]
    public Action<int, byte[]> \u003CCallback\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CFrameNumber\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CMaximumHeight\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CMaximumWidth\u003Ek__BackingField;
    [CompilerGenerated]
    public byte[] \u003CPngData\u003Ek__BackingField;
    [CompilerGenerated]
    public object \u003CSource\u003Ek__BackingField;
    [CompilerGenerated]
    public UnityUserReportingPlatform.ScreenshotStage \u003CStage\u003Ek__BackingField;
    [CompilerGenerated]
    public Texture2D \u003CTexture\u003Ek__BackingField;
    [CompilerGenerated]
    public Texture2D \u003CTextureResized\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CUnityFrame\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CWaitFrames\u003Ek__BackingField;

    public Action<int, byte[]> Callback
    {
      get => this.\u003CCallback\u003Ek__BackingField;
      set => this.\u003CCallback\u003Ek__BackingField = value;
    }

    public int FrameNumber
    {
      get => this.\u003CFrameNumber\u003Ek__BackingField;
      set => this.\u003CFrameNumber\u003Ek__BackingField = value;
    }

    public int MaximumHeight
    {
      get => this.\u003CMaximumHeight\u003Ek__BackingField;
      set => this.\u003CMaximumHeight\u003Ek__BackingField = value;
    }

    public int MaximumWidth
    {
      get => this.\u003CMaximumWidth\u003Ek__BackingField;
      set => this.\u003CMaximumWidth\u003Ek__BackingField = value;
    }

    public byte[] PngData
    {
      get => this.\u003CPngData\u003Ek__BackingField;
      set => this.\u003CPngData\u003Ek__BackingField = value;
    }

    public object Source
    {
      get => this.\u003CSource\u003Ek__BackingField;
      set => this.\u003CSource\u003Ek__BackingField = value;
    }

    public UnityUserReportingPlatform.ScreenshotStage Stage
    {
      get => this.\u003CStage\u003Ek__BackingField;
      set => this.\u003CStage\u003Ek__BackingField = value;
    }

    public Texture2D Texture
    {
      get => this.\u003CTexture\u003Ek__BackingField;
      set => this.\u003CTexture\u003Ek__BackingField = value;
    }

    public Texture2D TextureResized
    {
      get => this.\u003CTextureResized\u003Ek__BackingField;
      set => this.\u003CTextureResized\u003Ek__BackingField = value;
    }

    public int UnityFrame
    {
      get => this.\u003CUnityFrame\u003Ek__BackingField;
      set => this.\u003CUnityFrame\u003Ek__BackingField = value;
    }

    public int WaitFrames
    {
      get => this.\u003CWaitFrames\u003Ek__BackingField;
      set => this.\u003CWaitFrames\u003Ek__BackingField = value;
    }
  }

  public enum ScreenshotStage
  {
    Render,
    ReadPixels,
    GetPixels,
    EncodeToPNG,
    Done,
  }
}
