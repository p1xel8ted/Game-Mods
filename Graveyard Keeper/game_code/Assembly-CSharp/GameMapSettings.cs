// Decompiled with JetBrains decompiler
// Type: GameMapSettings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[CreateAssetMenu]
[Serializable]
public class GameMapSettings : ScriptableObject
{
  public GameMapSettings.GameMapZoneDescription[] zones;

  public Color GetColorOfType(GameMapSettings.GameMapZoneType t)
  {
    foreach (GameMapSettings.GameMapZoneDescription zone in this.zones)
    {
      if (zone.zone_type == t)
        return zone.color;
    }
    return Color.cyan;
  }

  [Serializable]
  public enum GameMapZoneType
  {
    Ground = 0,
    Sand = 50, // 0x00000032
    Gravel = 100, // 0x00000064
    Water = 200, // 0x000000C8
    Rocks = 255, // 0x000000FF
  }

  [Serializable]
  public struct GameMapZoneDescription
  {
    public GameMapSettings.GameMapZoneType zone_type;
    public Color color;
  }
}
