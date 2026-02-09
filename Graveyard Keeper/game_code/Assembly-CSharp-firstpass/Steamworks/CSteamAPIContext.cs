// Decompiled with JetBrains decompiler
// Type: Steamworks.CSteamAPIContext
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

public static class CSteamAPIContext
{
  public static IntPtr m_pSteamClient;
  public static IntPtr m_pSteamUser;
  public static IntPtr m_pSteamFriends;
  public static IntPtr m_pSteamUtils;
  public static IntPtr m_pSteamMatchmaking;
  public static IntPtr m_pSteamUserStats;
  public static IntPtr m_pSteamApps;
  public static IntPtr m_pSteamMatchmakingServers;
  public static IntPtr m_pSteamNetworking;
  public static IntPtr m_pSteamRemoteStorage;
  public static IntPtr m_pSteamScreenshots;
  public static IntPtr m_pSteamHTTP;
  public static IntPtr m_pController;
  public static IntPtr m_pSteamUGC;
  public static IntPtr m_pSteamAppList;
  public static IntPtr m_pSteamMusic;
  public static IntPtr m_pSteamMusicRemote;
  public static IntPtr m_pSteamHTMLSurface;
  public static IntPtr m_pSteamInventory;
  public static IntPtr m_pSteamVideo;
  public static IntPtr m_pSteamParentalSettings;

  public static void Clear()
  {
    CSteamAPIContext.m_pSteamClient = IntPtr.Zero;
    CSteamAPIContext.m_pSteamUser = IntPtr.Zero;
    CSteamAPIContext.m_pSteamFriends = IntPtr.Zero;
    CSteamAPIContext.m_pSteamUtils = IntPtr.Zero;
    CSteamAPIContext.m_pSteamMatchmaking = IntPtr.Zero;
    CSteamAPIContext.m_pSteamUserStats = IntPtr.Zero;
    CSteamAPIContext.m_pSteamApps = IntPtr.Zero;
    CSteamAPIContext.m_pSteamMatchmakingServers = IntPtr.Zero;
    CSteamAPIContext.m_pSteamNetworking = IntPtr.Zero;
    CSteamAPIContext.m_pSteamRemoteStorage = IntPtr.Zero;
    CSteamAPIContext.m_pSteamHTTP = IntPtr.Zero;
    CSteamAPIContext.m_pSteamScreenshots = IntPtr.Zero;
    CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
    CSteamAPIContext.m_pController = IntPtr.Zero;
    CSteamAPIContext.m_pSteamUGC = IntPtr.Zero;
    CSteamAPIContext.m_pSteamAppList = IntPtr.Zero;
    CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
    CSteamAPIContext.m_pSteamMusicRemote = IntPtr.Zero;
    CSteamAPIContext.m_pSteamHTMLSurface = IntPtr.Zero;
    CSteamAPIContext.m_pSteamInventory = IntPtr.Zero;
    CSteamAPIContext.m_pSteamVideo = IntPtr.Zero;
    CSteamAPIContext.m_pSteamParentalSettings = IntPtr.Zero;
  }

