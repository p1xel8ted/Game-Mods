// Decompiled with JetBrains decompiler
// Type: GraphicsSettingsUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public class GraphicsSettingsUtilities
{
  private const string kShadowsOnKeyword = "_SHADOWS_ON";
  public static System.Action OnEnvironmentSettingsChanged;
  public static GraphicsSettingsUtilities.GraphicsPresetValues LowPreset = new GraphicsSettingsUtilities.GraphicsPresetValues()
  {
    EnvironmentDetail = 0,
    LightingQuality = 0,
    Shadows = false,
    Bloom = false,
    Vignette = false,
    ChromaticAberration = false,
    DepthOfField = false,
    AntiAliasing = false
  };
  public static readonly GraphicsSettingsUtilities.GraphicsPresetValues MediumPreset = new GraphicsSettingsUtilities.GraphicsPresetValues()
  {
    EnvironmentDetail = 1,
    LightingQuality = 1,
    Shadows = true,
    Bloom = false,
    Vignette = false,
    ChromaticAberration = false,
    DepthOfField = false,
    AntiAliasing = false
  };
  public static readonly GraphicsSettingsUtilities.GraphicsPresetValues HighPreset = new GraphicsSettingsUtilities.GraphicsPresetValues()
  {
    EnvironmentDetail = 1,
    LightingQuality = 2,
    Shadows = true,
    Bloom = true,
    Vignette = true,
    ChromaticAberration = true,
    DepthOfField = true,
    AntiAliasing = true
  };
  public static readonly GraphicsSettingsUtilities.GraphicsPresetValues UltraPreset = new GraphicsSettingsUtilities.GraphicsPresetValues()
  {
    EnvironmentDetail = 1,
    LightingQuality = 2,
    Shadows = true,
    Bloom = true,
    Vignette = true,
    ChromaticAberration = true,
    DepthOfField = true,
    AntiAliasing = true
  };

  public static void SetLightingQuality(int index)
  {
    switch (index)
    {
      case 0:
        QualitySettings.pixelLightCount = 0;
        break;
      case 1:
        QualitySettings.pixelLightCount = 2;
        break;
      case 2:
        QualitySettings.pixelLightCount = 5;
        break;
    }
  }

  public static void SetEnvironmentDetail(int index)
  {
    System.Action environmentSettingsChanged = GraphicsSettingsUtilities.OnEnvironmentSettingsChanged;
    if (environmentSettingsChanged == null)
      return;
    environmentSettingsChanged();
  }

  public static void UpdateShadows(bool value)
  {
    if (!value)
    {
      QualitySettings.shadows = ShadowQuality.Disable;
      QualitySettings.shadowResolution = ShadowResolution.Low;
      QualitySettings.shadowDistance = 0.0f;
      Shader.DisableKeyword("_SHADOWS_ON");
    }
    else
    {
      QualitySettings.shadows = ShadowQuality.All;
      QualitySettings.shadowResolution = ShadowResolution.Low;
      QualitySettings.shadowDistance = 50f;
      Shader.EnableKeyword("_SHADOWS_ON");
    }
  }

  public static void UpdatePostProcessing()
  {
    if (!((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null))
      return;
    BiomeConstants.Instance.updatePostProcessing();
  }

  public static void SetTargetFramerate(int index)
  {
    int num = 0;
    switch (index)
    {
      case 0:
        num = 30;
        break;
      case 1:
        num = 60;
        break;
      case 2:
        num = 300;
        break;
    }
    Application.targetFrameRate = num;
  }

  public static PostProcessLayer.Antialiasing AntiAliasingModeFromBool(bool b)
  {
    return !b ? PostProcessLayer.Antialiasing.None : PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
  }

  public struct GraphicsPresetValues
  {
    public int EnvironmentDetail;
    public int LightingQuality;
    public bool Shadows;
    public bool Bloom;
    public bool ChromaticAberration;
    public bool Vignette;
    public bool DepthOfField;
    public bool AntiAliasing;
  }
}
