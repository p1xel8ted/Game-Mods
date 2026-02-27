// Decompiled with JetBrains decompiler
// Type: src.UI.Testing.MenuTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace src.UI.Testing;

public class MenuTester : MonoBehaviour
{
  private UIMenuBase _testInstance;
  [SerializeField]
  private UIMainMenuController _mainMenuTemplate;
  [SerializeField]
  private UISettingsMenuController _settingsMenuTemplate;
  [SerializeField]
  private UIPauseMenuController _pauseMenuTemplate;
  [SerializeField]
  private UICreditsMenuController _creditsMenuTemplate;
  [SerializeField]
  private UIBuildMenuController _buildMenuTemplate;
  [SerializeField]
  private UIFollowerSelectMenuController _followerSelectMenuTemplate;
  [SerializeField]
  private UIFollowerIndoctrinationMenuController _followerIndoctrinationMenuTemplate;
  [SerializeField]
  private UICultNameMenuController _uiCultNameMenuTemplate;
  [SerializeField]
  private UIPauseDetailsMenuController _pauseDetailsMenuTemplate;
  [SerializeField]
  private UIFollowerFormsMenuController _followerFormsMenuTemplate;
  [SerializeField]
  private UITarotCardsMenuController _tarotCardsMenuTemplate;
  [SerializeField]
  private UIUpgradeShopMenuController _upgradeShopMenuTemplate;
  [SerializeField]
  private UIWorldMapMenuController _worldMapMenuController;
  [SerializeField]
  private UITutorialMenuController _tutorialMenuTemplate;
  [SerializeField]
  private UIUpgradeTreeMenuController _upgradeTreeTemplate;
  [SerializeField]
  private UIUpgradePlayerTreeMenuController _playerUpgradeTreeTemplate;
  [SerializeField]
  private UIRefineryMenuController _refineryMenuControllerTemplate;
  [SerializeField]
  private UICookingFireMenuController _cookingFireMenuTemplate;
  [SerializeField]
  private UIKitchenMenuController _kitchenMenuTemplate;
  [SerializeField]
  private UIFollowerSummaryMenuController _followerSummaryMenuTemplate;
  [SerializeField]
  private UIVideoExportMenuController _uiVideoExportMenuController;
  [SerializeField]
  private UIPrisonMenuController _prisonMenuTemplate;
  [SerializeField]
  private UIPrisonerMenuController _prisonerMenuTemplate;
  [SerializeField]
  private UIMissionaryMenuController _missionaryMenuTemplate;
  [SerializeField]
  private UIMissionMenuController _missionMenuTemplate;
  [SerializeField]
  private UIDemonSummonMenuController _demonSummonMenuTemplate;
  [SerializeField]
  private UIDemonMenuController _demonMenuTemplate;
  [SerializeField]
  private UIMenuConfirmationWindow _confirmationWindowTemplate;
  [SerializeField]
  private UIConfirmationCountdownWindow _confirmationCountdownWindowTemplate;
  [SerializeField]
  private UISaveErrorMenuController _saveErrorWindowTemplate;
  [SerializeField]
  private UIDoctrineMenuController _doctrineMenuTemplate;
  [SerializeField]
  private UIAltarMenuController _altarMenuTemplate;
  [SerializeField]
  private UIDoctrineChoicesMenuController _doctrineChoicesMenuTemplate;
  [SerializeField]
  private UIRitualsMenuController _ritualMenuTemplate;
  [SerializeField]
  private UIHeartsOfTheFaithfulChoiceMenuController _heartsOfTheFaithfulChoiceMenuController;
  [SerializeField]
  private UIPlayerUpgradesMenuController _playerUpgradesMenuControllerTemplate;
  [SerializeField]
  private UISermonWheelController _sermonWheelTemplate;
  [SerializeField]
  private UIFollowerInteractionWheelOverlayController _followerInteractionWheelTemplate;
  [SerializeField]
  private UICurseWheelController _curseWheelOverlayTemplate;
  [SerializeField]
  private UIDeathScreenOverlayController _deathScreenOverlayTemplate;
  [SerializeField]
  private UIKeyScreenOverlayController _keyScreenTemplate;
  [SerializeField]
  private UIWeaponWheelController _weaponWheelOverlayTemplate;
  [SerializeField]
  private UIUpgradeUnlockOverlayControllerBase _ugpradeUnlockOverlaytemplate;
  [SerializeField]
  private UITutorialOverlayController _tutorialOverlayTemplate;
  [SerializeField]
  private UIRecipeConfirmationOverlayController _recipeConfirmationOverlayTemplate;
  [SerializeField]
  private UIItemSelectorOverlayController _itemSelectorOverlayTemplate;
  [SerializeField]
  private UIDifficultySelectorOverlayController _difficultyOverlayTemplate;
  [SerializeField]
  private UITarotChoiceOverlayController _tarotChoiceOverlayTemplate;
  [SerializeField]
  private UIFleeceTarotRewardOverlayController fleeceTarotRewardOverlayController;
  [SerializeField]
  private UIControlBindingOverlayController _controlBindingOverlayController;
  [SerializeField]
  private UIAdventureMapOverlayController _adventureMapTemplate;
  [SerializeField]
  private UIRoadmapOverlayController _roadmapOverlayController;
  [SerializeField]
  private UIBugReportingOverlayController _bugReportingOverlayController;
  [SerializeField]
  private UIKnuckleBonesController _knuckleBonesTemplate;
  [SerializeField]
  private KnucklebonesPlayerConfiguration[] _opponentConfigurations;
  [SerializeField]
  private UIKnucklebonesBettingSelectionController _kbBettingTemplate;
  [SerializeField]
  private UIKnucklebonesOpponentSelectionController _kbOpponentTemplate;
  [SerializeField]
  private UITwitchTotemWheel _twitchTotemWheelTemplate;

