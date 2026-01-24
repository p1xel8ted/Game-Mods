// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.PrisonMenuInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus;

public class PrisonMenuInfoCardController : UIInfoCardController<PrisonInfoCard, FollowerInfo>
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
