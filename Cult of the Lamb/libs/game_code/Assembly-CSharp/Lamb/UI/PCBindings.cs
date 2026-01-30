// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PCBindings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using src.Extensions;
using src.UINavigator;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class PCBindings : UISubmenuBase
{
  [SerializeField]
  public BindingConflictResolver _bindingCOnflictResolver;
  [SerializeField]
  public InputDisplay[] _controllers;
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
      keybindItem.KeyboardBinding.BindingButton.onClick.AddListener((UnityAction) (() => this.OnBindingItemClicked(keybindItem.KeyboardBinding)));
      keybindItem.MouseBinding.BindingButton.onClick.AddListener((UnityAction) (() => this.OnBindingItemClicked(keybindItem.MouseBinding)));
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

  [CompilerGenerated]
  public void \u003COnBindingItemClicked\u003Eb__8_0() => this._controlPrompts.SetActive(true);
}
