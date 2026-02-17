// Decompiled with JetBrains decompiler
// Type: Structures_BuildSiteProject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class Structures_BuildSiteProject : StructureBrain, ITaskProvider
{
  public System.Action OnBuildProgressChanged;
  public System.Action OnBuildComplete;
  public int UsedSlotCount;

  public int AvailableSlotCount => this.TotalSlotCount - this.UsedSlotCount;

  public int TotalSlotCount => this.Data.Bounds.x * this.Data.Bounds.y;

  public float BuildProgress
  {
    get => this.Data.Progress;
    set
    {
      if (this.ProgressFinished)
        return;
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
    this.FindPlacementRegion()?.MarkObstructionsForClearing(this.Data.GridTilePosition, this.Data.Bounds);
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
    AudioManager.Instance.PlayOneShot(StructuresData.GetBuildSfx(this.Data.ToBuildType), this.Data.Position);
    StructuresData infoByType = StructuresData.GetInfoByType(this.Data.ToBuildType, 0);
    infoByType.Rotation = this.Data.Rotation;
    infoByType.Direction = this.Data.Direction;
    infoByType.GridTilePosition = this.Data.GridTilePosition;
    infoByType.PlacementRegionPosition = this.Data.PlacementRegionPosition;
    infoByType.FollowerID = this.Data.FollowerID;
    StructureManager.RemoveStructure((StructureBrain) this);
    StructureManager.BuildStructure(this.Data.Location, infoByType, this.Data.Position, this.Data.Bounds);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    return (FollowerTask) new FollowerTask_BuildProject(this.Data.ID);
  }

  public bool CheckOverrideComplete() => this.ProgressFinished;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ProgressFinished)
      return;
    for (int index = 0; index < this.AvailableSlotCount; ++index)
    {
      FollowerTask_BuildProject taskBuildProject = new FollowerTask_BuildProject(this.Data.ID);
      tasks.Add(taskBuildProject.Priorty, (FollowerTask) taskBuildProject);
    }
  }

  public override void ToDebugString(StringBuilder sb)
  {
    base.ToDebugString(sb);
    sb.AppendLine($"ToBuild: {this.Data.ToBuildType}, Slots: {this.UsedSlotCount}/{this.TotalSlotCount}");
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
          StructuresData obstructionAtPosition = placementRegion.GetObstructionAtPosition(Position);
          if (obstructionAtPosition != null)
            obstructionAtPosition.Prioritised = true;
          else
            tileGridTile.Obstructed = false;
        }
      }
    }
  }
}
