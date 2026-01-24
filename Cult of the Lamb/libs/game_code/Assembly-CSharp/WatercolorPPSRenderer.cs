// Decompiled with JetBrains decompiler
// Type: WatercolorPPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public sealed class WatercolorPPSRenderer : PostProcessEffectRenderer<WatercolorPPSSettings>
{
  public override void Render(PostProcessRenderContext context)
  {
    PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Watercolor"));
    if ((Object) this.settings._MainTex.value != (Object) null)
      propertySheet.properties.SetTexture("_MainTex", (Texture) (ParameterOverride<Texture>) this.settings._MainTex);
    propertySheet.properties.SetColor("_Color", (Color) (ParameterOverride<Color>) this.settings._Color);
    propertySheet.properties.SetFloat("_StencilComp", (float) (ParameterOverride<float>) this.settings._StencilComp);
    propertySheet.properties.SetFloat("_Stencil", (float) (ParameterOverride<float>) this.settings._Stencil);
    propertySheet.properties.SetFloat("_StencilOp", (float) (ParameterOverride<float>) this.settings._StencilOp);
    propertySheet.properties.SetFloat("_StencilWriteMask", (float) (ParameterOverride<float>) this.settings._StencilWriteMask);
    propertySheet.properties.SetFloat("_StencilReadMask", (float) (ParameterOverride<float>) this.settings._StencilReadMask);
    propertySheet.properties.SetFloat("_ColorMask", (float) (ParameterOverride<float>) this.settings._ColorMask);
    propertySheet.properties.SetFloat("_UseUIAlphaClip", (float) (ParameterOverride<float>) this.settings._UseUIAlphaClip);
    propertySheet.properties.SetFloat("_BlotchMultiply", (float) (ParameterOverride<float>) this.settings._BlotchMultiply);
    propertySheet.properties.SetFloat("_BlotchSubtract", (float) (ParameterOverride<float>) this.settings._BlotchSubtract);
    if ((Object) this.settings._Texture1.value != (Object) null)
      propertySheet.properties.SetTexture("_Texture1", (Texture) (ParameterOverride<Texture>) this.settings._Texture1);
    propertySheet.properties.SetFloat("_MovementSpeed", (float) (ParameterOverride<float>) this.settings._MovementSpeed);
    propertySheet.properties.SetVector("_MovementDirection", (Vector4) (ParameterOverride<Vector4>) this.settings._MovementDirection);
    propertySheet.properties.SetFloat("_CloudDensity", (float) (ParameterOverride<float>) this.settings._CloudDensity);
    if ((Object) this.settings._Texture0.value != (Object) null)
      propertySheet.properties.SetTexture("_Texture0", (Texture) (ParameterOverride<Texture>) this.settings._Texture0);
    propertySheet.properties.SetVector("_TilingUV", (Vector4) (ParameterOverride<Vector4>) this.settings._TilingUV);
    propertySheet.properties.SetFloat("_FadeOffset", (float) (ParameterOverride<float>) this.settings._FadeOffset);
    context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
  }
}
