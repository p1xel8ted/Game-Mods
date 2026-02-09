// Decompiled with JetBrains decompiler
// Type: src.UI.PhotoGalleryAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;

#nullable disable
namespace src.UI;

public class PhotoGalleryAlert : AlertBadge<string>
{
  public override AlertCategory<string> _source
  {
    get => (AlertCategory<string>) DataManager.Instance.Alerts.GalleryAlerts;
  }
}
