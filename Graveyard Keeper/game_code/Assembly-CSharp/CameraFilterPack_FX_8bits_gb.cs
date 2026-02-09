// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_FX_8bits_gb
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Pixel/8bits_gb")]
[ExecuteInEditMode]
public class CameraFilterPack_FX_8bits_gb : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Material SCMaterial;
  [Range(-1f, 1f)]
  public float Brightness;

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
    this.SCShader = Shader.Find("CameraFilterPack/FX_8bits_gb");
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
      if ((double) this.Brightness == 0.0)
        this.Brightness = 1f / 1000f;
      this.material.SetFloat("_Distortion", this.Brightness);
      RenderTexture temporary = RenderTexture.GetTemporary(160 /*0xA0*/, 144 /*0x90*/, 0);
      Graphics.Blit((Texture) sourceTexture, temporary, this.material);
      temporary.filterMode = FilterMode.Point;
      Graphics.Blit((Texture) temporary, destTexture);
      RenderTexture.ReleaseTemporary(temporary);
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
