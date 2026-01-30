// Decompiled with JetBrains decompiler
// Type: JellyFishInvestment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class JellyFishInvestment
{
  [Key(0)]
  public int InitialInvestment;
  [Key(1)]
  public int ActualInvestedAmount;
  [Key(2)]
  public int NewInvestment;
  [Key(3)]
  public int InvestmentDay;
  [Key(4)]
  public List<JellyFishInvestmentDay> InvestmentDays = new List<JellyFishInvestmentDay>();
  [Key(5)]
  public int LastDayCheckedInvestment = -1;
}
