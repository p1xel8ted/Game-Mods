// Decompiled with JetBrains decompiler
// Type: Unity.Screenshots.ScreenshotRecorder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Unity.Screenshots;

public class ScreenshotRecorder
{
  public static int nextIdentifier;
  public List<ScreenshotRecorder.ScreenshotOperation> operationPool;

  public ScreenshotRecorder()
  {
    this.operationPool = new List<ScreenshotRecorder.ScreenshotOperation>();
  }

  public ScreenshotRecorder.ScreenshotOperation GetOperation()
  {
    foreach (ScreenshotRecorder.ScreenshotOperation operation in this.operationPool)
    {
      if (!operation.IsInUse)
      {
        operation.IsInUse = true;
        return operation;
      }
    }
    ScreenshotRecorder.ScreenshotOperation operation1 = new ScreenshotRecorder.ScreenshotOperation();
    operation1.IsInUse = true;
    this.operationPool.Add(operation1);
    return operation1;
  }

  public void Screenshot(
    int maximumWidth,
    int maximumHeight,
    ScreenshotType type,
    Action<byte[], object> callback,
    object state)
  {
    this.Screenshot(ScreenCapture.CaptureScreenshotAsTexture(), maximumWidth, maximumHeight, type, callback, state);
  }

  public void Screenshot(
    Camera source,
    int maximumWidth,
    int maximumHeight,
    ScreenshotType type,
    Action<byte[], object> callback,
    object state)
  {
    RenderTexture source1 = new RenderTexture(maximumWidth, maximumHeight, 24);
    RenderTexture targetTexture = source.targetTexture;
    source.targetTexture = source1;
    source.Render();
    source.targetTexture = targetTexture;
    this.Screenshot(source1, maximumWidth, maximumHeight, type, callback, state);
  }

  public void Screenshot(
    RenderTexture source,
    int maximumWidth,
    int maximumHeight,
    ScreenshotType type,
    Action<byte[], object> callback,
    object state)
  {
    this.ScreenshotInternal((Texture) source, maximumWidth, maximumHeight, type, callback, state);
  }

  public void Screenshot(
    Texture2D source,
    int maximumWidth,
    int maximumHeight,
    ScreenshotType type,
    Action<byte[], object> callback,
    object state)
  {
    this.ScreenshotInternal((Texture) source, maximumWidth, maximumHeight, type, callback, state);
  }

  public void ScreenshotInternal(
    Texture source,
    int maximumWidth,
    int maximumHeight,
    ScreenshotType type,
    Action<byte[], object> callback,
    object state)
  {
    ScreenshotRecorder.ScreenshotOperation operation = this.GetOperation();
    operation.Identifier = ScreenshotRecorder.nextIdentifier++;
    operation.Source = source;
    operation.MaximumWidth = maximumWidth;
    operation.MaximumHeight = maximumHeight;
    operation.Type = type;
    operation.Callback = callback;
    operation.State = state;
    AsyncGPUReadback.Request(source, 0, TextureFormat.RGBA32, (Action<AsyncGPUReadbackRequest>) operation.ScreenshotCallbackDelegate);
  }

  public class ScreenshotOperation
  {
    public WaitCallback EncodeCallbackDelegate;
    public Action<AsyncGPUReadbackRequest> ScreenshotCallbackDelegate;
    [CompilerGenerated]
    public Action<byte[], object> \u003CCallback\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CHeight\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CIdentifier\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CIsInUse\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CMaximumHeight\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CMaximumWidth\u003Ek__BackingField;
    [CompilerGenerated]
    public NativeArray<byte> \u003CNativeData\u003Ek__BackingField;
    [CompilerGenerated]
    public Texture \u003CSource\u003Ek__BackingField;
    [CompilerGenerated]
    public object \u003CState\u003Ek__BackingField;
    [CompilerGenerated]
    public ScreenshotType \u003CType\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CWidth\u003Ek__BackingField;

    public ScreenshotOperation()
    {
      this.ScreenshotCallbackDelegate = new Action<AsyncGPUReadbackRequest>(this.ScreenshotCallback);
      this.EncodeCallbackDelegate = new WaitCallback(this.EncodeCallback);
    }

    public Action<byte[], object> Callback
    {
      get => this.\u003CCallback\u003Ek__BackingField;
      set => this.\u003CCallback\u003Ek__BackingField = value;
    }

    public int Height
    {
      get => this.\u003CHeight\u003Ek__BackingField;
      set => this.\u003CHeight\u003Ek__BackingField = value;
    }

    public int Identifier
    {
      get => this.\u003CIdentifier\u003Ek__BackingField;
      set => this.\u003CIdentifier\u003Ek__BackingField = value;
    }

    public bool IsInUse
    {
      get => this.\u003CIsInUse\u003Ek__BackingField;
      set => this.\u003CIsInUse\u003Ek__BackingField = value;
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

    public NativeArray<byte> NativeData
    {
      get => this.\u003CNativeData\u003Ek__BackingField;
      set => this.\u003CNativeData\u003Ek__BackingField = value;
    }

    public Texture Source
    {
      get => this.\u003CSource\u003Ek__BackingField;
      set => this.\u003CSource\u003Ek__BackingField = value;
    }

    public object State
    {
      get => this.\u003CState\u003Ek__BackingField;
      set => this.\u003CState\u003Ek__BackingField = value;
    }

    public ScreenshotType Type
    {
      get => this.\u003CType\u003Ek__BackingField;
      set => this.\u003CType\u003Ek__BackingField = value;
    }

    public int Width
    {
      get => this.\u003CWidth\u003Ek__BackingField;
      set => this.\u003CWidth\u003Ek__BackingField = value;
    }

    public void EncodeCallback(object state)
    {
      int downsampledStride;
      byte[] dataRgba = Downsampler.Downsample(this.NativeData.ToArray(), this.Width * 4, this.MaximumWidth, this.MaximumHeight, out downsampledStride);
      if (this.Type == ScreenshotType.Png)
        dataRgba = PngEncoder.Encode(dataRgba, downsampledStride);
      if (this.Callback != null)
        this.Callback(dataRgba, this.State);
      this.NativeData.Dispose();
      this.IsInUse = false;
    }

    public void SavePngToDisk(byte[] byteData)
    {
      if (!Directory.Exists("Screenshots"))
        Directory.CreateDirectory("Screenshots");
      File.WriteAllBytes($"Screenshots/{this.Identifier % 60}.png", byteData);
    }

    public void ScreenshotCallback(AsyncGPUReadbackRequest request)
    {
      if (!request.hasError)
      {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        NativeArray<byte> nativeArray = new NativeArray<byte>(request.GetData<byte>(), Allocator.Persistent);
        this.Width = request.width;
        this.Height = request.height;
        this.NativeData = nativeArray;
        ThreadPool.QueueUserWorkItem(this.EncodeCallbackDelegate, (object) null);
      }
      else if (this.Callback != null)
        this.Callback((byte[]) null, this.State);
      if (!((UnityEngine.Object) this.Source != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Source);
    }
  }
}
