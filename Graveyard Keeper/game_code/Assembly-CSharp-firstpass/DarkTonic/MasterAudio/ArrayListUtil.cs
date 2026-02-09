// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.ArrayListUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class ArrayListUtil
{
  public static void SortIntArray(ref List<int> list)
  {
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      int num = list[index1];
      int index2 = Random.Range(index1, list.Count);
      list[index1] = list[index2];
      list[index2] = num;
    }
  }

  public static bool IsExcludedChildName(string name)
  {
    return DarkTonic.MasterAudio.MasterAudio.ExemptChildNames.Contains(name);
  }
}
