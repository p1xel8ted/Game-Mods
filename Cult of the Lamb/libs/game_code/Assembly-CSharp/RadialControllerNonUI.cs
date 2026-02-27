// Decompiled with JetBrains decompiler
// Type: RadialControllerNonUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RadialControllerNonUI : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer _radialProgress;
  [SerializeField]
  public SpriteRenderer _radialInstant;
  public const string Material_Property = "_Arc2";

  public void OnEnable() => this._radialInstant.material.SetFloat("_Arc2", 360f);

  public void SetBarSize(float Size, bool Animate, bool Lock = false)
  {
    Size = 1f - Size;
    this._radialProgress.DOKill();
    if (!Animate)
      this._radialProgress.material.SetFloat("_Arc2", 360f * Size);
    else
      this._radialProgress.material.DOFloat(360f * Size, "_Arc2", 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
  }

  public void ShrinkBarToEmpty(float duration)
  {
    this._radialProgress.DOKill();
    this._radialProgress.material.DOFloat(360f, "_Arc2", 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.enabled = true));
  }

  [CompilerGenerated]
  public void \u003CShrinkBarToEmpty\u003Eb__5_0() => this.enabled = true;
}
