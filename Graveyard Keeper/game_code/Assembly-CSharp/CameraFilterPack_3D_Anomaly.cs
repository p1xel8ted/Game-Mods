// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_3D_Anomaly
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Anomaly")]
public class CameraFilterPack_3D_Anomaly : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(0.0f, 100f)]
  public float _FixDistance = 23f;
  [Range(-0.5f, 0.99f)]
  public float Anomaly_Near = 0.045f;
  [Range(0.0f, 1f)]
  public float Anomaly_Far = 0.11f;
  [Range(0.0f, 2f)]
  public float Intensity = 1f;
  [Range(0.0f, 1f)]
  public float AnomalyWithoutObject = 1f;
  [Range(0.1f, 1f)]
  public float Anomaly_Distortion = 0.25f;
  [Range(4f, 64f)]
  public float Anomaly_Distortion_Size = 12f;
  [Range(-4f, 8f)]
  public float Anomaly_Intensity = 2f;

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
    this.SCShader = Shader.Find("CameraFilterPack/3D_Anomaly");
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
      this.material.SetFloat("_Value2", this.Intensity);
      this.material.SetFloat("Anomaly_Distortion", this.Anomaly_Distortion);
      this.material.SetFloat("Anomaly_Distortion_Size", this.Anomaly_Distortion_Size);
      this.material.SetFloat("Anomaly_Intensity", this.Anomaly_Intensity);
      this.material.SetFloat("_FixDistance", this._FixDistance);
      this.material.SetFloat("Anomaly_Near", this.Anomaly_Near);
      this.material.SetFloat("Anomaly_Far", this.Anomaly_Far);
      this.material.SetFloat("Anomaly_With_Obj", this.AnomalyWithoutObject);
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
