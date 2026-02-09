// Decompiled with JetBrains decompiler
// Type: EnvironmentEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

#nullable disable
[Serializable]
public class EnvironmentEngine : MonoBehaviour
{
  public static bool log_weather;
  public static EnvironmentEngine _me;
  public List<WeatherState> _states;
  public TimeOfDay time_of_day;
  public float _cur_time = 0.72f;
  public float day_time_period = 9f;
  public bool _auto_adjust_time = true;
  public bool _is_rainy;
  public SmartWeatherState[] states;
  public bool weather_is_forced;
  public EnvironmentEngine.EnvironmentEngineData data = new EnvironmentEngine.EnvironmentEngineData();
  public Texture2D lut_inside;
  public LUTMorpher lut;
  public AmplifyColorEffect lut_effect_timeofday;
  [NonSerialized]
  public static EnvironmentPreset cur_preset;

  public static EnvironmentEngine me
  {
    get
    {
      return EnvironmentEngine._me ?? (EnvironmentEngine._me = UnityEngine.Object.FindObjectOfType<EnvironmentEngine>());
    }
  }

  public bool auto_adjust_time => this._auto_adjust_time;

  public bool is_rainy => this._is_rainy;

  public void Awake()
  {
    EnvironmentEngine._me = this;
    this._states = ((IEnumerable<WeatherState>) this.GetComponentsInChildren<WeatherState>(true)).ToList<WeatherState>();
  }

  public void Init()
  {
    Debug.Log((object) "EnvironmentEngine.Init()");
    MainGame.me.grain_fx_component = MainGame.me.GetComponent<NoiseAndGrain>();
    this.data = new EnvironmentEngine.EnvironmentEngineData();
    this.data.lut_controller = MainGame.me.gameObject.AddComponent<LUTController>();
    this.data.lut_controller.InitLUTController();
    this.ResetStates();
    this.FindStateByType(SmartWeatherState.WeatherType.Fog).controller.FindParameterOfType(SmartControllerParameter.Action.AbstractControllerCmp).abs_cmp = (AbstractControllerComponent) Fog.me;
  }

  public void Update()
  {
    if (MainGame.game_starting || MainGame.paused || !MainGame.game_started)
      return;
    float deltaTime = Time.deltaTime;
    if (!this.IsTimeStopped())
    {
      this._cur_time += deltaTime / 225f;
      if ((UnityEngine.Object) this.time_of_day != (UnityEngine.Object) null)
        this.time_of_day.time_of_day = this._cur_time;
      if ((double) this._cur_time > 1.0)
      {
        this._cur_time -= 2f;
        if ((UnityEngine.Object) this.time_of_day != (UnityEngine.Object) null)
          this.time_of_day.time_of_day = this._cur_time;
        this.OnEndOfDay();
      }
    }
    this.UpdateCurEnvironmentPreset();
    this.UpdateWeather();
    this._is_rainy = false;
    foreach (WeatherState state in this._states)
    {
      state.StateUpdate(deltaTime);
      if (state.type == WeatherState.WeatherType.Rain && (double) state.amount > 0.7)
        this._is_rainy = true;
    }
  }

  public void OnEndOfDay()
  {
    ++MainGame.me.save.day;
    if ((UnityEngine.Object) MainGame.me.gui_elements.hud != (UnityEngine.Object) null)
      MainGame.me.gui_elements.hud.OnEndOfDay();
    MainGame.me.save.quests.CheckKeyQuests("end_of_day");
    SmartWeatherEngine.me.UpdateWeather();
    WorldMap.VendorsTradeWithBank();
    string[] strArray = new string[2]
    {
      "graveyard",
      "church"
    };
    foreach (string str in strArray)
    {
      WorldZone zoneById = WorldZone.GetZoneByID(str);
      if ((UnityEngine.Object) zoneById == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Not found zone by name " + str));
      }
      else
      {
        float totalQuality = zoneById.GetTotalQuality();
        float num = MainGame.me.player.GetParam(str + "_qual");
        float f = totalQuality - num;
        Stats.ResourceEvent((double) f > 0.0, str, Mathf.Abs(f), "quality");
        MainGame.me.player.SetParam(str + "_qual", totalQuality);
      }
    }
    Stats.DesignEvent("Day");
  }

