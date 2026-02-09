// Decompiled with JetBrains decompiler
// Type: ManualSkeletonUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ManualSkeletonUpdater
{
  public int AnimationFramerate = 60;
  public SkeletonAnimation skeletonAnimation;
  public float deltaTimeSumBetweenLastUpdate;
  public int FrameIntervalOffset;
  public int maxInterval = 6;
  [CompilerGenerated]
  public bool \u003COnScreenUpdate\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCanChangeAnimationQuality\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CVisible\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CStopUpdates\u003Ek__BackingField;
  public bool enableSpread;

  public float TimeInterval => 1f / (float) this.AnimationFramerate;

  public float timeInterval => 1f / (float) this.AnimationFramerate;

  public bool OnScreenUpdate
  {
    get => this.\u003COnScreenUpdate\u003Ek__BackingField;
    set => this.\u003COnScreenUpdate\u003Ek__BackingField = value;
  }

  public bool CanChangeAnimationQuality
  {
    get => this.\u003CCanChangeAnimationQuality\u003Ek__BackingField;
    set => this.\u003CCanChangeAnimationQuality\u003Ek__BackingField = value;
  }

  public bool Visible
  {
    get => this.\u003CVisible\u003Ek__BackingField;
    set => this.\u003CVisible\u003Ek__BackingField = value;
  }

  public bool StopUpdates
  {
    get => this.\u003CStopUpdates\u003Ek__BackingField;
    set => this.\u003CStopUpdates\u003Ek__BackingField = value;
  }

  public void QualityToOff()
  {
    this.enableSpread = true;
    this.ChangeAnimationQuality(ManualSkeletonUpdater.AnimationQuality.None);
    this.AnimationFramerate = 1;
  }

  public void QualityToMinimum()
  {
    this.enableSpread = true;
    this.ChangeAnimationQuality(ManualSkeletonUpdater.AnimationQuality.Low);
    this.AnimationFramerate = 25;
  }

  public void QualityToHigh()
  {
    this.enableSpread = false;
    this.ChangeAnimationQuality(ManualSkeletonUpdater.AnimationQuality.High);
    this.AnimationFramerate = 60;
  }

  public void QualityToNormal()
  {
    this.enableSpread = true;
    this.ChangeAnimationQuality(ManualSkeletonUpdater.AnimationQuality.Normal);
    this.AnimationFramerate = 30;
  }

  public ManualSkeletonUpdater(SkeletonAnimation skeletonAnimation)
  {
    this.skeletonAnimation = skeletonAnimation;
    skeletonAnimation.SetVisibility(true);
    skeletonAnimation.Initialize(false);
    skeletonAnimation.clearStateOnDisable = false;
    skeletonAnimation.enabled = false;
    this.deltaTimeSumBetweenLastUpdate = 0.0f;
    this.ChangeAnimationQuality(ManualSkeletonUpdater.AnimationQuality.Normal);
  }

  public void ChangeAnimationQuality(ManualSkeletonUpdater.AnimationQuality quality)
  {
    if (quality == (ManualSkeletonUpdater.AnimationQuality) this.maxInterval)
      return;
    this.maxInterval = !this.CanChangeAnimationQuality ? 4 : (int) quality;
    if (this.enableSpread)
      this.skeletonAnimation.UpdateInterval = this.maxInterval;
    this.FrameIntervalOffset = Mathf.Abs(this.skeletonAnimation.GetInstanceID()) % this.skeletonAnimation.UpdateInterval;
  }

  public void Update()
  {
    this.skeletonAnimation.SetVisibility(false);
    if (!this.OnScreenUpdate)
    {
      if (this.StopUpdates && !this.skeletonAnimation.ForceVisible)
        return;
      this.ManualUpdate(false);
    }
    else
    {
      if (!this.Visible)
        return;
      this.ManualUpdate(true);
    }
  }

  public void LateUpdate()
  {
    this.skeletonAnimation.lodVisible = this.Visible;
    this.skeletonAnimation.BaseLateUpdate();
  }

  public void ManualUpdate(bool isOnScreen)
  {
    if (!this.skeletonAnimation.UseDeltaTime)
    {
      double unscaledDeltaTime = (double) Time.unscaledDeltaTime;
    }
    else
    {
      double deltaTime = (double) Time.deltaTime;
    }
    this.skeletonAnimation.SetVisibility(isOnScreen);
    this.skeletonAnimation.OnUpdate();
  }

  public void ForceFirstUpdate()
  {
    this.skeletonAnimation.Update(0.0f);
    DispatchThread.Instance.Dequeue();
    this.skeletonAnimation.BaseLateUpdate();
  }

  public enum AnimationQuality
  {
    None = -1, // 0xFFFFFFFF
    High = 2,
    Normal = 4,
    Low = 6,
  }
}
