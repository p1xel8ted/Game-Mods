// Decompiled with JetBrains decompiler
// Type: JellyFishInvestment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class JellyFishInvestment
{
  public int InitialInvestment;
  public int ActualInvestedAmount;
  public int NewInvestment;
  public int InvestmentDay;
  public List<JellyFishInvestmentDay> InvestmentDays = new List<JellyFishInvestmentDay>();
  public int LastDayCheckedInvestment = -1;
}
