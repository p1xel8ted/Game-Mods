// Decompiled with JetBrains decompiler
// Type: ProgressBarWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProgressBarWidget : BaseMonoBehaviour
{
  public SkeletonAnimation _skeletonAnimation;
  public int _currentProgress;

  public int CurrentProgress => this._currentProgress;

  public SkeletonAnimation SkeletonAnimation => this._skeletonAnimation;

  public void Awake() => this._skeletonAnimation = this.GetComponent<SkeletonAnimation>();

  public void SetProgress(int progress = 0)
  {
    int num = Math.Min(5, progress);
    if (this._currentProgress == num)
      return;
    this._currentProgress = num;
    try
    {
      this._skeletonAnimation.AnimationState.SetAnimation(0, $"{Math.Max(this._currentProgress - 1, 0)}_activated", true);
    }
    catch (Exception ex)
    {
      Debug.LogException((Exception) ex);
    }
  }

  public void SetInactivate()
  {
    this._currentProgress = 0;
    this._skeletonAnimation.AnimationState.SetAnimation(0, "0", true);
  }

  public IEnumerator ProgressNext()
  {
    if (this._currentProgress < 5)
    {
      this._skeletonAnimation.AnimationState.AddAnimation(0, $"{this._currentProgress}_activated", true, 0.0f);
      ++this._currentProgress;
      yield return (object) new WaitForSeconds(0.2f);
      CameraManager.instance.shakeCamera1(2f, (float) UnityEngine.Random.Range(0, 360));
    }
  }
}
