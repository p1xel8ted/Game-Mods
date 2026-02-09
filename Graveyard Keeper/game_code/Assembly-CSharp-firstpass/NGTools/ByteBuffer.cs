// Decompiled with JetBrains decompiler
// Type: NGTools.ByteBuffer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace NGTools;

public class ByteBuffer
{
  public ByteBuffer.ResizeMode resizeMode;
  public bool writable;
  public int length;
  [CompilerGenerated]
  public int \u003CPosition\u003Ek__BackingField;
  public byte[] buffer;

  public int Capacity => this.buffer.Length;

  public int Length
  {
    get => this.length;
    set
    {
      if (!this.writable)
        throw new InvalidOperationException("Buffer is unwritable.");
      this.length = value;
    }
  }

  public int Position
  {
    get => this.\u003CPosition\u003Ek__BackingField;
    set => this.\u003CPosition\u003Ek__BackingField = value;
  }

  public ByteBuffer(int capacity)
  {
    this.resizeMode = ByteBuffer.ResizeMode.Double;
    this.buffer = new byte[capacity];
    this.writable = true;
  }

  public ByteBuffer(int capacity, ByteBuffer.ResizeMode mode)
  {
    this.resizeMode = mode;
    this.buffer = new byte[capacity];
    this.writable = true;
  }

  public ByteBuffer(int capacity, bool writable)
  {
    this.resizeMode = ByteBuffer.ResizeMode.Double;
    this.buffer = new byte[capacity];
    this.writable = writable;
  }

  public ByteBuffer(int capacity, ByteBuffer.ResizeMode mode, bool writable)
  {
    this.resizeMode = mode;
    this.buffer = new byte[capacity];
    this.writable = writable;
  }

  public ByteBuffer(byte[] buffer)
  {
    this.buffer = (byte[]) buffer.Clone();
    this.length = this.buffer.Length;
    this.writable = false;
  }

  public ByteBuffer(byte[] buffer, bool writable)
  {
    this.buffer = (byte[]) buffer.Clone();
    this.length = this.buffer.Length;
    this.writable = writable;
  }

  public void Resize(int newSize) => this.Resize(newSize, false);

  public void Resize(int newSize, bool force)
  {
    if (!this.writable && !force)
      return;
    switch (this.resizeMode)
    {
      case ByteBuffer.ResizeMode.Strict:
        while (newSize > this.Length)
        {
          byte[] dst = new byte[newSize];
          if (this.Length > 0)
            Buffer.BlockCopy((Array) this.buffer, 0, (Array) dst, 0, this.Length);
          this.buffer = dst;
        }
        break;
      case ByteBuffer.ResizeMode.Double:
        int length = this.buffer.Length << 1;
        while (newSize > length)
          length <<= 1;
        byte[] dst1 = new byte[length];
        if (this.Length > 0)
          Buffer.BlockCopy((Array) this.buffer, 0, (Array) dst1, 0, this.Length);
        this.buffer = dst1;
        break;
    }
  }

  public void AppendUnicodeString(string content)
  {
    if (!this.writable)
      return;
    if (string.IsNullOrEmpty(content))
    {
      this.Append(0);
    }
    else
    {
      byte[] bytes = Encoding.UTF8.GetBytes(content);
      this.Append(bytes.Length);
      this.Append((Array) bytes);
    }
  }

  public void Append(ByteBuffer src)
  {
    if (!this.writable)
      return;
    if (this.Length + src.Length > this.buffer.Length)
      this.Resize(this.Length + src.Length);
    Buffer.BlockCopy((Array) src.buffer, src.Position, (Array) this.buffer, this.Length, src.Length);
    this.Length += src.Length;
  }

  public void Append(byte[] src, int position, int length)
  {
    if (!this.writable)
      return;
    if (this.Length + length > this.buffer.Length)
      this.Resize(this.Length + length);
    Buffer.BlockCopy((Array) src, position, (Array) this.buffer, this.Length, length);
    this.Length += length;
  }

  public void Append(Array src)
  {
    if (!this.writable)
      return;
    if (this.Length + src.Length > this.buffer.Length)
      this.Resize(this.Length + src.Length);
    Buffer.BlockCopy(src, 0, (Array) this.buffer, this.Length, src.Length);
    this.Length += src.Length;
  }

