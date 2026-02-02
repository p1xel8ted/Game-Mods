// Decompiled with JetBrains decompiler
// Type: JellyFishInvestmentDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
