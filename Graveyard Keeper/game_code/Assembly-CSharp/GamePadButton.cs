// Decompiled with JetBrains decompiler
// Type: GamePadButton
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Flags]
public enum GamePadButton
{
  None = 0,
  B = 1,
  A = 2,
  X = 4,
  Y = 8,
  Left = 16, // 0x00000010
  Right = 32, // 0x00000020
  Up = 64, // 0x00000040
  Down = 128, // 0x00000080
  LB = 256, // 0x00000100
  RB = 512, // 0x00000200
  Back = 1024, // 0x00000400
  Start = 2048, // 0x00000800
  DUp = 4096, // 0x00001000
  DDown = 8192, // 0x00002000
  DLeft = 16384, // 0x00004000
  DRight = 32768, // 0x00008000
  LT = 65536, // 0x00010000
  RT = 131072, // 0x00020000
}
