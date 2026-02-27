// Decompiled with JetBrains decompiler
// Type: src.Alerts.RecipeAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace src.Alerts;

public class RecipeAlerts : AlertCategory<InventoryItem.ITEM_TYPE>
{
  public RecipeAlerts()
  {
    CookingData.OnRecipeDiscovered += new Action<InventoryItem.ITEM_TYPE>(this.OnRecipeDiscovered);
  }

  ~RecipeAlerts()
  {
    CookingData.OnRecipeDiscovered -= new Action<InventoryItem.ITEM_TYPE>(this.OnRecipeDiscovered);
  }

  private void OnRecipeDiscovered(InventoryItem.ITEM_TYPE recipe) => this.AddOnce(recipe);
}
