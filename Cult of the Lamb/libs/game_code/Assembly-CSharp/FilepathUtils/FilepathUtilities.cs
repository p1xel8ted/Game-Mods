// Decompiled with JetBrains decompiler
// Type: FilepathUtils.FilepathUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
