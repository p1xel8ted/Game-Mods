// Decompiled with JetBrains decompiler
// Type: SaveSlotData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SaveSlotData
{
  [NonSerialized]
  public string filename_no_extension;
  [NonSerialized]
  public GameSave linked_save;
  public string stats;
  public string real_time;
  public float game_time;
  public float version;

  public static SaveSlotData FromJSON(string s) => JsonUtility.FromJson<SaveSlotData>(s);

  public string ToJSON() => JsonUtility.ToJson((object) this);

  public void PrepareForSave()
  {
    this.game_time = MainGame.game_time;
    this.real_time = DateTime.Now.ToString("HH:mm, dd MMM yyyy");
    float totalQuality1 = WorldZone.GetZoneByID("church").GetTotalQuality();
    float totalQuality2 = WorldZone.GetZoneByID("graveyard").GetTotalQuality();
    this.stats = $"(wskull){totalQuality2:0.#}  (cross){totalQuality1:0.#}";
    this.version = LazyConsts.VERSION;
    if ((double) this.game_time <= 1.5)
      return;
    PlatformSpecific.SetGameMetric(GameEvents.GameMetric.DaysInGame, this.game_time - 1.5f);
    PlatformSpecific.SetGameMetric(GameEvents.GameMetric.GraveyardQuality, totalQuality2);
    PlatformSpecific.SetGameMetric(GameEvents.GameMetric.ChurchQuality, totalQuality1);
  }

  public bool IsBinaryFormat() => true;
}
