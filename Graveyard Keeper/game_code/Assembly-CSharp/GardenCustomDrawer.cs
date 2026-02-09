// Decompiled with JetBrains decompiler
// Type: GardenCustomDrawer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GardenCustomDrawer : MonoBehaviour
{
  public List<GameObject> garden_stages;
  public List<float> stage_costs;
  public const string GARDEN_GROWING_PARAM_NAME = "growing";
  public int _prev_stage = -1;

  public void Redraw(WorldGameObject wgo)
  {
    float progress = wgo.GetParam("growing");
    if ((double) progress < 0.0)
    {
      progress = 0.0f;
      wgo.SetParam("growing", 0.0f);
    }
    int currentGrowStage = this.GetCurrentGrowStage(progress);
    if (currentGrowStage == this._prev_stage)
      return;
    this._prev_stage = currentGrowStage;
    for (int index = 0; index < this.garden_stages.Count; ++index)
      this.garden_stages[index].SetActive(currentGrowStage == index);
  }

  public int GetCurrentGrowStage(float progress)
  {
    int num = Mathf.FloorToInt(progress);
    for (int index = this.garden_stages.Count - 1; index >= 0; --index)
    {
      if (num >= (int) this.stage_costs[index])
        return index;
    }
    return 0;
  }

  public bool IsCorrectDrawer(out string err)
  {
    err = "";
    if (this.garden_stages == null || this.garden_stages.Count == 0)
    {
      err += "Wrong garden stages count!\n";
      return false;
    }
    if (this.stage_costs == null || this.stage_costs.Count == 0)
    {
      err += "Wrong stages costs count!\n";
      return false;
    }
    if (this.garden_stages.Count != this.stage_costs.Count)
    {
      err += "garden_stages count != stage_costs count!\n";
      return false;
    }
    int num = 1;
    foreach (Object gardenStage in this.garden_stages)
    {
      if (gardenStage == (Object) null)
        err = $"{err}Garden Stage #{num.ToString()} is NULL!\n";
      ++num;
    }
    if (this.stage_costs.Count > 1)
    {
      for (int index = 0; index < this.stage_costs.Count - 1; ++index)
      {
        if ((double) this.stage_costs[index] >= (double) this.stage_costs[index + 1])
        {
          ref string local = ref err;
          string[] strArray = new string[8]
          {
            err,
            "Wrong stage_cost ",
            (index + 1).ToString(),
            ": ",
            null,
            null,
            null,
            null
          };
          float stageCost = this.stage_costs[index];
          strArray[4] = stageCost.ToString();
          strArray[5] = " >= ";
          stageCost = this.stage_costs[index + 1];
          strArray[6] = stageCost.ToString();
          strArray[7] = "!\n";
          string str = string.Concat(strArray);
          local = str;
        }
      }
    }
    return string.IsNullOrEmpty(err);
  }
}
