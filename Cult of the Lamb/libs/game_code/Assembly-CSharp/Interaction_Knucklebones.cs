// Decompiled with JetBrains decompiler
// Type: Interaction_Knucklebones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.Tarot;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.UI.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Knucklebones : Interaction
{
  public List<KnucklebonesOpponent> KnucklebonePlayers = new List<KnucklebonesOpponent>();
  public KnucklebonesOpponent KnuckleboneOpponent;
  public GameObject Exit;
  public GameObject Blocker;
  [SerializeField]
  public Interaction_SimpleConversation knuckleboneConvo;
  [SerializeField]
  public Interaction_SimpleConversation lostGameConvo;
  [SerializeField]
  public Interaction_SimpleConversation playAgainConvo;
  [SerializeField]
  public Interaction_SimpleConversation wonGameConvo;
  [SerializeField]
  public SkeletonAnimation ratauSpine;
  public GoopFade goopTransition;
  [SerializeField]
  public KnucklebonesPlayerConfiguration knuckleBonesPlayerConfig;
  [SerializeField]
  public GameObject[] playerPositions;
  public UIKnuckleBonesController _knuckleBonesInstance;
  public List<KnucklebonesOpponent> _availableOpponents = new List<KnucklebonesOpponent>();
  public int _betAmount;
  public Interaction_TarotCardUnlock t;
  public GameObject CareToBetConversation;
  public Interaction_KeyPiece KeyPiecePrefab;

  public bool canPlayRatauStaffWinConvo
  {
    get
    {
      return this.KnuckleboneOpponent.Tag == KnucklebonesOpponent.OppnentTags.Ratau && DataManager.Instance.MAJOR_DLC && !DataManager.Instance.RatauKilled && DataManager.Instance.RatauIntroWoolhaven && !DataManager.Instance.RatauStaffQuestWonGame && !this.wonGameConvo.Spoken;
    }
  }

  public bool canPlayRatauStaffLoseConvo
  {
    get
    {
      return this.KnuckleboneOpponent.Tag == KnucklebonesOpponent.OppnentTags.Ratau && DataManager.Instance.MAJOR_DLC && !DataManager.Instance.RatauKilled && DataManager.Instance.RatauIntroWoolhaven && !DataManager.Instance.RatauStaffQuestWonGame && !this.lostGameConvo.Spoken;
    }
  }

  public void Start()
  {
    this.Interactable = this.GetAvailableOpponents().Count > 0;
    this.UpdateLocalisation();
    if (DataManager.Instance.MAJOR_DLC && DataManager.Instance.KnucklebonesIntroCompleted && !DataManager.Instance.RatauKilled && DataManager.Instance.RatauIntroWoolhaven && !DataManager.Instance.RatauStaffQuestWonGame)
    {
      if (DataManager.Instance.RatauStaffQuestGameConvoPlayed)
      {
        this.playAgainConvo.gameObject.SetActive(true);
      }
      else
      {
        this.knuckleboneConvo.gameObject.SetActive(true);
        this.knuckleboneConvo.Callback.AddListener((UnityAction) (() => DataManager.Instance.RatauStaffQuestGameConvoPlayed = true));
      }
    }
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Sword_Ratau))
    {
      this.ratauSpine?.Skeleton.FindSlot("STICK").Attachment = (Attachment) null;
      this.ratauSpine?.Skeleton.FindSlot("HAND").Attachment = (Attachment) null;
    }
    if (!DataManager.Instance.UnlockedFleeces.Contains(11))
      return;
    this.ratauSpine?.Skeleton.SetSkin("naked");
    this.ratauSpine?.AnimationState.SetAnimation(0, "idle-shy", true);
    this.SetNakedRatauConvoTalkAnims();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    if (!this.Interactable)
      this.Label = "";
    else
      this.Label = ScriptLocalization.Interactions.PlayKnucklebones;
  }

  public override void GetLabel()
  {
    if (!this.Interactable)
      this.Label = "";
    else
      this.Label = ScriptLocalization.Interactions.PlayKnucklebones;
    this.HasSecondaryInteraction = PlayerFarming.playersCount > 1;
    this.HasThirdInteraction = TwitchAuthentication.IsAuthenticated;
    this.ThirdInteractable = TwitchAuthentication.IsAuthenticated;
  }

  public override void GetThirdLabel()
  {
    base.GetThirdLabel();
    this.ThirdLabel = LocalizationManager.GetTranslation("UI/Twitch/VsChat").Colour(StaticColors.TwitchPurple) + " <sprite name=\"icon_TwitchIcon\">";
  }

  public override void OnThirdInteract(StateMachine state)
  {
    base.OnThirdInteract(state);
    int num = state.GetComponent<PlayerFarming>().isLamb ? 1 : 0;
    this.StartCoroutine((IEnumerator) this.PlayMatchTwitch());
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this._availableOpponents.Clear();
    this._availableOpponents = this.GetAvailableOpponents();
  }

  public List<KnucklebonesOpponent> GetAvailableOpponents()
  {
    List<KnucklebonesOpponent> availableOpponents = new List<KnucklebonesOpponent>();
    List<KnucklebonesOpponent> list = this.KnucklebonePlayers.ToList<KnucklebonesOpponent>();
    if (DataManager.Instance.RatauKilled)
      list.Remove(list.LastElement<KnucklebonesOpponent>());
    foreach (KnucklebonesOpponent knucklebonesOpponent in list)
    {
      if (knucklebonesOpponent.Config.VariableToShow != DataManager.Variables.Knucklebones_Opponent_Ratau_Won)
      {
        Debug.Log((object) ("Variable Cast: " + knucklebonesOpponent.Config.VariableToShow.ToString()));
        if (!DataManager.Instance.GetVariable(knucklebonesOpponent.Config.VariableToShow))
        {
          Debug.Log((object) ("NPC locked" + knucklebonesOpponent.Spine.gameObject.name));
          knucklebonesOpponent.Spine.gameObject.SetActive(false);
        }
        else
        {
          Debug.Log((object) ("NPC unlocked" + knucklebonesOpponent.Spine.gameObject.name));
          knucklebonesOpponent.Spine.gameObject.SetActive(true);
          availableOpponents.Add(knucklebonesOpponent);
        }
      }
      else
        availableOpponents.Add(knucklebonesOpponent);
    }
    return availableOpponents;
  }

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this._availableOpponents.Count <= 0)
      return;
    this.StartCoroutine((IEnumerator) this.SelectOpponent());
  }

  public void GameQuit()
  {
    Debug.Log((object) "quitGame");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void CompleteGame(UIKnuckleBonesController.KnucklebonesResult result)
  {
    Debug.Log((object) "COMPLETE GAME!");
    this.GameQuit();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    switch (result)
    {
      case UIKnuckleBonesController.KnucklebonesResult.Win:
        bool flag = false;
        if (!DataManager.Instance.GetVariable(this.KnuckleboneOpponent.Config.VariableToChangeOnWin))
        {
          Debug.Log((object) "Show First Win Convo");
          this.enabled = false;
          this.KnuckleboneOpponent.FirstWinConvo.Callback.AddListener((UnityAction) (() =>
          {
            this.enabled = true;
            this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
          }));
          this.KnuckleboneOpponent.FirstWinConvo.gameObject.SetActive(true);
          this.KnuckleboneOpponent.FirstWinConvo.Play(this.playerFarming.gameObject);
          Debug.Log((object) "FIRST TIME DE");
          flag = true;
        }
        else
        {
          Debug.Log((object) "Show Win Convo");
          if (this._betAmount > 0)
            this.StartCoroutine((IEnumerator) this.GiveGold(true));
          else
            this.TryToGiveKnucklbonesStructure();
        }
        DataManager.Instance.SetVariable(this.KnuckleboneOpponent.Config.VariableToChangeOnWin, true);
        if (flag)
          ObjectiveManager.CompleteDefeatKnucklebones(this.KnuckleboneOpponent.Config.OpponentName);
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("WIN_KNUCKLEBONES"));
        break;
      case UIKnuckleBonesController.KnucklebonesResult.Loss:
        Debug.Log((object) "Show Lose Convo");
        if (this._betAmount > 0)
          this.StartCoroutine((IEnumerator) this.GiveGold(false));
        else
          this.TryToGiveKnucklbonesStructure();
        if (this._betAmount > 0 && !this.canPlayRatauStaffLoseConvo || this.KnuckleboneOpponent.Tag != KnucklebonesOpponent.OppnentTags.Ratau)
        {
          this.KnuckleboneOpponent.LoseConvo.gameObject.SetActive(true);
          this.KnuckleboneOpponent.LoseConvo.Play(this.playerFarming.gameObject);
          break;
        }
        break;
      default:
        this.KnuckleboneOpponent.DrawConvo.gameObject.SetActive(true);
        this.KnuckleboneOpponent.DrawConvo.Play(this.playerFarming.gameObject);
        this.TryToGiveKnucklbonesStructure();
        break;
    }
    PlayerFarming.SetStateForAllPlayers();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.StartCoroutine((IEnumerator) this.PlayMatchCoop());
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = PlayerFarming.playersCount > 1 ? LocalizationManager.GetTranslation("UI/PlayCoop") : "";
  }

  public void TryToGiveKnucklbonesStructure()
  {
    if (!DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_0_Won) || !DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_1_Won) || !DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_2_Won) || !DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_Ratau_Won) && !DataManager.Instance.RatauKilled)
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("WIN_KNUCKLEBONES_ALL"));
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Knucklebones))
      return;
    this.StartCoroutine((IEnumerator) this.GiveKnucklebonesStructureIE());
  }

  public IEnumerator GiveKnucklebonesStructureIE()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    while (true)
    {
      if (!LetterBox.IsPlaying)
      {
        yield return (object) new WaitForSeconds(0.1f);
        if (!LetterBox.IsPlaying)
          break;
      }
      yield return (object) null;
    }
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.5f);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Knucklebones);
    bool waiting = true;
    GameManager.GetInstance().OnConversationNext(DecorationCustomTarget.Create(interactionKnucklebones.transform.position - Vector3.forward + Vector3.up * 2f, PlayerFarming.Instance.transform.position, 1f, StructureBrain.TYPES.KNUCKLEBONES_ARENA, (System.Action) (() => waiting = false)), 6f);
    yield return (object) new WaitUntil((Func<bool>) (() => !waiting));
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator MakeBet()
  {
    bool betConfirmed = false;
    bool betCancelled = false;
    this._betAmount = 0;
    UIKnucklebonesBettingSelectionController selectionController = MonoSingleton<UIManager>.Instance.KnucklebonesBettingSelectionTemplate.Instantiate<UIKnucklebonesBettingSelectionController>();
    selectionController.Show(this.KnuckleboneOpponent.Config);
    selectionController.OnConfirmBet += (System.Action<int>) (bet =>
    {
      this._betAmount = bet * 5;
      // ISSUE: reference to a compiler-generated method
      this.\u003CMakeBet\u003Eg__ConfirmBet\u007C0();
    });
    selectionController.OnHide = selectionController.OnHide + (System.Action) (() =>
    {
      if (betConfirmed)
        return;
      // ISSUE: reference to a compiler-generated method
      this.\u003CMakeBet\u003Eg__CancelBet\u007C1();
    });
    while (!betConfirmed && !betCancelled)
      yield return (object) null;
    if (betConfirmed)
      yield return (object) this.ContinueToKnucklebones();
    else if (betCancelled)
      this.GameQuit();
  }

  public IEnumerator SelectOpponent()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    bool opponentSelected = false;
    bool opponentCancelled = false;
    GameManager.GetInstance().OnConversationNew();
    List<KnucklebonesPlayerConfiguration> playerConfigurationList = new List<KnucklebonesPlayerConfiguration>();
    foreach (KnucklebonesOpponent availableOpponent in interactionKnucklebones._availableOpponents)
      playerConfigurationList.Add(availableOpponent.Config);
    UIKnucklebonesOpponentSelectionController selectionController = MonoSingleton<UIManager>.Instance.KnucklebonesOpponentSelectionTemplate.Instantiate<UIKnucklebonesOpponentSelectionController>();
    selectionController.Show(playerConfigurationList.ToArray());
    selectionController.OnConfirmOpponent += (System.Action) (() => opponentSelected = true);
    selectionController.OnOpponentSelectionChanged += (System.Action<int>) (selection =>
    {
      Debug.Log((object) $"Knucklebones Selection {selection}".Colour(Color.red));
      this.KnuckleboneOpponent = this._availableOpponents[selection];
      GameManager.GetInstance().OnConversationNext(this.KnuckleboneOpponent.Spine.gameObject, 10f);
      AudioManager.Instance.PlayOneShot(this.KnuckleboneOpponent.Config.SoundToPlay);
      AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", this.gameObject);
      this.KnuckleboneOpponent.Spine.AnimationState.SetAnimation(0, "selected", false);
      this.KnuckleboneOpponent.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    });
    selectionController.OnHide = selectionController.OnHide + (System.Action) (() =>
    {
      if (opponentSelected)
        return;
      // ISSUE: reference to a compiler-generated method
      this.\u003CSelectOpponent\u003Eg__CancelSelection\u007C1();
    });
    while (!opponentSelected && !opponentCancelled)
      yield return (object) null;
    if (opponentCancelled)
      interactionKnucklebones.GameQuit();
    else if (DataManager.Instance.GetVariable(interactionKnucklebones.KnuckleboneOpponent.Config.VariableToChangeOnWin))
    {
      AudioManager.Instance.PlayOneShot("event:/ui/confirm_selection", interactionKnucklebones.gameObject);
      interactionKnucklebones.StartCoroutine((IEnumerator) interactionKnucklebones.MakeBet());
    }
    else
      yield return (object) interactionKnucklebones.ContinueToKnucklebones();
  }

  public IEnumerator ContinueToKnucklebones()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.gameObject, 6f);
    interactionKnucklebones.goopTransition.FadeIn(1f);
    interactionKnucklebones.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1f);
    interactionKnucklebones._knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    SimulationManager.Pause();
    UIKnuckleBonesController knuckleBonesInstance1 = interactionKnucklebones._knuckleBonesInstance;
    knuckleBonesInstance1.OnHidden = knuckleBonesInstance1.OnHidden + new System.Action(SimulationManager.UnPause);
    interactionKnucklebones._knuckleBonesInstance.Show(interactionKnucklebones.playerFarming, interactionKnucklebones.KnuckleboneOpponent, interactionKnucklebones._betAmount);
    UIKnuckleBonesController knuckleBonesInstance2 = interactionKnucklebones._knuckleBonesInstance;
    knuckleBonesInstance2.OnHidden = knuckleBonesInstance2.OnHidden + new System.Action(interactionKnucklebones.\u003CContinueToKnucklebones\u003Eb__36_0);
    interactionKnucklebones._knuckleBonesInstance.OnGameCompleted += new System.Action<UIKnuckleBonesController.KnucklebonesResult>(interactionKnucklebones.CompleteGame);
    interactionKnucklebones._knuckleBonesInstance.OnGameQuit += new System.Action(interactionKnucklebones.GameQuit);
    yield return (object) null;
  }

  public IEnumerator GiveGold(bool Won)
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.state.gameObject, 5f);
    interactionKnucklebones.playerFarming.state.facingAngle = 0.0f;
    interactionKnucklebones.playerFarming.state.LookAngle = 0.0f;
    int i;
    if (Won)
    {
      for (i = 0; i < interactionKnucklebones._betAmount; ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionKnucklebones.transform.position);
        ResourceCustomTarget.Create(interactionKnucklebones.playerFarming.gameObject, interactionKnucklebones.KnuckleboneOpponent.Spine.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1);
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
      }
    }
    else
    {
      for (i = 0; i < interactionKnucklebones._betAmount; ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionKnucklebones.transform.position);
        ResourceCustomTarget.Create(interactionKnucklebones.KnuckleboneOpponent.Spine.gameObject, interactionKnucklebones.playerFarming.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        Inventory.ChangeItemQuantity(20, -1);
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
      }
    }
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.25f, 0.5f);
    interactionKnucklebones.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.25f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForEndOfFrame();
    if (Won)
    {
      if (interactionKnucklebones.canPlayRatauStaffWinConvo)
      {
        interactionKnucklebones.enabled = false;
        interactionKnucklebones.wonGameConvo.Callback.AddListener(new UnityAction(interactionKnucklebones.UnlockRatauFleece));
        interactionKnucklebones.wonGameConvo.gameObject.SetActive(true);
        interactionKnucklebones.wonGameConvo.Play(interactionKnucklebones.playerFarming.gameObject);
      }
      else if (!interactionKnucklebones.KnuckleboneOpponent.WinConvo.Spoken)
      {
        interactionKnucklebones.enabled = false;
        interactionKnucklebones.KnuckleboneOpponent.WinConvo.Callback.AddListener(new UnityAction(interactionKnucklebones.\u003CGiveGold\u003Eb__37_0));
        interactionKnucklebones.KnuckleboneOpponent.WinConvo.gameObject.SetActive(true);
        interactionKnucklebones.KnuckleboneOpponent.WinConvo.Play(interactionKnucklebones.playerFarming.gameObject);
      }
    }
    else if (interactionKnucklebones.canPlayRatauStaffLoseConvo)
    {
      interactionKnucklebones.enabled = false;
      interactionKnucklebones.lostGameConvo.Callback.AddListener(new UnityAction(interactionKnucklebones.\u003CGiveGold\u003Eb__37_1));
      interactionKnucklebones.lostGameConvo.gameObject.SetActive(true);
      interactionKnucklebones.lostGameConvo.Play(interactionKnucklebones.playerFarming.gameObject);
    }
    else if (!interactionKnucklebones.KnuckleboneOpponent.LoseConvo.Spoken)
    {
      interactionKnucklebones.enabled = false;
      interactionKnucklebones.KnuckleboneOpponent.LoseConvo.Callback.AddListener(new UnityAction(interactionKnucklebones.\u003CGiveGold\u003Eb__37_2));
    }
    interactionKnucklebones.TryToGiveKnucklbonesStructure();
  }

  public IEnumerator BlockDoor()
  {
    while ((UnityEngine.Object) this.t != (UnityEngine.Object) null)
    {
      this.Exit.SetActive(false);
      this.Blocker.SetActive(true);
      yield return (object) null;
    }
    this.Exit.SetActive(true);
    this.Blocker.SetActive(false);
  }

  public void TestReward(int Player = 3)
  {
    this.KnuckleboneOpponent = this.KnucklebonePlayers[Player];
    this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
  }

  public void GiveReward()
  {
  }

  public IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    Debug.Log((object) ("Card Reward = " + interactionKnucklebones.KnuckleboneOpponent.TarotCardReward.ToString()));
    TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(interactionKnucklebones.KnuckleboneOpponent.Spine.transform.position + Vector3.back * 0.5f, interactionKnucklebones.playerFarming.transform.position + Vector3.back * 0.5f, 1f, interactionKnucklebones.KnuckleboneOpponent.TarotCardReward, new System.Action(interactionKnucklebones.\u003CGiveKeyPieceRoutine\u003Eb__44_0));
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject, 6f);
  }

  public IEnumerator PlayMatchCoop()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    PlayerFarming.Instance.GoToAndStop(interactionKnucklebones.playerPositions[0].transform.position, interactionKnucklebones.gameObject, groupAction: true, forcedOtherPosition: new Vector3?(interactionKnucklebones.playerPositions[1].transform.position));
    UIKnuckleBonesController.KnucklebonesResult result = UIKnuckleBonesController.KnucklebonesResult.Loss;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.playerPositions[2], 4f);
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      bool flag = false;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.GoToAndStopping)
          flag = true;
      }
      if (flag)
        yield return (object) null;
      else
        break;
    }
    interactionKnucklebones.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    int num = interactionKnucklebones.playerFarming.isLamb ? 1 : 0;
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayMusic("event:/music/ratau_home/ratau_home");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    KnucklebonesOpponent opponent = new KnucklebonesOpponent()
    {
      Config = interactionKnucklebones.knuckleBonesPlayerConfig
    };
    opponent.Config.OpponentName = LocalizationManager.GetTranslation("NAMES/Knucklebones/Goat");
    opponent.IsCoopPlayer = true;
    UIKnuckleBonesController knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    UIKnuckleBonesController knuckleBonesController1 = knuckleBonesInstance;
    knuckleBonesController1.OnHidden = knuckleBonesController1.OnHidden + new System.Action(SimulationManager.UnPause);
    SimulationManager.Pause();
    bool gameQuit = true;
    knuckleBonesInstance.Show(PlayerFarming.players[0], opponent, 0);
    UIKnuckleBonesController knuckleBonesController2 = knuckleBonesInstance;
    knuckleBonesController2.OnShow = knuckleBonesController2.OnShow + (System.Action) (() =>
    {
      KBGameScreen gameScreen = knuckleBonesInstance.GameScreen;
      gameScreen.OnShow = gameScreen.OnShow + (System.Action) (() => { });
    });
    UIKnuckleBonesController knuckleBonesController3 = knuckleBonesInstance;
    knuckleBonesController3.OnHidden = knuckleBonesController3.OnHidden + (System.Action) (() => knuckleBonesInstance = (UIKnuckleBonesController) null);
    knuckleBonesInstance.OnGameCompleted += (System.Action<UIKnuckleBonesController.KnucklebonesResult>) (r =>
    {
      AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
      result = r;
      gameQuit = r == UIKnuckleBonesController.KnucklebonesResult.Quit;
    });
    knuckleBonesInstance.OnGameQuit += (System.Action) (() => this.GameQuit());
    while ((UnityEngine.Object) knuckleBonesInstance != (UnityEngine.Object) null)
      yield return (object) null;
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.playerPositions[2], 6f);
    yield return (object) new WaitForEndOfFrame();
    if (!gameQuit)
    {
      string str = (double) UnityEngine.Random.value < 0.5 ? "" : "2";
      PlayerFarming.players[0].state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.players[1].state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.players[0].Spine.AnimationState.SetAnimation(0, result == UIKnuckleBonesController.KnucklebonesResult.Win ? "reactions/react-happy" + str : "reactions/react-angry" + str, false);
      PlayerFarming.players[1].Spine.AnimationState.SetAnimation(0, result == UIKnuckleBonesController.KnucklebonesResult.Loss ? "reactions/react-happy" + str : "reactions/react-angry" + str, false);
      yield return (object) new WaitForSeconds(1.5f);
    }
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator PlayMatchTwitch()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.playerPositions[2], 4f);
    interactionKnucklebones.playerFarming.GoToAndStop(interactionKnucklebones.playerPositions[0].transform.position, interactionKnucklebones.gameObject);
    UIKnuckleBonesController.KnucklebonesResult result = UIKnuckleBonesController.KnucklebonesResult.Loss;
    while (true)
    {
      bool flag = false;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.GoToAndStopping)
          flag = true;
      }
      if (flag)
        yield return (object) null;
      else
        break;
    }
    interactionKnucklebones.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayMusic("event:/music/ratau_home/ratau_home");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    KnucklebonesOpponent opponent = new KnucklebonesOpponent()
    {
      Config = interactionKnucklebones.knuckleBonesPlayerConfig
    };
    opponent.Config.OpponentName = LocalizationManager.GetTranslation("UI/Twitch/Chat");
    opponent.IsTwitchChat = true;
    UIKnuckleBonesController knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    UIKnuckleBonesController knuckleBonesController1 = knuckleBonesInstance;
    knuckleBonesController1.OnHidden = knuckleBonesController1.OnHidden + new System.Action(SimulationManager.UnPause);
    SimulationManager.Pause();
    bool gameQuit = true;
    knuckleBonesInstance.Show(PlayerFarming.players[0], opponent, 0);
    UIKnuckleBonesController knuckleBonesController2 = knuckleBonesInstance;
    knuckleBonesController2.OnShow = knuckleBonesController2.OnShow + (System.Action) (() =>
    {
      KBGameScreen gameScreen = knuckleBonesInstance.GameScreen;
      gameScreen.OnShow = gameScreen.OnShow + (System.Action) (() => { });
    });
    UIKnuckleBonesController knuckleBonesController3 = knuckleBonesInstance;
    knuckleBonesController3.OnHidden = knuckleBonesController3.OnHidden + (System.Action) (() => knuckleBonesInstance = (UIKnuckleBonesController) null);
    knuckleBonesInstance.OnGameCompleted += (System.Action<UIKnuckleBonesController.KnucklebonesResult>) (r =>
    {
      result = r;
      gameQuit = r == UIKnuckleBonesController.KnucklebonesResult.Quit;
    });
    knuckleBonesInstance.OnGameQuit += (System.Action) (() => this.GameQuit());
    while ((UnityEngine.Object) knuckleBonesInstance != (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    if (!gameQuit)
    {
      string str = (double) UnityEngine.Random.value < 0.5 ? "" : "2";
      interactionKnucklebones.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      interactionKnucklebones.playerFarming.Spine.AnimationState.SetAnimation(0, result == UIKnuckleBonesController.KnucklebonesResult.Win ? "reactions/react-happy" + str : "reactions/react-angry" + str, false);
      yield return (object) new WaitForSeconds(1.5f);
    }
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void UnlockRatauFleece()
  {
    this.StartCoroutine((IEnumerator) this.UnlockRatauFleeceSequence());
  }

  public IEnumerator UnlockRatauFleeceSequence()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    while (interactionKnucklebones.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    if (!((UnityEngine.Object) interactionKnucklebones.ratauSpine == (UnityEngine.Object) null))
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionKnucklebones.ratauSpine.gameObject, 7f);
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/ratau/goes_naked");
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionKnucklebones.ratauSpine.transform.position);
      interactionKnucklebones.ratauSpine.Skeleton.SetSkin("naked");
      interactionKnucklebones.ratauSpine.AnimationState.SetAnimation(0, "idle-shy", true);
      interactionKnucklebones.SetNakedRatauConvoTalkAnims();
      yield return (object) new WaitForSeconds(1.5f);
      GameManager.GetInstance().OnConversationNext(interactionKnucklebones.playerFarming.gameObject);
      if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate == (UnityEngine.Object) null)
        MonoSingleton<UIManager>.Instance.LoadPlayerUpgradesMenu();
      yield return (object) new WaitForSeconds(0.5f);
      Time.timeScale = 0.0f;
      interactionKnucklebones.enabled = true;
      DataManager.Instance.RatauStaffQuestWonGame = true;
      DataManager.Instance.UnlockedFleeces.Add(11);
      UIPlayerUpgradesMenuController upgradesMenuController = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
      upgradesMenuController.OnHide = upgradesMenuController.OnHide + (System.Action) (() =>
      {
        Time.timeScale = 1f;
        GameManager.GetInstance().OnConversationEnd();
      });
      upgradesMenuController.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
      {
        PlayerFleeceManager.FleeceType.RatauCloak
      }, true);
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.RATAU_END));
    }
  }

  public void SetNakedRatauConvoTalkAnims()
  {
    KnucklebonesOpponent knucklebonesOpponent = (KnucklebonesOpponent) null;
    foreach (KnucklebonesOpponent knucklebonePlayer in this.KnucklebonePlayers)
    {
      if (knucklebonePlayer.Tag == KnucklebonesOpponent.OppnentTags.Ratau)
      {
        knucklebonesOpponent = knucklebonePlayer;
        break;
      }
    }
    foreach (ConversationEntry entry in knucklebonesOpponent.WinConvo.Entries)
      entry.Animation = "idle-shy-talk";
    foreach (ConversationEntry entry in knucklebonesOpponent.LoseConvo.Entries)
      entry.Animation = "idle-shy-talk";
    foreach (ConversationEntry entry in knucklebonesOpponent.DrawConvo.Entries)
      entry.Animation = "idle-shy-talk";
  }

  [CompilerGenerated]
  public void \u003CCompleteGame\u003Eb__29_0()
  {
    this.enabled = true;
    this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
  }

  [CompilerGenerated]
  public void \u003CContinueToKnucklebones\u003Eb__36_0()
  {
    this.goopTransition.FadeOut(1f);
    this._knuckleBonesInstance = (UIKnuckleBonesController) null;
  }

  [CompilerGenerated]
  public void \u003CGiveGold\u003Eb__37_0() => this.enabled = true;

  [CompilerGenerated]
  public void \u003CGiveGold\u003Eb__37_1() => this.enabled = true;

  [CompilerGenerated]
  public void \u003CGiveGold\u003Eb__37_2() => this.enabled = true;

  [CompilerGenerated]
  public void \u003CGiveKeyPieceRoutine\u003Eb__44_0()
  {
    GameManager.GetInstance().OnConversationEnd();
    if (this.KnuckleboneOpponent.Tag == KnucklebonesOpponent.OppnentTags.Ratau)
    {
      this.enabled = false;
      this.CareToBetConversation.SetActive(true);
    }
    this.TryToGiveKnucklbonesStructure();
  }
}
