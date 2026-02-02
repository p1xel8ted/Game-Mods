// Decompiled with JetBrains decompiler
// Type: PsychedelicFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PsychedelicFade : BaseMonoBehaviour
{
  public Image image;
  public float Duration = 2f;
  public float Delay;
  public bool UseDeltaTime = true;

  public void FadeIn(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true, System.Action onComplete = null)
  {
    this.image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    this.image.enabled = true;
    this.image.DOKill();
    DOTweenModuleUI.DOFade(this.image, 1f, _Duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(UseDeltaTime).SetDelay<TweenerCore<Color, Color, ColorOptions>>(_Delay).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      System.Action action = onComplete;
      if (action == null)
        return;
      action();
    }));
  }

  public void FadeOut(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true, System.Action onComplete = null)
  {
    this.image.enabled = true;
    this.image.DOKill();
    DOTweenModuleUI.DOFade(this.image, 0.0f, _Duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(UseDeltaTime).SetDelay<TweenerCore<Color, Color, ColorOptions>>(_Delay).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      System.Action action = onComplete;
      if (action != null)
        action();
      this.gameObject.SetActive(false);
    }));
  }

  public void OnEnable() => this.image.enabled = false;
}
