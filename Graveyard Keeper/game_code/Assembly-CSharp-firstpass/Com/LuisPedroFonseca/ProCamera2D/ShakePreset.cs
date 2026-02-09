// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ShakePreset
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[CreateAssetMenu(menuName = "ProCamera2D/Shake Preset")]
[Serializable]
public class ShakePreset : ScriptableObject
{
  public Vector3 Strength = (Vector3) new Vector2(10f, 10f);
  [Range(0.02f, 4f)]
  public float Duration = 0.5f;
  [Range(1f, 100f)]
  public int Vibrato = 10;
  [Range(0.0f, 1f)]
  public float Randomness = 0.1f;
  [Range(0.0f, 0.5f)]
  public float Smoothness = 0.1f;
  public bool UseRandomInitialAngle = true;
  [Range(0.0f, 360f)]
  public float InitialAngle;
  public Vector3 Rotation;
  public bool IgnoreTimeScale;
}
