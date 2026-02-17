// Decompiled with JetBrains decompiler
// Type: StencilLighting_MaskEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public class StencilLighting_MaskEffect : BaseMonoBehaviour
{
  public Camera LightingCamera;
  [CompilerGenerated]
  public static StencilLighting_MaskEffect \u003CInstance\u003Ek__BackingField;
  public AmplifyPostEffect amplifyPostEffect;
  public PostProcessVolume ppv;
  public Light mainDirLight;
  public string mainDirLightTag = "MainDirLight";
  public static bool DisableRender;
  public static RenderTexture _lightingRenderTexture;
  public static RenderTexture _grabReplacementTexture;
  public static Material FlipBlitGrabMat;
  public Resolution _currentResolution;
  public StencilLighting_MaskEffect_Performance _MaskEffect_Performance;

  public static StencilLighting_MaskEffect Instance
  {
    get => StencilLighting_MaskEffect.\u003CInstance\u003Ek__BackingField;
    set => StencilLighting_MaskEffect.\u003CInstance\u003Ek__BackingField = value;
  }

  public void Add(IStencilLighting go) => this._MaskEffect_Performance.Add(go);

  public void Remove(IStencilLighting go) => this._MaskEffect_Performance.Remove(go);

  public void Awake()
  {
    if (!((Object) StencilLighting_MaskEffect.Instance == (Object) null))
      return;
    StencilLighting_MaskEffect.Instance = this;
  }

  public void Start()
  {
    if ((Object) GameObject.FindGameObjectWithTag(this.mainDirLightTag) != (Object) null)
      this.mainDirLight = GameObject.FindGameObjectWithTag(this.mainDirLightTag).GetComponent<Light>();
    this.LightingCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
    this.UpdateRenderTexture();
    if (!((Object) LightingManager.Instance != (Object) null))
      return;
    this.ppv = LightingManager.Instance.ppv;
    this.ppv.profile.TryGetSettings<AmplifyPostEffect>(out this.amplifyPostEffect);
    if ((bool) (Object) this.amplifyPostEffect)
      return;
    this.amplifyPostEffect = this.ppv.profile.AddSettings<AmplifyPostEffect>();
  }

  public void OnPerformanceModeChanged(bool isPerformanceMode) => this.enabled = !isPerformanceMode;

  public void UpdateRenderTexture(bool forceUpdate = false)
  {
    int width = Screen.width;
    int height = Screen.height;
    if (((this._currentResolution.width != width ? 1 : (this._currentResolution.height != height ? 1 : 0)) | (forceUpdate ? 1 : 0)) == 0)
      return;
    this.DestroyRenderTexture();
    this._currentResolution.width = width;
    this._currentResolution.height = height;
    RenderTexture renderTexture = new RenderTexture(new RenderTextureDescriptor(this._currentResolution.width, this._currentResolution.height, RenderTextureFormat.Default, 16 /*0x10*/, 0));
    renderTexture.name = "LightingCam_Mask" + Time.frameCount.ToString();
    StencilLighting_MaskEffect._lightingRenderTexture = renderTexture;
    Shader.SetGlobalTexture("_lightingRenderGlobalTexture", (Texture) StencilLighting_MaskEffect._lightingRenderTexture);
  }

  public void OnDestroy()
  {
    this.DestroyRenderTexture();
    StencilLighting_MaskEffect.Instance = (StencilLighting_MaskEffect) null;
  }

  public void DestroyRenderTexture()
  {
    if ((Object) StencilLighting_MaskEffect._lightingRenderTexture != (Object) null)
    {
      StencilLighting_MaskEffect._lightingRenderTexture.Release();
      StencilLighting_MaskEffect._lightingRenderTexture.DiscardContents();
      Object.Destroy((Object) StencilLighting_MaskEffect._lightingRenderTexture);
      StencilLighting_MaskEffect._lightingRenderTexture = (RenderTexture) null;
    }
    if (!((Object) StencilLighting_MaskEffect._grabReplacementTexture != (Object) null))
      return;
    StencilLighting_MaskEffect._grabReplacementTexture.Release();
    StencilLighting_MaskEffect._grabReplacementTexture.DiscardContents();
    Object.Destroy((Object) StencilLighting_MaskEffect._grabReplacementTexture);
    StencilLighting_MaskEffect._grabReplacementTexture = (RenderTexture) null;
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (StencilLighting_MaskEffect.DisableRender || !this.enabled)
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((Object) this.LightingCamera == (Object) null || (Object) this.amplifyPostEffect == (Object) null || (Object) this.mainDirLight == (Object) null)
        return;
      this.LightingCamera.enabled = false;
      this.mainDirLight.shadows = LightShadows.None;
      this.UpdateRenderTexture();
      this.LightingCamera.targetTexture = StencilLighting_MaskEffect._lightingRenderTexture;
      RenderTexture active = RenderTexture.active;
      RenderTexture.active = StencilLighting_MaskEffect._lightingRenderTexture;
      GL.Clear(false, true, Color.clear);
      RenderTexture.active = active;
      this.LightingCamera.SetTargetBuffers(StencilLighting_MaskEffect._lightingRenderTexture.colorBuffer, source.depthBuffer);
      this.LightingCamera.Render();
      this.amplifyPostEffect.MaskTexture.value = StencilLighting_MaskEffect._lightingRenderTexture;
      this.mainDirLight.shadows = LightShadows.Soft;
      Graphics.Blit((Texture) source, destination);
    }
  }
}
