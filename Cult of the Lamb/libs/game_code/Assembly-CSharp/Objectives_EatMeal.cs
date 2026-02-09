// Decompiled with JetBrains decompiler
// Type: Objectives_EatMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_EatMeal : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public StructureBrain.TYPES MealType;
  [Key(17)]
  public int TargetFollower;
  [IgnoreMember]
  public bool complete;

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

  public override bool CheckComplete()
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

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_EatMeal : ObjectivesDataFinalized
  {
    [Key(3)]
    public StructureBrain.TYPES MealType;
    [Key(4)]
    public string TargetFollowerName;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.EatMeal, (object) this.TargetFollowerName, (object) CookingData.GetLocalizedName(CookingData.GetMealFromStructureType(this.MealType)));
    }
  }
}
