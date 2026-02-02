// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.AchievementInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
