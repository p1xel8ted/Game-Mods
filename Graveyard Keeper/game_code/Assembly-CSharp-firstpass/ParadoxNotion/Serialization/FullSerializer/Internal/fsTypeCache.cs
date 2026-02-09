// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsTypeCache
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public static class fsTypeCache
{
  public static Type GetType(string name) => fsTypeCache.GetType(name, false, (Type) null);

  public static Type GetType(string name, Type fallbackAssignable)
  {
    return fsTypeCache.GetType(name, true, fallbackAssignable);
  }

  public static Type GetType(string name, bool fallbackNoNamespace, Type fallbackAssignable)
  {
    return ReflectionTools.GetType(name, fallbackNoNamespace, fallbackAssignable);
  }
}
