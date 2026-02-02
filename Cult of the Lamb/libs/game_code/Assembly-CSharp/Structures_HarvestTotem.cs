// Decompiled with JetBrains decompiler
// Type: Structures_HarvestTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_HarvestTotem : StructureBrain
{
  public override int SoulMax => this.Data.Type == StructureBrain.TYPES.HARVEST_TOTEM ? 3 : 15;

  public float TimeBetweenSouls => 1200f;
}
