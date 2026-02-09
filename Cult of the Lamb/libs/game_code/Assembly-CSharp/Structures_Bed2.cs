// Decompiled with JetBrains decompiler
// Type: Structures_Bed2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