  public void Append(bool value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(byte value)
  {
    if (!this.writable)
      return;
    if (this.Length + 1 > this.buffer.Length)
      this.Resize(this.Length + 1);
    this.buffer[this.Length] = value;
    ++this.Length;
  }

  public void Append(sbyte value) => this.Append((Array) BitConverter.GetBytes((short) value));

  public void Append(char value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(float value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(double value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(short value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(int value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(long value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(ushort value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(uint value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(ulong value) => this.Append((Array) BitConverter.GetBytes(value));

  public void Append(string src) => this.Append((Array) Encoding.UTF8.GetBytes(src));

  public short ReadInt16()
  {
    int num = this.Position + 2 <= this.Length ? (int) BitConverter.ToInt16(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({2.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 2;
    return (short) num;
  }

  public int ReadInt32()
  {
    int num = this.Position + 4 <= this.Length ? BitConverter.ToInt32(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({4.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 4;
    return num;
  }

  public long ReadInt64()
  {
    long num = this.Position + 8 <= this.Length ? BitConverter.ToInt64(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({8.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 8;
    return num;
  }

  public ushort ReadUInt16()
  {
    int num = this.Position + 2 <= this.Length ? (int) BitConverter.ToUInt16(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({2.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 2;
    return (ushort) num;
  }

  public uint ReadUInt32()
  {
    int num = this.Position + 4 <= this.Length ? (int) BitConverter.ToUInt32(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({4.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 4;
    return (uint) num;
  }

  public ulong ReadUInt64()
  {
    long num = this.Position + 8 <= this.Length ? (long) BitConverter.ToUInt64(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({8.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 8;
    return (ulong) num;
  }

  public float ReadSingle()
  {
    double num = this.Position + 4 <= this.Length ? (double) BitConverter.ToSingle(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({2.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 4;
    return (float) num;
  }

  public double ReadDouble()
  {
    double num = this.Position + 8 <= this.Length ? BitConverter.ToDouble(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({8.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 8;
    return num;
  }

  public byte ReadByte()
  {
    if (this.Position + 1 > this.Length)
      throw new OverflowException($"Unsufficient bytes ({1.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    ++this.Position;
    return this.buffer[this.Position - 1];
  }

  public sbyte ReadSByte()
  {
    if (this.Position + 1 > this.Length)
      throw new OverflowException($"Unsufficient bytes ({1.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    ++this.Position;
    return (sbyte) this.buffer[this.Position - 1];
  }

  public bool ReadBoolean()
  {
    if (this.Position + 1 > this.Length)
      throw new OverflowException($"Unsufficient bytes ({1.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    int num = BitConverter.ToBoolean(this.buffer, this.Position) ? 1 : 0;
    ++this.Position;
    return num != 0;
  }

  public char ReadChar()
  {
    int num = this.Position + 2 <= this.Length ? (int) BitConverter.ToChar(this.buffer, this.Position) : throw new OverflowException($"Unsufficient bytes ({2.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    this.Position += 2;
    return (char) num;
  }

  public string ReadString(int length)
  {
    if (this.Position + length > this.Length)
      throw new OverflowException($"Unsufficient bytes ({length.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    string str = Encoding.UTF8.GetString(this.buffer, this.Position, length);
    this.Position += length;
    return str;
  }

  public string ReadUnicodeString()
  {
    int length = this.ReadInt32();
    return length > 0 ? Encoding.UTF8.GetString(this.ReadBytes(length)) : string.Empty;
  }

  public byte[] ReadBytes(int length)
  {
    if (this.Position + length > this.Length)
      throw new OverflowException($"Unsufficient bytes ({length.ToString()} bytes) in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    byte[] dst = new byte[length];
    Buffer.BlockCopy((Array) this.buffer, this.Position, (Array) dst, 0, length);
    this.Position += length;
    return dst;
  }

  public void Clear()
  {
    this.Length = 0;
    this.Position = 0;
  }

  public byte[] Flush()
  {
    byte[] buffer = this.GetBuffer();
    this.Clear();
    return buffer;
  }

  public byte[] GetRawBuffer() => this.buffer;

  public byte[] GetBuffer()
  {
    byte[] dst = new byte[this.Length];
    Buffer.BlockCopy((Array) this.buffer, 0, (Array) dst, 0, this.Length);
    return dst;
  }

  public void CopyBuffer(ByteBuffer destination, int length)
  {
    if (this.Position + length > this.Length)
      throw new OverflowException($"Unsufficient bytes in buffer of {this.Length.ToString()} at {this.Position.ToString()}.");
    if (destination.buffer.Length < length)
      destination.Resize(length, true);
    Buffer.BlockCopy((Array) this.buffer, this.Position, (Array) destination.buffer, 0, length);
    destination.length = length;
    destination.Position = 0;
  }

  public void CopyBuffer(ByteBuffer destination, int position, int length)
  {
    if (position + length > this.Length)
    {
      string[] strArray = new string[5]
      {
        "Unsufficient bytes in buffer of ",
        null,
        null,
        null,
        null
      };
      int num = this.Length;
      strArray[1] = num.ToString();
      strArray[2] = " at ";
      num = this.Position;
      strArray[3] = num.ToString();
      strArray[4] = ".";
      throw new OverflowException(string.Concat(strArray));
    }
    if (destination.buffer.Length < length)
      destination.Resize(length, true);
    Buffer.BlockCopy((Array) this.buffer, position, (Array) destination.buffer, 0, length);
    destination.length = length;
    destination.Position = 0;
  }

  public enum ResizeMode
  {
    Strict,
    Double,
  }
}
