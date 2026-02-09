// Decompiled with JetBrains decompiler
// Type: InteractionAnimationPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu]
public class InteractionAnimationPreset : ScriptableObject
{
  public float duration = 1f;
  public float duration_random = 0.15f;
  public AnimationCurve alpha_curve;
  public float rotation_a = 10f;
}
