// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.CurveLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[CreateAssetMenu(fileName = "CurveLibrary", menuName = "Text Typer/Curve Library", order = 1)]
public class CurveLibrary : ScriptableObject
{
  public List<CurvePreset> CurvePresets;

  public CurvePreset this[string key]
  {
    get => this.FindPresetOrNull(key) ?? throw new KeyNotFoundException();
  }

  public bool ContainsKey(string key) => this.FindPresetOrNull(key) != null;

  public CurvePreset FindPresetOrNull(string key)
  {
    foreach (CurvePreset curvePreset in this.CurvePresets)
    {
      if (curvePreset.Name.ToUpper() == key.ToUpper())
        return curvePreset;
    }
    return (CurvePreset) null;
  }
}
