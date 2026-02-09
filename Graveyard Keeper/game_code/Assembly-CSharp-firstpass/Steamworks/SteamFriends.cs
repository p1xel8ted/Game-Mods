// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamFriends
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamFriends
{
  public static string GetPersonaName()
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPersonaName(CSteamAPIContext.GetSteamFriends()));
  }

  public static SteamAPICall_t SetPersonaName(string pchPersonaName)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchPersonaName1 = new InteropHelp.UTF8StringHandle(pchPersonaName))
      return (SteamAPICall_t) NativeMethods.ISteamFriends_SetPersonaName(CSteamAPIContext.GetSteamFriends(), pchPersonaName1);
  }

  public static EPersonaState GetPersonaState()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetPersonaState(CSteamAPIContext.GetSteamFriends());
  }

  public static int GetFriendCount(EFriendFlags iFriendFlags)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendCount(CSteamAPIContext.GetSteamFriends(), iFriendFlags);
  }

  public static CSteamID GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetFriendByIndex(CSteamAPIContext.GetSteamFriends(), iFriend, iFriendFlags);
  }

  public static EFriendRelationship GetFriendRelationship(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendRelationship(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static EPersonaState GetFriendPersonaState(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendPersonaState(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static string GetFriendPersonaName(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaName(CSteamAPIContext.GetSteamFriends(), steamIDFriend));
  }

  public static bool GetFriendGamePlayed(
    CSteamID steamIDFriend,
    out FriendGameInfo_t pFriendGameInfo)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendGamePlayed(CSteamAPIContext.GetSteamFriends(), steamIDFriend, out pFriendGameInfo);
  }

  public static string GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaNameHistory(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iPersonaName));
  }

  public static int GetFriendSteamLevel(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendSteamLevel(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static string GetPlayerNickname(CSteamID steamIDPlayer)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPlayerNickname(CSteamAPIContext.GetSteamFriends(), steamIDPlayer));
  }

  public static int GetFriendsGroupCount()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendsGroupCount(CSteamAPIContext.GetSteamFriends());
  }

  public static FriendsGroupID_t GetFriendsGroupIDByIndex(int iFG)
  {
    InteropHelp.TestIfAvailableClient();
    return (FriendsGroupID_t) NativeMethods.ISteamFriends_GetFriendsGroupIDByIndex(CSteamAPIContext.GetSteamFriends(), iFG);
  }

  public static string GetFriendsGroupName(FriendsGroupID_t friendsGroupID)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendsGroupName(CSteamAPIContext.GetSteamFriends(), friendsGroupID));
  }

  public static int GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendsGroupMembersCount(CSteamAPIContext.GetSteamFriends(), friendsGroupID);
  }

  public static void GetFriendsGroupMembersList(
    FriendsGroupID_t friendsGroupID,
    CSteamID[] pOutSteamIDMembers,
    int nMembersCount)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_GetFriendsGroupMembersList(CSteamAPIContext.GetSteamFriends(), friendsGroupID, pOutSteamIDMembers, nMembersCount);
  }

  public static bool HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_HasFriend(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iFriendFlags);
  }

  public static int GetClanCount()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetClanCount(CSteamAPIContext.GetSteamFriends());
  }

  public static CSteamID GetClanByIndex(int iClan)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetClanByIndex(CSteamAPIContext.GetSteamFriends(), iClan);
  }

  public static string GetClanName(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanName(CSteamAPIContext.GetSteamFriends(), steamIDClan));
  }

  public static string GetClanTag(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanTag(CSteamAPIContext.GetSteamFriends(), steamIDClan));
  }

  public static bool GetClanActivityCounts(
    CSteamID steamIDClan,
    out int pnOnline,
    out int pnInGame,
    out int pnChatting)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetClanActivityCounts(CSteamAPIContext.GetSteamFriends(), steamIDClan, out pnOnline, out pnInGame, out pnChatting);
  }

  public static SteamAPICall_t DownloadClanActivityCounts(
    CSteamID[] psteamIDClans,
    int cClansToRequest)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamFriends_DownloadClanActivityCounts(CSteamAPIContext.GetSteamFriends(), psteamIDClans, cClansToRequest);
  }

  public static int GetFriendCountFromSource(CSteamID steamIDSource)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendCountFromSource(CSteamAPIContext.GetSteamFriends(), steamIDSource);
  }

  public static CSteamID GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetFriendFromSourceByIndex(CSteamAPIContext.GetSteamFriends(), steamIDSource, iFriend);
  }

  public static bool IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_IsUserInSource(CSteamAPIContext.GetSteamFriends(), steamIDUser, steamIDSource);
  }

  public static void SetInGameVoiceSpeaking(CSteamID steamIDUser, bool bSpeaking)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_SetInGameVoiceSpeaking(CSteamAPIContext.GetSteamFriends(), steamIDUser, bSpeaking);
  }

  public static void ActivateGameOverlay(string pchDialog)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchDialog1 = new InteropHelp.UTF8StringHandle(pchDialog))
      NativeMethods.ISteamFriends_ActivateGameOverlay(CSteamAPIContext.GetSteamFriends(), pchDialog1);
  }

  public static void ActivateGameOverlayToUser(string pchDialog, CSteamID steamID)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchDialog1 = new InteropHelp.UTF8StringHandle(pchDialog))
      NativeMethods.ISteamFriends_ActivateGameOverlayToUser(CSteamAPIContext.GetSteamFriends(), pchDialog1, steamID);
  }

  public static void ActivateGameOverlayToWebPage(string pchURL)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchURL1 = new InteropHelp.UTF8StringHandle(pchURL))
      NativeMethods.ISteamFriends_ActivateGameOverlayToWebPage(CSteamAPIContext.GetSteamFriends(), pchURL1);
  }

  public static void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_ActivateGameOverlayToStore(CSteamAPIContext.GetSteamFriends(), nAppID, eFlag);
  }

  public static void SetPlayedWith(CSteamID steamIDUserPlayedWith)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_SetPlayedWith(CSteamAPIContext.GetSteamFriends(), steamIDUserPlayedWith);
  }

  public static void ActivateGameOverlayInviteDialog(CSteamID steamIDLobby)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialog(CSteamAPIContext.GetSteamFriends(), steamIDLobby);
  }

  public static int GetSmallFriendAvatar(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetSmallFriendAvatar(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static int GetMediumFriendAvatar(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetMediumFriendAvatar(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static int GetLargeFriendAvatar(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetLargeFriendAvatar(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static bool RequestUserInformation(CSteamID steamIDUser, bool bRequireNameOnly)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_RequestUserInformation(CSteamAPIContext.GetSteamFriends(), steamIDUser, bRequireNameOnly);
  }

  public static SteamAPICall_t RequestClanOfficerList(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamFriends_RequestClanOfficerList(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static CSteamID GetClanOwner(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetClanOwner(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static int GetClanOfficerCount(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetClanOfficerCount(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static CSteamID GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetClanOfficerByIndex(CSteamAPIContext.GetSteamFriends(), steamIDClan, iOfficer);
  }

  public static uint GetUserRestrictions()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetUserRestrictions(CSteamAPIContext.GetSteamFriends());
  }

  public static bool SetRichPresence(string pchKey, string pchValue)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
    {
      using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
        return NativeMethods.ISteamFriends_SetRichPresence(CSteamAPIContext.GetSteamFriends(), pchKey1, pchValue1);
    }
  }

  public static void ClearRichPresence()
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_ClearRichPresence(CSteamAPIContext.GetSteamFriends());
  }

  public static string GetFriendRichPresence(CSteamID steamIDFriend, string pchKey)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresence(CSteamAPIContext.GetSteamFriends(), steamIDFriend, pchKey1));
  }

  public static int GetFriendRichPresenceKeyCount(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendRichPresenceKeyCount(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static string GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey)
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresenceKeyByIndex(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iKey));
  }

  public static void RequestFriendRichPresence(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamFriends_RequestFriendRichPresence(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static bool InviteUserToGame(CSteamID steamIDFriend, string pchConnectString)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchConnectString1 = new InteropHelp.UTF8StringHandle(pchConnectString))
      return NativeMethods.ISteamFriends_InviteUserToGame(CSteamAPIContext.GetSteamFriends(), steamIDFriend, pchConnectString1);
  }

  public static int GetCoplayFriendCount()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetCoplayFriendCount(CSteamAPIContext.GetSteamFriends());
  }

  public static CSteamID GetCoplayFriend(int iCoplayFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetCoplayFriend(CSteamAPIContext.GetSteamFriends(), iCoplayFriend);
  }

  public static int GetFriendCoplayTime(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetFriendCoplayTime(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static AppId_t GetFriendCoplayGame(CSteamID steamIDFriend)
  {
    InteropHelp.TestIfAvailableClient();
    return (AppId_t) NativeMethods.ISteamFriends_GetFriendCoplayGame(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
  }

  public static SteamAPICall_t JoinClanChatRoom(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamFriends_JoinClanChatRoom(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static bool LeaveClanChatRoom(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_LeaveClanChatRoom(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static int GetClanChatMemberCount(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_GetClanChatMemberCount(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static CSteamID GetChatMemberByIndex(CSteamID steamIDClan, int iUser)
  {
    InteropHelp.TestIfAvailableClient();
    return (CSteamID) NativeMethods.ISteamFriends_GetChatMemberByIndex(CSteamAPIContext.GetSteamFriends(), steamIDClan, iUser);
  }

  public static bool SendClanChatMessage(CSteamID steamIDClanChat, string pchText)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchText1 = new InteropHelp.UTF8StringHandle(pchText))
      return NativeMethods.ISteamFriends_SendClanChatMessage(CSteamAPIContext.GetSteamFriends(), steamIDClanChat, pchText1);
  }

  public static int GetClanChatMessage(
    CSteamID steamIDClanChat,
    int iMessage,
    out string prgchText,
    int cchTextMax,
    out EChatEntryType peChatEntryType,
    out CSteamID psteamidChatter)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cchTextMax);
    int clanChatMessage = NativeMethods.ISteamFriends_GetClanChatMessage(CSteamAPIContext.GetSteamFriends(), steamIDClanChat, iMessage, num, cchTextMax, out peChatEntryType, out psteamidChatter);
    prgchText = clanChatMessage != 0 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return clanChatMessage;
  }

  public static bool IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_IsClanChatAdmin(CSteamAPIContext.GetSteamFriends(), steamIDClanChat, steamIDUser);
  }

  public static bool IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_IsClanChatWindowOpenInSteam(CSteamAPIContext.GetSteamFriends(), steamIDClanChat);
  }

  public static bool OpenClanChatWindowInSteam(CSteamID steamIDClanChat)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_OpenClanChatWindowInSteam(CSteamAPIContext.GetSteamFriends(), steamIDClanChat);
  }

  public static bool CloseClanChatWindowInSteam(CSteamID steamIDClanChat)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_CloseClanChatWindowInSteam(CSteamAPIContext.GetSteamFriends(), steamIDClanChat);
  }

  public static bool SetListenForFriendsMessages(bool bInterceptEnabled)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_SetListenForFriendsMessages(CSteamAPIContext.GetSteamFriends(), bInterceptEnabled);
  }

  public static bool ReplyToFriendMessage(CSteamID steamIDFriend, string pchMsgToSend)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchMsgToSend1 = new InteropHelp.UTF8StringHandle(pchMsgToSend))
      return NativeMethods.ISteamFriends_ReplyToFriendMessage(CSteamAPIContext.GetSteamFriends(), steamIDFriend, pchMsgToSend1);
  }

  public static int GetFriendMessage(
    CSteamID steamIDFriend,
    int iMessageID,
    out string pvData,
    int cubData,
    out EChatEntryType peChatEntryType)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal(cubData);
    int friendMessage = NativeMethods.ISteamFriends_GetFriendMessage(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iMessageID, num, cubData, out peChatEntryType);
    pvData = friendMessage != 0 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return friendMessage;
  }

  public static SteamAPICall_t GetFollowerCount(CSteamID steamID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamFriends_GetFollowerCount(CSteamAPIContext.GetSteamFriends(), steamID);
  }

  public static SteamAPICall_t IsFollowing(CSteamID steamID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamFriends_IsFollowing(CSteamAPIContext.GetSteamFriends(), steamID);
  }

  public static SteamAPICall_t EnumerateFollowingList(uint unStartIndex)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamFriends_EnumerateFollowingList(CSteamAPIContext.GetSteamFriends(), unStartIndex);
  }

  public static bool IsClanPublic(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_IsClanPublic(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }

  public static bool IsClanOfficialGameGroup(CSteamID steamIDClan)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamFriends_IsClanOfficialGameGroup(CSteamAPIContext.GetSteamFriends(), steamIDClan);
  }
}
