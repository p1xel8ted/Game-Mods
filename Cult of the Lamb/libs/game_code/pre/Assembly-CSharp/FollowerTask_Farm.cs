// Decompiled with JetBrains decompiler
// Type: FollowerTask_Farm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Farm : FollowerTask
{
  public const float WATERING_DURATION_GAME_MINUTES = 10f;
  public const float SEEDING_DURATION_GAME_MINUTES = 5f;
  public const float FERTILIZING_DURATION_GAME_MINUTES = 4f;
  public const float PICKING_DURATION_GAME_MINUTES = 10f;
  private int _farmerStationID;
  private Structures_FarmerStation _farmerStation;
  private int _farmPlotID;
  private int _siloFertiziler;
  private int _siloSeeder;
  private int _cropID;
  private int _previousSiloFertiziler;
  private int _previousSiloSeeder;
  private bool targetFarmerStation;
  private float _progress;
  private float _gameTimeSinceLastProgress;
  private int seedTypeToPlant = -1;
  private bool holdingPoop;
  private Follower _follower;
  private Structures_BerryBush currentCrop;

  public override FollowerTaskType Type => FollowerTaskType.Farm;

  public override FollowerLocation Location => this._farmerStation.Data.Location;

  public override int UsingStructureID => this._farmerStationID;

  public override bool BlockReactTasks => true;

  public override float Priorty => 22f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Lumberjack:
      case FollowerRole.Monk:
        return PriorityCategory.Low;
      case FollowerRole.Farmer:
        return PriorityCategory.WorkPriority;
      default:
        return PriorityCategory.Low;
    }
  }

  public FollowerTask_Farm(int farmerStationID)
  {
    this._farmerStationID = farmerStationID;
    this._farmerStation = StructureManager.GetStructureByID<Structures_FarmerStation>(this._farmerStationID);
    this._farmPlotID = 0;
    this._siloFertiziler = 0;
    this._siloSeeder = 0;
    this.targetFarmerStation = false;
  }

  protected override int GetSubTaskCode() => this._farmerStationID;

  public override void ClaimReservations()
  {
    Structures_FarmerStation structureById1 = StructureManager.GetStructureByID<Structures_FarmerStation>(this._farmerStationID);
    if (structureById1 != null)
      structureById1.ReservedForTask = true;
    Structures_FarmerPlot structureById2 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
    if (structureById2 != null)
    {
      if (structureById2.CanPlantSeed())
        structureById2.ReservedForSeeding = true;
      else if (structureById2.CanWater())
      {
        structureById2.ReservedForWatering = true;
      }
      else
      {
        if (!structureById2.CanFertilize())
          return;
        structureById2.ReservedForFertilizing = true;
      }
    }
    else
    {
      Structures_FarmerPlot structureById3 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._cropID);
      if (structureById3 == null)
        return;
      structureById3.ReservedForTask = true;
    }
  }

  public override void ReleaseReservations()
  {
    Structures_FarmerStation structureById1 = StructureManager.GetStructureByID<Structures_FarmerStation>(this._farmerStationID);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    Structures_FarmerPlot structureById2 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
    if (structureById2 != null)
    {
      if (structureById2.ReservedForSeeding)
        structureById2.ReservedForSeeding = false;
      else if (structureById2.ReservedForWatering)
      {
        structureById2.ReservedForWatering = false;
      }
      else
      {
        if (!structureById2.ReservedForFertilizing)
          return;
        structureById2.ReservedForFertilizing = false;
      }
    }
    else
    {
      Structures_FarmerPlot structureById3 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._cropID);
      if (structureById3 == null)
        return;
      structureById3.ReservedForTask = false;
    }
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    float num = 1f;
    if (this._cropID != 0 && (double) this._progress < 10.0 && this.currentCrop != null && this.currentCrop.BerryPicked)
    {
      this._cropID = 0;
      this._progress = 0.0f;
      this.currentCrop = (Structures_BerryBush) null;
      this.ProgressTask();
    }
    this._gameTimeSinceLastProgress += deltaGameTime * num;
    this.ProgressTask();
  }

  public override void ProgressTask()
  {
    Structures_FarmerPlot structureById = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
    if (structureById == null)
    {
      this.currentCrop = StructureManager.GetStructureByID<Structures_BerryBush>(this._cropID);
      if (this.currentCrop != null && !this.currentCrop.ReservedByPlayer)
      {
        this.currentCrop.Data.ProgressTarget = 10f;
        this._progress += this._gameTimeSinceLastProgress;
        this.currentCrop.PickBerries(this._gameTimeSinceLastProgress, false);
        this._gameTimeSinceLastProgress = 0.0f;
        if ((double) this._progress < 10.0)
          return;
        this._progress = 0.0f;
        List<InventoryItem.ITEM_TYPE> berries = this.currentCrop.GetBerries();
        foreach (InventoryItem.ITEM_TYPE Type in berries)
        {
          if (this._brain != null && this._brain._directInfoAccess != null && this._brain._directInfoAccess.Inventory != null)
            this._brain._directInfoAccess.Inventory.Add(new InventoryItem(Type, 1));
        }
        List<Structures_BerryBush> cropsAtPosition = this._farmerStation.GetCropsAtPosition(this.currentCrop.Data.Position);
        for (int index = cropsAtPosition.Count - 1; index >= 0; --index)
        {
          if (cropsAtPosition[index] != null)
            cropsAtPosition[index].PickBerries(100f, false);
        }
        this._brain.GetXP(1f);
        this._cropID = 0;
        this.currentCrop = (Structures_BerryBush) null;
        this.targetFarmerStation = berries.Count > 0;
        this.UpdatePlot();
        this.Loop();
      }
      else
        this.Loop();
    }
    else if (structureById.ReservedForSeeding)
    {
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 5.0)
        return;
      this._progress = 0.0f;
      if (structureById.CanPlantSeed())
      {
        structureById.Data.Inventory.Add(new InventoryItem((InventoryItem.ITEM_TYPE) this.seedTypeToPlant, 1));
        structureById.PlantSeed((InventoryItem.ITEM_TYPE) this.seedTypeToPlant);
      }
      else
        this.RefundSeed();
      this.seedTypeToPlant = -1;
      structureById.ReservedForSeeding = false;
      this._brain.GetXP(1f);
      this.UpdatePlot();
      this.Loop();
    }
    else if (structureById.ReservedForWatering)
    {
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 10.0)
        return;
      this._progress = 0.0f;
      structureById.Data.Watered = true;
      structureById.Data.WateredCount = 0;
      structureById.ReservedForWatering = false;
      this._brain.GetXP(1f);
      this.UpdatePlot();
      this.Loop();
    }
    else if (structureById.ReservedForFertilizing)
    {
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 4.0)
        return;
      this._progress = 0.0f;
      if (structureById.CanFertilize())
      {
        structureById.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.POOP, 1));
        structureById.AddFertilizer(InventoryItem.ITEM_TYPE.POOP);
      }
      else
        this.RefundPoop();
      this.holdingPoop = false;
      structureById.ReservedForFertilizing = false;
      this._brain.GetXP(1f);
      this.UpdatePlot();
      this.Loop();
    }
    else
      this.Loop();
  }

  protected override void OnAbort()
  {
    base.OnAbort();
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (!this.holdingPoop)
      return;
    this.RefundPoop();
  }

  protected override void OnComplete()
  {
    base.OnComplete();
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (!this.holdingPoop)
      return;
    this.RefundPoop();
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (!this.holdingPoop)
      return;
    this.RefundPoop();
  }

  private void RefundSeed()
  {
    Structures_SiloSeed structureById = StructureManager.GetStructureByID<Structures_SiloSeed>(this._previousSiloSeeder);
    if (structureById == null)
      return;
    structureById.Data.Inventory.Add(new InventoryItem()
    {
      type = this.seedTypeToPlant,
      quantity = 1
    });
    foreach (Interaction_SiloSeeder siloSeeder in Interaction_SiloSeeder.SiloSeeders)
    {
      if (siloSeeder.StructureBrain.Data.ID == this._previousSiloSeeder)
      {
        siloSeeder.UpdateCapacityIndicators();
        break;
      }
    }
    this.seedTypeToPlant = -1;
  }

  private void RefundPoop()
  {
    Structures_SiloFertiliser structureById = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._previousSiloFertiziler);
    if (structureById == null)
      return;
    structureById.Data.Inventory.Add(new InventoryItem()
    {
      type = 39,
      quantity = 1
    });
    foreach (Interaction_SiloFertilizer siloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
    {
      if (siloFertilizer.StructureBrain.Data.ID == this._previousSiloFertiziler)
      {
        siloFertilizer.UpdateCapacityIndicators();
        break;
      }
    }
    this.holdingPoop = false;
  }

  private void UpdatePlot()
  {
    FarmPlot farmPlot = this.FindFarmPlot();
    if (!((UnityEngine.Object) farmPlot != (UnityEngine.Object) null))
      return;
    farmPlot.UpdateCropImage();
  }

  private void Loop()
  {
    if (this.targetFarmerStation)
    {
      this.ClearDestination();
      this._farmPlotID = 0;
      this._siloFertiziler = 0;
      this._siloSeeder = 0;
      this.SetState(FollowerTaskState.GoingTo);
      if (!(bool) (UnityEngine.Object) this._follower)
        return;
      this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetMovementAnim());
    }
    else
    {
      Structures_BerryBush nextUnpickedPlot = this._farmerStation.GetNextUnpickedPlot();
      if (nextUnpickedPlot == null || this._farmerStation.Data.Type != StructureBrain.TYPES.FARM_STATION_II)
      {
        Structures_FarmerPlot nextUnseededPlot = this._farmerStation.GetNextUnseededPlot();
        if (nextUnseededPlot == null || Structures_SiloSeed.GetClosestSeeder(this._brain.LastPosition, this._brain.Location) == null)
        {
          Structures_FarmerPlot nextUnwateredPlot = this._farmerStation.GetNextUnwateredPlot();
          if (nextUnwateredPlot == null)
          {
            Structures_FarmerPlot unfertilizedPlot = this._farmerStation.GetNextUnfertilizedPlot();
            if (unfertilizedPlot == null || Structures_SiloFertiliser.GetClosestFertiliser(this._brain.LastPosition, this._brain.Location) == null)
            {
              this.End();
            }
            else
            {
              this.ClearDestination();
              this._farmPlotID = unfertilizedPlot.Data.ID;
              this._cropID = 0;
              unfertilizedPlot.ReservedForFertilizing = true;
              this.NextTarget(unfertilizedPlot);
              this.SetState(FollowerTaskState.GoingTo);
            }
          }
          else
          {
            this.ClearDestination();
            this._farmPlotID = nextUnwateredPlot.Data.ID;
            this._cropID = 0;
            nextUnwateredPlot.ReservedForWatering = true;
            this.SetState(FollowerTaskState.GoingTo);
          }
        }
        else
        {
          this.ClearDestination();
          this._farmPlotID = nextUnseededPlot.Data.ID;
          this._cropID = 0;
          nextUnseededPlot.ReservedForSeeding = true;
          this.NextTarget(nextUnseededPlot);
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      else
      {
        this.ClearDestination();
        this._cropID = nextUnpickedPlot.Data.ID;
        this._farmPlotID = 0;
        this.currentCrop = nextUnpickedPlot;
        nextUnpickedPlot.ReservedForTask = true;
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
  }

  private void NextTarget(Structures_FarmerPlot nextPlot)
  {
    if (nextPlot.ReservedForSeeding)
    {
      Structures_SiloSeed closestSeeder = Structures_SiloSeed.GetClosestSeeder(this._brain.LastPosition, this._brain.Location);
      if (closestSeeder == null)
        return;
      this._siloSeeder = closestSeeder.Data.ID;
      this._previousSiloSeeder = closestSeeder.Data.ID;
    }
    else
    {
      if (!nextPlot.ReservedForFertilizing)
        return;
      Structures_SiloFertiliser closestFertiliser = Structures_SiloFertiliser.GetClosestFertiliser(this._brain.LastPosition, this._brain.Location);
      if (closestFertiliser == null)
        return;
      this._siloFertiziler = closestFertiliser.Data.ID;
      this._previousSiloFertiziler = closestFertiliser.Data.ID;
    }
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3 = new Vector3();
    Structures_FarmerPlot structureById1 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    Structures_SiloFertiliser structureById3 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    Structures_BerryBush structureById4 = StructureManager.GetStructureByID<Structures_BerryBush>(this._cropID);
    if (structureById2 != null)
      vector3 = structureById2.Data.Position;
    else if (structureById3 != null)
      vector3 = structureById3.Data.Position;
    else if (structureById4 != null)
      vector3 = structureById4.Data.Position;
    else if (structureById1 != null)
    {
      vector3 = structureById1.Data.Position + new Vector3(-Farm.FarmTileSize, 0.0f, 0.0f);
    }
    else
    {
      FarmStation farmStation = this.FindFarmStation();
      if ((UnityEngine.Object) farmStation != (UnityEngine.Object) null)
        vector3 = farmStation.WorshipperPosition.transform.position;
      else
        this.Abort();
    }
    return vector3;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this._farmPlotID != 0)
      follower.SetHat(HatType.Farm);
    this._follower = follower;
  }

  private string GetMovementAnim()
  {
    foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
    {
      switch ((InventoryItem.ITEM_TYPE) inventoryItem.type)
      {
        case InventoryItem.ITEM_TYPE.BERRY:
          return "Farming/run-berries";
        case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
          return "Farming/run-mushroom";
        case InventoryItem.ITEM_TYPE.PUMPKIN:
          return "Farming/run-pumpkin";
        case InventoryItem.ITEM_TYPE.FLOWER_RED:
          return "Farming/run-redflower";
        case InventoryItem.ITEM_TYPE.BEETROOT:
          return "Farming/run-beetroot";
        case InventoryItem.ITEM_TYPE.CAULIFLOWER:
          return "Farming/run-cauliflower";
        default:
          continue;
      }
    }
    return "Farming/run-berries";
  }

  private string GetDepositFoodAnim()
  {
    foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
    {
      switch ((InventoryItem.ITEM_TYPE) inventoryItem.type)
      {
        case InventoryItem.ITEM_TYPE.BERRY:
          return "Farming/add-berries";
        case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
          return "Farming/add-mushroom";
        case InventoryItem.ITEM_TYPE.PUMPKIN:
          return "Farming/add-pumpkin";
        case InventoryItem.ITEM_TYPE.FLOWER_RED:
          return "Farming/add-redflower";
        case InventoryItem.ITEM_TYPE.BEETROOT:
          return "Farming/add-beetroot";
        case InventoryItem.ITEM_TYPE.CAULIFLOWER:
          return "Farming/add-cauliflower";
        default:
          continue;
      }
    }
    return "Farming/add-berries";
  }

  public override void OnDoingBegin(Follower follower)
  {
    this._follower = follower;
    this.DoingBegin(follower);
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    this._follower = follower;
  }

  public override void SimDoingBegin(SimFollower simFollower) => this.DoingBegin((Follower) null);

  private void DoingBegin(Follower follower)
  {
    if ((bool) (UnityEngine.Object) follower)
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (this._siloFertiziler != 0)
    {
      Structures_SiloFertiliser structureById = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
      if (structureById.Data.Inventory.Count <= 0)
      {
        this.End();
      }
      else
      {
        this.holdingPoop = true;
        --structureById.Data.Inventory[0].quantity;
        if (structureById.Data.Inventory[0].quantity <= 0)
          structureById.Data.Inventory.RemoveAt(0);
        foreach (Interaction_SiloFertilizer siloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
        {
          if (siloFertilizer.StructureBrain.Data.ID == this._siloFertiziler)
          {
            siloFertilizer.UpdateCapacityIndicators();
            break;
          }
        }
        if ((bool) (UnityEngine.Object) follower)
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-poop");
        this._siloFertiziler = 0;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
    else if (this._siloSeeder != 0)
    {
      Structures_SiloSeed structureById = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
      if (structureById == null || structureById.Data.Inventory.Count <= 0)
      {
        this.End();
      }
      else
      {
        --structureById.Data.Inventory[0].quantity;
        this.seedTypeToPlant = structureById.Data.Inventory[0].type;
        if (structureById.Data.Inventory[0].quantity <= 0)
          structureById.Data.Inventory.RemoveAt(0);
        foreach (Interaction_SiloSeeder siloSeeder in Interaction_SiloSeeder.SiloSeeders)
        {
          if (siloSeeder.StructureBrain.Data.ID == this._siloSeeder)
          {
            siloSeeder.UpdateCapacityIndicators();
            break;
          }
        }
        if ((bool) (UnityEngine.Object) follower)
        {
          if (this.seedTypeToPlant == 8)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed");
          else if (this.seedTypeToPlant == 70)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-mushroom");
          else if (this.seedTypeToPlant == 51)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-pumpkin");
          else if (this.seedTypeToPlant == 72)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-redflower");
          else if (this.seedTypeToPlant == 71)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-whiteflower");
          else if (this.seedTypeToPlant == 98)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-beetroot");
          else if (this.seedTypeToPlant == 103)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-cauliflower");
        }
        this._siloSeeder = 0;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
    else if (this._cropID != 0)
    {
      if ((bool) (UnityEngine.Object) follower)
      {
        Structures_BerryBush structureById = StructureManager.GetStructureByID<Structures_BerryBush>(this._cropID);
        follower.FacePosition(structureById.Data.Position);
        string animation = "action";
        float timer = 10f;
        follower.TimedAnimation(animation, timer, (System.Action) (() => this.ProgressTask()));
      }
      else
        this.ProgressTask();
    }
    else if (this.targetFarmerStation)
    {
      if ((bool) (UnityEngine.Object) follower)
      {
        FarmStation farmStation = this.FindFarmStation();
        follower.FacePosition(farmStation.transform.position);
      }
      this.targetFarmerStation = false;
      if ((bool) (UnityEngine.Object) this._follower)
        this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
      if ((bool) (UnityEngine.Object) follower)
      {
        float timer = 1.5f;
        this.ClearDestination();
        this.SetState(FollowerTaskState.Idle);
        follower.TimedAnimation(this.GetDepositFoodAnim(), timer, (System.Action) (() =>
        {
          foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
            StructureManager.GetStructureByID<Structures_FarmerStation>(this._farmerStationID).DepositItem((InventoryItem.ITEM_TYPE) inventoryItem.type);
          this._brain._directInfoAccess.Inventory.Clear();
          this.ProgressTask();
        }));
      }
      else
      {
        foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
          StructureManager.GetStructureByID<Structures_FarmerStation>(this._farmerStationID).DepositItem((InventoryItem.ITEM_TYPE) inventoryItem.type);
        this._brain._directInfoAccess.Inventory.Clear();
        this.ProgressTask();
      }
    }
    else if (this._farmPlotID == 0)
    {
      if ((bool) (UnityEngine.Object) follower)
        follower.SetHat(HatType.Farm);
      this.ProgressTask();
    }
    else if ((bool) (UnityEngine.Object) follower)
    {
      FarmPlot farmPlot = this.FindFarmPlot();
      follower.FacePosition(farmPlot.transform.position);
      string animation = "Farming-water";
      float timer = 3.5f;
      if (farmPlot.StructureBrain.ReservedForSeeding)
      {
        timer = 5f;
        if (this.seedTypeToPlant == 8)
          animation = "Farming/add-seed";
        else if (this.seedTypeToPlant == 70)
          animation = "Farming/add-seed-mushroom";
        else if (this.seedTypeToPlant == 51)
          animation = "Farming/add-seed-pumpkin";
        else if (this.seedTypeToPlant == 72)
          animation = "Farming/add-seed-redflower";
        else if (this.seedTypeToPlant == 71)
          animation = "Farming/add-seed-whiteflower";
        else if (this.seedTypeToPlant == 98)
          animation = "Farming/add-seed-beetroot";
        else if (this.seedTypeToPlant == 103)
          animation = "Farming/add-seed-cauliflower";
      }
      else if (farmPlot.StructureBrain.ReservedForFertilizing)
      {
        animation = "Farming/add-fertiliser";
        timer = 4f;
      }
      follower.TimedAnimation(animation, timer, (System.Action) (() => this.ProgressTask()));
    }
    else
      this.ProgressTask();
  }

  public override void Cleanup(Follower follower)
  {
    follower.SetHat(HatType.None);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (this.holdingPoop)
      this.RefundPoop();
    base.Cleanup(follower);
  }

  private FarmStation FindFarmStation()
  {
    FarmStation farmStation1 = (FarmStation) null;
    foreach (FarmStation farmStation2 in FarmStation.FarmStations)
    {
      if ((UnityEngine.Object) farmStation2 != (UnityEngine.Object) null && farmStation2.StructureInfo.ID == this._farmerStationID)
      {
        farmStation1 = farmStation2;
        break;
      }
    }
    return farmStation1;
  }

  private FarmPlot FindFarmPlot()
  {
    FarmPlot farmPlot1 = (FarmPlot) null;
    foreach (FarmPlot farmPlot2 in FarmPlot.FarmPlots)
    {
      if ((UnityEngine.Object) farmPlot2 != (UnityEngine.Object) null && farmPlot2.StructureInfo.ID == this._farmPlotID)
      {
        farmPlot1 = farmPlot2;
        break;
      }
    }
    return farmPlot1;
  }
}
