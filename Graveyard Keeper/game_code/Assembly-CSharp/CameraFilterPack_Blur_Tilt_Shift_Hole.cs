// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_Blur_Tilt_Shift_Hole
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Blur/Tilt_Shift_Hole")]
[ExecuteInEditMode]
public class CameraFilterPack_Blur_Tilt_Shift_Hole : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(0.0f, 20f)]
  public float Amount = 3f;
  [Range(2f, 16f)]
  public int FastFilter = 8;
  [Range(0.0f, 1f)]
  public float Smooth = 0.5f;
  [Range(0.0f, 1f)]
  public float Size = 0.2f;
  [Range(-1f, 1f)]
  public float PositionX = 0.5f;
  [Range(-1f, 1f)]
  public float PositionY = 0.5f;

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
    this.SCShader = Shader.Find("CameraFilterPack/BlurTiltShift_Hole");
    if (SystemInfo.supportsImageEffects)
      return;
    this.enabled = false;
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if ((Object) this.SCShader != (Object) null)
    {
      int fastFilter = this.FastFilter;
      this.TimeX += Time.deltaTime;
      if ((double) this.TimeX > 100.0)
        this.TimeX = 0.0f;
      this.material.SetFloat("_TimeX", this.TimeX);
      this.material.SetFloat("_Amount", this.Amount);
      this.material.SetFloat("_Value1", this.Smooth);
      this.material.SetFloat("_Value2", this.Size);
      this.material.SetFloat("_Value3", this.PositionX);
      this.material.SetFloat("_Value4", this.PositionY);
      int width = sourceTexture.width / fastFilter;
      int height = sourceTexture.height / fastFilter;
      if (this.FastFilter > 1)
      {
        RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0);
        RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
        temporary1.filterMode = FilterMode.Trilinear;
        Graphics.Blit((Texture) sourceTexture, temporary1, this.material, 2);
        Graphics.Blit((Texture) temporary1, temporary2, this.material, 0);
        this.material.SetFloat("_Amount", this.Amount * 2f);
        Graphics.Blit((Texture) temporary2, temporary1, this.material, 2);
        Graphics.Blit((Texture) temporary1, temporary2, this.material, 0);
        this.material.SetTexture("_MainTex2", (Texture) temporary2);
        RenderTexture.ReleaseTemporary(temporary1);
        RenderTexture.ReleaseTemporary(temporary2);
        Graphics.Blit((Texture) sourceTexture, destTexture, this.material, 1);
      }
      else
        Graphics.Blit((Texture) sourceTexture, destTexture, this.material, 0);
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
