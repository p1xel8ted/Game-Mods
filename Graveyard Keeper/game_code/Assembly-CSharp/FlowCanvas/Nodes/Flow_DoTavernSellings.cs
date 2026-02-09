// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DoTavernSellings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Do Tavern Sellings", 0)]
[Category("Game Actions")]
public class Flow_DoTavernSellings : MyFlowNode
{
  public float _reputation_earned;
  public float _money_earned;

  public override void RegisterPorts()
  {
    ValueInput<float> in_period = this.AddValueInput<float>("Period (days)");
    ValueInput<WorldGameObject> in_tavern_cashbox = this.AddValueInput<WorldGameObject>("tavern_cashbox");
    this.AddValueOutput<float>("reputation earned", (ValueHandler<float>) (() => this._reputation_earned));
    this.AddValueOutput<float>("money earned", (ValueHandler<float>) (() => this._money_earned));
    FlowOutput flow_out = this.AddFlowOutput("out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this._reputation_earned = 0.0f;
      this._money_earned = 0.0f;
      WorldZone zoneById = WorldZone.GetZoneByID("player_tavern_cellar");
      if ((UnityEngine.Object) zoneById == (UnityEngine.Object) null)
      {
        flow_out.Call(f);
      }
      else
      {
        List<Inventory> multiInventory = zoneById.GetMultiInventory();
        int[] numArray = new int[MainGame.me.save.players_tavern_engine.ITEMS_SELLING_IN_TAVERN.Length];
        float totalQuality = WorldZone.GetZoneByID("players_tavern").GetTotalQuality();
        int num1 = Mathf.RoundToInt(totalQuality * in_period.value);
        int num2 = num1;
        float num3 = 0.0f;
        foreach (Inventory inventory in multiInventory)
        {
          bool flag = false;
          for (int index1 = 0; index1 < inventory.data.inventory.Count; ++index1)
          {
            Item obj = inventory.data.inventory[index1];
            if (num1 == 0)
            {
              flag = true;
              break;
            }
            if (obj != null && !obj.IsEmpty() && obj.value != 0 && ((IEnumerable<string>) MainGame.me.save.players_tavern_engine.ITEMS_SELLING_IN_TAVERN).Contains<string>(obj.id))
            {
              int index2 = Array.IndexOf<string>(MainGame.me.save.players_tavern_engine.ITEMS_SELLING_IN_TAVERN, obj.id);
              if (num1 >= obj.value)
              {
                num1 -= obj.value;
                numArray[index2] += obj.value;
                num3 += (float) ((double) obj.value * (double) obj.definition.base_price * 1.0);
                inventory.data.inventory.RemoveAt(index1);
                --index1;
              }
              else
              {
                obj.value -= num1;
                numArray[index2] += num1;
                num3 += (float) ((double) num1 * (double) obj.definition.base_price * 1.0);
                num1 = 0;
              }
            }
          }
          if (flag)
            break;
        }
        if ((double) Mathf.Abs(num3) > 0.0099999997764825821)
        {
          float average_item_price = num3 / (float) (num2 - num1);
          num3 += PlayersTavernEngine.CalculateAlcoholSellingBonus(num3);
          this._reputation_earned = num3 * 0.01f * PlayersTavernEngine.CalculateCorrecterCoeff(totalQuality, average_item_price);
          this._money_earned = num3;
        }
        if ((UnityEngine.Object) in_tavern_cashbox.value != (UnityEngine.Object) null)
        {
          for (int index = 0; index < numArray.Length; ++index)
            in_tavern_cashbox.value.AddToParams(MainGame.me.save.players_tavern_engine.ITEMS_SELLING_IN_TAVERN[index].Replace(":", "_"), (float) numArray[index]);
        }
        Debug.Log((object) ($"Tavern Sellings done: total money: {num3}; sold items ={num2 - num1}; " + $"reputation earned: {this._reputation_earned}"));
        flow_out.Call(f);
      }
    }));
  }
}
