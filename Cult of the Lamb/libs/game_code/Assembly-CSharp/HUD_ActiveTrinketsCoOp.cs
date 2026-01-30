// Decompiled with JetBrains decompiler
// Type: HUD_ActiveTrinketsCoOp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUD_ActiveTrinketsCoOp : BaseMonoBehaviour
{
  public HUD_TrinketCard CardPrefab;
  public List<HUD_TrinketCard> _cards = new List<HUD_TrinketCard>();
  public bool movePosition = true;
  public static List<HUD_ActiveTrinkets> activeTrinkets = new List<HUD_ActiveTrinkets>();
  public PlayerFarming playerFarming;
  public int activeTrinketsId;
  public bool wasOff;
  public AnimationCurve animationCurve;

  public void OnEnable()
  {
  }

  public void Init(PlayerFarming playerFarmingVar)
  {
    this.playerFarming = playerFarmingVar;
    this.OnTrinketsChanged(this.playerFarming);
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.OnTrinketRemoved);
    TrinketManager.OnTrinketsCleared += new TrinketManager.TrinketsUpdated(this.OnTrinketsChanged);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged += new Action<bool>(this.OnRomanNumeralSettingChanged);
    if (playerFarmingVar.RunTrinkets.Count != 0)
      return;
    this.CardPrefab.gameObject.SetActive(false);
    this.wasOff = true;
  }

  public void OnDestroy()
  {
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.OnTrinketRemoved);
    TrinketManager.OnTrinketsCleared -= new TrinketManager.TrinketsUpdated(this.OnTrinketsChanged);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged -= new Action<bool>(this.OnRomanNumeralSettingChanged);
  }

  public void OnTrinketAdded(TarotCards.Card card, PlayerFarming playerFarming)
  {
    if (!((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) playerFarming))
      return;
    this.OnTrinketsChanged(playerFarming);
  }

  public void OnTrinketRemoved(TarotCards.Card trinket, PlayerFarming playerFarming)
  {
    this.CardPrefab.CardContainer.transform.DOShakePosition(0.5f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    this.OnTrinketsChanged(playerFarming);
  }

  public void OnTrinketsChanged(PlayerFarming playerFarming = null)
  {
    if (!((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) playerFarming))
      return;
    if (playerFarming.RunTrinkets.Count != 0)
    {
      this.CardPrefab.gameObject.SetActive(true);
      if (this.wasOff)
      {
        this.CardPrefab.transform.localScale = Vector3.zero;
        this.StartCoroutine((IEnumerator) this.CardAnimateIn(this.CardPrefab));
      }
      this.wasOff = false;
    }
    else
    {
      this.CardPrefab.gameObject.SetActive(false);
      this.wasOff = true;
    }
    if (SettingsManager.Settings.Accessibility.RomanNumerals)
      this.CardPrefab.CardQuantity.text = this.playerFarming.RunTrinkets.Count.ToNumeral();
    else
      this.CardPrefab.CardQuantity.text = playerFarming.RunTrinkets.Count.ToString();
  }

  public void OnRomanNumeralSettingChanged(bool state)
  {
    if (state)
      this.CardPrefab.CardQuantity.text = this.playerFarming.RunTrinkets.Count.ToNumeral();
    else
      this.CardPrefab.CardQuantity.text = this.playerFarming.RunTrinkets.Count.ToString();
  }

  public IEnumerator CardAnimateIn(HUD_TrinketCard card)
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
}
