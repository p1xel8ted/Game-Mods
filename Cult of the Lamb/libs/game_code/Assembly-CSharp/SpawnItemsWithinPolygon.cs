// Decompiled with JetBrains decompiler
// Type: SpawnItemsWithinPolygon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (PolygonCollider2D))]
public class SpawnItemsWithinPolygon : BaseMonoBehaviour
{
  public bool RegenerateOnPlay;
  public bool RandomOffset;
  public PolygonCollider2D Polygon;
  [Range(0.0f, 100f)]
  public float ChanceToSpawn = 50f;
  public Vector2Int Spacing = new Vector2Int(1, 1);
  public List<GameObject> Prefabs = new List<GameObject>();
  [HideInInspector]
  public float spawnX;
  [HideInInspector]
  public float spawnY;

  public void OnEnable()
  {
    if (!((Object) RoomManager.Instance != (Object) null))
      return;
    RoomManager.Instance.OnInitEnemies += new RoomManager.InitEnemiesAction(this.InitEnemies);
  }

  public void InitEnemies()
  {
    if (!this.RegenerateOnPlay)
      return;
    this.GenerateAtRuntime();
  }

  public void OnDisable()
  {
    if (!((Object) RoomManager.Instance != (Object) null))
      return;
    RoomManager.Instance.OnInitEnemies -= new RoomManager.InitEnemiesAction(this.InitEnemies);
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.blue;
    this.Polygon = this.GetComponent<PolygonCollider2D>();
    int index = -1;
    while (++index < this.Polygon.points.Length - 1)
      Gizmos.DrawLine((Vector3) ((Vector2) this.transform.position + this.Polygon.points[index]), (Vector3) ((Vector2) this.transform.position + this.Polygon.points[index + 1]));
    Gizmos.DrawLine((Vector3) ((Vector2) this.transform.position + this.Polygon.points[0]), (Vector3) ((Vector2) this.transform.position + this.Polygon.points[this.Polygon.points.Length - 1]));
  }

  public void GenerateAtRuntime()
  {
    if (this.Prefabs.Count < 1)
      return;
    this.ClearAtRuntime();
    this.Polygon = this.GetComponent<PolygonCollider2D>();
    double x1 = (double) this.Polygon.bounds.center.x;
    Bounds bounds1 = this.Polygon.bounds;
    double x2 = (double) bounds1.extents.x;
    float num1 = (float) (x1 - x2);
    bounds1 = this.Polygon.bounds;
    double y1 = (double) bounds1.center.y;
    Bounds bounds2 = this.Polygon.bounds;
    double y2 = (double) bounds2.extents.y;
    float num2 = (float) (y1 - y2);
    bounds2 = this.Polygon.bounds;
    this.spawnX = bounds2.extents.x;
    bounds2 = this.Polygon.bounds;
    this.spawnY = bounds2.extents.y;
    int num3 = 0;
    while (true)
    {
      double num4 = (double) num3;
      bounds2 = this.Polygon.bounds;
      double num5 = (double) Mathf.Ceil(bounds2.extents.x * 2f);
      if (num4 < num5)
      {
        int num6 = 0;
        while (true)
        {
          double num7 = (double) num6;
          bounds2 = this.Polygon.bounds;
          double num8 = (double) Mathf.Ceil(bounds2.extents.y * 2f);
          if (num7 < num8)
          {
            float num9 = num6 % 2 == 0 ? 0.5f : 0.0f;
            if ((double) Random.Range(0, 100) <= (double) this.ChanceToSpawn && this.Polygon.OverlapPoint((Vector2) new Vector3(num1 + (float) num3 + num9, num2 + (float) num6, this.Polygon.transform.position.z)))
              Object.Instantiate<GameObject>(this.Prefabs[Random.Range(0, this.Prefabs.Count)], new Vector3(num1 + (float) num3 + num9, num2 + (float) num6, 0.0f), Quaternion.identity).transform.parent = this.transform;
            ++num6;
          }
          else
            break;
        }
        ++num3;
      }
      else
        break;
    }
  }

  public void ClearAtRuntime()
  {
    int childCount = this.transform.childCount;
    while (--childCount > -1)
      Object.Destroy((Object) this.transform.GetChild(childCount).gameObject);
  }
}
