// Decompiled with JetBrains decompiler
// Type: PrayReportGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PrayReportGUI : DialogGUI
{
  public PrayReportItemGUI row_prefab;
  public UITableOrGrid rows_table;

  public override void Init() => base.Init();

  public void Open(PrayLogics.PrayResult pray_result)
  {
    this.OpenOK("sermon_report_header");
    this.row_prefab.gameObject.SetActive(false);
    this.rows_table.DestroyChildren<PrayReportItemGUI>(new PrayReportItemGUI[1]
    {
      this.row_prefab
    });
    this.DrawResultRow(GJL.L("serm_res_people"), pray_result.people.ToString());
    this.DrawResultRow(GJL.L("serm_res_faith"), pray_result.faith.ToString());
    string str = Trading.FormatMoney(pray_result.money);
    this.DrawResultRow(GJL.L("serm_res_money"), string.IsNullOrEmpty(str) ? "-" : str);
    this.DrawResultRow(GJL.L("serm_res_success"), GJL.L("serm_res_" + (pray_result.success ? "1" : "0")));
    this.DrawResultRow(GJL.L("sermon_success_chance", "").Replace(": ", ""), pray_result.success_percent.ToString() + "%");
    if (pray_result.success)
    {
      if (pray_result.faith_bonus > 0)
        this.DrawResultRow(GJL.L("serm_res_faith_bonus"), pray_result.faith_bonus.ToString());
      if (!pray_result.money_bonus.EqualsTo(0.0f))
        this.DrawResultRow(GJL.L("serm_res_money_bonus"), Trading.FormatMoney(pray_result.money_bonus));
    }
    this.rows_table.Reposition();
  }

  public PrayReportItemGUI DrawResultRow(string txt, string value)
  {
    PrayReportItemGUI prayReportItemGui = this.row_prefab.Copy<PrayReportItemGUI>();
    prayReportItemGui.txt.text = txt + ":";
    prayReportItemGui.value.text = value;
    return prayReportItemGui;
  }

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

  public void OpenMerchantTraidingResult(WorldGameObject cashbox_wgo)
  {
    if ((Object) cashbox_wgo == (Object) null)
    {
      Debug.LogError((object) "Can not OpenMerchantTraidingResult: cashbox_wgo is NULL!");
    }
    else
    {
      this.OpenOK("traiding_report_header");
      this.row_prefab.gameObject.SetActive(false);
      this.rows_table.DestroyChildren<PrayReportItemGUI>(new PrayReportItemGUI[1]
      {
        this.row_prefab
      });
      this.DrawResultRow(GJL.L("total_crates_sold"), cashbox_wgo.data.GetParamInt("total_crates_sold").ToString());
      string str1 = string.Empty;
      ItemDefinition dataOrNull1 = GameBalance.me.GetDataOrNull<ItemDefinition>("box_vegetables_silver");
      if (dataOrNull1 != null)
        str1 = " x " + Trading.FormatMoney(dataOrNull1.base_price);
      this.DrawResultRow(GJL.L("silver_crates_sold"), cashbox_wgo.data.GetParamInt("silver_crates_sold").ToString() + str1);
      string str2 = string.Empty;
      ItemDefinition dataOrNull2 = GameBalance.me.GetDataOrNull<ItemDefinition>("box_vegetables_gold");
      if (dataOrNull2 != null)
        str2 = " x " + Trading.FormatMoney(dataOrNull2.base_price);
      this.DrawResultRow(GJL.L("gold_crates_sold"), cashbox_wgo.data.GetParamInt("gold_crates_sold").ToString() + str2);
      string str3 = string.Empty;
      ItemDefinition dataOrNull3 = GameBalance.me.GetDataOrNull<ItemDefinition>("box_goods");
      if (dataOrNull3 != null)
        str3 = " x " + Trading.FormatMoney(dataOrNull3.base_price);
      this.DrawResultRow(GJL.L("goods_crates_sold"), cashbox_wgo.data.GetParamInt("goods_crates_sold").ToString() + str3);
      string str4 = Trading.FormatMoney(cashbox_wgo.data.GetParam("total_money"), true);
      this.DrawResultRow(GJL.L("total_money"), string.IsNullOrEmpty(str4) ? "-" : str4);
      MainGame.me.player.data.AddToParams("money", cashbox_wgo.data.GetParam("total_money"));
      cashbox_wgo.data.SetParam("total_crates_sold", 0.0f);
      cashbox_wgo.data.SetParam("silver_crates_sold", 0.0f);
      cashbox_wgo.data.SetParam("gold_crates_sold", 0.0f);
      cashbox_wgo.data.SetParam("goods_crates_sold", 0.0f);
      cashbox_wgo.data.SetParam("total_money", 0.0f);
      this.rows_table.Reposition();
    }
  }

  public void OpenTavernCashbox(WorldGameObject tavern_cashbox)
  {
    if ((Object) tavern_cashbox == (Object) null)
    {
      Debug.LogError((object) "Can not OpenTavernCashbox: tavern_cashbox is NULL!");
    }
    else
    {
      this.OpenOK("traiding_report_header");
      this.row_prefab.gameObject.SetActive(false);
      this.rows_table.DestroyChildren<PrayReportItemGUI>(new PrayReportItemGUI[1]
      {
        this.row_prefab
      });
      foreach (string id in MainGame.me.save.players_tavern_engine.ITEMS_SELLING_IN_TAVERN)
      {
        string param_name = id.Replace(":", "_");
        int paramInt = tavern_cashbox.GetParamInt(param_name);
        if (paramInt != 0)
        {
          ItemDefinition data = GameBalance.me.GetData<ItemDefinition>(id);
          string str = $"{paramInt.ToString()} x {Trading.FormatMoney(data.base_price * 1f)}";
          string itemName = data.GetItemName();
          if (data.quality_type == ItemDefinition.QualityType.Stars)
          {
            switch (Mathf.RoundToInt(data.quality))
            {
              case 1:
                itemName += " (s1)";
                break;
              case 2:
                itemName += " (s2)";
                break;
              case 3:
                itemName += " (s3)";
                break;
            }
          }
          this.DrawResultRow(itemName, str);
          tavern_cashbox.SetParam(param_name, 0.0f);
        }
      }
      float money_earned = tavern_cashbox.data.GetParam("money");
      tavern_cashbox.data.SetParam("money", 0.0f);
      float alcoholSellingBonus = PlayersTavernEngine.CalculateAlcoholSellingBonus(money_earned);
      if ((double) Mathf.Abs(alcoholSellingBonus) > 0.0099999997764825821)
        this.DrawResultRow(GJL.L("tavern_upgrade_bonus"), Trading.FormatMoney(alcoholSellingBonus, true));
      this.DrawResultRow(GJL.L("total_money"), Trading.FormatMoney(money_earned, true));
      float num = tavern_cashbox.data.GetParam("reputation");
      tavern_cashbox.data.SetParam("reputation", 0.0f);
      string str1 = string.Empty;
      if ((double) num >= 3.0)
      {
        str1 = "(tavern_reputation) (tavern_reputation) (tavern_reputation)";
      }
      else
      {
        if ((double) num > 2.0)
        {
          str1 += " (tavern_reputation) (tavern_reputation)";
          num -= 2f;
        }
        else if ((double) num > 1.0)
        {
          str1 += " (tavern_reputation)";
          --num;
        }
        if ((double) num > 0.0)
          str1 = $"{str1} (tavern_reputation){Mathf.RoundToInt(num * 100f).ToString()}%";
      }
      this.DrawResultRow(GJL.L("total_reputation"), str1);
      MainGame.me.player.data.AddToParams("money", money_earned);
      this.rows_table.Reposition();
    }
  }
}
