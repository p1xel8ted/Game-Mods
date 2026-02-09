// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_TV_Vignetting
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/TV/Vignetting")]
[ExecuteInEditMode]
public class CameraFilterPack_TV_Vignetting : MonoBehaviour
{
  public Shader SCShader;
  public Material SCMaterial;
  public Texture2D Vignette;
  [Range(0.0f, 1f)]
  public float Vignetting = 1f;
  [Range(0.0f, 1f)]
  public float VignettingFull;
  [Range(0.0f, 1f)]
  public float VignettingDirt;
  public Color VignettingColor = new Color(0.0f, 0.0f, 0.0f, 1f);

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
    this.SCShader = Shader.Find("CameraFilterPack/TV_Vignetting");
    this.Vignette = Resources.Load("CameraFilterPack_TV_Vignetting1") as Texture2D;
    if (SystemInfo.supportsImageEffects)
      return;
    this.enabled = false;
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if ((Object) this.SCShader != (Object) null)
    {
      this.material.SetTexture("Vignette", (Texture) this.Vignette);
      this.material.SetFloat("_Vignetting", this.Vignetting);
      this.material.SetFloat("_Vignetting2", this.VignettingFull);
      this.material.SetColor("_VignettingColor", this.VignettingColor);
      this.material.SetFloat("_VignettingDirt", this.VignettingDirt);
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
