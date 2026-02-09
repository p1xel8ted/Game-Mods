// Decompiled with JetBrains decompiler
// Type: NotClosingMemoryStream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.IO;

#nullable disable
public sealed class NotClosingMemoryStream : MemoryStream
{
  public MemoryStream stream;
  public bool closed;

  public NotClosingMemoryStream(MemoryStream stream)
  {
    this.stream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
  }

  public Stream BaseStream => (Stream) this.stream;

  public void CheckClosed()
  {
    if (this.closed)
      throw new InvalidOperationException("Wrapper has been closed or disposed");
  }

  public override IAsyncResult BeginRead(
    byte[] buffer,
    int offset,
    int count,
    AsyncCallback callback,
    object state)
  {
    this.CheckClosed();
    return this.stream.BeginRead(buffer, offset, count, callback, state);
  }

  public override IAsyncResult BeginWrite(
    byte[] buffer,
    int offset,
    int count,
    AsyncCallback callback,
    object state)
  {
    this.CheckClosed();
    return this.stream.BeginWrite(buffer, offset, count, callback, state);
  }

  public override bool CanRead => !this.closed && this.stream.CanRead;

  public override bool CanSeek => !this.closed && this.stream.CanSeek;

  public override bool CanWrite => !this.closed && this.stream.CanWrite;

  public override void Close()
  {
    if (this.closed)
      return;
    this.stream.Flush();
  }

  public override int EndRead(IAsyncResult asyncResult)
  {
    this.CheckClosed();
    return this.stream.EndRead(asyncResult);
  }

  public override void EndWrite(IAsyncResult asyncResult)
  {
    this.CheckClosed();
    this.stream.EndWrite(asyncResult);
  }

  public override void Flush()
  {
    this.CheckClosed();
    this.stream.Flush();
  }

  public override object InitializeLifetimeService() => throw new NotSupportedException();

  public override long Length
  {
    get
    {
      this.CheckClosed();
      return this.stream.Length;
    }
  }

  public override long Position
  {
    get
    {
      this.CheckClosed();
      return this.stream.Position;
    }
    set
    {
      this.CheckClosed();
      this.stream.Position = value;
    }
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    this.CheckClosed();
    return this.stream.Read(buffer, offset, count);
  }

  public override int ReadByte()
  {
    this.CheckClosed();
    return this.stream.ReadByte();
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    this.CheckClosed();
    return this.stream.Seek(offset, origin);
  }

  public override void SetLength(long value)
  {
    this.CheckClosed();
    this.stream.SetLength(value);
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    this.CheckClosed();
    this.stream.Write(buffer, offset, count);
  }

  public override void WriteByte(byte value)
  {
    this.CheckClosed();
    this.stream.WriteByte(value);
  }

  public override byte[] GetBuffer() => this.stream.GetBuffer();
}
