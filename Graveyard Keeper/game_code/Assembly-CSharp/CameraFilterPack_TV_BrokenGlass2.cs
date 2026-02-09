// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_TV_BrokenGlass2
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Broken Glass2")]
public class CameraFilterPack_TV_BrokenGlass2 : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  [Range(0.0f, 1f)]
  public float Bullet_1;
  [Range(0.0f, 1f)]
  public float Bullet_2;
  [Range(0.0f, 1f)]
  public float Bullet_3;
  [Range(0.0f, 1f)]
  public float Bullet_4 = 1f;
  [Range(0.0f, 1f)]
  public float Bullet_5;
  [Range(0.0f, 1f)]
  public float Bullet_6;
  [Range(0.0f, 1f)]
  public float Bullet_7;
  [Range(0.0f, 1f)]
  public float Bullet_8;
  [Range(0.0f, 1f)]
  public float Bullet_9;
  [Range(0.0f, 1f)]
  public float Bullet_10;
  [Range(0.0f, 1f)]
  public float Bullet_11;
  [Range(0.0f, 1f)]
  public float Bullet_12;
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
    this.Texture2 = Resources.Load("CameraFilterPack_TV_BrokenGlass_2") as Texture2D;
    this.SCShader = Shader.Find("CameraFilterPack/TV_BrokenGlass2");
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
      if ((double) this.Bullet_1 < 0.0)
        this.Bullet_1 = 0.0f;
      if ((double) this.Bullet_2 < 0.0)
        this.Bullet_2 = 0.0f;
      if ((double) this.Bullet_3 < 0.0)
        this.Bullet_3 = 0.0f;
      if ((double) this.Bullet_4 < 0.0)
        this.Bullet_4 = 0.0f;
      if ((double) this.Bullet_5 < 0.0)
        this.Bullet_5 = 0.0f;
      if ((double) this.Bullet_6 < 0.0)
        this.Bullet_6 = 0.0f;
      if ((double) this.Bullet_7 < 0.0)
        this.Bullet_7 = 0.0f;
      if ((double) this.Bullet_8 < 0.0)
        this.Bullet_8 = 0.0f;
      if ((double) this.Bullet_9 < 0.0)
        this.Bullet_9 = 0.0f;
      if ((double) this.Bullet_10 < 0.0)
        this.Bullet_10 = 0.0f;
      if ((double) this.Bullet_11 < 0.0)
        this.Bullet_11 = 0.0f;
      if ((double) this.Bullet_12 < 0.0)
        this.Bullet_12 = 0.0f;
      if ((double) this.Bullet_1 > 1.0)
        this.Bullet_1 = 1f;
      if ((double) this.Bullet_2 > 1.0)
        this.Bullet_2 = 1f;
      if ((double) this.Bullet_3 > 1.0)
        this.Bullet_3 = 1f;
      if ((double) this.Bullet_4 > 1.0)
        this.Bullet_4 = 1f;
      if ((double) this.Bullet_5 > 1.0)
        this.Bullet_5 = 1f;
      if ((double) this.Bullet_6 > 1.0)
        this.Bullet_6 = 1f;
      if ((double) this.Bullet_7 > 1.0)
        this.Bullet_7 = 1f;
      if ((double) this.Bullet_8 > 1.0)
        this.Bullet_8 = 1f;
      if ((double) this.Bullet_9 > 1.0)
        this.Bullet_9 = 1f;
      if ((double) this.Bullet_10 > 1.0)
        this.Bullet_10 = 1f;
      if ((double) this.Bullet_11 > 1.0)
        this.Bullet_11 = 1f;
      if ((double) this.Bullet_12 > 1.0)
        this.Bullet_12 = 1f;
      this.material.SetFloat("_Bullet_1", this.Bullet_1);
      this.material.SetFloat("_Bullet_2", this.Bullet_2);
      this.material.SetFloat("_Bullet_3", this.Bullet_3);
      this.material.SetFloat("_Bullet_4", this.Bullet_4);
      this.material.SetFloat("_Bullet_5", this.Bullet_5);
      this.material.SetFloat("_Bullet_6", this.Bullet_6);
      this.material.SetFloat("_Bullet_7", this.Bullet_7);
      this.material.SetFloat("_Bullet_8", this.Bullet_8);
      this.material.SetFloat("_Bullet_9", this.Bullet_9);
      this.material.SetFloat("_Bullet_10", this.Bullet_10);
      this.material.SetFloat("_Bullet_11", this.Bullet_11);
      this.material.SetFloat("_Bullet_12", this.Bullet_12);
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
