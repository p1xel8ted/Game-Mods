// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.PhotoInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
