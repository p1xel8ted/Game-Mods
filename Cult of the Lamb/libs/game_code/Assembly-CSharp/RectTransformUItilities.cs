// Decompiled with JetBrains decompiler
// Type: RectTransformUItilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class RectTransformUItilities
{
  public static Vector3[] GetWorldCorners(this RectTransform rectTransform)
  {
    Vector3[] fourCornersArray = new Vector3[4];
    rectTransform.GetWorldCorners(fourCornersArray);
    return fourCornersArray;
  }

  public static Bounds GetWorldSpaceBounds(this RectTransform rectTransform)
  {
    Vector3[] worldCorners = rectTransform.GetWorldCorners();
    Bounds worldSpaceBounds = new Bounds(worldCorners[1], Vector3.zero);
    for (int index = 1; index < worldCorners.Length; ++index)
      worldSpaceBounds.Encapsulate(worldCorners[index]);
    return worldSpaceBounds;
  }
}
