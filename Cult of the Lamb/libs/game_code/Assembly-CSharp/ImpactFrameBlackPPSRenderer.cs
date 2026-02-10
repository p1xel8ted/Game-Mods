// Decompiled with JetBrains decompiler
// Type: ImpactFrameBlackPPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public sealed class ImpactFrameBlackPPSRenderer : 
  PostProcessEffectRenderer<ImpactFrameBlackPPSSettings>
{
  public override void Render(PostProcessRenderContext context)
  {
    PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("ImpactFrameBlack"));
    if ((Object) this.settings._Texture0.value != (Object) null)
      propertySheet.properties.SetTexture("_Texture0", (Texture) (ParameterOverride<Texture>) this.settings._Texture0);
    propertySheet.properties.SetFloat("_LerpAmount", (float) (ParameterOverride<float>) this.settings._LerpAmount);
    context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
  }
}
