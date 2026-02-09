// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamMatchmakingServers
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamMatchmakingServers
{
  public static HServerListRequest RequestInternetServerList(
    AppId_t iApp,
    MatchMakingKeyValuePair_t[] ppchFilters,
    uint nFilters,
    ISteamMatchmakingServerListResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestInternetServerList(CSteamAPIContext.GetSteamMatchmakingServers(), iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
  }

  public static HServerListRequest RequestLANServerList(
    AppId_t iApp,
    ISteamMatchmakingServerListResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestLANServerList(CSteamAPIContext.GetSteamMatchmakingServers(), iApp, (IntPtr) pRequestServersResponse);
  }

  public static HServerListRequest RequestFriendsServerList(
    AppId_t iApp,
    MatchMakingKeyValuePair_t[] ppchFilters,
    uint nFilters,
    ISteamMatchmakingServerListResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestFriendsServerList(CSteamAPIContext.GetSteamMatchmakingServers(), iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
  }

  public static HServerListRequest RequestFavoritesServerList(
    AppId_t iApp,
    MatchMakingKeyValuePair_t[] ppchFilters,
    uint nFilters,
    ISteamMatchmakingServerListResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestFavoritesServerList(CSteamAPIContext.GetSteamMatchmakingServers(), iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
  }

  public static HServerListRequest RequestHistoryServerList(
    AppId_t iApp,
    MatchMakingKeyValuePair_t[] ppchFilters,
    uint nFilters,
    ISteamMatchmakingServerListResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestHistoryServerList(CSteamAPIContext.GetSteamMatchmakingServers(), iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
  }

  public static HServerListRequest RequestSpectatorServerList(
    AppId_t iApp,
    MatchMakingKeyValuePair_t[] ppchFilters,
    uint nFilters,
    ISteamMatchmakingServerListResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestSpectatorServerList(CSteamAPIContext.GetSteamMatchmakingServers(), iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
  }

  public static void ReleaseRequest(HServerListRequest hServerListRequest)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamMatchmakingServers_ReleaseRequest(CSteamAPIContext.GetSteamMatchmakingServers(), hServerListRequest);
  }

  public static gameserveritem_t GetServerDetails(HServerListRequest hRequest, int iServer)
  {
    InteropHelp.TestIfAvailableClient();
    return (gameserveritem_t) Marshal.PtrToStructure(NativeMethods.ISteamMatchmakingServers_GetServerDetails(CSteamAPIContext.GetSteamMatchmakingServers(), hRequest, iServer), typeof (gameserveritem_t));
  }

  public static void CancelQuery(HServerListRequest hRequest)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamMatchmakingServers_CancelQuery(CSteamAPIContext.GetSteamMatchmakingServers(), hRequest);
  }

  public static void RefreshQuery(HServerListRequest hRequest)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamMatchmakingServers_RefreshQuery(CSteamAPIContext.GetSteamMatchmakingServers(), hRequest);
  }

  public static bool IsRefreshing(HServerListRequest hRequest)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamMatchmakingServers_IsRefreshing(CSteamAPIContext.GetSteamMatchmakingServers(), hRequest);
  }

  public static int GetServerCount(HServerListRequest hRequest)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamMatchmakingServers_GetServerCount(CSteamAPIContext.GetSteamMatchmakingServers(), hRequest);
  }

  public static void RefreshServer(HServerListRequest hRequest, int iServer)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamMatchmakingServers_RefreshServer(CSteamAPIContext.GetSteamMatchmakingServers(), hRequest, iServer);
  }

  public static HServerQuery PingServer(
    uint unIP,
    ushort usPort,
    ISteamMatchmakingPingResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerQuery) NativeMethods.ISteamMatchmakingServers_PingServer(CSteamAPIContext.GetSteamMatchmakingServers(), unIP, usPort, (IntPtr) pRequestServersResponse);
  }

  public static HServerQuery PlayerDetails(
    uint unIP,
    ushort usPort,
    ISteamMatchmakingPlayersResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerQuery) NativeMethods.ISteamMatchmakingServers_PlayerDetails(CSteamAPIContext.GetSteamMatchmakingServers(), unIP, usPort, (IntPtr) pRequestServersResponse);
  }

  public static HServerQuery ServerRules(
    uint unIP,
    ushort usPort,
    ISteamMatchmakingRulesResponse pRequestServersResponse)
  {
    InteropHelp.TestIfAvailableClient();
    return (HServerQuery) NativeMethods.ISteamMatchmakingServers_ServerRules(CSteamAPIContext.GetSteamMatchmakingServers(), unIP, usPort, (IntPtr) pRequestServersResponse);
  }

  public static void CancelServerQuery(HServerQuery hServerQuery)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamMatchmakingServers_CancelServerQuery(CSteamAPIContext.GetSteamMatchmakingServers(), hServerQuery);
  }
}
