// Decompiled with JetBrains decompiler
// Type: src.Alerts.RecipeAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class RecipeAlerts : AlertCategory<InventoryItem.ITEM_TYPE>
{
  public RecipeAlerts()
  {
    CookingData.OnRecipeDiscovered += new Action<InventoryItem.ITEM_TYPE>(this.OnRecipeDiscovered);
  }

  void object.Finalize()
  {
    try
    {
      CookingData.OnRecipeDiscovered -= new Action<InventoryItem.ITEM_TYPE>(this.OnRecipeDiscovered);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnRecipeDiscovered(InventoryItem.ITEM_TYPE recipe) => this.AddOnce(recipe);
}
