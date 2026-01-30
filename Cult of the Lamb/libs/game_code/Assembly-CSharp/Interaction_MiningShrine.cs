// Decompiled with JetBrains decompiler
// Type: Interaction_MiningShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public class Interaction_MiningShrine : Interaction
{
  [CompilerGenerated]
  public Structure \u003CStructure\u003Ek__BackingField;
  public Structures_MiningShrine _StructureInfo;

  public Structure Structure
  {
    get => this.\u003CStructure\u003Ek__BackingField;
    set => this.\u003CStructure\u003Ek__BackingField = value;
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_MiningShrine structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_MiningShrine;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }
}
