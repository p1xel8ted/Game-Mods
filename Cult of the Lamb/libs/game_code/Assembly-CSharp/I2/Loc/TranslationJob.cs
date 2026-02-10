// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
