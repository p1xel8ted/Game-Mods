// Decompiled with JetBrains decompiler
// Type: GJLine
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GJLine
{
  public static UI2DSprite _pixel_prefab;
  public static Stack<UI2DSprite> _free = new Stack<UI2DSprite>();
  public static List<UI2DSprite> _used = new List<UI2DSprite>();
  public static GameObject _root;

  public static UI2DSprite DrawPixel(Transform parent, Vector2 coords)
  {
    UI2DSprite ui2Dsprite = GJLine._free.Count != 0 ? GJLine._free.Pop() : UnityEngine.Object.Instantiate<UI2DSprite>(GJLine._pixel_prefab);
    GJLine._used.Add(ui2Dsprite);
    ui2Dsprite.gameObject.SetActive(true);
    ui2Dsprite.transform.parent = parent;
    ui2Dsprite.transform.localScale = Vector3.one;
    ui2Dsprite.transform.localPosition = (Vector3) coords;
    return ui2Dsprite;
  }

  public static void RemovePixel(UI2DSprite p)
  {
    p.gameObject.SetActive(false);
    GJLine._used.Remove(p);
    GJLine._free.Push(p);
    p.transform.parent = GJLine._root.transform;
  }

  public static void PreCache(int n)
  {
    for (int index = 0; index < n; ++index)
    {
      UI2DSprite ui2Dsprite = UnityEngine.Object.Instantiate<UI2DSprite>(GJLine._pixel_prefab);
      ui2Dsprite.gameObject.SetActive(false);
      ui2Dsprite.transform.parent = GJLine._root.transform;
      GJLine._free.Push(ui2Dsprite);
    }
  }

  public static void Init(GameObject pixel_prefab)
  {
    GJLine._root = new GameObject("GJ Line Prefabs");
    GJLine._pixel_prefab = pixel_prefab.GetComponent<UI2DSprite>();
    GJLine.PreCache(500);
  }

  public static void Swap(ref int v1, ref int v2)
  {
    int num = v1;
    v1 = v2;
    v2 = num;
  }

  public static GameObject DrawLine(
    Transform parent,
    int x0,
    int y0,
    int x1,
    int y1,
    Color color,
    int every_n_pixel = 1)
  {
    GameObject gameObject = new GameObject("Line");
    gameObject.transform.parent = parent;
    gameObject.transform.localScale = Vector3.one;
    bool flag = false;
    if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
    {
      GJLine.Swap(ref x0, ref y0);
      GJLine.Swap(ref x1, ref y1);
      flag = true;
    }
    if (x0 > x1)
    {
      GJLine.Swap(ref x0, ref x1);
      GJLine.Swap(ref y0, ref y1);
    }
    int num1 = x1 - x0;
    float num2 = Math.Abs((float) (y1 - y0) / (float) num1);
    float num3 = 0.0f;
    int num4 = y0;
    for (int index = x0; index <= x1; ++index)
    {
      int x = flag ? num4 : index;
      int y = flag ? index : num4;
      if (every_n_pixel == 1 || x % every_n_pixel == 0)
        GJLine.DrawPixel(gameObject.transform, new Vector2((float) x, (float) y)).color = color;
      num3 += num2;
      if ((double) num3 > 0.5)
      {
        num4 += y1 > y0 ? 1 : -1;
        --num3;
      }
    }
    return gameObject;
  }

  public static void DrawLine(
    Texture2D tx,
    int x0,
    int y0,
    int x1,
    int y1,
    Color color,
    int every_n_pixel = 1)
  {
    bool flag = false;
    if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
    {
      GJLine.Swap(ref x0, ref y0);
      GJLine.Swap(ref x1, ref y1);
      flag = true;
    }
    if (x0 > x1)
    {
      GJLine.Swap(ref x0, ref x1);
      GJLine.Swap(ref y0, ref y1);
    }
    int num1 = x1 - x0;
    float num2 = Math.Abs((float) (y1 - y0) / (float) num1);
    float num3 = 0.0f;
    int num4 = y0;
    for (int index = x0; index <= x1; ++index)
    {
      int x = flag ? num4 : index;
      int y = flag ? index : num4;
      if (every_n_pixel == 1 || x % every_n_pixel == 0)
        tx.SetPixel(x, y, color);
      num3 += num2;
      if ((double) num3 > 0.5)
      {
        num4 += y1 > y0 ? 1 : -1;
        --num3;
      }
    }
  }

  public static void RemoveLine(GameObject line)
  {
    foreach (UI2DSprite componentsInChild in line.GetComponentsInChildren<UI2DSprite>())
      GJLine.RemovePixel(componentsInChild);
    UnityEngine.Object.Destroy((UnityEngine.Object) line);
  }

  public static List<GameObject> DrawLineGraph(
    UIWidget graph_area,
    List<Vector2> points,
    Color color,
    int every_n_pixel = 1)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    int width = graph_area.width;
    int height = graph_area.height;
    int val1_1 = int.MinValue;
    int val1_2 = int.MinValue;
    int num1 = int.MaxValue;
    int num2 = int.MaxValue;
    Transform transform = graph_area.transform;
    foreach (Vector2 point in points)
    {
      val1_1 = Math.Max(val1_1, (int) point.x);
      val1_2 = Math.Max(val1_2, (int) point.y);
      num1 = Math.Min(num1, (int) point.x);
      num2 = Math.Min(num2, (int) point.y);
    }
    float num3 = ((float) val1_1 - (float) num1 * 1f) / (float) width;
    float num4 = ((float) val1_2 - (float) num2 * 1f) / (float) height;
    Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
    int num5 = height / 2;
    int num6 = width / 2;
    for (int index = 1; index < points.Count; ++index)
    {
      Vector2 vector2_2 = points[index - 1] - vector2_1;
      vector2_2 = new Vector2(vector2_2.x / num3 - (float) num6, vector2_2.y / num4 + (float) num5);
      Vector2 vector2_3 = points[index] - vector2_1;
      vector2_3 = new Vector2(vector2_3.x / num3 - (float) num6, vector2_3.y / num4 + (float) num5);
      gameObjectList.Add(GJLine.DrawLine(transform, (int) vector2_2.x, (int) vector2_2.y, (int) vector2_3.x, (int) vector2_3.y, color, every_n_pixel));
    }
    return gameObjectList;
  }

  public static Vector2 DrawLineGraph(
    Texture2D tx,
    List<Vector2> points,
    Color color,
    Vector2? min = null,
    Vector2? max = null,
    int every_n_pixel = 1)
  {
    int width = tx.width;
    int height = tx.height;
    int val1_1 = int.MinValue;
    int val1_2 = int.MinValue;
    int num1 = int.MaxValue;
    int num2 = int.MaxValue;
    foreach (Vector2 point in points)
    {
      val1_1 = Math.Max(val1_1, (int) point.x);
      val1_2 = Math.Max(val1_2, (int) point.y);
      num1 = Math.Min(num1, (int) point.x);
      num2 = Math.Min(num2, (int) point.y);
    }
    if (min.HasValue)
    {
      num1 = (int) min.Value.x;
      num2 = (int) min.Value.y;
    }
    if (max.HasValue)
    {
      val1_1 = (int) max.Value.x;
      val1_2 = (int) max.Value.y;
    }
    float x = ((float) val1_1 - (float) num1 * 1f) / (float) width;
    float y = ((float) val1_2 - (float) num2 * 1f) / (float) height;
    Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
    for (int index = 1; index < points.Count; ++index)
    {
      Vector2 vector2_2 = points[index - 1] - vector2_1;
      vector2_2 = new Vector2(Mathf.Max(0.0f, vector2_2.x / x), Mathf.Max(0.0f, vector2_2.y / y));
      Vector2 vector2_3 = points[index] - vector2_1;
      vector2_3 = new Vector2(Mathf.Max(0.0f, vector2_3.x / x), Mathf.Max(0.0f, vector2_3.y / y));
      GJLine.DrawLine(tx, (int) vector2_2.x, (int) vector2_2.y, (int) vector2_3.x, (int) vector2_3.y, color, every_n_pixel);
    }
    return new Vector2(x, y);
  }
}
