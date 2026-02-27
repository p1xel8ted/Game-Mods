// Decompiled with JetBrains decompiler
// Type: UITwitchFollowerSelectOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.UINavigator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UITwitchFollowerSelectOverlayController : UIMenuBase
{
  [SerializeField]
  private UINavigatorFollowElement buttonHighlightController;
  [SerializeField]
  private GameObject _loadingContainer;
  [SerializeField]
  private GameObject _errorContainer;
  [SerializeField]
  private MMButton _acceptErrorButton;
  [SerializeField]
  private GameObject preRaffleContainer;
  [SerializeField]
  private MMButton startRaffleButton;
  [SerializeField]
  private GameObject activeRaffleContainer;
  [SerializeField]
  private MMButton _cancelRaffleButton;
  [SerializeField]
  private MMButton endRaffleButton;
  [SerializeField]
  private TMP_Text currentParticipants;
  [SerializeField]
  private GameObject waitingRaffleContainer;
  [SerializeField]
  private MMButton _cancelButton;
  [SerializeField]
  private TMP_Text resultName;
  [SerializeField]
  private TMP_Text totalParticipants;
  [SerializeField]
  private Image progressBar;
  [SerializeField]
  private SkeletonGraphic _waitingFollower;
  [SerializeField]
  private GameObject resultedRaffleContainer;
  [SerializeField]
  private MMButton continueButton;
  [SerializeField]
  private TMP_Text winnerChannelName;
  [SerializeField]
  private SkeletonGraphic winnerFollower;
  [SerializeField]
  private ParticleSystem _confettiLeft;
  [SerializeField]
  private ParticleSystem _confettiRight;
  private UIFollowerIndoctrinationMenuController _indoctrinationMenu;
  private UITwitchFollowerSelectOverlayController.State _currentState;
  private bool resulted;

  public override void Awake()
  {
    base.Awake();
    this._confettiLeft.Stop();
    this._confettiRight.Stop();
    this._confettiLeft.Clear();
    this._confettiRight.Clear();
  }

  private void OnEnable()
  {
    TwitchFollowers.RaffleUpdated += new TwitchFollowers.RaffleResponse(this.RaffleUpdated);
    TwitchFollowers.FollowerCreated += new TwitchFollowers.FollowerResponse(this.FollowerCreated);
    TwitchFollowers.FollowerCreationProgress += new TwitchFollowers.FollowerResponse(this.FollowerCreationProgress);
  }

  private void OnDisable()
  {
    TwitchFollowers.RaffleUpdated -= new TwitchFollowers.RaffleResponse(this.RaffleUpdated);
    TwitchFollowers.FollowerCreated -= new TwitchFollowers.FollowerResponse(this.FollowerCreated);
    TwitchFollowers.FollowerCreationProgress -= new TwitchFollowers.FollowerResponse(this.FollowerCreationProgress);
  }

  public void Show(
    UIFollowerIndoctrinationMenuController indoctrinationMenu,
    bool immediate = false)
  {
    this.currentParticipants.text = LocalizationManager.GetTranslation("UI/Twitch/Raffle/Participants") + " 0";
    this.totalParticipants.text = LocalizationManager.GetTranslation("UI/Twitch/Raffle/TotalParticipants") + " 0";
    this._indoctrinationMenu = indoctrinationMenu;
    this.Show(immediate);
  }

  private void SetState(
    UITwitchFollowerSelectOverlayController.State state)
  {
    this.buttonHighlightController.gameObject.SetActive(state != UITwitchFollowerSelectOverlayController.State.Loading);
    this._loadingContainer.SetActive(state == UITwitchFollowerSelectOverlayController.State.Loading);
    this._errorContainer.SetActive(state == UITwitchFollowerSelectOverlayController.State.Error);
    this.preRaffleContainer.SetActive(state == UITwitchFollowerSelectOverlayController.State.Pre);
    this.activeRaffleContainer.SetActive(state == UITwitchFollowerSelectOverlayController.State.Active);
    this.waitingRaffleContainer.SetActive(state == UITwitchFollowerSelectOverlayController.State.Waiting);
    this.resultedRaffleContainer.SetActive(state == UITwitchFollowerSelectOverlayController.State.Resulted);
    if (state == UITwitchFollowerSelectOverlayController.State.Pre)
      this.OverrideDefault((Selectable) this.startRaffleButton);
    else if (state == UITwitchFollowerSelectOverlayController.State.Active)
    {
      this.OverrideDefault((Selectable) this._cancelRaffleButton);
      this.endRaffleButton.interactable = false;
    }
    else if (state == UITwitchFollowerSelectOverlayController.State.Waiting)
    {
      this._confettiLeft.Play();
      this._confettiRight.Play();
      this.OverrideDefault((Selectable) this._cancelButton);
    }
    else if (state == UITwitchFollowerSelectOverlayController.State.Resulted)
    {
      this._confettiLeft.Play();
      this._confettiRight.Play();
      this.OverrideDefault((Selectable) this.continueButton);
    }
    else if (state == UITwitchFollowerSelectOverlayController.State.Error)
      this.OverrideDefault((Selectable) this._acceptErrorButton);
    if (state == UITwitchFollowerSelectOverlayController.State.Loading)
      return;
    this.ActivateNavigation();
  }

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    this.SetState(UITwitchFollowerSelectOverlayController.State.Loading);
    TwitchFollowers.GetFollowersAll((TwitchFollowers.FollowerAllResponse) (data =>
    {
      bool flag = false;
      foreach (TwitchFollowers.ViewerFollowerData data1 in data)
      {
        if (data1 != null)
        {
          string str = data1.viewer_display_name + data1.created_at;
          if (data1.status == "CREATED" && !DataManager.Instance.TwitchFollowerViewerIDs.Contains(str) && !DataManager.Instance.TwitchFollowerIDs.Contains(data1.id))
          {
            this.FollowerCreated(data1);
            flag = true;
            break;
          }
        }
      }
      if (flag || this.resulted)
        return;
      this.SetState(UITwitchFollowerSelectOverlayController.State.Pre);
    }));
    this.startRaffleButton.onClick.AddListener(new UnityAction(this.OnStartRaffleButtonClicked));
    this.endRaffleButton.onClick.AddListener(new UnityAction(this.OnEndRaffleButtonClicked));
    this._cancelButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this._acceptErrorButton.onClick.AddListener(new UnityAction(this.OnAcceptErrorButtonClicked));
    this._cancelRaffleButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
  }

  private void OnStartRaffleButtonClicked()
  {
    this.startRaffleButton.Interactable = false;
    TwitchFollowers.StartRaffle((TwitchFollowers.RaffleResponse) ((response, data) =>
    {
      if (response != TwitchRequest.ResponseType.Failure)
        return;
      this.SetState(UITwitchFollowerSelectOverlayController.State.Error);
    }));
    this.SetState(UITwitchFollowerSelectOverlayController.State.Active);
  }

  private void OnEndRaffleButtonClicked()
  {
    this.endRaffleButton.interactable = false;
    TwitchFollowers.EndRaffle((TwitchFollowers.RaffleResponse) ((response, data) =>
    {
      if (response != TwitchRequest.ResponseType.Failure)
        return;
      this.SetState(UITwitchFollowerSelectOverlayController.State.Error);
    }));
    this.SetState(UITwitchFollowerSelectOverlayController.State.Loading);
  }

  private void RaffleUpdated(TwitchRequest.ResponseType response, TwitchFollowers.RaffleData data)
  {
    if (data.participants == 0 && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == this.endRaffleButton)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._cancelRaffleButton);
    this.endRaffleButton.interactable = data.participants > 0;
    if (!string.IsNullOrEmpty(data.created_follower.viewer_display_name))
    {
      this.SetState(UITwitchFollowerSelectOverlayController.State.Waiting);
      this.SetWinner(data.created_follower.viewer_display_name);
    }
    this.currentParticipants.text = $"{LocalizationManager.GetTranslation("UI/Twitch/Raffle/Participants")} {(object) data.participants}";
    this.totalParticipants.text = $"{LocalizationManager.GetTranslation("UI/Twitch/Raffle/TotalParticipants")} {(object) data.participants}";
  }

  private void SetWinner(string winnerName)
  {
    this.resultName.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Raffle/Winner"), (object) winnerName);
    this.winnerChannelName.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Raffle/WinnerCreated"), (object) winnerName);
    this.resulted = true;
  }

  private void OnContinueButtonClicked()
  {
    this.Hide();
    TwitchFollowers.WaitingForCreationCancelled();
  }

  private void FollowerCreated(TwitchFollowers.ViewerFollowerData data)
  {
    TwitchFollowers.WaitingForCreationCancelled();
    this.SetState(UITwitchFollowerSelectOverlayController.State.Resulted);
    if (data.customisations.skin_name == "HorseKing")
      data.customisations.skin_name = "Horse1";
    string skinName = data.customisations.skin_name;
    int num = 0;
    if (char.IsDigit(skinName[skinName.Length - 1]) && skinName[skinName.Length - 2] != ' ')
    {
      num = int.Parse(skinName[skinName.Length - 1].ToString()) - 1;
      skinName = skinName.Remove(skinName.Length - 1);
    }
    int colorOptionIndex = data.customisations.color.colorOptionIndex;
    FollowerInfoSnapshot followerInfoSnapshot = new FollowerInfoSnapshot()
    {
      Name = data.viewer_display_name,
      SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(skinName),
      SkinVariation = num,
      SkinColour = colorOptionIndex,
      SkinName = skinName
    };
    this.winnerFollower.ConfigureFollowerSkin(followerInfoSnapshot);
    this._indoctrinationMenu.CreatedTwitchFollower(followerInfoSnapshot, data.viewer_display_name + data.created_at, data.id, data.viewer_id);
    this.SetWinner(data.viewer_display_name);
  }

  private void FollowerCreationProgress(TwitchFollowers.ViewerFollowerData data)
  {
    float endValue = 0.0f;
    if (data.customisation_step == "INTRO")
      endValue = 0.0f;
    else if (data.customisation_step == "SKIN_SELECTION")
      endValue = 0.25f;
    else if (data.customisation_step == "COLOR_SELECTION")
      endValue = 0.5f;
    else if (data.customisation_step == "VARIATION_SELECTION")
      endValue = 0.75f;
    this.progressBar.DOKill();
    this.progressBar.DOFillAmount(endValue, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack);
    if (data.customisations == null || string.IsNullOrEmpty(data.customisations.skin_name))
      return;
    if (data.customisations.skin_name == "HorseKing")
      data.customisations.skin_name = "Horse1";
    string skinName = data.customisations.skin_name;
    int num = 0;
    if (char.IsDigit(skinName[skinName.Length - 1]) && skinName[skinName.Length - 2] != ' ')
    {
      num = int.Parse(skinName[skinName.Length - 1].ToString()) - 1;
      skinName = skinName.Remove(skinName.Length - 1);
    }
    int colorOptionIndex = data.customisations.color != null ? data.customisations.color.colorOptionIndex : 0;
    this._waitingFollower.ConfigureFollowerSkin(new FollowerInfoSnapshot()
    {
      Name = data.viewer_display_name,
      SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(skinName),
      SkinVariation = num,
      SkinColour = colorOptionIndex,
      SkinName = skinName
    });
  }

  private void OnAcceptErrorButtonClicked() => this.Hide();

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
    TwitchFollowers.WaitingForCreationCancelled();
  }

  public enum State
  {
    Pre,
    Active,
    Waiting,
    Resulted,
    Loading,
    Error,
  }
}
