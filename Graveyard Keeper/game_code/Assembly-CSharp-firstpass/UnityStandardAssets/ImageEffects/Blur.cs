// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.Blur
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Blur/Blur")]
public class Blur : MonoBehaviour
{
  [Range(0.0f, 10f)]
  public int iterations = 3;
  [Range(0.0f, 1f)]
  public float blurSpread = 0.6f;
  public Shader blurShader;
  public static Material m_Material;

  public Material material
  {
    get
    {
      if ((Object) Blur.m_Material == (Object) null)
      {
        Blur.m_Material = new Material(this.blurShader);
        Blur.m_Material.hideFlags = HideFlags.DontSave;
      }
      return Blur.m_Material;
    }
  }

  public void OnDisable()
  {
    if (!(bool) (Object) Blur.m_Material)
      return;
    Object.DestroyImmediate((Object) Blur.m_Material);
  }

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.blurShader && this.material.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  public void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    int width = source.width / 4;
    int height = source.height / 4;
    RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
    this.DownSample4x(source, renderTexture);
    for (int iteration = 0; iteration < this.iterations; ++iteration)
    {
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
      this.FourTapCone(renderTexture, temporary, iteration);
      RenderTexture.ReleaseTemporary(renderTexture);
      renderTexture = temporary;
    }
    Graphics.Blit((Texture) renderTexture, destination);
    RenderTexture.ReleaseTemporary(renderTexture);
  }
}
