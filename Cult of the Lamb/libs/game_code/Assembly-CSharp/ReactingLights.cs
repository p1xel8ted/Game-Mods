// Decompiled with JetBrains decompiler
// Type: ReactingLights
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class ReactingLights : MonoBehaviour
{
  public VideoPlayer videoSource;
  public Light[] lights;
  public Color averageColor;
  public Texture2D tex;
  public ReactingLights.VideoSide videoSide;
  public bool createTexture;

  public void Start()
  {
    this.videoSource.frameReady += new VideoPlayer.FrameReadyEventHandler(this.NewFrame);
    this.videoSource.sendFrameReadyEvents = true;
  }

  public void NewFrame(VideoPlayer vplayer, long frame)
  {
    if (!this.createTexture)
    {
      this.createTexture = true;
      switch (this.videoSide)
      {
        case ReactingLights.VideoSide.up:
          this.tex = new Texture2D(this.videoSource.texture.width / 2, 20);
          break;
        case ReactingLights.VideoSide.left:
          this.tex = new Texture2D(20, this.videoSource.texture.height / 2);
          break;
        case ReactingLights.VideoSide.right:
          this.tex = new Texture2D(20, this.videoSource.texture.height / 2);
          break;
        case ReactingLights.VideoSide.down:
          this.tex = new Texture2D(this.videoSource.texture.width / 2, 20);
          break;
        case ReactingLights.VideoSide.center:
          this.tex = new Texture2D(this.videoSource.texture.height / 2, this.videoSource.texture.height / 2);
          break;
      }
    }
    RenderTexture.active = (RenderTexture) this.videoSource.texture;
    switch (this.videoSide)
    {
      case ReactingLights.VideoSide.up:
        this.tex.ReadPixels(new Rect((float) (this.videoSource.texture.width / 2), 0.0f, (float) (this.videoSource.texture.width / 2), 20f), 0, 0);
        break;
      case ReactingLights.VideoSide.left:
        this.tex.ReadPixels(new Rect(0.0f, 0.0f, 20f, (float) (this.videoSource.texture.height / 2)), 0, 0);
        break;
      case ReactingLights.VideoSide.right:
        this.tex.ReadPixels(new Rect((float) (this.videoSource.texture.width - 20), 0.0f, 20f, (float) (this.videoSource.texture.height / 2)), 0, 0);
        break;
      case ReactingLights.VideoSide.down:
        this.tex.ReadPixels(new Rect((float) (this.videoSource.texture.width / 2), (float) (this.videoSource.texture.height - 20), (float) (this.videoSource.texture.width / 2), 20f), 0, 0);
        break;
      case ReactingLights.VideoSide.center:
        this.tex.ReadPixels(new Rect((float) (this.videoSource.texture.width / 2 - this.videoSource.texture.width / 2), (float) (this.videoSource.texture.height / 2 - this.videoSource.texture.height / 2), (float) (this.videoSource.texture.width / 2), (float) (this.videoSource.texture.height / 2)), 0, 0);
        break;
    }
    this.tex.Apply();
    this.averageColor = (Color) this.AverageColorFromTexture(this.tex);
    this.averageColor.a = 1f;
    foreach (Light light in this.lights)
      light.color = this.averageColor;
  }

  public Color32 AverageColorFromTexture(Texture2D tex)
  {
    Color32[] pixels32 = tex.GetPixels32();
    int length = pixels32.Length;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    for (int index = 0; index < length; ++index)
    {
      num1 += (float) pixels32[index].r;
      num2 += (float) pixels32[index].g;
      num3 += (float) pixels32[index].b;
    }
    return new Color32((byte) ((double) num1 / (double) length), (byte) ((double) num2 / (double) length), (byte) ((double) num3 / (double) length), (byte) 0);
  }

  public enum VideoSide
  {
    up,
    left,
    right,
    down,
    center,
  }
}
