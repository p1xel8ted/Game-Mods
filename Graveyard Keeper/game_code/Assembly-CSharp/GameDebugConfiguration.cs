// Decompiled with JetBrains decompiler
// Type: GameDebugConfiguration
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GameDebugConfiguration : MonoBehaviour
{
  public string scene_to_load = "";
  public bool multiplayer;
  public string[] _scenes_values = new string[2]
  {
    "scene_graveyard",
    "scene_prefabs_list"
  };
}
