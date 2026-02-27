// Decompiled with JetBrains decompiler
// Type: VFXImpactFramePPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
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
    context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
  }
}
