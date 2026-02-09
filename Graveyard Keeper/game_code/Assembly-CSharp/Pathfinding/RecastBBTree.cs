// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastBBTree
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class RecastBBTree
{
  public RecastBBTreeBox root;

  public void QueryInBounds(Rect bounds, List<RecastMeshObj> buffer)
  {
    if (this.root == null)
      return;
    this.QueryBoxInBounds(this.root, bounds, buffer);
  }

  public void QueryBoxInBounds(RecastBBTreeBox box, Rect bounds, List<RecastMeshObj> boxes)
  {
    if ((UnityEngine.Object) box.mesh != (UnityEngine.Object) null)
    {
      if (!RecastBBTree.RectIntersectsRect(box.rect, bounds))
        return;
      boxes.Add(box.mesh);
    }
    else
    {
      if (RecastBBTree.RectIntersectsRect(box.c1.rect, bounds))
        this.QueryBoxInBounds(box.c1, bounds, boxes);
      if (!RecastBBTree.RectIntersectsRect(box.c2.rect, bounds))
        return;
      this.QueryBoxInBounds(box.c2, bounds, boxes);
    }
  }

  public bool Remove(RecastMeshObj mesh)
  {
    if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
      throw new ArgumentNullException(nameof (mesh));
    if (this.root == null)
      return false;
    bool found = false;
    Bounds bounds1 = mesh.GetBounds();
    Rect bounds2 = Rect.MinMaxRect(bounds1.min.x, bounds1.min.z, bounds1.max.x, bounds1.max.z);
    this.root = this.RemoveBox(this.root, mesh, bounds2, ref found);
    return found;
  }

  public RecastBBTreeBox RemoveBox(
    RecastBBTreeBox c,
    RecastMeshObj mesh,
    Rect bounds,
    ref bool found)
  {
    if (!RecastBBTree.RectIntersectsRect(c.rect, bounds))
      return c;
    if ((UnityEngine.Object) c.mesh == (UnityEngine.Object) mesh)
    {
      found = true;
      return (RecastBBTreeBox) null;
    }
    if ((UnityEngine.Object) c.mesh == (UnityEngine.Object) null && !found)
    {
      c.c1 = this.RemoveBox(c.c1, mesh, bounds, ref found);
      if (c.c1 == null)
        return c.c2;
      if (!found)
      {
        c.c2 = this.RemoveBox(c.c2, mesh, bounds, ref found);
        if (c.c2 == null)
          return c.c1;
      }
      if (found)
        c.rect = RecastBBTree.ExpandToContain(c.c1.rect, c.c2.rect);
    }
    return c;
  }

  public void Insert(RecastMeshObj mesh)
  {
    RecastBBTreeBox recastBbTreeBox1 = new RecastBBTreeBox(mesh);
    if (this.root == null)
    {
      this.root = recastBbTreeBox1;
    }
    else
    {
      RecastBBTreeBox recastBbTreeBox2 = this.root;
      while (true)
      {
        recastBbTreeBox2.rect = RecastBBTree.ExpandToContain(recastBbTreeBox2.rect, recastBbTreeBox1.rect);
        if (!((UnityEngine.Object) recastBbTreeBox2.mesh != (UnityEngine.Object) null))
        {
          float num1 = RecastBBTree.ExpansionRequired(recastBbTreeBox2.c1.rect, recastBbTreeBox1.rect);
          float num2 = RecastBBTree.ExpansionRequired(recastBbTreeBox2.c2.rect, recastBbTreeBox1.rect);
          recastBbTreeBox2 = (double) num1 >= (double) num2 ? ((double) num2 >= (double) num1 ? ((double) RecastBBTree.RectArea(recastBbTreeBox2.c1.rect) < (double) RecastBBTree.RectArea(recastBbTreeBox2.c2.rect) ? recastBbTreeBox2.c1 : recastBbTreeBox2.c2) : recastBbTreeBox2.c2) : recastBbTreeBox2.c1;
        }
        else
          break;
      }
      recastBbTreeBox2.c1 = recastBbTreeBox1;
      RecastBBTreeBox recastBbTreeBox3 = new RecastBBTreeBox(recastBbTreeBox2.mesh);
      recastBbTreeBox2.c2 = recastBbTreeBox3;
      recastBbTreeBox2.mesh = (RecastMeshObj) null;
    }
  }

  public static bool RectIntersectsRect(Rect r, Rect r2)
  {
    return (double) r.xMax > (double) r2.xMin && (double) r.yMax > (double) r2.yMin && (double) r2.xMax > (double) r.xMin && (double) r2.yMax > (double) r.yMin;
  }

  public static float ExpansionRequired(Rect r, Rect r2)
  {
    float num1 = Mathf.Min(r.xMin, r2.xMin);
    double num2 = (double) Mathf.Max(r.xMax, r2.xMax);
    float num3 = Mathf.Min(r.yMin, r2.yMin);
    float num4 = Mathf.Max(r.yMax, r2.yMax);
    double num5 = (double) num1;
    return (float) ((num2 - num5) * ((double) num4 - (double) num3)) - RecastBBTree.RectArea(r);
  }

  public static Rect ExpandToContain(Rect r, Rect r2)
  {
    double xmin = (double) Mathf.Min(r.xMin, r2.xMin);
    float num1 = Mathf.Max(r.xMax, r2.xMax);
    float num2 = Mathf.Min(r.yMin, r2.yMin);
    float num3 = Mathf.Max(r.yMax, r2.yMax);
    double ymin = (double) num2;
    double xmax = (double) num1;
    double ymax = (double) num3;
    return Rect.MinMaxRect((float) xmin, (float) ymin, (float) xmax, (float) ymax);
  }

  public static float RectArea(Rect r) => r.width * r.height;
}
