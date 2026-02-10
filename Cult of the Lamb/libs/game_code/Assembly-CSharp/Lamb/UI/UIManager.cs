// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using FMOD.Studio;
using Lamb.UI.AltarMenu;
using Lamb.UI.BuildMenu;
using Lamb.UI.DeathScreen;
using Lamb.UI.FollowerInteractionWheel;
using Lamb.UI.FollowerSelect;
using Lamb.UI.KitchenMenu;
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
using Lamb.UI.VideoMenu;
using MMTools;
using src.Extensions;
using src.UI;
using src.UI.Items;
using src.UI.Menus;
using src.UI.Menus.Achievements_Menu;
using src.UI.Menus.CryptMenu;
using src.UI.Menus.ShareHouseMenu;
using src.UI.Overlays.SermonXPOverlay;
using src.UI.Overlays.TutorialOverlay;
using src.UI.Overlays.TwitchFollowerVotingOverlay;
using src.UI.Prompts;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UI.Menus.TwitchMenu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
namespace Lamb.UI;

[DefaultExecutionOrder(-100)]
public class UIManager : MonoSingleton<UIManager>
{
  [Header("Cursor")]
  [SerializeField]
  public Texture2D[] _cursors;
  [Header("Direct References")]
  [SerializeField]
  public UISettingsMenuController _settingsMenuTemplate;
  [SerializeField]
  public UIMenuConfirmationWindow _confirmationWindowTemplate;
  [SerializeField]
  public UIConfirmationCountdownWindow _confirmationCountdownWindowTemplate;
  [Header("Controllers")]
  [SerializeField]
  public AssetReferenceGameObject _xboxControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ps4ControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ps5ControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _switchProControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _switchJoyConsControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _switchJoyConsDetachedControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _switchHandheldControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _keyboardTemplate;
  [SerializeField]
  public AssetReferenceGameObject _mouseTemplate;
  [Header("Menu Templates")]
  [SerializeField]
  public AssetReferenceGameObject _creditsMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _roadmapOverlayControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _pauseMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _pauseDetailsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _indoctrinationMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _customizeClothingMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tarotCardsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _worldMapTemplate;
  [SerializeField]
  public AssetReferenceGameObject _dlcWorldMapTemplate;
  [SerializeField]
  public AssetReferenceGameObject _upgradeTreeTemplate;
  [SerializeField]
  public AssetReferenceGameObject _dlcUpgradeTreeTemplate;
  [SerializeField]
  public AssetReferenceGameObject _upgradePlayerTreeTemplate;
  [SerializeField]
  public AssetReferenceGameObject _cookingFireTemplate;
  [SerializeField]
  public AssetReferenceGameObject _kitchenMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _coopAssignMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerSummaryMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _playerUpgradesMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _cultUpgradesMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _altarMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerSelectMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _jobBoardMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ranchMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ranchAssignMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ranchSelectMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ranchMatingMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _exhumeSpiritsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _shareHouseMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _deadFollowerSelectMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _doctrineMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _doctrineChoicesMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ritualsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _sermonWheelTemplate;
  [SerializeField]
  public AssetReferenceGameObject _demonSummonMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _demonMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _executionerMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _prisonMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _prisonerMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _missionaryMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _missionMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _matingProgressMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _refineryMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tutorialMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _buildMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerFormsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _appearanceMenuFormTemplate;
  [SerializeField]
  public AssetReferenceGameObject _appearanceMenuColourTemplate;
  [SerializeField]
  public AssetReferenceGameObject _appearanceMenuVariantTemplate;
  [SerializeField]
  public AssetReferenceGameObject _appearanceMenuOutfitTemplate;
  [SerializeField]
  public AssetReferenceGameObject _matingMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _sinMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _traitSelectorTemplate;
  [SerializeField]
  public AssetReferenceGameObject _daycareMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _customizeClothesColourTemplate;
  [SerializeField]
  public AssetReferenceGameObject _videoExportMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _sacrificeFollowerMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _newGameOptionsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerKitchenMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _pubMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tailorMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _relicMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _photoGalleryMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _editPhotoMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _sandboxMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _cryptMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _achievementsMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _comicMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _logisticsMenuControllerTemplate;
  [SerializeField]
  public AssetReferenceGameObject _flockadePiecesMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _weaponsChangeNameMenuTemplate;
  [Header("Overlays")]
  [SerializeField]
  public AssetReferenceGameObject _bugReportingOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _keyScreenOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tutorialOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _recipeConfirmationOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _itemSelectorTemplate;
  [SerializeField]
  public AssetReferenceGameObject _difficultySelectorTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tarotPickUpOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tarotChoiceOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fleeceTarotRewardOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerInteractionWheelTemplate;
  [SerializeField]
  public AssetReferenceGameObject _newItemOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _cultNameMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _mysticSellerNameMenuTemplate;
  [SerializeField]
  public AssetReferenceGameObject _deathScreenOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _adventureMapOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fishingOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _cookingMinigameOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _tailorMinigameOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _drumMinigameOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _rebuildBedMinigameOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _buildSnowmanMinigameOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _loadingOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _bindingOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _dropdownOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _takePhotoOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _mysticShopOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _sermonXPOverlayTemplate;
  [SerializeField]
  public AssetReferenceGameObject _traitManipulatorTemplate;
  [SerializeField]
  public AssetReferenceGameObject _traitManipulatorResultTemplate;
  [SerializeField]
  public AssetReferenceGameObject _traitManipulatorInProgressTemplate;
  [Header("Knucklebones")]
  [SerializeField]
  public AssetReferenceGameObject _knucklebonesTemplate;
  [SerializeField]
  public AssetReferenceGameObject _knucklebonesBettingSelectorTemplate;
  [SerializeField]
  public AssetReferenceGameObject _knucklebonesOpponentSelectorTemplate;
  [Header("Flockade")]
  [SerializeField]
  public AssetReferenceGameObject _flockadeTemplate;
  [SerializeField]
  public AssetReferenceGameObject _flockadeOpponentWindowTemplate;
  [SerializeField]
  public AssetReferenceGameObject _flockadeBettingWindowTemplate;
  [Header("Other")]
  [SerializeField]
  public AssetReferenceGameObject _inventoryPromptTemplate;
  [SerializeField]
  public AssetReferenceGameObject _relicPickupPromptTemplate;
  [SerializeField]
  public AssetReferenceGameObject _weaponPickupPromptTemplate;
  [Header("Pooling")]
  [SerializeField]
  public AssetReferenceGameObject _followerInformationBoxTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ranchInformationBoxTemplate;
  [SerializeField]
  public AssetReferenceGameObject _ranchMatingInformationBoxTemplate;
  [SerializeField]
  public AssetReferenceGameObject _missionaryFollowerItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _matingFollowerItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerColourItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerFormItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerVariantItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerFormNecklaceTemplate;
  [SerializeField]
  public AssetReferenceGameObject _followerFormOutfitTemplate;
  [SerializeField]
  public AssetReferenceGameObject _adventureMapNodeTemplate;
  [SerializeField]
  public AssetReferenceGameObject _demonFollowerItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _deadFollowerItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _relicItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _photoItemTemplate;
  [SerializeField]
  public AssetReferenceGameObject _flockadePieceItemTemplate;
  [Header("Twitch")]
  [SerializeField]
  public AssetReferenceGameObject _twitchFollowerSelect;
  [SerializeField]
  public AssetReferenceGameObject _twitchTotemWheel;
  [SerializeField]
  public AssetReferenceGameObject _twitchSettingsTemplate;
  [SerializeField]
  public AssetReferenceGameObject _twitchInformationBoxTemplate;
  [SerializeField]
  public AssetReferenceGameObject _twitchVotingOverlayTemplate;
  [Header("Farm Plots")]
  [SerializeField]
  public AssetReferenceGameObject _wateredGameObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _unWateredGameObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _wateredRotGameObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _unWateredRotGameObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fertilizedObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fertilizedGoldObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fertilizedGlowObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fertilizedRainbowObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fertilizedDevotionObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _fertilizedRotstoneObjectTemplate;
  [SerializeField]
  public AssetReferenceGameObject _frozenGroundObjectTemplate;
  [CompilerGenerated]
  public UIPauseMenuController \u003CPauseMenuController\u003Ek__BackingField;
  [CompilerGenerated]
  public UICreditsMenuController \u003CCreditsMenuControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIPauseDetailsMenuController \u003CPauseDetailsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public FollowerInformationBox \u003CFollowerInformationBoxTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public RanchMenuItem \u003CRanchInformationBoxTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public RanchMatingMenuItem \u003CRanchMatingInformationBoxTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerIndoctrinationMenuController \u003CIndoctrinationMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICustomizeClothingMenuController \u003CCustomiseCothingMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIUpgradeTreeMenuController \u003CUpgradeTreeMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDLCUpgradeTreeMenuController \u003CDLCUpgradeTreeMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITraitManipulatorMenuController \u003CTraitManipulatorMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITraitManipulatorResultsScreen \u003CTraitManipulatorResultMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITraitManipulatorInProgressMenuController \u003CTraitManipulatorInProgressMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIUpgradePlayerTreeMenuController \u003CUpgradePlayerTreeMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIWorldMapMenuController \u003CWorldMapTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDLCMapMenuController \u003CDLCWorldMapTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITarotCardsMenuController \u003CTarotCardsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICookingFireMenuController \u003CCookingFireMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIKitchenMenuController \u003CKitchenMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICoopAssignController \u003CCoopAssignMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITutorialOverlayController \u003CTutorialOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIKeyScreenOverlayController \u003CKeyScreenTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAltarMenuController \u003CAltarMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIPlayerUpgradesMenuController \u003CPlayerUpgradesMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIReapSoulsMenuController \u003CReapSoulsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICultUpgradesMenuController \u003CCultUpgradesMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIItemSelectorOverlayController \u003CItemSelectorOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRecipeConfirmationOverlayController \u003CRecipeConfirmationTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDifficultySelectorOverlayController \u003CDifficultySelectorTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIComicMenuController \u003CComicMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIInventoryPromptOverlay \u003CInventoryPromptTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerInteractionWheelOverlayController \u003CFollowerInteractionWheelTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITarotPickUpOverlayController \u003CTarotPickUpOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITarotChoiceOverlayController \u003CTarotChoiceOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFleeceTarotRewardOverlayController \u003CFleeceTarotChoiceOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerSummaryMenuController \u003CFollowerSummaryMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerSelectMenuController \u003CFollowerSelectMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIJobBoardMenuController \u003CJobBoardMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRanchMenuController \u003CRanchMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRanchAssignMenuController \u003CRanchAssignMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRanchSelectMenuController \u003CRanchSelectMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRanchMatingMenu \u003CRanchMatingMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIExhumeSpiritsMenuController \u003CExhumeSpriritMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDeadFollowerSelectMenu \u003CDeadFollowerSelectMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDoctrineMenuController \u003CDoctrineMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDoctrineChoicesMenuController \u003CDoctrineChoicesMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRitualsMenuController \u003CRitualsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UISermonWheelController \u003CSermonWheelTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDemonSummonMenuController \u003CDemonSummonTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDemonMenuController \u003CDemonMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIPrisonMenuController \u003CPrisonMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIPrisonerMenuController \u003CPrisonerMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIMissionaryMenuController \u003CMissionaryMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIMissionMenuController \u003CMissionMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIMatingProgressMenuController \u003CMatingProgressMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICultNameMenuController \u003CCultNameMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIMysticSellerNameMenuController \u003CMysticSellerNameMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeUIController \u003CFlockadeTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeBettingSelectionUIController \u003CFlockadeBettingSelectionTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeOpponentSelectionUIController \u003CFlockadeOpponentSelectionTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIKnuckleBonesController \u003CKnucklebonesTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIKnucklebonesBettingSelectionController \u003CKnucklebonesBettingSelectionTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIKnucklebonesOpponentSelectionController \u003CKnucklebonesOpponentSelectionTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRefineryMenuController \u003CRefineryMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITutorialMenuController \u003CTutorialMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDeathScreenOverlayController \u003CDeathScreenOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIBuildMenuController \u003CBuildMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerFormsMenuController \u003CFollowerFormsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UINewItemOverlayController \u003CNewItemOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAppearanceMenuController_Form \u003CAppearanceMenuFormTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAppearanceMenuController_Colour \u003CAppearanceMenuColourTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAppearanceMenuController_Variant \u003CAppearanceMenuVariantTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAppearanceMenuController_Outfit \u003CAppearanceMenuOutfitTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIMatingMenuController \u003CMatingMenuControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITraitSelector \u003CTraitSelectorTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICustomizeClothesMenuController_Colour \u003CCustomizeClothesColourTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public IndoctrinationColourItem \u003CFollowerColourItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public IndoctrinationFormItem \u003CFollowerFormItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public IndoctrinationVariantItem \u003CFollowerVariantItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public necklaceFormItem \u003CFollowerFormNecklaceTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public IndoctrinationOutfitItem \u003CFollowerFormOutfitTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAdventureMapOverlayController \u003CAdventureMapOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public AdventureMapNode \u003CAdventureMapNodeTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRoadmapOverlayController \u003CRoadmapOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIVideoExportMenuController \u003CVideoExportTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFishingOverlayController \u003CFishingOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICookingMinigameOverlayController \u003CCookingMinigameOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIExtinguishMiniGameOverlay \u003CExtinguishMinigameOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITailorMinigameOverlayController \u003CTailorMinigameOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDrumMinigameOverlayController \u003CDrumMinigameOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRebuildBedMinigameOverlayController \u003CRebuildBedMinigameOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIBuildSnowmanMinigameOverlayController \u003CBuildSnowmanMinigameOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public MissionaryFollowerItem \u003CMissionaryFollowerItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public MissionaryFollowerItem \u003CMatingFollowerItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public DemonFollowerItem \u003CDemonFollowerItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public DeadFollowerInformationBox \u003CDeadFollowerInformationBox\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerKitchenMenuController \u003CFollowerKitchenMenuControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIPubMenuController \u003CPubMenuControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITailorMenuController \u003CTailorMenuControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITwitchFollowerSelectOverlayController \u003CTwitchFollowerSelectOverlayController\u003Ek__BackingField;
  [CompilerGenerated]
  public UITwitchTotemWheel \u003CTwitchTotemWheelController\u003Ek__BackingField;
  [CompilerGenerated]
  public UITwitchSettingsMenuController \u003CTwitchSettingsMenuController\u003Ek__BackingField;
  [CompilerGenerated]
  public TwitchInformationBox \u003CTwitchInformationBox\u003Ek__BackingField;
  [CompilerGenerated]
  public UITwitchFollowerVotingOverlayController \u003CTwitchFollowerVotingOverlayController\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFollowerSelectMenuController \u003CSacrificeFollowerMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UILoadingOverlayController \u003CLoadingOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIBugReportingOverlayController \u003CBugReportingOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIControlBindingOverlayController \u003CBindingOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRelicPickupPromptController \u003CRelicPickupPromptControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIWeaponPickupPromptController \u003CWeaponPickPromptControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDropdownOverlayController \u003CDropdownOverlayControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UINewGameOptionsMenuController \u003CNewGameOptionsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CXboxControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CPS4ControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CPS5ControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CSwitchJoyConsTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CSwitchJoyConsDockedTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CSwitchHandheldTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CSwitchProControllerTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CKeyboardTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public InputController \u003CMouseTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIRelicMenuController \u003CRelicMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public RelicItem \u003CRelicItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIFlockadePiecesMenuController \u003CFlockadePiecesMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadePieceItem \u003CFlockadePieceItemTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIWeaponsNameChangeMenu \u003CWeaponsNameChangeMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UITakePhotoOverlayController \u003CTakePhotoOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIPhotoGalleryMenuController \u003CPhotoGalleryMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIEditPhotoOverlayController \u003CEditPhotoMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public PhotoInformationBox \u003CPhotoInformationBoxTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UISandboxMenuController \u003CSandboxMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIShareHouseMenuController \u003CShareHouseMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UICryptMenuController \u003CCryptMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UILogisticsMenuController \u003CLogisticsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIMysticShopOverlayController \u003CMysticShopOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIAchievementsMenuController \u003CAchievementsMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UISermonXPOverlayController \u003CSermonXPOverlayTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public UIDaycareMenu \u003CDaycareMenuTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CWateredGameObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CUnWateredGameObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CWateredRotGameObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CUnWateredRotGameObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFertilizedObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFertilizedGoldObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFertilizedGlowObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFertilizedRainbowObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFertilizedDevotionObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFertilizedRotstoneObjectTemplate\u003Ek__BackingField;
  [CompilerGenerated]
  public GameObject \u003CFrozenGroundObjectTemplate\u003Ek__BackingField;
  public const string kAssetPath = "Assets/UI/Prefabs/UIManager.prefab";
  public bool ForceBlockMenus;
  public bool ForceBlockPause;
  [CompilerGenerated]
  public bool \u003CIsPaused\u003Ek__BackingField;
  public bool ForceDisableSaving;
  public UIMenuBase _currentInstance;
  public float _previousClipPlane;
  public Camera _currentMain;
  public Texture2D _currentCursorTexture;
  public Resolution _currentResolution;
  public int _cursorSize;
  public Dictionary<GameObject, AsyncOperationHandle> _addressablesTracker = new Dictionary<GameObject, AsyncOperationHandle>();
  public static bool isUIManagerLoaded = false;
  public int currentCursor;
  public bool _cursorIconChanged;
  public int _previousCursor = -1;
  public static ConcurrentQueue<System.Action> _mainThreadActions = new ConcurrentQueue<System.Action>();

  public UIPauseMenuController PauseMenuController
  {
    set => this.\u003CPauseMenuController\u003Ek__BackingField = value;
    get => this.\u003CPauseMenuController\u003Ek__BackingField;
  }

  public UISettingsMenuController SettingsMenuControllerTemplate => this._settingsMenuTemplate;

  public UICreditsMenuController CreditsMenuControllerTemplate
  {
    set => this.\u003CCreditsMenuControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CCreditsMenuControllerTemplate\u003Ek__BackingField;
  }

  public UIPauseDetailsMenuController PauseDetailsMenuTemplate
  {
    set => this.\u003CPauseDetailsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CPauseDetailsMenuTemplate\u003Ek__BackingField;
  }

  public FollowerInformationBox FollowerInformationBoxTemplate
  {
    set => this.\u003CFollowerInformationBoxTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerInformationBoxTemplate\u003Ek__BackingField;
  }

  public RanchMenuItem RanchInformationBoxTemplate
  {
    set => this.\u003CRanchInformationBoxTemplate\u003Ek__BackingField = value;
    get => this.\u003CRanchInformationBoxTemplate\u003Ek__BackingField;
  }

  public RanchMatingMenuItem RanchMatingInformationBoxTemplate
  {
    set => this.\u003CRanchMatingInformationBoxTemplate\u003Ek__BackingField = value;
    get => this.\u003CRanchMatingInformationBoxTemplate\u003Ek__BackingField;
  }

  public UIFollowerIndoctrinationMenuController IndoctrinationMenuTemplate
  {
    set => this.\u003CIndoctrinationMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CIndoctrinationMenuTemplate\u003Ek__BackingField;
  }

