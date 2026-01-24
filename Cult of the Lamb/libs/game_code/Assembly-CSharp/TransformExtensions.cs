// Decompiled with JetBrains decompiler
// Type: TransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class TransformExtensions
{
  public static bool IsChildOf(this Transform transform, Transform possibleParent)
  {
    Transform parent = transform.parent;
    while ((Object) parent != (Object) null)
    {
      parent = parent.parent;
      if ((Object) parent == (Object) possibleParent)
        return true;
    }
    return false;
  }

  public static void DestroyAllChildren(this Transform transform)
  {
    int childCount = transform.childCount;
    while (--childCount >= 0)
    {
      if (Application.isPlaying)
        Object.Destroy((Object) transform.GetChild(childCount).gameObject);
      else
        Object.DestroyImmediate((Object) transform.GetChild(childCount).gameObject);
    }
  }
}
