// Decompiled with JetBrains decompiler
// Type: MMImageReadWriterBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
public abstract class MMImageReadWriterBase<T>
{
  public Action<T> OnReadCompleted;
  public System.Action OnCreateDefault;
  public System.Action OnWriteCompleted;
  public System.Action OnDeletionComplete;
  public Action<MMReadWriteError> OnReadError;
  public Action<MMReadWriteError> OnWriteError;

  public abstract void Write(T data, string filename);

  public abstract void Read(string filename);

  public abstract void Delete(string filename);

  public abstract bool FileExists(string filename);

  public abstract string[] GetFiles();
}
