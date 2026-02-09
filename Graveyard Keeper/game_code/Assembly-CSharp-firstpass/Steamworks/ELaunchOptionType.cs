// Decompiled with JetBrains decompiler
// Type: Steamworks.ELaunchOptionType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum ELaunchOptionType
{
  k_ELaunchOptionType_None = 0,
  k_ELaunchOptionType_Default = 1,
  k_ELaunchOptionType_SafeMode = 2,
  k_ELaunchOptionType_Multiplayer = 3,
  k_ELaunchOptionType_Config = 4,
  k_ELaunchOptionType_OpenVR = 5,
  k_ELaunchOptionType_Server = 6,
  k_ELaunchOptionType_Editor = 7,
  k_ELaunchOptionType_Manual = 8,
  k_ELaunchOptionType_Benchmark = 9,
  k_ELaunchOptionType_Option1 = 10, // 0x0000000A
  k_ELaunchOptionType_Option2 = 11, // 0x0000000B
  k_ELaunchOptionType_Option3 = 12, // 0x0000000C
  k_ELaunchOptionType_OculusVR = 13, // 0x0000000D
  k_ELaunchOptionType_OpenVROverlay = 14, // 0x0000000E
  k_ELaunchOptionType_OSVR = 15, // 0x0000000F
  k_ELaunchOptionType_Dialog = 1000, // 0x000003E8
}
