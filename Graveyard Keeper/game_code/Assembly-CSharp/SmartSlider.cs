// Decompiled with JetBrains decompiler
// Type: SmartSlider
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SmartSlider : MonoBehaviour
{
  public const int GAMEPAD_FAST_SLIDER_CHANGE_VALUE = 10;
  public UILabel min_counter;
  public UILabel max_counter;
  public UILabel _input_label;
  public UIInput _input_field;
  public Collider2D _input_collider;
  public UISlider _slider;
  public int _min;
  public int _max;
  public int _step_for_game_keys = 1;
  public int _prev_value;
  public bool _input_field_enabled;
  public bool _game_keys_enabled;
  public Action<int> _on_value_changed;
  public bool auto_steps;

  public int value
  {
    get => this._min + Mathf.RoundToInt((float) (this._max - this._min) * this._slider.value);
  }

  public bool input_field_enabled
  {
    get => this._input_field_enabled;
    set => this._input_field_enabled = value;
  }

  public bool game_keys_enabled
  {
    get => this._game_keys_enabled;
    set => this._game_keys_enabled = value;
  }

  public int number_of_steps
  {
    get => this._slider.numberOfSteps;
    set => this._slider.numberOfSteps = value;
  }

  public void Init()
  {
    this._slider = this.GetComponentInChildren<UISlider>(true);
    this._slider.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnSliderChanged)));
    this._input_field = this.GetComponentInChildren<UIInput>(true);
    this._input_label = this._input_field.GetComponent<UILabel>();
    this._input_collider = this._input_field.GetComponent<Collider2D>();
    this._input_field.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputFieldChanged)));
    this._input_field.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputFieldSubmit)));
  }

  public void Open(
    int value,
    int min,
    int max,
    Action<int> on_value_changed,
    bool input_field_enabled = false,
    bool game_keys_enabled = false,
    int step_for_game_keys = 1)
  {
    this._min = min;
    this._max = max;
    this._prev_value = -9999;
    this._slider.numberOfSteps = this.auto_steps ? this._max - this._min : 0;
    if ((UnityEngine.Object) this.min_counter != (UnityEngine.Object) null)
      this.min_counter.text = this._min.ToString();
    if ((UnityEngine.Object) this.max_counter != (UnityEngine.Object) null)
      this.max_counter.text = this._max.ToString();
    this._on_value_changed = on_value_changed;
    this._input_collider.enabled = this._input_field_enabled = input_field_enabled;
    this._game_keys_enabled = game_keys_enabled;
    this._step_for_game_keys = step_for_game_keys != 0 ? step_for_game_keys : 1;
    this.SetValue(value, false);
    this._input_field.value = this._input_label.text = value.ToString();
  }

  public void SetValue(int new_value, bool invoke_callback = true)
  {
    if (new_value < this._min)
      new_value = this._min;
    if (new_value > this._max)
      new_value = this._max;
    this._slider.value = (float) (new_value - this._min) / (float) (this._max - this._min);
    if (!invoke_callback || this._on_value_changed == null)
      return;
    this._on_value_changed(this.value);
  }

  public void OnSliderChanged()
  {
    if (this._prev_value == this.value)
      return;
    this._prev_value = this.value;
    string str = this.value.ToString();
    if (this._input_field_enabled)
    {
      if (this._input_field.value != str && !this._input_field.isSelected)
        this._input_field.value = str;
    }
    else
      this._input_label.text = str;
    if (this._on_value_changed != null)
      this._on_value_changed(this.value);
    if (Sounds.WasAnySoundPlayedThisFrame())
      return;
    Sounds.OnGUIClick();
  }

  public void OnInputFieldChanged()
  {
    if (!this._input_field_enabled)
      return;
    int result = 0;
    if (!int.TryParse(this._input_field.value, out result))
      return;
    if (result < this._min)
      result = this._min;
    if (result > this._max)
      result = this._max;
    if (result == this.value)
      return;
    this.SetValue(result);
  }

  public void OnInputFieldSubmit()
  {
    if (!this._input_field_enabled)
      return;
    this._input_field.isSelected = false;
    this.OnSliderChanged();
  }

  public void Update()
  {
    if (!this._game_keys_enabled || this._input_field.isSelected)
      return;
    if (LazyInput.GetKeyDown(GameKey.SliderInc))
      this.ChangeValue(this._step_for_game_keys);
    else if (LazyInput.GetKeyDown(GameKey.SliderDec))
      this.ChangeValue(-this._step_for_game_keys);
    else if (LazyInput.GetKeyDown(GameKey.PrevTab))
    {
      this.ChangeValue(-10);
    }
    else
    {
      if (!LazyInput.GetKeyDown(GameKey.NextTab))
        return;
      this.ChangeValue(10);
    }
  }

  public void ChangeValue(int delta) => this.SetValue(this.value + delta);
}
