// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ConstantShakePreset
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[CreateAssetMenu(menuName = "ProCamera2D/Constant Shake Preset")]
[Serializable]
public class ConstantShakePreset : ScriptableObject
{
  public float Intensity = 0.3f;
  public List<ConstantShakeLayer> Layers;
}
