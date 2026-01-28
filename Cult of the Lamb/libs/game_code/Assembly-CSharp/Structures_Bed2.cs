// Decompiled with JetBrains decompiler
// Type: Structures_Bed2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
