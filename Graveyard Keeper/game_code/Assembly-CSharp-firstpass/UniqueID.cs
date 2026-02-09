// Decompiled with JetBrains decompiler
// Type: UniqueID
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public static class UniqueID
{
  public static long _unique_id;

  public static long GetUniqueID()
  {
    long uniqueId = ++UniqueID._unique_id;
    switch (uniqueId)
    {
      case -1:
      case 0:
        uniqueId = UniqueID.GetUniqueID();
        break;
    }
    return uniqueId;
  }

  public static void SetIterator(long n)
  {
    UniqueID._unique_id = n;
    Debug.Log((object) ("Setting UniqueID = " + n.ToString()));
  }
}
