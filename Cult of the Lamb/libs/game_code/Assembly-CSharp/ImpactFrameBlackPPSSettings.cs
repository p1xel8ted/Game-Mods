// Decompiled with JetBrains decompiler
// Type: ImpactFrameBlackPPSSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
[PostProcess(typeof (ImpactFrameBlackPPSRenderer), PostProcessEvent.AfterStack, "ImpactFrameBlack", true)]
[Serializable]
public sealed class ImpactFrameBlackPPSSettings : PostProcessEffectSettings
{
  [Tooltip("Texture 0")]
  public TextureParameter _Texture0;
  [Tooltip("LerpAmount")]
  public FloatParameter _LerpAmount;

  public ImpactFrameBlackPPSSettings()
  {
    FloatParameter floatParameter = new FloatParameter();
    floatParameter.value = 1f;
    this._LerpAmount = floatParameter;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
