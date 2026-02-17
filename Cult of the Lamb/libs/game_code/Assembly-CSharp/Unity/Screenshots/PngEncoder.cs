// Decompiled with JetBrains decompiler
// Type: Unity.Screenshots.PngEncoder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace Unity.Screenshots;

public static class PngEncoder
{
  public static PngEncoder.Crc32 crc32 = new PngEncoder.Crc32();

  public static uint Adler32(byte[] bytes)
  {
    uint num1 = 1;
    uint num2 = 0;
    foreach (byte num3 in bytes)
    {
      num1 = (num1 + (uint) num3) % 65521U;
      num2 = (num2 + num1) % 65521U;
    }
    return num2 << 16 /*0x10*/ | num1;
  }

  public static void AppendByte(this byte[] data, ref int position, byte value)
  {
    data[position] = value;
    ++position;
  }

  public static void AppendBytes(this byte[] data, ref int position, byte[] value)
  {
    foreach (byte num in value)
      data.AppendByte(ref position, num);
  }

  public static void AppendChunk(
    this byte[] data,
    ref int position,
    string chunkType,
    byte[] chunkData)
  {
    byte[] chunkTypeBytes = PngEncoder.GetChunkTypeBytes(chunkType);
    if (chunkTypeBytes == null)
      return;
    data.AppendInt(ref position, chunkData.Length);
    data.AppendBytes(ref position, chunkTypeBytes);
    data.AppendBytes(ref position, chunkData);
    data.AppendUInt(ref position, PngEncoder.GetChunkCrc(chunkTypeBytes, chunkData));
  }

  public static void AppendInt(this byte[] data, ref int position, int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    if (BitConverter.IsLittleEndian)
      Array.Reverse<byte>(bytes);
    data.AppendBytes(ref position, bytes);
  }

  public static void AppendUInt(this byte[] data, ref int position, uint value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    if (BitConverter.IsLittleEndian)
      Array.Reverse<byte>(bytes);
    data.AppendBytes(ref position, bytes);
  }

  public static byte[] Compress(byte[] bytes)
  {
    using (MemoryStream memoryStream1 = new MemoryStream())
    {
      using (DeflateStream deflateStream = new DeflateStream((Stream) memoryStream1, CompressionMode.Compress))
      {
        using (MemoryStream memoryStream2 = new MemoryStream(bytes))
          memoryStream2.WriteTo((Stream) deflateStream);
      }
      return memoryStream1.ToArray();
    }
  }

  public static byte[] Encode(byte[] dataRgba, int stride)
  {
    if (dataRgba == null)
      throw new ArgumentNullException(nameof (dataRgba));
    if (dataRgba.Length == 0)
      throw new ArgumentException("The data length must be greater than 0.");
    if (stride == 0)
      throw new ArgumentException("The stride must be greater than 0.");
    if (stride % 4 != 0)
      throw new ArgumentException("The stride must be evenly divisible by 4.");
    if (dataRgba.Length % 4 != 0)
      throw new ArgumentException("The data must be evenly divisible by 4.");
    if (dataRgba.Length % stride != 0)
      throw new ArgumentException("The data must be evenly divisible by the stride.");
    int num1 = dataRgba.Length / 4;
    int num2 = stride / 4;
    int num3 = num2;
    int num4 = num1 / num3;
    byte[] numArray1 = new byte[13];
    int position1 = 0;
    numArray1.AppendInt(ref position1, num2);
    numArray1.AppendInt(ref position1, num4);
    numArray1.AppendByte(ref position1, (byte) 8);
    numArray1.AppendByte(ref position1, (byte) 6);
    numArray1.AppendByte(ref position1, (byte) 0);
    numArray1.AppendByte(ref position1, (byte) 0);
    numArray1.AppendByte(ref position1, (byte) 0);
    byte[] numArray2 = new byte[dataRgba.Length + num4];
    int position2 = 0;
    int num5 = 0;
    for (int index = 0; index < dataRgba.Length; ++index)
    {
      if (num5 >= stride)
        num5 = 0;
      if (num5 == 0)
        numArray2.AppendByte(ref position2, (byte) 0);
      numArray2.AppendByte(ref position2, dataRgba[index]);
      ++num5;
    }
    byte[] numArray3 = PngEncoder.Compress(numArray2);
    byte[] numArray4 = new byte[2 + numArray3.Length + 4];
    int position3 = 0;
    numArray4.AppendByte(ref position3, (byte) 120);
    numArray4.AppendByte(ref position3, (byte) 156);
    numArray4.AppendBytes(ref position3, numArray3);
    numArray4.AppendUInt(ref position3, PngEncoder.Adler32(numArray2));
    byte[] data = new byte[8 + numArray1.Length + 12 + numArray4.Length + 12 + 12];
    int position4 = 0;
    data.AppendByte(ref position4, (byte) 137);
    data.AppendByte(ref position4, (byte) 80 /*0x50*/);
    data.AppendByte(ref position4, (byte) 78);
    data.AppendByte(ref position4, (byte) 71);
    data.AppendByte(ref position4, (byte) 13);
    data.AppendByte(ref position4, (byte) 10);
    data.AppendByte(ref position4, (byte) 26);
    data.AppendByte(ref position4, (byte) 10);
    data.AppendChunk(ref position4, "IHDR", numArray1);
    data.AppendChunk(ref position4, "IDAT", numArray4);
    data.AppendChunk(ref position4, "IEND", new byte[0]);
    return data;
  }

