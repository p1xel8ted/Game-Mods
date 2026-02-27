// Decompiled with JetBrains decompiler
// Type: FilepathUtils.FilepathUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.IO;

#nullable disable
namespace FilepathUtils;

public static class FilepathUtilities
{
  public static string NormalizePath(this string path)
  {
    return Path.DirectorySeparatorChar == '\\' ? path.Replace('/', Path.DirectorySeparatorChar) : path.Replace('\\', Path.DirectorySeparatorChar);
  }
}
