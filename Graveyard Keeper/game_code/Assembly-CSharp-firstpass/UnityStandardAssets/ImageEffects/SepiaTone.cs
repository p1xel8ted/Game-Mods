// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.SepiaTone
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
public class SepiaTone : ImageEffectBase
{
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
