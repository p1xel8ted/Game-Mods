// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.MotionBlur
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")]
public class MotionBlur : ImageEffectBase
{
  [Range(0.0f, 0.92f)]
  public float blurAmount = 0.8f;
  public bool extraBlur;
  public RenderTexture accumTexture;

  public override void Start()
  {
    if (!SystemInfo.supportsRenderTextures)
      this.enabled = false;
    else
      base.Start();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Object.DestroyImmediate((Object) this.accumTexture);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this.accumTexture == (Object) null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
    {
      Object.DestroyImmediate((Object) this.accumTexture);
      this.accumTexture = new RenderTexture(source.width, source.height, 0);
      this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
      Graphics.Blit((Texture) source, this.accumTexture);
    }
    if (this.extraBlur)
    {
      RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
      this.accumTexture.MarkRestoreExpected();
      Graphics.Blit((Texture) this.accumTexture, temporary);
      Graphics.Blit((Texture) temporary, this.accumTexture);
      RenderTexture.ReleaseTemporary(temporary);
    }
    this.blurAmount = Mathf.Clamp(this.blurAmount, 0.0f, 0.92f);
    this.material.SetTexture("_MainTex", (Texture) this.accumTexture);
    this.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
    this.accumTexture.MarkRestoreExpected();
    Graphics.Blit((Texture) source, this.accumTexture, this.material);
    Graphics.Blit((Texture) this.accumTexture, destination);
  }
}