  public void SetEngineGlobalState(EnvironmentEngine.State engine_state)
  {
    if (this.data == null)
    {
      Debug.LogError((object) "Data of EnvironmentEngine is null", (UnityEngine.Object) this);
    }
    else
    {
      this.data.state = engine_state;
      this.UpdateCurEnvironmentPreset();
    }
  }

  public void UpdateCurEnvironmentPreset()
  {
    switch (this.data.state)
    {
      case EnvironmentEngine.State.RealTime:
        if (!((UnityEngine.Object) EnvironmentEngine.cur_preset != (UnityEngine.Object) null) || !(EnvironmentEngine.cur_preset.name == "inside"))
          break;
        this.ApplyEnvironmentPreset((EnvironmentPreset) null);
        break;
      case EnvironmentEngine.State.Inside:
        if (!((UnityEngine.Object) EnvironmentEngine.cur_preset == (UnityEngine.Object) null))
          break;
        this.ApplyEnvironmentPreset(EnvironmentPreset.Load("inside"));
        break;
    }
  }

  public void ChangeEnvironment(string teleport_tag)
  {
    Debug.Log((object) ("Change Environment, teleport_tag = " + teleport_tag));
    string[] parts = teleport_tag.Split('_');
    if (parts.Length >= 3 && parts[0] == "tp")
      MainGame.me.save.SetEnvironmentPreset(parts);
    else
      Debug.LogError((object) ("Unknown teleport_tag format: " + teleport_tag));
  }

  public void ApplyEnvironmentPreset(EnvironmentPreset preset)
  {
    float morph_speed = 4f;
    bool flag = true;
    if ((UnityEngine.Object) preset != (UnityEngine.Object) null)
    {
      morph_speed = preset.lut_morph_speed;
      flag = !preset.disable_timeofday_lut;
      if (preset.name.Contains("_none_"))
        preset = (EnvironmentPreset) null;
    }
    EnvironmentEngine.cur_preset = preset;
    this.lut.SetMainLUT((UnityEngine.Object) preset == (UnityEngine.Object) null ? (Texture) null : preset.lut, morph_speed);
    this.lut_effect_timeofday.enabled = flag;
    this.UpdateCurEnvironmentPreset();
    this.Update();
    this.time_of_day.Update();
  }

  public static void SetTime(float time)
  {
    if (!Application.isPlaying || (UnityEngine.Object) EnvironmentEngine._me == (UnityEngine.Object) null)
      return;
    EnvironmentEngine._me._cur_time = time;
  }

  public void EnableTime(bool enable)
  {
    Debug.Log((object) ("EnableTime: " + enable.ToString()));
    this._auto_adjust_time = enable;
  }

  public bool IsTimeStopped()
  {
    if (!this._auto_adjust_time)
      return true;
    return MainGame.me.player_char != null && !MainGame.me.player_char.control_enabled;
  }

  public void ResetStates()
  {
    foreach (SmartWeatherState state in this.states)
    {
      if (!((UnityEngine.Object) state == (UnityEngine.Object) null))
        state.value = 0.0f;
    }
    this.data.nature_weather_line = new List<SwitchableWeatherState>();
    this.data.local_weather_line = new List<SwitchableWeatherState>();
    this.data.forced_weather_line = new List<ForcedWeatherState>();
    this.data.lut_line = new List<LUTAtom>();
    this.UpdateWeather();
  }

  public void DisableAutoTime() => this._auto_adjust_time = false;

  public void SetWeatherEnabled(bool enable)
  {
    foreach (SmartWeatherState state in this.states)
      state.SetEnabled(enable);
  }

  public SmartWeatherState FindStateByType(SmartWeatherState.WeatherType type)
  {
    if (type == SmartWeatherState.WeatherType.LUT)
      return (SmartWeatherState) null;
    foreach (SmartWeatherState state in this.states)
    {
      if (state.type == type)
        return state;
    }
    Debug.LogError((object) ("Couldn't find a weather state: " + type.ToString()));
    return (SmartWeatherState) null;
  }

