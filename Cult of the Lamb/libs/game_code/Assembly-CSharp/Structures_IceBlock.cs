// Decompiled with JetBrains decompiler
// Type: Structures_IceBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Structures_IceBlock : StructureBrain
{
  public override void Init(StructuresData data)
  {
    base.Init(data);
    this.OnSeasonChanged(SeasonsManager.CurrentSeason);
  }

  public override void OnSeasonChanged(SeasonsManager.Season season)
  {
    base.OnSeasonChanged(season);
    if (season == SeasonsManager.Season.Winter)
      return;
    for (int index = Structure.Structures.Count - 1; index >= 0; --index)
    {
      if (Structure.Structures[index].Brain != null && Structure.Structures[index].Brain.Data.ID == this.Data.ID)
        Object.Destroy((Object) Structure.Structures[index].gameObject);
    }
    this.Remove();
  }
}
