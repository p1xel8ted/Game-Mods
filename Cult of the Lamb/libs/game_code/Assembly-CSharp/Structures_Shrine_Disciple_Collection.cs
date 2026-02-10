// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Disciple_Collection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Shrine_Disciple_Collection : StructureBrain, ITaskProvider
{
  public override int SoulMax => 150;

  public static float Range => 11f;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => false;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.GetNextStructure() == null || this.SoulCount >= this.SoulMax)
      return;
    FollowerTask_CollectDevotion taskCollectDevotion = new FollowerTask_CollectDevotion(this.Data.ID);
    sortedTasks.Add(taskCollectDevotion.Priorty, (FollowerTask) taskCollectDevotion);
  }

  public StructureBrain GetNextStructure()
  {
    StructureBrain nextStructure = (StructureBrain) null;
    float num1 = Structures_Shrine_Disciple_Collection.Range;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) boxCollider2D == (Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    List<StructureBrain> allBrains = StructureBrain.AllBrains;
    boxCollider2D.size = Vector2.one * Structures_Shrine_Disciple_Collection.Range;
    boxCollider2D.transform.position = this.Data.Position + Vector3.up * 0.7f;
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (StructureBrain structureBrain in allBrains)
    {
      if (structureBrain.Data.SoulCount >= 3 && structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION && structureBrain.Data.Type != StructureBrain.TYPES.JANITOR_STATION && structureBrain.Data.Type != StructureBrain.TYPES.JANITOR_STATION_2 && structureBrain.Data.Type != StructureBrain.TYPES.SHRINE && structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_II && structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_III && structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_IV && structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST && !structureBrain.ReservedForTask)
      {
        float num2 = Vector3.Distance(this.Data.Position, structureBrain.Data.Position);
        Vector3 position = structureBrain.Data.Position;
        if ((double) num2 < (double) num1 && boxCollider2D.OverlapPoint((Vector2) position))
        {
          nextStructure = structureBrain;
          num1 = num2;
        }
      }
    }
    return nextStructure;
  }
}
