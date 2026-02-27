// Decompiled with JetBrains decompiler
// Type: SettingsData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  public class GameSettings : IEquatable<SettingsData.GameSettings>
  {
    public string Language = string.Empty;
    public float RumbleIntensity = 1f;
    public bool ShowTutorials = true;
    public int GamepadPrompts;

    public GameSettings()
    {
    }

    public GameSettings(SettingsData.GameSettings gameSettings)
    {
      this.Language = gameSettings.Language;
      this.RumbleIntensity = gameSettings.RumbleIntensity;
      this.ShowTutorials = gameSettings.ShowTutorials;
      this.GamepadPrompts = gameSettings.GamepadPrompts;
    }

    public bool Equals(SettingsData.GameSettings other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return this.Language == other.Language && this.RumbleIntensity.Equals(other.RumbleIntensity) && this.ShowTutorials == other.ShowTutorials && this.GamepadPrompts == other.GamepadPrompts;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((SettingsData.GameSettings) obj);
    }

    public override int GetHashCode()
    {
      return (((this.Language != null ? this.Language.GetHashCode() : 0) * 397 ^ this.RumbleIntensity.GetHashCode()) * 397 ^ this.ShowTutorials.GetHashCode()) * 397 ^ this.GamepadPrompts;
    }
  }

  [Serializable]
  public class GraphicsSettings : IEquatable<SettingsData.GraphicsSettings>
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

    public bool Equals(SettingsData.GraphicsSettings other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return this.GraphicsPreset == other.GraphicsPreset && this.FullscreenMode == other.FullscreenMode && this.Resolution == other.Resolution && this.TargetFrameRate == other.TargetFrameRate && this.VSync == other.VSync && this.LightingQuality == other.LightingQuality && this.EnvironmentDetail == other.EnvironmentDetail && this.Shadows == other.Shadows && this.Bloom == other.Bloom && this.ChromaticAberration == other.ChromaticAberration && this.Vignette == other.Vignette && this.DepthOfField == other.DepthOfField;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((SettingsData.GraphicsSettings) obj);
    }

    public override int GetHashCode()
    {
      return ((((((((((this.GraphicsPreset * 397 ^ this.FullscreenMode) * 397 ^ this.Resolution) * 397 ^ this.TargetFrameRate) * 397 ^ this.VSync.GetHashCode()) * 397 ^ this.LightingQuality) * 397 ^ this.EnvironmentDetail) * 397 ^ this.Shadows.GetHashCode()) * 397 ^ this.Bloom.GetHashCode()) * 397 ^ this.ChromaticAberration.GetHashCode()) * 397 ^ this.Vignette.GetHashCode()) * 397 ^ this.DepthOfField.GetHashCode();
    }
  }

  [Serializable]
  public class AccessibilitySettings : IEquatable<SettingsData.AccessibilitySettings>
  {
    public bool DyslexicFont;
    public float TextScale = 1f;
    public bool AnimatedText = true;
    public bool TextStyling = true;
    public bool FlashingLights = true;
    public float ScreenShake = 1f;
    public bool ReduceCameraMotion;
    public float DitherFadeDistance = 1f;
    public bool HoldActions = true;
    public bool AutoCook;
    public bool AutoFish;

    public AccessibilitySettings()
    {
    }

    public AccessibilitySettings(
      SettingsData.AccessibilitySettings accessibilitySettings)
    {
      this.DyslexicFont = accessibilitySettings.DyslexicFont;
      this.TextScale = accessibilitySettings.TextScale;
      this.AnimatedText = accessibilitySettings.AnimatedText;
      this.TextStyling = accessibilitySettings.TextStyling;
      this.FlashingLights = accessibilitySettings.FlashingLights;
      this.ScreenShake = accessibilitySettings.ScreenShake;
      this.ReduceCameraMotion = accessibilitySettings.ReduceCameraMotion;
      this.DitherFadeDistance = accessibilitySettings.DitherFadeDistance;
    }

    public bool Equals(SettingsData.AccessibilitySettings other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return this.DyslexicFont == other.DyslexicFont && this.TextScale.Equals(other.TextScale) && this.AnimatedText == other.AnimatedText && this.TextStyling == other.TextStyling && this.FlashingLights == other.FlashingLights && this.ScreenShake.Equals(other.ScreenShake) && this.ReduceCameraMotion == other.ReduceCameraMotion && this.DitherFadeDistance.Equals(other.DitherFadeDistance);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((SettingsData.AccessibilitySettings) obj);
    }

    public override int GetHashCode()
    {
      return ((((((this.DyslexicFont.GetHashCode() * 397 ^ this.TextScale.GetHashCode()) * 397 ^ this.AnimatedText.GetHashCode()) * 397 ^ this.TextStyling.GetHashCode()) * 397 ^ this.FlashingLights.GetHashCode()) * 397 ^ this.ScreenShake.GetHashCode()) * 397 ^ this.ReduceCameraMotion.GetHashCode()) * 397 ^ this.DitherFadeDistance.GetHashCode();
    }
  }

  [Serializable]
  public class AudioSettings : IEquatable<SettingsData.AudioSettings>
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

    public bool Equals(SettingsData.AudioSettings other)
    {
      return this.MasterVolume.Equals(other.MasterVolume) && this.MusicVolume.Equals(other.MusicVolume) && this.SFXVolume.Equals(other.SFXVolume) && this.VOVolume.Equals(other.VOVolume);
    }

    public override bool Equals(object obj)
    {
      return obj is SettingsData.AudioSettings other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return ((this.MasterVolume.GetHashCode() * 397 ^ this.MusicVolume.GetHashCode()) * 397 ^ this.SFXVolume.GetHashCode()) * 397 ^ this.VOVolume.GetHashCode();
    }
  }

  [Serializable]
  public class ControlSettings : IEquatable<SettingsData.ControlSettings>
  {
    public List<Binding> KeyboardBindings = new List<Binding>();
    public List<UnboundBinding> KeyboardBindingsUnbound = new List<UnboundBinding>();
    public List<Binding> MouseBindings = new List<Binding>();
    public List<UnboundBinding> MouseBindingsUnbound = new List<UnboundBinding>();
    public int GamepadLayout;
    public List<Binding> GamepadBindings = new List<Binding>();
    public List<UnboundBinding> GamepadBindingsUnbound = new List<UnboundBinding>();

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
      this.GamepadBindingsUnbound = new List<UnboundBinding>((IEnumerable<UnboundBinding>) controlSettings.GamepadBindingsUnbound);
      this.GamepadLayout = controlSettings.GamepadLayout;
    }

    public bool Equals(SettingsData.ControlSettings other)
    {
      return ListExtensions.Equals<Binding>(this.KeyboardBindings, other.KeyboardBindings) && this.GamepadLayout == other.GamepadLayout;
    }

    public override bool Equals(object obj)
    {
      return obj is SettingsData.ControlSettings other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return (this.KeyboardBindings != null ? this.KeyboardBindings.GetHashCode() : 0) * 397 ^ this.GamepadLayout;
    }
  }
}
