// Decompiled with JetBrains decompiler
// Type: FarmPlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class FarmPlot : Interaction
{
  public const float WATERING_DURATION_SECONDS = 0.95f;
  public Vector2 WITHER_SHAKE_DURATION_RANGE = new Vector2(2f, 6f);
  public GameObject WateredGameObject;
  public GameObject UnWateredGameObject;
  public GameObject WateredRotGameObject;
  public GameObject UnWateredRotGameObject;
  public GameObject FertilizedObject;
  public GameObject FertilizedGoldObject;
  public GameObject FertilizedGlowObject;
  public GameObject FertilizedRainbowObject;
  public GameObject FertilizedDevotionObject;
  public GameObject FrozenGroundObject;
  [Header("Farm plots (Only for reference) not used in code")]
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
  public AssetReferenceGameObject _frozenGroundObjectTemplate;
  public GameObject ScareCrowSymbol;
  public GameObject CropParent;
  public SpriteXPBar DevotionBar;
  public Transform DirtTransform;
  public bool _scareCrowSymbol;
  public static List<FarmPlot> FarmPlots = new List<FarmPlot>();
  public Structure Structure;
  public Structures_FarmerPlot _StructureInfo;
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypesToPlant = new List<InventoryItem.ITEM_TYPE>();
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypesToFertilize = new List<InventoryItem.ITEM_TYPE>();
  public GameObject SeedIndicatorPrefab;
  public List<CropController> CropPrefabs;
  public Dictionary<InventoryItem.ITEM_TYPE, CropController> _cropPrefabsBySeedType;
  [CompilerGenerated]
  public CropController \u003C_activeCropController\u003Ek__BackingField;
  [SerializeField]
  public GameObject wateringIndicator;
  public GameObject Player;
  public List<InventoryItem> ToDeposit = new List<InventoryItem>();
  public float WateringTime = 0.95f;
  public float Delay;
  public bool _watered;
  public bool _isWateringActivated;
  public bool _isCurrentInteraction;
  public bool beingMoved;
  public bool _isBerryShaking;
  public string sPlant;
  public string sWater;
  public string sFertilize;
  public string sFrozenGround;
  public EventInstance loopedSound;
  public GameObject BirdPrefab;
  public CritterBaseBird Bird;
  public Coroutine cBirdRoutine;
  public GameObject g;
  public bool BirdIsPlaying;
  public bool BirdHasLanded;
  public Vector3 RandomPosition;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_FarmerPlot StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_FarmerPlot;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public static FarmPlot GetFarmPlot(int ID)
  {
    foreach (FarmPlot farmPlot in FarmPlot.FarmPlots)
    {
      if (farmPlot.StructureInfo.ID == ID)
        return farmPlot;
    }
    return (FarmPlot) null;
  }

  public CropController _activeCropController
  {
    get => this.\u003C_activeCropController\u003Ek__BackingField;
    set => this.\u003C_activeCropController\u003Ek__BackingField = value;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_FarmCropGrower.OnRotate += new System.Action<Interaction_FarmCropGrower>(this.OnFarmCropGrowerRotate);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_FarmCropGrower.OnRotate -= new System.Action<Interaction_FarmCropGrower>(this.OnFarmCropGrowerRotate);
  }

  public void Awake()
  {
    this._cropPrefabsBySeedType = new Dictionary<InventoryItem.ITEM_TYPE, CropController>();
    foreach (CropController cropPrefab in this.CropPrefabs)
      this._cropPrefabsBySeedType.Add(cropPrefab.SeedType, cropPrefab);
    FarmPlot.FarmPlots.Add(this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    FarmPlot.FarmPlots.Remove(this);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnGrowthStageChanged -= new System.Action(this.UpdateCropImage);
    this.StructureBrain.OnBirdAttack -= new System.Action(this.OnBirdAttack);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.HasSecondaryInteraction = false;
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.OnStructuresMoved);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructuresMoved);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    else
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    PlacementRegion.OnBuildingBeganMoving += new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced += new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
  }

  public void OnStructuresPlaced()
  {
    this.UpdateCropImage();
    this.UpdateScareCrowSymbol();
  }

  public void OnStructuresMoved(StructuresData structure)
  {
    this.UpdateCropImage();
    this.UpdateScareCrowSymbol();
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    DataManager.Instance.HasBuiltFarmPlot = true;
    this.StructureBrain.OnGrowthStageChanged += new System.Action(this.UpdateCropImage);
    this.StructureBrain.OnBirdAttack += new System.Action(this.OnBirdAttack);
    if (this.StructureInfo.HasBird)
      this.OnBirdAttack();
    if (this.BirdIsPlaying)
    {
      if (!this.BirdHasLanded)
        this.OnBirdAttack();
      else if ((bool) (UnityEngine.Object) this.Bird)
      {
        this.Bird.gameObject.SetActive(true);
        if ((UnityEngine.Object) this.Bird.skeletonAnimationLODManager != (UnityEngine.Object) null)
          this.Bird.skeletonAnimationLODManager.DoUpdate = true;
        this.cBirdRoutine = this.StartCoroutine((IEnumerator) this.LandedBird());
      }
    }
    this.UpdateLocalisation();
    this.UpdateWatered();
    this.UpdateCropImage();
  }

  public void OnStructureAdded(StructuresData structure)
  {
    switch (structure.Type)
    {
      case global::StructureBrain.TYPES.SCARECROW:
      case global::StructureBrain.TYPES.SCARECROW_2:
        this.UpdateScareCrowSymbol();
        break;
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    PlacementRegion.OnBuildingBeganMoving -= new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced -= new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.OnStructuresMoved);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructuresMoved);
    if (this.StructureBrain != null)
    {
      this.StructureBrain.OnGrowthStageChanged -= new System.Action(this.UpdateCropImage);
      this.StructureBrain.OnBirdAttack -= new System.Action(this.OnBirdAttack);
    }
    if (!(bool) (UnityEngine.Object) this.Bird)
      return;
    this.Bird.gameObject.SetActive(false);
  }

  public void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = true;
  }

  public void OnBuildingPlaced(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = false;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this._isCurrentInteraction = true;
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this._isCurrentInteraction = false;
  }

  public override void GetLabel()
  {
    this.Interactable = true;
    if (this.StructureBrain.CanPickCrop())
    {
      this.ContinuouslyHold = false;
      this.Label = "";
    }
    else if (this.StructureBrain.IsGroundFrozen)
    {
      if (Inventory.GetItemQuantity(187) > 0)
      {
        this.ContinuouslyHold = true;
        this.Label = $"{this.sFertilize} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.POOP_ROTSTONE, 1)}";
      }
      else
      {
        this.ContinuouslyHold = false;
        this.Interactable = false;
        this.Label = this.sFrozenGround;
      }
    }
    else if (this.StructureBrain.CanPlantSeed())
    {
      this.ContinuouslyHold = false;
      this.Label = this.ToDeposit.Count == 0 ? this.sPlant : "";
    }
    else if (this._isWateringActivated)
    {
      this.ContinuouslyHold = false;
      this.Label = "";
    }
    else if (this.StructureBrain.CanWater())
    {
      this.ContinuouslyHold = false;
      this.Label = this.sWater;
    }
    else if (this.StructureBrain.CanFertilize())
    {
      this.ContinuouslyHold = true;
      this.Label = this.sFertilize;
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP) <= 0 || Inventory.GetItemQuantity(144 /*0x90*/) > 0 || Inventory.GetItemQuantity(142) > 0 || Inventory.GetItemQuantity(143) > 0 || Inventory.GetItemQuantity(162) > 0 || Inventory.GetItemQuantity(187) > 0)
        return;
      this.Label = $"{this.sFertilize} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.POOP, 1)}";
    }
    else
    {
      this.ContinuouslyHold = false;
      this.Label = "";
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sPlant = ScriptLocalization.Interactions.FarmPlant;
    this.sWater = ScriptLocalization.Interactions.FarmWater;
    this.sFertilize = ScriptLocalization.Interactions.FarmFertilize;
    this.sFrozenGround = ScriptLocalization.Interactions.FrozenGround;
  }

  public override void OnInteract(StateMachine state)
  {
    PlayerFarming playerFarming = state.GetComponent<PlayerFarming>();
    if (this.StructureBrain.IsGroundFrozen)
    {
      base.OnInteract(state);
      this.IndicateHighlighted(playerFarming);
      if (Inventory.GetItemQuantity(187) > 0)
      {
        this.AddFertiliser(InventoryItem.ITEM_TYPE.POOP_ROTSTONE);
        this.UpdateCropImage();
        this.UpdateWatered();
      }
      else
        playerFarming.indicator.PlayShake();
    }
    else if (this.StructureBrain.CanPlantSeed() && this.ToDeposit.Count <= 0)
    {
      base.OnInteract(state);
      this.IndicateHighlighted(playerFarming);
      this.Interactable = false;
      state.CURRENT_STATE = StateMachine.State.InActive;
      state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
      UIItemSelectorOverlayController itemSelector;
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SEED_SOZO) > 0)
        itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(playerFarming, new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) InventoryItem.AllSeeds)
        {
          InventoryItem.ITEM_TYPE.SEED_SOZO
        }, new ItemSelector.Params()
        {
          Key = "plant_seeds_sozo",
          Context = ItemSelector.Context.Plant,
          Offset = new Vector2(0.0f, 100f),
          HideOnSelection = true,
          RequiresDiscovery = true,
          ShowEmpty = true
        });
      else
        itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(playerFarming, InventoryItem.AllSeeds, new ItemSelector.Params()
        {
          Key = "plant_seeds",
          Context = ItemSelector.Context.Plant,
          Offset = new Vector2(0.0f, 100f),
          HideOnSelection = true,
          RequiresDiscovery = true,
          ShowEmpty = true
        });
      itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
      {
        Debug.Log((object) $"ItemToDeposit {chosenItem}".Colour(Color.yellow));
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.Init((int) chosenItem, 1);
        this.ToDeposit.Add(inventoryItem);
        AudioManager.Instance.PlayOneShot("event:/material/footstep_sand", this.gameObject);
        ResourceCustomTarget.Create(this.gameObject, playerFarming.transform.position, chosenItem, new System.Action(this.PlantSeed));
        Inventory.ChangeItemQuantity((int) chosenItem, -1);
        this.UpdateCropImage();
        this.UpdateWatered();
      });
      UIItemSelectorOverlayController overlayController = itemSelector;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        if (UIItemSelectorOverlayController.CanRegainControl(playerFarming))
          state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
        itemSelector = (UIItemSelectorOverlayController) null;
        this.Interactable = true;
        this.HasChanged = true;
      });
    }
    else if (this.StructureBrain.CanWater() && !this._isWateringActivated)
    {
      base.OnInteract(state);
      this.IndicateHighlighted(playerFarming);
      this._isWateringActivated = true;
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.WaterRoutine());
    }
    else
    {
      if (!this.StructureBrain.CanFertilize())
        return;
      base.OnInteract(state);
      this.IndicateHighlighted(playerFarming);
      if (Inventory.GetItemQuantity(39) > 0 || Inventory.GetItemQuantity(144 /*0x90*/) > 0 || Inventory.GetItemQuantity(142) > 0 || Inventory.GetItemQuantity(143) > 0 || Inventory.GetItemQuantity(162) > 0 || Inventory.GetItemQuantity(187) > 0)
      {
        if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP) > 0 && Inventory.GetItemQuantity(144 /*0x90*/) <= 0 && Inventory.GetItemQuantity(142) <= 0 && Inventory.GetItemQuantity(143) <= 0 && Inventory.GetItemQuantity(162) <= 0 && Inventory.GetItemQuantity(187) <= 0)
        {
          this.AddFertiliser(InventoryItem.ITEM_TYPE.POOP);
        }
        else
        {
          this.Interactable = false;
          state.CURRENT_STATE = StateMachine.State.InActive;
          state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
          string str = "fertiliser";
          List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) InventoryItem.AllPoops);
          if (!this.StructureBrain.Data.DefrostedCrop && SeasonsManager.Active)
          {
            str = "fertiliser_including_rotburn";
            items.Add(InventoryItem.ITEM_TYPE.POOP_ROTSTONE);
          }
          UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(playerFarming, items, new ItemSelector.Params()
          {
            Key = str,
            Context = ItemSelector.Context.AddFertiliser,
            Offset = new Vector2(0.0f, 100f),
            HideOnSelection = true,
            RequiresDiscovery = true,
            ShowEmpty = true
          });
          itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
          {
            this.AddFertiliser(chosenItem);
            this.UpdateCropImage();
            this.UpdateWatered();
          });
          UIItemSelectorOverlayController overlayController = itemSelector;
          overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
          {
            if (UIItemSelectorOverlayController.CanRegainControl(playerFarming))
              state.CURRENT_STATE = StateMachine.State.Idle;
            itemSelector = (UIItemSelectorOverlayController) null;
            this.Interactable = true;
            this.HasChanged = true;
          });
        }
      }
      else
        playerFarming.indicator.PlayShake();
    }
  }

  public void AddFertiliser(InventoryItem.ITEM_TYPE chosenItem)
  {
    if (chosenItem == InventoryItem.ITEM_TYPE.POOP_ROTSTONE)
    {
      BiomeConstants.Instance.EmitSnowImpactVFX(this.transform.position);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    }
    this.StructureBrain.AddFertilizer(chosenItem);
    ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, chosenItem, new System.Action(this.AddFertilizer));
    Inventory.ChangeItemQuantity((int) chosenItem, -1);
  }

  public IEnumerator WaterRoutine()
  {
    FarmPlot farmPlot = this;
    farmPlot.EndIndicateHighlighted(farmPlot.playerFarming);
    farmPlot.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    farmPlot.state.facingAngle = Utils.GetAngle(farmPlot.state.transform.position, farmPlot.transform.position);
    farmPlot.loopedSound = AudioManager.Instance.CreateLoop("event:/player/watering", farmPlot.gameObject, true);
    yield return (object) null;
    farmPlot.playerFarming.simpleSpineAnimator.Animate("oldstuff/Farming-water_track", 0, true);
    while ((double) (farmPlot.WateringTime -= Time.deltaTime) >= 0.0)
      yield return (object) null;
    AudioManager.Instance.StopLoop(farmPlot.loopedSound);
    farmPlot._isWateringActivated = false;
    farmPlot.StructureInfo.Watered = true;
    farmPlot.StructureInfo.WateredCount = 0;
    farmPlot.WateringTime = 0.95f;
    farmPlot.state.CURRENT_STATE = StateMachine.State.Idle;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.WaterCrops);
    farmPlot.IndicateHighlighted(farmPlot.playerFarming);
    farmPlot.IndicateHighlighted(farmPlot.playerFarming);
  }

  public override void Update()
  {
    base.Update();
    if (this.StructureInfo == null || this.StructureBrain == null)
      return;
    if (this.StructureBrain.IsGroundFrozen && !this.StructureInfo.Watered)
      this.StructureInfo.Watered = true;
    else if (WeatherSystemController.Instance.IsRaining && !this.StructureInfo.Watered)
      this.StructureInfo.Watered = (double) UnityEngine.Random.Range(0.0f, 1f) < 0.004999999888241291;
    if (this.StructureInfo.Watered == this._watered)
      return;
    this.UpdateWatered();
  }

  public bool IsEnoughFertilizerEnRoute()
  {
    InventoryItem fertilizer = this.StructureBrain.GetFertilizer();
    return (fertilizer == null ? 0 : fertilizer.quantity) + this.ToDeposit.Count >= 1;
  }

  public void PlantSeed()
  {
    this.StructureBrain.PlantSeed((InventoryItem.ITEM_TYPE) this.ToDeposit[0].type);
    this.ToDeposit.RemoveAt(0);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlantCrops);
    this.UpdateCropImage();
    this.HasChanged = true;
    this.checkWaterIndicator();
  }

  public void AddFertilizer()
  {
    this.UpdateCropImage();
    if (!this.StructureBrain.CanFertilize())
      this.HasChanged = true;
    this.checkWaterIndicator();
    if (this.StructureBrain.GetFertilizer() == null)
      return;
    this.StructureBrain.Data.BenefitedFromFertilizer = true;
    if (this.StructureBrain.GetFertilizer().type != 143 || this.StructureBrain.IsFullyGrown)
      return;
    GameManager.GetInstance().WaitForSeconds(1f, new System.Action(this.StructureBrain.ForceFullyGrown));
  }

  public void CheckState()
  {
    Debug.Log((object) "========================= ");
    Debug.Log((object) ("checkWaterIndicator()  " + this.StructureBrain.CanWater().ToString()));
    Debug.Log((object) ("HasPlantedSeed() " + this.StructureBrain.HasPlantedSeed().ToString()));
    Debug.Log((object) ("!Data.Watered " + this.StructureBrain.Data.Watered.ToString()));
    Debug.Log((object) ("!IsFullyGrown " + this.StructureBrain.IsFullyGrown.ToString()));
  }

  public void checkWaterIndicator()
  {
    if (this.StructureBrain.CanWater())
      this.wateringIndicator.SetActive(true);
    else
      this.wateringIndicator.SetActive(false);
  }

  public void UpdateWatered()
  {
    this._watered = this.StructureInfo.Watered;
    if (this._watered)
    {
      if ((UnityEngine.Object) this.UnWateredGameObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.UnWateredGameObject);
      if (this.StructureBrain.Data.DefrostedCrop)
      {
        if ((UnityEngine.Object) this.WateredGameObject != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.WateredGameObject);
        if ((UnityEngine.Object) this.WateredRotGameObject == (UnityEngine.Object) null)
          this.WateredGameObject = UnityEngine.Object.Instantiate<GameObject>(MonoSingleton<UIManager>.Instance.WateredRotGameObjectTemplate, this.DirtTransform);
      }
      else if ((UnityEngine.Object) this.WateredGameObject == (UnityEngine.Object) null)
        this.WateredGameObject = UnityEngine.Object.Instantiate<GameObject>(MonoSingleton<UIManager>.Instance.WateredGameObjectTemplate, this.DirtTransform);
    }
    else
    {
      if ((UnityEngine.Object) this.WateredGameObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.WateredGameObject);
      if (this.StructureBrain.Data.DefrostedCrop)
      {
        if ((UnityEngine.Object) this.UnWateredGameObject != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.UnWateredGameObject);
        if ((UnityEngine.Object) this.UnWateredRotGameObject == (UnityEngine.Object) null)
          this.UnWateredRotGameObject = UnityEngine.Object.Instantiate<GameObject>(MonoSingleton<UIManager>.Instance.UnWateredRotGameObjectTemplate, this.DirtTransform);
      }
      else if ((UnityEngine.Object) this.UnWateredGameObject == (UnityEngine.Object) null)
        this.UnWateredGameObject = UnityEngine.Object.Instantiate<GameObject>(MonoSingleton<UIManager>.Instance.UnWateredGameObjectTemplate, this.DirtTransform);
    }
    this.EndIndicateHighlighted(this.playerFarming);
    this.HasChanged = true;
    this.checkWaterIndicator();
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public void UpdateScareCrowSymbol()
  {
    if (this.StructureBrain == null)
      return;
    bool scareCrowSymbol = this._scareCrowSymbol;
    this._scareCrowSymbol = this.StructureBrain.NearbyScarecrow() && !this.StructureBrain.Data.Withered;
    if (this._scareCrowSymbol == scareCrowSymbol || !((UnityEngine.Object) this.ScareCrowSymbol != (UnityEngine.Object) null))
      return;
    if (this._scareCrowSymbol)
    {
      this.ScareCrowSymbol.SetActive(true);
      this.ScareCrowSymbol.transform.localScale = Vector3.zero;
      this.ScareCrowSymbol.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
    else
      this.ScareCrowSymbol.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.ScareCrowSymbol.SetActive(false)));
  }

  public void Harvested()
  {
    this.StructureBrain.Harvest();
    this.UpdateCropImage();
    this.HasChanged = true;
  }

  public virtual void UpdateCropImage()
  {
    if (this.StructureBrain == null)
      return;
    if (this.StructureBrain.HasPlantedSeed())
    {
      if ((UnityEngine.Object) this._activeCropController == (UnityEngine.Object) null)
      {
        InventoryItem.ITEM_TYPE key = this.StructureBrain.GetPlantedSeed() != null ? (InventoryItem.ITEM_TYPE) this.StructureBrain.GetPlantedSeed().type : InventoryItem.ITEM_TYPE.NONE;
        if (key == InventoryItem.ITEM_TYPE.NONE)
          return;
        CropController original = this._cropPrefabsBySeedType.ContainsKey(key) ? this._cropPrefabsBySeedType[key] : (CropController) null;
        if ((bool) (UnityEngine.Object) original)
          this._activeCropController = UnityEngine.Object.Instantiate<CropController>(original, this.CropParent.transform);
      }
      try
      {
        this._activeCropController.SetCropImage(this.StructureInfo.GrowthStage, this.StructureInfo.BenefitedFromFertilizer, this.StructureInfo.Location);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex);
      }
    }
    else if ((UnityEngine.Object) this._activeCropController != (UnityEngine.Object) null)
    {
      this._activeCropController.HideAll();
      UnityEngine.Object.Destroy((UnityEngine.Object) this._activeCropController.gameObject);
      this._activeCropController = (CropController) null;
    }
    if (this.StructureBrain.HasFertilized())
    {
      this.UpdateFertilizerStatus(ref this.FertilizedObject, MonoSingleton<UIManager>.Instance.FertilizedObjectTemplate, 39);
      this.UpdateFertilizerStatus(ref this.FertilizedGoldObject, MonoSingleton<UIManager>.Instance.FertilizedGoldObjectTemplate, 142);
      this.UpdateFertilizerStatus(ref this.FertilizedGlowObject, MonoSingleton<UIManager>.Instance.FertilizedGlowObjectTemplate, 144 /*0x90*/);
      this.UpdateFertilizerStatus(ref this.FertilizedRainbowObject, MonoSingleton<UIManager>.Instance.FertilizedRainbowObjectTemplate, 143);
      this.UpdateFertilizerStatus(ref this.FertilizedDevotionObject, MonoSingleton<UIManager>.Instance.FertilizedDevotionObjectTemplate, 162);
    }
    else
    {
      if ((UnityEngine.Object) this.FertilizedObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.FertilizedObject);
      if ((UnityEngine.Object) this.FertilizedGoldObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.FertilizedGoldObject);
      if ((UnityEngine.Object) this.FertilizedGlowObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.FertilizedGlowObject);
      if ((UnityEngine.Object) this.FertilizedRainbowObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.FertilizedRainbowObject);
      if ((UnityEngine.Object) this.FertilizedDevotionObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.FertilizedDevotionObject);
    }
    if (this.StructureBrain.IsGroundFrozen)
    {
      if ((UnityEngine.Object) this.FrozenGroundObject == (UnityEngine.Object) null)
      {
        this.FrozenGroundObject = UnityEngine.Object.Instantiate<GameObject>(MonoSingleton<UIManager>.Instance.FrozenGroundObjectTemplate, this.transform);
        if ((double) SeasonsManager.SEASON_NORMALISED_PROGRESS < 0.10000000149011612)
          AudioManager.Instance.PlayOneShot("event:/dlc/env/plant/wither", this.transform.position);
      }
    }
    else if ((UnityEngine.Object) this.FrozenGroundObject != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.FrozenGroundObject);
    Interaction_Berries componentInChildren = this.GetComponentInChildren<Interaction_Berries>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
    {
      if (this.StructureBrain.HasFertilized() && this.StructureBrain.GetFertilizer().type == 162)
        componentInChildren.SetDevotionBerry(true, this.StructureBrain, new System.Action<float>(this.UpdateDevotionBar));
      if (this.StructureBrain.Data.Withered)
      {
        this.StructureInfo.SoulCount = 0;
        componentInChildren.SetWithered();
      }
    }
    if ((bool) (UnityEngine.Object) this._activeCropController)
    {
      this._activeCropController.SetGrowRateIcons(this.StructureBrain.NearbyHarvestTotem());
      this._activeCropController.SetCropGrowerIcons(this.StructureBrain.NearbyFarmCropGrower());
    }
    if (this.StructureBrain.IsGroundFrozen)
      return;
    this.checkWaterIndicator();
  }

  public void ShowSoil()
  {
    if (!((UnityEngine.Object) this.UnWateredGameObject == (UnityEngine.Object) null))
      return;
    this.UnWateredGameObject = UnityEngine.Object.Instantiate<GameObject>(MonoSingleton<UIManager>.Instance.UnWateredGameObjectTemplate, this.DirtTransform);
  }

  public void UpdateFertilizerStatus(ref GameObject go, GameObject template, int type)
  {
    if (type == 187 || this.StructureBrain.GetFertilizer().type == type)
    {
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
        return;
      go = UnityEngine.Object.Instantiate<GameObject>(template, this.transform);
    }
    else
    {
      if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) go);
    }
  }

  public void UpdateDevotionBar(float value)
  {
    if ((UnityEngine.Object) this.DevotionBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.DevotionBar.gameObject.SetActive(true);
    this.DevotionBar.UpdateBar(value);
  }

  public void OnFarmCropGrowerRotate(Interaction_FarmCropGrower farmCropGrower)
  {
    if (this.StructureBrain.Data.Withered || this.StructureBrain.GetFertilizer() != null && this.StructureBrain.GetFertilizer().type != 187)
      return;
    this.StructureBrain.GetFertilizer();
  }

  public void OnBirdAttack()
  {
    if (this.cBirdRoutine != null)
      return;
    this.BirdIsPlaying = true;
    this.BirdHasLanded = false;
    this.g = UnityEngine.Object.Instantiate<GameObject>(this.BirdPrefab, new Vector3(0.0f, 0.0f, -10f), Quaternion.identity, this.transform);
    this.g.SetActive(true);
    this.Bird = this.g.GetComponent<CritterBaseBird>();
    this.Bird.PrepareForSpawn();
    this.Bird.ManualControl = true;
    this.Bird.FlipTimerIntervals /= 2f;
    this.Bird.EatChance = 0.75f;
    this.cBirdRoutine = this.StartCoroutine((IEnumerator) this.BirdRoutine());
  }

  public IEnumerator BirdRoutine()
  {
    FarmPlot farmPlot = this;
    yield return (object) new WaitForEndOfFrame();
    Vector3 vector3 = Vector3.forward * -10f + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    farmPlot.Bird.transform.position = farmPlot.transform.position + vector3;
    farmPlot.Bird.FlyOutPosition = Vector3.forward * -10f;
    farmPlot.RandomPosition = farmPlot.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;
    farmPlot.Bird.TargetPosition = farmPlot.RandomPosition;
    if ((UnityEngine.Object) farmPlot.Bird.skeletonAnimationLODManager != (UnityEngine.Object) null)
      farmPlot.Bird.skeletonAnimationLODManager.DoUpdate = true;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.0f, 5f));
    farmPlot.Bird.CurrentState = CritterBaseBird.State.FlyingIn;
    while (farmPlot.Bird.CurrentState != CritterBaseBird.State.Idle)
      yield return (object) null;
    farmPlot.Bird.CurrentState = CritterBaseBird.State.Idle;
    farmPlot.cBirdRoutine = farmPlot.StartCoroutine((IEnumerator) farmPlot.LandedBird());
  }

  public IEnumerator LandedBird()
  {
    FarmPlot farmPlot = this;
    farmPlot.BirdHasLanded = true;
    yield return (object) new WaitForEndOfFrame();
    if (farmPlot.Bird.transform.position == farmPlot.Bird.FlyOutPosition)
      farmPlot.Bird.transform.position = farmPlot.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;
    float EatingTimer = 0.0f;
    while ((UnityEngine.Object) farmPlot.playerFarming == (UnityEngine.Object) null || (double) Vector3.Distance(farmPlot.transform.position, farmPlot.playerFarming.transform.position) > 2.0)
    {
      EatingTimer += Time.deltaTime;
      if (!((UnityEngine.Object) farmPlot.playerFarming != (UnityEngine.Object) null) || (double) Vector3.Distance(farmPlot.transform.position, farmPlot.playerFarming.transform.position) >= 5.0 || (double) EatingTimer <= 5.0)
        yield return (object) null;
      else
        break;
    }
    if ((double) EatingTimer > 5.0 && (bool) (UnityEngine.Object) farmPlot.playerFarming && (double) Vector3.Distance(farmPlot.transform.position, farmPlot.playerFarming.transform.position) > 2.0)
    {
      farmPlot.StructureBrain.Harvest();
      farmPlot.StructureInfo.Watered = WeatherSystemController.Instance.IsRaining || farmPlot.StructureBrain.IsGroundFrozen;
      farmPlot.StructureInfo.WateredCount = 0;
      farmPlot.UpdateCropImage();
    }
    farmPlot.checkWaterIndicator();
    farmPlot.StructureInfo.HasBird = false;
    farmPlot.Bird.FlyOut();
    while (farmPlot.Bird.CurrentState == CritterBaseBird.State.FlyingOut)
      yield return (object) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) farmPlot.g);
    farmPlot.cBirdRoutine = (Coroutine) null;
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StopCoroutine(this.cBirdRoutine);
    this.cBirdRoutine = (Coroutine) null;
    this.StructureInfo.HasBird = false;
  }

  [CompilerGenerated]
  public void \u003CUpdateScareCrowSymbol\u003Eb__86_0() => this.ScareCrowSymbol.SetActive(false);
}
