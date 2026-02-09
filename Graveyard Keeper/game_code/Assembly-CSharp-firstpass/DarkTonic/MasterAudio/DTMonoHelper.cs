// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.DTMonoHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class DTMonoHelper
{
  public static Transform GetChildTransform(this Transform transParent, string childName)
  {
    return transParent.Find(childName);
  }

  public static bool IsActive(GameObject go) => go.activeInHierarchy;

  public static void SetActive(GameObject go, bool isActive) => go.SetActive(isActive);

  public static void DestroyAllChildren(this Transform tran)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < tran.childCount; ++index)
      gameObjectList.Add(tran.GetChild(index).gameObject);
    for (int index = 0; gameObjectList.Count > 0 && index < 200; ++index)
    {
      GameObject gameObject = gameObjectList[0];
      Object.Destroy((Object) gameObject);
      if ((Object) gameObjectList[0] == (Object) gameObject)
        gameObjectList.RemoveAt(0);
    }
  }
}
