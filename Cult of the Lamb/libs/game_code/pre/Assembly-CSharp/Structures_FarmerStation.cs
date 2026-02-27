// Decompiled with JetBrains decompiler
// Type: Structures_FarmerStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_FarmerStation : StructureBrain, ITaskProvider
{
  public const float MAX_PLOT_DISTANCE = 6f;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    if (this.GetNextUnseededPlot() != null && Structures_SiloSeed.GetClosestSeeder(this.Data.Position, this.Data.Location) != null)
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

  public Structures_FarmerPlot GetNextUnwateredPlot()
  {
    Structures_FarmerPlot nextUnwateredPlot = (Structures_FarmerPlot) null;
    float num1 = 6f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) boxCollider2D == (Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * 6f;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_FarmerPlot allUnwateredPlot in StructureManager.GetAllUnwateredPlots(this.Data.Location))
    {
      float num2 = Vector3.Distance(this.Data.Position, allUnwateredPlot.Data.Position);
      Vector3 position = allUnwateredPlot.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num2 < (double) num1)
      {
        nextUnwateredPlot = allUnwateredPlot;
        num1 = num2;
      }
    }
    return nextUnwateredPlot;
  }

  public Structures_FarmerPlot GetNextUnseededPlot()
  {
    Structures_FarmerPlot nextUnseededPlot = (Structures_FarmerPlot) null;
    float num1 = 6f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) boxCollider2D == (Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * 6f;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_FarmerPlot allUnseededPlot in StructureManager.GetAllUnseededPlots(this.Data.Location))
    {
      float num2 = Vector3.Distance(this.Data.Position, allUnseededPlot.Data.Position);
      Vector3 position = allUnseededPlot.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num2 < (double) num1)
      {
        nextUnseededPlot = allUnseededPlot;
        num1 = num2;
      }
    }
    return nextUnseededPlot;
  }

  public Structures_FarmerPlot GetNextUnfertilizedPlot()
  {
    Structures_FarmerPlot unfertilizedPlot1 = (Structures_FarmerPlot) null;
    float num1 = 6f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) boxCollider2D == (Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * 6f;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_FarmerPlot unfertilizedPlot2 in StructureManager.GetAllUnfertilizedPlots(this.Data.Location))
    {
      float num2 = Vector3.Distance(this.Data.Position, unfertilizedPlot2.Data.Position);
      Vector3 position = unfertilizedPlot2.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num2 < (double) num1)
      {
        unfertilizedPlot1 = unfertilizedPlot2;
        num1 = num2;
      }
    }
    return unfertilizedPlot1;
  }

  public List<Structures_BerryBush> GetCropsAtPosition(Vector3 position)
  {
    List<Structures_BerryBush> cropsAtPosition = new List<Structures_BerryBush>();
    foreach (Structures_BerryBush allUnpickedPlot in StructureManager.GetAllUnpickedPlots(this.Data.Location))
    {
      if ((double) Vector3.Distance(allUnpickedPlot.Data.Position, position) < 0.5 && !allUnpickedPlot.BerryPicked)
        cropsAtPosition.Add(allUnpickedPlot);
    }
    return cropsAtPosition;
  }

  public Structures_BerryBush GetNextUnpickedPlot()
  {
    Structures_BerryBush nextUnpickedPlot = (Structures_BerryBush) null;
    float num1 = 6f;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) boxCollider2D == (Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.size = Vector2.one * 6f;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (Structures_BerryBush allUnpickedPlot in StructureManager.GetAllUnpickedPlots(this.Data.Location))
    {
      if (PlayerFarming.Location == FollowerLocation.Base)
      {
        bool flag1 = false;
        foreach (FarmPlot farmPlot in FarmPlot.FarmPlots)
        {
          if (farmPlot.StructureInfo.ID == allUnpickedPlot.CropID && (Object) farmPlot._activeCropController != (Object) null)
          {
            bool flag2 = false;
            foreach (GameObject cropState in farmPlot._activeCropController.CropStates)
            {
              if (cropState.activeSelf)
                flag2 = true;
            }
            if (farmPlot._activeCropController.BumperCropObject.activeSelf)
              flag2 = true;
            if (flag2)
            {
              flag1 = true;
              break;
            }
          }
        }
        if (!flag1)
          continue;
      }
      float num2 = Vector3.Distance(this.Data.Position, allUnpickedPlot.Data.Position);
      Vector3 position = allUnpickedPlot.Data.Position;
      if (boxCollider2D.OverlapPoint((Vector2) position) && (double) num2 < (double) num1)
      {
        nextUnpickedPlot = allUnpickedPlot;
        num1 = num2;
      }
    }
    return nextUnpickedPlot;
  }
}
