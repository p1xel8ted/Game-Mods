// Decompiled with JetBrains decompiler
// Type: Utils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Utils : BaseMonoBehaviour
{
  public static Matrix4x4 matrix = Matrix4x4.identity;
  public static bool gizmos = true;

  public static bool Between(float Value, float LowInclusive, float HighExclusive)
  {
    return (double) Value >= (double) LowInclusive && (double) Value < (double) HighExclusive;
  }

  public static float BounceLerp(
    float Target,
    float Scale,
    ref float ScaleSpeed,
    float Elasticity = 0.3f,
    float Bounce = 0.8f)
  {
    ScaleSpeed += (Target - Scale) * Elasticity / Time.deltaTime;
    return Scale += (ScaleSpeed *= Bounce) * Time.deltaTime;
  }

  public static float BounceLerpUnscaledDeltaTime(
    float Target,
    float Scale,
    ref float ScaleSpeed,
    float Elasticity = 0.3f,
    float Bounce = 0.8f)
  {
    ScaleSpeed += (Target - Scale) * Elasticity / Time.unscaledDeltaTime;
    return Scale += (ScaleSpeed *= Bounce) * Time.unscaledDeltaTime;
  }

  public static void SetColor(Color color)
  {
    if (!Utils.gizmos || !(Gizmos.color != color))
      return;
    Gizmos.color = color;
  }

  public static void DrawLine(Vector3 a, Vector3 b, Color color)
  {
    Utils.SetColor(color);
    if (Utils.gizmos)
      Gizmos.DrawLine(Utils.matrix.MultiplyPoint3x4(a), Utils.matrix.MultiplyPoint3x4(b));
    else
      Debug.DrawLine(Utils.matrix.MultiplyPoint3x4(a), Utils.matrix.MultiplyPoint3x4(b), color);
  }

  public static void DrawCircleXY(
    Vector3 center,
    float radius,
    Color color,
    float startAngle = 0.0f,
    float endAngle = 6.28318548f,
    int steps = 40)
  {
    while ((double) startAngle > (double) endAngle)
      startAngle -= 6.28318548f;
    Vector3 vector3_1 = new Vector3(Mathf.Cos(startAngle) * radius, 0.0f, Mathf.Sin(startAngle) * radius);
    for (int index = 0; index <= steps; ++index)
    {
      Vector3 vector3_2 = new Vector3(Mathf.Cos(Mathf.Lerp(startAngle, endAngle, (float) index / (float) steps)) * radius, Mathf.Sin(Mathf.Lerp(startAngle, endAngle, (float) index / (float) steps)) * radius, 0.0f);
      Utils.DrawLine(center + vector3_1, center + vector3_2, color);
      vector3_1 = vector3_2;
    }
  }

  public static void DrawCircleXZ(
    Vector3 center,
    float radius,
    Color color,
    float startAngle = 0.0f,
    float endAngle = 6.28318548f,
    int steps = 40)
  {
    while ((double) startAngle > (double) endAngle)
      startAngle -= 6.28318548f;
    Vector3 vector3_1 = new Vector3(Mathf.Cos(startAngle) * radius, 0.0f, Mathf.Sin(startAngle) * radius);
    for (int index = 0; index <= steps; ++index)
    {
      Vector3 vector3_2 = new Vector3(Mathf.Cos(Mathf.Lerp(startAngle, endAngle, (float) index / (float) steps)) * radius, 0.0f, Mathf.Sin(Mathf.Lerp(startAngle, endAngle, (float) index / (float) steps)) * radius);
      Utils.DrawLine(center + vector3_1, center + vector3_2, color);
      vector3_1 = vector3_2;
    }
  }

  public static float GetAngle(Vector3 fromPosition, Vector3 toPosition)
  {
    return Utils.Repeat(Mathf.Atan2(toPosition.y - fromPosition.y, toPosition.x - fromPosition.x) * 57.29578f, 360f);
  }

  public static float GetAngleR(Vector3 fromPosition, Vector3 toPosition)
  {
    return Utils.Repeat(Mathf.Atan2(toPosition.y - fromPosition.y, toPosition.x - fromPosition.x), 6.28318548f);
  }

  public static Utils.Direction GetAngleDirection(float Angle)
  {
    if ((double) Angle >= -45.0 && (double) Angle < 45.0)
      return Utils.Direction.Right;
    if ((double) Angle >= 45.0 && (double) Angle < 135.0)
      return Utils.Direction.Up;
    if ((double) Angle >= 135.0 || (double) Angle < -135.0)
      return Utils.Direction.Left;
    return (double) Angle >= -135.0 && (double) Angle < -45.0 ? Utils.Direction.Down : Utils.Direction.Right;
  }

  public static Utils.DirectionFull GetAngleDirectionFull(float Angle)
  {
    Angle = Utils.Repeat(Angle, 360f);
    if ((double) Angle > 112.5 && (double) Angle < 157.5 || (double) Angle > 22.5 && (double) Angle < 67.5)
      return Utils.DirectionFull.Up_Diagonal;
    if ((double) Angle > 202.5 && (double) Angle < 247.5 || (double) Angle > 292.5 && (double) Angle < 337.5)
      return Utils.DirectionFull.Down_Diagonal;
    if ((double) Angle >= 67.5 && (double) Angle <= 112.5)
      return Utils.DirectionFull.Up;
    if ((double) Angle >= 247.5 && (double) Angle <= 292.5)
      return Utils.DirectionFull.Down;
    return (double) Angle >= 337.5 && (double) Angle <= 22.5 || (double) Angle < 157.5 || (double) Angle > 202.5 ? Utils.DirectionFull.Right : Utils.DirectionFull.Left;
  }

  public static int GetRandomWeightedIndex(int[] weights, float multiplier = 1f)
  {
    if (weights == null || weights.Length == 0)
      return -1;
    int num1 = 0;
    for (int index = 0; index < weights.Length; ++index)
    {
      if (weights[index] >= 0)
        num1 += weights[index];
    }
    float num2 = Mathf.Clamp01(UnityEngine.Random.value * multiplier);
    float num3 = 0.0f;
    for (int randomWeightedIndex = 0; randomWeightedIndex < weights.Length; ++randomWeightedIndex)
    {
      if ((double) weights[randomWeightedIndex] > 0.0)
      {
        num3 += (float) weights[randomWeightedIndex] / (float) num1;
        if ((double) num3 >= (double) num2)
          return randomWeightedIndex;
      }
    }
    return -1;
  }

  public static float SmoothAngle(float CurrentAngle, float TargetAngle, float Easing)
  {
    return CurrentAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) TargetAngle - (double) CurrentAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) TargetAngle - (double) CurrentAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (double) Easing * ((double) Time.deltaTime * 60.0));
  }

  public static Vector3 RayToPosition(
    float Radius,
    Vector3 StartPosition,
    float Direction,
    float Distance,
    LayerMask layerToCheck)
  {
    Direction *= (float) Math.PI / 180f;
    Vector3 vector3 = StartPosition + new Vector3(Distance * Mathf.Cos(Direction), Distance * Mathf.Sin(Direction));
    RaycastHit2D[] raycastHit2DArray = Physics2D.CircleCastAll((Vector2) (StartPosition + new Vector3(Radius * Mathf.Cos(Direction), Radius * Mathf.Sin(Direction))), Radius, (Vector2) Vector3.Normalize(vector3 - StartPosition), Distance, (int) layerToCheck);
    return raycastHit2DArray.Length != 0 ? (Vector3) raycastHit2DArray[0].centroid : vector3;
  }

  public static bool WithinRange(float Value, float Min, float Max)
  {
    return (double) Value >= (double) Min && (double) Value <= (double) Max;
  }

  public static bool WithinRange(float Value, int Min, int Max)
  {
    return (double) Value >= (double) Min && (double) Value <= (double) Max;
  }

  public static bool PointWithinPolygon(Vector3 position, List<Vector3> points)
  {
    int index1 = points.Count - 1;
    bool flag = false;
    for (int index2 = 0; index2 < points.Count; ++index2)
    {
      if ((double) points[index2].y > (double) position.y != (double) points[index1].y > (double) position.y)
      {
        float num = (float) (((double) points[index1].x - (double) points[index2].x) * ((double) position.y - (double) points[index2].y) / ((double) points[index1].y - (double) points[index2].y)) + points[index2].x;
        if ((double) position.x < (double) num)
          flag = !flag;
      }
      index1 = index2;
    }
    return flag;
  }

  public static bool PointWithinPolygon(Vector3 position, Vector2[] points)
  {
    int index1 = points.Length - 1;
    bool flag = false;
    for (int index2 = 0; index2 < points.Length; ++index2)
    {
      if ((double) points[index2].y > (double) position.y != (double) points[index1].y > (double) position.y)
      {
        float num = (float) (((double) points[index1].x - (double) points[index2].x) * ((double) position.y - (double) points[index2].y) / ((double) points[index1].y - (double) points[index2].y)) + points[index2].x;
        if ((double) position.x < (double) num)
          flag = !flag;
      }
      index1 = index2;
    }
    return flag;
  }

  public static Vector2 RadianToVector2(float radian)
  {
    return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
  }

  public static Vector2 DegreeToVector2(float degree)
  {
    return Utils.RadianToVector2(degree * ((float) Math.PI / 180f));
  }

  public static float linear(float start, float end, float value) => Mathf.Lerp(start, end, value);

  public static float clerp(float start, float end, float value)
  {
    float num1 = 0.0f;
    float num2 = 360f;
    float num3 = Mathf.Abs((float) (((double) num2 - (double) num1) * 0.5));
    float num4;
    if ((double) end - (double) start < -(double) num3)
    {
      float num5 = (num2 - start + end) * value;
      num4 = start + num5;
    }
    else if ((double) end - (double) start > (double) num3)
    {
      float num6 = (float) -((double) num2 - (double) end + (double) start) * value;
      num4 = start + num6;
    }
    else
      num4 = start + (end - start) * value;
    return num4;
  }

  public static float spring(float start, float end, float value)
  {
    value = Mathf.Clamp01(value);
    value = (float) (((double) Mathf.Sin((float) ((double) value * 3.1415927410125732 * (0.20000000298023224 + 2.5 * (double) value * (double) value * (double) value))) * (double) Mathf.Pow(1f - value, 2.2f) + (double) value) * (1.0 + 1.2000000476837158 * (1.0 - (double) value)));
    return start + (end - start) * value;
  }

  public static float easeInQuad(float start, float end, float value)
  {
    end -= start;
    return end * value * value + start;
  }

  public static float easeOutQuad(float start, float end, float value)
  {
    end -= start;
    return (float) (-(double) end * (double) value * ((double) value - 2.0)) + start;
  }

  public static float easeInOutQuad(float start, float end, float value)
  {
    value /= 0.5f;
    end -= start;
    if ((double) value < 1.0)
      return end * 0.5f * value * value + start;
    --value;
    return (float) (-(double) end * 0.5 * ((double) value * ((double) value - 2.0) - 1.0)) + start;
  }

  public float easeInCubic(float start, float end, float value)
  {
    end -= start;
    return end * value * value * value + start;
  }

  public static float easeOutCubic(float start, float end, float value)
  {
    --value;
    end -= start;
    return end * (float) ((double) value * (double) value * (double) value + 1.0) + start;
  }

  public static float easeInOutCubic(float start, float end, float value)
  {
    value /= 0.5f;
    end -= start;
    if ((double) value < 1.0)
      return end * 0.5f * value * value * value + start;
    value -= 2f;
    return (float) ((double) end * 0.5 * ((double) value * (double) value * (double) value + 2.0)) + start;
  }

  public static float easeInQuart(float start, float end, float value)
  {
    end -= start;
    return end * value * value * value * value + start;
  }

  public static float easeOutQuart(float start, float end, float value)
  {
    --value;
    end -= start;
    return (float) (-(double) end * ((double) value * (double) value * (double) value * (double) value - 1.0)) + start;
  }

  public float easeInOutQuart(float start, float end, float value)
  {
    value /= 0.5f;
    end -= start;
    if ((double) value < 1.0)
      return end * 0.5f * value * value * value * value + start;
    value -= 2f;
    return (float) (-(double) end * 0.5 * ((double) value * (double) value * (double) value * (double) value - 2.0)) + start;
  }

  public static float easeInQuint(float start, float end, float value)
  {
    end -= start;
    return end * value * value * value * value * value + start;
  }

  public static float easeOutQuint(float start, float end, float value)
  {
    --value;
    end -= start;
    return end * (float) ((double) value * (double) value * (double) value * (double) value * (double) value + 1.0) + start;
  }

  public static float easeInOutQuint(float start, float end, float value)
  {
    value /= 0.5f;
    end -= start;
    if ((double) value < 1.0)
      return end * 0.5f * value * value * value * value * value + start;
    value -= 2f;
    return (float) ((double) end * 0.5 * ((double) value * (double) value * (double) value * (double) value * (double) value + 2.0)) + start;
  }

  public static float easeInSine(float start, float end, float value)
  {
    end -= start;
    return -end * Mathf.Cos(value * 1.57079637f) + end + start;
  }

  public static float easeOutSine(float start, float end, float value)
  {
    end -= start;
    return end * Mathf.Sin(value * 1.57079637f) + start;
  }

  public static float easeInOutSine(float start, float end, float value)
  {
    end -= start;
    return (float) (-(double) end * 0.5 * ((double) Mathf.Cos(3.14159274f * value) - 1.0)) + start;
  }

  public static float easeInExpo(float start, float end, float value)
  {
    end -= start;
    return end * Mathf.Pow(2f, (float) (10.0 * ((double) value - 1.0))) + start;
  }

  public static float easeOutExpo(float start, float end, float value)
  {
    end -= start;
    return end * (float) (-(double) Mathf.Pow(2f, -10f * value) + 1.0) + start;
  }

  public static float easeInOutExpo(float start, float end, float value)
  {
    value /= 0.5f;
    end -= start;
    if ((double) value < 1.0)
      return end * 0.5f * Mathf.Pow(2f, (float) (10.0 * ((double) value - 1.0))) + start;
    --value;
    return (float) ((double) end * 0.5 * (-(double) Mathf.Pow(2f, -10f * value) + 2.0)) + start;
  }

  public static float easeInCirc(float start, float end, float value)
  {
    end -= start;
    return (float) (-(double) end * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) - 1.0)) + start;
  }

  public static float easeOutCirc(float start, float end, float value)
  {
    --value;
    end -= start;
    return end * Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) + start;
  }

  public static float easeInOutCirc(float start, float end, float value)
  {
    value /= 0.5f;
    end -= start;
    if ((double) value < 1.0)
      return (float) (-(double) end * 0.5 * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) - 1.0)) + start;
    value -= 2f;
    return (float) ((double) end * 0.5 * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) + 1.0)) + start;
  }

  public static float easeInBounce(float start, float end, float value)
  {
    end -= start;
    float num = 1f;
    return end - Utils.easeOutBounce(0.0f, end, num - value) + start;
  }

  public static float easeOutBounce(float start, float end, float value)
  {
    value /= 1f;
    end -= start;
    if ((double) value < 0.36363637447357178)
      return end * (121f / 16f * value * value) + start;
    if ((double) value < 0.72727274894714355)
    {
      value -= 0.545454562f;
      return end * (float) (121.0 / 16.0 * (double) value * (double) value + 0.75) + start;
    }
    if ((double) value < 10.0 / 11.0)
    {
      value -= 0.8181818f;
      return end * (float) (121.0 / 16.0 * (double) value * (double) value + 15.0 / 16.0) + start;
    }
    value -= 0.954545438f;
    return end * (float) (121.0 / 16.0 * (double) value * (double) value + 63.0 / 64.0) + start;
  }

  public static float easeInOutBounce(float start, float end, float value)
  {
    end -= start;
    float num = 1f;
    return (double) value < (double) num * 0.5 ? Utils.easeInBounce(0.0f, end, value * 2f) * 0.5f + start : (float) ((double) Utils.easeOutBounce(0.0f, end, value * 2f - num) * 0.5 + (double) end * 0.5) + start;
  }

  public static float easeInBack(float start, float end, float value)
  {
    end -= start;
    value /= 1f;
    float num = 1.70158f;
    return (float) ((double) end * (double) value * (double) value * (((double) num + 1.0) * (double) value - (double) num)) + start;
  }

  public static float easeOutBack(float start, float end, float value)
  {
    float num = 1.70158f;
    end -= start;
    --value;
    return end * (float) ((double) value * (double) value * (((double) num + 1.0) * (double) value + (double) num) + 1.0) + start;
  }

  public static float easeInOutBack(float start, float end, float value)
  {
    float num1 = 1.70158f;
    end -= start;
    value /= 0.5f;
    if ((double) value < 1.0)
    {
      float num2 = num1 * 1.525f;
      return (float) ((double) end * 0.5 * ((double) value * (double) value * (((double) num2 + 1.0) * (double) value - (double) num2))) + start;
    }
    value -= 2f;
    float num3 = num1 * 1.525f;
    return (float) ((double) end * 0.5 * ((double) value * (double) value * (((double) num3 + 1.0) * (double) value + (double) num3) + 2.0)) + start;
  }

  public static float punch(float amplitude, float value)
  {
    if ((double) value == 0.0 || (double) value == 1.0)
      return 0.0f;
    float num1 = 0.3f;
    float num2 = num1 / 6.28318548f * Mathf.Asin(0.0f);
    return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((float) (((double) value * 1.0 - (double) num2) * 6.2831854820251465) / num1);
  }

  public static float easeInElastic(float start, float end, float value)
  {
    end -= start;
    float num1 = 1f;
    float num2 = num1 * 0.3f;
    float num3 = 0.0f;
    if ((double) value == 0.0)
      return start;
    if ((double) (value /= num1) == 1.0)
      return start + end;
    float num4;
    if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
    {
      num3 = end;
      num4 = num2 / 4f;
    }
    else
      num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
    return (float) -((double) num3 * (double) Mathf.Pow(2f, 10f * --value) * (double) Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.2831854820251465) / num2)) + start;
  }

  public static float easeOutElastic(float start, float end, float value)
  {
    end -= start;
    float num1 = 1f;
    float num2 = num1 * 0.3f;
    float num3 = 0.0f;
    if ((double) value == 0.0)
      return start;
    if ((double) (value /= num1) == 1.0)
      return start + end;
    float num4;
    if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
    {
      num3 = end;
      num4 = num2 * 0.25f;
    }
    else
      num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
    return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.2831854820251465) / num2) + end + start;
  }

  public float easeInOutElastic(float start, float end, float value)
  {
    end -= start;
    float num1 = 1f;
    float num2 = num1 * 0.3f;
    float num3 = 0.0f;
    if ((double) value == 0.0)
      return start;
    if ((double) (value /= num1 * 0.5f) == 2.0)
      return start + end;
    float num4;
    if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
    {
      num3 = end;
      num4 = num2 / 4f;
    }
    else
      num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
    return (double) value < 1.0 ? (float) (-0.5 * ((double) num3 * (double) Mathf.Pow(2f, 10f * --value) * (double) Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.2831854820251465) / num2))) + start : (float) ((double) num3 * (double) Mathf.Pow(2f, -10f * --value) * (double) Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.2831854820251465) / num2) * 0.5) + end + start;
  }

  public static float Repeat(float v1, float v2)
  {
    float num = v1 % v2;
    if ((double) num < 0.0)
      num += v2;
    return num;
  }

  public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
  {
    return Quaternion.Euler(angles) * (point - pivot) + pivot;
  }

  public static List<GameObject> GetChildren(GameObject obj)
  {
    List<GameObject> children = new List<GameObject>();
    IEnumerator enumerator = (IEnumerator) obj.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        children.Add(current.gameObject);
        children.AddRange((IEnumerable<GameObject>) Utils.GetChildren(current.gameObject));
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return children;
  }

  public enum Direction
  {
    Up,
    Down,
    Left,
    Right,
  }

  public enum DirectionFull
  {
    Up,
    Down,
    Left,
    Right,
    Up_Diagonal,
    Down_Diagonal,
  }
}
