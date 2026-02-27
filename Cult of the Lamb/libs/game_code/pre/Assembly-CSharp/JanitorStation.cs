// Decompiled with JetBrains decompiler
// Type: JanitorStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class JanitorStation : BaseMonoBehaviour
{
  public static List<JanitorStation> JanitorStations = new List<JanitorStation>();
  public Structure Structure;
  private Structures_JanitorStation _StructureInfo;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_JanitorStation StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_JanitorStation;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void OnEnable() => JanitorStation.JanitorStations.Add(this);

  private void OnDisable() => JanitorStation.JanitorStations.Remove(this);
}