  public void UpdateWeather()
  {
    if (!MainGame.game_started || this.data == null)
    {
      if (this.data != null)
        return;
      Debug.LogError((object) "EnvEngine: Data is null", (UnityEngine.Object) this);
    }
    else
    {
      if (this.data.forced_weather_line == null)
        this.data.forced_weather_line = new List<ForcedWeatherState>();
      if (this.data.local_weather_line == null)
        this.data.local_weather_line = new List<SwitchableWeatherState>();
      if (this.data.nature_weather_line == null)
        this.data.nature_weather_line = new List<SwitchableWeatherState>();
      this.data.lut_line = new List<LUTAtom>();
      if ((UnityEngine.Object) this.data.lut_controller == (UnityEngine.Object) null)
        this.data.lut_controller = MainGame.me.GetComponent<LUTController>();
      if (this.data.state == EnvironmentEngine.State.Inside && this.data.prev_state == EnvironmentEngine.State.RealTime)
        this.SetWeatherEnabled(false);
      else if (this.data.state == EnvironmentEngine.State.RealTime && this.data.prev_state == EnvironmentEngine.State.Inside)
        this.SetWeatherEnabled(true);
      this.UpdateNatureWeatherLine();
      this.ApplyNatureWeatherState();
      this.UpdateLocalWeatherLine();
      if (this.data.state == EnvironmentEngine.State.Inside)
        this.data.lut_line = new List<LUTAtom>();
      this.UpdateForcedWeatherLine();
      if (this.data.state == EnvironmentEngine.State.Inside)
      {
        foreach (SmartWeatherState state in this.states)
          state.SetValueImmediate(0.0f);
      }
      if (this.data.prev_state != this.data.state)
      {
        foreach (SmartWeatherState state in this.states)
          state.SetValueImmediate(state.value);
      }
      if (this.weather_is_forced)
      {
        this.data.lut_line = new List<LUTAtom>();
        foreach (SmartWeatherState state in this.states)
          state.SetValueImmediate(state.forced_value);
      }
      else
      {
        foreach (SmartWeatherState state in this.states)
          state.forced_value = state.value;
      }
      this.data.lut_controller.UpdateColorEffects(this.data.lut_line);
      this.data.prev_state = this.data.state;
    }
  }

  public bool UpdateForcedWeatherLine()
  {
    if (this.data.forced_weather_line.Count == 0)
      return false;
    bool flag = false;
    float gameTime = MainGame.game_time;
    List<ForcedWeatherState> forcedWeatherStateList = new List<ForcedWeatherState>();
    foreach (ForcedWeatherState w_state in this.data.forced_weather_line)
    {
      if ((double) w_state.t_start < (double) gameTime)
      {
        SmartWeatherState stateByType = this.FindStateByType(w_state.type);
        if ((double) w_state.t_start + (double) w_state.t_atk > (double) gameTime)
        {
          float num = (gameTime - w_state.t_start) / w_state.t_atk;
          if (w_state.type == SmartWeatherState.WeatherType.LUT)
          {
            this.InsertLUTAtomInLUTLine(num, (WeatherStateBase) w_state);
          }
          else
          {
            stateByType.value = Mathf.Lerp(stateByType.value, w_state.value, num);
            flag = true;
          }
        }
        else if ((double) w_state.t_start + (double) w_state.t_atk + (double) w_state.t_flat > (double) gameTime)
        {
          if (w_state.type == SmartWeatherState.WeatherType.LUT)
          {
            this.InsertLUTAtomInLUTLine(1f, (WeatherStateBase) w_state);
          }
          else
          {
            stateByType.value = w_state.value;
            flag = true;
          }
        }
        else if ((double) w_state.t_start + (double) w_state.t_atk + (double) w_state.t_flat + (double) w_state.t_dec > (double) gameTime)
        {
          float t = (gameTime - w_state.t_start - w_state.t_atk - w_state.t_flat) / w_state.t_dec;
          if (w_state.type == SmartWeatherState.WeatherType.LUT)
            this.InsertLUTAtomInLUTLine(1f - t, (WeatherStateBase) w_state);
          else
            stateByType.value = Mathf.Lerp(w_state.value, stateByType.value, t);
          flag = true;
        }
        else
        {
          forcedWeatherStateList.Add(w_state);
          if (EnvironmentEngine.log_weather)
            Debug.Log((object) $"#weather#Remove forced state [{w_state.type.ToString()}]");
        }
      }
    }
    if (forcedWeatherStateList.Count > 0)
    {
      foreach (ForcedWeatherState forcedWeatherState in forcedWeatherStateList)
        this.data.forced_weather_line.Remove(forcedWeatherState);
    }
    return flag;
  }

