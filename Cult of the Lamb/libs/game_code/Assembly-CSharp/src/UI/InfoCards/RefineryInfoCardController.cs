// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RefineryInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RefineryInfoCardController : UIInfoCardController<RefineryInfoCard, RefineryItem>
{
  public override bool IsSelectionValid(Selectable selectable, out RefineryItem showParam)
  {
    showParam = (RefineryItem) null;
    RefineryItem component;
    if (!selectable.TryGetComponent<RefineryItem>(out component))
      return false;
    showParam = component;
    return true;
  }
}
