// Decompiled with JetBrains decompiler
// Type: BiomeLightingSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "COTL/BiomeLightingSettings", fileName = "BiomeLightingSettings_")]
public class BiomeLightingSettings : ScriptableObject
{
  [Header("Time")]
  [SerializeField]
  public bool m_unscaledTime;
  [Header("Ambient")]
  [ColorUsage(false, true)]
  [SerializeField]
  public Color m_ambientColour = Color.grey;
  [Header("Directional Light")]
  [ColorUsage(false, false)]
  [SerializeField]
  public Color m_directionalLightColor = Color.white;
  [SerializeField]
  public float m_directionalLightIntensity = 1f;
  [Range(0.0f, 1f)]
  [SerializeField]
  public float m_ShadowStrength = 0.5f;
  [SerializeField]
  public Vector3 m_lightRotation = Vector3.zero;
  [Header("Lighting Ramp Colours")]
  [SerializeField]
  public Color m_globalHighlightColor = new Color(0.8f, 0.8f, 0.8f, 1f);
  [SerializeField]
  public Color m_globalShadowColor = new Color(0.0f, 0.0f, 0.0f, 1f);
  [Header("StencilLighting LUT Effects")]
  [SerializeField]
  public Texture m_LutTexture_Shadow;
  [SerializeField]
  public Texture m_LutTexture_Lit;
  [Range(0.0f, 2f)]
  [SerializeField]
  public float m_StencilInfluence = 0.5f;
  [SerializeField]
  public float m_exposure = 1.15f;
  [Header("Fog")]
  [ColorUsage(true, true)]
  [SerializeField]
  public Color m_fogColor = new Color(0.0741f, 0.0f, 0.129f, 1f);
  [SerializeField]
  public Vector2 m_fogDist = new Vector2(10f, 15f);
  [SerializeField]
  public float m_fogHeight = 0.5f;
  [SerializeField]
  public float m_fogSpread = 1f;
  [Header("God Rays Color")]
  [SerializeField]
  public Color m_godRayColor = Color.white;
  [Header("Screenspace Shadows")]
  [SerializeField]
  public Material m_ScreenSpaceOverlayMat;
  [HideInInspector]
  public OverrideLightingProperties overrideLightingProperties = new OverrideLightingProperties();

  public bool UnscaledTime
  {
    get => this.m_unscaledTime;
    set => this.m_unscaledTime = value;
  }

  public Color AmbientColour
  {
    get => this.m_ambientColour;
    set => this.m_ambientColour = value;
  }

  public Color DirectionalLightColour
  {
    get => this.m_directionalLightColor;
    set => this.m_directionalLightColor = value;
  }

  public float DirectionalLightIntensity
  {
    get => this.m_directionalLightIntensity;
    set => this.m_directionalLightIntensity = value;
  }

  public float ShadowStrength
  {
    get => this.m_ShadowStrength;
    set => this.m_ShadowStrength = value;
  }

  public Vector3 LightRotation
  {
    get => this.m_lightRotation;
    set => this.m_lightRotation = value;
  }

  public Color GlobalHighlightColor
  {
    get => this.m_globalHighlightColor;
    set => this.m_globalHighlightColor = value;
  }

  public Color GlobalShadowColor
  {
    get => this.m_globalShadowColor;
    set => this.m_globalShadowColor = value;
  }

  public Texture LutTexture_Shadow
  {
    get => this.m_LutTexture_Shadow;
    set => this.m_LutTexture_Shadow = value;
  }

  public Texture LutTexture_Lit
  {
    get => this.m_LutTexture_Lit;
    set => this.m_LutTexture_Lit = value;
  }

  public float StencilInfluence
  {
    get => this.m_StencilInfluence;
    set => this.m_StencilInfluence = value;
  }

  public float Exposure
  {
    get => this.m_exposure;
    set => this.m_exposure = value;
  }

  public Color FogColor
  {
    get => this.m_fogColor;
    set => this.m_fogColor = value;
  }

  public Vector2 FogDist
  {
    get => this.m_fogDist;
    set => this.m_fogDist = value;
  }

  public float FogHeight
  {
    get => this.m_fogHeight;
    set => this.m_fogHeight = value;
  }

  public float FogSpread
  {
    get => this.m_fogSpread;
    set => this.m_fogSpread = value;
  }

  public Color GodRayColor
  {
    get => this.m_godRayColor;
    set => this.m_godRayColor = value;
  }

  public Material ScreenSpaceOverlayMat
  {
    get => this.m_ScreenSpaceOverlayMat;
    set => this.m_ScreenSpaceOverlayMat = value;
  }

  public bool IsEquivalent(BiomeLightingSettings other)
  {
    return this.m_ambientColour == other.AmbientColour && this.m_directionalLightColor == other.DirectionalLightColour && (double) this.m_directionalLightIntensity == (double) other.DirectionalLightIntensity && (double) this.m_ShadowStrength == (double) other.ShadowStrength && !this.overrideLightingProperties.LightRotation && !other.overrideLightingProperties.LightRotation && this.m_globalHighlightColor == other.GlobalHighlightColor && this.m_globalShadowColor == other.GlobalShadowColor && (Object) this.m_LutTexture_Shadow == (Object) other.LutTexture_Shadow && (Object) this.m_LutTexture_Lit == (Object) other.LutTexture_Lit && (double) this.m_StencilInfluence == (double) other.StencilInfluence && (double) this.m_exposure == (double) other.Exposure && this.m_fogColor == other.FogColor && this.m_fogDist == other.FogDist && (double) this.m_fogHeight == (double) other.FogHeight && (double) this.m_fogSpread == (double) other.FogSpread && this.m_godRayColor == other.GodRayColor && (Object) this.m_ScreenSpaceOverlayMat == (Object) other.ScreenSpaceOverlayMat;
  }
}
