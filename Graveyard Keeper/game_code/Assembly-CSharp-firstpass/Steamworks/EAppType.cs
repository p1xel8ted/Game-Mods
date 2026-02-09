// Decompiled with JetBrains decompiler
// Type: Steamworks.EAppType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum EAppType
{
  k_EAppType_Invalid = 0,
  k_EAppType_Game = 1,
  k_EAppType_Application = 2,
  k_EAppType_Tool = 4,
  k_EAppType_Demo = 8,
  k_EAppType_Media_DEPRECATED = 16, // 0x00000010
  k_EAppType_DLC = 32, // 0x00000020
  k_EAppType_Guide = 64, // 0x00000040
  k_EAppType_Driver = 128, // 0x00000080
  k_EAppType_Config = 256, // 0x00000100
  k_EAppType_Hardware = 512, // 0x00000200
  k_EAppType_Franchise = 1024, // 0x00000400
  k_EAppType_Video = 2048, // 0x00000800
  k_EAppType_Plugin = 4096, // 0x00001000
  k_EAppType_Music = 8192, // 0x00002000
  k_EAppType_Series = 16384, // 0x00004000
  k_EAppType_Comic = 32768, // 0x00008000
  k_EAppType_Shortcut = 1073741824, // 0x40000000
  k_EAppType_DepotOnly = -2147483647, // 0x80000001
}
