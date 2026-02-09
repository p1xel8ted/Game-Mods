// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.Memory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding.Util;

public static class Memory
{
  public static void MemSet(byte[] array, byte value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int val1 = 32 /*0x20*/;
    int dstOffset = 0;
    int num = Math.Min(val1, array.Length);
    while (dstOffset < num)
      array[dstOffset++] = value;
    int length = array.Length;
    while (dstOffset < length)
    {
      Buffer.BlockCopy((Array) array, 0, (Array) array, dstOffset, Math.Min(val1, length - dstOffset));
      dstOffset += val1;
      val1 *= 2;
    }
  }

  public static void MemSet<T>(T[] array, T value, int byteSize) where T : struct
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int val1 = 32 /*0x20*/;
    int index1 = 0;
    for (int index2 = Math.Min(val1, array.Length); index1 < index2; ++index1)
      array[index1] = value;
    int length = array.Length;
    while (index1 < length)
    {
      Buffer.BlockCopy((Array) array, 0, (Array) array, index1 * byteSize, Math.Min(val1, length - index1) * byteSize);
      index1 += val1;
      val1 *= 2;
    }
  }

  public static void MemSet<T>(T[] array, T value, int totalSize, int byteSize) where T : struct
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int val1 = 32 /*0x20*/;
    int index1 = 0;
    for (int index2 = Math.Min(val1, totalSize); index1 < index2; ++index1)
      array[index1] = value;
    int num = totalSize;
    while (index1 < num)
    {
      Buffer.BlockCopy((Array) array, 0, (Array) array, index1 * byteSize, Math.Min(val1, totalSize - index1) * byteSize);
      index1 += val1;
      val1 *= 2;
    }
  }
}
