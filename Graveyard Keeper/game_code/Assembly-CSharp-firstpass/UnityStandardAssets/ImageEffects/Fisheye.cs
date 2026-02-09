// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.Fisheye
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/Displacement/Fisheye")]
[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
public class Fisheye : PostEffectsBase
{
  [Range(0.0f, 1.5f)]
  public float strengthX = 0.05f;
  [Range(0.0f, 1.5f)]
  public float strengthY = 0.05f;
  public Shader fishEyeShader;
  public Material fisheyeMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.fisheyeMaterial = this.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      float num1 = 5f / 32f;
      float num2 = (float) ((double) source.width * 1.0 / ((double) source.height * 1.0));
      this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num1, this.strengthY * num1, this.strengthX * num2 * num1, this.strengthY * num1));
      Graphics.Blit((Texture) source, destination, this.fisheyeMaterial);
    }
  }
}
