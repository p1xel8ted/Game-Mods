// Decompiled with JetBrains decompiler
// Type: TestPlanesUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unity.Burst;
using UnityEngine;

#nullable disable
public class TestPlanesUtility
{
  [BurstCompile]
  public static TestPlanesUtility.TestPlanesResults TestPlanesAABB(Plane[] planes, Bounds bounds)
  {
    Vector3 min = bounds.min;
    Vector3 max = bounds.max;
    return TestPlanesUtility.TestPlanesAABBInternalFast(planes, ref min, ref max);
  }

  [BurstCompile]
  public static TestPlanesUtility.TestPlanesResults TestPlanesAABBInternalFast(
    Plane[] planes,
    ref Vector3 boundsMin,
    ref Vector3 boundsMax,
    bool testIntersection = false)
  {
    TestPlanesUtility.TestPlanesResults testPlanesResults = TestPlanesUtility.TestPlanesResults.Inside;
    for (int index = 0; index < planes.Length; ++index)
    {
      Vector3 normal = planes[index].normal;
      float distance = planes[index].distance;
      Vector3 vector3_1;
      Vector3 vector3_2;
      if ((double) normal.x < 0.0)
      {
        vector3_1.x = boundsMin.x;
        vector3_2.x = boundsMax.x;
      }
      else
      {
        vector3_1.x = boundsMax.x;
        vector3_2.x = boundsMin.x;
      }
      if ((double) normal.y < 0.0)
      {
        vector3_1.y = boundsMin.y;
        vector3_2.y = boundsMax.y;
      }
      else
      {
        vector3_1.y = boundsMax.y;
        vector3_2.y = boundsMin.y;
      }
      if ((double) normal.z < 0.0)
      {
        vector3_1.z = boundsMin.z;
        vector3_2.z = boundsMax.z;
      }
      else
      {
        vector3_1.z = boundsMax.z;
        vector3_2.z = boundsMin.z;
      }
      if ((double) normal.x * (double) vector3_1.x + (double) normal.y * (double) vector3_1.y + (double) normal.z * (double) vector3_1.z + (double) distance < 0.0)
        return TestPlanesUtility.TestPlanesResults.Outside;
      if (testIntersection && (double) normal.x * (double) vector3_2.x + (double) normal.y * (double) vector3_2.y + (double) normal.z * (double) vector3_2.z + (double) distance <= 0.0)
        testPlanesResults = TestPlanesUtility.TestPlanesResults.Intersect;
    }
    return testPlanesResults;
  }

  public enum TestPlanesResults
  {
    Inside,
    Intersect,
    Outside,
  }
}
