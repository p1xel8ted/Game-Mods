// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.AchievementInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.Menus.Achievements_Menu;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class AchievementInfoCardController : 
  UIInfoCardController<AchievementInfoCard, AchievementItem>
{
  public override bool IsSelectionValid(Selectable selectable, out AchievementItem showParam)
  {
    AchievementItem component;
    if (selectable.TryGetComponent<AchievementItem>(out component))
    {
      showParam = component;
      return true;
    }
    showParam = (AchievementItem) null;
    return false;
  }
}
