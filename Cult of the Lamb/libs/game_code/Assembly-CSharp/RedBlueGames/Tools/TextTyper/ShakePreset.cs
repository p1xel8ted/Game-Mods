// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.ShakePreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[Serializable]
public class ShakePreset
{
  [Tooltip("Name identifying this preset. Can also be used as a ShakeLibrary indexer key.")]
  public string Name;
  [Range(0.0f, 20f)]
  [Tooltip("Amount of x-axis shake to apply during animation")]
  public float xPosStrength;
  [Range(0.0f, 20f)]
  [Tooltip("Amount of y-axis shake to apply during animation")]
  public float yPosStrength;
  [Range(0.0f, 90f)]
  [Tooltip("Amount of rotational shake to apply during animation")]
  public float RotationStrength;
  [Range(0.0f, 10f)]
  [Tooltip("Amount of scale shake to apply during animation")]
  public float ScaleStrength;
}
