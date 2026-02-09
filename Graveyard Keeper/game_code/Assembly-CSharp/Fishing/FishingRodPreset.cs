// Decompiled with JetBrains decompiler
// Type: Fishing.FishingRodPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Fishing;

[CreateAssetMenu(fileName = "FishingRodPreset", menuName = "Mini Games/Fishing Rod Preset", order = 1)]
public class FishingRodPreset : ScriptableObject
{
  [Header("Size of \"catching\" bar")]
  public int rect_size = 30;
  [Space]
  public float gravity = 0.8f;
  public float force = 0.1f;
  public float impulse = 1f;
  public float mass = 1f;
}
