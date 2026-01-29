// Decompiled with JetBrains decompiler
// Type: SettingsData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unify;

#nullable disable
[Serializable]
public class SettingsData
{
  public SettingsData.GameSettings Game = new SettingsData.GameSettings();
  public SettingsData.GraphicsSettings Graphics = new SettingsData.GraphicsSettings();
  public SettingsData.AccessibilitySettings Accessibility = new SettingsData.AccessibilitySettings();
  public SettingsData.AudioSettings Audio = new SettingsData.AudioSettings();
  public SettingsData.ControlSettings Control = new SettingsData.ControlSettings();

  [Serializable]
  public class GameSettings
  {
    public string Language = string.Empty;
    public float RumbleIntensity = 1f;
    public bool ShowTutorials = true;
    public int GamepadPrompts;
    public bool ShowFollowerNames;
    public float HorizontalAimSensitivity = 1f;
    public float VerticalAimSensitivity = 1f;
    public bool InvertAiming;

    public bool PerformanceMode => false;

    public GameSettings()
    {
    }

    public GameSettings(SettingsData.GameSettings gameSettings)
    {
      this.Language = gameSettings.Language;
      this.RumbleIntensity = gameSettings.RumbleIntensity;
      this.ShowTutorials = gameSettings.ShowTutorials;
      this.GamepadPrompts = gameSettings.GamepadPrompts;
      this.ShowFollowerNames = gameSettings.ShowFollowerNames;
      this.HorizontalAimSensitivity = gameSettings.HorizontalAimSensitivity;
      this.VerticalAimSensitivity = gameSettings.VerticalAimSensitivity;
      this.InvertAiming = gameSettings.InvertAiming;
    }
  }

  [Serializable]
  public class GraphicsSettings
  {
    public int GraphicsPreset = 3;
    public int FullscreenMode = 2;
    public int Resolution = -1;
    public int TargetFrameRate = 1;
    public bool VSync;
    public int LightingQuality = GraphicsSettingsUtilities.UltraPreset.LightingQuality;
    public int EnvironmentDetail = GraphicsSettingsUtilities.UltraPreset.EnvironmentDetail;
    public bool Shadows = GraphicsSettingsUtilities.UltraPreset.Shadows;
    public bool Bloom = GraphicsSettingsUtilities.UltraPreset.Bloom;
    public bool ChromaticAberration = GraphicsSettingsUtilities.UltraPreset.ChromaticAberration;
    public bool Vignette = GraphicsSettingsUtilities.UltraPreset.Vignette;
    public bool DepthOfField = GraphicsSettingsUtilities.UltraPreset.DepthOfField;
    public bool AntiAliasing = GraphicsSettingsUtilities.UltraPreset.AntiAliasing;

    public GraphicsSettings()
    {
      if (UnifyManager.platform != UnifyManager.Platform.Switch)
        return;
      this.GraphicsPreset = 0;
      this.FullscreenMode = 2;
      this.TargetFrameRate = 1;
      this.LightingQuality = GraphicsSettingsUtilities.LowPreset.LightingQuality;
      this.EnvironmentDetail = GraphicsSettingsUtilities.LowPreset.EnvironmentDetail;
      this.Shadows = GraphicsSettingsUtilities.LowPreset.Shadows;
      this.VSync = true;
      this.Bloom = GraphicsSettingsUtilities.LowPreset.Bloom;
      this.Vignette = GraphicsSettingsUtilities.LowPreset.Vignette;
      this.ChromaticAberration = GraphicsSettingsUtilities.LowPreset.ChromaticAberration;
      this.DepthOfField = GraphicsSettingsUtilities.LowPreset.DepthOfField;
      this.AntiAliasing = GraphicsSettingsUtilities.LowPreset.AntiAliasing;
    }

    public GraphicsSettings(SettingsData.GraphicsSettings graphicsSettings)
    {
      this.GraphicsPreset = graphicsSettings.GraphicsPreset;
      this.FullscreenMode = graphicsSettings.FullscreenMode;
      this.Resolution = graphicsSettings.Resolution;
      this.TargetFrameRate = graphicsSettings.TargetFrameRate;
      this.VSync = graphicsSettings.VSync;
      this.LightingQuality = graphicsSettings.LightingQuality;
      this.EnvironmentDetail = graphicsSettings.EnvironmentDetail;
      this.Shadows = graphicsSettings.Shadows;
      this.Bloom = graphicsSettings.Bloom;
      this.ChromaticAberration = graphicsSettings.ChromaticAberration;
      this.Vignette = graphicsSettings.Vignette;
      this.DepthOfField = graphicsSettings.DepthOfField;
    }
  }

  [Serializable]
  public class AccessibilitySettings
  {
    public bool DyslexicFont;
    public float TextScale = 1f;
    public bool AnimatedText = true;
    public bool FlashingLights = true;
    public float ScreenShake = 1f;
    public bool ReduceCameraMotion;
    public float DitherFadeDistance = 1f;
    public bool HoldActions = true;
    public bool AutoCook;
    public bool AutoCraft;
    public bool AutoFish;
    public bool RomanNumerals = true;
    public float WorldTimeScale = 1f;
    public bool StopTimeInCrusade;
    public bool HighContrastText;
    public bool ShowBuildModeFilter = true;
    public bool RemoveTextStyling;
    public bool RemoveLightingEffects;
    public bool DarkMode;
    public int MovementMode;
    public bool InvertMovement;
    public bool UnlimitedHP;
    public bool UnlimitedFervour;
    public bool ForceWeapon;
    public int ForcedWeapon = -1;
    public bool CoopVisualIndicators = true;
    [JsonConverter(typeof (LegacyBooltoFloatConverter))]
    public float CameraParticles = 1f;
    public float WeatherScreenTint = 1f;

