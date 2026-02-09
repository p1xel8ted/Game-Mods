// Decompiled with JetBrains decompiler
// Type: Pathfinding.FunnelModifier
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Modifiers/Funnel")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_funnel_modifier.php")]
[Serializable]
public class FunnelModifier : MonoModifier
{
  public override int Order => 10;

  public override void Apply(Path p)
  {
    List<GraphNode> path = p.path;
    List<Vector3> vectorPath = p.vectorPath;
    if (path == null || path.Count == 0 || vectorPath == null || vectorPath.Count != path.Count)
      return;
    List<Vector3> funnelPath = ListPool<Vector3>.Claim();
    List<Vector3> vector3List1 = ListPool<Vector3>.Claim(path.Count + 1);
    List<Vector3> vector3List2 = ListPool<Vector3>.Claim(path.Count + 1);
    vector3List1.Add(vectorPath[0]);
    vector3List2.Add(vectorPath[0]);
    for (int index = 0; index < path.Count - 1; ++index)
    {
      if (!path[index].GetPortal(path[index + 1], vector3List1, vector3List2, false))
      {
        vector3List1.Add((Vector3) path[index].position);
        vector3List2.Add((Vector3) path[index].position);
        vector3List1.Add((Vector3) path[index + 1].position);
        vector3List2.Add((Vector3) path[index + 1].position);
      }
    }
    vector3List1.Add(vectorPath[vectorPath.Count - 1]);
    vector3List2.Add(vectorPath[vectorPath.Count - 1]);
    if (!FunnelModifier.RunFunnel(vector3List1, vector3List2, funnelPath))
    {
      funnelPath.Add(vectorPath[0]);
      funnelPath.Add(vectorPath[vectorPath.Count - 1]);
    }
    ListPool<Vector3>.Release(p.vectorPath);
    p.vectorPath = funnelPath;
    ListPool<Vector3>.Release(vector3List1);
    ListPool<Vector3>.Release(vector3List2);
  }

  public static bool RunFunnel(List<Vector3> left, List<Vector3> right, List<Vector3> funnelPath)
  {
    if (left == null)
      throw new ArgumentNullException(nameof (left));
    if (right == null)
      throw new ArgumentNullException(nameof (right));
    if (funnelPath == null)
      throw new ArgumentNullException(nameof (funnelPath));
    if (left.Count != right.Count)
      throw new ArgumentException("left and right lists must have equal length");
    if (left.Count < 3)
      return false;
    while (left[1] == left[2] && right[1] == right[2])
    {
      left.RemoveAt(1);
      right.RemoveAt(1);
      if (left.Count <= 3)
        return false;
    }
    Vector3 p = left[2];
    if (p == left[1])
      p = right[2];
    while (VectorMath.IsColinearXZ(left[0], left[1], right[1]) || VectorMath.RightOrColinearXZ(left[1], right[1], p) == VectorMath.RightOrColinearXZ(left[1], right[1], left[0]))
    {
      left.RemoveAt(1);
      right.RemoveAt(1);
      if (left.Count <= 3)
        return false;
      p = left[2];
      if (p == left[1])
        p = right[2];
    }
    if (!VectorMath.IsClockwiseXZ(left[0], left[1], right[1]) && !VectorMath.IsColinearXZ(left[0], left[1], right[1]))
    {
      List<Vector3> vector3List = left;
      left = right;
      right = vector3List;
    }
    funnelPath.Add(left[0]);
    Vector3 a = left[0];
    Vector3 b1 = left[1];
    Vector3 b2 = right[1];
    int num1 = 1;
    int num2 = 1;
    for (int index = 2; index < left.Count; ++index)
    {
      if (funnelPath.Count > 2000)
      {
        Debug.LogWarning((object) "Avoiding infinite loop. Remove this check if you have this long paths.");
        break;
      }
      Vector3 c1 = left[index];
      Vector3 c2 = right[index];
      if ((double) VectorMath.SignedTriangleAreaTimes2XZ(a, b2, c2) >= 0.0)
      {
        if (a == b2 || (double) VectorMath.SignedTriangleAreaTimes2XZ(a, b1, c2) <= 0.0)
        {
          b2 = c2;
          num1 = index;
        }
        else
        {
          funnelPath.Add(b1);
          a = b1;
          int num3 = num2;
          b1 = a;
          b2 = a;
          num2 = num3;
          num1 = num3;
          index = num3;
          continue;
        }
      }
      if ((double) VectorMath.SignedTriangleAreaTimes2XZ(a, b1, c1) <= 0.0)
      {
        if (a == b1 || (double) VectorMath.SignedTriangleAreaTimes2XZ(a, b2, c1) >= 0.0)
        {
          b1 = c1;
          num2 = index;
        }
        else
        {
          funnelPath.Add(b2);
          a = b2;
          int num4 = num1;
          b1 = a;
          b2 = a;
          num2 = num4;
          num1 = num4;
          index = num4;
        }
      }
    }
    funnelPath.Add(left[left.Count - 1]);
    return true;
  }
}
