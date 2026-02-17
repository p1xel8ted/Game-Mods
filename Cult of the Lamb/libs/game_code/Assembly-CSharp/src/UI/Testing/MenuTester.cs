// Decompiled with JetBrains decompiler
// Type: src.UI.Testing.MenuTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.AltarMenu;
using Lamb.UI.BuildMenu;
using Lamb.UI.DeathScreen;
using Lamb.UI.FollowerInteractionWheel;
using Lamb.UI.FollowerSelect;
using Lamb.UI.KitchenMenu;
using Lamb.UI.MainMenu;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using Lamb.UI.Menus.PlayerMenu;
using Lamb.UI.Mission;
using Lamb.UI.PauseMenu;
using Lamb.UI.PrisonerMenu;
using Lamb.UI.RanchSelect;
using Lamb.UI.RefineryMenu;
using Lamb.UI.Rituals;
using Lamb.UI.SermonWheelOverlay;
using Lamb.UI.SettingsMenu;
using Lamb.UI.UpgradeMenu;
using Lamb.UI.VideoMenu;
using Map;
using MMBiomeGeneration;
using MMTools;
using src.Extensions;
using src.UI.Menus;
using src.UI.Menus.Achievements_Menu;
using src.UI.Menus.CryptMenu;
using src.UI.Overlays.TutorialOverlay;
using src.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UI.Menus.TwitchMenu;
using UnityEngine;

#nullable disable
namespace src.UI.Testing;

public class MenuTester : MonoBehaviour
{
  public UIMenuBase _testInstance;
  [SerializeField]
  public UIMenuBase _defaultMenuTemplate;
  [SerializeField]
  public MenuTester.PlayerConfiguration _playerConfiguration;
  [SerializeField]
  public MockedPlayer _lamb;
  [SerializeField]
  public MockedPlayer _goatSolo;
  [SerializeField]
  public MockedPlayer _goatCoop;
  public List<System.Action> _allMenuTests;
  public int _currentIndex;
  public string _screenshotPath;
  [SerializeField]
  public UIMainMenuController _mainMenuTemplate;
  [SerializeField]
  public UISettingsMenuController _settingsMenuTemplate;
  [SerializeField]
  public UIPauseMenuController _pauseMenuTemplate;
  [SerializeField]
  public UICreditsMenuController _creditsMenuTemplate;
  [SerializeField]
  public UIBuildMenuController _buildMenuTemplate;
  [SerializeField]
  public UIFollowerSelectMenuController _followerSelectMenuTemplate;
  [SerializeField]
  public UIDeadFollowerSelectMenu _deadFollowerSelectMenuTemplate;
  [SerializeField]
  public UICryptMenuController _cryptMenuControllerTemplate;
  [SerializeField]
  public UIFollowerIndoctrinationMenuController _followerIndoctrinationMenuTemplate;
  [SerializeField]
  public UICultNameMenuController _uiCultNameMenuTemplate;
  [SerializeField]
  public UIPauseDetailsMenuController _pauseDetailsMenuTemplate;
  [SerializeField]
  public UIFollowerFormsMenuController _followerFormsMenuTemplate;
  [SerializeField]
  public UITarotCardsMenuController _tarotCardsMenuTemplate;
  public TarotCards.Card tarotTestCard = TarotCards.Card.Count;
  [SerializeField]
  public UIRelicMenuController _relicMenuControllerTemplate;
  [SerializeField]
  public UIUpgradeShopMenuController _upgradeShopMenuTemplate;
  [SerializeField]
  public UIWorldMapMenuController _worldMapMenuController;
  [SerializeField]
  public UIDLCMapMenuController _dlcMapMenuController;
  [SerializeField]
  public UITutorialMenuController _tutorialMenuTemplate;
  [SerializeField]
  public UIUpgradeTreeMenuController _upgradeTreeTemplate;
  [SerializeField]
  public UIUpgradePlayerTreeMenuController _playerUpgradeTreeTemplate;
  [SerializeField]
  public UIRefineryMenuController _refineryMenuControllerTemplate;
  [SerializeField]
  public UICookingFireMenuController _cookingFireMenuTemplate;
  [SerializeField]
  public UIKitchenMenuController _kitchenMenuTemplate;
  [SerializeField]
  public UIFollowerSummaryMenuController _followerSummaryMenuTemplate;
  [SerializeField]
  public UIVideoExportMenuController _uiVideoExportMenuController;
  [SerializeField]
  public UISandboxMenuController _uiSandboxMenuControllerTemplate;
  [SerializeField]
  public UIAchievementsMenuController _achievementsMenuController;
  [SerializeField]
  public UIPrisonMenuController _prisonMenuTemplate;
  [SerializeField]
  public UIPrisonerMenuController _prisonerMenuTemplate;
  [SerializeField]
  public UIMissionaryMenuController _missionaryMenuTemplate;
  [SerializeField]
  public UIMissionMenuController _missionMenuTemplate;
  [SerializeField]
  public UIDemonSummonMenuController _demonSummonMenuTemplate;
  [SerializeField]
  public UIDemonMenuController _demonMenuTemplate;
  [SerializeField]
  public UIMenuConfirmationWindow _confirmationWindowTemplate;
  [SerializeField]
  public UIConfirmationCountdownWindow _confirmationCountdownWindowTemplate;
  [SerializeField]
  public UISaveErrorMenuController _saveErrorWindowTemplate;
  [SerializeField]
  public UIDoctrineMenuController _doctrineMenuTemplate;
  [SerializeField]
  public UIAltarMenuController _altarMenuTemplate;
  [SerializeField]
  public UIDoctrineChoicesMenuController _doctrineChoicesMenuTemplate;
  [SerializeField]
  public UIRitualsMenuController _ritualMenuTemplate;
  [SerializeField]
  public UIHeartsOfTheFaithfulChoiceMenuController _heartsOfTheFaithfulChoiceMenuController;
  [SerializeField]
  public UIPlayerUpgradesMenuController _playerUpgradesMenuControllerTemplate;
  [SerializeField]
  public UISermonWheelController _sermonWheelTemplate;
  [SerializeField]
  public UIFollowerInteractionWheelOverlayController _followerInteractionWheelTemplate;
  [SerializeField]
  public UICurseWheelController _curseWheelOverlayTemplate;
  [SerializeField]
  public UIDeathScreenOverlayController _deathScreenOverlayTemplate;
  [SerializeField]
  public UIKeyScreenOverlayController _keyScreenTemplate;
  [SerializeField]
  public UIWeaponWheelController _weaponWheelOverlayTemplate;
  [SerializeField]
  public UIUpgradeUnlockOverlayControllerBase _ugpradeUnlockOverlaytemplate;
  [SerializeField]
  public UITutorialOverlayController _tutorialOverlayTemplate;
  [SerializeField]
  public UIRecipeConfirmationOverlayController _recipeConfirmationOverlayTemplate;
  [SerializeField]
  public UIItemSelectorOverlayController _itemSelectorOverlayTemplate;
  [SerializeField]
  public UIDifficultySelectorOverlayController _difficultyOverlayTemplate;
  [SerializeField]
  public UITarotChoiceOverlayController _tarotChoiceOverlayTemplate;
  [SerializeField]
  public UIFleeceTarotRewardOverlayController fleeceTarotRewardOverlayController;
  [SerializeField]
  public UIControlBindingOverlayController _controlBindingOverlayController;
  [SerializeField]
  public UIAdventureMapOverlayController _adventureMapTemplate;
  [SerializeField]
  public UIRoadmapOverlayController _roadmapOverlayController;
  [SerializeField]
  public UIBugReportingOverlayController _bugReportingOverlayController;
  [SerializeField]
  public UIMysticShopOverlayController _mysticShopOverlayController;
  [SerializeField]
  public UIPhotoGalleryMenuController _photoGalleryMenuController;
  [SerializeField]
  public UIEditPhotoOverlayController _editPhotoOverlayController;
  [SerializeField]
  public UIKnuckleBonesController _knuckleBonesTemplate;
  [SerializeField]
  public KnucklebonesPlayerConfiguration[] _opponentConfigurations;
  [SerializeField]
  public UIKnucklebonesBettingSelectionController _kbBettingTemplate;
  [SerializeField]
  public UIKnucklebonesOpponentSelectionController _kbOpponentTemplate;
  [SerializeField]
  public FlockadeUIController _flockadeTemplate;
  [SerializeField]
  public FlockadeGamePiecesPoolConfiguration[] _poolConfigurations;
  [SerializeField]
  public FlockadeOpponentConfiguration[] _availableOpponents;
  [SerializeField]
  public int _bet;
  [SerializeField]
  public FlockadeBettingSelectionUIController _flockadeBettingTemplate;
  [SerializeField]
  public FlockadeOpponentSelectionUIController _flockadeOpponentSelectionTemplate;
  [SerializeField]
  public UIFlockadePiecesMenuController _flockadePiecesMenuTemplate;
  [SerializeField]
  public UITwitchTotemWheel _twitchTotemWheelTemplate;
  [SerializeField]
  public UITwitchSettingsMenuController _uiTwitchMenuSettingsController;
  [SerializeField]
  public UICoopAssignController _assignCoOpController;
  [SerializeField]
  public UIRanchSelectMenuController _ranchSelectMenuController;
  [SerializeField]
  public UIBuildSnowmanMinigameOverlayController _snowmanMiniGameController;

