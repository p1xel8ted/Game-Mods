// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.PhotoInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus;

public class PhotoInfoCardController : 
  UIInfoCardController<PhotoInfoCard, PhotoModeManager.PhotoData>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out PhotoModeManager.PhotoData showParam)
  {
    showParam = (PhotoModeManager.PhotoData) null;
    PhotoInformationBox component;
    if (!selectable.TryGetComponent<PhotoInformationBox>(out component))
      return false;
    showParam = component.PhotoData;
    return true;
  }
}
