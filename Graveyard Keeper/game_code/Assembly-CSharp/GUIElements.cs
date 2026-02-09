// Decompiled with JetBrains decompiler
// Type: GUIElements
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(0)]
public class GUIElements : MonoBehaviour
{
  public static GUIElements _instance;
  public UIWidget screen_size_widget;
  public WidgetsBubbleGUI bubble_widgets_container;
  public GameGUI game_gui;
  public MainMenuGUI main_menu;
  public InGameMenuGUI ingame_menu;
  public OptionsMenuGUI options;
  public LiveStreamingGUI live_streaming;
  public SaveSlotsMenuGUI saves;
  public TechTreeGUI tech_tree;
  public TimeMachineGUI time_machine_gui;
  public GlobalCraftControlGUI global_craft_control_gui;
  public SleepGUI sleep_gui;
  public WaitingGUI waiting_gui;
  public DialogGUI dialog;
  public TechUnlockDialogGUI tech_dialog;
  public LoadingGUI loading;
  public UILabel time_label;
  public CraftGUI craft;
  public ResourceBasedCraftGUI resource_based_craft;
  public PrayCraftGUI pray_craft;
  public MixedCraftGUI mixed_craft;
  public MixedCraftGUI mixed_craft_tabbed;
  public BodyCraftGUI body_craft;
  public InventoryGUI inventory;
  public EquipToToolbarGUI equip_to_toolbar;
  public VendorGUI vendor;
  public ChestGUI chest;
  public ItemCountGUI item_count;
  public BuffsGUI buffs;
  public EffectBubblesManager effect_bubbles;
  public AutopsyGUI autopsy;
  public SoulExtractorGUI soul_extractor_gui;
  public SoulContainerGUI soul_container_gui;
  public SoulHealerGUI soul_healer_gui;
  public OrganEnhancerGUI organ_enhancer_gui;
  public TextWindowGUI text_window;
  public CinematicTextGUI cinematic_text;
  public HUD hud;
  public SpeechBubbleGUI speech_bubble;
  public TooltipBubbleGUI tooltip_bubble;
  public ContextMenuBubbleGUI context_menu_bubble;
  public InteractionBubbleGUI interaction_bubble;
  public MultiAnswerGUI multi_answer;
  public CornerTalkGUI corner_talk;
  public RelationGUI relation;
  public RelationGUI relation_additional;
  public BuildModeGUI build_mode_gui;
  public CraftResourcesSelectGUI resource_picker;
  public DropResHint drop_res_hint;
  public UIPanel drop_hint_panel;
  public GraveGUI grave;
  public BuildsGUI builds;
  public QuestListGUI quest_list;
  public OverheadGUI overhead;
  public TutorialArrowGUI tutorial_arrow;
  public BuffsBarGUI buffs_bar;
  public TutorialGUI tutorial;
  public FishingGUI fishing;
  public DungeonWindowGUI dungeon_window;
  public GameObject mixer_voting;
  public UI2DSprite mixer_progress_bar;
  public NPCsListGUI npcs_list;
  public UIPanel overhead_panel;
  public PrayReportGUI pray_report;
  public TavernEventReportGUI tavern_event_report;
  public TechPointsSpawner tech_points_spawner;
  public BodyStorageGUI body_storage;
  public MapGUI map;
  public bool _initialized;
  public bool hud_enabled = true;
  public static bool gui_is_initializing;
  public FlyingObject flying_buff_prefab;
  public DiskIndicatorGUI disk_indicator;
  public UIPanel buffs_panel;
  public CreditsGUI credits;
  public UIPanel black_background;
  public Camera half_resolution_camera;
  public MeshRenderer half_resolution_mesh;
  public PorterStationGUI porter_station;
  public ResurrectionGUI resurrection_gui;
  public RatCellGUI rat_cell_gui;
  public IllustrationsGUI illustrations_gui;
  public TutorialWindowsGUI tutorial_windows_gui;
  public NewBodyArrivedGUI body_arrived_gui;

  public static GUIElements me
  {
    get
    {
      if ((UnityEngine.Object) GUIElements._instance == (UnityEngine.Object) null)
        GUIElements._instance = UnityEngine.Object.FindObjectOfType<GUIElements>();
      return GUIElements._instance;
    }
  }

