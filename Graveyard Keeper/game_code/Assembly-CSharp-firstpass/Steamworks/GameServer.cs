// Decompiled with JetBrains decompiler
// Type: Steamworks.GameServer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public static class GameServer
{
  public static bool Init(
    uint unIP,
    ushort usSteamPort,
    ushort usGamePort,
    ushort usQueryPort,
    EServerMode eServerMode,
    string pchVersionString)
  {
    InteropHelp.TestIfPlatformSupported();
    bool flag;
    using (InteropHelp.UTF8StringHandle pchVersionString1 = new InteropHelp.UTF8StringHandle(pchVersionString))
      flag = NativeMethods.SteamGameServer_Init(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, pchVersionString1);
    if (flag)
      flag = CSteamGameServerAPIContext.Init();
    return flag;
  }

  public static void Shutdown()
  {
    InteropHelp.TestIfPlatformSupported();
    NativeMethods.SteamGameServer_Shutdown();
    CSteamGameServerAPIContext.Clear();
  }

  public static void RunCallbacks()
  {
    InteropHelp.TestIfPlatformSupported();
    NativeMethods.SteamGameServer_RunCallbacks();
  }

  public static void ReleaseCurrentThreadMemory()
  {
    InteropHelp.TestIfPlatformSupported();
    NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
  }

  public static bool BSecure()
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamGameServer_BSecure();
  }

  public static CSteamID GetSteamID()
  {
    InteropHelp.TestIfPlatformSupported();
    return (CSteamID) NativeMethods.SteamGameServer_GetSteamID();
  }

  public static HSteamPipe GetHSteamPipe()
  {
    InteropHelp.TestIfPlatformSupported();
    return (HSteamPipe) NativeMethods.SteamGameServer_GetHSteamPipe();
  }

  public static HSteamUser GetHSteamUser()
  {
    InteropHelp.TestIfPlatformSupported();
    return (HSteamUser) NativeMethods.SteamGameServer_GetHSteamUser();
  }
}
