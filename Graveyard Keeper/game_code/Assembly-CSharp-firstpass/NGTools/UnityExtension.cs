// Decompiled with JetBrains decompiler
// Type: NGTools.UnityExtension
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NGTools;

public static class UnityExtension
{
  public static T GetIComponent<T>(this GameObject go) => go.GetComponent<T>();

  public static T[] GetIComponents<T>(this GameObject go) => go.GetComponents<T>();
}
