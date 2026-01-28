// Decompiled with JetBrains decompiler
// Type: JellyFishInvestmentDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
