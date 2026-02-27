// Decompiled with JetBrains decompiler
// Type: TreeBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TreeBase : Interaction
{
  public static List<TreeBase> Trees = new List<TreeBase>();
  public Structure Structure;
  private Structures_Tree _StructureBrain;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Tree StructureBrain
  {
    get
    {
      if (this._StructureBrain == null && this.Structure.Brain != null)
        this._StructureBrain = this.Structure.Brain as Structures_Tree;
      return this._StructureBrain;
    }
    set => this._StructureBrain = value;
  }
}
