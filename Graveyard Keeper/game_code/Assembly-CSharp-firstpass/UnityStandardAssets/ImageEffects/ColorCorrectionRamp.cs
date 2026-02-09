// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.ColorCorrectionRamp
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
public class ColorCorrectionRamp : ImageEffectBase
{
  public Texture textureRamp;

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.SetTexture("_RampTex", this.textureRamp);
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
