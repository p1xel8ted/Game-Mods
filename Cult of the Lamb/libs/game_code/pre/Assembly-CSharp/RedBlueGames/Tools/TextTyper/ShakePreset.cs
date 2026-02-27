// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.ShakePreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
