// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_TV_BrokenGlass
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Broken Glass")]
public class CameraFilterPack_TV_BrokenGlass : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  [Range(0.0f, 128f)]
  public float Broken_Small;
  [Range(0.0f, 128f)]
  public float Broken_Medium;
  [Range(0.0f, 128f)]
  public float Broken_High;
  [Range(0.0f, 128f)]
  public float Broken_Big = 1f;
  [Range(0.0f, 0.004f)]
  public float LightReflect = 1f / 500f;
  public Material SCMaterial;
  public Texture2D Texture2;

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
    this.Texture2 = Resources.Load("CameraFilterPack_TV_BrokenGlass1") as Texture2D;
    this.SCShader = Shader.Find("CameraFilterPack/TV_BrokenGlass");
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
      this.material.SetFloat("_Value", this.LightReflect);
      this.material.SetFloat("_Value2", this.Broken_Small);
      this.material.SetFloat("_Value3", this.Broken_Medium);
      this.material.SetFloat("_Value4", this.Broken_High);
      this.material.SetFloat("_Value5", this.Broken_Big);
      this.material.SetTexture("_MainTex2", (Texture) this.Texture2);
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
