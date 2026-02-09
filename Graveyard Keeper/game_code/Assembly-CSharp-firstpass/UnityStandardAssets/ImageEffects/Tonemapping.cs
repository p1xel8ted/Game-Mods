// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.Tonemapping
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/Color Adjustments/Tonemapping")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class Tonemapping : PostEffectsBase
{
  public Tonemapping.TonemapperType type = Tonemapping.TonemapperType.Photographic;
  public Tonemapping.AdaptiveTexSize adaptiveTextureSize = Tonemapping.AdaptiveTexSize.Square256;
  public AnimationCurve remapCurve;
  public Texture2D curveTex;
  public float exposureAdjustment = 1.5f;
  public float middleGrey = 0.4f;
  public float white = 2f;
  public float adaptionSpeed = 1.5f;
  public Shader tonemapper;
  public bool validRenderTextureFormat = true;
  public Material tonemapMaterial;
  public RenderTexture rt;
  public RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;

  public override bool CheckResources()
  {
    this.CheckSupport(false, true);
    this.tonemapMaterial = this.CheckShaderAndCreateMaterial(this.tonemapper, this.tonemapMaterial);
    if (!(bool) (Object) this.curveTex && this.type == Tonemapping.TonemapperType.UserCurve)
    {
      this.curveTex = new Texture2D(256 /*0x0100*/, 1, TextureFormat.ARGB32, false, true);
      this.curveTex.filterMode = FilterMode.Bilinear;
      this.curveTex.wrapMode = TextureWrapMode.Clamp;
      this.curveTex.hideFlags = HideFlags.DontSave;
    }
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public float UpdateCurve()
  {
    float num1 = 1f;
    if (this.remapCurve.keys.Length < 1)
      this.remapCurve = new AnimationCurve(new Keyframe[2]
      {
        new Keyframe(0.0f, 0.0f),
        new Keyframe(2f, 1f)
      });
    if (this.remapCurve != null)
    {
      if (this.remapCurve.length > 0)
        num1 = this.remapCurve[this.remapCurve.length - 1].time;
      for (float num2 = 0.0f; (double) num2 <= 1.0; num2 += 0.003921569f)
      {
        float num3 = this.remapCurve.Evaluate(num2 * 1f * num1);
        this.curveTex.SetPixel((int) Mathf.Floor(num2 * (float) byte.MaxValue), 0, new Color(num3, num3, num3));
      }
      this.curveTex.Apply();
    }
    return 1f / num1;
  }

  public void OnDisable()
  {
    if ((bool) (Object) this.rt)
    {
      Object.DestroyImmediate((Object) this.rt);
      this.rt = (RenderTexture) null;
    }
    if ((bool) (Object) this.tonemapMaterial)
    {
      Object.DestroyImmediate((Object) this.tonemapMaterial);
      this.tonemapMaterial = (Material) null;
    }
    if (!(bool) (Object) this.curveTex)
      return;
    Object.DestroyImmediate((Object) this.curveTex);
    this.curveTex = (Texture2D) null;
  }

  public bool CreateInternalRenderTexture()
  {
    if ((bool) (Object) this.rt)
      return false;
    this.rtFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.ARGBHalf;
    this.rt = new RenderTexture(1, 1, 0, this.rtFormat);
    this.rt.hideFlags = HideFlags.DontSave;
    return true;
  }

  [ImageEffectTransformsToLDR]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.exposureAdjustment = (double) this.exposureAdjustment < 1.0 / 1000.0 ? 1f / 1000f : this.exposureAdjustment;
      if (this.type == Tonemapping.TonemapperType.UserCurve)
      {
        this.tonemapMaterial.SetFloat("_RangeScale", this.UpdateCurve());
        this.tonemapMaterial.SetTexture("_Curve", (Texture) this.curveTex);
        Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 4);
      }
      else if (this.type == Tonemapping.TonemapperType.SimpleReinhard)
      {
        this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
        Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 6);
      }
      else if (this.type == Tonemapping.TonemapperType.Hable)
      {
        this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
        Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 5);
      }
      else if (this.type == Tonemapping.TonemapperType.Photographic)
      {
        this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
        Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 8);
      }
      else if (this.type == Tonemapping.TonemapperType.OptimizedHejiDawson)
      {
        this.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5f * this.exposureAdjustment);
        Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 7);
      }
      else
      {
        bool internalRenderTexture = this.CreateInternalRenderTexture();
        RenderTexture temporary = RenderTexture.GetTemporary((int) this.adaptiveTextureSize, (int) this.adaptiveTextureSize, 0, this.rtFormat);
        Graphics.Blit((Texture) source, temporary);
        int length = (int) Mathf.Log((float) temporary.width * 1f, 2f);
        int num = 2;
        RenderTexture[] renderTextureArray = new RenderTexture[length];
        for (int index = 0; index < length; ++index)
        {
          renderTextureArray[index] = RenderTexture.GetTemporary(temporary.width / num, temporary.width / num, 0, this.rtFormat);
          num *= 2;
        }
        RenderTexture source1 = renderTextureArray[length - 1];
        Graphics.Blit((Texture) temporary, renderTextureArray[0], this.tonemapMaterial, 1);
        if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
        {
          for (int index = 0; index < length - 1; ++index)
          {
            Graphics.Blit((Texture) renderTextureArray[index], renderTextureArray[index + 1], this.tonemapMaterial, 9);
            source1 = renderTextureArray[index + 1];
          }
        }
        else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
        {
          for (int index = 0; index < length - 1; ++index)
          {
            Graphics.Blit((Texture) renderTextureArray[index], renderTextureArray[index + 1]);
            source1 = renderTextureArray[index + 1];
          }
        }
        this.adaptionSpeed = (double) this.adaptionSpeed < 1.0 / 1000.0 ? 1f / 1000f : this.adaptionSpeed;
        this.tonemapMaterial.SetFloat("_AdaptionSpeed", this.adaptionSpeed);
        this.rt.MarkRestoreExpected();
        Graphics.Blit((Texture) source1, this.rt, this.tonemapMaterial, internalRenderTexture ? 3 : 2);
        this.middleGrey = (double) this.middleGrey < 1.0 / 1000.0 ? 1f / 1000f : this.middleGrey;
        this.tonemapMaterial.SetVector("_HdrParams", new Vector4(this.middleGrey, this.middleGrey, this.middleGrey, this.white * this.white));
        this.tonemapMaterial.SetTexture("_SmallTex", (Texture) this.rt);
        if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
          Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 0);
        else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
        {
          Graphics.Blit((Texture) source, destination, this.tonemapMaterial, 10);
        }
        else
        {
          Debug.LogError((object) "No valid adaptive tonemapper type found!");
          Graphics.Blit((Texture) source, destination);
        }
        for (int index = 0; index < length; ++index)
          RenderTexture.ReleaseTemporary(renderTextureArray[index]);
        RenderTexture.ReleaseTemporary(temporary);
      }
    }
  }

  public enum TonemapperType
  {
    SimpleReinhard,
    UserCurve,
    Hable,
    Photographic,
    OptimizedHejiDawson,
    AdaptiveReinhard,
    AdaptiveReinhardAutoWhite,
  }

  public enum AdaptiveTexSize
  {
    Square16 = 16, // 0x00000010
    Square32 = 32, // 0x00000020
    Square64 = 64, // 0x00000040
    Square128 = 128, // 0x00000080
    Square256 = 256, // 0x00000100
    Square512 = 512, // 0x00000200
    Square1024 = 1024, // 0x00000400
  }
}
