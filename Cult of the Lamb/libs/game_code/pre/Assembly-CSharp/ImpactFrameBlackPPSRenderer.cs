// Decompiled with JetBrains decompiler
// Type: ImpactFrameBlackPPSRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
