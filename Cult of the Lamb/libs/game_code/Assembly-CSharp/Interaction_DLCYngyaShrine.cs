// Decompiled with JetBrains decompiler
// Type: Interaction_DLCYngyaShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMTools;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_DLCYngyaShrine : Interaction
{
  public static Interaction_DLCYngyaShrine Instance;
  public SpriteXPBar XPBar;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public SimpleSetCamera SimpleSetCamera;
  public GameObject awoken;
  public GameObject ritual;
  public GameObject ritualWinter;
  public GameObject[] candles;
  [SerializeField]
  public GameObject ReceivePosition;
  [SerializeField]
  public SimpleSpineFlash StatueFlash;
  [SerializeField]
  public SimpleMaterialFlash BellFlash;
  public Interaction_SimpleConversation introConvo;
  [SerializeField]
  public Interaction_SimpleConversation findLostSoulsConvo;
  [SerializeField]
  public Interaction_SimpleConversation firepitConvo;
  [SerializeField]
  public Interaction_SimpleConversation onboardRotDungeonConvo;
  [SerializeField]
  public Interaction_SimpleConversation embraceRotConvo;
  [SerializeField]
  public Interaction_SimpleConversation rejectRotConvo;
  public Interaction_SimpleConversation winterRevealedConvo;
  public Interaction_SimpleConversation firstWinterConvo;
  public Interaction_SimpleConversation secondWinterIncrementConvo;
  public Interaction_SimpleConversation secondWinterIncrementConvo_B;
  public Interaction_SimpleConversation thirdWinterConvo;
  public Interaction_SimpleConversation fourthWinterIncrementConvo;
  public Interaction_SimpleConversation fourthWinterIncrementConvo_B;
  public Interaction_SimpleConversation fifthWinterConvo;
  public Interaction_SimpleConversation beatenWolfConvo;
  public Interaction_SimpleConversation requireLambsConvo;
  public Interaction_SimpleConversation timeToFight;
  [SerializeField]
  public Interaction_SimpleConversation[] miscConvos;
  [SerializeField]
  public GameObject notification;
  [SerializeField]
  public GameObject notification2;
  [SerializeField]
  public GameObject activatedShrine;
  [SerializeField]
  public GameObject deactivatedShrine;
  public bool Active;
  public int ghostsInAir;

  public event Interaction_DLCYngyaShrine.OnDepositEvent OnDepositGhosts;

  public void Awake() => Interaction_DLCYngyaShrine.Instance = this;

  public static int GetAmountRequiredForUpgrade()
  {
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem))
      return 4;
    return SeasonsManager.WinterSeverity <= 0 || SeasonsManager.WinterSeverity <= 1 || SeasonsManager.WinterSeverity <= 2 || SeasonsManager.WinterSeverity <= 3 || SeasonsManager.WinterSeverity <= 4 ? 12 : 16 /*0x10*/;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_DLCYngyaShrine.Instance = this;
    this.XPBar.Show();
    this.ActivateDistance = 3f;
    bool flag = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) > 0;
    this.Interactable = flag;
    this.notification.SetActive(flag);
    this.notification2.SetActive(flag);
    this.awoken.gameObject.SetActive(DataManager.Instance.OnboardedYngyaAwoken);
    if (DataManager.Instance.OnboardedYngyaAwoken)
    {
      foreach (GameObject candle in this.candles)
        candle.gameObject.SetActive(true);
    }
    this.StatueFlash.FlashWhite(false, 0.0f);
    this.BellFlash.FlashWhite(false, 0.0f);
    this.winterRevealedConvo.gameObject.SetActive(!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem) && DataManager.Instance.OnboardedRotstoneDungeon);
    this.winterRevealedConvo.Callback.RemoveAllListeners();
    this.winterRevealedConvo.Callback.AddListener((UnityAction) (() => WoolhavenYngyaStatue.PlayWinterIncrementGlobal(true)));
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem) && DataManager.Instance.OnboardedRotstoneDungeon && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) <= 0)
      this.winterRevealedConvo.Entries.RemoveAt(0);
    this.findLostSoulsConvo.gameObject.SetActive(!DataManager.Instance.OnboardedFindLostSouls && DataManager.Instance.OnboardedAddFuelToFurnace);
    if (this.findLostSoulsConvo.gameObject.activeSelf)
      BaseGoopDoor.WoolhavenDoor.SetDoorUp();
    this.XPBar.UpdateBar((float) DataManager.Instance.ShrineGhostJuice / (float) Interaction_DLCYngyaShrine.GetAmountRequiredForUpgrade());
    this.CheckReturnLastLambObjective();
  }

  public void Start()
  {
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.SimpleSetCamera.Play();
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.Active = false;
    this.SimpleSetCamera.Reset();
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) > 0)
      this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/DepositYngyaGhost") : "";
    else
      this.Label = "";
    this.SecondaryInteractable = false;
    this.HasSecondaryInteraction = false;
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_WinterChoice))
      return;
    this.SecondaryInteractable = true;
    this.HasSecondaryInteraction = true;
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = this.SecondaryInteractable ? (DataManager.Instance.WinterLoopModifiedDay != TimeManager.CurrentDay ? (DataManager.Instance.WinterLoopEnabled ? LocalizationManager.GetTranslation("Interactions/DisableWinter") : LocalizationManager.GetTranslation("Interactions/EnableWinter")) : ScriptLocalization.Interactions.Recharging) : "";
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.IndicateHighlighted(playerFarming);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_WinterChoice))
      return;
    playerFarming.indicator.ShowTopInfo(DataManager.Instance.WinterLoopEnabled ? LocalizationManager.GetTranslation("Interactions/WinterEnabled") : LocalizationManager.GetTranslation("Interactions/WinterDisabled"));
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.EndIndicateHighlighted(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.InteractYngyaShrine);
    if (this.Active)
      return;
    this.Active = true;
    this.StartCoroutine((IEnumerator) this.GiveGhostJuice());
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    DataManager.Instance.WinterLoopEnabled = !DataManager.Instance.WinterLoopEnabled;
    if (!DataManager.Instance.WinterLoopEnabled && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      this.StartCoroutine((IEnumerator) this.DisableWinterIE());
    else if (DataManager.Instance.WinterLoopEnabled && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      this.StartCoroutine((IEnumerator) this.EnableWinterIE());
    this.HasChanged = true;
  }

  public IEnumerator EnableWinterIE()
  {
    Interaction_DLCYngyaShrine interactionDlcYngyaShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcYngyaShrine.cameraTarget);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/winter_loop_enable");
    yield return (object) new WaitForSeconds(2f);
    interactionDlcYngyaShrine.activatedShrine.gameObject.SetActive(true);
    interactionDlcYngyaShrine.deactivatedShrine.gameObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", interactionDlcYngyaShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", interactionDlcYngyaShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy", interactionDlcYngyaShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", interactionDlcYngyaShrine.gameObject);
    BiomeConstants.Instance.EmitDisplacementEffect(interactionDlcYngyaShrine.gameObject.transform.position);
    WoolhavenYngyaStatue.Instance.Skeleton.AnimationState.SetAnimation(0, "next-bell", false);
    WoolhavenYngyaStatue.Instance.Skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    GameManager.GetInstance().OnConversationNext(interactionDlcYngyaShrine.cameraTarget, 12f);
    SeasonsManager.SetSeasonInstant = true;
    SeasonsManager.SetSeason(SeasonsManager.Season.Winter);
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Heavy, 2f);
    SeasonsManager.SetSeasonInstant = false;
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator DisableWinterIE()
  {
    Interaction_DLCYngyaShrine interactionDlcYngyaShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcYngyaShrine.cameraTarget);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/winter_loop_disable");
    yield return (object) new WaitForSeconds(2f);
    interactionDlcYngyaShrine.activatedShrine.gameObject.SetActive(false);
    interactionDlcYngyaShrine.deactivatedShrine.gameObject.SetActive(true);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", interactionDlcYngyaShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", interactionDlcYngyaShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy", interactionDlcYngyaShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", interactionDlcYngyaShrine.gameObject);
    SeasonsManager.StopWeatherEvent();
    SeasonsManager.SetSeason(SeasonsManager.Season.Spring);
    BiomeConstants.Instance.EmitDisplacementEffect(interactionDlcYngyaShrine.gameObject.transform.position);
    WoolhavenYngyaStatue.Instance.Skeleton.AnimationState.SetAnimation(0, "next-bell", false);
    WoolhavenYngyaStatue.Instance.Skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    GameManager.GetInstance().OnConversationNext(interactionDlcYngyaShrine.cameraTarget, 12f);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void DoorDown()
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) > 0)
      return;
    this.StartCoroutine((IEnumerator) this.DoorDownIE());
  }

  public IEnumerator DoorDownIE()
  {
    yield return (object) new WaitForEndOfFrame();
    while (LetterBox.IsPlaying)
      yield return (object) null;
    if (BaseGoopDoor.WoolhavenDoor.CanWoolhavenDoorOpen())
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(BaseGoopDoor.WoolhavenDoor.gameObject);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_camera_whoosh");
      yield return (object) new WaitForSeconds(1.5f);
      BaseGoopDoor.WoolhavenDoor.CheckWoolhavenDoor();
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_path_to_mountain_open");
      yield return (object) new WaitForSeconds(1.5f);
    }
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator GiveGhostJuice()
  {
    Interaction_DLCYngyaShrine interactionDlcYngyaShrine = this;
    while (interactionDlcYngyaShrine.Active && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) > 0 && InputManager.Gameplay.GetInteractButtonHeld())
    {
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST, -1);
      ++interactionDlcYngyaShrine.ghostsInAir;
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) <= 0)
        MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
      Vector3 position = interactionDlcYngyaShrine.playerFarming.transform.position;
      GameObject gameObject = GameObject.Find("GhostLostLamb");
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.activeInHierarchy)
      {
        Transform parent = gameObject.transform.parent;
        if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        {
          position = parent.position;
          BiomeConstants.Instance.EmitHitVFXSoul(parent.transform.position, Quaternion.identity);
          CameraManager.instance.ShakeCameraForDuration(0.25f, 0.5f, 0.1f);
          UnityEngine.Object.Destroy((UnityEngine.Object) parent.gameObject, 0.1f);
        }
        else
        {
          position = gameObject.transform.position;
          UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 0.1f);
        }
      }
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_fly", PlayerFarming.Instance.transform.position);
      SoulCustomTarget.Create(interactionDlcYngyaShrine.ReceivePosition, position, StaticColors.OffWhiteColor, new System.Action(interactionDlcYngyaShrine.\u003CGiveGhostJuice\u003Eb__55_0), sfxPath: " ", playDefaultSFX: false);
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) <= 0)
      {
        GameManager.GetInstance().OnConversationNew();
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DepositGhosts);
      }
      if (DataManager.Instance.ShrineGhostJuice + 1 >= Interaction_DLCYngyaShrine.GetAmountRequiredForUpgrade())
      {
        interactionDlcYngyaShrine.Active = false;
        interactionDlcYngyaShrine.HasChanged = true;
        break;
      }
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(0.2f);
    interactionDlcYngyaShrine.notification.SetActive(false);
    interactionDlcYngyaShrine.notification2.SetActive(false);
    interactionDlcYngyaShrine.HasChanged = true;
    interactionDlcYngyaShrine.Active = false;
  }

  public void AddGhostJuiceFromLostWool()
  {
    this.StartCoroutine((IEnumerator) this.AddGhostJuiceFromLostWoolIE());
  }

  public IEnumerator AddGhostJuiceFromLostWoolIE()
  {
    GameManager.SetGlobalOcclusionActive(false);
    yield return (object) new WaitForEndOfFrame();
    while (LetterBox.IsPlaying)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.cameraTarget, 11f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_ghost_awakened");
    yield return (object) new WaitForSeconds(2f);
    this.ghostsInAir = 4;
    bool blockGhostInAirCallback = DataManager.Instance.ShrineGhostJuice + this.ghostsInAir >= Interaction_DLCYngyaShrine.GetAmountRequiredForUpgrade() && DataManager.Instance.TotalShrineGhostJuice + this.ghostsInAir > 4;
    for (int index = 0; index < 4; ++index)
      this.ReceiveGhostJuice(false, blockGhostInAirCallback);
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    this.CheckReturnLastLambObjective();
  }

  public void CheckReturnLastLambObjective()
  {
    if (!DataManager.Instance.NPCGhostGeneric7Rescued || !DataManager.Instance.NPCGhostGeneric8Rescued || !DataManager.Instance.NPCGhostGeneric9Rescued || !DataManager.Instance.NPCGhostGeneric10Rescued || !DataManager.Instance.NPCGhostRancherRescued || !DataManager.Instance.NPCGhostTarotRescued || !DataManager.Instance.NPCGhostGraveyardRescued || !DataManager.Instance.NPCGhostDecoRescued || !DataManager.Instance.NPCGhostFlockadeRescued || !DataManager.Instance.NPCGhostBlacksmithRescued)
      return;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnLastLambGhosts);
  }

  public void ReceiveGhostJuice(bool playFlavourConvo = true, bool blockGhostInAirCallback = false)
  {
    --this.ghostsInAir;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_deposit", PlayerFarming.Instance.transform.position);
    Color color = Color.white;
    DOTween.To((DOGetter<Color>) (() => color), (DOSetter<Color>) (x =>
    {
      color = x;
      this.StatueFlash.SetColor(color);
    }), new Color(1f, 1f, 1f, 0.0f), 0.5f);
    DOTween.To((DOGetter<Color>) (() => color), (DOSetter<Color>) (x =>
    {
      color = x;
      this.BellFlash.SetColor(color);
    }), new Color(1f, 1f, 1f, 0.0f), 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.3f);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 0.1f);
    this.XPBar.Show();
    ++DataManager.Instance.ShrineGhostJuice;
    ++DataManager.Instance.TotalShrineGhostJuice;
    this.XPBar.UpdateBar((float) DataManager.Instance.ShrineGhostJuice / (float) Interaction_DLCYngyaShrine.GetAmountRequiredForUpgrade());
    foreach (ObjectivesData objective1 in DataManager.Instance.Objectives)
    {
      if (objective1 is Objectives_CollectItem objective2 && objective2.ItemType == InventoryItem.ITEM_TYPE.YNGYA_GHOST)
      {
        objective2.IncreaseCount();
        ObjectiveManager.UpdateObjective((ObjectivesData) objective2);
      }
    }
    if (DataManager.Instance.ShrineGhostJuice >= Interaction_DLCYngyaShrine.GetAmountRequiredForUpgrade() && DataManager.Instance.TotalShrineGhostJuice > 4)
    {
      DataManager.Instance.ShrineGhostJuice = 0;
      WoolhavenYngyaStatue.PlayWinterIncrementGlobal();
    }
    else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) + this.ghostsInAir <= 0 && !blockGhostInAirCallback)
    {
      if (DataManager.Instance.TotalShrineGhostJuice == 4)
      {
        WoolhavenYngyaStatue.PlayYngyaAwokenGlobal();
        DataManager.Instance.ShrineGhostJuice = 0;
      }
      else if (!DataManager.Instance.RevealedDLCMapDoor && DataManager.Instance.TotalShrineGhostJuice >= 12)
      {
        DataManager.Instance.RevealedDLCMapDoor = true;
        this.onboardRotDungeonConvo.Play();
        this.onboardRotDungeonConvo.Callback.AddListener((UnityAction) (() =>
        {
          MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
          GameManager.SetGlobalOcclusionActive(true);
        }));
      }
      else if (DataManager.Instance.TotalShrineGhostJuice >= 16 /*0x10*/ && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      {
        this.firepitConvo.Play();
        this.firepitConvo.Callback.AddListener((UnityAction) (() =>
        {
          MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
          GameManager.SetGlobalOcclusionActive(true);
        }));
      }
      else if (DataManager.Instance.TotalShrineGhostJuice == 8)
        GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() =>
        {
          MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
          GameManager.GetInstance().OnConversationEnd();
          GameManager.SetGlobalOcclusionActive(true);
        }));
      else if (DataManager.Instance.BeatenWolf && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnLastLambGhosts))
      {
        Interaction_DLCYngyaShrine.Instance.beatenWolfConvo.Play();
        Interaction_DLCYngyaShrine.Instance.beatenWolfConvo.Callback.AddListener((UnityAction) (() =>
        {
          MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
          GameManager.SetGlobalOcclusionActive(true);
        }));
        if (SeasonsManager.WinterSeverity < 6)
        {
          Interaction_DLCYngyaShrine.Instance.requireLambsConvo.Play();
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuryLostGhost", Objectives.CustomQuestTypes.ReturnLastLambGhosts), true, true);
        }
      }
      else if (playFlavourConvo && DataManager.Instance.YngyaMiscConvoIndex < this.miscConvos.Length && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) <= 0)
      {
        MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
        GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
        {
          this.miscConvos[DataManager.Instance.YngyaMiscConvoIndex].Play();
          ++DataManager.Instance.YngyaMiscConvoIndex;
          GameManager.SetGlobalOcclusionActive(true);
        }));
      }
      else
      {
        MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
        GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
        {
          GameManager.GetInstance().OnConversationEnd();
          GameManager.SetGlobalOcclusionActive(true);
        }));
      }
    }
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) + this.ghostsInAir <= 0 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) <= 0 && DataManager.Instance.TotalShrineGhostJuice >= 12)
      BaseGoopDoor.WoolhavenDoor.CheckWoolhavenDoor();
    Interaction_DLCYngyaShrine.OnDepositEvent onDepositGhosts = this.OnDepositGhosts;
    if (onDepositGhosts == null)
      return;
    onDepositGhosts();
  }

  public void HaroInitWinterQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/WarmCult", StructureBrain.TYPES.FURNACE_1), true, true);
    BaseGoopDoor.WoolhavenDoor.SetDoorUp();
    PlayerFarming.Instance.GetXP(1f);
  }

  public void GiveFirepitRitual() => this.StartCoroutine((IEnumerator) this.GiveFirepitRitualIE());

  public IEnumerator GiveFirepitRitualIE()
  {
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_FirePit_2);
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, 1, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    ObjectiveManager.Add((ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/WarmCult", UpgradeSystem.Type.Ritual_FirePit_2), true, true);
    UpgradeSystem.ClearCooldown(UpgradeSystem.Type.Ritual_FirePit);
    UpgradeSystem.ClearCooldown(UpgradeSystem.Type.Ritual_FirePit_2);
  }

  public void GiveHealingTouch() => this.StartCoroutine((IEnumerator) this.GiveHealingTouchIE());

  public IEnumerator GiveHealingTouchIE()
  {
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    DataManager.Instance.DoctrineUnlockedUpgrades.Add(DoctrineUpgradeSystem.DoctrineType.Special_HealingTouch);
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, 9, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public void GiveRotChoice() => this.StartCoroutine((IEnumerator) this.GiveRotChoiceIE());

  public IEnumerator GiveRotChoiceIE()
  {
    Interaction_DLCYngyaShrine interactionDlcYngyaShrine = this;
    yield return (object) new WaitForEndOfFrame();
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, 7, true, new System.Action(interactionDlcYngyaShrine.\u003CGiveRotChoiceIE\u003Eb__66_0)),
      new DoctrineResponse(SermonCategory.Special, 8, false, new System.Action(interactionDlcYngyaShrine.\u003CGiveRotChoiceIE\u003Eb__66_1))
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public void InteractedWithShrine()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.InteractYngyaShrine);
  }

  public void GiveDoctrineObjective()
  {
    if (DataManager.Instance.CompletedDoctrineStones <= 0)
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/YngyaDoctrine", Objectives.CustomQuestTypes.FindDoctrineStone), true, true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/YngyaDoctrine", Objectives.CustomQuestTypes.DeclareDoctrine_WINTER), true, true);
  }

  [CompilerGenerated]
  public void \u003CGiveGhostJuice\u003Eb__55_0() => this.ReceiveGhostJuice();

  [CompilerGenerated]
  public void \u003CGiveRotChoiceIE\u003Eb__66_0()
  {
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Special_EmbraceRot);
    this.embraceRotConvo.Play();
  }

  [CompilerGenerated]
  public void \u003CGiveRotChoiceIE\u003Eb__66_1()
  {
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Special_RejectRot);
    this.rejectRotConvo.Play();
  }

  public delegate void OnDepositEvent();
}
