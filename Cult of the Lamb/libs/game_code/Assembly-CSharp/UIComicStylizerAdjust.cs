// Decompiled with JetBrains decompiler
// Type: UIComicStylizerAdjust
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class UIComicStylizerAdjust : MonoBehaviour
{
  [SerializeField]
  public Palette palette;
  [Range(0.0f, 10f)]
  [SerializeField]
  public float durationIn;
  [Range(0.0f, 10f)]
  [SerializeField]
  public float durationOut;
  [SerializeField]
  public bool showOnEnable;
  [SerializeField]
  public bool hideOnDisable = true;
  [SerializeField]
  public Stylizer _stylizer;
  public TweenerCore<float, float, FloatOptions> tween;

  public void OnEnable()
  {
    if ((Object) this._stylizer == (Object) null)
      this._stylizer = this.GetComponentInParent<UIComicMenuController>().Camera.GetComponent<Stylizer>();
    if (!((Object) this._stylizer != (Object) null) || !this.showOnEnable)
      return;
    this.ShowStylizer();
  }

  public void OnDisable()
  {
    DOTween.Kill((object) this.tween);
    if (!this.hideOnDisable)
      return;
    this.HideStylizer();
  }

  public void ShowStylizer()
  {
    if ((Object) this._stylizer == (Object) null)
    {
      Debug.Log((object) "Stylizer not found!");
    }
    else
    {
      this._stylizer.Palette = this.palette;
      this._stylizer.EffectIntensity = 0.0f;
      float currentValue = 0.0f;
      float endValue = 1f;
      DOTween.Kill((object) this.tween);
      this.tween = DOTween.To((DOGetter<float>) (() => currentValue), (DOSetter<float>) (x => currentValue = x), endValue, this.durationIn).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._stylizer.EffectIntensity = currentValue));
    }
  }

  public void HideStylizer()
  {
    if ((Object) this._stylizer == (Object) null)
    {
      Debug.Log((object) "Stylizer not found!");
    }
    else
    {
      float currentValue = 1f;
      float endValue = 0.0f;
      DOTween.Kill((object) this.tween);
      this.tween = DOTween.To((DOGetter<float>) (() => currentValue), (DOSetter<float>) (x => currentValue = x), endValue, this.durationOut).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._stylizer.EffectIntensity = currentValue)).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._stylizer.Palette = this.GetComponentInParent<UIComicMenuController>().ComicPalette));
    }
  }

  public void KillTween()
  {
    if (this.tween == null)
      return;
    this.tween.Kill();
  }
}
