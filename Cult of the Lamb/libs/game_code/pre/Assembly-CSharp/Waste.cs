// Decompiled with JetBrains decompiler
// Type: Waste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Waste : BaseMonoBehaviour
{
  public static List<Waste> Wastes = new List<Waste>();
  public Structure structure;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public StructureBrain StructureBrain => this.structure.Brain;

  private void OnEnable() => Waste.Wastes.Add(this);

  private void OnDisable() => Waste.Wastes.Remove(this);
}
