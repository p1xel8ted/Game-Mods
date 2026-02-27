// Decompiled with JetBrains decompiler
// Type: Objectives_CookMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_CookMeal : ObjectivesData
{
  public InventoryItem.ITEM_TYPE MealType;
  public int Count;
  public int StartingAmount = -1;

  public override string Text
  {
    get
    {
      int num = CookingData.GetCookedMeal(this.MealType) - this.StartingAmount;
      string cookMeals = ScriptLocalization.Objectives.CookMeals;
      return this.MealType == InventoryItem.ITEM_TYPE.NONE ? string.Format(cookMeals, (object) ScriptLocalization.Interactions.Meals, (object) num, (object) this.Count) : string.Format(cookMeals, (object) CookingData.GetLocalizedName(this.MealType), (object) num, (object) this.Count);
    }
  }

  public Objectives_CookMeal()
  {
  }

  public Objectives_CookMeal(
    string groupId,
    InventoryItem.ITEM_TYPE mealType,
    int count,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.COOK_MEALS;
    this.MealType = mealType;
    this.Count = count;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    if (initialAssigning)
      this.StartingAmount = CookingData.GetCookedMeal(this.MealType);
    ObjectiveManager.CheckObjectives(this.Type);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    int num = CookingData.GetCookedMeal(this.MealType) - this.StartingAmount;
    Objectives_CookMeal.FinalizedData_CookMeal finalizedData = new Objectives_CookMeal.FinalizedData_CookMeal();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.MealType = this.MealType;
    finalizedData.Count = num;
    finalizedData.Target = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete()
  {
    return CookingData.GetCookedMeal(this.MealType) - this.StartingAmount >= this.Count;
  }

  [Serializable]
  public class FinalizedData_CookMeal : ObjectivesDataFinalized
  {
    public InventoryItem.ITEM_TYPE MealType;
    public int Target;
    public int Count;

    public override string GetText()
    {
      string cookMeals = ScriptLocalization.Objectives.CookMeals;
      return this.MealType == InventoryItem.ITEM_TYPE.NONE ? string.Format(cookMeals, (object) ScriptLocalization.Interactions.Meals, (object) this.Count, (object) this.Target) : string.Format(cookMeals, (object) CookingData.GetLocalizedName(this.MealType), (object) this.Count, (object) this.Target);
    }
  }
}
