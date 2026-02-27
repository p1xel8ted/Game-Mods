// Decompiled with JetBrains decompiler
// Type: FarmPlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FarmPlot : Interaction
{
  private const float WATERING_DURATION_SECONDS = 0.95f;
  public GameObject WateredGameObject;
  public GameObject UnWateredGameObject;
  public GameObject CropParent;
  public GameObject FertilizedObject;
  public GameObject ScareCrowSymbol;
  private bool _scareCrowSymbol;
  public static List<FarmPlot> FarmPlots = new List<FarmPlot>();
  public Structure Structure;
  private Structures_FarmerPlot _StructureInfo;
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypesToPlant = new List<InventoryItem.ITEM_TYPE>();
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypesToFertilize = new List<InventoryItem.ITEM_TYPE>();
  public GameObject SeedIndicatorPrefab;
  public List<CropController> CropPrefabs;
  private Dictionary<InventoryItem.ITEM_TYPE, CropController> _cropPrefabsBySeedType;
  [SerializeField]
  private GameObject wateringIndicator;
  private GameObject Player;
  private List<InventoryItem> ToDeposit = new List<InventoryItem>();
  private float WateringTime = 0.95f;
  private float Delay;
  private bool _watered;
  private bool _isCurrentInteraction;
  private bool beingMoved;
  private string sPlant;
  private string sWater;
  private string sFertilize;
  private EventInstance loopedSound;
  public GameObject BirdPrefab;
  public CritterBaseBird Bird;
  private Coroutine cBirdRoutine;
  private GameObject g;
  private bool BirdIsPlaying;
  private bool BirdHasLanded;
  private Vector3 RandomPosition;

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

  public CropController _activeCropController { get; private set; }

  private void Awake()
  {
    this._cropPrefabsBySeedType = new Dictionary<InventoryItem.ITEM_TYPE, CropController>();
    foreach (CropController cropPrefab in this.CropPrefabs)
      this._cropPrefabsBySeedType.Add(cropPrefab.SeedType, cropPrefab);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
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
    FarmPlot.FarmPlots.Add(this);
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

  private void OnStructuresPlaced()
  {
    this.UpdateCropImage();
    this.UpdateScareCrowSymbol();
  }

  private void OnStructuresMoved(StructuresData structure)
  {
    this.UpdateCropImage();
    this.UpdateScareCrowSymbol();
  }

  private void OnBrainAssigned()
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
        this.Bird.bird.enabled = true;
        this.cBirdRoutine = this.StartCoroutine((IEnumerator) this.LandedBird());
      }
    }
    this.UpdateLocalisation();
    this.UpdateWatered();
    this.UpdateCropImage();
  }

  private void OnStructureAdded(StructuresData structure)
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
    FarmPlot.FarmPlots.Remove(this);
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

  private void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = true;
  }

  private void OnBuildingPlaced(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = false;
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this._isCurrentInteraction = true;
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    this._isCurrentInteraction = false;
  }

  public override void GetLabel()
  {
    if (this.StructureBrain.CanPickCrop())
    {
      this.ContinuouslyHold = false;
      this.Label = "";
    }
    else if (this.StructureBrain.CanPlantSeed())
    {
      this.ContinuouslyHold = false;
      this.Label = this.ToDeposit.Count == 0 ? this.sPlant : "";
    }
    else if (this.StructureBrain.CanWater())
    {
      this.ContinuouslyHold = false;
      this.Label = this.sWater;
    }
    else if (this.StructureBrain.CanFertilize())
    {
      this.ContinuouslyHold = true;
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
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.StructureBrain.CanPlantSeed())
    {
      base.OnInteract(state);
      this.IndicateHighlighted();
      this.Interactable = false;
      state.CURRENT_STATE = StateMachine.State.InActive;
      state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
      UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(InventoryItem.AllSeeds, new ItemSelector.Params()
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
        ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, chosenItem, new System.Action(this.PlantSeed));
        Inventory.ChangeItemQuantity((int) chosenItem, -1);
      });
      UIItemSelectorOverlayController overlayController = itemSelector;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        state.CURRENT_STATE = StateMachine.State.Idle;
        itemSelector = (UIItemSelectorOverlayController) null;
        this.Interactable = true;
        this.HasChanged = true;
      });
    }
    else if (this.StructureBrain.CanWater())
    {
      base.OnInteract(state);
      this.IndicateHighlighted();
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.WaterRoutine());
    }
    else
    {
      if (!this.StructureBrain.CanFertilize())
        return;
      base.OnInteract(state);
      this.IndicateHighlighted();
      InventoryItem.ITEM_TYPE type = InventoryItem.ITEM_TYPE.POOP;
      if (Inventory.GetItemQuantity((int) type) > 0)
      {
        this.StructureBrain.AddFertilizer(type);
        ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, type, new System.Action(this.AddFertilizer));
        Inventory.ChangeItemQuantity((int) type, -1);
      }
      else
        MonoSingleton<Indicator>.Instance.PlayShake();
    }
  }

  private IEnumerator WaterRoutine()
  {
    FarmPlot farmPlot = this;
    farmPlot.EndIndicateHighlighted();
    farmPlot.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    farmPlot.state.facingAngle = Utils.GetAngle(farmPlot.state.transform.position, farmPlot.transform.position);
    farmPlot.loopedSound = AudioManager.Instance.CreateLoop("event:/player/watering", farmPlot.gameObject, true);
    yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("oldstuff/Farming-water_track", 0, true);
    while ((double) (farmPlot.WateringTime -= Time.deltaTime) >= 0.0)
      yield return (object) null;
    AudioManager.Instance.StopLoop(farmPlot.loopedSound);
    farmPlot.StructureInfo.Watered = true;
    farmPlot.StructureInfo.WateredCount = 0;
    farmPlot.WateringTime = 0.95f;
    farmPlot.state.CURRENT_STATE = StateMachine.State.Idle;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.WaterCrops);
    farmPlot.IndicateHighlighted();
    farmPlot.IndicateHighlighted();
  }

  protected override void Update()
  {
    base.Update();
    if (this.StructureInfo == null)
      return;
    if (WeatherController.isRaining && !this.StructureInfo.Watered)
      this.StructureInfo.Watered = (double) UnityEngine.Random.Range(0.0f, 1f) < 0.004999999888241291;
    if (this.StructureInfo.Watered == this._watered)
      return;
    this.UpdateWatered();
  }

  private bool IsEnoughFertilizerEnRoute()
  {
    InventoryItem fertilizer = this.StructureBrain.GetFertilizer();
    return (fertilizer == null ? 0 : fertilizer.quantity) + this.ToDeposit.Count >= 1;
  }

  private void PlantSeed()
  {
    this.StructureBrain.PlantSeed((InventoryItem.ITEM_TYPE) this.ToDeposit[0].type);
    this.ToDeposit.RemoveAt(0);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlantCrops);
    this.UpdateCropImage();
    this.HasChanged = true;
    this.checkWaterIndicator();
  }

  private void AddFertilizer()
  {
    this.UpdateCropImage();
    if (!this.StructureBrain.CanFertilize())
      this.HasChanged = true;
    this.checkWaterIndicator();
  }

  private void CheckState()
  {
    Debug.Log((object) "========================= ");
    Debug.Log((object) ("checkWaterIndicator()  " + this.StructureBrain.CanWater().ToString()));
    Debug.Log((object) ("HasPlantedSeed() " + this.StructureBrain.HasPlantedSeed().ToString()));
    Debug.Log((object) ("!Data.Watered " + this.StructureBrain.Data.Watered.ToString()));
    Debug.Log((object) ("!IsFullyGrown " + this.StructureBrain.IsFullyGrown.ToString()));
  }

  private void checkWaterIndicator()
  {
    if (this.StructureBrain.CanWater())
      this.wateringIndicator.SetActive(true);
    else
      this.wateringIndicator.SetActive(false);
  }

  private void UpdateWatered()
  {
    this._watered = this.StructureInfo.Watered;
    this.WateredGameObject.SetActive(this._watered);
    this.UnWateredGameObject.SetActive(!this._watered);
    this.HasChanged = true;
    this.checkWaterIndicator();
  }

  private void UpdateScareCrowSymbol()
  {
    if (this.StructureBrain == null)
      return;
    bool scareCrowSymbol = this._scareCrowSymbol;
    this._scareCrowSymbol = this.StructureBrain.NearbyScarecrow();
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
    this.FertilizedObject.SetActive(this.StructureBrain.HasFertilized());
    if ((bool) (UnityEngine.Object) this._activeCropController)
      this._activeCropController.SetGrowRateIcons(this.StructureBrain.NearbyHarvestTotem());
    this.checkWaterIndicator();
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
    this.Bird.ManualControl = true;
    this.Bird.FlipTimerIntervals /= 2f;
    this.Bird.EatChance = 0.75f;
    this.cBirdRoutine = this.StartCoroutine((IEnumerator) this.BirdRoutine());
  }

  private IEnumerator BirdRoutine()
  {
    FarmPlot farmPlot = this;
    yield return (object) new WaitForEndOfFrame();
    Vector3 vector3 = Vector3.forward * -10f + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    farmPlot.Bird.transform.position = farmPlot.transform.position + vector3;
    farmPlot.Bird.FlyOutPosition = Vector3.forward * -10f;
    farmPlot.RandomPosition = farmPlot.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;
    farmPlot.Bird.TargetPosition = farmPlot.RandomPosition;
    farmPlot.Bird.bird.enabled = true;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.0f, 5f));
    farmPlot.Bird.CurrentState = CritterBaseBird.State.FlyingIn;
    while (farmPlot.Bird.CurrentState != CritterBaseBird.State.Idle)
      yield return (object) null;
    farmPlot.Bird.CurrentState = CritterBaseBird.State.Idle;
    farmPlot.cBirdRoutine = farmPlot.StartCoroutine((IEnumerator) farmPlot.LandedBird());
  }

  private IEnumerator LandedBird()
  {
    FarmPlot farmPlot = this;
    farmPlot.BirdHasLanded = true;
    yield return (object) new WaitForEndOfFrame();
    if (farmPlot.Bird.transform.position == farmPlot.Bird.FlyOutPosition)
      farmPlot.Bird.transform.position = farmPlot.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;
    float EatingTimer = 0.0f;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || (double) Vector3.Distance(farmPlot.transform.position, PlayerFarming.Instance.transform.position) > 2.0)
    {
      EatingTimer += Time.deltaTime;
      if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || (double) Vector3.Distance(farmPlot.transform.position, PlayerFarming.Instance.transform.position) >= 5.0 || (double) EatingTimer <= 5.0)
        yield return (object) null;
      else
        break;
    }
    if ((double) EatingTimer > 5.0 && (bool) (UnityEngine.Object) PlayerFarming.Instance && (double) Vector3.Distance(farmPlot.transform.position, PlayerFarming.Instance.transform.position) > 2.0)
    {
      farmPlot.StructureBrain.Harvest();
      farmPlot.StructureInfo.Watered = WeatherController.isRaining;
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

  private void OnDie(
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
}
