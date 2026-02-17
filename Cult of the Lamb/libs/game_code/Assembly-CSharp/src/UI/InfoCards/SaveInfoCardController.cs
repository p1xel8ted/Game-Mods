// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.SaveInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.MainMenu;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class SaveInfoCardController : UIInfoCardController<SaveInfoCard, MetaData>
{
  [SerializeField]
  public Image _fade;

  public void Show(SaveInfoCard info)
  {
    this._fade.gameObject.SetActive(true);
    this._fade.DOKill();
    DOTweenModuleUI.DOFade(this._fade, 1f, 0.33f);
  }

  public void Hide()
  {
    this._fade.gameObject.SetActive(true);
    this._fade.DOKill();
    DOTweenModuleUI.DOFade(this._fade, 0.0f, 0.33f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this._fade.gameObject.SetActive(false)));
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.OnInfoCardShown = this.OnInfoCardShown + new Action<SaveInfoCard>(this.Show);
    this.OnInfoCardsHidden = this.OnInfoCardsHidden + new System.Action(this.Hide);
    this._fade.gameObject.SetActive(false);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.OnInfoCardShown = this.OnInfoCardShown - new Action<SaveInfoCard>(this.Show);
    this.OnInfoCardsHidden = this.OnInfoCardsHidden - new System.Action(this.Hide);
  }

  public override bool IsSelectionValid(Selectable selectable, out MetaData showParam)
  {
    showParam = new MetaData();
    SaveSlotButtonBase component;
    if (!selectable.TryGetComponent<SaveSlotButtonBase>(out component) || !component.Occupied || !component.MetaData.HasValue)
      return false;
    showParam = component.MetaData.Value;
    return true;
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__1_0() => this._fade.gameObject.SetActive(false);
}
