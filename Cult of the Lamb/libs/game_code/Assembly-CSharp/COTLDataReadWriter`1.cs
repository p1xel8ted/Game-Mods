// Decompiled with JetBrains decompiler
// Type: COTLDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
public class COTLDataReadWriter<T> : MMDataReadWriterBase<T>
{
  public MMDataReadWriterBase<T> _readWriter;

  public COTLDataReadWriter()
  {
    this._readWriter = (MMDataReadWriterBase<T>) new MMJsonDataReadWriter<T>();
    this._readWriter.OnWriteCompleted += (System.Action) (() =>
    {
      System.Action onWriteCompleted = this.OnWriteCompleted;
      if (onWriteCompleted == null)
        return;
      onWriteCompleted();
    });
    this._readWriter.OnReadCompleted += (Action<T>) (data =>
    {
      Action<T> onReadCompleted = this.OnReadCompleted;
      if (onReadCompleted == null)
        return;
      onReadCompleted(data);
    });
    this._readWriter.OnCreateDefault += (System.Action) (() =>
    {
      System.Action onCreateDefault = this.OnCreateDefault;
      if (onCreateDefault == null)
        return;
      onCreateDefault();
    });
    this._readWriter.OnDeletionComplete += (System.Action) (() =>
    {
      System.Action deletionComplete = this.OnDeletionComplete;
      if (deletionComplete == null)
        return;
      deletionComplete();
    });
    this._readWriter.OnReadError += (Action<MMReadWriteError>) (error =>
    {
      Action<MMReadWriteError> onReadError = this.OnReadError;
      if (onReadError == null)
        return;
      onReadError(error);
    });
    this._readWriter.OnWriteError += (Action<MMReadWriteError>) (error =>
    {
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError == null)
        return;
      onWriteError(error);
    });
  }

  public override void Write(T data, string filename, bool encrypt = true, bool backup = true)
  {
    this._readWriter.Write(data, filename, encrypt, backup);
  }

  public override void Read(string filename) => this._readWriter.Read(filename);

  public override void Delete(string filename) => this._readWriter.Delete(filename);

  public override bool FileExists(string filename) => this._readWriter.FileExists(filename);

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__1_0()
  {
    System.Action onWriteCompleted = this.OnWriteCompleted;
    if (onWriteCompleted == null)
      return;
    onWriteCompleted();
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__1_1(T data)
  {
    Action<T> onReadCompleted = this.OnReadCompleted;
    if (onReadCompleted == null)
      return;
    onReadCompleted(data);
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__1_2()
  {
    System.Action onCreateDefault = this.OnCreateDefault;
    if (onCreateDefault == null)
      return;
    onCreateDefault();
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__1_3()
  {
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete == null)
      return;
    deletionComplete();
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__1_4(MMReadWriteError error)
  {
    Action<MMReadWriteError> onReadError = this.OnReadError;
    if (onReadError == null)
      return;
    onReadError(error);
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__1_5(MMReadWriteError error)
  {
    Action<MMReadWriteError> onWriteError = this.OnWriteError;
    if (onWriteError == null)
      return;
    onWriteError(error);
  }
}
