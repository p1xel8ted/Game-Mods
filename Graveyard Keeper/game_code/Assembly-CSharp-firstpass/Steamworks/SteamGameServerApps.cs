// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamGameServerApps
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamGameServerApps
{
  public static bool BIsSubscribed()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsSubscribed(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static bool BIsLowViolence()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsLowViolence(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static bool BIsCybercafe()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsCybercafe(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static bool BIsVACBanned()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsVACBanned(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static string GetCurrentGameLanguage()
  {
    InteropHelp.TestIfAvailableGameServer();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage(CSteamGameServerAPIContext.GetSteamApps()));
  }

  public static string GetAvailableGameLanguages()
  {
    InteropHelp.TestIfAvailableGameServer();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages(CSteamGameServerAPIContext.GetSteamApps()));
  }

  public static bool BIsSubscribedApp(AppId_t appID)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsSubscribedApp(CSteamGameServerAPIContext.GetSteamApps(), appID);
  }

  public static bool BIsDlcInstalled(AppId_t appID)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsDlcInstalled(CSteamGameServerAPIContext.GetSteamApps(), appID);
  }

  public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(CSteamGameServerAPIContext.GetSteamApps(), nAppID);
  }

  public static bool BIsSubscribedFromFreeWeekend()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static int GetDLCCount()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_GetDLCCount(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static bool BGetDLCDataByIndex(
    int iDLC,
    out AppId_t pAppID,
    out bool pbAvailable,
    out string pchName,
    int cchNameBufferSize)
  {
    InteropHelp.TestIfAvailableGameServer();
    IntPtr num = Marshal.AllocHGlobal(cchNameBufferSize);
    bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(CSteamGameServerAPIContext.GetSteamApps(), iDLC, out pAppID, out pbAvailable, num, cchNameBufferSize);
    pchName = flag ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return flag;
  }

  public static void InstallDLC(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableGameServer();
    NativeMethods.ISteamApps_InstallDLC(CSteamGameServerAPIContext.GetSteamApps(), nAppID);
  }

  public static void UninstallDLC(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableGameServer();
    NativeMethods.ISteamApps_UninstallDLC(CSteamGameServerAPIContext.GetSteamApps(), nAppID);
  }

  public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableGameServer();
    NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(CSteamGameServerAPIContext.GetSteamApps(), nAppID);
  }

  public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
  {
    InteropHelp.TestIfAvailableGameServer();
    IntPtr num = Marshal.AllocHGlobal(cchNameBufferSize);
    bool currentBetaName = NativeMethods.ISteamApps_GetCurrentBetaName(CSteamGameServerAPIContext.GetSteamApps(), num, cchNameBufferSize);
    pchName = currentBetaName ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return currentBetaName;
  }

  public static bool MarkContentCorrupt(bool bMissingFilesOnly)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_MarkContentCorrupt(CSteamGameServerAPIContext.GetSteamApps(), bMissingFilesOnly);
  }

  public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_GetInstalledDepots(CSteamGameServerAPIContext.GetSteamApps(), appID, pvecDepots, cMaxDepots);
  }

  public static uint GetAppInstallDir(
    AppId_t appID,
    out string pchFolder,
    uint cchFolderBufferSize)
  {
    InteropHelp.TestIfAvailableGameServer();
    IntPtr num = Marshal.AllocHGlobal((int) cchFolderBufferSize);
    uint appInstallDir = NativeMethods.ISteamApps_GetAppInstallDir(CSteamGameServerAPIContext.GetSteamApps(), appID, num, cchFolderBufferSize);
    pchFolder = appInstallDir != 0U ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return appInstallDir;
  }

  public static bool BIsAppInstalled(AppId_t appID)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_BIsAppInstalled(CSteamGameServerAPIContext.GetSteamApps(), appID);
  }

  public static CSteamID GetAppOwner()
  {
    InteropHelp.TestIfAvailableGameServer();
    return (CSteamID) NativeMethods.ISteamApps_GetAppOwner(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static string GetLaunchQueryParam(string pchKey)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(CSteamGameServerAPIContext.GetSteamApps(), pchKey1));
  }

  public static bool GetDlcDownloadProgress(
    AppId_t nAppID,
    out ulong punBytesDownloaded,
    out ulong punBytesTotal)
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_GetDlcDownloadProgress(CSteamGameServerAPIContext.GetSteamApps(), nAppID, out punBytesDownloaded, out punBytesTotal);
  }

  public static int GetAppBuildId()
  {
    InteropHelp.TestIfAvailableGameServer();
    return NativeMethods.ISteamApps_GetAppBuildId(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static void RequestAllProofOfPurchaseKeys()
  {
    InteropHelp.TestIfAvailableGameServer();
    NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys(CSteamGameServerAPIContext.GetSteamApps());
  }

  public static SteamAPICall_t GetFileDetails(string pszFileName)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pszFileName1 = new InteropHelp.UTF8StringHandle(pszFileName))
      return (SteamAPICall_t) NativeMethods.ISteamApps_GetFileDetails(CSteamGameServerAPIContext.GetSteamApps(), pszFileName1);
  }
}
