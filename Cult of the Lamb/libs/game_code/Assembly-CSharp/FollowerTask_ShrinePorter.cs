// Decompiled with JetBrains decompiler
// Type: FollowerTask_ShrinePorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerTask_ShrinePorter : FollowerTask_FuelPorter
{
  public FollowerTask_ShrinePorter(int logisticsStructure, int rootStructure, int targetStructure)
    : base(logisticsStructure, rootStructure, targetStructure)
  {
    this.fuel = InventoryItem.ITEM_TYPE.LOG;
  }
}
