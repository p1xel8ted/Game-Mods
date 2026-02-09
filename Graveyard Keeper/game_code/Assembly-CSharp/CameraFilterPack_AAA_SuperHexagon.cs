// Decompiled with JetBrains decompiler
// Type: CameraFilterPack_AAA_SuperHexagon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Camera Filter Pack/AAA/Super Hexagon")]
[ExecuteInEditMode]
public class CameraFilterPack_AAA_SuperHexagon : MonoBehaviour
{
  public Shader SCShader;
  [Range(0.0f, 1f)]
  public float _AlphaHexa = 1f;
  public float TimeX = 1f;
  public Vector4 ScreenResolution;
  public Material SCMaterial;
  [Range(0.2f, 10f)]
  public float HexaSize = 2.5f;
  public float _BorderSize = 1f;
  public Color _BorderColor = new Color(0.75f, 0.75f, 1f, 1f);
  public Color _HexaColor = new Color(0.0f, 0.5f, 1f, 1f);
  public float _SpotSize = 2.5f;
  public Vector2 center = new Vector2(0.5f, 0.5f);
  public float Radius = 0.25f;

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
    this.SCShader = Shader.Find("CameraFilterPack/AAA_Super_Hexagon");
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
      this.material.SetFloat("_Value", this.HexaSize);
      this.material.SetFloat("_PositionX", this.center.x);
      this.material.SetFloat("_PositionY", this.center.y);
      this.material.SetFloat("_Radius", this.Radius);
      this.material.SetFloat("_BorderSize", this._BorderSize);
      this.material.SetColor("_BorderColor", this._BorderColor);
      this.material.SetColor("_HexaColor", this._HexaColor);
      this.material.SetFloat("_AlphaHexa", this._AlphaHexa);
      this.material.SetFloat("_SpotSize", this._SpotSize);
      this.material.SetVector("_ScreenResolution", new Vector4((float) sourceTexture.width, (float) sourceTexture.height, 0.0f, 0.0f));
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
