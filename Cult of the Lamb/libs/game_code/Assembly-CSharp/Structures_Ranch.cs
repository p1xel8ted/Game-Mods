// Decompiled with JetBrains decompiler
// Type: Structures_Ranch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Structures_Ranch : StructureBrain, ITaskProvider
{
  public const float TilesPerAnimal = 3f;
  public List<Structures_Ranch.FenceData> Fences = new List<Structures_Ranch.FenceData>();
  public List<PlacementRegion.TileGridTile> RanchingTiles = new List<PlacementRegion.TileGridTile>();
  public List<PlacementRegion.TileGridTile> touchingRanchTiles = new List<PlacementRegion.TileGridTile>();
  public const float ANIMAL_EAT_THRESHOLD = 50f;
  public const float ANIMAL_STARVATION_THRESHOLD = 25f;
  public const float HUNGER_SPEED = 2f;
  public const float INJURED_SPEED = 1f;
  public const float FERAL_CALMING_SPEED = 1f;
  public const int INJURED_HEALING_COST = 3;
  public const float MAX_INJURED = 15f;
  public const float MAX_FERAL = 10f;
  public const float TIMES_TO_CALM_FERAL = 2f;

  public int Capacity => Mathf.CeilToInt((float) this.RanchingTiles.Count / 3f);

  public event Structures_Ranch.AnimalEvent OnAnimalAdded;

  public bool IsOvercrowded => this.Data.Animals.Count > this.Capacity;

  public bool HasValidEnclosure()
  {
    this.touchingRanchTiles = this.GetSurroundingRanchTiles();
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
    for (int index = 0; index < this.touchingRanchTiles.Count; ++index)
    {
      if (this.touchingRanchTiles[index].ObjectOnTile == StructureBrain.TYPES.RANCH_FENCE && !this.touchingRanchTiles[index].Collapsed && !tileGridTileList.Contains(this.touchingRanchTiles[index]))
        tileGridTileList.Add(this.touchingRanchTiles[index]);
    }
    if (tileGridTileList.Count < 2)
      return false;
    for (int index = 0; index < tileGridTileList.Count; ++index)
    {
      List<PlacementRegion.TileGridTile> wallTiles;
      int num = this.AreWallsConnecting(tileGridTileList[index], out wallTiles) ? 1 : 0;
      List<Structures_RanchFence> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_RanchFence>();
      if (num != 0)
      {
        this.Fences.Clear();
        foreach (PlacementRegion.TileGridTile tileGridTile in wallTiles)
        {
          foreach (Structures_RanchFence structuresRanchFence in structuresOfType)
          {
            if (structuresRanchFence.Data.ID == tileGridTile.ObjectID)
            {
              Structures_Ranch.FenceData fenceData = new Structures_Ranch.FenceData()
              {
                Fence = structuresRanchFence,
                Tile = tileGridTile
              };
              if (!fenceData.Fence.ConnectedRanchIDs.Contains(this.Data.ID))
                fenceData.Fence.ConnectedRanchIDs.Add(this.Data.ID);
              this.Fences.Add(fenceData);
              break;
            }
          }
        }
        this.RanchingTiles.Clear();
        this.RanchingTiles = this.GetRanchingTiles();
        if (this.RanchingTiles.Count > 0)
          return true;
      }
      else
      {
        foreach (Structures_Ranch.FenceData fence in this.Fences)
        {
          if (fence.Fence.ConnectedRanchIDs.Contains(this.Data.ID))
            fence.Fence.ConnectedRanchIDs.Remove(this.Data.ID);
        }
        this.RanchingTiles.Clear();
      }
    }
    return false;
  }

  public bool AreWallsConnecting(
    PlacementRegion.TileGridTile fromFenceTile,
    out List<PlacementRegion.TileGridTile> wallTiles,
    List<PlacementRegion.TileGridTile> checkedTiles = null)
  {
    wallTiles = new List<PlacementRegion.TileGridTile>();
    List<PlacementRegion.TileGridTile> surroundingTiles = this.GetSurroundingTiles(fromFenceTile);
    if (checkedTiles == null)
    {
      checkedTiles = new List<PlacementRegion.TileGridTile>();
      checkedTiles.Add(fromFenceTile);
      wallTiles.Add(fromFenceTile);
    }
    bool flag1 = false;
    for (int index = 0; index < surroundingTiles.Count; ++index)
    {
      PlacementRegion.TileGridTile fromFenceTile1 = surroundingTiles[index];
      if (fromFenceTile1 != fromFenceTile)
      {
        bool flag2 = fromFenceTile1.ObjectOnTile == StructureBrain.TYPES.RANCH_FENCE && !fromFenceTile1.Collapsed;
        bool flag3 = !checkedTiles.Contains(fromFenceTile1);
        if (this.touchingRanchTiles.Contains(fromFenceTile1) & flag2 & flag3 && checkedTiles.Count > 12)
        {
          wallTiles.Add(fromFenceTile1);
          return true;
        }
        if (flag3)
          checkedTiles.Add(fromFenceTile1);
        List<PlacementRegion.TileGridTile> wallTiles1;
        if (flag2 & flag3 && this.AreWallsConnecting(fromFenceTile1, out wallTiles1, checkedTiles))
        {
          wallTiles.Add(fromFenceTile1);
          wallTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) wallTiles1);
          flag1 = true;
        }
      }
    }
    return flag1;
  }

  public List<PlacementRegion.TileGridTile> GetSurroundingTiles(PlacementRegion.TileGridTile tile)
  {
    List<PlacementRegion.TileGridTile> surroundingTiles = new List<PlacementRegion.TileGridTile>();
    int num1 = -2;
    while (num1++ < 1)
    {
      int num2 = -2;
      while (num2++ < 1)
      {
        if (Mathf.Abs(num1) + Mathf.Abs(num2) == 1)
        {
          PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tile.Position.x + num1, tile.Position.y + num2));
          if (tileGridTile != null && !surroundingTiles.Contains(tileGridTile))
            surroundingTiles.Add(tileGridTile);
        }
      }
    }
    return surroundingTiles;
  }

  public List<PlacementRegion.TileGridTile> GetRanchingTiles()
  {
    List<PlacementRegion.TileGridTile> currentValidTiles = new List<PlacementRegion.TileGridTile>();
    for (int index = 0; index < this.Fences.Count; ++index)
    {
      currentValidTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) this.GetValidRanchingTiles(0, 1, this.Fences[index].Tile, currentValidTiles));
      currentValidTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) this.GetValidRanchingTiles(1, 0, this.Fences[index].Tile, currentValidTiles));
      currentValidTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) this.GetValidRanchingTiles(0, -1, this.Fences[index].Tile, currentValidTiles));
      currentValidTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) this.GetValidRanchingTiles(-1, 0, this.Fences[index].Tile, currentValidTiles));
    }
    List<PlacementRegion.TileGridTile> ranchingTiles = new List<PlacementRegion.TileGridTile>();
    for (int index = 0; index < currentValidTiles.Count; ++index)
    {
      if (!ranchingTiles.Contains(currentValidTiles[index]))
        ranchingTiles.Add(currentValidTiles[index]);
    }
    return ranchingTiles;
  }

  public List<PlacementRegion.TileGridTile> GetValidRanchingTiles(
    int xDirection,
    int yDirection,
    PlacementRegion.TileGridTile fromTile,
    List<PlacementRegion.TileGridTile> currentValidTiles)
  {
    List<PlacementRegion.TileGridTile> validRanchingTiles = new List<PlacementRegion.TileGridTile>();
    bool flag1 = false;
    if (xDirection != 0)
    {
      float num = Mathf.Abs(PlacementRegion.X_Constraints.x) + Mathf.Abs(PlacementRegion.X_Constraints.y);
      for (int index = 1; index < (int) num; ++index)
      {
        Vector2Int Position = new Vector2Int(fromTile.Position.x + index * xDirection, fromTile.Position.y);
        PlacementRegion.TileGridTile tileGridTile1 = PlacementRegion.Instance.GetTileGridTile(Position);
        if (tileGridTile1 == null)
        {
          if ((double) Position.x <= (double) PlacementRegion.X_Constraints.x || (double) Position.x >= (double) PlacementRegion.X_Constraints.y)
          {
            validRanchingTiles.Clear();
            break;
          }
        }
        else if (tileGridTile1.ObjectOnTile == StructureBrain.TYPES.NONE || StructureManager.IndestructibleByRanchStructures.Contains(tileGridTile1.ObjectOnTile))
        {
          if (index == (int) num - 1)
          {
            validRanchingTiles.Clear();
            break;
          }
          if (!validRanchingTiles.Contains(tileGridTile1))
            validRanchingTiles.Add(tileGridTile1);
        }
        else
        {
          if ((tileGridTile1.ObjectOnTile == StructureBrain.TYPES.RANCH || tileGridTile1.ObjectOnTile == StructureBrain.TYPES.RANCH_2) && tileGridTile1.ObjectID == this.Data.ID)
          {
            flag1 = true;
            Debug.DrawLine(fromTile.WorldPosition, tileGridTile1.WorldPosition, Color.red, float.MaxValue);
            break;
          }
          if (this.ContainsFence(tileGridTile1))
          {
            int yDirection1 = -1;
            if ((double) this.Data.Position.y < (double) tileGridTile1.WorldPosition.y)
              yDirection1 = 1;
            PlacementRegion.TileGridTile tileGridTile2 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(fromTile.Position.x + xDirection * -1, fromTile.Position.y));
            PlacementRegion.TileGridTile tileGridTile3 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tileGridTile1.Position.x + xDirection, tileGridTile1.Position.y));
            if ((currentValidTiles.Contains(tileGridTile2) || this.ContainsFence(tileGridTile2)) && (currentValidTiles.Contains(tileGridTile3) || this.ContainsFence(tileGridTile3)))
            {
              PlacementRegion.TileGridTile tileGridTile4 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(fromTile.Position.x + xDirection, fromTile.Position.y));
              if (tileGridTile4 != null && !this.WillHitFence(0, yDirection1, tileGridTile4))
              {
                flag1 = false;
                break;
              }
              flag1 = true;
              Debug.DrawLine(fromTile.WorldPosition, tileGridTile1.WorldPosition, Color.red, float.MaxValue);
              break;
            }
            PlacementRegion.TileGridTile tileGridTile5 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(fromTile.Position.x + xDirection, fromTile.Position.y));
            if (tileGridTile5 != null && !this.WillHitFence(0, yDirection1, tileGridTile5))
            {
              flag1 = false;
              break;
            }
            flag1 = true;
            Debug.DrawLine(fromTile.WorldPosition, tileGridTile1.WorldPosition, Color.red, float.MaxValue);
            break;
          }
          if (!validRanchingTiles.Contains(tileGridTile1))
            validRanchingTiles.Add(tileGridTile1);
        }
      }
      if (!flag1)
        validRanchingTiles.Clear();
    }
    if (yDirection != 0)
    {
      bool flag2 = false;
      float num = Mathf.Abs(PlacementRegion.Y_Constraints.x) + Mathf.Abs(PlacementRegion.Y_Constraints.y);
      for (int index = 1; index < (int) num; ++index)
      {
        Vector2Int Position = new Vector2Int(fromTile.Position.x, fromTile.Position.y + index * yDirection);
        PlacementRegion.TileGridTile tileGridTile6 = PlacementRegion.Instance.GetTileGridTile(Position);
        if (tileGridTile6 == null)
        {
          if ((double) Position.y <= (double) PlacementRegion.Y_Constraints.x || (double) Position.y >= (double) PlacementRegion.Y_Constraints.y)
          {
            validRanchingTiles.Clear();
            break;
          }
        }
        else if (tileGridTile6.ObjectOnTile == StructureBrain.TYPES.NONE || StructureManager.IndestructibleByRanchStructures.Contains(tileGridTile6.ObjectOnTile))
        {
          if (index == (int) num - 1)
          {
            validRanchingTiles.Clear();
            break;
          }
          if (!validRanchingTiles.Contains(tileGridTile6))
            validRanchingTiles.Add(tileGridTile6);
        }
        else
        {
          if ((tileGridTile6.ObjectOnTile == StructureBrain.TYPES.RANCH || tileGridTile6.ObjectOnTile == StructureBrain.TYPES.RANCH_2) && tileGridTile6.ObjectID == this.Data.ID)
          {
            flag2 = true;
            Debug.DrawLine(fromTile.WorldPosition, tileGridTile6.WorldPosition, Color.blue, float.MaxValue);
            break;
          }
          if (this.ContainsFence(tileGridTile6))
          {
            PlacementRegion.TileGridTile tileGridTile7 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(fromTile.Position.x, fromTile.Position.y + yDirection * -1));
            PlacementRegion.TileGridTile tileGridTile8 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tileGridTile6.Position.x, tileGridTile6.Position.y + yDirection));
            if ((currentValidTiles.Contains(tileGridTile7) || this.ContainsFence(tileGridTile7)) && (currentValidTiles.Contains(tileGridTile8) || this.ContainsFence(tileGridTile8)))
            {
              flag2 = false;
              break;
            }
            Debug.DrawLine(fromTile.WorldPosition, tileGridTile6.WorldPosition, Color.blue, float.MaxValue);
            flag2 = true;
            break;
          }
          if (!validRanchingTiles.Contains(tileGridTile6))
            validRanchingTiles.Add(tileGridTile6);
        }
      }
      if (!flag2)
        validRanchingTiles.Clear();
    }
    return validRanchingTiles;
  }

  public bool ContainsFence(PlacementRegion.TileGridTile fenceTile)
  {
    foreach (Structures_Ranch.FenceData fence in this.Fences)
    {
      if (fence.Tile == fenceTile)
        return true;
    }
    return false;
  }

  public bool ContainsFence(
    PlacementRegion.TileGridTile fenceTile,
    out Structures_Ranch.FenceData fenceData)
  {
    foreach (Structures_Ranch.FenceData fence in this.Fences)
    {
      if (fence.Tile == fenceTile)
      {
        fenceData = fence;
        return true;
      }
    }
    fenceData = (Structures_Ranch.FenceData) null;
    return false;
  }

  public bool WillHitFence(int xDirection, int yDirection, PlacementRegion.TileGridTile fromTile)
  {
    if (xDirection != 0)
    {
      float num = Mathf.Abs(PlacementRegion.X_Constraints.x) + Mathf.Abs(PlacementRegion.X_Constraints.y);
      for (int index = 1; index < (int) num; ++index)
      {
        Vector2Int Position = new Vector2Int(fromTile.Position.x + index * xDirection, fromTile.Position.y);
        PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(Position);
        if (tileGridTile == null)
        {
          if ((double) Position.x <= (double) PlacementRegion.X_Constraints.x || (double) Position.x >= (double) PlacementRegion.X_Constraints.y)
            break;
        }
        else if (tileGridTile.ObjectOnTile == StructureBrain.TYPES.NONE || StructureManager.IndestructibleByRanchStructures.Contains(tileGridTile.ObjectOnTile))
        {
          if (index == (int) num - 1)
            break;
        }
        else if ((tileGridTile.ObjectOnTile == StructureBrain.TYPES.RANCH || tileGridTile.ObjectOnTile == StructureBrain.TYPES.RANCH_2) && tileGridTile.ObjectID == this.Data.ID || this.ContainsFence(tileGridTile))
          return true;
      }
    }
    if (yDirection != 0)
    {
      float num = Mathf.Abs(PlacementRegion.Y_Constraints.x) + Mathf.Abs(PlacementRegion.Y_Constraints.y);
      for (int index = 1; index < (int) num; ++index)
      {
        Vector2Int Position = new Vector2Int(fromTile.Position.x, fromTile.Position.y + index * yDirection);
        PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(Position);
        if (tileGridTile == null)
        {
          if ((double) Position.y <= (double) PlacementRegion.Y_Constraints.x || (double) Position.y >= (double) PlacementRegion.Y_Constraints.y)
            break;
        }
        else if (tileGridTile.ObjectOnTile == StructureBrain.TYPES.NONE || StructureManager.IndestructibleByRanchStructures.Contains(tileGridTile.ObjectOnTile))
        {
          if (index == (int) num - 1)
            break;
        }
        else if ((tileGridTile.ObjectOnTile == StructureBrain.TYPES.RANCH || tileGridTile.ObjectOnTile == StructureBrain.TYPES.RANCH_2) && tileGridTile.ObjectID == this.Data.ID || this.ContainsFence(tileGridTile))
          return true;
      }
    }
    return false;
  }

  public List<Structures_RanchTrough> GetTroughsContainingFood()
  {
    List<Structures_RanchTrough> troughsContainingFood = new List<Structures_RanchTrough>();
    foreach (Structures_RanchTrough structuresRanchTrough in StructureManager.GetAllStructuresOfType<Structures_RanchTrough>())
    {
      if (structuresRanchTrough.Data.Inventory.Count > 0)
        troughsContainingFood.Add(structuresRanchTrough);
    }
    return troughsContainingFood;
  }

  public InventoryItem.ITEM_TYPE AnimalEatFood(
    StructuresData.Ranchable_Animal animal,
    Structures_RanchTrough trough)
  {
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) trough.Data.Inventory[0].type;
    trough.Data.Inventory.RemoveAt(0);
    animal.Satiation += (float) Interaction_Ranchable.FoodSatiation(type);
    return type;
  }

  public List<PlacementRegion.TileGridTile> GetRanchTiles()
  {
    PlacementRegion.TileGridTile tileGridTile1 = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition);
    List<PlacementRegion.TileGridTile> ranchTiles = new List<PlacementRegion.TileGridTile>();
    int num1 = -1;
    while (++num1 < this.Data.Bounds.x)
    {
      int num2 = -1;
      while (++num2 < this.Data.Bounds.y)
      {
        PlacementRegion.TileGridTile tileGridTile2 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tileGridTile1.Position.x + num1, tileGridTile1.Position.y + num2));
        if (tileGridTile2 != null && !ranchTiles.Contains(tileGridTile2))
          ranchTiles.Add(tileGridTile2);
      }
    }
    return ranchTiles;
  }

  public List<PlacementRegion.TileGridTile> GetSurroundingRanchTiles()
  {
    PlacementRegion.TileGridTile tileGridTile1 = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition);
    List<PlacementRegion.TileGridTile> surroundingRanchTiles = new List<PlacementRegion.TileGridTile>();
    if (tileGridTile1 != null)
    {
      PlacementRegion.TileGridTile tileGridTile2 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tileGridTile1.Position.x + 1, tileGridTile1.Position.y - 1));
      if (tileGridTile2 != null)
        surroundingRanchTiles.Add(tileGridTile2);
      PlacementRegion.TileGridTile tileGridTile3 = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(tileGridTile1.Position.x - 1, tileGridTile1.Position.y + 1));
      if (tileGridTile3 != null)
        surroundingRanchTiles.Add(tileGridTile3);
    }
    else
      Debug.LogError((object) "mainTile is null!");
    return surroundingRanchTiles;
  }

  public List<PlacementRegion.TileGridTile> GetValidFenceTiles()
  {
    List<PlacementRegion.TileGridTile> validFenceTiles = new List<PlacementRegion.TileGridTile>((IEnumerable<PlacementRegion.TileGridTile>) this.GetRanchTiles());
    validFenceTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) this.GetSurroundingRanchTiles());
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_FENCE, true))
    {
      validFenceTiles.Add(PlacementRegion.Instance.GetTileGridTile(structureBrain.Data.GridTilePosition));
      validFenceTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) this.GetSurroundingTiles(PlacementRegion.Instance.GetTileGridTile(structureBrain.Data.GridTilePosition)));
    }
    return validFenceTiles;
  }

  public override void OnNewPhaseStarted()
  {
    base.OnNewPhaseStarted();
    List<StructuresData.Ranchable_Animal> ranchableAnimalList = new List<StructuresData.Ranchable_Animal>((IEnumerable<StructuresData.Ranchable_Animal>) this.Data.Animals);
    if (StructureManager.GetAllStructuresOfType<Structures_Ranch>().OrderBy<Structures_Ranch, int>((Func<Structures_Ranch, int>) (x => x.Data.ID)).FirstOrDefault<Structures_Ranch>().Data.ID == this.Data.ID)
      ranchableAnimalList.AddRange((IEnumerable<StructuresData.Ranchable_Animal>) DataManager.Instance.BreakingOutAnimals);
    foreach (StructuresData.Ranchable_Animal animal in ranchableAnimalList)
    {
      if (animal.State != Interaction_Ranchable.State.BabyInHutch)
      {
        int age = animal.Age;
        if (TimeManager.CurrentPhase == DayPhase.Dawn)
          ++animal.Age;
        if (age < 2 && animal.Age >= 2)
        {
          animal.WorkedReady = true;
          animal.WorkedToday = false;
          animal.MilkedReady = true;
          animal.MilkedToday = false;
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.WaitForAnimalToGrowUp);
        }
        if (PlayerFarming.Location != FollowerLocation.Base)
        {
          if ((double) animal.Satiation < 50.0)
          {
            foreach (Structures_RanchTrough trough in this.GetTroughsContainingFood())
            {
              if (trough.Data.Inventory.Count > 0)
              {
                int num = (int) this.AnimalEatFood(animal, trough);
                break;
              }
            }
          }
          if ((double) TimeManager.TotalElapsedGameTime > (double) animal.TimeSinceLastWash && animal.Ailment == Interaction_Ranchable.Ailment.None && this.Data.Type != StructureBrain.TYPES.RANCH_2)
          {
            animal.Ailment = Interaction_Ranchable.Ailment.Stinky;
            animal.AilmentGameTime = TimeManager.TotalElapsedGameTime;
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalStinky", animal.GetName());
          }
        }
        if (DataManager.Instance.OnboardedWool)
          animal.Satiation -= 2f;
        animal.Satiation = Mathf.Clamp(animal.Satiation, 0.0f, (float) int.MaxValue);
        if ((double) animal.Injured > 0.0)
          --animal.Injured;
        if (animal.Ailment == Interaction_Ranchable.Ailment.Feral)
          --animal.FeralCalming;
        if (TimeManager.CurrentPhase == DayPhase.Dawn)
        {
          if (animal.Age >= 2)
          {
            if (animal.State != Interaction_Ranchable.State.Overcrowded)
              ++animal.GrowthStage;
            if (animal.GrowthStage >= Interaction_Ranchable.getResourceGrowthRateDays(animal))
            {
              animal.GrowthStage = 0;
              animal.WorkedReady = true;
              animal.WorkedToday = false;
            }
            animal.MilkedReady = true;
            animal.MilkedToday = false;
          }
          animal.EatenToday = false;
          animal.PetToday = false;
          animal.PlayerMadeHappyToday = false;
          if (PlayerFarming.Location != FollowerLocation.Base && animal.Age >= 15 && animal.State != Interaction_Ranchable.State.Dead && !animal.IsPlayersAnimal() && (double) UnityEngine.Random.value <= (double) animal.Age / 100.0)
          {
            animal.CauseOfDeath = Interaction_Ranchable.CauseOfDeath.DiedFromOldAge;
            animal.State = Interaction_Ranchable.State.Dead;
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalDiedFromOldAge", animal.GetName());
            for (int index = Interaction_Ranchable.Ranchables.Count - 1; index >= 0; --index)
            {
              Interaction_Ranchable ranchable = Interaction_Ranchable.Ranchables[index];
              if ((UnityEngine.Object) ranchable != (UnityEngine.Object) null && ranchable.Animal == animal)
              {
                Interaction_Ranchable.Ranchables.Remove(ranchable);
                Interaction_Ranchable.DeadRanchables.Add(ranchable);
                ranchable.SetDead();
              }
            }
          }
        }
      }
    }
    if (PlayerFarming.Location != FollowerLocation.Base && this.IsOvercrowded)
      NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.RanchOvercrowded);
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
      ranchable.UpdateGrowthState();
  }

  public void BeginMating(
    Structures_RanchHutch hutch,
    StructuresData.Ranchable_Animal parent1,
    StructuresData.Ranchable_Animal parent2)
  {
    AnimalData.AddAnimal(parent1, hutch.Data.Animals);
    AnimalData.AddAnimal(parent2, hutch.Data.Animals);
    parent1.State = Interaction_Ranchable.State.InsideHutch;
    parent2.State = Interaction_Ranchable.State.InsideHutch;
    hutch.Data.TargetPhase = TimeManager.CurrentPhase >= DayPhase.Night ? DayPhase.Dawn : TimeManager.CurrentPhase + 1;
  }

  public void AbortMating(Structures_RanchHutch hutch)
  {
    this.TryDetachParents(hutch, out StructuresData.Ranchable_Animal _, out StructuresData.Ranchable_Animal _);
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    foreach (StructuresData.Ranchable_Animal animal1 in this.Data.Animals)
    {
      if (animal1.State == Interaction_Ranchable.State.EnteringHutch)
      {
        Interaction_Ranchable animal2 = Interaction_Ranch.GetAnimal(animal1);
        if (animal2.IsTargetingHutch(hutch))
        {
          animal1.State = Interaction_Ranchable.State.Default;
          animal2.ResetTargetHutch();
        }
      }
    }
  }

  public bool TryDetachParents(
    Structures_RanchHutch hutch,
    out StructuresData.Ranchable_Animal coparent1,
    out StructuresData.Ranchable_Animal coparent2)
  {
    if (hutch.Data.Animals.Count <= 0)
    {
      coparent1 = (StructuresData.Ranchable_Animal) null;
      coparent2 = (StructuresData.Ranchable_Animal) null;
      return false;
    }
    for (int index = 0; index < hutch.Data.Animals.Count; ++index)
    {
      Debug.Log((object) $"Structures_Ranch: Detaching animal {hutch.Data.Animals[index].ID} (Age: {hutch.Data.Animals[index].Age}) from hutch...");
      hutch.Data.Animals[index].State = Interaction_Ranchable.State.Default;
      Interaction_Ranch.GetAnimal(hutch.Data.Animals[index])?.ResetTargetHutch();
    }
    coparent1 = hutch.Data.Animals[0];
    coparent2 = hutch.Data.Animals.Count >= 2 ? hutch.Data.Animals[1] : (StructuresData.Ranchable_Animal) null;
    hutch.Data.Animals.Clear();
    return coparent2 != null;
  }

  public void EndMating(Structures_RanchHutch hutch)
  {
    StructuresData.Ranchable_Animal coparent1;
    StructuresData.Ranchable_Animal coparent2;
    if (!this.TryDetachParents(hutch, out coparent1, out coparent2))
      return;
    AnimalData.AddAnimal(this.GetChildAnimal(coparent1, coparent2), hutch.Data.Animals);
    if (PlayerFarming.Location == FollowerLocation.Base)
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/baby_born", hutch.Data.Position);
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    foreach (Interaction_RanchHutch hutch1 in Interaction_RanchHutch.Hutches)
    {
      if (hutch1.Brain.Data.ID == hutch.Data.ID)
      {
        hutch1.SpawnBaby();
        break;
      }
    }
  }

  public void AnimalAdded(
    StructuresData.Ranchable_Animal animal,
    Vector3 position,
    System.Action callback)
  {
    int num = this.IsOvercrowded ? 1 : 0;
    AnimalData.AddAnimal(animal, this.Data.Animals);
    Structures_Ranch.AnimalEvent onAnimalAdded = this.OnAnimalAdded;
    if (onAnimalAdded == null)
      return;
    onAnimalAdded(animal, position, callback);
  }

  public StructuresData.Ranchable_Animal GetChildAnimal(
    StructuresData.Ranchable_Animal parent1,
    StructuresData.Ranchable_Animal parent2)
  {
    if ((double) UnityEngine.Random.value < 0.5)
    {
      StructuresData.Ranchable_Animal ranchableAnimal = parent2;
      parent2 = parent1;
      parent1 = ranchableAnimal;
    }
    ++DataManager.Instance.AnimalID;
    return new StructuresData.Ranchable_Animal()
    {
      ID = DataManager.Instance.AnimalID,
      Type = parent1.Type,
      Horns = parent1.Horns,
      Ears = parent1.Ears,
      Colour = parent2.Colour,
      Head = (double) UnityEngine.Random.value < 0.5 ? parent2.Head : parent1.Head,
      Speed = (float) (((double) parent1.Speed + (double) parent2.Speed) / 2.0) + UnityEngine.Random.Range(-0.2f, 0.2f),
      FavouriteFood = parent2.FavouriteFood
    };
  }

  public StructuresData.Ranchable_Animal GetAnimal(int animalID)
  {
    foreach (StructuresData.Ranchable_Animal animal in this.Data.Animals)
    {
      if (animal.ID == animalID)
        return animal;
    }
    return (StructuresData.Ranchable_Animal) null;
  }

  public static int GetAnimalGrowthState(StructuresData.Ranchable_Animal animal)
  {
    return animal.GrowthState;
  }

  public void RemoveAnimal(StructuresData.Ranchable_Animal animal)
  {
    AnimalData.RemoveAnimal(animal, this.Data.Animals);
  }

  public void AnimalPooped(
    StructuresData.Ranchable_Animal animal,
    Vector3 position,
    Action<GameObject> callback)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.POOP, 0);
    PlacementRegion.TileGridTile closeTile = StructureManager.GetCloseTile(position, FollowerLocation.Base);
    if (closeTile != null)
    {
      infoByType.GridTilePosition = closeTile.Position;
      StructureManager.BuildStructure(FollowerLocation.Base, infoByType, closeTile.WorldPosition, Vector2Int.one, false, callback);
    }
    else
      StructureManager.BuildStructure(FollowerLocation.Base, infoByType, position, Vector2Int.one, false, callback);
    animal.TimeSincePoop = TimeManager.TotalElapsedGameTime + 1920f + UnityEngine.Random.Range(-120f, 120f);
  }

  public override void OnSeasonChanged(SeasonsManager.Season season)
  {
    base.OnSeasonChanged(season);
    if (season != SeasonsManager.Season.Winter)
      return;
    DataManager.Instance.TimeSinceLastWolf = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(Interaction_Ranchable.TIME_BETWEEN_WOLVES.x, Interaction_Ranchable.TIME_BETWEEN_WOLVES.y);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    if (this.ReservedForTask || this.Data.Animals.Count <= 0 || this.Data.Type != StructureBrain.TYPES.RANCH_2)
      return;
    for (int index = 0; index < this.Data.Animals.Count; ++index)
    {
      if (this.Data.Animals[index].IsAvailableForFollowerTask())
      {
        FollowerTask_Rancher followerTaskRancher = new FollowerTask_Rancher(this.Data.ID);
        sortedTasks.Add(followerTaskRancher.Priorty, (FollowerTask) followerTaskRancher);
        break;
      }
    }
  }

  public class FenceData
  {
    public PlacementRegion.TileGridTile Tile;
    public Structures_RanchFence Fence;
  }

  public delegate void AnimalEvent(
    StructuresData.Ranchable_Animal animal,
    Vector3 position,
    System.Action spawnAnimalCallback);
}
