// Decompiled with JetBrains decompiler
// Type: DialogButtonsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DialogButtonsGUI : MonoBehaviour
{
  public List<DialogButtonGUI> _buttons;
  public UITable _table;
  public ButtonTipsStr _button_tips;
  public PlatformDependentElement[] platform_dependent_elements;
  public List<string> _texts = new List<string>();
  public List<GJCommons.VoidDelegate> _delegates = new List<GJCommons.VoidDelegate>();
  public List<GameKey> _game_keys = new List<GameKey>();
  public List<bool> _enables = new List<bool>();
  public bool _initialized;

  public void Init()
  {
    if (this._initialized)
      return;
    this._buttons = ((IEnumerable<DialogButtonGUI>) this.GetComponentsInChildren<DialogButtonGUI>(true)).ToList<DialogButtonGUI>();
    foreach (DialogButtonGUI button in this._buttons)
      button.Init(this);
    this.platform_dependent_elements = this.GetComponentsInChildren<PlatformDependentElement>(true);
    this._button_tips = this.GetComponentInChildren<ButtonTipsStr>(true);
    this._table = this.GetComponentInChildren<UITable>(true);
    this._initialized = true;
  }

  public void Update()
  {
    for (int index = 0; index < this._game_keys.Count; ++index)
    {
      if (this._enables[index] && LazyInput.GetKeyDown(this._game_keys[index]))
      {
        LazyInput.ClearKeyDown(this._game_keys[index]);
        this.InvokeOption(index);
        break;
      }
    }
  }

  public void Set(
    string text_1,
    GJCommons.VoidDelegate delegate_1,
    string text_2 = null,
    GJCommons.VoidDelegate delegate_2 = null,
    string text_3 = null,
    GJCommons.VoidDelegate delegate_3 = null,
    GameKey key_1 = GameKey.Select,
    GameKey key_2 = GameKey.Back)
  {
    this.Init();
    this._texts.Clear();
    this._delegates.Clear();
    this._game_keys.Clear();
    this._texts.Add(text_1);
    if (!string.IsNullOrEmpty(text_2))
      this._texts.Add(text_2);
    if (!string.IsNullOrEmpty(text_3))
      this._texts.Add(text_3);
    this._delegates.Add(delegate_1);
    if (delegate_2 != null)
      this._delegates.Add(delegate_2);
    if (delegate_3 != null)
      this._delegates.Add(delegate_3);
    this._game_keys.Add(key_1);
    this._game_keys.Add(key_2);
    this.SetEnabled();
    this.Redraw();
  }

  public void SetEnabled(bool enable_1 = true, bool enable_2 = true, bool enable_3 = true)
  {
    this.Init();
    this._enables.Clear();
    this._enables.Add(enable_1);
    this._enables.Add(enable_2);
    this._enables.Add(enable_3);
    this.Redraw();
    this._buttons[0].SetEnabled(enable_1);
    if (this._buttons.Count < 2)
      return;
    this._buttons[1].SetEnabled(enable_2);
  }

  public void Redraw()
  {
    this._button_tips.SetActive(BaseGUI.for_gamepad);
    this._table.SetActive(!BaseGUI.for_gamepad);
    if (BaseGUI.for_gamepad)
    {
      List<GameKeyTip> tips = new List<GameKeyTip>();
      for (int index = 0; index < this._texts.Count; ++index)
        tips.Add(new GameKeyTip(this._game_keys[index], this._texts[index], this._enables[index]));
      this._button_tips.Print(tips);
    }
    else
    {
      for (int index = 0; index < 3; ++index)
      {
        if (index < this._buttons.Count)
        {
          if (index >= this._texts.Count)
          {
            this._buttons[index].Deactivate<DialogButtonGUI>();
          }
          else
          {
            this._buttons[index].Activate<DialogButtonGUI>();
            this._buttons[index].SetText(this._texts[index]);
            this._buttons[index].SetEnabled(this._enables[index]);
          }
        }
      }
      if (this._texts.Count == 1)
        this._table.Reposition();
      else if (this._buttons == null || this._buttons.Count == 0)
      {
        Debug.LogError((object) "No buttons found", (UnityEngine.Object) this);
      }
      else
      {
        int width1 = this._buttons.Select<DialogButtonGUI, int>((Func<DialogButtonGUI, int>) (button => button.GetWidth())).Max();
        foreach (DialogButtonGUI button in this._buttons)
        {
          int width2 = button.GetWidth();
          if (width2 > width1)
            width1 = width2;
        }
        foreach (DialogButtonGUI button in this._buttons)
          button.SetWidth(width1);
        this._table.Reposition();
      }
    }
  }

  public void OnBtnClicked(DialogButtonGUI btn) => this.InvokeOption(this._buttons.IndexOf(btn));

  public void InvokeOption(int index)
  {
    if (index < 0)
      return;
    if (index >= this._delegates.Count)
      Debug.LogError((object) $"button delegate index is too large = {index.ToString()}, delegates = {this._delegates.Count.ToString()}", (UnityEngine.Object) this);
    else
      this._delegates[index].TryInvoke();
  }
}
