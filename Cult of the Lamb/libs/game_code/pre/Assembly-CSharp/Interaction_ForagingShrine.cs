// Decompiled with JetBrains decompiler
// Type: Interaction_ForagingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Interaction_ForagingShrine : Interaction
{
  private Structures_ForagingShrine _StructureInfo;

  public Structure Structure { get; private set; }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_ForagingShrine structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_ForagingShrine;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }
}
