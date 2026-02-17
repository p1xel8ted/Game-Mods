// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.ShakeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[CreateAssetMenu(fileName = "ShakeLibrary", menuName = "Text Typer/Shake Library", order = 1)]
public class ShakeLibrary : ScriptableObject
{
  public List<ShakePreset> ShakePresets;

  public ShakePreset this[string key]
  {
    get => this.FindPresetOrNull(key) ?? throw new KeyNotFoundException();
  }

  public bool ContainsKey(string key) => this.FindPresetOrNull(key) != null;

  public ShakePreset FindPresetOrNull(string key)
  {
    foreach (ShakePreset shakePreset in this.ShakePresets)
    {
      if (shakePreset.Name.ToUpper() == key.ToUpper())
        return shakePreset;
    }
    return (ShakePreset) null;
  }
}
