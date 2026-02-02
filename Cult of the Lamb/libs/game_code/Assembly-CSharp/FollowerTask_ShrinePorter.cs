// Decompiled with JetBrains decompiler
// Type: FollowerTask_ShrinePorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
