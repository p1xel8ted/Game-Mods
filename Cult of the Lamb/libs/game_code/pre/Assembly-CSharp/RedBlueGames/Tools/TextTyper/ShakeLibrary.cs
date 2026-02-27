// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.ShakeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private ShakePreset FindPresetOrNull(string key)
  {
    foreach (ShakePreset shakePreset in this.ShakePresets)
    {
      if (shakePreset.Name.ToUpper() == key.ToUpper())
        return shakePreset;
    }
    return (ShakePreset) null;
  }
}