  public void TestMainMenu() => this.ShowTestMenu((UIMenuBase) this._mainMenuTemplate);

  public void TestSettingsMenu() => this.ShowTestMenu((UIMenuBase) this._settingsMenuTemplate);

  public void TestPauseMenu() => this.ShowTestMenu((UIMenuBase) this._pauseMenuTemplate);

  public void TestCreditsMenu() => this.ShowTestMenu((UIMenuBase) this._creditsMenuTemplate);

  public void TestBuidMenu() => this.ShowTestMenu((UIMenuBase) this._buildMenuTemplate);

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
    List<FollowerInfo> followerInfo1 = new List<FollowerInfo>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo2 = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo2.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerInfo1.Add(followerInfo2);
    }
    UIFollowerSelectMenuController selectMenuController1 = this._followerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    selectMenuController1.Show(followerInfo1, (List<FollowerInfo>) null, false, UpgradeSystem.Type.Count, true, true, true);
    UIFollowerSelectMenuController selectMenuController2 = selectMenuController1;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) selectMenuController1;
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
    indoctrinationMenuController1.Show(follower);
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
    TarotCards.Card card = DataManager.AllTrinkets.RandomElement<TarotCards.Card>();
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

  public void TestTutorialMenu() => this.ShowTestMenu((UIMenuBase) this._tutorialMenuTemplate);

  public void TestUpgradeTree() => this.ShowTestMenu((UIMenuBase) this._upgradeTreeTemplate);

  public void TestPlayerUpgradeTree()
  {
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
    menu.Show(100, false);
    this.SetInstance((UIMenuBase) menu);
  }

  public void TestPrisonMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerInfo> followerInfo1 = new List<FollowerInfo>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo2 = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo2.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerInfo1.Add(followerInfo2);
    }
    UIPrisonMenuController prisonMenuController1 = this._prisonMenuTemplate.Instantiate<UIPrisonMenuController>();
    prisonMenuController1.Show(followerInfo1, (List<FollowerInfo>) null, false, UpgradeSystem.Type.Count, true, true, true);
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
    List<FollowerInfo> followerInfo1 = new List<FollowerInfo>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo2 = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo2.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerInfo1.Add(followerInfo2);
    }
    UIMissionaryMenuController missionaryMenuController1 = this._missionaryMenuTemplate.Instantiate<UIMissionaryMenuController>();
    missionaryMenuController1.Show(followerInfo1, (List<FollowerInfo>) null, false, UpgradeSystem.Type.Count, true, true, true);
    UIMissionaryMenuController missionaryMenuController2 = missionaryMenuController1;
    missionaryMenuController2.OnHidden = missionaryMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) missionaryMenuController1;
  }

  public void TestMissionMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base);
    followerInfo.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
    followerInfo.MissionaryTimestamp = 3f;
    UIMissionMenuController missionMenuController1 = this._missionMenuTemplate.Instantiate<UIMissionMenuController>();
    missionMenuController1.Show(new List<int>(followerInfo.ID));
    UIMissionMenuController missionMenuController2 = missionMenuController1;
    missionMenuController2.OnHidden = missionMenuController2.OnHidden + (System.Action) (() => this._testInstance = (UIMenuBase) null);
    this._testInstance = (UIMenuBase) missionMenuController1;
  }

  public void TestDemonSummonMenu()
  {
    FollowerTrait.TraitType[] array = Enum.GetValues(typeof (FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>().ToArray<FollowerTrait.TraitType>();
    List<FollowerInfo> followerInfo1 = new List<FollowerInfo>();
    for (int index = 0; index < 10; ++index)
    {
      FollowerInfo followerInfo2 = FollowerInfo.NewCharacter(FollowerLocation.Base);
      followerInfo2.Traits.Add(array.RandomElement<FollowerTrait.TraitType>());
      followerInfo1.Add(followerInfo2);
    }
    UIDemonSummonMenuController summonMenuController1 = this._demonSummonMenuTemplate.Instantiate<UIDemonSummonMenuController>();
    summonMenuController1.Show(followerInfo1, (List<FollowerInfo>) null, false, UpgradeSystem.Type.Count, true, true, true);
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
    demonMenuController1.Show(new List<int>()
    {
      followerInfo.ID
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

  public void TestSermonWheelOverlay() => this.ShowTestMenu((UIMenuBase) this._sermonWheelTemplate);

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
    DataManager.Instance.dungeonVisitedRooms = new List<NodeType>()
    {
      NodeType.FirstFloor,
      NodeType.DungeonFloor,
      NodeType.DungeonFloor,
      NodeType.DungeonFloor,
      NodeType.Boss
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
    menu.Show(items, new ItemSelector.Params()
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

  public void TestKnucklebones()
  {
    if (!((UnityEngine.Object) this._knuckleBonesTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this._testInstance == (UnityEngine.Object) null))
      return;
    UIKnuckleBonesController knuckleBonesController1 = this._knuckleBonesTemplate.Instantiate<UIKnuckleBonesController>();
    knuckleBonesController1.Show(new KnucklebonesOpponent()
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

  public void TestTotemWheel()
  {
    UITwitchTotemWheel menu = this._twitchTotemWheelTemplate.Instantiate<UITwitchTotemWheel>();
    List<UITwitchTotemWheel.Segment> ts = new List<UITwitchTotemWheel.Segment>();
    ts.Add(new UITwitchTotemWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.LOG
    });
    ts.Add(new UITwitchTotemWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.MEAT
    });
    ts.Add(new UITwitchTotemWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.FOLLOWERS
    });
    ts.Add(new UITwitchTotemWheel.Segment()
    {
      probability = 0.2f,
      reward = InventoryItem.ITEM_TYPE.STONE
    });
    ts.Shuffle<UITwitchTotemWheel.Segment>();
    ts.Insert(0, new UITwitchTotemWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    ts.Insert(2, new UITwitchTotemWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    ts.Insert(4, new UITwitchTotemWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    ts.Insert(6, new UITwitchTotemWheel.Segment()
    {
      probability = 0.05f,
      reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
    });
    menu.Show(ts.ToArray());
    this.SetInstance((UIMenuBase) menu);
  }

  private void Start()
  {
    Singleton<SettingsManager>.Instance.LoadAndApply();
    SaveAndLoad.Load(5);
  }

  private void ShowTestMenu(UIMenuBase menu)
  {
    if ((UnityEngine.Object) this._testInstance != (UnityEngine.Object) null)
      return;
    UIMenuBase menu1 = menu.Instantiate<UIMenuBase>();
    menu1.Show();
    this.SetInstance(menu1);
  }

  private void SetInstance(UIMenuBase menu)
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
        TrinketManager.AddTrinket(new TarotCards.TarotCard(type, 0));
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

  public void SaveData() => SaveAndLoad.Save();

  public void ResetData()
  {
    SaveAndLoad.ResetSave(5, true);
    Singleton<SettingsManager>.Instance.ApplySettings();
  }
}
