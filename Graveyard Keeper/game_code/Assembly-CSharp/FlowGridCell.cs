// Decompiled with JetBrains decompiler
// Type: FlowGridCell
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FlowGridCell : MonoBehaviour
{
  public FlowGridCell.CellType _cell_type;
  public const float COLLIDER_RADIUS = 0.9f;

  public FlowGridCell.CellType cell_type => this._cell_type;

  public bool IsPlaceAvailable(string build_zone_id)
  {
    Vector3 position = this.transform.position;
    bool flag = Physics2D.OverlapBoxAll((Vector2) position, Vector2.one * 0.9f * 96f, 0.0f, 1).Length == 0;
    if (string.IsNullOrEmpty(build_zone_id))
      return flag;
    if (!flag)
      return false;
    Collider2D[] collider2DArray = Physics2D.OverlapBoxAll((Vector2) position, Vector2.one * 0.9f * 96f, 0.0f, 524288 /*0x080000*/);
    if (collider2DArray.Length == 0)
      return false;
    foreach (Component component1 in collider2DArray)
    {
      WorldZone component2 = component1.GetComponent<WorldZone>();
      if ((Object) component2 != (Object) null && component2.id == build_zone_id)
        return true;
    }
    return false;
  }

  public bool IsInsideWorldZone(string zone_id, string sub_zone_id)
  {
    foreach (Collider2D collider2D in Physics2D.OverlapBoxAll((Vector2) this.transform.position, BuildGrid.GRID_CHECK_BOX_SIZE, 0.0f, 524288 /*0x080000*/))
    {
      if (string.IsNullOrEmpty(sub_zone_id))
      {
        WorldZone component = collider2D.GetComponent<WorldZone>();
        if (!((Object) component == (Object) null) && component.id == zone_id)
          return true;
      }
      else
      {
        WorldSubZone component = collider2D.GetComponent<WorldSubZone>();
        if ((Object) component != (Object) null && component.sub_zone_id == sub_zone_id)
          return true;
      }
    }
    return false;
  }

  public static FlowGridCell Create(
    Transform parent_tr,
    Vector2 pos,
    int grid_scale_divider,
    FlowGridCell.CellType cell_type)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(MainGame.me.build_grid_cell);
    gameObject.transform.SetParent(parent_tr, false);
    gameObject.transform.localPosition = new Vector3(pos.x, pos.y, 1f);
    gameObject.transform.localScale = Vector3.one / (float) grid_scale_divider;
    FlowGridCell component1 = gameObject.GetComponent<FlowGridCell>();
    component1._cell_type = cell_type;
    component1.SetRedColorState(false);
    if ((Object) BuildGrid.current_sub_zone_configuration != (Object) null)
    {
      SpriteRenderer component2 = component1.GetComponent<SpriteRenderer>();
      component2.sortingLayerName = BuildGrid.current_sub_zone_configuration.build_cell_sorting_layer;
      if (!BuildGrid.current_sub_zone_configuration.is_cell_ground_object)
        Object.Destroy((Object) component1.GetComponent<GroundObject>());
      if (BuildGrid.current_sub_zone_configuration.override_grid_cell_sorting_order)
        component2.sortingOrder = BuildGrid.current_sub_zone_configuration.grid_cell_sorting_order;
    }
    return component1;
  }

  public void SetRedColorState(bool is_red_color)
  {
    foreach (SpriteRenderer componentsInChild in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = this._cell_type == FlowGridCell.CellType.TotemArea ? Color.blue : (is_red_color ? Color.red : Color.white);
  }

  public enum CellType
  {
    None,
    UnderObject,
    TotemArea,
  }
}
