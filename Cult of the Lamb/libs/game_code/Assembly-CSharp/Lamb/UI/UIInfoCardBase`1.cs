// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIInfoCardBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasGroup))]
public abstract class UIInfoCardBase<T> : MonoBehaviour
{
  public const string kShownGenericAnimationState = "Shown";
  public const string kHiddenGenericAnimationState = "Hidden";
  public const string kShowTrigger = "Show";
  public const string kHideTrigger = "Hide";
  [SerializeField]
  public Animator _animator;
  [CompilerGenerated]
  public RectTransform \u003CRectTransform\u003Ek__BackingField;
  [CompilerGenerated]
  public CanvasGroup \u003CCanvasGroup\u003Ek__BackingField;

  public RectTransform RectTransform
  {
    set => this.\u003CRectTransform\u003Ek__BackingField = value;
    get => this.\u003CRectTransform\u003Ek__BackingField;
  }

  public CanvasGroup CanvasGroup
  {
    set => this.\u003CCanvasGroup\u003Ek__BackingField = value;
    get => this.\u003CCanvasGroup\u003Ek__BackingField;
  }

  public Animator Animator => this._animator;

  public virtual void Awake()
  {
    this.RectTransform = this.GetComponent<RectTransform>();
    this.CanvasGroup = this.GetComponent<CanvasGroup>();
  }

  public void Show(T config, bool instant = false)
  {
    this.Configure(config);
    this.Show(instant);
  }

  public void Show(bool instant = false)
  {
    this.ResetTriggers();
    this.DoShow(instant);
  }

  public void ForceAnimate() => this._animator.SetTrigger("Show");

  public virtual void DoShow(bool instant)
  {
    if (instant)
      this._animator.Play("Shown");
    else
      this._animator.SetTrigger("Show");
  }

  public void Hide(bool instant = false)
  {
    this.ResetTriggers();
    this.DoHide(instant);
  }

  public virtual void DoHide(bool instant)
  {
    if (instant)
      this._animator.Play("Hidden");
    else
      this._animator.SetTrigger("Hide");
  }

  public void ResetTriggers()
  {
    if (!((Object) this._animator != (Object) null))
      return;
    this._animator.ResetTrigger("Show");
    this._animator.ResetTrigger("Hide");
  }

  public abstract void Configure(T config);
}
