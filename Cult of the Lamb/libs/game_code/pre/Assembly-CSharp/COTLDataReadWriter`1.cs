// Decompiled with JetBrains decompiler
// Type: COTLDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class COTLDataReadWriter<T> : MMDataReadWriterBase<T>
{
  protected MMDataReadWriterBase<T> _readWriter;

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
}
