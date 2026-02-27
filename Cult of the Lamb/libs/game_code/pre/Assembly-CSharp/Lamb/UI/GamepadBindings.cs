// Decompiled with JetBrains decompiler
// Type: Lamb.UI.GamepadBindings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class GamepadBindings : UISubmenuBase
{
  [SerializeField]
  private InputDisplay[] _controllers;
  [SerializeField]
  private Selectable _selectable;
  [SerializeField]
  private KeybindItem[] _keybindItems;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private GameObject _controlPrompts;

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
      keybindItem.KeybindConflictLookup.Configure(this._keybindItems);
    }
  }

  protected override void OnShowStarted()
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

  private void OnBindingItemClicked(BindingItem bindingItem)
  {
    UIControlBindingOverlayController menu = MonoSingleton<UIManager>.Instance.BindingOverlayControllerTemplate.Instantiate<UIControlBindingOverlayController>();
    menu.Show(bindingItem.BindingTerm, bindingItem.Category, bindingItem.Action, bindingItem.AxisContribution, bindingItem.ControllerType);
    this.PushInstance<UIControlBindingOverlayController>(menu);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this._controlPrompts.SetActive(false);
    UIControlBindingOverlayController overlayController = menu;
    overlayController.OnHide = overlayController.OnHide + (System.Action) (() => this._controlPrompts.SetActive(true));
  }

  public Selectable ProvideSelectableForLayoutSelector() => this._selectable;
}
