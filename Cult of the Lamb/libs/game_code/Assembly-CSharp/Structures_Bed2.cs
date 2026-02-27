// Decompiled with JetBrains decompiler
// Type: Structures_Bed2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Bed2 : Structures_Bed
{
  public override int Level => 2;

  public override float ChanceToCollapse
  {
    get => WeatherSystemController.Instance.IsRaining ? 0.05f : 0.025f;
  }

  public override StructureBrain.TYPES CollapsedType => StructureBrain.TYPES.BED_2_COLLAPSED;
}
