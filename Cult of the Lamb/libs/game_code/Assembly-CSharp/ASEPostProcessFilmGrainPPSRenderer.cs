// Decompiled with JetBrains decompiler
// Type: ASEPostProcessFilmGrainPPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public sealed class ASEPostProcessFilmGrainPPSRenderer : 
  PostProcessEffectRenderer<ASEPostProcessFilmGrainPPSSettings>
{
  public override void Render(PostProcessRenderContext context)
  {
    PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Post Process/FilmGrain"));
    propertySheet.properties.SetFloat("_Strength", (float) (ParameterOverride<float>) this.settings._Strength);
    propertySheet.properties.SetFloat("_ColorMaskRange", (float) (ParameterOverride<float>) this.settings._ColorMaskRange);
    propertySheet.properties.SetFloat("_ColorMaskFuzziness", (float) (ParameterOverride<float>) this.settings._ColorMaskFuzziness);
    propertySheet.properties.SetColor("_ColorToMask", (Color) (ParameterOverride<Color>) this.settings._ColorToMask);
    context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
  }
}
