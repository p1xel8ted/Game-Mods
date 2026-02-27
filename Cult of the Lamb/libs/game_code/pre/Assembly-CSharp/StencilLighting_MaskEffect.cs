// Decompiled with JetBrains decompiler
// Type: StencilLighting_MaskEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public class StencilLighting_MaskEffect : BaseMonoBehaviour
{
  private Camera LightingCamera;
  private AmplifyPostEffect amplifyPostEffect;
  private PostProcessVolume ppv;
  private Light mainDirLight;
  private string mainDirLightTag = "MainDirLight";
  public static bool DisableRender;
  private static RenderTexture _lightingRenderTexture;
  private Resolution _currentResolution;

  private void Start()
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

  private void UpdateRenderTexture()
  {
    int width = Screen.width;
    int height = Screen.height;
    if (this._currentResolution.width == width && this._currentResolution.height == height)
      return;
    this.DestroyRenderTexture();
    this._currentResolution.width = width;
    this._currentResolution.height = height;
    RenderTexture renderTexture = new RenderTexture(new RenderTextureDescriptor(this._currentResolution.width, this._currentResolution.height, RenderTextureFormat.Default, 16 /*0x10*/, 0));
    renderTexture.name = "LightingCam_Mask" + (object) Time.frameCount;
    StencilLighting_MaskEffect._lightingRenderTexture = renderTexture;
  }

  public void OnDestroy() => this.DestroyRenderTexture();

  private void DestroyRenderTexture()
  {
    if (!((Object) StencilLighting_MaskEffect._lightingRenderTexture != (Object) null))
      return;
    StencilLighting_MaskEffect._lightingRenderTexture.Release();
    StencilLighting_MaskEffect._lightingRenderTexture.DiscardContents();
    Object.Destroy((Object) StencilLighting_MaskEffect._lightingRenderTexture);
    StencilLighting_MaskEffect._lightingRenderTexture = (RenderTexture) null;
  }

  [ImageEffectOpaque]
  protected void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (StencilLighting_MaskEffect.DisableRender)
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
