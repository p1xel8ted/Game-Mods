// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOLayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding.RVO;

[Flags]
public enum RVOLayer
{
  DefaultAgent = 1,
  DefaultObstacle = 2,
  Layer2 = 4,
  Layer3 = 8,
  Layer4 = 16, // 0x00000010
  Layer5 = 32, // 0x00000020
  Layer6 = 64, // 0x00000040
  Layer7 = 128, // 0x00000080
  Layer8 = 256, // 0x00000100
  Layer9 = 512, // 0x00000200
  Layer10 = 1024, // 0x00000400
  Layer11 = 2048, // 0x00000800
  Layer12 = 4096, // 0x00001000
  Layer13 = 8192, // 0x00002000
  Layer14 = 16384, // 0x00004000
  Layer15 = 32768, // 0x00008000
  Layer16 = 65536, // 0x00010000
  Layer17 = 131072, // 0x00020000
  Layer18 = 262144, // 0x00040000
  Layer19 = 524288, // 0x00080000
  Layer20 = 1048576, // 0x00100000
  Layer21 = 2097152, // 0x00200000
  Layer22 = 4194304, // 0x00400000
  Layer23 = 8388608, // 0x00800000
  Layer24 = 16777216, // 0x01000000
  Layer25 = 33554432, // 0x02000000
  Layer26 = 67108864, // 0x04000000
  Layer27 = 134217728, // 0x08000000
  Layer28 = 268435456, // 0x10000000
  Layer29 = 536870912, // 0x20000000
  Layer30 = 1073741824, // 0x40000000
}
