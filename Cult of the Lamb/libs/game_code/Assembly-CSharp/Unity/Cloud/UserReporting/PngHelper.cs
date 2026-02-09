// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.PngHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Unity.Cloud.UserReporting;

public static class PngHelper
{
  public static int GetPngHeightFromBase64Data(string data)
  {
    if (data == null || data.Length < 32 /*0x20*/)
      return 0;
    byte[] numArray = PngHelper.Slice(Convert.FromBase64String(data.Substring(0, 32 /*0x20*/)), 20, 24);
    Array.Reverse<byte>(numArray);
    return BitConverter.ToInt32(numArray, 0);
  }

  public static int GetPngWidthFromBase64Data(string data)
  {
    if (data == null || data.Length < 32 /*0x20*/)
      return 0;
    byte[] numArray = PngHelper.Slice(Convert.FromBase64String(data.Substring(0, 32 /*0x20*/)), 16 /*0x10*/, 20);
    Array.Reverse<byte>(numArray);
    return BitConverter.ToInt32(numArray, 0);
  }

  public static byte[] Slice(byte[] source, int start, int end)
  {
    if (end < 0)
      end = source.Length + end;
    int length = end - start;
    byte[] numArray = new byte[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = source[index + start];
    return numArray;
  }
}
