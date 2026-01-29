// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Weather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Shrine_Weather : StructureBrain
{
  public bool IsRewardReady() => this.Data.Inventory[0].quantity >= 100;

  public void Clear() => this.Data.Inventory.Clear();
}
