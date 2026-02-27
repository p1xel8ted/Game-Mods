// Decompiled with JetBrains decompiler
// Type: Desaturate_StencilRTMaskPPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
