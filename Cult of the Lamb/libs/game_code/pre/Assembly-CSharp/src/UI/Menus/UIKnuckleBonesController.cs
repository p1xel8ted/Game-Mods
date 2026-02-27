// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.UIKnuckleBonesController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class UIKnuckleBonesController : UIMenuBase
{
  public Action<bool> OnGameCompleted;
  public System.Action OnGameQuit;
  [Header("Knucklebones")]
  [SerializeField]
  private KBMainMenu _mainMenu;
  [SerializeField]
  private KBInstructionsScreen _instructionsScreen;
  [SerializeField]
  private KBGameScreen _gameScreen;
  [Header("Misc")]
  [SerializeField]
  private CanvasGroup _blackOverlayCanvasGroup;
  private bool _continueToPlay;
  private UIMenuBase _currentMenu;

  public override void Awake()
  {
    base.Awake();
    this._blackOverlayCanvasGroup.alpha = 1f;
  }

  public void Show(KnucklebonesOpponent opponent, int bet, bool instant = false)
  {
    this._gameScreen.Configure(opponent, bet);
    this.Show(instant);
  }

  protected override IEnumerator DoShowAnimation()
  {
    while ((double) this._blackOverlayCanvasGroup.alpha > 0.0)
    {
      this._blackOverlayCanvasGroup.alpha -= Time.unscaledDeltaTime;
      yield return (object) null;
    }
  }

  protected override IEnumerator DoHideAnimation()
  {
    while ((double) this._blackOverlayCanvasGroup.alpha < 1.0)
    {
      this._blackOverlayCanvasGroup.alpha += Time.unscaledDeltaTime;
      yield return (object) null;
    }
  }

  public void Start()
  {
    AudioManager.Instance.SetMusicRoomID(2, "ratau_home_id");
    this._mainMenu.Show(true);
    this._currentMenu = (UIMenuBase) this._mainMenu;
    this._mainMenu.OnPlayButtonPressed += (System.Action) (() =>
    {
      if (DataManager.Instance.ShownKnuckleboneTutorial)
      {
        MonoSingleton<UINavigatorNew>.Instance.Clear();
        this.TransitionTo((UIMenuBase) this._gameScreen);
      }
      else
      {
        this._continueToPlay = true;
        this.TransitionTo((UIMenuBase) this._instructionsScreen);
      }
    });
    this._mainMenu.OnInstructionsButtonPressed += (System.Action) (() => this.TransitionTo((UIMenuBase) this._instructionsScreen));
    this._mainMenu.OnQuitButtonPressed += new System.Action(this.OnMatchQuit);
    this._instructionsScreen.OnContinueButtonPressed += (System.Action) (() =>
    {
      if (this._continueToPlay)
      {
        MonoSingleton<UINavigatorNew>.Instance.Clear();
        this.TransitionTo((UIMenuBase) this._gameScreen);
      }
      else
        this.TransitionTo((UIMenuBase) this._mainMenu);
    });
    this._instructionsScreen.OnBackButtonPressed += (System.Action) (() => this.TransitionTo((UIMenuBase) this._mainMenu));
    this._gameScreen.OnMatchFinished += new Action<bool>(this.OnMatchFinished);
  }

  private void TransitionTo(UIMenuBase menu)
  {
    if (!((UnityEngine.Object) this._currentMenu != (UnityEngine.Object) menu))
      return;
    this.PerformMenuTransition(this._currentMenu, menu);
    this._currentMenu = menu;
  }

  private void PerformMenuTransition(UIMenuBase from, UIMenuBase to)
  {
    from.Hide();
    to.Show();
  }

  private void OnMatchFinished(bool victory)
  {
    this.OnHidden = this.OnHidden + (System.Action) (() =>
    {
      Action<bool> onGameCompleted = this.OnGameCompleted;
      if (onGameCompleted == null)
        return;
      onGameCompleted(victory);
    });
    this.Hide();
  }

  private void OnMatchQuit()
  {
    this.OnHidden = this.OnHidden + (System.Action) (() =>
    {
      System.Action onGameQuit = this.OnGameQuit;
      if (onGameQuit == null)
        return;
      onGameQuit();
    });
    this.Hide();
  }

  protected override void OnHideCompleted()
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
