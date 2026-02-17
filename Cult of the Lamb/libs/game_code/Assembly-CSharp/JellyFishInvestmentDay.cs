// Decompiled with JetBrains decompiler
// Type: JellyFishInvestmentDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
