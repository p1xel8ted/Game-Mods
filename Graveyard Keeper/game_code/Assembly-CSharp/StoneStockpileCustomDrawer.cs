// Decompiled with JetBrains decompiler
// Type: StoneStockpileCustomDrawer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StoneStockpileCustomDrawer : MonoBehaviour
{
  public List<GameObject> stone_stages;
  public List<GameObject> marble_stages;
  public const string stone_items_name = "stone";
  public const string marble_items_name = "marble";
  public float update_period;
  public WorldGameObject _wobj;
  public bool _obj_is_set;
  public float _cur_update_time;

  public void Update()
  {
    if (!this._obj_is_set)
    {
      this._wobj = this.GetComponent<WorldObjectPart>()?.parent;
      this._obj_is_set = (Object) this._wobj != (Object) null;
    }
    if (!this._obj_is_set)
      return;
    this._cur_update_time += Time.deltaTime;
    if ((double) this._cur_update_time <= (double) this.update_period)
      return;
    this.Redraw(this._wobj);
    this._cur_update_time = 0.0f;
  }

  public void Redraw(WorldGameObject wobj)
  {
    List<Item> objList = new List<Item>((IEnumerable<Item>) wobj.data.inventory);
    int insertItemsLimit = wobj.obj_def.can_insert_items_limit;
    if (objList != null)
    {
      int num = 0;
      for (int index = 0; index < insertItemsLimit; ++index)
      {
        if (num < objList.Count)
        {
          switch (objList[index].id)
          {
            case "stone":
              this.stone_stages[index].SetActive(true);
              this.marble_stages[index].SetActive(false);
              break;
            case "marble":
              this.stone_stages[index].SetActive(false);
              this.marble_stages[index].SetActive(true);
              break;
          }
          ++num;
        }
        else
        {
          this.stone_stages[index].SetActive(false);
          this.marble_stages[index].SetActive(false);
        }
      }
    }
    else
      Debug.LogError((object) "Inventory of object is null");
  }

  public bool IsCorrectDrawer(out string err_mes)
  {
    err_mes = "";
    if (this.stone_stages == null || this.stone_stages.Count == 0)
    {
      err_mes += "Wrong stone_stages count'\n";
      return false;
    }
    if (this.marble_stages == null || this.marble_stages.Count == 0)
    {
      err_mes += "Wrong marble_stages count\n";
      return false;
    }
    if (this.stone_stages.Count != this.marble_stages.Count)
    {
      err_mes += "Stone and marble stages must be the same size\n";
      return false;
    }
    WorldGameObject componentInParent = this.GetComponentInParent<WorldGameObject>();
    if ((Object) componentInParent != (Object) null && componentInParent.obj_id == "mf_stones_1" && componentInParent.obj_def.can_insert_items_limit != this.stone_stages.Count)
      err_mes += "Stages count must be same as WGOs max insert limit\n";
    return string.IsNullOrEmpty(err_mes);
  }
}
