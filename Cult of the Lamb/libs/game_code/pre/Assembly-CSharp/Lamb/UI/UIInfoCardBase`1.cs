// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIInfoCardBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasGroup))]
public abstract class UIInfoCardBase<T> : MonoBehaviour
{
  private const string kShownGenericAnimationState = "Shown";
  private const string kHiddenGenericAnimationState = "Hidden";
  private const string kShowTrigger = "Show";
  private const string kHideTrigger = "Hide";
  [SerializeField]
  private Animator _animator;

  public RectTransform RectTransform { private set; get; }

  public CanvasGroup CanvasGroup { private set; get; }

  public Animator Animator => this._animator;

  public void Awake()
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

  protected virtual void DoShow(bool instant)
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

  protected virtual void DoHide(bool instant)
  {
    if (instant)
      this._animator.Play("Hidden");
    else
      this._animator.SetTrigger("Hide");
  }

  private void ResetTriggers()
  {
    if (!((Object) this._animator != (Object) null))
      return;
    this._animator.ResetTrigger("Show");
    this._animator.ResetTrigger("Hide");
  }

  public abstract void Configure(T config);
}