  public static bool Init()
  {
    HSteamUser hsteamUser = SteamAPI.GetHSteamUser();
    HSteamPipe hsteamPipe = SteamAPI.GetHSteamPipe();
    if (hsteamPipe == (HSteamPipe) 0)
      return false;
    using (InteropHelp.UTF8StringHandle ver = new InteropHelp.UTF8StringHandle("SteamClient017"))
      CSteamAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(ver);
    if (CSteamAPIContext.m_pSteamClient == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamUser = SteamClient.GetISteamUser(hsteamUser, hsteamPipe, "SteamUser019");
    if (CSteamAPIContext.m_pSteamUser == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamFriends = SteamClient.GetISteamFriends(hsteamUser, hsteamPipe, "SteamFriends015");
    if (CSteamAPIContext.m_pSteamFriends == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamUtils = SteamClient.GetISteamUtils(hsteamPipe, "SteamUtils009");
    if (CSteamAPIContext.m_pSteamUtils == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamMatchmaking = SteamClient.GetISteamMatchmaking(hsteamUser, hsteamPipe, "SteamMatchMaking009");
    if (CSteamAPIContext.m_pSteamMatchmaking == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(hsteamUser, hsteamPipe, "SteamMatchMakingServers002");
    if (CSteamAPIContext.m_pSteamMatchmakingServers == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamUserStats = SteamClient.GetISteamUserStats(hsteamUser, hsteamPipe, "STEAMUSERSTATS_INTERFACE_VERSION011");
    if (CSteamAPIContext.m_pSteamUserStats == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamApps = SteamClient.GetISteamApps(hsteamUser, hsteamPipe, "STEAMAPPS_INTERFACE_VERSION008");
    if (CSteamAPIContext.m_pSteamApps == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamNetworking = SteamClient.GetISteamNetworking(hsteamUser, hsteamPipe, "SteamNetworking005");
    if (CSteamAPIContext.m_pSteamNetworking == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(hsteamUser, hsteamPipe, "STEAMREMOTESTORAGE_INTERFACE_VERSION014");
    if (CSteamAPIContext.m_pSteamRemoteStorage == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamScreenshots = SteamClient.GetISteamScreenshots(hsteamUser, hsteamPipe, "STEAMSCREENSHOTS_INTERFACE_VERSION003");
    if (CSteamAPIContext.m_pSteamScreenshots == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamHTTP = SteamClient.GetISteamHTTP(hsteamUser, hsteamPipe, "STEAMHTTP_INTERFACE_VERSION002");
    if (CSteamAPIContext.m_pSteamHTTP == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pController = SteamClient.GetISteamController(hsteamUser, hsteamPipe, "SteamController006");
    if (CSteamAPIContext.m_pController == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamUGC = SteamClient.GetISteamUGC(hsteamUser, hsteamPipe, "STEAMUGC_INTERFACE_VERSION010");
    if (CSteamAPIContext.m_pSteamUGC == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamAppList = SteamClient.GetISteamAppList(hsteamUser, hsteamPipe, "STEAMAPPLIST_INTERFACE_VERSION001");
    if (CSteamAPIContext.m_pSteamAppList == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamMusic = SteamClient.GetISteamMusic(hsteamUser, hsteamPipe, "STEAMMUSIC_INTERFACE_VERSION001");
    if (CSteamAPIContext.m_pSteamMusic == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamMusicRemote = SteamClient.GetISteamMusicRemote(hsteamUser, hsteamPipe, "STEAMMUSICREMOTE_INTERFACE_VERSION001");
    if (CSteamAPIContext.m_pSteamMusicRemote == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(hsteamUser, hsteamPipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_004");
    if (CSteamAPIContext.m_pSteamHTMLSurface == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamInventory = SteamClient.GetISteamInventory(hsteamUser, hsteamPipe, "STEAMINVENTORY_INTERFACE_V002");
    if (CSteamAPIContext.m_pSteamInventory == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamVideo = SteamClient.GetISteamVideo(hsteamUser, hsteamPipe, "STEAMVIDEO_INTERFACE_V002");
    if (CSteamAPIContext.m_pSteamVideo == IntPtr.Zero)
      return false;
    CSteamAPIContext.m_pSteamParentalSettings = SteamClient.GetISteamParentalSettings(hsteamUser, hsteamPipe, "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
    return !(CSteamAPIContext.m_pSteamParentalSettings == IntPtr.Zero);
  }

  public static IntPtr GetSteamClient() => CSteamAPIContext.m_pSteamClient;

  public static IntPtr GetSteamUser() => CSteamAPIContext.m_pSteamUser;

  public static IntPtr GetSteamFriends() => CSteamAPIContext.m_pSteamFriends;

  public static IntPtr GetSteamUtils() => CSteamAPIContext.m_pSteamUtils;

  public static IntPtr GetSteamMatchmaking() => CSteamAPIContext.m_pSteamMatchmaking;

  public static IntPtr GetSteamUserStats() => CSteamAPIContext.m_pSteamUserStats;

  public static IntPtr GetSteamApps() => CSteamAPIContext.m_pSteamApps;

  public static IntPtr GetSteamMatchmakingServers() => CSteamAPIContext.m_pSteamMatchmakingServers;

  public static IntPtr GetSteamNetworking() => CSteamAPIContext.m_pSteamNetworking;

  public static IntPtr GetSteamRemoteStorage() => CSteamAPIContext.m_pSteamRemoteStorage;

  public static IntPtr GetSteamScreenshots() => CSteamAPIContext.m_pSteamScreenshots;

  public static IntPtr GetSteamHTTP() => CSteamAPIContext.m_pSteamHTTP;

  public static IntPtr GetSteamController() => CSteamAPIContext.m_pController;

  public static IntPtr GetSteamUGC() => CSteamAPIContext.m_pSteamUGC;

  public static IntPtr GetSteamAppList() => CSteamAPIContext.m_pSteamAppList;

  public static IntPtr GetSteamMusic() => CSteamAPIContext.m_pSteamMusic;

  public static IntPtr GetSteamMusicRemote() => CSteamAPIContext.m_pSteamMusicRemote;

  public static IntPtr GetSteamHTMLSurface() => CSteamAPIContext.m_pSteamHTMLSurface;

  public static IntPtr GetSteamInventory() => CSteamAPIContext.m_pSteamInventory;

  public static IntPtr GetSteamVideo() => CSteamAPIContext.m_pSteamVideo;

  public static IntPtr GetSteamParentalSettings() => CSteamAPIContext.m_pSteamParentalSettings;
}