  public UICustomizeClothingMenuController CustomiseCothingMenuTemplate
  {
    set => this.\u003CCustomiseCothingMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CCustomiseCothingMenuTemplate\u003Ek__BackingField;
  }

  public UIUpgradeTreeMenuController UpgradeTreeMenuTemplate
  {
    set => this.\u003CUpgradeTreeMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CUpgradeTreeMenuTemplate\u003Ek__BackingField;
  }

  public UIDLCUpgradeTreeMenuController DLCUpgradeTreeMenuTemplate
  {
    set => this.\u003CDLCUpgradeTreeMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CDLCUpgradeTreeMenuTemplate\u003Ek__BackingField;
  }

  public UITraitManipulatorMenuController TraitManipulatorMenuTemplate
  {
    set => this.\u003CTraitManipulatorMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CTraitManipulatorMenuTemplate\u003Ek__BackingField;
  }

  public UITraitManipulatorResultsScreen TraitManipulatorResultMenuTemplate
  {
    set => this.\u003CTraitManipulatorResultMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CTraitManipulatorResultMenuTemplate\u003Ek__BackingField;
  }

  public UITraitManipulatorInProgressMenuController TraitManipulatorInProgressMenuTemplate
  {
    set => this.\u003CTraitManipulatorInProgressMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CTraitManipulatorInProgressMenuTemplate\u003Ek__BackingField;
  }

  public UIUpgradePlayerTreeMenuController UpgradePlayerTreeMenuTemplate
  {
    set => this.\u003CUpgradePlayerTreeMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CUpgradePlayerTreeMenuTemplate\u003Ek__BackingField;
  }

  public UIWorldMapMenuController WorldMapTemplate
  {
    set => this.\u003CWorldMapTemplate\u003Ek__BackingField = value;
    get => this.\u003CWorldMapTemplate\u003Ek__BackingField;
  }

  public UIDLCMapMenuController DLCWorldMapTemplate
  {
    set => this.\u003CDLCWorldMapTemplate\u003Ek__BackingField = value;
    get => this.\u003CDLCWorldMapTemplate\u003Ek__BackingField;
  }

  public UITarotCardsMenuController TarotCardsMenuTemplate
  {
    set => this.\u003CTarotCardsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CTarotCardsMenuTemplate\u003Ek__BackingField;
  }

  public UICookingFireMenuController CookingFireMenuTemplate
  {
    set => this.\u003CCookingFireMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CCookingFireMenuTemplate\u003Ek__BackingField;
  }

  public UIKitchenMenuController KitchenMenuTemplate
  {
    set => this.\u003CKitchenMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CKitchenMenuTemplate\u003Ek__BackingField;
  }

  public UICoopAssignController CoopAssignMenuTemplate
  {
    set => this.\u003CCoopAssignMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CCoopAssignMenuTemplate\u003Ek__BackingField;
  }

  public UITutorialOverlayController TutorialOverlayTemplate
  {
    set => this.\u003CTutorialOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CTutorialOverlayTemplate\u003Ek__BackingField;
  }

  public UIKeyScreenOverlayController KeyScreenTemplate
  {
    set => this.\u003CKeyScreenTemplate\u003Ek__BackingField = value;
    get => this.\u003CKeyScreenTemplate\u003Ek__BackingField;
  }

  public UIAltarMenuController AltarMenuTemplate
  {
    set => this.\u003CAltarMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CAltarMenuTemplate\u003Ek__BackingField;
  }

  public UIPlayerUpgradesMenuController PlayerUpgradesMenuTemplate
  {
    set => this.\u003CPlayerUpgradesMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CPlayerUpgradesMenuTemplate\u003Ek__BackingField;
  }

  public UIReapSoulsMenuController ReapSoulsMenuTemplate
  {
    set => this.\u003CReapSoulsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CReapSoulsMenuTemplate\u003Ek__BackingField;
  }

  public UICultUpgradesMenuController CultUpgradesMenuTemplate
  {
    set => this.\u003CCultUpgradesMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CCultUpgradesMenuTemplate\u003Ek__BackingField;
  }

  public UIItemSelectorOverlayController ItemSelectorOverlayTemplate
  {
    set => this.\u003CItemSelectorOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CItemSelectorOverlayTemplate\u003Ek__BackingField;
  }

  public UIRecipeConfirmationOverlayController RecipeConfirmationTemplate
  {
    set => this.\u003CRecipeConfirmationTemplate\u003Ek__BackingField = value;
    get => this.\u003CRecipeConfirmationTemplate\u003Ek__BackingField;
  }

  public UIDifficultySelectorOverlayController DifficultySelectorTemplate
  {
    set => this.\u003CDifficultySelectorTemplate\u003Ek__BackingField = value;
    get => this.\u003CDifficultySelectorTemplate\u003Ek__BackingField;
  }

  public UIComicMenuController ComicMenuTemplate
  {
    set => this.\u003CComicMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CComicMenuTemplate\u003Ek__BackingField;
  }

  public UIInventoryPromptOverlay InventoryPromptTemplate
  {
    set => this.\u003CInventoryPromptTemplate\u003Ek__BackingField = value;
    get => this.\u003CInventoryPromptTemplate\u003Ek__BackingField;
  }

  public UIFollowerInteractionWheelOverlayController FollowerInteractionWheelTemplate
  {
    set => this.\u003CFollowerInteractionWheelTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerInteractionWheelTemplate\u003Ek__BackingField;
  }

  public UITarotPickUpOverlayController TarotPickUpOverlayTemplate
  {
    set => this.\u003CTarotPickUpOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CTarotPickUpOverlayTemplate\u003Ek__BackingField;
  }

  public UITarotChoiceOverlayController TarotChoiceOverlayTemplate
  {
    set => this.\u003CTarotChoiceOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CTarotChoiceOverlayTemplate\u003Ek__BackingField;
  }

  public UIFleeceTarotRewardOverlayController FleeceTarotChoiceOverlayTemplate
  {
    set => this.\u003CFleeceTarotChoiceOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CFleeceTarotChoiceOverlayTemplate\u003Ek__BackingField;
  }

  public UIFollowerSummaryMenuController FollowerSummaryMenuTemplate
  {
    set => this.\u003CFollowerSummaryMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerSummaryMenuTemplate\u003Ek__BackingField;
  }

  public UIFollowerSelectMenuController FollowerSelectMenuTemplate
  {
    set => this.\u003CFollowerSelectMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerSelectMenuTemplate\u003Ek__BackingField;
  }

  public UIJobBoardMenuController JobBoardMenuTemplate
  {
    set => this.\u003CJobBoardMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CJobBoardMenuTemplate\u003Ek__BackingField;
  }

  public UIRanchMenuController RanchMenuTemplate
  {
    set => this.\u003CRanchMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRanchMenuTemplate\u003Ek__BackingField;
  }

  public UIRanchAssignMenuController RanchAssignMenuTemplate
  {
    set => this.\u003CRanchAssignMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRanchAssignMenuTemplate\u003Ek__BackingField;
  }

  public UIRanchSelectMenuController RanchSelectMenuTemplate
  {
    set => this.\u003CRanchSelectMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRanchSelectMenuTemplate\u003Ek__BackingField;
  }

  public UIRanchMatingMenu RanchMatingMenuTemplate
  {
    set => this.\u003CRanchMatingMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRanchMatingMenuTemplate\u003Ek__BackingField;
  }

  public UIExhumeSpiritsMenuController ExhumeSpriritMenuTemplate
  {
    set => this.\u003CExhumeSpriritMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CExhumeSpriritMenuTemplate\u003Ek__BackingField;
  }

  public UIDeadFollowerSelectMenu DeadFollowerSelectMenuTemplate
  {
    set => this.\u003CDeadFollowerSelectMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CDeadFollowerSelectMenuTemplate\u003Ek__BackingField;
  }

  public UIDoctrineMenuController DoctrineMenuTemplate
  {
    set => this.\u003CDoctrineMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CDoctrineMenuTemplate\u003Ek__BackingField;
  }

  public UIDoctrineChoicesMenuController DoctrineChoicesMenuTemplate
  {
    set => this.\u003CDoctrineChoicesMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CDoctrineChoicesMenuTemplate\u003Ek__BackingField;
  }

  public UIRitualsMenuController RitualsMenuTemplate
  {
    set => this.\u003CRitualsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRitualsMenuTemplate\u003Ek__BackingField;
  }

  public UISermonWheelController SermonWheelTemplate
  {
    set => this.\u003CSermonWheelTemplate\u003Ek__BackingField = value;
    get => this.\u003CSermonWheelTemplate\u003Ek__BackingField;
  }

  public UIDemonSummonMenuController DemonSummonTemplate
  {
    set => this.\u003CDemonSummonTemplate\u003Ek__BackingField = value;
    get => this.\u003CDemonSummonTemplate\u003Ek__BackingField;
  }

  public UIDemonMenuController DemonMenuTemplate
  {
    set => this.\u003CDemonMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CDemonMenuTemplate\u003Ek__BackingField;
  }

  public UIPrisonMenuController PrisonMenuTemplate
  {
    set => this.\u003CPrisonMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CPrisonMenuTemplate\u003Ek__BackingField;
  }

  public UIPrisonerMenuController PrisonerMenuTemplate
  {
    set => this.\u003CPrisonerMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CPrisonerMenuTemplate\u003Ek__BackingField;
  }

  public UIMissionaryMenuController MissionaryMenuTemplate
  {
    set => this.\u003CMissionaryMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CMissionaryMenuTemplate\u003Ek__BackingField;
  }

  public UIMissionMenuController MissionMenuTemplate
  {
    set => this.\u003CMissionMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CMissionMenuTemplate\u003Ek__BackingField;
  }

  public UIMatingProgressMenuController MatingProgressMenuTemplate
  {
    set => this.\u003CMatingProgressMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CMatingProgressMenuTemplate\u003Ek__BackingField;
  }

  public UICultNameMenuController CultNameMenuTemplate
  {
    set => this.\u003CCultNameMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CCultNameMenuTemplate\u003Ek__BackingField;
  }

  public UIMysticSellerNameMenuController MysticSellerNameMenuTemplate
  {
    set => this.\u003CMysticSellerNameMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CMysticSellerNameMenuTemplate\u003Ek__BackingField;
  }

  public FlockadeUIController FlockadeTemplate
  {
    set => this.\u003CFlockadeTemplate\u003Ek__BackingField = value;
    get => this.\u003CFlockadeTemplate\u003Ek__BackingField;
  }

  public FlockadeBettingSelectionUIController FlockadeBettingSelectionTemplate
  {
    set => this.\u003CFlockadeBettingSelectionTemplate\u003Ek__BackingField = value;
    get => this.\u003CFlockadeBettingSelectionTemplate\u003Ek__BackingField;
  }

  public FlockadeOpponentSelectionUIController FlockadeOpponentSelectionTemplate
  {
    set => this.\u003CFlockadeOpponentSelectionTemplate\u003Ek__BackingField = value;
    get => this.\u003CFlockadeOpponentSelectionTemplate\u003Ek__BackingField;
  }

  public UIKnuckleBonesController KnucklebonesTemplate
  {
    set => this.\u003CKnucklebonesTemplate\u003Ek__BackingField = value;
    get => this.\u003CKnucklebonesTemplate\u003Ek__BackingField;
  }

  public UIKnucklebonesBettingSelectionController KnucklebonesBettingSelectionTemplate
  {
    set => this.\u003CKnucklebonesBettingSelectionTemplate\u003Ek__BackingField = value;
    get => this.\u003CKnucklebonesBettingSelectionTemplate\u003Ek__BackingField;
  }

  public UIKnucklebonesOpponentSelectionController KnucklebonesOpponentSelectionTemplate
  {
    set => this.\u003CKnucklebonesOpponentSelectionTemplate\u003Ek__BackingField = value;
    get => this.\u003CKnucklebonesOpponentSelectionTemplate\u003Ek__BackingField;
  }

  public UIRefineryMenuController RefineryMenuTemplate
  {
    set => this.\u003CRefineryMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRefineryMenuTemplate\u003Ek__BackingField;
  }

  public UITutorialMenuController TutorialMenuTemplate
  {
    set => this.\u003CTutorialMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CTutorialMenuTemplate\u003Ek__BackingField;
  }

  public UIMenuConfirmationWindow ConfirmationWindowTemplate => this._confirmationWindowTemplate;

  public UIConfirmationCountdownWindow ConfirmationCountdownTemplate
  {
    get => this._confirmationCountdownWindowTemplate;
  }

  public UIDeathScreenOverlayController DeathScreenOverlayTemplate
  {
    set => this.\u003CDeathScreenOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CDeathScreenOverlayTemplate\u003Ek__BackingField;
  }

  public UIBuildMenuController BuildMenuTemplate
  {
    set => this.\u003CBuildMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CBuildMenuTemplate\u003Ek__BackingField;
  }

  public UIFollowerFormsMenuController FollowerFormsMenuTemplate
  {
    set => this.\u003CFollowerFormsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerFormsMenuTemplate\u003Ek__BackingField;
  }

  public UINewItemOverlayController NewItemOverlayTemplate
  {
    set => this.\u003CNewItemOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CNewItemOverlayTemplate\u003Ek__BackingField;
  }

  public UIAppearanceMenuController_Form AppearanceMenuFormTemplate
  {
    set => this.\u003CAppearanceMenuFormTemplate\u003Ek__BackingField = value;
    get => this.\u003CAppearanceMenuFormTemplate\u003Ek__BackingField;
  }

  public UIAppearanceMenuController_Colour AppearanceMenuColourTemplate
  {
    set => this.\u003CAppearanceMenuColourTemplate\u003Ek__BackingField = value;
    get => this.\u003CAppearanceMenuColourTemplate\u003Ek__BackingField;
  }

  public UIAppearanceMenuController_Variant AppearanceMenuVariantTemplate
  {
    set => this.\u003CAppearanceMenuVariantTemplate\u003Ek__BackingField = value;
    get => this.\u003CAppearanceMenuVariantTemplate\u003Ek__BackingField;
  }

  public UIAppearanceMenuController_Outfit AppearanceMenuOutfitTemplate
  {
    set => this.\u003CAppearanceMenuOutfitTemplate\u003Ek__BackingField = value;
    get => this.\u003CAppearanceMenuOutfitTemplate\u003Ek__BackingField;
  }

  public UIMatingMenuController MatingMenuControllerTemplate
  {
    set => this.\u003CMatingMenuControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CMatingMenuControllerTemplate\u003Ek__BackingField;
  }

  public UITraitSelector TraitSelectorTemplate
  {
    set => this.\u003CTraitSelectorTemplate\u003Ek__BackingField = value;
    get => this.\u003CTraitSelectorTemplate\u003Ek__BackingField;
  }

  public UICustomizeClothesMenuController_Colour CustomizeClothesColourTemplate
  {
    set => this.\u003CCustomizeClothesColourTemplate\u003Ek__BackingField = value;
    get => this.\u003CCustomizeClothesColourTemplate\u003Ek__BackingField;
  }

  public IndoctrinationColourItem FollowerColourItemTemplate
  {
    set => this.\u003CFollowerColourItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerColourItemTemplate\u003Ek__BackingField;
  }

  public IndoctrinationFormItem FollowerFormItemTemplate
  {
    set => this.\u003CFollowerFormItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerFormItemTemplate\u003Ek__BackingField;
  }

  public IndoctrinationVariantItem FollowerVariantItemTemplate
  {
    set => this.\u003CFollowerVariantItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerVariantItemTemplate\u003Ek__BackingField;
  }

  public necklaceFormItem FollowerFormNecklaceTemplate
  {
    set => this.\u003CFollowerFormNecklaceTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerFormNecklaceTemplate\u003Ek__BackingField;
  }

  public IndoctrinationOutfitItem FollowerFormOutfitTemplate
  {
    set => this.\u003CFollowerFormOutfitTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerFormOutfitTemplate\u003Ek__BackingField;
  }

  public UIAdventureMapOverlayController AdventureMapOverlayTemplate
  {
    set => this.\u003CAdventureMapOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CAdventureMapOverlayTemplate\u003Ek__BackingField;
  }

  public AdventureMapNode AdventureMapNodeTemplate
  {
    set => this.\u003CAdventureMapNodeTemplate\u003Ek__BackingField = value;
    get => this.\u003CAdventureMapNodeTemplate\u003Ek__BackingField;
  }

