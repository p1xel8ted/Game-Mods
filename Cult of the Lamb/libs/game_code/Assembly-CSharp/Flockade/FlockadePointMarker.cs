// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePointMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadePointMarker : MonoBehaviour
{
  public const float _APPEARANCE_DISAPPEARANCE_DURATION = 0.116666667f;
  public const Ease _APPEARANCE_DISAPPEARANCE_EASING = Ease.InOutQuad;
  public Sequence _sequence;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CBonus\u003Ek__BackingField;

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public virtual bool Bonus
  {
    get => this.\u003CBonus\u003Ek__BackingField;
    set => this.\u003CBonus\u003Ek__BackingField = value;
  }

  public virtual void Awake() => this.gameObject.SetActive(false);

  public Sequence SetActive(bool value, Action onLayoutChanged = null)
  {
    this.Active = value;
    Sequence sequence = this._sequence;
    if (sequence != null)
      sequence.Complete(true);
    if (this.Active)
    {
      this.gameObject.SetActive(true);
      Action action = onLayoutChanged;
      if (action != null)
        action();
      this._sequence = this.Show();
      return this._sequence;
    }
    this._sequence = this.Hide().AppendCallback((TweenCallback) (() =>
    {
      this.gameObject.SetActive(false);
      this.Bonus = false;
      Action action = onLayoutChanged;
      if (action == null)
        return;
      action();
    }));
    return this._sequence;
  }

  public virtual Sequence Show() => DOTween.Sequence().AppendInterval(0.0166666675f);

  public virtual Sequence Hide() => DOTween.Sequence().AppendInterval(0.0166666675f);
}