  public void StartingStartUI()
  {
  }

  public void Start()
  {
    this._screenshotPath = Path.Combine(Application.persistentDataPath, "MenuScreenshots");
    if (!Directory.Exists(this._screenshotPath))
      Directory.CreateDirectory(this._screenshotPath);
    this.StartCoroutine((IEnumerator) this.WaitASecond());
  }

  public IEnumerator WaitASecond()
  {
    yield return (object) new WaitForSeconds(1f);
    this.StartingStartUI();
  }

  public void Awake()
  {
    Singleton<SettingsManager>.Instance.LoadAndApply();
    SaveAndLoad.Load(5);
    DataManager.Instance.MAJOR_DLC = true;
    this.UpdateActivePlayerSetup();
  }

  public void UpdateActivePlayerSetup()
  {
    if (!Application.isPlaying)
      return;
    GameObject gameObject = this._lamb.gameObject;
    MenuTester.PlayerConfiguration playerConfiguration = this._playerConfiguration;
    int num = (playerConfiguration == MenuTester.PlayerConfiguration.SoloLamb ? 0 : (playerConfiguration != MenuTester.PlayerConfiguration.Coop ? 1 : 0)) == 0 ? 1 : 0;
    gameObject.SetActive(num != 0);
    this._goatSolo.gameObject.SetActive(this._playerConfiguration == MenuTester.PlayerConfiguration.SoloGoat);
    this._goatCoop.gameObject.SetActive(this._playerConfiguration == MenuTester.PlayerConfiguration.Coop);
    if (this._playerConfiguration != MenuTester.PlayerConfiguration.Coop)
      return;
    this.TestCoOpInput();
  }

  public void TestMainMenu() => this.ShowTestMenu((UIMenuBase) this._mainMenuTemplate);

  public void TestSettingsMenu() => this.ShowTestMenu((UIMenuBase) this._settingsMenuTemplate);

  public void TestPauseMenu() => this.ShowTestMenu((UIMenuBase) this._pauseMenuTemplate);

  public void TestCreditsMenu() => this.ShowTestMenu((UIMenuBase) this._creditsMenuTemplate);

  public void TestBuildMenu() => this.ShowTestMenu((UIMenuBase) this._buildMenuTemplate);

