// Decompiled with JetBrains decompiler
// Type: MidasDonation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class MidasDonation
{
  [Key(0)]
  public int Day;
  [Key(1)]
  public int InvestmentAmount;
}
