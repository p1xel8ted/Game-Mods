// Decompiled with JetBrains decompiler
// Type: FollowerTask_PropagandaPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerTask_PropagandaPorter : FollowerTask_FuelPorter
{
  public FollowerTask_PropagandaPorter(
    int logisticsStructure,
    int rootStructure,
    int targetStructure)
    : base(logisticsStructure, rootStructure, targetStructure)
  {
    this.fuel = InventoryItem.ITEM_TYPE.GOLD_REFINED;
  }
}
