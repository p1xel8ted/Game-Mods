// Decompiled with JetBrains decompiler
// Type: Onboarding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.Managers;
using src.UI.Overlays.TutorialOverlay;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
  public GameObject RatSurvivalMode;
  public GameObject RatWinterMode;
  [SerializeField]
  public PlayerSleepBar playerSleepBar;
  [SerializeField]
  public PlayerHungerBar playerHungerBar;
  [SerializeField]
  public GameObject dlcBridge;
  [SerializeField]
  public GameObject dlcFog;
  [SerializeField]
  public GameObject dlcBridgeCameraTarget;
  [SerializeField]
  public Interaction_PurchaseLand baseExpansionSign;
  public SkeletonAnimation RatauSpine;
  public float timeBetweenFollowerQuestCheck = 10f;
  public float timestamp;
  public bool onboardingFollowerSin;
  public string DEBUG_MESSAGE = "";
  public GameObject UITutorialPrefab;

  public static DataManager.OnboardingPhase CurrentPhase
  {
    get => DataManager.Instance.CurrentOnboardingPhase;
    set => DataManager.Instance.CurrentOnboardingPhase = value;
  }

  public void OnEnable()
  {
    Onboarding.Instance = this;
    if (DataManager.Instance.QuickStartActive)
      return;
    ObjectiveManager.OnObjectiveGroupCompleted += new Action<string>(this.OnObjectiveComplete);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    FollowerRecruit.OnNewRecruit += new System.Action(this.OnNewRecruit);
    DoctrineController.OnUnlockedFirstRitual += new System.Action(this.OnUnlockedFirstRitaul);
  }

  public void OnUnlockedFirstRitaul()
  {
    this.HideAll();
    DataManager.Instance.BonesEnabled = true;
    BaseGoopDoor.UnblockGoopDoor();
    this.RatRitaul.SetActive(true);
  }

  public void Start()
  {
    Debug.Log((object) "Onboarding Start");
    this.HideAll();
    if (!DataManager.Instance.InTutorial && !DataManager.Instance.Tutorial_First_Indoctoring)
    {
      Onboarding.CurrentPhase = DataManager.OnboardingPhase.Indoctrinate;
      TimeManager.PauseGameTime = true;
      this.Rat1Indoctrinate.SetActive(true);
      DataManager.Instance.AllowSaving = true;
      BaseGoopDoor.BlockGoopDoor();
    }
    else if (Onboarding.CurrentPhase == DataManager.OnboardingPhase.Indoctrinate)
      DataManager.Instance.InTutorial = true;
    if (DataManager.Instance.OnboardingFinished && !DataManager.Instance.DiscoveredLocations.Contains(FollowerLocation.Hub1_RatauOutside))
      DataManager.Instance.DiscoverLocation(FollowerLocation.Hub1_RatauOutside);
    if (ObjectiveManager.GroupExists("Objectives/GroupTitles/DeclareDoctrine") || ObjectiveManager.GroupExists("Objectives/GroupTitles/Temple"))
      BaseGoopDoor.BlockGoopDoor(ObjectiveManager.GroupExists("Objectives/GroupTitles/DeclareDoctrine") ? "Objectives/Custom/DeclareDoctrine" : "Objectives/GroupTitles/Temple");
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
      BaseGoopDoor.BlockGoopDoor(ScriptLocalization.Interactions.IndoctrinateBeforeLeaving);
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
      List<ObjectivesData> objectivesOfGroup = ObjectiveManager.GetAllObjectivesOfGroup("Objectives/GroupTitles/BonesAndRitual");
      if (objectivesOfGroup.Count > 0 && objectivesOfGroup.First<ObjectivesData>((Func<ObjectivesData, bool>) (obj => obj is Objectives_CollectItem)) is Objectives_CollectItem objectivesCollectItem && objectivesCollectItem.Count < 25)
      {
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BONE, num - itemQuantity1);
        objectivesCollectItem.Count = objectivesCollectItem.Target;
      }
      int itemQuantity2 = Inventory.GetItemQuantity(9);
      Debug.Log((object) $"CurrentBones: {itemQuantity2.ToString()}   BoneCost: {num.ToString()}");
      if (itemQuantity2 >= num)
        BaseGoopDoor.BlockGoopDoor("Objectives/Custom/PerformAnyRitual");
    }
    if (FollowerBrain.AllBrains.Count > 1 && Onboarding.CurrentPhase != DataManager.OnboardingPhase.Shrine)
      Onboarding.CurrentPhase = DataManager.OnboardingPhase.Shrine;
    if (!DataManager.Instance.SurvivalModeOnboarded && DataManager.Instance.WinterModeActive)
    {
      DataManager.Instance.SurvivalModeOnboarded = true;
      this.RatWinterMode.gameObject.SetActive(true);
    }
    else if (!DataManager.Instance.SurvivalModeOnboarded && DataManager.Instance.SurvivalModeActive)
    {
      DataManager.Instance.SurvivalModeOnboarded = true;
      this.RatSurvivalMode.gameObject.SetActive(true);
    }
    this.StartCoroutine((IEnumerator) this.WaitForPlayerFarmingToExist((System.Action) (() =>
    {
      if (DataManager.Instance.CompletedObjectivesHistory.Count > 0 && !ObjectiveManager.GroupExists(DataManager.Instance.CompletedObjectivesHistory[0].GroupId) && !DataManager.Instance.OnboardingFinished)
        this.OnObjectiveComplete(DataManager.Instance.CompletedObjectivesHistory[0].GroupId);
      if (DataManager.Instance.RevealedBaseYngyaShrine || !DataManager.Instance.OnboardingFinished)
        return;
      if (DataManager.Instance.YngyaOffering == 0)
      {
        if (!DataManager.Instance.MAJOR_DLC && DataManager.Instance.BossesCompleted.Count < 4)
          return;
        this.StartCoroutine((IEnumerator) Interaction_DLCShrine.Instance.RevealIE());
      }
      else
        DataManager.Instance.RevealedBaseYngyaShrine = true;
    })));
    if (DataManager.Instance.OnboardingFinished && TimeManager.PauseGameTime)
      TimeManager.PauseGameTime = false;
    if (SeasonsManager.Active)
      this.StartCoroutine((IEnumerator) this.WaitForPlayerFarmingToExist((System.Action) (() =>
      {
        if (DataManager.Instance.OnboardedSeasons)
          return;
        DataManager.Instance.OnboardedSeasons = true;
        WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Dusting, 0.0f);
      })));
    this.StartCoroutine((IEnumerator) this.WaitForPlayerFarmingToExist((System.Action) (() =>
    {
      if (DataManager.Instance.YngyaOffering != -1 || DataManager.Instance.OnboardedDLCEntrance)
        return;
      this.OnboardDLCEntrance();
    })));
    if (DataManager.Instance.YngyaOffering == -1 && !DataManager.Instance.OnboardedDLCEntrance)
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Dusting);
    this.dlcBridge.gameObject.SetActive(DataManager.Instance.OnboardedDLCEntrance);
    this.baseExpansionSign.gameObject.SetActive(DataManager.Instance.LandPurchased <= 0 && DataManager.Instance.OnboardedBaseExpansion);
  }

  public IEnumerator WaitForPlayerFarmingToExist(System.Action callback)
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while (MMConversation.isPlaying || LetterBox.IsPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void Update()
  {
    if ((double) TimeManager.TotalElapsedGameTime <= (double) this.timestamp)
      return;
    this.timestamp = TimeManager.TotalElapsedGameTime + this.timeBetweenFollowerQuestCheck;
    this.TryGiveFollowerOnboardingQuest();
  }

  public void OnDisable()
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
    BaseGoopDoor.UnblockGoopDoor();
  }

  public void OpenDoor()
  {
    DataManager.Instance.InTutorial = true;
    BaseGoopDoor.MainDoor.PlayOpenDoorSequence((System.Action) (() =>
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDungeon", Objectives.CustomQuestTypes.GoToDungeon));
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDungeon", Objectives.CustomQuestTypes.GetMoreGoldFromDungeon));
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDungeon", Objectives.CustomQuestTypes.GetNewFollowersFromDungeon));
    }));
    this.EndTutorial();
  }

  public void HideAll()
  {
    int index = -1;
    while (++index < this.transform.childCount)
      this.transform.GetChild(index).gameObject.SetActive(false);
  }

  public void OnStructureAdded(StructuresData structure)
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

  public IEnumerator DelayedSetActive(GameObject obj, float delay = 0.5f)
  {
    yield return (object) new WaitForSeconds(delay);
    this.HideAll();
    obj.SetActive(true);
    obj.gameObject.GetComponent<Interaction_SimpleConversation>().Play();
  }

  public void OnObjectiveComplete(string groupID)
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
            PlayerFarming.Instance.GoToAndStop(this.RatPreachSermon.transform.position + Vector3.right * 1.5f, this.RatPreachSermon, DisableCollider: true, groupAction: true);
            this.StartCoroutine((IEnumerator) this.WaitForConversationToFinish((System.Action) (() =>
            {
              PlayerFarming.Instance.EndGoToAndStop();
              PlayerFarming.SetStateForAllPlayers();
            })));
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
            DataManager.Instance.UnlockBaseTeleporter = true;
            DataManager.Instance.GivenLoyaltyQuestDay = TimeManager.CurrentDay;
            DataManager.Instance.OnboardingFinished = true;
            DataManager.Instance.ShowLoyaltyBars = true;
          }
          if (ObjectiveManager.GroupExists("Objectives/GroupTitles/DeclareDoctrine"))
            break;
          BaseGoopDoor.UnblockGoopDoor();
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
          MMConversation.OnConversationEnd += (MMConversation.ConversationEnd) ((SetPlayerToIdle1, ShowHUD1) =>
          {
            DataManager.Instance.GivenSermonQuest = true;
            MMConversation.OnConversationEnd -= (MMConversation.ConversationEnd) ((SetPlayerToIdle2, ShowHUD2) =>
            {
              // ISSUE: unable to decompile the method.
            });
          });
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
  }

  public IEnumerator WaitForPlayerToStopGoToAndStopping(System.Action callback)
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

  public IEnumerator WaitForPlayerToStopMeditiating(System.Action callback)
  {
    yield return (object) null;
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Meditate)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator WaitForConversationToFinish(System.Action callback)
  {
    while (LetterBox.IsPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator OnboardFollowerSin(Follower follower)
  {
    yield return (object) new WaitForEndOfFrame();
    follower.HideAllFollowerIcons();
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.CurrentState = (FollowerState) new FollowerState_Floating();
    yield return (object) new WaitForEndOfFrame();
    follower.transform.position = Interaction_ShrinePleasure.Instance.transform.position;
    follower.Spine.transform.localPosition = new Vector3(0.0f, 0.0f, -15f);
    double num = (double) follower.SetBodyAnimation("sin-floating", true);
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || !PlayerFarming.LongToPerformPlayerStates.Contains(PlayerFarming.Instance.state.CURRENT_STATE))
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    follower.Spine.transform.DOLocalMove(new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-1f, -2f)), 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(3f);
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Floating());
    yield return (object) new WaitForSeconds(0.25f);
    Interaction_ShrinePleasure.Instance.UpdateBar();
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.OnboardedFollowerPleasure = true;
    GameManager.GetInstance().OnConversationEnd();
  }

  public void ShowNewBuildingsAvailable()
  {
    DataManager.Instance.NewBuildings = true;
    System.Action buildingUnlocked = UpgradeSystem.OnBuildingUnlocked;
    if (buildingUnlocked == null)
      return;
    buildingUnlocked();
  }

  public void OnNewRecruit()
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

  public IEnumerator LoyaltyRoutineIE()
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
    BaseGoopDoor.UnblockGoopDoor();
    DataManager.Instance.GivenLoyaltyQuestDay = TimeManager.CurrentDay;
    DataManager.Instance.OnboardingFinished = true;
  }

  public void ShowWinterUpgradeTree()
  {
    GameManager.GetInstance().OnConversationNew();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.Instance;
    MonoSingleton<UIManager>.Instance.ShowUpgradeTree((System.Action) (() =>
    {
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.WinterSystem);
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      foreach (Follower follower in Follower.Followers)
      {
        if (follower.Brain.CurrentTaskType == FollowerTaskType.GetPlayerAttention)
          follower.Brain.CompleteCurrentTask();
      }
      foreach (Follower follower in Follower.Followers)
      {
        if (FollowerBrain.CanFollowerGiveQuest(follower.Brain._directInfoAccess))
        {
          follower.transform.position = Vector3.zero;
          DataManager.Instance.CurrentOnboardingFollowerID = follower.Brain.Info.ID;
          follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveOnboarding, false)
          {
            AutoInteract = true
          });
          DataManager.Instance.LastFollowerQuestGivenTime = TimeManager.TotalElapsedGameTime;
          break;
        }
      }
    }), UpgradeSystem.Type.WinterSystem);
  }

  public void GiveSurvivalGold() => this.StartCoroutine((IEnumerator) this.GiveSurvivalGoldIE());

  public IEnumerator GiveSurvivalGoldIE()
  {
    Onboarding onboarding = this;
    yield return (object) onboarding.StartCoroutine((IEnumerator) onboarding.GiveGoldIE());
    onboarding.RatSurvivalMode.GetComponent<spineChangeAnimationSimple>().changeAnimation();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    yield return (object) onboarding.StartCoroutine((IEnumerator) onboarding.RevealSurvivalBars());
  }

  public IEnumerator RevealSurvivalBars()
  {
    Onboarding onboarding = this;
    yield return (object) onboarding.StartCoroutine((IEnumerator) onboarding.playerHungerBar.RevealRoutine());
    yield return (object) onboarding.StartCoroutine((IEnumerator) onboarding.playerSleepBar.RevealRoutine());
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure(ScriptLocalization.Objectives_GroupTitles.BuildCookingFire, StructureBrain.TYPES.COOKING_FIRE), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom(ScriptLocalization.Objectives_GroupTitles.BuildCookingFire, Objectives.CustomQuestTypes.EatMeal), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade(ScriptLocalization.Objectives_GroupTitles.BuildHouse, UpgradeSystem.Type.Building_Beds), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure(ScriptLocalization.Objectives_GroupTitles.BuildHouse, StructureBrain.TYPES.BED), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom(ScriptLocalization.Objectives_GroupTitles.BuildHouse, Objectives.CustomQuestTypes.ClaimBed), true);
  }

  public void GiveGold() => this.StartCoroutine((IEnumerator) this.GiveGoldIE());

  public IEnumerator GiveGoldIE()
  {
    for (int i = 0; i < 30; ++i)
    {
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.RatSurvivalMode.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
  }

  public void GiveLogStone() => this.StartCoroutine((IEnumerator) this.GiveLogStoneIE());

  public IEnumerator GiveLogStoneIE()
  {
    int i;
    for (i = 0; i < 10; ++i)
    {
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.RatSurvivalMode.transform.position, InventoryItem.ITEM_TYPE.LOG, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
    for (i = 0; i < 5; ++i)
    {
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.RatSurvivalMode.transform.position, InventoryItem.ITEM_TYPE.STONE, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
  }

  public void GiveRotstone() => this.StartCoroutine((IEnumerator) this.GiveRotstoneIE());

  public IEnumerator GiveRotstoneIE()
  {
    for (int i = 0; i < 20; ++i)
    {
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.RatSurvivalMode.transform.position, InventoryItem.ITEM_TYPE.MAGMA_STONE, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
  }

  public void FinishWinterOnlyConvo()
  {
    this.RatWinterMode.GetComponent<spineChangeAnimationSimple>().changeAnimation();
    GameManager.GetInstance().OnConversationEnd();
    Objectives_BuildStructure objective = new Objectives_BuildStructure("Objectives/GroupTitles/WarmCult", StructureBrain.TYPES.FURNACE_1);
    ObjectiveManager.Add((ObjectivesData) objective, true, true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/WarmCult", Objectives.CustomQuestTypes.LightFurnace), true, true);
    if (DataManager.Instance.SurvivalModeActive)
      this.StartCoroutine((IEnumerator) this.RevealSurvivalBars());
    DataManager.Instance.OnboardingFinished = false;
    BaseGoopDoor.BlockGoopDoor(objective.Text);
    DataManager.Instance.UnlockBaseTeleporter = false;
  }

  public IEnumerator ActivateTeleporterRoutine()
  {
    while (PlayerFarming.Location != FollowerLocation.Base)
      yield return (object) null;
    Interaction_BaseTeleporter.Instance.ActivateRoutine();
  }

  public void ShowMissionBoard()
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
    FollowerManager.SpawnExistingRecruit(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position).ManualTriggerAnimateIn();
    return true;
  }

  public void CreateFollowers() => this.StartCoroutine((IEnumerator) this.CreateFollowersRoutine());

  public IEnumerator CreateFollowersRoutine()
  {
    if (!BiomeBaseManager.Instance.SpawnExistingRecruits || DataManager.Instance.Followers_Recruit.Count <= 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew((PlayerFarming) null);
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

  public IEnumerator ShowBaseFaithRoutine()
  {
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.Instance.BaseDetailsTransition.MoveBackInFunction();
  }

  public void ShowPausePrompt()
  {
    Debug.Log((object) "PAUSE PROMPT!");
    this.StartCoroutine((IEnumerator) this.PausePromptRoutine());
  }

  public IEnumerator PausePromptRoutine()
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
    PlayerFarming.SetStateForAllPlayers();
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

  public IEnumerator ShowMeditatePromptRoutine()
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

  public void OnboardDLCEntrance()
  {
    this.StartCoroutine((IEnumerator) this.OnboardDLCEntranceIE());
  }

  public IEnumerator OnboardDLCEntranceIE()
  {
    Onboarding onboarding = this;
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying || LetterBox.IsPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(onboarding.dlcBridgeCameraTarget);
    yield return (object) new WaitForSeconds(1f);
    onboarding.dlcFog.transform.DOMoveZ(3f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    onboarding.StartCoroutine((IEnumerator) onboarding.ShakeCameraWithRampUp());
    AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/bridge_appear", onboarding.dlcBridge.transform.position);
    yield return (object) new WaitForSeconds(1.5f);
    onboarding.dlcBridge.transform.position = new Vector3(onboarding.dlcBridge.transform.position.x, onboarding.dlcBridge.transform.position.y, 1f);
    onboarding.StartCoroutine((IEnumerator) onboarding.BridgeSmokeRoutine(2.5f));
    yield return (object) new WaitForSeconds(0.5f);
    onboarding.dlcBridge.gameObject.SetActive(true);
    onboarding.dlcBridge.transform.DOMoveZ(-1.1f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(4f);
    onboarding.dlcFog.transform.DOMoveZ(6.13f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    DataManager.Instance.OnboardedDLCEntrance = true;
    BaseGoopDoor.DLCDoor.BlockingCollider.SetActive(false);
    BaseGoopDoor.DLCDoor.SetReveal(true);
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitLambTown", Objectives.CustomQuestTypes.VisitLambTown), true, true);
  }

  public IEnumerator BridgeSmokeRoutine(float Duration)
  {
    float Timer = 0.0f;
    float emitInterval = 0.1f;
    float Scale = 5f;
    while ((double) (Timer += Time.deltaTime) < (double) Duration + 0.20000000298023224)
    {
      if ((double) Timer % (double) emitInterval < (double) Time.deltaTime)
      {
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.dlcBridge.transform.position + new Vector3(1.5f, 1.5f), Vector3.one * Scale);
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.dlcBridge.transform.position + new Vector3(4.5f, 1.5f), Vector3.one * Scale);
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.dlcBridge.transform.position + new Vector3(7.5f, 1.5f), Vector3.one * Scale);
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.dlcBridge.transform.position + new Vector3(10.5f, 1.5f), Vector3.one * Scale);
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.dlcBridge.transform.position + new Vector3(10.5f, 4.5f), Vector3.one * Scale);
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.dlcBridge.transform.position + new Vector3(10.5f, 7.5f), Vector3.one * Scale);
      }
      yield return (object) null;
    }
  }

  public IEnumerator OnboardBaseExpansionIE()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.baseExpansionSign.gameObject);
    yield return (object) new WaitForSeconds(1.5f);
    this.baseExpansionSign.gameObject.SetActive(true);
    this.baseExpansionSign.ForceShown = true;
    yield return (object) new WaitForSeconds(3f);
    DataManager.Instance.OnboardedBaseExpansion = true;
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator ShakeCameraWithRampUp()
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 3.9000000953674316)
    {
      float t1 = t / 3.9f;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), 3.9f, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }

  public void RevealCultFaith()
  {
    if (!(bool) (UnityEngine.Object) CultFaithManager.Instance)
      return;
    CultFaithManager.Instance.Reveal();
  }

  public void RevealCultHunger() => HungerBar.Instance.Reveal();

  public void TryGiveFollowerOnboardingQuest()
  {
    if (PlayerFarming.Location != FollowerLocation.Base || DataManager.Instance.Followers.Count == 0)
      return;
    List<FollowerInfo> ts = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers);
    for (int index = ts.Count - 1; index >= 0; --index)
    {
      if (!FollowerBrain.CanFollowerGiveQuest(ts[index]))
        ts.RemoveAt(index);
    }
    if (ts.Count == 0)
      return;
    ts.Shuffle<FollowerInfo>();
    if (DataManager.Instance.CurrentOnboardingFollowerID != -1)
    {
      Follower followerById = FollowerManager.FindFollowerByID(DataManager.Instance.CurrentOnboardingFollowerID);
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
        DataManager.Instance.CurrentOnboardingFollowerID = -1;
      else if (!FollowerBrain.CanContinueToGiveQuest(followerById.Brain._directInfoAccess, true) || followerById.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention)
        DataManager.Instance.CurrentOnboardingFollowerID = -1;
    }
    if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerQuestGivenTime < 480.0 || DataManager.Instance.CurrentOnboardingFollowerID != -1)
      return;
    foreach (FollowerInfo followerInfo in ts)
    {
      if ((!TimeManager.IsNight || followerInfo.Traits.Contains(FollowerTrait.TraitType.Insomniac)) && (followerInfo.CursedState != Thought.Child && (this.CanGiveMealQuest(followerInfo) || this.CanGiveCleanUpQuest(followerInfo) || this.CanGiveHousingQuest(followerInfo) || this.CanGiveFarmingQuest(followerInfo) || this.CanGiveCultNameQuest(followerInfo) || this.CanGiveIllnessQuest(followerInfo) || this.CanGiveDissentQuest(followerInfo) || this.CanGiveResourceYardQuest(followerInfo) || this.CanGiveCrisisOfFaithQuest() || this.CanGiveSermonQuest() || this.CanGiveRaiseFaithQuest() || this.CanGiveOldQuest(followerInfo) || this.CanGiveHalloweenQuest() || this.CanGiveDeathCatAymAndBaalSecret(followerInfo) || this.CanGiveShamuraAymAndBaalSecret(followerInfo) || this.CanGiveLeshyRelicQuest(followerInfo) || this.CanGiveHeketRelicQuest(followerInfo) || this.CanGiveKallamarRelicQuest(followerInfo) || this.CanGiveShamuraRelicQuest(followerInfo) || this.CanGiveDiscipleOnboarding(followerInfo) || this.CanGiveScaredTraitFightFollowerConvo(followerInfo) || this.CanGiveScaredTraitFightPitConvo(followerInfo) || this.CanGiveSozoMushroomoQuest(followerInfo) || this.CanOnboardDrunk(followerInfo) || this.CanGivePilgrimPart1Quest(followerInfo) || this.CanGivePilgrimPart2Quest(followerInfo) || this.CanGivePilgrimPart3Quest(followerInfo) || this.CanGiveValentinQuest(followerInfo) || this.CanGiveLeshyHealingQuest(followerInfo) || this.CanGiveHeketHealingQuest(followerInfo) || this.CanGiveKallamarHealingQuest(followerInfo) || this.CanGiveShamuraHealingQuest(followerInfo) || this.CanGiveDeathCatRelic(followerInfo) || this.CanGiveYarlenRandomLine(followerInfo) || this.CanGiveRinorRandomLine(followerInfo) || this.CanGiveMidasQuestHelp() || this.CanOnboardWinterAlmostHere() || this.CanOnboardRanchChoppingBlock() || this.CanGivePerformBonfireRitualQuest(followerInfo) || this.CanGiveFlowerBasketsQuest(followerInfo) || this.CanRecieveLegendaryAxe(followerInfo) || this.CanGiveFeedAnimalFollowerMeat() || this.CanGiveBringSnowmanToLife() || this.CanGivePureBloodMatingQuest(followerInfo)) || followerInfo.CursedState == Thought.Child && this.CanBecomeAdult(followerInfo) || followerInfo.CursedState == Thought.Child && this.CanGiveChosenChildQuest(followerInfo)))
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
    if (infoById.CursedState == Thought.Child)
    {
      if (this.CanBecomeAdult(infoById))
      {
        onboardingQuests.Add((ObjectivesData) null);
        infoById.BabyIgnored = (double) UnityEngine.Random.value > (double) infoById.CuddledAmount / 4.0 && !infoById.Traits.Contains(FollowerTrait.TraitType.Zombie) && infoById.ID != 100006 && !infoById.IsSnowman && !FollowerManager.LambIDs.Contains(followerID) && !infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood_1) && !infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood_2) && !infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood_3);
        DataManager.Instance.CurrentOnboardingFollowerTerm = !infoById.BabyIgnored || infoById.Traits.Contains(FollowerTrait.TraitType.Mutated) ? (infoById.ID != 100006 ? (!infoById.IsSnowman ? $"Conversation_NPC/ReadyToBecomeAdult_{UnityEngine.Random.Range(0, 3)}/0" : "Conversation_NPC/Snowman_ReadyToBecomeAdult/0") : "Conversation_NPC/Midas_ReadyToBecomeAdult/0") : $"Conversation_NPC/BadChildhood_{UnityEngine.Random.Range(0, 3)}/0";
      }
      else if (this.CanGiveChosenChildQuest(infoById))
      {
        Objectives_Custom objectivesCustom = new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.LegendarySword, followerID);
        objectivesCustom.FailLocked = true;
        onboardingQuests.Add((ObjectivesData) objectivesCustom);
        DataManager.Instance.GaveChosenChildQuest = true;
        DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/ChosenChild/LegendarySword/Call/0";
      }
    }
    else if (this.CanOnboardWinterAlmostHere())
    {
      DataManager.Instance.FollowerOnboardedWinterAlmostHere = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Follower/FirstSnow/0";
      onboardingQuests.Add((ObjectivesData) null);
    }
    else if (this.CanGiveHalloweenQuest())
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
    else if (this.CanGiveDeathCatAymAndBaalSecret(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.DeathCatBaalAndAymSecret = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/DeathCat_AymAndBaal/0";
    }
    else if (this.CanGiveShamuraAymAndBaalSecret(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.ShamuraBaalAndAymSecret = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Shaumra_AymAndBaal/0";
    }
    else if (this.CanGiveLeshyRelicQuest(infoById))
    {
      Objective_FindRelic quest = new Objective_FindRelic("Objectives/GroupTitles/FindRelic", FollowerLocation.Dungeon1_1, RelicType.DamageOnTouch_Familiar);
      quest.FailLocked = true;
      quest.CompleteTerm = "Dungeon1_1/QuestCompleted";
      DataManager.Instance.LeshyHealingQuestDay = TimeManager.CurrentDay;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Dungeon1_1/GiveQuest/0";
    }
    else if (this.CanGiveLeshyHealingQuest(infoById))
    {
      Objectives_Custom objectivesCustom = new Objectives_Custom("Objectives/GroupTitles/HealingBishop", Objectives.CustomQuestTypes.HealingBishop_Leshy, infoById.ID);
      objectivesCustom.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) objectivesCustom);
      DataManager.Instance.GaveLeshyHealingQuest = true;
      DataManager.Instance.HealingQuestDay = TimeManager.CurrentDay;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Leshy/Healing/First/0";
    }
    else if (this.CanGiveHeketRelicQuest(infoById))
    {
      Objective_FindRelic quest = new Objective_FindRelic("Objectives/GroupTitles/FindRelic", FollowerLocation.Dungeon1_2, RelicType.IncreaseDamageForDuration);
      quest.FailLocked = true;
      quest.CompleteTerm = "Dungeon1_2/QuestCompleted";
      DataManager.Instance.HeketHealingQuestDay = TimeManager.CurrentDay;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Dungeon1_2/GiveQuest/0";
    }
    else if (this.CanGiveHeketHealingQuest(infoById))
    {
      Objectives_Custom objectivesCustom = new Objectives_Custom("Objectives/GroupTitles/HealingBishop", Objectives.CustomQuestTypes.HealingBishop_Heket, infoById.ID);
      objectivesCustom.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) objectivesCustom);
      DataManager.Instance.GaveHeketHealingQuest = true;
      DataManager.Instance.HealingQuestDay = TimeManager.CurrentDay;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Heket/Healing/First/0";
    }
    else if (this.CanGiveKallamarRelicQuest(infoById))
    {
      Objective_FindRelic quest = new Objective_FindRelic("Objectives/GroupTitles/FindRelic", FollowerLocation.Dungeon1_3, RelicType.SpawnCombatFollower);
      quest.Follower = infoById.ID;
      quest.FailLocked = true;
      quest.CompleteTerm = "Dungeon1_3/QuestCompleted";
      DataManager.Instance.KallamarHealingQuestDay = TimeManager.CurrentDay;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Dungeon1_3/GiveQuest/0";
    }
    else if (this.CanGiveKallamarHealingQuest(infoById))
    {
      Objectives_Custom objectivesCustom = new Objectives_Custom("Objectives/GroupTitles/HealingBishop", Objectives.CustomQuestTypes.HealingBishop_Kallamar, infoById.ID);
      objectivesCustom.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) objectivesCustom);
      DataManager.Instance.GaveKallamarHealingQuest = true;
      DataManager.Instance.HealingQuestDay = TimeManager.CurrentDay;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Kallamar/Healing/First/0";
    }
    else if (this.CanGiveShamuraRelicQuest(infoById))
    {
      Objective_FindRelic quest = new Objective_FindRelic("Objectives/GroupTitles/FindRelic", FollowerLocation.Dungeon1_4, RelicType.GungeonBlank);
      quest.FailLocked = true;
      quest.CompleteTerm = "Dungeon1_4/QuestCompleted";
      DataManager.Instance.ShamuraHealingQuestDay = TimeManager.CurrentDay;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Dungeon1_4/GiveQuest/0";
    }
    else if (this.CanGiveShamuraHealingQuest(infoById))
    {
      Objectives_Custom objectivesCustom = new Objectives_Custom("Objectives/GroupTitles/HealingBishop", Objectives.CustomQuestTypes.HealingBishop_Shamura, infoById.ID);
      objectivesCustom.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) objectivesCustom);
      DataManager.Instance.GaveShamuraHealingQuest = true;
      DataManager.Instance.HealingQuestDay = TimeManager.CurrentDay;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Shamura/Healing/First/0";
    }
    else if (this.CanGiveDiscipleOnboarding(infoById))
    {
      Objectives_PerformRitual quest = new Objectives_PerformRitual("Objectives/GroupTitles/Disciple", UpgradeSystem.Type.Ritual_BecomeDisciple, infoById.ID, 1);
      quest.FailLocked = true;
      quest.CompleteTerm = "BecameDisciple/0";
      if (infoById.BornInCult)
        quest.CompleteTerm = "BecameDisciple/BornInCult/0";
      else if (FollowerManager.BishopIDs.Contains(infoById.ID))
        quest.CompleteTerm = "BecameDisciple/Bishop/0";
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
      DataManager.Instance.OnboardedDisciple = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/BecomeDisciple/0";
    }
    else if (this.CanGiveScaredTraitFightFollowerConvo(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      ++infoById.FollowersFought;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/TraitConversation/Scared/FollowerFight/0";
    }
    else if (this.CanGiveScaredTraitFightPitConvo(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      ++infoById.FightPitsFought;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/TraitConversation/Scared/FightPit/0";
    }
    else if (this.CanOnboardDrunk(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.DrunkDay = TimeManager.CurrentDay;
      DataManager.Instance.CurrentOnboardingFollowerTerm = $"Conversation_NPC/DrunkConvo_{DataManager.Instance.DrunkIncrement}/0";
      DataManager.Instance.DrunkIncrement = (int) Utils.Repeat((float) (DataManager.Instance.DrunkIncrement + 1), 4f);
      if (infoById.ID == 99991)
        DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/DrunkConvo_0/0/Heket";
      else
        DataManager.Instance.CurrentOnboardingFollowerTerm += "/Bishop";
    }
    else if (this.CanGivePilgrimPart1Quest(infoById))
    {
      Objectives_RecruitCursedFollower recruitCursedFollower = new Objectives_RecruitCursedFollower("Objectives/GroupTitles/PilgrimsQuest", Thought.None)
      {
        FollowerID = 99998,
        FollowerName = "Yarlen"
      };
      recruitCursedFollower.FollowerName = LocalizeIntegration.Arabic_ReverseNonRTL(recruitCursedFollower.FollowerName);
      recruitCursedFollower.FollowerSkin = "Panda";
      recruitCursedFollower.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) recruitCursedFollower);
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/PilgrimsPartOne/0";
      DataManager.Instance.PilgrimPart2TargetDay = TimeManager.CurrentDay + 3;
      DataManager.Instance.HasAcceptedPilgrimPart1 = true;
    }
    else if (this.CanGivePilgrimPart2Quest(infoById))
    {
      Objectives_FindFollower objectivesFindFollower = new Objectives_FindFollower("Objectives/GroupTitles/PilgrimsQuest", FollowerLocation.Dungeon1_4, "Jalala", 0, 0, "Jalala", 0);
      objectivesFindFollower.CompleteTerm = "Pilgrims/PartTwo/0";
      objectivesFindFollower.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) objectivesFindFollower);
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/PilgrimsPartTwo/0";
      DataManager.Instance.PilgrimPart2TargetDay = TimeManager.CurrentDay + 1;
      DataManager.Instance.PilgrimPart3TargetDay = TimeManager.CurrentDay + 3;
      DataManager.Instance.HasAcceptedPilgrimPart2 = true;
      DataManager.Instance.PilgrimTargetLocation = FollowerLocation.Dungeon1_4;
    }
    else if (this.CanGivePilgrimPart3Quest(infoById))
    {
      Objectives_Custom objectivesCustom = new Objectives_Custom("Objectives/GroupTitles/PilgrimsQuest", Objectives.CustomQuestTypes.FindJalalaBag);
      objectivesCustom.CompleteTerm = "PilgrimsPartFour/0";
      objectivesCustom.FailLocked = true;
      onboardingQuests.Add((ObjectivesData) objectivesCustom);
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/PilgrimsPartThree/0";
      DataManager.Instance.PilgrimPart3TargetDay = TimeManager.CurrentDay + 1;
      DataManager.Instance.HasAcceptedPilgrimPart3 = true;
    }
    else if (this.CanGiveYarlenRandomLine(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.GivenYarlenLine = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/YarlenLine/0";
    }
    else if (this.CanGiveRinorRandomLine(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.GivenRinorLine = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/RinorLine/0";
    }
    else if (this.CanGiveValentinQuest(infoById))
    {
      DataManager.Instance.ValentinsDayYear = DateTime.Now.Year;
      Objectives_Custom objectivesCustom = new Objectives_Custom("", Objectives.CustomQuestTypes.None);
      objectivesCustom.Type = Objectives.TYPES.NONE;
      onboardingQuests.Add((ObjectivesData) objectivesCustom);
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Valentines/0";
    }
    else if (this.CanGiveDeathCatRelic(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/DeathCat/Relic/First/0";
    }
    else if (this.CanOnboardRanchChoppingBlock())
    {
      DataManager.Instance.FollowerOnboardedRanchChoppingBlock = true;
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RanchChoppingBlock))
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/RanchChoppingBlock", UpgradeSystem.Type.Building_RanchChoppingBlock), infoById));
      if (StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_CHOPPING_BLOCK).Count <= 0)
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/RanchChoppingBlock", StructureBrain.TYPES.RANCH_CHOPPING_BLOCK), infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/OnboardChoppingBlock/0";
    }
    else if (this.CanGiveMidasQuestHelp())
    {
      DataManager.Instance.GivenMidasFollowerQuest = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/MidasFollowerQuestHelp/0";
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Wolf_Trap))
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/TrappingMidas", UpgradeSystem.Type.Building_Wolf_Trap), infoById));
      if (StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.WOLF_TRAP).Count <= 0)
        onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/TrappingMidas", StructureBrain.TYPES.WOLF_TRAP), infoById));
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/TrappingMidas", Objectives.CustomQuestTypes.SetMidasTrap), infoById));
    }
    else if (this.CanGiveSozoMushroomoQuest(infoById))
    {
      Objectives_FindFollower quest = new Objectives_FindFollower("Objectives/GroupTitle/SozoStory", FollowerLocation.Dungeon1_2, "Mushroom", 0, 0, ScriptLocalization.NAMES_NPC.MushroomKeepers, 0);
      string str1 = "Conversation_NPC/SozoFollower/GiveQuestFirst/0";
      string str2 = "SozoFollower/CompletedFirst/0";
      if (DataManager.Instance.SozoMushroomCount == 1)
      {
        str1 = "Conversation_NPC/SozoFollower/GiveQuestSecond/0";
        str2 = "SozoFollower/CompletedSecond/0";
      }
      else if (DataManager.Instance.SozoMushroomCount >= 2)
      {
        int num = DataManager.Instance.SozoMushroomCount + 1;
        if (num > 7)
          num = UnityEngine.Random.Range(3, 8);
        str1 = $"Conversation_NPC/SozoFollower/GiveQuest{num}/0";
        str2 = "SozoFollower/Completed3/0";
      }
      quest.CompleteTerm = str2;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
      DataManager.Instance.CurrentOnboardingFollowerTerm = str1;
      DataManager.Instance.SozoAteMushroomDay = int.MaxValue;
    }
    else if (this.CanGivePerformBonfireRitualQuest(infoById))
    {
      Objectives_PerformRitual quest = new Objectives_PerformRitual("Objectives/GroupTitles/YngyaQuests", UpgradeSystem.Type.Ritual_FirePit_2, followerID);
      quest.FailLocked = true;
      DataManager.Instance.HasYngyaFirePitRitualQuestAccepted = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "FollowerInteractions/GiveQuest/Ritual_Bonfire/Yngya/0";
      quest.CompleteTerm = "GiveQuest/Ritual_Bonfire/Yngya/Complete";
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
    }
    else if (this.CanGiveFlowerBasketsQuest(infoById))
    {
      Objectives_FlowerBaskets quest = new Objectives_FlowerBaskets("Objectives/GroupTitles/YngyaQuests");
      quest.FailLocked = true;
      quest.CompleteTerm = "GiveQuest/WoolhavenFlowers/Yngya/Complete";
      DataManager.Instance.CurrentOnboardingFollowerTerm = "FollowerInteractions/GiveQuest/WoolhavenFlowers/Yngya/0";
      DataManager.Instance.HasYngyaFlowerBasketQuestAccepted = true;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
    }
    else if (this.CanRecieveLegendaryAxe(infoById))
    {
      onboardingQuests.Add((ObjectivesData) null);
      DataManager.Instance.CurrentOnboardingFollowerTerm = "Conversation_NPC/Executioner/Follower/LegendaryAxe/0";
      DataManager.Instance.ExecutionerGivenWeaponFragment = true;
    }
    else if (this.CanGiveFeedAnimalFollowerMeat())
    {
      List<StructuresData.Ranchable_Animal> ranchableAnimalList = new List<StructuresData.Ranchable_Animal>();
      foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
        ranchableAnimalList.AddRange((IEnumerable<StructuresData.Ranchable_Animal>) ranch.Brain.Data.Animals);
      for (int index = ranchableAnimalList.Count - 1; index >= 0; --index)
      {
        if (ranchableAnimalList[index].State == Interaction_Ranchable.State.Dead)
          ranchableAnimalList.RemoveAt(index);
      }
      Objectives_FeedAnimal quest = new Objectives_FeedAnimal("Objectives/GroupTitles/SadisticFeedAnimal", ranchableAnimalList[UnityEngine.Random.Range(0, ranchableAnimalList.Count)].ID, InventoryItem.ITEM_TYPE.FOLLOWER_MEAT);
      quest.CompleteTerm = "GiveQuest/Onboarding/FeedAnimal/0/FOLLOWER_MEAT/Complete/0";
      DataManager.Instance.CurrentOnboardingFollowerTerm = "FollowerInteractions/GiveQuest/Onboarding/FeedAnimal/0/FOLLOWER_MEAT";
      DataManager.Instance.HasAnimalFeedMeatQuest0Accepted = true;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
    }
    else if (this.CanGiveBringSnowmanToLife())
    {
      Objectives_Custom quest = new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.BuildGoodSnowman);
      quest.CompleteTerm = "GiveQuest/BuildGoodSnowman/Complete";
      DataManager.Instance.CurrentOnboardingFollowerTerm = "FollowerInteractions/GiveQuest/BuildGoodSnowman";
      DataManager.Instance.HasBuildGoodSnowmanQuestAccepted = true;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
    }
    else if (this.CanGivePureBloodMatingQuest(infoById))
    {
      Objectives_Custom quest = new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.SendFollowerToMatingTent, followerID);
      quest.FailLocked = true;
      DataManager.Instance.CurrentOnboardingFollowerTerm = "FollowerInteractions/GiveQuest/BreedPureBlood/0";
      DataManager.Instance.HasPureBloodMatingQuestAccepted = true;
      onboardingQuests.Add(this.OnboardingQuestAssigned((ObjectivesData) quest, infoById));
    }
    return onboardingQuests;
  }

  public ObjectivesData OnboardingQuestAssigned(ObjectivesData quest, FollowerInfo follower)
  {
    FollowerBrain.GetOrCreateBrain(follower);
    quest.AutoRemoveQuestOnceComplete = false;
    quest.Follower = follower.ID;
    return quest;
  }

  public bool CanGiveMealQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedMakingMoreFood && DataManager.Instance.MealsCooked < 5 && StructureManager.GetAllStructuresOfType<Structures_Meal>().Count <= 0 && (double) follower.Satiation < 30.0;
  }

  public bool CanGiveCleanUpQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedCleaningBase && Interaction_Poop.Poops.Count + Vomit.Vomits.Count > 2;
  }

  public bool CanGiveHousingQuest(FollowerInfo follower)
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

  public bool CanGiveFarmingQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedBuildFarm && TimeManager.CurrentDay > 3 && StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.FARM_PLOT).Count == 0 && StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SHRINE).Count != 0;
  }

  public bool CanGiveCultNameQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedCultName && TimeManager.CurrentDay > 2;
  }

  public bool CanGiveIllnessQuest(FollowerInfo follower)
  {
    if (DataManager.Instance.OnboardedSickFollower || 1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max >= 0.25 || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToBecomeIll <= 600.0 / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier() || Interaction_Poop.Poops.Count <= 0 || Vomit.Vomits.Count > 0)
      return false;
    foreach (Interaction_Poop poop in Interaction_Poop.Poops)
    {
      if (poop.StructureInfo.Type != StructureBrain.TYPES.TOXIC_WASTE)
        return true;
    }
    return false;
  }

  public bool CanGiveDissentQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedDissenter && (double) CultFaithManager.CurrentFaith / 85.0 < 0.25 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToStartDissenting > 600.0 / (double) DifficultyManager.GetTimeBetweenDissentingMultiplier() && !follower.Traits.Contains(FollowerTrait.TraitType.BishopOfCult) && follower.Necklace != InventoryItem.ITEM_TYPE.Necklace_Loyalty;
  }

  public bool CanGiveOldQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedOldFollower && follower.Age >= follower.LifeExpectancy && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToReachOldAge > 600.0 / (double) DifficultyManager.GetTimeBetweenOldAgeMultiplier();
  }

  public bool CanGiveResourceYardQuest(FollowerInfo follower)
  {
    return !DataManager.Instance.OnboardedResourceYard && TimeManager.CurrentDay > 12 && !DataManager.Instance.HistoryOfStructures.Contains(StructureBrain.TYPES.LUMBERJACK_STATION) && !DataManager.Instance.HistoryOfStructures.Contains(StructureBrain.TYPES.BLOODSTONE_MINE);
  }

  public bool CanGiveCrisisOfFaithQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedCrisisOfFaith && TimeManager.CurrentDay > 5 && (double) CultFaithManager.CurrentFaith <= 5.0 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.TimeSinceLastCrisisOfFaithQuest > 3600.0 && (double) DataManager.Instance.TimeSinceFaithHitEmpty != -1.0 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.TimeSinceFaithHitEmpty > 240.0;
  }

  public bool CanGiveSermonQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedSermon && TimeManager.CurrentDay > 5 && TimeManager.CurrentDay < 10 && (double) CultFaithManager.CurrentFaith <= 42.5 && TimeManager.CurrentDay - DataManager.Instance.PreviousSermonDayIndex > 1;
  }

  public bool CanGiveFaithOfFlockQuest()
  {
    return !DataManager.Instance.OnboardedFaithOfFlock && DataManager.Instance.LastDaySincePlayerUpgrade != -1 && (TimeManager.CurrentDay - DataManager.Instance.LastDaySincePlayerUpgrade > 5 || DataManager.Instance.playerDeathsInARow >= 2 && TimeManager.CurrentDay < 10);
  }

  public bool CanGiveRaiseFaithQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedRaiseFaith && (double) CultFaithManager.CurrentFaith <= 42.5 && DataManager.Instance.GivenLoyaltyQuestDay != -1 && TimeManager.CurrentDay - DataManager.Instance.GivenLoyaltyQuestDay >= 1 && TimeManager.CurrentDay - DataManager.Instance.GivenLoyaltyQuestDay < 5;
  }

  public bool CanGiveHalloweenQuest()
  {
    return DataManager.Instance.OnboardingFinished && !DataManager.Instance.OnboardedHalloween && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Halloween) && SeasonalEventManager.IsSeasonalEventActive(SeasonalEventType.Halloween) && !FollowerBrainStats.IsBloodMoon && (double) DataManager.Instance.LastHalloween == -3.4028234663852886E+38;
  }

  public bool CanGiveDeathCatAymAndBaalSecret(FollowerInfo info)
  {
    return DataManager.Instance.HasBaalSkin && DataManager.Instance.HasAymSkin && info.ID == 666 && !DataManager.Instance.DeathCatBaalAndAymSecret;
  }

  public bool CanGiveShamuraAymAndBaalSecret(FollowerInfo info)
  {
    return DataManager.Instance.HasBaalSkin && DataManager.Instance.HasAymSkin && info.ID == 99993 && !DataManager.Instance.ShamuraBaalAndAymSecret;
  }

  public bool CanGiveLeshyRelicQuest(FollowerInfo info)
  {
    return info.ID == 99990 && !ObjectiveManager.HasCustomObjective(Objectives.TYPES.FIND_RELIC) && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.DamageOnTouch_Familiar) && DataManager.Instance.OnboardedRelics;
  }

  public bool CanGiveLeshyHealingQuest(FollowerInfo info)
  {
    return info.ID == 99990 && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Leshy) && DataManager.Instance.PlayerFoundRelics.Contains(RelicType.DamageOnTouch_Familiar) && !DataManager.Instance.GaveLeshyHealingQuest && TimeManager.CurrentDay - DataManager.Instance.HealingQuestDay > 3 && TimeManager.CurrentDay - DataManager.Instance.LeshyHealingQuestDay > 6;
  }

  public bool CanGiveHeketRelicQuest(FollowerInfo info)
  {
    return info.ID == 99991 && !ObjectiveManager.HasCustomObjective(Objectives.TYPES.FIND_RELIC) && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.IncreaseDamageForDuration) && DataManager.Instance.OnboardedRelics;
  }

  public bool CanGiveHeketHealingQuest(FollowerInfo info)
  {
    return info.ID == 99991 && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Heket) && DataManager.Instance.PlayerFoundRelics.Contains(RelicType.IncreaseDamageForDuration) && !DataManager.Instance.GaveHeketHealingQuest && TimeManager.CurrentDay - DataManager.Instance.HealingQuestDay > 3 && TimeManager.CurrentDay - DataManager.Instance.HeketHealingQuestDay > 6;
  }

  public bool CanGiveKallamarRelicQuest(FollowerInfo info)
  {
    return info.ID == 99992 && !ObjectiveManager.HasCustomObjective(Objectives.TYPES.FIND_RELIC) && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.SpawnCombatFollower) && DataManager.Instance.OnboardedRelics;
  }

  public bool CanGiveKallamarHealingQuest(FollowerInfo info)
  {
    return info.ID == 99992 && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Kallamar) && DataManager.Instance.PlayerFoundRelics.Contains(RelicType.SpawnCombatFollower) && !DataManager.Instance.GaveKallamarHealingQuest && TimeManager.CurrentDay - DataManager.Instance.HealingQuestDay > 3 && TimeManager.CurrentDay - DataManager.Instance.KallamarHealingQuestDay > 6;
  }

  public bool CanGiveShamuraRelicQuest(FollowerInfo info)
  {
    return info.ID == 99993 && !ObjectiveManager.HasCustomObjective(Objectives.TYPES.FIND_RELIC) && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.GungeonBlank) && DataManager.Instance.OnboardedRelics;
  }

  public bool CanGiveShamuraHealingQuest(FollowerInfo info)
  {
    return info.ID == 99993 && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Shamura) && DataManager.Instance.PlayerFoundRelics.Contains(RelicType.GungeonBlank) && !DataManager.Instance.GaveShamuraHealingQuest && TimeManager.CurrentDay - DataManager.Instance.HealingQuestDay > 3 && TimeManager.CurrentDay - DataManager.Instance.ShamuraHealingQuestDay > 6;
  }

  public bool CanGiveDiscipleOnboarding(FollowerInfo info)
  {
    return info.XPLevel >= 10 && !info.IsDisciple && !DataManager.Instance.OnboardedDisciple && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit);
  }

  public bool CanGiveScaredTraitFightFollowerConvo(FollowerInfo info)
  {
    return info.Traits.Contains(FollowerTrait.TraitType.Scared) && info.FollowersFought == 1;
  }

  public bool CanGiveScaredTraitFightPitConvo(FollowerInfo info)
  {
    return info.Traits.Contains(FollowerTrait.TraitType.Scared) && info.FightPitsFought == 1;
  }

  public bool CanGiveSozoMushroomoQuest(FollowerInfo info)
  {
    if (info.ID != 99996 || DataManager.Instance.SozoNoLongerBrainwashed || ObjectiveManager.GroupExists("Objectives/GroupTitle/SozoStory") || TimeManager.CurrentDay <= DataManager.Instance.SozoAteMushroomDay && TimeManager.CurrentDay <= DataManager.Instance.SozoMushroomRecruitedDay)
      return false;
    for (int index = 0; index < DataManager.Instance.Followers_Recruit.Count; ++index)
    {
      if (DataManager.Instance.Followers_Recruit[index].SkinName == "Mushroom")
        return false;
    }
    return true;
  }

  public bool CanOnboardDrunk(FollowerInfo info)
  {
    return (double) info.Drunk >= 0.0 && TimeManager.CurrentDay - DataManager.Instance.DrunkDay > 0 && (double) info.Drunk > 0.0;
  }

  public bool CanBecomeAdult(FollowerInfo info)
  {
    return info.CursedState == Thought.Child && info.Age >= 18 && !UIDrumMinigameOverlayController.IsPlaying && info.ID != 100000;
  }

  public bool CanGivePilgrimPart1Quest(FollowerInfo info)
  {
    return DataManager.Instance.DLC_Pilgrim_Pack && TimeManager.CurrentDay > 6 && !DataManager.Instance.HasAcceptedPilgrimPart1 && TimeManager.CurrentDay >= DataManager.Instance.PilgrimPart1TargetDay && (DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_4) || DataManager.Instance.DeathCatBeaten);
  }

  public bool CanGivePilgrimPart2Quest(FollowerInfo info)
  {
    return DataManager.Instance.DLC_Pilgrim_Pack && info.ID == 99998 && DataManager.Instance.HasAcceptedPilgrimPart1 && !DataManager.Instance.HasAcceptedPilgrimPart2 && TimeManager.CurrentDay >= DataManager.Instance.PilgrimPart2TargetDay;
  }

  public bool CanGivePilgrimPart3Quest(FollowerInfo info)
  {
    return DataManager.Instance.DLC_Pilgrim_Pack && !PersistenceManager.PersistentData.UnlockedBonusComicPages && PersistenceManager.PersistentData.RevealedJalalasBag && info.ID == 99997 && DataManager.Instance.HasAcceptedPilgrimPart2 && !DataManager.Instance.HasAcceptedPilgrimPart3 && TimeManager.CurrentDay >= DataManager.Instance.PilgrimPart3TargetDay;
  }

  public bool CanGiveValentinQuest(FollowerInfo info)
  {
    if (info.CursedState == Thought.None)
    {
      DateTime now = DateTime.Now;
      if (now.Day == 14)
      {
        now = DateTime.Now;
        if (now.Month == 2)
        {
          now = DateTime.Now;
          if (now.Year > DataManager.Instance.ValentinsDayYear)
            return true;
        }
      }
    }
    return false;
  }

  public bool CanGiveDeathCatRelic(FollowerInfo info)
  {
    return info.ID == 666 && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.KillNonBossEnemies) && DataManager.Instance.LeshyHealQuestCompleted && DataManager.Instance.HeketHealQuestCompleted && DataManager.Instance.KallamarHealQuestCompleted && DataManager.Instance.ShamuraHealQuestCompleted;
  }

  public bool CanGiveRinorRandomLine(FollowerInfo info)
  {
    return DataManager.Instance.DLC_Pilgrim_Pack && info.ID == 99999 && !DataManager.Instance.GivenRinorLine;
  }

  public bool CanGiveYarlenRandomLine(FollowerInfo info)
  {
    return DataManager.Instance.DLC_Pilgrim_Pack && info.ID == 99998 && !DataManager.Instance.GivenYarlenLine && DataManager.Instance.HasAcceptedPilgrimPart3;
  }

  public bool CanOnboardWinterAlmostHere()
  {
    return !DataManager.Instance.FollowerOnboardedWinterAlmostHere && DataManager.Instance.YngyaOffering >= 1;
  }

  public bool CanOnboardRanchChoppingBlock()
  {
    return !DataManager.Instance.FollowerOnboardedRanchChoppingBlock && StructureManager.GetAllStructuresOfType<Structures_Ranch>().Count > 0 && Interaction_Ranchable.Ranchables.Count >= 3 && SeasonsManager.WinterSeverity >= 2;
  }

  public bool CanOnboardFreezing(FollowerInfo info)
  {
    return !DataManager.Instance.FollowerOnboardedFreezing && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && FollowerBrain.GetOrCreateBrain(info).CanFreeze() && (double) WarmthBar.WarmthNormalized < 0.25 && DataManager.Instance.BuiltFurnace && (DataManager.Instance.WintersOccured > 1 || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.5) && SeasonsManager.WinterSeverity >= 1;
  }

  public bool CanGiveMidasQuestHelp()
  {
    if (!DataManager.Instance.HasMidasHiding || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.RanchingSystem) || DataManager.Instance.GivenMidasFollowerQuest || DataManager.Instance.MidasHiddenDay == -1 || TimeManager.CurrentDay - DataManager.Instance.MidasHiddenDay < 3)
      return false;
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.WOLF_TRAP))
    {
      if (structureBrain.Data.Inventory.Count > 0 && structureBrain.Data.Inventory[0].type == 86)
        return false;
    }
    return true;
  }

  public bool CanGiveChosenChildQuest(FollowerInfo info)
  {
    return info.ID == 100000 && info.MemberDuration >= 5 && !DataManager.Instance.GaveChosenChildQuest;
  }

  public bool CanGivePerformBonfireRitualQuest(FollowerInfo info)
  {
    return info.ID == 100007 && !DataManager.Instance.HasYngyaFirePitRitualQuestAccepted && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2);
  }

  public bool CanGiveFlowerBasketsQuest(FollowerInfo info)
  {
    if (info.ID != 100007 || !DataManager.Instance.HasYngyaFirePitRitualQuestAccepted || DataManager.Instance.HasYngyaFlowerBasketQuestAccepted)
      return false;
    List<ObjectivesData> objectivesOfGroupId = ObjectiveManager.GetAllObjectivesOfGroupID("Objectives/GroupTitles/YngyaQuests");
    for (int index = 0; index < objectivesOfGroupId.Count; ++index)
    {
      if (objectivesOfGroupId[index].Type == Objectives.TYPES.PERFORM_RITUAL && !objectivesOfGroupId[index].IsComplete)
        return false;
    }
    return true;
  }

  public bool CanRecieveLegendaryAxe(FollowerInfo info)
  {
    return info.ID == 10010 && DataManager.Instance.ExecutionerWoolhavenSaved && !DataManager.Instance.ExecutionerGivenWeaponFragment && !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Axe_Legendary);
  }

  public bool CanGiveFeedAnimalFollowerMeat()
  {
    if (!DataManager.Instance.HasAnimalFeedMeatQuest0Accepted && Interaction_Ranch.Ranches.Count > 0 && Interaction_Ranchable.Ranchables.Count >= 3 && ObjectiveManager.GetAllObjectivesOfGroupID("Objectives/GroupTitles/SadisticFeedAnimal").Count <= 0)
    {
      List<StructuresData.Ranchable_Animal> ranchableAnimalList = new List<StructuresData.Ranchable_Animal>();
      foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
      {
        if (ranch.GetAnimalCount() > 0)
          return true;
      }
    }
    return false;
  }

  public bool CanGiveBringSnowmanToLife()
  {
    if (!DataManager.Instance.HasBuildGoodSnowmanQuestAccepted && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      if (StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.ICE_BLOCK).Count > 0 && TimeManager.CurrentDay < SeasonsManager.SeasonTimestamp && (double) SeasonsManager.SEASON_NORMALISED_PROGRESS > 0.5)
        return true;
    }
    return false;
  }

  public bool CanGivePureBloodMatingQuest(FollowerInfo info)
  {
    return !DataManager.Instance.HasPureBloodMatingQuestAccepted && info.Traits.Contains(FollowerTrait.TraitType.PureBlood_1) && TimeManager.CurrentDay >= info.DayJoined;
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__33_0()
  {
    if (DataManager.Instance.CompletedObjectivesHistory.Count > 0 && !ObjectiveManager.GroupExists(DataManager.Instance.CompletedObjectivesHistory[0].GroupId) && !DataManager.Instance.OnboardingFinished)
      this.OnObjectiveComplete(DataManager.Instance.CompletedObjectivesHistory[0].GroupId);
    if (DataManager.Instance.RevealedBaseYngyaShrine || !DataManager.Instance.OnboardingFinished)
      return;
    if (DataManager.Instance.YngyaOffering == 0)
    {
      if (!DataManager.Instance.MAJOR_DLC && DataManager.Instance.BossesCompleted.Count < 4)
        return;
      this.StartCoroutine((IEnumerator) Interaction_DLCShrine.Instance.RevealIE());
    }
    else
      DataManager.Instance.RevealedBaseYngyaShrine = true;
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__33_2()
  {
    if (DataManager.Instance.YngyaOffering != -1 || DataManager.Instance.OnboardedDLCEntrance)
      return;
    this.OnboardDLCEntrance();
  }

  [CompilerGenerated]
  public static void \u003COnObjectiveComplete\u003Eg__CompletedSermon\u007C43_2(
    bool SetPlayerToIdle1,
    bool ShowHUD1)
  {
    DataManager.Instance.GivenSermonQuest = true;
    MMConversation.OnConversationEnd -= (MMConversation.ConversationEnd) ((SetPlayerToIdle2, ShowHUD2) =>
    {
      // ISSUE: unable to decompile the method.
    });
  }
}
