// Decompiled with JetBrains decompiler
// Type: FilepathUtils.FilepathUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
