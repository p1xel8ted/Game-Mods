// Decompiled with JetBrains decompiler
// Type: ItemDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[Serializable]
public class ItemDefinition : BalanceBaseObject
{
  public const int MAX_ALCHEMY_TIER = 3;
  public const int MAX_PRODUCT_TIER = 3;
  public static string[] _params_on_use_types = new string[3]
  {
    "hp",
    "energy",
    "sanity"
  };
  public string icon;
  public string custom_ovr_icon;
  public bool not_used;
  public ItemDefinition.ItemType type;
  public List<int> can_be_placed_on = new List<int>();
  public int stack_count;
  public GameRes parameters = new GameRes();
  public int item_size = 1;
  public int base_count;
  public int product_tier;
  public string run_script_after_drop;
  public bool destroy_after_drop;
  public float base_price;
  public float custom_sell_price_koeff;
  public bool is_static_cost;
  public float quality;
  public float quality_multiplyer;
  public ItemDefinition.QualityType quality_type;
  public float durability_decrease;
  public bool durability_decrease_on_use;
  public float durability_decrease_on_use_speed;
  public float durability_modificator;
  public bool has_durability;
  public bool is_update_children_durability;
  public float durability_modificator_for_children = 1f;
  public float efficiency;
  public float armor;
  public string on_use_script = "";
  public string on_use_snd = "";
  public List<SmartExpression> on_use_expressions = new List<SmartExpression>();
  public List<SmartExpression> on_trade_expressions = new List<SmartExpression>();
  public bool dont_break_on_zero_dur;
  public string dur_0_change;
  public List<Item> drop_on_use;
  public int q_plus;
  public int q_minus;
  public string q_hint;
  public bool player_cant_throw_out;
  public SmartExpression cooldown;
  public bool stay_on_use = true;
  public List<string> product_types = new List<string>();
  public int product_weight;
  public bool can_be_used;
  public bool close_inv_on_use;
  public bool autouse;
  public GameRes params_on_use = new GameRes();
  public SmartExpression tool_energy_k;
  public static ItemDefinition none;
  public float rat_speed;
  public float rat_obedience;
  public float rat_speed_add;
  public float rat_obedience_add;
  public float rat_speed_multiply;
  public float rat_obedience_multiply;
  public bool can_insert_into_barmen;
  public float[] tavern_event_coeffs = new float[4];
  public ItemDefinition.BagType bag_type;
  public int bag_size_x;
  public int bag_size_y;
  public List<ItemDefinition.BagType> can_be_inserted_in_bag = new List<ItemDefinition.BagType>();
  public ItemDefinition.ItemDetails _details;
  public CraftDefinition _linked_craft;
  public SmartExpression show_q_hint;
  public ItemDefinition.ItemReplaceData item_replace;
  public ItemDefinition.AlchemyType alch_type;

  public bool is_small => this.item_size == 1;

  public bool is_big => this.item_size > 1;

  public bool is_crate => this.type == ItemDefinition.ItemType.Crate;

  public bool is_placeholder => this.id.Contains("placeholder");

  public bool is_bag => this.type == ItemDefinition.ItemType.Bag;

  public ItemDefinition.EquipmentType equipment_type
  {
    get
    {
      switch (this.type)
      {
        case ItemDefinition.ItemType.Axe:
          return ItemDefinition.EquipmentType.Axe;
        case ItemDefinition.ItemType.Pickaxe:
          return ItemDefinition.EquipmentType.Pickaxe;
        case ItemDefinition.ItemType.Shovel:
          return ItemDefinition.EquipmentType.Shovel;
        case ItemDefinition.ItemType.Sword:
          return ItemDefinition.EquipmentType.Sword;
        case ItemDefinition.ItemType.Hammer:
          return ItemDefinition.EquipmentType.Hammer;
        case ItemDefinition.ItemType.FishingRod:
          return ItemDefinition.EquipmentType.FishingRod;
        case ItemDefinition.ItemType.HeadArmor:
          return ItemDefinition.EquipmentType.HeadArmor;
        case ItemDefinition.ItemType.BodyArmor:
          return ItemDefinition.EquipmentType.BodyArmor;
        default:
          return ItemDefinition.EquipmentType.None;
      }
    }
  }

