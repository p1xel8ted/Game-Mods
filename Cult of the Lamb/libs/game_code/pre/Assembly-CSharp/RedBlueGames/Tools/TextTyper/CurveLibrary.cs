// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.CurveLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private CurvePreset FindPresetOrNull(string key)
  {
    foreach (CurvePreset curvePreset in this.CurvePresets)
    {
      if (curvePreset.Name.ToUpper() == key.ToUpper())
        return curvePreset;
    }
    return (CurvePreset) null;
  }
}
