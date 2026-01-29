// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Sirenix.OdinInspector;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIMenuBase : SerializedMonoBehaviour
{
  public const string kShownAnimationState = "Shown";
  public const string kShowAnimationState = "Show";
  public const string kHideAnimationState = "Hide";
  public const string kHiddenAnimationState = "Hidden";
  public const float kNoAnimatorFadeTime = 0.1f;
  public static List<UIMenuBase> ActiveMenus = new List<UIMenuBase>();
  public System.Action OnShow;
  public System.Action OnShown;
  public System.Action OnShownCompleted;
  public System.Action OnHide;
  public System.Action OnHidden;
  public System.Action OnCancel;
  public System.Action OnDestroyed;
  public System.Action OnDisabled;
  public static System.Action OnFirstMenuShow;
  public static System.Action OnFirstMenuShown;
  public static System.Action OnFinalMenuHide;
  public static System.Action OnFinalMenuHidden;
  [Header("Base Menu Components")]
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [Header("Navigation Defaults")]
  [SerializeField]
  public Selectable _defaultSelectable;
  [SerializeField]
  public Selectable[] _defaultSelectableFallbacks;
  public Canvas _canvas;
  public Selectable _cachedSelectable;
  [CompilerGenerated]
  public bool \u003CIsHiding\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsShowing\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CSetTimeScaleOnHidden\u003Ek__BackingField = true;

  public virtual bool _addToActiveMenus => true;

  public virtual bool _releaseOnHide => false;

  public bool IsHiding
  {
    set => this.\u003CIsHiding\u003Ek__BackingField = value;
    get => this.\u003CIsHiding\u003Ek__BackingField;
  }

  public bool IsShowing
  {
    set => this.\u003CIsShowing\u003Ek__BackingField = value;
    get => this.\u003CIsShowing\u003Ek__BackingField;
  }

  public CanvasGroup CanvasGroup => this._canvasGroup;

  public Canvas Canvas => this._canvas;

  public bool SetTimeScaleOnHidden
  {
    get => this.\u003CSetTimeScaleOnHidden\u003Ek__BackingField;
    set => this.\u003CSetTimeScaleOnHidden\u003Ek__BackingField = value;
  }

  public virtual void Awake()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnCancelDown += new System.Action(this.OnCancelButtonInput);
    Canvas component;
    if (!this.TryGetComponent<Canvas>(out component))
      return;
    this._canvas = component;
    this.UpdateSortingOrder();
  }

  public virtual void UpdateSortingOrder()
  {
    try
    {
      if (!((UnityEngine.Object) this._canvas != (UnityEngine.Object) null))
        return;
      if (UIMenuBase.ActiveMenus.Count > 0)
        this._canvas.sortingOrder = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>()._canvas.sortingOrder + 1;
      else
        this._canvas.sortingOrder = 100;
    }
    catch (Exception ex)
    {
    }
  }

  public void OnDisable()
  {
    System.Action onDisabled = this.OnDisabled;
    if (onDisabled == null)
      return;
    onDisabled();
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UINavigatorNew>.Instance.OnCancelDown -= new System.Action(this.OnCancelButtonInput);
    System.Action onDestroyed = this.OnDestroyed;
    if (onDestroyed == null)
      return;
    onDestroyed();
  }

  public virtual void Show(bool immediate = false)
  {
    this.IsShowing = true;
    if (this._addToActiveMenus && (UnityEngine.Object) this._canvas != (UnityEngine.Object) null && !UIMenuBase.ActiveMenus.Contains(this))
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
      if (this._addToActiveMenus && (UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
      {
        System.Action onFirstMenuShow = UIMenuBase.OnFirstMenuShow;
        if (onFirstMenuShow != null)
          onFirstMenuShow();
      }
      System.Action onShow = this.OnShow;
      if (onShow != null)
        onShow();
      if (this._addToActiveMenus && (UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
      {
        System.Action onFirstMenuShown = UIMenuBase.OnFirstMenuShown;
        if (onFirstMenuShown != null)
          onFirstMenuShown();
      }
      System.Action onShown = this.OnShown;
      if (onShown != null)
        onShown();
      this.OnShowCompleted();
      System.Action onShownCompleted = this.OnShownCompleted;
      if (onShownCompleted != null)
        onShownCompleted();
      InputManager.General.MouseInputEnabled = !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      Cursor.visible = InputManager.General.MouseInputEnabled && !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      foreach (MMControlPrompt componentsInChild in this.GetComponentsInChildren<MMControlPrompt>())
        componentsInChild.ForceUpdate();
      this.OnShowFinished();
      this.IsShowing = false;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow());
  }

  public virtual IEnumerator DoShow()
  {
    UIMenuBase uiMenuBase = this;
    yield return (object) null;
    uiMenuBase.SetActiveStateForMenu(true);
    uiMenuBase.OnShowStarted();
    if (uiMenuBase._addToActiveMenus && (UnityEngine.Object) uiMenuBase._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
    {
      System.Action onFirstMenuShow = UIMenuBase.OnFirstMenuShow;
      if (onFirstMenuShow != null)
        onFirstMenuShow();
    }
    System.Action onShow = uiMenuBase.OnShow;
    if (onShow != null)
      onShow();
    yield return (object) uiMenuBase.DoShowAnimation();
    if (uiMenuBase._addToActiveMenus && (UnityEngine.Object) uiMenuBase._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
    {
      System.Action onFirstMenuShown = UIMenuBase.OnFirstMenuShown;
      if (onFirstMenuShown != null)
        onFirstMenuShown();
    }
    System.Action onShown = uiMenuBase.OnShown;
    if (onShown != null)
      onShown();
    uiMenuBase.OnShowCompleted();
    InputManager.General.MouseInputEnabled = !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    Cursor.visible = InputManager.General.MouseInputEnabled && !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    foreach (MMControlPrompt componentsInChild in uiMenuBase.GetComponentsInChildren<MMControlPrompt>())
      componentsInChild.ForceUpdate();
    System.Action onShownCompleted = uiMenuBase.OnShownCompleted;
    if (onShownCompleted != null)
      onShownCompleted();
    uiMenuBase.OnShowFinished();
    uiMenuBase.IsShowing = false;
  }

  public virtual IEnumerator DoShowAnimation()
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

  public virtual void OnShowStarted()
  {
  }

  public virtual void OnShowCompleted()
  {
    Debug.Log((object) $"AutomationClient recived:{this.gameObject.name}_SHOWN");
    UnifyManager.Instance.AutomationClient.SendGameEvent(this.gameObject.name.Replace(" ", "_") + "_SHOWN");
  }

  public virtual void OnShowFinished()
  {
  }

  public void Hide(bool immediate = false)
  {
    this.IsHiding = true;
    this.OnHideStarted();
    if (this._addToActiveMenus && (UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Contains(this))
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
      if (this._addToActiveMenus && (UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
      {
        System.Action onFinalMenuHide = UIMenuBase.OnFinalMenuHide;
        if (onFinalMenuHide != null)
          onFinalMenuHide();
      }
      System.Action onHide = this.OnHide;
      if (onHide != null)
        onHide();
      this.gameObject.SetActive(false);
      if (this._addToActiveMenus && (UnityEngine.Object) this._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
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

  public virtual IEnumerator DoHide()
  {
    UIMenuBase uiMenuBase = this;
    if (uiMenuBase._addToActiveMenus && (UnityEngine.Object) uiMenuBase._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
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
    if (uiMenuBase._addToActiveMenus && (UnityEngine.Object) uiMenuBase._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
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

  public virtual IEnumerator DoHideAnimation()
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

  public virtual void OnHideStarted()
  {
  }

  public virtual void OnHideCompleted()
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
    if (menu._releaseOnHide)
    {
      // ISSUE: variable of a boxed type
      __Boxed<T> local = (object) menu;
      local.OnHide = local.OnHide + new System.Action(this.DoRelease);
    }
    else
    {
      // ISSUE: variable of a boxed type
      __Boxed<T> local = (object) menu;
      local.OnHidden = local.OnHidden + new System.Action(this.DoRelease);
    }
    this.OnPush();
    this.SetActiveStateForMenu(false);
    return menu;
  }

  public virtual void DoRelease()
  {
    this.OnRelease();
    if (this.IsHiding)
      return;
    this.SetActiveStateForMenu(true);
  }

  public virtual void OnPush()
  {
  }

  public virtual void OnRelease()
  {
  }

  public void ActivateNavigation()
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

  public void OverrideDefaultOnce(Selectable selectable) => this._cachedSelectable = selectable;

  public void OverrideDefault(Selectable selectable) => this._defaultSelectable = selectable;

  public bool WillProvideNavigation()
  {
    return (UnityEngine.Object) this._defaultSelectable != (UnityEngine.Object) null || this._defaultSelectableFallbacks.Length != 0 || (UnityEngine.Object) this._cachedSelectable != (UnityEngine.Object) null;
  }

  public virtual void SetActiveStateForMenu(bool state)
  {
    if (!((UnityEngine.Object) this._canvasGroup != (UnityEngine.Object) null))
      return;
    this._canvasGroup.interactable = state;
    this.SetActiveStateForMenu(this.gameObject, state);
    if (!this.WillProvideNavigation() || !state)
      return;
    this.ActivateNavigation();
  }

  public virtual void SetActiveStateForMenu(GameObject target, bool state)
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
