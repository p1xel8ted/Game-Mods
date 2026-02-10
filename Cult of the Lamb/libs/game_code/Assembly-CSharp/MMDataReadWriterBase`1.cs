// Decompiled with JetBrains decompiler
// Type: MMDataReadWriterBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
public abstract class MMDataReadWriterBase<T>
{
  public Action<T> OnReadCompleted;
  public System.Action OnCreateDefault;
  public System.Action OnWriteCompleted;
  public System.Action OnDeletionComplete;
  public Action<MMReadWriteError> OnReadError;
  public Action<MMReadWriteError> OnWriteError;

  public abstract void Write(T data, string filename, bool encrypt = true, bool backup = true);

  public abstract void Read(string filename);

  public abstract void Delete(string filename);

  public abstract bool FileExists(string filename);

  public bool SavesAsJson(string filename)
  {
    return filename.StartsWith("settings") || filename.StartsWith("persistence") || filename == "Prefs.dict" || filename == "ScreenShots";
  }
}
