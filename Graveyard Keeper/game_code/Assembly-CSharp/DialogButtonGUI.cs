// Decompiled with JetBrains decompiler
// Type: DialogButtonGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DialogButtonGUI : MonoBehaviour
{
  public UILabel _label;
  public UIButton _button;
  public DialogButtonsGUI _gui;
  public int _overflow_width;
  public UILabel.Overflow _overflow;
  public UIWidget[] _widgets;
  public int _anchor_top;
  public int _anchor_bottom;

  public void Init(DialogButtonsGUI gui)
  {
    this._gui = gui;
    this._label = this.GetComponentInChildren<UILabel>();
    this._button = this.GetComponentInChildren<UIButton>();
    this._widgets = this.GetComponentsInChildren<UIWidget>();
    this._anchor_top = this._label.topAnchor.absolute;
    this._anchor_bottom = this._label.bottomAnchor.absolute;
    this._overflow = this._label.overflowMethod;
    this._overflow_width = this._label.overflowWidth;
  }

  public void SetText(string text, bool translate = true)
  {
    this._label.overflowMethod = this._overflow;
    this._label.overflowWidth = this._overflow_width;
    bool flag = string.IsNullOrEmpty(text);
    this.SetActive(!flag);
    this.SetEnabled(!flag);
    this._label.text = !translate || flag ? text : GJL.L(text);
  }

  public int GetWidth() => !this.gameObject.activeSelf ? 0 : this._label.width;

  public void SetWidth(int width)
  {
    this._label.overflowMethod = UILabel.Overflow.ResizeHeight;
    this._label.width = width;
    foreach (UIRect widget in this._widgets)
      widget.UpdateAnchors();
  }

  public void SetEnabled(bool active) => this._button.isEnabled = active;

  public void SetActive(bool active) => this.gameObject.SetActive(active);

  public void OnClick() => this._gui.OnBtnClicked(this);

  public void RestoreHeight()
  {
    if ((Object) this._label == (Object) null || this._label.topAnchor == null || this._label.bottomAnchor == null)
      return;
    this._label.topAnchor.absolute = this._anchor_top;
    this._label.bottomAnchor.absolute = this._anchor_bottom;
  }
}
