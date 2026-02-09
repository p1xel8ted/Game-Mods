// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.ColorCorrectionLookup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")]
public class ColorCorrectionLookup : PostEffectsBase
{
  public Shader shader;
  public Material material;
  public Texture3D converted3DLut;
  public string basedOnTempTex = "";

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.material = this.CheckShaderAndCreateMaterial(this.shader, this.material);
    if (!this.isSupported || !SystemInfo.supports3DTextures)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this.material)
      return;
    Object.DestroyImmediate((Object) this.material);
    this.material = (Material) null;
  }

  public void OnDestroy()
  {
    if ((bool) (Object) this.converted3DLut)
      Object.DestroyImmediate((Object) this.converted3DLut);
    this.converted3DLut = (Texture3D) null;
  }

  public void SetIdentityLut()
  {
    int num1 = 16 /*0x10*/;
    Color[] colors = new Color[num1 * num1 * num1];
    float num2 = (float) (1.0 / (1.0 * (double) num1 - 1.0));
    for (int index1 = 0; index1 < num1; ++index1)
    {
      for (int index2 = 0; index2 < num1; ++index2)
      {
        for (int index3 = 0; index3 < num1; ++index3)
          colors[index1 + index2 * num1 + index3 * num1 * num1] = new Color((float) index1 * 1f * num2, (float) index2 * 1f * num2, (float) index3 * 1f * num2, 1f);
      }
    }
    if ((bool) (Object) this.converted3DLut)
      Object.DestroyImmediate((Object) this.converted3DLut);
    this.converted3DLut = new Texture3D(num1, num1, num1, TextureFormat.ARGB32, false);
    this.converted3DLut.SetPixels(colors);
    this.converted3DLut.Apply();
    this.basedOnTempTex = "";
  }

  public bool ValidDimensions(Texture2D tex2d)
  {
    return (bool) (Object) tex2d && tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float) tex2d.width));
  }

  public void Convert(Texture2D temp2DTex, string path)
  {
    if ((bool) (Object) temp2DTex)
    {
      int num1 = temp2DTex.width * temp2DTex.height;
      int height = temp2DTex.height;
      if (!this.ValidDimensions(temp2DTex))
      {
        Debug.LogWarning((object) $"The given 2D texture {temp2DTex.name} cannot be used as a 3D LUT.");
        this.basedOnTempTex = "";
      }
      else
      {
        Color[] pixels = temp2DTex.GetPixels();
        Color[] colors = new Color[pixels.Length];
        for (int index1 = 0; index1 < height; ++index1)
        {
          for (int index2 = 0; index2 < height; ++index2)
          {
            for (int index3 = 0; index3 < height; ++index3)
            {
              int num2 = height - index2 - 1;
              colors[index1 + index2 * height + index3 * height * height] = pixels[index3 * height + index1 + num2 * height * height];
            }
          }
        }
        if ((bool) (Object) this.converted3DLut)
          Object.DestroyImmediate((Object) this.converted3DLut);
        this.converted3DLut = new Texture3D(height, height, height, TextureFormat.ARGB32, false);
        this.converted3DLut.SetPixels(colors);
        this.converted3DLut.Apply();
        this.basedOnTempTex = path;
      }
    }
    else
      Debug.LogError((object) "Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources() || !SystemInfo.supports3DTextures)
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((Object) this.converted3DLut == (Object) null)
        this.SetIdentityLut();
      int width = this.converted3DLut.width;
      this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
      this.material.SetFloat("_Scale", (float) (width - 1) / (1f * (float) width));
      this.material.SetFloat("_Offset", (float) (1.0 / (2.0 * (double) width)));
      this.material.SetTexture("_ClutTex", (Texture) this.converted3DLut);
      Graphics.Blit((Texture) source, destination, this.material, QualitySettings.activeColorSpace == ColorSpace.Linear ? 1 : 0);
    }
  }
}
