// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamApps
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamApps
{
  public static bool BIsSubscribed()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsSubscribed(CSteamAPIContext.GetSteamApps());
  }

  public static bool BIsLowViolence()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsLowViolence(CSteamAPIContext.GetSteamApps());
  }

  public static bool BIsCybercafe()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsCybercafe(CSteamAPIContext.GetSteamApps());
  }

  public static bool BIsVACBanned()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsVACBanned(CSteamAPIContext.GetSteamApps());
  }

  public static string GetCurrentGameLanguage()
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage(CSteamAPIContext.GetSteamApps()));
  }

  public static string GetAvailableGameLanguages()
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages(CSteamAPIContext.GetSteamApps()));
  }

  public static bool BIsSubscribedApp(AppId_t appID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsSubscribedApp(CSteamAPIContext.GetSteamApps(), appID);
  }

  public static bool BIsDlcInstalled(AppId_t appID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsDlcInstalled(CSteamAPIContext.GetSteamApps(), appID);
  }

  public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(CSteamAPIContext.GetSteamApps(), nAppID);
  }

  public static bool BIsSubscribedFromFreeWeekend()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend(CSteamAPIContext.GetSteamApps());
  }

  public static int GetDLCCount()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_GetDLCCount(CSteamAPIContext.GetSteamApps());
  }

  public static bool BGetDLCDataByIndex(
    int iDLC,
    out AppId_t pAppID,
    out bool pbAvailable,
    out string pchName,
    int cchNameBufferSize)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cchNameBufferSize);
    bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(CSteamAPIContext.GetSteamApps(), iDLC, out pAppID, out pbAvailable, num, cchNameBufferSize);
    pchName = flag ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return flag;
  }

  public static void InstallDLC(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamApps_InstallDLC(CSteamAPIContext.GetSteamApps(), nAppID);
  }

  public static void UninstallDLC(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamApps_UninstallDLC(CSteamAPIContext.GetSteamApps(), nAppID);
  }

  public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(CSteamAPIContext.GetSteamApps(), nAppID);
  }

  public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cchNameBufferSize);
    bool currentBetaName = NativeMethods.ISteamApps_GetCurrentBetaName(CSteamAPIContext.GetSteamApps(), num, cchNameBufferSize);
    pchName = currentBetaName ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return currentBetaName;
  }

  public static bool MarkContentCorrupt(bool bMissingFilesOnly)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_MarkContentCorrupt(CSteamAPIContext.GetSteamApps(), bMissingFilesOnly);
  }

  public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_GetInstalledDepots(CSteamAPIContext.GetSteamApps(), appID, pvecDepots, cMaxDepots);
  }

  public static uint GetAppInstallDir(
    AppId_t appID,
    out string pchFolder,
    uint cchFolderBufferSize)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal((int) cchFolderBufferSize);
    uint appInstallDir = NativeMethods.ISteamApps_GetAppInstallDir(CSteamAPIContext.GetSteamApps(), appID, num, cchFolderBufferSize);
    pchFolder = appInstallDir != 0U ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return appInstallDir;
  }

  public static bool BIsAppInstalled(AppId_t appID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_BIsAppInstalled(CSteamAPIContext.GetSteamApps(), appID);
  }

  public static CSteamID GetAppOwner()
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamApps_GetAppOwner(CSteamAPIContext.GetSteamApps());
  }

  public static string GetLaunchQueryParam(string pchKey)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(CSteamAPIContext.GetSteamApps(), pchKey1));
  }

  public static bool GetDlcDownloadProgress(
    AppId_t nAppID,
    out ulong punBytesDownloaded,
    out ulong punBytesTotal)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_GetDlcDownloadProgress(CSteamAPIContext.GetSteamApps(), nAppID, out punBytesDownloaded, out punBytesTotal);
  }

  public static int GetAppBuildId()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamApps_GetAppBuildId(CSteamAPIContext.GetSteamApps());
  }

  public static void RequestAllProofOfPurchaseKeys()
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys(CSteamAPIContext.GetSteamApps());
  }

  public static SteamAPICall_t GetFileDetails(string pszFileName)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszFileName1 = new InteropHelp.UTF8StringHandle(pszFileName))
      return (SteamAPICall_t) NativeMethods.ISteamApps_GetFileDetails(CSteamAPIContext.GetSteamApps(), pszFileName1);
  }
}
