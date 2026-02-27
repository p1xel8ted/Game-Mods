// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMTabNavigatorBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class MMTabNavigatorBase<T> : MonoBehaviour where T : MMTab
{
  public Action<int> OnTabChanged;
  [SerializeField]
  private UIMenuBase _uiMenuController;
  [SerializeField]
  protected int _defaultTabIndex;
  [SerializeField]
  protected T[] _tabs;
  [SerializeField]
  private GameObject _changeTabLeft;
  [SerializeField]
  private GameObject _changeTabRight;
  private T _currentTab;

  public UIMenuBase CurrentMenu => this._currentTab.Menu;

  public int NumTabs => this._tabs.Length;

  public int CurrentMenuIndex => this._tabs.IndexOf<T>(this._currentTab);

  public int DefaultTabIndex
  {
    get => this._defaultTabIndex;
    set => this._defaultTabIndex = value;
  }

  protected virtual void Start()
  {
    foreach (T tab1 in this._tabs)
    {
      T tab = tab1;
      tab.Configure();
      // ISSUE: variable of a boxed type
      __Boxed<T> local = (object) tab;
      local.OnTabPressed = local.OnTabPressed + (System.Action) (() => this.TransitionTo(tab));
      tab.Menu.OnHide += (System.Action) (() =>
      {
        IMMSelectable currentSelectable = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable;
        if (currentSelectable == null || !currentSelectable.Selectable.transform.IsChildOf(tab.Menu.transform))
          return;
        MonoSingleton<UINavigatorNew>.Instance.Clear();
      });
    }
    this._uiMenuController.OnShow += new System.Action(this.OnMenuShow);
    this._uiMenuController.OnHide += new System.Action(this.OnMenuHide);
  }

  protected virtual void OnMenuShow() => this.ShowDefault();

  protected virtual void OnMenuHide()
  {
  }

  public void SetNavigationVisibility(bool visibility)
  {
    this._changeTabLeft.SetActive(visibility);
    this._changeTabRight.SetActive(visibility);
  }

  public virtual void ShowDefault()
  {
    if (!this._tabs[this._defaultTabIndex].Button.interactable)
    {
      int num = -1;
      for (int index = 0; index < this._tabs.Length; ++index)
      {
        if (this._tabs[index].Button.interactable && num == -1)
          num = index;
      }
      this._defaultTabIndex = num;
    }
    this.SetDefaultTab(this._tabs[this._defaultTabIndex]);
  }

  protected void SetDefaultTab(T tab)
  {
    this._currentTab = tab;
    this._currentTab.Menu.Show(true);
    this._uiMenuController.OnShow -= new System.Action(this.OnMenuShow);
  }

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateLeft += new System.Action(this.NavigatePageLeft);
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateRight += new System.Action(this.NavigatePageRight);
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateLeft -= new System.Action(this.NavigatePageLeft);
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateRight -= new System.Action(this.NavigatePageRight);
  }

  private void TransitionTo(T tab)
  {
    if (!((UnityEngine.Object) this._currentTab != (UnityEngine.Object) tab))
      return;
    this.PerformTransitionTo(this._currentTab, tab);
    this._currentTab = tab;
  }

  protected virtual void PerformTransitionTo(T from, T to)
  {
    from.Menu.Hide();
    to.Menu.Show();
  }

  public bool TryNavigatePageLeft()
  {
    int index = this._tabs.IndexOf<T>(this._currentTab);
    while (index-- > 0)
    {
      if (this._tabs[index].gameObject.activeInHierarchy && this._tabs[index].Button.interactable)
      {
        this._tabs[index].Button.TryPerformConfirmAction();
        Action<int> onTabChanged = this.OnTabChanged;
        if (onTabChanged != null)
          onTabChanged(index);
        return true;
      }
    }
    return false;
  }

  private void NavigatePageLeft()
  {
    if (!this._uiMenuController.CanvasGroup.interactable)
      return;
    this.TryNavigatePageLeft();
  }

  public bool TryNavigatePageRight()
  {
    int index = this._tabs.IndexOf<T>(this._currentTab);
    while (index++ < this._tabs.Length - 1)
    {
      if (this._tabs[index].gameObject.activeInHierarchy && this._tabs[index].Button.interactable)
      {
        this._tabs[index].Button.TryPerformConfirmAction();
        Action<int> onTabChanged = this.OnTabChanged;
        if (onTabChanged != null)
          onTabChanged(index);
        return true;
      }
    }
    return false;
  }

  public bool CanNavigateLeft()
  {
    if ((UnityEngine.Object) this._currentTab == (UnityEngine.Object) null)
      return false;
    int index = this._tabs.IndexOf<T>(this._currentTab);
    while (index-- > 0)
    {
      if (this._tabs[index].gameObject.activeInHierarchy && this._tabs[index].Button.interactable)
        return true;
    }
    return false;
  }

  public bool CanNavigateRight()
  {
    if ((UnityEngine.Object) this._currentTab == (UnityEngine.Object) null)
      return false;
    int index = this._tabs.IndexOf<T>(this._currentTab);
    while (index++ < this._tabs.Length - 1)
    {
      if (this._tabs[index].gameObject.activeInHierarchy && this._tabs[index].Button.interactable)
        return true;
    }
    return false;
  }

  private void NavigatePageRight()
  {
    if (!this._uiMenuController.CanvasGroup.interactable)
      return;
    this.TryNavigatePageRight();
  }
}
