// Decompiled with JetBrains decompiler
// Type: TechUnlock
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TechUnlock
{
  public TechUnlock.TechUnlockType type;
  public string id;
  public bool visible = true;
  public TechUnlock.TechUnlockData _data;

  public TechUnlock()
  {
  }

  public TechUnlock(string id, TechUnlock.TechUnlockType type)
  {
    if (string.IsNullOrEmpty(id))
    {
      Debug.LogError((object) "Can't create a TechUnlock with an empty id");
    }
    else
    {
      if (id[0] == '@')
      {
        id = id.Substring(1);
        this.visible = false;
      }
      this.id = id;
      this.type = type;
      this._data = (TechUnlock.TechUnlockData) null;
    }
  }

  public void ResetLanguageCache() => this._data = (TechUnlock.TechUnlockData) null;

  public TechUnlock.TechUnlockData GetData()
  {
    if (this._data == null)
    {
      this._data = new TechUnlock.TechUnlockData();
      string sprite_name = "";
      string lng_id1 = "";
      string lng_id2 = "unknown";
      switch (this.type)
      {
        case TechUnlock.TechUnlockType.Craft:
          CraftDefinition craftDefinition = GameBalance.me.GetDataOrNull<CraftDefinition>(this.id);
          lng_id1 = "recipe";
          if (craftDefinition == null && this.id.Contains(":"))
          {
            craftDefinition = (CraftDefinition) GameBalance.me.GetData<ObjectCraftDefinition>(this.id);
            lng_id1 = "blueprint";
          }
          if (craftDefinition == null)
          {
            Debug.LogError((object) $"Craft {this.id} not found!");
            break;
          }
          lng_id2 = craftDefinition.GetNameNonLocalized();
          int craftType = (int) craftDefinition.craft_type;
          sprite_name = craftDefinition.GetCraftIcon();
          Debug.Log((object) $"Craft id = {craftDefinition.id}, icon = {sprite_name}");
          if (craftDefinition.IsBodyPartExtractionCraft())
          {
            lng_id1 = "extract";
            break;
          }
          break;
        case TechUnlock.TechUnlockType.Work:
          ObjectGroupDefinition data1 = GameBalance.me.GetData<ObjectGroupDefinition>(this.id);
          if (data1 != null)
          {
            sprite_name = data1.tech_icon;
            lng_id2 = this.id;
            lng_id1 = "gathering";
            break;
          }
          break;
        case TechUnlock.TechUnlockType.Perk:
          PerkDefinition data2 = GameBalance.me.GetData<PerkDefinition>(this.id);
          if (data2 != null)
          {
            sprite_name = data2.GetIcon();
            lng_id2 = this.id;
            lng_id1 = data2.show ? "perk" : "gathering";
            break;
          }
          break;
      }
      this._data.sprite = EasySpritesCollection.GetSprite(sprite_name);
      this._data.name = (string.IsNullOrEmpty(lng_id1) ? "" : $"{GJL.L(lng_id1)}{GJL.L(":")} ") + GJL.L(lng_id2);
      string lng_id3 = lng_id2 + "_d";
      string str = GJL.L(lng_id3);
      this._data.description = lng_id3 == str ? "" : str;
    }
    return this._data;
  }

  public void GetTooltip(Tooltip tooltip)
  {
    if ((Object) tooltip == (Object) null)
      return;
    TechUnlock.TechUnlockData data1 = this.GetData();
    tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(data1.name, UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
    tooltip.AddData((BubbleWidgetData) new BubbleWidgetBlankSeparatorData());
    if (this.type == TechUnlock.TechUnlockType.Craft)
    {
      CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>(this.id);
      if (dataOrNull == null && this.id.Contains(":"))
        GameBalance.me.GetData<ObjectCraftDefinition>(this.id);
      if (dataOrNull == null)
        return;
      Item obj1 = (Item) null;
      List<ItemDefinition> itemDefinitionList = (List<ItemDefinition>) null;
      foreach (Item obj2 in dataOrNull.output)
      {
        if (!(obj2.id == "r") && !(obj2.id == "g") && !(obj2.id == "b") && obj2.is_multiquality)
        {
          bool flag = false;
          itemDefinitionList = new List<ItemDefinition>();
          foreach (string multiqualityItem in obj2.multiquality_items)
          {
            ItemDefinition data2 = GameBalance.me.GetData<ItemDefinition>(multiqualityItem);
            if (data2 == null)
            {
              flag = true;
              break;
            }
            if (data2.type != ItemDefinition.ItemType.Preach)
            {
              flag = true;
              break;
            }
            itemDefinitionList.Add(data2);
          }
          if (!flag)
          {
            obj1 = obj2;
            break;
          }
        }
      }
      bool flag1 = obj1 != null && itemDefinitionList != null && itemDefinitionList.Count > 0;
      if (flag1)
      {
        string str1 = "";
        List<float> floatList1 = new List<float>();
        List<float> floatList2 = new List<float>();
        List<float> floatList3 = new List<float>();
        List<float> floatList4 = new List<float>();
        List<float> floatList5 = new List<float>();
        for (int index = 0; index < itemDefinitionList.Count; ++index)
        {
          CraftDefinition linkedCraft = itemDefinitionList[index].linked_craft;
          if (linkedCraft != null)
          {
            floatList1.Add(linkedCraft.needs_quality);
            floatList2.Add(linkedCraft.k_money * 100f);
            floatList3.Add(linkedCraft.k_faith * 100f);
            if (linkedCraft.output.Count > 0)
            {
              foreach (Item obj3 in linkedCraft.output)
              {
                switch (obj3.id)
                {
                  case "money":
                    floatList4.Add((float) obj3.value / 100f);
                    continue;
                  case "faith":
                    floatList5.Add((float) obj3.value);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        string s1 = string.Empty;
        if (floatList1.Count > 0)
        {
          floatList1.Sort();
          if ((double) floatList1[0] < (double) floatList1[floatList1.Count - 1])
          {
            string s2 = s1;
            float num = floatList1[0];
            string str2 = num.ToString();
            num = floatList1[floatList1.Count - 1];
            string str3 = num.ToString();
            string ss = GJL.L("preach_params", $"(cross){str2}-{str3}");
            s1 = s2.ConcatWithSeparator(ss);
          }
          else if ((double) floatList1[floatList1.Count - 1] > 0.0)
            s1 = s1.ConcatWithSeparator(GJL.L("preach_params", "(cross)" + floatList1[floatList1.Count - 1].ToString()));
        }
        string text = LocalizedLabel.ColorizeTags(s1 + GJL.L(obj1.id + "_d"), LocalizedLabel.TextColor.SpeechBubble);
        if (floatList2.Count > 0)
        {
          floatList2.Sort();
          if ((double) floatList2[0] < (double) floatList2[floatList2.Count - 1])
            str1 = str1.ConcatWithSeparator(GJL.L("sermon_money_k", $"+{floatList2[0]:0}-{floatList2[floatList2.Count - 1]:0}%"));
          else if ((double) floatList2[floatList2.Count - 1] > 0.0)
            str1 = str1.ConcatWithSeparator(GJL.L("sermon_money_k", $"+{floatList2[floatList2.Count - 1]:0}%"));
        }
        if (floatList3.Count > 0)
        {
          floatList3.Sort();
          if ((double) floatList3[0] < (double) floatList3[floatList3.Count - 1])
            str1 = str1.ConcatWithSeparator(GJL.L("sermon_faith_k", $"+{floatList3[0]:0}-{floatList3[floatList3.Count - 1]:0}%"));
          else if ((double) floatList3[floatList3.Count - 1] > 0.0)
            str1 = str1.ConcatWithSeparator(GJL.L("sermon_faith_k", $"+{floatList3[floatList3.Count - 1]:0}%"));
        }
        string str4 = string.Empty;
        if (floatList5.Count > 0)
        {
          floatList5.Sort();
          if ((double) floatList5[0] < (double) floatList5[floatList5.Count - 1])
            str4 = str4.ConcatWithSeparator(GJL.L("faith") + $" (x{floatList5[0]:0}-{floatList5[floatList5.Count - 1]})", ",");
          else if ((double) floatList5[floatList5.Count - 1] > 0.0)
            str4 = str4.ConcatWithSeparator(GJL.L("faith") + $" (x{floatList5[floatList5.Count - 1]})", ",");
        }
        if (floatList4.Count > 0)
        {
          floatList4.Sort();
          if ((double) floatList4[0] < (double) floatList4[floatList4.Count - 1])
            str4 = str4.ConcatWithSeparator($"{Trading.FormatMoney(floatList4[0])}-{Trading.FormatMoney(floatList4[floatList4.Count - 1])}", ",");
          else if ((double) floatList4[floatList4.Count - 1] > 0.0)
            str4 = str4.ConcatWithSeparator(Trading.FormatMoney(floatList4[floatList4.Count - 1]), ",");
        }
        if (!string.IsNullOrEmpty(str4))
          str1 = str1.ConcatWithSeparator(str4);
        if (!string.IsNullOrEmpty(text))
          tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(text, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
        if (!string.IsNullOrEmpty(str1))
        {
          tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(GJL.L("preach_params_2"), UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
          tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(str1, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
        }
      }
      if (dataOrNull.output.Count <= 0)
        return;
      Item obj4 = (Item) null;
      ItemDefinition itemDefinition = (ItemDefinition) null;
      for (int index = 0; index < dataOrNull.output.Count; ++index)
      {
        if (!(dataOrNull.output[index].id == "r") && !(dataOrNull.output[index].id == "g") && !(dataOrNull.output[index].id == "b") && !(dataOrNull.output[index].id == "v"))
        {
          if (dataOrNull.output[index].definition != null)
          {
            obj4 = dataOrNull.output[index];
            itemDefinition = obj4.definition;
            break;
          }
          obj4 = dataOrNull.output[index];
          itemDefinition = GameBalance.me.GetDataOrNull<ItemDefinition>(obj4.id + ":1");
          break;
        }
      }
      if (obj4 != null && itemDefinition != null && !flag1)
      {
        List<BubbleWidgetData> tooltipData = itemDefinition.GetTooltipData(obj4, false);
        tooltipData.RemoveAt(0);
        foreach (BubbleWidgetData tooltip_data in tooltipData)
        {
          tooltip_data.TrySetAlign(NGUIText.Alignment.Left);
          tooltip.AddData(tooltip_data);
        }
      }
      if (!flag1)
        return;
      foreach (BubbleWidgetData tooltip_data in itemDefinition.GetTooltipDataCraftAt(obj4))
      {
        tooltip_data.TrySetAlign(NGUIText.Alignment.Left);
        tooltip.AddData(tooltip_data);
      }
    }
    else
      tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(data1.description, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
  }

  public class TechUnlockData
  {
    public UnityEngine.Sprite sprite;
    public UnityEngine.Sprite quality_icon;
    public string name;
    public string description;
  }

  public enum TechUnlockType
  {
    Craft,
    Work,
    Phrase,
    Perk,
  }
}
