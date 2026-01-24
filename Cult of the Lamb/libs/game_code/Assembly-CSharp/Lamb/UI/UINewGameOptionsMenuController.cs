// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UINewGameOptionsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using src.Managers;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UINewGameOptionsMenuController : UIMenuBase
{
  public Action<UINewGameOptionsMenuController.NewGameOptions> OnOptionsAccepted;
  [SerializeField]
  public MMToggle _quickStartToggle;
  [SerializeField]
  public MMSelectable_Toggle _quickStartToggleSelectable;
  [SerializeField]
  public GameObject _quickStartdescription;
  [SerializeField]
  public GameObject _quickStartContainer;
  [SerializeField]
  public GameObject _quickStartDescription;
  [SerializeField]
  public MMToggle _permadeathMode;
  [SerializeField]
  public GameObject _permadeathModeContainer;
  [SerializeField]
  public GameObject _permadeathModeDescription;
  [SerializeField]
  public GameObject _survivalModeNewIcon;
  [SerializeField]
  public MMToggle _survivalMode;
  [SerializeField]
  public GameObject _survivalModeContainer;
  [SerializeField]
  public GameObject _survivalModeDescription;
  [SerializeField]
  public GameObject _winterModeNewIcon;
  [SerializeField]
  public MMToggle _winterMode;
  [SerializeField]
  public GameObject _winterModeContainer;
  [SerializeField]
  public GameObject _winterModeDescription;
  [SerializeField]
  public MMButton _acceptButton;

  public override bool _releaseOnHide => true;

  public override void Awake()
  {
    base.Awake();
    this._quickStartToggle.Value = false;
    this._permadeathMode.Value = false;
    this._survivalMode.Value = false;
    this._winterMode.Value = false;
    this._survivalModeNewIcon.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedSurvivalMode && !PersistenceManager.PersistentData.PlayedSurvivalMode);
    this._survivalModeContainer.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedSurvivalMode);
    this._winterModeContainer.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedWinterMode);
    this._winterModeNewIcon.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedWinterMode && !PersistenceManager.PersistentData.PlayedWinterMode);
    this._winterModeDescription.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedWinterMode);
    this._survivalModeDescription.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedSurvivalMode);
    this._quickStartContainer.gameObject.SetActive(PersistenceManager.PersistentData.GameCompletionSnapshots.Count > 0 || PersistenceManager.PersistentData.PostGameRevealed || PersistenceManager.PersistentData.UnlockedSurvivalMode);
    this._quickStartDescription.gameObject.SetActive(PersistenceManager.PersistentData.GameCompletionSnapshots.Count > 0 || PersistenceManager.PersistentData.PostGameRevealed || PersistenceManager.PersistentData.UnlockedSurvivalMode);
    this._permadeathModeContainer.gameObject.SetActive(PersistenceManager.PersistentData.GameCompletionSnapshots.Count > 0 || PersistenceManager.PersistentData.PostGameRevealed);
    this._permadeathModeDescription.gameObject.SetActive(PersistenceManager.PersistentData.GameCompletionSnapshots.Count > 0 || PersistenceManager.PersistentData.PostGameRevealed);
    this._quickStartToggle.OnValueChanged += new Action<bool>(this.OnQuickStartToggleChanged);
    this._permadeathMode.OnValueChanged += new Action<bool>(this.OnPermadeathModeToggleChanged);
    this._survivalMode.OnValueChanged += new Action<bool>(this.OnSurvivalModeToggleChanged);
    this._winterMode.OnValueChanged += new Action<bool>(this.OnWinterModeToggleChanged);
    this._acceptButton.onClick.AddListener(new UnityAction(this.OnAcceptButtonClicked));
  }

  public void OnQuickStartToggleChanged(bool value)
  {
  }

  public void OnPermadeathModeToggleChanged(bool value)
  {
  }

  public void OnWinterModeToggleChanged(bool value)
  {
    if (value)
      this._quickStartToggle.Value = true;
    this._quickStartToggleSelectable.Interactable = !value;
  }

  public void OnSurvivalModeToggleChanged(bool value)
  {
    if (value)
      this._quickStartToggle.Value = true;
    this._quickStartToggleSelectable.Interactable = !value;
  }

  public void OnAcceptButtonClicked()
  {
    if (this._quickStartToggle.Value)
    {
      UIDifficultySelectorOverlayController menu = MonoSingleton<UIManager>.Instance.DifficultySelectorTemplate.Instantiate<UIDifficultySelectorOverlayController>();
      menu.Show(true, this._permadeathMode.Value);
      this.PushInstance<UIDifficultySelectorOverlayController>(menu);
      menu.OnDifficultySelected += (Action<int>) (result =>
      {
        Action<UINewGameOptionsMenuController.NewGameOptions> onOptionsAccepted = this.OnOptionsAccepted;
        if (onOptionsAccepted != null)
          onOptionsAccepted(new UINewGameOptionsMenuController.NewGameOptions()
          {
            QuickStart = this._quickStartToggle.Value,
            DifficultyIndex = result,
            PermadeathMode = this._permadeathMode.Value,
            SurvivalMode = this._survivalMode.Value,
            WinterMode = this._winterMode.Value
          });
        this.Hide();
      });
    }
    else
    {
      Action<UINewGameOptionsMenuController.NewGameOptions> onOptionsAccepted = this.OnOptionsAccepted;
      if (onOptionsAccepted != null)
        onOptionsAccepted(new UINewGameOptionsMenuController.NewGameOptions()
        {
          QuickStart = this._quickStartToggle.Value,
          DifficultyIndex = DifficultyManager.AllAvailableDifficulties().IndexOf<DifficultyManager.Difficulty>(DifficultyManager.Difficulty.Medium),
          PermadeathMode = this._permadeathMode.Value,
          SurvivalMode = this._survivalMode.Value,
          WinterMode = this._winterMode.Value
        });
      this.Hide();
    }
  }

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  public void \u003COnAcceptButtonClicked\u003Eb__26_0(int result)
  {
    Action<UINewGameOptionsMenuController.NewGameOptions> onOptionsAccepted = this.OnOptionsAccepted;
    if (onOptionsAccepted != null)
      onOptionsAccepted(new UINewGameOptionsMenuController.NewGameOptions()
      {
        QuickStart = this._quickStartToggle.Value,
        DifficultyIndex = result,
        PermadeathMode = this._permadeathMode.Value,
        SurvivalMode = this._survivalMode.Value,
        WinterMode = this._winterMode.Value
      });
    this.Hide();
  }

  public struct NewGameOptions
  {
    public bool QuickStart;
    public int DifficultyIndex;
    public bool PermadeathMode;
    public bool SurvivalMode;
    public bool WinterMode;
  }
}
