// Decompiled with JetBrains decompiler
// Type: Compression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.IO;
using System.IO.Compression;

#nullable disable
public class Compression
{
  public static byte[] Compress(byte[] data)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
      {
        gzipStream.Write(data, 0, data.Length);
        gzipStream.Close();
        return memoryStream.ToArray();
      }
    }
  }
}
