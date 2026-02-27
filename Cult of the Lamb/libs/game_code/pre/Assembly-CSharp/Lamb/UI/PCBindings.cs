// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PCBindings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class PCBindings : UISubmenuBase
{
  [SerializeField]
  private InputDisplay[] _controllers;
  [SerializeField]
  private KeybindItem[] _keybindItems;
  [SerializeField]
  private MMScrollRect _scrollRect;

  public override void Awake()
  {
    base.Awake();
    foreach (KeybindItem keybindItem1 in this._keybindItems)
    {
      KeybindItem keybindItem = keybindItem1;
      keybindItem.KeyboardBinding.BindingButton.onClick.AddListener((UnityAction) (() => this.OnBindingItemClicked(keybindItem.KeyboardBinding)));
      keybindItem.MouseBinding.BindingButton.onClick.AddListener((UnityAction) (() => this.OnBindingItemClicked(keybindItem.MouseBinding)));
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
  }
}
