// Decompiled with JetBrains decompiler
// Type: Interaction_KnucklebonesBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using src.UI.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_KnucklebonesBuilding : Interaction
{
  public static List<Interaction_KnucklebonesBuilding> KnuckleboneBuildings = new List<Interaction_KnucklebonesBuilding>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject playerPosition;
  [SerializeField]
  public GameObject followerPosition;
  [SerializeField]
  public GameObject cameraPosition;
  public GameObject[] FollowerPositions;
  public GameObject[] Dice;
  [SerializeField]
  public KnucklebonesPlayerConfiguration knuckleBonesPlayerConfig;
  public UIKnuckleBonesController knuckleBonesInstance;

  public Structure Structure => this.structure;

  public void Start() => this.UpdateLocalisation();

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_KnucklebonesBuilding.KnuckleboneBuildings.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_KnucklebonesBuilding.KnuckleboneBuildings.Remove(this);
  }

  public override void GetLabel()
  {
    this.Interactable = DataManager.Instance.FollowersPlayedKnucklebonesToday.Count < 3;
    this.Label = this.Interactable ? ScriptLocalization.Interactions.PlayKnucklebones : ScriptLocalization.UI_Generic.Cooldown;
    this.HasSecondaryInteraction = PlayerFarming.playersCount > 1;
    this.HasThirdInteraction = TwitchAuthentication.IsAuthenticated;
    this.ThirdInteractable = TwitchAuthentication.IsAuthenticated;
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = PlayerFarming.playersCount > 1 ? LocalizationManager.GetTranslation("UI/PlayCoop") : "";
  }

  public override void GetThirdLabel()
  {
    base.GetThirdLabel();
    this.ThirdLabel = LocalizationManager.GetTranslation("UI/Twitch/VsChat").Colour(StaticColors.TwitchPurple) + " <sprite name=\"icon_TwitchIcon\">";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    this.StopFollowersPlaying();
    bool followerSelected = false;
    UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectMenu.VotingType = TwitchVoting.VotingType.KNUCKLEBONES;
    followerSelectMenu.Show(this.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (info =>
    {
      followerSelected = true;
      Follower followerById = FollowerManager.FindFollowerByID(info.ID);
      if (TimeManager.IsNight && followerById.Brain.CurrentTask != null && followerById.Brain.CurrentTask.State == FollowerTaskState.Doing && (followerById.Brain.CurrentTaskType == FollowerTaskType.Sleep || followerById.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
        CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, followerById.Brain.Info.ID);
      this.StartCoroutine((IEnumerator) this.PlayMatchIE(followerById));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() =>
    {
      if (followerSelected)
        return;
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      this.HasChanged = true;
      this.SetFollowerSpinePaused(false);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectMenu;
    selectMenuController3.OnShow = selectMenuController3.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
          followerInfoBox.ShowFaithGain(FollowerBrain.AdorationsAndActions[FollowerBrain.AdorationActions.KB_Win], followerInfoBox.followBrain.Stats.MAX_ADORATION);
        if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
          followerInfoBox.ShowDifficulty(Mathf.Clamp((float) followerInfoBox.followBrain.Info.XPLevel, 1f, 10f) / 10f);
      }
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectMenu;
    selectMenuController4.OnShownCompleted = selectMenuController4.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available && followerInfoBox.followBrain.Info.XPLevel < 10)
          followerInfoBox.ShowFaithGain(FollowerBrain.AdorationsAndActions[FollowerBrain.AdorationActions.KB_Win], followerInfoBox.followBrain.Stats.MAX_ADORATION);
        if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
          followerInfoBox.ShowDifficulty(Mathf.Clamp((float) followerInfoBox.followBrain.Info.XPLevel, 1f, 10f) / 10f);
      }
    });
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.StopFollowersPlaying();
    int num = state.GetComponent<PlayerFarming>().isLamb ? 1 : 0;
    this.StartCoroutine((IEnumerator) this.PlayMatchCoop());
  }

  public override void OnThirdInteract(StateMachine state)
  {
    base.OnThirdInteract(state);
    this.StopFollowersPlaying();
    int num = state.GetComponent<PlayerFarming>().isLamb ? 1 : 0;
    this.StartCoroutine((IEnumerator) this.PlayMatchTwitch());
  }

  public void GameQuit()
  {
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.UnPause();
    this.SetFollowerSpinePaused(false);
  }

  public IEnumerator PlayMatchIE(Follower targetFollower)
  {
    Interaction_KnucklebonesBuilding knucklebonesBuilding = this;
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadKnucklebonesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    UIKnuckleBonesController.KnucklebonesResult result = UIKnuckleBonesController.KnucklebonesResult.Loss;
    knucklebonesBuilding.SetFollowerSpinePaused(false);
    targetFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    SimulationManager.Pause();
    knucklebonesBuilding.playerFarming.GoToAndStop(knucklebonesBuilding.playerPosition.transform.position, knucklebonesBuilding.gameObject);
    targetFollower.transform.position = knucklebonesBuilding.followerPosition.transform.position;
    targetFollower.Spine.transform.position -= Vector3.forward * 0.2f;
    targetFollower.HideAllFollowerIcons();
    targetFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(0.5f);
    double num1 = (double) targetFollower.SetBodyAnimation("Reactions/react-bow", false);
    targetFollower.AddBodyAnimation("idle", true, 0.0f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(knucklebonesBuilding.cameraPosition, 4f);
    knucklebonesBuilding.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1.5f);
    knucklebonesBuilding.SetFollowerSpinePaused(true);
    AudioManager.Instance.PlayMusic("event:/music/ratau_home/ratau_home");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    KnucklebonesOpponent opponent = new KnucklebonesOpponent()
    {
      Config = knucklebonesBuilding.knuckleBonesPlayerConfig
    };
    opponent.Config.OpponentName = targetFollower.Brain.Info.Name;
    opponent.Config.Difficulty = Mathf.Clamp(targetFollower.Brain.Info.XPLevel, 1, 10);
    opponent.IsFollower = true;
    DataManager.Instance.FollowersPlayedKnucklebonesToday.Add(targetFollower.Brain.Info.ID);
    knucklebonesBuilding.knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    UIKnuckleBonesController knuckleBonesInstance1 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance1.OnHidden = knuckleBonesInstance1.OnHidden + new System.Action(SimulationManager.UnPause);
    knucklebonesBuilding.knuckleBonesInstance.Show(knucklebonesBuilding.playerFarming, opponent, 0);
    UIKnuckleBonesController knuckleBonesInstance2 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance2.OnShow = knuckleBonesInstance2.OnShow + (System.Action) (() =>
    {
      KBGameScreen gameScreen = this.knuckleBonesInstance.GameScreen;
      gameScreen.OnShow = gameScreen.OnShow + (System.Action) (() => FollowerBrain.SetFollowerCostume(this.knuckleBonesInstance.GameScreen.Opponent.Spine.Skeleton, targetFollower.Brain._directInfoAccess, forceUpdate: true, setData: false));
    });
    UIKnuckleBonesController knuckleBonesInstance3 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance3.OnHidden = knuckleBonesInstance3.OnHidden + (System.Action) (() => this.knuckleBonesInstance = (UIKnuckleBonesController) null);
    knucklebonesBuilding.knuckleBonesInstance.OnGameCompleted += (System.Action<UIKnuckleBonesController.KnucklebonesResult>) (r =>
    {
      result = r;
      this.DoFollowerThoughts(targetFollower.Brain, r);
    });
    knucklebonesBuilding.knuckleBonesInstance.OnGameQuit += new System.Action(knucklebonesBuilding.GameQuit);
    while ((UnityEngine.Object) knucklebonesBuilding.knuckleBonesInstance != (UnityEngine.Object) null)
      yield return (object) null;
    BiomeBaseManager.Instance.InitMusic();
    knucklebonesBuilding.SetFollowerSpinePaused(false);
    if (result != UIKnuckleBonesController.KnucklebonesResult.Loss)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(targetFollower.gameObject, 3f);
      yield return (object) new WaitForSeconds(0.25f);
      double num2 = (double) targetFollower.SetBodyAnimation("Reactions/react-admire" + UnityEngine.Random.Range(1, 4).ToString(), false);
      targetFollower.AddBodyAnimation("idle", true, 0.0f);
      yield return (object) new WaitForSeconds(1f);
      if (result == UIKnuckleBonesController.KnucklebonesResult.Win)
        targetFollower.Brain.AddAdoration(FollowerBrain.AdorationActions.KB_Win, (System.Action) null);
      else if (result == UIKnuckleBonesController.KnucklebonesResult.Draw)
        targetFollower.Brain.AddAdoration(FollowerBrain.AdorationActions.KB_Draw, (System.Action) null);
      yield return (object) new WaitForSeconds(2.5f);
    }
    MonoSingleton<UIManager>.Instance.UnloadKnucklebonesAssets();
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
    targetFollower.ShowAllFollowerIcons();
    targetFollower.Spine.transform.localPosition = Vector3.zero;
    targetFollower.Brain.CompleteCurrentTask();
  }

  public void DoFollowerThoughts(
    FollowerBrain followerBrain,
    UIKnuckleBonesController.KnucklebonesResult result)
  {
    ThoughtData thought = (ThoughtData) null;
    switch (result)
    {
      case UIKnuckleBonesController.KnucklebonesResult.Win:
        thought = FollowerThoughts.GetData(Thought.Knucklebones_Lost_2);
        break;
      case UIKnuckleBonesController.KnucklebonesResult.Loss:
        thought = FollowerThoughts.GetData(Thought.Knucklebones_Win_1);
        break;
      case UIKnuckleBonesController.KnucklebonesResult.Draw:
        thought = FollowerThoughts.GetData(Thought.Knucklebones_Draw_2);
        break;
    }
    if (thought == null)
      return;
    thought.Init();
    followerBrain.AddThought(thought);
  }

  public List<FollowerSelectEntry> GetFollowerSelectEntries()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (DataManager.Instance.FollowersPlayedKnucklebonesToday.Contains(follower.Brain.Info.ID))
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableAlreadyPlayedKnucklebonesToday));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerManager.GetFollowerAvailabilityStatus(follower)));
    }
    return followerSelectEntries;
  }

  public IEnumerator PlayMatchCoop()
  {
    Interaction_KnucklebonesBuilding knucklebonesBuilding = this;
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadKnucklebonesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    int count = 0;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      if (index == 0)
        PlayerFarming.players[index].GoToAndStop(knucklebonesBuilding.playerPosition.transform.position, knucklebonesBuilding.gameObject, GoToCallback: (System.Action) (() => ++count));
      else
        PlayerFarming.players[index].GoToAndStop(knucklebonesBuilding.followerPosition.transform.position, knucklebonesBuilding.gameObject, GoToCallback: (System.Action) (() => ++count));
    }
    UIKnuckleBonesController.KnucklebonesResult result = UIKnuckleBonesController.KnucklebonesResult.Loss;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(knucklebonesBuilding.cameraPosition, 4f);
    while (count < PlayerFarming.playersCount)
      yield return (object) null;
    knucklebonesBuilding.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayMusic("event:/music/ratau_home/ratau_home");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    KnucklebonesOpponent opponent = new KnucklebonesOpponent()
    {
      Config = knucklebonesBuilding.knuckleBonesPlayerConfig
    };
    opponent.Config.OpponentName = LocalizationManager.GetTranslation("NAMES/Knucklebones/Goat");
    opponent.IsCoopPlayer = true;
    knucklebonesBuilding.knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    UIKnuckleBonesController knuckleBonesInstance1 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance1.OnHidden = knuckleBonesInstance1.OnHidden + new System.Action(SimulationManager.UnPause);
    SimulationManager.Pause();
    knucklebonesBuilding.SetFollowerSpinePaused(true);
    bool gameQuit = true;
    knucklebonesBuilding.knuckleBonesInstance.Show(PlayerFarming.players[0], opponent, 0);
    UIKnuckleBonesController knuckleBonesInstance2 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance2.OnShow = knuckleBonesInstance2.OnShow + (System.Action) (() =>
    {
      KBGameScreen gameScreen = this.knuckleBonesInstance.GameScreen;
      gameScreen.OnShow = gameScreen.OnShow + (System.Action) (() => { });
    });
    UIKnuckleBonesController knuckleBonesInstance3 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance3.OnHidden = knuckleBonesInstance3.OnHidden + (System.Action) (() => this.knuckleBonesInstance = (UIKnuckleBonesController) null);
    knucklebonesBuilding.knuckleBonesInstance.OnGameCompleted += (System.Action<UIKnuckleBonesController.KnucklebonesResult>) (r =>
    {
      result = r;
      gameQuit = r == UIKnuckleBonesController.KnucklebonesResult.Quit;
    });
    knucklebonesBuilding.knuckleBonesInstance.OnGameQuit += (System.Action) (() => this.GameQuit());
    while ((UnityEngine.Object) knucklebonesBuilding.knuckleBonesInstance != (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    if (!gameQuit)
    {
      GameManager.GetInstance().OnConversationNext(knucklebonesBuilding.cameraPosition, 6f);
      string str = (double) UnityEngine.Random.value < 0.5 ? "" : "2";
      PlayerFarming.players[0].state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.players[1].state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.players[0].Spine.AnimationState.SetAnimation(0, result == UIKnuckleBonesController.KnucklebonesResult.Win ? "reactions/react-happy" + str : "reactions/react-angry" + str, false);
      PlayerFarming.players[1].Spine.AnimationState.SetAnimation(0, result == UIKnuckleBonesController.KnucklebonesResult.Loss ? "reactions/react-happy" + str : "reactions/react-angry" + str, false);
      yield return (object) new WaitForSeconds(1.5f);
    }
    MonoSingleton<UIManager>.Instance.UnloadKnucklebonesAssets();
    SimulationManager.UnPause();
    knucklebonesBuilding.SetFollowerSpinePaused(false);
    BiomeBaseManager.Instance.InitMusic();
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator PlayMatchTwitch()
  {
    Interaction_KnucklebonesBuilding knucklebonesBuilding = this;
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadKnucklebonesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    int count = 0;
    knucklebonesBuilding.playerFarming.GoToAndStop(knucklebonesBuilding.playerPosition.transform.position, knucklebonesBuilding.gameObject, GoToCallback: (System.Action) (() => ++count));
    UIKnuckleBonesController.KnucklebonesResult result = UIKnuckleBonesController.KnucklebonesResult.Loss;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(knucklebonesBuilding.cameraPosition, 4f);
    while (count < 1)
      yield return (object) null;
    knucklebonesBuilding.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayMusic("event:/music/ratau_home/ratau_home");
    AudioManager.Instance.SetMusicRoomID(1, SoundParams.Ratau);
    KnucklebonesOpponent opponent = new KnucklebonesOpponent()
    {
      Config = knucklebonesBuilding.knuckleBonesPlayerConfig
    };
    opponent.Config.OpponentName = LocalizationManager.GetTranslation("UI/Twitch/Chat");
    opponent.IsTwitchChat = true;
    knucklebonesBuilding.knuckleBonesInstance = MonoSingleton<UIManager>.Instance.KnucklebonesTemplate.Instantiate<UIKnuckleBonesController>();
    UIKnuckleBonesController knuckleBonesInstance1 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance1.OnHidden = knuckleBonesInstance1.OnHidden + new System.Action(SimulationManager.UnPause);
    SimulationManager.Pause();
    knucklebonesBuilding.SetFollowerSpinePaused(true);
    bool gameQuit = true;
    knucklebonesBuilding.knuckleBonesInstance.Show(PlayerFarming.players[0], opponent, 0);
    UIKnuckleBonesController knuckleBonesInstance2 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance2.OnShow = knuckleBonesInstance2.OnShow + (System.Action) (() =>
    {
      KBGameScreen gameScreen = this.knuckleBonesInstance.GameScreen;
      gameScreen.OnShow = gameScreen.OnShow + (System.Action) (() => { });
    });
    UIKnuckleBonesController knuckleBonesInstance3 = knucklebonesBuilding.knuckleBonesInstance;
    knuckleBonesInstance3.OnHidden = knuckleBonesInstance3.OnHidden + (System.Action) (() => this.knuckleBonesInstance = (UIKnuckleBonesController) null);
    knucklebonesBuilding.knuckleBonesInstance.OnGameCompleted += (System.Action<UIKnuckleBonesController.KnucklebonesResult>) (r =>
    {
      result = r;
      gameQuit = r == UIKnuckleBonesController.KnucklebonesResult.Quit;
    });
    knucklebonesBuilding.knuckleBonesInstance.OnGameQuit += (System.Action) (() => this.GameQuit());
    while ((UnityEngine.Object) knucklebonesBuilding.knuckleBonesInstance != (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    if (!gameQuit)
    {
      GameManager.GetInstance().OnConversationNext(knucklebonesBuilding.cameraPosition, 6f);
      string str = (double) UnityEngine.Random.value < 0.5 ? "" : "2";
      knucklebonesBuilding.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      knucklebonesBuilding.playerFarming.Spine.AnimationState.SetAnimation(0, result == UIKnuckleBonesController.KnucklebonesResult.Win ? "reactions/react-happy" + str : "reactions/react-angry" + str, false);
      yield return (object) new WaitForSeconds(1.5f);
    }
    MonoSingleton<UIManager>.Instance.UnloadKnucklebonesAssets();
    SimulationManager.UnPause();
    knucklebonesBuilding.SetFollowerSpinePaused(false);
    BiomeBaseManager.Instance.InitMusic();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void StopFollowersPlaying()
  {
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.Knucklebones)
        follower.Brain.CompleteCurrentTask();
    }
  }

  public void SetFollowerSpinePaused(bool state)
  {
    foreach (Follower follower in new List<Follower>((IEnumerable<Follower>) FollowerManager.Followers[FollowerLocation.Base]))
      follower.Spine.timeScale = state ? 0.0f : 1f;
  }
}
