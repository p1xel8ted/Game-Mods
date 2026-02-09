// Decompiled with JetBrains decompiler
// Type: CraftNeedsBubbleGUIOLD
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CraftNeedsBubbleGUIOLD : TooltipBubbleGUI
{
  public static CraftDefinition last_definition;
  public static CraftNeedsBubbleGUIOLD current;
  [HideInInspector]
  public UITable content_table;
  [HideInInspector]
  public List<BaseItemCellGUI> ingredients;
  public BaseItemCellGUI single_ingredient;
  public UILabel time_label;
  public UILabel output_item_label;
  public UILabel description;
  public UILabel ingredients_label;

  public override void Init()
  {
    this.ingredients = ((IEnumerable<BaseItemCellGUI>) this.GetComponentsInChildren<BaseItemCellGUI>(true)).ToList<BaseItemCellGUI>();
    this.ingredients.Remove(this.single_ingredient);
    this.content_table = this.GetComponentInChildren<UITable>(true);
    base.Init();
  }

  public void ShowDefinition(CraftDefinition craft_definition)
  {
    CraftNeedsBubbleGUIOLD.last_definition = craft_definition;
    MultiInventory inventoryForInteraction = MainGame.me.player.GetMultiInventoryForInteraction();
    List<Item> needs = craft_definition.needs;
    if (needs.Count == 1 && needs[0].definition.is_big)
    {
      this.single_ingredient.Activate<BaseItemCellGUI>();
      this.single_ingredient.DrawIngredient(needs[0], inventoryForInteraction);
      foreach (BaseItemCellGUI ingredient in this.ingredients)
        ingredient.DrawIngredient((Item) null, inventoryForInteraction);
    }
    else
    {
      this.single_ingredient.Deactivate<BaseItemCellGUI>();
      for (int index = 0; index < 4; ++index)
      {
        Item obj = index < needs.Count ? needs[index] : (Item) null;
        BaseItemCellGUI ingredient = this.ingredients[index];
        if ((Object) ingredient == (Object) null)
          Debug.LogError((object) ("CraftIngredientGUI is null, i = " + index.ToString()), (Object) this);
        else
          ingredient.DrawIngredient(obj, inventoryForInteraction);
      }
    }
    this.ingredients_label.SetActive(needs.Count > 0);
    string lng_id = craft_definition.id;
    if (lng_id.Contains("p:"))
    {
      while (lng_id.Contains(":"))
        lng_id = lng_id.Substring(lng_id.IndexOf(":") + 1);
    }
    this.output_item_label.text = GJL.L(lng_id);
    this.ingredients_label.text = GJL.L("ingredients") + GJL.L(":");
    if (craft_definition.craft_time.EvaluateFloat().EqualsTo(0.0f))
      this.time_label.Deactivate<UILabel>();
    else
      this.time_label.text = $"{GJL.L("craft time")}{GJL.L(":")} {craft_definition.craft_time?.ToString()}";
    this.description.Deactivate<UILabel>();
    this.gameObject.Activate();
    this.content_table.Reposition();
    this.content_table.repositionNow = true;
    this.try_show_down = true;
    this.OnContentChanged();
    this.widget = this.GetComponent<UIWidget>();
    this.Update();
    if (this.for_gamepad)
      this.widget.alpha = 0.0f;
    GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
    {
      foreach (BaseItemCellGUI ingredient in this.ingredients)
        this.ReactivateIfNecessary(ingredient);
      this.ReactivateIfNecessary(this.single_ingredient);
    }));
  }

  public void ReactivateIfNecessary(BaseItemCellGUI ingredient)
  {
    if ((Object) ingredient == (Object) null || !ingredient.isActiveAndEnabled)
      return;
    ingredient.Deactivate<BaseItemCellGUI>();
    ingredient.Activate<BaseItemCellGUI>();
  }

  public override void DestroyBubble()
  {
    if ((Object) CraftNeedsBubbleGUIOLD.current == (Object) this)
      CraftNeedsBubbleGUIOLD.current = (CraftNeedsBubbleGUIOLD) null;
    Object.Destroy((Object) this.gameObject);
  }

  public static TooltipBubbleGUI ShowMessage(
    CraftDefinition craft_definition,
    bool for_gamepad,
    Collider2D tooltip_collider)
  {
    return (TooltipBubbleGUI) null;
  }

  [CompilerGenerated]
  public void \u003CShowDefinition\u003Eb__10_0()
  {
    foreach (BaseItemCellGUI ingredient in this.ingredients)
      this.ReactivateIfNecessary(ingredient);
    this.ReactivateIfNecessary(this.single_ingredient);
  }
}
