// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
using Lamb.UI.RefineryMenu;
using Lamb.UI.Rituals;
using Lamb.UI.SermonWheelOverlay;
using Lamb.UI.SettingsMenu;
using Lamb.UI.VideoMenu;
using MMTools;
using src.Extensions;
using src.UI;
using src.UI.Menus;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
namespace Lamb.UI;

[DefaultExecutionOrder(-100)]
public class UIManager : MonoSingleton<UIManager>
{
  private const string kAssetPath = "Assets/UI/Prefabs/UIManager.prefab";
  public bool ForceBlockMenus;
  private UIMenuBase _currentInstance;
  private float _previousClipPlane = 100f;
  private Camera _currentMain;
  private Texture2D _currentCursorTexture;
  private Resolution _currentResolution;
  private int _cursorSize;
  private Dictionary<GameObject, AsyncOperationHandle> _addressablesTracker = new Dictionary<GameObject, AsyncOperationHandle>();
  [Header("Cursor")]
  [SerializeField]
  private Texture2D _cursor;
  [Header("Direct References")]
  [SerializeField]
  private UISettingsMenuController _settingsMenuTemplate;
  [SerializeField]
  private UIMenuConfirmationWindow _confirmationWindowTemplate;
  [SerializeField]
  private UIConfirmationCountdownWindow _confirmationCountdownWindowTemplate;
  [Header("Controllers")]
  [SerializeField]
  private AssetReferenceGameObject _xboxControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _ps4ControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _ps5ControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _switchProControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _switchJoyConsControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _switchJoyConsDetachedControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _switchHandheldControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _keyboardTemplate;
  [SerializeField]
  private AssetReferenceGameObject _mouseTemplate;
  [Header("Menu Templates")]
  [SerializeField]
  private AssetReferenceGameObject _creditsMenuControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _roadmapOverlayControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _pauseMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _pauseDetailsMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _indoctrinationMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _tarotCardsMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _worldMapTemplate;
  [SerializeField]
  private AssetReferenceGameObject _upgradeTreeTemplate;
  [SerializeField]
  private AssetReferenceGameObject _upgradePlayerTreeTemplate;
  [SerializeField]
  private AssetReferenceGameObject _cookingFireTemplate;
  [SerializeField]
  private AssetReferenceGameObject _kitchenMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerSummaryMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _playerUpgradesMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _altarMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerSelectMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _doctrineMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _doctrineChoicesMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _ritualsMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _sermonWheelTemplate;
  [SerializeField]
  private AssetReferenceGameObject _demonSummonMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _demonMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _prisonMenuControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _prisonerMenuControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _missionaryMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _missionMenuControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _refineryMenuControllerTemplate;
  [SerializeField]
  private AssetReferenceGameObject _tutorialMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _buildMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerFormsMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _appearanceMenuFormTemplate;
  [SerializeField]
  private AssetReferenceGameObject _appearanceMenuColourTemplate;
  [SerializeField]
  private AssetReferenceGameObject _appearanceMenuVariantTemplate;
  [SerializeField]
  private AssetReferenceGameObject _videoExportMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _sacrificeFollowerMenuTemplate;
  [Header("Windows")]
  [SerializeField]
  private AssetReferenceGameObject _saveErrorWindowTemplate;
  [Header("Overlays")]
  [SerializeField]
  private AssetReferenceGameObject _bugReportingOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _keyScreenOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _tutorialOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _recipeConfirmationOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _itemSelectorTemplate;
  [SerializeField]
  private AssetReferenceGameObject _difficultySelectorTemplate;
  [SerializeField]
  private AssetReferenceGameObject _tarotChoiceOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _fleeceTarotRewardOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerInteractionWheelTemplate;
  [SerializeField]
  private AssetReferenceGameObject _newItemOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _cultNameMenuTemplate;
  [SerializeField]
  private AssetReferenceGameObject _deathScreenOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _adventureMapOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _fishingOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _cookingMinigameOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _loadingOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _bindingOverlayTemplate;
  [SerializeField]
  private AssetReferenceGameObject _dropdownOverlayTemplate;
  [Header("Knucklebones")]
  [SerializeField]
  private AssetReferenceGameObject _knucklebonesTemplate;
  [SerializeField]
  private AssetReferenceGameObject _knucklebonesBettingSelectorTemplate;
  [SerializeField]
  private AssetReferenceGameObject _knucklebonesOpponentSelectorTemplate;
  [Header("Other")]
  [SerializeField]
  private AssetReferenceGameObject _inventoryPromptTemplate;
  [Header("Pooling")]
  [SerializeField]
  private AssetReferenceGameObject _followerInformationBoxTemplate;
  [SerializeField]
  private AssetReferenceGameObject _missionaryFollowerItemTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerColourItemTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerFormItemTemplate;
  [SerializeField]
  private AssetReferenceGameObject _followerVariantItemTemplate;
  [SerializeField]
  private AssetReferenceGameObject _adventureMapNodeTemplate;
  [SerializeField]
  private AssetReferenceGameObject _demonFollowerItemTemplate;
  [Header("Twitch")]
  [SerializeField]
  private AssetReferenceGameObject _twitchFollowerSelect;
  [SerializeField]
  private AssetReferenceGameObject _twitchTotemWheel;

