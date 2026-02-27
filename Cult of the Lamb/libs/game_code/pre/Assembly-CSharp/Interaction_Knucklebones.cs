// Decompiled with JetBrains decompiler
// Type: Interaction_Knucklebones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.Tarot;
using MMTools;
using src.Extensions;
using src.UI.Menus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Knucklebones : Interaction
{
  public List<KnucklebonesOpponent> KnucklebonePlayers = new List<KnucklebonesOpponent>();
  public KnucklebonesOpponent KnuckleboneOpponent;
  public GameObject Exit;
  public GameObject Blocker;
  public GoopFade goopTransition;
  private UIKnuckleBonesController _knuckleBonesInstance;
  private List<KnucklebonesOpponent> _availableOpponents = new List<KnucklebonesOpponent>();
  private int _betAmount;
  private Interaction_TarotCardUnlock t;
  public GameObject CareToBetConversation;
  public Interaction_KeyPiece KeyPiecePrefab;

  private void Start()
  {
    this.Interactable = this.GetAvailableOpponents().Count > 0;
    this.UpdateLocalisation();
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
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this._availableOpponents.Clear();
    this._availableOpponents = this.GetAvailableOpponents();
  }

  private List<KnucklebonesOpponent> GetAvailableOpponents()
  {
    List<KnucklebonesOpponent> availableOpponents = new List<KnucklebonesOpponent>();
    List<KnucklebonesOpponent> list = this.KnucklebonePlayers.ToList<KnucklebonesOpponent>();
    if (DataManager.Instance.RatauKilled)
      list.Remove(list.LastElement<KnucklebonesOpponent>());
    foreach (KnucklebonesOpponent knucklebonesOpponent in list)
    {
      if (knucklebonesOpponent.Config.VariableToShow != DataManager.Variables.Knucklebones_Opponent_Ratau_Won)
      {
        Debug.Log((object) ("Variable Cast: " + (object) knucklebonesOpponent.Config.VariableToShow));
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

  private void GameQuit()
  {
    Debug.Log((object) "quitGame");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
  }

  private void CompleteGame(bool victory)
  {
    Debug.Log((object) "COMPLETE GAME!");
    this.GameQuit();
    if (victory)
    {
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
        Debug.Log((object) "FIRST TIME DE");
        flag = true;
      }
      else
      {
        Debug.Log((object) "Show Win Convo");
        if (this._betAmount > 0)
          this.StartCoroutine((IEnumerator) this.GiveGold(true));
      }
      DataManager.Instance.SetVariable(this.KnuckleboneOpponent.Config.VariableToChangeOnWin, true);
      if (flag)
        ObjectiveManager.CompleteDefeatKnucklebones(this.KnuckleboneOpponent.Config.OpponentName);
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("WIN_KNUCKLEBONES"));
    }
    else
    {
      Debug.Log((object) "Show Lose Convo");
      if (this._betAmount > 0)
        this.StartCoroutine((IEnumerator) this.GiveGold(false));
      this.KnuckleboneOpponent.LoseConvo.gameObject.SetActive(true);
    }
    if (!DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_0_Won) || !DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_1_Won) || !DataManager.Instance.GetVariable(DataManager.Variables.Knucklebones_Opponent_2_Won))
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("WIN_KNUCKLEBONES_ALL"));
  }

  private IEnumerator MakeBet()
  {
    bool betConfirmed = false;
    bool betCancelled = false;
    this._betAmount = 0;
    UIKnucklebonesBettingSelectionController selectionController = MonoSingleton<UIManager>.Instance.KnucklebonesBettingSelectionTemplate.Instantiate<UIKnucklebonesBettingSelectionController>();
    selectionController.Show(this.KnuckleboneOpponent.Config);
    selectionController.OnConfirmBet += (System.Action<int>) (bet =>
    {
      this._betAmount = bet * 5;
      ConfirmBet();
    });
    selectionController.OnHide = selectionController.OnHide + (System.Action) (() =>
    {
      if (betConfirmed)
        return;
      CancelBet();
    });
    while (!betConfirmed && !betCancelled)
      yield return (object) null;
    if (betConfirmed)
      yield return (object) this.ContinueToKnucklebones();
    else if (betCancelled)
      this.GameQuit();

    void ConfirmBet() => betConfirmed = true;

    void CancelBet() => betCancelled = true;
  }

  private IEnumerator SelectOpponent()
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
    selectionController.OnConfirmOpponent += new System.Action(ConfirmSelection);
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
      CancelSelection();
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

    void ConfirmSelection() => opponentSelected = true;

    void CancelSelection() => opponentCancelled = true;
  }

  private IEnumerator ContinueToKnucklebones()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.gameObject, 6f);
    interactionKnucklebones.goopTransition.FadeIn(1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1f);
    interactionKnucklebones._knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    SimulationManager.Pause();
    UIKnuckleBonesController knuckleBonesInstance1 = interactionKnucklebones._knuckleBonesInstance;
    knuckleBonesInstance1.OnHidden = knuckleBonesInstance1.OnHidden + new System.Action(SimulationManager.UnPause);
    interactionKnucklebones._knuckleBonesInstance.Show(interactionKnucklebones.KnuckleboneOpponent, interactionKnucklebones._betAmount);
    UIKnuckleBonesController knuckleBonesInstance2 = interactionKnucklebones._knuckleBonesInstance;
    // ISSUE: reference to a compiler-generated method
    knuckleBonesInstance2.OnHidden = knuckleBonesInstance2.OnHidden + new System.Action(interactionKnucklebones.\u003CContinueToKnucklebones\u003Eb__19_0);
    interactionKnucklebones._knuckleBonesInstance.OnGameCompleted += new System.Action<bool>(interactionKnucklebones.CompleteGame);
    interactionKnucklebones._knuckleBonesInstance.OnGameQuit += new System.Action(interactionKnucklebones.GameQuit);
    yield return (object) null;
  }

  private IEnumerator GiveGold(bool Won)
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKnucklebones.state.gameObject, 5f);
    PlayerFarming.Instance.state.facingAngle = 0.0f;
    PlayerFarming.Instance.state.LookAngle = 0.0f;
    int i;
    if (Won)
    {
      for (i = 0; i < interactionKnucklebones._betAmount; ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionKnucklebones.transform.position);
        ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, interactionKnucklebones.KnuckleboneOpponent.Spine.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1);
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
      }
    }
    else
    {
      for (i = 0; i < interactionKnucklebones._betAmount; ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionKnucklebones.transform.position);
        ResourceCustomTarget.Create(interactionKnucklebones.KnuckleboneOpponent.Spine.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
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
      if (!interactionKnucklebones.KnuckleboneOpponent.WinConvo.Spoken)
      {
        interactionKnucklebones.enabled = false;
        // ISSUE: reference to a compiler-generated method
        interactionKnucklebones.KnuckleboneOpponent.WinConvo.Callback.AddListener(new UnityAction(interactionKnucklebones.\u003CGiveGold\u003Eb__20_0));
        interactionKnucklebones.KnuckleboneOpponent.WinConvo.gameObject.SetActive(true);
      }
    }
    else if (!interactionKnucklebones.KnuckleboneOpponent.LoseConvo.Spoken)
    {
      interactionKnucklebones.enabled = false;
      // ISSUE: reference to a compiler-generated method
      interactionKnucklebones.KnuckleboneOpponent.LoseConvo.Callback.AddListener(new UnityAction(interactionKnucklebones.\u003CGiveGold\u003Eb__20_1));
      interactionKnucklebones.KnuckleboneOpponent.LoseConvo.gameObject.SetActive(true);
    }
  }

  private IEnumerator BlockDoor()
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

  private void TestReward(int Player = 3)
  {
    this.KnuckleboneOpponent = this.KnucklebonePlayers[Player];
    this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
  }

  public void GiveReward()
  {
  }

  private IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_Knucklebones interactionKnucklebones = this;
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    Debug.Log((object) ("Card Reward = " + (object) interactionKnucklebones.KnuckleboneOpponent.TarotCardReward));
    // ISSUE: reference to a compiler-generated method
    TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(interactionKnucklebones.KnuckleboneOpponent.Spine.transform.position + Vector3.back * 0.5f, PlayerFarming.Instance.transform.position + Vector3.back * 0.5f, 1f, interactionKnucklebones.KnuckleboneOpponent.TarotCardReward, new System.Action(interactionKnucklebones.\u003CGiveKeyPieceRoutine\u003Eb__27_0));
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject, 6f);
  }
}
