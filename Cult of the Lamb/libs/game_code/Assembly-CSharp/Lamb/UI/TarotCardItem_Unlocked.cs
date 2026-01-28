// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotCardItem_Unlocked
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class TarotCardItem_Unlocked : TarotCardItemBase
{
  public bool _isUnlocked;

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

  public void ForceIncognitoMode()
  {
    this._alert.gameObject.SetActive(false);
    this.TarotCard.Spine.color = new Color(0.0f, 1f, 1f, 1f);
  }

  public void AnimateIncognitoOut()
  {
    this.TarotCard.Spine.DOColor(Color.white, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public IEnumerator ShowAlert()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TarotCardItem_Unlocked cardItemUnlocked = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Vector3 one = Vector3.one;
    cardItemUnlocked._alert.transform.localScale = Vector3.zero;
    cardItemUnlocked._alert.transform.DOScale(one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    cardItemUnlocked._alert.gameObject.SetActive(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
