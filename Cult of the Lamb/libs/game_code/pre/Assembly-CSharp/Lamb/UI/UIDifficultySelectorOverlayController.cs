// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDifficultySelectorOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Alerts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIDifficultySelectorOverlayController : UIMenuBase
{
  public Action<int> OnDifficultySelected;
  [SerializeField]
  private MMButton _easyButton;
  [SerializeField]
  private MMButton _mediumButton;
  [SerializeField]
  private MMButton _hardButton;
  [SerializeField]
  private MMButton _extraHardButton;
  [SerializeField]
  private Image _alertBadgeImage;
  [SerializeField]
  private AlertBadgeBase _alertBadge;
  [SerializeField]
  private TextMeshProUGUI _developerRecommendedText;
  [SerializeField]
  private TextMeshProUGUI _difficutlyDescriptionText;
  private int _difficultySelected;

  protected override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this._easyButton.onClick.AddListener((UnityAction) (() => this.SelectDifficulty(0)));
    this._mediumButton.onClick.AddListener((UnityAction) (() => this.SelectDifficulty(1)));
    this._hardButton.onClick.AddListener((UnityAction) (() => this.SelectDifficulty(2)));
    this._extraHardButton.onClick.AddListener((UnityAction) (() => this.SelectDifficulty(3)));
    this._mediumButton.OnSelected += new System.Action(this.OnDeveloperRecommendedButtonSelected);
    this._mediumButton.OnDeselected += new System.Action(this.OnDeveloperRecommendedButtonDeselected);
    this._easyButton.OnSelected += (System.Action) (() => this.UpdateDifficultyText(DifficultyManager.Difficulty.Easy));
    this._mediumButton.OnSelected += (System.Action) (() => this.UpdateDifficultyText(DifficultyManager.Difficulty.Medium));
    this._hardButton.OnSelected += (System.Action) (() => this.UpdateDifficultyText(DifficultyManager.Difficulty.Hard));
    this._extraHardButton.OnSelected += (System.Action) (() => this.UpdateDifficultyText(DifficultyManager.Difficulty.ExtraHard));
    this.OverrideDefault((Selectable) this._mediumButton);
    this.ActivateNavigation();
  }

  private void UpdateDifficultyText(DifficultyManager.Difficulty difficulty)
  {
    switch (difficulty)
    {
      case DifficultyManager.Difficulty.Easy:
        this._difficutlyDescriptionText.text = ScriptLocalization.UI_Settings_Game_Difficulty.EasyDescription;
        break;
      case DifficultyManager.Difficulty.Medium:
        this._difficutlyDescriptionText.text = ScriptLocalization.UI_Settings_Game_Difficulty.MediumDescription;
        break;
      case DifficultyManager.Difficulty.Hard:
        this._difficutlyDescriptionText.text = ScriptLocalization.UI_Settings_Game_Difficulty.HardDescription;
        break;
      case DifficultyManager.Difficulty.ExtraHard:
        this._difficutlyDescriptionText.text = ScriptLocalization.UI_Settings_Game_Difficulty.ExtraHardDescription;
        break;
    }
  }

  private void OnDeveloperRecommendedButtonSelected()
  {
    this._alertBadge.enabled = true;
    this._alertBadgeImage.DOKill();
    DOTweenModuleUI.DOColor(this._alertBadgeImage, new Color(1f, 1f, 1f, 1f), 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._developerRecommendedText.DOKill();
    ShortcutExtensionsTMPText.DOFade(this._developerRecommendedText, 1f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  private void OnDeveloperRecommendedButtonDeselected()
  {
    this._alertBadge.enabled = false;
    this._alertBadgeImage.DOKill();
    DOTweenModuleUI.DOColor(this._alertBadgeImage, new Color(1f, 1f, 1f, 0.0f), 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._developerRecommendedText.DOKill();
    ShortcutExtensionsTMPText.DOFade(this._developerRecommendedText, 0.0f, 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  private void SelectDifficulty(int difficultyIndex)
  {
    this._difficultySelected = difficultyIndex;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  protected override void OnHideCompleted()
  {
    Action<int> difficultySelected = this.OnDifficultySelected;
    if (difficultySelected != null)
      difficultySelected(this._difficultySelected);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
