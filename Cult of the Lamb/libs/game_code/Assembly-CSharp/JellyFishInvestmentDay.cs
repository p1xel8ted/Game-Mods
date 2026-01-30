// Decompiled with JetBrains decompiler
// Type: JellyFishInvestmentDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class JellyFishInvestmentDay
{
  [Key(0)]
  public int Day;
  [Key(1)]
  public int InvestmentAmount;
  [Key(2)]
  public float InterestRate;
}
