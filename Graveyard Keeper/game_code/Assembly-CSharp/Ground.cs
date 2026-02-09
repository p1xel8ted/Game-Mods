// Decompiled with JetBrains decompiler
// Type: Ground
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class Ground : MonoBehaviour
{
  public Ground.GroudType type;

  public enum GroudType
  {
    None = 0,
    Grass = 1,
    Dirt = 2,
    Sand = 3,
    Swamp = 4,
    Rocks = 5,
    Shit = 6,
    Rug = 100, // 0x00000064
    Wood = 101, // 0x00000065
  }
}