  public bool is_placable => this.can_be_placed_on.Count > 0;

  public string GetQualityString(Item real_item = null)
  {
    string v2 = $"{this.quality:0.#}";
    if (real_item != null && !real_item.GetItemQuality().EqualsTo(this.quality))
      v2 = GJL.L("n_of", $"{real_item.GetItemQuality():0.#}", v2);
    return v2;
  }

  public bool is_tool => this.type < ItemDefinition.ItemType.Hand && this.type != 0;

  public string GetItemDescription(Item real_item = null)
  {
    string lng_id = this.id + "_d";
    string s1 = GJL.L(lng_id);
    if (s1 == lng_id && this.id.Contains(":"))
      s1 = GJL.L(this.id.Substring(0, this.id.LastIndexOf(':')) + "_d");
    string str1 = LocalizedLabel.ColorizeTags(s1, LocalizedLabel.TextColor.SpeechBubble);
    string s2 = "";
    GameRes add_game_res = new GameRes();
    if (this.on_use_expressions != null && this.on_use_expressions.Count > 0)
    {
      foreach (SmartExpression onUseExpression in this.on_use_expressions)
      {
        if (onUseExpression != null && !onUseExpression.HasNoExpresion())
        {
          foreach (GameResAtom atom in GameRes.ParseSmartExpression(onUseExpression).ToAtomList())
            add_game_res.Add(atom);
        }
      }
    }
    string formattedString = this.params_on_use.ToFormattedString(add_game_res: add_game_res);
    if (!this.can_be_used && this.is_tool && !string.IsNullOrEmpty(formattedString))
      s2 = s2.ConcatWithSeparator(GJL.L("tool_energy_spend", formattedString));
    if (this.type == ItemDefinition.ItemType.Preach)
    {
      if (this.linked_craft == null)
        Debug.LogError((object) $"Sermon {this.id} has no preaching craft!");
      else
        s2 = s2.ConcatWithSeparator(GJL.L("preach_params", "(cross)" + this.linked_craft.needs_quality.ToString()));
    }
    if (!string.IsNullOrEmpty(this.q_hint) && (!this.show_q_hint.has_expression || this.show_q_hint.EvaluateBoolean()))
      s2 = s2.ConcatWithSeparator(GJL.L(this.q_hint, this.GetQualityString(real_item)));
    if (this.is_tool)
    {
      if (this.type == ItemDefinition.ItemType.Sword)
      {
        int num = this.parameters.GetInt("damage");
        if (num > 0)
          s2 = s2.ConcatWithSeparator($"{num.ToString()} {GJL.L("damage")}");
      }
      else
      {
        int efficiencyPercent = GameBalance.me.GetToolEfficiencyPercent(this);
        if (efficiencyPercent > 0)
          s2 = s2.ConcatWithSeparator(GJL.L("tool_eff_hint", efficiencyPercent.ToString() + "%"));
      }
    }
    if (this.can_be_used)
    {
      string str2 = "";
      foreach (SmartExpression onUseExpression in this.on_use_expressions)
      {
        Regex regex = new Regex("AddBuff\\(\"(.+?)\"\\)");
        string expressionString = onUseExpression.GetRawExpressionString();
        if (!string.IsNullOrEmpty(expressionString) && expressionString.Contains("AddBuff("))
        {
          Match match = regex.Match(expressionString);
          if (match.Success)
          {
            BuffDefinition data = GameBalance.me.GetData<BuffDefinition>(match.Groups[1].Captures[0].ToString());
            if (data != null)
            {
              str2 = $"[c][C16000]{data.GetLocalizedName()}[-][/c]";
              string descriptionIfExists = data.GetDescriptionIfExists();
              if (!string.IsNullOrEmpty(descriptionIfExists))
                str2 = $"{str2} ({descriptionIfExists})";
            }
          }
        }
      }
      if (!string.IsNullOrEmpty(str2) || !string.IsNullOrEmpty(formattedString))
      {
        string str3 = formattedString;
        if (string.IsNullOrEmpty(formattedString))
          str3 = str2;
        else if (!string.IsNullOrEmpty(str2))
          str3 = $"{str3}, {str2}";
        s2 = $"{s2.ConcatWithSeparator(GJL.L("item_effect_on_use"))} {str3}";
      }
    }
    if (real_item?.definition != null)
    {
      if (real_item.definition.type == ItemDefinition.ItemType.Rat)
      {
        foreach (Item allRatBuff in real_item.GetAllRatBuffs())
        {
          if (allRatBuff.definition.has_durability)
          {
            int num1 = (int) ((double) allRatBuff.durability / (double) allRatBuff.definition.durability_decrease);
            int num2 = num1 % 60;
            s2 = s2.ConcatWithSeparator($"{GJL.L("rat_status")}: {allRatBuff.definition.GetItemName()}({(num1 / 60).ToString()}:{(num2 < 10 ? "0" : "")}{num2.ToString()})");
            break;
          }
        }
        s2 = s2.ConcatWithSeparator(real_item.GetRatDescription(false));
      }
      else if (real_item.definition.type == ItemDefinition.ItemType.RatBuff)
      {
        string ss = string.Empty;
        if (!real_item.definition.rat_speed_add.EqualsTo(0.0f, 0.01f))
          ss = $"{ss}{((double) real_item.definition.rat_speed_add > 0.0 ? "+" : string.Empty)}{real_item.definition.rat_speed_add.ToString()}(speed) ";
        if (!real_item.definition.rat_obedience_add.EqualsTo(0.0f, 0.01f))
          ss = $"{ss}{((double) real_item.definition.rat_obedience_add > 0.0 ? "+" : string.Empty)}{real_item.definition.rat_obedience_add.ToString()}(obedience) ";
        if (!real_item.definition.rat_speed_multiply.EqualsTo(1f, 0.01f))
          ss = $"{ss}{((double) real_item.definition.rat_speed_multiply > 0.0 ? "+" : "")}{((float) (((double) real_item.definition.rat_speed_multiply - 1.0) * 100.0)).ToString()}%(speed) ";
        if (!real_item.definition.rat_obedience_multiply.EqualsTo(1f, 0.01f))
          ss = $"{ss}{((double) real_item.definition.rat_obedience_multiply > 0.0 ? "+" : "")}{((float) (((double) real_item.definition.rat_obedience_multiply - 1.0) * 100.0)).ToString()}%(obedience) ";
        if (!string.IsNullOrEmpty(ss))
          s2 = s2.ConcatWithSeparator(ss);
      }
    }
    if (lng_id == str1)
      return s2;
    if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(s2))
      s2 += "\n";
    return !string.IsNullOrEmpty(s2) ? s2 + str1 : str1;
  }

  public CraftDefinition linked_craft
  {
    get
    {
      if (this._linked_craft == null || string.IsNullOrEmpty(this._linked_craft.id))
      {
        this._linked_craft = GameBalance.me.GetData<CraftDefinition>("pray:" + this.id);
        if (this._linked_craft == null)
          Debug.LogError((object) ("Craft for sermon not found: " + this.id));
      }
      return this._linked_craft;
    }
  }

  public string GetItemName(bool localized = true)
  {
    string lng_id = this.id;
    if (this.quality_type == ItemDefinition.QualityType.Stars && lng_id.Contains(":") && (double) this.quality > 0.10000000149011612)
      lng_id = lng_id.Substring(0, lng_id.LastIndexOf(':'));
    string str = GJL.L(lng_id);
    return str != lng_id ? (!localized ? lng_id : str) : (!localized ? this.id : GJL.L(this.id));
  }

  public void AddWidgetSeparator(List<BubbleWidgetData> res, bool full_detail)
  {
    if (!full_detail)
      return;
    res.Add((BubbleWidgetData) new BubbleWidgetSeparatorData());
  }

  public List<BubbleWidgetData> GetTooltipData(Item item = null, bool full_detail = true)
  {
    List<BubbleWidgetData> res = new List<BubbleWidgetData>()
    {
      (BubbleWidgetData) new BubbleWidgetTextData(this.GetItemName(), UITextStyles.TextStyle.HintTitle),
      (BubbleWidgetData) new BubbleWidgetTextData(this.GetItemDescription(item), UITextStyles.TextStyle.TinyDescription)
    };
    if (item == null)
      return res;
    if ((double) item.durability < 1.0)
      res.Add((BubbleWidgetData) new BubbleWidgetTextData(item.GetDurabilityHint(), UITextStyles.TextStyle.TinyDescription));
    if (item.definition != null && item.definition.type == ItemDefinition.ItemType.Preach && item.definition.linked_craft != null)
    {
      string str1 = "";
      CraftDefinition linkedCraft = item.definition.linked_craft;
      if (linkedCraft != null)
      {
        if ((double) linkedCraft.k_money > 0.0)
          str1 = str1.ConcatWithSeparator(GJL.L("sermon_money_k", $"+{(ValueType) (float) ((double) linkedCraft.k_money * 100.0):0}%"));
        if ((double) linkedCraft.k_faith > 0.0)
          str1 = str1.ConcatWithSeparator(GJL.L("sermon_faith_k", $"+{(ValueType) (float) ((double) linkedCraft.k_faith * 100.0):0}%"));
        if (linkedCraft.output.Count > 0)
        {
          string str2 = "";
          foreach (Item obj in linkedCraft.output)
          {
            string itemName = obj.GetItemName();
            str2 = str2.ConcatWithSeparator(itemName, ", ");
          }
          str1 = str1.ConcatWithSeparator(str2);
        }
      }
      if (!string.IsNullOrEmpty(str1))
      {
        res.Add((BubbleWidgetData) new BubbleWidgetTextData(GJL.L("preach_params_2"), UITextStyles.TextStyle.HintTitle));
        res.Add((BubbleWidgetData) new BubbleWidgetTextData(str1, UITextStyles.TextStyle.TinyDescription));
      }
    }
    string bodyModificators = item.GetItemBodyModificators();
    if (!string.IsNullOrEmpty(bodyModificators))
    {
      if (item.definition.type == ItemDefinition.ItemType.BodyUniversalPart)
      {
        this.AddWidgetSeparator(res, full_detail);
        res.Add((BubbleWidgetData) new BubbleWidgetTextData(bodyModificators, UITextStyles.TextStyle.TinyDescription));
      }
      else
      {
        this.AddWidgetSeparator(res, full_detail);
        res.Add((BubbleWidgetData) new BubbleWidgetTextData($"{GJL.L("embalming_effect")}\n{bodyModificators}", UITextStyles.TextStyle.TinyDescription));
      }
    }
    ItemDefinition.ItemDetails itemDetails = this.GetItemDetails();
    CraftDefinition surveyCraft = this.GetSurveyCraft();
    if (full_detail && surveyCraft != null)
    {
      if (surveyCraft.sub_type == CraftDefinition.CraftSubType.SurveySciencePoints)
      {
        int v1 = surveyCraft.output_to_wgo.Count == 0 ? 0 : surveyCraft.output_to_wgo[0].value;
        if (v1 > 0)
        {
          this.AddWidgetSeparator(res, full_detail);
          res.Add((BubbleWidgetData) new BubbleWidgetTextData(GJL.L("hint_science_decompose", v1), UITextStyles.TextStyle.TinyDescription));
        }
      }
      if (MainGame.me.save.completed_one_time_crafts.Contains(surveyCraft.id))
      {
        this.AddWidgetSeparator(res, full_detail);
        res.Add((BubbleWidgetData) new BubbleWidgetTextData($"{GJL.L("survey")}{GJL.L(":")} {GJL.L("survey_complete")}", UITextStyles.TextStyle.TinyDescription));
        if (itemDetails?.alchemy != null)
          res.Add((BubbleWidgetData) new BubbleWidgetAlchemyItemData(this.id));
      }
      else
      {
        string str3 = "";
        foreach (Item obj in MainGame.game_started ? ResModificator.ProcessItemsListBeforeDrop(surveyCraft.output, (WorldGameObject) null, MainGame.me.player) : surveyCraft.output)
        {
          if (TechDefinition.TECH_POINTS.Contains(obj.id) && (!MainGame.game_started || obj.value > 0))
            str3 = $"{str3}({obj.id})";
        }
        if (!string.IsNullOrEmpty(str3))
        {
          this.AddWidgetSeparator(res, full_detail);
          string str4 = $" ({str3})";
          res.Add((BubbleWidgetData) new BubbleWidgetTextData($"{GJL.L("survey")}: [c][B73B1F]{GJL.L("survey_not_complete")}[-][/c]{str4}", UITextStyles.TextStyle.TinyDescription));
        }
      }
    }
    if (itemDetails != null)
    {
      string str5 = ", ";
      string str6 = GJL.L(",");
      if (!string.IsNullOrEmpty(str6))
        str5 = str6;
      string empty = string.Empty;
      for (int index = 0; index < itemDetails.crafts_in.Count; ++index)
      {
        if (index > 0)
          empty += str5;
        empty += GJL.L(itemDetails.crafts_in[index].id);
      }
      if (!string.IsNullOrEmpty(empty))
      {
        this.AddWidgetSeparator(res, full_detail);
        res.Add((BubbleWidgetData) new BubbleWidgetTextData($"{GJL.L("crafted_at")} {empty}", UITextStyles.TextStyle.TinyDescription));
      }
    }
    return res;
  }

  public List<BubbleWidgetData> GetTooltipDataCraftAt(Item item = null)
  {
    List<BubbleWidgetData> tooltipDataCraftAt = new List<BubbleWidgetData>();
    ItemDefinition.ItemDetails itemDetails = this.GetItemDetails();
    if (itemDetails != null)
    {
      string str1 = ", ";
      string str2 = GJL.L(",");
      if (!string.IsNullOrEmpty(str2))
        str1 = str2;
      string empty = string.Empty;
      for (int index = 0; index < itemDetails.crafts_in.Count; ++index)
      {
        if (index > 0)
          empty += str1;
        empty += GJL.L(itemDetails.crafts_in[index].id);
      }
      if (!string.IsNullOrEmpty(empty))
        tooltipDataCraftAt.Add((BubbleWidgetData) new BubbleWidgetTextData($"{GJL.L("crafted_at")} {empty}", UITextStyles.TextStyle.TinyDescription));
    }
    return tooltipDataCraftAt;
  }

  public bool IsWeapon() => this.type == ItemDefinition.ItemType.Sword;

  public bool IsEquipment()
  {
    switch (this.type)
    {
      case ItemDefinition.ItemType.Axe:
      case ItemDefinition.ItemType.Pickaxe:
      case ItemDefinition.ItemType.Shovel:
      case ItemDefinition.ItemType.Hammer:
      case ItemDefinition.ItemType.FishingRod:
      case ItemDefinition.ItemType.HeadArmor:
      case ItemDefinition.ItemType.BodyArmor:
        return true;
      default:
        return false;
    }
  }

  public static int GetProductTier(string string_to_parse)
  {
    int result;
    if (int.TryParse(string_to_parse.Replace(" ", "").ToLower().Replace("common", "3").Replace("unusual", "6").Replace("rare", "9").Replace("epic", "12").Replace("legendary", "15"), out result))
      return result;
    Debug.LogError((object) $"Can't parse a product tier: [{string_to_parse}]");
    return 0;
  }

  public float GetPrice(int cur_count, int modified_base_count = 0)
  {
    if (cur_count <= 0)
      cur_count = 1;
    if (modified_base_count == 0)
      modified_base_count = this.base_count;
    return this.is_static_cost ? this.base_price : this.base_price * Mathf.Sqrt((float) modified_base_count / (float) cur_count);
  }

  public static int CompareProductTypes(string left_product_type, string right_product_type)
  {
    ProductTypeDefinition dataOrNull1 = GameBalance.me.GetDataOrNull<ProductTypeDefinition>(left_product_type);
    ProductTypeDefinition dataOrNull2 = GameBalance.me.GetDataOrNull<ProductTypeDefinition>(right_product_type);
    if (dataOrNull1 == null && dataOrNull2 == null)
      return 0;
    if (dataOrNull1 == null)
      return 1;
    if (dataOrNull2 == null)
      return -1;
    if (dataOrNull1.sort_weight < dataOrNull2.sort_weight)
      return 1;
    return dataOrNull1.sort_weight > dataOrNull2.sort_weight ? -1 : 0;
  }

  public void ResetLanguageCache() => this._details = (ItemDefinition.ItemDetails) null;

  public ItemDefinition.ItemDetails GetItemDetails()
  {
    if (this._details == null)
    {
      this._details = new ItemDefinition.ItemDetails()
      {
        crafts_in = GameBalance.me.GetItemCraftsIn(this.id)
      };
      if (this.alch_type != ItemDefinition.AlchemyType.None)
        return this._details;
      this._details.alchemy = new ItemDefinition.ItemDetailsAlchemy()
      {
        details_type = ItemDefinition.ItemDetailsAlchemy.DetailsType.Decompose
      };
      if (this._details.alchemy.details_type != ItemDefinition.ItemDetailsAlchemy.DetailsType.Decompose)
        throw new ArgumentOutOfRangeException();
      foreach (CraftDefinition craftDefinition in GameBalance.me.craft_data)
      {
        if (craftDefinition.craft_type == CraftDefinition.CraftType.AlchemyDecompose && (craftDefinition.needs[0].id == this.id || this.id.Contains<char>(':') && this.id.Contains(craftDefinition.needs[0].id)) && craftDefinition.output[0].definition.alch_type != ItemDefinition.AlchemyType.None)
        {
          int alchType = (int) craftDefinition.output[0].definition.alch_type;
          if (!this._details.alchemy.decomposes.Contains(alchType))
            this._details.alchemy.decomposes.Add(alchType);
        }
      }
    }
    return this._details;
  }

  public UnityEngine.Sprite TryGetQualitySprite()
  {
    string qualityIconName = this.GetQualityIconName();
    return !string.IsNullOrEmpty(qualityIconName) ? EasySpritesCollection.GetSprite(qualityIconName) : (UnityEngine.Sprite) null;
  }

  public string GetQualityIconName()
  {
    if (this.quality_type == ItemDefinition.QualityType.Default)
      return (string) null;
    string qualityIconName = "";
    if (this.quality_type == ItemDefinition.QualityType.Stars && Mathf.FloorToInt(this.quality) > 0)
      qualityIconName = "item_star_" + this.quality.ToString();
    return qualityIconName;
  }

  public void TryDrawQualityOrDisableGameObject(UI2DSprite ui_sprite)
  {
    ui_sprite.sprite2D = this.TryGetQualitySprite();
    ui_sprite.SetActive((UnityEngine.Object) ui_sprite.sprite2D != (UnityEngine.Object) null);
  }

  public string GetNameWithoutQualitySuffix()
  {
    return ItemDefinition.StaticGetNameWithoutQualitySuffix(this.id);
  }

  public static string StaticGetNameWithoutQualitySuffix(string id)
  {
    int length = id.LastIndexOf(':');
    return length == -1 ? id : id.Substring(0, length);
  }

  public CraftDefinition GetSurveyCraft()
  {
    return GameBalance.me.GetDataOrNull<CraftDefinition>("surv:" + this.GetNameWithoutQualitySuffix()) ?? GameBalance.me.GetDataOrNull<CraftDefinition>("surv:" + this.id);
  }

  public string GetIcon() => !string.IsNullOrEmpty(this.icon) ? this.icon : "i_" + this.id;

  public string GetOverheadIcon()
  {
    return !string.IsNullOrEmpty(this.custom_ovr_icon) ? this.custom_ovr_icon : this.GetIcon();
  }

  public static string GetGooFromAlchemyIngridient(string ingridient)
  {
    if (string.IsNullOrEmpty(ingridient))
      return string.Empty;
    if (ingridient.Contains(":"))
    {
      string[] strArray = ingridient.Split(':');
      ingridient = strArray[strArray.Length - 1];
    }
    if (ingridient.StartsWith("alchemy_"))
    {
      if (ingridient.Length < 10)
      {
        Debug.LogError((object) ("Wrong alchemy ingridient: " + ingridient));
        return string.Empty;
      }
      int num;
      switch (ingridient[8])
      {
        case '1':
          num = 1;
          break;
        case '2':
          num = 2;
          break;
        case '3':
          num = 3;
          break;
        default:
          Debug.LogError((object) $"Wrong alchemy ingridient number: {ingridient}[8]={ingridient[8].ToString()}");
          return string.Empty;
      }
      ingridient = ingridient.Replace($"alchemy_{num.ToString()}_", "");
    }
    else if (ingridient.StartsWith("powder_"))
      ingridient = ingridient.Replace("powder_", "");
    else if (ingridient.StartsWith("drop_"))
      ingridient = ingridient.Replace("drop_", "");
    ingridient = "goo_" + ingridient;
    return ingridient;
  }

  static ItemDefinition()
  {
    ItemDefinition itemDefinition = new ItemDefinition();
    itemDefinition.id = "";
    ItemDefinition.none = itemDefinition;
  }

  public enum QualityType
  {
    Default,
    Stars,
  }

  public enum ItemType
  {
    PseudoitemFirst = -1, // 0xFFFFFFFF
    None = 0,
    Axe = 1,
    Pickaxe = 2,
    Shovel = 3,
    Sword = 4,
    Hammer = 5,
    FishingRod = 6,
    Torch = 9,
    Hand = 10, // 0x0000000A
    Item = 11, // 0x0000000B
    HeadArmor = 12, // 0x0000000C
    BodyArmor = 13, // 0x0000000D
    Preach = 20, // 0x00000014
    Bait = 31, // 0x0000001F
    Crate = 50, // 0x00000032
    Rat = 60, // 0x0000003C
    RatBuff = 61, // 0x0000003D
    GraveStone = 101, // 0x00000065
    GraveFence = 102, // 0x00000066
    GraveCover = 103, // 0x00000067
    Body = 200, // 0x000000C8
    BodyHead = 201, // 0x000000C9
    BodyBody = 202, // 0x000000CA
    BodyArmR = 203, // 0x000000CB
    BodyArmL = 204, // 0x000000CC
    BodyLegR = 205, // 0x000000CD
    BodyLegL = 206, // 0x000000CE
    BodyHeadPart = 210, // 0x000000D2
    BodyBodyPart = 220, // 0x000000DC
    BodyArmPart = 230, // 0x000000E6
    BodyLegPart = 250, // 0x000000FA
    BodyUniversalPart = 270, // 0x0000010E
    SoulBodyPart = 271, // 0x0000010F
    Soul = 280, // 0x00000118
    ZombieWorker = 300, // 0x0000012C
    Bag = 400, // 0x00000190
    GraveStoneReq = 10101, // 0x00002775
    GraveFenceReq = 10102, // 0x00002776
    GraveCoverReq = 10103, // 0x00002777
    PseudoitemLast = 100000, // 0x000186A0
  }

  public enum EquipmentType
  {
    None = 0,
    Axe = 1,
    Pickaxe = 2,
    Shovel = 3,
    Sword = 4,
    Hammer = 5,
    FishingRod = 6,
    HeadArmor = 12, // 0x0000000C
    BodyArmor = 13, // 0x0000000D
  }

  public enum BagType
  {
    None,
    Universal,
    Alchemy,
    Farming,
    Fishing,
    Tools,
    Potions,
    Builder,
    Food,
  }

  public class ItemDetails
  {
    public ItemDefinition.ItemDetailsAlchemy alchemy;
    public List<ObjectDefinition> crafts_in = new List<ObjectDefinition>();
  }

  public class ItemDetailsAlchemy
  {
    public ItemDefinition.ItemDetailsAlchemy.DetailsType details_type;
    public List<List<bool>> slots = new List<List<bool>>();
    public List<int> decomposes = new List<int>();

    public enum DetailsType
    {
      Decompose,
      Slots,
    }
  }

  public enum AlchemyType
  {
    None = 0,
    Powder = 1,
    Fluid = 2,
    Essence = 3,
    Universal = 9,
  }

  [Serializable]
  public class ItemReplaceData
  {
    public string player_flag;
    public string replace_id;
  }
}
