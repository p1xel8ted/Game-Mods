// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
