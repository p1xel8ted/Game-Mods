// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.ScreenOverlay
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Other/Screen Overlay")]
public class ScreenOverlay : PostEffectsBase
{
  public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;
  public float intensity = 1f;
  public Texture2D texture;
  public Shader overlayShader;
  public Material overlayMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.overlayMaterial = this.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.overlayMaterial.SetVector("_UV_Transform", new Vector4(1f, 0.0f, 0.0f, 1f));
      this.overlayMaterial.SetFloat("_Intensity", this.intensity);
      this.overlayMaterial.SetTexture("_Overlay", (Texture) this.texture);
      Graphics.Blit((Texture) source, destination, this.overlayMaterial, (int) this.blendMode);
    }
  }

  public enum OverlayBlendMode
  {
    Additive,
    ScreenBlend,
    Multiply,
    Overlay,
    AlphaBlend,
  }
}
