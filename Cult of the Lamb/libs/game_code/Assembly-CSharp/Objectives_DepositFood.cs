// Decompiled with JetBrains decompiler
// Type: Objectives_DepositFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
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

  public override bool CheckComplete()
  {
    bool flag = false;
    Structure ofType1 = Structure.GetOfType(StructureBrain.TYPES.KITCHEN);
    if ((UnityEngine.Object) ofType1 != (UnityEngine.Object) null)
      return ofType1.Inventory.Count > 0;
    Structure ofType2 = Structure.GetOfType(StructureBrain.TYPES.KITCHEN_II);
    return (UnityEngine.Object) ofType2 != (UnityEngine.Object) null ? ofType2.Inventory.Count > 0 : flag;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_DepositFood : ObjectivesDataFinalized
  {
    public override string GetText() => ScriptLocalization.Objectives.CookMeals;
  }
}