  public UIRoadmapOverlayController RoadmapOverlayControllerTemplate
  {
    set => this.\u003CRoadmapOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CRoadmapOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIVideoExportMenuController VideoExportTemplate
  {
    set => this.\u003CVideoExportTemplate\u003Ek__BackingField = value;
    get => this.\u003CVideoExportTemplate\u003Ek__BackingField;
  }

  public UIFishingOverlayController FishingOverlayControllerTemplate
  {
    set => this.\u003CFishingOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CFishingOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UICookingMinigameOverlayController CookingMinigameOverlayControllerTemplate
  {
    set => this.\u003CCookingMinigameOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CCookingMinigameOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIExtinguishMiniGameOverlay ExtinguishMinigameOverlayControllerTemplate
  {
    set => this.\u003CExtinguishMinigameOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CExtinguishMinigameOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UITailorMinigameOverlayController TailorMinigameOverlayControllerTemplate
  {
    set => this.\u003CTailorMinigameOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CTailorMinigameOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIDrumMinigameOverlayController DrumMinigameOverlayControllerTemplate
  {
    set => this.\u003CDrumMinigameOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CDrumMinigameOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIRebuildBedMinigameOverlayController RebuildBedMinigameOverlayControllerTemplate
  {
    set => this.\u003CRebuildBedMinigameOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CRebuildBedMinigameOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIBuildSnowmanMinigameOverlayController BuildSnowmanMinigameOverlayControllerTemplate
  {
    set => this.\u003CBuildSnowmanMinigameOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CBuildSnowmanMinigameOverlayControllerTemplate\u003Ek__BackingField;
  }

  public MissionaryFollowerItem MissionaryFollowerItemTemplate
  {
    set => this.\u003CMissionaryFollowerItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CMissionaryFollowerItemTemplate\u003Ek__BackingField;
  }

  public MissionaryFollowerItem MatingFollowerItemTemplate
  {
    set => this.\u003CMatingFollowerItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CMatingFollowerItemTemplate\u003Ek__BackingField;
  }

  public DemonFollowerItem DemonFollowerItemTemplate
  {
    set => this.\u003CDemonFollowerItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CDemonFollowerItemTemplate\u003Ek__BackingField;
  }

  public DeadFollowerInformationBox DeadFollowerInformationBox
  {
    set => this.\u003CDeadFollowerInformationBox\u003Ek__BackingField = value;
    get => this.\u003CDeadFollowerInformationBox\u003Ek__BackingField;
  }

  public UIFollowerKitchenMenuController FollowerKitchenMenuControllerTemplate
  {
    set => this.\u003CFollowerKitchenMenuControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CFollowerKitchenMenuControllerTemplate\u003Ek__BackingField;
  }

  public UIPubMenuController PubMenuControllerTemplate
  {
    set => this.\u003CPubMenuControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CPubMenuControllerTemplate\u003Ek__BackingField;
  }

  public UITailorMenuController TailorMenuControllerTemplate
  {
    set => this.\u003CTailorMenuControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CTailorMenuControllerTemplate\u003Ek__BackingField;
  }

  public UITwitchFollowerSelectOverlayController TwitchFollowerSelectOverlayController
  {
    set => this.\u003CTwitchFollowerSelectOverlayController\u003Ek__BackingField = value;
    get => this.\u003CTwitchFollowerSelectOverlayController\u003Ek__BackingField;
  }

  public UITwitchTotemWheel TwitchTotemWheelController
  {
    set => this.\u003CTwitchTotemWheelController\u003Ek__BackingField = value;
    get => this.\u003CTwitchTotemWheelController\u003Ek__BackingField;
  }

  public UITwitchSettingsMenuController TwitchSettingsMenuController
  {
    set => this.\u003CTwitchSettingsMenuController\u003Ek__BackingField = value;
    get => this.\u003CTwitchSettingsMenuController\u003Ek__BackingField;
  }

  public TwitchInformationBox TwitchInformationBox
  {
    set => this.\u003CTwitchInformationBox\u003Ek__BackingField = value;
    get => this.\u003CTwitchInformationBox\u003Ek__BackingField;
  }

  public UITwitchFollowerVotingOverlayController TwitchFollowerVotingOverlayController
  {
    set => this.\u003CTwitchFollowerVotingOverlayController\u003Ek__BackingField = value;
    get => this.\u003CTwitchFollowerVotingOverlayController\u003Ek__BackingField;
  }

  public UIFollowerSelectMenuController SacrificeFollowerMenuTemplate
  {
    set => this.\u003CSacrificeFollowerMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CSacrificeFollowerMenuTemplate\u003Ek__BackingField;
  }

  public UILoadingOverlayController LoadingOverlayControllerTemplate
  {
    set => this.\u003CLoadingOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CLoadingOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIBugReportingOverlayController BugReportingOverlayTemplate
  {
    set => this.\u003CBugReportingOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CBugReportingOverlayTemplate\u003Ek__BackingField;
  }

  public UIControlBindingOverlayController BindingOverlayControllerTemplate
  {
    set => this.\u003CBindingOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CBindingOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UIRelicPickupPromptController RelicPickupPromptControllerTemplate
  {
    set => this.\u003CRelicPickupPromptControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CRelicPickupPromptControllerTemplate\u003Ek__BackingField;
  }

  public UIWeaponPickupPromptController WeaponPickPromptControllerTemplate
  {
    set => this.\u003CWeaponPickPromptControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CWeaponPickPromptControllerTemplate\u003Ek__BackingField;
  }

  public UIDropdownOverlayController DropdownOverlayControllerTemplate
  {
    set => this.\u003CDropdownOverlayControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CDropdownOverlayControllerTemplate\u003Ek__BackingField;
  }

  public UINewGameOptionsMenuController NewGameOptionsMenuTemplate
  {
    set => this.\u003CNewGameOptionsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CNewGameOptionsMenuTemplate\u003Ek__BackingField;
  }

  public InputController XboxControllerTemplate
  {
    set => this.\u003CXboxControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CXboxControllerTemplate\u003Ek__BackingField;
  }

  public InputController PS4ControllerTemplate
  {
    set => this.\u003CPS4ControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CPS4ControllerTemplate\u003Ek__BackingField;
  }

  public InputController PS5ControllerTemplate
  {
    set => this.\u003CPS5ControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CPS5ControllerTemplate\u003Ek__BackingField;
  }

  public InputController SwitchJoyConsTemplate
  {
    set => this.\u003CSwitchJoyConsTemplate\u003Ek__BackingField = value;
    get => this.\u003CSwitchJoyConsTemplate\u003Ek__BackingField;
  }

  public InputController SwitchJoyConsDockedTemplate
  {
    set => this.\u003CSwitchJoyConsDockedTemplate\u003Ek__BackingField = value;
    get => this.\u003CSwitchJoyConsDockedTemplate\u003Ek__BackingField;
  }

  public InputController SwitchHandheldTemplate
  {
    set => this.\u003CSwitchHandheldTemplate\u003Ek__BackingField = value;
    get => this.\u003CSwitchHandheldTemplate\u003Ek__BackingField;
  }

  public InputController SwitchProControllerTemplate
  {
    set => this.\u003CSwitchProControllerTemplate\u003Ek__BackingField = value;
    get => this.\u003CSwitchProControllerTemplate\u003Ek__BackingField;
  }

  public InputController KeyboardTemplate
  {
    set => this.\u003CKeyboardTemplate\u003Ek__BackingField = value;
    get => this.\u003CKeyboardTemplate\u003Ek__BackingField;
  }

  public InputController MouseTemplate
  {
    set => this.\u003CMouseTemplate\u003Ek__BackingField = value;
    get => this.\u003CMouseTemplate\u003Ek__BackingField;
  }

  public UIRelicMenuController RelicMenuTemplate
  {
    set => this.\u003CRelicMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CRelicMenuTemplate\u003Ek__BackingField;
  }

  public RelicItem RelicItemTemplate
  {
    set => this.\u003CRelicItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CRelicItemTemplate\u003Ek__BackingField;
  }

  public UIFlockadePiecesMenuController FlockadePiecesMenuTemplate
  {
    set => this.\u003CFlockadePiecesMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CFlockadePiecesMenuTemplate\u003Ek__BackingField;
  }

  public FlockadePieceItem FlockadePieceItemTemplate
  {
    set => this.\u003CFlockadePieceItemTemplate\u003Ek__BackingField = value;
    get => this.\u003CFlockadePieceItemTemplate\u003Ek__BackingField;
  }

  public UIWeaponsNameChangeMenu WeaponsNameChangeMenuTemplate
  {
    set => this.\u003CWeaponsNameChangeMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CWeaponsNameChangeMenuTemplate\u003Ek__BackingField;
  }

  public UITakePhotoOverlayController TakePhotoOverlayTemplate
  {
    set => this.\u003CTakePhotoOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CTakePhotoOverlayTemplate\u003Ek__BackingField;
  }

  public UIPhotoGalleryMenuController PhotoGalleryMenuTemplate
  {
    set => this.\u003CPhotoGalleryMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CPhotoGalleryMenuTemplate\u003Ek__BackingField;
  }

  public UIEditPhotoOverlayController EditPhotoMenuTemplate
  {
    set => this.\u003CEditPhotoMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CEditPhotoMenuTemplate\u003Ek__BackingField;
  }

  public PhotoInformationBox PhotoInformationBoxTemplate
  {
    set => this.\u003CPhotoInformationBoxTemplate\u003Ek__BackingField = value;
    get => this.\u003CPhotoInformationBoxTemplate\u003Ek__BackingField;
  }

  public UISandboxMenuController SandboxMenuTemplate
  {
    set => this.\u003CSandboxMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CSandboxMenuTemplate\u003Ek__BackingField;
  }

  public UIShareHouseMenuController ShareHouseMenuTemplate
  {
    set => this.\u003CShareHouseMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CShareHouseMenuTemplate\u003Ek__BackingField;
  }

  public UICryptMenuController CryptMenuTemplate
  {
    set => this.\u003CCryptMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CCryptMenuTemplate\u003Ek__BackingField;
  }

  public UILogisticsMenuController LogisticsMenuTemplate
  {
    set => this.\u003CLogisticsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CLogisticsMenuTemplate\u003Ek__BackingField;
  }

  public UIMysticShopOverlayController MysticShopOverlayTemplate
  {
    set => this.\u003CMysticShopOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CMysticShopOverlayTemplate\u003Ek__BackingField;
  }

  public UIAchievementsMenuController AchievementsMenuTemplate
  {
    set => this.\u003CAchievementsMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CAchievementsMenuTemplate\u003Ek__BackingField;
  }

  public UISermonXPOverlayController SermonXPOverlayTemplate
  {
    set => this.\u003CSermonXPOverlayTemplate\u003Ek__BackingField = value;
    get => this.\u003CSermonXPOverlayTemplate\u003Ek__BackingField;
  }

  public UIDaycareMenu DaycareMenuTemplate
  {
    set => this.\u003CDaycareMenuTemplate\u003Ek__BackingField = value;
    get => this.\u003CDaycareMenuTemplate\u003Ek__BackingField;
  }

  public GameObject WateredGameObjectTemplate
  {
    set => this.\u003CWateredGameObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CWateredGameObjectTemplate\u003Ek__BackingField;
  }

  public GameObject UnWateredGameObjectTemplate
  {
    set => this.\u003CUnWateredGameObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CUnWateredGameObjectTemplate\u003Ek__BackingField;
  }

  public GameObject WateredRotGameObjectTemplate
  {
    set => this.\u003CWateredRotGameObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CWateredRotGameObjectTemplate\u003Ek__BackingField;
  }

  public GameObject UnWateredRotGameObjectTemplate
  {
    set => this.\u003CUnWateredRotGameObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CUnWateredRotGameObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FertilizedObjectTemplate
  {
    set => this.\u003CFertilizedObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFertilizedObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FertilizedGoldObjectTemplate
  {
    set => this.\u003CFertilizedGoldObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFertilizedGoldObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FertilizedGlowObjectTemplate
  {
    set => this.\u003CFertilizedGlowObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFertilizedGlowObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FertilizedRainbowObjectTemplate
  {
    set => this.\u003CFertilizedRainbowObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFertilizedRainbowObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FertilizedDevotionObjectTemplate
  {
    set => this.\u003CFertilizedDevotionObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFertilizedDevotionObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FertilizedRotstoneObjectTemplate
  {
    set => this.\u003CFertilizedRotstoneObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFertilizedRotstoneObjectTemplate\u003Ek__BackingField;
  }

  public GameObject FrozenGroundObjectTemplate
  {
    set => this.\u003CFrozenGroundObjectTemplate\u003Ek__BackingField = value;
    get => this.\u003CFrozenGroundObjectTemplate\u003Ek__BackingField;
  }

  public bool MenusBlocked
  {
    get
    {
      return this.ForceBlockMenus || MMTransition.IsPlaying || (double) Time.timeScale < 0.25 || GameManager.InMenu || UIMenuBase.ActiveMenus.Count > 0;
    }
  }

  public bool IsPaused
  {
    get => this.\u003CIsPaused\u003Ek__BackingField;
    set => this.\u003CIsPaused\u003Ek__BackingField = value;
  }

  public static bool IsUIManagerLoaded => UIManager.isUIManagerLoaded;

  public UIMenuBase CurrentMenu => this._currentInstance;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void LoadUIManager()
  {
    Addressables.LoadAssetAsync<GameObject>((object) "Assets/UI/Prefabs/UIManager.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (asyncOperation =>
    {
      asyncOperation.Result.GetComponent<UIManager>().Instantiate<UIManager>();
      UIManager.isUIManagerLoaded = true;
      Debug.Log((object) "UI Manager Instantiated!".Colour(Color.cyan));
    });
  }

  public override void Awake()
  {
    base.Awake();
    this.LoadLifecyclePersistentAssets();
  }

  public async System.Threading.Tasks.Task LoadMainMenuAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    Task<UICreditsMenuController> creditsMenuTask = UIManager.LoadAsset<UICreditsMenuController>(this._creditsMenuControllerTemplate);
    Task<UIRoadmapOverlayController> roadmapOverlayTask = UIManager.LoadAsset<UIRoadmapOverlayController>(this._roadmapOverlayControllerTemplate);
    Task<UIAchievementsMenuController> achievementsMenuTask = UIManager.LoadAsset<UIAchievementsMenuController>(this._achievementsMenuTemplate);
    Task<UILoadingOverlayController> loadingOverlayTask = UIManager.LoadAsset<UILoadingOverlayController>(this._loadingOverlayTemplate);
    Task<UINewGameOptionsMenuController> newGameOptionsTask = UIManager.LoadAsset<UINewGameOptionsMenuController>(this._newGameOptionsMenuTemplate);
    Task<UIDifficultySelectorOverlayController> difficultySelectorTask = UIManager.LoadAsset<UIDifficultySelectorOverlayController>(this._difficultySelectorTemplate);
    Task<UIComicMenuController> comicMenuTask = UIManager.LoadAsset<UIComicMenuController>(this._comicMenuControllerTemplate);
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) new List<System.Threading.Tasks.Task>()
    {
      (System.Threading.Tasks.Task) creditsMenuTask,
      (System.Threading.Tasks.Task) roadmapOverlayTask,
      (System.Threading.Tasks.Task) achievementsMenuTask,
      (System.Threading.Tasks.Task) loadingOverlayTask,
      (System.Threading.Tasks.Task) newGameOptionsTask,
      (System.Threading.Tasks.Task) difficultySelectorTask,
      (System.Threading.Tasks.Task) comicMenuTask
    });
    this.CreditsMenuControllerTemplate = creditsMenuTask.Result;
    this.RoadmapOverlayControllerTemplate = roadmapOverlayTask.Result;
    this.AchievementsMenuTemplate = achievementsMenuTask.Result;
    this.LoadingOverlayControllerTemplate = loadingOverlayTask.Result;
    this.NewGameOptionsMenuTemplate = newGameOptionsTask.Result;
    this.DifficultySelectorTemplate = difficultySelectorTask.Result;
    this.ComicMenuTemplate = comicMenuTask.Result;
    Application.backgroundLoadingPriority = ThreadPriority.Normal;
    creditsMenuTask = (Task<UICreditsMenuController>) null;
    roadmapOverlayTask = (Task<UIRoadmapOverlayController>) null;
    achievementsMenuTask = (Task<UIAchievementsMenuController>) null;
    loadingOverlayTask = (Task<UILoadingOverlayController>) null;
    newGameOptionsTask = (Task<UINewGameOptionsMenuController>) null;
    difficultySelectorTask = (Task<UIDifficultySelectorOverlayController>) null;
    comicMenuTask = (Task<UIComicMenuController>) null;
  }

  public void UnloadMainMenuAssets()
  {
    UIManager.UnloadAsset<UICreditsMenuController>(this.CreditsMenuControllerTemplate);
    UIManager.UnloadAsset<UIRoadmapOverlayController>(this.RoadmapOverlayControllerTemplate);
    UIManager.UnloadAsset<UIAchievementsMenuController>(this.AchievementsMenuTemplate);
    UIManager.UnloadAsset<UILoadingOverlayController>(this.LoadingOverlayControllerTemplate);
    UIManager.UnloadAsset<UINewGameOptionsMenuController>(this.NewGameOptionsMenuTemplate);
    UIManager.UnloadAsset<UIDifficultySelectorOverlayController>(this.DifficultySelectorTemplate);
    UIManager.UnloadAsset<UIComicMenuController>(this.ComicMenuTemplate);
  }

  public void UnlockRatauShackAssets()
  {
    UIManager.UnloadAsset<UIExhumeSpiritsMenuController>(this.ExhumeSpriritMenuTemplate);
    this.UnloadKnucklebonesAssets();
  }

  public async System.Threading.Tasks.Task LoadRatauShackAssets()
  {
    this.ExhumeSpriritMenuTemplate = await UIManager.LoadAsset<UIExhumeSpiritsMenuController>(this._exhumeSpiritsMenuTemplate);
    await this.LoadKnucklebonesAssets();
  }

  public async System.Threading.Tasks.Task LoadKnucklebonesAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    Task<UIKnuckleBonesController> knucklebonesTask = UIManager.LoadAsset<UIKnuckleBonesController>(this._knucklebonesTemplate);
    Task<UIKnucklebonesBettingSelectionController> bettingSelectionTask = UIManager.LoadAsset<UIKnucklebonesBettingSelectionController>(this._knucklebonesBettingSelectorTemplate);
    Task<UIKnucklebonesOpponentSelectionController> opponentSelectionTask = UIManager.LoadAsset<UIKnucklebonesOpponentSelectionController>(this._knucklebonesOpponentSelectorTemplate);
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) new List<System.Threading.Tasks.Task>()
    {
      (System.Threading.Tasks.Task) knucklebonesTask,
      (System.Threading.Tasks.Task) bettingSelectionTask,
      (System.Threading.Tasks.Task) opponentSelectionTask
    });
    this.KnucklebonesTemplate = knucklebonesTask.Result;
    this.KnucklebonesBettingSelectionTemplate = bettingSelectionTask.Result;
    this.KnucklebonesOpponentSelectionTemplate = opponentSelectionTask.Result;
    Application.backgroundLoadingPriority = ThreadPriority.Normal;
    knucklebonesTask = (Task<UIKnuckleBonesController>) null;
    bettingSelectionTask = (Task<UIKnucklebonesBettingSelectionController>) null;
    opponentSelectionTask = (Task<UIKnucklebonesOpponentSelectionController>) null;
  }

  public void UnloadKnucklebonesAssets()
  {
    UIManager.UnloadAsset<UIKnuckleBonesController>(this.KnucklebonesTemplate);
    UIManager.UnloadAsset<UIKnucklebonesBettingSelectionController>(this.KnucklebonesBettingSelectionTemplate);
    UIManager.UnloadAsset<UIKnucklebonesOpponentSelectionController>(this.KnucklebonesOpponentSelectionTemplate);
  }

  public async System.Threading.Tasks.Task LoadFlockadePiecesAssets()
  {
    Task<UIFlockadePiecesMenuController> flockadePiecesMenuTask = UIManager.LoadAsset<UIFlockadePiecesMenuController>(this._flockadePiecesMenuTemplate);
    Task<FlockadePieceItem> flockadePieceItemTask = UIManager.LoadAsset<FlockadePieceItem>(this._flockadePieceItemTemplate);
    await System.Threading.Tasks.Task.WhenAll((System.Threading.Tasks.Task) flockadePiecesMenuTask, (System.Threading.Tasks.Task) flockadePieceItemTask);
    this.FlockadePiecesMenuTemplate = flockadePiecesMenuTask.Result;
    this.FlockadePieceItemTemplate = flockadePieceItemTask.Result;
    flockadePiecesMenuTask = (Task<UIFlockadePiecesMenuController>) null;
    flockadePieceItemTask = (Task<FlockadePieceItem>) null;
  }

  public async System.Threading.Tasks.Task LoadFlockadeAssets()
  {
    Task<FlockadeUIController> flocadeTemplate = UIManager.LoadAsset<FlockadeUIController>(this._flockadeTemplate);
    Task<FlockadeOpponentSelectionUIController> opponentWindowTask = UIManager.LoadAsset<FlockadeOpponentSelectionUIController>(this._flockadeOpponentWindowTemplate);
    Task<FlockadeBettingSelectionUIController> bettingWindowTask = UIManager.LoadAsset<FlockadeBettingSelectionUIController>(this._flockadeBettingWindowTemplate);
    Task<UIFlockadePiecesMenuController> flockadePiecesMenuTask = UIManager.LoadAsset<UIFlockadePiecesMenuController>(this._flockadePiecesMenuTemplate);
    Task<FlockadePieceItem> flockadePieceItemTask = UIManager.LoadAsset<FlockadePieceItem>(this._flockadePieceItemTemplate);
    await System.Threading.Tasks.Task.WhenAll((System.Threading.Tasks.Task) flocadeTemplate, (System.Threading.Tasks.Task) opponentWindowTask, (System.Threading.Tasks.Task) bettingWindowTask, (System.Threading.Tasks.Task) flockadePiecesMenuTask, (System.Threading.Tasks.Task) flockadePieceItemTask);
    this.FlockadeTemplate = flocadeTemplate.Result;
    this.FlockadeOpponentSelectionTemplate = opponentWindowTask.Result;
    this.FlockadeBettingSelectionTemplate = bettingWindowTask.Result;
    this.FlockadePiecesMenuTemplate = flockadePiecesMenuTask.Result;
    this.FlockadePieceItemTemplate = flockadePieceItemTask.Result;
    flocadeTemplate = (Task<FlockadeUIController>) null;
    opponentWindowTask = (Task<FlockadeOpponentSelectionUIController>) null;
    bettingWindowTask = (Task<FlockadeBettingSelectionUIController>) null;
    flockadePiecesMenuTask = (Task<UIFlockadePiecesMenuController>) null;
    flockadePieceItemTask = (Task<FlockadePieceItem>) null;
  }

  public void UnloadFlockadePiecesAssets()
  {
    UIManager.UnloadAsset<UIFlockadePiecesMenuController>(this.FlockadePiecesMenuTemplate);
    UIManager.UnloadAsset<FlockadePieceItem>(this.FlockadePieceItemTemplate);
  }

  public void UnloadFlockadeAssets()
  {
    UIManager.UnloadAsset<FlockadeUIController>(this.FlockadeTemplate);
    UIManager.UnloadAsset<FlockadeOpponentSelectionUIController>(this.FlockadeOpponentSelectionTemplate);
    UIManager.UnloadAsset<FlockadeBettingSelectionUIController>(this.FlockadeBettingSelectionTemplate);
  }

  public async System.Threading.Tasks.Task LoadLifecyclePersistentAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    Task<UIDropdownOverlayController> dropdownOverlayTask = UIManager.LoadAsset<UIDropdownOverlayController>(this._dropdownOverlayTemplate);
    Task<UIControlBindingOverlayController> bindingOverlayTask = UIManager.LoadAsset<UIControlBindingOverlayController>(this._bindingOverlayTemplate);
    Task<InputController> keyboardTask = UIManager.LoadAsset<InputController>(this._keyboardTemplate);
    Task<InputController> mouseTask = UIManager.LoadAsset<InputController>(this._mouseTemplate);
    Task<InputController> xboxControllerTask = UIManager.LoadAsset<InputController>(this._xboxControllerTemplate);
    Task<InputController> ps4ControllerTask = UIManager.LoadAsset<InputController>(this._ps4ControllerTemplate);
    Task<InputController> ps5ControllerTask = UIManager.LoadAsset<InputController>(this._ps5ControllerTemplate);
    Task<InputController> switchProTask = UIManager.LoadAsset<InputController>(this._switchProControllerTemplate);
    Task<InputController> switchJoyConsTask = UIManager.LoadAsset<InputController>(this._switchJoyConsDetachedControllerTemplate);
    Task<InputController> switchJoyConsDockedTask = UIManager.LoadAsset<InputController>(this._switchJoyConsControllerTemplate);
    Task<InputController> switchHandheldTask = UIManager.LoadAsset<InputController>(this._switchHandheldControllerTemplate);
    List<System.Threading.Tasks.Task> taskList = new List<System.Threading.Tasks.Task>()
    {
      (System.Threading.Tasks.Task) dropdownOverlayTask,
      (System.Threading.Tasks.Task) bindingOverlayTask
    };
    taskList.AddRange((IEnumerable<System.Threading.Tasks.Task>) new System.Threading.Tasks.Task[9]
    {
      (System.Threading.Tasks.Task) keyboardTask,
      (System.Threading.Tasks.Task) mouseTask,
      (System.Threading.Tasks.Task) xboxControllerTask,
      (System.Threading.Tasks.Task) ps4ControllerTask,
      (System.Threading.Tasks.Task) ps5ControllerTask,
      (System.Threading.Tasks.Task) switchProTask,
      (System.Threading.Tasks.Task) switchJoyConsTask,
      (System.Threading.Tasks.Task) switchJoyConsDockedTask,
      (System.Threading.Tasks.Task) switchHandheldTask
    });
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) taskList);
    this.DropdownOverlayControllerTemplate = dropdownOverlayTask.Result;
    this.BindingOverlayControllerTemplate = bindingOverlayTask.Result;
    this.KeyboardTemplate = keyboardTask.Result;
    this.MouseTemplate = mouseTask.Result;
    this.XboxControllerTemplate = xboxControllerTask.Result;
    this.PS4ControllerTemplate = ps4ControllerTask.Result;
    this.PS5ControllerTemplate = ps5ControllerTask.Result;
    this.SwitchProControllerTemplate = switchProTask.Result;
    this.SwitchJoyConsTemplate = switchJoyConsTask.Result;
    this.SwitchJoyConsDockedTemplate = switchJoyConsDockedTask.Result;
    this.SwitchHandheldTemplate = switchHandheldTask.Result;
    Application.backgroundLoadingPriority = ThreadPriority.Normal;
    dropdownOverlayTask = (Task<UIDropdownOverlayController>) null;
    bindingOverlayTask = (Task<UIControlBindingOverlayController>) null;
    keyboardTask = (Task<InputController>) null;
    mouseTask = (Task<InputController>) null;
    xboxControllerTask = (Task<InputController>) null;
    ps4ControllerTask = (Task<InputController>) null;
    ps5ControllerTask = (Task<InputController>) null;
    switchProTask = (Task<InputController>) null;
    switchJoyConsTask = (Task<InputController>) null;
    switchJoyConsDockedTask = (Task<InputController>) null;
    switchHandheldTask = (Task<InputController>) null;
  }

  public async System.Threading.Tasks.Task LoadPhotomodeAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    Task<UITakePhotoOverlayController> takePhotoOverlayTask = UIManager.LoadAsset<UITakePhotoOverlayController>(this._takePhotoOverlayTemplate);
    Task<UIPhotoGalleryMenuController> photoGalleryMenuTask = UIManager.LoadAsset<UIPhotoGalleryMenuController>(this._photoGalleryMenuTemplate);
    Task<UIEditPhotoOverlayController> editPhotoMenuTask = UIManager.LoadAsset<UIEditPhotoOverlayController>(this._editPhotoMenuTemplate);
    Task<PhotoInformationBox> photoInfoBoxTask = UIManager.LoadAsset<PhotoInformationBox>(this._photoItemTemplate);
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) new List<System.Threading.Tasks.Task>()
    {
      (System.Threading.Tasks.Task) takePhotoOverlayTask,
      (System.Threading.Tasks.Task) photoGalleryMenuTask,
      (System.Threading.Tasks.Task) editPhotoMenuTask,
      (System.Threading.Tasks.Task) photoInfoBoxTask
    });
    this.TakePhotoOverlayTemplate = takePhotoOverlayTask.Result;
    this.PhotoGalleryMenuTemplate = photoGalleryMenuTask.Result;
    this.EditPhotoMenuTemplate = editPhotoMenuTask.Result;
    this.PhotoInformationBoxTemplate = photoInfoBoxTask.Result;
    takePhotoOverlayTask = (Task<UITakePhotoOverlayController>) null;
    photoGalleryMenuTask = (Task<UIPhotoGalleryMenuController>) null;
    editPhotoMenuTask = (Task<UIEditPhotoOverlayController>) null;
    photoInfoBoxTask = (Task<PhotoInformationBox>) null;
  }

  public void UnloadPhotomodeAssets()
  {
    UIManager.UnloadAsset<UITakePhotoOverlayController>(this.TakePhotoOverlayTemplate);
    UIManager.UnloadAsset<UIPhotoGalleryMenuController>(this.PhotoGalleryMenuTemplate);
    UIManager.UnloadAsset<UIEditPhotoOverlayController>(this.EditPhotoMenuTemplate);
    UIManager.UnloadAsset<PhotoInformationBox>(this.PhotoInformationBoxTemplate);
  }

  public async System.Threading.Tasks.Task LoadPersistentGameAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    Task<UIPauseMenuController> pauseMenuTask = UIManager.LoadAsset<UIPauseMenuController>(this._pauseMenuTemplate);
    Task<UIPauseDetailsMenuController> pauseDetailsMenuTask = UIManager.LoadAsset<UIPauseDetailsMenuController>(this._pauseDetailsMenuTemplate);
    Task<UITutorialOverlayController> tutorialOverlayTask = UIManager.LoadAsset<UITutorialOverlayController>(this._tutorialOverlayTemplate);
    Task<UITutorialMenuController> tutorialMenuTask = UIManager.LoadAsset<UITutorialMenuController>(this._tutorialMenuTemplate);
    Task<UIFollowerSummaryMenuController> followerSummaryMenuTask = UIManager.LoadAsset<UIFollowerSummaryMenuController>(this._followerSummaryMenuTemplate);
    Task<FollowerInformationBox> followerInfoBoxTask = UIManager.LoadAsset<FollowerInformationBox>(this._followerInformationBoxTemplate);
    Task<UIFollowerSelectMenuController> followerSelectMenuTask = UIManager.LoadAsset<UIFollowerSelectMenuController>(this._followerSelectMenuTemplate);
    Task<UIDeathScreenOverlayController> deathScreenOverlayTask = UIManager.LoadAsset<UIDeathScreenOverlayController>(this._deathScreenOverlayTemplate);
    Task<UITarotCardsMenuController> tarotCardsMenuTask = UIManager.LoadAsset<UITarotCardsMenuController>(this._tarotCardsMenuTemplate);
    Task<UIKeyScreenOverlayController> keyScreenTask = UIManager.LoadAsset<UIKeyScreenOverlayController>(this._keyScreenOverlayTemplate);
    Task<UIItemSelectorOverlayController> itemSelectorTask = UIManager.LoadAsset<UIItemSelectorOverlayController>(this._itemSelectorTemplate);
    Task<UINewItemOverlayController> newItemOverlayTask = UIManager.LoadAsset<UINewItemOverlayController>(this._newItemOverlayTemplate);
    Task<UIBuildMenuController> buildMenuTask = UIManager.LoadAsset<UIBuildMenuController>(this._buildMenuTemplate);
    Task<UICoopAssignController> coopAssignMenuTask = UIManager.LoadAsset<UICoopAssignController>(this._coopAssignMenuTemplate);
    Task<UIFollowerFormsMenuController> followerFormsMenuTask = UIManager.LoadAsset<UIFollowerFormsMenuController>(this._followerFormsMenuTemplate);
    Task<UIDoctrineChoicesMenuController> doctrineChoicesMenuTask = UIManager.LoadAsset<UIDoctrineChoicesMenuController>(this._doctrineChoicesMenuTemplate);
    Task<UIRelicMenuController> relicMenuTask = UIManager.LoadAsset<UIRelicMenuController>(this._relicMenuTemplate);
    Task<RelicItem> relicItemTask = UIManager.LoadAsset<RelicItem>(this._relicItemTemplate);
    Task<UIWeaponsNameChangeMenu> weaponChangeNameMenuTask = UIManager.LoadAsset<UIWeaponsNameChangeMenu>(this._weaponsChangeNameMenuTemplate);
    Task<UIBugReportingOverlayController> bugReportingOverlayTask = UIManager.LoadAsset<UIBugReportingOverlayController>(this._bugReportingOverlayTemplate);
    Task<UITwitchSettingsMenuController> twitchSettingsMenuTask = UIManager.LoadAsset<UITwitchSettingsMenuController>(this._twitchSettingsTemplate);
    Task<UITwitchFollowerVotingOverlayController> twitchVotingOverlayTask = UIManager.LoadAsset<UITwitchFollowerVotingOverlayController>(this._twitchVotingOverlayTemplate);
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) new List<System.Threading.Tasks.Task>()
    {
      (System.Threading.Tasks.Task) pauseMenuTask,
      (System.Threading.Tasks.Task) pauseDetailsMenuTask,
      (System.Threading.Tasks.Task) tutorialOverlayTask,
      (System.Threading.Tasks.Task) tutorialMenuTask,
      (System.Threading.Tasks.Task) followerSummaryMenuTask,
      (System.Threading.Tasks.Task) followerInfoBoxTask,
      (System.Threading.Tasks.Task) followerSelectMenuTask,
      (System.Threading.Tasks.Task) deathScreenOverlayTask,
      (System.Threading.Tasks.Task) tarotCardsMenuTask,
      (System.Threading.Tasks.Task) keyScreenTask,
      (System.Threading.Tasks.Task) itemSelectorTask,
      (System.Threading.Tasks.Task) newItemOverlayTask,
      (System.Threading.Tasks.Task) buildMenuTask,
      (System.Threading.Tasks.Task) coopAssignMenuTask,
      (System.Threading.Tasks.Task) followerFormsMenuTask,
      (System.Threading.Tasks.Task) doctrineChoicesMenuTask,
      (System.Threading.Tasks.Task) relicMenuTask,
      (System.Threading.Tasks.Task) relicItemTask,
      (System.Threading.Tasks.Task) weaponChangeNameMenuTask,
      (System.Threading.Tasks.Task) bugReportingOverlayTask,
      (System.Threading.Tasks.Task) twitchSettingsMenuTask,
      (System.Threading.Tasks.Task) twitchVotingOverlayTask
    });
    this.PauseMenuController = pauseMenuTask.Result;
    this.PauseDetailsMenuTemplate = pauseDetailsMenuTask.Result;
    this.TutorialOverlayTemplate = tutorialOverlayTask.Result;
    this.TutorialMenuTemplate = tutorialMenuTask.Result;
    this.FollowerSummaryMenuTemplate = followerSummaryMenuTask.Result;
    this.FollowerInformationBoxTemplate = followerInfoBoxTask.Result;
    this.FollowerSelectMenuTemplate = followerSelectMenuTask.Result;
    this.DeathScreenOverlayTemplate = deathScreenOverlayTask.Result;
    this.TarotCardsMenuTemplate = tarotCardsMenuTask.Result;
    this.KeyScreenTemplate = keyScreenTask.Result;
    this.ItemSelectorOverlayTemplate = itemSelectorTask.Result;
    this.NewItemOverlayTemplate = newItemOverlayTask.Result;
    this.BuildMenuTemplate = buildMenuTask.Result;
    this.CoopAssignMenuTemplate = coopAssignMenuTask.Result;
    this.FollowerFormsMenuTemplate = followerFormsMenuTask.Result;
    this.DoctrineChoicesMenuTemplate = doctrineChoicesMenuTask.Result;
    this.RelicMenuTemplate = relicMenuTask.Result;
    this.RelicItemTemplate = relicItemTask.Result;
    this.WeaponsNameChangeMenuTemplate = weaponChangeNameMenuTask.Result;
    this.BugReportingOverlayTemplate = bugReportingOverlayTask.Result;
    this.TwitchSettingsMenuController = twitchSettingsMenuTask.Result;
    this.TwitchFollowerVotingOverlayController = twitchVotingOverlayTask.Result;
    Application.backgroundLoadingPriority = ThreadPriority.Normal;
    pauseMenuTask = (Task<UIPauseMenuController>) null;
    pauseDetailsMenuTask = (Task<UIPauseDetailsMenuController>) null;
    tutorialOverlayTask = (Task<UITutorialOverlayController>) null;
    tutorialMenuTask = (Task<UITutorialMenuController>) null;
    followerSummaryMenuTask = (Task<UIFollowerSummaryMenuController>) null;
    followerInfoBoxTask = (Task<FollowerInformationBox>) null;
    followerSelectMenuTask = (Task<UIFollowerSelectMenuController>) null;
    deathScreenOverlayTask = (Task<UIDeathScreenOverlayController>) null;
    tarotCardsMenuTask = (Task<UITarotCardsMenuController>) null;
    keyScreenTask = (Task<UIKeyScreenOverlayController>) null;
    itemSelectorTask = (Task<UIItemSelectorOverlayController>) null;
    newItemOverlayTask = (Task<UINewItemOverlayController>) null;
    buildMenuTask = (Task<UIBuildMenuController>) null;
    coopAssignMenuTask = (Task<UICoopAssignController>) null;
    followerFormsMenuTask = (Task<UIFollowerFormsMenuController>) null;
    doctrineChoicesMenuTask = (Task<UIDoctrineChoicesMenuController>) null;
    relicMenuTask = (Task<UIRelicMenuController>) null;
    relicItemTask = (Task<RelicItem>) null;
    weaponChangeNameMenuTask = (Task<UIWeaponsNameChangeMenu>) null;
    bugReportingOverlayTask = (Task<UIBugReportingOverlayController>) null;
    twitchSettingsMenuTask = (Task<UITwitchSettingsMenuController>) null;
    twitchVotingOverlayTask = (Task<UITwitchFollowerVotingOverlayController>) null;
  }

