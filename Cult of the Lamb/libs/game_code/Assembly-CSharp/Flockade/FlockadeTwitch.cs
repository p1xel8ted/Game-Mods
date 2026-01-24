// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeTwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeTwitch : FlockadeNpcBase
{
  public const float _VOTING_DURATION = 15f;
  public const float _VOTING_IN_PROGRESS_DOTS_STEP_DURATION = 0.5f;
  public const float _VOTING_IN_PROGRESS_FADE_DURATION = 0.25f;
  public static string[] _DOTS = new string[4]
  {
    string.Empty,
    ".",
    "..",
    "..."
  };
  [SerializeField]
  public Image _avatar;
  [SerializeField]
  [TermsPopup("")]
  public string _placePiecePrompt;
  [SerializeField]
  [TermsPopup("")]
  public string _selectPiecePrompt;
  [SerializeField]
  [TermsPopup("")]
  public string _victorySentence;
  [SerializeField]
  public Localize _votingInProgress;
  [SerializeField]
  public CanvasGroup _bottomContainer;
  public IFlockadeTwitchMetadataProvider _metadataProvider;
  public FlockadeGameBoardTile votedTile;

  public override Graphic Avatar => (Graphic) this._avatar;

  public override Color Highlight => StaticColors.TwitchPurple;

  public override string VictorySentence => this._victorySentence;

  public TextMeshProUGUI VotingText
  {
    get => ((LocalizeTarget<TextMeshProUGUI>) this._votingInProgress.mLocalizeTarget).mTarget;
  }

  public void Configure(
    IFlockadeTwitchMetadataProvider metadataProvider,
    FlockadeGameBoardSide side,
    FlockadeGamePieceBag bag,
    FlockadeControlPrompts controlPrompts,
    UIMenuBase parent)
  {
    this._metadataProvider = metadataProvider;
    this.Configure(side, bag, controlPrompts, parent);
  }

  public virtual void Start() => this.VotingText.alpha = 0.0f;

  public override IEnumerator Select(FlockadeGamePieceChoice[] choices)
  {
    IEnumerator coroutine;
    yield return (object) (coroutine = this.Vote((FlockadeGamePieceHolder[]) choices, Org.OpenAPITools.Model.PollType.FLOCKADEPIECEPLACEMENT, this._selectPiecePrompt));
    yield return coroutine.Current;
  }

  public override IEnumerator Place(FlockadeGameBoardTile[] availableTiles)
  {
    yield return (object) this.votedTile;
  }

  public IEnumerator Vote(FlockadeGamePieceHolder[] choices, Org.OpenAPITools.Model.PollType pollType, string promptTerm)
  {
    FlockadeGamePieceHolder flockadeGamePieceHolder;
    if (((IEnumerable<FlockadeGamePieceHolder>) choices).Select<FlockadeGamePieceHolder, Org.OpenAPITools.Model.PollOption>((Func<FlockadeGamePieceHolder, Org.OpenAPITools.Model.PollOption>) (choice => new Org.OpenAPITools.Model.PollOption(choice.Identifier, choice.LocalizedIdentifier))).ToList<Org.OpenAPITools.Model.PollOption>().Count > 1)
    {
      ShortcutExtensionsTMPText.DOFade(this.VotingText, 1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      FlockadeTwitchMetadata twitchMetadata = this._metadataProvider.GetTwitchMetadata();
      System.Threading.Tasks.Task poll = TwitchRequest.EBS_API.StartPollAsync(new Org.OpenAPITools.Model.StartPollRequest(new Org.OpenAPITools.Model.StartPollRequestType(pollType), LocalizationManager.GetTranslation(promptTerm), (object) twitchMetadata, new List<Org.OpenAPITools.Model.PollOption>()), new CancellationToken());
      yield return (object) this.AnimateDots();
      if (poll.IsCompleted)
      {
        Task<Org.OpenAPITools.Model.Poll> results = TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
        yield return (object) new WaitUntil((Func<bool>) (() => results.IsCompleted));
        results.Result.Options.Shuffle<Org.OpenAPITools.Model.PollOption>();
        Org.OpenAPITools.Model.PollOption option = results.Result.Options[0];
        List<Org.OpenAPITools.Model.PollOption> options = results.Result.Options;
        for (int index = 1; index < options.Count; ++index)
        {
          if (options[index].Votes > option.Votes)
            option = options[index];
        }
        string str = "";
        string region = "";
        bool flag = false;
        for (int index = 0; index < option.Id.Length; ++index)
        {
          if (option.Id[index] == ':')
            flag = true;
          else if (flag)
            region += option.Id[index].ToString();
          else
            str += option.Id[index].ToString();
        }
        this.votedTile = this._metadataProvider.GetTileFromRegionID(region);
        flockadeGamePieceHolder = choices[0];
        foreach (FlockadeGamePieceHolder choice in choices)
        {
          if (choice.GamePiece.Core.Configuration.Image.name == str)
            flockadeGamePieceHolder = choice;
        }
      }
      else
        flockadeGamePieceHolder = choices[UnityEngine.Random.Range(0, choices.Length)];
      ShortcutExtensionsTMPText.DOFade(this.VotingText, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      poll = (System.Threading.Tasks.Task) null;
    }
    else
      flockadeGamePieceHolder = choices[0];
    this._bottomContainer.gameObject.SetActive(true);
    this._bottomContainer.alpha = 0.0f;
    this._bottomContainer.DOFade(1f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) flockadeGamePieceHolder;
  }

  public IEnumerator AnimateDots()
  {
    float remainingTime = 15f;
    float timeBeforeNextStep = 0.5f;
    int count = 0;
    while ((double) (remainingTime -= Time.unscaledDeltaTime) > 0.0)
    {
      timeBeforeNextStep -= Time.unscaledDeltaTime;
      if ((double) timeBeforeNextStep <= 0.0)
      {
        count = (count + 1) % 4;
        this._votingInProgress.TermSuffix = FlockadeTwitch._DOTS[count];
        this._votingInProgress.OnLocalize(true);
        timeBeforeNextStep = 0.5f;
      }
      yield return (object) null;
    }
  }

  public override void OnBeforeTurn() => this._bottomContainer.gameObject.SetActive(false);

  public void OnDisable() => TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());

  public void OnDestroy() => TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
}
