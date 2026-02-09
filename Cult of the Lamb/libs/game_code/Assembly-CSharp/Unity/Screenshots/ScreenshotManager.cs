// Decompiled with JetBrains decompiler
// Type: Unity.Screenshots.ScreenshotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Unity.Screenshots;

public class ScreenshotManager
{
  public Action<byte[], object> screenshotCallbackDelegate;
  public List<ScreenshotManager.ScreenshotOperation> screenshotOperations;
  public ScreenshotRecorder screenshotRecorder;

  public ScreenshotManager()
  {
    this.screenshotRecorder = new ScreenshotRecorder();
    this.screenshotCallbackDelegate = new Action<byte[], object>(this.ScreenshotCallback);
    this.screenshotOperations = new List<ScreenshotManager.ScreenshotOperation>();
  }

  public ScreenshotManager.ScreenshotOperation GetScreenshotOperation()
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

  public void ScreenshotCallback(byte[] data, object state)
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

  public class ScreenshotOperation
  {
    [CompilerGenerated]
    public Action<int, byte[]> \u003CCallback\u003Ek__BackingField;
    [CompilerGenerated]
    public byte[] \u003CData\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CFrameNumber\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CIsAwaiting\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CIsComplete\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CIsInUse\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CMaximumHeight\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CMaximumWidth\u003Ek__BackingField;
    [CompilerGenerated]
    public object \u003CSource\u003Ek__BackingField;

    public Action<int, byte[]> Callback
    {
      get => this.\u003CCallback\u003Ek__BackingField;
      set => this.\u003CCallback\u003Ek__BackingField = value;
    }

    public byte[] Data
    {
      get => this.\u003CData\u003Ek__BackingField;
      set => this.\u003CData\u003Ek__BackingField = value;
    }

    public int FrameNumber
    {
      get => this.\u003CFrameNumber\u003Ek__BackingField;
      set => this.\u003CFrameNumber\u003Ek__BackingField = value;
    }

    public bool IsAwaiting
    {
      get => this.\u003CIsAwaiting\u003Ek__BackingField;
      set => this.\u003CIsAwaiting\u003Ek__BackingField = value;
    }

    public bool IsComplete
    {
      get => this.\u003CIsComplete\u003Ek__BackingField;
      set => this.\u003CIsComplete\u003Ek__BackingField = value;
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

    public object Source
    {
      get => this.\u003CSource\u003Ek__BackingField;
      set => this.\u003CSource\u003Ek__BackingField = value;
    }

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
