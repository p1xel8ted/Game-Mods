// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.PauseDetails;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class TarotInfoCardController : UIInfoCardController<TarotInfoCard, TarotCards.TarotCard>
{
  public override bool IsSelectionValid(Selectable selectable, out TarotCards.TarotCard showParam)
  {
    showParam = (TarotCards.TarotCard) null;
    TarotCardItem_Unlocked component1;
    if (selectable.TryGetComponent<TarotCardItem_Unlocked>(out component1))
    {
      showParam = new TarotCards.TarotCard(component1.Type, 0);
      return TarotCards.IsUnlocked(component1.Type);
    }
    TarotCardItem_Run component2;
    if (!selectable.TryGetComponent<TarotCardItem_Run>(out component2))
      return false;
    showParam = component2.Card;
    return true;
  }

  public override TarotCards.TarotCard DefaultShowParam() => (TarotCards.TarotCard) null;
}
