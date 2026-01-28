// Decompiled with JetBrains decompiler
// Type: FollowerTask_Farm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Farm : FollowerTask
{
  public const float WATERING_DURATION_GAME_MINUTES = 10f;
  public const float SEEDING_DURATION_GAME_MINUTES = 1.95f;
  public const float FERTILIZING_DURATION_GAME_MINUTES = 4f;
  public int _farmerStationID;
  public Structures_FarmerStation _farmerStation;
  public int _farmPlotID;
  public int _siloFertiziler;
  public int _siloSeeder;
  public int _cropID;
  public int _previousSiloFertiziler;
  public int _previousSiloSeeder;
  public bool targetFarmerStation;
  public float _progress;
  public float _gameTimeSinceLastProgress;
  public int seedTypeToPlant = -1;
  public bool holdingPoop;
  public bool holdingSeeds;
  public List<InventoryItem> itemsBeingCarried = new List<InventoryItem>();
  public Follower _follower;
  public Structures_BerryBush currentCrop;
  public Structures_SiloSeed seedBucket;
  public Structures_SiloFertiliser fertiliserBucket;
  public bool busy;

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

  public override int GetSubTaskCode() => this._farmerStationID;

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
        structureById2.ReservedForWatering = false;
      else if (structureById2.ReservedForFertilizing)
        structureById2.ReservedForFertilizing = false;
    }
    else
    {
      Structures_FarmerPlot structureById3 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._cropID);
      if (structureById3 != null)
        structureById3.ReservedForTask = false;
    }
    Structures_SiloFertiliser structureById4 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    if (structureById4 != null)
      structureById4.ReservedForTask = false;
    Structures_SiloSeed structureById5 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    if (structureById5 != null)
      structureById5.ReservedForTask = false;
    if (this.seedBucket != null)
      this.seedBucket.ReservedForTask = false;
    if (this.fertiliserBucket == null)
      return;
    this.fertiliserBucket.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
    {
      float num = 1f;
      if (this._cropID != 0 && (double) this.currentCrop.Data.Progress < (double) this.currentCrop.Data.ProgressTarget && this.currentCrop != null && (this.currentCrop.BerryPicked || this.currentCrop.ReservedByPlayer))
      {
        this._cropID = 0;
        this._progress = 0.0f;
        this.currentCrop = (Structures_BerryBush) null;
        if ((bool) (UnityEngine.Object) this._follower)
          this._follower.State.CURRENT_STATE = StateMachine.State.Idle;
        this.ProgressTask();
      }
      this._gameTimeSinceLastProgress += deltaGameTime * num;
      this.ProgressTask();
    }
    else
    {
      if (this.State != FollowerTaskState.GoingTo)
        return;
      if (this._siloFertiziler != 0)
      {
        Structures_SiloFertiliser structureById = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
        if (structureById == null || structureById.ReservedByPlayer)
        {
          this.RefundPoop();
          this.End();
        }
      }
      if (this._siloSeeder == 0)
        return;
      Structures_SiloSeed structureById1 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
      if (structureById1 != null && !structureById1.ReservedByPlayer)
        return;
      this.RefundSeeds();
      this.End();
    }
  }

  public override void ProgressTask()
  {
    Structures_FarmerPlot plot = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
    if (this.fertiliserBucket != null || this.seedBucket != null || this.busy)
    {
      if ((double) this._gameTimeSinceLastProgress <= 1.0)
        return;
      if (this.seedBucket != null)
        this.seedBucket = (Structures_SiloSeed) null;
      if (this.fertiliserBucket == null)
        return;
      this.fertiliserBucket = (Structures_SiloFertiliser) null;
    }
    else if (plot == null)
    {
      this.currentCrop = StructureManager.GetStructureByID<Structures_BerryBush>(this._cropID);
      if (this.currentCrop != null && !this.currentCrop.ReservedByPlayer)
      {
        List<InventoryItem.ITEM_TYPE> berries = this.currentCrop.GetBerries();
        this.currentCrop.PickBerries(this._gameTimeSinceLastProgress, false);
        this._gameTimeSinceLastProgress = 0.0f;
        if ((double) this.currentCrop.Data.Progress < (double) this.currentCrop.Data.ProgressTarget)
          return;
        if (!this.currentCrop.Data.Withered)
        {
          foreach (InventoryItem.ITEM_TYPE Type in berries)
          {
            if (this._brain != null && this._brain._directInfoAccess != null && this._brain._directInfoAccess.Inventory != null)
              this._brain._directInfoAccess.Inventory.Add(new InventoryItem(Type, 1));
          }
        }
        else
          berries.Clear();
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
        if (!this.targetFarmerStation && (UnityEngine.Object) this._follower != (UnityEngine.Object) null && this._follower.State.CURRENT_STATE == StateMachine.State.TimedAction)
          this._follower.State.CURRENT_STATE = StateMachine.State.Idle;
        this.UpdatePlot();
        this.Loop();
      }
      else
      {
        if (this.currentCrop != null && this.currentCrop.ReservedByPlayer)
          this.currentCrop.ReservedForTask = false;
        this.Loop();
      }
    }
    else if (plot.ReservedForSeeding)
    {
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 1.9500000476837158)
        return;
      this._progress = 0.0f;
      if (plot.CanPlantSeed())
      {
        plot.Data.Inventory.Add(new InventoryItem((InventoryItem.ITEM_TYPE) this.seedTypeToPlant, 1));
        plot.PlantSeed((InventoryItem.ITEM_TYPE) this.seedTypeToPlant);
      }
      else
        this.RefundSeed();
      this.seedTypeToPlant = -1;
      plot.ReservedForSeeding = false;
      this._brain.GetXP(1f);
      this.UpdatePlot();
      this.Loop();
    }
    else if (plot.ReservedForWatering)
    {
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 10.0)
        return;
      this._progress = 0.0f;
      plot.Data.Watered = true;
      plot.Data.WateredCount = 0;
      plot.ReservedForWatering = false;
      this._brain.GetXP(1f);
      this.UpdatePlot();
      this.Loop();
    }
    else if (plot.ReservedForFertilizing)
    {
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 4.0)
        return;
      this._progress = 0.0f;
      if (plot.CanFertilize())
      {
        InventoryItem.ITEM_TYPE itemType = this.itemsBeingCarried.Count > 0 ? (InventoryItem.ITEM_TYPE) this.itemsBeingCarried[0].type : InventoryItem.ITEM_TYPE.POOP;
        plot.Data.Inventory.Add(new InventoryItem(itemType, 1));
        plot.AddFertilizer(itemType);
        if (itemType == InventoryItem.ITEM_TYPE.POOP_RAINBOW)
        {
          if (PlayerFarming.Location == FollowerLocation.Base)
            GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => plot.ForceFullyGrown()));
          else
            plot.ForceFullyGrown();
        }
      }
      else
        this.RefundPoop();
      this.holdingPoop = false;
      plot.ReservedForFertilizing = false;
      this._brain.GetXP(1f);
      this.UpdatePlot();
      this.Loop();
    }
    else
      this.Loop();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (this.holdingPoop)
      this.RefundPoop();
    if (this.holdingSeeds)
      this.RefundSeeds();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
      followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    Structures_SiloFertiliser structureById1 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    if (structureById2 != null)
      structureById2.ReservedForTask = false;
    if (this.seedBucket != null)
      this.seedBucket.ReservedForTask = false;
    if (this.fertiliserBucket == null)
      return;
    this.fertiliserBucket.ReservedForTask = false;
  }

  public override void OnComplete()
  {
    base.OnComplete();
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (this.holdingPoop)
      this.RefundPoop();
    if (this.holdingSeeds)
      this.RefundSeeds();
    Structures_SiloFertiliser structureById1 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    if (structureById2 != null)
      structureById2.ReservedForTask = false;
    if (this.seedBucket != null)
      this.seedBucket.ReservedForTask = false;
    if (this.fertiliserBucket == null)
      return;
    this.fertiliserBucket.ReservedForTask = false;
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (this.holdingPoop)
      this.RefundPoop();
    if (this.holdingSeeds)
      this.RefundSeeds();
    Structures_SiloFertiliser structureById1 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    if (structureById2 == null)
      return;
    structureById2.ReservedForTask = false;
  }

  public void RefundSeed()
  {
    Structures_SiloSeed structureById = StructureManager.GetStructureByID<Structures_SiloSeed>(this._previousSiloSeeder);
    if (structureById == null)
      return;
    structureById.Data.Inventory.Add(new InventoryItem((InventoryItem.ITEM_TYPE) this.seedTypeToPlant, 1));
    this.itemsBeingCarried.Clear();
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

  public void RefundSeeds()
  {
    Structures_SiloSeed structureById = StructureManager.GetStructureByID<Structures_SiloSeed>(this._previousSiloSeeder);
    if (structureById == null)
      return;
    foreach (InventoryItem inventoryItem in this.itemsBeingCarried)
      structureById.Data.Inventory.Add(inventoryItem);
    this.itemsBeingCarried.Clear();
    foreach (Interaction_SiloSeeder siloSeeder in Interaction_SiloSeeder.SiloSeeders)
    {
      if (siloSeeder.StructureBrain.Data.ID == this._previousSiloSeeder)
      {
        siloSeeder.UpdateCapacityIndicators();
        break;
      }
    }
    this.holdingSeeds = false;
  }

  public void RefundPoop()
  {
    Structures_SiloFertiliser structureById = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._previousSiloFertiziler);
    if (structureById == null)
      return;
    foreach (InventoryItem inventoryItem in this.itemsBeingCarried)
      structureById.Data.Inventory.Add(inventoryItem);
    this.itemsBeingCarried.Clear();
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

  public void UpdatePlot()
  {
    FarmPlot farmPlot = this.FindFarmPlot();
    if (!((UnityEngine.Object) farmPlot != (UnityEngine.Object) null))
      return;
    farmPlot.UpdateCropImage();
  }

  public void Loop()
  {
    if (this.targetFarmerStation)
    {
      this.ClearDestination();
      this._farmPlotID = 0;
      Structures_SiloFertiliser structureById1 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
      if (structureById1 != null)
        structureById1.ReservedForTask = false;
      Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
      if (structureById2 != null)
        structureById2.ReservedForTask = false;
      this._siloFertiziler = 0;
      this._siloSeeder = 0;
      this.SetState(FollowerTaskState.GoingTo);
      if (!(bool) (UnityEngine.Object) this._follower)
        return;
      this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetMovementAnim());
    }
    else
    {
      if ((UnityEngine.Object) this._follower != (UnityEngine.Object) null && this._follower.State.CURRENT_STATE == StateMachine.State.TimedAction)
        return;
      Structures_SiloFertiliser emptyFertiliserSilo = this._farmerStation.GetEmptyFertiliserSilo();
      List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.POOP_BUCKET);
      if (emptyFertiliserSilo != null && structuresOfType1.Count > 0 && structuresOfType1[0] is Structures_SiloFertiliser structuresSiloFertiliser1 && structuresSiloFertiliser1.GetCompostCount() > 0 && !emptyFertiliserSilo.ReservedForTask)
      {
        this.ClearDestination();
        this._farmPlotID = 0;
        this._cropID = 0;
        this.fertiliserBucket = structuresSiloFertiliser1;
        this.SetState(FollowerTaskState.GoingTo);
      }
      else
      {
        Structures_SiloSeed emptySeedSilo = this._farmerStation.GetEmptySeedSilo();
        List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SEED_BUCKET);
        if (emptySeedSilo != null && structuresOfType2.Count > 0 && structuresOfType2[0] is Structures_SiloSeed structuresSiloSeed1 && structuresSiloSeed1.GetCompostCount() > 0 && !emptySeedSilo.ReservedForTask)
        {
          this.ClearDestination();
          this._farmPlotID = 0;
          this._cropID = 0;
          this.seedBucket = structuresSiloSeed1;
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
        {
          Structures_BerryBush nextUnpickedPlot = this._farmerStation.GetNextUnpickedPlot();
          if (nextUnpickedPlot == null || this._farmerStation.Data.Type != StructureBrain.TYPES.FARM_STATION_II)
          {
            Structures_FarmerPlot nextUnseededPlot = this._farmerStation.GetNextUnseededPlot();
            if (nextUnseededPlot == null)
            {
              Structures_FarmerPlot nextUnwateredPlot = this._farmerStation.GetNextUnwateredPlot();
              if (nextUnwateredPlot == null)
              {
                Structures_FarmerPlot unfertilizedPlot = this._farmerStation.GetNextUnfertilizedPlot();
                if (unfertilizedPlot == null || Structures_SiloFertiliser.GetClosestFertiliser(this._brain.LastPosition, this._brain.Location) == null)
                {
                  Structures_SiloFertiliser unfilledFertiliserSilo = this._farmerStation.GetUnfilledFertiliserSilo();
                  if (unfilledFertiliserSilo != null && structuresOfType1.Count > 0 && structuresOfType1[0] is Structures_SiloFertiliser structuresSiloFertiliser && structuresSiloFertiliser.GetCompostCount() > 0 && !unfilledFertiliserSilo.ReservedForTask)
                  {
                    this.ClearDestination();
                    this._farmPlotID = 0;
                    this._cropID = 0;
                    this.fertiliserBucket = structuresSiloFertiliser;
                    this.SetState(FollowerTaskState.GoingTo);
                  }
                  else
                  {
                    Structures_SiloSeed unfilledSeedSilo = this._farmerStation.GetUnfilledSeedSilo();
                    List<StructureBrain> structuresOfType3 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SEED_BUCKET);
                    if (unfilledSeedSilo != null && structuresOfType3.Count > 0 && structuresOfType3[0] is Structures_SiloSeed structuresSiloSeed && structuresSiloSeed.GetCompostCount() > 0 && !unfilledSeedSilo.ReservedForTask)
                    {
                      this.ClearDestination();
                      this._farmPlotID = 0;
                      this._cropID = 0;
                      this.seedBucket = structuresSiloSeed;
                      this.SetState(FollowerTaskState.GoingTo);
                    }
                    else
                      this.End();
                  }
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
    }
  }

  public override void OnEnd()
  {
    base.OnEnd();
    Structures_SiloFertiliser structureById1 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    if (structureById2 != null)
      structureById2.ReservedForTask = false;
    if (this.seedBucket != null)
      this.seedBucket.ReservedForTask = false;
    if (this.fertiliserBucket == null)
      return;
    this.fertiliserBucket.ReservedForTask = false;
  }

  public void NextTarget(Structures_FarmerPlot nextPlot)
  {
    if (nextPlot.ReservedForSeeding)
    {
      InventoryItem.ITEM_TYPE prioritisedSeedType = InventoryItem.ITEM_TYPE.NONE;
      Structures_FarmerPlot structureById = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
      if (structureById != null)
        prioritisedSeedType = structureById.GetPrioritisedSeedType();
      Structures_SiloSeed closestSeeder = Structures_SiloSeed.GetClosestSeeder(this._brain.LastPosition, this._brain.Location, prioritisedSeedType);
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

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3 = new Vector3();
    Structures_FarmerPlot structureById1 = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    Structures_SiloFertiliser structureById3 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    Structures_BerryBush structureById4 = StructureManager.GetStructureByID<Structures_BerryBush>(this._cropID);
    if (this.fertiliserBucket != null)
      vector3 = this.fertiliserBucket.Data.Position;
    else if (this.seedBucket != null)
      vector3 = this.seedBucket.Data.Position;
    else if (structureById2 != null)
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
      follower.SetHat(FollowerHatType.Farm);
    this._follower = follower;
  }

  public string GetMovementAnim()
  {
    foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
    {
      switch ((InventoryItem.ITEM_TYPE) inventoryItem.type)
      {
        case InventoryItem.ITEM_TYPE.BERRY:
          return "Farming/run-berries";
        case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
          return "Farming/run-mushroom";
        case InventoryItem.ITEM_TYPE.GRASS:
          return "Farming/run-grass";
        case InventoryItem.ITEM_TYPE.PUMPKIN:
          return "Farming/run-pumpkin";
        case InventoryItem.ITEM_TYPE.FLOWER_RED:
          return "Farming/run-redflower";
        case InventoryItem.ITEM_TYPE.BEETROOT:
          return "Farming/run-beetroot";
        case InventoryItem.ITEM_TYPE.CAULIFLOWER:
          return "Farming/run-cauliflower";
        case InventoryItem.ITEM_TYPE.COTTON:
          return "Farming/run-cotton";
        case InventoryItem.ITEM_TYPE.HOPS:
          return "Farming/run-hops";
        case InventoryItem.ITEM_TYPE.GRAPES:
          return "Farming/run-grapes";
        case InventoryItem.ITEM_TYPE.SNOW_FRUIT:
          return "Farming/run-snow-fruit";
        case InventoryItem.ITEM_TYPE.CHILLI:
          return "Farming/run-chilli";
        default:
          continue;
      }
    }
    return "Farming/run-berries";
  }

  public string GetDepositFoodAnim()
  {
    foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
    {
      switch ((InventoryItem.ITEM_TYPE) inventoryItem.type)
      {
        case InventoryItem.ITEM_TYPE.BERRY:
          return "Farming/add-berries";
        case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
          return "Farming/add-mushroom";
        case InventoryItem.ITEM_TYPE.GRASS:
          return "Farming/add-grass";
        case InventoryItem.ITEM_TYPE.PUMPKIN:
          return "Farming/add-pumpkin";
        case InventoryItem.ITEM_TYPE.FLOWER_RED:
          return "Farming/add-redflower";
        case InventoryItem.ITEM_TYPE.BEETROOT:
          return "Farming/add-beetroot";
        case InventoryItem.ITEM_TYPE.CAULIFLOWER:
          return "Farming/add-cauliflower";
        case InventoryItem.ITEM_TYPE.COTTON:
          return "Farming/add-cotton";
        case InventoryItem.ITEM_TYPE.HOPS:
          return "Farming/add-hops";
        case InventoryItem.ITEM_TYPE.GRAPES:
          return "Farming/add-grapes";
        case InventoryItem.ITEM_TYPE.SNOW_FRUIT:
          return "Farming/add-snow-fruit";
        case InventoryItem.ITEM_TYPE.CHILLI:
          return "Farming/add-chilli";
        default:
          continue;
      }
    }
    return "Farming/add-berries";
  }

  public string GetFertiliserAnim(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.POOP_GOLD:
        return "Farming/add-fertiliser-gold";
      case InventoryItem.ITEM_TYPE.POOP_RAINBOW:
        return "Farming/add-fertiliser-rainbow";
      case InventoryItem.ITEM_TYPE.POOP_GLOW:
        return "Farming/add-fertiliser-glow";
      case InventoryItem.ITEM_TYPE.POOP_DEVOTION:
        return "Farming/add-fertiliser-devotion";
      case InventoryItem.ITEM_TYPE.POOP_ROTSTONE:
        return "Farming/add-fertiliser-rotstone";
      default:
        return "Farming/add-fertiliser";
    }
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

  public void DoingBegin(Follower follower)
  {
    if ((bool) (UnityEngine.Object) follower)
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    Structures_SiloFertiliser unfilledFertiliserSilo = this._farmerStation.GetUnfilledFertiliserSilo();
    Structures_SiloSeed unfilledSeedSilo = this._farmerStation.GetUnfilledSeedSilo();
    if (this.fertiliserBucket != null)
    {
      if (unfilledFertiliserSilo == null || this.fertiliserBucket.GetCompostCount() <= 0)
      {
        this.fertiliserBucket = (Structures_SiloFertiliser) null;
        this.Loop();
      }
      else
      {
        unfilledFertiliserSilo.ReservedForTask = true;
        this.holdingPoop = true;
        this._previousSiloFertiziler = this.fertiliserBucket.Data.ID;
        List<InventoryItem> inventoryItemList = this.fertiliserBucket.RemoveCompostAmount(Mathf.Clamp((int) unfilledFertiliserSilo.Capacity - unfilledFertiliserSilo.GetCompostCount(), 0, (int) unfilledFertiliserSilo.Capacity));
        for (int index = 0; index < inventoryItemList.Count; ++index)
          this.itemsBeingCarried.Add(inventoryItemList[index]);
        int type = this.itemsBeingCarried[0].type;
        foreach (Interaction_SiloFertilizer siloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
          siloFertilizer.UpdateCapacityIndicators();
        this.fertiliserBucket.ReservedForTask = false;
        this.fertiliserBucket = (Structures_SiloFertiliser) null;
        this._siloFertiziler = unfilledFertiliserSilo.Data.ID;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        if (!(bool) (UnityEngine.Object) follower)
          return;
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetRunPoopAnim((InventoryItem.ITEM_TYPE) type));
      }
    }
    else if (this.seedBucket != null && unfilledSeedSilo != null)
    {
      if (unfilledSeedSilo == null || this.seedBucket.GetCompostCount() <= 0)
      {
        this.seedBucket = (Structures_SiloSeed) null;
        this.Loop();
      }
      else
      {
        unfilledSeedSilo.ReservedForTask = true;
        this.holdingSeeds = true;
        this._previousSiloSeeder = this.seedBucket.Data.ID;
        List<InventoryItem.ITEM_TYPE> compost = this.seedBucket.GetCompost((int) Mathf.Min(unfilledSeedSilo.Capacity, (float) this.seedBucket.GetCompostCount()));
        for (int index = 0; index < compost.Count; ++index)
          this.itemsBeingCarried.Add(new InventoryItem(compost[index], 1));
        this.seedBucket.RemoveCompost(compost);
        foreach (Interaction_SiloSeeder siloSeeder in Interaction_SiloSeeder.SiloSeeders)
          siloSeeder.UpdateCapacityIndicators();
        this.seedBucket.ReservedForTask = false;
        this.seedBucket = (Structures_SiloSeed) null;
        this._siloSeeder = unfilledSeedSilo.Data.ID;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        if (!(bool) (UnityEngine.Object) follower)
          return;
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed");
      }
    }
    else if (this._siloFertiziler != 0)
    {
      Structures_SiloFertiliser siloFertiliser = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
      if (siloFertiliser != null)
      {
        if (this.holdingPoop)
        {
          this.holdingPoop = false;
          InventoryItem.ITEM_TYPE type = this.itemsBeingCarried.Count > 0 ? (InventoryItem.ITEM_TYPE) this.itemsBeingCarried[0].type : InventoryItem.ITEM_TYPE.POOP;
          Structures_SiloFertiliser structureById = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._previousSiloFertiziler);
          foreach (InventoryItem inventoryItem in this.itemsBeingCarried)
          {
            if ((double) siloFertiliser.GetCompostCount() < (double) siloFertiliser.Capacity)
              siloFertiliser.Data.Inventory.Add(inventoryItem);
            else
              structureById.Data.Inventory.Add(inventoryItem);
          }
          this.itemsBeingCarried.Clear();
          this.busy = true;
          if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
          {
            follower.TimedAnimation(this.GetFertiliserAnim(type), 2f, (System.Action) (() =>
            {
              foreach (Interaction_SiloFertilizer siloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
              {
                if (siloFertilizer.StructureBrain.Data.ID == this._siloFertiziler)
                  siloFertilizer.UpdateCapacityIndicators();
              }
              this.busy = false;
              siloFertiliser.ReservedForTask = false;
              this._siloFertiziler = 0;
              this.ClearDestination();
              this.Loop();
            }));
          }
          else
          {
            this.busy = false;
            siloFertiliser.ReservedForTask = false;
            this._siloFertiziler = 0;
            this.ClearDestination();
            this.Loop();
          }
        }
        else if (siloFertiliser.Data.Inventory.Count <= 0)
        {
          this.End();
        }
        else
        {
          InventoryItem.ITEM_TYPE itemType = InventoryItem.ITEM_TYPE.NONE;
          if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
          {
            for (int index = siloFertiliser.Data.Inventory.Count - 1; index >= 0; --index)
            {
              if (siloFertiliser.Data.Inventory[index].type == 187)
              {
                itemType = (InventoryItem.ITEM_TYPE) siloFertiliser.Data.Inventory[index].type;
                break;
              }
            }
            if (itemType == InventoryItem.ITEM_TYPE.NONE)
            {
              this.End();
              return;
            }
          }
          else
            itemType = (InventoryItem.ITEM_TYPE) siloFertiliser.Data.Inventory[0].type;
          this.holdingPoop = true;
          this.itemsBeingCarried.Clear();
          this.itemsBeingCarried.Add(new InventoryItem(itemType, 1));
          siloFertiliser.RemoveItems(itemType, 1);
          if ((bool) (UnityEngine.Object) follower)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetRunPoopAnim(itemType));
          foreach (Interaction_SiloFertilizer siloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
          {
            if (siloFertilizer.StructureBrain.Data.ID == this._siloFertiziler)
              siloFertilizer.UpdateCapacityIndicators();
          }
          siloFertiliser.ReservedForTask = false;
          this._siloFertiziler = 0;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      else
        this.End();
    }
    else if (this._siloSeeder != 0)
    {
      Structures_SiloSeed siloSeeder = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
      if (siloSeeder != null)
      {
        if (this.holdingSeeds)
        {
          this.holdingSeeds = false;
          Structures_SiloSeed structureById = StructureManager.GetStructureByID<Structures_SiloSeed>(this._previousSiloSeeder);
          foreach (InventoryItem inventoryItem in this.itemsBeingCarried)
          {
            if ((double) siloSeeder.GetCompostCount() < (double) siloSeeder.Capacity)
              siloSeeder.Data.Inventory.Add(inventoryItem);
            else
              structureById.Data.Inventory.Add(inventoryItem);
          }
          this.itemsBeingCarried.Clear();
          this.busy = true;
          if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
          {
            follower.TimedAnimation("Farming/add-seed", 2f, (System.Action) (() =>
            {
              foreach (Interaction_SiloSeeder siloSeeder1 in Interaction_SiloSeeder.SiloSeeders)
              {
                if (siloSeeder1.StructureBrain.Data.ID == this._siloSeeder)
                  siloSeeder1.UpdateCapacityIndicators();
              }
              this.busy = false;
              siloSeeder.ReservedForTask = false;
              this._siloSeeder = 0;
              this.ClearDestination();
              this.Loop();
            }));
          }
          else
          {
            this.busy = false;
            siloSeeder.ReservedForTask = false;
            this._siloSeeder = 0;
            this.ClearDestination();
            this.Loop();
          }
        }
        else if (siloSeeder == null || siloSeeder.Data.Inventory.Count <= 0)
        {
          this.End();
        }
        else
        {
          InventoryItem.ITEM_TYPE itemType = InventoryItem.ITEM_TYPE.NONE;
          Structures_FarmerPlot structureById = StructureManager.GetStructureByID<Structures_FarmerPlot>(this._farmPlotID);
          if (structureById != null)
            itemType = structureById.GetPrioritisedSeedType();
          InventoryItem inventoryItem = siloSeeder.Data.Inventory[0];
          for (int index = siloSeeder.Data.Inventory.Count - 1; index >= 0; --index)
          {
            if ((InventoryItem.ITEM_TYPE) siloSeeder.Data.Inventory[index].type == itemType && siloSeeder.Data.Inventory[index].quantity > 0)
              inventoryItem = siloSeeder.Data.Inventory[index];
          }
          --inventoryItem.quantity;
          this.seedTypeToPlant = inventoryItem.type;
          if (inventoryItem.quantity <= 0)
            siloSeeder.Data.Inventory.Remove(inventoryItem);
          foreach (Interaction_SiloSeeder siloSeeder2 in Interaction_SiloSeeder.SiloSeeders)
          {
            if (siloSeeder2.StructureBrain.Data.ID == this._siloSeeder)
            {
              siloSeeder2.UpdateCapacityIndicators();
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
            else if (this.seedTypeToPlant == 140)
              follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-cotton");
            else if (this.seedTypeToPlant == 152)
              follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-hops");
            else if (this.seedTypeToPlant == 153)
              follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-grapes");
            else if (this.seedTypeToPlant == 166)
              follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-snow-fruit");
            else if (this.seedTypeToPlant == 169)
              follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-seed-chilli");
            else if (this.seedTypeToPlant == 35)
              follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-grass");
          }
          siloSeeder.ReservedForTask = false;
          this._siloSeeder = 0;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      else
        this.End();
    }
    else if (this._cropID != 0)
    {
      if ((bool) (UnityEngine.Object) follower)
      {
        Structures_BerryBush structureById = StructureManager.GetStructureByID<Structures_BerryBush>(this._cropID);
        follower.FacePosition(structureById.Data.Position);
        string animation = "action";
        float progressTarget = structureById.Data.ProgressTarget;
        follower.TimedAnimation(animation, progressTarget, (System.Action) (() => this.ProgressTask()));
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
        follower.SetHat(FollowerHatType.Farm);
      this.ProgressTask();
    }
    else if ((bool) (UnityEngine.Object) follower)
    {
      FarmPlot farmPlot = this.FindFarmPlot();
      if ((UnityEngine.Object) farmPlot != (UnityEngine.Object) null)
      {
        follower.FacePosition(farmPlot.transform.position);
        string animation = "Farming-water";
        float timer = 3.5f;
        if (farmPlot.StructureBrain.ReservedForSeeding)
        {
          timer = 1.95f;
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
          else if (this.seedTypeToPlant == 140)
            animation = "Farming/add-seed-cotton";
          else if (this.seedTypeToPlant == 152)
            animation = "Farming/add-seed-hops";
          else if (this.seedTypeToPlant == 166)
            animation = "Farming/add-seed-snow-fruit";
          else if (this.seedTypeToPlant == 169)
            animation = "Farming/add-seed-chilli";
          else if (this.seedTypeToPlant == 153)
            animation = "Farming/add-seed-grapes";
          else if (this.seedTypeToPlant == 35)
            animation = "Farming/add-seed-grass";
        }
        else if (farmPlot.StructureBrain.ReservedForFertilizing)
        {
          animation = this.GetFertiliserAnim(this.itemsBeingCarried.Count > 0 ? (InventoryItem.ITEM_TYPE) this.itemsBeingCarried[0].type : InventoryItem.ITEM_TYPE.POOP);
          timer = 4f;
        }
        follower.TimedAnimation(animation, timer, (System.Action) (() => this.ProgressTask()));
      }
      else
        this.ProgressTask();
    }
    else
      this.ProgressTask();
  }

  public string GetRunPoopAnim(InventoryItem.ITEM_TYPE poopType)
  {
    switch (poopType)
    {
      case InventoryItem.ITEM_TYPE.POOP_GOLD:
        return "Farming/run-poop-gold";
      case InventoryItem.ITEM_TYPE.POOP_RAINBOW:
        return "Farming/run-poop-rainbow";
      case InventoryItem.ITEM_TYPE.POOP_GLOW:
        return "Farming/run-poop-glow";
      case InventoryItem.ITEM_TYPE.POOP_DEVOTION:
        return "Farming/run-poop-devotion";
      case InventoryItem.ITEM_TYPE.POOP_ROTSTONE:
        return "Farming/run-poop-rotstone";
      default:
        return "Farming/run-poop";
    }
  }

  public override void Cleanup(Follower follower)
  {
    follower.SetHat(FollowerHatType.None);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (this.seedTypeToPlant != -1)
      this.RefundSeed();
    if (this.holdingPoop)
      this.RefundPoop();
    if (this.holdingSeeds)
      this.RefundSeeds();
    Structures_SiloFertiliser structureById1 = StructureManager.GetStructureByID<Structures_SiloFertiliser>(this._siloFertiziler);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    Structures_SiloSeed structureById2 = StructureManager.GetStructureByID<Structures_SiloSeed>(this._siloSeeder);
    if (structureById2 != null)
      structureById2.ReservedForTask = false;
    base.Cleanup(follower);
  }

  public FarmStation FindFarmStation()
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

  public FarmPlot FindFarmPlot()
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

  [CompilerGenerated]
  public void \u003CDoingBegin\u003Eb__59_2() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003CDoingBegin\u003Eb__59_3()
  {
    foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
      StructureManager.GetStructureByID<Structures_FarmerStation>(this._farmerStationID).DepositItem((InventoryItem.ITEM_TYPE) inventoryItem.type);
    this._brain._directInfoAccess.Inventory.Clear();
    this.ProgressTask();
  }

  [CompilerGenerated]
  public void \u003CDoingBegin\u003Eb__59_4() => this.ProgressTask();
}
