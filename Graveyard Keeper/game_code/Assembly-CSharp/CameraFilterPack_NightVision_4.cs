// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_NightVision_4
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 4")]
[ExecuteInEditMode]
public class CameraFilterPack_NightVision_4 : MonoBehaviour
{
  public string ShaderName = "CameraFilterPack/NightVision_4";
  public Shader SCShader;
  [Range(0.0f, 1f)]
  public float FadeFX = 1f;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  public float[] Matrix9;

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

  public void ChangeFilters()
  {
    this.Matrix9 = new float[12]
    {
      200f,
      -200f,
      -200f,
      195f,
      4f,
      -160f,
      200f,
      -200f,
      -200f,
      -200f,
      10f,
      -200f
    };
  }

  public void Start()
  {
    this.ChangeFilters();
    this.SCShader = Shader.Find(this.ShaderName);
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
      this.material.SetFloat("_Red_R", this.Matrix9[0] / 100f);
      this.material.SetFloat("_Red_G", this.Matrix9[1] / 100f);
      this.material.SetFloat("_Red_B", this.Matrix9[2] / 100f);
      this.material.SetFloat("_Green_R", this.Matrix9[3] / 100f);
      this.material.SetFloat("_Green_G", this.Matrix9[4] / 100f);
      this.material.SetFloat("_Green_B", this.Matrix9[5] / 100f);
      this.material.SetFloat("_Blue_R", this.Matrix9[6] / 100f);
      this.material.SetFloat("_Blue_G", this.Matrix9[7] / 100f);
      this.material.SetFloat("_Blue_B", this.Matrix9[8] / 100f);
      this.material.SetFloat("_Red_C", this.Matrix9[9] / 100f);
      this.material.SetFloat("_Green_C", this.Matrix9[10] / 100f);
      this.material.SetFloat("_Blue_C", this.Matrix9[11] / 100f);
      this.material.SetFloat("_FadeFX", this.FadeFX);
      this.material.SetVector("_ScreenResolution", new Vector4((float) sourceTexture.width, (float) sourceTexture.height, 0.0f, 0.0f));
      Graphics.Blit((Texture) sourceTexture, destTexture, this.material);
    }
    else
      Graphics.Blit((Texture) sourceTexture, destTexture);
  }

  public void OnValidate() => this.ChangeFilters();

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
