// Decompiled with JetBrains decompiler
// Type: Desaturate_StencilRTMaskPPSSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
[PostProcess(typeof (Desaturate_StencilRTMaskPPSRenderer), PostProcessEvent.BeforeStack, "Desaturate_StencilRTMask", true)]
[Serializable]
public sealed class Desaturate_StencilRTMaskPPSSettings : PostProcessEffectSettings
{
  [Tooltip("Intensity")]
  [Range(0.0f, 1f)]
  public FloatParameter _Intensity;
  [Tooltip("Contrast")]
  public FloatParameter _Contrast;
  [Tooltip("Brightness")]
  [Range(-1f, 1f)]
  public FloatParameter _Brightness;

  public Desaturate_StencilRTMaskPPSSettings()
  {
    FloatParameter floatParameter1 = new FloatParameter();
    floatParameter1.value = 1f;
    this._Intensity = floatParameter1;
    FloatParameter floatParameter2 = new FloatParameter();
    floatParameter2.value = 1f;
    this._Contrast = floatParameter2;
    FloatParameter floatParameter3 = new FloatParameter();
    floatParameter3.value = 0.0f;
    this._Brightness = floatParameter3;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
