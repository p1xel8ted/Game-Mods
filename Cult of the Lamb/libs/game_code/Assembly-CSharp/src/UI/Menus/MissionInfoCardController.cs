// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.MissionInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus;

public class MissionInfoCardController : UIInfoCardController<MissionInfoCard, FollowerInfo>
{
  public override bool IsSelectionValid(Selectable selectable, out FollowerInfo showParam)
  {
    showParam = (FollowerInfo) null;
    FollowerInformationBox component;
    if (!selectable.TryGetComponent<FollowerInformationBox>(out component))
      return false;
    showParam = component.FollowerInfo;
    return true;
  }
}
