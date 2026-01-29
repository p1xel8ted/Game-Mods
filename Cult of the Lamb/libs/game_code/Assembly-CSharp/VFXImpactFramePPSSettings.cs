// Decompiled with JetBrains decompiler
// Type: VFXImpactFramePPSSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
[PostProcess(typeof (VFXImpactFramePPSRenderer), PostProcessEvent.AfterStack, "VFXImpactFrame", true)]
[Serializable]
public sealed class VFXImpactFramePPSSettings : PostProcessEffectSettings
{
  [Tooltip("ContrastAmount")]
  public FloatParameter _ContrastAmount;
  [Tooltip("PosterizeAmount")]
  public FloatParameter _PosterizeAmount;
  [Tooltip("ImpactFrame")]
  public TextureParameter _ImpactFrame;
  [Tooltip("Texture 1")]
  public TextureParameter _Texture1;
  [Tooltip("BlendAmount")]
  public FloatParameter _BlendAmount;
  [Tooltip("DistortionUV")]
  public FloatParameter _DistortionUV;
  [Tooltip("EffectAlpha")]
  [Range(0.0f, 1f)]
  public FloatParameter _EffectAlpha;

  public VFXImpactFramePPSSettings()
  {
    FloatParameter floatParameter1 = new FloatParameter();
    floatParameter1.value = 101.33f;
    this._ContrastAmount = floatParameter1;
    FloatParameter floatParameter2 = new FloatParameter();
    floatParameter2.value = 101.33f;
    this._PosterizeAmount = floatParameter2;
    this._ImpactFrame = new TextureParameter();
    this._Texture1 = new TextureParameter();
    FloatParameter floatParameter3 = new FloatParameter();
    floatParameter3.value = 0.0f;
    this._BlendAmount = floatParameter3;
    FloatParameter floatParameter4 = new FloatParameter();
    floatParameter4.value = 0.25f;
    this._DistortionUV = floatParameter4;
    FloatParameter floatParameter5 = new FloatParameter();
    floatParameter5.value = 1f;
    this._EffectAlpha = floatParameter5;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
