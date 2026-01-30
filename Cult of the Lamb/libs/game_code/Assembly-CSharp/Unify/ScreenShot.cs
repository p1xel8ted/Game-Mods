// Decompiled with JetBrains decompiler
// Type: Unify.ScreenShot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Unify;

public class ScreenShot
{
  public static ScreenShot singleton;
  public string ProjectName;

  public static ScreenShot Instance => ScreenShot.singleton;

  public static void Init()
  {
    Debug.Log((object) "ScreenShot Init called");
    ScreenShot.singleton = new ScreenShot();
    ScreenShot.singleton.ProjectName = Application.productName;
  }

  public virtual void Update()
  {
  }

  public virtual void Terminate()
  {
  }

  public virtual void TakeAndShareScreenshot(
    Texture2D screenShot,
    ScreenShot.ImageFormats _imageFormat = ScreenShot.ImageFormats.JPEG)
  {
    screenShot.EncodeToJPG(100);
    Debug.Log((object) "ScreenShot Share Encoding successful");
  }

  public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
  {
    Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, false);
    Color[] pixels = texture2D.GetPixels(0);
    float num1 = 1f / (float) targetWidth;
    float num2 = 1f / (float) targetHeight;
    for (int index = 0; index < pixels.Length; ++index)
      pixels[index] = source.GetPixelBilinear(num1 * ((float) index % (float) targetWidth), num2 * Mathf.Floor((float) (index / targetWidth)));
    texture2D.SetPixels(pixels, 0);
    texture2D.Apply();
    return texture2D;
  }

  public virtual void SavingScreenshot()
  {
  }

  public enum ImageFormats
  {
    JPEG,
    PNG,
  }
}
