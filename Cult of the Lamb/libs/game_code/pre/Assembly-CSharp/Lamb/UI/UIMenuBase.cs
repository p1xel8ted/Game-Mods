// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIMenuBase : MonoBehaviour
{
  protected const string kShownAnimationState = "Shown";
  protected const string kShowAnimationState = "Show";
  protected const string kHideAnimationState = "Hide";
  protected const string kHiddenAnimationState = "Hidden";
  private const float kNoAnimatorFadeTime = 0.1f;
  public static readonly List<UIMenuBase> ActiveMenus = new List<UIMenuBase>();
  public System.Action OnShow;
  public System.Action OnShown;
  public System.Action OnHide;
  public System.Action OnHidden;
  public System.Action OnCancel;
  public static System.Action OnFirstMenuShow;
  public static System.Action OnFirstMenuShown;
  public static System.Action OnFinalMenuHide;
  public static System.Action OnFinalMenuHidden;
  [Header("Base Menu Components")]
  [SerializeField]
  protected Animator _animator;
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [Header("Navigation Defaults")]
  [SerializeField]
  private Selectable _defaultSelectable;
  [SerializeField]
  private Selectable[] _defaultSelectableFallbacks;
  protected Canvas _canvas;
  private Selectable _cachedSelectable;

  public bool IsHiding { private set; get; }

  public bool IsShowing { private set; get; }

  public CanvasGroup CanvasGroup => this._canvasGroup;

  public Canvas Canvas => this._canvas;

  public virtual void Awake()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnCancelDown += new System.Action(this.OnCancelButtonInput);
    Canvas component;
    if (!this.TryGetComponent<Canvas>(out component))
      return;
    this._canvas = component;
    this.UpdateSortingOrder();
  }

  protected virtual void UpdateSortingOrder()
  {
    if (!((UnityEngine.Object) this._canvas != (UnityEngine.Object) null))
      return;
    if (UIMenuBase.ActiveMenus.Count > 0)
      this._canvas.sortingOrder = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>()._canvas.sortingOrder + 1;
    else
      this._canvas.sortingOrder = 100;
  }

  protected virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnCancelDown -= new System.Action(this.OnCancelButtonInput);
  }

  public void Show(bool immediate = false)
  {
    this.IsShowing = true;
    if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && !UIMenuBase.ActiveMenus.Contains(this))
    {
      UIMenuBase.ActiveMenus.Add(this);
      this.UpdateSortingOrder();
    }
    this.gameObject.SetActive(true);
    this.StopAllCoroutines();
    if (immediate)
    {
      if ((UnityEngine.Object) this._animator != (UnityEngine.Object) null)
        this._animator.Play("Shown");
      else
        this._canvasGroup.alpha = 1f;
      this.SetActiveStateForMenu(true);
      this.OnShowStarted();
      if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
      {
        System.Action onFirstMenuShow = UIMenuBase.OnFirstMenuShow;
        if (onFirstMenuShow != null)
          onFirstMenuShow();
      }
      System.Action onShow = this.OnShow;
      if (onShow != null)
        onShow();
      if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
      {
        System.Action onFirstMenuShown = UIMenuBase.OnFirstMenuShown;
        if (onFirstMenuShown != null)
          onFirstMenuShown();
      }
      System.Action onShown = this.OnShown;
      if (onShown != null)
        onShown();
      this.OnShowCompleted();
      this.IsShowing = false;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow());
  }

  protected virtual IEnumerator DoShow()
  {
    yield return (object) null;
    this.SetActiveStateForMenu(true);
    this.OnShowStarted();
    if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
    {
      System.Action onFirstMenuShow = UIMenuBase.OnFirstMenuShow;
      if (onFirstMenuShow != null)
        onFirstMenuShow();
    }
    System.Action onShow = this.OnShow;
    if (onShow != null)
      onShow();
    yield return (object) this.DoShowAnimation();
    if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
    {
      System.Action onFirstMenuShown = UIMenuBase.OnFirstMenuShown;
      if (onFirstMenuShown != null)
        onFirstMenuShown();
    }
    System.Action onShown = this.OnShown;
    if (onShown != null)
      onShown();
    this.OnShowCompleted();
    this.IsShowing = false;
  }

  protected virtual IEnumerator DoShowAnimation()
  {
    if ((UnityEngine.Object) this._animator != (UnityEngine.Object) null)
    {
      yield return (object) this._animator.YieldForAnimation("Show");
    }
    else
    {
      while ((double) this._canvasGroup.alpha < 1.0)
      {
        this._canvasGroup.alpha += Time.unscaledDeltaTime * 10f;
        yield return (object) null;
      }
    }
  }

  protected virtual void OnShowStarted()
  {
  }

  protected virtual void OnShowCompleted()
  {
  }

  public void Hide(bool immediate = false)
  {
    this.IsHiding = true;
    this.OnHideStarted();
    if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Contains(this))
    {
      UIMenuBase.ActiveMenus.Remove(this);
      if (UIMenuBase.ActiveMenus.Count == 0)
        MonoSingleton<UINavigatorNew>.Instance.Clear();
    }
    this.StopAllCoroutines();
    if (immediate)
    {
      if ((UnityEngine.Object) this._animator != (UnityEngine.Object) null)
        this._animator.Play("Hidden");
      else
        this._canvasGroup.alpha = 0.0f;
      if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
      {
        System.Action onFinalMenuHide = UIMenuBase.OnFinalMenuHide;
        if (onFinalMenuHide != null)
          onFinalMenuHide();
      }
      System.Action onHide = this.OnHide;
      if (onHide != null)
        onHide();
      this.gameObject.SetActive(false);
      if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
      {
        System.Action onFinalMenuHidden = UIMenuBase.OnFinalMenuHidden;
        if (onFinalMenuHidden != null)
          onFinalMenuHidden();
      }
      System.Action onHidden = this.OnHidden;
      if (onHidden != null)
        onHidden();
      this.OnHideCompleted();
      this.SetActiveStateForMenu(false);
      this.IsHiding = false;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  protected virtual IEnumerator DoHide()
  {
    UIMenuBase uiMenuBase = this;
    if ((UnityEngine.Object) uiMenuBase._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
    {
      System.Action onFinalMenuHide = UIMenuBase.OnFinalMenuHide;
      if (onFinalMenuHide != null)
        onFinalMenuHide();
    }
    System.Action onHide = uiMenuBase.OnHide;
    if (onHide != null)
      onHide();
    uiMenuBase.SetActiveStateForMenu(false);
    yield return (object) uiMenuBase.DoHideAnimation();
    if ((UnityEngine.Object) uiMenuBase._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
    {
      System.Action onFinalMenuHidden = UIMenuBase.OnFinalMenuHidden;
      if (onFinalMenuHidden != null)
        onFinalMenuHidden();
    }
    uiMenuBase.gameObject.SetActive(false);
    System.Action onHidden = uiMenuBase.OnHidden;
    if (onHidden != null)
      onHidden();
    uiMenuBase.OnHideCompleted();
    uiMenuBase.IsHiding = false;
  }

  protected virtual IEnumerator DoHideAnimation()
  {
    if ((UnityEngine.Object) this._animator != (UnityEngine.Object) null)
    {
      yield return (object) this._animator.YieldForAnimation("Hide");
    }
    else
    {
      while ((double) this._canvasGroup.alpha > 0.0)
      {
        this._canvasGroup.alpha -= Time.unscaledDeltaTime * 10f;
        yield return (object) null;
      }
    }
  }

  protected virtual void OnHideStarted()
  {
  }

  protected virtual void OnHideCompleted()
  {
  }

  public virtual T Push<T>(T menu) where T : UIMenuBase
  {
    T menu1 = menu.Instantiate<T>();
    menu1.Show(false);
    return this.PushInstance<T>(menu1);
  }

  public virtual T PushInstance<T>(T menu) where T : UIMenuBase
  {
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null)
      this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    // ISSUE: variable of a boxed type
    __Boxed<T> local = (object) menu;
    local.OnHide = local.OnHide + new System.Action(this.DoRelease);
    this.OnPush();
    this.SetActiveStateForMenu(false);
    return menu;
  }

  protected virtual void DoRelease()
  {
    this.OnRelease();
    if (this.IsHiding)
      return;
    this.SetActiveStateForMenu(true);
  }

  protected virtual void OnPush()
  {
  }

  protected virtual void OnRelease()
  {
  }

  protected void ActivateNavigation()
  {
    Selectable newSelectable = (UnityEngine.Object) this._cachedSelectable != (UnityEngine.Object) null ? this._cachedSelectable : this._defaultSelectable;
    if ((UnityEngine.Object) newSelectable == (UnityEngine.Object) null || !newSelectable.interactable || !newSelectable.gameObject.activeInHierarchy)
    {
      for (int index = 0; index < this._defaultSelectableFallbacks.Length; ++index)
      {
        if (this._defaultSelectableFallbacks[index].gameObject.activeSelf && this._defaultSelectableFallbacks[index].interactable && this._defaultSelectableFallbacks[index].gameObject.activeInHierarchy)
        {
          newSelectable = this._defaultSelectableFallbacks[index];
          break;
        }
      }
    }
    if ((UnityEngine.Object) newSelectable == (UnityEngine.Object) null)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(newSelectable as IMMSelectable);
    this._cachedSelectable = (Selectable) null;
  }

  protected void OverrideDefaultOnce(Selectable selectable) => this._cachedSelectable = selectable;

  protected void OverrideDefault(Selectable selectable) => this._defaultSelectable = selectable;

  private bool WillProvideNavigation()
  {
    return (UnityEngine.Object) this._defaultSelectable != (UnityEngine.Object) null || this._defaultSelectableFallbacks.Length != 0 || (UnityEngine.Object) this._cachedSelectable != (UnityEngine.Object) null;
  }

  protected virtual void SetActiveStateForMenu(bool state)
  {
    this._canvasGroup.interactable = state;
    this.SetActiveStateForMenu(this.gameObject, state);
    if (!this.WillProvideNavigation() || !state)
      return;
    this.ActivateNavigation();
  }

  protected virtual void SetActiveStateForMenu(GameObject target, bool state)
  {
    foreach (IMMSelectable componentsInChild in target.GetComponentsInChildren<IMMSelectable>())
      componentsInChild.SetInteractionState(state);
    foreach (Behaviour componentsInChild in target.GetComponentsInChildren<MMScrollRect>())
      componentsInChild.enabled = state;
  }

  public virtual void OnCancelButtonInput()
  {
  }
}
