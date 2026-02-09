// Decompiled with JetBrains decompiler
// Type: GridUnderMovingObj
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class GridUnderMovingObj
{
  public static Vector2 obj_size = Vector2.zero;
  public static IntVector2 obj_size_min = new IntVector2();
  public static IntVector2 obj_size_max = new IntVector2();

  public static bool UpdateObjSize(WorldGameObject obj) => true;

  public static bool DoesObjectStandsOnPoint(
    List<Collider2D> obj_colliders,
    Vector2 pos,
    int x,
    int y)
  {
    Vector2 point = pos + new Vector2((float) (x * 32 /*0x20*/), (float) (y * 32 /*0x20*/));
    Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(point, BuildGrid.GRID_CHECK_BOX_SIZE, 0.0f, 1);
    new GameObject("* test " + pos.ToString()).transform.position = (Vector3) point;
    bool flag = false;
    foreach (Collider2D collider2D in collider2DArray)
    {
      if (obj_colliders.Contains(collider2D))
        flag = true;
    }
    return flag;
  }

  public static void UpdateColor()
  {
  }
}
