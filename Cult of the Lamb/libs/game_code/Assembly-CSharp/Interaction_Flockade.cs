// Decompiled with JetBrains decompiler
// Type: Interaction_Flockade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Flockade;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

#nullable disable
public class Interaction_Flockade : Interaction
{
  [SerializeField]
  public bool useCustomPool;
  [SerializeField]
  public FlockadeGamePiecesPoolConfiguration[] _poolConfigurations;
  [SerializeField]
  public Interaction_SimpleConversation _firstGameConversation;
  [SerializeField]
  public GameObject flockadePiecesPrefab;
  [SerializeField]
  public List<FlockadeOpponent> _npcOpponents = new List<FlockadeOpponent>();
  [SerializeField]
  public FlockadeOpponentConfiguration coopOpponent;
  [SerializeField]
  public FlockadeOpponentConfiguration twitchOpponent;
  [SerializeField]
  public FlockadeJobBoardManager _jobBoard;
  [SerializeField]
  public UnityEvent OnFirstWin;
  public FlockadeOpponent _currentOpponent;
  public int _betAmount;
  public List<FlockadeOpponent> _availableOpponents = new List<FlockadeOpponent>();
  public bool isLoadingAssets;

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_Flockade interactionFlockade = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionFlockade.isLoadingAssets = true;
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadFlockadeAssets();
    interactionFlockade.StartCoroutine((IEnumerator) UIManager.LoadAssets(task, new System.Action(interactionFlockade.\u003CStart\u003Eb__12_0)));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    MonoSingleton<UIManager>.Instance.UnloadFlockadeAssets();
    MonoSingleton<UIManager>.Instance.UnloadFlockadePiecesAssets();
    FlockadePieceManager.UnloadPieces();
    Resources.UnloadUnusedAssets();
  }

  public override void GetLabel()
  {
    if ((double) this.GetUnlockedPieces().Count < 12.0)
    {
      this.Interactable = false;
      this.Label = "";
      Debug.LogError((object) "Not enough unlocked pieces or incorrect value in the peices pool!");
    }
    else
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
    }
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
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlayFlockade);
    List<FlockadeGamePieceConfiguration> gamePiecesPool = new List<FlockadeGamePieceConfiguration>((IEnumerable<FlockadeGamePieceConfiguration>) this.GetUnlockedPieces());
    for (int index = gamePiecesPool.Count - 1; index >= 0; --index)
    {
      if (gamePiecesPool[index].Type == FlockadePieceType.ScribeScout || gamePiecesPool[index].Type == FlockadePieceType.ShieldScout || gamePiecesPool[index].Type == FlockadePieceType.SwordScout)
        gamePiecesPool.RemoveAt(index);
    }
    FlockadeUIController flockadeUiController = MonoSingleton<UIManager>.Instance.FlockadeTemplate.Instantiate<FlockadeUIController>();
    flockadeUiController.Show(this._playerFarming, (IEnumerable<FlockadeGamePieceConfiguration>) gamePiecesPool, this.twitchOpponent);
    flockadeUiController.OnHidden = flockadeUiController.OnHidden + (System.Action) (() =>
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.GetAvailableOpponents();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.playerFarming.GoToAndStop(this.transform.position + new Vector3(-0.75f, -2.2f), this.gameObject, GoToCallback: (System.Action) (() =>
    {
      if (this._availableOpponents.Count > 0)
        this.StartCoroutine((IEnumerator) this.SelectOpponent());
      else
        state.CURRENT_STATE = StateMachine.State.Idle;
    }));
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (this.isLoadingAssets)
      return;
    base.OnSecondaryInteract(state);
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlayFlockade);
    FlockadeUIController flockadeUiController = MonoSingleton<UIManager>.Instance.FlockadeTemplate.Instantiate<FlockadeUIController>();
    flockadeUiController.Show(this._playerFarming, (IEnumerable<FlockadeGamePieceConfiguration>) this.GetUnlockedPieces(), this.coopOpponent);
    flockadeUiController.OnHidden = flockadeUiController.OnHidden + (System.Action) (() =>
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = PlayerFarming.playersCount > 1 ? LocalizationManager.GetTranslation("UI/PlayCoop") : "";
  }

  public List<FlockadeGamePieceConfiguration> GetUnlockedPieces()
  {
    IEnumerable<FlockadeGamePieceConfiguration> gamePieces1 = ((IEnumerable<FlockadeGamePiecesPoolConfiguration>) this._poolConfigurations).First<FlockadeGamePiecesPoolConfiguration>().GetGamePieces();
    if (this.useCustomPool)
      return gamePieces1.ToList<FlockadeGamePieceConfiguration>();
    IEnumerable<FlockadeGamePieceConfiguration> gamePieces2 = FlockadePieceManager.GetPiecesPool().GetGamePieces();
    List<FlockadeGamePieceConfiguration> unlockedPieces = new List<FlockadeGamePieceConfiguration>();
    foreach (FlockadeGamePieceConfiguration pieceConfiguration in gamePieces2)
    {
      if (DataManager.Instance.PlayerFoundPieces.Contains(pieceConfiguration.Type))
        unlockedPieces.Add(pieceConfiguration);
    }
    return unlockedPieces;
  }

  public IEnumerator SelectOpponent()
  {
    Interaction_Flockade interactionFlockade = this;
    bool opponentSelected = false;
    bool opponentCancelled = false;
    DataManager.Instance.HasNewFlockadePieces = false;
    GameManager.GetInstance().OnConversationNew();
    List<FlockadeOpponentConfiguration> opponentConfigurationList;
    PooledObject<List<FlockadeOpponentConfiguration>> pooledObject = CollectionPool<List<FlockadeOpponentConfiguration>, FlockadeOpponentConfiguration>.Get(out opponentConfigurationList);
    bool flag;
    try
    {
      foreach (FlockadeOpponent availableOpponent in interactionFlockade._availableOpponents)
        opponentConfigurationList.Add(availableOpponent.Config);
      FlockadeOpponentSelectionUIController selectionUiController = MonoSingleton<UIManager>.Instance.FlockadeOpponentSelectionTemplate.Instantiate<FlockadeOpponentSelectionUIController>();
      selectionUiController.Show(opponentConfigurationList.ToArray());
      selectionUiController.OpponentConfirmed += (System.Action) (() => opponentSelected = true);
      selectionUiController.SelectedOpponentChanged += (System.Action<int, FlockadeOpponentConfiguration>) ((selection, opponent) =>
      {
        Debug.Log((object) $"Flockade Selection Changed: {selection}:{opponent.NpcConfiguration.Name}");
        if (selection < 0 || selection >= this._availableOpponents.Count)
        {
          Debug.LogError((object) $"Invalid index for available opponents: {{Index:{selection},OpponentCount:{this._availableOpponents.Count}}}");
        }
        else
        {
          this._currentOpponent = this._availableOpponents[selection];
          GameManager.GetInstance().OnConversationNext(this._currentOpponent.Spine.gameObject, 10f);
          AudioManager.Instance.PlayOneShot(opponent.NpcConfiguration.SelectedSound);
          AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", this.gameObject);
        }
      });
      selectionUiController.OnHide = selectionUiController.OnHide + (System.Action) (() =>
      {
        if (opponentSelected)
          return;
        // ISSUE: reference to a compiler-generated method
        this.\u003CSelectOpponent\u003Eg__CancelSelection\u007C2();
      });
      while (!opponentSelected && !opponentCancelled)
        yield return (object) null;
      if (opponentCancelled)
      {
        interactionFlockade.GameQuit();
        flag = false;
      }
      else if (DataManager.Instance.GetVariable(interactionFlockade._currentOpponent.Config.NpcConfiguration.VariableToChangeOnWin))
      {
        AudioManager.Instance.PlayOneShot("event:/ui/confirm_selection", interactionFlockade.gameObject);
        interactionFlockade.StartCoroutine((IEnumerator) interactionFlockade.MakeBet());
        flag = false;
      }
      else
      {
        yield return (object) interactionFlockade.ContinueToFlockade();
        goto label_16;
      }
      goto label_15;
    }
    finally
    {
      pooledObject.Dispose();
    }
label_16:
    pooledObject = new PooledObject<List<FlockadeOpponentConfiguration>>();
    yield break;
label_15:
    return flag;
  }

  public IEnumerator MakeBet()
  {
    bool betConfirmed = false;
    bool betCancelled = false;
    this._betAmount = 0;
    FlockadeBettingSelectionUIController selectionUiController = MonoSingleton<UIManager>.Instance.FlockadeBettingSelectionTemplate.Instantiate<FlockadeBettingSelectionUIController>();
    selectionUiController.Show(this._currentOpponent.Config);
    selectionUiController.BetConfirmed += (System.Action<int>) (bet =>
    {
      this._betAmount = bet;
      // ISSUE: reference to a compiler-generated method
      this.\u003CMakeBet\u003Eg__ConfirmBet\u007C1();
    });
    selectionUiController.OnHide = selectionUiController.OnHide + (System.Action) (() =>
    {
      if (betConfirmed)
        return;
      // ISSUE: reference to a compiler-generated method
      this.\u003CMakeBet\u003Eg__CancelBet\u007C2();
    });
    while (!betConfirmed && !betCancelled)
      yield return (object) null;
    if (betCancelled)
      this.GameQuit();
    else
      yield return (object) this.ContinueToFlockade();
  }

  public IEnumerator ContinueToFlockade()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_Flockade interactionFlockade = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionFlockade.Interactable = false;
    interactionFlockade.HasChanged = true;
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFlockade.gameObject, 6f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlayFlockade);
    interactionFlockade.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    FlockadeUIController flockadeUiController = MonoSingleton<UIManager>.Instance.FlockadeTemplate.Instantiate<FlockadeUIController>();
    flockadeUiController.GameCompleted += new System.Action<FlockadeUIController.Result>(interactionFlockade.GameCompleted);
    flockadeUiController.Show(interactionFlockade._playerFarming, (IEnumerable<FlockadeGamePieceConfiguration>) interactionFlockade.GetUnlockedPieces(), interactionFlockade._currentOpponent.Config, interactionFlockade._betAmount);
    flockadeUiController.OnHidden = flockadeUiController.OnHidden + (System.Action) (() =>
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
    });
    return false;
  }

  public void GameCompleted(FlockadeUIController.Result result)
  {
    this.StartCoroutine((IEnumerator) this.GameCompletedRoutine(result));
  }

  public IEnumerator GameCompletedRoutine(FlockadeUIController.Result result)
  {
    Interaction_Flockade interactionFlockade = this;
    interactionFlockade.Interactable = false;
    interactionFlockade.HasChanged = true;
    bool firstFlockadeGame = !DataManager.Instance.FlockadePlayed;
    DataManager.Instance.FlockadePlayed = true;
    Debug.Log((object) "Flockade Game was Completed!");
    switch (result)
    {
      case FlockadeUIController.Result.Win:
        if ((UnityEngine.Object) interactionFlockade._jobBoard == (UnityEngine.Object) null)
          interactionFlockade._jobBoard = UnityEngine.Object.FindObjectOfType<FlockadeJobBoardManager>(true);
        interactionFlockade._jobBoard.Reveal();
        if (!DataManager.Instance.GetVariable(interactionFlockade._currentOpponent.Config.NpcConfiguration.VariableToChangeOnWin))
        {
          if ((UnityEngine.Object) interactionFlockade._currentOpponent.FirstWinConversation != (UnityEngine.Object) null)
          {
            Debug.Log((object) "Show First Win Convo");
            interactionFlockade.enabled = false;
            interactionFlockade._currentOpponent.FirstWinConversation.gameObject.SetActive(true);
            interactionFlockade._currentOpponent.FirstWinConversation.Play(interactionFlockade.playerFarming.gameObject);
            while (!interactionFlockade._currentOpponent.FirstWinConversation.Finished)
              yield return (object) null;
            interactionFlockade.enabled = true;
            yield return (object) interactionFlockade.GiveRewardRoutine(interactionFlockade._currentOpponent.Spine.transform.position + Vector3.back);
            Debug.Log((object) "FIRST TIME DE");
          }
          interactionFlockade.OnFirstWin?.Invoke();
        }
        else
        {
          interactionFlockade.enabled = false;
          Debug.Log((object) "Awarding Wool Bet");
          int variableInt = DataManager.Instance.GetVariableInt(interactionFlockade._currentOpponent.Config.NpcConfiguration.VariableForWoolWonCount);
          DataManager.Instance.SetVariableInt(interactionFlockade._currentOpponent.Config.NpcConfiguration.VariableForWoolWonCount, variableInt + interactionFlockade._betAmount);
          yield return (object) interactionFlockade.GiveWool(true);
          Debug.Log((object) "Show Win Convo");
          interactionFlockade._currentOpponent.WinConversation.gameObject.SetActive(true);
          interactionFlockade._currentOpponent.WinConversation.Play(interactionFlockade.playerFarming.gameObject);
          while (!interactionFlockade._currentOpponent.WinConversation.Finished)
            yield return (object) null;
          interactionFlockade.enabled = true;
        }
        DataManager.Instance.SetVariable(interactionFlockade._currentOpponent.Config.NpcConfiguration.VariableToChangeOnWin, true);
        interactionFlockade.GetAvailableOpponents();
        break;
      case FlockadeUIController.Result.Loss:
        interactionFlockade.enabled = false;
        Debug.Log((object) "Settling Wool Bet.");
        yield return (object) interactionFlockade.GiveWool(false);
        Debug.Log((object) "Show Lose Convo");
        interactionFlockade._currentOpponent.LoseConversation.gameObject.SetActive(true);
        interactionFlockade._currentOpponent.LoseConversation.Play(interactionFlockade.playerFarming.gameObject);
        while (!interactionFlockade._currentOpponent.LoseConversation.Finished)
          yield return (object) null;
        interactionFlockade.enabled = true;
        break;
      default:
        interactionFlockade.enabled = true;
        break;
    }
    if (firstFlockadeGame && (UnityEngine.Object) interactionFlockade._firstGameConversation != (UnityEngine.Object) null)
    {
      interactionFlockade.enabled = false;
      yield return (object) interactionFlockade.FirstTimeGameRoutine();
      interactionFlockade.enabled = true;
    }
    interactionFlockade.Interactable = true;
    interactionFlockade.HasChanged = true;
    interactionFlockade._betAmount = 0;
    AudioManager.Instance.PlayMusic("event:/music/base/base_main");
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.woolhaven);
  }

  public IEnumerator GiveWool(bool won)
  {
    Interaction_Flockade interactionFlockade = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFlockade.state.gameObject, 5f);
    interactionFlockade.playerFarming.state.facingAngle = 0.0f;
    interactionFlockade.playerFarming.state.LookAngle = 0.0f;
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.WOOL, won ? interactionFlockade._betAmount : -interactionFlockade._betAmount);
    for (int i = 0; i < interactionFlockade._betAmount; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionFlockade.transform.position);
      if (won)
        ResourceCustomTarget.Create(interactionFlockade.playerFarming.gameObject, interactionFlockade._currentOpponent.Spine.gameObject.transform.position, InventoryItem.ITEM_TYPE.WOOL, (System.Action) null);
      else
        ResourceCustomTarget.Create(interactionFlockade._currentOpponent.Spine.gameObject, interactionFlockade.playerFarming.gameObject.transform.position, InventoryItem.ITEM_TYPE.WOOL, (System.Action) null);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
    }
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.25f, 0.5f);
    interactionFlockade.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    interactionFlockade._betAmount = 0;
  }

  public IEnumerator FirstTimeGameRoutine()
  {
    Interaction_Flockade interactionFlockade = this;
    interactionFlockade._firstGameConversation.Play(interactionFlockade.playerFarming.gameObject);
    while (!interactionFlockade._firstGameConversation.Finished)
      yield return (object) null;
    yield return (object) interactionFlockade.GiveRewardRoutine(interactionFlockade._currentOpponent.Spine.transform.position + Vector3.back);
  }

  public IEnumerator GiveRewardRoutine(Vector3 fromPosition)
  {
    if (this._currentOpponent.Config.NpcConfiguration.RewardPieces.Count != 0)
      yield return (object) this.GiveFlockadePieces(this._currentOpponent.Config.NpcConfiguration.RewardPieces, fromPosition);
  }

  public void GameQuit()
  {
    Debug.Log((object) "Quitting Flockade");
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.woolhaven);
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
    this.Interactable = true;
    this.HasChanged = true;
  }

  public void GetAvailableOpponents()
  {
    this._availableOpponents.Clear();
    foreach (FlockadeOpponent npcOpponent in this._npcOpponents)
    {
      Debug.Log((object) $"Checking variable for {npcOpponent.Config.NpcConfiguration.Name}: {npcOpponent.Config.NpcConfiguration.VariableToShow}");
      if (!DataManager.Instance.GetVariable(npcOpponent.Config.NpcConfiguration.VariableToShow) || !DataManager.Instance.GetVariable(npcOpponent.Config.NpcConfiguration.PreviousOpponentToBeat))
      {
        Debug.Log((object) (npcOpponent.Config.NpcConfiguration.Name + " is locked!"));
        npcOpponent.gameObject.SetActive(false);
      }
      else
      {
        Debug.Log((object) (npcOpponent.Config.NpcConfiguration.Name + " is available to play!"));
        npcOpponent.gameObject.SetActive(true);
        this._availableOpponents.Add(npcOpponent);
      }
    }
  }

  public IEnumerator GiveFlockadePieces(
    IReadOnlyList<FlockadePieceType> pieces,
    Vector3 fromPosition)
  {
    Interaction_Flockade interactionFlockade = this;
    if (pieces.Count != 0)
    {
      GameObject flockadePieces = UnityEngine.Object.Instantiate<GameObject>(interactionFlockade.flockadePiecesPrefab, fromPosition, Quaternion.identity);
      flockadePieces.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(flockadePieces.gameObject, 5f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/aniel/flockade_bag_give");
      yield return (object) new WaitForSeconds(1f);
      flockadePieces.transform.DOMove(interactionFlockade.playerFarming.transform.position + Vector3.back, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionFlockade.transform.position);
      interactionFlockade.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
      GameManager.GetInstance().OnConversationNext(interactionFlockade.playerFarming.gameObject, 5f);
      yield return (object) new WaitForSeconds(1f);
      UnityEngine.Object.Destroy((UnityEngine.Object) flockadePieces.gameObject);
      yield return (object) FlockadePieceManager.AwardPieces(pieces);
      GameManager.GetInstance().OnConversationEnd();
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__12_0() => this.isLoadingAssets = false;
}
