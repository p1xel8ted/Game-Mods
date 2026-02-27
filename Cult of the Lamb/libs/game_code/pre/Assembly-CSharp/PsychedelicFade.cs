// Decompiled with JetBrains decompiler
// Type: PsychedelicFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PsychedelicFade : BaseMonoBehaviour
{
  public Image image;
  private float Duration = 2f;
  private float Delay;
  private bool UseDeltaTime = true;

  public void FadeIn(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true)
  {
    this.image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    this.image.enabled = true;
    this.image.DOKill();
    DOTweenModuleUI.DOFade(this.image, 1f, _Duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(UseDeltaTime).SetDelay<TweenerCore<Color, Color, ColorOptions>>(_Delay);
  }

  public void FadeOut(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true)
  {
    this.image.enabled = true;
    this.image.DOKill();
    DOTweenModuleUI.DOFade(this.image, 0.0f, _Duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(UseDeltaTime).SetDelay<TweenerCore<Color, Color, ColorOptions>>(_Delay).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  private void OnEnable() => this.image.enabled = false;
}
