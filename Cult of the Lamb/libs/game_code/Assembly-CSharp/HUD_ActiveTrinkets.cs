// Decompiled with JetBrains decompiler
// Type: HUD_ActiveTrinkets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUD_ActiveTrinkets : BaseMonoBehaviour
{
  public HUD_TrinketCard CardPrefab;
  public List<HUD_TrinketCard> _cards = new List<HUD_TrinketCard>();
  public bool movePosition = true;
  public static List<HUD_ActiveTrinkets> activeTrinkets = new List<HUD_ActiveTrinkets>();
  public PlayerFarming playerFarming;
  public int activeTrinketsId;
  public AnimationCurve animationCurve;

  public void Init(PlayerFarming playerFarmingVar)
  {
    this.playerFarming = playerFarmingVar;
    this.OnTrinketsChanged(this.playerFarming);
  }

  public void OnEnable()
  {
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.OnTrinketRemoved);
    TrinketManager.OnTrinketsCleared += new TrinketManager.TrinketsUpdated(this.OnTrinketsChanged);
  }

  public void OnDisable()
  {
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.OnTrinketRemoved);
    TrinketManager.OnTrinketsCleared -= new TrinketManager.TrinketsUpdated(this.OnTrinketsChanged);
  }

  public void OnDestroy()
  {
  }

  public void OnTrinketAdded(TarotCards.Card card, PlayerFarming playerFarming)
  {
    if (!((Object) this.playerFarming == (Object) playerFarming))
      return;
    this.OnTrinketsChanged(playerFarming);
  }

  public void OnTrinketRemoved(TarotCards.Card trinket, PlayerFarming playerFarming)
  {
    if (!((Object) this.playerFarming == (Object) playerFarming))
      return;
    HUD_TrinketCard card1 = (HUD_TrinketCard) null;
    foreach (HUD_TrinketCard card2 in this._cards)
    {
      if (card2.CardType.CardType == trinket)
      {
        card1 = card2;
        break;
      }
    }
    if (!((Object) card1 != (Object) null))
      return;
    this._cards.Remove(card1);
    this.OnTrinketsChanged(playerFarming);
    this.StartCoroutine((IEnumerator) this.CardAnimateDestroy(card1));
  }

  public void OnTrinketsChanged(PlayerFarming playerFarming = null)
  {
    if (!((Object) this.playerFarming == (Object) playerFarming))
      return;
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      TarotCards.TarotCard runTrinket = playerFarming.RunTrinkets[index];
      HUD_TrinketCard card;
      if (index >= this._cards.Count)
      {
        card = Object.Instantiate<HUD_TrinketCard>(this.CardPrefab, this.transform);
        this.StartCoroutine((IEnumerator) this.CardAnimateIn(card));
        this._cards.Add(card);
      }
      else
        card = this._cards[index];
      card.gameObject.SetActive(true);
      card.SetCard(runTrinket);
    }
    for (int index = 0; index < this._cards.Count; ++index)
      this._cards[index].gameObject.SetActive(true);
  }

  public IEnumerator CardAnimateIn(HUD_TrinketCard card)
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    if (this.movePosition)
      card.transform.localPosition = new Vector3(card.transform.localPosition.x, -200f);
    if ((Object) card.Card != (Object) null)
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

  public IEnumerator CardAnimateDestroy(HUD_TrinketCard card)
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
    Object.Destroy((Object) card.gameObject);
  }
}
