// Decompiled with JetBrains decompiler
// Type: UIChoiceCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIChoiceCards : BaseMonoBehaviour
{
  public GameObject ChoiceCardPrefab;
  public RectTransform Container;
  public UI_NavigatorSimple UINavigator;
  public List<UIChoiceCard> ChoiceCards;
  public System.Action MakePayment;
  public Action<ChoiceReward> CompleteCallback;
  public System.Action CancelCallback;

  public void Play(
    List<ChoiceReward> Rewards,
    System.Action MakePayment,
    Action<ChoiceReward> CompleteCallback)
  {
    Time.timeScale = 0.0f;
    this.ChoiceCards = new List<UIChoiceCard>();
    int index = -1;
    while (++index < Rewards.Count)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ChoiceCardPrefab, (Transform) this.Container);
      gameObject.SetActive(true);
      UIChoiceCard component = gameObject.GetComponent<UIChoiceCard>();
      component.Play(Rewards[index], (float) index * 0.2f);
      this.ChoiceCards.Add(component);
    }
    this.UINavigator.startingItem = this.ChoiceCards[0].gameObject.GetComponent<Selectable>();
    this.UINavigator.setDefault();
    this.UINavigator.OnCancelDown += new System.Action(this.OnClose);
    this.UINavigator.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelectionUnity);
    this.UINavigator.OnSelectDown += new System.Action(this.OnSelect);
    this.MakePayment = MakePayment;
    this.CompleteCallback = CompleteCallback;
  }

  public void OnChangeSelectionUnity(Selectable PrevSelectable, Selectable NewSelectable)
  {
    PrevSelectable.gameObject.GetComponent<UIChoiceCard>().OnDehighlighted();
    NewSelectable.gameObject.GetComponent<UIChoiceCard>().OnHighlighted();
  }

  public void OnClose()
  {
    if (this.CancelCallback == null)
      return;
    System.Action cancelCallback = this.CancelCallback;
    if (cancelCallback != null)
      cancelCallback();
    this.Close();
  }

  public void Close()
  {
    this.UINavigator.OnCancelDown -= new System.Action(this.OnClose);
    Time.timeScale = 1f;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnSelect()
  {
    int index = -1;
    while (++index < this.ChoiceCards.Count)
    {
      if ((UnityEngine.Object) this.UINavigator.selectable.gameObject == (UnityEngine.Object) this.ChoiceCards[index].gameObject)
      {
        if (this.ChoiceCards[index].Reward.Locked)
        {
          this.ChoiceCards[index].Shake();
          return;
        }
        if (!this.ChoiceCards[index].CanAfford())
        {
          this.ChoiceCards[index].Shake();
          return;
        }
        if (this.ChoiceCards[index].Play(this.CancelCallback))
        {
          Action<ChoiceReward> completeCallback = this.CompleteCallback;
          if (completeCallback != null)
            completeCallback(this.ChoiceCards[index].Reward);
        }
      }
    }
    System.Action makePayment = this.MakePayment;
    if (makePayment != null)
      makePayment();
    this.UINavigator.canvasGroup.interactable = false;
    this.Close();
  }

  public void OnDisable()
  {
    this.UINavigator.OnCancelDown += new System.Action(this.OnClose);
    this.UINavigator.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelectionUnity);
    this.UINavigator.OnSelectDown -= new System.Action(this.OnSelect);
  }
}
