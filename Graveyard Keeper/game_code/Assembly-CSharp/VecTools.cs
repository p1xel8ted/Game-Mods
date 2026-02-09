// Decompiled with JetBrains decompiler
// Type: VecTools
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class VecTools
{
  public static Direction ToDirection(this Vector2 vec)
  {
    if (vec.magnitude.EqualsTo(0.0f))
      return Direction.None;
    return (double) Mathf.Abs(vec.x) > (double) Mathf.Abs(vec.y) ? ((double) vec.x >= 0.0 ? Direction.Right : Direction.Left) : ((double) vec.y >= 0.0 ? Direction.Up : Direction.Down);
  }

  public static Vector2 ToVec(this Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        return Vector2.right;
      case Direction.Up:
        return Vector2.up;
      case Direction.Left:
        return Vector2.left;
      case Direction.Down:
        return Vector2.down;
      default:
        return Vector2.zero;
    }
  }

  public static Vector3 ToVec3(this Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        return Vector3.right;
      case Direction.Up:
        return Vector3.up;
      case Direction.Left:
        return Vector3.left;
      case Direction.Down:
        return Vector3.down;
      default:
        return Vector3.zero;
    }
  }

  public static Vector3 ToRotaionVector(this Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        return new Vector3(0.0f, 0.0f, -90f);
      case Direction.Up:
        return new Vector3(0.0f, 0.0f, 0.0f);
      case Direction.Left:
        return new Vector3(0.0f, 0.0f, 90f);
      case Direction.Down:
        return new Vector3(0.0f, 0.0f, 180f);
      default:
        return Vector3.zero;
    }
  }

  public static Direction ClockwiseDir(this Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        return Direction.Down;
      case Direction.Up:
        return Direction.Right;
      case Direction.Left:
        return Direction.Up;
      case Direction.Down:
        return Direction.Left;
      default:
        return Direction.None;
    }
  }

  public static Direction Opposite(this Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        return Direction.Left;
      case Direction.Up:
        return Direction.Down;
      case Direction.Left:
        return Direction.Right;
      case Direction.Down:
        return Direction.Up;
      default:
        return Direction.None;
    }
  }

  public static float DistSqrTo(this Vector3 from, Vector3 to)
  {
    from -= to;
    return (float) ((double) from.x * (double) from.x + (double) from.y * (double) from.y);
  }

  public static float DistSqrTo(this Vector3 from, Vector3 to, float scale)
  {
    from = (from - to) / scale;
    return (float) ((double) from.x * (double) from.x + (double) from.y * (double) from.y);
  }

  public static float DistTo(this Vector3 from, Vector3 to) => Mathf.Sqrt(from.DistSqrTo(to));

  public static float GridDistTo(this Vector2 from, Vector2 to, float grid_size = 96f)
  {
    return (from - to).magnitude / grid_size;
  }

  public static Vector2 DirTo(this Transform from_tf, Transform to_tf, float grid_size = 96f)
  {
    return (Object) from_tf == (Object) null || (Object) to_tf == (Object) null ? Vector2.zero : (Vector2) ((to_tf.position - from_tf.position) / grid_size);
  }

  public static Vector2 DirTo(this Vector2 from, Vector2 to, float grid_size = 96f)
  {
    return (to - from) / grid_size;
  }

  public static Vector2 GetRandomNormalizedVec2()
  {
    return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
  }

  public static Vector3 GetRandomNormalizedVec3()
  {
    return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
  }

  public static float Atan2(this Vector2 v) => Mathf.Atan2(v.y, v.x);
}
