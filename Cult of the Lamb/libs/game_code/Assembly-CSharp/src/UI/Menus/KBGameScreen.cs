// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBGameScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using KnuckleBones;
using Lamb.UI;
using src.UINavigator;
using src.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable enable
namespace src.UI.Menus;

public class KBGameScreen : UISubmenuBase
{
  public 
  #nullable disable
  Action<UIKnuckleBonesController.KnucklebonesResult> OnMatchFinished;
  [Header("Knuckle Bones")]
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public KBBackground _background;
  [SerializeField]
  public Transform _diceContainer;
  [SerializeField]
  public Transform _luckyDiceIndicator;
  [SerializeField]
  public KBPlayer _player;
  [SerializeField]
  public KBOpponent _opponent;
  [SerializeField]
  public KBPlayer _coopPlayer;
  [SerializeField]
  public KBOpponent _twitchChat;
  [SerializeField]
  public CanvasGroup _announcementCanvasGroup;
  [SerializeField]
  public RectTransform _announcementRectTransform;
  [SerializeField]
  public TextMeshProUGUI _announcementText;
  [SerializeField]
  public TextMeshProUGUI _winningsText;
  public KnucklebonesOpponent _opponentConfiguration;
  public Vector3 _announceTextStartingPosition;
  public UIMenuConfirmationWindow _exitConfirmationInstance;
  public bool _matchCompleted;
  public int _bet;
  public bool _isCoop;
  public bool _isTwitch;
  public PlayerFarming playerFarming;
  public int uniqueID;

  public KBOpponent Opponent => this._opponent;