  public static void EncodeAsync(byte[] dataRgba, int stride, Action<Exception, byte[]> callback)
  {
    ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
    {
      try
      {
        callback((Exception) null, PngEncoder.Encode(dataRgba, stride));
      }
      catch (Exception ex)
      {
        callback(ex, (byte[]) null);
        throw;
      }
    }), (object) null);
  }

  public static uint GetChunkCrc(byte[] chunkTypeBytes, byte[] chunkData)
  {
    byte[] numArray = new byte[chunkTypeBytes.Length + chunkData.Length];
    Array.Copy((Array) chunkTypeBytes, 0, (Array) numArray, 0, chunkTypeBytes.Length);
    Array.Copy((Array) chunkData, 0, (Array) numArray, 4, chunkData.Length);
    return PngEncoder.crc32.Calculate<byte>((IEnumerable<byte>) numArray);
  }

  public static byte[] GetChunkTypeBytes(string value)
  {
    char[] charArray = value.ToCharArray();
    if (charArray.Length < 4)
      return (byte[]) null;
    byte[] chunkTypeBytes = new byte[4];
    for (int index = 0; index < chunkTypeBytes.Length; ++index)
      chunkTypeBytes[index] = (byte) charArray[index];
    return chunkTypeBytes;
  }

  public class Crc32
  {
    public static uint generator = 3988292384;
    public uint[] checksumTable;

    public Crc32()
    {
      this.checksumTable = Enumerable.Range(0, 256 /*0x0100*/).Select<int, uint>((Func<int, uint>) (i =>
      {
        uint num = (uint) i;
        for (int index = 0; index < 8; ++index)
          num = ((int) num & 1) != 0 ? PngEncoder.Crc32.generator ^ num >> 1 : num >> 1;
        return num;
      })).ToArray<uint>();
    }

    public uint Calculate<T>(IEnumerable<T> byteStream)
    {
      return ~byteStream.Aggregate<T, uint>(uint.MaxValue, (Func<uint, T, uint>) ((checksumRegister, currentByte) => this.checksumTable[(int) checksumRegister & (int) byte.MaxValue ^ (int) Convert.ToByte((object) currentByte)] ^ checksumRegister >> 8));
    }

    [CompilerGenerated]
    public uint \u003CCalculate\u003Eb__3_0<T>(uint checksumRegister, T currentByte)
    {
      return this.checksumTable[(int) checksumRegister & (int) byte.MaxValue ^ (int) Convert.ToByte((object) currentByte)] ^ checksumRegister >> 8;
    }
  }
}
