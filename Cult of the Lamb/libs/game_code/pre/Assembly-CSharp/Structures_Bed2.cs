// Decompiled with JetBrains decompiler
// Type: Structures_Bed2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_Bed2 : Structures_Bed
{
  public override int Level => 2;

  public override float ChanceToCollapse => WeatherController.isRaining ? 0.05f : 0.025f;

  public override StructureBrain.TYPES CollapsedType => StructureBrain.TYPES.BED_2_COLLAPSED;
}
