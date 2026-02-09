// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_Atmosphere_Fog
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Weather/Fog")]
public class CameraFilterPack_Atmosphere_Fog : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(0.0f, 1f)]
  public float _Near;
  [Range(0.0f, 1f)]
  public float _Far = 0.05f;
  public Color FogColor = new Color(0.4f, 0.4f, 0.4f, 1f);
  [Range(0.0f, 1f)]
  public float Fade = 1f;
  public static Color ChangeColorRGB;
  public Texture2D Texture2;

  public Material material
  {
    get
    {
      if ((Object) this.SCMaterial == (Object) null)
      {
        this.SCMaterial = new Material(this.SCShader);
        this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.SCMaterial;
    }
  }

  public void Start()
  {
    this.Texture2 = Resources.Load("CameraFilterPack_Atmosphere_Rain_FX") as Texture2D;
    this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Fog");
    if (SystemInfo.supportsImageEffects)
      return;
    this.enabled = false;
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if ((Object) this.SCShader != (Object) null)
    {
      this.TimeX += Time.deltaTime;
      if ((double) this.TimeX > 100.0)
        this.TimeX = 0.0f;
      this.material.SetFloat("_TimeX", this.TimeX);
      this.material.SetFloat("_DepthLevel", this.Fade);
      this.material.SetFloat("_Near", this._Near);
      this.material.SetFloat("_Far", this._Far);
      this.material.SetColor("_ColorRGB", this.FogColor);
      this.material.SetVector("_ScreenResolution", new Vector4((float) sourceTexture.width, (float) sourceTexture.height, 0.0f, 0.0f));
      this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
      this.material.SetTexture("Texture2", (Texture) this.Texture2);
      Graphics.Blit((Texture) sourceTexture, destTexture, this.material);
    }
    else
      Graphics.Blit((Texture) sourceTexture, destTexture);
  }

  public void Update()
  {
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this.SCMaterial)
      return;
    Object.DestroyImmediate((Object) this.SCMaterial);
  }
}
