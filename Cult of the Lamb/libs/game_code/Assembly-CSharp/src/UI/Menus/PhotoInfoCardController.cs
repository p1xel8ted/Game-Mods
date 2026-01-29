// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.PhotoInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
