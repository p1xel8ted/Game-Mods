// Decompiled with JetBrains decompiler
// Type: DropResCurve
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class DropResCurve
{
  public AnimationCurve curve;
  [Range(0.0f, 1f)]
  public float duration_factor;
}
