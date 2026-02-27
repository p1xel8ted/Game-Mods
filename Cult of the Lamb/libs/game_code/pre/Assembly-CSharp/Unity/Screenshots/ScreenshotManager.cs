// Decompiled with JetBrains decompiler
// Type: Unity.Screenshots.ScreenshotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Unity.Screenshots;

public class ScreenshotManager
{
  private Action<byte[], object> screenshotCallbackDelegate;
  private List<ScreenshotManager.ScreenshotOperation> screenshotOperations;
  private ScreenshotRecorder screenshotRecorder;

  public ScreenshotManager()
  {
    this.screenshotRecorder = new ScreenshotRecorder();
    this.screenshotCallbackDelegate = new Action<byte[], object>(this.ScreenshotCallback);
    this.screenshotOperations = new List<ScreenshotManager.ScreenshotOperation>();
  }

  private ScreenshotManager.ScreenshotOperation GetScreenshotOperation()
  {
    foreach (ScreenshotManager.ScreenshotOperation screenshotOperation in this.screenshotOperations)
    {
      if (!screenshotOperation.IsInUse)
      {
        screenshotOperation.Use();
        return screenshotOperation;
      }
    }
    ScreenshotManager.ScreenshotOperation screenshotOperation1 = new ScreenshotManager.ScreenshotOperation();
    screenshotOperation1.Use();
    this.screenshotOperations.Add(screenshotOperation1);
    return screenshotOperation1;
  }

  public void OnEndOfFrame()
  {
    foreach (ScreenshotManager.ScreenshotOperation screenshotOperation in this.screenshotOperations)
    {
      if (screenshotOperation.IsInUse)
      {
        if (screenshotOperation.IsAwaiting)
        {
          screenshotOperation.IsAwaiting = false;
          if (screenshotOperation.Source == null)
            this.screenshotRecorder.Screenshot(screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, (object) screenshotOperation);
          else if (screenshotOperation.Source is Camera)
            this.screenshotRecorder.Screenshot(screenshotOperation.Source as Camera, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, (object) screenshotOperation);
          else if (screenshotOperation.Source is RenderTexture)
            this.screenshotRecorder.Screenshot(screenshotOperation.Source as RenderTexture, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, (object) screenshotOperation);
          else if (screenshotOperation.Source is Texture2D)
            this.screenshotRecorder.Screenshot(screenshotOperation.Source as Texture2D, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, (object) screenshotOperation);
          else
            this.ScreenshotCallback((byte[]) null, (object) screenshotOperation);
        }
        else if (screenshotOperation.IsComplete)
        {
          screenshotOperation.IsInUse = false;
          try
          {
            if (screenshotOperation != null)
            {
              if (screenshotOperation.Callback != null)
                screenshotOperation.Callback(screenshotOperation.FrameNumber, screenshotOperation.Data);
            }
          }
          catch
          {
          }
        }
      }
    }
  }

  private void ScreenshotCallback(byte[] data, object state)
  {
    if (!(state is ScreenshotManager.ScreenshotOperation screenshotOperation))
      return;
    screenshotOperation.Data = data;
    screenshotOperation.IsComplete = true;
  }

  public void TakeScreenshot(
    object source,
    int frameNumber,
    int maximumWidth,
    int maximumHeight,
    Action<int, byte[]> callback)
  {
    ScreenshotManager.ScreenshotOperation screenshotOperation = this.GetScreenshotOperation();
    screenshotOperation.FrameNumber = frameNumber;
    screenshotOperation.MaximumWidth = maximumWidth;
    screenshotOperation.MaximumHeight = maximumHeight;
    screenshotOperation.Source = source;
    screenshotOperation.Callback = callback;
  }

  private class ScreenshotOperation
  {
    public Action<int, byte[]> Callback { get; set; }

    public byte[] Data { get; set; }

    public int FrameNumber { get; set; }

    public bool IsAwaiting { get; set; }

    public bool IsComplete { get; set; }

    public bool IsInUse { get; set; }

    public int MaximumHeight { get; set; }

    public int MaximumWidth { get; set; }

    public object Source { get; set; }

    public void Use()
    {
      this.Callback = (Action<int, byte[]>) null;
      this.Data = (byte[]) null;
      this.FrameNumber = 0;
      this.IsAwaiting = true;
      this.IsComplete = false;
      this.IsInUse = true;
      this.MaximumHeight = 0;
      this.MaximumWidth = 0;
      this.Source = (object) null;
    }
  }
}
