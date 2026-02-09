// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.Guid
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Text;

#nullable disable
namespace Pathfinding.Util;

public struct Guid
{
  public const string hex = "0123456789ABCDEF";
  public static Guid zero = new Guid(new byte[16 /*0x10*/]);
  public static string zeroString = new Guid(new byte[16 /*0x10*/]).ToString();
  public ulong _a;
  public ulong _b;
  public static Random random = new Random();
  public static StringBuilder text;

  public Guid(byte[] bytes)
  {
    ulong num1 = (ulong) ((long) bytes[0] | (long) bytes[1] << 8 | (long) bytes[2] << 16 /*0x10*/ | (long) bytes[3] << 24 | (long) bytes[4] << 32 /*0x20*/ | (long) bytes[5] << 40 | (long) bytes[6] << 48 /*0x30*/ | (long) bytes[7] << 56);
    ulong num2 = (ulong) ((long) bytes[8] | (long) bytes[9] << 8 | (long) bytes[10] << 16 /*0x10*/ | (long) bytes[11] << 24 | (long) bytes[12] << 32 /*0x20*/ | (long) bytes[13] << 40 | (long) bytes[14] << 48 /*0x30*/ | (long) bytes[15] << 56);
    this._a = BitConverter.IsLittleEndian ? num1 : Guid.SwapEndianness(num1);
    this._b = BitConverter.IsLittleEndian ? num2 : Guid.SwapEndianness(num2);
  }

  public Guid(string str)
  {
    this._a = 0UL;
    this._b = 0UL;
    if (str.Length < 32 /*0x20*/)
      throw new FormatException("Invalid Guid format");
    int num1 = 0;
    int index = 0;
    int num2 = 60;
    while (num1 < 16 /*0x10*/)
    {
      if (index >= str.Length)
        throw new FormatException("Invalid Guid format. String too short");
      char c = str[index];
      if (c != '-')
      {
        int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
        if (num3 == -1)
          throw new FormatException($"Invalid Guid format : {c.ToString()} is not a hexadecimal character");
        this._a |= (ulong) num3 << num2;
        num2 -= 4;
        ++num1;
      }
      ++index;
    }
    int num4 = 60;
    while (num1 < 32 /*0x20*/)
    {
      if (index >= str.Length)
        throw new FormatException("Invalid Guid format. String too short");
      char c = str[index];
      if (c != '-')
      {
        int num5 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
        if (num5 == -1)
          throw new FormatException($"Invalid Guid format : {c.ToString()} is not a hexadecimal character");
        this._b |= (ulong) num5 << num4;
        num4 -= 4;
        ++num1;
      }
      ++index;
    }
  }

  public static Guid Parse(string input) => new Guid(input);

  public static ulong SwapEndianness(ulong value)
  {
    return (ulong) (((long) value & (long) byte.MaxValue) << 56 | (long) (value >> 8 & (ulong) byte.MaxValue) << 48 /*0x30*/ | (long) (value >> 16 /*0x10*/ & (ulong) byte.MaxValue) << 40 | (long) (value >> 24 & (ulong) byte.MaxValue) << 32 /*0x20*/ | (long) (value >> 32 /*0x20*/ & (ulong) byte.MaxValue) << 24 | (long) (value >> 40 & (ulong) byte.MaxValue) << 16 /*0x10*/ | (long) (value >> 48 /*0x30*/ & (ulong) byte.MaxValue) << 8) | value >> 56 & (ulong) byte.MaxValue;
  }

  public byte[] ToByteArray()
  {
    byte[] byteArray = new byte[16 /*0x10*/];
    byte[] bytes1 = BitConverter.GetBytes(!BitConverter.IsLittleEndian ? Guid.SwapEndianness(this._a) : this._a);
    byte[] bytes2 = BitConverter.GetBytes(!BitConverter.IsLittleEndian ? Guid.SwapEndianness(this._b) : this._b);
    for (int index = 0; index < 8; ++index)
    {
      byteArray[index] = bytes1[index];
      byteArray[index + 8] = bytes2[index];
    }
    return byteArray;
  }

  public static Guid NewGuid()
  {
    byte[] numArray = new byte[16 /*0x10*/];
    Guid.random.NextBytes(numArray);
    return new Guid(numArray);
  }

  public static bool operator ==(Guid lhs, Guid rhs)
  {
    return (long) lhs._a == (long) rhs._a && (long) lhs._b == (long) rhs._b;
  }

  public static bool operator !=(Guid lhs, Guid rhs)
  {
    return (long) lhs._a != (long) rhs._a || (long) lhs._b != (long) rhs._b;
  }

  public override bool Equals(object _rhs)
  {
    return _rhs is Guid guid && (long) this._a == (long) guid._a && (long) this._b == (long) guid._b;
  }

  public override int GetHashCode()
  {
    ulong num = this._a ^ this._b;
    return (int) (num >> 32 /*0x20*/) ^ (int) num;
  }

  public override string ToString()
  {
    if (Guid.text == null)
      Guid.text = new StringBuilder();
    lock (Guid.text)
    {
      Guid.text.Length = 0;
      StringBuilder text = Guid.text;
      ulong num = this._a;
      string str1 = num.ToString("x16");
      StringBuilder stringBuilder = text.Append(str1).Append('-');
      num = this._b;
      string str2 = num.ToString("x16");
      stringBuilder.Append(str2);
      return Guid.text.ToString();
    }
  }
}
