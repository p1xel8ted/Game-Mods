// Decompiled with JetBrains decompiler
// Type: TreeBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TreeBase : Interaction
{
  public static List<TreeBase> Trees = new List<TreeBase>();
  public Structure Structure;
  public Structures_Tree _StructureBrain;

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
