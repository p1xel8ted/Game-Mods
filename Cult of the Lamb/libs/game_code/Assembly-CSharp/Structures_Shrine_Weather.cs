// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Weather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Shrine_Weather : StructureBrain
{
  public bool IsRewardReady() => this.Data.Inventory[0].quantity >= 100;

  public void Clear() => this.Data.Inventory.Clear();
}
