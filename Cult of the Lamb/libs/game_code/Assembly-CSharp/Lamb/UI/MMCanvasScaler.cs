// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMCanvasScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMCanvasScaler : CanvasScaler
{
  public const int kReferenceResolution_Width = 1920;
  public const int kReferenceResolution_Height = 1080;

  public static float CanvasScale
  {
    get => Mathf.Min((float) Screen.width / 1920f, (float) Screen.height / 1080f);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (Application.isPlaying)
    {
      if (SettingsManager.Settings == null)
        return;
      this.OnUIScaleChanged();
    }
    else
    {
      this.referenceResolution = new Vector2(1920f, 1080f);
      this.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      this.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    int num = Application.isPlaying ? 1 : 0;
  }

  public void OnUIScaleChanged()
  {
    float num = 1f;
    this.referenceResolution = new Vector2((float) (1920.0 + 1920.0 * (1.0 - (double) num)), (float) (1080.0 + 1080.0 * (1.0 - (double) num)));
  }
}
