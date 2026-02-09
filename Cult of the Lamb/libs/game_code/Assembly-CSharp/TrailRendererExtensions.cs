// Decompiled with JetBrains decompiler
// Type: TrailRendererExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class TrailRendererExtensions
{
  public static float GetTrailLength(this TrailRenderer trailRenderer)
  {
    Vector3[] positions1 = new Vector3[trailRenderer.positionCount];
    int positions2 = trailRenderer.GetPositions(positions1);
    if (positions2 < 2)
      return 0.0f;
    float trailLength = 0.0f;
    Vector3 a = positions1[0];
    for (int index = 1; index < positions2; ++index)
    {
      Vector3 b = positions1[index];
      trailLength += Vector3.Distance(a, b);
      a = b;
    }
    return trailLength;
  }
}
