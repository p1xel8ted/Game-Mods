// Decompiled with JetBrains decompiler
// Type: SimpleOptionsSwitcher
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SimpleOptionsSwitcher : MonoBehaviour
{
  [HideInInspector]
  public UILabel label;
  public int _current_option_index;
  public int _max_option_index;
  public bool _game_keys_enabled;
  public Action<int, UILabel> _on_changed;

  public bool game_keys_enabled
  {
    get => this._game_keys_enabled;
    set => this._game_keys_enabled = value;
  }

  public void Init(
    int current_option_index,
    int max_option_index,
    string current_option_name,
    Action<int, UILabel> on_changed,
    bool call_onchanged_on_init = false)
  {
    if ((UnityEngine.Object) this.label == (UnityEngine.Object) null)
      this.label = this.GetComponentInChildren<UILabel>(true);
    this.label.text = current_option_name;
    this._current_option_index = current_option_index;
    this._max_option_index = max_option_index;
    this._on_changed = on_changed;
    if (!call_onchanged_on_init)
      return;
    this._on_changed(this._current_option_index, this.label);
  }

  public void Dec()
  {
    if (--this._current_option_index < 0)
      this._current_option_index = this._max_option_index;
    if (this._on_changed != null)
      this._on_changed(this._current_option_index, this.label);
    Debug.Log((object) ("new index = " + this._current_option_index.ToString()));
  }

  public void Inc()
  {
    if (++this._current_option_index > this._max_option_index)
      this._current_option_index = 0;
    if (this._on_changed != null)
      this._on_changed(this._current_option_index, this.label);
    Debug.Log((object) ("new index = " + this._current_option_index.ToString()));
  }

  public void Update()
  {
    if (!this._game_keys_enabled)
      return;
    if (LazyInput.GetKeyDown(GameKey.SliderInc))
    {
      this.Inc();
    }
    else
    {
      if (!LazyInput.GetKeyDown(GameKey.SliderDec))
        return;
      this.Dec();
    }
  }
}
