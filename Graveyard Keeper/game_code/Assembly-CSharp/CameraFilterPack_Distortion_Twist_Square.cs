// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_Distortion_Twist_Square
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Distortion/Twist_Square")]
[ExecuteInEditMode]
public class CameraFilterPack_Distortion_Twist_Square : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(-2f, 2f)]
  public float CenterX = 0.5f;
  [Range(-2f, 2f)]
  public float CenterY = 0.5f;
  [Range(-3.14f, 3.14f)]
  public float Distortion = 0.5f;
  [Range(-2f, 2f)]
  public float Size = 0.25f;

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
    this.SCShader = Shader.Find("CameraFilterPack/Distortion_Twist_Square");
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
      this.material.SetFloat("_CenterX", this.CenterX);
      this.material.SetFloat("_CenterY", this.CenterY);
      this.material.SetFloat("_Distortion", this.Distortion);
      this.material.SetFloat("_Size", this.Size);
      this.material.SetVector("_ScreenResolution", (Vector4) new Vector2((float) Screen.width, (float) Screen.height));
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
