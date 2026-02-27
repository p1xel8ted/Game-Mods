// Decompiled with JetBrains decompiler
// Type: Objectives_DepositFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_DepositFood : ObjectivesData
{
  public override string Text => ScriptLocalization.Objectives.CookMeals;

  public Objectives_DepositFood()
  {
  }

  public Objectives_DepositFood(string groupId)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.DEPOSIT_FOOD;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_DepositFood.FinalizedData_DepositFood finalizedData = new Objectives_DepositFood.FinalizedData_DepositFood();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete()
  {
    bool flag = false;
    Structure ofType1 = Structure.GetOfType(StructureBrain.TYPES.KITCHEN);
    if ((UnityEngine.Object) ofType1 != (UnityEngine.Object) null)
      return ofType1.Inventory.Count > 0;
    Structure ofType2 = Structure.GetOfType(StructureBrain.TYPES.KITCHEN_II);
    return (UnityEngine.Object) ofType2 != (UnityEngine.Object) null ? ofType2.Inventory.Count > 0 : flag;
  }

  [Serializable]
  public class FinalizedData_DepositFood : ObjectivesDataFinalized
  {
    public override string GetText() => ScriptLocalization.Objectives.CookMeals;
  }
}
