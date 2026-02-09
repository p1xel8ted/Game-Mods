// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_AAA_WaterDrop
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/AAA/WaterDrop")]
[ExecuteInEditMode]
public class CameraFilterPack_AAA_WaterDrop : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  [Range(8f, 64f)]
  public float Distortion = 8f;
  [Range(0.0f, 7f)]
  public float SizeX = 1f;
  [Range(0.0f, 7f)]
  public float SizeY = 0.5f;
  [Range(0.0f, 10f)]
  public float Speed = 1f;
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
    this.Texture2 = Resources.Load("CameraFilterPack_WaterDrop") as Texture2D;
    this.SCShader = Shader.Find("CameraFilterPack/AAA_WaterDrop");
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
      this.material.SetFloat("_Distortion", this.Distortion);
      this.material.SetFloat("_SizeX", this.SizeX);
      this.material.SetFloat("_SizeY", this.SizeY);
      this.material.SetFloat("_Speed", this.Speed);
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
