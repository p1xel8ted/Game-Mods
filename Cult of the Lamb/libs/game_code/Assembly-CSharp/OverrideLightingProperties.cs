// Decompiled with JetBrains decompiler
// Type: OverrideLightingProperties
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class OverrideLightingProperties
{
  public bool UnscaledTime;
  public bool Enabled;
  [Header("Ambient")]
  public bool AmbientColor;
  [Header("Directional Light")]
  public bool DirectionalLightColor;
  public bool DirectionalLightIntensity;
  public bool ShadowStrength;
  public bool LightRotation;
  [Header("Lighting Ramp Colours")]
  public bool GlobalHighlightColor;
  public bool GlobalShadowColor;
  [Header("StencilLighting LUT Effects")]
  public bool LUTTextureShadow;
  public bool LUTTextureLit;
  public bool StencilInfluence;
  public bool Exposure;
  [Header("Fog")]
  public bool FogColor;
  public bool FogDist;
  public bool FogHeight;
  public bool FogSpread;
  [Header("God Rays Color")]
  public bool GodRayColor;
  [Header("Screenspace Shadows")]
  public bool ScreenSpaceOverlayMat;
}
