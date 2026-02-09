// Decompiled with JetBrains decompiler
// Type: Expressive.Helpers.ReflectionTools
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Expressive.Helpers;

public static class ReflectionTools
{
  public static TypeCode GetTypeCode(object value) => Type.GetTypeCode(value.GetType());
}
