// Decompiled with JetBrains decompiler
// Type: VFXImpactFramePPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public sealed class VFXImpactFramePPSRenderer : PostProcessEffectRenderer<VFXImpactFramePPSSettings>
{
  public override void Render(PostProcessRenderContext context)
  {
    if (!((Object) Shader.Find("VFX/ImpactFrame") != (Object) null))
      return;
    PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("VFX/ImpactFrame"));
    propertySheet.properties.SetFloat("_ContrastAmount", (float) (ParameterOverride<float>) this.settings._ContrastAmount);
    propertySheet.properties.SetFloat("_PosterizeAmount", (float) (ParameterOverride<float>) this.settings._PosterizeAmount);
    if ((Object) this.settings._ImpactFrame.value != (Object) null)
      propertySheet.properties.SetTexture("_ImpactFrame", (Texture) (ParameterOverride<Texture>) this.settings._ImpactFrame);
    if ((Object) this.settings._Texture1.value != (Object) null)
      propertySheet.properties.SetTexture("_Texture1", (Texture) (ParameterOverride<Texture>) this.settings._Texture1);
    propertySheet.properties.SetFloat("_BlendAmount", (float) (ParameterOverride<float>) this.settings._BlendAmount);
    propertySheet.properties.SetFloat("_DistortionUV", (float) (ParameterOverride<float>) this.settings._DistortionUV);
    propertySheet.properties.SetFloat("_EffectAlpha", (float) (ParameterOverride<float>) this.settings._EffectAlpha);
    context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
  }
}
