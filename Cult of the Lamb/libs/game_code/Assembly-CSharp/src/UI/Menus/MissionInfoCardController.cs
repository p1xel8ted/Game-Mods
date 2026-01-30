// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.MissionInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
