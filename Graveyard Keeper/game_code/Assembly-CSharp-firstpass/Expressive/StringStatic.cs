// Decompiled with JetBrains decompiler
// Type: Expressive.StringStatic
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Expressive;

public static class StringStatic
{
  public static bool IsNullOrWhiteSpace(this string value)
  {
    return value == null || string.IsNullOrEmpty(value.Trim());
  }
}
