// Decompiled with JetBrains decompiler
// Type: SmartDrawer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartDrawer : MonoBehaviour
{
  public List<SmartDrawerAtom> smart_drawer_atoms = new List<SmartDrawerAtom>();
  public WorldGameObject _wgo;

  public void Update() => this.Redraw();

  public void Redraw(bool force = false)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      this._wgo = this.gameObject.transform.GetComponentInParent<WorldGameObject>();
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
        return;
    }
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
          case SmartDrawerAtom.SmartDrawingCondition.OnCrafting:
            flag = this._wgo.components.craft.is_crafting;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.ItemsEqual:
            flag = this._wgo.data.inventory.Count == smartDrawerAtom.int_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.ItemsMoreEqual:
            flag = this._wgo.data.inventory.Count >= smartDrawerAtom.int_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.GameResEqual:
            flag = (double) Mathf.Abs(this._wgo.data.GetParam(smartDrawerAtom.string_value) - smartDrawerAtom.float_value) < 1.0 / 1000.0;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.GameResMore:
            flag = (double) this._wgo.data.GetParam(smartDrawerAtom.string_value) > (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
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
          case SmartDrawerAtom.SmartDrawingCondition.ContainItem:
            using (List<Item>.Enumerator enumerator = this._wgo.data.inventory.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.id == smartDrawerAtom.string_value)
                {
                  flag = true;
                  break;
                }
              }
              goto case SmartDrawerAtom.SmartDrawingCondition.None;
            }
          case SmartDrawerAtom.SmartDrawingCondition.GameResBetween:
            float num1 = this._wgo.data.GetParam(smartDrawerAtom.string_value);
            flag = (double) smartDrawerAtom.float_value < (double) num1 && (double) num1 <= (double) smartDrawerAtom.float_value_2;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.GameResLess:
            flag = (double) this._wgo.data.GetParam(smartDrawerAtom.string_value) <= (double) smartDrawerAtom.float_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.ConcreteItemsBetween:
            int itemsCount1 = this._wgo.data.GetItemsCount(smartDrawerAtom.string_value);
            flag = smartDrawerAtom.int_value < itemsCount1 && itemsCount1 <= smartDrawerAtom.int_value_2;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.ConcreteItemsMore:
            int itemsCount2 = this._wgo.data.GetItemsCount(smartDrawerAtom.string_value);
            flag = smartDrawerAtom.int_value < itemsCount2;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.NoLinkedWorker:
            flag = !this._wgo.has_linked_worker;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.PorterStationStateIsNot:
            flag = !((UnityEngine.Object) this._wgo.porter_station == (UnityEngine.Object) null) && this._wgo.has_linked_worker && this._wgo.porter_station.state != (PorterStation.PorterState) smartDrawerAtom.int_value;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.HasLinkedWorker:
            flag = this._wgo.has_linked_worker;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.TotalItemsCount:
            int num2 = 0;
            foreach (Item obj in this._wgo.data.inventory)
              num2 += obj.value;
            flag = num2 >= smartDrawerAtom.int_value && num2 < smartDrawerAtom.int_value_2;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.PlayerHasItem:
            flag = MainGame.me.player.data.HasItemInInventory(smartDrawerAtom.string_value);
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.PlayerGameResEqual:
            flag = (double) Mathf.Abs(MainGame.me.player.data.GetParam(smartDrawerAtom.string_value) - smartDrawerAtom.float_value) < 1.0 / 1000.0;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.GDPointEnabled:
            flag = WorldMap.GetGDPointByGDTag(smartDrawerAtom.string_value).gameObject.activeSelf;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.GDPointDisabled:
            flag = !WorldMap.GetGDPointByGDTag(smartDrawerAtom.string_value).gameObject.activeSelf;
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          case SmartDrawerAtom.SmartDrawingCondition.HasItemInInventoryByIndex:
            Item itemByIndex = this._wgo.data.GetItemByIndex(smartDrawerAtom.int_value);
            flag = itemByIndex != null && !itemByIndex.IsEmpty();
            goto case SmartDrawerAtom.SmartDrawingCondition.None;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
  }
}
