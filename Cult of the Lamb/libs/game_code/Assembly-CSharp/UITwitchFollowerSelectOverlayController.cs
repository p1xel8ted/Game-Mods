// Decompiled with JetBrains decompiler
// Type: UITwitchFollowerSelectOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.UINavigator;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UITwitchFollowerSelectOverlayController : UIMenuBase
{
  [SerializeField]
  public UINavigatorFollowElement buttonHighlightController;
  [SerializeField]
  public GameObject _loadingContainer;
  [SerializeField]
  public GameObject _errorContainer;
  [SerializeField]
  public MMButton _acceptErrorButton;
  [SerializeField]
  public GameObject preRaffleContainer;
  [SerializeField]
  public MMButton startRaffleButton;
  [SerializeField]
  public GameObject activeRaffleContainer;
  [SerializeField]
  public MMButton _cancelRaffleButton;
  [SerializeField]
  public MMButton endRaffleButton;
  [SerializeField]
  public TMP_Text currentParticipants;
  [SerializeField]
  public GameObject waitingRaffleContainer;
  [SerializeField]
  public MMButton _cancelButton;
  [SerializeField]
  public TMP_Text resultName;
  [SerializeField]
  public TMP_Text totalParticipants;
  [SerializeField]
  public Image progressBar;
  [SerializeField]
  public SkeletonGraphic _waitingFollower;
  [SerializeField]
  public GameObject resultedRaffleContainer;
  [SerializeField]
  public MMButton continueButton;
  [SerializeField]
  public TMP_Text winnerChannelName;
  [SerializeField]
  public SkeletonGraphic winnerFollower;
  [SerializeField]
  public ParticleSystem _confettiLeft;
  [SerializeField]
  public ParticleSystem _confettiRight;
  public UIFollowerIndoctrinationMenuController _indoctrinationMenu;
  public UITwitchFollowerSelectOverlayController.State _currentState;
  public bool resulted;
  public int participants;

  public override void Awake()
  {
    base.Awake();
    this._confettiLeft.Stop();
    this._confettiRight.Stop();
    this._confettiLeft.Clear();
    this._confettiRight.Clear();
  }

  public void OnEnable()
  {
    TwitchFollowers.RaffleUpdated += new TwitchFollowers.RaffleResponse(this.RaffleUpdated);
    TwitchFollowers.FollowerCreated += new TwitchFollowers.FollowerResponse(this.FollowerCreated);
    TwitchFollowers.FollowerCreationProgress += new TwitchFollowers.FollowerResponse(this.FollowerCreationProgress);
  }

  public new void OnDisable()
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

  public void SetState(
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

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this.SetState(UITwitchFollowerSelectOverlayController.State.Pre);
    this.startRaffleButton.onClick.AddListener(new UnityAction(this.OnStartRaffleButtonClicked));
    this.endRaffleButton.onClick.AddListener(new UnityAction(this.OnEndRaffleButtonClicked));
    this._cancelButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this._acceptErrorButton.onClick.AddListener(new UnityAction(this.OnAcceptErrorButtonClicked));
    this._cancelRaffleButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
  }

  public void OnStartRaffleButtonClicked()
  {
    this.startRaffleButton.Interactable = false;
    TwitchFollowers.StartRaffle((TwitchFollowers.RaffleResponse) ((response, data) =>
    {
      if (response != TwitchRequest.ResponseType.Failure)
        return;
      TwitchFollowers.Abort();
      this.SetState(UITwitchFollowerSelectOverlayController.State.Error);
    }));
    this.SetState(UITwitchFollowerSelectOverlayController.State.Active);
  }

  public void OnEndRaffleButtonClicked()
  {
    this.endRaffleButton.interactable = false;
    TwitchFollowers.EndRaffle((TwitchFollowers.RaffleResponse) ((response, data) =>
    {
      if (response != TwitchRequest.ResponseType.Failure)
        return;
      TwitchFollowers.Abort();
      this.SetState(UITwitchFollowerSelectOverlayController.State.Error);
    }));
    this.SetState(UITwitchFollowerSelectOverlayController.State.Loading);
  }

  public void RaffleUpdated(TwitchRequest.ResponseType response, TwitchFollowers.RaffleData data)
  {
    if (data == null)
      return;
    if (data.participants == 0 && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == this.endRaffleButton)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._cancelRaffleButton);
    this.endRaffleButton.interactable = data.participants > 0;
    if (!string.IsNullOrEmpty(data.winning_viewer_display_name))
    {
      this.SetState(UITwitchFollowerSelectOverlayController.State.Waiting);
      this.SetWinner(data.winning_viewer_display_name);
    }
    this.participants = data.participants > 0 ? data.participants : this.participants;
    this.currentParticipants.text = $"{LocalizationManager.GetTranslation("UI/Twitch/Raffle/Participants")} {this.participants.ToString()}";
    this.totalParticipants.text = $"{LocalizationManager.GetTranslation("UI/Twitch/Raffle/TotalParticipants")} {this.participants.ToString()}";
  }

  public void SetWinner(string winnerName)
  {
    this.resultName.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Raffle/Winner"), (object) winnerName);
    this.winnerChannelName.text = string.Format(LocalizationManager.GetTranslation("UI/Twitch/Raffle/WinnerCreated"), (object) winnerName);
    this.resulted = true;
  }

  public void OnContinueButtonClicked() => this.Hide();

  public void FollowerCreated(TwitchFollowers.ViewerFollowerData data)
  {
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
    FollowerClothingType followerClothingType = FollowerClothingType.None;
    foreach (ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.Variants.Contains("Clothes/" + data.customisations.outfit_skin_name))
      {
        followerClothingType = clothingData.ClothingType;
        break;
      }
    }
    FollowerInfoSnapshot followerInfoSnapshot = new FollowerInfoSnapshot()
    {
      Name = data.viewer_display_name,
      SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(skinName),
      SkinVariation = num,
      SkinColour = colorOptionIndex,
      SkinName = skinName,
      Clothing = followerClothingType
    };
    this.winnerFollower.ConfigureFollowerSkin(followerInfoSnapshot);
    this._indoctrinationMenu.CreatedTwitchFollower(followerInfoSnapshot, data.viewer_display_name + data.created_at, data.id, data.viewer_id);
    this.SetWinner(data.viewer_display_name);
  }

  public void FollowerCreationProgress(TwitchFollowers.ViewerFollowerData data)
  {
    float endValue = 0.0f;
    if (data.customisation_step == "INTRO")
      endValue = 0.0f;
    else if (data.customisation_step == "SKIN_SELECTION")
      endValue = 0.2f;
    else if (data.customisation_step == "COLOR_SELECTION")
      endValue = 0.4f;
    else if (data.customisation_step == "VARIATION_SELECTION")
      endValue = 0.6f;
    else if (data.customisation_step == "OUTFIT_SELECTION")
      endValue = 0.8f;
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
    FollowerClothingType followerClothingType = FollowerClothingType.None;
    string outfitSkinName = data.customisations.outfit_skin_name;
    foreach (ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.Variants.Contains(outfitSkinName))
      {
        followerClothingType = clothingData.ClothingType;
        break;
      }
    }
    this._waitingFollower.ConfigureFollowerSkin(new FollowerInfoSnapshot()
    {
      Name = data.viewer_display_name,
      SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(skinName),
      SkinVariation = num,
      SkinColour = colorOptionIndex,
      SkinName = skinName,
      Clothing = followerClothingType,
      ClothingVariant = outfitSkinName == "None" ? "" : outfitSkinName
    });
  }

  public void OnAcceptErrorButtonClicked() => this.Hide();

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
    TwitchFollowers.Abort();
  }

  [CompilerGenerated]
  public void \u003COnStartRaffleButtonClicked\u003Eb__33_0(
    TwitchRequest.ResponseType response,
    TwitchFollowers.RaffleData data)
  {
    if (response != TwitchRequest.ResponseType.Failure)
      return;
    TwitchFollowers.Abort();
    this.SetState(UITwitchFollowerSelectOverlayController.State.Error);
  }

  [CompilerGenerated]
  public void \u003COnEndRaffleButtonClicked\u003Eb__34_0(
    TwitchRequest.ResponseType response,
    TwitchFollowers.RaffleData data)
  {
    if (response != TwitchRequest.ResponseType.Failure)
      return;
    TwitchFollowers.Abort();
    this.SetState(UITwitchFollowerSelectOverlayController.State.Error);
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
