// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.UtilStrings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace DarkTonic.MasterAudio;

public static class UtilStrings
{
  public static string TrimSpace(string untrimmed)
  {
    return string.IsNullOrEmpty(untrimmed) ? string.Empty : untrimmed.Trim();
  }

  public static string ReplaceUnsafeChars(string source)
  {
    source = source.Replace("'", "&apos;");
    source = source.Replace("\"", "&quot;");
    source = source.Replace("&", "&amp;");
    return source;
  }
}
