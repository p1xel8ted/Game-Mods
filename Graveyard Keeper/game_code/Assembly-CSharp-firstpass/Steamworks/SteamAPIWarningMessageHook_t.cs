// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPIWarningMessageHook_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Steamworks;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
