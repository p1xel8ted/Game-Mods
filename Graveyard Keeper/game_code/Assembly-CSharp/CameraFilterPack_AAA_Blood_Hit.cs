// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_AAA_Blood_Hit
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Blood_Hit")]
public class CameraFilterPack_AAA_Blood_Hit : MonoBehaviour
{
  public Shader SCShader;
  public float TimeX = 1f;
  [Range(0.0f, 1f)]
  public float Hit_Left = 1f;
  [Range(0.0f, 1f)]
  public float Hit_Up;
  [Range(0.0f, 1f)]
  public float Hit_Right;
  [Range(0.0f, 1f)]
  public float Hit_Down;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Left;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Up;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Right;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Down;
  [Range(0.0f, 1f)]
  public float Hit_Full;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Full_1;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Full_2;
  [Range(0.0f, 1f)]
  public float Blood_Hit_Full_3;
  [Range(0.0f, 1f)]
  public float LightReflect = 0.5f;
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
    this.Texture2 = Resources.Load("CameraFilterPack_AAA_Blood_Hit1") as Texture2D;
    this.SCShader = Shader.Find("CameraFilterPack/AAA_Blood_Hit");
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
      this.material.SetFloat("_Value2", Mathf.Clamp(this.Hit_Left, 0.0f, 1f));
      this.material.SetFloat("_Value3", Mathf.Clamp(this.Hit_Up, 0.0f, 1f));
      this.material.SetFloat("_Value4", Mathf.Clamp(this.Hit_Right, 0.0f, 1f));
      this.material.SetFloat("_Value5", Mathf.Clamp(this.Hit_Down, 0.0f, 1f));
      this.material.SetFloat("_Value6", Mathf.Clamp(this.Blood_Hit_Left, 0.0f, 1f));
      this.material.SetFloat("_Value7", Mathf.Clamp(this.Blood_Hit_Up, 0.0f, 1f));
      this.material.SetFloat("_Value8", Mathf.Clamp(this.Blood_Hit_Right, 0.0f, 1f));
      this.material.SetFloat("_Value9", Mathf.Clamp(this.Blood_Hit_Down, 0.0f, 1f));
      this.material.SetFloat("_Value10", Mathf.Clamp(this.Hit_Full, 0.0f, 1f));
      this.material.SetFloat("_Value11", Mathf.Clamp(this.Blood_Hit_Full_1, 0.0f, 1f));
      this.material.SetFloat("_Value12", Mathf.Clamp(this.Blood_Hit_Full_2, 0.0f, 1f));
      this.material.SetFloat("_Value13", Mathf.Clamp(this.Blood_Hit_Full_3, 0.0f, 1f));
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
