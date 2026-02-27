// Decompiled with JetBrains decompiler
// Type: TransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
