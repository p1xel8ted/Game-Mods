// Decompiled with JetBrains decompiler
// Type: GamepadSelectableButton
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GamepadSelectableButton : MonoBehaviour
{
  public GameObject gamepad_frame;
  [HideInInspector]
  public GamepadNavigationItem navigation_item;
  [HideInInspector]
  public UIButton ui_button;
  public GJCommons.VoidDelegate _on_press;
  public GJCommons.VoidDelegate _on_over;
  public Color _normal_color;
  public bool _inited;

  public void Init()
  {
    this.ui_button = this.GetComponentInChildren<UIButton>();
    this._normal_color = this.ui_button.defaultColor;
    this.navigation_item = this.GetComponent<GamepadNavigationItem>();
    this.navigation_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnButtonSelect));
    this.gamepad_frame.Deactivate();
    this._inited = true;
  }

  public void SetCallbacks(GJCommons.VoidDelegate on_press, GJCommons.VoidDelegate on_over)
  {
    if (!this._inited)
      this.Init();
    this.gamepad_frame.Deactivate();
    this._on_press = on_press;
    this._on_over = on_over;
  }

  public void OnButtonSelect()
  {
    if (!this.ui_button.isEnabled)
      return;
    this._on_press.TryInvoke();
  }

  public void OnOver()
  {
    this.gamepad_frame.Activate();
    this._on_over.TryInvoke();
  }

  public void OnOut() => this.gamepad_frame.Deactivate();

  public void SetEnabled(bool enabled)
  {
    if (enabled)
      this.ui_button.defaultColor = this._normal_color;
    this.ui_button.tweenTarget.GetComponent<UIWidget>().color = enabled ? this.ui_button.defaultColor : this.ui_button.disabledColor;
    this.ui_button.isEnabled = enabled;
  }
}
