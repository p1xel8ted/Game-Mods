// Decompiled with JetBrains decompiler
// Type: WeatherPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "WeatherPreset", menuName = "WeatherPreset", order = 1)]
public class WeatherPreset : ScriptableObject
{
  public List<WeatherPresetAtom> preset_atoms;

  public string preset_name => this.name;

  public static WeatherPreset GetPreset(string preset_name)
  {
    if (string.IsNullOrEmpty(preset_name))
    {
      Debug.LogError((object) "Preset name is empty!");
      return (WeatherPreset) null;
    }
    WeatherPreset preset = Resources.Load<WeatherPreset>("Weather/" + preset_name);
    if (!((Object) preset == (Object) null))
      return preset;
    Debug.LogError((object) $"Failed to load WeatherPreset Weather/{preset_name}.asset");
    return preset;
  }

  public override string ToString()
  {
    string str = this.preset_name + " = {";
    if (this.preset_atoms.Count > 0)
    {
      foreach (WeatherPresetAtom presetAtom in this.preset_atoms)
      {
        str += presetAtom.ToString();
        if (this.preset_atoms[this.preset_atoms.Count - 1] != presetAtom)
          str += ", ";
      }
    }
    else
      str += "NOTHING";
    return str + "}";
  }
}
