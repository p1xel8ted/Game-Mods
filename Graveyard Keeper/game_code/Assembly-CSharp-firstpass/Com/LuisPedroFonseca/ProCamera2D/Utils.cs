// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.Utils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public static class Utils
{
  public static float EaseFromTo(float start, float end, float value, EaseType type = EaseType.EaseInOut)
  {
    value = Mathf.Clamp01(value);
    switch (type)
    {
      case EaseType.EaseInOut:
        return Mathf.Lerp(start, end, (float) ((double) value * (double) value * (3.0 - 2.0 * (double) value)));
      case EaseType.EaseOut:
        return Mathf.Lerp(start, end, Mathf.Sin((float) ((double) value * 3.1415927410125732 * 0.5)));
      case EaseType.EaseIn:
        return Mathf.Lerp(start, end, 1f - Mathf.Cos((float) ((double) value * 3.1415927410125732 * 0.5)));
      default:
        return Mathf.Lerp(start, end, value);
    }
  }

  public static float SmoothApproach(
    float pastPosition,
    float pastTargetPosition,
    float targetPosition,
    float speed,
    float deltaTime)
  {
    float num1 = deltaTime * speed;
    float num2 = (targetPosition - pastTargetPosition) / num1;
    float num3 = pastPosition - pastTargetPosition + num2;
    return (float) ((double) targetPosition - (double) num2 + (double) num3 * (double) Mathf.Exp(-num1));
  }

  public static float Remap(this float value, float from1, float to1, float from2, float to2)
  {
    return Mathf.Clamp((float) (((double) value - (double) from1) / ((double) to1 - (double) from1) * ((double) to2 - (double) from2)) + from2, from2, to2);
  }

  public static void DrawArrowForGizmo(
    Vector3 pos,
    Vector3 direction,
    float arrowHeadLength = 0.25f,
    float arrowHeadAngle = 20f)
  {
    Gizmos.DrawRay(pos, direction);
    Utils.DrawArrowEnd(true, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
  }

  public static void DrawArrowForGizmo(
    Vector3 pos,
    Vector3 direction,
    Color color,
    float arrowHeadLength = 0.25f,
    float arrowHeadAngle = 20f)
  {
    Gizmos.DrawRay(pos, direction);
    Utils.DrawArrowEnd(true, pos, direction, color, arrowHeadLength, arrowHeadAngle);
  }

  public static void DrawArrowForDebug(
    Vector3 pos,
    Vector3 direction,
    float arrowHeadLength = 0.25f,
    float arrowHeadAngle = 20f)
  {
    Debug.DrawRay(pos, direction);
    Utils.DrawArrowEnd(false, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
  }

  public static void DrawArrowForDebug(
    Vector3 pos,
    Vector3 direction,
    Color color,
    float arrowHeadLength = 0.25f,
    float arrowHeadAngle = 20f)
  {
    Debug.DrawRay(pos, direction, color);
    Utils.DrawArrowEnd(false, pos, direction, color, arrowHeadLength, arrowHeadAngle);
  }

  public static void DrawArrowEnd(
    bool gizmos,
    Vector3 pos,
    Vector3 direction,
    Color color,
    float arrowHeadLength = 0.25f,
    float arrowHeadAngle = 20f)
  {
    if (direction == Vector3.zero)
      return;
    Vector3 vector3_1 = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0.0f, 0.0f) * Vector3.back;
    Vector3 vector3_2 = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0.0f, 0.0f) * Vector3.back;
    Vector3 vector3_3 = Quaternion.LookRotation(direction) * Quaternion.Euler(0.0f, arrowHeadAngle, 0.0f) * Vector3.back;
    Vector3 vector3_4 = Quaternion.LookRotation(direction) * Quaternion.Euler(0.0f, -arrowHeadAngle, 0.0f) * Vector3.back;
    if (gizmos)
    {
      Gizmos.color = color;
      Gizmos.DrawRay(pos + direction, vector3_1 * arrowHeadLength);
      Gizmos.DrawRay(pos + direction, vector3_2 * arrowHeadLength);
      Gizmos.DrawRay(pos + direction, vector3_3 * arrowHeadLength);
      Gizmos.DrawRay(pos + direction, vector3_4 * arrowHeadLength);
    }
    else
    {
      Debug.DrawRay(pos + direction, vector3_1 * arrowHeadLength, color);
      Debug.DrawRay(pos + direction, vector3_2 * arrowHeadLength, color);
      Debug.DrawRay(pos + direction, vector3_3 * arrowHeadLength, color);
      Debug.DrawRay(pos + direction, vector3_4 * arrowHeadLength, color);
    }
  }

  public static bool AreNearlyEqual(float a, float b, float tolerance = 0.02f)
  {
    return (double) Mathf.Abs(a - b) < (double) tolerance;
  }

  public static Vector2 GetScreenSizeInWorldCoords(Camera gameCamera, float distance = 10f)
  {
    float x;
    float y;
    if (gameCamera.orthographic)
    {
      if ((double) gameCamera.orthographicSize <= 1.0 / 1000.0)
        return Vector2.zero;
      Vector3 worldPoint1 = gameCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, gameCamera.nearClipPlane));
      Vector3 worldPoint2 = gameCamera.ViewportToWorldPoint(new Vector3(1f, 0.0f, gameCamera.nearClipPlane));
      Vector3 worldPoint3 = gameCamera.ViewportToWorldPoint(new Vector3(1f, 1f, gameCamera.nearClipPlane));
      x = (worldPoint2 - worldPoint1).magnitude;
      Vector3 vector3 = worldPoint2;
      y = (worldPoint3 - vector3).magnitude;
    }
    else
    {
      y = 2f * Mathf.Abs(distance) * Mathf.Tan((float) ((double) gameCamera.fieldOfView * 0.5 * (Math.PI / 180.0)));
      x = y * gameCamera.aspect;
    }
    return new Vector2(x, y);
  }

  public static Vector3 GetVectorsSum(IList<Vector3> input)
  {
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < input.Count; ++index)
      zero += input[index];
    return zero;
  }

  public static float AlignToGrid(float input, float gridSize)
  {
    return Mathf.Round(Mathf.Round(input / gridSize) * gridSize / gridSize) * gridSize;
  }

  public static bool IsInsideRectangle(
    float x,
    float y,
    float width,
    float height,
    float pointX,
    float pointY)
  {
    return (double) pointX >= (double) x - (double) width * 0.5 && (double) pointX <= (double) x + (double) width * 0.5 && (double) pointY >= (double) y - (double) height * 0.5 && (double) pointY <= (double) y + (double) height * 0.5;
  }

  public static bool IsInsideCircle(float x, float y, float radius, float pointX, float pointY)
  {
    return ((double) pointX - (double) x) * ((double) pointX - (double) x) + ((double) pointY - (double) y) * ((double) pointY - (double) y) < (double) radius * (double) radius;
  }
}
