// Decompiled with JetBrains decompiler
// Type: HPActionComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class HPActionComponent : WorldGameObjectComponent
{
  public bool _hp_was_positive;

  public override void StartComponent()
  {
    base.StartComponent();
    this.InitComponent();
  }

  public override bool DoAction(
    WorldGameObject player_wgo,
    float delta_time,
    bool for_gratitude_points = false)
  {
    Item equippedTool = player_wgo.GetEquippedTool();
    float k;
    if (equippedTool == null || !this.wgo.obj_def.tool_actions.GetToolK(equippedTool.definition.type, out k))
      return false;
    k *= equippedTool.definition.efficiency;
    CraftComponent craft = this.components.craft;
    if (craft.enabled && craft.is_crafting && !craft.current_craft.is_auto || !this.CanSpendPlayerEnergy(player_wgo, delta_time, true))
      return false;
    this.wgo.OnWorkAction();
    if (equippedTool.definition.durability_decrease_on_use)
    {
      float num = equippedTool.definition.durability_decrease_on_use_speed * delta_time;
      Debug.Log((object) ("Dur dec = " + num.ToString()));
      equippedTool.durability -= num;
      if (equippedTool.durability_state == Item.DurabilityState.Broken)
      {
        MainGame.me.OnEquippedToolBroken(equippedTool);
        MainGame.me.player.components.interaction.UpdateNearestHint();
        MainGame.me.player.components.tool.TryStop();
        GUIElements.me.dialog.OpenOK(GJL.L("tool_broken_txt", equippedTool.GetItemName()));
        Sounds.PlaySound("break");
        return false;
      }
    }
    this.wgo.hp -= k * delta_time;
    return (double) this.wgo.hp <= 0.0 && this._hp_was_positive;
  }

  public bool CanSpendPlayerEnergy(
    WorldGameObject player_wgo,
    float delta_time,
    bool really_spend_energy = false)
  {
    Item equippedTool = player_wgo.GetEquippedTool();
    if (equippedTool == null)
      return false;
    GameRes gameRes = new GameRes(equippedTool.definition.params_on_use);
    if (gameRes.IsEmpty())
      return true;
    float num = gameRes.Get("energy");
    if ((double) num >= 0.0)
      return true;
    if (equippedTool.definition.type == ItemDefinition.ItemType.Hand)
      num *= delta_time;
    if (equippedTool.definition.tool_energy_k != null && equippedTool.definition.tool_energy_k.has_expression)
      num *= equippedTool.definition.tool_energy_k.EvaluateFloat(this.wgo, player_wgo);
    if ((double) player_wgo.energy < -(double) num)
    {
      Debug.Log((object) $"player energy = {player_wgo.energy.ToString()}, need = {num.ToString()}");
      return false;
    }
    if (really_spend_energy)
    {
      if (!player_wgo.components.character.player.TrySpendEnergy(-num))
      {
        Debug.LogError((object) "Impossable error. Energy check was OK, but TrySpendEnergy returned false");
        return false;
      }
      gameRes.Set("energy", 0.0f);
      if (!gameRes.IsEmpty())
      {
        player_wgo.AddToParams(gameRes);
        EffectBubblesManager.ShowStacked(player_wgo, gameRes);
      }
    }
    return true;
  }

  public void DecHP(float value)
  {
    if (!this.wgo.is_player)
      value *= this.wgo.obj_def.damage_factor;
    if ((double) value > 0.0)
    {
      if (this.wgo.is_player)
      {
        Item equippedItem1 = this.wgo.GetEquippedItem(ItemDefinition.EquipmentType.HeadArmor);
        if (equippedItem1 != null)
          value -= equippedItem1.definition.armor;
        Item equippedItem2 = this.wgo.GetEquippedItem(ItemDefinition.EquipmentType.BodyArmor);
        if (equippedItem2 != null)
          value -= equippedItem2.definition.armor;
        value -= this.wgo.GetParam("add_armor");
      }
      else
        value -= this.wgo.obj_def.armor;
    }
    if ((double) value < 0.0)
      value = 0.0f;
    if (this.wgo.is_player && this.wgo.IsPlayerInvulnerable())
      value = 0.0f;
    this.wgo.hp -= value;
    if ((double) value > 0.0)
      EffectBubblesManager.ShowStackedHP(this.wgo, -value);
    if (!this.wgo.obj_def.IsCharacter() || (double) this.wgo.hp <= 0.0)
      return;
    EnemiesHPBarsManager.me.AddIfNeeded(this.wgo);
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    if ((double) this.wgo.hp > 0.0)
    {
      this._hp_was_positive = true;
    }
    else
    {
      if (!this._hp_was_positive)
        return;
      this.wgo.hp = 0.0f;
      this.wgo.DoPreZeroHPActivity();
      this._hp_was_positive = false;
    }
  }

  public bool HasHPInDefinition()
  {
    try
    {
      return (double) this.wgo.obj_def.hp.EvaluateFloat(this.wgo) > 0.0;
    }
    catch (NullReferenceException ex)
    {
      Debug.LogError((object) ("Probably obj_def is null for object id = " + this.wgo.obj_id), (UnityEngine.Object) this.wgo);
      return false;
    }
  }

  public float GetMaxHP() => this.wgo.obj_def.hp.EvaluateFloat(this.wgo);

  public float GetHPProgress()
  {
    float b = this.wgo.hp;
    float maxHp = this.GetMaxHP();
    if ((double) b < 0.0)
      b = 0.0f;
    return !maxHp.EqualsTo(b) ? (maxHp - b) / maxHp : -1f;
  }

  public override void InitComponent()
  {
    if ((UnityEngine.Object) this.wgo == (UnityEngine.Object) null || this.wgo.obj_def == null)
      return;
    if (!this.wgo.is_player && this.wgo.GetParamInt("hp_inited") == 0)
    {
      this.wgo.hp = this.wgo.obj_def.hp.EvaluateFloat(this.wgo);
      for (int i = 0; i < 10; ++i)
      {
        if ((double) Mathf.Abs(this.wgo.obj_def.Damage(i)) >= 0.0099999997764825821)
          this.wgo.SetParam("damage" + (i == 0 ? "" : "_" + i.ToString()), this.wgo.obj_def.Damage(i));
      }
      if (Application.isPlaying)
        this.wgo.SetParam("hp_inited", 1f);
    }
    this._hp_was_positive = (double) this.wgo.hp > 0.0;
  }

  public override int GetExecutionOrder() => 11;

  public override void RefreshComponentBubbleData(bool show_interaction_buttons)
  {
    if (this.components.craft.enabled && (this.components.craft.is_crafting && (this.components.craft.current_craft == null || !this.components.craft.current_craft.hidden) || !this.components.craft.IsCraftQueueEmpty()))
      return;
    BubbleWidgetProgressData wdata = (BubbleWidgetProgressData) null;
    if (this.HasHPInDefinition() & show_interaction_buttons)
      wdata = new BubbleWidgetProgressData(new BubbleWidgetProgressData.ProgressDelegate(this.components.hp.GetHPProgress));
    this.wgo.SetBubbleWidgetData((BubbleWidgetData) wdata, BubbleWidgetData.WidgetID.HPProgress);
    if (!show_interaction_buttons || this.wgo.obj_def.tool_actions.no_actions || !this.wgo.CanProcessWork())
      return;
    string text1 = "";
    ItemDefinition.ItemType actionTool = this.wgo.obj_def.tool_actions.action_tools[0];
    if (actionTool != ItemDefinition.ItemType.Sword)
    {
      Item equippedTool = MainGame.me.player.GetEquippedTool(actionTool);
      text1 = $"({actionTool.ToString().ToLower()})";
      if (MainGame.me.save.IsWorkAvailible(this.wgo.obj_def))
      {
        if (equippedTool == null)
          text1 = "(not_equipped)" + text1;
      }
      else
        text1 = "(tech_locked)";
    }
    if (string.IsNullOrEmpty(text1))
      return;
    string text2 = GameKeyTip.Get(GameKey.Work, text1);
    MainGame.me.player.components.character.wgo_hilighted_for_work = this.wgo;
    this.wgo.SetBubbleWidgetData(text2, BubbleWidgetData.WidgetID.Work);
  }
}