  public void UnloadPersistentGameAssets()
  {
    UIManager.UnloadAsset<UIPauseMenuController>(this.PauseMenuController);
    UIManager.UnloadAsset<UIPauseDetailsMenuController>(this.PauseDetailsMenuTemplate);
    UIManager.UnloadAsset<UITutorialOverlayController>(this.TutorialOverlayTemplate);
    UIManager.UnloadAsset<UITutorialMenuController>(this.TutorialMenuTemplate);
    UIManager.UnloadAsset<UIFollowerSummaryMenuController>(this.FollowerSummaryMenuTemplate);
    UIManager.UnloadAsset<FollowerInformationBox>(this.FollowerInformationBoxTemplate);
    UIManager.UnloadAsset<UIFollowerSelectMenuController>(this.FollowerSelectMenuTemplate);
    UIManager.UnloadAsset<UIDeathScreenOverlayController>(this.DeathScreenOverlayTemplate);
    UIManager.UnloadAsset<UITarotCardsMenuController>(this.TarotCardsMenuTemplate);
    UIManager.UnloadAsset<UIKeyScreenOverlayController>(this.KeyScreenTemplate);
    UIManager.UnloadAsset<UIItemSelectorOverlayController>(this.ItemSelectorOverlayTemplate);
    UIManager.UnloadAsset<UINewItemOverlayController>(this.NewItemOverlayTemplate);
    UIManager.UnloadAsset<UITakePhotoOverlayController>(this.TakePhotoOverlayTemplate);
    UIManager.UnloadAsset<UIPhotoGalleryMenuController>(this.PhotoGalleryMenuTemplate);
    UIManager.UnloadAsset<UIEditPhotoOverlayController>(this.EditPhotoMenuTemplate);
    UIManager.UnloadAsset<PhotoInformationBox>(this.PhotoInformationBoxTemplate);
    UIManager.UnloadAsset<UIBuildMenuController>(this.BuildMenuTemplate);
    UIManager.UnloadAsset<UICoopAssignController>(this.CoopAssignMenuTemplate);
    UIManager.UnloadAsset<UIFollowerFormsMenuController>(this.FollowerFormsMenuTemplate);
    UIManager.UnloadAsset<UIDoctrineChoicesMenuController>(this.DoctrineChoicesMenuTemplate);
    UIManager.UnloadAsset<UIRelicMenuController>(this.RelicMenuTemplate);
    UIManager.UnloadAsset<RelicItem>(this.RelicItemTemplate);
    UIManager.UnloadAsset<UIWorldMapMenuController>(this.WorldMapTemplate);
    UIManager.UnloadAsset<UIDLCMapMenuController>(this.DLCWorldMapTemplate);
    UIManager.UnloadAsset<UIWeaponsNameChangeMenu>(this.WeaponsNameChangeMenuTemplate);
    UIManager.UnloadAsset<UIBugReportingOverlayController>(this.BugReportingOverlayTemplate);
    UIManager.UnloadAsset<UITwitchSettingsMenuController>(this.TwitchSettingsMenuController);
    UIManager.UnloadAsset<UITwitchFollowerVotingOverlayController>(this.TwitchFollowerVotingOverlayController);
  }

