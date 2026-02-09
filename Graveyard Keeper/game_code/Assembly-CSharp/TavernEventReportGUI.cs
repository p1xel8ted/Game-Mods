// Decompiled with JetBrains decompiler
// Type: TavernEventReportGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TavernEventReportGUI : DialogGUI
{
  public PrayReportItemGUI row_prefab;
  public UITableOrGrid rows_table;

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override bool OnPressedSelect()
  {
    this.OnClosePressed();
    return true;
  }

  public void OpenPlayersTavernEventResult(
    WorldGameObject barmen_wgo,
    TavernEventDefinition tavern_event)
  {
    if ((UnityEngine.Object) barmen_wgo == (UnityEngine.Object) null)
      Debug.LogError((object) "OpenPlayersTavernEventResult error: barmen_wgo is null!");
    else if (tavern_event == null)
    {
      Debug.LogError((object) "OpenPlayersTavernEventResult error: tavern_event is null!");
    }
    else
    {
      this.OpenOK(tavern_event.id + "_header");
      this.row_prefab.gameObject.SetActive(false);
      this.rows_table.DestroyChildren<PrayReportItemGUI>(new PrayReportItemGUI[1]
      {
        this.row_prefab
      });
      int num1 = Mathf.RoundToInt(tavern_event.max_food_sold.EvaluateFloat(barmen_wgo, MainGame.me.player));
      TavernEventDefinition.TavernEventType type = tavern_event.type;
      float num2 = 0.0f;
      float num3 = 0.0f;
      while (num1 > 0 && barmen_wgo.data.inventory.Count != 0)
      {
        Item obj1 = (Item) null;
        foreach (Item obj2 in barmen_wgo.data.inventory)
        {
          if (obj2?.definition != null && obj2.definition.can_insert_into_barmen)
          {
            obj1 = obj2;
            break;
          }
        }
        if (obj1 != null)
        {
          float tavernEventCoeff = obj1.definition.tavern_event_coeffs[(int) type];
          if (barmen_wgo.data.inventory.Count > 1)
          {
            for (int index = 1; index < barmen_wgo.data.inventory.Count; ++index)
            {
              Item obj3 = barmen_wgo.data.inventory[index];
              if (obj3?.definition != null && obj3.definition.can_insert_into_barmen && (double) obj3.definition.tavern_event_coeffs[(int) type] > (double) tavernEventCoeff)
              {
                obj1 = obj3;
                tavernEventCoeff = obj3.definition.tavern_event_coeffs[(int) type];
              }
            }
          }
          string itemName = obj1.definition.GetItemName();
          if (obj1.definition.quality_type == ItemDefinition.QualityType.Stars)
          {
            itemName += " ";
            switch (Mathf.RoundToInt(obj1.definition.quality))
            {
              case 1:
                itemName += "(s1)";
                break;
              case 2:
                itemName += "(s2)";
                break;
              case 3:
                itemName += "(s3)";
                break;
            }
          }
          string str1 = Trading.FormatMoney(obj1.definition.base_price * tavernEventCoeff);
          string str2 = (double) tavernEventCoeff > 0.85000002384185791 ? "(:-))" : ((double) tavernEventCoeff < 0.64999997615814209 ? "(:-()" : "(:-|)");
          int count;
          if (num1 > obj1.value)
          {
            count = obj1.value;
            num1 -= obj1.value;
            barmen_wgo.data.RemoveItem(obj1);
          }
          else
          {
            count = num1;
            num1 = 0;
            barmen_wgo.data.RemoveItem(obj1, count);
          }
          num2 += obj1.definition.base_price * tavernEventCoeff * (float) count;
          num3 += tavernEventCoeff * (float) count;
          this.DrawResultRow(itemName, $"{count.ToString()} x {str1} {str2}");
        }
        else
          break;
      }
      string empty = string.Empty;
      float num4;
      string key;
      switch (type)
      {
        case TavernEventDefinition.TavernEventType.Aclofest:
          num4 = 60f;
          key = "dlc_stories_good_alcoparty";
          break;
        case TavernEventDefinition.TavernEventType.Standup:
          num4 = 80f;
          key = "dlc_stories_good_stand_up";
          break;
        case TavernEventDefinition.TavernEventType.Sharmel_song:
          num4 = 120f;
          key = "dlc_stories_good_sharmel_song";
          break;
        case TavernEventDefinition.TavernEventType.RatRace:
          num4 = 100f;
          key = "dlc_stories_good_rat_race";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      Debug.Log((object) $"Achievement check: event_type = {type}, by_money = {(ValueType) false}, total_money = {num2}, sum_of_coeffs = {num3}");
      if ((double) num3 > (double) num4)
        MainGame.me.save.quests.CheckKeyQuests(key);
      float num5 = num2 + (float) Mathf.RoundToInt(MainGame.me.player.GetParam("zombie_greeter_placed")) * 5f + tavern_event.unconditional_income;
      this.DrawResultRow(GJL.L("event_income"), Trading.FormatMoney(tavern_event.unconditional_income, true));
      bool flag = tavern_event.id == "rat_race" && MainGame.me.player.GetParamInt("rat_race_double_money") == 1;
      MainGame.me.player.SetParam("rat_race_double_money", 0.0f);
      MainGame.me.player.data.money += num5 * (flag ? 2f : 1f);
      string str = Trading.FormatMoney(num5, true);
      this.DrawResultRow(GJL.L("total_money"), string.IsNullOrEmpty(str + (flag ? " x2" : "")) ? "-" : str);
      this.rows_table.Reposition();
    }
  }

  public PrayReportItemGUI DrawResultRow(string txt, string value)
  {
    PrayReportItemGUI prayReportItemGui = this.row_prefab.Copy<PrayReportItemGUI>();
    prayReportItemGui.txt.text = txt + ":";
    prayReportItemGui.value.text = value;
    return prayReportItemGui;
  }
}
