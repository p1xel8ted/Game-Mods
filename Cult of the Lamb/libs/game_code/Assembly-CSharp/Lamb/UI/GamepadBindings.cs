// Decompiled with JetBrains decompiler
// Type: Lamb.UI.GamepadBindings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using src.Extensions;
using src.UINavigator;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class GamepadBindings : UISubmenuBase
{
  [SerializeField]
  public BindingConflictResolver _bindingCOnflictResolver;
  [SerializeField]
  public InputDisplay[] _controllers;
  [SerializeField]
  public Selectable _selectable;
  [SerializeField]
  public KeybindItem[] _keybindItems;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public GameObject _controlPrompts;

  public override void Awake()
  {
    base.Awake();
    foreach (KeybindItem keybindItem1 in this._keybindItems)
    {
      KeybindItem keybindItem = keybindItem1;
      keybindItem.Button.onClick.AddListener((UnityAction) (() =>
      {
        if (!keybindItem.IsRebindable)
          return;
        this.OnBindingItemClicked(keybindItem.JoystickBinding);
      }));
      keybindItem.KeybindConflictLookup.Configure(this._bindingCOnflictResolver);
    }
  }

  public override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = true;
  }

  public void Configure(InputType inputType)
  {
    foreach (InputDisplay controller in this._controllers)
      controller.Configure(inputType);
  }

  public void OnBindingItemClicked(BindingItem bindingItem)
  {
    UIControlBindingOverlayController menu = MonoSingleton<UIManager>.Instance.BindingOverlayControllerTemplate.Instantiate<UIControlBindingOverlayController>();
    menu.Show(this._bindingCOnflictResolver, bindingItem.BindingTerm, bindingItem.Category, bindingItem.Action, bindingItem.AxisContribution, bindingItem.ControllerType);
    this.PushInstance<UIControlBindingOverlayController>(menu);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this._controlPrompts.SetActive(false);
    UIControlBindingOverlayController overlayController = menu;
    overlayController.OnHide = overlayController.OnHide + (System.Action) (() => this._controlPrompts.SetActive(true));
  }

  public Selectable ProvideSelectableForLayoutSelector() => this._selectable;

  [CompilerGenerated]
  public void \u003COnBindingItemClicked\u003Eb__9_0() => this._controlPrompts.SetActive(true);
}