  public bool UpdateLocalWeatherLine()
  {
    return this.UpdateSwitchableWeatherLine(this.data.local_weather_line);
  }

  public bool UpdateSwitchableWeatherLine(
    List<SwitchableWeatherState> switchable_weather_line,
    bool is_nature = false)
  {
    if (switchable_weather_line.Count == 0)
      return false;
    bool flag = false;
    float gameTime = MainGame.game_time;
    for (int index = 0; index < switchable_weather_line.Count && (double) switchable_weather_line[index].t_start <= (double) gameTime; ++index)
    {
      SwitchableWeatherState w_state = switchable_weather_line[index];
      SmartWeatherState stateByType = this.FindStateByType(w_state.type);
      if ((double) w_state.start_removing_time > 1.0)
      {
        if ((double) w_state.start_removing_time + (double) w_state.t_dec < (double) gameTime)
        {
          switchable_weather_line.RemoveAt(index);
          --index;
          if (EnvironmentEngine.log_weather)
          {
            Debug.Log((object) $"#weather#Removed local weather state from line [{w_state.preset_name}]");
            continue;
          }
          continue;
        }
        if (!w_state.do_dec_now && (double) w_state.start_removing_time < (double) gameTime && (double) w_state.t_start + (double) w_state.t_atk > (double) gameTime)
        {
          float num = (gameTime - w_state.t_start) / w_state.t_atk;
          w_state.start_removing_time = gameTime - (1f - num) * w_state.t_dec;
          w_state.do_dec_now = true;
        }
        if (w_state.do_dec_now || (double) w_state.start_removing_time < (double) gameTime)
        {
          float t = (gameTime - w_state.start_removing_time) / w_state.t_dec;
          if (w_state.type == SmartWeatherState.WeatherType.LUT)
          {
            this.InsertLUTAtomInLUTLine(1f - t, (WeatherStateBase) w_state);
            continue;
          }
          if (is_nature)
            stateByType.nature_value = Mathf.Lerp(w_state.value, stateByType.nature_value, t);
          else
            stateByType.value = Mathf.Lerp(w_state.value, stateByType.value, t);
          flag = true;
          continue;
        }
      }
      if ((double) w_state.t_start + (double) w_state.t_atk > (double) gameTime)
      {
        float num = (gameTime - w_state.t_start) / w_state.t_atk;
        if (w_state.type == SmartWeatherState.WeatherType.LUT)
        {
          this.InsertLUTAtomInLUTLine(num, (WeatherStateBase) w_state);
        }
        else
        {
          if (is_nature)
            stateByType.nature_value = Mathf.Lerp(stateByType.nature_value, w_state.value, num);
          else
            stateByType.value = Mathf.Lerp(stateByType.value, w_state.value, num);
          flag = true;
        }
      }
      else if (w_state.type == SmartWeatherState.WeatherType.LUT)
      {
        this.InsertLUTAtomInLUTLine(1f, (WeatherStateBase) w_state);
      }
      else
      {
        if (is_nature)
          stateByType.nature_value = w_state.value;
        else
          stateByType.value = w_state.value;
        flag = true;
      }
    }
    return flag;
  }

  public bool UpdateNatureWeatherLine()
  {
    return this.UpdateSwitchableWeatherLine(this.data.nature_weather_line, true);
  }

  public void ApplyNatureWeatherState()
  {
    foreach (SmartWeatherState state in this.states)
      state.value = state.nature_value;
  }

  public void AddNatureWeatherState(SwitchableWeatherState n_state)
  {
    this.AddSwitchableWeatherState(n_state, this.data.nature_weather_line);
  }

  public void AddLocalWeatherState(SwitchableWeatherState l_state)
  {
    this.AddSwitchableWeatherState(l_state, this.data.local_weather_line);
  }

