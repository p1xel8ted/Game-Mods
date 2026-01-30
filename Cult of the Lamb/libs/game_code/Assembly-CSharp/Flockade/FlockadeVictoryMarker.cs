// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVictoryMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeVictoryMarker : MonoBehaviour
{
  public const float _APPEARANCE_DURATION = 0.166666672f;
  public const Ease _APPEARANCE_EASING = Ease.InOutCubic;
  public const float _INITIAL_SCALE = 1.5f;
  public const float _TILT_ANGLE = -10f;
  [SerializeField]
  public Image _active;
  public DG.Tweening.Sequence _sequence;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField = true;

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public virtual void Awake() => this.SetActive(false).Complete(true);

  public DG.Tweening.Sequence SetActive(bool value)
  {
    this.Active = value;
    DG.Tweening.Sequence sequence = this._sequence;
    if (sequence != null)
      sequence.Complete(true);
    if (this.Active)
    {
      this._sequence = DOTween.Sequence().Append((Tween) DOTweenModuleUI.DOFade(this._active, 1f, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InOutCubic)).Join((Tween) this._active.rectTransform.DOScale(1f, 0.166666672f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCubic)).Join((Tween) this._active.rectTransform.DORotate(Vector3.zero, 0.166666672f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutCubic));
      return this._sequence;
    }
    this._sequence = DOTween.Sequence().Append((Tween) DOTweenModuleUI.DOFade(this._active, 0.0f, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InOutCubic)).Join((Tween) this._active.rectTransform.DOScale(1.5f, 0.166666672f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCubic)).Join((Tween) this._active.rectTransform.DORotate(new Vector3(0.0f, 0.0f, -10f), 0.166666672f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutCubic));
    return this._sequence;
  }
}
