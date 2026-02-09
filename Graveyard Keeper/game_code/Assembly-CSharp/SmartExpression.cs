// Decompiled with JetBrains decompiler
// Type: SmartExpression
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Expressive;
using Expressive.Exceptions;
using Expressive.Expressions;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartExpression
{
  [SerializeField]
  public string _expression = "";
  [SerializeField]
  public bool _simplified;
  [SerializeField]
  public float _simpified_float;
  public Expression _exp;
  public WorldGameObject _wgo;
  public WorldGameObject _character;
  [NonSerialized]
  public List<Item> _items;
  public float default_value;
  public static Dictionary<string, string> equatings = new Dictionary<string, string>()
  {
    {
      "\\$(\\w*) *\\+= *(.*)",
      "AddPpar"
    },
    {
      "\\$(\\w*) *\\-= *(.*)",
      "DecPpar"
    },
    {
      "\\$(\\w*) *\\*= *(.*)",
      "MultiplyPpar"
    },
    {
      "\\$(\\w*) *\\/= *(.*)",
      "DividePpar"
    },
    {
      "\\$(\\w*) *\\= *([^=].*)",
      "SetPpar"
    }
  };

  public bool has_expression => !string.IsNullOrEmpty(this._expression);

  public void FromString(string s)
  {
    s = s.Replace("&quot;", "\"");
    this._expression = s;
    this._exp = (Expression) null;
    this._simplified = float.TryParse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out this._simpified_float);
  }

  public bool HasNoExpresion() => string.IsNullOrEmpty(this._expression);

  public static WorldGameObject player => MainGame.me.player;

  public static GameSave save => MainGame.me.save;

  public void CheckExpressionInit()
  {
    if (this._exp != null || this._simplified)
      return;
    this._exp = new Expression(this._expression);
    this._exp.RegisterFunction("WGOpar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
        return (object) this._wgo.GetParam(pars[0].EvaluateAsString(values));
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) 0;
    }));
    this._exp.RegisterFunction("Ppar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) SmartExpression.player.GetParam(pars[0].EvaluateAsString(values))));
    this._exp.RegisterFunction("Ife", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => pars[0].EvaluateAsFloat(values).EqualsTo(pars[1].EvaluateAsFloat(values), 1f / 1000f) ? (object) 1f : (object) 0));
    this._exp.RegisterFunction("Ifn", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => pars[0].EvaluateAsBoolean(values) ? (object) 1f : (object) 0.0f));
    this._exp.RegisterFunction("GetDay", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) SmartExpression.save.day));
    this._exp.RegisterFunction("IsDay", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => !TimeOfDay.me.is_night ? (object) 1f : (object) 0.0f));
    this._exp.RegisterFunction("IsNight", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => TimeOfDay.me.is_night ? (object) 1f : (object) 0.0f));
    this._exp.RegisterFunction("GetTime", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) TimeOfDay.me.GetTimeK()));
    this._exp.RegisterFunction("GetTotalTime", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) MainGame.game_time));
    this._exp.RegisterFunction("GetSessionTime", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) MainGame.session_time));
    this._exp.RegisterFunction("SetPpar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      SmartExpression.player.SetParam(pars[0].EvaluateAsString(values), pars[1].EvaluateAsFloat(values));
      return (object) SmartExpression.player.GetParam(pars[0].EvaluateAsString(values));
    }));
    this._exp.RegisterFunction("AddPpar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      string asString = pars[0].EvaluateAsString(values);
      float num = SmartExpression.player.GetParam(asString) + pars[1].EvaluateAsFloat(values);
      switch (asString)
      {
        case "hp":
          if ((double) num > (double) MainGame.me.save.max_hp)
          {
            num = (float) MainGame.me.save.max_hp;
            break;
          }
          break;
        case "energy":
          if ((double) num < 0.0)
          {
            num = 0.0f;
            break;
          }
          if ((double) num > (double) MainGame.me.save.max_energy)
          {
            num = (float) MainGame.me.save.max_energy;
            break;
          }
          break;
      }
      SmartExpression.player.SetParam(asString, num);
      return (object) num;
    }));
    this._exp.RegisterFunction("DecPpar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      string asString = pars[0].EvaluateAsString(values);
      float num = SmartExpression.player.GetParam(asString) - pars[1].EvaluateAsFloat(values);
      switch (asString)
      {
        case "hp":
          if ((double) num > (double) MainGame.me.save.max_hp)
          {
            num = (float) MainGame.me.save.max_hp;
            break;
          }
          break;
        case "energy":
          if ((double) num < 0.0)
          {
            num = 0.0f;
            break;
          }
          if ((double) num > (double) MainGame.me.save.max_energy)
          {
            num = (float) MainGame.me.save.max_energy;
            break;
          }
          break;
      }
      SmartExpression.player.SetParam(asString, num);
      return (object) num;
    }));
    this._exp.RegisterFunction("MultiplyPpar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      string asString = pars[0].EvaluateAsString(values);
      float num = SmartExpression.player.GetParam(asString) * pars[1].EvaluateAsFloat(values);
      switch (asString)
      {
        case "hp":
          if ((double) num > (double) MainGame.me.save.max_hp)
          {
            num = (float) MainGame.me.save.max_hp;
            break;
          }
          break;
        case "energy":
          if ((double) num < 0.0)
          {
            num = 0.0f;
            break;
          }
          if ((double) num > (double) MainGame.me.save.max_energy)
          {
            num = (float) MainGame.me.save.max_energy;
            break;
          }
          break;
      }
      SmartExpression.player.SetParam(asString, num);
      return (object) num;
    }));
    this._exp.RegisterFunction("DividePpar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      string asString = pars[0].EvaluateAsString(values);
      float asFloat = pars[1].EvaluateAsFloat(values);
      float num1 = SmartExpression.player.GetParam(asString);
      if (asFloat.EqualsTo(0.0f))
        return (object) num1;
      float num2 = num1 / asFloat;
      switch (asString)
      {
        case "hp":
          if ((double) num2 > (double) MainGame.me.save.max_hp)
          {
            num2 = (float) MainGame.me.save.max_hp;
            break;
          }
          break;
        case "energy":
          if ((double) num2 < 0.0)
          {
            num2 = 0.0f;
            break;
          }
          if ((double) num2 > (double) MainGame.me.save.max_energy)
          {
            num2 = (float) MainGame.me.save.max_energy;
            break;
          }
          break;
      }
      SmartExpression.player.SetParam(asString, num2);
      return (object) num2;
    }));
    this._exp.RegisterFunction("HasOverheadBody", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      return overheadItem == null || overheadItem.IsEmpty() ? (object) false : (object) (overheadItem.definition.type == ItemDefinition.ItemType.Body);
    }));
    this._exp.RegisterFunction("HasBodyInWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
        return (object) (this._wgo.GetBodyFromInventory() != null);
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }));
    this._exp.RegisterFunction("CanTakeWoodFromWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      return SmartExpression.player.components.character.has_overhead ? (object) false : (object) (this._wgo.data.GetItemWithID("wood") != null);
    }));
    this._exp.RegisterFunction("CanTakeStoneFromWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      return SmartExpression.player.components.character.has_overhead ? (object) false : (object) (bool) (this._wgo.data.GetItemWithID("stone") != null ? 1 : (this._wgo.data.GetItemWithID("marble") != null ? 1 : 0));
    }));
    this._exp.RegisterFunction("CanTakeOreMetalFromWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      return SmartExpression.player.components.character.has_overhead ? (object) false : (object) (this._wgo.data.GetItemWithID("ore_metal") != null);
    }));
    this._exp.RegisterFunction("HasItemInWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      if (pars.Length == 0)
      {
        Debug.LogError((object) "Function \"HasItemInWGO\" need parameters!");
        return (object) false;
      }
      for (int index = 0; index < pars.Length; ++index)
      {
        string asString = pars[index].EvaluateAsString(values);
        if (string.IsNullOrEmpty(asString))
        {
          Debug.LogError((object) "Function \"HasItemInWGO\" parameter is null!");
          return (object) false;
        }
        if (this._wgo.data.GetItemWithID(asString) != null)
          return (object) true;
      }
      return (object) false;
    }));
    this._exp.RegisterFunction("TakeItemFromWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      if (this._wgo.data.inventory.Count == 0)
      {
        Debug.LogError((object) "Can not take smth from WGO: WGO inventory is empty!");
        return (object) false;
      }
      Item obj = this._wgo.data.inventory[this._wgo.data.inventory.Count - 1];
      this._wgo.data.RemoveItem(obj, 1);
      if (obj.definition.item_size == 2)
      {
        if (SmartExpression.player.components.character.has_overhead)
          SmartExpression.player.components.character.DropOverheadItem(true);
        SmartExpression.player.components.character.SetOverheadItem(obj);
      }
      else
        SmartExpression.player.AddToInventory(obj.id, 1);
      this._wgo.Redraw();
      MainGame.me.player.components.interaction.UpdateNearestHint();
      return (object) true;
    }));
    this._exp.RegisterFunction("CanPutOverheadToWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
        return (object) this._wgo.CanInsertItem(overheadItem);
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }));
    this._exp.RegisterFunction("CanPutOverheadToCrematorium", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
        return (object) (bool) (this._wgo.GetParamInt("is_body_inserted") != 0 ? 0 : (this._wgo.CanInsertItem(overheadItem) ? 1 : 0));
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }));
    this._exp.RegisterFunction("PutPromoZombie", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      if (!this._wgo.CanInsertItem(overheadItem))
        return (object) false;
      if (overheadItem.id != "working_zombie_pseudoitem_1")
        return (object) false;
      this._wgo.AddToInventory(overheadItem);
      SmartExpression.player.components.character.SetOverheadItem((Item) null);
      GDPoint componentInChildren = this._wgo.GetComponentInChildren<GDPoint>(true);
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        WorldMap.SpawnWGO(MainGame.me.world_root, "zombie_promo", (Vector2) componentInChildren.transform.position, "zombie_promo");
      return (object) true;
    }));
    this._exp.RegisterFunction("CanTakePromoZombie", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      return this._wgo.data.GetTotalCount("working_zombie_pseudoitem_1") < 1 ? (object) false : (object) true;
    }));
    this._exp.RegisterFunction("TakePromoZombie", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      if (this._wgo.data.GetTotalCount("working_zombie_pseudoitem_1") < 1)
        return (object) false;
      if (MainGame.me.player_char.has_overhead)
      {
        Item overheadItem = MainGame.me.player_char.GetOverheadItem();
        MainGame.me.player.DropItem(overheadItem);
        MainGame.me.player_char.SetOverheadItem((Item) null);
      }
      Item obj1 = (Item) null;
      foreach (Item obj2 in this._wgo.data.inventory)
      {
        if (obj2 != null && obj2.value >= 1 && !(obj2.id != "working_zombie_pseudoitem_1"))
        {
          obj1 = obj2;
          break;
        }
      }
      if (obj1 == null)
        return (object) false;
      this._wgo.data.inventory.Remove(obj1);
      MainGame.me.player_char.SetOverheadItem(obj1);
      WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("zombie_promo");
      if ((UnityEngine.Object) objectByCustomTag != (UnityEngine.Object) null)
        objectByCustomTag.DestroyMe();
      return (object) true;
    }));
    this._exp.RegisterFunction("CanLinkOverheadZombieWorker", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if (!overheadItem.id.StartsWith("working_zombie_pseudoitem"))
        return (object) false;
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
        return (object) false;
      if (this._wgo.has_linked_worker)
        return (object) false;
      DockPoint dockPointForZombie = this._wgo.GetAvailableDockPointForZombie();
      return (UnityEngine.Object) dockPointForZombie == (UnityEngine.Object) null || (UnityEngine.Object) dockPointForZombie.tf == (UnityEngine.Object) null ? (object) false : (object) true;
    }));
    this._exp.RegisterFunction("LinkOverheadZombieWorker", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if (!overheadItem.id.StartsWith("working_zombie_pseudoitem"))
        return (object) false;
      bool is_success;
      string message = WorldMap.SpawnZombieWorkerFromStock(this._wgo, overheadItem, out WorldGameObject _, out is_success);
      if (!string.IsNullOrEmpty(message))
        Debug.LogError((object) message);
      if (!is_success)
        return (object) false;
      SmartExpression.player.components.character.SetOverheadItem((Item) null);
      if ((UnityEngine.Object) this._wgo != (UnityEngine.Object) null)
      {
        switch (this._wgo.obj_id)
        {
          case "zombie_sawmill_completed":
            string craft_name1 = "zombie_sawmill_wood_production";
            if (!this._wgo.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_sawmill_completed has no craft!");
              this._wgo.TryStartCraft(craft_name1);
              break;
            }
            break;
          case "zombie_mine_fence_left_front":
            string craft_name2 = "zombie_mine_stone_production";
            if (!this._wgo.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_left_front has no craft!");
              this._wgo.TryStartCraft(craft_name2);
              break;
            }
            break;
          case "zombie_mine_fence_front":
            string craft_name3 = "zombie_mine_marble_production";
            try
            {
              if ((double) ((Vector2) this._wgo.transform.position - new Vector2(-3932f, 6622f)).sqrMagnitude < 10.0)
                craft_name3 = "zombie_mine_stone_production";
            }
            catch (Exception ex)
            {
              Debug.LogError((object) ex);
            }
            if (!this._wgo.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_front has no craft!");
              this._wgo.TryStartCraft(craft_name3);
              break;
            }
            break;
        }
      }
      return (object) true;
    }));
    this._exp.RegisterFunction("HasLinkedZombieWorker", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
        return (object) false;
      if (!this._wgo.has_linked_worker)
        return (object) false;
      return !this._wgo.components.craft.is_crafting ? (object) false : (object) true;
    }));
    this._exp.RegisterFunction("TakeLinkedZombieWorker", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
        return (object) false;
      if (!this._wgo.has_linked_worker)
        return (object) false;
      if (!this._wgo.components.craft.is_crafting)
        return (object) false;
      if (MainGame.me.player_char.has_overhead)
      {
        Item overheadItem = MainGame.me.player_char.GetOverheadItem();
        MainGame.me.player.DropItem(overheadItem);
        MainGame.me.player_char.SetOverheadItem((Item) null);
      }
      if (string.IsNullOrEmpty(this._wgo.linked_worker?.worker?.definition?.item_overhead))
      {
        Debug.LogError((object) "FATAL ERROR: can not take worker from workbench: worker_item_id is NULL!");
        return (object) false;
      }
      Worker worker = this._wgo.linked_worker.worker;
      string stock = WorldMap.RemoveZombieWorkerToStock(this._wgo.linked_worker);
      if (!string.IsNullOrEmpty(stock))
        Debug.LogError((object) stock);
      MainGame.me.player_char.SetOverheadItem(worker.GetOverheadItem());
      return (object) true;
    }));
    this._exp.RegisterFunction("LinkOverheadWorkerToMine", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if (!overheadItem.id.StartsWith("working_zombie_pseudoitem"))
        return (object) false;
      bool is_success;
      string message = WorldMap.SpawnZombieWorkerFromStock(this._wgo, overheadItem, out WorldGameObject _, out is_success);
      if (!string.IsNullOrEmpty(message))
        Debug.LogError((object) message);
      if (!is_success)
        return (object) false;
      SmartExpression.player.components.character.SetOverheadItem((Item) null);
      WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("mine_zombie");
      if ((UnityEngine.Object) objectByCustomTag == (UnityEngine.Object) null)
        Debug.LogError((object) "FATAL ERROR: mine_zombie not found on map!");
      else
        objectByCustomTag.data.AddToParams("zombies_inside", 1f);
      if ((UnityEngine.Object) this._wgo != (UnityEngine.Object) null && this._wgo.obj_id == "mine_zombie_bench" && !this._wgo.components.craft.is_crafting)
      {
        Debug.LogError((object) "FATAL ERROR: mine_zombie_bench has no craft!");
        this._wgo.TryStartCraft("mine_zombie_bench_iron_production");
      }
      return (object) true;
    }));
    this._exp.RegisterFunction("TakeLinkedWorkerFromMine", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
        return (object) false;
      if (!this._wgo.has_linked_worker)
        return (object) false;
      if (!this._wgo.components.craft.is_crafting)
        return (object) false;
      if (MainGame.me.player_char.has_overhead)
      {
        Item overheadItem = MainGame.me.player_char.GetOverheadItem();
        MainGame.me.player.DropItem(overheadItem);
        MainGame.me.player_char.SetOverheadItem((Item) null);
      }
      if (string.IsNullOrEmpty(this._wgo.linked_worker?.worker?.definition?.item_overhead))
      {
        Debug.LogError((object) "FATAL ERROR: can not take worker from workbench: worker_item_id is NULL!");
        return (object) false;
      }
      Worker worker = this._wgo.linked_worker.worker;
      string stock = WorldMap.RemoveZombieWorkerToStock(this._wgo.linked_worker);
      if (!string.IsNullOrEmpty(stock))
        Debug.LogError((object) stock);
      MainGame.me.player_char.SetOverheadItem(worker.GetOverheadItem());
      WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("mine_zombie");
      if ((UnityEngine.Object) objectByCustomTag == (UnityEngine.Object) null)
        Debug.LogError((object) "FATAL ERROR: mine_zombie not found on map!");
      else
        objectByCustomTag.data.AddToParams("zombies_inside", -1f);
      return (object) true;
    }));
    this._exp.RegisterFunction("PutOverheadToWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      if (!this._wgo.CanInsertItem(overheadItem))
      {
        Debug.LogError((object) "Coudln't insert overhead item into WGO", (UnityEngine.Object) this._wgo);
        return (object) false;
      }
      if (this._wgo.AddToInventory(overheadItem))
        SmartExpression.player.components.character.SetOverheadItem((Item) null);
      this._wgo.Redraw();
      MainGame.me.player.components.interaction.UpdateNearestHint();
      return (object) true;
    }));
    this._exp.RegisterFunction("PutBarmanToWGO", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      if (!this._wgo.CanInsertItem(overheadItem))
      {
        Debug.LogError((object) "Coudln't insert overhead item into WGO", (UnityEngine.Object) this._wgo);
        return (object) false;
      }
      if (this._wgo.AddToInventory(overheadItem))
        SmartExpression.player.components.character.SetOverheadItem((Item) null);
      this._wgo.Redraw();
      MainGame.me.player.components.interaction.UpdateNearestHint();
      GS.RunFlowScript("on_barman_placed");
      return (object) true;
    }));
    this._exp.RegisterFunction("OpenCraft", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      GUIElements.me.OpenCraftGUI(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("GetZoneQ", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      WorldZone zoneById = WorldZone.GetZoneByID(pars[0].EvaluateAsString(values));
      return (UnityEngine.Object) zoneById == (UnityEngine.Object) null ? (object) 0.0f : (object) zoneById.GetTotalQuality();
    }));
    this._exp.RegisterFunction("AddBuff", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      BuffsLogics.AddBuff(pars[0].EvaluateAsString(values));
      return (object) true;
    }));
    this._exp.RegisterFunction("AddBuffLen", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      BuffsLogics.AddBuff(pars[0].EvaluateAsString(values), new float?(pars[1].EvaluateAsFloat(values)));
      return (object) true;
    }));
    this._exp.RegisterFunction("RemoveBuff", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      BuffsLogics.RemoveBuff(pars[0].EvaluateAsString(values));
      return (object) true;
    }));
    this._exp.RegisterFunction("BlackList", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) SmartExpression.save.game_logics.AddToBlackList(pars[0].EvaluateAsString(values))));
    this._exp.RegisterFunction("ForceExecute", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) MainGame.me.save.game_logics.ForceExecute(pars[0].EvaluateAsString(values))));
    this._exp.RegisterFunction("ForceExecuteCond", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) MainGame.me.save.game_logics.ForceExecuteCond(pars[0].EvaluateAsString(values))));
    this._exp.RegisterFunction("UnlockTech", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      GUIElements.me.tech_dialog.Open(GameBalance.me.GetData<TechDefinition>(pars[0].EvaluateAsString(values)), (GJCommons.VoidDelegate) null, true);
      return (object) true;
    }));
    this._exp.RegisterFunction("UnlockCraft", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      MainGame.me.save.UnlockCraft(pars[0].EvaluateAsString(values));
      return (object) true;
    }));
    this._exp.RegisterFunction("IsNotCraftingNow", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (!this._wgo.components.craft.enabled)
        return (object) true;
      return this._wgo.components.craft.is_crafting ? (object) false : (object) true;
    }));
    this._exp.RegisterFunction("OpenPorterStationGUI", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      GUIElements.me.OpenPorterStationGUI(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("OpenResurrectionGUI", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      GUIElements.me.OpenResurrectionGUI(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("IsDLCAvailable", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) DLCEngine.IsDLCAvailable((DLCEngine.DLCVersion) pars[0].EvaluateAsInt(values))));
    this._exp.RegisterFunction("IsCraftUnlocked", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) => (object) MainGame.me.save.unlocked_crafts.Contains(pars[0].EvaluateAsString(values))));
    this._exp.RegisterFunction("CanTakeWaterFromPump", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (this._wgo?.data == null)
        return (object) false;
      return this._wgo.data.GetItemsCount("water") == 0 ? (object) false : (object) true;
    }));
    this._exp.RegisterFunction("TakeWaterFromPump", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      int item_value = this._wgo.data.GetItemsCount("water");
      if (item_value > 10)
        item_value = 10;
      this._wgo.DropItem(new Item("water", item_value), Direction.ToPlayer);
      WorldMap.GetWorldGameObjectByObjId("water_well").TriggerSmartAnimation("work");
      this._wgo.data.RemoveItem("water", item_value);
      return (object) true;
    }));
    this._exp.RegisterFunction("TakeWaterFromRefugeeWell", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (!MainGame.me.player.components.character.player.TrySpendEnergy(3f))
        return (object) false;
      this._wgo.DropItem(new Item("water", 10), Direction.ToPlayer);
      this._wgo.TriggerSmartAnimation("work");
      return (object) true;
    }));
    this._exp.RegisterFunction("StartEvent", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      this._wgo.FireEvent(pars[0].EvaluateAsString(values));
      return (object) true;
    }));
    this._exp.RegisterFunction("PlayerHasOverheadItem", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
      if (overheadItem == null || overheadItem.IsEmpty())
        return (object) false;
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      string asString = pars[0].EvaluateAsString(values);
      return overheadItem.id == asString && !string.IsNullOrEmpty(asString) ? (object) true : (object) false;
    }));
    this._exp.RegisterFunction("UnlockRandomAlchemy", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, ValueSerializerAttribute) =>
    {
      List<CraftDefinition> craftDefinitionList1 = new List<CraftDefinition>();
      List<CraftDefinition> craftDefinitionList2 = new List<CraftDefinition>();
      foreach (CraftDefinition craftDefinition in GameBalance.me.craft_data)
      {
        if (craftDefinition.id.StartsWith("mix:") && !craftDefinition.id.Contains("goo") && !craftDefinition.id.Contains(":_:"))
        {
          craftDefinitionList2.Add(craftDefinition);
          if (!MainGame.me.save.completed_one_time_crafts.Contains(craftDefinition.id))
            craftDefinitionList1.Add(craftDefinition);
        }
      }
      if (craftDefinitionList1.Count > 0)
      {
        CraftDefinition craftDefinition = craftDefinitionList1[UnityEngine.Random.Range(0, craftDefinitionList1.Count)];
        MainGame.me.save.completed_one_time_crafts.Add(craftDefinition.id);
        MainGame.me.save.achievements.CheckKeyQuests("new_" + craftDefinition.ach_key);
        string str = string.Empty;
        if (craftDefinition.output == null || craftDefinition.output.Count == 0)
        {
          str = craftDefinition.id;
        }
        else
        {
          foreach (Item obj in craftDefinition.output)
          {
            if (!(obj.id == "r") && !(obj.id == "g") && !(obj.id == "b"))
            {
              str = obj.id;
              break;
            }
          }
          if (string.IsNullOrEmpty(str))
            str = craftDefinition.id;
        }
        GUIElements.me.tech_dialog.Open(new TechDefinition()
        {
          id = str,
          crafts = {
            craftDefinition.id
          },
          price = new GameRes()
        }, (GJCommons.VoidDelegate) (() => { }), true, pseudotech: true);
      }
      else
      {
        if (craftDefinitionList2.Count == 0)
        {
          Debug.LogError((object) "UnlockRandomAlchemy error: no mix crafts in Balance!");
          return (object) false;
        }
        List<Item> items = ResModificator.ProcessItemsListBeforeDrop(craftDefinitionList2[UnityEngine.Random.Range(0, craftDefinitionList2.Count)].output, (WorldGameObject) null, MainGame.me.player);
        MainGame.me.player.DropItems(items);
      }
      return (object) false;
    }));
    this._exp.RegisterFunction("UnlockAlchemy", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      string asString = pars[0].EvaluateAsString(values);
      CraftDefinition craftDefinition1 = (CraftDefinition) null;
      for (int index = 0; index < GameBalance.me.craft_data.Count; ++index)
      {
        CraftDefinition craftDefinition2 = GameBalance.me.craft_data[index];
        if (craftDefinition2.id == asString && !MainGame.me.save.completed_one_time_crafts.Contains(asString))
        {
          MainGame.me.save.completed_one_time_crafts.Add(craftDefinition2.id);
          craftDefinition1 = craftDefinition2;
        }
      }
      if (craftDefinition1 != null)
      {
        string str = string.Empty;
        if (craftDefinition1.output == null || craftDefinition1.output.Count == 0)
        {
          str = craftDefinition1.id;
        }
        else
        {
          foreach (Item obj in craftDefinition1.output)
          {
            if (!(obj.id == "r") && !(obj.id == "g") && !(obj.id == "b"))
            {
              str = obj.id;
              break;
            }
          }
          if (string.IsNullOrEmpty(str))
            str = craftDefinition1.id;
        }
        GUIElements.me.tech_dialog.Open(new TechDefinition()
        {
          id = str,
          crafts = {
            craftDefinition1.id
          },
          price = new GameRes()
        }, (GJCommons.VoidDelegate) (() => { }), true, pseudotech: true);
      }
      return (object) false;
    }));
    this._exp.RegisterFunction("OpenSoulContainerWindow", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      GUIElements.me.soul_container_gui.Open(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("OpenSoulHealingWindow", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      GUIElements.me.soul_healer_gui.Open(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("FillCraftsList", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      this._wgo.components.craft.FillCraftsList();
      GUIElements.me.OpenCraftGUI(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("OpenSoulWorkbenchWindow", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
        return (object) false;
      }
      GUIElements.me.organ_enhancer_gui.Open(this._wgo);
      return (object) true;
    }));
    this._exp.RegisterFunction("GetItemPar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (this._items.IsNullOrEmpty<Item>())
      {
        Debug.LogError((object) $"Item is null while evaluation expression: {this._exp}");
        return (object) 0.0f;
      }
      string asString1 = pars[0].EvaluateAsString(values);
      string asString2 = pars[1].EvaluateAsString(values);
      foreach (Item obj in this._items)
      {
        if (!(obj.id != asString1))
          return (object) obj.GetParam(asString2);
      }
      Debug.LogError((object) $"Item with id {asString1} not found in expression: {this._exp}");
      return (object) 0.0f;
    }));
    this._exp.RegisterFunction("SetItemPar", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (this._items.IsNullOrEmpty<Item>())
      {
        Debug.LogError((object) $"Item is null or empty while evaluation expression: {this._exp}");
        return (object) 0.0f;
      }
      string asString3 = pars[0].EvaluateAsString(values);
      string asString4 = pars[1].EvaluateAsString(values);
      float asFloat = pars[2].EvaluateAsFloat(values);
      foreach (Item obj in this._items)
      {
        if (!(obj.id != asString3))
          obj.SetParam(asString4, asFloat);
      }
      return (object) true;
    }));
    this._exp.RegisterFunction("GetItemRS", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (this._items.IsNullOrEmpty<Item>())
      {
        Debug.LogError((object) $"Item is null or empty while evaluation expression: {this._exp}");
        return (object) 0.0f;
      }
      string asString = pars[0].EvaluateAsString(values);
      foreach (Item obj in this._items)
      {
        if (!(obj.id != asString))
          return (object) obj.GetRedSkullsValue();
      }
      Debug.LogError((object) $"Item with id {asString} not found in expression: {this._exp}");
      return (object) 0.0f;
    }));
    this._exp.RegisterFunction("GetItemWS", (Func<IExpression[], IDictionary<string, object>, object>) ((pars, values) =>
    {
      if (this._items.IsNullOrEmpty<Item>())
      {
        Debug.LogError((object) $"Item is null or empty while evaluation expression: {this._exp}");
        return (object) 0.0f;
      }
      string asString = pars[0].EvaluateAsString(values);
      foreach (Item obj in this._items)
      {
        if (!(obj.id != asString))
          return (object) obj.GetWhiteSkullsValue();
      }
      Debug.LogError((object) $"Item with id {asString} not found in expression: {this._exp}");
      return (object) 0.0f;
    }));
  }

  public bool EvaluateChance(WorldGameObject wgo = null, WorldGameObject character = null)
  {
    float num = this.EvaluateFloat(wgo, character);
    return (double) num >= 0.0099999997764825821 && (double) num > (double) UnityEngine.Random.Range(0.0f, 1f);
  }

  public bool EvaluateBoolean(WorldGameObject wgo = null, WorldGameObject character = null)
  {
    if (string.IsNullOrEmpty(this._expression))
      return true;
    if (this._simplified)
      return (double) this._simpified_float > 0.0;
    this._wgo = wgo;
    this._character = character;
    this.CheckExpressionInit();
    try
    {
      return Convert.ToBoolean(this._exp.Evaluate());
    }
    catch (ExpressiveException ex)
    {
      if (ex.ToString().Contains("NullReference"))
        throw;
      Debug.LogError((object) $"Error in expression: {ex?.ToString()}\n{this._expression}");
      return true;
    }
  }

  public float EvaluateFloat(WorldGameObject wgo = null, WorldGameObject character = null)
  {
    if (string.IsNullOrEmpty(this._expression))
      return this.default_value;
    if (this._simplified)
      return this._simpified_float;
    this._wgo = wgo;
    this._character = character;
    this.CheckExpressionInit();
    try
    {
      return (float) Convert.ToDecimal(this._exp.Evaluate());
    }
    catch (ExpressiveException ex)
    {
      if (ex.ToString().Contains("NullReference"))
        throw;
      Debug.LogError((object) $"Error in expression: {ex?.ToString()}\n{this._expression}");
      return 1f;
    }
  }

  public void Evaluate(WorldGameObject wgo = null, WorldGameObject character = null)
  {
    if (string.IsNullOrEmpty(this._expression) || this._simplified)
      return;
    this._wgo = wgo;
    this._character = character;
    this.CheckExpressionInit();
    try
    {
      this._exp.Evaluate();
    }
    catch (ExpressiveException ex)
    {
      if (ex.ToString().Contains("NullReference"))
        throw;
      Debug.LogError((object) $"Error in expression: {ex?.ToString()}\n{this._expression}");
    }
  }

  public void Evaluate(List<Item> items)
  {
    if (string.IsNullOrEmpty(this._expression) || this._simplified)
      return;
    this._items = items;
    this.CheckExpressionInit();
    try
    {
      this._exp.Evaluate();
    }
    catch (ExpressiveException ex)
    {
      if (ex.ToString().Contains("NullReference"))
        throw;
      Debug.LogError((object) $"Error in expression: {ex?.ToString()}\n{this._expression}");
    }
  }

  public static SmartExpression ParseExpression(string expression)
  {
    if (string.IsNullOrEmpty(expression))
      return (SmartExpression) null;
    expression = SmartExpression.ParseRegex(expression);
    SmartExpression expression1 = new SmartExpression();
    expression1.FromString(expression);
    return expression1;
  }

  public static string ParseRegex(string input)
  {
    foreach (string key in SmartExpression.equatings.Keys)
      input = Regex.Replace(input, key, SmartExpression.equatings[key] + "(\"$1\", $2)");
    input = Regex.Replace(input, "\\$(\\w*)", "Ppar(\"$1\")");
    input = Regex.Replace(input, "@(\\w*)", "WGOpar(\"$1\")");
    return input;
  }

  public string GetRawExpressionString() => this._expression;

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_0(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
      return (object) this._wgo.GetParam(pars[0].EvaluateAsString(values));
    Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
    return (object) 0;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_16(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
      return (object) (this._wgo.GetBodyFromInventory() != null);
    Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
    return (object) false;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_17(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    return SmartExpression.player.components.character.has_overhead ? (object) false : (object) (this._wgo.data.GetItemWithID("wood") != null);
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_18(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    return SmartExpression.player.components.character.has_overhead ? (object) false : (object) (bool) (this._wgo.data.GetItemWithID("stone") != null ? 1 : (this._wgo.data.GetItemWithID("marble") != null ? 1 : 0));
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_19(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    return SmartExpression.player.components.character.has_overhead ? (object) false : (object) (this._wgo.data.GetItemWithID("ore_metal") != null);
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_20(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    if (pars.Length == 0)
    {
      Debug.LogError((object) "Function \"HasItemInWGO\" need parameters!");
      return (object) false;
    }
    for (int index = 0; index < pars.Length; ++index)
    {
      string asString = pars[index].EvaluateAsString(values);
      if (string.IsNullOrEmpty(asString))
      {
        Debug.LogError((object) "Function \"HasItemInWGO\" parameter is null!");
        return (object) false;
      }
      if (this._wgo.data.GetItemWithID(asString) != null)
        return (object) true;
    }
    return (object) false;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_21(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    if (this._wgo.data.inventory.Count == 0)
    {
      Debug.LogError((object) "Can not take smth from WGO: WGO inventory is empty!");
      return (object) false;
    }
    Item obj = this._wgo.data.inventory[this._wgo.data.inventory.Count - 1];
    this._wgo.data.RemoveItem(obj, 1);
    if (obj.definition.item_size == 2)
    {
      if (SmartExpression.player.components.character.has_overhead)
        SmartExpression.player.components.character.DropOverheadItem(true);
      SmartExpression.player.components.character.SetOverheadItem(obj);
    }
    else
      SmartExpression.player.AddToInventory(obj.id, 1);
    this._wgo.Redraw();
    MainGame.me.player.components.interaction.UpdateNearestHint();
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_22(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
      return (object) this._wgo.CanInsertItem(overheadItem);
    Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
    return (object) false;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_23(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if (!((UnityEngine.Object) this._wgo == (UnityEngine.Object) null))
      return (object) (bool) (this._wgo.GetParamInt("is_body_inserted") != 0 ? 0 : (this._wgo.CanInsertItem(overheadItem) ? 1 : 0));
    Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
    return (object) false;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_24(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    if (!this._wgo.CanInsertItem(overheadItem))
      return (object) false;
    if (overheadItem.id != "working_zombie_pseudoitem_1")
      return (object) false;
    this._wgo.AddToInventory(overheadItem);
    SmartExpression.player.components.character.SetOverheadItem((Item) null);
    GDPoint componentInChildren = this._wgo.GetComponentInChildren<GDPoint>(true);
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      WorldMap.SpawnWGO(MainGame.me.world_root, "zombie_promo", (Vector2) componentInChildren.transform.position, "zombie_promo");
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_25(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    return this._wgo.data.GetTotalCount("working_zombie_pseudoitem_1") < 1 ? (object) false : (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_26(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    if (this._wgo.data.GetTotalCount("working_zombie_pseudoitem_1") < 1)
      return (object) false;
    if (MainGame.me.player_char.has_overhead)
    {
      Item overheadItem = MainGame.me.player_char.GetOverheadItem();
      MainGame.me.player.DropItem(overheadItem);
      MainGame.me.player_char.SetOverheadItem((Item) null);
    }
    Item obj1 = (Item) null;
    foreach (Item obj2 in this._wgo.data.inventory)
    {
      if (obj2 != null && obj2.value >= 1 && !(obj2.id != "working_zombie_pseudoitem_1"))
      {
        obj1 = obj2;
        break;
      }
    }
    if (obj1 == null)
      return (object) false;
    this._wgo.data.inventory.Remove(obj1);
    MainGame.me.player_char.SetOverheadItem(obj1);
    WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("zombie_promo");
    if ((UnityEngine.Object) objectByCustomTag != (UnityEngine.Object) null)
      objectByCustomTag.DestroyMe();
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_27(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if (!overheadItem.id.StartsWith("working_zombie_pseudoitem"))
      return (object) false;
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      return (object) false;
    if (this._wgo.has_linked_worker)
      return (object) false;
    DockPoint dockPointForZombie = this._wgo.GetAvailableDockPointForZombie();
    return (UnityEngine.Object) dockPointForZombie == (UnityEngine.Object) null || (UnityEngine.Object) dockPointForZombie.tf == (UnityEngine.Object) null ? (object) false : (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_28(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if (!overheadItem.id.StartsWith("working_zombie_pseudoitem"))
      return (object) false;
    bool is_success;
    string message = WorldMap.SpawnZombieWorkerFromStock(this._wgo, overheadItem, out WorldGameObject _, out is_success);
    if (!string.IsNullOrEmpty(message))
      Debug.LogError((object) message);
    if (!is_success)
      return (object) false;
    SmartExpression.player.components.character.SetOverheadItem((Item) null);
    if ((UnityEngine.Object) this._wgo != (UnityEngine.Object) null)
    {
      switch (this._wgo.obj_id)
      {
        case "zombie_sawmill_completed":
          string craft_name1 = "zombie_sawmill_wood_production";
          if (!this._wgo.components.craft.is_crafting)
          {
            Debug.LogError((object) "FATAL ERROR: zombie_sawmill_completed has no craft!");
            this._wgo.TryStartCraft(craft_name1);
            break;
          }
          break;
        case "zombie_mine_fence_left_front":
          string craft_name2 = "zombie_mine_stone_production";
          if (!this._wgo.components.craft.is_crafting)
          {
            Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_left_front has no craft!");
            this._wgo.TryStartCraft(craft_name2);
            break;
          }
          break;
        case "zombie_mine_fence_front":
          string craft_name3 = "zombie_mine_marble_production";
          try
          {
            if ((double) ((Vector2) this._wgo.transform.position - new Vector2(-3932f, 6622f)).sqrMagnitude < 10.0)
              craft_name3 = "zombie_mine_stone_production";
          }
          catch (Exception ex)
          {
            Debug.LogError((object) ex);
          }
          if (!this._wgo.components.craft.is_crafting)
          {
            Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_front has no craft!");
            this._wgo.TryStartCraft(craft_name3);
            break;
          }
          break;
      }
    }
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_29(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      return (object) false;
    if (!this._wgo.has_linked_worker)
      return (object) false;
    return !this._wgo.components.craft.is_crafting ? (object) false : (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_30(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      return (object) false;
    if (!this._wgo.has_linked_worker)
      return (object) false;
    if (!this._wgo.components.craft.is_crafting)
      return (object) false;
    if (MainGame.me.player_char.has_overhead)
    {
      Item overheadItem = MainGame.me.player_char.GetOverheadItem();
      MainGame.me.player.DropItem(overheadItem);
      MainGame.me.player_char.SetOverheadItem((Item) null);
    }
    if (string.IsNullOrEmpty(this._wgo.linked_worker?.worker?.definition?.item_overhead))
    {
      Debug.LogError((object) "FATAL ERROR: can not take worker from workbench: worker_item_id is NULL!");
      return (object) false;
    }
    Worker worker = this._wgo.linked_worker.worker;
    string stock = WorldMap.RemoveZombieWorkerToStock(this._wgo.linked_worker);
    if (!string.IsNullOrEmpty(stock))
      Debug.LogError((object) stock);
    MainGame.me.player_char.SetOverheadItem(worker.GetOverheadItem());
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_31(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if (!overheadItem.id.StartsWith("working_zombie_pseudoitem"))
      return (object) false;
    bool is_success;
    string message = WorldMap.SpawnZombieWorkerFromStock(this._wgo, overheadItem, out WorldGameObject _, out is_success);
    if (!string.IsNullOrEmpty(message))
      Debug.LogError((object) message);
    if (!is_success)
      return (object) false;
    SmartExpression.player.components.character.SetOverheadItem((Item) null);
    WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("mine_zombie");
    if ((UnityEngine.Object) objectByCustomTag == (UnityEngine.Object) null)
      Debug.LogError((object) "FATAL ERROR: mine_zombie not found on map!");
    else
      objectByCustomTag.data.AddToParams("zombies_inside", 1f);
    if ((UnityEngine.Object) this._wgo != (UnityEngine.Object) null && this._wgo.obj_id == "mine_zombie_bench" && !this._wgo.components.craft.is_crafting)
    {
      Debug.LogError((object) "FATAL ERROR: mine_zombie_bench has no craft!");
      this._wgo.TryStartCraft("mine_zombie_bench_iron_production");
    }
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_32(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
      return (object) false;
    if (!this._wgo.has_linked_worker)
      return (object) false;
    if (!this._wgo.components.craft.is_crafting)
      return (object) false;
    if (MainGame.me.player_char.has_overhead)
    {
      Item overheadItem = MainGame.me.player_char.GetOverheadItem();
      MainGame.me.player.DropItem(overheadItem);
      MainGame.me.player_char.SetOverheadItem((Item) null);
    }
    if (string.IsNullOrEmpty(this._wgo.linked_worker?.worker?.definition?.item_overhead))
    {
      Debug.LogError((object) "FATAL ERROR: can not take worker from workbench: worker_item_id is NULL!");
      return (object) false;
    }
    Worker worker = this._wgo.linked_worker.worker;
    string stock = WorldMap.RemoveZombieWorkerToStock(this._wgo.linked_worker);
    if (!string.IsNullOrEmpty(stock))
      Debug.LogError((object) stock);
    MainGame.me.player_char.SetOverheadItem(worker.GetOverheadItem());
    WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("mine_zombie");
    if ((UnityEngine.Object) objectByCustomTag == (UnityEngine.Object) null)
      Debug.LogError((object) "FATAL ERROR: mine_zombie not found on map!");
    else
      objectByCustomTag.data.AddToParams("zombies_inside", -1f);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_33(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    if (!this._wgo.CanInsertItem(overheadItem))
    {
      Debug.LogError((object) "Coudln't insert overhead item into WGO", (UnityEngine.Object) this._wgo);
      return (object) false;
    }
    if (this._wgo.AddToInventory(overheadItem))
      SmartExpression.player.components.character.SetOverheadItem((Item) null);
    this._wgo.Redraw();
    MainGame.me.player.components.interaction.UpdateNearestHint();
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_34(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    if (!this._wgo.CanInsertItem(overheadItem))
    {
      Debug.LogError((object) "Coudln't insert overhead item into WGO", (UnityEngine.Object) this._wgo);
      return (object) false;
    }
    if (this._wgo.AddToInventory(overheadItem))
      SmartExpression.player.components.character.SetOverheadItem((Item) null);
    this._wgo.Redraw();
    MainGame.me.player.components.interaction.UpdateNearestHint();
    GS.RunFlowScript("on_barman_placed");
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_35(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    GUIElements.me.OpenCraftGUI(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_45(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (!this._wgo.components.craft.enabled)
      return (object) true;
    return this._wgo.components.craft.is_crafting ? (object) false : (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_46(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    GUIElements.me.OpenPorterStationGUI(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_47(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    GUIElements.me.OpenResurrectionGUI(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_50(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (this._wgo?.data == null)
      return (object) false;
    return this._wgo.data.GetItemsCount("water") == 0 ? (object) false : (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_51(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    int item_value = this._wgo.data.GetItemsCount("water");
    if (item_value > 10)
      item_value = 10;
    this._wgo.DropItem(new Item("water", item_value), Direction.ToPlayer);
    WorldMap.GetWorldGameObjectByObjId("water_well").TriggerSmartAnimation("work");
    this._wgo.data.RemoveItem("water", item_value);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_52(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (!MainGame.me.player.components.character.player.TrySpendEnergy(3f))
      return (object) false;
    this._wgo.DropItem(new Item("water", 10), Direction.ToPlayer);
    this._wgo.TriggerSmartAnimation("work");
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_53(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    this._wgo.FireEvent(pars[0].EvaluateAsString(values));
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_54(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    Item overheadItem = SmartExpression.player.components.character.GetOverheadItem();
    if (overheadItem == null || overheadItem.IsEmpty())
      return (object) false;
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    string asString = pars[0].EvaluateAsString(values);
    return overheadItem.id == asString && !string.IsNullOrEmpty(asString) ? (object) true : (object) false;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_57(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    GUIElements.me.soul_container_gui.Open(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_58(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    GUIElements.me.soul_healer_gui.Open(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_59(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    this._wgo.components.craft.FillCraftsList();
    GUIElements.me.OpenCraftGUI(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_60(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("WGO is null while evaluating expression: " + this._exp?.ToString()));
      return (object) false;
    }
    GUIElements.me.organ_enhancer_gui.Open(this._wgo);
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_61(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (this._items.IsNullOrEmpty<Item>())
    {
      Debug.LogError((object) $"Item is null while evaluation expression: {this._exp}");
      return (object) 0.0f;
    }
    string asString1 = pars[0].EvaluateAsString(values);
    string asString2 = pars[1].EvaluateAsString(values);
    foreach (Item obj in this._items)
    {
      if (!(obj.id != asString1))
        return (object) obj.GetParam(asString2);
    }
    Debug.LogError((object) $"Item with id {asString1} not found in expression: {this._exp}");
    return (object) 0.0f;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_62(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (this._items.IsNullOrEmpty<Item>())
    {
      Debug.LogError((object) $"Item is null or empty while evaluation expression: {this._exp}");
      return (object) 0.0f;
    }
    string asString1 = pars[0].EvaluateAsString(values);
    string asString2 = pars[1].EvaluateAsString(values);
    float asFloat = pars[2].EvaluateAsFloat(values);
    foreach (Item obj in this._items)
    {
      if (!(obj.id != asString1))
        obj.SetParam(asString2, asFloat);
    }
    return (object) true;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_63(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (this._items.IsNullOrEmpty<Item>())
    {
      Debug.LogError((object) $"Item is null or empty while evaluation expression: {this._exp}");
      return (object) 0.0f;
    }
    string asString = pars[0].EvaluateAsString(values);
    foreach (Item obj in this._items)
    {
      if (!(obj.id != asString))
        return (object) obj.GetRedSkullsValue();
    }
    Debug.LogError((object) $"Item with id {asString} not found in expression: {this._exp}");
    return (object) 0.0f;
  }

  [CompilerGenerated]
  public object \u003CCheckExpressionInit\u003Eb__16_64(
    IExpression[] pars,
    IDictionary<string, object> values)
  {
    if (this._items.IsNullOrEmpty<Item>())
    {
      Debug.LogError((object) $"Item is null or empty while evaluation expression: {this._exp}");
      return (object) 0.0f;
    }
    string asString = pars[0].EvaluateAsString(values);
    foreach (Item obj in this._items)
    {
      if (!(obj.id != asString))
        return (object) obj.GetWhiteSkullsValue();
    }
    Debug.LogError((object) $"Item with id {asString} not found in expression: {this._exp}");
    return (object) 0.0f;
  }
}
