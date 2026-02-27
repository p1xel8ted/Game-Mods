// Decompiled with JetBrains decompiler
// Type: Structures_FarmerPlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  public bool CanPlantSeed() => !this.HasPlantedSeed();

  public bool CanWater() => this.HasPlantedSeed() && !this.Data.Watered && !this.IsFullyGrown;

  public bool NearbyScarecrow()
  {
    float num1 = Scarecrow.EFFECTIVE_DISTANCE(this.Data.Type);
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * Scarecrow.EFFECTIVE_DISTANCE(this.Data.Type);
    boxCollider2D.transform.position = this.Data.Position;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_Scarecrow structuresScarecrow in StructureManager.GetAllStructuresOfType<Structures_Scarecrow>())
    {
      float num2 = Vector3.Distance(this.Data.Position, structuresScarecrow.Data.Position);
      Vector3 position = structuresScarecrow.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num2 < (double) num1)
        return true;
    }
    return false;
  }

  public bool NearbyHarvestTotem()
  {
    float effectiveDistance = HarvestTotem.EFFECTIVE_DISTANCE;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * HarvestTotem.EFFECTIVE_DISTANCE;
    boxCollider2D.transform.position = this.Data.Position;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_HarvestTotem structuresHarvestTotem in StructureManager.GetAllStructuresOfType<Structures_HarvestTotem>())
    {
      float num = Vector3.Distance(this.Data.Position, structuresHarvestTotem.Data.Position);
      Vector3 position = structuresHarvestTotem.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num < (double) effectiveDistance)
        return true;
    }
    return false;
  }

  public bool CanFertilize() => this.Data.Watered && !this.HasFertilized();

  public bool CanPickCrop() => this.IsFullyGrown;

  public override void OnAdded()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public override void OnRemoved()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted()
  {
    if (!this.IsFullyGrown && this.HasPlantedSeed() && !this.NearbyScarecrow() && (double) UnityEngine.Random.value <= 0.015)
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
      this.Data.Watered = WeatherController.isRaining;
    System.Action growthStageChanged = this.OnGrowthStageChanged;
    if (growthStageChanged == null)
      return;
    growthStageChanged();
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
      if (plantedSeed.type != 39 && plantedSeed.type > 0)
        return plantedSeed;
    }
    return (InventoryItem) null;
  }

  public void AddFertilizer(InventoryItem.ITEM_TYPE type) => this.DepositItem(type);

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
      if (inventoryItem.type == 39)
        fertilizer = inventoryItem;
    }
    return fertilizer;
  }

  public void Harvest()
  {
    InventoryItem fertilizer = this.GetFertilizer();
    if (fertilizer != null)
      this.Data.Inventory.Remove(fertilizer);
    this.Data.Watered = WeatherController.isRaining;
    this.Data.WateredCount = 0;
    this.Data.GrowthStage = 0.0f;
    this.Data.Inventory.Clear();
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
}
