// Decompiled with JetBrains decompiler
// Type: GroundLight
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class GroundLight : MonoBehaviour
{
  public float z_shift = -1f;
  public float intensity_k = 1f;
  public DynamicSpritePreset intensity_preset;

  public void LateUpdate()
  {
    this.transform.position = this.transform.position with
    {
      z = 2000f + this.z_shift
    };
  }
}
