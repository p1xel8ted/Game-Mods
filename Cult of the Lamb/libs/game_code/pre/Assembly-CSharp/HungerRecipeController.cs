// Decompiled with JetBrains decompiler
// Type: HungerRecipeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HungerRecipeController : MonoBehaviour
{
  public BarController BarController;
  private float HungerDelta;

  private void Start() => this.AddRecipe(InventoryItem.ITEM_TYPE.NONE);

  public void AddRecipe(InventoryItem.ITEM_TYPE recipe) => this.UpdateDelta(this.GetDelta(recipe));

  public void RemoveRecipe(InventoryItem.ITEM_TYPE recipe)
  {
    this.UpdateDelta(-this.GetDelta(recipe));
  }

  private void UpdateDelta(float modification)
  {
    this.HungerDelta += modification;
    this.BarController.SetBarSizeForInfo((HungerBar.Count + this.HungerDelta) / HungerBar.MAX_HUNGER, HungerBar.HungerNormalized, FollowerBrainStats.Fasting);
  }

  public float GetDelta(InventoryItem.ITEM_TYPE recipe)
  {
    return ((float) CookingData.GetSatationAmount(recipe) + HungerBar.ReservedSatiation) / FollowerManager.GetTotalNonLockedFollowers();
  }
}
