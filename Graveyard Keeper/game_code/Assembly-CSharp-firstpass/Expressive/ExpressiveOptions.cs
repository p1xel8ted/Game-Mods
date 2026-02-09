// Decompiled with JetBrains decompiler
// Type: Expressive.ExpressiveOptions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Expressive;

[Flags]
public enum ExpressiveOptions
{
  None = 1,
  IgnoreCase = 2,
  NoCache = 4,
  RoundAwayFromZero = 8,
  All = RoundAwayFromZero | NoCache | IgnoreCase, // 0x0000000E
}
