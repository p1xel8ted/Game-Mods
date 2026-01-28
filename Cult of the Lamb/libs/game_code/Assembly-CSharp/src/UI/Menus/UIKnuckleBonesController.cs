// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.UIKnuckleBonesController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class UIKnuckleBonesController : UIMenuBase
{
  public Action<UIKnuckleBonesController.KnucklebonesResult> OnGameCompleted;
  public System.Action OnGameQuit;
  [Header("Knucklebones")]
  [SerializeField]
  public KBMainMenu _mainMenu;
  [SerializeField]
  public KBInstructionsScreen _instructionsScreen;
  [SerializeField]
  public KBGameScreen _gameScreen;
  [Header("Misc")]
  [SerializeField]
  public CanvasGroup _blackOverlayCanvasGroup;
  public bool _continueToPlay;
  public UIMenuBase _currentMenu;

  public KBGameScreen GameScreen => this._gameScreen;

  public override void Awake()
  {
    base.Awake();
    this._blackOverlayCanvasGroup.alpha = 1f;
  }

  public void Show(
    PlayerFarming playerFarming,
    KnucklebonesOpponent opponent,
    int bet,
    bool instant = false)
  {
    this._gameScreen.Configure(playerFarming, opponent, bet);
    this.Show(instant);
  }

  public override IEnumerator DoShowAnimation()
  {
    while ((double) this._blackOverlayCanvasGroup.alpha > 0.0)
    {
      this._blackOverlayCanvasGroup.alpha -= Time.unscaledDeltaTime;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
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
    this._gameScreen.OnMatchFinished += new Action<UIKnuckleBonesController.KnucklebonesResult>(this.OnMatchFinished);
  }

  public void TransitionTo(UIMenuBase menu)
  {
    if (!((UnityEngine.Object) this._currentMenu != (UnityEngine.Object) menu))
      return;
    this.PerformMenuTransition(this._currentMenu, menu);
    this._currentMenu = menu;
  }

  public void PerformMenuTransition(UIMenuBase from, UIMenuBase to)
  {
    from.Hide();
    to.Show();
  }

  public void OnMatchFinished(
    UIKnuckleBonesController.KnucklebonesResult victory)
  {
    this.OnHidden = this.OnHidden + (System.Action) (() =>
    {
      Action<UIKnuckleBonesController.KnucklebonesResult> onGameCompleted = this.OnGameCompleted;
      if (onGameCompleted == null)
        return;
      onGameCompleted(victory);
    });
    this.Hide();
  }

  public void OnMatchQuit()
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

  public override void OnHideCompleted()
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__15_0()
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
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__15_1()
  {
    this.TransitionTo((UIMenuBase) this._instructionsScreen);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__15_2()
  {
    if (this._continueToPlay)
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.TransitionTo((UIMenuBase) this._gameScreen);
    }
    else
      this.TransitionTo((UIMenuBase) this._mainMenu);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__15_3() => this.TransitionTo((UIMenuBase) this._mainMenu);

  [CompilerGenerated]
  public void \u003COnMatchQuit\u003Eb__19_0()
  {
    System.Action onGameQuit = this.OnGameQuit;
    if (onGameQuit == null)
      return;
    onGameQuit();
  }

  public enum KnucklebonesResult
  {
    Win,
    Loss,
    Draw,
    Quit,
  }
}
