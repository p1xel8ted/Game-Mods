// Decompiled with JetBrains decompiler
// Type: Structures_BuildSite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class Structures_BuildSite : StructureBrain, ITaskProvider
{
  public System.Action OnBuildProgressChanged;
  public System.Action OnBuildComplete;
  public int UsedSlotCount;

  public int AvailableSlotCount => this.TotalSlotCount - this.UsedSlotCount;

  public int TotalSlotCount => this.Data.Bounds.x * this.Data.Bounds.y;

  public bool IsObstructed
  {
    get
    {
      bool isObstructed = false;
      Structures_PlacementRegion placementRegion = this.FindPlacementRegion();
      PlacementRegion.TileGridTile tileGridTile1 = placementRegion.GetTileGridTile(this.Data.GridTilePosition);
      int num1 = -1;
      while (++num1 < this.Data.Bounds.x)
      {
        int num2 = -1;
        while (++num2 < this.Data.Bounds.y)
        {
          PlacementRegion.TileGridTile tileGridTile2 = placementRegion.GetTileGridTile(new Vector2Int(tileGridTile1.Position.x + num1, tileGridTile1.Position.y + num2));
          if (tileGridTile2 != null && tileGridTile2.ObjectOnTile != this.Data.Type && tileGridTile2.Obstructed)
            isObstructed = true;
        }
      }
      return isObstructed;
    }
  }

  public float BuildProgress
  {
    get => this.Data.Progress;
    set
    {
      this.Data.Progress = value;
      System.Action buildProgressChanged = this.OnBuildProgressChanged;
      if (buildProgressChanged != null)
        buildProgressChanged();
      if (!this.ProgressFinished)
        return;
      this.Build();
      System.Action onBuildComplete = this.OnBuildComplete;
      if (onBuildComplete == null)
        return;
      onBuildComplete();
    }
  }

  public bool ProgressFinished
  {
    get
    {
      return (double) this.BuildProgress >= (double) StructuresData.BuildDurationGameMinutes(this.Data.ToBuildType);
    }
  }

  public override void Init(StructuresData data)
  {
    base.Init(data);
    Structures_PlacementRegion placementRegion = this.FindPlacementRegion();
    if (placementRegion == null || !this.IsObstructed)
      return;
    placementRegion.MarkObstructionsForClearing(this.Data.GridTilePosition, this.Data.Bounds);
  }

  public void Build()
  {
    if (StructuresData.GetCategory(this.Data.ToBuildType) == StructureBrain.Categories.AESTHETIC)
    {
      if (!DataManager.Instance.HasBuiltDecoration(this.Data.ToBuildType))
      {
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FalseIdols))
          CultFaithManager.AddThought(Thought.Cult_FalseIdols_Trait);
        else
          CultFaithManager.AddThought(Thought.Cult_NewDecoration);
      }
      DataManager.Instance.SetBuiltDecoration(this.Data.ToBuildType);
    }
    else
    {
      if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.ConstructionEnthusiast))
        CultFaithManager.AddThought(Thought.Cult_ConstructionEnthusiast_Trait);
      else if (this.Data.ToBuildType != StructureBrain.TYPES.FARM_PLOT)
        CultFaithManager.AddThought(Thought.Cult_NewBuilding);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.Location == this.Data.Location && this.Data.ToBuildType != StructureBrain.TYPES.FARM_PLOT && this.Data.ToBuildType != StructureBrain.TYPES.PROPAGANDA_SPEAKER)
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.ConstructionEnthusiast))
            allBrain.AddThought(Thought.CultHasNewBuildingConstructionEnthusiast);
          else
            allBrain.AddThought(Thought.CultHasNewBuilding);
        }
      }
    }
    if (PlayerFarming.Location == FollowerLocation.Base)
      AudioManager.Instance.PlayOneShot(StructuresData.GetBuildSfx(this.Data.ToBuildType), this.Data.Position);
    StructuresData infoByType = StructuresData.GetInfoByType(this.Data.ToBuildType, 0);
    infoByType.Direction = this.Data.Direction;
    infoByType.Rotation = this.Data.Rotation;
    infoByType.Inventory = this.Data.Inventory;
    infoByType.QueuedResources = this.Data.QueuedResources;
    infoByType.QueuedRefineryVariants = this.Data.QueuedRefineryVariants;
    infoByType.GridTilePosition = this.Data.GridTilePosition;
    infoByType.PlacementRegionPosition = this.Data.PlacementRegionPosition;
    infoByType.FollowerID = this.Data.FollowerID;
    infoByType.MultipleFollowerIDs = this.Data.MultipleFollowerIDs;
    infoByType.ClaimedByPlayer = this.Data.ClaimedByPlayer;
    StructureManager.RemoveStructure((StructureBrain) this);
    StructureManager.BuildStructure(this.Data.Location, infoByType, this.Data.Position, this.Data.Bounds);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    FollowerTask overrideTask = (FollowerTask) null;
    if (!this.IsObstructed)
      overrideTask = (FollowerTask) new FollowerTask_Build(this.Data.ID);
    return overrideTask;
  }

  public bool CheckOverrideComplete() => this.ProgressFinished;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ProgressFinished || this.IsObstructed)
      return;
    for (int index = 0; index < this.AvailableSlotCount; ++index)
    {
      FollowerTask_Build followerTaskBuild = new FollowerTask_Build(this.Data.ID);
      tasks.Add(followerTaskBuild.Priorty, (FollowerTask) followerTaskBuild);
    }
  }

  public override void ToDebugString(StringBuilder sb)
  {
    base.ToDebugString(sb);
    sb.AppendLine($"ToBuild: {this.Data.ToBuildType}, Slots: {this.UsedSlotCount}/{this.TotalSlotCount}");
    if (!this.IsObstructed)
      return;
    Structures_PlacementRegion placementRegion = this.FindPlacementRegion();
    int num1 = -1;
    while (++num1 < this.Data.Bounds.x)
    {
      int num2 = -1;
      while (++num2 < this.Data.Bounds.y)
      {
        Vector2Int Position = new Vector2Int(this.Data.GridTilePosition.x + num1, this.Data.GridTilePosition.y + num2);
        PlacementRegion.TileGridTile tileGridTile = placementRegion.GetTileGridTile(Position);
        if (tileGridTile != null && tileGridTile.Obstructed)
        {
          StructuresData obstructionAtPosition = placementRegion.GetObstructionAtPosition(Position);
          sb.AppendLine($"Obstruction at ({num1},{num2}): {(obstructionAtPosition != null ? (object) $"{obstructionAtPosition.Type.ToString()} {obstructionAtPosition.Prioritised}" : (object) "(none!)")}");
        }
      }
    }
  }

  public void MarkObstructionsForClearing(
    Vector2Int GridPosition,
    Vector2Int Bounds,
    bool Prioritised)
  {
    Structures_PlacementRegion placementRegion = this.FindPlacementRegion();
    int num1 = -1;
    while (++num1 < Bounds.x)
    {
      int num2 = -1;
      while (++num2 < Bounds.y)
      {
        Vector2Int Position = new Vector2Int(GridPosition.x + num1, GridPosition.y + num2);
        PlacementRegion.TileGridTile tileGridTile = placementRegion.GetTileGridTile(Position);
        if (tileGridTile != null && tileGridTile.Obstructed)
        {
          if (placementRegion.GetObstructionAtPosition(Position) != null)
          {
            if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
            {
              Transform weedAtPosition = PlacementRegion.Instance.GetWeedAtPosition(tileGridTile.WorldPosition);
              if ((UnityEngine.Object) weedAtPosition != (UnityEngine.Object) null)
                UnityEngine.Object.Destroy((UnityEngine.Object) weedAtPosition.gameObject);
            }
          }
          else
            tileGridTile.Obstructed = false;
        }
      }
    }
  }
}
