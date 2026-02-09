// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamEncryptedAppTicket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamEncryptedAppTicket
{
  public static bool BDecryptTicket(
    byte[] rgubTicketEncrypted,
    uint cubTicketEncrypted,
    byte[] rgubTicketDecrypted,
    ref uint pcubTicketDecrypted,
    byte[] rgubKey,
    int cubKey)
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamEncryptedAppTicket_BDecryptTicket(rgubTicketEncrypted, cubTicketEncrypted, rgubTicketDecrypted, ref pcubTicketDecrypted, rgubKey, cubKey);
  }

  public static bool BIsTicketForApp(
    byte[] rgubTicketDecrypted,
    uint cubTicketDecrypted,
    AppId_t nAppID)
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamEncryptedAppTicket_BIsTicketForApp(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
  }

  public static uint GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamEncryptedAppTicket_GetTicketIssueTime(rgubTicketDecrypted, cubTicketDecrypted);
  }

  public static void GetTicketSteamID(
    byte[] rgubTicketDecrypted,
    uint cubTicketDecrypted,
    out CSteamID psteamID)
  {
    InteropHelp.TestIfPlatformSupported();
    NativeMethods.SteamEncryptedAppTicket_GetTicketSteamID(rgubTicketDecrypted, cubTicketDecrypted, out psteamID);
  }

  public static uint GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamEncryptedAppTicket_GetTicketAppID(rgubTicketDecrypted, cubTicketDecrypted);
  }

  public static bool BUserOwnsAppInTicket(
    byte[] rgubTicketDecrypted,
    uint cubTicketDecrypted,
    AppId_t nAppID)
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamEncryptedAppTicket_BUserOwnsAppInTicket(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
  }

  public static bool BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
  {
    InteropHelp.TestIfPlatformSupported();
    return NativeMethods.SteamEncryptedAppTicket_BUserIsVacBanned(rgubTicketDecrypted, cubTicketDecrypted);
  }

  public static byte[] GetUserVariableData(
    byte[] rgubTicketDecrypted,
    uint cubTicketDecrypted,
    out uint pcubUserData)
  {
    InteropHelp.TestIfPlatformSupported();
    IntPtr userVariableData1 = NativeMethods.SteamEncryptedAppTicket_GetUserVariableData(rgubTicketDecrypted, cubTicketDecrypted, out pcubUserData);
    byte[] userVariableData2 = new byte[(int) pcubUserData];
    byte[] destination = userVariableData2;
    int length = (int) pcubUserData;
    Marshal.Copy(userVariableData1, destination, 0, length);
    return userVariableData2;
  }
}
