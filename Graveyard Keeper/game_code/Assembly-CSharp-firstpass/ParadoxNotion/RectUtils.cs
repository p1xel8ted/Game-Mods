// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.RectUtils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace ParadoxNotion;

public static class RectUtils
{
  public static Rect GetBoundRect(params Rect[] rects)
  {
    float num1 = float.PositiveInfinity;
    float num2 = float.NegativeInfinity;
    float num3 = float.PositiveInfinity;
    float num4 = float.NegativeInfinity;
    for (int index = 0; index < rects.Length; ++index)
    {
      num1 = Mathf.Min(num1, rects[index].xMin);
      num2 = Mathf.Max(num2, rects[index].xMax);
      num3 = Mathf.Min(num3, rects[index].yMin);
      num4 = Mathf.Max(num4, rects[index].yMax);
    }
    return Rect.MinMaxRect(num1, num3, num2, num4);
  }

  public static Rect GetBoundRect(params Vector2[] positions)
  {
    float num1 = float.PositiveInfinity;
    float num2 = float.NegativeInfinity;
    float num3 = float.PositiveInfinity;
    float num4 = float.NegativeInfinity;
    for (int index = 0; index < positions.Length; ++index)
    {
      num1 = Mathf.Min(num1, positions[index].x);
      num2 = Mathf.Max(num2, positions[index].x);
      num3 = Mathf.Min(num3, positions[index].y);
      num4 = Mathf.Max(num4, positions[index].y);
    }
    return Rect.MinMaxRect(num1, num3, num2, num4);
  }

  public static bool Encapsulates(this Rect a, Rect b)
  {
    return (double) a.x < (double) b.x && (double) a.xMax > (double) b.xMax && (double) a.y < (double) b.y && (double) a.yMax > (double) b.yMax;
  }

  public static Rect ExpandBy(this Rect rect, float margin)
  {
    return Rect.MinMaxRect(rect.xMin - margin, rect.yMin - margin, rect.xMax + margin, rect.yMax + margin);
  }

  public static Rect TransformSpace(this Rect rect, Rect oldContainer, Rect newContainer)
  {
    return new Rect()
    {
      xMin = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, rect.xMin)),
      xMax = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, rect.xMax)),
      yMin = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, rect.yMin)),
      yMax = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, rect.yMax))
    };
  }
}
