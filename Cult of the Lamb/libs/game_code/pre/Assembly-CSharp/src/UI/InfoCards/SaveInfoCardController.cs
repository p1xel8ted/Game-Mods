// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.SaveInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.MainMenu;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class SaveInfoCardController : UIInfoCardController<SaveInfoCard, MetaData>
{
  [SerializeField]
  private Image _fade;

  private void Show(SaveInfoCard info)
  {
    this._fade.gameObject.SetActive(true);
    this._fade.DOKill();
    DOTweenModuleUI.DOFade(this._fade, 1f, 0.33f);
  }

  private void Hide()
  {
    this._fade.gameObject.SetActive(true);
    this._fade.DOKill();
    DOTweenModuleUI.DOFade(this._fade, 0.0f, 0.33f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this._fade.gameObject.SetActive(false)));
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.OnInfoCardShown = this.OnInfoCardShown + new Action<SaveInfoCard>(this.Show);
    this.OnInfoCardsHidden = this.OnInfoCardsHidden + new System.Action(this.Hide);
    this._fade.gameObject.SetActive(false);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.OnInfoCardShown = this.OnInfoCardShown - new Action<SaveInfoCard>(this.Show);
    this.OnInfoCardsHidden = this.OnInfoCardsHidden - new System.Action(this.Hide);
  }

  protected override bool IsSelectionValid(Selectable selectable, out MetaData showParam)
  {
    showParam = new MetaData();
    SaveSlotButtonBase component;
    if (!selectable.TryGetComponent<SaveSlotButtonBase>(out component) || !component.Occupied || !component.MetaData.HasValue)
      return false;
    showParam = component.MetaData.Value;
    return true;
  }
}