  public static bool dont_allow_change_input_method
  {
    get
    {
      if (BaseGUI.all_guis_closed)
        return false;
      if (BaseGUI.opened_windows.Count > 1)
        return true;
      return BaseGUI.opened_windows.Count == 1 && (UnityEngine.Object) BaseGUI.opened_windows[0] != (UnityEngine.Object) GUIElements.me.main_menu;
    }
  }

  public void OnEnable()
  {
    if ((UnityEngine.Object) GUIElements._instance == (UnityEngine.Object) null)
      GUIElements._instance = this;
    foreach (AlwaysActiveOrInactive componentsInChild in this.GetComponentsInChildren<AlwaysActiveOrInactive>(true))
      componentsInChild.UpdateState();
  }

  public void Init()
  {
    foreach (AlwaysActiveOrInactive componentsInChild in this.GetComponentsInChildren<AlwaysActiveOrInactive>(true))
      componentsInChild.UpdateState();
    if (this._initialized)
      return;
    this._initialized = true;
    this.RecalcScreenResolution();
    this.disk_indicator.gameObject.SetActive(false);
    GUIElements.gui_is_initializing = true;
    this.ReLinkElements();
    LazyInput.on_input_changed += new System.Action(BaseGUI.UpdateSourceType);
    GUIElements._instance = this;
    this.text_window.Init();
    this.cinematic_text.Init();
    this.sleep_gui.Init();
    this.waiting_gui.Init();
    this.craft.Init();
    this.global_craft_control_gui.Init();
    this.resource_based_craft.Init();
    this.pray_craft.Init();
    this.mixed_craft.Init();
    this.body_craft.Init();
    this.inventory.Init();
    this.equip_to_toolbar.Init();
    this.vendor.Init();
    this.chest.Init();
    this.item_count.Init();
    this.effect_bubbles.Init();
    this.buffs.Init();
    this.hud.Init();
    DropCollectGUI componentInChildren = this.GetComponentInChildren<DropCollectGUI>(true);
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      componentInChildren.Start();
    this.autopsy.Init();
    this.soul_extractor_gui.Init();
    this.soul_container_gui.Init();
    this.soul_healer_gui.Init();
    this.organ_enhancer_gui.Init();
    if ((UnityEngine.Object) this.tech_tree != (UnityEngine.Object) null)
      this.tech_tree.Init();
    if ((UnityEngine.Object) this.build_mode_gui != (UnityEngine.Object) null)
      this.build_mode_gui.Init();
    if ((UnityEngine.Object) this.quest_list != (UnityEngine.Object) null)
      this.quest_list.Init();
    if ((UnityEngine.Object) this.tutorial_arrow != (UnityEngine.Object) null)
      this.tutorial_arrow.Init();
    if ((UnityEngine.Object) this.drop_res_hint != (UnityEngine.Object) null)
      this.drop_res_hint.Init();
    if ((UnityEngine.Object) this.body_storage != (UnityEngine.Object) null)
      this.body_storage.Init();
    if ((UnityEngine.Object) this.mixed_craft_tabbed != (UnityEngine.Object) null)
      this.mixed_craft_tabbed.Init();
    this.bubble_widgets_container.InitWidgetsContainer();
    this.tooltip_bubble.Init();
    this.context_menu_bubble.Init();
    this.interaction_bubble.Init();
    this.speech_bubble.Init();
    this.multi_answer.Init();
    this.corner_talk.Init();
    this.relation.Init();
    this.relation_additional.Init();
    this.game_gui.Init();
    this.main_menu.Init();
    this.ingame_menu.Init();
    this.options.Init();
    this.saves.Init();
    this.grave.Init();
    this.resource_picker.Init();
    this.live_streaming.Init();
    this.dialog.Init();
    this.tech_dialog.Init();
    this.tutorial.Init();
    this.loading.Init();
    this.pray_report.Init();
    this.tavern_event_report.Init();
    this.builds?.Init();
    this.buffs_bar?.Init();
    this.fishing?.Init();
    this.map?.Init();
    this.porter_station?.Init();
    this.resurrection_gui?.Init();
    this.rat_cell_gui?.Init();
    this.illustrations_gui?.Init();
    this.tutorial_windows_gui?.Init();
    this.npcs_list.Init();
    this.time_machine_gui.Init();
    this.hud.Open();
    this.UpdateGUISizeSettings();
    if (MainGame.loaded_from_scene_main)
      this.main_menu.Open(true);
    UITextStyles.me.Deactivate<UITextStyles>();
    if ((UnityEngine.Object) this.drop_hint_panel != (UnityEngine.Object) null)
      this.drop_hint_panel.gameObject.SetActive(true);
    if ((UnityEngine.Object) this.flying_buff_prefab != (UnityEngine.Object) null)
      this.flying_buff_prefab.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.buffs_panel != (UnityEngine.Object) null)
      this.buffs_panel.gameObject.SetActive(false);
    this.credits.Init();
    if ((UnityEngine.Object) this.credits != (UnityEngine.Object) null)
      this.credits.SetActive(false);
    if ((UnityEngine.Object) this.black_background != (UnityEngine.Object) null)
      this.black_background.gameObject.SetActive(false);
    this.dungeon_window.Init();
    GUIElements.gui_is_initializing = false;
    PlatformSpecific.ApplyScreenSafeZones();
    GUIElements.UpdateLanguageChangeForAllBaseGUI();
  }

  public void RecalcScreenResolution(int w = -1, int h = -1)
  {
    if ((UnityEngine.Object) this.screen_size_widget == (UnityEngine.Object) null)
      return;
    if (w == -1)
      w = Screen.width;
    if (h == -1)
      h = Screen.height;
    this.screen_size_widget.leftAnchor.target = this.screen_size_widget.rightAnchor.target = this.screen_size_widget.topAnchor.target = this.screen_size_widget.bottomAnchor.target = (Transform) null;
    this.screen_size_widget.width = w / 2;
    this.screen_size_widget.height = h / 2;
    ResolutionConfig resolutionConfigOrNull = ResolutionConfig.GetResolutionConfigOrNull(w, h);
    if (resolutionConfigOrNull == null)
      return;
    this.game_gui.transform.localScale = this.inventory.transform.localScale = this.map.transform.localScale = this.npcs_list.transform.localScale = this.tech_tree.transform.localScale = Vector3.one * resolutionConfigOrNull.large_gui_scale;
  }

  public void Update()
  {
    if ((UnityEngine.Object) MainGame.me == (UnityEngine.Object) null || MainGame.me.save == null)
      return;
    this.time_label.text = "Time: " + MainGame.me.save.cur_time.ToString();
  }

  public void LateUpdate()
  {
    if (!((UnityEngine.Object) this.overhead != (UnityEngine.Object) null))
      return;
    this.overhead.CustomUpdate();
  }

  public void InitAtGameStart()
  {
  }

  public void OpenCraftGUI(WorldGameObject craftery_wgo)
  {
    if (craftery_wgo.components.craft.enabled && craftery_wgo.components.craft.is_crafting && !craftery_wgo.obj_def.can_insert_zombie)
      return;
    if (craftery_wgo.is_body_storage)
      this.body_storage.Open(craftery_wgo);
    else if (craftery_wgo.is_autopsy_table)
      this.autopsy.Open(craftery_wgo);
    else if (craftery_wgo.obj_def.HasObjectGroupWithID("pulpit"))
      this.pray_craft.Open(craftery_wgo);
    else if (craftery_wgo.obj_def.HasObjectGroupWithID("balsamation"))
      this.body_craft.Open(craftery_wgo);
    else if (craftery_wgo.is_rat_cell)
      this.rat_cell_gui.Open(craftery_wgo);
    else if (craftery_wgo.is_soul_extractor_table)
      this.soul_extractor_gui.Open(craftery_wgo);
    else if (craftery_wgo.obj_id == "soul_healer")
    {
      this.soul_healer_gui.Open(craftery_wgo);
    }
    else
    {
      if (craftery_wgo.obj_id == "soul_workbench")
        craftery_wgo.components.craft.FillCraftsList();
      foreach (CraftDefinition craft in craftery_wgo.components.craft.crafts)
      {
        switch (craft.craft_type)
        {
          case CraftDefinition.CraftType.ResourcesBasedCraft:
          case CraftDefinition.CraftType.Survey:
          case CraftDefinition.CraftType.AlchemyDecompose:
            this.resource_based_craft.Open(craftery_wgo, craft.craft_type);
            return;
          case CraftDefinition.CraftType.MixedCraft:
            if (craftery_wgo.obj_def.filter_craft_subtype == CraftDefinition.CraftSubType.Alchemy)
            {
              this.craft.OpenAsAlchemy(craftery_wgo);
              return;
            }
            List<string> prodlist = craftery_wgo.obj_def.res_product_types;
            this.mixed_craft.Open(craftery_wgo, craftery_wgo.obj_def.craft_preset, false, (InventoryWidget.ItemFilterDelegate) ((item, widget) =>
            {
              if (item.definition == null)
                return InventoryWidget.ItemFilterResult.Hide;
              if (prodlist.Count == 0)
                return InventoryWidget.ItemFilterResult.Active;
              foreach (string productType in item.definition.product_types)
              {
                if (prodlist.Contains(productType))
                  return InventoryWidget.ItemFilterResult.Active;
              }
              return InventoryWidget.ItemFilterResult.Inactive;
            }));
            return;
          case CraftDefinition.CraftType.PrayCraft:
            this.pray_craft.Open(craftery_wgo);
            return;
          default:
            continue;
        }
      }
      this.craft.OpenCraftList(craftery_wgo);
    }
  }

  public void ReLinkElements()
  {
    this.craft = this.GetComponentInChildren<CraftGUI>(true);
    this.resource_based_craft = this.GetComponentInChildren<ResourceBasedCraftGUI>(true);
    this.pray_craft = this.GetComponentInChildren<PrayCraftGUI>(true);
    this.mixed_craft = this.GetComponentInChildren<MixedCraftGUI>(true);
    this.sleep_gui = this.GetComponentInChildren<SleepGUI>(true);
    this.waiting_gui = this.GetComponentInChildren<WaitingGUI>(true);
    this.inventory = this.GetComponentInChildren<InventoryGUI>(true);
    this.equip_to_toolbar = this.GetComponentInChildren<EquipToToolbarGUI>(true);
    this.vendor = this.GetComponentInChildren<VendorGUI>(true);
    this.chest = this.GetComponentInChildren<ChestGUI>(true);
    this.item_count = this.GetComponentInChildren<ItemCountGUI>(true);
    this.buffs = this.GetComponentInChildren<BuffsGUI>(true);
    this.effect_bubbles = this.GetComponentInChildren<EffectBubblesManager>(true);
    this.autopsy = this.GetComponentInChildren<AutopsyGUI>(true);
    this.hud = this.GetComponentInChildren<HUD>(true);
    this.text_window = this.GetComponentInChildren<TextWindowGUI>(true);
    this.cinematic_text = this.GetComponentInChildren<CinematicTextGUI>(true);
    this.speech_bubble = this.GetComponentInChildren<SpeechBubbleGUI>(true);
    this.tooltip_bubble = this.GetComponentInChildren<TooltipBubbleGUI>(true);
    this.context_menu_bubble = this.GetComponentInChildren<ContextMenuBubbleGUI>(true);
    this.interaction_bubble = this.GetComponentInChildren<InteractionBubbleGUI>(true);
    this.multi_answer = this.GetComponentInChildren<MultiAnswerGUI>(true);
    this.corner_talk = this.GetComponentInChildren<CornerTalkGUI>(true);
    this.relation = this.GetComponentInChildren<RelationGUI>(true);
    this.game_gui = this.GetComponentInChildren<GameGUI>(true);
    this.main_menu = this.GetComponentInChildren<MainMenuGUI>(true);
    this.ingame_menu = this.GetComponentInChildren<InGameMenuGUI>(true);
    this.options = this.GetComponentInChildren<OptionsMenuGUI>(true);
    this.saves = this.GetComponentInChildren<SaveSlotsMenuGUI>(true);
    this.dialog = this.GetComponentInChildren<DialogGUI>(true);
    this.tech_dialog = this.GetComponentInChildren<TechUnlockDialogGUI>(true);
    this.loading = this.GetComponentInChildren<LoadingGUI>(true);
    this.builds = this.GetComponentInChildren<BuildsGUI>(true);
    this.tech_tree = this.GetComponentInChildren<TechTreeGUI>(true);
    this.build_mode_gui = this.GetComponentInChildren<BuildModeGUI>(true);
    this.overhead = this.GetComponentInChildren<OverheadGUI>(true);
    this.quest_list = this.GetComponentInChildren<QuestListGUI>(true);
    this.tutorial_arrow = this.GetComponentInChildren<TutorialArrowGUI>(true);
    this.grave = this.GetComponentInChildren<GraveGUI>(true);
    this.resource_picker = this.GetComponentInChildren<CraftResourcesSelectGUI>(true);
    this.drop_res_hint = this.GetComponentInChildren<DropResHint>(true);
    this.buffs_bar = this.GetComponentInChildren<BuffsBarGUI>(true);
    this.tutorial = this.GetComponentInChildren<TutorialGUI>(true);
    this.fishing = this.GetComponentInChildren<FishingGUI>(true);
    this.dungeon_window = this.GetComponentInChildren<DungeonWindowGUI>(true);
    this.body_storage = this.GetComponentInChildren<BodyStorageGUI>(true);
    this.porter_station = this.GetComponentInChildren<PorterStationGUI>(true);
    this.resurrection_gui = this.GetComponentInChildren<ResurrectionGUI>(true);
    this.rat_cell_gui = this.GetComponentInChildren<RatCellGUI>(true);
    this.illustrations_gui = this.GetComponentInChildren<IllustrationsGUI>(true);
    this.tutorial_windows_gui = this.GetComponentInChildren<TutorialWindowsGUI>(true);
  }

  public void UpdateGUISizeSettings()
  {
    this.GetComponentInChildren<Camera>().orthographicSize = 0.5f;
  }

  public static void ChangeHUDAlpha(bool show, bool animated)
  {
    GUIElements.me.hud.gameObject.TryFinishAlphaTween();
    GUIElements.me.buffs.buffs_hud.TryFinishAlphaTween();
    if (!animated)
    {
      GUIElements.me.hud.panel.alpha = show ? 1f : 0.0f;
      GUIElements.me.buffs.buffs_hud_panel.alpha = show ? 1f : 0.0f;
    }
    else
    {
      GUIElements.me.hud.panel.ChangeAlpha(GUIElements.me.hud.panel.alpha, show ? 1f : 0.0f, 0.2f);
      GUIElements.me.buffs.buffs_hud_panel.ChangeAlpha(GUIElements.me.buffs.buffs_hud_panel.alpha, show ? 1f : 0.0f, 0.2f);
    }
  }

  public static void ChangeBubblesVisibility(bool show)
  {
    EffectBubblesManager.ChangeBubblesVisibility(show);
    InteractionBubbleGUI.ChangeBubblesVisibility(show);
  }

  public void EnableHUD(bool enable)
  {
    Debug.Log((object) ("EnableHUD: " + enable.ToString()));
    this.hud.gameObject.SetActive(enable);
    this.hud_enabled = enable;
  }

  public bool IsAnyMassiveWindowOpened()
  {
    return this.craft.is_shown || this.grave.is_shown || this.inventory.is_shown || this.resource_based_craft.is_shown || this.resource_picker.is_shown || this.chest.is_shown || this.pray_craft.gameObject.activeSelf || this.fishing.is_shown || MainGame.me.gui_elements.build_mode_gui.is_shown;
  }

  public void ShowSavingStatus(bool show)
  {
    Debug.Log((object) ("ShowSavingStatus: " + show.ToString()));
    this.disk_indicator.SetActive(show);
    foreach (LocalizedLabel componentsInChild in this.disk_indicator.GetComponentsInChildren<LocalizedLabel>(true))
      componentsInChild.Localize();
  }

  public static void UpdateLanguageChangeForAllBaseGUI()
  {
    if ((UnityEngine.Object) GUIElements.me == (UnityEngine.Object) null)
      return;
    if (GUIElements.me.options.is_shown)
      GUIElements.me.options.UpdateLocalizedLabels();
    foreach (Component componentsInChild in GUIElements.me.GetComponentsInChildren<BaseGUI>(true))
      GJL.EnsureChildLabelsHasCorrectFont(componentsInChild.gameObject);
    GJL.EnsureChildLabelsHasCorrectFont(GUIElements.me.hud.gameObject);
  }

  public void CloseAllInGameWindows() => InteractionBubbleGUI.DestroyAll();

  public void OpenPorterStationGUI(WorldGameObject wgo) => this.porter_station.Open(wgo);

  public void OpenResurrectionGUI(WorldGameObject wgo) => this.resurrection_gui.Open(wgo);
}
