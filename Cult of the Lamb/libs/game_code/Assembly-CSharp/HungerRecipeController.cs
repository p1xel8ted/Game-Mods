// Decompiled with JetBrains decompiler
// Type: HungerRecipeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HungerRecipeController : MonoBehaviour
{
  public BarController BarController;
  public BarController PlayerHungerBarController;
  public float HungerDelta;
  public float HungerDeltaPlayer;

  public void Start()
  {
    this.AddRecipe(InventoryItem.ITEM_TYPE.NONE);
    if (!((Object) this.PlayerHungerBarController != (Object) null))
      return;
    this.PlayerHungerBarController.gameObject.SetActive(DataManager.Instance.SurvivalModeActive);
  }

  public void AddRecipe(InventoryItem.ITEM_TYPE recipe)
  {
    this.UpdateDelta(this.GetDelta(recipe));
    this.UpdateDeltaPlayer(this.GetDeltaPlayer(recipe));
  }

  public void RemoveRecipe(InventoryItem.ITEM_TYPE recipe)
  {
    this.UpdateDelta(-this.GetDelta(recipe));
    this.UpdateDeltaPlayer(-this.GetDeltaPlayer(recipe));
  }

  public void UpdateDelta(float modification)
  {
    this.HungerDelta += modification;
    this.BarController.SetBarSizeForInfo((HungerBar.Count + this.HungerDelta) / HungerBar.MAX_HUNGER, HungerBar.HungerNormalized, FollowerBrainStats.Fasting);
  }

  public float GetDelta(InventoryItem.ITEM_TYPE recipe)
  {
    float nonLockedFollowers = FollowerManager.GetTotalNonLockedFollowers();
    return (double) nonLockedFollowers <= 0.0 ? 0.0f : ((float) CookingData.GetSatationAmount(recipe) + HungerBar.ReservedSatiation) / nonLockedFollowers;
  }

  public void UpdateDeltaPlayer(float modification)
  {
    this.HungerDeltaPlayer += modification;
    if (!((Object) this.PlayerHungerBarController != (Object) null))
      return;
    this.PlayerHungerBarController.SetBarSizeForInfo(DataManager.Instance.SurvivalMode_Hunger / 100f + this.HungerDeltaPlayer, DataManager.Instance.SurvivalMode_Hunger / 100f, false);
  }

  public float GetDeltaPlayer(InventoryItem.ITEM_TYPE recipe)
  {
    return (float) CookingData.GetSatationAmountPlayer(recipe) / 100f;
  }
}
