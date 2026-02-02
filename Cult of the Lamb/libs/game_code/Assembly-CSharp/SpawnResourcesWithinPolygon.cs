// Decompiled with JetBrains decompiler
// Type: SpawnResourcesWithinPolygon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (PolygonCollider2D))]
[ExecuteInEditMode]
public class SpawnResourcesWithinPolygon : BaseMonoBehaviour
{
  public PolygonCollider2D Polygon;
  [Range(0.0f, 100f)]
  public float ChanceToSpawn = 50f;
  public List<GameObject> Prefabs = new List<GameObject>();
  [Range(0.0f, 10f)]
  public float RanNoiseOffsetX;
  public float prevRanNoiseOffsetX;
  [Range(0.0f, 10f)]
  public float RanNoiseOffsetY;
  public float prevRanNoiseOffsetY;
  public List<Vector3> Nodes;
  public int xSpacing = 2;
  public int ySpacing = 2;

  public void OnDrawGizmos()
  {
    if ((double) this.prevRanNoiseOffsetX != (double) this.RanNoiseOffsetX || (double) this.prevRanNoiseOffsetY != (double) this.RanNoiseOffsetY)
      this.Generate();
    this.prevRanNoiseOffsetX = this.RanNoiseOffsetX;
    this.prevRanNoiseOffsetY = this.RanNoiseOffsetY;
    Gizmos.color = Color.red;
    this.Polygon = this.GetComponent<PolygonCollider2D>();
    int index = -1;
    while (++index < this.Polygon.points.Length - 1)
      Gizmos.DrawLine((Vector3) ((Vector2) this.transform.position + this.Polygon.points[index]), (Vector3) ((Vector2) this.transform.position + this.Polygon.points[index + 1]));
    Gizmos.DrawLine((Vector3) ((Vector2) this.transform.position + this.Polygon.points[0]), (Vector3) ((Vector2) this.transform.position + this.Polygon.points[this.Polygon.points.Length - 1]));
  }

  public void OnEnable()
  {
    this.prevRanNoiseOffsetX = this.RanNoiseOffsetX;
    this.prevRanNoiseOffsetY = this.RanNoiseOffsetY;
    this.CountNodes();
  }

  public void Generate()
  {
    if (this.Prefabs.Count < 1)
      return;
    this.Clear();
    this.CountNodes();
  }

  public void CountNodes()
  {
    this.Nodes = new List<Vector3>();
    this.Polygon = this.GetComponent<PolygonCollider2D>();
    Bounds bounds1 = this.Polygon.bounds;
    double x1 = (double) bounds1.center.x;
    bounds1 = this.Polygon.bounds;
    double x2 = (double) bounds1.extents.x;
    float num1 = (float) (x1 - x2);
    Bounds bounds2 = this.Polygon.bounds;
    double y1 = (double) bounds2.center.y;
    bounds2 = this.Polygon.bounds;
    double y2 = (double) bounds2.extents.y;
    float num2 = (float) (y1 - y2);
    int num3 = 0;
    while (true)
    {
      double num4 = (double) num3;
      Bounds bounds3 = this.Polygon.bounds;
      double num5 = (double) Mathf.Ceil(bounds3.extents.x * 2f);
      if (num4 < num5)
      {
        int num6 = 0;
        while (true)
        {
          double num7 = (double) num6;
          bounds3 = this.Polygon.bounds;
          double num8 = (double) Mathf.Ceil(bounds3.extents.y * 2f);
          if (num7 < num8)
          {
            float num9 = num6 % 2 == 0 ? (float) this.xSpacing * 0.5f : 0.0f;
            if (this.Polygon.OverlapPoint((Vector2) new Vector3(num1 + (float) num3 + num9, num2 + (float) num6, this.Polygon.transform.position.z)))
            {
              Vector3 vector3 = new Vector3(num1 + (float) num3 + num9, num2 + (float) num6, this.Polygon.transform.position.z);
              if (!this.Nodes.Contains(vector3))
                this.Nodes.Add(vector3);
            }
            num6 += this.ySpacing;
          }
          else
            break;
        }
        num3 += this.xSpacing;
      }
      else
        break;
    }
  }

  public void Clear()
  {
  }

  public void OnDisable()
  {
  }
}
