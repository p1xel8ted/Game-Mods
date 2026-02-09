// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_3D_Shield
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/3D/Shield")]
[ExecuteInEditMode]
public class CameraFilterPack_3D_Shield : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(0.0f, 100f)]
  public float _FixDistance = 1.5f;
  [Range(-0.99f, 0.99f)]
  public float _Distance = 0.4f;
  [Range(0.0f, 0.5f)]
  public float _Size = 0.5f;
  [Range(0.0f, 1f)]
  public float _FadeShield = 0.75f;
  [Range(-0.2f, 0.2f)]
  public float LightIntensity = 0.025f;
  public bool AutoAnimatedNear;
  [Range(-5f, 5f)]
  public float AutoAnimatedNearSpeed = 0.5f;
  [Range(0.0f, 10f)]
  public float Speed = 0.2f;
  [Range(0.0f, 10f)]
  public float Speed_X = 0.2f;
  [Range(0.0f, 1f)]
  public float Speed_Y = 0.3f;
  [Range(0.0f, 10f)]
  public float Intensity = 2.4f;
  public static Color ChangeColorRGB;

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
    this.SCShader = Shader.Find("CameraFilterPack/3D_Shield");
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
      if (this.AutoAnimatedNear)
      {
        this._Distance += Time.deltaTime * this.AutoAnimatedNearSpeed;
        if ((double) this._Distance > 1.0)
          this._Distance = -1f;
        if ((double) this._Distance < -1.0)
          this._Distance = 1f;
        this.material.SetFloat("_Near", this._Distance);
      }
      else
        this.material.SetFloat("_Near", this._Distance);
      this.material.SetFloat("_Far", this._Size);
      this.material.SetFloat("_FixDistance", this._FixDistance);
      this.material.SetFloat("_LightIntensity", this.LightIntensity * 64f);
      this.material.SetFloat("_FadeShield", this._FadeShield);
      this.material.SetFloat("_Value", this.Speed);
      this.material.SetFloat("_Value2", this.Speed_X);
      this.material.SetFloat("_Value3", this.Speed_Y);
      this.material.SetFloat("_Value4", this.Intensity);
      this.material.SetFloat("_FarCamera", 1000f / this.GetComponent<Camera>().farClipPlane);
      this.material.SetVector("_ScreenResolution", new Vector4((float) sourceTexture.width, (float) sourceTexture.height, 0.0f, 0.0f));
      this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
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
