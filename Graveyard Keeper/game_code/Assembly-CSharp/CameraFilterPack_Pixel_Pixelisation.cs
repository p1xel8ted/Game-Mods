// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_Pixel_Pixelisation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Pixel/Pixelisation")]
[ExecuteInEditMode]
public class CameraFilterPack_Pixel_Pixelisation : MonoBehaviour
{
  public Shader SCShader;
  [Range(0.6f, 120f)]
  public float _Pixelisation = 8f;
  [Range(0.6f, 120f)]
  public float _SizeX = 1f;
  [Range(0.6f, 120f)]
  public float _SizeY = 1f;
  public Material SCMaterial;

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
    this.SCShader = Shader.Find("CameraFilterPack/Pixel_Pixelisation");
    if (SystemInfo.supportsImageEffects)
      return;
    this.enabled = false;
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if ((Object) this.SCShader != (Object) null)
    {
      this.material.SetFloat("_Val", this._Pixelisation);
      this.material.SetFloat("_Val2", this._SizeX);
      this.material.SetFloat("_Val3", this._SizeY);
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
