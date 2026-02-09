// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAppList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamAppList
{
  public static uint GetNumInstalledApps()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamAppList_GetNumInstalledApps(CSteamAPIContext.GetSteamAppList());
  }

  public static uint GetInstalledApps(AppId_t[] pvecAppID, uint unMaxAppIDs)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamAppList_GetInstalledApps(CSteamAPIContext.GetSteamAppList(), pvecAppID, unMaxAppIDs);
  }

  public static int GetAppName(AppId_t nAppID, out string pchName, int cchNameMax)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cchNameMax);
    int appName = NativeMethods.ISteamAppList_GetAppName(CSteamAPIContext.GetSteamAppList(), nAppID, num, cchNameMax);
    pchName = appName != -1 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return appName;
  }

  public static int GetAppInstallDir(AppId_t nAppID, out string pchDirectory, int cchNameMax)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cchNameMax);
    int appInstallDir = NativeMethods.ISteamAppList_GetAppInstallDir(CSteamAPIContext.GetSteamAppList(), nAppID, num, cchNameMax);
    pchDirectory = appInstallDir != -1 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return appInstallDir;
  }

  public static int GetAppBuildId(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamAppList_GetAppBuildId(CSteamAPIContext.GetSteamAppList(), nAppID);
  }
}
