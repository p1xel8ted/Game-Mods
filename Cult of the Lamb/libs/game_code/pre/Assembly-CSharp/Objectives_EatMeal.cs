// Decompiled with JetBrains decompiler
// Type: Objectives_EatMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_EatMeal : ObjectivesData
{
  public StructureBrain.TYPES MealType;
  public int TargetFollower;
  private bool complete;

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.EatMeal, (object) FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name, (object) CookingData.GetLocalizedName(CookingData.GetMealFromStructureType(this.MealType)));
    }
  }

  public Objectives_EatMeal()
  {
  }

  public Objectives_EatMeal(
    string groupId,
    StructureBrain.TYPES mealType,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.EAT_MEAL;
    this.MealType = mealType;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_EatMeal.FinalizedData_EatMeal finalizedData = new Objectives_EatMeal.FinalizedData_EatMeal();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.MealType = this.MealType;
    finalizedData.TargetFollowerName = FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete()
  {
    base.CheckComplete();
    return this.complete;
  }

  public override void Update()
  {
    base.Update();
    if (this.IsFailed || this.TargetFollower == -1 || FollowerInfo.GetInfoByID(this.TargetFollower) != null)
      return;
    this.Failed();
  }

  public void CheckComplete(StructureBrain.TYPES mealType, int targetFollowerID_1)
  {
    if (mealType == this.MealType && this.TargetFollower == targetFollowerID_1)
      this.complete = true;
    else if (FollowerInfo.GetInfoByID(this.TargetFollower) == null)
      this.Failed();
    this.CheckComplete();
  }

  [Serializable]
  public class FinalizedData_EatMeal : ObjectivesDataFinalized
  {
    public StructureBrain.TYPES MealType;
    public string TargetFollowerName;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.EatMeal, (object) this.TargetFollowerName, (object) CookingData.GetLocalizedName(CookingData.GetMealFromStructureType(this.MealType)));
    }
  }
}
