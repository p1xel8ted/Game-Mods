// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.PauseDetails;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class TarotInfoCardController : UIInfoCardController<TarotInfoCard, TarotCards.TarotCard>
{
  protected override bool IsSelectionValid(
    Selectable selectable,
    out TarotCards.TarotCard showParam)
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

  protected override TarotCards.TarotCard DefaultShowParam() => (TarotCards.TarotCard) null;
}
