// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIInfoCardController`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIInfoCardController<T, U> : MonoBehaviour where T : UIInfoCardBase<U>
{
  public Action<T> OnInfoCardShown;
  public System.Action OnInfoCardsHidden;
  [SerializeField]
  private T _card1;
  [SerializeField]
  private T _card2;
  private T _currentCard;
  private U _currentShowParam;

  public T CurrentCard => this._currentCard;

  public T Card1 => this._card1;

  public T Card2 => this._card2;

  private void Start()
  {
    this._currentShowParam = this.DefaultShowParam();
    this._card1.Hide(true);
    this._card2.Hide(true);
  }

  protected virtual void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnClear += new System.Action(this.OnClear);
  }

  protected virtual void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnClear -= new System.Action(this.OnClear);
  }

  protected abstract bool IsSelectionValid(Selectable selectable, out U showParam);

  private void OnSelection(Selectable current)
  {
    U showParam;
    if (!this.IsSelectionValid(current, out showParam))
    {
      if ((UnityEngine.Object) this._currentCard != (UnityEngine.Object) null)
      {
        this._currentCard.Hide(false);
        this._currentCard = default (T);
        System.Action onInfoCardsHidden = this.OnInfoCardsHidden;
        if (onInfoCardsHidden != null)
          onInfoCardsHidden();
      }
      this._currentShowParam = this.DefaultShowParam();
    }
    else
      this.ShowCardWithParam(showParam);
  }

  public void ShowCardWithParam(U showParam)
  {
    if ((object) this._currentShowParam != null && this._currentShowParam.Equals((object) showParam))
      return;
    this._currentShowParam = showParam;
    if ((UnityEngine.Object) this._currentCard == (UnityEngine.Object) null)
    {
      this._currentCard = this._card1;
      this._card1.Show(showParam, false);
      this._card2.Hide(true);
    }
    else if ((UnityEngine.Object) this._currentCard == (UnityEngine.Object) this._card2)
    {
      this._card1.Show(showParam, false);
      this._card2.Hide(false);
      this._currentCard = this._card1;
    }
    else
    {
      this._card2.Show(showParam, false);
      this._card1.Hide(false);
      this._currentCard = this._card2;
    }
    Action<T> onInfoCardShown = this.OnInfoCardShown;
    if (onInfoCardShown == null)
      return;
    onInfoCardShown(this._currentCard);
  }

  protected virtual U DefaultShowParam() => default (U);

  private void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  private void OnClear()
  {
    if ((UnityEngine.Object) this._currentCard != (UnityEngine.Object) null)
      this._currentCard.Hide(false);
    this._currentShowParam = this.DefaultShowParam();
  }

  public void ForceCurrentCard(T card, U param)
  {
    this._currentCard = card;
    this._currentShowParam = param;
  }
}
