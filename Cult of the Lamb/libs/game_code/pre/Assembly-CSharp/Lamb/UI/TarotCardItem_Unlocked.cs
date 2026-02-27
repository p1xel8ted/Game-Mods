// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotCardItem_Unlocked
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class TarotCardItem_Unlocked : TarotCardItemBase
{
  private bool _isUnlocked;

  public override void Configure(TarotCards.Card card)
  {
    base.Configure(card);
    this._isUnlocked = TarotCards.IsUnlocked(card);
    if (this._isUnlocked)
    {
      this.TarotCard.Spine.color = Color.white;
      this._tarotCard.SetStaticFront();
    }
    else
    {
      this.TarotCard.Spine.color = new Color(0.0f, 1f, 1f, 1f);
      this._tarotCard.SetStaticBack();
    }
  }

  public override void OnSelect(BaseEventData eventData)
  {
    base.OnSelect(eventData);
    if (this._isUnlocked)
      return;
    this.TarotCard.Spine.DOKill();
    this.TarotCard.Spine.DOColor(Color.white, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public override void OnDeselect(BaseEventData eventData)
  {
    base.OnDeselect(eventData);
    if (this._isUnlocked)
      return;
    this.TarotCard.Spine.DOKill();
    this.TarotCard.Spine.DOColor(new Color(0.0f, 1f, 1f, 1f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }
}
