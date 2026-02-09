// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.CurveLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