  public void Configure(PlayerFarming playerFarming, KnucklebonesOpponent opponent, int bet)
  {
    this.playerFarming = playerFarming;
    this._opponentConfiguration = opponent;
    this._bet = bet;
    this._opponent.gameObject.SetActive(!opponent.IsCoopPlayer && !opponent.IsTwitchChat);
    this._coopPlayer.gameObject.SetActive(opponent.IsCoopPlayer);
    this._twitchChat.gameObject.SetActive(opponent.IsTwitchChat);
    this._luckyDiceIndicator.gameObject.SetActive(DataManager.Instance.NextKnucklbonesLucky);
    if (DataManager.Instance.NextKnucklbonesLucky)
      this._luckyDiceIndicator.DORotate(new Vector3(0.0f, 0.0f, 25f), 1.5f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutQuad).SetLoops<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(-1, DG.Tweening.LoopType.Yoyo);
    if (!opponent.IsCoopPlayer)
      this._coopPlayer.HideTubs();
    this._isCoop = opponent.IsCoopPlayer;
    this._isTwitch = opponent.IsTwitchChat;
    this.uniqueID = UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._parent.CanvasGroup.interactable || !this._canvasGroup.interactable || !((UnityEngine.Object) this._exitConfirmationInstance == (UnityEngine.Object) null) || this._matchCompleted)
      return;
    this._exitConfirmationInstance = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    this._exitConfirmationInstance.Configure(KnucklebonesModel.GetLocalizedString("Exit"), KnucklebonesModel.GetLocalizedString("AreYouSure"));
    this._exitConfirmationInstance.OnConfirm += (System.Action) (() =>
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.StopAllCoroutines();
      Action<UIKnuckleBonesController.KnucklebonesResult> onMatchFinished = this.OnMatchFinished;
      if (onMatchFinished == null)
        return;
      onMatchFinished(this._isCoop || this._isTwitch ? UIKnuckleBonesController.KnucklebonesResult.Quit : UIKnuckleBonesController.KnucklebonesResult.Loss);
    });
    this._exitConfirmationInstance.OnCancel += (System.Action) (() => MonoSingleton<UINavigatorNew>.Instance.Clear());
    UIMenuConfirmationWindow confirmationInstance = this._exitConfirmationInstance;
    confirmationInstance.OnHide = confirmationInstance.OnHide + (System.Action) (() => this._exitConfirmationInstance = (UIMenuConfirmationWindow) null);
  }

  public override void OnShowStarted()
  {
    this._announceTextStartingPosition = this._announcementRectTransform.position;
    this._player.Configure(this.playerFarming, new Vector2(-800f, 0.0f), new Vector2(0.0f, -800f));
    if (this._isCoop)
      this._coopPlayer.Configure(PlayerFarming.players[1], new Vector2(800f, 0.0f), new Vector2(0.0f, 800f));
    else if (this._isTwitch)
    {
      this._twitchChat.IsTwitchChat = true;
      this._twitchChat.Configure(this._opponentConfiguration, new Vector2(800f, 0.0f), new Vector2(0.0f, 800f));
    }
    else
      this._opponent.Configure(this._opponentConfiguration, new Vector2(800f, 0.0f), new Vector2(0.0f, 800f));
    this._controlPrompts.HideAcceptButton();
    this._winningsText.gameObject.SetActive(false);
  }

  public override IEnumerator DoShowAnimation()
  {
    KBGameScreen kbGameScreen = this;
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
    kbGameScreen._player.Show();
    if (kbGameScreen._isCoop)
      kbGameScreen._coopPlayer.Show();
    else if (kbGameScreen._isTwitch)
      kbGameScreen._twitchChat.Show();
    else
      kbGameScreen._opponent.Show();
    kbGameScreen._canvasGroup.DOFade(1f, 1.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) kbGameScreen._background.TransitionToEndValues();
  }

  public override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.DoGameLoop());

  public override IEnumerator DoHideAnimation()
  {
    KBGameScreen kbGameScreen = this;
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
    kbGameScreen._player.Hide();
    if (kbGameScreen._isCoop)
      kbGameScreen._coopPlayer.Hide();
    else if (kbGameScreen._isTwitch)
      kbGameScreen._twitchChat.Hide();
    else
      kbGameScreen._opponent.Hide();
    kbGameScreen._canvasGroup.DOFade(0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) kbGameScreen._background.TransitionToStartValues();
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void LateUpdate()
  {
    if (!(Input.mousePosition != MonoSingleton<UINavigatorNew>.Instance.PreviousMouseInput))
      return;
    Cursor.visible = true;
  }

  public IEnumerator DoGameLoop()
  {
    this._diceContainer.DestroyAllChildren();
    bool playerTurn = BoolUtilities.RandomBool();
    if (!this._isCoop && !this._isTwitch)
    {
      if (DataManager.Instance.KnucklebonesFirstGameRatauStart)
        playerTurn = false;
      DataManager.Instance.KnucklebonesFirstGameRatauStart = false;
    }
    this._announcementRectTransform.position = this._announceTextStartingPosition + new Vector3(800f, 0.0f);
    this._announcementRectTransform.DOMove(this._announceTextStartingPosition, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._announcementCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    string str1 = !this._isTwitch ? (this._isCoop ? this._coopPlayer.GetLocalizedName() : this._opponent.GetLocalizedName()) : this._twitchChat.GetLocalizedName();
    string str2 = playerTurn ? this._player.GetLocalizedName() : str1;
    this._announcementText.text = string.Format(KnucklebonesModel.GetLocalizedString("RollsFirst"), (object) str2);
    yield return (object) new WaitForSecondsRealtime(3f);
    this._announcementRectTransform.DOMove(this._announceTextStartingPosition + new Vector3(-800f, 0.0f), 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._announcementCanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    KBGameScreen.\u003C\u003Ec__DisplayClass38_0 cDisplayClass380 = new KBGameScreen.\u003C\u003Ec__DisplayClass38_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass380.completed = false;
    // ISSUE: reference to a compiler-generated field
    while (!cDisplayClass380.completed)
    {
      for (int i = 0; i < 2; ++i)
      {
        if (playerTurn)
        {
          this._controlPrompts.ShowAcceptButton();
          // ISSUE: reference to a compiler-generated method
          yield return (object) this.PerformTurn((KBPlayerBase) this._player, new System.Action(cDisplayClass380.\u003CDoGameLoop\u003Eg__Complete\u007C0));
          this._controlPrompts.HideAcceptButton();
        }
        else if (this._isCoop)
        {
          this._controlPrompts.ShowAcceptButton();
          // ISSUE: reference to a compiler-generated method
          yield return (object) this.PerformTurn((KBPlayerBase) this._coopPlayer, new System.Action(cDisplayClass380.\u003CDoGameLoop\u003Eg__Complete\u007C0));
          this._controlPrompts.HideAcceptButton();
        }
        else if (this._isTwitch)
        {
          // ISSUE: reference to a compiler-generated method
          yield return (object) this.PerformTurn((KBPlayerBase) this._twitchChat, new System.Action(cDisplayClass380.\u003CDoGameLoop\u003Eg__Complete\u007C0));
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          yield return (object) this.PerformTurn((KBPlayerBase) this._opponent, new System.Action(cDisplayClass380.\u003CDoGameLoop\u003Eg__Complete\u007C0));
        }
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass380.completed)
        {
          if (!this._isCoop)
            DataManager.Instance.NextKnucklbonesLucky = false;
          // ISSUE: reference to a compiler-generated method
          cDisplayClass380.\u003CDoGameLoop\u003Eg__Complete\u007C0();
          break;
        }
        playerTurn = !playerTurn;
      }
    }
    cDisplayClass380 = (KBGameScreen.\u003C\u003Ec__DisplayClass38_0) null;
    yield return (object) this.DoEndGame();
  }

  public IEnumerator PerformTurn(KBPlayerBase player, System.Action onCompleted)
  {
    KBPlayerBase oppositePlayer = this.GetOppositePlayer(player);
    player.HighlightMe();
    oppositePlayer.UnHighlightMe();
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Dice currentDice = (Dice) null;
    AudioManager.Instance.PlayOneShot("event:/knuckle_bones/die_roll");
    currentDice = UnityEngine.Object.Instantiate<Dice>(player.DicePrefab, player.Position.position, Quaternion.identity, this._diceContainer);
    int luckyRoll = -1;
    if (!this._isCoop && !this._isTwitch && (UnityEngine.Object) player == (UnityEngine.Object) this._player && DataManager.Instance.NextKnucklbonesLucky)
    {
      int[] numArray = new int[6]{ 10, 10, 10, 10, 10, 10 };
      foreach (int allDicesNumber in this._player.GetAllDicesNumbers())
        numArray[allDicesNumber - 1] += 5;
      foreach (int allDicesNumber in this._opponent.GetAllDicesNumbers())
      {
        if (allDicesNumber >= 4)
          numArray[allDicesNumber - 1] += 5;
      }
      for (int index1 = 0; index1 < 3; ++index1)
      {
        List<int> intList = new List<int>();
        List<int> tubDicesNumbers = this._opponent.GetTubDicesNumbers(index1);
        for (int index2 = 0; index2 < tubDicesNumbers.Count; ++index2)
        {
          if (intList.Contains(tubDicesNumbers[index2]))
            numArray[tubDicesNumbers[index2]] += 5;
          else
            intList.Add(tubDicesNumbers[index2]);
        }
      }
      List<int> intList1 = new List<int>();
      int num1 = 0;
      for (int index = 0; index < numArray.Length; ++index)
      {
        num1 += numArray[index];
        intList1.Add(num1);
      }
      int num2 = UnityEngine.Random.Range(0, num1 - 1);
      for (int index = 0; index < intList1.Count; ++index)
      {
        if (num2 < intList1[index])
        {
          luckyRoll = index;
          break;
        }
      }
    }
    yield return (object) currentDice.StartCoroutine((IEnumerator) currentDice.RollRoutine(1f, luckyRoll));
    yield return (object) new WaitForSecondsRealtime(0.5f);
    while ((UnityEngine.Object) this._exitConfirmationInstance != (UnityEngine.Object) null && (UnityEngine.Object) player == (UnityEngine.Object) this._player)
      yield return (object) null;
    yield return (object) null;
    yield return (object) player.SelectTub(currentDice);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if (this.CheckGameCompleted(player))
    {
      System.Action action = onCompleted;
      if (action != null)
        action();
    }
  }

  public IEnumerator DoEndGame()
  {
    this._matchCompleted = true;
    AudioManager.Instance.SetMusicRoomID(1, "ratau_home_id");
    int score = this._player.Score;
    int num = this._isCoop ? this._coopPlayer.Score : this._opponent.Score;
    if (this._isTwitch)
      num = this._twitchChat.Score;
    UIKnuckleBonesController.KnucklebonesResult result = UIKnuckleBonesController.KnucklebonesResult.Draw;
    if (score == num)
    {
      this._player.SetLostLoop();
      this._player.SetLostLoop();
      this._announcementText.text = KnucklebonesModel.GetLocalizedString("Draw");
      this._winningsText.gameObject.SetActive(false);
    }
    else
    {
      KBPlayerBase player = score > num ? (KBPlayerBase) this._player : (this._isCoop ? (KBPlayerBase) this._coopPlayer : (KBPlayerBase) this._opponent);
      if (this._isTwitch)
        player = score > num ? (KBPlayerBase) this._player : (this._isTwitch ? (KBPlayerBase) this._twitchChat : (KBPlayerBase) this._opponent);
      KBPlayerBase oppositePlayer = this.GetOppositePlayer(player);
      player.HighlightMe();
      oppositePlayer.HighlightMe();
      player.FinalizeScores();
      oppositePlayer.FinalizeScores();
      player.SetWonLoop();
      oppositePlayer.SetLostLoop();
      this._announcementText.text = $"{string.Format(KnucklebonesModel.GetLocalizedString("Win"), (object) player.GetLocalizedName())} {player.Score} - {oppositePlayer.Score}";
      this._winningsText.gameObject.SetActive(this._bet > 0);
      if (!this._isCoop && !this._isTwitch)
        this._opponent.PlayEndGameVoiceover(score > num);
      if (score > num)
      {
        result = UIKnuckleBonesController.KnucklebonesResult.Win;
        this._winningsText.text = $"+ {"<sprite name=\"icon_blackgold\">"} {this._bet}";
        AudioManager.Instance.PlayOneShot("event:/Stings/knucklebones_win");
      }
      else if (score < num)
      {
        result = UIKnuckleBonesController.KnucklebonesResult.Loss;
        this._winningsText.text = $"- {"<sprite name=\"icon_blackgold\">"} {this._bet}".Colour(StaticColors.RedColor);
        AudioManager.Instance.PlayOneShot("event:/Stings/knucklebones_lose");
      }
    }
    this._announcementRectTransform.position = this._announceTextStartingPosition + new Vector3(800f, 0.0f);
    this._announcementRectTransform.DOMove(this._announceTextStartingPosition, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._announcementCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1.5f);
    this._controlPrompts.HideCancelButton();
    this._controlPrompts.ShowAcceptButton();
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    Action<UIKnuckleBonesController.KnucklebonesResult> onMatchFinished = this.OnMatchFinished;
    if (onMatchFinished != null)
      onMatchFinished(result);
  }

  public KBPlayerBase GetOppositePlayer(KBPlayerBase player)
  {
    if (!((UnityEngine.Object) player == (UnityEngine.Object) this._player))
      return (KBPlayerBase) this._player;
    if (this._isCoop)
      return (KBPlayerBase) this._coopPlayer;
    return this._isTwitch ? (KBPlayerBase) this._twitchChat : (KBPlayerBase) this._opponent;
  }

  public bool CheckGameCompleted(KBPlayerBase player) => player.CheckGameCompleted();

  public void ForceEndGame()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoEndGame());
  }

  public KBGameScreen.TwitchMetadata GetBoardData(int dieToPlace)
  {
    string[] strArray = new string[4]
    {
      "default",
      "default",
      "double",
      "triple"
    };
    KBGameScreen.TwitchMetadata boardData = new KBGameScreen.TwitchMetadata();
    boardData.Id = this.uniqueID.ToString();
    boardData.DieValueToPlace = dieToPlace;
    boardData.Boards = new KBGameScreen.TwitchBoards()
    {
      Player = new KBGameScreen.TwitchPlayerBoard()
      {
        Columns = new KBGameScreen.TwitchColumn[3],
        Score = this._player.Score
      },
      Chat = new KBGameScreen.TwitchPlayerBoard()
      {
        Columns = new KBGameScreen.TwitchColumn[3],
        Score = this._twitchChat.Score
      }
    };
    for (int index1 = 0; index1 < 3; ++index1)
    {
      boardData.Boards.Player.Columns[index1] = new KBGameScreen.TwitchColumn()
      {
        Score = this._player.GetTubScore(index1),
        Dice = new KBGameScreen.TwitchDice[3]
      };
      List<int> tubDicesNumbers = this._player.GetTubDicesNumbers(index1);
      for (int index2 = 0; index2 < Mathf.Min(tubDicesNumbers.Count, 3); ++index2)
        boardData.Boards.Player.Columns[index1].Dice[index2] = new KBGameScreen.TwitchDice()
        {
          State = strArray[this._player.GetDuplicateCount(index1, index2)],
          Value = tubDicesNumbers[index2]
        };
    }
    for (int index3 = 0; index3 < 3; ++index3)
    {
      boardData.Boards.Chat.Columns[index3] = new KBGameScreen.TwitchColumn()
      {
        Score = this._twitchChat.GetTubScore(index3),
        Dice = new KBGameScreen.TwitchDice[3]
      };
      List<int> tubDicesNumbers = this._twitchChat.GetTubDicesNumbers(index3);
      for (int index4 = 0; index4 < Mathf.Min(tubDicesNumbers.Count, 3); ++index4)
        boardData.Boards.Chat.Columns[index3].Dice[index4] = new KBGameScreen.TwitchDice()
        {
          State = strArray[this._twitchChat.GetDuplicateCount(index3, index4)],
          Value = tubDicesNumbers[index4]
        };
    }
    return boardData;
  }

  [CompilerGenerated]
  public void \u003COnCancelButtonInput\u003Eb__32_0()
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.StopAllCoroutines();
    Action<UIKnuckleBonesController.KnucklebonesResult> onMatchFinished = this.OnMatchFinished;
    if (onMatchFinished == null)
      return;
    onMatchFinished(this._isCoop || this._isTwitch ? UIKnuckleBonesController.KnucklebonesResult.Quit : UIKnuckleBonesController.KnucklebonesResult.Loss);
  }

  [CompilerGenerated]
  public void \u003COnCancelButtonInput\u003Eb__32_2()
  {
    this._exitConfirmationInstance = (UIMenuConfirmationWindow) null;
  }

  [Serializable]
  public struct TwitchKnucklebonesGame
  {
    [CompilerGenerated]
    public string \u003CType\u003Ek__BackingField;
    [CompilerGenerated]
    public string \u003CPrompt\u003Ek__BackingField;
    [CompilerGenerated]
    public KBGameScreen.TwitchMetadata \u003CMetadata\u003Ek__BackingField;
    [CompilerGenerated]
    public KBGameScreen.TwitchOption[] \u003COptions\u003Ek__BackingField;

    public string Type
    {
      readonly get => this.\u003CType\u003Ek__BackingField;
      set => this.\u003CType\u003Ek__BackingField = value;
    }

    public string Prompt
    {
      readonly get => this.\u003CPrompt\u003Ek__BackingField;
      set => this.\u003CPrompt\u003Ek__BackingField = value;
    }

    public KBGameScreen.TwitchMetadata Metadata
    {
      readonly get => this.\u003CMetadata\u003Ek__BackingField;
      set => this.\u003CMetadata\u003Ek__BackingField = value;
    }

    public KBGameScreen.TwitchOption[] Options
    {
      readonly get => this.\u003COptions\u003Ek__BackingField;
      set => this.\u003COptions\u003Ek__BackingField = value;
    }
  }

  [Serializable]
  public class TwitchMetadata
  {
    public string Id;
    public int DieValueToPlace;
    public KBGameScreen.TwitchBoards Boards;
  }

  [Serializable]
  public class TwitchBoards
  {
    public KBGameScreen.TwitchPlayerBoard Player;
    public KBGameScreen.TwitchPlayerBoard Chat;
  }

  [Serializable]
  public class TwitchPlayerBoard
  {
    public int Score;
    public KBGameScreen.TwitchColumn[] Columns;
  }

  [Serializable]
  public class TwitchColumn
  {
    public int Score;
    public 
    #nullable enable
    KBGameScreen.TwitchDice?[] Dice;
  }

  [Serializable]
  public class TwitchDice
  {
    public 
    #nullable disable
    string State;
    public int Value;
  }

  [Serializable]
  public class TwitchOption
  {
    public string Id;
    public string Label;
  }
}
