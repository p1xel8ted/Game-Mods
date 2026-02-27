// Decompiled with JetBrains decompiler
// Type: Structures_IceBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
