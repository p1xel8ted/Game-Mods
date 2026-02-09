// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBOpponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using KnuckleBones;
using Org.OpenAPITools.Model;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class KBOpponent : KBPlayerBase
{
  [SerializeField]
  public SkeletonGraphic _opponentSpine;
  [SerializeField]
  public TMP_Text _votingInProgressText;
  public KBOpponentAI _ai;
  public KnucklebonesOpponent _opponent;
  [CompilerGenerated]
  public bool \u003CIsTwitchChat\u003Ek__BackingField;

  public override string _playDiceAnimation => "knucklebones/play-dice";

  public override string _playerIdleAnimation => "animation";

  public override string _playerTakeDiceAnimation => "knucklebones/take-dice";

  public override string _playerLostDiceAnimation => "knucklebones/lose-dice";

  public override string _playerWonAnimation => "knucklebones/win-game";

  public override string _playerWonLoop => "knucklebones/win-game-loop";

  public override string _playerLostAnimation => "knucklebones/lose-game";

  public override string _playerLostLoop => "knucklebones/lose-game-loop";

  public override SkeletonGraphic _spine => this._opponentSpine;

  public SkeletonGraphic Spine => this._spine;

  public bool IsTwitchChat
  {
    get => this.\u003CIsTwitchChat\u003Ek__BackingField;
    set => this.\u003CIsTwitchChat\u003Ek__BackingField = value;
  }

  public void Configure(
    KnucklebonesOpponent opponent,
    Vector2 contentOffscreenOffset,
    Vector2 tubOffscreenOffset)
  {
    this._opponent = opponent;
    this._ai = this._opponent.Config.CreateAI();
    this._playerName = this.GetLocalizedName();
    this._opponentSpine.skeletonDataAsset = opponent.Config.Spine;
    this._opponentSpine.initialSkinName = string.Empty;
    this._opponentSpine.startingAnimation = string.Empty;
    this._opponentSpine.Initialize(true);
    this._opponentSpine.Skeleton.SetSkin(opponent.Config.InitialSkinName);
    this._opponentSpine.AnimationState.SetAnimation(0, opponent.IsFollower ? "knucklebones/idle" : this._playerIdleAnimation, true);
    this._opponentSpine.transform.localPosition = (Vector3) opponent.Config.PositionOffset;
    this._opponentSpine.transform.localScale = (Vector3) opponent.Config.Scale;
    if ((UnityEngine.Object) this._votingInProgressText != (UnityEngine.Object) null)
      this._votingInProgressText.alpha = 0.0f;
    this.Configure(contentOffscreenOffset, tubOffscreenOffset);
  }

  public override IEnumerator SelectTub(Dice dice)
  {
    KBOpponent kbOpponent = this;
    KBDiceTub diceTub;
    if (kbOpponent.IsTwitchChat)
    {
      string[] ids = new string[3]
      {
        "LEFT",
        "MIDDLE",
        "RIGHT"
      };
      string[] strArray = new string[3]
      {
        LocalizationManager.GetTranslation("UI/Twitch/KB/Left"),
        LocalizationManager.GetTranslation("UI/Twitch/KB/Middle"),
        LocalizationManager.GetTranslation("UI/Twitch/KB/Right")
      };
      List<PollOption> options = new List<PollOption>();
      for (int index = 0; index < 3; ++index)
      {
        if (kbOpponent._diceTubs[index].Dice.Count < 3)
          options.Add(new PollOption(ids[index], strArray[index]));
      }
      if (options.Count > 1)
      {
        ShortcutExtensionsTMPText.DOFade(kbOpponent._votingInProgressText, 1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        KBGameScreen.TwitchMetadata boardData = kbOpponent.GetComponentInParent<KBGameScreen>().GetBoardData(dice.Num);
        StartPollRequest startPollRequest = new StartPollRequest(new StartPollRequestType(PollType.KNUCKLEBONES), LocalizationManager.GetTranslation("Twitch/Knucklebones/DecideTub"), (object) boardData, options);
        System.Threading.Tasks.Task poll = TwitchRequest.EBS_API.StartPollAsync(startPollRequest, new CancellationToken());
        float duration = 7f;
        float timer = 0.5f;
        int count = 0;
        while ((double) (duration -= Time.unscaledDeltaTime) > 0.0)
        {
          timer -= Time.unscaledDeltaTime;
          if ((double) timer <= 0.0)
          {
            timer = 0.5f;
            ++count;
            if (count > 3)
              count = 0;
            kbOpponent._votingInProgressText.text = LocalizationManager.GetTranslation("UI/Twitch/Voting/InProgress");
            for (int index = 0; index < count; ++index)
              kbOpponent._votingInProgressText.text += ".";
          }
          yield return (object) null;
        }
        if (poll.IsCompleted)
        {
          Task<Poll> pollResults = TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
          yield return (object) new WaitUntil((Func<bool>) (() => pollResults.IsCompleted));
          pollResults.Result.Options.Shuffle<PollOption>();
          PollOption option = pollResults.Result.Options[0];
          for (int index = 1; index < pollResults.Result.Options.Count; ++index)
          {
            if (pollResults.Result.Options[index].Votes > option.Votes)
              option = pollResults.Result.Options[index];
          }
          diceTub = kbOpponent._diceTubs[ids.IndexOf<string>(option.Id)];
        }
        else
          diceTub = kbOpponent._diceTubs[UnityEngine.Random.Range(0, kbOpponent._diceTubs.Count)];
        ShortcutExtensionsTMPText.DOFade(kbOpponent._votingInProgressText, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        poll = (System.Threading.Tasks.Task) null;
      }
      else
        diceTub = kbOpponent._diceTubs[ids.IndexOf<string>(options[0].Id)];
      ids = (string[]) null;
    }
    else
    {
      int index = kbOpponent._ai.Evaluate(kbOpponent._diceTubs, dice);
      diceTub = kbOpponent._diceTubs[index];
    }
    yield return (object) kbOpponent.FinishTubSelection(dice, diceTub);
  }

  public void PlayEndGameVoiceover(bool victory)
  {
    if (victory)
      AudioManager.Instance.PlayOneShot(this._opponent.Config.DefeatAudio);
    else
      AudioManager.Instance.PlayOneShot(this._opponent.Config.VictoryAudio);
  }

  public override string GetLocalizedName()
  {
    string translation = LocalizationManager.GetTranslation(this._opponent.Config.OpponentName);
    return this.IsTwitchChat ? translation.Colour(StaticColors.TwitchPurple) : translation;
  }

  public override void OnDiceMatched()
  {
    this.tookDice = true;
    this._spine.AnimationState.SetAnimation(0, this._playerTakeDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._opponent.IsFollower ? "knucklebones/idle" : this._playerIdleAnimation, true, 0.0f);
  }

  public override void OnDiceLost()
  {
    this._scoreText.text = this.Score.ToString();
    this._spine.AnimationState.SetAnimation(0, this._playerLostDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._opponent.IsFollower ? "knucklebones/idle" : this._playerIdleAnimation, true, 0.0f);
  }

  public override IEnumerator FinishTubSelection(Dice dice, KBDiceTub diceTub)
  {
    KBOpponent kbOpponent = this;
    yield return (object) diceTub.AddDice(dice);
    if (!kbOpponent.tookDice)
    {
      kbOpponent._spine.AnimationState.SetAnimation(0, kbOpponent._playDiceAnimation, false);
      kbOpponent._spine.AnimationState.AddAnimation(0, kbOpponent._opponent.IsFollower ? "knucklebones/idle" : kbOpponent._playerIdleAnimation, true, 0.0f);
    }
    kbOpponent.tookDice = false;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    kbOpponent._scoreText.text = kbOpponent.Score.ToString();
  }
}
