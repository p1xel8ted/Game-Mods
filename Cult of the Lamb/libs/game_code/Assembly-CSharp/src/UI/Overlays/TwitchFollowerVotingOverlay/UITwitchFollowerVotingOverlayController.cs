// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.TwitchFollowerVotingOverlay.UITwitchFollowerVotingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Overlays.TwitchFollowerVotingOverlay;

public class UITwitchFollowerVotingOverlayController : UIMenuBase
{
  public Action<FollowerBrain> OnFollowerChosen;
  [SerializeField]
  public UINavigatorFollowElement buttonHighlightController;
  [SerializeField]
  public GameObject _loadingContainer;
  [SerializeField]
  public GameObject _activeVotingContainer;
  [SerializeField]
  public MMButton _cancelVoting;
  [SerializeField]
  public MMButton _endVoting;
  [SerializeField]
  public TMP_Text _totalVotesText;
  [SerializeField]
  public GameObject _resultContainer;
  [SerializeField]
  public MMButton _continueButton;
  [SerializeField]
  public SkeletonGraphic _chosenFollowerGraphic;
  [SerializeField]
  public ParticleSystem _confettiLeft;
  [SerializeField]
  public ParticleSystem _confettiRight;
  [SerializeField]
  public TMP_Text _votedForText;
  [SerializeField]
  public GameObject _errorContianer;
  [SerializeField]
  public MMButton _acceptErrorButton;
  public FollowerBrain _chosenFollower;
  public UITwitchFollowerVotingOverlayController.State _currentState;
  public int _finalTotalVotes;

  public override void Awake()
  {
    base.Awake();
    this._confettiLeft.Stop();
    this._confettiRight.Stop();
    this._confettiLeft.Clear();
    this._confettiRight.Clear();
  }

  public void Show(
    List<FollowerInfo> followerInfos,
    TwitchVoting.VotingType votingType,
    bool instant)
  {
    this.Show(instant);
    this._finalTotalVotes = 0;
    this.SetState(UITwitchFollowerVotingOverlayController.State.Loading);
    TwitchVoting.StartVoting(votingType, followerInfos, (TwitchVoting.VotingReadyResponse) (result =>
    {
      if (result)
        this.SetState(UITwitchFollowerVotingOverlayController.State.Voting);
      else
        this.SetState(UITwitchFollowerVotingOverlayController.State.Error);
    }));
    TwitchVoting.OnVotingUpdated += (TwitchVoting.VotingEvent) (totalVotes =>
    {
      this._finalTotalVotes = totalVotes;
      this._totalVotesText.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Voting/Votes"), (object) totalVotes.ToString());
    });
  }

  public void SetState(
    UITwitchFollowerVotingOverlayController.State state)
  {
    this.buttonHighlightController.gameObject.SetActive(state != 0);
    this._loadingContainer.SetActive(state == UITwitchFollowerVotingOverlayController.State.Loading);
    this._activeVotingContainer.SetActive(state == UITwitchFollowerVotingOverlayController.State.Voting);
    this._resultContainer.SetActive(state == UITwitchFollowerVotingOverlayController.State.Result);
    this._errorContianer.SetActive(state == UITwitchFollowerVotingOverlayController.State.Error);
    this._totalVotesText.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Voting/Votes"), (object) 0);
    this._currentState = state;
    if (state == UITwitchFollowerVotingOverlayController.State.Voting)
      this.OverrideDefault((Selectable) this._endVoting);
    else if (state == UITwitchFollowerVotingOverlayController.State.Result)
    {
      this._confettiLeft.Play();
      this._confettiRight.Play();
      this.OverrideDefault((Selectable) this._continueButton);
    }
    else if (state == UITwitchFollowerVotingOverlayController.State.Error)
      this.OverrideDefault((Selectable) this._acceptErrorButton);
    if (state == UITwitchFollowerVotingOverlayController.State.Loading)
      return;
    this.ActivateNavigation();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this._cancelVoting.onClick.AddListener(new UnityAction(this.OnCancelVotingButtonClicked));
    this._endVoting.onClick.AddListener(new UnityAction(this.OnEndVotingButtonClicked));
    this._continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this._acceptErrorButton.onClick.AddListener(new UnityAction(this.OnAcceptErrorButtonClicked));
  }

  public void OnCancelVotingButtonClicked()
  {
    TwitchVoting.Abort();
    this.Hide();
  }

  public void OnEndVotingButtonClicked()
  {
    if (this._finalTotalVotes <= 0)
    {
      this.OnCancelVotingButtonClicked();
    }
    else
    {
      this.SetState(UITwitchFollowerVotingOverlayController.State.Loading);
      this._votedForText.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Voting/Result"), (object) "");
      TwitchVoting.EndVoting((TwitchVoting.VotingResponse) (result =>
      {
        if (result != null)
        {
          this.SetState(UITwitchFollowerVotingOverlayController.State.Result);
          this._chosenFollower = result;
          this._chosenFollowerGraphic.ConfigureFollower(this._chosenFollower._directInfoAccess);
          this._votedForText.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Voting/Result"), (object) ("<color=#FFD201>" + this._chosenFollower.Info.Name));
        }
        else
          this.SetState(UITwitchFollowerVotingOverlayController.State.Error);
      }));
    }
  }

  public void OnContinueButtonClicked() => this.Hide();

  public void OnAcceptErrorButtonClicked() => this.Hide();

  public override void OnHideStarted()
  {
    if (this._chosenFollower == null)
      return;
    Action<FollowerBrain> onFollowerChosen = this.OnFollowerChosen;
    if (onFollowerChosen == null)
      return;
    onFollowerChosen(this._chosenFollower);
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable)
      return;
    if (this._currentState == UITwitchFollowerVotingOverlayController.State.Voting)
      this.OnCancelVotingButtonClicked();
    else if (this._currentState == UITwitchFollowerVotingOverlayController.State.Result)
    {
      this.Hide();
    }
    else
    {
      if (this._currentState != UITwitchFollowerVotingOverlayController.State.Error)
        return;
      this.Hide();
    }
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__20_0(bool result)
  {
    if (result)
      this.SetState(UITwitchFollowerVotingOverlayController.State.Voting);
    else
      this.SetState(UITwitchFollowerVotingOverlayController.State.Error);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__20_1(int totalVotes)
  {
    this._finalTotalVotes = totalVotes;
    this._totalVotesText.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Voting/Votes"), (object) totalVotes.ToString());
  }

  [CompilerGenerated]
  public void \u003COnEndVotingButtonClicked\u003Eb__24_0(FollowerBrain result)
  {
    if (result != null)
    {
      this.SetState(UITwitchFollowerVotingOverlayController.State.Result);
      this._chosenFollower = result;
      this._chosenFollowerGraphic.ConfigureFollower(this._chosenFollower._directInfoAccess);
      this._votedForText.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Voting/Result"), (object) ("<color=#FFD201>" + this._chosenFollower.Info.Name));
    }
    else
      this.SetState(UITwitchFollowerVotingOverlayController.State.Error);
  }

  public enum State
  {
    Loading,
    Voting,
    Result,
    Error,
  }
}
