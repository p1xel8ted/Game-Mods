// Decompiled with JetBrains decompiler
// Type: Structures_HarvestTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_HarvestTotem : StructureBrain
{
  public override int SoulMax => this.Data.Type == StructureBrain.TYPES.HARVEST_TOTEM ? 3 : 15;

  public float TimeBetweenSouls => 1200f;
}
