// Decompiled with JetBrains decompiler
// Type: Unity.Screenshots.ScreenshotRecorder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Unity.Screenshots;

public class ScreenshotRecorder
{
  private static int nextIdentifier;
  private List<ScreenshotRecorder.ScreenshotOperation> operationPool;

  public ScreenshotRecorder()
  {
    this.operationPool = new List<ScreenshotRecorder.ScreenshotOperation>();
  }

  private ScreenshotRecorder.ScreenshotOperation GetOperation()
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

  private void ScreenshotInternal(
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

  private class ScreenshotOperation
  {
    public WaitCallback EncodeCallbackDelegate;
    public Action<AsyncGPUReadbackRequest> ScreenshotCallbackDelegate;

    public ScreenshotOperation()
    {
      this.ScreenshotCallbackDelegate = new Action<AsyncGPUReadbackRequest>(this.ScreenshotCallback);
      this.EncodeCallbackDelegate = new WaitCallback(this.EncodeCallback);
    }

    public Action<byte[], object> Callback { get; set; }

    public int Height { get; set; }

    public int Identifier { get; set; }

    public bool IsInUse { get; set; }

    public int MaximumHeight { get; set; }

    public int MaximumWidth { get; set; }

    public NativeArray<byte> NativeData { get; set; }

    public Texture Source { get; set; }

    public object State { get; set; }

    public ScreenshotType Type { get; set; }

    public int Width { get; set; }

    private void EncodeCallback(object state)
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

    private void SavePngToDisk(byte[] byteData)
    {
      if (!Directory.Exists("Screenshots"))
        Directory.CreateDirectory("Screenshots");
      File.WriteAllBytes($"Screenshots/{this.Identifier % 60}.png", byteData);
    }

    private void ScreenshotCallback(AsyncGPUReadbackRequest request)
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
