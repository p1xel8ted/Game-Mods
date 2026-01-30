// Decompiled with JetBrains decompiler
// Type: Structures_FarmerPlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class Structures_FarmerPlot : StructureBrain
{
  public const int FertilizerRequired = 1;
  public bool ReservedForWatering;
  public bool ReservedForSeeding;
  public bool ReservedForFertilizing;
  public System.Action OnGrowthStageChanged;
  public System.Action OnBirdAttack;
  public const float CHANCE_OF_WITHERING_IN_BLIZZARD = 0.35f;

  public int CropStatesCount
  {
    get
    {
      return CropController.CropStatesForSeedType((InventoryItem.ITEM_TYPE) this.GetPlantedSeed().type);
    }
  }

  public float growthTime
  {
    get => CropController.CropGrowthTimes((InventoryItem.ITEM_TYPE) this.GetPlantedSeed().type);
  }

  public bool IsFullyGrown
  {
    get => this.HasPlantedSeed() && (double) this.Data.GrowthStage >= (double) this.growthTime;
  }

  public bool IsGroundFrozen
  {
    get
    {
      return SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !this.Data.DefrostedCrop && SeasonsManager.WinterSeverity > 0;
    }
  }

  public bool GrowsDuringWinter
  {
    get
    {
      InventoryItem plantedSeed = this.GetPlantedSeed();
      return plantedSeed == null || this.NearbyFarmCropGrower() || plantedSeed.type == 166;
    }
  }

  public bool CanGrow(SeasonsManager.Season season)
  {
    if (this.NearbyFarmCropGrower() || this.HasFertilized() && this.GetFertilizer().type == 187 || this.Data.DefrostedCrop || season == SeasonsManager.Season.Winter && this.GrowsDuringWinter)
      return true;
    return season != SeasonsManager.Season.Winter && !this.GrowsDuringWinter;
  }

  public bool CanPlantSeed() => !this.HasPlantedSeed() && !this.IsGroundFrozen;

  public bool CanWater()
  {
    return this.HasPlantedSeed() && !this.Data.Watered && !this.IsFullyGrown && !this.IsGroundFrozen;
  }

  public bool NearbyScarecrow()
  {
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_Scarecrow structuresScarecrow in StructureManager.GetAllStructuresOfType<Structures_Scarecrow>())
    {
      if (structuresScarecrow.Data != null && !structuresScarecrow.Data.IsCollapsed && !structuresScarecrow.Data.IsSnowedUnder)
      {
        double num1 = (double) Vector3.Distance(this.Data.Position, structuresScarecrow.Data.Position);
        float num2 = Scarecrow.EFFECTIVE_DISTANCE(structuresScarecrow.Data.Type);
        Vector3 position = structuresScarecrow.Data.Position;
        boxCollider2D.transform.position = position;
        boxCollider2D.size = Vector2.one * num2;
        double num3 = (double) num2 + 0.5;
        if (num1 <= num3 && boxCollider2D.OverlapPoint((Vector2) this.Data.Position))
          return true;
      }
    }
    return false;
  }

  public bool NearbyHarvestTotem()
  {
    if ((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null)
      return false;
    float effectiveDistance = HarvestTotem.EFFECTIVE_DISTANCE;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * HarvestTotem.EFFECTIVE_DISTANCE;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_HarvestTotem structuresHarvestTotem in StructureManager.GetAllStructuresOfType<Structures_HarvestTotem>())
    {
      if (!structuresHarvestTotem.Data.IsCollapsed && !structuresHarvestTotem.Data.IsSnowedUnder)
      {
        double num1 = (double) Vector3.Distance(this.Data.Position, structuresHarvestTotem.Data.Position);
        Vector3 position = structuresHarvestTotem.Data.Position;
        boxCollider2D.transform.position = position;
        double num2 = (double) effectiveDistance + 0.5;
        if (num1 <= num2 && boxCollider2D.OverlapPoint((Vector2) this.Data.Position))
          return true;
      }
    }
    return false;
  }

  public bool NearbyFarmCropGrower()
  {
    if ((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null)
      return false;
    float num1 = Interaction_FarmCropGrower.EFFECTIVE_DISTANCE / 2f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_FarmCropGrower structuresFarmCropGrower in StructureManager.GetAllStructuresOfType<Structures_FarmCropGrower>())
    {
      if (!structuresFarmCropGrower.Data.IsCollapsed && !structuresFarmCropGrower.Data.IsSnowedUnder && structuresFarmCropGrower.Data.Fuel > 0)
      {
        Vector3 b = structuresFarmCropGrower.Data.Position + Interaction_FarmCropGrower.OFFSETS[structuresFarmCropGrower.Data.Rotation];
        double num2 = (double) Vector3.Distance(this.Data.Position, b);
        boxCollider2D.transform.position = b;
        boxCollider2D.size = Vector2.one * Interaction_FarmCropGrower.EFFECTIVE_DISTANCE;
        double num3 = (double) num1 + 0.5;
        if (num2 <= num3 && boxCollider2D.OverlapPoint((Vector2) this.Data.Position))
          return true;
      }
    }
    return false;
  }

  public bool CanFertilize() => this.Data.Watered && !this.HasFertilized();

  public bool CanPickCrop() => this.IsFullyGrown;

  public override void OnAdded()
  {
    base.OnAdded();
    SeasonsManager.OnWeatherBegan += new SeasonsManager.WeatherTypeEvent(this.SeasonsManager_OnWeatherBegan);
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    SeasonsManager.OnWeatherBegan -= new SeasonsManager.WeatherTypeEvent(this.SeasonsManager_OnWeatherBegan);
  }

  public override void OnSeasonChanged(SeasonsManager.Season season)
  {
    base.OnSeasonChanged(season);
    this.GetPlantedSeed();
  }

  public void SeasonsManager_OnWeatherBegan(SeasonsManager.WeatherEvent weatherEvent)
  {
    if (weatherEvent != SeasonsManager.WeatherEvent.Blizzard || (double) UnityEngine.Random.value >= 0.34999999403953552 || this.CanGrow(SeasonsManager.Season.Winter))
      return;
    this.SetWithered();
  }

  public override void OnNewPhaseStarted()
  {
    if (!this.IsFullyGrown && this.HasPlantedSeed() && !this.NearbyScarecrow() && (double) UnityEngine.Random.value <= 0.015 && this.GetPlantedSeed().type != 160 /*0xA0*/)
    {
      if ((UnityEngine.Object) FarmPlot.GetFarmPlot(this.Data.ID) != (UnityEngine.Object) null)
      {
        this.Data.HasBird = true;
        System.Action onBirdAttack = this.OnBirdAttack;
        if (onBirdAttack != null)
          onBirdAttack();
      }
      else
        this.Data.HasBird = true;
    }
    if (this.HasFertilized() && this.GetFertilizer().type == 162 && !this.Data.Withered)
      this.SoulCount = Mathf.Clamp(this.SoulCount + 1, 0, this.SoulMax);
    if (this.CanGrow(SeasonsManager.CurrentSeason))
    {
      if (!this.Data.Watered)
        return;
      if (!this.Data.HasBird)
        ++this.Data.GrowthStage;
      if (this.NearbyHarvestTotem() && !this.Data.HasBird)
        this.Data.GrowthStage += 0.5f;
      if (this.HasFertilized() && !this.Data.BenefitedFromFertilizer && !this.IsFullyGrown)
        this.Data.BenefitedFromFertilizer = true;
      if (this.IsFullyGrown && !this.Data.HasBird && this.HasFertilized() && (DataManager.Instance.FirstTimeFertilizing || UnityEngine.Random.Range(0, 3) == 0))
      {
        ++this.Data.GrowthStage;
        DataManager.Instance.FirstTimeFertilizing = false;
      }
      if (this.Data.Watered && ++this.Data.WateredCount >= 5)
        this.Data.Watered = WeatherSystemController.Instance.IsRaining || this.IsGroundFrozen;
      System.Action growthStageChanged = this.OnGrowthStageChanged;
      if (growthStageChanged == null)
        return;
      growthStageChanged();
    }
    else
    {
      if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard || (double) UnityEngine.Random.value >= 0.34999999403953552)
        return;
      this.SetWithered();
    }
  }

  public void PlantSeed(InventoryItem.ITEM_TYPE type)
  {
    this.Data.BenefitedFromFertilizer = false;
    this.DepositItem(type);
    this.Data.GrowthStage = 0.0f;
    Vector2Int harvestsPerSeedRange = CropController.GetHarvestsPerSeedRange(type);
    this.Data.RemainingHarvests = UnityEngine.Random.Range(harvestsPerSeedRange.x, harvestsPerSeedRange.y + 1);
  }

  public bool HasPlantedSeed() => this.GetPlantedSeed() != null;

  public InventoryItem GetPlantedSeed()
  {
    foreach (InventoryItem plantedSeed in this.Data.Inventory)
    {
      if (!InventoryItem.AllPoops.Contains((InventoryItem.ITEM_TYPE) plantedSeed.type) && plantedSeed.type > 0)
        return plantedSeed;
    }
    return (InventoryItem) null;
  }

  public static InventoryItem GetPlantedSeed(List<InventoryItem> inventory)
  {
    foreach (InventoryItem plantedSeed in inventory)
    {
      if (!InventoryItem.AllPoops.Contains((InventoryItem.ITEM_TYPE) plantedSeed.type) && plantedSeed.type > 0)
        return plantedSeed;
    }
    return (InventoryItem) null;
  }

  public void AddFertilizer(InventoryItem.ITEM_TYPE type)
  {
    if (type == InventoryItem.ITEM_TYPE.POOP_ROTSTONE)
    {
      this.Data.Watered = true;
      this.Data.WateredCount = 0;
      this.Data.DefrostedCrop = true;
    }
    else
      this.DepositItem(type);
  }

  public int RemainingFertilizerRequired()
  {
    InventoryItem fertilizer = this.GetFertilizer();
    return 1 - (fertilizer == null ? 0 : fertilizer.quantity);
  }

  public bool HasFertilized() => this.RemainingFertilizerRequired() <= 0;

  public InventoryItem GetFertilizer()
  {
    InventoryItem fertilizer = (InventoryItem) null;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      if (InventoryItem.AllPoops.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
        fertilizer = inventoryItem;
    }
    return fertilizer;
  }

  public void Harvest()
  {
    InventoryItem fertilizer = this.GetFertilizer();
    if (fertilizer != null)
      this.Data.Inventory.Remove(fertilizer);
    this.Data.Watered = WeatherSystemController.Instance.IsRaining || this.IsGroundFrozen;
    this.Data.WateredCount = 0;
    this.Data.GrowthStage = 0.0f;
    this.Data.Inventory.Clear();
    this.Data.Withered = false;
  }

  public override void ToDebugString(StringBuilder sb)
  {
    base.ToDebugString(sb);
    InventoryItem plantedSeed = this.GetPlantedSeed();
    InventoryItem.ITEM_TYPE itemType = plantedSeed == null ? InventoryItem.ITEM_TYPE.NONE : (InventoryItem.ITEM_TYPE) plantedSeed.type;
    InventoryItem fertilizer = this.GetFertilizer();
    int quantity = fertilizer == null ? 0 : fertilizer.quantity;
    sb.AppendLine($"Type: {itemType}, Growth: {this.Data.GrowthStage}, Fertilizer: {quantity}/{1}, Harvests: {this.Data.RemainingHarvests}");
  }

  public void ForceFullyGrown()
  {
    if (this.GetPlantedSeed() == null)
      return;
    this.Data.HasBird = false;
    this.Data.GrowthStage = this.growthTime;
    System.Action growthStageChanged = this.OnGrowthStageChanged;
    if (growthStageChanged == null)
      return;
    growthStageChanged();
  }

  public void SetWithered()
  {
    if (this.Data.Withered)
      return;
    InventoryItem plantedSeed = this.GetPlantedSeed();
    if (plantedSeed == null || plantedSeed.type == 160 /*0xA0*/ || (this.GetFertilizer() == null || this.GetFertilizer().type == 187) && this.GetFertilizer() != null)
      return;
    if (PlayerFarming.Location == FollowerLocation.Base)
      BiomeConstants.Instance.EmitSmokeInteractionVFX(this.Data.Position, Vector3.one / 2f);
    this.Data.Withered = true;
    System.Action growthStageChanged = this.OnGrowthStageChanged;
    if (growthStageChanged == null)
      return;
    growthStageChanged();
  }

  public InventoryItem.ITEM_TYPE GetPrioritisedSeedType(List<StructureBrain> signs = null)
  {
    StructureBrain structureBrain = (StructureBrain) null;
    float num1 = 5f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * 5f;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    if (signs == null)
      signs = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.FARM_PLOT_SIGN);
    foreach (StructureBrain sign in signs)
    {
      float num2 = Vector3.Distance(this.Data.Position, sign.Data.Position);
      Vector3 position = sign.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num2 < (double) num1)
      {
        structureBrain = sign;
        num1 = num2;
      }
    }
    return structureBrain != null && structureBrain.Data.SignPostItem != InventoryItem.ITEM_TYPE.NONE ? InventoryItem.GetSeedType(structureBrain.Data.SignPostItem) : InventoryItem.ITEM_TYPE.NONE;
  }
}