  public void AddSwitchableWeatherState(
    SwitchableWeatherState sw_state,
    List<SwitchableWeatherState> switchable_weather_line)
  {
    if (switchable_weather_line == null)
      switchable_weather_line = new List<SwitchableWeatherState>();
    if (sw_state == null)
    {
      Debug.LogError((object) "Switchable state is null!");
    }
    else
    {
      if (switchable_weather_line.Count == 0)
      {
        switchable_weather_line.Add(sw_state);
      }
      else
      {
        float gameTime = MainGame.game_time;
        if ((double) Mathf.Abs(sw_state.t_start - gameTime) < 1.0 / 1000.0)
        {
          foreach (SwitchableWeatherState switchableWeatherState in switchable_weather_line)
          {
            if (!(switchableWeatherState.preset_name != sw_state.preset_name) && switchableWeatherState.do_dec_now)
            {
              float num = (gameTime - switchableWeatherState.start_removing_time) / switchableWeatherState.t_dec;
              switchableWeatherState.t_start = gameTime - (1f - num) * switchableWeatherState.t_atk;
              switchableWeatherState.do_dec_now = false;
              switchableWeatherState.start_removing_time = -1f;
              switchableWeatherState.t_dec = 0.0f;
              if (!EnvironmentEngine.log_weather)
                return;
              Debug.Log((object) $"#weather#Changed [{switchableWeatherState.type.ToString()}] switchable state to ATK");
              return;
            }
          }
        }
        bool flag = false;
        for (int index = 0; index < switchable_weather_line.Count; ++index)
        {
          if ((double) sw_state.t_start < (double) switchable_weather_line[index].t_start)
          {
            switchable_weather_line.Insert(index, sw_state);
            flag = true;
            break;
          }
        }
        if (!flag)
          switchable_weather_line.Add(sw_state);
      }
      if (!EnvironmentEngine.log_weather)
        return;
      Debug.Log((object) $"#weather#Inserted switchable weather state to line {sw_state?.ToString()} [{(switchable_weather_line.IndexOf(sw_state) + 1).ToString()} of {switchable_weather_line.Count.ToString()}]");
    }
  }

  public bool TryRemoveLocalWeatherState(
    string preset_name,
    float start_removing_time,
    float dec_time)
  {
    return this.TryRemoveSwitchableWeatherState(this.data.local_weather_line, preset_name, start_removing_time, dec_time);
  }

  public bool TryRemoveNatureWeatherState(
    string preset_name,
    float start_removing_time,
    float dec_time)
  {
    return this.TryRemoveSwitchableWeatherState(this.data.nature_weather_line, preset_name, start_removing_time, dec_time);
  }

  public bool TryRemoveSwitchableWeatherState(
    List<SwitchableWeatherState> switchable_weather_line,
    string preset_name,
    float start_removing_time,
    float dec_time)
  {
    if (switchable_weather_line == null)
    {
      switchable_weather_line = new List<SwitchableWeatherState>();
      return false;
    }
    if (string.IsNullOrEmpty(preset_name))
    {
      Debug.LogError((object) "Preset name is null!");
      return false;
    }
    if (switchable_weather_line.Count == 0)
      return false;
    int num = 0;
    foreach (SwitchableWeatherState switchableWeatherState in switchable_weather_line)
    {
      if (switchableWeatherState.preset_name == preset_name && !switchableWeatherState.HasRemoveCommand())
      {
        switchableWeatherState.t_dec = dec_time;
        switchableWeatherState.start_removing_time = start_removing_time;
        ++num;
        if (EnvironmentEngine.log_weather)
          Debug.Log((object) $"#weather#Added comand: removing [{num.ToString()}] switchable weather state from line [{preset_name}], start_removing_time = {start_removing_time.ToString()}, dec_time = {dec_time.ToString()}");
      }
    }
    if (num != 0)
      return true;
    if (EnvironmentEngine.log_weather)
      Debug.Log((object) $"#weather#Error removing switchable weather state from line [{preset_name}]");
    return false;
  }

  public void AddForcedWeatherState(ForcedWeatherState f_state)
  {
    if (this.data.forced_weather_line == null)
      this.data.forced_weather_line = new List<ForcedWeatherState>();
    if (f_state == null)
    {
      Debug.LogError((object) "Forced state is null!");
    }
    else
    {
      if (this.data.forced_weather_line.Count == 0)
      {
        this.data.forced_weather_line.Add(f_state);
      }
      else
      {
        bool flag = false;
        for (int index = 0; index < this.data.forced_weather_line.Count; ++index)
        {
          if ((double) f_state.t_start < (double) this.data.forced_weather_line[index].t_start)
          {
            this.data.forced_weather_line.Insert(index, f_state);
            flag = true;
            break;
          }
        }
        if (!flag)
          this.data.forced_weather_line.Add(f_state);
      }
      if (!EnvironmentEngine.log_weather)
        return;
      Debug.Log((object) $"#weather#Inserted forced weather state to line [{(this.data.forced_weather_line.IndexOf(f_state) + 1).ToString()} of {this.data.forced_weather_line.Count.ToString()}]");
    }
  }

