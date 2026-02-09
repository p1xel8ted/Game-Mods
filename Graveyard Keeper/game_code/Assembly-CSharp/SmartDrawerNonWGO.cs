// Decompiled with JetBrains decompiler
// Type: SmartDrawerNonWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartDrawerNonWGO : MonoBehaviour
{
  public List<SmartDrawerAtom> smart_drawer_atoms = new List<SmartDrawerAtom>();

  public void Update() => this.Redraw();

  public void Redraw(bool force = false)
  {
    foreach (SmartDrawerAtom smartDrawerAtom in this.smart_drawer_atoms)
    {
      if (force || smartDrawerAtom.CheckNeedRecalc(Time.deltaTime))
      {
        bool flag = false;
        switch (smartDrawerAtom.condition)
        {
          case SmartDrawerAtom.SmartDrawingCondition.None:
            smartDrawerAtom.obj.gameObject.SetActive(flag);
            continue;
          case SmartDrawerAtom.SmartDrawingCondition.DayTime_Day:
            flag = !TimeOfDay.me.is_night;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.RainLess:
            flag = (double) EnvironmentEngine.me.FindStateByType(SmartWeatherState.WeatherType.Rain).value <= (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.RainMore:
            flag = (double) EnvironmentEngine.me.FindStateByType(SmartWeatherState.WeatherType.Rain).value > (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.WindLess:
            flag = (double) EnvironmentEngine.me.FindStateByType(SmartWeatherState.WeatherType.Wind).value <= (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.WindMore:
            flag = (double) EnvironmentEngine.me.FindStateByType(SmartWeatherState.WeatherType.Rain).value > (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.FogLess:
            flag = (double) EnvironmentEngine.me.FindStateByType(SmartWeatherState.WeatherType.Rain).value <= (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.FogMore:
            flag = (double) EnvironmentEngine.me.FindStateByType(SmartWeatherState.WeatherType.Rain).value > (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.DayTime_Night:
            flag = TimeOfDay.me.is_night;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.PlayerHasItem:
            flag = MainGame.me.player.data.HasItemInInventory(smartDrawerAtom.string_value);
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.PlayerGameResEqual:
            flag = (double) Mathf.Abs(MainGame.me.player.data.GetParam(smartDrawerAtom.string_value) - smartDrawerAtom.float_value) < 1.0 / 1000.0;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          default:
            Debug.Log((object) "ERROR TRYING TO USE WGO CONDITION IN NON WGO SMART DRAWER");
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
        }
      }
    }
  }
}
