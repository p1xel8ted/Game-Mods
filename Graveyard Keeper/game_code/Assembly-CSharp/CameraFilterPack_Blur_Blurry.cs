// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_Blur_Blurry
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Blurry")]
public class CameraFilterPack_Blur_Blurry : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(0.0f, 20f)]
  public float Amount = 2f;
  [Range(1f, 8f)]
  public int FastFilter = 2;

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
    this.SCShader = Shader.Find("CameraFilterPack/Blur_Blurry");
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
      this.material.SetVector("_ScreenResolution", (Vector4) new Vector2((float) (Screen.width / fastFilter), (float) (Screen.height / fastFilter)));
      int width = sourceTexture.width / fastFilter;
      int height = sourceTexture.height / fastFilter;
      if (this.FastFilter > 1)
      {
        RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
        temporary.filterMode = FilterMode.Trilinear;
        Graphics.Blit((Texture) sourceTexture, temporary, this.material);
        Graphics.Blit((Texture) temporary, destTexture);
        RenderTexture.ReleaseTemporary(temporary);
      }
      else
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
