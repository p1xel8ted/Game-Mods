// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.PhotoInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
