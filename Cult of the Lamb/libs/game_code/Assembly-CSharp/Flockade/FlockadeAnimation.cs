// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (RectTransform))]
public abstract class FlockadeAnimation : MonoBehaviour
{
  public (bool X, bool Y) _flip;
  [CompilerGenerated]
  public CanvasGroup \u003CCanvasGroup\u003Ek__BackingField;
  [CompilerGenerated]
  public RectTransform \u003CRectTransform\u003Ek__BackingField;

  public (bool X, bool Y) Flip
  {
    get => this._flip;
    set
    {
      this._flip = value;
      if (!(bool) (Object) this.RectTransform)
        return;
      this.RectTransform.localScale = new Vector3(value.X ? -1f : 1f, value.Y ? -1f : 1f, 1f);
    }
  }

  public CanvasGroup CanvasGroup
  {
    get => this.\u003CCanvasGroup\u003Ek__BackingField;
    set => this.\u003CCanvasGroup\u003Ek__BackingField = value;
  }

  public RectTransform RectTransform
  {
    get => this.\u003CRectTransform\u003Ek__BackingField;
    set => this.\u003CRectTransform\u003Ek__BackingField = value;
  }

  public virtual void Awake()
  {
    RectTransform component1;
    if (this.TryGetComponent<RectTransform>(out component1))
    {
      component1.anchorMin = Vector2.zero;
      component1.anchorMax = Vector2.one;
      component1.offsetMin = Vector2.zero;
      component1.offsetMax = Vector2.zero;
      component1.anchoredPosition = Vector2.zero;
      component1.localScale = new Vector3(this.Flip.X ? -1f : 1f, this.Flip.Y ? -1f : 1f, 1f);
      this.RectTransform = component1;
    }
    CanvasGroup component2;
    if (!this.TryGetComponent<CanvasGroup>(out component2))
      return;
    component2.alpha = 0.0f;
    this.CanvasGroup = component2;
  }

  public Sequence Play()
  {
    return this.Animate().AppendCallback((TweenCallback) (() => Object.Destroy((Object) this.gameObject)));
  }

  public abstract Sequence Animate();

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__13_0() => Object.Destroy((Object) this.gameObject);
}
