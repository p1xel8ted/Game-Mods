// Decompiled with JetBrains decompiler
// Type: Onboarding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Onboarding : BaseMonoBehaviour
{
  public static Onboarding Instance;
  public GameObject Rat1Indoctrinate;
  public GameObject Rat2Shrine;
  public GameObject Rat2Food;
  public GameObject Rat3Devotion;
  public GameObject Rat4GoToDungeon;
  public GameObject Rat5PauseScreen;
  public GameObject Rat6Meditation;
  public GameObject Rat7MessageBoard;
  public GameObject RatReturnWithFolloer;
  public GameObject RatBonesAndRitual;
  public GameObject RatPreachSermon;
  public GameObject RatMonsterHeart;
  public GameObject RatLoyalty;
  public GameObject RatRitaul;
  public SkeletonAnimation RatauSpine;
  private float timeBetweenFollowerQuestCheck = 10f;
  private float timestamp;
  public GameObject UITutorialPrefab;

  public static DataManager.OnboardingPhase CurrentPhase
  {
    get => DataManager.Instance.CurrentOnboardingPhase;
    set => DataManager.Instance.CurrentOnboardingPhase = value;
  }

  private void OnEnable()
  {
    Onboarding.Instance = this;
    ObjectiveManager.OnObjectiveGroupCompleted += new Action<string>(this.OnObjectiveComplete);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    FollowerRecruit.OnNewRecruit += new System.Action(this.OnNewRecruit);
    DoctrineController.OnUnlockedFirstRitual += new System.Action(this.OnUnlockedFirstRitaul);
  }

  private void OnUnlockedFirstRitaul()
  {
    this.HideAll();
    DataManager.Instance.BonesEnabled = true;
    BaseGoopDoor.Instance.UnblockGoopDoor();
    this.RatRitaul.SetActive(true);
  }

  private void Start()
  {
    Debug.Log((object) "Onboarding Start");
    this.HideAll();
    if (!DataManager.Instance.InTutorial && !DataManager.Instance.Tutorial_First_Indoctoring)
    {
      Onboarding.CurrentPhase = DataManager.OnboardingPhase.Indoctrinate;
      TimeManager.PauseGameTime = true;
      this.Rat1Indoctrinate.SetActive(true);
      DataManager.Instance.AllowSaving = true;
      BaseGoopDoor.Instance.BlockGoopDoor();
    }
    else if (Onboarding.CurrentPhase == DataManager.OnboardingPhase.Indoctrinate)
      DataManager.Instance.InTutorial = true;
    if (DataManager.Instance.OnboardingFinished && !DataManager.Instance.DiscoveredLocations.Contains(FollowerLocation.Hub1_RatauOutside))
      DataManager.Instance.DiscoverLocation(FollowerLocation.Hub1_RatauOutside);
    if (ObjectiveManager.GroupExists("Objectives/GroupTitles/DeclareDoctrine") || ObjectiveManager.GroupExists("Objectives/GroupTitles/Temple"))
    {
      string loc = ObjectiveManager.GroupExists("Objectives/GroupTitles/DeclareDoctrine") ? "Objectives/Custom/DeclareDoctrine" : "Objectives/GroupTitles/Temple";
      BaseGoopDoor.Instance.BlockGoopDoor(loc);
    }
    if (ObjectiveManager.GroupExists("Objectives/GroupTitles/GoToDungeon") && DataManager.Instance.Followers_Recruit.Count > 0 && DataManager.Instance.KilledBosses.Count > 0)
    {
      int num = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) - StructuresData.GetCost(StructureBrain.TYPES.SHRINE)[0].CostValue;
      if (num < 0)
        Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, num * -1);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GetMoreGoldFromDungeon);
      Debug.Log((object) "YOU ARE BACK!!");
      Onboarding.CurrentPhase = DataManager.OnboardingPhase.IndoctrinateBerriesAllowed;
      TimeManager.PauseGameTime = true;
      DataManager.Instance.UnlockBaseTeleporter = false;
      BaseGoopDoor.Instance.BlockGoopDoor(ScriptLocalization.Interactions.IndoctrinateBeforeLeaving);
      DataManager.Instance.AllowBuilding = false;
      DataManager.Instance.BuildShrineEnabled = true;
      this.RatReturnWithFolloer.SetActive(true);
    }
    int itemQuantity1 = Inventory.GetItemQuantity(9);
    if (ObjectiveManager.GroupExists("Objectives/GroupTitles/BonesAndRitual"))
    {
      Debug.Log((object) "ONBOARDING SET UP FOR BONES AND RITUAL");
      List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(UpgradeSystem.Type.Ritual_FirePit);
      int num = 0;
      foreach (StructuresData.ItemCost itemCost in cost)
      {
        if (itemCost.CostItem == InventoryItem.ITEM_TYPE.BONE)
          num = itemCost.CostValue;
      }
      if (ObjectiveManager.GetAllObjectivesOfGroup("Objectives/GroupTitles/BonesAndRitual").First<ObjectivesData>((Func<ObjectivesData, bool>) (obj => obj is Objectives_CollectItem)) is Objectives_CollectItem objectivesCollectItem && objectivesCollectItem.Count < 25)
      {
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BONE, num - itemQuantity1);
        objectivesCollectItem.Count = objectivesCollectItem.Target;
      }
      int itemQuantity2 = Inventory.GetItemQuantity(9);
      Debug.Log((object) $"CurrentBones: {(object) itemQuantity2}   BoneCost: {(object) num}");
      if (itemQuantity2 >= num)
        BaseGoopDoor.Instance.BlockGoopDoor("Objectives/Custom/PerformAnyRitual");
    }
    if (FollowerBrain.AllBrains.Count > 1 && Onboarding.CurrentPhase != DataManager.OnboardingPhase.Shrine)
      Onboarding.CurrentPhase = DataManager.OnboardingPhase.Shrine;
    this.StartCoroutine((IEnumerator) this.WaitForPlayerFarmingToExist());
  }

  private IEnumerator WaitForPlayerFarmingToExist()
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while (MMConversation.isPlaying || LetterBox.IsPlaying)
      yield return (object) null;
    if (DataManager.Instance.CompletedObjectivesHistory.Count > 0 && !ObjectiveManager.GroupExists(DataManager.Instance.CompletedObjectivesHistory[0].GroupId) && !DataManager.Instance.OnboardingFinished)
      this.OnObjectiveComplete(DataManager.Instance.CompletedObjectivesHistory[0].GroupId);
  }

  private void Update()
  {
    if ((double) TimeManager.TotalElapsedGameTime <= (double) this.timestamp)
      return;
    this.timestamp = TimeManager.TotalElapsedGameTime + this.timeBetweenFollowerQuestCheck;
    this.TryGiveFollowerOnboardingQuest();
  }

  private void OnDisable()
  {
    ObjectiveManager.OnObjectiveGroupCompleted -= new Action<string>(this.OnObjectiveComplete);
    FollowerRecruit.OnNewRecruit -= new System.Action(this.OnNewRecruit);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    DoctrineController.OnUnlockedFirstRitual -= new System.Action(this.OnUnlockedFirstRitaul);
  }

  public void EndTutorial() => HUD_Manager.Instance.BaseDetailsTransition.MoveBackInFunction();

  public void EndTutorial2()
  {
    TimeManager.PauseGameTime = false;
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.TimeTransitions.MoveBackInFunction();
    DataManager.Instance.UnlockBaseTeleporter = true;
    BaseGoopDoor.Instance.UnblockGoopDoor();
  }

  public void OpenDoor()
  {
    DataManager.Instance.InTutorial = true;
    BaseGoopDoor.Instance.PlayOpenDoorSequence((System.Action) (() =>
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDungeon", Objectives.CustomQuestTypes.GoToDungeon));
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDungeon", Objectives.CustomQuestTypes.GetMoreGoldFromDungeon));
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDungeon", Objectives.CustomQuestTypes.GetNewFollowersFromDungeon));
    }));
    this.EndTutorial();
  }

  private void HideAll()
  {
    int index = -1;
    while (++index < this.transform.childCount)
      this.transform.GetChild(index).gameObject.SetActive(false);
  }

  private void OnStructureAdded(StructuresData structure)
  {
    if (structure.Type == StructureBrain.TYPES.BUILD_SITE && structure.ToBuildType == StructureBrain.TYPES.SHRINE)
    {
      foreach (Follower follower in Follower.Followers)
        follower.Brain.CompleteCurrentTask();
    }
    int type = (int) structure.Type;
    if (structure.Type != StructureBrain.TYPES.TEMPLE)
      return;
    int num = DataManager.Instance.OnboardingFinished ? 1 : 0;
  }

  private IEnumerator DelayedSetActive(GameObject obj, float delay = 0.5f)
  {
    yield return (object) new WaitForSeconds(delay);
    this.HideAll();
    obj.SetActive(true);
    obj.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
  }

  private void OnObjectiveComplete(string groupID)
  {
    if (UIMenuBase.ActiveMenus.Count > 0)
    {
      UIMenuBase.ActiveMenus[0].OnHidden += (System.Action) (() => this.OnObjectiveComplete(groupID));
    }
    else
    {
      string s = groupID;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 730659986:
          if (!(s == "Objectives/GroupTitles/Temple") || ObjectiveManager.GroupExists("Objectives/GroupTitles/PreachSermon"))
            break;
          Debug.Log((object) "UNLOCK PREACH SERMON!");
          this.HideAll();
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
          this.StartCoroutine((IEnumerator) this.WaitForPlayerToStopMeditiating((System.Action) (() =>
          {
            this.RatPreachSermon.SetActive(true);
            this.RatPreachSermon.GetComponent<Interaction_SimpleConversation>().Play();
            PlayerFarming.Instance.GoToAndStop(this.RatPreachSermon.transform.position + Vector3.right * 1.5f, DisableCollider: true);
            this.RatPreachSermon.GetComponent<Interaction_SimpleConversation>().OnInteraction += (Interaction.InteractionEvent) (state => PlayerFarming.Instance.EndGoToAndStop());
          })));
          break;
        case 744602802:
          int num = s == "Objectives/Custom/GetFollowerUpgradePoint" ? 1 : 0;
          break;
        case 789678384:
          if (!(s == "Objectives/GroupTitles/BonesAndRitual"))
            break;
          if (!DataManager.Instance.ShowLoyaltyBars && DataManager.Instance.HasPerformedRitual)
          {
            if (PlayerFarming.Location == FollowerLocation.Base)
              DataManager.Instance.DiscoverLocation(FollowerLocation.Hub1_RatauOutside);
            if (!ObjectiveManager.GroupExists("Objectives/GroupTitles/Disciple"))
            {
              ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.BlessAFollower), true);
              ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.GiveGift), true);
              ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.Disciple), true);
              ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.LoyaltyCollectReward), true);
            }
            DataManager.Instance.GivenLoyaltyQuestDay = TimeManager.CurrentDay;
            DataManager.Instance.OnboardingFinished = true;
            DataManager.Instance.ShowLoyaltyBars = true;
          }
          if (ObjectiveManager.GroupExists("Objectives/GroupTitles/DeclareDoctrine"))
            break;
          BaseGoopDoor.Instance.UnblockGoopDoor();
          break;
        case 798497968:
          if (!(s == "Objectives/GroupTitles/CookFirstMeal") || ObjectiveManager.GroupExists("Objectives/GroupTitles/GoToDungeon"))
            break;
          DataManager.Instance.RatExplainDungeon = false;
          this.HideAll();
          this.Rat4GoToDungeon.SetActive(true);
          this.Rat4GoToDungeon.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
          break;
        case 1262927861:
          if (!(s == "Objectives/GroupTitles/UnlockSacredKnowledge") || ObjectiveManager.GroupExists("Objectives/GroupTitles/CollectDivineInspiration"))
            break;
          Onboarding.CurrentPhase = DataManager.OnboardingPhase.Done;
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CollectDivineInspiration", Objectives.CustomQuestTypes.CollectDivineInspiration));
          break;
        case 1570524142:
          if (!(s == "Objectives/GroupTitles/RepairTheShrine") || ObjectiveManager.GroupExists("Objectives/GroupTitles/UnlockSacredKnowledge"))
            break;
          Onboarding.CurrentPhase = DataManager.OnboardingPhase.Shrine;
          DataManager.Instance.AllowBuilding = false;
          this.StartCoroutine((IEnumerator) this.DelayedSetActive(this.Rat3Devotion));
          break;
        case 1688499747:
          if (!(s == "Objectives/GroupTitles/Food") || ObjectiveManager.GroupExists("Objectives/GroupTitles/BuildCookingFire"))
            break;
          ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/BuildCookingFire", StructureBrain.TYPES.COOKING_FIRE));
          break;
        case 1747491650:
          if (!(s == "Objectives/GroupTitles/GoToDungeon") || ObjectiveManager.GroupExists("Objectives/GroupTitles/RepairTheShrine"))
            break;
          Debug.Log((object) "COMPLETED GO TO DUNGEON QUEST!");
          this.HideAll();
          this.Rat2Shrine.SetActive(true);
          this.Rat2Shrine.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
          DataManager.Instance.AllowBuilding = true;
          break;
        case 2022974580:
          if (!(s == "Objectives/GroupTitles/CollectDivineInspiration") || ObjectiveManager.GroupExists("Objectives/GroupTitles/Temple"))
            break;
          ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Temple", StructureBrain.TYPES.TEMPLE));
          break;
        case 2082029340:
          if (!(s == "Objectives/GroupTitles/PreachSermon") || DataManager.Instance.GivenSermonQuest || ObjectiveManager.GroupExists("Objectives/GroupTitles/BonesAndRitual"))
            break;
          this.HideAll();
          this.RatBonesAndRitual.SetActive(true);
          MMConversation.OnConversationEnd += new MMConversation.ConversationEnd(CompletedSermon);
          if (PlayerFarming.Location != FollowerLocation.Base)
            break;
          this.RatBonesAndRitual.transform.position += Vector3.down * 2f;
          break;
        case 2781065261:
          if (!(s == "Objectives/GroupTitles/RecruitFollower") || ObjectiveManager.GroupExists("Objectives/GroupTitles/Food"))
            break;
          this.StartCoroutine((IEnumerator) this.WaitForConversationToFinish((System.Action) (() =>
          {
            this.HideAll();
            this.Rat2Food.SetActive(true);
            this.Rat2Food.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
            DataManager.Instance.AllowBuilding = true;
          })));
          break;
        case 3053595497:
          if (!(s == "Objectives/GroupTitles/DeclareDoctrine") || ObjectiveManager.GroupExists("Objectives/GroupTitles/BonesAndRitual"))
            break;
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BonesAndRitual", Objectives.CustomQuestTypes.GoToDungeon), true);
          ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/BonesAndRitual", InventoryItem.ITEM_TYPE.BONE, 25), true);
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BonesAndRitual", Objectives.CustomQuestTypes.PerformAnyRitual), true);
          break;
        case 3187750823:
          if (!(s == "Objectives/GroupTitles/BuildCookingFire") || ObjectiveManager.GroupExists("Objectives/GroupTitles/CookFirstMeal"))
            break;
          if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Food))
          {
            UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Food);
            overlayController.OnHide = overlayController.OnHide + new System.Action(HungerBar.Instance.Reveal);
          }
          ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/CookFirstMeal", InventoryItem.ITEM_TYPE.BERRY, CookingData.GetRecipe(InventoryItem.ITEM_TYPE.MEAL_BERRIES)[0][0].quantity));
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CookFirstMeal", Objectives.CustomQuestTypes.CookFirstMeal));
          break;
      }
    }

    void CompletedSermon(bool SetPlayerToIdle, bool ShowHUD)
    {
      DataManager.Instance.GivenSermonQuest = true;
      MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(CompletedSermon);
    }
  }

  private IEnumerator WaitForPlayerToStopGoToAndStopping(System.Action callback)
  {
    Debug.Log((object) ("PlayerFarming.Instance.GoToAndStopping " + PlayerFarming.Instance.GoToAndStopping.ToString()));
    yield return (object) null;
    Debug.Log((object) ("PlayerFarming.Instance.GoToAndStopping " + PlayerFarming.Instance.GoToAndStopping.ToString()));
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Meditate)
      yield return (object) null;
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    Debug.Log((object) "TO ACTIVATE");
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator WaitForPlayerToStopMeditiating(System.Action callback)
  {
    yield return (object) null;
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Meditate)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator WaitForConversationToFinish(System.Action callback)
  {
    while (LetterBox.IsPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void ShowNewBuildingsAvailable()
  {
    DataManager.Instance.NewBuildings = true;
    System.Action buildingUnlocked = UpgradeSystem.OnBuildingUnlocked;
    if (buildingUnlocked == null)
      return;
    buildingUnlocked();
  }

  private void OnNewRecruit()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.IndoctrinateNewRecruit);
    Debug.Log((object) "A");
    if (Onboarding.CurrentPhase != DataManager.OnboardingPhase.Off && Onboarding.CurrentPhase != DataManager.OnboardingPhase.Indoctrinate && Onboarding.CurrentPhase != DataManager.OnboardingPhase.IndoctrinateBerriesAllowed && Onboarding.CurrentPhase != DataManager.OnboardingPhase.Off && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Temple))
    {
      Debug.Log((object) "B");
      Onboarding.CurrentPhase = DataManager.OnboardingPhase.Devotion;
      DataManager.Instance.AllowBuilding = true;
      DataManager.Instance.NewBuildings = true;
    }
    DataManager.Instance.Tutorial_First_Indoctoring = true;
  }

  public void MakeRatauAppearInDungeonGiveCurses()
  {
    DataManager.Instance.RatauToGiveCurseNextRun = true;
    DataManager.Instance.ShowHaroDoctrineStoneRoom = true;
  }

  public void AllowShrineBuild()
  {
  }

  public void LoyaltyRoutine() => this.StartCoroutine((IEnumerator) this.LoyaltyRoutineIE());

  private IEnumerator LoyaltyRoutineIE()
  {
    yield return (object) new WaitForEndOfFrame();
    DataManager.Instance.ShowLoyaltyBars = true;
    Interaction_TempleAltar.Instance.FrontWall.SetActive(false);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.RatLoyalty, 5f);
    GameManager.GetInstance().CameraSetOffset(this.RatLoyalty.GetComponent<Interaction_SimpleConversation>().CameraOffset);
    FollowerBrain followerBrain = (FollowerBrain) null;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        followerBrain = allBrain;
        break;
      }
    }
    bool waiting = true;
    AstarPath currentPath = AstarPath.active;
    AstarPath.active = (AstarPath) null;
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerBrain._directInfoAccess, ChurchFollowerManager.Instance.DoorPosition.position, ChurchFollowerManager.Instance.transform, FollowerLocation.Church);
    spawnedFollower.Follower.GoTo(this.RatLoyalty.transform.position + Vector3.right * 2f, (System.Action) (() => waiting = false));
    yield return (object) new WaitForEndOfFrame();
    while (waiting)
      yield return (object) null;
    spawnedFollower.Follower.FacePosition(this.RatLoyalty.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    waiting = true;
    if ((UnityEngine.Object) this.RatauSpine != (UnityEngine.Object) null)
    {
      this.RatauSpine.AnimationState.SetAnimation(0, "give-item", false);
      this.RatauSpine.AnimationState.AddAnimation(0, "idle", false, 0.0f);
    }
    spawnedFollower.Follower.AdorationUI.Show();
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.RatLoyalty.transform.position);
    ResourceCustomTarget.Create(spawnedFollower.Follower.gameObject, this.RatLoyalty.transform.position, InventoryItem.ITEM_TYPE.GIFT_SMALL, (System.Action) (() =>
    {
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", spawnedFollower.Follower.transform.position);
      spawnedFollower.FollowerFakeBrain.AddAdoration(spawnedFollower.Follower, FollowerBrain.AdorationActions.Gift, (System.Action) null);
      spawnedFollower.FollowerBrain.AddAdoration(spawnedFollower.Follower, FollowerBrain.AdorationActions.Gift, (System.Action) null);
      spawnedFollower.Follower.TimedAnimation("Reactions/react-enlightened1", 2f, (System.Action) (() => waiting = false));
    }));
    while (waiting)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    List<ConversationEntry> c = new List<ConversationEntry>()
    {
      new ConversationEntry(this.RatLoyalty, "Conversation_NPC/Ratau/Base/Loyalty/4", "talk-excited"),
      new ConversationEntry(this.RatLoyalty, "Conversation_NPC/Ratau/Spells/1/2")
    };
    c[0].CharacterName = ScriptLocalization.NAMES.Ratau;
    c[1].CharacterName = ScriptLocalization.NAMES.Ratau;
    c[0].soundPath = "event:/dialogue/ratau/standard_ratau";
    c[1].soundPath = "event:/dialogue/ratau/standard_ratau";
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.LevellingUp))
    {
      UITutorialOverlayController tutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.LevellingUp);
      UITutorialOverlayController overlayController = tutorialOverlay;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => tutorialOverlay = (UITutorialOverlayController) null);
      while ((UnityEngine.Object) tutorialOverlay != (UnityEngine.Object) null)
        yield return (object) null;
    }
    MMConversation.Play(new ConversationObject(c, (List<MMTools.Response>) null, (System.Action) (() =>
    {
      UnlockMapLocation component = this.GetComponent<UnlockMapLocation>();
      component.Callback.AddListener((UnityAction) (() =>
      {
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitRatau", Objectives.CustomQuestTypes.VisitRatau));
        this.StartCoroutine((IEnumerator) this.ActivateTeleporterRoutine());
      }));
      component.Play();
    })));
    spawnedFollower.Follower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) (() => FollowerManager.CleanUpCopyFollower(spawnedFollower)));
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    this.RatLoyalty.GetComponent<spineChangeAnimationSimple>().Play();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    Interaction_TempleAltar.Instance.FrontWall.SetActive(true);
    AstarPath.active = currentPath;
    if (!ObjectiveManager.GroupExists("Objectives/GroupTitles/Disciple"))
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.BlessAFollower), true);
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.GiveGift), true);
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.Disciple), true);
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Disciple", Objectives.CustomQuestTypes.LoyaltyCollectReward), true);
    }
    BaseGoopDoor.Instance.UnblockGoopDoor();
    DataManager.Instance.GivenLoyaltyQuestDay = TimeManager.CurrentDay;
    DataManager.Instance.OnboardingFinished = true;
  }

  private IEnumerator ActivateTeleporterRoutine()
  {
    while (PlayerFarming.Location != FollowerLocation.Base)
      yield return (object) null;
    Interaction_BaseTeleporter.Instance.ActivateRoutine();
  }

  private void ShowMissionBoard()
  {
    DataManager.Instance.Tutorial_Mission_Board = true;
    DataManager.Instance.MissionShrineUnlocked = true;
    this.Rat7MessageBoard.SetActive(true);
    this.Rat7MessageBoard.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
  }

  public void AddMission() => UnityEngine.Object.FindObjectOfType<Interaction_MissionShrine>().AddNewMission();

  public bool FirstFollower()
  {
    if (DataManager.Instance.Followers_Recruit.Count == 0)
    {
      string randomUnlockedSkin = DataManager.GetRandomUnlockedSkin();
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, randomUnlockedSkin);
      DataManager.SetFollowerSkinUnlocked(randomUnlockedSkin);
      DataManager.Instance.Followers_Recruit.Add(followerInfo);
    }
    if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count > 1)
      return false;
    BiomeBaseManager.Instance.SpawnExistingRecruits = true;
    FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
    UnityEngine.Object.FindObjectOfType<FollowerRecruit>().ManualTriggerAnimateIn();
    return true;
  }

  public void CreateFollowers() => this.StartCoroutine((IEnumerator) this.CreateFollowersRoutine());

  private IEnumerator CreateFollowersRoutine()
  {
    if (!BiomeBaseManager.Instance.SpawnExistingRecruits || DataManager.Instance.Followers_Recruit.Count <= 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew(true, true, true);
      GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 6f);
      yield return (object) new WaitForSeconds(0.5f);
      this.FirstFollower();
      yield return (object) new WaitForSeconds(2f);
      GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 8f);
      yield return (object) new WaitForSeconds(1f);
      GameManager.GetInstance().OnConversationEnd();
    }
    else
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      DataManager.Instance.Followers_Recruit.Add(followerInfo);
      DataManager.SetFollowerSkinUnlocked(followerInfo.SkinName);
    }
  }

  public void PlayShrineTutorial()
  {
    UnityEngine.Object.Instantiate<GameObject>(this.UITutorialPrefab, GameObject.FindWithTag("Canvas").transform);
  }

  public void ShowBaseFaith() => this.StartCoroutine((IEnumerator) this.ShowBaseFaithRoutine());

  private IEnumerator ShowBaseFaithRoutine()
  {
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.Instance.BaseDetailsTransition.MoveBackInFunction();
  }

  public void ShowPausePrompt()
  {
    Debug.Log((object) "PAUSE PROMPT!");
    this.StartCoroutine((IEnumerator) this.PausePromptRoutine());
  }

  private IEnumerator PausePromptRoutine()
  {
    yield return (object) null;
    if (PlayerFarming.Instance.GoToAndStopping)
      PlayerFarming.Instance.EndGoToAndStop();
    yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Control Prompt Tutorial Menu Screen"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    CanvasGroup ControlsHUD = g.GetComponent<CanvasGroup>();
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      ControlsHUD.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    while (!InputManager.Gameplay.GetMenuButtonHeld())
      yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) g);
    this.HideAll();
    this.Rat6Meditation.SetActive(true);
    this.Rat6Meditation.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
  }

  public void ShowMeditatePrompt()
  {
    Debug.Log((object) "MEDITATE PROMPT!");
    this.StartCoroutine((IEnumerator) this.ShowMeditatePromptRoutine());
  }

  private IEnumerator ShowMeditatePromptRoutine()
  {
    yield return (object) null;
    if (PlayerFarming.Instance.GoToAndStopping)
      PlayerFarming.Instance.EndGoToAndStop();
    yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "idle", true);
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Control Prompt Tutorial Meditate"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    CanvasGroup ControlsHUD = g.GetComponent<CanvasGroup>();
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      ControlsHUD.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    while (!InputManager.Gameplay.GetCurseButtonHeld())
      yield return (object) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) g);
    this.HideAll();
  }

  public void RevealCultFaith()
  {
    if (!(bool) (UnityEngine.Object) CultFaithManager.Instance)
      return;
    CultFaithManager.Instance.Reveal();
  }

  public void RevealCultHunger() => HungerBar.Instance.Reveal();

  private void TryGiveFollowerOnboardingQuest()
  {
    if (PlayerFarming.Location != FollowerLocation.Base || DataManager.Instance.Followers.Count == 0)
      return;
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers);
    for (int index = followerInfoList.Count - 1; index >= 0; --index)
    {
      if (!FollowerBrain.CanFollowerGiveQuest(followerInfoList[index]))
        followerInfoList.RemoveAt(index);
    }
    if (followerInfoList.Count == 0)
      return;
    if (DataManager.Instance.CurrentOnboardingFollowerID != -1)
    {
      Follower followerById = FollowerManager.FindFollowerByID(DataManager.Instance.CurrentOnboardingFollowerID);
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null || !FollowerBrain.CanContinueToGiveQuest(followerById.Brain._directInfoAccess))
        DataManager.Instance.CurrentOnboardingFollowerID = -1;
    }
    if (TimeManager.IsNight || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerQuestGivenTime < 480.0 || DataManager.Instance.CurrentOnboardingFollowerID != -1)
      return;
    foreach (FollowerInfo followerInfo in followerInfoList)
    {
      if (this.CanGiveMealQuest(followerInfo) || this.CanGiveCleanUpQuest(followerInfo) || this.CanGiveHousingQuest(followerInfo) || this.CanGiveFarmingQuest(followerInfo) || this.CanGiveCultNameQuest(followerInfo) || this.CanGiveIllnessQuest(followerInfo) || this.CanGiveDissentQuest(followerInfo) || this.CanGiveResourceYardQuest(followerInfo) || this.CanGiveCrisisOfFaithQuest() || this.CanGiveSermonQuest() || this.CanGiveRaiseFaithQuest() || this.CanGiveOldQuest(followerInfo) || this.CanGiveHalloweenQuest())
      {
        DataManager.Instance.CurrentOnboardingFollowerID = followerInfo.ID;
        FollowerBrain.GetOrCreateBrain(followerInfo)?.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveOnboarding, false));
        DataManager.Instance.LastFollowerQuestGivenTime = TimeManager.TotalElapsedGameTime;
        break;
      }
    }
  }

  public List<ObjectivesData> GetOnboardingQuests(int followerID)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
    List<ObjectivesData> onboardingQuests = new List<ObjectivesData>();
    if (this.CanGiveHalloweenQuest())
    {
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_PerformRitual("SeasonalEvents/Halloween/Onboarding/Title", UpgradeSystem.Type.Ritual_Halloween), infoById));
      DataManager.Instance.OnboardedHalloween = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/HalloweenEvent/PerformRitualRequest";
    }
    else if (this.CanGiveMealQuest(infoById))
    {
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/MakeMoreFood", Objectives.CustomQuestTypes.CookFirstMeal), infoById));
      DataManager.Instance.OnboardedMakingMoreFood = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/MakeMoreFood";
    }
    else if (this.CanGiveCleanUpQuest(infoById))
    {
      int count1 = Interaction_Poop.Poops.Count;
      int count2 = Vomit.Vomits.Count;
      if (count1 > 0)
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_RemoveStructure("Objectives/GroupTitles/CleanUpBase", StructureBrain.TYPES.POOP), infoById));
      if (count2 > 0)
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_RemoveStructure("Objectives/GroupTitles/CleanUpBase", StructureBrain.TYPES.VOMIT), infoById));
      DataManager.Instance.OnboardedCleaningBase = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/CleanUpBase";
    }
    else if (this.CanGiveHousingQuest(infoById))
    {
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Beds))
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuildHouse", UpgradeSystem.Type.Building_Beds), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/BuildHouse", StructureBrain.TYPES.BED), infoById));
      DataManager.Instance.OnboardedBuildingHouse = true;
      DataManager.Instance.OnboardedHomeless = true;
      UIDynamicNotificationCenter.HomelessFollowerData.CheckAll();
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/BuildHouse";
    }
    else if (this.CanGiveFarmingQuest(infoById))
    {
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Farms))
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuildFarm", UpgradeSystem.Type.Building_Farms), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/BuildFarm", StructureBrain.TYPES.FARM_PLOT), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildFarm", Objectives.CustomQuestTypes.PlantCrops), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildFarm", Objectives.CustomQuestTypes.WaterCrops), infoById));
      DataManager.Instance.OnboardedBuildFarm = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/BuildFarm";
    }
    else if (this.CanGiveCultNameQuest(infoById))
    {
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/Custom/NameCult", Objectives.CustomQuestTypes.NameCult), infoById));
      DataManager.Instance.OnboardedCultName = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/NameCult";
    }
    else if (this.CanGiveResourceYardQuest(infoById))
    {
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Lumberyard))
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuildResourceYard", UpgradeSystem.Type.Economy_Lumberyard), infoById));
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Mine))
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuildResourceYard", UpgradeSystem.Type.Economy_Mine), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/BuildResourceYard", StructureBrain.TYPES.LUMBERJACK_STATION), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/BuildResourceYard", StructureBrain.TYPES.BLOODSTONE_MINE), infoById));
      DataManager.Instance.OnboardedResourceYard = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/BuildResourceYard";
    }
    else if (this.CanGiveIllnessQuest(infoById))
    {
      bool flag = false;
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective.Type == Objectives.TYPES.REMOVE_STRUCTURES && (((Objectives_RemoveStructure) objective).StructureType == StructureBrain.TYPES.POOP || ((Objectives_RemoveStructure) objective).StructureType == StructureBrain.TYPES.VOMIT))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        if (Interaction_Poop.Poops.Count > 0)
          onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_RemoveStructure("Objectives/GroupTitles/SickFollower", StructureBrain.TYPES.POOP), infoById));
        if (Vomit.Vomits.Count > 0)
          onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_RemoveStructure("Objectives/GroupTitles/SickFollower", StructureBrain.TYPES.VOMIT), infoById));
      }
      DataManager.Instance.LastFollowerToBecomeIll = TimeManager.TotalElapsedGameTime;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/SickFollower";
      DataManager.Instance.OnboardedSickFollower = true;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BedRest("Objectives/GroupTitles/SickFollower", infoById.Name), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/SickFollower", Objectives.CustomQuestTypes.FollowerRecoverIllness, infoById.ID), infoById));
    }
    else if (this.CanGiveDissentQuest(infoById))
    {
      DataManager.Instance.OnboardedDissenter = true;
      DataManager.Instance.LastFollowerToStartDissenting = TimeManager.TotalElapsedGameTime;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboarding/CureDissenter";
      ObjectivesData quest = (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CureDissenter", Objectives.CustomQuestTypes.CureDissenter, infoById.ID);
      onboardingQuests.Add(this.OnboardingQuestAssigned(quest, infoById));
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.ID != infoById.ID)
        {
          quest.Follower = allBrain.Info.ID;
          break;
        }
      }
    }
    else if (this.CanGiveOldQuest(infoById))
    {
      DataManager.Instance.OnboardedOldFollower = true;
      DataManager.Instance.LastFollowerToReachOldAge = TimeManager.TotalElapsedGameTime;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/FollowerOnboardOldAge";
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Temple", Objectives.CustomQuestTypes.None, questExpireDuration: 2400f), infoById));
    }
    else if (this.CanGiveCrisisOfFaithQuest())
    {
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CrisisOfFaith", Objectives.CustomQuestTypes.CrisisOfFaith, questExpireDuration: 2400f), infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = "FollowerInteractions/GiveQuest/CrisisOfFaith";
      DataManager.Instance.TimeSinceLastCrisisOfFaithQuest = TimeManager.TotalElapsedGameTime;
    }
    return onboardingQuests;
  }

  private ObjectivesData OnboardingQuestAssigned(ObjectivesData quest, FollowerInfo follower)
  {
    FollowerBrain.GetOrCreateBrain(follower);
    quest.AutoRemoveQuestOnceComplete = false;
    quest.Follower = follower.ID;
    return quest;
  }

  private bool CanGiveMealQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedMakingMoreFood && DataManager.Instance.MealsCooked < 5 && StructureManager.GetAllStructuresOfType<Structures_Meal>().Count <= 0 && (double) follower.Satiation < 30.0;
  }

  private bool CanGiveCleanUpQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedCleaningBase && Interaction_Poop.Poops.Count + Vomit.Vomits.Count > 2;
  }

  private bool CanGiveHousingQuest(FollowerInfo follower)
  {
    if (DataManager.Instance.OnboardedBuildingHouse || FollowerBrain.GetOrCreateBrain(follower).HasHome || follower.DaysSleptOutside < 2 || follower.DwellingID != Dwelling.NO_HOME)
      return false;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_BuildStructure && ((Objectives_BuildStructure) objective).StructureType == StructureBrain.TYPES.BED)
        return false;
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective is Objectives_BuildStructure && ((Objectives_BuildStructure) completedObjective).StructureType == StructureBrain.TYPES.BED)
        return false;
    }
    return true;
  }

  private bool CanGiveFarmingQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedBuildFarm && TimeManager.CurrentDay > 3 && StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.FARM_PLOT).Count == 0 && StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SHRINE).Count != 0;
  }

  private bool CanGiveCultNameQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedCultName && TimeManager.CurrentDay > 2;
  }

  private bool CanGiveIllnessQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedSickFollower && 1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max < 0.25 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToBecomeIll > 600.0 / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier();
  }

  private bool CanGiveDissentQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedDissenter && (double) CultFaithManager.CurrentFaith / 85.0 < 0.25 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToStartDissenting > 600.0 / (double) DifficultyManager.GetTimeBetweenDissentingMultiplier();
  }

  private bool CanGiveOldQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedOldFollower && follower.Age >= follower.LifeExpectancy && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToReachOldAge > 600.0 / (double) DifficultyManager.GetTimeBetweenOldAgeMultiplier();
  }

  private bool CanGiveResourceYardQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedResourceYard && TimeManager.CurrentDay > 12 && !DataManager.Instance.HistoryOfStructures.Contains(StructureBrain.TYPES.LUMBERJACK_STATION) && !DataManager.Instance.HistoryOfStructures.Contains(StructureBrain.TYPES.BLOODSTONE_MINE);
  }

  private bool CanGiveCrisisOfFaithQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedCrisisOfFaith && TimeManager.CurrentDay > 5 && (double) CultFaithManager.CurrentFaith <= 5.0 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.TimeSinceLastCrisisOfFaithQuest > 3600.0 && (double) DataManager.Instance.TimeSinceFaithHitEmpty != -1.0 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.TimeSinceFaithHitEmpty > 240.0;
  }

  private bool CanGiveSermonQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedSermon && TimeManager.CurrentDay > 5 && TimeManager.CurrentDay < 10 && (double) CultFaithManager.CurrentFaith <= 42.5 && TimeManager.CurrentDay - DataManager.Instance.PreviousSermonDayIndex > 1;
  }

  private bool CanGiveFaithOfFlockQuest()
  {
    return !DataManager.Instance.OnboardedFaithOfFlock && DataManager.Instance.LastDaySincePlayerUpgrade != -1 && (TimeManager.CurrentDay - DataManager.Instance.LastDaySincePlayerUpgrade > 5 || DataManager.Instance.playerDeathsInARow >= 2 && TimeManager.CurrentDay < 10);
  }

  private bool CanGiveRaiseFaithQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedRaiseFaith && (double) CultFaithManager.CurrentFaith <= 42.5 && DataManager.Instance.GivenLoyaltyQuestDay != -1 && TimeManager.CurrentDay - DataManager.Instance.GivenLoyaltyQuestDay >= 1 && TimeManager.CurrentDay - DataManager.Instance.GivenLoyaltyQuestDay < 5;
  }

  private bool CanGiveHalloweenQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedHalloween && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Halloween) && SeasonalEventManager.IsSeasonalEventActive(SeasonalEventType.Halloween) && !FollowerBrainStats.IsBloodMoon && (double) DataManager.Instance.LastHalloween == -3.4028234663852886E+38;
  }
}
