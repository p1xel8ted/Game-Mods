// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_Colors_HSV
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Colors/HSV")]
[ExecuteInEditMode]
public class CameraFilterPack_Colors_HSV : MonoBehaviour
{
  public Shader SCShader;
  [Range(0.0f, 360f)]
  public float _HueShift = 180f;
  [Range(-32f, 32f)]
  public float _Saturation = 1f;
  [Range(-32f, 32f)]
  public float _ValueBrightness = 1f;
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
    if (SystemInfo.supportsImageEffects)
      return;
    this.enabled = false;
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if ((Object) this.SCShader != (Object) null)
    {
      this.material.SetFloat("_HueShift", this._HueShift);
      this.material.SetFloat("_Sat", this._Saturation);
      this.material.SetFloat("_Val", this._ValueBrightness);
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
