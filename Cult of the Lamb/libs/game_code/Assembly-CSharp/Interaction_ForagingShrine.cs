// Decompiled with JetBrains decompiler
// Type: Interaction_ForagingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public class Interaction_ForagingShrine : Interaction
{
  [CompilerGenerated]
  public Structure \u003CStructure\u003Ek__BackingField;
  public Structures_ForagingShrine _StructureInfo;

  public Structure Structure
  {
    get => this.\u003CStructure\u003Ek__BackingField;
    set => this.\u003CStructure\u003Ek__BackingField = value;
  }

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
