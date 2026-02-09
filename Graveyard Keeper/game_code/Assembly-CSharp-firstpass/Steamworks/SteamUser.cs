// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamUser
{
  public static HSteamUser GetHSteamUser()
  {
    InteropHelp.TestIfAvailableClient();
    return (HSteamUser) NativeMethods.ISteamUser_GetHSteamUser(CSteamAPIContext.GetSteamUser());
  }

  public static bool BLoggedOn()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BLoggedOn(CSteamAPIContext.GetSteamUser());
  }

  public static CSteamID GetSteamID()
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamUser_GetSteamID(CSteamAPIContext.GetSteamUser());
  }

  public static int InitiateGameConnection(
    byte[] pAuthBlob,
    int cbMaxAuthBlob,
    CSteamID steamIDGameServer,
    uint unIPServer,
    ushort usPortServer,
    bool bSecure)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_InitiateGameConnection(CSteamAPIContext.GetSteamUser(), pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
  }

  public static void TerminateGameConnection(uint unIPServer, ushort usPortServer)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUser_TerminateGameConnection(CSteamAPIContext.GetSteamUser(), unIPServer, usPortServer);
  }

  public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchExtraInfo1 = new InteropHelp.UTF8StringHandle(pchExtraInfo))
      NativeMethods.ISteamUser_TrackAppUsageEvent(CSteamAPIContext.GetSteamUser(), gameID, eAppUsageEvent, pchExtraInfo1);
  }

  public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cubBuffer);
    bool userDataFolder = NativeMethods.ISteamUser_GetUserDataFolder(CSteamAPIContext.GetSteamUser(), num, cubBuffer);
    pchBuffer = userDataFolder ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return userDataFolder;
  }

  public static void StartVoiceRecording()
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUser_StartVoiceRecording(CSteamAPIContext.GetSteamUser());
  }

  public static void StopVoiceRecording()
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUser_StopVoiceRecording(CSteamAPIContext.GetSteamUser());
  }

  public static EVoiceResult GetAvailableVoice(out uint pcbCompressed)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_GetAvailableVoice(CSteamAPIContext.GetSteamUser(), out pcbCompressed, IntPtr.Zero, 0U);
  }

  public static EVoiceResult GetVoice(
    bool bWantCompressed,
    byte[] pDestBuffer,
    uint cbDestBufferSize,
    out uint nBytesWritten)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_GetVoice(CSteamAPIContext.GetSteamUser(), bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, false, IntPtr.Zero, 0U, IntPtr.Zero, 0U);
  }

  public static EVoiceResult DecompressVoice(
    byte[] pCompressed,
    uint cbCompressed,
    byte[] pDestBuffer,
    uint cbDestBufferSize,
    out uint nBytesWritten,
    uint nDesiredSampleRate)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_DecompressVoice(CSteamAPIContext.GetSteamUser(), pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
  }

  public static uint GetVoiceOptimalSampleRate()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate(CSteamAPIContext.GetSteamUser());
  }

  public static HAuthTicket GetAuthSessionTicket(
    byte[] pTicket,
    int cbMaxTicket,
    out uint pcbTicket)
  {
    InteropHelp.TestIfAvailableClient();
    return (HAuthTicket) NativeMethods.ISteamUser_GetAuthSessionTicket(CSteamAPIContext.GetSteamUser(), pTicket, cbMaxTicket, out pcbTicket);
  }

  public static EBeginAuthSessionResult BeginAuthSession(
    byte[] pAuthTicket,
    int cbAuthTicket,
    CSteamID steamID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BeginAuthSession(CSteamAPIContext.GetSteamUser(), pAuthTicket, cbAuthTicket, steamID);
  }

  public static void EndAuthSession(CSteamID steamID)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUser_EndAuthSession(CSteamAPIContext.GetSteamUser(), steamID);
  }

  public static void CancelAuthTicket(HAuthTicket hAuthTicket)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUser_CancelAuthTicket(CSteamAPIContext.GetSteamUser(), hAuthTicket);
  }

  public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_UserHasLicenseForApp(CSteamAPIContext.GetSteamUser(), steamID, appID);
  }

  public static bool BIsBehindNAT()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BIsBehindNAT(CSteamAPIContext.GetSteamUser());
  }

  public static void AdvertiseGame(
    CSteamID steamIDGameServer,
    uint unIPServer,
    ushort usPortServer)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUser_AdvertiseGame(CSteamAPIContext.GetSteamUser(), steamIDGameServer, unIPServer, usPortServer);
  }

  public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUser_RequestEncryptedAppTicket(CSteamAPIContext.GetSteamUser(), pDataToInclude, cbDataToInclude);
  }

  public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_GetEncryptedAppTicket(CSteamAPIContext.GetSteamUser(), pTicket, cbMaxTicket, out pcbTicket);
  }

  public static int GetGameBadgeLevel(int nSeries, bool bFoil)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_GetGameBadgeLevel(CSteamAPIContext.GetSteamUser(), nSeries, bFoil);
  }

  public static int GetPlayerSteamLevel()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_GetPlayerSteamLevel(CSteamAPIContext.GetSteamUser());
  }

  public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchRedirectURL1 = new InteropHelp.UTF8StringHandle(pchRedirectURL))
      return (SteamAPICall_t) NativeMethods.ISteamUser_RequestStoreAuthURL(CSteamAPIContext.GetSteamUser(), pchRedirectURL1);
  }

  public static bool BIsPhoneVerified()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BIsPhoneVerified(CSteamAPIContext.GetSteamUser());
  }

  public static bool BIsTwoFactorEnabled()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BIsTwoFactorEnabled(CSteamAPIContext.GetSteamUser());
  }

  public static bool BIsPhoneIdentifying()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BIsPhoneIdentifying(CSteamAPIContext.GetSteamUser());
  }

  public static bool BIsPhoneRequiringVerification()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUser_BIsPhoneRequiringVerification(CSteamAPIContext.GetSteamUser());
  }
}
