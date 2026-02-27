// Decompiled with JetBrains decompiler
// Type: WatercolorPPSSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
[PostProcess(typeof (WatercolorPPSRenderer), PostProcessEvent.AfterStack, "Watercolor", true)]
[Serializable]
public sealed class WatercolorPPSSettings : PostProcessEffectSettings
{
  [Tooltip("Sprite Texture")]
  public TextureParameter _MainTex;
  [Tooltip("Tint")]
  public ColorParameter _Color;
  [Tooltip("Stencil Comparison")]
  public FloatParameter _StencilComp;
  [Tooltip("Stencil ID")]
  public FloatParameter _Stencil;
  [Tooltip("Stencil Operation")]
  public FloatParameter _StencilOp;
  [Tooltip("Stencil Write Mask")]
  public FloatParameter _StencilWriteMask;
  [Tooltip("Stencil Read Mask")]
  public FloatParameter _StencilReadMask;
  [Tooltip("Color Mask")]
  public FloatParameter _ColorMask;
  [Tooltip("Use Alpha Clip")]
  public FloatParameter _UseUIAlphaClip;
  [Tooltip("BlotchMultiply")]
  public FloatParameter _BlotchMultiply;
  [Tooltip("BlotchSubtract")]
  public FloatParameter _BlotchSubtract;
  [Tooltip("Texture 1")]
  public TextureParameter _Texture1;
  [Tooltip("MovementSpeed")]
  public FloatParameter _MovementSpeed;
  [Tooltip("MovementDirection")]
  public Vector4Parameter _MovementDirection;
  [Tooltip("CloudDensity")]
  public FloatParameter _CloudDensity;
  [Tooltip("Texture 0")]
  public TextureParameter _Texture0;
  [Tooltip("TilingUV")]
  public Vector4Parameter _TilingUV;
  [Tooltip("FadeOffset")]
  public FloatParameter _FadeOffset;

  public WatercolorPPSSettings()
  {
    ColorParameter colorParameter = new ColorParameter();
    colorParameter.value = new Color(1f, 1f, 1f, 1f);
    this._Color = colorParameter;
    FloatParameter floatParameter1 = new FloatParameter();
    floatParameter1.value = 8f;
    this._StencilComp = floatParameter1;
    FloatParameter floatParameter2 = new FloatParameter();
    floatParameter2.value = 0.0f;
    this._Stencil = floatParameter2;
    FloatParameter floatParameter3 = new FloatParameter();
    floatParameter3.value = 0.0f;
    this._StencilOp = floatParameter3;
    FloatParameter floatParameter4 = new FloatParameter();
    floatParameter4.value = (float) byte.MaxValue;
    this._StencilWriteMask = floatParameter4;
    FloatParameter floatParameter5 = new FloatParameter();
    floatParameter5.value = (float) byte.MaxValue;
    this._StencilReadMask = floatParameter5;
    FloatParameter floatParameter6 = new FloatParameter();
    floatParameter6.value = 15f;
    this._ColorMask = floatParameter6;
    FloatParameter floatParameter7 = new FloatParameter();
    floatParameter7.value = 0.0f;
    this._UseUIAlphaClip = floatParameter7;
    FloatParameter floatParameter8 = new FloatParameter();
    floatParameter8.value = 4.003983f;
    this._BlotchMultiply = floatParameter8;
    FloatParameter floatParameter9 = new FloatParameter();
    floatParameter9.value = 2f;
    this._BlotchSubtract = floatParameter9;
    this._Texture1 = new TextureParameter();
    FloatParameter floatParameter10 = new FloatParameter();
    floatParameter10.value = 1f;
    this._MovementSpeed = floatParameter10;
    Vector4Parameter vector4Parameter1 = new Vector4Parameter();
    vector4Parameter1.value = new Vector4(0.0f, 1f, 0.0f, 0.0f);
    this._MovementDirection = vector4Parameter1;
    FloatParameter floatParameter11 = new FloatParameter();
    floatParameter11.value = 1f;
    this._CloudDensity = floatParameter11;
    this._Texture0 = new TextureParameter();
    Vector4Parameter vector4Parameter2 = new Vector4Parameter();
    vector4Parameter2.value = new Vector4(1f, 1f, 0.0f, 0.0f);
    this._TilingUV = vector4Parameter2;
    FloatParameter floatParameter12 = new FloatParameter();
    floatParameter12.value = 0.0f;
    this._FadeOffset = floatParameter12;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