  public void TestBuildMenuReveal()
  {
    StructureBrain.TYPES structureToReveal = AestheticCategory.AllStructures().RandomElement<StructureBrain.TYPES>();
    DataManager.Instance.UnlockedStructures.Add(structureToReveal);
    UIBuildMenuController menu = this._buildMenuTemplate.Instantiate<UIBuildMenuController>();
    menu.Show(structureToReveal);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestFollowerSelectmenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    }
    UIFollowerSelectMenuController selectMenuController1 = this._followerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    selectMenuController1.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIFollowerSelectMenuController selectMenuController2 = selectMenuController1;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectMenuController1;
  }

  public void TestDeadFollowerSelect()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    }
    UIDeadFollowerSelectMenu followerSelectMenu = this._deadFollowerSelectMenuTemplate.Instantiate<UIDeadFollowerSelectMenu>();
    followerSelectMenu.Show(followerSelectEntries, hideOnSelection: false);
    this._testInstance = (UIMenuBase) followerSelectMenu;
  }

  public void TestCryptMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    }
    UICryptMenuController cryptMenuController = this._cryptMenuControllerTemplate.Instantiate<UICryptMenuController>();
    cryptMenuController.Show(followerSelectEntries, hideOnSelection: false);
    this._testInstance = (UIMenuBase) cryptMenuController;
  }

  public void TestFollowerIndoctrinationMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.None);
    info.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
    Follower follower = FollowerManager.FollowerPrefab.Instantiate<Follower>();
    follower.name = "Follower " + brain.Info.Name;
    follower.Init(brain, brain.CreateOutfit());
    follower.transform.position = Vector3.zero;
    UIFollowerIndoctrinationMenuController indoctrinationMenuController1 = this._followerIndoctrinationMenuTemplate.Instantiate<UIFollowerIndoctrinationMenuController>();
    indoctrinationMenuController1.Show(follower, new OriginalFollowerLookData(follower.Brain._directInfoAccess));
    UIFollowerIndoctrinationMenuController indoctrinationMenuController2 = indoctrinationMenuController1;
    indoctrinationMenuController2.OnHidden = indoctrinationMenuController2.OnHidden + (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject);
    });
    this._testInstance = (UIMenuBase) indoctrinationMenuController1;
  }

  public void TestCultNameMenu() => this.ShowTestMenu((UIMenuBase) this._uiCultNameMenuTemplate);

  public void TestPauseDetailsMenu()
  {
    this.ShowTestMenu((UIMenuBase) this._pauseDetailsMenuTemplate);
  }

  public void TestFollowerFormsMenu()
  {
    this.ShowTestMenu((UIMenuBase) this._followerFormsMenuTemplate);
  }

  public void TestTarotCardsMenu() => this.ShowTestMenu((UIMenuBase) this._tarotCardsMenuTemplate);

  public void TestTarotCardsMenuNewUnlock()
  {
    if (!((UnityEngine.Object) this._tarotCardsMenuTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    Time.timeScale = 0.0f;
    TarotCards.Card card = this.tarotTestCard != TarotCards.Card.Count ? this.tarotTestCard : DataManager.AllTrinkets.RandomElement<TarotCards.Card>();
    Debug.Log((object) $"Unlocking Tarot Card: {card}");
    TarotCards.UnlockTrinket(card);
    UITarotCardsMenuController cardsMenuController = this._tarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show(card);
    this._testInstance = (UIMenuBase) cardsMenuController;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void TestTarotCardsMenuNewUnlock2()
  {
    if (!((UnityEngine.Object) this._tarotCardsMenuTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    List<TarotCards.Card> cardList = new List<TarotCards.Card>();
    for (int index = 0; index < 10; ++index)
    {
      TarotCards.Card card = DataManager.AllTrinkets.RandomElement<TarotCards.Card>();
      while (cardList.Contains(card))
        card = DataManager.AllTrinkets.RandomElement<TarotCards.Card>();
      cardList.Add(card);
    }
    Time.timeScale = 0.0f;
    UITarotCardsMenuController cardsMenuController = this._tarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show(cardList.ToArray());
    this._testInstance = (UIMenuBase) cardsMenuController;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void TestRelicsMenu() => this.ShowTestMenu((UIMenuBase) this._relicMenuControllerTemplate);

  public void TestRelicMenuNewUnlock()
  {
    if (!((UnityEngine.Object) this._relicMenuControllerTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    Time.timeScale = 0.0f;
    RelicType relicType = Enum.GetValues(typeof (RelicType)).Cast<RelicType>().ToArray<RelicType>().RandomElement<RelicType>();
    DataManager.Instance.PlayerFoundRelics.Add(relicType);
    UIRelicMenuController relicMenuController = this._relicMenuControllerTemplate.Instantiate<UIRelicMenuController>();
    relicMenuController.Show(relicType, (PlayerFarming) null);
    this._testInstance = (UIMenuBase) relicMenuController;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void TestRelicMenuNewUnlocks()
  {
    if (!((UnityEngine.Object) this._relicMenuControllerTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    List<RelicData> relicDatas = new List<RelicData>();
    relicDatas.Add(EquipmentManager.RelicData.RandomElement<RelicData>());
    relicDatas.Add(EquipmentManager.RelicData.RandomElement<RelicData>());
    relicDatas.Add(EquipmentManager.RelicData.RandomElement<RelicData>());
    relicDatas.Add(EquipmentManager.RelicData.RandomElement<RelicData>());
    DataManager.Instance.PlayerFoundRelics.Add(relicDatas[0].RelicType);
    DataManager.Instance.PlayerFoundRelics.Add(relicDatas[1].RelicType);
    DataManager.Instance.PlayerFoundRelics.Add(relicDatas[2].RelicType);
    DataManager.Instance.PlayerFoundRelics.Add(relicDatas[3].RelicType);
    UIRelicMenuController relicMenuController = this._relicMenuControllerTemplate.Instantiate<UIRelicMenuController>();
    relicMenuController.Show(relicDatas);
    this._testInstance = (UIMenuBase) relicMenuController;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void TestUpgradeShopMenu()
  {
    this.ShowTestMenu((UIMenuBase) this._upgradeShopMenuTemplate);
  }

  public void TestWorldMapMenu()
  {
    DataManager.Instance.DiscoverLocation(FollowerLocation.Base);
    this.ShowTestMenu((UIMenuBase) this._worldMapMenuController);
  }

  public void TestWorldMapMenuReveal()
  {
    if (!((UnityEngine.Object) this._worldMapMenuController != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    FollowerLocation followerLocation = UIWorldMapMenuController.UnlockableMapLocations.RandomElement<FollowerLocation>();
    DataManager.Instance.DiscoverLocation(FollowerLocation.Base);
    DataManager.Instance.DiscoverLocation(followerLocation);
    Time.timeScale = 0.0f;
    UIWorldMapMenuController mapMenuController = this._worldMapMenuController.Instantiate<UIWorldMapMenuController>();
    mapMenuController.Show(followerLocation);
    this._testInstance = (UIMenuBase) mapMenuController;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void TestWorldMapMenuReReveal()
  {
    if (!((UnityEngine.Object) this._worldMapMenuController != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    FollowerLocation followerLocation = UIWorldMapMenuController.UnlockableMapLocations.RandomElement<FollowerLocation>();
    DataManager.Instance.DiscoverLocation(FollowerLocation.Base);
    DataManager.Instance.DiscoverLocation(followerLocation);
    Time.timeScale = 0.0f;
    UIWorldMapMenuController mapMenuController = this._worldMapMenuController.Instantiate<UIWorldMapMenuController>();
    mapMenuController.Show(followerLocation, true);
    this._testInstance = (UIMenuBase) mapMenuController;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void TestDLCMapMenu()
  {
    DataManager.Instance.CurrentDLCNodeType = DungeonWorldMapIcon.NodeType.Shrine;
    DataManager.Instance.IsLambGhostRescue = false;
    GameManager.NextDungeonLayer(0);
    GameManager.NewRun("", false, FollowerLocation.Dungeon1_5);
    DataManager.Instance.CurrentLocation = FollowerLocation.Base;
  }

  public void TestDLCMapMenuMain()
  {
    UIDLCMapMenuController menu = MonoSingleton<UIManager>.Instance.DLCWorldMapTemplate.Instantiate<UIDLCMapMenuController>();
    menu.transform.SetAsFirstSibling();
    menu.Show();
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestTutorialMenu() => this.ShowTestMenu((UIMenuBase) this._tutorialMenuTemplate);

  public void TestUpgradeTree() => this.ShowTestMenu((UIMenuBase) this._upgradeTreeTemplate);

  public void TestPlayerUpgradeTree()
  {
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Major_DLC_Sermon_Packs);
    this.ShowTestMenu((UIMenuBase) this._playerUpgradeTreeTemplate);
  }

  public void TestRefineryMenu()
  {
    UIRefineryMenuController menu = this._refineryMenuControllerTemplate.Instantiate<UIRefineryMenuController>();
    menu.Show(new StructuresData(), (Interaction_Refinery) null);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestCookingFireMenu()
  {
    StructuresData kitchenData = new StructuresData();
    UICookingFireMenuController menu = this._cookingFireMenuTemplate.Instantiate<UICookingFireMenuController>();
    menu.Show(kitchenData);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestKitchenMenu()
  {
    StructuresData kitchenData = new StructuresData();
    UIKitchenMenuController menu = this._kitchenMenuTemplate.Instantiate<UIKitchenMenuController>();
    menu.Show(kitchenData);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestFollowerSummaryMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.None);
    info.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
    Follower follower = FollowerManager.FollowerPrefab.Instantiate<Follower>();
    follower.name = "Follower " + brain.Info.Name;
    follower.Init(brain, brain.CreateOutfit());
    follower.transform.position = Vector3.zero;
    UIFollowerSummaryMenuController menu = this._followerSummaryMenuTemplate.Instantiate<UIFollowerSummaryMenuController>();
    menu.Show(follower);
    UIFollowerSummaryMenuController summaryMenuController = menu;
    summaryMenuController.OnHidden = summaryMenuController.OnHidden + (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject));
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestVideoExportMenu()
  {
    UIVideoExportMenuController menu = this._uiVideoExportMenuController.Instantiate<UIVideoExportMenuController>();
    menu.Show();
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestSandboxMenu()
  {
    DataManager.Instance.OnboardedBossRush = true;
    this.ShowTestMenu((UIMenuBase) this._uiSandboxMenuControllerTemplate);
  }

  public void TestUnlockBossRushSandbox()
  {
    DataManager.Instance.OnboardedBossRush = false;
    DataManager.Instance.SandboxProgression.Clear();
    DungeonSandboxManager.ProgressionSnapshot progressionForScenario = DungeonSandboxManager.GetProgressionForScenario(DungeonSandboxManager.ScenarioType.DungeonMode, PlayerFleeceManager.FleeceType.Default);
    progressionForScenario.CompletedRuns = 1;
    progressionForScenario.CompletionSeen = 0;
    this.ShowTestMenu((UIMenuBase) this._uiSandboxMenuControllerTemplate);
  }

  public void TestSandboxMultipleItems()
  {
    DataManager.Instance.OnboardedBossRush = true;
    DataManager.Instance.SandboxProgression.Clear();
    DungeonSandboxManager.ProgressionSnapshot progressionForScenario = DungeonSandboxManager.GetProgressionForScenario(DungeonSandboxManager.ScenarioType.DungeonMode, PlayerFleeceManager.FleeceType.Default);
    progressionForScenario.CompletedRuns = 6;
    progressionForScenario.CompletionSeen = 0;
    this.ShowTestMenu((UIMenuBase) this._uiSandboxMenuControllerTemplate);
  }

  public void TestAchievementsMenu()
  {
    this.ShowTestMenu((UIMenuBase) this._achievementsMenuController);
  }

  public void TestPrisonMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    }
    UIPrisonMenuController prisonMenuController1 = this._prisonMenuTemplate.Instantiate<UIPrisonMenuController>();
    prisonMenuController1.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIPrisonMenuController prisonMenuController2 = prisonMenuController1;
    prisonMenuController2.OnHidden = prisonMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) prisonMenuController1;
  }

  public void TestPrisonerMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
    followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
    UIPrisonerMenuController prisonerMenuController1 = this._prisonerMenuTemplate.Instantiate<UIPrisonerMenuController>();
    prisonerMenuController1.Show(followerInfo, (StructuresData) null);
    UIPrisonerMenuController prisonerMenuController2 = prisonerMenuController1;
    prisonerMenuController2.OnHidden = prisonerMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) prisonerMenuController1;
  }

  public void TestMissionaryMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    }
    UIMissionaryMenuController missionaryMenuController1 = this._missionaryMenuTemplate.Instantiate<UIMissionaryMenuController>();
    missionaryMenuController1.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIMissionaryMenuController missionaryMenuController2 = missionaryMenuController1;
    missionaryMenuController2.OnHidden = missionaryMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) missionaryMenuController1;
  }

  public void TestMissionMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
    followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
    followerInfo.MissionaryTimestamp = 3f;
    followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    UIMissionMenuController missionMenuController1 = this._missionMenuTemplate.Instantiate<UIMissionMenuController>();
    missionMenuController1.Show(followerSelectEntries);
    UIMissionMenuController missionMenuController2 = missionMenuController1;
    missionMenuController2.OnHidden = missionMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) missionMenuController1;
  }

  public void TestDemonSummonMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    }
    UIDemonSummonMenuController summonMenuController1 = this._demonSummonMenuTemplate.Instantiate<UIDemonSummonMenuController>();
    summonMenuController1.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIDemonSummonMenuController summonMenuController2 = summonMenuController1;
    summonMenuController2.OnHidden = summonMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) summonMenuController1;
  }

  public void TestDemonMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
    followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
    UIDemonMenuController demonMenuController1 = this._demonMenuTemplate.Instantiate<UIDemonMenuController>();
    demonMenuController1.Show(new List<FollowerSelectEntry>()
    {
      new FollowerSelectEntry(followerInfo)
    });
    UIDemonMenuController demonMenuController2 = demonMenuController1;
    demonMenuController2.OnHidden = demonMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) demonMenuController1;
  }

  public void TestConfirmationMenu()
  {
    this.ShowTestMenu((UIMenuBase) this._confirmationWindowTemplate);
  }

  public void TestConfirmationCountdownMenu()
  {
    UIConfirmationCountdownWindow confirmationCountdownWindowInstance = this._confirmationCountdownWindowTemplate.Instantiate<UIConfirmationCountdownWindow>();
    confirmationCountdownWindowInstance.Show();
    confirmationCountdownWindowInstance.Configure("Are you sure", "Are you sure you'd like to keep these settings? Reverting settings in {0}", 15);
    UIConfirmationCountdownWindow confirmationCountdownWindow1 = confirmationCountdownWindowInstance;
    confirmationCountdownWindow1.OnConfirm = confirmationCountdownWindow1.OnConfirm + (System.Action) (() => Debug.Log((object) "Settings confirmed!".Colour(Color.yellow)));
    UIConfirmationCountdownWindow confirmationCountdownWindow2 = confirmationCountdownWindowInstance;
    confirmationCountdownWindow2.OnCancel = confirmationCountdownWindow2.OnCancel + (System.Action) (() => Debug.Log((object) "Settings reverted!".Colour(Color.yellow)));
    UIConfirmationCountdownWindow confirmationCountdownWindow3 = confirmationCountdownWindowInstance;
    confirmationCountdownWindow3.OnHidden = confirmationCountdownWindow3.OnHidden + (System.Action) (() => confirmationCountdownWindowInstance = (UIConfirmationCountdownWindow) null);
  }

  public void TestSaveErrorWindow()
  {
    this.ShowTestMenu((UIMenuBase) this._saveErrorWindowTemplate);
  }

  public void TestDoctrineMenu() => this.ShowTestMenu((UIMenuBase) this._doctrineMenuTemplate);

  public void TestAltarMenu() => this.ShowTestMenu((UIMenuBase) this._altarMenuTemplate);

  public void TestDoctrineChoicesMenu()
  {
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null);
    MMConversation.CURRENT_CONVERSATION.DoctrineResponses = new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Afterlife, 1, true, (System.Action) null),
      new DoctrineResponse(SermonCategory.Afterlife, 1, false, (System.Action) null)
    };
    this.ShowTestMenu((UIMenuBase) this._doctrineChoicesMenuTemplate);
  }

  public void TestDoctrineChoiceSingle()
  {
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null);
    MMConversation.CURRENT_CONVERSATION.DoctrineResponses = new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Afterlife, 1, true, (System.Action) null)
    };
    this.ShowTestMenu((UIMenuBase) this._doctrineChoicesMenuTemplate);
  }

  public void TestRitualsMenu() => this.ShowTestMenu((UIMenuBase) this._ritualMenuTemplate);

  public void TestRitualsMenuUnlockSequence()
  {
    UpgradeSystem.Type type = UpgradeSystem.SecondaryRituals.RandomElement<UpgradeSystem.Type>();
    UpgradeSystem.UnlockAbility(type);
    UIRitualsMenuController menu = this._ritualMenuTemplate.Instantiate<UIRitualsMenuController>();
    menu.Show(type);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestRitualChoiceMenu()
  {
    this.ShowTestMenu((UIMenuBase) this._heartsOfTheFaithfulChoiceMenuController);
  }

  public void TestPlayerUpgrades()
  {
    this.ShowTestMenu((UIMenuBase) this._playerUpgradesMenuControllerTemplate);
  }

  public void TestPlayerUpgradesCrystal()
  {
    UIPlayerUpgradesMenuController menu = this._playerUpgradesMenuControllerTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    menu.ShowCrystalUnlock();
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestPlayerUpgradesFleeces()
  {
    DataManager.Instance.DeathCatBeaten = true;
    UIPlayerUpgradesMenuController menu = this._playerUpgradesMenuControllerTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[4]
    {
      PlayerFleeceManager.FleeceType.CurseInsteadOfWeapon,
      PlayerFleeceManager.FleeceType.OneHitKills,
      PlayerFleeceManager.FleeceType.HollowHeal,
      PlayerFleeceManager.FleeceType.NoRolling
    });
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestPurgatoryCompletedFleece()
  {
    DataManager.Instance.UnlockedFleeces.Add(1000);
    DataManager.Instance.DeathCatBeaten = true;
    UIPlayerUpgradesMenuController menu = this._playerUpgradesMenuControllerTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
    {
      PlayerFleeceManager.FleeceType.GodOfDeath
    });
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestSermonWheelOverlay()
  {
    UISermonWheelController menu = this._sermonWheelTemplate.Instantiate<UISermonWheelController>();
    menu.Show(InventoryItem.ITEM_TYPE.DOCTRINE_STONE);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestSermonWheelOverlayCrystal()
  {
    UISermonWheelController menu = this._sermonWheelTemplate.Instantiate<UISermonWheelController>();
    menu.Show(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestFollowerInteractionWheelOverlay()
  {
    if (!((UnityEngine.Object) this._followerInteractionWheelTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    CommandItem commandItem1 = new CommandItem()
    {
      Command = FollowerCommands.CutTrees
    };
    CommandItem commandItem2 = new CommandItem()
    {
      Command = FollowerCommands.ClearRubble
    };
    CommandItem commandItem3 = new CommandItem()
    {
      Command = FollowerCommands.ClearWeeds
    };
    CommandItem commandItem4 = new CommandItem()
    {
      Command = FollowerCommands.Refiner_2
    };
    List<CommandItem> commandItemList = new List<CommandItem>()
    {
      commandItem1,
      commandItem1,
      commandItem2,
      commandItem2,
      commandItem3,
      commandItem3,
      commandItem4,
      commandItem4
    };
    List<CommandItem> commandItems = new List<CommandItem>()
    {
      new CommandItem()
      {
        Command = FollowerCommands.GiveWorkerCommand_2,
        SubCommands = commandItemList
      },
      new CommandItem() { Command = FollowerCommands.Talk }
    };
    UIFollowerInteractionWheelOverlayController overlayController1 = this._followerInteractionWheelTemplate.Instantiate<UIFollowerInteractionWheelOverlayController>();
    overlayController1.Show((Follower) null, commandItems);
    UIFollowerInteractionWheelOverlayController overlayController2 = overlayController1;
    overlayController2.OnHide = overlayController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) overlayController1;
  }

  public void TestCurseWheelOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._curseWheelOverlayTemplate);
  }

  public void TestDeathScreenOverlay()
  {
    DataManager.Instance.dungeonVisitedRooms = new List<Map.NodeType>()
    {
      Map.NodeType.FirstFloor,
      Map.NodeType.DungeonFloor,
      Map.NodeType.DungeonFloor,
      Map.NodeType.DungeonFloor,
      Map.NodeType.Boss
    };
    DataManager.Instance.itemsDungeon = new List<InventoryItem>()
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.BONE, 10),
      new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, 10),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLOOD_STONE, 10)
    };
    UIDeathScreenOverlayController overlayController1 = this._deathScreenOverlayTemplate.Instantiate<UIDeathScreenOverlayController>();
    overlayController1.Show(UIDeathScreenOverlayController.Results.Killed, 5);
    UIDeathScreenOverlayController overlayController2 = overlayController1;
    overlayController2.OnHide = overlayController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) overlayController1;
  }

  public void TestDeathScreenSurvival()
  {
    UIDeathScreenOverlayController overlayController1 = this._deathScreenOverlayTemplate.Instantiate<UIDeathScreenOverlayController>();
    overlayController1.Show(UIDeathScreenOverlayController.Results.GameOver);
    UIDeathScreenOverlayController overlayController2 = overlayController1;
    overlayController2.OnHide = overlayController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) overlayController1;
  }

  public void TestKeyScreenOverlay()
  {
    ++Inventory.KeyPieces;
    this.ShowTestMenu((UIMenuBase) this._keyScreenTemplate);
  }

  public void TestWeaponWheelOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._weaponWheelOverlayTemplate);
  }

  public void TestUpgradeUnlockOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._ugpradeUnlockOverlaytemplate);
  }

  public void TestTutorialOverlay()
  {
    TutorialTopic topic = Enum.GetValues(typeof (TutorialTopic)).Cast<TutorialTopic>().ToArray<TutorialTopic>().RandomElement<TutorialTopic>();
    UITutorialOverlayController menu = this._tutorialOverlayTemplate.Instantiate<UITutorialOverlayController>();
    menu.Show(topic);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestRecipeConfirmationOverlay()
  {
    UIRecipeConfirmationOverlayController menu = this._recipeConfirmationOverlayTemplate.Instantiate<UIRecipeConfirmationOverlayController>();
    menu.Show(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>());
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestRecipeConfirmationOverlay2()
  {
    UIRecipeConfirmationOverlayController menu = this._recipeConfirmationOverlayTemplate.Instantiate<UIRecipeConfirmationOverlayController>();
    menu.Show(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>(), true);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestItemSelectorOverlay()
  {
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    items.Add(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>());
    items.Add(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>());
    items.Add(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>());
    items.Add(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>());
    items.Add(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>());
    UIItemSelectorOverlayController menu = this._itemSelectorOverlayTemplate.Instantiate<UIItemSelectorOverlayController>();
    menu.Show(PlayerFarming.Instance, items, new ItemSelector.Params()
    {
      Key = "sell",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 100f)
    });
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestDifficultyOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._difficultyOverlayTemplate);
  }

  public void TestTarotChoice()
  {
    this._tarotChoiceOverlayTemplate.Instantiate<UITarotChoiceOverlayController>().Show(new TarotCards.TarotCard(TarotCards.Card.Hearts1, 0), new TarotCards.TarotCard(TarotCards.Card.DiseasedHeart, 0));
    this.SetInstance((UIMenuBase) this._tarotChoiceOverlayTemplate);
  }

  public void TestFleeceTarots()
  {
    this.fleeceTarotRewardOverlayController.Instantiate<UIFleeceTarotRewardOverlayController>().Show(new TarotCards.TarotCard(TarotCards.Card.Hearts1, 0), new TarotCards.TarotCard(TarotCards.Card.DiseasedHeart, 0), new TarotCards.TarotCard(TarotCards.Card.AttackRate, 0), new TarotCards.TarotCard(TarotCards.Card.MovementSpeed, 0));
    this.SetInstance((UIMenuBase) this._tarotChoiceOverlayTemplate);
  }

  public void TestControlBindingOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._controlBindingOverlayController);
  }

  public void TestAdventureMapOverlay()
  {
    UIAdventureMapOverlayController menu = this._adventureMapTemplate.Instantiate<UIAdventureMapOverlayController>();
    BiomeGenerator.Instance.Seed = UnityEngine.Random.Range(-10000, 10000);
    MapManager.Instance.MapGenerated = false;
    Map.Map map = MapGenerator.GetMap(MapManager.Instance.DungeonConfig);
    menu.Show(map);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestRoadmapOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._roadmapOverlayController);
  }

  public void TestBugReportingOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._bugReportingOverlayController);
  }

  public void TestMysticShopOverlay()
  {
    WeightedCollection<InventoryItem.ITEM_TYPE> rewards = new WeightedCollection<InventoryItem.ITEM_TYPE>();
    for (int index = 0; index < UnityEngine.Random.Range(3, 5); ++index)
      rewards.Add(Enum.GetValues(typeof (InventoryItem.ITEM_TYPE)).Cast<InventoryItem.ITEM_TYPE>().ToList<InventoryItem.ITEM_TYPE>().RandomElement<InventoryItem.ITEM_TYPE>(), 5f);
    UIMysticShopOverlayController menu = this._mysticShopOverlayController.Instantiate<UIMysticShopOverlayController>();
    menu.Show(rewards);
    this.SetInstance((UIMenuBase) menu);
    this.ShowTestMenu((UIMenuBase) this._mysticShopOverlayController);
  }

  public void TestPhotoModeGallery()
  {
    this.ShowTestMenu((UIMenuBase) this._photoGalleryMenuController);
  }

  public void TestEditPhotoOverlay()
  {
    this.ShowTestMenu((UIMenuBase) this._editPhotoOverlayController);
  }

  public void TestKnucklebones()
  {
    if (!((UnityEngine.Object) this._knuckleBonesTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    UIKnuckleBonesController knuckleBonesController1 = this._knuckleBonesTemplate.Instantiate<UIKnuckleBonesController>();
    knuckleBonesController1.Show(PlayerFarming.Instance, new KnucklebonesOpponent()
    {
      Config = this._opponentConfigurations[0]
    }, 100);
    UIKnuckleBonesController knuckleBonesController2 = knuckleBonesController1;
    knuckleBonesController2.OnHide = knuckleBonesController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) knuckleBonesController1;
  }

  public void TestKnuckleBonesBettingWindow()
  {
    if (!((UnityEngine.Object) this._knuckleBonesTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    UIKnucklebonesBettingSelectionController selectionController1 = this._kbBettingTemplate.Instantiate<UIKnucklebonesBettingSelectionController>();
    selectionController1.Show(this._opponentConfigurations[0]);
    UIKnucklebonesBettingSelectionController selectionController2 = selectionController1;
    selectionController2.OnHide = selectionController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectionController1;
  }

  public void TestKnuckleBonesOpponentWindow()
  {
    if (!((UnityEngine.Object) this._kbOpponentTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    UIKnucklebonesOpponentSelectionController selectionController1 = this._kbOpponentTemplate.Instantiate<UIKnucklebonesOpponentSelectionController>();
    selectionController1.Show(this._opponentConfigurations);
    UIKnucklebonesOpponentSelectionController selectionController2 = selectionController1;
    selectionController2.OnHide = selectionController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectionController1;
  }

  public void TestFlockade()
  {
    if (!(bool) (UnityEngine.Object) this._flockadeTemplate || (bool) (UnityEngine.Object) this._testInstance)
      return;
    FlockadeGamePiecesPoolConfiguration poolConfiguration = ((IEnumerable<FlockadeGamePiecesPoolConfiguration>) this._poolConfigurations).First<FlockadeGamePiecesPoolConfiguration>();
    foreach (FlockadeGamePieceConfiguration allPiece in poolConfiguration.GetAllPieces())
      FlockadePieceManager.UnlockPiece(allPiece.Type);
    FlockadeUIController flockadeUiController1 = this._flockadeTemplate.Instantiate<FlockadeUIController>();
    flockadeUiController1.Show(PlayerFarming.Instance, poolConfiguration.GetGamePieces(), ((IEnumerable<FlockadeOpponentConfiguration>) this._availableOpponents).First<FlockadeOpponentConfiguration>(), this._bet);
    FlockadeUIController flockadeUiController2 = flockadeUiController1;
    flockadeUiController2.OnHide = flockadeUiController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) flockadeUiController1;
  }

  public void TestFlockadeBettingWindow()
  {
    if ((UnityEngine.Object) this._flockadeBettingTemplate == (UnityEngine.Object) null || (UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    FlockadeBettingSelectionUIController selectionUiController1 = this._flockadeBettingTemplate.Instantiate<FlockadeBettingSelectionUIController>();
    selectionUiController1.Show(((IEnumerable<FlockadeOpponentConfiguration>) this._availableOpponents).First<FlockadeOpponentConfiguration>());
    FlockadeBettingSelectionUIController selectionUiController2 = selectionUiController1;
    selectionUiController2.OnHide = selectionUiController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectionUiController1;
  }

  public void TestFlockadeOpponentWindow()
  {
    if ((UnityEngine.Object) this._flockadeOpponentSelectionTemplate == (UnityEngine.Object) null || (UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    FlockadeOpponentSelectionUIController selectionUiController1 = this._flockadeOpponentSelectionTemplate.Instantiate<FlockadeOpponentSelectionUIController>();
    selectionUiController1.Show(this._availableOpponents);
    FlockadeOpponentSelectionUIController selectionUiController2 = selectionUiController1;
    selectionUiController2.OnHide = selectionUiController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectionUiController1;
  }

  public void TestFlockadePiecesMenu()
  {
    if ((UnityEngine.Object) this._flockadePiecesMenuTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    UIFlockadePiecesMenuController piecesMenuController1 = this._flockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
    piecesMenuController1.Show();
    UIFlockadePiecesMenuController piecesMenuController2 = piecesMenuController1;
    piecesMenuController2.OnHide = piecesMenuController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) piecesMenuController1;
  }

  public void TestUnlockOneFlockadePiecesMenu()
  {
    if ((UnityEngine.Object) this._flockadePiecesMenuTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    FlockadePieceType flockadePieceType = Enum.GetValues(typeof (FlockadePieceType)).Cast<FlockadePieceType>().ToArray<FlockadePieceType>().RandomElement<FlockadePieceType>();
    FlockadePieceManager.UnlockPiece(flockadePieceType);
    UIFlockadePiecesMenuController piecesMenuController1 = this._flockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
    piecesMenuController1.Show(flockadePieceType, PlayerFarming.Instance);
    UIFlockadePiecesMenuController piecesMenuController2 = piecesMenuController1;
    piecesMenuController2.OnHide = piecesMenuController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) piecesMenuController1;
  }

  public void TestUnlockManyFlockadePiecesMenu()
  {
    if ((UnityEngine.Object) this._flockadePiecesMenuTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    List<FlockadePieceType> pieceTypes = new List<FlockadePieceType>();
    foreach (FlockadePieceType piece in Enum.GetValues(typeof (FlockadePieceType)).Cast<FlockadePieceType>().OrderBy<FlockadePieceType, float>((Func<FlockadePieceType, float>) (_ => UnityEngine.Random.value)).Take<FlockadePieceType>(4))
    {
      FlockadePieceManager.UnlockPiece(piece);
      pieceTypes.Add(piece);
    }
    UIFlockadePiecesMenuController piecesMenuController1 = this._flockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
    piecesMenuController1.Show(pieceTypes, PlayerFarming.Instance);
    UIFlockadePiecesMenuController piecesMenuController2 = piecesMenuController1;
    piecesMenuController2.OnHide = piecesMenuController2.OnHide + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) piecesMenuController1;
  }

  public void TestTotemWheel()
  {
    UITwitchTotemWheel menu = this._twitchTotemWheelTemplate.Instantiate<UITwitchTotemWheel>();
    List<UIRandomWheel.Segment> ts = new List<UIRandomWheel.Segment>();
    ts.Add(new UIRandomWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.LOG
    });
    ts.Add(new UIRandomWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.MEAT
    });
    ts.Add(new UIRandomWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.FOLLOWERS
    });
    ts.Add(new UIRandomWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.STONE
    });
    ts.Shuffle<UIRandomWheel.Segment>();
    ts.Insert(0, new UIRandomWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    ts.Insert(2, new UIRandomWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    ts.Insert(4, new UIRandomWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    ts.Insert(6, new UIRandomWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    menu.Show(ts.ToArray());
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestTwitchSettingsMenu()
  {
    UITwitchSettingsMenuController menu = this._uiTwitchMenuSettingsController.Instantiate<UITwitchSettingsMenuController>();
    menu.Show();
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestCoOpInput()
  {
    this._assignCoOpController.Instantiate<UICoopAssignController>().Show();
  }

  public void TestRanchMenu()
  {
    List<RanchSelectEntry> followerSelectEntries = new List<RanchSelectEntry>();
    for (int index = 0; index < 10; ++index)
    {
      StructuresData.Ranchable_Animal animalInfo = this.NewCharacter();
      followerSelectEntries.Add(new RanchSelectEntry(animalInfo));
    }
    UIRanchSelectMenuController selectMenuController1 = this._ranchSelectMenuController.Instantiate<UIRanchSelectMenuController>();
    selectMenuController1.Show(followerSelectEntries, 0, true);
    UIRanchSelectMenuController selectMenuController2 = selectMenuController1;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectMenuController1;
  }

  public void TestSnowManMiniGame()
  {
    this._snowmanMiniGameController.gameObject.SetActive(true);
    this._snowmanMiniGameController.Show();
    this._snowmanMiniGameController.Initialize();
  }

  public StructuresData.Ranchable_Animal NewCharacter()
  {
    InventoryItem.ITEM_TYPE allAnimal = InventoryItem.AllAnimals[UnityEngine.Random.Range(0, InventoryItem.AllAnimals.Count)];
    float num = UnityEngine.Random.Range(-0.5f, 0.0f);
    switch (allAnimal)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        num = UnityEngine.Random.Range(-0.2f, 0.0f);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        num = UnityEngine.Random.Range(-0.5f, -0.3f);
        break;
    }
    return new StructuresData.Ranchable_Animal()
    {
      Type = allAnimal,
      Age = 1,
      ID = DataManager.Instance.AnimalID,
      Ears = UnityEngine.Random.Range(1, 6),
      Head = UnityEngine.Random.Range(1, 6),
      Horns = UnityEngine.Random.Range(1, 6),
      Colour = UnityEngine.Random.Range(0, 10),
      Speed = num,
      TimeSinceLastWash = TimeManager.TotalElapsedGameTime + 4800f + UnityEngine.Random.Range(-120f, 120f),
      TimeSincePoop = TimeManager.TotalElapsedGameTime + 1920f + UnityEngine.Random.Range(-120f, 120f)
    };
  }

  public void TestAllMenusInArabic()
  {
    if (LocalizationManager.HasLanguage("Arabic"))
    {
      LocalizationManager.CurrentLanguage = "Arabic";
      Debug.Log((object) "LANGUAGE NOT FOUND");
    }
    else
      this.TestMenus();
  }

  public void TestAllMenusInChineseSimplified()
  {
    if (LocalizationManager.HasLanguage("Chinese (Simplified)"))
    {
      LocalizationManager.CurrentLanguage = "Chinese (Simplified)";
      Debug.Log((object) "LANGUAGE NOT FOUND");
      this.TestMenus();
    }
    else
      Debug.Log((object) "LANGUAGE NOT FOUND");
  }

  public void TestAllMenusInChineseTraditional()
  {
    if (LocalizationManager.HasLanguage("Chinese (Traditional)"))
    {
      LocalizationManager.CurrentLanguage = "Chinese (Traditional)";
      this.TestMenus();
    }
    else
      Debug.Log((object) "LANGUAGE NOT FOUND");
  }

  public void TestMenus()
  {
    this._allMenuTests = new List<System.Action>()
    {
      new System.Action(this.TestMainMenu),
      new System.Action(this.TestSettingsMenu),
      new System.Action(this.TestPauseMenu),
      new System.Action(this.TestCreditsMenu),
      new System.Action(this.TestBuildMenu),
      new System.Action(this.TestFollowerSelectmenu),
      new System.Action(this.TestDeadFollowerSelect),
      new System.Action(this.TestCryptMenu),
      new System.Action(this.TestFollowerIndoctrinationMenu),
      new System.Action(this.TestCultNameMenu),
      new System.Action(this.TestPauseDetailsMenu),
      new System.Action(this.TestFollowerFormsMenu),
      new System.Action(this.TestTarotCardsMenu),
      new System.Action(this.TestRelicsMenu),
      new System.Action(this.TestUpgradeShopMenu),
      new System.Action(this.TestWorldMapMenu),
      new System.Action(this.TestTutorialMenu),
      new System.Action(this.TestUpgradeTree),
      new System.Action(this.TestPlayerUpgradeTree),
      new System.Action(this.TestRefineryMenu),
      new System.Action(this.TestKitchenMenu),
      new System.Action(this.TestFollowerSummaryMenu),
      new System.Action(this.TestVideoExportMenu),
      new System.Action(this.TestSandboxMenu),
      new System.Action(this.TestAchievementsMenu),
      new System.Action(this.TestPrisonMenu),
      new System.Action(this.TestPrisonerMenu),
      new System.Action(this.TestMissionaryMenu),
      new System.Action(this.TestMissionMenu),
      new System.Action(this.TestDemonSummonMenu),
      new System.Action(this.TestDemonMenu),
      new System.Action(this.TestConfirmationMenu),
      new System.Action(this.TestConfirmationCountdownMenu),
      new System.Action(this.TestSaveErrorWindow),
      new System.Action(this.TestDoctrineMenu),
      new System.Action(this.TestAltarMenu),
      new System.Action(this.TestDoctrineChoicesMenu),
      new System.Action(this.TestRitualsMenu),
      new System.Action(this.TestRitualChoiceMenu),
      new System.Action(this.TestPlayerUpgrades),
      new System.Action(this.TestSermonWheelOverlay),
      new System.Action(this.TestFollowerInteractionWheelOverlay),
      new System.Action(this.TestCurseWheelOverlay),
      new System.Action(this.TestDeathScreenOverlay),
      new System.Action(this.TestKeyScreenOverlay),
      new System.Action(this.TestWeaponWheelOverlay),
      new System.Action(this.TestUpgradeUnlockOverlay),
      new System.Action(this.TestTutorialOverlay),
      new System.Action(this.TestRecipeConfirmationOverlay),
      new System.Action(this.TestItemSelectorOverlay),
      new System.Action(this.TestDifficultyOverlay),
      new System.Action(this.TestTarotChoice),
      new System.Action(this.TestFleeceTarots),
      new System.Action(this.TestControlBindingOverlay),
      new System.Action(this.TestAdventureMapOverlay),
      new System.Action(this.TestRoadmapOverlay),
      new System.Action(this.TestBugReportingOverlay),
      new System.Action(this.TestMysticShopOverlay),
      new System.Action(this.TestPhotoModeGallery),
      new System.Action(this.TestEditPhotoOverlay),
      new System.Action(this.TestKnucklebones),
      new System.Action(this.TestKnuckleBonesBettingWindow),
      new System.Action(this.TestKnuckleBonesOpponentWindow),
      new System.Action(this.TestFlockade),
      new System.Action(this.TestFlockadeBettingWindow),
      new System.Action(this.TestFlockadeOpponentWindow),
      new System.Action(this.TestTotemWheel),
      new System.Action(this.TestTwitchSettingsMenu),
      new System.Action(this.TestCoOpInput),
      new System.Action(this.TestRanchMenu),
      new System.Action(this.TestCookingFireMenu)
    };
    this.StartCoroutine((IEnumerator) this.TestMenusSequentially());
  }

  public IEnumerator TestMenusSequentially()
  {
    while (this._currentIndex < this._allMenuTests.Count)
    {
      System.Action allMenuTest = this._allMenuTests[this._currentIndex];
      if (allMenuTest != null)
        allMenuTest();
      yield return (object) new WaitForSecondsRealtime(1f);
      string filename = Path.Combine(this._screenshotPath, $"Menu_{this._currentIndex}_Arabic.png");
      ScreenCapture.CaptureScreenshot(filename);
      Debug.Log((object) ("Screenshot saved: " + filename));
      yield return (object) new WaitForSecondsRealtime(0.1f);
      this.Hide();
      ++this._currentIndex;
      yield return (object) new WaitForSecondsRealtime(0.1f);
    }
    Debug.Log((object) "All menus tested in Arabic.");
  }

  public void ShowTestMenu(UIMenuBase menu)
  {
    if ((UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    UIMenuBase menu1 = menu.Instantiate<UIMenuBase>();
    menu1.Show();
    this.SetInstance(menu1);
  }

  public void SetInstance(UIMenuBase menu)
  {
    if (!((UnityEngine.Object) menu != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    Time.timeScale = 0.0f;
    this._testInstance = menu;
    this._testInstance.OnHide += (System.Action) (() =>
    {
      this._testInstance = (UIMenuBase) null;
      Time.timeScale = 1f;
    });
  }

  public void Hide()
  {
    if (!((UnityEngine.Object) this._testInstance != (UnityEngine.Object) null))
      return;
    this._testInstance.Hide();
  }

  public void ClearSandbox() => DataManager.Instance.SandboxProgression.Clear();

  public void AllFleeces()
  {
    DataManager.Instance.UnlockedFleeces.Add(0);
    DataManager.Instance.UnlockedFleeces.Add(1);
    DataManager.Instance.UnlockedFleeces.Add(2);
    DataManager.Instance.UnlockedFleeces.Add(3);
    DataManager.Instance.UnlockedFleeces.Add(4);
    DataManager.Instance.UnlockedFleeces.Add(5);
  }

  public void ObjectivesTest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.FLOWER_RED, 10, false, FollowerLocation.Dungeon1_1, 4800f));
    ObjectiveManager.Add((ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Ill, UnityEngine.Random.Range(2, 4)));
    ObjectiveManager.Add((ObjectivesData) new Objectives_CookMeal("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.MEAL_GREAT, 3, 9600f));
    ObjectiveManager.Add((ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_WorkThroughNight, questExpireDuration: 4800f));
  }

  public void UnlockRecipesMinimal()
  {
    DataManager.Instance.RecipesDiscovered.Add(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>());
    DataManager.Instance.RecipesDiscovered.Add(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>());
    DataManager.Instance.RecipesDiscovered.Add(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>());
    DataManager.Instance.RecipesDiscovered.Add(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>());
    DataManager.Instance.RecipesDiscovered.Add(CookingData.GetAllMeals().RandomElement<InventoryItem.ITEM_TYPE>());
    Inventory.AddItem(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>(), 3);
    Inventory.AddItem(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>(), 3);
    Inventory.AddItem(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>(), 3);
    Inventory.AddItem(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>(), 3);
    Inventory.AddItem(CookingData.GetAllFoods().RandomElement<InventoryItem.ITEM_TYPE>(), 3);
  }

  public void UnlockRecipes()
  {
    foreach (InventoryItem.ITEM_TYPE allMeal in CookingData.GetAllMeals())
    {
      if (!DataManager.Instance.RecipesDiscovered.Contains(allMeal))
        DataManager.Instance.RecipesDiscovered.Add(allMeal);
    }
    foreach (InventoryItem.ITEM_TYPE allFood in CookingData.GetAllFoods())
      Inventory.AddItem(allFood, 3);
  }

  public void RandomAbilityPoints()
  {
    DataManager.Instance.AbilityPoints += UnityEngine.Random.Range(5, 10);
    DataManager.Instance.DiscipleAbilityPoints += UnityEngine.Random.Range(5, 10);
  }

  public void RandomTutorials()
  {
    List<TutorialTopic> list = ((IEnumerable<TutorialTopic>) Enum.GetValues(typeof (TutorialTopic)).Cast<TutorialTopic>().ToArray<TutorialTopic>()).ToList<TutorialTopic>();
    int num = UnityEngine.Random.Range(0, list.Count / 2);
    for (int index = 0; index < num; ++index)
    {
      TutorialTopic topic = list.RandomElement<TutorialTopic>();
      list.Remove(topic);
      DataManager.Instance.TryRevealTutorialTopic(topic);
    }
  }

  public void AllTutorials()
  {
    foreach (TutorialTopic topic in Enum.GetValues(typeof (TutorialTopic)).Cast<TutorialTopic>().ToArray<TutorialTopic>())
      DataManager.Instance.TryRevealTutorialTopic(topic);
  }

  public void RandomLocations()
  {
    for (int index = 0; (double) index < (double) UIWorldMapMenuController.UnlockableMapLocations.Length * 0.5; ++index)
    {
      FollowerLocation location = UIWorldMapMenuController.UnlockableMapLocations.RandomElement<FollowerLocation>();
      while (!DataManager.Instance.DiscoverLocation(location))
        location = UIWorldMapMenuController.UnlockableMapLocations.RandomElement<FollowerLocation>();
      DataManager.Instance.DiscoveredLocations.Add(location);
    }
  }

  public void AllLocations()
  {
    foreach (FollowerLocation unlockableMapLocation in UIWorldMapMenuController.UnlockableMapLocations)
      DataManager.Instance.DiscoverLocation(unlockableMapLocation);
  }

  public void DoctrineDataTest()
  {
    SaveAndLoad.ResetSave(0, true);
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 4, true));
    SaveAndLoad.Save();
  }

  public void DoctrineDataTest2()
  {
    SaveAndLoad.ResetSave(0, true);
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 2, false));
    SaveAndLoad.Save();
  }

  public void AllDoctrinesFirstChoice()
  {
    SaveAndLoad.ResetSave(0, true);
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 4, true));
    SaveAndLoad.Save();
  }

  public void AllDoctrinesSecondChoice()
  {
    SaveAndLoad.ResetSave(0, true);
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 4, false));
    SaveAndLoad.Save();
  }

  public void AllDoctrines()
  {
    SaveAndLoad.ResetSave(0, true);
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 1, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 2, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 3, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 4, true));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Food, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.WorkAndWorship, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Possession, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Afterlife, 4, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 1, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 2, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 3, false));
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.LawAndOrder, 4, false));
    SaveAndLoad.Save();
  }

  public void RitualDataTest()
  {
    SaveAndLoad.ResetSave(0, true);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_UnlockWeapon);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_UnlockCurse);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Sacrifice);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Reindoctrinate);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_ConsumeFollower);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FasterBuilding);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Enlightenment);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_WorkThroughNight);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Holiday);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AlmsToPoor);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_DonationRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fast);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Feast);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HarvestRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FishingRitual);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ressurect);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Funeral);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fightpit);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Wedding);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignFaithEnforcer);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignTaxCollector);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ascend);
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FirePit);
    SaveAndLoad.Save();
  }

  public void UnlockAllStructures() => CheatConsole.AllBuildingsUnlocked = true;

  public void UnlockSkins()
  {
    for (int index = 0; index < 10; ++index)
      DataManager.SetFollowerSkinUnlocked(WorshipperData.Instance.Characters.RandomElement<WorshipperData.SkinAndData>().Skin[0].Skin);
  }

  public void UnlockAllSkins()
  {
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
      DataManager.SetFollowerSkinUnlocked(character.Skin[0].Skin);
  }

  public void RandomTarotCards()
  {
    List<TarotCards.Card> cardList = new List<TarotCards.Card>();
    int num = UnityEngine.Random.Range(5, 10);
    while (cardList.Count < num)
    {
      TarotCards.Card card = DataManager.AllTrinkets.RandomElement<TarotCards.Card>();
      if (!cardList.Contains(card) && !DataManager.TarotCardBlacklist.Contains(card))
      {
        cardList.Add(card);
        TarotCards.UnlockTrinket(card);
      }
    }
  }

  public void AllTarotCards()
  {
    foreach (TarotCards.Card card in Enum.GetValues(typeof (TarotCards.Card)).Cast<TarotCards.Card>())
      TarotCards.UnlockTrinket(card);
  }

  public void FillInventory()
  {
    foreach (InventoryItem.ITEM_TYPE type in Enum.GetValues(typeof (InventoryItem.ITEM_TYPE)))
      Inventory.AddItem((int) type, 5);
  }

  public void RandomiseCursesWeapons()
  {
    this.RandomCurses();
    List<TarotCards.Card> cardList1 = new List<TarotCards.Card>();
    List<TarotCards.Card> cardList2 = new List<TarotCards.Card>();
    int num = UnityEngine.Random.Range((int) ((double) DataManager.AllTrinkets.Count / 2.0), DataManager.AllTrinkets.Count);
    while (cardList2.Count < num)
    {
      TarotCards.Card type = DataManager.AllTrinkets.RandomElement<TarotCards.Card>();
      if (!cardList2.Contains(type) && !DataManager.TarotCardBlacklist.Contains(type))
      {
        cardList2.Add(type);
        TrinketManager.AddTrinket(new TarotCards.TarotCard(type, 0), PlayerFarming.Instance);
      }
    }
  }

  public void RandomCurses()
  {
    List<TarotCards.Card> cardList = new List<TarotCards.Card>();
  }

  public void AllCurses()
  {
  }

  public void BonesAndCoins()
  {
    Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1000);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.BONE, 1000);
  }

  public void AbilityPoints() => DataManager.Instance.AbilityPoints += 100;

  public void AllRelics()
  {
    foreach (RelicType relicType in Enum.GetValues(typeof (RelicType)))
      DataManager.Instance.PlayerFoundRelics.Add(relicType);
  }

  public void SaveData() => SaveAndLoad.Save();

  public void ResetData()
  {
    SaveAndLoad.ResetSave(5, true);
    DataManager.Instance.UnlockedFleeces.Add(0);
    DataManager.Instance.UnlockedFleeces.Add(1);
    DataManager.Instance.UnlockedFleeces.Add(2);
    DataManager.Instance.UnlockedFleeces.Add(3);
    Singleton<SettingsManager>.Instance.ApplySettings();
  }

  [CompilerGenerated]
  public void \u003CTestFollowerSelectmenu\u003Eb__27_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestTarotCardsMenuNewUnlock\u003Eb__43_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CTestTarotCardsMenuNewUnlock2\u003Eb__44_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CTestRelicMenuNewUnlock\u003Eb__47_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CTestRelicMenuNewUnlocks\u003Eb__48_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CTestWorldMapMenuReveal\u003Eb__53_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CTestWorldMapMenuReReveal\u003Eb__54_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CTestPrisonMenu\u003Eb__81_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestPrisonerMenu\u003Eb__83_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestMissionaryMenu\u003Eb__85_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestMissionMenu\u003Eb__87_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestDemonSummonMenu\u003Eb__89_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestDemonMenu\u003Eb__91_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestFollowerInteractionWheelOverlay\u003Eb__119_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestDeathScreenOverlay\u003Eb__123_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestDeathScreenSurvival\u003Eb__124_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestKnucklebones\u003Eb__159_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestKnuckleBonesBettingWindow\u003Eb__162_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestKnuckleBonesOpponentWindow\u003Eb__164_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestFlockade\u003Eb__169_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CTestFlockadeBettingWindow\u003Eb__171_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestFlockadeOpponentWindow\u003Eb__173_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestFlockadePiecesMenu\u003Eb__175_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestUnlockOneFlockadePiecesMenu\u003Eb__176_0()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestUnlockManyFlockadePiecesMenu\u003Eb__177_1()
  {
    this._testInstance = (UIMenuBase) null;
  }

  [CompilerGenerated]
  public void \u003CTestRanchMenu\u003Eb__185_0() => this._testInstance = (UIMenuBase) null;

  [CompilerGenerated]
  public void \u003CSetInstance\u003Eb__195_0()
  {
    this._testInstance = (UIMenuBase) null;
    Time.timeScale = 1f;
  }

  public enum PlayerConfiguration
  {
    SoloLamb,
    SoloGoat,
    Coop,
  }
}