  public async System.Threading.Tasks.Task LoadDifficultySelector()
  {
    this.DifficultySelectorTemplate = await UIManager.LoadAsset<UIDifficultySelectorOverlayController>(this._difficultySelectorTemplate);
  }

  public async System.Threading.Tasks.Task LoadBaseAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    SeasonalEventData activeEvent = SeasonalEventManager.GetActiveEvent();
    if ((UnityEngine.Object) activeEvent != (UnityEngine.Object) null)
      await activeEvent.LoadUIAssets();
    List<System.Threading.Tasks.Task> taskList = new List<System.Threading.Tasks.Task>();
    Task<UIFollowerInteractionWheelOverlayController> followerWheelTask = UIManager.LoadAsset<UIFollowerInteractionWheelOverlayController>(this._followerInteractionWheelTemplate);
    Task<UIFollowerIndoctrinationMenuController> indoctrinationTask = UIManager.LoadAsset<UIFollowerIndoctrinationMenuController>(this._indoctrinationMenuTemplate);
    Task<UICustomizeClothingMenuController> customClothesTask = UIManager.LoadAsset<UICustomizeClothingMenuController>(this._customizeClothingMenuTemplate);
    Task<UIFollowerSelectMenuController> sacrificeTask = UIManager.LoadAsset<UIFollowerSelectMenuController>(this._sacrificeFollowerMenuTemplate);
    Task<UIUpgradeTreeMenuController> upgradeTreeTask = UIManager.LoadAsset<UIUpgradeTreeMenuController>(this._upgradeTreeTemplate);
    Task<UIUpgradePlayerTreeMenuController> upgradePlayerTreeTask = UIManager.LoadAsset<UIUpgradePlayerTreeMenuController>(this._upgradePlayerTreeTemplate);
    Task<UIKitchenMenuController> kitchenMenuTask = UIManager.LoadAsset<UIKitchenMenuController>(this._kitchenMenuTemplate);
    Task<UIPlayerUpgradesMenuController> playerUpgradesTask = UIManager.LoadAsset<UIPlayerUpgradesMenuController>(this._playerUpgradesMenuTemplate);
    Task<UICultUpgradesMenuController> cultUpgradesTask = UIManager.LoadAsset<UICultUpgradesMenuController>(this._cultUpgradesMenuTemplate);
    Task<UIAltarMenuController> altarMenuTask = UIManager.LoadAsset<UIAltarMenuController>(this._altarMenuTemplate);
    Task<UIDoctrineMenuController> doctrineMenuTask = UIManager.LoadAsset<UIDoctrineMenuController>(this._doctrineMenuTemplate);
    Task<UIDeadFollowerSelectMenu> deadFollowerTask = UIManager.LoadAsset<UIDeadFollowerSelectMenu>(this._deadFollowerSelectMenuTemplate);
    Task<UIRitualsMenuController> ritualsMenuTask = UIManager.LoadAsset<UIRitualsMenuController>(this._ritualsMenuTemplate);
    Task<UISermonWheelController> sermonWheelTask = UIManager.LoadAsset<UISermonWheelController>(this._sermonWheelTemplate);
    Task<UIDemonSummonMenuController> demonSummonTask = UIManager.LoadAsset<UIDemonSummonMenuController>(this._demonSummonMenuTemplate);
    Task<UIDemonMenuController> demonMenuTask = UIManager.LoadAsset<UIDemonMenuController>(this._demonMenuTemplate);
    Task<UIPrisonMenuController> prisonMenuTask = UIManager.LoadAsset<UIPrisonMenuController>(this._prisonMenuControllerTemplate);
    Task<UIPrisonerMenuController> prisonerMenuTask = UIManager.LoadAsset<UIPrisonerMenuController>(this._prisonerMenuControllerTemplate);
    Task<UIMissionaryMenuController> missionaryMenuTask = UIManager.LoadAsset<UIMissionaryMenuController>(this._missionaryMenuTemplate);
    Task<UIMissionMenuController> missionMenuTask = UIManager.LoadAsset<UIMissionMenuController>(this._missionMenuControllerTemplate);
    Task<UIMatingProgressMenuController> matingProgressTask = UIManager.LoadAsset<UIMatingProgressMenuController>(this._matingProgressMenuControllerTemplate);
    Task<UIRefineryMenuController> refineryMenuTask = UIManager.LoadAsset<UIRefineryMenuController>(this._refineryMenuControllerTemplate);
    Task<UIAppearanceMenuController_Colour> appearanceColourTask = UIManager.LoadAsset<UIAppearanceMenuController_Colour>(this._appearanceMenuColourTemplate);
    Task<UIAppearanceMenuController_Form> appearanceFormTask = UIManager.LoadAsset<UIAppearanceMenuController_Form>(this._appearanceMenuFormTemplate);
    Task<UIAppearanceMenuController_Variant> appearanceVariantTask = UIManager.LoadAsset<UIAppearanceMenuController_Variant>(this._appearanceMenuVariantTemplate);
    Task<UIAppearanceMenuController_Outfit> appearanceOutfitTask = UIManager.LoadAsset<UIAppearanceMenuController_Outfit>(this._appearanceMenuOutfitTemplate);
    Task<UITraitSelector> traitSelectorTask = UIManager.LoadAsset<UITraitSelector>(this._traitSelectorTemplate);
    Task<UICustomizeClothesMenuController_Colour> customizeClothesColorTask = UIManager.LoadAsset<UICustomizeClothesMenuController_Colour>(this._customizeClothesColourTemplate);
    Task<MissionaryFollowerItem> missionaryFollowerItemTask = UIManager.LoadAsset<MissionaryFollowerItem>(this._missionaryFollowerItemTemplate);
    Task<MissionaryFollowerItem> matingFollowerItemTask = UIManager.LoadAsset<MissionaryFollowerItem>(this._matingFollowerItemTemplate);
    Task<IndoctrinationColourItem> followerColourItemTask = UIManager.LoadAsset<IndoctrinationColourItem>(this._followerColourItemTemplate);
    Task<IndoctrinationFormItem> followerFormItemTask = UIManager.LoadAsset<IndoctrinationFormItem>(this._followerFormItemTemplate);
    Task<IndoctrinationVariantItem> followerVariantItemTask = UIManager.LoadAsset<IndoctrinationVariantItem>(this._followerVariantItemTemplate);
    Task<necklaceFormItem> followerNecklaceItemTask = UIManager.LoadAsset<necklaceFormItem>(this._followerFormNecklaceTemplate);
    Task<IndoctrinationOutfitItem> followerOutfitItemTask = UIManager.LoadAsset<IndoctrinationOutfitItem>(this._followerFormOutfitTemplate);
    Task<DemonFollowerItem> demonFollowerItemTask = UIManager.LoadAsset<DemonFollowerItem>(this._demonFollowerItemTemplate);
    Task<DeadFollowerInformationBox> deadFollowerInfoBoxTask = UIManager.LoadAsset<DeadFollowerInformationBox>(this._deadFollowerItemTemplate);
    Task<UIFollowerKitchenMenuController> followerKitchenMenuTask = UIManager.LoadAsset<UIFollowerKitchenMenuController>(this._followerKitchenMenuTemplate);
    Task<UIPubMenuController> pubMenuTask = UIManager.LoadAsset<UIPubMenuController>(this._pubMenuTemplate);
    Task<UITailorMenuController> tailorMenuTask = UIManager.LoadAsset<UITailorMenuController>(this._tailorMenuTemplate);
    Task<UIShareHouseMenuController> shareHouseMenuTask = UIManager.LoadAsset<UIShareHouseMenuController>(this._shareHouseMenuTemplate);
    Task<UICryptMenuController> cryptMenuTask = UIManager.LoadAsset<UICryptMenuController>(this._cryptMenuTemplate);
    Task<UILogisticsMenuController> logisticsMenuTask = UIManager.LoadAsset<UILogisticsMenuController>(this._logisticsMenuControllerTemplate);
    Task<UISermonXPOverlayController> sermonXPOverlayTask = UIManager.LoadAsset<UISermonXPOverlayController>(this._sermonXPOverlayTemplate);
    Task<UIDaycareMenu> daycareMenuTask = UIManager.LoadAsset<UIDaycareMenu>(this._daycareMenuTemplate);
    Task<RanchMenuItem> ranchInfoBoxTask = UIManager.LoadAsset<RanchMenuItem>(this._ranchInformationBoxTemplate);
    Task<RanchMatingMenuItem> ranchMatingInfoBoxTask = UIManager.LoadAsset<RanchMatingMenuItem>(this._ranchMatingInformationBoxTemplate);
    Task<UIRanchSelectMenuController> ranchSelectMenuTask = UIManager.LoadAsset<UIRanchSelectMenuController>(this._ranchSelectMenuTemplate);
    Task<UIRanchMatingMenu> ranchMatingMenuTask = UIManager.LoadAsset<UIRanchMatingMenu>(this._ranchMatingMenuTemplate);
    Task<UIJobBoardMenuController> jobBoardMenuTask = UIManager.LoadAsset<UIJobBoardMenuController>(this._jobBoardMenuTemplate);
    Task<UIRanchMenuController> ranchMenuTask = UIManager.LoadAsset<UIRanchMenuController>(this._ranchMenuTemplate);
    Task<UIRanchAssignMenuController> ranchAssignMenuTask = UIManager.LoadAsset<UIRanchAssignMenuController>(this._ranchAssignMenuTemplate);
    Task<UIWeaponPickupPromptController> weaponPickPromptTask = UIManager.LoadAsset<UIWeaponPickupPromptController>(this._weaponPickupPromptTemplate);
    Task<Transform> _wateredGameObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._wateredGameObjectTemplate);
    Task<Transform> _unWateredGameObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._unWateredGameObjectTemplate);
    Task<Transform> _wateredRotGameObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._wateredRotGameObjectTemplate);
    Task<Transform> _unWateredRotGameObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._unWateredRotGameObjectTemplate);
    Task<Transform> _fertilizedObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._fertilizedObjectTemplate);
    Task<Transform> _fertilizedGoldObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._fertilizedGoldObjectTemplate);
    Task<Transform> _fertilizedGlowObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._fertilizedGlowObjectTemplate);
    Task<Transform> _fertilizedRainbowObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._fertilizedRainbowObjectTemplate);
    Task<Transform> _fertilizedDevotionObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._fertilizedDevotionObjectTemplate);
    Task<Transform> _fertilizedRotstoneObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._fertilizedRotstoneObjectTemplate);
    Task<Transform> _frozenGroundObjectTask = UIManager.LoadGameObjectAsset<Transform>(this._frozenGroundObjectTemplate);
    taskList.AddRange((IEnumerable<System.Threading.Tasks.Task>) new System.Threading.Tasks.Task[64 /*0x40*/]
    {
      (System.Threading.Tasks.Task) followerWheelTask,
      (System.Threading.Tasks.Task) indoctrinationTask,
      (System.Threading.Tasks.Task) customClothesTask,
      (System.Threading.Tasks.Task) sacrificeTask,
      (System.Threading.Tasks.Task) upgradeTreeTask,
      (System.Threading.Tasks.Task) upgradePlayerTreeTask,
      (System.Threading.Tasks.Task) kitchenMenuTask,
      (System.Threading.Tasks.Task) playerUpgradesTask,
      (System.Threading.Tasks.Task) cultUpgradesTask,
      (System.Threading.Tasks.Task) altarMenuTask,
      (System.Threading.Tasks.Task) doctrineMenuTask,
      (System.Threading.Tasks.Task) deadFollowerTask,
      (System.Threading.Tasks.Task) ritualsMenuTask,
      (System.Threading.Tasks.Task) sermonWheelTask,
      (System.Threading.Tasks.Task) demonSummonTask,
      (System.Threading.Tasks.Task) demonMenuTask,
      (System.Threading.Tasks.Task) prisonMenuTask,
      (System.Threading.Tasks.Task) prisonerMenuTask,
      (System.Threading.Tasks.Task) missionaryMenuTask,
      (System.Threading.Tasks.Task) missionMenuTask,
      (System.Threading.Tasks.Task) matingProgressTask,
      (System.Threading.Tasks.Task) refineryMenuTask,
      (System.Threading.Tasks.Task) appearanceColourTask,
      (System.Threading.Tasks.Task) appearanceFormTask,
      (System.Threading.Tasks.Task) appearanceVariantTask,
      (System.Threading.Tasks.Task) appearanceOutfitTask,
      (System.Threading.Tasks.Task) traitSelectorTask,
      (System.Threading.Tasks.Task) customizeClothesColorTask,
      (System.Threading.Tasks.Task) missionaryFollowerItemTask,
      (System.Threading.Tasks.Task) matingFollowerItemTask,
      (System.Threading.Tasks.Task) followerColourItemTask,
      (System.Threading.Tasks.Task) followerFormItemTask,
      (System.Threading.Tasks.Task) followerVariantItemTask,
      (System.Threading.Tasks.Task) followerNecklaceItemTask,
      (System.Threading.Tasks.Task) followerOutfitItemTask,
      (System.Threading.Tasks.Task) demonFollowerItemTask,
      (System.Threading.Tasks.Task) deadFollowerInfoBoxTask,
      (System.Threading.Tasks.Task) followerKitchenMenuTask,
      (System.Threading.Tasks.Task) pubMenuTask,
      (System.Threading.Tasks.Task) tailorMenuTask,
      (System.Threading.Tasks.Task) shareHouseMenuTask,
      (System.Threading.Tasks.Task) cryptMenuTask,
      (System.Threading.Tasks.Task) logisticsMenuTask,
      (System.Threading.Tasks.Task) sermonXPOverlayTask,
      (System.Threading.Tasks.Task) daycareMenuTask,
      (System.Threading.Tasks.Task) ranchInfoBoxTask,
      (System.Threading.Tasks.Task) ranchMatingInfoBoxTask,
      (System.Threading.Tasks.Task) ranchSelectMenuTask,
      (System.Threading.Tasks.Task) ranchMatingMenuTask,
      (System.Threading.Tasks.Task) jobBoardMenuTask,
      (System.Threading.Tasks.Task) ranchMenuTask,
      (System.Threading.Tasks.Task) ranchAssignMenuTask,
      (System.Threading.Tasks.Task) weaponPickPromptTask,
      (System.Threading.Tasks.Task) _wateredGameObjectTask,
      (System.Threading.Tasks.Task) _unWateredGameObjectTask,
      (System.Threading.Tasks.Task) _wateredRotGameObjectTask,
      (System.Threading.Tasks.Task) _unWateredRotGameObjectTask,
      (System.Threading.Tasks.Task) _fertilizedObjectTask,
      (System.Threading.Tasks.Task) _fertilizedGoldObjectTask,
      (System.Threading.Tasks.Task) _fertilizedGlowObjectTask,
      (System.Threading.Tasks.Task) _fertilizedRainbowObjectTask,
      (System.Threading.Tasks.Task) _fertilizedDevotionObjectTask,
      (System.Threading.Tasks.Task) _fertilizedRotstoneObjectTask,
      (System.Threading.Tasks.Task) _frozenGroundObjectTask
    });
    Task<UIDLCUpgradeTreeMenuController> dlcUpgradeTreeTask = (Task<UIDLCUpgradeTreeMenuController>) null;
    Task<UITraitManipulatorMenuController> traitManipulatorTask = (Task<UITraitManipulatorMenuController>) null;
    Task<UITraitManipulatorResultsScreen> traitManipulatorResultTask = (Task<UITraitManipulatorResultsScreen>) null;
    Task<UITraitManipulatorInProgressMenuController> traitManipulatorInProgressTask = (Task<UITraitManipulatorInProgressMenuController>) null;
    dlcUpgradeTreeTask = UIManager.LoadAsset<UIDLCUpgradeTreeMenuController>(this._dlcUpgradeTreeTemplate);
    traitManipulatorTask = UIManager.LoadAsset<UITraitManipulatorMenuController>(this._traitManipulatorTemplate);
    traitManipulatorResultTask = UIManager.LoadAsset<UITraitManipulatorResultsScreen>(this._traitManipulatorResultTemplate);
    traitManipulatorInProgressTask = UIManager.LoadAsset<UITraitManipulatorInProgressMenuController>(this._traitManipulatorInProgressTemplate);
    taskList.Add((System.Threading.Tasks.Task) dlcUpgradeTreeTask);
    taskList.Add((System.Threading.Tasks.Task) traitManipulatorTask);
    taskList.Add((System.Threading.Tasks.Task) traitManipulatorResultTask);
    taskList.Add((System.Threading.Tasks.Task) traitManipulatorInProgressTask);
    Task<UITwitchFollowerSelectOverlayController> twitchFollowerOverlayTask = UIManager.LoadAsset<UITwitchFollowerSelectOverlayController>(this._twitchFollowerSelect);
    Task<UITwitchTotemWheel> twitchTotemWheelTask = UIManager.LoadAsset<UITwitchTotemWheel>(this._twitchTotemWheel);
    Task<TwitchInformationBox> twitchInfoBoxTask = UIManager.LoadAsset<TwitchInformationBox>(this._twitchInformationBoxTemplate);
    taskList.Add((System.Threading.Tasks.Task) twitchFollowerOverlayTask);
    taskList.Add((System.Threading.Tasks.Task) twitchTotemWheelTask);
    taskList.Add((System.Threading.Tasks.Task) twitchInfoBoxTask);
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) taskList);
    this.FollowerInteractionWheelTemplate = followerWheelTask.Result;
    this.IndoctrinationMenuTemplate = indoctrinationTask.Result;
    this.CustomiseCothingMenuTemplate = customClothesTask.Result;
    this.SacrificeFollowerMenuTemplate = sacrificeTask.Result;
    this.UpgradeTreeMenuTemplate = upgradeTreeTask.Result;
    this.UpgradePlayerTreeMenuTemplate = upgradePlayerTreeTask.Result;
    this.KitchenMenuTemplate = kitchenMenuTask.Result;
    this.PlayerUpgradesMenuTemplate = playerUpgradesTask.Result;
    this.CultUpgradesMenuTemplate = cultUpgradesTask.Result;
    this.AltarMenuTemplate = altarMenuTask.Result;
    this.DoctrineMenuTemplate = doctrineMenuTask.Result;
    this.DeadFollowerSelectMenuTemplate = deadFollowerTask.Result;
    this.RitualsMenuTemplate = ritualsMenuTask.Result;
    this.SermonWheelTemplate = sermonWheelTask.Result;
    this.DemonSummonTemplate = demonSummonTask.Result;
    this.DemonMenuTemplate = demonMenuTask.Result;
    this.PrisonMenuTemplate = prisonMenuTask.Result;
    this.PrisonerMenuTemplate = prisonerMenuTask.Result;
    this.MissionaryMenuTemplate = missionaryMenuTask.Result;
    this.MissionMenuTemplate = missionMenuTask.Result;
    this.MatingProgressMenuTemplate = matingProgressTask.Result;
    this.RefineryMenuTemplate = refineryMenuTask.Result;
    this.AppearanceMenuColourTemplate = appearanceColourTask.Result;
    this.AppearanceMenuFormTemplate = appearanceFormTask.Result;
    this.AppearanceMenuVariantTemplate = appearanceVariantTask.Result;
    this.AppearanceMenuOutfitTemplate = appearanceOutfitTask.Result;
    this.TraitSelectorTemplate = traitSelectorTask.Result;
    this.CustomizeClothesColourTemplate = customizeClothesColorTask.Result;
    this.MissionaryFollowerItemTemplate = missionaryFollowerItemTask.Result;
    this.MatingFollowerItemTemplate = matingFollowerItemTask.Result;
    this.FollowerColourItemTemplate = followerColourItemTask.Result;
    this.FollowerFormItemTemplate = followerFormItemTask.Result;
    this.FollowerVariantItemTemplate = followerVariantItemTask.Result;
    this.FollowerFormNecklaceTemplate = followerNecklaceItemTask.Result;
    this.FollowerFormOutfitTemplate = followerOutfitItemTask.Result;
    this.DemonFollowerItemTemplate = demonFollowerItemTask.Result;
    this.DeadFollowerInformationBox = deadFollowerInfoBoxTask.Result;
    this.FollowerKitchenMenuControllerTemplate = followerKitchenMenuTask.Result;
    this.PubMenuControllerTemplate = pubMenuTask.Result;
    this.TailorMenuControllerTemplate = tailorMenuTask.Result;
    this.ShareHouseMenuTemplate = shareHouseMenuTask.Result;
    this.CryptMenuTemplate = cryptMenuTask.Result;
    this.LogisticsMenuTemplate = logisticsMenuTask.Result;
    this.SermonXPOverlayTemplate = sermonXPOverlayTask.Result;
    this.DaycareMenuTemplate = daycareMenuTask.Result;
    this.JobBoardMenuTemplate = jobBoardMenuTask.Result;
    this.RanchInformationBoxTemplate = ranchInfoBoxTask.Result;
    this.RanchMatingInformationBoxTemplate = ranchMatingInfoBoxTask.Result;
    this.RanchSelectMenuTemplate = ranchSelectMenuTask.Result;
    this.RanchMatingMenuTemplate = ranchMatingMenuTask.Result;
    this.RanchMenuTemplate = ranchMenuTask.Result;
    this.RanchAssignMenuTemplate = ranchAssignMenuTask.Result;
    this.WeaponPickPromptControllerTemplate = weaponPickPromptTask.Result;
    this.DLCUpgradeTreeMenuTemplate = dlcUpgradeTreeTask.Result;
    this.TraitManipulatorMenuTemplate = traitManipulatorTask.Result;
    this.TraitManipulatorResultMenuTemplate = traitManipulatorResultTask.Result;
    this.TraitManipulatorInProgressMenuTemplate = traitManipulatorInProgressTask.Result;
    this.WateredGameObjectTemplate = _wateredGameObjectTask.Result.gameObject;
    this.UnWateredGameObjectTemplate = _unWateredGameObjectTask.Result.gameObject;
    this.WateredRotGameObjectTemplate = _wateredRotGameObjectTask.Result.gameObject;
    this.UnWateredRotGameObjectTemplate = _unWateredRotGameObjectTask.Result.gameObject;
    this.FertilizedObjectTemplate = _fertilizedObjectTask.Result.gameObject;
    this.FertilizedGoldObjectTemplate = _fertilizedGoldObjectTask.Result.gameObject;
    this.FertilizedGlowObjectTemplate = _fertilizedGlowObjectTask.Result.gameObject;
    this.FertilizedRainbowObjectTemplate = _fertilizedRainbowObjectTask.Result.gameObject;
    this.FertilizedDevotionObjectTemplate = _fertilizedDevotionObjectTask.Result.gameObject;
    this.FertilizedRotstoneObjectTemplate = _fertilizedRotstoneObjectTask.Result.gameObject;
    this.FrozenGroundObjectTemplate = _frozenGroundObjectTask.Result.gameObject;
    this.TwitchFollowerSelectOverlayController = twitchFollowerOverlayTask.Result;
    this.TwitchTotemWheelController = twitchTotemWheelTask.Result;
    this.TwitchInformationBox = twitchInfoBoxTask.Result;
    Application.backgroundLoadingPriority = ThreadPriority.Normal;
    followerWheelTask = (Task<UIFollowerInteractionWheelOverlayController>) null;
    indoctrinationTask = (Task<UIFollowerIndoctrinationMenuController>) null;
    customClothesTask = (Task<UICustomizeClothingMenuController>) null;
    sacrificeTask = (Task<UIFollowerSelectMenuController>) null;
    upgradeTreeTask = (Task<UIUpgradeTreeMenuController>) null;
    upgradePlayerTreeTask = (Task<UIUpgradePlayerTreeMenuController>) null;
    kitchenMenuTask = (Task<UIKitchenMenuController>) null;
    playerUpgradesTask = (Task<UIPlayerUpgradesMenuController>) null;
    cultUpgradesTask = (Task<UICultUpgradesMenuController>) null;
    altarMenuTask = (Task<UIAltarMenuController>) null;
    doctrineMenuTask = (Task<UIDoctrineMenuController>) null;
    deadFollowerTask = (Task<UIDeadFollowerSelectMenu>) null;
    ritualsMenuTask = (Task<UIRitualsMenuController>) null;
    sermonWheelTask = (Task<UISermonWheelController>) null;
    demonSummonTask = (Task<UIDemonSummonMenuController>) null;
    demonMenuTask = (Task<UIDemonMenuController>) null;
    prisonMenuTask = (Task<UIPrisonMenuController>) null;
    prisonerMenuTask = (Task<UIPrisonerMenuController>) null;
    missionaryMenuTask = (Task<UIMissionaryMenuController>) null;
    missionMenuTask = (Task<UIMissionMenuController>) null;
    matingProgressTask = (Task<UIMatingProgressMenuController>) null;
    refineryMenuTask = (Task<UIRefineryMenuController>) null;
    appearanceColourTask = (Task<UIAppearanceMenuController_Colour>) null;
    appearanceFormTask = (Task<UIAppearanceMenuController_Form>) null;
    appearanceVariantTask = (Task<UIAppearanceMenuController_Variant>) null;
    appearanceOutfitTask = (Task<UIAppearanceMenuController_Outfit>) null;
    traitSelectorTask = (Task<UITraitSelector>) null;
    customizeClothesColorTask = (Task<UICustomizeClothesMenuController_Colour>) null;
    missionaryFollowerItemTask = (Task<MissionaryFollowerItem>) null;
    matingFollowerItemTask = (Task<MissionaryFollowerItem>) null;
    followerColourItemTask = (Task<IndoctrinationColourItem>) null;
    followerFormItemTask = (Task<IndoctrinationFormItem>) null;
    followerVariantItemTask = (Task<IndoctrinationVariantItem>) null;
    followerNecklaceItemTask = (Task<necklaceFormItem>) null;
    followerOutfitItemTask = (Task<IndoctrinationOutfitItem>) null;
    demonFollowerItemTask = (Task<DemonFollowerItem>) null;
    deadFollowerInfoBoxTask = (Task<DeadFollowerInformationBox>) null;
    followerKitchenMenuTask = (Task<UIFollowerKitchenMenuController>) null;
    pubMenuTask = (Task<UIPubMenuController>) null;
    tailorMenuTask = (Task<UITailorMenuController>) null;
    shareHouseMenuTask = (Task<UIShareHouseMenuController>) null;
    cryptMenuTask = (Task<UICryptMenuController>) null;
    logisticsMenuTask = (Task<UILogisticsMenuController>) null;
    sermonXPOverlayTask = (Task<UISermonXPOverlayController>) null;
    daycareMenuTask = (Task<UIDaycareMenu>) null;
    ranchInfoBoxTask = (Task<RanchMenuItem>) null;
    ranchMatingInfoBoxTask = (Task<RanchMatingMenuItem>) null;
    ranchSelectMenuTask = (Task<UIRanchSelectMenuController>) null;
    ranchMatingMenuTask = (Task<UIRanchMatingMenu>) null;
    jobBoardMenuTask = (Task<UIJobBoardMenuController>) null;
    ranchMenuTask = (Task<UIRanchMenuController>) null;
    ranchAssignMenuTask = (Task<UIRanchAssignMenuController>) null;
    weaponPickPromptTask = (Task<UIWeaponPickupPromptController>) null;
    _wateredGameObjectTask = (Task<Transform>) null;
    _unWateredGameObjectTask = (Task<Transform>) null;
    _wateredRotGameObjectTask = (Task<Transform>) null;
    _unWateredRotGameObjectTask = (Task<Transform>) null;
    _fertilizedObjectTask = (Task<Transform>) null;
    _fertilizedGoldObjectTask = (Task<Transform>) null;
    _fertilizedGlowObjectTask = (Task<Transform>) null;
    _fertilizedRainbowObjectTask = (Task<Transform>) null;
    _fertilizedDevotionObjectTask = (Task<Transform>) null;
    _fertilizedRotstoneObjectTask = (Task<Transform>) null;
    _frozenGroundObjectTask = (Task<Transform>) null;
    dlcUpgradeTreeTask = (Task<UIDLCUpgradeTreeMenuController>) null;
    traitManipulatorTask = (Task<UITraitManipulatorMenuController>) null;
    traitManipulatorResultTask = (Task<UITraitManipulatorResultsScreen>) null;
    traitManipulatorInProgressTask = (Task<UITraitManipulatorInProgressMenuController>) null;
    twitchFollowerOverlayTask = (Task<UITwitchFollowerSelectOverlayController>) null;
    twitchTotemWheelTask = (Task<UITwitchTotemWheel>) null;
    twitchInfoBoxTask = (Task<TwitchInformationBox>) null;
  }

  public void UnloadBaseAssets()
  {
    Debug.Log((object) "Unload base assets".Colour(Color.red));
    SeasonalEventData activeEvent = SeasonalEventManager.GetActiveEvent();
    if ((UnityEngine.Object) activeEvent != (UnityEngine.Object) null)
      activeEvent.UnloadUIAssets();
    UIManager.UnloadAsset<UIFollowerIndoctrinationMenuController>(this.IndoctrinationMenuTemplate);
    UIManager.UnloadAsset<UICustomizeClothingMenuController>(this.CustomiseCothingMenuTemplate);
    UIManager.UnloadAsset<UIFollowerSelectMenuController>(this.SacrificeFollowerMenuTemplate);
    UIManager.UnloadAsset<UIUpgradeTreeMenuController>(this.UpgradeTreeMenuTemplate);
    UIManager.UnloadAsset<UIUpgradePlayerTreeMenuController>(this.UpgradePlayerTreeMenuTemplate);
    UIManager.UnloadAsset<UIKitchenMenuController>(this.KitchenMenuTemplate);
    UIManager.UnloadAsset<UIPlayerUpgradesMenuController>(this.PlayerUpgradesMenuTemplate);
    UIManager.UnloadAsset<UICultUpgradesMenuController>(this.CultUpgradesMenuTemplate);
    UIManager.UnloadAsset<UIAltarMenuController>(this.AltarMenuTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Colour>(this.AppearanceMenuColourTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Form>(this.AppearanceMenuFormTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Variant>(this.AppearanceMenuVariantTemplate);
    UIManager.UnloadAsset<UICultNameMenuController>(this.CultNameMenuTemplate);
    UIManager.UnloadAsset<UIMysticSellerNameMenuController>(this.MysticSellerNameMenuTemplate);
    UIManager.UnloadAsset<UIDoctrineMenuController>(this.DoctrineMenuTemplate);
    UIManager.UnloadAsset<UIRitualsMenuController>(this.RitualsMenuTemplate);
    UIManager.UnloadAsset<UISermonWheelController>(this.SermonWheelTemplate);
    UIManager.UnloadAsset<UIDemonSummonMenuController>(this.DemonSummonTemplate);
    UIManager.UnloadAsset<UIDemonMenuController>(this.DemonMenuTemplate);
    UIManager.UnloadAsset<UIPrisonMenuController>(this.PrisonMenuTemplate);
    UIManager.UnloadAsset<UIDeadFollowerSelectMenu>(this.DeadFollowerSelectMenuTemplate);
    UIManager.UnloadAsset<DeadFollowerInformationBox>(this.DeadFollowerInformationBox);
    UIManager.UnloadAsset<UIPrisonerMenuController>(this.PrisonerMenuTemplate);
    UIManager.UnloadAsset<UIMissionaryMenuController>(this.MissionaryMenuTemplate);
    UIManager.UnloadAsset<UIMatingProgressMenuController>(this.MatingProgressMenuTemplate);
    UIManager.UnloadAsset<UIMissionMenuController>(this.MissionMenuTemplate);
    UIManager.UnloadAsset<UIRefineryMenuController>(this.RefineryMenuTemplate);
    UIManager.UnloadAsset<UICookingMinigameOverlayController>(this.CookingMinigameOverlayControllerTemplate);
    UIManager.UnloadAsset<UIDifficultySelectorOverlayController>(this.DifficultySelectorTemplate);
    UIManager.UnloadAsset<MissionaryFollowerItem>(this.MatingFollowerItemTemplate);
    UIManager.UnloadAsset<MissionaryFollowerItem>(this.MissionaryFollowerItemTemplate);
    UIManager.UnloadAsset<IndoctrinationColourItem>(this.FollowerColourItemTemplate);
    UIManager.UnloadAsset<IndoctrinationFormItem>(this.FollowerFormItemTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Outfit>(this.AppearanceMenuOutfitTemplate);
    UIManager.UnloadAsset<UIMatingMenuController>(this.MatingMenuControllerTemplate);
    UIManager.UnloadAsset<IndoctrinationVariantItem>(this.FollowerVariantItemTemplate);
    UIManager.UnloadAsset<necklaceFormItem>(this.FollowerFormNecklaceTemplate);
    UIManager.UnloadAsset<IndoctrinationOutfitItem>(this.FollowerFormOutfitTemplate);
    UIManager.UnloadAsset<DemonFollowerItem>(this.DemonFollowerItemTemplate);
    UIManager.UnloadAsset<UIFollowerInteractionWheelOverlayController>(this.FollowerInteractionWheelTemplate);
    UIManager.UnloadAsset<UIShareHouseMenuController>(this.ShareHouseMenuTemplate);
    UIManager.UnloadAsset<UICryptMenuController>(this.CryptMenuTemplate);
    UIManager.UnloadAsset<UILogisticsMenuController>(this.LogisticsMenuTemplate);
    UIManager.UnloadAsset<UIMysticShopOverlayController>(this.MysticShopOverlayTemplate);
    UIManager.UnloadAsset<UIFollowerKitchenMenuController>(this.FollowerKitchenMenuControllerTemplate);
    UIManager.UnloadAsset<UIPubMenuController>(this.PubMenuControllerTemplate);
    UIManager.UnloadAsset<UISandboxMenuController>(this.SandboxMenuTemplate);
    UIManager.UnloadAsset<UITailorMenuController>(this.TailorMenuControllerTemplate);
    UIManager.UnloadAsset<UITailorMinigameOverlayController>(this.TailorMinigameOverlayControllerTemplate);
    UIManager.UnloadAsset<UIDrumMinigameOverlayController>(this.DrumMinigameOverlayControllerTemplate);
    UIManager.UnloadAsset<UIRebuildBedMinigameOverlayController>(this.RebuildBedMinigameOverlayControllerTemplate);
    UIManager.UnloadAsset<UIBuildSnowmanMinigameOverlayController>(this.BuildSnowmanMinigameOverlayControllerTemplate);
    UIManager.UnloadAsset<UICustomizeClothesMenuController_Colour>(this.CustomizeClothesColourTemplate);
    UIManager.UnloadAsset<UISermonXPOverlayController>(this.SermonXPOverlayTemplate);
    UIManager.UnloadAsset<UITraitSelector>(this.TraitSelectorTemplate);
    UIManager.UnloadAsset<UIDaycareMenu>(this.DaycareMenuTemplate);
    UIManager.UnloadAsset<UIKnuckleBonesController>(this.KnucklebonesTemplate);
    UIManager.UnloadAsset<UIJobBoardMenuController>(this.JobBoardMenuTemplate);
    UIManager.UnloadAsset<UIRanchSelectMenuController>(this.RanchSelectMenuTemplate);
    UIManager.UnloadAsset<RanchMenuItem>(this.RanchInformationBoxTemplate);
    UIManager.UnloadAsset<RanchMatingMenuItem>(this.RanchMatingInformationBoxTemplate);
    UIManager.UnloadAsset<UIDLCUpgradeTreeMenuController>(this.DLCUpgradeTreeMenuTemplate);
    UIManager.UnloadAsset<UITraitManipulatorMenuController>(this.TraitManipulatorMenuTemplate);
    UIManager.UnloadAsset<UITraitManipulatorResultsScreen>(this.TraitManipulatorResultMenuTemplate);
    UIManager.UnloadAsset<UITraitManipulatorInProgressMenuController>(this.TraitManipulatorInProgressMenuTemplate);
    UIManager.UnloadAsset<UIRanchMenuController>(this.RanchMenuTemplate);
    UIManager.UnloadAsset<UIRanchMatingMenu>(this.RanchMatingMenuTemplate);
    UIManager.UnloadAsset<UIRanchAssignMenuController>(this.RanchAssignMenuTemplate);
    UIManager.UnloadAsset<UIWeaponPickupPromptController>(this.WeaponPickPromptControllerTemplate);
    UIManager.UnloadAsset<UITwitchFollowerSelectOverlayController>(this.TwitchFollowerSelectOverlayController);
    UIManager.UnloadAsset<UITwitchTotemWheel>(this.TwitchTotemWheelController);
    UIManager.UnloadAsset<TwitchInformationBox>(this.TwitchInformationBox);
    this.UnloadFlockadeAssets();
    if ((UnityEngine.Object) this.WateredGameObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.WateredGameObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.WateredGameObjectTemplate.transform);
    if ((UnityEngine.Object) this.UnWateredGameObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.UnWateredGameObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.UnWateredGameObjectTemplate.transform);
    if ((UnityEngine.Object) this.WateredRotGameObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.WateredRotGameObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.WateredRotGameObjectTemplate.transform);
    if ((UnityEngine.Object) this.UnWateredRotGameObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.UnWateredRotGameObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.UnWateredRotGameObjectTemplate.transform);
    if ((UnityEngine.Object) this.FertilizedObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.FertilizedObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.FertilizedObjectTemplate.transform);
    if ((UnityEngine.Object) this.FertilizedGoldObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.FertilizedGoldObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.FertilizedGoldObjectTemplate.transform);
    if ((UnityEngine.Object) this.FertilizedGlowObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.FertilizedGlowObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.FertilizedGlowObjectTemplate.transform);
    if ((UnityEngine.Object) this.FertilizedRainbowObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.FertilizedRainbowObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.FertilizedRainbowObjectTemplate.transform);
    if ((UnityEngine.Object) this.FertilizedDevotionObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.FertilizedDevotionObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.FertilizedDevotionObjectTemplate.transform);
    if ((UnityEngine.Object) this.FertilizedRotstoneObjectTemplate != (UnityEngine.Object) null && (UnityEngine.Object) this.FertilizedRotstoneObjectTemplate.transform != (UnityEngine.Object) null)
      UIManager.UnloadGameObjectAsset<Transform>(this.FertilizedRotstoneObjectTemplate.transform);
    if (!((UnityEngine.Object) this.FrozenGroundObjectTemplate != (UnityEngine.Object) null) || !((UnityEngine.Object) this.FrozenGroundObjectTemplate.transform != (UnityEngine.Object) null))
      return;
    UIManager.UnloadGameObjectAsset<Transform>(this.FrozenGroundObjectTemplate.transform);
  }

  public async System.Threading.Tasks.Task LoadDungeonAssets()
  {
    Application.backgroundLoadingPriority = ThreadPriority.High;
    Task<UIInventoryPromptOverlay> inventoryPromptTask = UIManager.LoadAsset<UIInventoryPromptOverlay>(this._inventoryPromptTemplate);
    Task<UIAdventureMapOverlayController> adventureMapOverlayTask = UIManager.LoadAsset<UIAdventureMapOverlayController>(this._adventureMapOverlayTemplate);
    Task<UITarotPickUpOverlayController> tarotPickUpOverlayTask = UIManager.LoadAsset<UITarotPickUpOverlayController>(this._tarotPickUpOverlayTemplate);
    Task<UITarotChoiceOverlayController> tarotChoiceOverlayTask = UIManager.LoadAsset<UITarotChoiceOverlayController>(this._tarotChoiceOverlayTemplate);
    Task<UIFleeceTarotRewardOverlayController> fleeceTarotRewardTask = UIManager.LoadAsset<UIFleeceTarotRewardOverlayController>(this._fleeceTarotRewardOverlayTemplate);
    Task<AdventureMapNode> adventureMapNodeTask = UIManager.LoadAsset<AdventureMapNode>(this._adventureMapNodeTemplate);
    Task<UIRelicPickupPromptController> relicPickupPromptTask = UIManager.LoadAsset<UIRelicPickupPromptController>(this._relicPickupPromptTemplate);
    Task<UIWeaponPickupPromptController> weaponPickPromptTask = UIManager.LoadAsset<UIWeaponPickupPromptController>(this._weaponPickupPromptTemplate);
    await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) new List<System.Threading.Tasks.Task>()
    {
      (System.Threading.Tasks.Task) inventoryPromptTask,
      (System.Threading.Tasks.Task) adventureMapOverlayTask,
      (System.Threading.Tasks.Task) tarotPickUpOverlayTask,
      (System.Threading.Tasks.Task) tarotChoiceOverlayTask,
      (System.Threading.Tasks.Task) fleeceTarotRewardTask,
      (System.Threading.Tasks.Task) adventureMapNodeTask,
      (System.Threading.Tasks.Task) relicPickupPromptTask,
      (System.Threading.Tasks.Task) weaponPickPromptTask
    });
    this.InventoryPromptTemplate = inventoryPromptTask.Result;
    this.AdventureMapOverlayTemplate = adventureMapOverlayTask.Result;
    this.TarotPickUpOverlayTemplate = tarotPickUpOverlayTask.Result;
    this.TarotChoiceOverlayTemplate = tarotChoiceOverlayTask.Result;
    this.FleeceTarotChoiceOverlayTemplate = fleeceTarotRewardTask.Result;
    this.AdventureMapNodeTemplate = adventureMapNodeTask.Result;
    this.RelicPickupPromptControllerTemplate = relicPickupPromptTask.Result;
    this.WeaponPickPromptControllerTemplate = weaponPickPromptTask.Result;
    ObjectPool.CreatePool<FollowerInformationBox>(this.FollowerInformationBoxTemplate, Mathf.Max(20, DataManager.Instance.Followers.Count));
    Application.backgroundLoadingPriority = ThreadPriority.Normal;
    inventoryPromptTask = (Task<UIInventoryPromptOverlay>) null;
    adventureMapOverlayTask = (Task<UIAdventureMapOverlayController>) null;
    tarotPickUpOverlayTask = (Task<UITarotPickUpOverlayController>) null;
    tarotChoiceOverlayTask = (Task<UITarotChoiceOverlayController>) null;
    fleeceTarotRewardTask = (Task<UIFleeceTarotRewardOverlayController>) null;
    adventureMapNodeTask = (Task<AdventureMapNode>) null;
    relicPickupPromptTask = (Task<UIRelicPickupPromptController>) null;
    weaponPickPromptTask = (Task<UIWeaponPickupPromptController>) null;
  }

  public void UnloadDungeonAssets()
  {
    UIManager.UnloadAsset<UIInventoryPromptOverlay>(this.InventoryPromptTemplate);
    UIManager.UnloadAsset<UIAdventureMapOverlayController>(this.AdventureMapOverlayTemplate);
    UIManager.UnloadAsset<UITarotPickUpOverlayController>(this.TarotPickUpOverlayTemplate);
    UIManager.UnloadAsset<UITarotChoiceOverlayController>(this.TarotChoiceOverlayTemplate);
    UIManager.UnloadAsset<UIFleeceTarotRewardOverlayController>(this.FleeceTarotChoiceOverlayTemplate);
    UIManager.UnloadAsset<AdventureMapNode>(this.AdventureMapNodeTemplate);
    UIManager.UnloadAsset<UIRelicPickupPromptController>(this.RelicPickupPromptControllerTemplate);
    UIManager.UnloadAsset<UIWeaponPickupPromptController>(this.WeaponPickPromptControllerTemplate);
    UIManager.UnloadAsset<UIUpgradeTreeMenuController>(this.UpgradeTreeMenuTemplate);
  }

  public async System.Threading.Tasks.Task LoadWorldMapAssets()
  {
    this.WorldMapTemplate = await UIManager.LoadAsset<UIWorldMapMenuController>(this._worldMapTemplate);
  }

  public void UnloadWorldMapAssets()
  {
    UIManager.UnloadAsset<UIWorldMapMenuController>(this.WorldMapTemplate);
  }

  public async System.Threading.Tasks.Task LoadDLCWorldMapAssets()
  {
    this.DLCWorldMapTemplate = await UIManager.LoadAsset<UIDLCMapMenuController>(this._dlcWorldMapTemplate);
  }

  public void UnloadDLCWorldMapAssets()
  {
    UIManager.UnloadAsset<UIDLCMapMenuController>(this.DLCWorldMapTemplate);
  }

  public async System.Threading.Tasks.Task LoadCultNameAssets()
  {
    this.CultNameMenuTemplate = await UIManager.LoadAsset<UICultNameMenuController>(this._cultNameMenuTemplate);
  }

  public void UnloadCultNameAssets()
  {
    UIManager.UnloadAsset<UICultNameMenuController>(this.CultNameMenuTemplate);
  }

  public async System.Threading.Tasks.Task LoadMysticSellerNameMenuAssets()
  {
    this.MysticSellerNameMenuTemplate = await UIManager.LoadAsset<UIMysticSellerNameMenuController>(this._mysticSellerNameMenuTemplate);
  }

  public void UnloadMysticSellerNameMenuAssets()
  {
    UIManager.UnloadAsset<UIMysticSellerNameMenuController>(this.MysticSellerNameMenuTemplate);
  }

  public async System.Threading.Tasks.Task LoadMatingMenuAssets()
  {
    this.MatingMenuControllerTemplate = await UIManager.LoadAsset<UIMatingMenuController>(this._matingMenuControllerTemplate);
  }

  public void UnloadMatingMenuAssets()
  {
    UIManager.UnloadAsset<UIMatingMenuController>(this.MatingMenuControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadCookingMinigameAssets()
  {
    this.CookingMinigameOverlayControllerTemplate = await UIManager.LoadAsset<UICookingMinigameOverlayController>(this._cookingMinigameOverlayTemplate);
  }

  public void UnloadCookingMinigameAssets()
  {
    UIManager.UnloadAsset<UICookingMinigameOverlayController>(this.CookingMinigameOverlayControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadRebuildBedMinigameAssets()
  {
    this.RebuildBedMinigameOverlayControllerTemplate = await UIManager.LoadAsset<UIRebuildBedMinigameOverlayController>(this._rebuildBedMinigameOverlayTemplate);
  }

  public void UnloadRebuildBedMinigameAssets()
  {
    UIManager.UnloadAsset<UIRebuildBedMinigameOverlayController>(this.RebuildBedMinigameOverlayControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadTailorMinigameAssets()
  {
    this.TailorMinigameOverlayControllerTemplate = await UIManager.LoadAsset<UITailorMinigameOverlayController>(this._tailorMinigameOverlayTemplate);
  }

  public void UnloadTailorMinigameAssets()
  {
    UIManager.UnloadAsset<UITailorMinigameOverlayController>(this.TailorMinigameOverlayControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadMysticShopAssets()
  {
    this.MysticShopOverlayTemplate = await UIManager.LoadAsset<UIMysticShopOverlayController>(this._mysticShopOverlayTemplate);
  }

  public void UnloadMysticShopAssets()
  {
    UIManager.UnloadAsset<UIMysticShopOverlayController>(this.MysticShopOverlayTemplate);
  }

  public async System.Threading.Tasks.Task LoadPlayerUpgradesMenu()
  {
    this.PlayerUpgradesMenuTemplate = await UIManager.LoadAsset<UIPlayerUpgradesMenuController>(this._playerUpgradesMenuTemplate);
  }

  public void UnloadPlayerUpgradesMenu()
  {
    UIManager.UnloadAsset<UIPlayerUpgradesMenuController>(this.PlayerUpgradesMenuTemplate);
  }

  public async System.Threading.Tasks.Task LoadDrumMinigameAssets()
  {
    this.DrumMinigameOverlayControllerTemplate = await UIManager.LoadAsset<UIDrumMinigameOverlayController>(this._drumMinigameOverlayTemplate);
  }

  public void UnloadDrumMinigameAssets()
  {
    UIManager.UnloadAsset<UIDrumMinigameOverlayController>(this.DrumMinigameOverlayControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadBuildSnowmanMinigameAssets()
  {
    this.BuildSnowmanMinigameOverlayControllerTemplate = await UIManager.LoadAsset<UIBuildSnowmanMinigameOverlayController>(this._buildSnowmanMinigameOverlayTemplate);
  }

  public void UnloadBuildSnowmanMinigameAssets()
  {
    UIManager.UnloadAsset<UIBuildSnowmanMinigameOverlayController>(this.BuildSnowmanMinigameOverlayControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadSandboxMenuAssets()
  {
    this.SandboxMenuTemplate = await UIManager.LoadAsset<UISandboxMenuController>(this._sandboxMenuTemplate);
  }

  public void UnloadSandboxMenuAssets()
  {
    UIManager.UnloadAsset<UISandboxMenuController>(this.SandboxMenuTemplate);
  }

  public async System.Threading.Tasks.Task LoadHubShoreAssets()
  {
    this.FishingOverlayControllerTemplate = await UIManager.LoadAsset<UIFishingOverlayController>(this._fishingOverlayTemplate);
  }

  public async System.Threading.Tasks.Task LoadUpgradeTree()
  {
    this.UpgradeTreeMenuTemplate = await UIManager.LoadAsset<UIUpgradeTreeMenuController>(this._upgradeTreeTemplate);
  }

  public async System.Threading.Tasks.Task LoadDLCUpgradeTree()
  {
    this.DLCUpgradeTreeMenuTemplate = await UIManager.LoadAsset<UIDLCUpgradeTreeMenuController>(this._dlcUpgradeTreeTemplate);
  }

  public void UnloadUpgradeTree()
  {
    UIManager.UnloadAsset<UIUpgradeTreeMenuController>(this.UpgradeTreeMenuTemplate);
  }

  public void UnloadDLCUpgradeTree()
  {
    UIManager.UnloadAsset<UIDLCUpgradeTreeMenuController>(this.DLCUpgradeTreeMenuTemplate);
  }

  public void UnloadHubShoreAssets()
  {
    UIManager.UnloadAsset<UIFishingOverlayController>(this.FishingOverlayControllerTemplate);
  }

  public static async Task<T> LoadAsset<T>(AssetReferenceGameObject assetReferenceGameObject) where T : MonoBehaviour
  {
    AsyncOperationHandle<GameObject> asyncOperation = Addressables.LoadAssetAsync<GameObject>((object) assetReferenceGameObject);
    GameObject task = await asyncOperation.Task;
    T component = asyncOperation.Result.GetComponent<T>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (MonoSingleton<UIManager>.Instance._addressablesTracker.ContainsKey(component.gameObject))
        UIManager.Log($"Asset is already loaded : {component.name}. Ensure Unload is called somewhere!");
      else
        MonoSingleton<UIManager>.Instance._addressablesTracker.Add(component.gameObject, (AsyncOperationHandle) asyncOperation);
      return component;
    }
    UIManager.Log("Load Fail : " + assetReferenceGameObject.AssetGUID);
    return default (T);
  }

  public static async Task<T> LoadGameObjectAsset<T>(
    AssetReferenceGameObject assetReferenceGameObject)
    where T : Transform
  {
    AsyncOperationHandle<GameObject> asyncOperation = Addressables.LoadAssetAsync<GameObject>((object) assetReferenceGameObject);
    GameObject task = await asyncOperation.Task;
    T component = asyncOperation.Result.GetComponent<T>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (MonoSingleton<UIManager>.Instance._addressablesTracker.ContainsKey(component.gameObject))
        UIManager.Log($"Asset is already loaded : {component.name}. Ensure Unload is called somewhere!");
      else
        MonoSingleton<UIManager>.Instance._addressablesTracker.Add(component.gameObject, (AsyncOperationHandle) asyncOperation);
      return component;
    }
    UIManager.Log("Load Fail : " + assetReferenceGameObject.AssetGUID);
    return default (T);
  }

  public static void UnloadAsset<T>(T asset) where T : MonoBehaviour
  {
    if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
      return;
    AsyncOperationHandle handle;
    if (MonoSingleton<UIManager>.Instance._addressablesTracker.TryGetValue(asset.gameObject, out handle))
    {
      MonoSingleton<UIManager>.Instance._addressablesTracker.Remove(asset.gameObject);
      Addressables.Release(handle);
    }
    asset = default (T);
  }

  public static void UnloadGameObjectAsset<T>(T asset) where T : Transform
  {
    if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
      return;
    AsyncOperationHandle handle;
    if (MonoSingleton<UIManager>.Instance._addressablesTracker.TryGetValue(asset.gameObject, out handle))
    {
      MonoSingleton<UIManager>.Instance._addressablesTracker.Remove(asset.gameObject);
      Addressables.Release(handle);
    }
    asset = default (T);
  }

  public static IEnumerator LoadAssets(System.Threading.Tasks.Task task, System.Action callback)
  {
    yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override void Start()
  {
    base.Start();
    this.transform.SetParent((Transform) null);
    this._cursorSize = this._cursors[this.currentCursor].width;
    this.UpdateCursorIfRequired();
  }

  public void UpdateCursorIfRequired()
  {
    if (this._currentResolution.width == Screen.width && this._currentResolution.height == Screen.height && !this._cursorIconChanged)
      return;
    this._cursorIconChanged = false;
    this._currentResolution.width = Screen.width;
    this._currentResolution.height = Screen.height;
    if ((UnityEngine.Object) this._currentCursorTexture != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._currentCursorTexture);
      this._currentCursorTexture = (Texture2D) null;
    }
    float num1 = Mathf.Min((float) this._currentResolution.height / 1440f, 1f);
    int num2 = Mathf.RoundToInt((float) this._cursorSize * num1);
    RenderTexture renderTexture = new RenderTexture(num2, num2, 24);
    renderTexture.name = "Cursor_Render_Texture" + Time.frameCount.ToString();
    RenderTexture dest = renderTexture;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = dest;
    Graphics.Blit((Texture) this._cursors[this.currentCursor], dest);
    Texture2D texture2D = new Texture2D(num2, num2, TextureFormat.RGBA32, false);
    texture2D.name = "Cursor_Texture" + Time.frameCount.ToString();
    this._currentCursorTexture = texture2D;
    this._currentCursorTexture.ReadPixels(new Rect(0.0f, 0.0f, (float) num2, (float) num2), 0, 0, false);
    this._currentCursorTexture.Apply();
    RenderTexture.active = active;
    dest.Release();
    dest.DiscardContents();
    UnityEngine.Object.Destroy((UnityEngine.Object) dest);
    Cursor.SetCursor(this._currentCursorTexture, new Vector2(18f * num1, 6f * num1), CursorMode.ForceSoftware);
  }

  public void SetCurrentCursor(int id)
  {
    if (this.currentCursor == id)
    {
      this._previousCursor = id;
    }
    else
    {
      this._previousCursor = this.currentCursor;
      this._cursorIconChanged = true;
      this.currentCursor = id;
      this.UpdateCursorIfRequired();
    }
  }

  public void ResetPreviousCursor() => this._previousCursor = -1;

  public void SetPreviousCursor()
  {
    if (this._previousCursor != -1)
      this.SetCurrentCursor(this._previousCursor);
    else
      this.SetCurrentCursor(0);
  }

  public static void Log(string message)
  {
  }

  public void Update()
  {
    GameManager instance = GameManager.GetInstance();
    if (!this.MenusBlocked && !this.ForceBlockPause && UIItemSelectorOverlayController.SelectorOverlays.Count == 0 && (UnityEngine.Object) instance != (UnityEngine.Object) null && (UnityEngine.Object) this._currentInstance == (UnityEngine.Object) null)
    {
      if (PlayerFarming.playersCount == 0 && InputManager.Gameplay.GetPauseButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !CheatConsole.IN_DEMO)
        this.ShowPauseMenu((PlayerFarming) null);
      if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null && MMConversation.isPlaying && !MMConversation.isBark)
      {
        if (InputManager.Gameplay.GetPauseButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !CheatConsole.IN_DEMO)
          this.ShowPauseMenu(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      }
      else
      {
        for (int index = 0; index < PlayerFarming.playersCount; ++index)
        {
          PlayerFarming player = PlayerFarming.players[index];
          if (InputManager.Gameplay.GetPauseButtonDown(player) && !CheatConsole.IN_DEMO)
          {
            this.ShowPauseMenu(player);
            break;
          }
          if (InputManager.Gameplay.GetMenuButtonDown(player) && DataManager.Instance.HadInitialDeathCatConversation)
          {
            this.ShowDetailsMenu(player);
            break;
          }
        }
      }
    }
    this.UpdateCursorIfRequired();
  }

  public void ShowPauseMenu(PlayerFarming playerFarming)
  {
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    MMVibrate.StopRumble();
    this.IsPaused = true;
    UIPauseMenuController pauseMenuController = this.SetMenu<UIPauseMenuController>(this.PauseMenuController, filter: true);
    pauseMenuController.OnHidden = pauseMenuController.OnHidden + (System.Action) (() => this.IsPaused = false);
  }

  public void ShowDetailsMenu(PlayerFarming playerFarming)
  {
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    MMVibrate.StopRumble();
    CharacterMenu.OpeningPlayerFarming = playerFarming;
    this.IsPaused = true;
    UIPauseDetailsMenuController detailsMenuController = this.SetMenu<UIPauseDetailsMenuController>(this.PauseDetailsMenuTemplate, filter: true);
    detailsMenuController.OnHidden = detailsMenuController.OnHidden + (System.Action) (() =>
    {
      this.IsPaused = false;
      if (DataManager.Instance.ShownInventoryTutorial || !UIInventoryPromptOverlay.Showing)
        return;
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
      DataManager.Instance.ShownInventoryTutorial = true;
      CameraFollowTarget.Instance.SetOffset(Vector3.zero);
      UIInventoryPromptOverlay.Showing = false;
    });
  }

  public UIKeyScreenOverlayController ShowKeyScreen()
  {
    return this.SetMenu<UIKeyScreenOverlayController>(this.KeyScreenTemplate);
  }

  public UIWorldMapMenuController ShowWorldMap()
  {
    return this.SetMenu<UIWorldMapMenuController>(this.WorldMapTemplate);
  }

  public UITutorialOverlayController ShowTutorialOverlay(
    TutorialTopic topic,
    float delay = 0.0f,
    System.Action callback = null)
  {
    UITutorialOverlayController menu = this.TutorialOverlayTemplate.Instantiate<UITutorialOverlayController>();
    menu.Show(topic, delay);
    UITutorialOverlayController overlayController = menu;
    overlayController.OnHidden = overlayController.OnHidden + callback;
    return this.SetMenuInstance((UIMenuBase) menu) as UITutorialOverlayController;
  }

  public UIAltarMenuController ShowAltarMenu()
  {
    return this.SetMenu<UIAltarMenuController>(this.AltarMenuTemplate);
  }

  public UIUpgradeTreeMenuController ShowUpgradeTree(
    System.Action closeCallback = null,
    UpgradeSystem.Type revealType = UpgradeSystem.Type.Count,
    bool hideControls = false,
    bool showDLCTree = false)
  {
    UIUpgradeTreeMenuController upgradeTreeInstance = showDLCTree ? (UIUpgradeTreeMenuController) this.DLCUpgradeTreeMenuTemplate.Instantiate<UIDLCUpgradeTreeMenuController>() : this.UpgradeTreeMenuTemplate.Instantiate<UIUpgradeTreeMenuController>();
    if (hideControls || revealType != UpgradeSystem.Type.Count)
      upgradeTreeInstance.DisableControlPrompts();
    HUD_Manager.Instance.Hide(true);
    upgradeTreeInstance.Show(revealType);
    UIUpgradeTreeMenuController treeMenuController1 = upgradeTreeInstance;
    treeMenuController1.OnHidden = treeMenuController1.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
      HUD_Manager.Instance.Show(0);
      System.Action action = closeCallback;
      if (action == null)
        return;
      action();
    });
    UIUpgradeTreeMenuController treeMenuController2 = upgradeTreeInstance;
    treeMenuController2.OnUpgradeUnlocked = treeMenuController2.OnUpgradeUnlocked + (Action<UpgradeSystem.Type>) (upgrade =>
    {
      if (upgrade != UpgradeSystem.Type.Building_Temple2 && upgrade != UpgradeSystem.Type.Temple_III && upgrade != UpgradeSystem.Type.Temple_IV)
        return;
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      upgradeTreeInstance.Hide();
    });
    this.SetMenuInstance((UIMenuBase) upgradeTreeInstance);
    return upgradeTreeInstance;
  }

  public UIUpgradePlayerTreeMenuController ShowPlayerUpgradeTree()
  {
    UIUpgradePlayerTreeMenuController upgradeTreeInstance = this.UpgradePlayerTreeMenuTemplate.Instantiate<UIUpgradePlayerTreeMenuController>();
    UpgradeSystem.Type revealType = UpgradeSystem.Type.Count;
    if (SeasonsManager.Active)
      revealType = UpgradeSystem.Type.Major_DLC_Sermon_Packs;
    upgradeTreeInstance.Show(revealType);
    UIUpgradePlayerTreeMenuController treeMenuController1 = upgradeTreeInstance;
    treeMenuController1.OnHidden = treeMenuController1.OnHidden + (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine()));
    UIUpgradePlayerTreeMenuController treeMenuController2 = upgradeTreeInstance;
    treeMenuController2.OnUpgradeUnlocked = treeMenuController2.OnUpgradeUnlocked + (Action<UpgradeSystem.Type>) (type => upgradeTreeInstance.Hide());
    this.SetMenuInstance((UIMenuBase) upgradeTreeInstance);
    return upgradeTreeInstance;
  }

  public UICookingFireMenuController ShowCookingFireMenu(StructuresData cookingFireData)
  {
    UICookingFireMenuController menu = this.CookingFireMenuTemplate.Instantiate<UICookingFireMenuController>();
    menu.Show(cookingFireData);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIKitchenMenuController ShowKitchenMenu(StructuresData kitchenData)
  {
    UIKitchenMenuController menu = this.KitchenMenuTemplate.Instantiate<UIKitchenMenuController>();
    menu.Show(kitchenData);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UICoopAssignController ShowCoopAssignMenu()
  {
    UICoopAssignController menu = this.CoopAssignMenuTemplate.Instantiate<UICoopAssignController>();
    menu.Show();
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UITailorMenuController ShowTailorMenu(Structures_Tailor tailor)
  {
    UITailorMenuController menu = this.TailorMenuControllerTemplate.Instantiate<UITailorMenuController>();
    menu.Show(tailor);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIRanchMenuController ShowRanchMenu(Structures_Ranch ranch)
  {
    UIRanchMenuController menu = this.RanchMenuTemplate.Instantiate<UIRanchMenuController>();
    menu.Show(ranch, ranch.Data);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIRanchAssignMenuController ShowRanchAssignMenu(Structures_Ranch ranch)
  {
    UIRanchAssignMenuController menu = this.RanchAssignMenuTemplate.Instantiate<UIRanchAssignMenuController>();
    menu.Show(ranch);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIRanchSelectMenuController ShowRanchSelectMenu(
    List<RanchSelectEntry> animal,
    int capacity,
    Structures_Ranch ranch)
  {
    UIRanchSelectMenuController menu = this.RanchSelectMenuTemplate.Instantiate<UIRanchSelectMenuController>();
    menu.Show(animal, capacity, false, true, true, ranch);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIRanchSelectMenuController ShowRanchSacrificeMenu(List<RanchSelectEntry> animal)
  {
    UIRanchSelectMenuController menu = this.RanchSelectMenuTemplate.Instantiate<UIRanchSelectMenuController>();
    menu.Show(animal, false, true, true, true, true, false);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIRanchMatingMenu ShowRanchMatingtMenu(
    List<RanchSelectEntry> animals,
    int capacity,
    Interaction_RanchHutch hutch)
  {
    UIRanchMatingMenu menu = this.RanchMatingMenuTemplate.Instantiate<UIRanchMatingMenu>();
    menu.Show(hutch, animals, capacity);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIRanchSelectMenuController ShowRanchSelectMenu(List<RanchSelectEntry> animal)
  {
    UIRanchSelectMenuController menu = this.RanchSelectMenuTemplate.Instantiate<UIRanchSelectMenuController>();
    menu.Show(animal, false, true, true, true, false, false);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIFollowerSummaryMenuController ShowFollowerSummaryMenu(Follower follower)
  {
    UIFollowerSummaryMenuController menu = this.FollowerSummaryMenuTemplate.Instantiate<UIFollowerSummaryMenuController>();
    menu.Show(follower);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIPlayerUpgradesMenuController ShowPlayerUpgradesMenu()
  {
    UIPlayerUpgradesMenuController menu = this.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    if (!DataManager.Instance.PostGameFleecesOnboarded && DataManager.Instance.DeathCatBeaten && MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb)
    {
      DataManager.Instance.PostGameFleecesOnboarded = true;
      menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[4]
      {
        PlayerFleeceManager.FleeceType.CurseInsteadOfWeapon,
        PlayerFleeceManager.FleeceType.OneHitKills,
        PlayerFleeceManager.FleeceType.HollowHeal,
        PlayerFleeceManager.FleeceType.NoRolling
      });
    }
    else if (!DataManager.Instance.GoatFleeceOnboarded && MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb)
    {
      DataManager.Instance.GoatFleeceOnboarded = true;
      if (!DataManager.Instance.CowboyFleeceOnboarded && DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss) && DataManager.Instance.PleasureEnabled)
      {
        DataManager.Instance.CowboyFleeceOnboarded = true;
        menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[2]
        {
          PlayerFleeceManager.FleeceType.Goat,
          PlayerFleeceManager.FleeceType.Fleece676
        });
      }
      else
        menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
        {
          PlayerFleeceManager.FleeceType.Goat
        });
    }
    else if (!DataManager.Instance.CowboyFleeceOnboarded && DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss) && MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb && DataManager.Instance.PleasureEnabled)
    {
      DataManager.Instance.CowboyFleeceOnboarded = true;
      menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
      {
        PlayerFleeceManager.FleeceType.Fleece676
      });
    }
    else
      menu.Show();
    UIPlayerUpgradesMenuController upgradesMenuController = menu;
    upgradesMenuController.OnHidden = upgradesMenuController.OnHidden + (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine()));
    return this.SetMenuInstance((UIMenuBase) menu, 1f) as UIPlayerUpgradesMenuController;
  }

  public UICultUpgradesMenuController ShowCultUpgradesMenu()
  {
    UICultUpgradesMenuController menu = this.CultUpgradesMenuTemplate.Instantiate<UICultUpgradesMenuController>();
    menu.Show();
    UICultUpgradesMenuController upgradesMenuController = menu;
    upgradesMenuController.OnHidden = upgradesMenuController.OnHidden + (System.Action) (() => { });
    return this.SetMenuInstance((UIMenuBase) menu, 1f) as UICultUpgradesMenuController;
  }

  public UIItemSelectorOverlayController ShowItemSelector(
    PlayerFarming playerFarming,
    List<InventoryItem.ITEM_TYPE> items,
    ItemSelector.Params parameters,
    bool dontcache = false)
  {
    UIItemSelectorOverlayController overlayController = this.ItemSelectorOverlayTemplate.Instantiate<UIItemSelectorOverlayController>();
    overlayController.Show(playerFarming, items, parameters, dontCache: dontcache);
    return overlayController;
  }

  public UIItemSelectorOverlayController ShowItemSelector(
    PlayerFarming playerFarming,
    List<InventoryItem> items,
    ItemSelector.Params parameters)
  {
    UIItemSelectorOverlayController overlayController = this.ItemSelectorOverlayTemplate.Instantiate<UIItemSelectorOverlayController>();
    overlayController.Show(playerFarming, items, parameters);
    return overlayController;
  }

  public UIItemSelectorOverlayController ShowItemSelector(
    Transform CustomTarget,
    PlayerFarming playerFarming,
    List<InventoryItem> items,
    ItemSelector.Params parameters)
  {
    UIItemSelectorOverlayController overlayController = this.ItemSelectorOverlayTemplate.Instantiate<UIItemSelectorOverlayController>();
    overlayController.Show(CustomTarget, playerFarming, items, parameters);
    return overlayController;
  }

  public UIDifficultySelectorOverlayController ShowDifficultySelector()
  {
    if ((UnityEngine.Object) this.DifficultySelectorTemplate == (UnityEngine.Object) null)
      return (UIDifficultySelectorOverlayController) null;
    UIDifficultySelectorOverlayController menu = this.DifficultySelectorTemplate.Instantiate<UIDifficultySelectorOverlayController>();
    menu.Show(false, DataManager.Instance.PermadeDeathActive);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UITarotPickUpOverlayController ShowTarotPickUp(TarotCards.TarotCard card)
  {
    UITarotPickUpOverlayController menu = this.TarotPickUpOverlayTemplate.Instantiate<UITarotPickUpOverlayController>();
    menu.Show(card);
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    return menu;
  }

  public UITarotChoiceOverlayController ShowTarotChoice(
    TarotCards.TarotCard card1,
    TarotCards.TarotCard card2)
  {
    UITarotChoiceOverlayController menu = this.TarotChoiceOverlayTemplate.Instantiate<UITarotChoiceOverlayController>();
    menu.Show(card1, card2);
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    return menu;
  }

  public UIFleeceTarotRewardOverlayController ShowFleeceTarotReward(
    TarotCards.TarotCard card1,
    TarotCards.TarotCard card2,
    TarotCards.TarotCard card3,
    TarotCards.TarotCard card4)
  {
    UIFleeceTarotRewardOverlayController menu = this.FleeceTarotChoiceOverlayTemplate.Instantiate<UIFleeceTarotRewardOverlayController>();
    menu.Show(card1, card2, card3, card4);
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    return menu;
  }

  public UIFollowerIndoctrinationMenuController ShowIndoctrinationMenu(
    Follower follower,
    OriginalFollowerLookData originalFollowerLook)
  {
    UIFollowerIndoctrinationMenuController menu = this.IndoctrinationMenuTemplate.Instantiate<UIFollowerIndoctrinationMenuController>();
    menu.Show(follower, originalFollowerLook);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UICustomizeClothingMenuController ShowCustomizeClothingMenu(
    FollowerClothingType clothingType,
    string variant)
  {
    UICustomizeClothingMenuController menu = this.CustomiseCothingMenuTemplate.Instantiate<UICustomizeClothingMenuController>();
    menu.Show(clothingType, variant);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UINewItemOverlayController ShowNewItemOverlay()
  {
    UINewItemOverlayController menu = this.NewItemOverlayTemplate.Instantiate<UINewItemOverlayController>();
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    return menu;
  }

  public UIDeathScreenOverlayController ShowDeathScreenOverlay(
    UIDeathScreenOverlayController.Results result,
    int levels)
  {
    UIDeathScreenOverlayController deathScreenInstance = this.DeathScreenOverlayTemplate.Instantiate<UIDeathScreenOverlayController>();
    deathScreenInstance.Show(result, levels);
    this.DoShowDeathScreen(deathScreenInstance);
    return deathScreenInstance;
  }

  public UIDeathScreenOverlayController ShowDeathScreenOverlay(
    UIDeathScreenOverlayController.Results result)
  {
    UIDeathScreenOverlayController deathScreenInstance = this.DeathScreenOverlayTemplate.Instantiate<UIDeathScreenOverlayController>();
    deathScreenInstance.Show(result);
    this.DoShowDeathScreen(deathScreenInstance);
    return deathScreenInstance;
  }

  public void DoShowDeathScreen(UIDeathScreenOverlayController deathScreenInstance)
  {
    this.SetMenuInstance((UIMenuBase) deathScreenInstance);
  }

  public UIAdventureMapOverlayController ShowAdventureMap(Map.Map map, bool disableInput)
  {
    UIAdventureMapOverlayController menu = this.AdventureMapOverlayTemplate.Instantiate<UIAdventureMapOverlayController>();
    menu.Show(map, disableInput);
    this.SetMenuInstance((UIMenuBase) menu, filter: true);
    return menu;
  }

  public UIBuildMenuController ShowBuildMenu(StructureBrain.TYPES structureToReveal)
  {
    UIBuildMenuController menu = this.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
    menu.Show(structureToReveal);
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIDLCMapMenuController ShowDLCMapMenu(PlayerFarming playerFarming, bool showAllCheat = false)
  {
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    UIDLCMapMenuController menu = MonoSingleton<UIManager>.Instance.DLCWorldMapTemplate.Instantiate<UIDLCMapMenuController>();
    if (showAllCheat)
      menu.RevealAllSelectable();
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    menu.transform.SetAsFirstSibling();
    menu.Show();
    return menu;
  }

  public T SetMenu<T>(T template, float timeScale = 0.0f, bool filter = false) where T : UIMenuBase
  {
    T menu = template.Instantiate<T>();
    menu.Show(false);
    this.SetMenuInstance((UIMenuBase) menu, timeScale, filter);
    return menu;
  }

  public UIMenuBase SetMenuInstance(UIMenuBase menu, float timeScale = 0.0f, bool filter = false)
  {
    if (!((UnityEngine.Object) this._currentInstance == (UnityEngine.Object) null))
      return (UIMenuBase) null;
    if (filter)
      AudioManager.Instance.ToggleFilter(SoundParams.Filter, true);
    Time.timeScale = timeScale;
    this._currentInstance = menu;
    this._currentInstance.OnHidden += (System.Action) (() =>
    {
      if (filter)
        AudioManager.Instance.ToggleFilter(SoundParams.Filter, false);
      if ((UnityEngine.Object) this._currentInstance != (UnityEngine.Object) null && this._currentInstance.SetTimeScaleOnHidden)
        Time.timeScale = 1f;
      this._currentInstance = (UIMenuBase) null;
      GameManager.InMenu = false;
    });
    return this._currentInstance;
  }

  public static bool GetActiveMenu<T>(out T menu) where T : UIMenuBase
  {
    menu = UIManager.GetActiveMenu<T>();
    return (UnityEngine.Object) menu != (UnityEngine.Object) null;
  }

  public static T GetActiveMenu<T>() where T : UIMenuBase
  {
    foreach (UIMenuBase activeMenu1 in UIMenuBase.ActiveMenus)
    {
      if (activeMenu1 is T activeMenu2)
        return activeMenu2;
    }
    return default (T);
  }

  public static void DeactivateActiveMenu()
  {
    foreach (UIMenuBase activeMenu in UIMenuBase.ActiveMenus)
      activeMenu.Hide(true);
  }

  public static void PlayAudio(string soundPath)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      AudioManager.Instance.PlayOneShot(soundPath, PlayerFarming.Instance.gameObject);
    else
      AudioManager.Instance.PlayOneShot(soundPath);
  }

  public static void PlayAudio(string soundPath, float volume)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      AudioManager.Instance.PlayOneShot(soundPath, PlayerFarming.Instance.gameObject);
    else
      AudioManager.Instance.PlayOneShot(soundPath);
  }

  public static EventInstance CreateLoop(string soundPath)
  {
    return (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? AudioManager.Instance.CreateLoop(soundPath, PlayerFarming.Instance.gameObject, true) : AudioManager.Instance.CreateLoop(soundPath, true);
  }

  [CompilerGenerated]
  public void \u003CShowPauseMenu\u003Eb__848_0() => this.IsPaused = false;

  [CompilerGenerated]
  public void \u003CShowDetailsMenu\u003Eb__849_0()
  {
    this.IsPaused = false;
    if (DataManager.Instance.ShownInventoryTutorial || !UIInventoryPromptOverlay.Showing)
      return;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddPlayerToCamera();
    DataManager.Instance.ShownInventoryTutorial = true;
    CameraFollowTarget.Instance.SetOffset(Vector3.zero);
    UIInventoryPromptOverlay.Showing = false;
  }

  public enum CursorTypes
  {
    Standard,
    Blunderbuss,
  }
}
