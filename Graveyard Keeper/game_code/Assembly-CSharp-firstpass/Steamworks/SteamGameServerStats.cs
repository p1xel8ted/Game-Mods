// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamGameServerStats
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public static class SteamGameServerStats
{
  public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
  {
    InteropHelp.TestIfAvailableGameServer();
    return (SteamAPICall_t) NativeMethods.ISteamGameServerStats_RequestUserStats(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser);
  }

  public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_GetUserStat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1, out pData);
  }

  public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_GetUserStat0(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1, out pData);
  }

  public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_GetUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1, out pbAchieved);
  }

  public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_SetUserStat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1, nData);
  }

  public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_SetUserStat0(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1, fData);
  }

  public static bool UpdateUserAvgRateStat(
    CSteamID steamIDUser,
    string pchName,
    float flCountThisSession,
    double dSessionLength)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1, flCountThisSession, dSessionLength);
  }

  public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_SetUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1);
  }

  public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
  {
    InteropHelp.TestIfAvailableGameServer();
    using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      return NativeMethods.ISteamGameServerStats_ClearUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, pchName1);
  }

  public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
  {
    InteropHelp.TestIfAvailableGameServer();
    return (SteamAPICall_t) NativeMethods.ISteamGameServerStats_StoreUserStats(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser);
  }
}