  public bool TryGetLUTAtomFromLUTLine(string texture_name, out LUTAtom found_lut_atom)
  {
    found_lut_atom = (LUTAtom) null;
    if (this.data.lut_line == null)
    {
      Debug.LogError((object) "LUT Line is null!");
      this.data.lut_line = new List<LUTAtom>();
      return false;
    }
    if (this.data.lut_line.Count == 0)
      return false;
    foreach (LUTAtom lutAtom in this.data.lut_line)
    {
      if (lutAtom.lut_name == texture_name)
      {
        found_lut_atom = lutAtom;
        return true;
      }
    }
    return false;
  }

  public void InsertLUTAtomInLUTLine(float value_k, WeatherStateBase w_state)
  {
    LUTAtom found_lut_atom;
    if (this.TryGetLUTAtomFromLUTLine(w_state.lut_texture.name, out found_lut_atom))
      found_lut_atom.value = w_state.value * value_k;
    else
      this.data.lut_line.Add(new LUTAtom()
      {
        lut_texture = w_state.lut_texture,
        lut_name = w_state.lut_texture.name,
        value = w_state.value * value_k
      });
  }

  public List<string> FindPresetsWithoutRemoves(
    List<SwitchableWeatherState> switchable_weather_line)
  {
    List<string> presetsWithoutRemoves = new List<string>();
    foreach (SwitchableWeatherState switchableWeatherState in switchable_weather_line)
    {
      if (!switchableWeatherState.HasRemoveCommand() && !presetsWithoutRemoves.Contains(switchableWeatherState.preset_name))
        presetsWithoutRemoves.Add(switchableWeatherState.preset_name);
    }
    return presetsWithoutRemoves;
  }

  public List<string> FindNatureWithoutRemoves()
  {
    return this.FindPresetsWithoutRemoves(this.data.nature_weather_line);
  }

  public void PrepareForSave()
  {
    if (this.states == null || this.states.Length == 0)
      this.Awake();
    this.data.serialized_states = new List<SmartWeatherState.SerializedWeatherState>();
    foreach (SmartWeatherState state in this.states)
      this.data.serialized_states.Add(state.Serialize());
  }

  public void DeserializeData(
    EnvironmentEngine.EnvironmentEngineData loaded_data)
  {
    if (loaded_data == null)
      return;
    this.data = loaded_data;
    this.data.lut_controller = MainGame.me.GetComponent<LUTController>();
    this._auto_adjust_time = true;
    if (this.states == null || this.states.Length == 0)
      this.Awake();
    foreach (SmartWeatherState.SerializedWeatherState serializedState in loaded_data.serialized_states)
    {
      bool flag = false;
      foreach (SmartWeatherState state in this.states)
      {
        if (state.name == serializedState.name)
        {
          flag = true;
          state.Deserialize(serializedState);
          break;
        }
      }
      if (!flag)
        Debug.LogError((object) ("Couldn't find a weather state with name = " + serializedState.name));
    }
  }

  public void StoreEnvironmentEngineState()
  {
    this.data.stored_state = new EnvironmentEngine.State?(this.data.state);
  }

  public void RestoreEnvironmentEngineState()
  {
    if (!this.data.stored_state.HasValue)
      return;
    this.data.state = this.data.stored_state.Value;
    this.data.stored_state = new EnvironmentEngine.State?();
  }

  [Serializable]
  public class EnvironmentEngineData
  {
    public EnvironmentEngine.State state;
    public EnvironmentEngine.State prev_state;
    public List<SwitchableWeatherState> nature_weather_line;
    public List<SwitchableWeatherState> local_weather_line;
    public List<ForcedWeatherState> forced_weather_line;
    public List<LUTAtom> lut_line;
    [SmartDontSerialize]
    public LUTController lut_controller;
    public List<SmartWeatherState.SerializedWeatherState> serialized_states = new List<SmartWeatherState.SerializedWeatherState>();
    public EnvironmentEngine.State? stored_state;
  }

  public enum State
  {
    RealTime,
    Inside,
  }
}
