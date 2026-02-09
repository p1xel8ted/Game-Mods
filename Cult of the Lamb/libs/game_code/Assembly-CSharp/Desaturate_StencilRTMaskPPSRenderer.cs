// Decompiled with JetBrains decompiler
// Type: Desaturate_StencilRTMaskPPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public sealed class Desaturate_StencilRTMaskPPSRenderer : 
  PostProcessEffectRenderer<Desaturate_StencilRTMaskPPSSettings>
{
  public override void Render(PostProcessRenderContext context)
  {
    PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Desaturate_StencilRTMask"));
    propertySheet.properties.SetFloat("_Intensity", (float) (ParameterOverride<float>) this.settings._Intensity);
    propertySheet.properties.SetFloat("_Contrast", (float) (ParameterOverride<float>) this.settings._Contrast);
    propertySheet.properties.SetFloat("_Brightness", (float) (ParameterOverride<float>) this.settings._Brightness);
    context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
  }
}
