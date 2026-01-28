// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDifficultySelectorOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Alerts;
using src.UINavigator;
using System;
using System.Runtime.CompilerServices;
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
  public UIMenuControlPrompts _controlPrompts;
  [Header("Difficulty")]
  [SerializeField]
  public MMButton _easyButton;
  [SerializeField]
  public MMButton _mediumButton;
  [SerializeField]
  public MMButton _hardButton;
  [SerializeField]
  public MMButton _extraHardButton;
  [SerializeField]
  public Image _alertBadgeImage;
  [SerializeField]
  public AlertBadgeBase _alertBadge;
  [SerializeField]
  public TextMeshProUGUI _developerRecommendedText;
  [SerializeField]
  public TextMeshProUGUI _difficutlyDescriptionText;
  [Header("Messages")]
  [SerializeField]
  public GameObject _changeDisclaimer;
  [SerializeField]
  public GameObject _permadeathDislaimer;
  public bool _cancellable;

  public override bool _releaseOnHide => true;

  public void Show(bool cancellable, bool permadeath, bool instant = false)
  {
    this.Show(instant);
    if (PlayerFarming.players != null && PlayerFarming.players.Count > 0)
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.players[0];
    this._cancellable = cancellable;
    this._changeDisclaimer.SetActive(!permadeath);
    this._permadeathDislaimer.SetActive(permadeath);
  }

  public override void OnShowStarted()
  {
    if (!this._cancellable)
      this._controlPrompts.HideCancelButton();
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

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if (!SaveGameCorruptionTool.s_RunningTest)
      return;
    this.SelectDifficulty(1);
  }

  public void UpdateDifficultyText(DifficultyManager.Difficulty difficulty)
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

  public void OnDeveloperRecommendedButtonSelected()
  {
    this._alertBadge.enabled = true;
    this._alertBadgeImage.DOKill();
    DOTweenModuleUI.DOColor(this._alertBadgeImage, new Color(1f, 1f, 1f, 1f), 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._developerRecommendedText.DOKill();
    ShortcutExtensionsTMPText.DOFade(this._developerRecommendedText, 1f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void OnDeveloperRecommendedButtonDeselected()
  {
    this._alertBadge.enabled = false;
    this._alertBadgeImage.DOKill();
    DOTweenModuleUI.DOColor(this._alertBadgeImage, new Color(1f, 1f, 1f, 0.0f), 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._developerRecommendedText.DOKill();
    ShortcutExtensionsTMPText.DOFade(this._developerRecommendedText, 0.0f, 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void SelectDifficulty(int difficultyIndex)
  {
    Action<int> difficultySelected = this.OnDifficultySelected;
    if (difficultySelected != null)
      difficultySelected(difficultyIndex);
    this.Hide();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override void OnCancelButtonInput()
  {
    if (!this._cancellable)
      return;
    this.Hide();
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_0() => this.SelectDifficulty(0);

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_1() => this.SelectDifficulty(1);

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_2() => this.SelectDifficulty(2);

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_3() => this.SelectDifficulty(3);

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_4()
  {
    this.UpdateDifficultyText(DifficultyManager.Difficulty.Easy);
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_5()
  {
    this.UpdateDifficultyText(DifficultyManager.Difficulty.Medium);
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_6()
  {
    this.UpdateDifficultyText(DifficultyManager.Difficulty.Hard);
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__16_7()
  {
    this.UpdateDifficultyText(DifficultyManager.Difficulty.ExtraHard);
  }
}
