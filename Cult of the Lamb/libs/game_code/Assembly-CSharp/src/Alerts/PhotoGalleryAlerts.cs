// Decompiled with JetBrains decompiler
// Type: src.Alerts.PhotoGalleryAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class PhotoGalleryAlerts : AlertCategory<string>
{
  public PhotoGalleryAlerts()
  {
    PhotoModeManager.OnPhotoSaved += new Action<string>(this.OnPhotoTaken);
    PhotoModeManager.OnPhotoDeleted += new Action<string>(this.OnPhotoDeleted);
  }

  void object.Finalize()
  {
    try
    {
      PhotoModeManager.OnPhotoSaved -= new Action<string>(this.OnPhotoTaken);
      PhotoModeManager.OnPhotoDeleted -= new Action<string>(this.OnPhotoDeleted);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnPhotoTaken(string filename) => this.AddOnce(filename);

  public void OnPhotoDeleted(string filename) => this.Remove(filename);
}
