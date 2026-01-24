// Decompiled with JetBrains decompiler
// Type: DrinkRecipeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UI.InfoCards;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DrinkRecipeInfoCard : RecipeInfoCard
{
  [SerializeField]
  public Image bar;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    base.Configure(config);
    this._hungerDelta.text = "<size=12><sprite name=\"icon_FaithUp\"></size><br>" + FollowerBrain.GetPleasureAmount(CookingData.GetPleasure(config)).ToString();
    this.bar.fillAmount = (float) FollowerBrain.GetPleasureAmount(CookingData.GetPleasure(config)) / 65f;
  }
}
