// Decompiled with JetBrains decompiler
// Type: Structures_FarmerStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_FarmerStation : StructureBrain, ITaskProvider
{
  public const float MAX_PLOT_DISTANCE = 6f;
  public List<Structures_BerryBush> cachedBerryBushes = new List<Structures_BerryBush>();
  public List<Structures_FarmerPlot> cachedFullgrownPlots = new List<Structures_FarmerPlot>();
  public List<Structures_FarmerPlot> cachedUnwateredPlots = new List<Structures_FarmerPlot>();
  public List<Structures_FarmerPlot> cachedUnseededPlots = new List<Structures_FarmerPlot>();
  public List<Structures_FarmerPlot> cachedUnfertilizedPlots = new List<Structures_FarmerPlot>();

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    Structures_SiloFertiliser result1;
    if (StructureManager.TryGetFirstStructureOfType<Structures_SiloFertiliser>(out Structures_SiloFertiliser _, (Func<Structures_SiloFertiliser, bool>) (s => s.GetCompostCount() <= 0 && !s.ReservedForTask)) && StructureManager.TryGetFirstStructureOfType<Structures_SiloFertiliser>(out result1, (Func<Structures_SiloFertiliser, bool>) (siloFertiliser => siloFertiliser.Data.Type == StructureBrain.TYPES.POOP_BUCKET && siloFertiliser.GetCompostCount() > 0 && !siloFertiliser.ReservedForTask)) && result1 != null)
    {
      FollowerTask_Farm followerTaskFarm = new FollowerTask_Farm(this.Data.ID);
      tasks.Add(followerTaskFarm.Priorty, (FollowerTask) followerTaskFarm);
    }
    else
    {
      Structures_SiloSeed result2;
      if (StructureManager.TryGetFirstStructureOfType<Structures_SiloSeed>(out Structures_SiloSeed _, (Func<Structures_SiloSeed, bool>) (s => s.GetCompostCount() <= 0 && !s.ReservedForTask)) && StructureManager.TryGetFirstStructureOfType<Structures_SiloSeed>(out result2, (Func<Structures_SiloSeed, bool>) (siloSeed => siloSeed.Data.Type == StructureBrain.TYPES.SEED_BUCKET && siloSeed.GetCompostCount() > 0 && !siloSeed.ReservedForTask)) && result2 != null)
      {
        FollowerTask_Farm followerTaskFarm = new FollowerTask_Farm(this.Data.ID);
        tasks.Add(followerTaskFarm.Priorty, (FollowerTask) followerTaskFarm);
      }
      else if (this.GetNextUnseededPlot() != null)
      {
        FollowerTask_Farm followerTaskFarm = new FollowerTask_Farm(this.Data.ID);
        tasks.Add(followerTaskFarm.Priorty, (FollowerTask) followerTaskFarm);
      }
      else if (this.GetNextUnwateredPlot() != null)
      {
        FollowerTask_Farm followerTaskFarm = new FollowerTask_Farm(this.Data.ID);
        tasks.Add(followerTaskFarm.Priorty, (FollowerTask) followerTaskFarm);
      }
      else if (this.GetNextUnfertilizedPlot() != null && Structures_SiloFertiliser.GetClosestFertiliser(this.Data.Position, this.Data.Location) != null)
      {
        FollowerTask_Farm followerTaskFarm = new FollowerTask_Farm(this.Data.ID);
        tasks.Add(followerTaskFarm.Priorty, (FollowerTask) followerTaskFarm);
      }
      else
      {
        if (this.GetNextUnpickedPlot() == null || this.Data.Type != StructureBrain.TYPES.FARM_STATION_II)
          return;
        FollowerTask_Farm followerTaskFarm = new FollowerTask_Farm(this.Data.ID);
        tasks.Add(followerTaskFarm.Priorty, (FollowerTask) followerTaskFarm);
      }
    }
  }

  public Structures_FarmerPlot GetNextUnwateredPlot()
  {
    Structures_FarmerPlot nextUnwateredPlot = (Structures_FarmerPlot) null;
    float num1 = 6f;
    BoxCollider2D boxCollider = GameManager.BoxCollider;
    boxCollider.size = Vector2.one * 6f;
    boxCollider.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    StructureManager.TryGetAllUnwateredPlots(ref this.cachedUnwateredPlots, this.Data.Location);
    foreach (Structures_FarmerPlot cachedUnwateredPlot in this.cachedUnwateredPlots)
    {
      if (!cachedUnwateredPlot.IsGroundFrozen)
      {
        float num2 = Vector3.Distance(this.Data.Position, cachedUnwateredPlot.Data.Position);
        Vector3 position = cachedUnwateredPlot.Data.Position;
        if ((double) num2 <= (double) num1 + 0.5 && boxCollider.OverlapPoint((Vector2) position))
        {
          nextUnwateredPlot = cachedUnwateredPlot;
          num1 = num2;
        }
      }
    }
    this.cachedUnwateredPlots.Clear();
    return nextUnwateredPlot;
  }

  public Structures_FarmerPlot GetNextUnseededPlot()
  {
    Structures_FarmerPlot nextUnseededPlot = (Structures_FarmerPlot) null;
    float num1 = 6f;
    BoxCollider2D boxCollider = GameManager.BoxCollider;
    StructureManager.TryGetAllUnseededPlots(ref this.cachedUnseededPlots, this.Data.Location);
    boxCollider.size = Vector2.one * 6f;
    boxCollider.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_FarmerPlot cachedUnseededPlot in this.cachedUnseededPlots)
    {
      if (!cachedUnseededPlot.IsGroundFrozen)
      {
        float num2 = Vector3.Distance(this.Data.Position, cachedUnseededPlot.Data.Position);
        Vector3 position = cachedUnseededPlot.Data.Position;
        if ((double) num2 <= (double) num1 + 0.5 && boxCollider.OverlapPoint((Vector2) position))
        {
          nextUnseededPlot = cachedUnseededPlot;
          num1 = num2;
        }
      }
    }
    this.cachedUnseededPlots.Clear();
    return nextUnseededPlot;
  }

  public Structures_FarmerPlot GetNextUnfertilizedPlot()
  {
    Structures_FarmerPlot unfertilizedPlot1 = (Structures_FarmerPlot) null;
    float num1 = 6f;
    BoxCollider2D boxCollider = GameManager.BoxCollider;
    boxCollider.size = Vector2.one * 6f;
    boxCollider.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    StructureManager.TryGetAllUnfertilizedPlots(ref this.cachedUnfertilizedPlots, this.Data.Location);
    foreach (Structures_FarmerPlot unfertilizedPlot2 in this.cachedUnfertilizedPlots)
    {
      float num2 = Vector3.Distance(this.Data.Position, unfertilizedPlot2.Data.Position);
      Vector3 position = unfertilizedPlot2.Data.Position;
      if ((double) num2 <= (double) num1 + 0.5 && boxCollider.OverlapPoint((Vector2) position))
      {
        unfertilizedPlot1 = unfertilizedPlot2;
        num1 = num2;
      }
    }
    this.cachedUnfertilizedPlots.Clear();
    return unfertilizedPlot1;
  }

  public Structures_SiloFertiliser GetEmptyFertiliserSilo()
  {
    foreach (Structures_SiloFertiliser emptyFertiliserSilo in StructureManager.GetAllStructuresOfType<Structures_SiloFertiliser>())
    {
      if (emptyFertiliserSilo.GetCompostCount() <= 0 && !emptyFertiliserSilo.ReservedForTask)
        return emptyFertiliserSilo;
    }
    return (Structures_SiloFertiliser) null;
  }

  public Structures_SiloFertiliser GetUnfilledFertiliserSilo()
  {
    foreach (Structures_SiloFertiliser unfilledFertiliserSilo in StructureManager.GetAllStructuresOfType<Structures_SiloFertiliser>())
    {
      if ((double) unfilledFertiliserSilo.GetCompostCount() < (double) unfilledFertiliserSilo.Capacity && !unfilledFertiliserSilo.ReservedForTask && unfilledFertiliserSilo.Data.Type == StructureBrain.TYPES.SILO_FERTILISER)
        return unfilledFertiliserSilo;
    }
    return (Structures_SiloFertiliser) null;
  }

  public Structures_SiloSeed GetEmptySeedSilo()
  {
    foreach (Structures_SiloSeed emptySeedSilo in StructureManager.GetAllStructuresOfType<Structures_SiloSeed>())
    {
      if (emptySeedSilo.GetCompostCount() <= 0 && !emptySeedSilo.ReservedForTask)
        return emptySeedSilo;
    }
    return (Structures_SiloSeed) null;
  }

  public Structures_SiloSeed GetUnfilledSeedSilo()
  {
    foreach (Structures_SiloSeed unfilledSeedSilo in StructureManager.GetAllStructuresOfType<Structures_SiloSeed>())
    {
      if ((double) unfilledSeedSilo.GetCompostCount() < (double) unfilledSeedSilo.Capacity && !unfilledSeedSilo.ReservedForTask && unfilledSeedSilo.Data.Type == StructureBrain.TYPES.SILO_SEED)
        return unfilledSeedSilo;
    }
    return (Structures_SiloSeed) null;
  }

  public List<Structures_BerryBush> GetCropsAtPosition(Vector3 position)
  {
    List<Structures_BerryBush> cropsAtPosition = new List<Structures_BerryBush>();
    StructureManager.TryGetAllUnpickedPlots(ref this.cachedBerryBushes, this.Data.Location);
    foreach (Structures_BerryBush cachedBerryBush in this.cachedBerryBushes)
    {
      if ((double) Vector3.Distance(cachedBerryBush.Data.Position, position) < 0.5 && !cachedBerryBush.BerryPicked)
        cropsAtPosition.Add(cachedBerryBush);
    }
    this.cachedBerryBushes.Clear();
    return cropsAtPosition;
  }

  public Structures_BerryBush GetNextUnpickedPlot()
  {
    Structures_BerryBush nextUnpickedPlot = (Structures_BerryBush) null;
    float num1 = 6f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * 6f;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    StructureManager.TryGetAllUnpickedPlots(ref this.cachedBerryBushes, this.Data.Location);
    StructureManager.TryGetAllFullGrownPlots(ref this.cachedFullgrownPlots, this.Data.Location);
    foreach (Structures_BerryBush cachedBerryBush in this.cachedBerryBushes)
    {
      bool flag = false;
      foreach (Structures_FarmerPlot cachedFullgrownPlot in this.cachedFullgrownPlots)
      {
        if (cachedFullgrownPlot.Data.ID == cachedBerryBush.CropID)
        {
          if (!cachedFullgrownPlot.Data.Withered)
          {
            if (cachedFullgrownPlot.HasFertilized())
            {
              if (cachedFullgrownPlot.GetFertilizer().type == 144 /*0x90*/)
                break;
            }
            if (cachedFullgrownPlot.HasFertilized())
            {
              if (cachedFullgrownPlot.GetFertilizer().type == 162)
                break;
            }
          }
          if (cachedFullgrownPlot.GetPlantedSeed().type != 160 /*0xA0*/)
          {
            flag = true;
            break;
          }
          break;
        }
      }
      if (flag)
      {
        float num2 = Vector3.Distance(this.Data.Position, cachedBerryBush.Data.Position);
        Vector3 position = cachedBerryBush.Data.Position;
        if ((double) num2 <= (double) num1 + 0.5 && boxCollider2D.OverlapPoint((Vector2) position))
        {
          nextUnpickedPlot = cachedBerryBush;
          num1 = num2;
        }
      }
    }
    this.cachedBerryBushes.Clear();
    this.cachedFullgrownPlots.Clear();
    return nextUnpickedPlot;
  }
}
