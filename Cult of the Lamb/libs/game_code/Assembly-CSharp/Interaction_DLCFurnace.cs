// Decompiled with JetBrains decompiler
// Type: Interaction_DLCFurnace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DLCFurnace : Interaction_AddFuel
{
  public static Interaction_DLCFurnace Instance;
  public static List<Interaction_DLCFurnace> Furnaces = new List<Interaction_DLCFurnace>();
  [SerializeField]
  public Color FlashColor = new Color(0.0f, 159f, (float) byte.MaxValue);
  [SerializeField]
  public SpriteRenderer[] FlashingSprites;
  [SerializeField]
  public GameObject[] ShakeObjects;
  [SerializeField]
  public GameObject BuildingOFF;
  [SerializeField]
  public GameObject BuildingON;
  [SerializeField]
  public float FlashingSpeed = 3f;
  [SerializeField]
  public float ShakeTimer;
  [SerializeField]
  public float ShakeCooldown = 2f;
  [SerializeField]
  public GameObject ColdAuraVFX;
  [SerializeField]
  public GameObject ColdSnow;
  [SerializeField]
  public GameObject WarmAuraVFX;
  public float t;
  public Color currentColor;
  public bool IsFlashing = true;
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public GameObject resourceSpawnPos;
  [SerializeField]
  public GameObject resourceTargetPos;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public GameObject bulletPrefab;
  [SerializeField]
  public ItemGauge gauge;
  [SerializeField]
  public GameObject[] litStates;
  [SerializeField]
  public GameObject clearBlizzardBeam;
  public Interaction_DropOffItem dropOffAnimalFurnace;
  [SerializeField]
  public SpriteRenderer rangeSprite;
  [SerializeField]
  public ParticleSystem radiusParticleSystem;
  [SerializeField]
  public ParticleSystem radiusParticleSystemIntro;
  public float lerp;
  public int cachedAmount;
  public int previousCount;
  public float previousPrecentage;
  public const int CLEAR_BLIZZARD_COST = 60;
  public const float WASTE_CHANCE = 0.04f;
  public const int FURNACE_1_RANGE = 3;
  public const int FURNACE_2_RANGE = 6;
  public const int FURNACE_3_RANGE = 10;
  public float resourcesDelay = -1f;
  public bool busy;
  public bool activating;
  public float depositDelay = 0.5f;
  public Vector3 _updatePos;
  public float distanceRadius = 1f;
  public bool distanceChanged;
  public string insertFollowerSFX = "event:/dlc/building/furnace/follower_insert";
  public string insertFuelSFX = "event:/dlc/building/furnace/fuel_insert";
  public string turnOnLevel01SFX = "event:/dlc/building/furnace/turn_on_level_01";
  public string turnOnLevel02SFX = "event:/dlc/building/furnace/turn_on_level_02";
  public string turnOnLevel03SFX = "event:/dlc/building/furnace/turn_on_level_03";
  public string turnOffLevel01SFX = "event:/dlc/building/furnace/turn_off_level_01";
  public string turnOffLevel02SFX = "event:/dlc/building/furnace/turn_off_level_02";
  public string turnOffLevel03SFX = "event:/dlc/building/furnace/turn_off_level_03";
  public string turnOffWinterEnd01SFX = "event:/dlc/building/furnace/turn_off_level_01_winter_end";
  public string turnOffWinterEnd02SFX = "event:/dlc/building/furnace/turn_off_level_02_winter_end";
  public string turnOffWinterEnd03SFX = "event:/dlc/building/furnace/turn_off_level_03_winter_end";
  public string burningLoop01SFX = "event:/dlc/building/furnace/fire_burning_loop_level_01";
  public string burningLoop02SFX = "event:/dlc/building/furnace/fire_burning_loop_level_02";
  public string burningLoop03SFX = "event:/dlc/building/furnace/fire_burning_loop_level_03";
  public string clearBlizzardSFX = "event:/dlc/building/furnace/blizzard_clear";
  public string rattleOneShotSFX = "event:/dlc/building/furnace/rattle_os";
  public EventInstance burningLoopInstance;
  public EventInstance rattlingLoopInstance;
  public bool playingLowFuelSfx;
  [SerializeField]
  public Vector3 RandomDir;
  [SerializeField]
  public float ShakeStrength = 0.05f;
  [SerializeField]
  public float ShakeDuration = 0.5f;
  [SerializeField]
  public int ShakeVibrato = 15;
  [SerializeField]
  public int ShakeElasticity = 10;
  public Coroutine animateRoutine;
  public Coroutine spawnPollutionRoutine;

  public static event Interaction_DLCFurnace.FurnaceEvent OnFurnaceLit;

  public static event Interaction_DLCFurnace.FurnaceEvent OnFurnaceTurnOff;

  public bool Lit
  {
    get
    {
      return (UnityEngine.Object) this.structure != (UnityEngine.Object) null && this.structure.Structure_Info != null && this.structure.Structure_Info.Fuel > 0;
    }
  }

  public bool IsWithinScreenView()
  {
    Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(this.transform.position);
    return (double) screenPoint.x > 0.0 & (double) screenPoint.x < (double) Screen.width && (double) screenPoint.y > 0.0 && (double) screenPoint.y < (double) (Screen.height - 100);
  }

  public Vector3 GetScreenPosition()
  {
    return CameraManager.instance.CameraRef.WorldToScreenPoint(this.transform.position + new Vector3(0.0f, 1.7f, -1.5f));
  }

  public static int GetRangeForType(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.FURNACE_1:
        return 3;
      case StructureBrain.TYPES.FURNACE_2:
        return 6;
      case StructureBrain.TYPES.FURNACE_3:
        return 10;
      default:
        return 0;
    }
  }

  public int GetCurrentRange()
  {
    switch (this.structure.Brain.Data.Type)
    {
      case StructureBrain.TYPES.FURNACE_1:
        return 3;
      case StructureBrain.TYPES.FURNACE_2:
        return 6;
      case StructureBrain.TYPES.FURNACE_3:
        return 10;
      default:
        return 3;
    }
  }

  public void Start()
  {
    if (DataManager.Instance.OnboardedRotstone)
      return;
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) > 0)
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/WarmCult", Objectives.CustomQuestTypes.LightFurnace), true, true);
    }
    else
    {
      DataManager.Instance.OnboardedAddFuelToFurnace = true;
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/InteractYngyaShrine", Objectives.CustomQuestTypes.InteractYngyaShrine), true, true);
    }
    DataManager.Instance.OnboardedRotstone = true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_DLCFurnace.Furnaces.Add(this);
    Interaction_DLCFurnace.Instance = this;
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    this.UpdateLockedWarmthFuel();
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || this.structure.Brain == null || this.structure.Brain.Data.Fuel <= 0 || AudioManager.Instance.IsEventInstancePlaying(this.burningLoopInstance))
      return;
    this.RefreshBurningLoop();
  }

  public void SeasonsManager_OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    this.UpdateVisuals();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_DLCFurnace.Furnaces.Remove(this);
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    AudioManager.Instance.StopLoop(this.burningLoopInstance);
    AudioManager.Instance.StopLoop(this.rattlingLoopInstance);
    if ((UnityEngine.Object) this.radiusParticleSystem != (UnityEngine.Object) null)
      this.radiusParticleSystem.emission.enabled = false;
    if (!((UnityEngine.Object) this.radiusParticleSystemIntro != (UnityEngine.Object) null))
      return;
    this.radiusParticleSystemIntro.emission.enabled = false;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.burningLoopInstance);
    AudioManager.Instance.StopLoop(this.rattlingLoopInstance);
  }

  public override void OnBrainAssigned()
  {
    base.OnBrainAssigned();
    this.UpdateVisuals();
    this.InitializeParticleSystem();
    BaseGoopDoor.UnblockGoopDoor();
    DataManager.Instance.BuiltFurnace = true;
    DataManager.Instance.OnboardingFinished = true;
    DataManager.Instance.UnlockBaseTeleporter = true;
    if (!FollowerBrainStats.LockedWarmth)
      return;
    this.structure.Brain.Data.Fuel = this.structure.Brain.Data.MaxFuel;
    this.structure.Brain.Data.FullyFueled = true;
  }

  public override void OnEnableInteraction()
  {
    if ((UnityEngine.Object) this.fuelUI == (UnityEngine.Object) null)
      this.fuelUI = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Add Fuel Furnace"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<UIAddFuel>();
    this.fuelUI.offset = this.fuelBarOffset;
    this.structure.OnBrainAssigned += new System.Action(((Interaction_AddFuel) this).OnBrainAssigned);
  }

  public bool isFull() => this.structure.Brain.Data.Fuel >= this.structure.Brain.Data.MaxFuel;

  public override void GetLabel()
  {
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LightFurnace))
    {
      this.Interactable = false;
      this.Label = LocalizationManager.GetTranslation("Interactions/FurnaceOff");
    }
    else
    {
      if (FollowerBrainStats.LockedWarmth)
      {
        this.Label = ScriptLocalization.Interactions.Full;
        this.Interactable = false;
      }
      else if (this.busy)
      {
        this.Interactable = false;
        this.Label = "";
      }
      else if (this.structure.Brain.Data.Fuel >= this.structure.Brain.Data.MaxFuel - InventoryItem.FuelWeight(InventoryItem.ITEM_TYPE.MAGMA_STONE))
      {
        this.Label = ScriptLocalization.Interactions.Full;
        this.Interactable = false;
      }
      else
      {
        this.Interactable = true;
        this.Label = $"{LocalizationManager.GetTranslation("Interactions/Bank/Deposit")}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1)}";
      }
      this.HasSecondaryInteraction = false;
      if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
        return;
      this.SecondaryInteractable = true;
      this.HasSecondaryInteraction = true;
    }
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    if (this.busy)
      this.SecondaryInteractable = false;
    this.SecondaryLabel = this.busy || !this.HasSecondaryInteraction ? "" : $"{LocalizationManager.GetTranslation("Interactions/FurnaceOverdrive")}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.MAGMA_STONE, 60)}";
  }

  public override void OnInteract(StateMachine state)
  {
    bool flag = this.Structure.Brain.Data.Fuel <= 0;
    this._playerFarming = state.GetComponent<PlayerFarming>();
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceFollower) && this._playerFarming.CarryingDeadFollowerID != -1)
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillInFurnace, this._playerFarming.CarryingDeadFollowerID);
      this._playerFarming.CarryingDeadFollowerID = -1;
      for (int index = 0; index < 10; ++index)
        this.AddFuel(InventoryItem.ITEM_TYPE.MAGMA_STONE, false, (System.Action) null, true);
      this.HasChanged = true;
      this.AddSacrificeFuelThoughtUpdate(Thought.FurnaceFollower);
      this.Animate();
      AudioManager.Instance.PlayOneShot(this.insertFollowerSFX);
    }
    else if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceAnimal) && (state.GetComponent<PlayerFarming>().IsLeashingAnimal() || state.GetComponent<PlayerFarming>().IsRidingAnimal()))
    {
      Interaction_Ranchable interactionRanchable = state.GetComponent<PlayerFarming>().GetLeashingAnimal();
      if ((UnityEngine.Object) interactionRanchable != (UnityEngine.Object) null)
      {
        state.GetComponent<PlayerFarming>().StopLeashingAnimal();
      }
      else
      {
        interactionRanchable = state.GetComponent<PlayerFarming>().GetRidingAnimal();
        state.GetComponent<PlayerFarming>().StopRidingOnAnimal();
      }
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionRanchable.transform.position);
      int num = 50;
      if (interactionRanchable.Animal.Age < 2)
        num = 20;
      for (int index = 0; index < num; ++index)
        this.AddFuel(InventoryItem.ITEM_TYPE.MAGMA_STONE, false, (System.Action) null, true);
      this.HasChanged = true;
      this.AddSacrificeFuelThoughtUpdate(Thought.FurnaceAnimal);
      this.StartCoroutine((IEnumerator) interactionRanchable.RemoveAnimal(false));
      this.Animate();
      interactionRanchable.PlayDieVO();
      AudioManager.Instance.PlayOneShot(this.insertFollowerSFX);
    }
    else if (!this.activating)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) > 0 && this.structure.Brain.Data.Fuel < this.structure.Brain.Data.MaxFuel - InventoryItem.FuelWeight(InventoryItem.ITEM_TYPE.MAGMA_STONE))
        this.activating = true;
      else
        state.GetComponent<PlayerFarming>().indicator.PlayShake();
    }
    if (!flag || this.Structure.Brain.Data.Fuel <= 0)
      return;
    this.DoFurnaceRelightEffects();
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      Interaction_DLCFurnace.FurnaceEvent onFurnaceLit = Interaction_DLCFurnace.OnFurnaceLit;
      if (onFurnaceLit == null)
        return;
      onFurnaceLit();
    }));
  }

  public void DoFurnaceRelightEffects()
  {
    NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.Furnace_On);
    BiomeConstants.Instance.EmitDisplacementEffect(this.transform.position);
    this.ShakeAllSprites();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) >= 60)
      this.StartCoroutine((IEnumerator) this.ClearBlizzardIE());
    else
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
  }

  public IEnumerator ClearBlizzardIE()
  {
    Interaction_DLCFurnace interactionDlcFurnace = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/building/furnace/blizzard_clear_interact", interactionDlcFurnace.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcFurnace.cameraTarget);
    yield return (object) new WaitForSeconds(1f);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE, -60);
    interactionDlcFurnace.fuelUI.Hide();
    interactionDlcFurnace.cachedAmount += 20;
    float num = (float) ((double) interactionDlcFurnace.cachedAmount / 2.0 * 0.10000000149011612);
    double targetDistance = (double) GameManager.GetInstance().CamFollowTarget.targetDistance;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), GameManager.GetInstance().CamFollowTarget.targetDistance - 3f, num + 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetDelay<TweenerCore<float, float, FloatOptions>>(0.5f);
    for (int i = 0; i < interactionDlcFurnace.cachedAmount / 2; ++i)
    {
      AudioManager.Instance.PlayOneShot(interactionDlcFurnace.insertFuelSFX);
      interactionDlcFurnace.SpawnItem(Mathf.Lerp(0.1f, 0.4f, (float) i / (float) (interactionDlcFurnace.cachedAmount / 2)), false);
      yield return (object) new WaitForSeconds(0.1f);
    }
    GameManager.GetInstance().OnConversationNext(interactionDlcFurnace.cameraTarget, 12f);
    BiomeConstants.Instance.EmitDisplacementEffect(interactionDlcFurnace.transform.position);
    interactionDlcFurnace.clearBlizzardBeam?.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot(interactionDlcFurnace.clearBlizzardSFX);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 1f);
    SeasonsManager.DisableCurrentActiveBlizzards(true);
    SeasonsManager.StopWeatherEvent();
    yield return (object) new WaitForSeconds(3f);
    NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.ClearedBlizzard);
    interactionDlcFurnace.clearBlizzardBeam?.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationEnd();
    interactionDlcFurnace.HasChanged = true;
  }

  public override void Update()
  {
    base.Update();
    if (this.structure.Brain != null && this.structure.Brain.Data.Fuel <= 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      Color color = Color.Lerp(this.FlashColor, Color.white, Mathf.PingPong(Time.time * this.FlashingSpeed, 1f));
      foreach (SpriteRenderer flashingSprite in this.FlashingSprites)
      {
        if ((UnityEngine.Object) flashingSprite != (UnityEngine.Object) null)
          flashingSprite.color = color;
      }
      this.IsFlashing = true;
      this.ShakeTimer -= Time.deltaTime;
      if ((double) this.ShakeTimer <= 0.0)
      {
        AudioManager.Instance.PlayOneShot(this.rattleOneShotSFX, this.transform.position);
        this.ShakeAllSprites();
        this.ShakeTimer = this.ShakeCooldown;
      }
    }
    else if (this.IsFlashing)
    {
      this.IsFlashing = false;
      this.t = 0.0f;
      this.ShakeTimer = 0.0f;
      foreach (SpriteRenderer flashingSprite in this.FlashingSprites)
      {
        if ((UnityEngine.Object) flashingSprite != (UnityEngine.Object) null)
          flashingSprite.color = Color.white;
      }
    }
    if (this.structure.Brain != null && this.structure.Brain.Data != null)
    {
      if (this.previousCount != this.structure.Brain.Data.Fuel)
      {
        this.UpdateGauage();
        this.UpdateVisuals();
        this.previousCount = this.structure.Brain.Data.Fuel;
      }
      this.noFuelIcon.gameObject.SetActive(this.structure.Brain.Data.Fuel <= 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter);
      if ((double) this.structure.Brain.Data.Fuel / (double) this.structure.Brain.Data.MaxFuel < 0.20000000298023224 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      {
        if (!this.playingLowFuelSfx)
        {
          this.rattlingLoopInstance = AudioManager.Instance.CreateLoop("event:/dlc/building/furnace/rattling_loop_level_01", this.gameObject, true);
          this.playingLowFuelSfx = true;
        }
      }
      else if (this.playingLowFuelSfx)
      {
        this.playingLowFuelSfx = false;
        AudioManager.Instance.StopLoop(this.rattlingLoopInstance);
      }
    }
    if (this.activating)
    {
      if ((double) Time.time > (double) this.depositDelay)
      {
        this.resourcesDelay = Time.time + 2f;
        Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE, -1);
        ++this.cachedAmount;
        bool wasEmpty = this.Structure.Brain.Data.Fuel <= 0;
        for (int index = 0; index < 1; ++index)
          this.AddFuel(InventoryItem.ITEM_TYPE.MAGMA_STONE, false, (System.Action) null, true);
        if (wasEmpty && this.Structure.Brain.Data.Fuel > 0)
        {
          this.DoFurnaceRelightEffects();
          GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
          {
            Interaction_DLCFurnace.FurnaceEvent onFurnaceLit = Interaction_DLCFurnace.OnFurnaceLit;
            if (onFurnaceLit == null)
              return;
            onFurnaceLit();
          }));
        }
        this.HasChanged = true;
        this.depositDelay = Time.time + 0.1f;
        this.SpawnItem(0.1f, wasEmpty);
      }
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) <= 0 || InputManager.Gameplay.GetInteractButtonUp(this.playerFarming) || (double) Vector3.Distance(this.transform.position, this.playerFarming.transform.position) > (double) this.ActivateDistance || this.structure.Brain.Data.Fuel > this.structure.Brain.Data.MaxFuel - InventoryItem.FuelWeight(InventoryItem.ITEM_TYPE.MAGMA_STONE))
      {
        this.activating = false;
        if (this.animateRoutine == null)
          this.animateRoutine = this.StartCoroutine((IEnumerator) this.AnimateResourcesRoutine((float) this.cachedAmount));
      }
    }
    this.UpdateRangeVisuals();
  }

  public void ShakeAllSprites()
  {
    this.RandomDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0.0f, 0.0f).normalized * this.ShakeStrength;
    foreach (SpriteRenderer flashingSprite in this.FlashingSprites)
    {
      if ((UnityEngine.Object) flashingSprite != (UnityEngine.Object) null)
      {
        flashingSprite.transform.DOComplete();
        flashingSprite.transform.DOPunchPosition(this.RandomDir, this.ShakeDuration, this.ShakeVibrato, (float) this.ShakeElasticity);
      }
    }
    foreach (GameObject shakeObject in this.ShakeObjects)
    {
      if ((UnityEngine.Object) shakeObject != (UnityEngine.Object) null)
      {
        shakeObject.transform.DOComplete();
        shakeObject.transform.DOPunchPosition(this.RandomDir, this.ShakeDuration, this.ShakeVibrato, (float) this.ShakeElasticity);
      }
    }
  }

  public override void AddFuel(
    InventoryItem.ITEM_TYPE itemType,
    bool changeItemQuantity = true,
    System.Action onFull = null,
    bool playSfx = true)
  {
    base.AddFuel(itemType, changeItemQuantity, onFull, playSfx);
    AudioManager.Instance.PlayOneShot(this.insertFuelSFX);
    DataManager.Instance.OnboardedAddFuelToFurnace = true;
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LightFurnace))
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LightFurnace);
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/InteractYngyaShrine", Objectives.CustomQuestTypes.InteractYngyaShrine), true, true);
    }
    this.UpdateVisuals();
    WarmthBar.ForceCountUpdate();
    this.UpdateParticleEmissions();
  }

  public override void SpawnFuelItem(InventoryItem.ITEM_TYPE itemType) => ++this.cachedAmount;

  public override void OnItemSelectorHidden(StateMachine state)
  {
    base.OnItemSelectorHidden(state);
    this.AnimateRoutine((float) this.cachedAmount);
  }

  public void AnimateRoutine(float spawnAmount)
  {
    this.StartCoroutine((IEnumerator) this.AnimateResourcesRoutine(spawnAmount));
  }

  public IEnumerator AnimateResourcesRoutine(float spawnAmount)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_DLCFurnace interactionDlcFurnace = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionDlcFurnace.Animate();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionDlcFurnace.Interactable = false;
    interactionDlcFurnace.HasChanged = true;
    interactionDlcFurnace.busy = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Animate()
  {
    if (this.structure.Brain.Data.Type == StructureBrain.TYPES.FURNACE_3)
    {
      for (int index = 0; index < UnityEngine.Random.Range(6, 10); ++index)
        this.structure.Brain.DepositItemUnstacked(InventoryItem.ITEM_TYPE.SOOT);
      this.busy = false;
    }
    this.busy = false;
    this.Interactable = true;
    this.HasChanged = true;
    this.cachedAmount = 0;
  }

  public void SpawnItem(float punch, bool wasEmpty)
  {
    PickUp pickup = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1, this.resourceSpawnPos.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle / 2f, 1f);
    pickup.SetInitialSpeedAndDiraction(2f, 0.0f);
    pickup.Bounce = false;
    pickup.MagnetToPlayer = false;
    pickup.AddToInventory = false;
    pickup.AddToChestIfNotCollected = false;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one / 2f;
    pickup.transform.DOMove(this.resourceTargetPos.transform.position, 0.35f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (!wasEmpty)
      {
        foreach (SpriteRenderer flashingSprite in this.FlashingSprites)
        {
          if ((UnityEngine.Object) flashingSprite != (UnityEngine.Object) null)
          {
            flashingSprite.transform.DOComplete();
            flashingSprite.transform.localScale = new Vector3(1.2f, 0.8f, 1f);
            flashingSprite.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
          }
        }
        foreach (GameObject shakeObject in this.ShakeObjects)
        {
          if ((UnityEngine.Object) shakeObject != (UnityEngine.Object) null)
          {
            shakeObject.transform.DOComplete();
            shakeObject.transform.localScale = new Vector3(1.2f, 0.8f, 1f);
            shakeObject.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
          }
        }
      }
      UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
    }));
  }

  public void DoBounce(Transform transform)
  {
    foreach (GameObject shakeObject in this.ShakeObjects)
    {
      shakeObject.transform.DOComplete();
      shakeObject.transform.localScale = new Vector3(1.2f, 0.8f, 1f);
      shakeObject.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
  }

  public void UpdateVisuals()
  {
    bool activeSelf = this.BuildingON.activeSelf;
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !activeSelf)
    {
      if (this.structure.Brain != null && this.structure.Brain.Data.Fuel > 0)
        this.StartCoroutine((IEnumerator) this.PlayTurnOnSFX());
      this.DoBounce(this.BuildingON.transform);
    }
    this.BuildingON.SetActive(SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter);
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter & this.BuildingOFF.activeSelf)
    {
      this.DoBounce(this.BuildingOFF.transform);
      AudioManager.Instance.StopLoop(this.burningLoopInstance);
      this.PlayWinterEndsSFX();
    }
    this.BuildingOFF.SetActive(SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter);
    if (this.structure.Brain != null)
    {
      this.ColdAuraVFX.SetActive(this.structure.Brain.Data.Fuel <= 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter);
      this.ColdSnow.SetActive(this.ColdAuraVFX.activeSelf);
      this.WarmAuraVFX.SetActive(!this.ColdAuraVFX.activeSelf);
      this.noFuelIcon?.gameObject.SetActive(this.structure.Brain.Data.Fuel <= 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter);
      int num = Mathf.CeilToInt((float) this.structure.Brain.Data.Fuel / (float) this.structure.Brain.Data.MaxFuel * (float) this.litStates.Length);
      if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      {
        num = 0;
        if (this.previousCount > 0)
        {
          Interaction_DLCFurnace.FurnaceEvent onFurnaceTurnOff = Interaction_DLCFurnace.OnFurnaceTurnOff;
          if (onFurnaceTurnOff != null)
            onFurnaceTurnOff();
          this.PlayWinterEndsSFX();
        }
      }
      else if (this.previousCount > 0 && this.structure.Brain.Data.Fuel <= 0)
      {
        this.StartCoroutine((IEnumerator) this.DoOutOfFuelEffectsRoutine());
        num = 0;
        Interaction_DLCFurnace.FurnaceEvent onFurnaceTurnOff = Interaction_DLCFurnace.OnFurnaceTurnOff;
        if (onFurnaceTurnOff != null)
          onFurnaceTurnOff();
        this.PlayOutOfFuelSFX();
      }
      for (int index = 0; index < this.litStates.Length; ++index)
      {
        bool flag = num - 1 == index;
        this.litStates[index].gameObject.SetActive(flag);
        if (flag && this.previousCount == 0)
          this.StartCoroutine((IEnumerator) this.PlayTurnOnSFX());
      }
      this.UpdateGauage();
      this.previousCount = this.structure.Brain.Data.Fuel;
    }
    this.UpdateParticleEmissions();
    if (this.structure.Brain.Data.Fuel > 0)
      return;
    AudioManager.Instance.StopLoop(this.burningLoopInstance);
  }

  public IEnumerator DoOutOfFuelEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_DLCFurnace interactionDlcFurnace = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      BiomeConstants.Instance.ChromaticAbberationTween(0.5f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.EmitDisplacementEffect(interactionDlcFurnace.transform.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    this.HasChanged = false;
    if (this.AutomaticallyInteract)
      return;
    this.IndicateHighlighted(playerFarming);
    this.UpdateLockedWarmthFuel();
    if (this.fuelUI.IsShowing || !this.Interactable || this.structure.Brain.Data.Fuel >= this.structure.Brain.Data.MaxFuel)
      return;
    this.fuelUI.Show((Interaction_AddFuel) this);
  }

  public IEnumerator OnboardRotstoneIE()
  {
    Interaction_DLCFurnace interactionDlcFurnace = this;
    yield return (object) new WaitForSeconds(1f);
    while (PlayerFarming.Location != FollowerLocation.Base || MMConversation.isPlaying || LetterBox.IsPlaying)
      yield return (object) null;
    DataManager.Instance.OnboardedRotstone = true;
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
    foreach (PlacementRegion.TileGridTile tileGridTile in PlacementRegion.Instance.structureBrain.Data.Grid)
    {
      if (tileGridTile.CanPlaceObstruction)
        tileGridTileList.Add(tileGridTile);
    }
    PlacementRegion.TileGridTile tile = tileGridTileList[UnityEngine.Random.Range(0, tileGridTileList.Count)];
    GameObject empty = new GameObject();
    empty.transform.position = tile.WorldPosition;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(Interaction_DLCShrine.Instance.gameObject);
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.YngyaOffering = -2;
    Interaction_DLCShrine.Instance.gameObject.SetActive(true);
    Interaction_DLCShrine.Instance.Spine.transform.localPosition = new Vector3(0.0f, -7f, 0.0f);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 5f);
    Interaction_DLCShrine.Instance.Spine.transform.DOLocalMove(Vector3.zero, 3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc);
    interactionDlcFurnace.transform.DOShakePosition(5f, new Vector3(0.25f, 0.0f, 0.0f)).SetUpdate<Tweener>(true);
    MMVibrate.Rumble(0.0f, 2f, 5f, (MonoBehaviour) GameManager.GetInstance());
    yield return (object) new WaitForSeconds(3f);
    Interaction_DLCShrine.Instance.SpawnSplatter(20, 1f);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(empty);
    yield return (object) new WaitForSeconds(1f);
    interactionDlcFurnace.PlaceRotRubble(tile, 0.75f);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/WarmCult", InventoryItem.ITEM_TYPE.MAGMA_STONE, 5), true, true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/WarmCult", Objectives.CustomQuestTypes.LightFurnace), true, true);
  }

  public void PlaceRotRubble(PlacementRegion.TileGridTile tile, float chanceToSpawnChildren)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.RUBBLE, 0);
    Vector3 worldPosition = tile.WorldPosition;
    infoByType.DontLoadMe = false;
    infoByType.GrowthStage = 1f;
    infoByType.PlacementRegionPosition = new Vector3Int((int) PlacementRegion.Instance.structureBrain.Data.Position.x, (int) PlacementRegion.Instance.structureBrain.Data.Position.y, 0);
    infoByType.GridTilePosition = tile.Position;
    infoByType.VariantIndex = 1;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, worldPosition, new Vector2Int(1, 1));
    PlacementRegion.Instance.structureBrain.AddStructureToGrid(infoByType);
    GameManager.GetInstance().WaitForSeconds(0.25f, (System.Action) (() =>
    {
      for (int x = -1; x < 1; ++x)
      {
        for (int y = -1; y < 1; ++y)
        {
          if ((x != 0 || y != 0) && (double) UnityEngine.Random.value < (double) chanceToSpawnChildren)
          {
            PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.structureBrain.GetTileGridTile(tile.Position + new Vector2Int(x, y));
            if (tileGridTile != null && tileGridTile.CanPlaceObstruction)
              this.PlaceRotRubble(tileGridTile, chanceToSpawnChildren / 2f);
          }
        }
      }
    }));
  }

  public void AddSacrificeFuelThoughtUpdate(Thought sacrificeThought)
  {
    foreach (Follower follower in Follower.Followers)
    {
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null && (double) Vector3.Distance(follower.transform.position, this.transform.position) < 8.0)
        follower.Brain.AddThought(sacrificeThought);
    }
  }

  public void PlayOutOfFuelSFX()
  {
    string soundPath = "";
    switch (this.structure.Brain.Data.Type)
    {
      case StructureBrain.TYPES.FURNACE_1:
        soundPath = this.turnOffLevel01SFX;
        break;
      case StructureBrain.TYPES.FURNACE_2:
        soundPath = this.turnOffLevel02SFX;
        break;
      case StructureBrain.TYPES.FURNACE_3:
        soundPath = this.turnOffLevel03SFX;
        break;
    }
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, this.gameObject);
    AudioManager.Instance.StopLoop(this.burningLoopInstance);
    AudioManager.Instance.StopLoop(this.rattlingLoopInstance);
  }

  public void PlayWinterEndsSFX()
  {
    string soundPath = "";
    switch (this.structure.Brain.Data.Type)
    {
      case StructureBrain.TYPES.FURNACE_1:
        soundPath = this.turnOffWinterEnd01SFX;
        break;
      case StructureBrain.TYPES.FURNACE_2:
        soundPath = this.turnOffWinterEnd02SFX;
        break;
      case StructureBrain.TYPES.FURNACE_3:
        soundPath = this.turnOffWinterEnd03SFX;
        break;
    }
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, this.gameObject);
    AudioManager.Instance.StopLoop(this.burningLoopInstance);
    AudioManager.Instance.StopLoop(this.rattlingLoopInstance);
  }

  public IEnumerator PlayTurnOnSFX()
  {
    Interaction_DLCFurnace interactionDlcFurnace = this;
    while (MMTransition.IsPlaying || MMConversation.isPlaying || LetterBox.IsPlaying || (UnityEngine.Object) BaseLocationManager.Instance == (UnityEngine.Object) null || !BaseLocationManager.Instance.StructuresPlaced)
      yield return (object) null;
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
    {
      AudioManager.Instance.StopLoop(interactionDlcFurnace.burningLoopInstance);
    }
    else
    {
      string soundPath1 = "";
      string soundPath2 = "";
      switch (interactionDlcFurnace.structure.Brain.Data.Type)
      {
        case StructureBrain.TYPES.FURNACE_1:
          soundPath1 = interactionDlcFurnace.turnOnLevel01SFX;
          soundPath2 = interactionDlcFurnace.burningLoop01SFX;
          break;
        case StructureBrain.TYPES.FURNACE_2:
          soundPath1 = interactionDlcFurnace.turnOnLevel02SFX;
          soundPath2 = interactionDlcFurnace.burningLoop02SFX;
          break;
        case StructureBrain.TYPES.FURNACE_3:
          soundPath1 = interactionDlcFurnace.turnOnLevel03SFX;
          soundPath2 = interactionDlcFurnace.burningLoop03SFX;
          break;
      }
      if (!string.IsNullOrEmpty(soundPath1))
        AudioManager.Instance.PlayOneShot(soundPath1, interactionDlcFurnace.gameObject);
      if (!string.IsNullOrEmpty(soundPath2))
      {
        AudioManager.Instance.StopLoop(interactionDlcFurnace.burningLoopInstance);
        interactionDlcFurnace.burningLoopInstance = AudioManager.Instance.CreateLoop(soundPath2, interactionDlcFurnace.gameObject, true);
      }
    }
  }

  public void RefreshBurningLoop()
  {
    string soundPath = "";
    switch (this.structure.Brain.Data.Type)
    {
      case StructureBrain.TYPES.FURNACE_1:
        soundPath = this.burningLoop01SFX;
        break;
      case StructureBrain.TYPES.FURNACE_2:
        soundPath = this.burningLoop02SFX;
        break;
      case StructureBrain.TYPES.FURNACE_3:
        soundPath = this.burningLoop03SFX;
        break;
    }
    if (string.IsNullOrEmpty(soundPath) || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || this.structure.Brain.Data.Fuel <= 0)
      return;
    AudioManager.Instance.StopLoop(this.burningLoopInstance);
    this.burningLoopInstance = AudioManager.Instance.CreateLoop(soundPath, this.gameObject, true);
  }

  public void InitializeParticleSystem()
  {
    if (!DataManager.Instance.OnboardedSnowedUnder)
    {
      this.HideVisuals();
    }
    else
    {
      int currentRange = this.GetCurrentRange();
      if ((UnityEngine.Object) this.rangeSprite != (UnityEngine.Object) null)
      {
        this.rangeSprite.color = new Color(1f, 1f, 1f, 0.0f);
        this.rangeSprite.size = new Vector2((float) currentRange, (float) currentRange);
      }
      if ((UnityEngine.Object) this.radiusParticleSystem != (UnityEngine.Object) null)
      {
        this.radiusParticleSystem.main.startSize = (ParticleSystem.MinMaxCurve) (float) currentRange;
        this.radiusParticleSystem.emission.enabled = this.Lit;
      }
      if (!((UnityEngine.Object) this.radiusParticleSystemIntro != (UnityEngine.Object) null))
        return;
      ParticleSystem.MainModule main = this.radiusParticleSystemIntro.main;
      this.radiusParticleSystemIntro.emission.enabled = this.Lit;
    }
  }

  public void UpdateParticleEmissions()
  {
    if (!DataManager.Instance.OnboardedSnowedUnder)
      return;
    int currentRange = this.GetCurrentRange();
    if ((UnityEngine.Object) this.rangeSprite != (UnityEngine.Object) null)
      this.rangeSprite.size = new Vector2((float) currentRange, (float) currentRange);
    if ((UnityEngine.Object) this.radiusParticleSystem != (UnityEngine.Object) null)
    {
      this.radiusParticleSystem.main.startSize = (ParticleSystem.MinMaxCurve) (float) currentRange;
      this.radiusParticleSystem.emission.enabled = this.Lit;
    }
    if (!((UnityEngine.Object) this.radiusParticleSystemIntro != (UnityEngine.Object) null))
      return;
    ParticleSystem.MainModule main = this.radiusParticleSystemIntro.main;
    this.radiusParticleSystemIntro.emission.enabled = this.Lit;
  }

  public void UpdateRangeVisuals()
  {
    if (!DataManager.Instance.OnboardedSnowedUnder)
      return;
    if (!GameManager.overridePlayerPosition && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      this._updatePos = PlayerFarming.Instance.transform.position;
      this.distanceRadius = 2f;
    }
    else if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.distanceRadius)
    {
      this.rangeSprite?.gameObject.SetActive(true);
      SpriteRenderer rangeSprite1 = this.rangeSprite;
      if (rangeSprite1 != null)
        rangeSprite1.DOKill();
      SpriteRenderer rangeSprite2 = this.rangeSprite;
      if (rangeSprite2 != null)
        rangeSprite2.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else
    {
      if (!this.distanceChanged || !((UnityEngine.Object) this.rangeSprite != (UnityEngine.Object) null))
        return;
      this.rangeSprite.DOKill();
      this.rangeSprite.DOColor(new Color(1f, 1f, 1f, 0.0f), 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
  }

  public void UpdateGauage()
  {
    float norm = (float) this.structure.Brain.Data.Fuel / (float) this.structure.Brain.Data.MaxFuel;
    if ((double) Mathf.Abs(norm - this.previousPrecentage) < 0.10000000149011612)
      return;
    this.gauge.SetPosition(norm);
    this.previousPrecentage = norm;
  }

  public void HideVisuals()
  {
    this.rangeSprite?.gameObject.SetActive(false);
    this.radiusParticleSystem?.gameObject.SetActive(false);
    this.radiusParticleSystemIntro?.gameObject.SetActive(false);
  }

  public bool CanAddFuel()
  {
    return this.structure.Structure_Info.Fuel < this.structure.Structure_Info.MaxFuel && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter;
  }

  public void UpdateLockedWarmthFuel()
  {
    if (!FollowerBrainStats.LockedWarmth || this.structure.Brain == null)
      return;
    this.structure.Brain.Data.Fuel = this.structure.Brain.Data.MaxFuel;
    this.structure.Brain.Data.FullyFueled = true;
  }

  public delegate void FurnaceEvent();
}
