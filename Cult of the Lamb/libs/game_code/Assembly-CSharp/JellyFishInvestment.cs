// Decompiled with JetBrains decompiler
// Type: JellyFishInvestment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
