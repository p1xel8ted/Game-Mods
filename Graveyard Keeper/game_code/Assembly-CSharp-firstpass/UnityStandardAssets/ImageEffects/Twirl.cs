// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.Twirl
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/Displacement/Twirl")]
[ExecuteInEditMode]
public class Twirl : ImageEffectBase
{
  public Vector2 radius = new Vector2(0.3f, 0.3f);
  [Range(0.0f, 360f)]
  public float angle = 50f;
  public Vector2 center = new Vector2(0.5f, 0.5f);

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    UnityStandardAssets.ImageEffects.ImageEffects.RenderDistortion(this.material, source, destination, this.angle, this.center, this.radius);
  }
}
