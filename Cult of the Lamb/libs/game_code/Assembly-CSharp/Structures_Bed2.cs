// Decompiled with JetBrains decompiler
// Type: Structures_Bed2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
