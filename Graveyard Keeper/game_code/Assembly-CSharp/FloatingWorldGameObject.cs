// Decompiled with JetBrains decompiler
// Type: FloatingWorldGameObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FloatingWorldGameObject : MonoBehaviour
{
  public WorldGameObject _wo;
  public static FloatingWorldGameObject _cur_floating = (FloatingWorldGameObject) null;
  public const int MASK_SIZE = 51;
  public bool[,] _mask = new bool[51, 51];
  public Vector2 center_offsest = Vector2.zero;
  public bool _need_obj_mask_update = true;
  public static bool can_be_built = false;
  public static List<FlowGridCell> _cells = new List<FlowGridCell>();
  public static List<GameObject> _docks = new List<GameObject>();

  public WorldGameObject wobj => this._wo;

  public static FloatingWorldGameObject cur_floating => FloatingWorldGameObject._cur_floating;

  public static Vector3 cur_floating_pos
  {
    get
    {
      return !((Object) FloatingWorldGameObject._cur_floating != (Object) null) ? Vector3.zero : FloatingWorldGameObject._cur_floating.transform.position;
    }
  }

  public static FloatingWorldGameObject CreateFloatingObject(WorldGameObject prefab)
  {
    WorldGameObject worldGameObject = prefab.Copy<WorldGameObject>(LazyEngine.world_root);
    worldGameObject.GetComponent<RoundAndSortComponent>().grid_divider = 3;
    FloatingWorldGameObject floatingObject = worldGameObject.gameObject.AddComponent<FloatingWorldGameObject>();
    floatingObject._wo = worldGameObject;
    FloatingWorldGameObject._cur_floating = floatingObject;
    worldGameObject.SetBuildingColor(new Color(1f, 1f, 1f, 0.7f));
    floatingObject.RecalculateAvailability();
    floatingObject.UpdateGridColor();
    WorldMap.ActivateGameObject(worldGameObject.gameObject);
    return floatingObject;
  }

  public static void StopCurrentFloating(bool leave_on_scene = false)
  {
    if ((Object) FloatingWorldGameObject._cur_floating == (Object) null)
      return;
    FloatingWorldGameObject._cur_floating._wo.CancelSortOverEverything();
    InteractionBubbleGUI.RemoveBubble(FloatingWorldGameObject._cur_floating.wobj.unique_id);
    Debug.Log((object) ("StopCurrentFloating, leave_on_scene = " + leave_on_scene.ToString()));
    if (leave_on_scene)
    {
      FloatingWorldGameObject._cur_floating.wobj.GetComponent<RoundAndSortComponent>().SetZ();
      FloatingWorldGameObject._cur_floating.UpdateRoundAndSort();
      FloatingWorldGameObject.DestroyFlowGridCells();
      FloatingWorldGameObject._cur_floating.EnableAllColliders();
      FloatingWorldGameObject._cur_floating.DestroyComponent();
      FloatingWorldGameObject._cur_floating.wobj.UpdatePathCell();
      FloatingWorldGameObject._cur_floating.wobj.RecalculateZoneBelonging();
      FloatingWorldGameObject.can_be_built = true;
      FloatingWorldGameObject._cur_floating.UpdateGridColor();
      WorldZone myWorldZone = FloatingWorldGameObject._cur_floating.wobj.GetMyWorldZone();
      if ((Object) myWorldZone != (Object) null)
        myWorldZone.Recalculate();
    }
    else
      Object.Destroy((Object) FloatingWorldGameObject._cur_floating.gameObject);
    FloatingWorldGameObject._cur_floating = (FloatingWorldGameObject) null;
    LazyEngine.CancelCurrentItem();
  }

  public void EnableAllColliders(bool enable = true)
  {
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Collider2D>(true))
      componentsInChild.enabled = enable;
  }

  public static void DestroyFlowGridCells()
  {
    foreach (FlowGridCell cell in FloatingWorldGameObject._cells)
    {
      if (!((Object) cell == (Object) null))
        Object.Destroy((Object) cell.gameObject);
    }
    FloatingWorldGameObject._cells.Clear();
    foreach (GameObject dock in FloatingWorldGameObject._docks)
    {
      if (!((Object) dock == (Object) null))
        Object.Destroy((Object) dock);
    }
    FloatingWorldGameObject._docks.Clear();
  }

  public static void MoveCurrentFloatingObject(
    Vector2 pos,
    bool is_global_pos = true,
    Vector2? direction_shift = null)
  {
    if ((Object) FloatingWorldGameObject._cur_floating == (Object) null)
      return;
    if (!is_global_pos)
      pos -= FloatingWorldGameObject._cur_floating.center_offsest;
    FloatingWorldGameObject._cur_floating.gameObject.SetActive(true);
    if (direction_shift.HasValue)
      pos += new Vector2((double) direction_shift.Value.x > 0.0 ? 1f : ((double) direction_shift.Value.x < 0.0 ? -1f : 0.0f), (double) direction_shift.Value.y > 0.0 ? (float) (int) GridUnderMovingObj.obj_size.y : ((double) direction_shift.Value.y < 0.0 ? -1f : 0.0f));
    if (is_global_pos)
      FloatingWorldGameObject._cur_floating.wobj.MoveWhenPlacingGlobalPos(pos);
    else
      FloatingWorldGameObject._cur_floating.wobj.MoveWhenPlacingLocalPos(pos);
    FloatingWorldGameObject._cur_floating.RecalculateAvailability();
    FloatingWorldGameObject._cur_floating.UpdateGridColor();
  }

  public static void RotateCurrentFloatingObject(bool rotate_right = true)
  {
    if ((Object) FloatingWorldGameObject._cur_floating == (Object) null)
      return;
    if (rotate_right)
      FloatingWorldGameObject._cur_floating._wo.NextVariationRadiobutton();
    else
      FloatingWorldGameObject._cur_floating._wo.PrevVariationRadiobutton();
    FloatingWorldGameObject._cur_floating.RecalculateObjectMask();
    FloatingWorldGameObject._cur_floating.RecalculateAvailability();
    FloatingWorldGameObject._cur_floating.UpdateGridColor();
  }

  public static void MoveCurrentByDir(Vector2 dir)
  {
    if ((Object) FloatingWorldGameObject._cur_floating == (Object) null || dir.magnitude.EqualsTo(0.0f))
      return;
    Vector3 vector3 = FloatingWorldGameObject._cur_floating.transform.position + (Vector3) dir.normalized * 32f;
    Vector2 screenPoint = (Vector2) Camera.main.WorldToScreenPoint(vector3);
    if ((double) screenPoint.x < 0.0 || (double) screenPoint.x > (double) Screen.width || (double) screenPoint.y < 0.0 || (double) screenPoint.y > (double) Screen.height)
      return;
    FloatingWorldGameObject._cur_floating.wobj.MoveWhenPlacingGlobalPos((Vector2) vector3);
    FloatingWorldGameObject._cur_floating.RecalculateAvailability();
    FloatingWorldGameObject._cur_floating.UpdateGridColor();
  }

  public static bool IsFloating()
  {
    return (Object) FloatingWorldGameObject._cur_floating != (Object) null;
  }

  public void UpdateObjSize()
  {
    if ((Object) FloatingWorldGameObject._cur_floating == (Object) null)
      return;
    Debug.Log((object) "FloatingWorldGameObject.UpdateObjSize");
    this._need_obj_mask_update = true;
  }

  public void RecalculateObjectMask()
  {
    Debug.Log((object) "FloatingWorldGameObject.RecalculateObjectMask - calculating obj size");
    this._need_obj_mask_update = false;
    Vector2 position = (Vector2) this.transform.position;
    Collider2D[] componentsInChildren = this.GetComponentsInChildren<Collider2D>(true);
    List<Collider2D> target_colliders = new List<Collider2D>();
    List<Collider2D> collider2DList = new List<Collider2D>();
    foreach (Collider2D collider in componentsInChildren)
    {
      if (!BuildGrid.SkipCollider(collider))
      {
        target_colliders.Add(collider);
        if (!collider.enabled)
        {
          collider.enabled = true;
          collider2DList.Add(collider);
        }
      }
    }
    Vector2 vector2 = position - (new Vector2(51f, 51f) / 2f - Vector2.one * 0.5f) * 32f;
    for (int index1 = 0; index1 < 51; ++index1)
    {
      for (int index2 = 0; index2 < 51; ++index2)
      {
        this._mask[index1, 51 - index2 - 1] = false;
        Vector2 pos = vector2 + new Vector2((float) (index1 * 32 /*0x20*/), (float) (index2 * 32 /*0x20*/));
        this._mask[index1, 51 - index2 - 1] = BuildGrid.IsCellBusy(pos, target_colliders);
      }
    }
    this.DrawFlowGrid();
    this.wobj.RecalculateGridShape();
    foreach (Behaviour behaviour in collider2DList)
      behaviour.enabled = false;
  }

  public static FloatingWorldGameObject CreateFloatingWorldObjectById(string obj_id)
  {
    FloatingWorldGameObject floatingObject = FloatingWorldGameObject.CreateFloatingObject(Prefabs.wgo_prefab);
    if (!string.IsNullOrEmpty(obj_id))
    {
      floatingObject.wobj.SetObject(obj_id);
      floatingObject.wobj.ForceInitOptimizedColliders();
      floatingObject.UpdateObjSize();
    }
    if ((Object) BuildGrid.current_sub_zone_configuration != (Object) null && BuildGrid.current_sub_zone_configuration.sort_floating_over_everything)
      floatingObject._wo.SetSortOverEverything();
    FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) MainGame.me.player.transform.localPosition, false);
    FloatingWorldGameObject._cur_floating = floatingObject;
    foreach (ObjectDynamicShadow componentsInChild in floatingObject.GetComponentsInChildren<ObjectDynamicShadow>())
    {
      componentsInChild.Start();
      componentsInChild.Awake();
    }
    foreach (OptimizedCollider2D componentsInChild in floatingObject.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
    floatingObject.RecalculateObjectMask();
    floatingObject.EnableAllColliders(false);
    return floatingObject;
  }

  public static WorldGameObject GetWGOUnderFloatingCursor()
  {
    Collider2D[] collider2DArray = Physics2D.OverlapBoxAll((Vector2) FloatingWorldGameObject._cur_floating.gameObject.GetComponentInChildren<FlowGridCell>().transform.position, Vector2.one * 0.9f * 96f, 0.0f, 1);
    return collider2DArray.Length == 0 ? (WorldGameObject) null : collider2DArray[0].GetComponentInParent<WorldGameObject>();
  }

  public void UpdateRoundAndSort()
  {
    foreach (RoundAndSortComponent componentsInChild in this.wobj.GetComponentsInChildren<RoundAndSortComponent>(true))
      componentsInChild.DoUpdateStuff(true);
  }

  public void Update()
  {
    if (!this._need_obj_mask_update)
      return;
    this.RecalculateObjectMask();
  }

  public void DrawFlowGrid()
  {
    Debug.Log((object) "FloatingWorldGameObject.DrawFlowGrid");
    FloatingWorldGameObject.DestroyFlowGridCells();
    if (this.wobj.obj_def == null)
    {
      Debug.LogError((object) ("Obj def is null, id = " + this.wobj.obj_id));
    }
    else
    {
      Transform transform = this.transform;
      bool flag1 = this.wobj.obj_def.IsTotem();
      float totemRadius = this.wobj.obj_def.totem_radius;
      Debug.Log((object) $"Is totem = {flag1.ToString()}, totem_r = {totemRadius.ToString()}");
      Vector2 vector2_1 = Vector2.one * float.MaxValue;
      Vector2 vector2_2 = Vector2.one * float.MinValue;
      float num1 = 25f;
      bool flag2 = this.center_offsest.magnitude.EqualsTo(0.0f);
      for (int index1 = 0; index1 < 51; ++index1)
      {
        float x = (float) index1 - num1;
        for (int index2 = 0; index2 < 51; ++index2)
        {
          float y = (float) -index2 + num1;
          FlowGridCell.CellType cell_type = FlowGridCell.CellType.None;
          Vector2 pos = new Vector2(x, y);
          if (this._mask[index1, index2])
          {
            cell_type = FlowGridCell.CellType.UnderObject;
          }
          else
          {
            int num2 = flag1 ? 1 : 0;
          }
          if (cell_type != FlowGridCell.CellType.None)
          {
            pos /= 3f;
            FlowGridCell flowGridCell = FlowGridCell.Create(transform, pos, 3, cell_type);
            FloatingWorldGameObject._cells.Add(flowGridCell);
            if (flag2 && cell_type == FlowGridCell.CellType.UnderObject)
            {
              if ((double) pos.x < (double) vector2_1.x)
                vector2_1.x = pos.x;
              if ((double) pos.x > (double) vector2_2.x)
                vector2_2.x = pos.x;
              if ((double) pos.y < (double) vector2_1.y)
                vector2_1.y = pos.y;
              if ((double) pos.y > (double) vector2_2.y)
                vector2_2.y = pos.y;
            }
          }
        }
      }
      foreach (DockPoint componentsInChild in this._wo.GetComponentsInChildren<DockPoint>())
      {
        GameObject gameObject = Object.Instantiate<GameObject>(MainGame.me.dock_point_marker.GetMarker(componentsInChild.GetActionDir()));
        gameObject.transform.SetParent(this._wo.gameObject.transform, false);
        gameObject.transform.position = componentsInChild.transform.position;
        FloatingWorldGameObject._docks.Add(gameObject);
      }
      if (!flag2)
        return;
      Transform center = (Object) this.wobj.wop != (Object) null ? this.wobj.wop.center : (Transform) null;
      if ((Object) center == (Object) null)
      {
        Vector3 vector3 = (Vector3) (vector2_2 - vector2_1);
        this.center_offsest.x = vector2_1.x + Mathf.Abs(vector3.x) / 2f;
        this.center_offsest.y = vector2_1.y + Mathf.Abs(vector3.y) / 2f;
      }
      else
        this.center_offsest = (Vector2) ((center.position - this.wobj.tf.position) / 96f);
    }
  }

  public void RecalculateAvailability()
  {
    FloatingWorldGameObject.can_be_built = true;
    foreach (FlowGridCell cell in FloatingWorldGameObject._cells)
    {
      if (!((Object) cell == (Object) null) && !((Object) cell.gameObject == (Object) null) && cell.gameObject.activeSelf && cell.cell_type != FlowGridCell.CellType.TotemArea)
      {
        bool flag = !BuildGrid.IsCellBusy((Vector2) cell.transform.position) && cell.IsInsideWorldZone(MainGame.me.build_mode_logics.cur_build_zone_id, BuildGrid.GetCurrentSubZoneID());
        FloatingWorldGameObject.can_be_built &= flag;
      }
    }
  }

  public void UpdateGridColor()
  {
    if ((Object) this.wobj != (Object) null)
      this.wobj.SetBuildingColor(FloatingWorldGameObject.can_be_built ? Color.white : Color.red);
    foreach (FlowGridCell cell in FloatingWorldGameObject._cells)
    {
      if (!((Object) cell == (Object) null) && !((Object) cell.gameObject == (Object) null) && cell.gameObject.activeSelf)
        cell.SetRedColorState(!FloatingWorldGameObject.can_be_built);
    }
  }

  public static bool IsObjectRotatable()
  {
    return !((Object) FloatingWorldGameObject.cur_floating == (Object) null) && !((Object) FloatingWorldGameObject.cur_floating.wobj == (Object) null) && FloatingWorldGameObject.cur_floating.wobj.CanBeRotatedWhilePlacing();
  }
}
