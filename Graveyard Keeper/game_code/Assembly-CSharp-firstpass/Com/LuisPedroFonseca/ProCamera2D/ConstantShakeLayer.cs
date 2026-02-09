// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ConstantShakeLayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[Serializable]
public struct ConstantShakeLayer
{
  [MinMaxSlider(0.001f, 10f)]
  public Vector2 Frequency;
  [Range(0.0f, 100f)]
  public float AmplitudeHorizontal;
  [Range(0.0f, 100f)]
  public float AmplitudeVertical;
  [Range(0.0f, 100f)]
  public float AmplitudeDepth;
}
