// Decompiled with JetBrains decompiler
// Type: Desaturate_StencilRTMaskPPSSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
