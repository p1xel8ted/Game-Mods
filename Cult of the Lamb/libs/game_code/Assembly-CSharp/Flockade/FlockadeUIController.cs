// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeUIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeUIController : UIMenuBase
{
  public const string _MAIN_MENU_MUSIC = "title";
  public const string _MUSIC = "event:/dlc/music/flockade/main";
  [Header("Flockade")]
  [SerializeField]
  public FlockadeMainMenu _mainMenu;
  [SerializeField]
  public FlockadeInstructionsScreen _instructionsScreen;
  [SerializeField]
  public FlockadeGameScreen _gameScreen;
  [Header("Misc")]
  [SerializeField]
  public CanvasGroup _blackOverlay;
  public List<IMMSelectable> _wereNotInteractable = new List<IMMSelectable>();
  public FlockadeUIController.AfterInstructionsTransition _afterInstructions;
  public UIMenuBase _currentMenu;
  public EventInstance _previousMusic;
  public float _previousMusicBaseId;

  public event Action<FlockadeUIController.Result> GameCompleted;

  public event System.Action GameQuit;

  public override void Awake()
  {
    this._blackOverlay.alpha = 1f;
    base.Awake();
  }

  public void Show(
    PlayerFarming playerFarming,
    IEnumerable<FlockadeGamePieceConfiguration> gamePiecesPool,
    FlockadeOpponentConfiguration opponentConfiguration,
    int bet = 0,
    bool immediate = false)
  {
    this._gameScreen.Configure(playerFarming, gamePiecesPool, opponentConfiguration, bet);
    if (opponentConfiguration.Type == FlockadeOpponentConfiguration.OpponentType.Twitch)
      this._mainMenu.HideHardMode();
    this.Show(immediate);
  }

  public override IEnumerator DoShowAnimation()
  {
    yield return (object) this._blackOverlay.DOFade(0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).WaitForCompletion();
  }

  public void Start()
  {
    this._previousMusic = AudioManager.Instance.CurrentMusicInstance;
    this._previousMusicBaseId = AudioManager.Instance.GetEventInstanceParameter(this._previousMusic, SoundParams.BaseID);
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PlayMusic("event:/dlc/music/flockade/main");
    AudioManager.Instance.SetFlockadeGameState(SoundConstants.FlockadeGameState.Title);
    this._mainMenu.Show(true);
    this._currentMenu = (UIMenuBase) this._mainMenu;
    this._mainMenu.PlayButtonPressed += (System.Action) (() =>
    {
      this._gameScreen.LateConfigure(FlockadeGameScreen.Difficulty.Normal);
      this.\u003CStart\u003Eg__TransitionToGameScreen\u007C22_5();
    });
    this._mainMenu.PlayHardModeButtonPressed += (System.Action) (() =>
    {
      this._gameScreen.LateConfigure(FlockadeGameScreen.Difficulty.Hard);
      this.\u003CStart\u003Eg__TransitionToGameScreen\u007C22_5();
    });
    this._mainMenu.InstructionsButtonPressed += (System.Action) (() => this.TransitionTo((UIMenuBase) this._instructionsScreen));
    this._mainMenu.QuitButtonPressed += new System.Action(this.Quit);
    this._instructionsScreen.ContinueButtonPressed += (System.Action) (() =>
    {
      if (this._afterInstructions == FlockadeUIController.AfterInstructionsTransition.GameScreen)
      {
        MonoSingleton<UINavigatorNew>.Instance.Clear();
        this.TransitionTo((UIMenuBase) this._gameScreen);
      }
      else
        this.TransitionTo((UIMenuBase) this._mainMenu);
    });
    this._instructionsScreen.BackButtonPressed += (System.Action) (() =>
    {
      this._afterInstructions = FlockadeUIController.AfterInstructionsTransition.MainMenu;
      this.TransitionTo((UIMenuBase) this._mainMenu);
    });
    this._gameScreen.MatchCompleted += new Action<FlockadeUIController.Result>(this.Complete);
    this._gameScreen.MatchQuit += new System.Action(this.Quit);
  }

  public void TransitionTo(UIMenuBase menu)
  {
    if ((UnityEngine.Object) this._currentMenu == (UnityEngine.Object) menu)
      return;
    this._currentMenu.Hide();
    menu.Show();
    this._currentMenu = menu;
  }

  public void FakeWin() => this._gameScreen.FakeEnd(FlockadeUIController.Result.Win);

  public void FakeLoss() => this._gameScreen.FakeEnd(FlockadeUIController.Result.Loss);

  public void Complete(FlockadeUIController.Result result)
  {
    this.OnHidden = this.OnHidden + (System.Action) (() =>
    {
      Action<FlockadeUIController.Result> gameCompleted = this.GameCompleted;
      if (gameCompleted == null)
        return;
      gameCompleted(result);
    });
    this.Hide();
  }

  public void Quit()
  {
    this.OnHidden = this.OnHidden + (System.Action) (() =>
    {
      System.Action gameQuit = this.GameQuit;
      if (gameQuit == null)
        return;
      gameQuit();
    });
    this.Hide();
  }

  public override IEnumerator DoHideAnimation()
  {
    yield return (object) this._blackOverlay.DOFade(1f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).WaitForCompletion();
  }

  public override void OnHideCompleted()
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    Debug.unityLogger.logEnabled = true;
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PlayMusic("event:/music/base/base_main", false);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.woolhaven);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => AudioManager.Instance.StartMusic()));
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnPush()
  {
    this._wereNotInteractable.AddRange(((IEnumerable<IMMSelectable>) this.gameObject.GetComponentsInChildren<IMMSelectable>()).Where<IMMSelectable>((Func<IMMSelectable, bool>) (selectable => !selectable.Interactable)));
  }

  public override void DoRelease()
  {
    base.DoRelease();
    foreach (IMMSelectable mmSelectable in this._wereNotInteractable)
      mmSelectable.Interactable = false;
    this._wereNotInteractable.Clear();
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__22_0()
  {
    this._gameScreen.LateConfigure(FlockadeGameScreen.Difficulty.Normal);
    this.\u003CStart\u003Eg__TransitionToGameScreen\u007C22_5();
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__22_1()
  {
    this._gameScreen.LateConfigure(FlockadeGameScreen.Difficulty.Hard);
    this.\u003CStart\u003Eg__TransitionToGameScreen\u007C22_5();
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__22_2()
  {
    this.TransitionTo((UIMenuBase) this._instructionsScreen);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__22_3()
  {
    if (this._afterInstructions == FlockadeUIController.AfterInstructionsTransition.GameScreen)
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.TransitionTo((UIMenuBase) this._gameScreen);
    }
    else
      this.TransitionTo((UIMenuBase) this._mainMenu);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__22_4()
  {
    this._afterInstructions = FlockadeUIController.AfterInstructionsTransition.MainMenu;
    this.TransitionTo((UIMenuBase) this._mainMenu);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eg__TransitionToGameScreen\u007C22_5()
  {
    if (!DataManager.Instance.FlockadeTutorialShown)
    {
      this._afterInstructions = FlockadeUIController.AfterInstructionsTransition.GameScreen;
      this.TransitionTo((UIMenuBase) this._instructionsScreen);
    }
    else if (FlockadePieceManager.IsAnyPieceOfSameKindUnlocked(FlockadePieceType.Shepherd) && !DataManager.Instance.FlockadeShepherdsTutorialShown)
    {
      this._afterInstructions = FlockadeUIController.AfterInstructionsTransition.GameScreen;
      this._instructionsScreen.ShowOnlyShepherdTutorialNextTime = true;
      this.TransitionTo((UIMenuBase) this._instructionsScreen);
    }
    else
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.TransitionTo((UIMenuBase) this._gameScreen);
    }
  }

  [CompilerGenerated]
  public void \u003CQuit\u003Eb__27_0()
  {
    System.Action gameQuit = this.GameQuit;
    if (gameQuit == null)
      return;
    gameQuit();
  }

  public enum AfterInstructionsTransition
  {
    MainMenu,
    GameScreen,
  }

  public enum Result
  {
    Win = 1,
    Loss = 2,
    Draw = 3,
  }
}
