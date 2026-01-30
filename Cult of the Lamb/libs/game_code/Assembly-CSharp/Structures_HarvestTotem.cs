// Decompiled with JetBrains decompiler
// Type: Structures_HarvestTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_HarvestTotem : StructureBrain
{
  public override int SoulMax => this.Data.Type == StructureBrain.TYPES.HARVEST_TOTEM ? 3 : 15;

  public float TimeBetweenSouls => 1200f;
}
