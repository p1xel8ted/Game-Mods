// Decompiled with JetBrains decompiler
// Type: Objectives_CookMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_CookMeal : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public InventoryItem.ITEM_TYPE MealType;
  [Key(17)]
  public int Count;
  [Key(18)]
  public int StartingAmount = -1;

  [IgnoreMember]
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

  public override bool CheckComplete()
  {
    return CookingData.GetCookedMeal(this.MealType) - this.StartingAmount >= this.Count;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_CookMeal : ObjectivesDataFinalized
  {
    [Key(3)]
    public InventoryItem.ITEM_TYPE MealType;
    [Key(4)]
    public int Target;
    [Key(5)]
    public int Count;

    public override string GetText()
    {
      string cookMeals = ScriptLocalization.Objectives.CookMeals;
      string str1 = LocalizeIntegration.ReverseText(this.Count.ToString());
      string str2 = LocalizeIntegration.ReverseText(this.Target.ToString());
      return this.MealType == InventoryItem.ITEM_TYPE.NONE ? string.Format(cookMeals, (object) ScriptLocalization.Interactions.Meals, (object) str1, (object) str2) : string.Format(cookMeals, (object) CookingData.GetLocalizedName(this.MealType), (object) str1, (object) str2);
    }
  }
}
