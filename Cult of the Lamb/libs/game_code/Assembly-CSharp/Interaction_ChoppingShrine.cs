// Decompiled with JetBrains decompiler
// Type: Interaction_ChoppingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public class Interaction_ChoppingShrine : Interaction
{
  [CompilerGenerated]
  public Structure \u003CStructure\u003Ek__BackingField;
  public Structures_ChoppingShrine _StructureInfo;

  public Structure Structure
  {
    get => this.\u003CStructure\u003Ek__BackingField;
    set => this.\u003CStructure\u003Ek__BackingField = value;
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_ChoppingShrine structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_ChoppingShrine;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }
}
