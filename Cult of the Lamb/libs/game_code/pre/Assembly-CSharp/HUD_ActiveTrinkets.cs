// Decompiled with JetBrains decompiler
// Type: HUD_ActiveTrinkets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUD_ActiveTrinkets : BaseMonoBehaviour
{
  public static HUD_ActiveTrinkets Instance;
  public HUD_TrinketCard CardPrefab;
  private List<HUD_TrinketCard> _cards = new List<HUD_TrinketCard>();
  public bool movePosition = true;
  public AnimationCurve animationCurve;

  private void OnEnable()
  {
    HUD_ActiveTrinkets.Instance = this;
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnTrinketsChanged);
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.OnTrinketRemoved);
    TrinketManager.OnTrinketsCleared += new TrinketManager.TrinketsUpdated(this.OnTrinketsChanged);
    this.OnTrinketsChanged();
  }

  private void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnTrinketsChanged);
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.OnTrinketRemoved);
    TrinketManager.OnTrinketsCleared -= new TrinketManager.TrinketsUpdated(this.OnTrinketsChanged);
  }

  private void OnTrinketAdded(TarotCards.Card card) => this.OnTrinketsChanged();

  private void OnTrinketRemoved(TarotCards.Card trinket)
  {
    HUD_TrinketCard card1 = (HUD_TrinketCard) null;
    foreach (HUD_TrinketCard card2 in this._cards)
    {
      if (card2.CardType.CardType == trinket)
      {
        card1 = card2;
        break;
      }
    }
    if (!((UnityEngine.Object) card1 != (UnityEngine.Object) null))
      return;
    this._cards.Remove(card1);
    this.OnTrinketsChanged();
    this.StartCoroutine((IEnumerator) this.CardAnimateDestroy(card1));
  }

  private void OnTrinketsChanged()
  {
    int index;
    for (index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      TarotCards.TarotCard playerRunTrinket = DataManager.Instance.PlayerRunTrinkets[index];
      HUD_TrinketCard card;
      if (index >= this._cards.Count)
      {
        card = UnityEngine.Object.Instantiate<HUD_TrinketCard>(this.CardPrefab, this.transform);
        this.StartCoroutine((IEnumerator) this.CardAnimateIn(card));
        this._cards.Add(card);
      }
      else
        card = this._cards[index];
      card.gameObject.SetActive(true);
      card.SetCard(playerRunTrinket);
    }
    for (; index < this._cards.Count; ++index)
      this._cards[index].gameObject.SetActive(false);
  }

  private IEnumerator CardAnimateIn(HUD_TrinketCard card)
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    if (this.movePosition)
      card.transform.localPosition = new Vector3(card.transform.localPosition.x, -200f);
    if ((UnityEngine.Object) card.Card != (UnityEngine.Object) null)
      card.Card.enabled = true;
    if (this.movePosition)
    {
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        card.transform.localPosition = new Vector3(card.transform.localPosition.x, Mathf.LerpUnclamped(-200f, 0.0f, this.animationCurve.Evaluate(Progress / Duration)));
        yield return (object) null;
      }
    }
  }

  private IEnumerator CardAnimateDestroy(HUD_TrinketCard card)
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    if (this.movePosition)
      card.transform.localPosition = new Vector3(card.transform.localPosition.x, 0.0f);
    if (this.movePosition)
    {
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        card.transform.localPosition = new Vector3(card.transform.localPosition.x, Mathf.LerpUnclamped(0.0f, 200f, this.animationCurve.Evaluate(Progress / Duration)));
        yield return (object) null;
      }
    }
    Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      card.transform.localScale = Vector3.one * Mathf.LerpUnclamped(0.0f, 1f, this.animationCurve.Evaluate((float) (1.0 - (double) Progress / (double) Duration)));
      yield return (object) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) card.gameObject);
  }
}
