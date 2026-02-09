// Decompiled with JetBrains decompiler
// Type: JobDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class JobDefinition : BalanceBaseObject
{
  public int time_len;
  public int real_time;
  public JobDefinition.JobType type;

  public enum JobType
  {
    DigGrave = 1,
  }
}
