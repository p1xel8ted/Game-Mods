// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace I2.Loc;

public class TranslationJob : IDisposable
{
  public TranslationJob.eJobState mJobState;

  public virtual TranslationJob.eJobState GetState() => this.mJobState;

  public virtual void Dispose()
  {
  }

  public enum eJobState
  {
    Running,
    Succeeded,
    Failed,
  }
}