    public AccessibilitySettings()
    {
    }

    public AccessibilitySettings(
      SettingsData.AccessibilitySettings accessibilitySettings)
    {
      this.DyslexicFont = accessibilitySettings.DyslexicFont;
      this.TextScale = accessibilitySettings.TextScale;
      this.AnimatedText = accessibilitySettings.AnimatedText;
      this.FlashingLights = accessibilitySettings.FlashingLights;
      this.ScreenShake = accessibilitySettings.ScreenShake;
      this.ReduceCameraMotion = accessibilitySettings.ReduceCameraMotion;
      this.DitherFadeDistance = accessibilitySettings.DitherFadeDistance;
      this.RomanNumerals = accessibilitySettings.RomanNumerals;
      this.WorldTimeScale = accessibilitySettings.WorldTimeScale;
      this.StopTimeInCrusade = accessibilitySettings.StopTimeInCrusade;
      this.HighContrastText = accessibilitySettings.HighContrastText;
      this.ShowBuildModeFilter = accessibilitySettings.ShowBuildModeFilter;
      this.RemoveTextStyling = accessibilitySettings.RemoveTextStyling;
      this.RemoveLightingEffects = accessibilitySettings.RemoveLightingEffects;
      this.MovementMode = accessibilitySettings.MovementMode;
      this.InvertMovement = accessibilitySettings.InvertMovement;
      this.UnlimitedHP = accessibilitySettings.UnlimitedHP;
      this.UnlimitedFervour = accessibilitySettings.UnlimitedFervour;
      this.CoopVisualIndicators = accessibilitySettings.CoopVisualIndicators;
      this.CameraParticles = accessibilitySettings.CameraParticles;
      this.WeatherScreenTint = accessibilitySettings.WeatherScreenTint;
      this.ForceWeapon = accessibilitySettings.ForceWeapon;
      this.ForcedWeapon = accessibilitySettings.ForcedWeapon;
    }
  }

  [Serializable]
  public class AudioSettings
  {
    public float MasterVolume = 1f;
    public float MusicVolume = 0.75f;
    public float SFXVolume = 0.75f;
    public float VOVolume = 0.75f;

    public AudioSettings()
    {
    }

    public AudioSettings(SettingsData.AudioSettings audioSettings)
    {
      this.MasterVolume = audioSettings.MasterVolume;
      this.MusicVolume = audioSettings.MusicVolume;
      this.SFXVolume = audioSettings.SFXVolume;
      this.VOVolume = audioSettings.VOVolume;
    }
  }

  [Serializable]
  public class ControlSettings
  {
    public List<Binding> KeyboardBindings = new List<Binding>();
    public List<UnboundBinding> KeyboardBindingsUnbound = new List<UnboundBinding>();
    public List<Binding> MouseBindings = new List<Binding>();
    public List<UnboundBinding> MouseBindingsUnbound = new List<UnboundBinding>();
    public int GamepadLayout;
    public int GamepadLayout_P2;
    public List<Binding> GamepadBindings = new List<Binding>();
    public List<UnboundBinding> GamepadBindingsUnbound = new List<UnboundBinding>();
    public List<Binding> GamepadBindings_P2 = new List<Binding>();
    public List<UnboundBinding> GamepadBindingsUnbound_P2 = new List<UnboundBinding>();

    public ControlSettings()
    {
    }

    public ControlSettings(SettingsData.ControlSettings controlSettings)
    {
      this.KeyboardBindings = new List<Binding>((IEnumerable<Binding>) controlSettings.KeyboardBindings);
      this.KeyboardBindingsUnbound = new List<UnboundBinding>((IEnumerable<UnboundBinding>) controlSettings.KeyboardBindingsUnbound);
      this.MouseBindings = new List<Binding>((IEnumerable<Binding>) controlSettings.MouseBindings);
      this.MouseBindingsUnbound = new List<UnboundBinding>((IEnumerable<UnboundBinding>) controlSettings.MouseBindingsUnbound);
      this.GamepadBindings = new List<Binding>((IEnumerable<Binding>) controlSettings.GamepadBindings);
      this.GamepadBindings_P2 = new List<Binding>((IEnumerable<Binding>) controlSettings.GamepadBindings_P2);
      this.GamepadBindingsUnbound = new List<UnboundBinding>((IEnumerable<UnboundBinding>) controlSettings.GamepadBindingsUnbound);
      this.GamepadBindingsUnbound_P2 = new List<UnboundBinding>((IEnumerable<UnboundBinding>) controlSettings.GamepadBindingsUnbound_P2);
      this.GamepadLayout = controlSettings.GamepadLayout;
      this.GamepadLayout_P2 = controlSettings.GamepadLayout_P2;
    }
  }
}