  public bool MenusBlocked
  {
    get
    {
      return this.ForceBlockMenus || MMTransition.IsPlaying || (double) Time.timeScale < 1.0 || GameManager.InMenu || UIMenuBase.ActiveMenus.Count > 0;
    }
  }

  public bool IsPaused { get; private set; }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void LoadUIManager()
  {
    Addressables.LoadAssetAsync<GameObject>((object) "Assets/UI/Prefabs/UIManager.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (asyncOperation =>
    {
      asyncOperation.Result.GetComponent<UIManager>().Instantiate<UIManager>();
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
    this.CreditsMenuControllerTemplate = await UIManager.LoadAsset<UICreditsMenuController>(this._creditsMenuControllerTemplate);
    this.RoadmapOverlayControllerTemplate = await UIManager.LoadAsset<UIRoadmapOverlayController>(this._roadmapOverlayControllerTemplate);
    this.LoadingOverlayControllerTemplate = await UIManager.LoadAsset<UILoadingOverlayController>(this._loadingOverlayTemplate);
  }

  public void UnloadMainMenuAssets()
  {
    UIManager.UnloadAsset<UICreditsMenuController>(this.CreditsMenuControllerTemplate);
    UIManager.UnloadAsset<UIRoadmapOverlayController>(this.RoadmapOverlayControllerTemplate);
    UIManager.UnloadAsset<UILoadingOverlayController>(this.LoadingOverlayControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadKnucklebonesAssets()
  {
    this.KnucklebonesTemplate = await UIManager.LoadAsset<UIKnuckleBonesController>(this._knucklebonesTemplate);
    this.KnucklebonesBettingSelectionTemplate = await UIManager.LoadAsset<UIKnucklebonesBettingSelectionController>(this._knucklebonesBettingSelectorTemplate);
    this.KnucklebonesOpponentSelectionTemplate = await UIManager.LoadAsset<UIKnucklebonesOpponentSelectionController>(this._knucklebonesOpponentSelectorTemplate);
  }

  public void UnloadKnucklebonesAssets()
  {
    UIManager.UnloadAsset<UIKnuckleBonesController>(this.KnucklebonesTemplate);
    UIManager.UnloadAsset<UIKnucklebonesBettingSelectionController>(this.KnucklebonesBettingSelectionTemplate);
    UIManager.UnloadAsset<UIKnucklebonesOpponentSelectionController>(this.KnucklebonesOpponentSelectionTemplate);
  }

  public async System.Threading.Tasks.Task LoadWorldMap()
  {
    this.WorldMapTemplate = await UIManager.LoadAsset<UIWorldMapMenuController>(this._worldMapTemplate);
  }

  public void UnloadWorldMap()
  {
    UIManager.UnloadAsset<UIWorldMapMenuController>(this.WorldMapTemplate);
  }

  public async System.Threading.Tasks.Task LoadLifecyclePersistentAssets()
  {
    this.DropdownOverlayControllerTemplate = await UIManager.LoadAsset<UIDropdownOverlayController>(this._dropdownOverlayTemplate);
    this.BindingOverlayControllerTemplate = await UIManager.LoadAsset<UIControlBindingOverlayController>(this._bindingOverlayTemplate);
    this.KeyboardTemplate = await UIManager.LoadAsset<InputController>(this._keyboardTemplate);
    this.MouseTemplate = await UIManager.LoadAsset<InputController>(this._mouseTemplate);
    this.XboxControllerTemplate = await UIManager.LoadAsset<InputController>(this._xboxControllerTemplate);
    this.PS4ControllerTemplate = await UIManager.LoadAsset<InputController>(this._ps4ControllerTemplate);
    this.PS5ControllerTemplate = await UIManager.LoadAsset<InputController>(this._ps5ControllerTemplate);
    this.SwitchProControllerTemplate = await UIManager.LoadAsset<InputController>(this._switchProControllerTemplate);
    this.SwitchJoyConsTemplate = await UIManager.LoadAsset<InputController>(this._switchJoyConsDetachedControllerTemplate);
    this.SwitchJoyConsDockedTemplate = await UIManager.LoadAsset<InputController>(this._switchJoyConsControllerTemplate);
    this.SwitchHandheldTemplate = await UIManager.LoadAsset<InputController>(this._switchHandheldControllerTemplate);
  }

  public async System.Threading.Tasks.Task LoadPersistentGameAssets()
  {
    this.PauseMenuController = await UIManager.LoadAsset<UIPauseMenuController>(this._pauseMenuTemplate);
    this.PauseDetailsMenuTemplate = await UIManager.LoadAsset<UIPauseDetailsMenuController>(this._pauseDetailsMenuTemplate);
    this.TutorialOverlayTemplate = await UIManager.LoadAsset<UITutorialOverlayController>(this._tutorialOverlayTemplate);
    this.TutorialMenuTemplate = await UIManager.LoadAsset<UITutorialMenuController>(this._tutorialMenuTemplate);
    this.FollowerSummaryMenuTemplate = await UIManager.LoadAsset<UIFollowerSummaryMenuController>(this._followerSummaryMenuTemplate);
    this.FollowerInformationBoxTemplate = await UIManager.LoadAsset<FollowerInformationBox>(this._followerInformationBoxTemplate);
    this.FollowerSelectMenuTemplate = await UIManager.LoadAsset<UIFollowerSelectMenuController>(this._followerSelectMenuTemplate);
    this.DeathScreenOverlayTemplate = await UIManager.LoadAsset<UIDeathScreenOverlayController>(this._deathScreenOverlayTemplate);
    this.TarotCardsMenuTemplate = await UIManager.LoadAsset<UITarotCardsMenuController>(this._tarotCardsMenuTemplate);
    this.KeyScreenTemplate = await UIManager.LoadAsset<UIKeyScreenOverlayController>(this._keyScreenOverlayTemplate);
    this.ItemSelectorOverlayTemplate = await UIManager.LoadAsset<UIItemSelectorOverlayController>(this._itemSelectorTemplate);
    this.NewItemOverlayTemplate = await UIManager.LoadAsset<UINewItemOverlayController>(this._newItemOverlayTemplate);
    this.BugReportingOverlayTemplate = await UIManager.LoadAsset<UIBugReportingOverlayController>(this._bugReportingOverlayTemplate);
    await this.LoadWorldMap();
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
    UIManager.UnloadAsset<UIBugReportingOverlayController>(this.BugReportingOverlayTemplate);
    this.UnloadWorldMap();
  }

  public async System.Threading.Tasks.Task LoadBaseAssets()
  {
    SeasonalEventData activeEvent = SeasonalEventManager.GetActiveEvent();
    if ((UnityEngine.Object) activeEvent != (UnityEngine.Object) null)
      await activeEvent.LoadUIAssets();
    this.IndoctrinationMenuTemplate = await UIManager.LoadAsset<UIFollowerIndoctrinationMenuController>(this._indoctrinationMenuTemplate);
    this.SacrificeFollowerMenuTemplate = await UIManager.LoadAsset<UIFollowerSelectMenuController>(this._sacrificeFollowerMenuTemplate);
    this.UpgradeTreeMenuTemplate = await UIManager.LoadAsset<UIUpgradeTreeMenuController>(this._upgradeTreeTemplate);
    this.UpgradePlayerTreeMenuTemplate = await UIManager.LoadAsset<UIUpgradePlayerTreeMenuController>(this._upgradePlayerTreeTemplate);
    this.KitchenMenuTemplate = await UIManager.LoadAsset<UIKitchenMenuController>(this._kitchenMenuTemplate);
    this.PlayerUpgradesMenuTemplate = await UIManager.LoadAsset<UIPlayerUpgradesMenuController>(this._playerUpgradesMenuTemplate);
    this.AltarMenuTemplate = await UIManager.LoadAsset<UIAltarMenuController>(this._altarMenuTemplate);
    this.AppearanceMenuColourTemplate = await UIManager.LoadAsset<UIAppearanceMenuController_Colour>(this._appearanceMenuColourTemplate);
    this.AppearanceMenuFormTemplate = await UIManager.LoadAsset<UIAppearanceMenuController_Form>(this._appearanceMenuFormTemplate);
    this.AppearanceMenuVariantTemplate = await UIManager.LoadAsset<UIAppearanceMenuController_Variant>(this._appearanceMenuVariantTemplate);
    this.CultNameMenuTemplate = await UIManager.LoadAsset<UICultNameMenuController>(this._cultNameMenuTemplate);
    this.DoctrineMenuTemplate = await UIManager.LoadAsset<UIDoctrineMenuController>(this._doctrineMenuTemplate);
    this.BuildMenuTemplate = await UIManager.LoadAsset<UIBuildMenuController>(this._buildMenuTemplate);
    this.FollowerFormsMenuTemplate = await UIManager.LoadAsset<UIFollowerFormsMenuController>(this._followerFormsMenuTemplate);
    this.DoctrineChoicesMenuTemplate = await UIManager.LoadAsset<UIDoctrineChoicesMenuController>(this._doctrineChoicesMenuTemplate);
    this.RitualsMenuTemplate = await UIManager.LoadAsset<UIRitualsMenuController>(this._ritualsMenuTemplate);
    this.SermonWheelTemplate = await UIManager.LoadAsset<UISermonWheelController>(this._sermonWheelTemplate);
    this.DemonSummonTemplate = await UIManager.LoadAsset<UIDemonSummonMenuController>(this._demonSummonMenuTemplate);
    this.DemonMenuTemplate = await UIManager.LoadAsset<UIDemonMenuController>(this._demonMenuTemplate);
    this.PrisonMenuTemplate = await UIManager.LoadAsset<UIPrisonMenuController>(this._prisonMenuControllerTemplate);
    this.PrisonerMenuTemplate = await UIManager.LoadAsset<UIPrisonerMenuController>(this._prisonerMenuControllerTemplate);
    this.MissionaryMenuTemplate = await UIManager.LoadAsset<UIMissionaryMenuController>(this._missionaryMenuTemplate);
    this.MissionMenuTemplate = await UIManager.LoadAsset<UIMissionMenuController>(this._missionMenuControllerTemplate);
    this.RefineryMenuTemplate = await UIManager.LoadAsset<UIRefineryMenuController>(this._refineryMenuControllerTemplate);
    this.CookingMinigameOverlayControllerTemplate = await UIManager.LoadAsset<UICookingMinigameOverlayController>(this._cookingMinigameOverlayTemplate);
    if (!DataManager.Instance.DifficultyChosen)
      this.DifficultySelectorTemplate = await UIManager.LoadAsset<UIDifficultySelectorOverlayController>(this._difficultySelectorTemplate);
    this.MissionaryFollowerItemTemplate = await UIManager.LoadAsset<MissionaryFollowerItem>(this._missionaryFollowerItemTemplate);
    this.FollowerColourItemTemplate = await UIManager.LoadAsset<IndoctrinationColourItem>(this._followerColourItemTemplate);
    this.FollowerFormItemTemplate = await UIManager.LoadAsset<IndoctrinationFormItem>(this._followerFormItemTemplate);
    this.FollowerVariantItemTemplate = await UIManager.LoadAsset<IndoctrinationVariantItem>(this._followerVariantItemTemplate);
    this.DemonFollowerItemTemplate = await UIManager.LoadAsset<DemonFollowerItem>(this._demonFollowerItemTemplate);
    this.FollowerInteractionWheelTemplate = await UIManager.LoadAsset<UIFollowerInteractionWheelOverlayController>(this._followerInteractionWheelTemplate);
    this.TwitchFollowerSelectOverlayController = await UIManager.LoadAsset<UITwitchFollowerSelectOverlayController>(this._twitchFollowerSelect);
    this.TwitchTotemWheelController = await UIManager.LoadAsset<UITwitchTotemWheel>(this._twitchTotemWheel);
  }

  public void UnloadBaseAssets()
  {
    SeasonalEventData activeEvent = SeasonalEventManager.GetActiveEvent();
    if ((UnityEngine.Object) activeEvent != (UnityEngine.Object) null)
      activeEvent.UnloadUIAssets();
    UIManager.UnloadAsset<UIFollowerIndoctrinationMenuController>(this.IndoctrinationMenuTemplate);
    UIManager.UnloadAsset<UIFollowerSelectMenuController>(this.SacrificeFollowerMenuTemplate);
    UIManager.UnloadAsset<UIUpgradeTreeMenuController>(this.UpgradeTreeMenuTemplate);
    UIManager.UnloadAsset<UIUpgradePlayerTreeMenuController>(this.UpgradePlayerTreeMenuTemplate);
    UIManager.UnloadAsset<UIKitchenMenuController>(this.KitchenMenuTemplate);
    UIManager.UnloadAsset<UIPlayerUpgradesMenuController>(this.PlayerUpgradesMenuTemplate);
    UIManager.UnloadAsset<UIAltarMenuController>(this.AltarMenuTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Colour>(this.AppearanceMenuColourTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Form>(this.AppearanceMenuFormTemplate);
    UIManager.UnloadAsset<UIAppearanceMenuController_Variant>(this.AppearanceMenuVariantTemplate);
    UIManager.UnloadAsset<UICultNameMenuController>(this.CultNameMenuTemplate);
    UIManager.UnloadAsset<UIDoctrineMenuController>(this.DoctrineMenuTemplate);
    UIManager.UnloadAsset<UIBuildMenuController>(this.BuildMenuTemplate);
    UIManager.UnloadAsset<UIFollowerFormsMenuController>(this.FollowerFormsMenuTemplate);
    UIManager.UnloadAsset<UIDoctrineChoicesMenuController>(this.DoctrineChoicesMenuTemplate);
    UIManager.UnloadAsset<UIRitualsMenuController>(this.RitualsMenuTemplate);
    UIManager.UnloadAsset<UISermonWheelController>(this.SermonWheelTemplate);
    UIManager.UnloadAsset<UIDemonSummonMenuController>(this.DemonSummonTemplate);
    UIManager.UnloadAsset<UIDemonMenuController>(this.DemonMenuTemplate);
    UIManager.UnloadAsset<UIPrisonMenuController>(this.PrisonMenuTemplate);
    UIManager.UnloadAsset<UIPrisonerMenuController>(this.PrisonerMenuTemplate);
    UIManager.UnloadAsset<UIMissionaryMenuController>(this.MissionaryMenuTemplate);
    UIManager.UnloadAsset<UIMissionMenuController>(this.MissionMenuTemplate);
    UIManager.UnloadAsset<UIRefineryMenuController>(this.RefineryMenuTemplate);
    UIManager.UnloadAsset<UICookingMinigameOverlayController>(this.CookingMinigameOverlayControllerTemplate);
    UIManager.UnloadAsset<UIDifficultySelectorOverlayController>(this.DifficultySelectorTemplate);
    UIManager.UnloadAsset<MissionaryFollowerItem>(this.MissionaryFollowerItemTemplate);
    UIManager.UnloadAsset<IndoctrinationColourItem>(this.FollowerColourItemTemplate);
    UIManager.UnloadAsset<IndoctrinationFormItem>(this.FollowerFormItemTemplate);
    UIManager.UnloadAsset<IndoctrinationVariantItem>(this.FollowerVariantItemTemplate);
    UIManager.UnloadAsset<DemonFollowerItem>(this.DemonFollowerItemTemplate);
    UIManager.UnloadAsset<UIFollowerInteractionWheelOverlayController>(this.FollowerInteractionWheelTemplate);
    UIManager.UnloadAsset<UITwitchFollowerSelectOverlayController>(this.TwitchFollowerSelectOverlayController);
    UIManager.UnloadAsset<UITwitchTotemWheel>(this.TwitchTotemWheelController);
  }

  public async System.Threading.Tasks.Task LoadDungeonAssets()
  {
    this.InventoryPromptTemplate = await UIManager.LoadAsset<UIInventoryPromptOverlay>(this._inventoryPromptTemplate);
    this.AdventureMapOverlayTemplate = await UIManager.LoadAsset<UIAdventureMapOverlayController>(this._adventureMapOverlayTemplate);
    this.TarotChoiceOverlayTemplate = await UIManager.LoadAsset<UITarotChoiceOverlayController>(this._tarotChoiceOverlayTemplate);
    this.FleeceTarotChoiceOverlayTemplate = await UIManager.LoadAsset<UIFleeceTarotRewardOverlayController>(this._fleeceTarotRewardOverlayTemplate);
    this.AdventureMapNodeTemplate = await UIManager.LoadAsset<AdventureMapNode>(this._adventureMapNodeTemplate);
  }

  public void UnloadDungeonAssets()
  {
    UIManager.UnloadAsset<UIInventoryPromptOverlay>(this.InventoryPromptTemplate);
    UIManager.UnloadAsset<UIAdventureMapOverlayController>(this.AdventureMapOverlayTemplate);
    UIManager.UnloadAsset<UITarotChoiceOverlayController>(this.TarotChoiceOverlayTemplate);
    UIManager.UnloadAsset<UIFleeceTarotRewardOverlayController>(this.FleeceTarotChoiceOverlayTemplate);
    UIManager.UnloadAsset<AdventureMapNode>(this.AdventureMapNodeTemplate);
  }

  public async System.Threading.Tasks.Task LoadHubShoreAssets()
  {
    this.FishingOverlayControllerTemplate = await UIManager.LoadAsset<UIFishingOverlayController>(this._fishingOverlayTemplate);
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
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return default (T);
    MonoSingleton<UIManager>.Instance._addressablesTracker.Add(component.gameObject, (AsyncOperationHandle) asyncOperation);
    return component;
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

  public override void Start()
  {
    base.Start();
    this.transform.SetParent((Transform) null);
    this._cursorSize = this._cursor.width;
    this.UpdateCursorIfRequired();
  }

  private void UpdateCursorIfRequired()
  {
    if (this._currentResolution.width == Screen.width && this._currentResolution.height == Screen.height)
      return;
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
    renderTexture.name = "Cursor_Render_Texture" + (object) Time.frameCount;
    RenderTexture dest = renderTexture;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = dest;
    Graphics.Blit((Texture) this._cursor, dest);
    Texture2D texture2D = new Texture2D(num2, num2, TextureFormat.RGBA32, false);
    texture2D.name = "Cursor_Texture" + (object) Time.frameCount;
    this._currentCursorTexture = texture2D;
    this._currentCursorTexture.ReadPixels(new Rect(0.0f, 0.0f, (float) num2, (float) num2), 0, 0, false);
    this._currentCursorTexture.Apply();
    RenderTexture.active = active;
    dest.Release();
    dest.DiscardContents();
    UnityEngine.Object.Destroy((UnityEngine.Object) dest);
    Cursor.SetCursor(this._currentCursorTexture, new Vector2(18f * num1, 6f * num1), CursorMode.ForceSoftware);
  }

  public void Update()
  {
    GameManager instance = GameManager.GetInstance();
    if (!this.MenusBlocked && (UnityEngine.Object) instance != (UnityEngine.Object) null && (UnityEngine.Object) this._currentInstance == (UnityEngine.Object) null)
    {
      if (InputManager.Gameplay.GetPauseButtonDown() && !CheatConsole.IN_DEMO)
        this.ShowPauseMenu();
      else if (InputManager.Gameplay.GetMenuButtonDown() && DataManager.Instance.HadInitialDeathCatConversation)
        this.ShowDetailsMenu();
    }
    this.UpdateCursorIfRequired();
  }

  public void ShowPauseMenu()
  {
    this.IsPaused = true;
    UIPauseMenuController pauseMenuController = this.SetMenu<UIPauseMenuController>(this.PauseMenuController, filter: true);
    pauseMenuController.OnHidden = pauseMenuController.OnHidden + (System.Action) (() => this.IsPaused = false);
  }

  public void ShowDetailsMenu()
  {
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

  public UITutorialOverlayController ShowTutorialOverlay(TutorialTopic topic, float delay = 0.0f)
  {
    UITutorialOverlayController menu = this.TutorialOverlayTemplate.Instantiate<UITutorialOverlayController>();
    menu.Show(topic, delay);
    return this.SetMenuInstance((UIMenuBase) menu) as UITutorialOverlayController;
  }

  public UIAltarMenuController ShowAltarMenu()
  {
    return this.SetMenu<UIAltarMenuController>(this.AltarMenuTemplate);
  }

  public UIUpgradeTreeMenuController ShowUpgradeTree(System.Action closeCallback = null)
  {
    UIUpgradeTreeMenuController upgradeTreeInstance = this.UpgradeTreeMenuTemplate.Instantiate<UIUpgradeTreeMenuController>();
    HUD_Manager.Instance.Hide(true);
    upgradeTreeInstance.Show();
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
      upgradeTreeInstance.Hide();
    });
    this.SetMenuInstance((UIMenuBase) upgradeTreeInstance);
    return upgradeTreeInstance;
  }

  public UIUpgradePlayerTreeMenuController ShowPlayerUpgradeTree()
  {
    UIUpgradePlayerTreeMenuController upgradeTreeInstance = this.UpgradePlayerTreeMenuTemplate.Instantiate<UIUpgradePlayerTreeMenuController>();
    upgradeTreeInstance.Show();
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
    menu.Show();
    UIPlayerUpgradesMenuController upgradesMenuController = menu;
    upgradesMenuController.OnHidden = upgradesMenuController.OnHidden + (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine()));
    return this.SetMenuInstance((UIMenuBase) menu, 1f) as UIPlayerUpgradesMenuController;
  }

  public UIItemSelectorOverlayController ShowItemSelector(
    List<InventoryItem.ITEM_TYPE> items,
    ItemSelector.Params parameters)
  {
    UIItemSelectorOverlayController menu = this.ItemSelectorOverlayTemplate.Instantiate<UIItemSelectorOverlayController>();
    menu.Show(items, parameters);
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    return menu;
  }

  public UIItemSelectorOverlayController ShowItemSelector(
    List<InventoryItem> items,
    ItemSelector.Params parameters)
  {
    UIItemSelectorOverlayController menu = this.ItemSelectorOverlayTemplate.Instantiate<UIItemSelectorOverlayController>();
    menu.Show(items, parameters);
    this.SetMenuInstance((UIMenuBase) menu, 1f);
    return menu;
  }

  public UIDifficultySelectorOverlayController ShowDifficultySelector()
  {
    if (!((UnityEngine.Object) this.DifficultySelectorTemplate == (UnityEngine.Object) null))
      return this.SetMenu<UIDifficultySelectorOverlayController>(this.DifficultySelectorTemplate);
    Debug.LogError((object) "DifficultySelectorTemplate is null! Null reference or error in loading/unloading process");
    return (UIDifficultySelectorOverlayController) null;
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

  public UISaveErrorMenuController ShowSaveError()
  {
    UISaveErrorMenuController menu = this.SaveErrorWindowTemplate.Instantiate<UISaveErrorMenuController>();
    if (UIMenuBase.ActiveMenus.Count > 0)
      UIMenuBase.ActiveMenus.LastElement<UIMenuBase>().PushInstance<UISaveErrorMenuController>(menu);
    menu.Show();
    this.SetMenuInstance((UIMenuBase) menu);
    return menu;
  }

  public UIFollowerIndoctrinationMenuController ShowIndoctrinationMenu(Follower follower)
  {
    UIFollowerIndoctrinationMenuController menu = this.IndoctrinationMenuTemplate.Instantiate<UIFollowerIndoctrinationMenuController>();
    menu.Show(follower);
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

  private void DoShowDeathScreen(UIDeathScreenOverlayController deathScreenInstance)
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

  private T SetMenu<T>(T template, float timeScale = 0.0f, bool filter = false) where T : UIMenuBase
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

  public static EventInstance CreateLoop(string soundPath)
  {
    return (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? AudioManager.Instance.CreateLoop(soundPath, PlayerFarming.Instance.gameObject, true) : AudioManager.Instance.CreateLoop(soundPath, true);
  }

  public UIPauseMenuController PauseMenuController { private set; get; }

  public UISettingsMenuController SettingsMenuControllerTemplate => this._settingsMenuTemplate;

  public UICreditsMenuController CreditsMenuControllerTemplate { private set; get; }

  public UIPauseDetailsMenuController PauseDetailsMenuTemplate { private set; get; }

  public FollowerInformationBox FollowerInformationBoxTemplate { private set; get; }

  public UIFollowerIndoctrinationMenuController IndoctrinationMenuTemplate { private set; get; }

  public UIUpgradeTreeMenuController UpgradeTreeMenuTemplate { private set; get; }

  public UIUpgradePlayerTreeMenuController UpgradePlayerTreeMenuTemplate { private set; get; }

  public UIWorldMapMenuController WorldMapTemplate { private set; get; }

  public UITarotCardsMenuController TarotCardsMenuTemplate { private set; get; }

  public UICookingFireMenuController CookingFireMenuTemplate { private set; get; }

  public UIKitchenMenuController KitchenMenuTemplate { private set; get; }

  public UITutorialOverlayController TutorialOverlayTemplate { private set; get; }

  public UIKeyScreenOverlayController KeyScreenTemplate { private set; get; }

  public UIAltarMenuController AltarMenuTemplate { private set; get; }

  public UIPlayerUpgradesMenuController PlayerUpgradesMenuTemplate { private set; get; }

  public UIItemSelectorOverlayController ItemSelectorOverlayTemplate { private set; get; }

  public UIRecipeConfirmationOverlayController RecipeConfirmationTemplate { private set; get; }

  public UIDifficultySelectorOverlayController DifficultySelectorTemplate { private set; get; }

  public UIInventoryPromptOverlay InventoryPromptTemplate { private set; get; }

  public UIFollowerInteractionWheelOverlayController FollowerInteractionWheelTemplate { private set; get; }

  public UITarotChoiceOverlayController TarotChoiceOverlayTemplate { private set; get; }

  public UIFleeceTarotRewardOverlayController FleeceTarotChoiceOverlayTemplate { private set; get; }

  public UIFollowerSummaryMenuController FollowerSummaryMenuTemplate { private set; get; }

  public UIFollowerSelectMenuController FollowerSelectMenuTemplate { private set; get; }

  public UIDoctrineMenuController DoctrineMenuTemplate { private set; get; }

  public UIDoctrineChoicesMenuController DoctrineChoicesMenuTemplate { private set; get; }

  public UIRitualsMenuController RitualsMenuTemplate { private set; get; }

  public UISermonWheelController SermonWheelTemplate { private set; get; }

  public UIDemonSummonMenuController DemonSummonTemplate { private set; get; }

  public UIDemonMenuController DemonMenuTemplate { private set; get; }

  public UIPrisonMenuController PrisonMenuTemplate { private set; get; }

  public UIPrisonerMenuController PrisonerMenuTemplate { private set; get; }

  public UIMissionaryMenuController MissionaryMenuTemplate { private set; get; }

  public UIMissionMenuController MissionMenuTemplate { private set; get; }

  public UICultNameMenuController CultNameMenuTemplate { private set; get; }

  public UIKnuckleBonesController KnucklebonesTemplate { private set; get; }

  public UIKnucklebonesBettingSelectionController KnucklebonesBettingSelectionTemplate { private set; get; }

  public UIKnucklebonesOpponentSelectionController KnucklebonesOpponentSelectionTemplate { private set; get; }

  public UIRefineryMenuController RefineryMenuTemplate { private set; get; }

  public UITutorialMenuController TutorialMenuTemplate { private set; get; }

  public UIMenuConfirmationWindow ConfirmationWindowTemplate => this._confirmationWindowTemplate;

  public UIConfirmationCountdownWindow ConfirmationCountdownTemplate
  {
    get => this._confirmationCountdownWindowTemplate;
  }

  public UISaveErrorMenuController SaveErrorWindowTemplate { private set; get; }

  public UIDeathScreenOverlayController DeathScreenOverlayTemplate { private set; get; }

  public UIBuildMenuController BuildMenuTemplate { private set; get; }

  public UIFollowerFormsMenuController FollowerFormsMenuTemplate { private set; get; }

  public UINewItemOverlayController NewItemOverlayTemplate { private set; get; }

  public UIAppearanceMenuController_Form AppearanceMenuFormTemplate { private set; get; }

  public UIAppearanceMenuController_Colour AppearanceMenuColourTemplate { private set; get; }

  public UIAppearanceMenuController_Variant AppearanceMenuVariantTemplate { private set; get; }

  public IndoctrinationColourItem FollowerColourItemTemplate { private set; get; }

  public IndoctrinationFormItem FollowerFormItemTemplate { private set; get; }

  public IndoctrinationVariantItem FollowerVariantItemTemplate { private set; get; }

  public UIAdventureMapOverlayController AdventureMapOverlayTemplate { private set; get; }

  public AdventureMapNode AdventureMapNodeTemplate { private set; get; }

  public UIRoadmapOverlayController RoadmapOverlayControllerTemplate { private set; get; }

  public UIVideoExportMenuController VideoExportTemplate { private set; get; }

  public UIFishingOverlayController FishingOverlayControllerTemplate { private set; get; }

  public UICookingMinigameOverlayController CookingMinigameOverlayControllerTemplate { private set; get; }

  public MissionaryFollowerItem MissionaryFollowerItemTemplate { private set; get; }

  public DemonFollowerItem DemonFollowerItemTemplate { private set; get; }

  public UITwitchFollowerSelectOverlayController TwitchFollowerSelectOverlayController { private set; get; }

  public UITwitchTotemWheel TwitchTotemWheelController { private set; get; }

  public UIFollowerSelectMenuController SacrificeFollowerMenuTemplate { private set; get; }

  public UILoadingOverlayController LoadingOverlayControllerTemplate { private set; get; }

  public UIBugReportingOverlayController BugReportingOverlayTemplate { private set; get; }

  public UIControlBindingOverlayController BindingOverlayControllerTemplate { private set; get; }

  public UIDropdownOverlayController DropdownOverlayControllerTemplate { private set; get; }

  public InputController XboxControllerTemplate { private set; get; }

  public InputController PS4ControllerTemplate { private set; get; }

  public InputController PS5ControllerTemplate { private set; get; }

  public InputController SwitchJoyConsTemplate { private set; get; }

  public InputController SwitchJoyConsDockedTemplate { private set; get; }

  public InputController SwitchHandheldTemplate { private set; get; }

  public InputController SwitchProControllerTemplate { private set; get; }

  public InputController KeyboardTemplate { private set; get; }

  public InputController MouseTemplate { private set; get; }
}
