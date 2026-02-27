// Decompiled with JetBrains decompiler
// Type: src.Alerts.RecipeAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
