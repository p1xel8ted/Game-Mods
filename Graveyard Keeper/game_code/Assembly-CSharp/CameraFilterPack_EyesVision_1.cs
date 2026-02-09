// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_EyesVision_1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/Vision/Eyes 1")]
[ExecuteInEditMode]
public class CameraFilterPack_EyesVision_1 : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  [Range(1f, 32f)]
  public float _EyeWave = 15f;
  [Range(0.0f, 10f)]
  public float _EyeSpeed = 1f;
  [Range(0.0f, 8f)]
  public float _EyeMove = 2f;
  [Range(0.0f, 1f)]
  public float _EyeBlink = 1f;
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
    this.Texture2 = Resources.Load("CameraFilterPack_eyes_vision_1") as Texture2D;
    this.SCShader = Shader.Find("CameraFilterPack/EyesVision_1");
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
      this.material.SetFloat("_Value", this._EyeWave);
      this.material.SetFloat("_Value2", this._EyeSpeed);
      this.material.SetFloat("_Value3", this._EyeMove);
      this.material.SetFloat("_Value4", this._EyeBlink);
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
