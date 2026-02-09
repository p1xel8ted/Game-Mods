// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUGC
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamUGC
{
  public static UGCQueryHandle_t CreateQueryUserUGCRequest(
    AccountID_t unAccountID,
    EUserUGCList eListType,
    EUGCMatchingUGCType eMatchingUGCType,
    EUserUGCListSortOrder eSortOrder,
    AppId_t nCreatorAppID,
    AppId_t nConsumerAppID,
    uint unPage)
  {
    InteropHelp.TestIfAvailableClient();
    return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(CSteamAPIContext.GetSteamUGC(), unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
  }

  public static UGCQueryHandle_t CreateQueryAllUGCRequest(
    EUGCQuery eQueryType,
    EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType,
    AppId_t nCreatorAppID,
    AppId_t nConsumerAppID,
    uint unPage)
  {
    InteropHelp.TestIfAvailableClient();
    return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryAllUGCRequest(CSteamAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
  }

  public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(
    PublishedFileId_t[] pvecPublishedFileID,
    uint unNumPublishedFileIDs)
  {
    InteropHelp.TestIfAvailableClient();
    return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
  }

  public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_SendQueryUGCRequest(CSteamAPIContext.GetSteamUGC(), handle);
  }

  public static bool GetQueryUGCResult(
    UGCQueryHandle_t handle,
    uint index,
    out SteamUGCDetails_t pDetails)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetQueryUGCResult(CSteamAPIContext.GetSteamUGC(), handle, index, out pDetails);
  }

  public static bool GetQueryUGCPreviewURL(
    UGCQueryHandle_t handle,
    uint index,
    out string pchURL,
    uint cchURLSize)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal((int) cchURLSize);
    bool queryUgcPreviewUrl = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(CSteamAPIContext.GetSteamUGC(), handle, index, num, cchURLSize);
    pchURL = queryUgcPreviewUrl ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return queryUgcPreviewUrl;
  }

  public static bool GetQueryUGCMetadata(
    UGCQueryHandle_t handle,
    uint index,
    out string pchMetadata,
    uint cchMetadatasize)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal((int) cchMetadatasize);
    bool queryUgcMetadata = NativeMethods.ISteamUGC_GetQueryUGCMetadata(CSteamAPIContext.GetSteamUGC(), handle, index, num, cchMetadatasize);
    pchMetadata = queryUgcMetadata ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return queryUgcMetadata;
  }

  public static bool GetQueryUGCChildren(
    UGCQueryHandle_t handle,
    uint index,
    PublishedFileId_t[] pvecPublishedFileID,
    uint cMaxEntries)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetQueryUGCChildren(CSteamAPIContext.GetSteamUGC(), handle, index, pvecPublishedFileID, cMaxEntries);
  }

  public static bool GetQueryUGCStatistic(
    UGCQueryHandle_t handle,
    uint index,
    EItemStatistic eStatType,
    out ulong pStatValue)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetQueryUGCStatistic(CSteamAPIContext.GetSteamUGC(), handle, index, eStatType, out pStatValue);
  }

  public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(CSteamAPIContext.GetSteamUGC(), handle, index);
  }

  public static bool GetQueryUGCAdditionalPreview(
    UGCQueryHandle_t handle,
    uint index,
    uint previewIndex,
    out string pchURLOrVideoID,
    uint cchURLSize,
    out string pchOriginalFileName,
    uint cchOriginalFileNameSize,
    out EItemPreviewType pPreviewType)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num1 = Marshal.AllocHGlobal((int) cchURLSize);
    IntPtr num2 = Marshal.AllocHGlobal((int) cchOriginalFileNameSize);
    bool additionalPreview = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(CSteamAPIContext.GetSteamUGC(), handle, index, previewIndex, num1, cchURLSize, num2, cchOriginalFileNameSize, out pPreviewType);
    pchURLOrVideoID = additionalPreview ? InteropHelp.PtrToStringUTF8(num1) : (string) null;
    Marshal.FreeHGlobal(num1);
    pchOriginalFileName = additionalPreview ? InteropHelp.PtrToStringUTF8(num2) : (string) null;
    Marshal.FreeHGlobal(num2);
    return additionalPreview;
  }

  public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, index);
  }

  public static bool GetQueryUGCKeyValueTag(
    UGCQueryHandle_t handle,
    uint index,
    uint keyValueTagIndex,
    out string pchKey,
    uint cchKeySize,
    out string pchValue,
    uint cchValueSize)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num1 = Marshal.AllocHGlobal((int) cchKeySize);
    IntPtr num2 = Marshal.AllocHGlobal((int) cchValueSize);
    bool queryUgcKeyValueTag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, index, keyValueTagIndex, num1, cchKeySize, num2, cchValueSize);
    pchKey = queryUgcKeyValueTag ? InteropHelp.PtrToStringUTF8(num1) : (string) null;
    Marshal.FreeHGlobal(num1);
    pchValue = queryUgcKeyValueTag ? InteropHelp.PtrToStringUTF8(num2) : (string) null;
    Marshal.FreeHGlobal(num2);
    return queryUgcKeyValueTag;
  }

  public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(CSteamAPIContext.GetSteamUGC(), handle);
  }

  public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pTagName1 = new InteropHelp.UTF8StringHandle(pTagName))
      return NativeMethods.ISteamUGC_AddRequiredTag(CSteamAPIContext.GetSteamUGC(), handle, pTagName1);
  }

  public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pTagName1 = new InteropHelp.UTF8StringHandle(pTagName))
      return NativeMethods.ISteamUGC_AddExcludedTag(CSteamAPIContext.GetSteamUGC(), handle, pTagName1);
  }

  public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnOnlyIDs(CSteamAPIContext.GetSteamUGC(), handle, bReturnOnlyIDs);
  }

  public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, bReturnKeyValueTags);
  }

  public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnLongDescription(CSteamAPIContext.GetSteamUGC(), handle, bReturnLongDescription);
  }

  public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnMetadata(CSteamAPIContext.GetSteamUGC(), handle, bReturnMetadata);
  }

  public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnChildren(CSteamAPIContext.GetSteamUGC(), handle, bReturnChildren);
  }

  public static bool SetReturnAdditionalPreviews(
    UGCQueryHandle_t handle,
    bool bReturnAdditionalPreviews)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(CSteamAPIContext.GetSteamUGC(), handle, bReturnAdditionalPreviews);
  }

  public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnTotalOnly(CSteamAPIContext.GetSteamUGC(), handle, bReturnTotalOnly);
  }

  public static bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetReturnPlaytimeStats(CSteamAPIContext.GetSteamUGC(), handle, unDays);
  }

  public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchLanguage1 = new InteropHelp.UTF8StringHandle(pchLanguage))
      return NativeMethods.ISteamUGC_SetLanguage(CSteamAPIContext.GetSteamUGC(), handle, pchLanguage1);
  }

  public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetAllowCachedResponse(CSteamAPIContext.GetSteamUGC(), handle, unMaxAgeSeconds);
  }

  public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pMatchCloudFileName1 = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
      return NativeMethods.ISteamUGC_SetCloudFileNameFilter(CSteamAPIContext.GetSteamUGC(), handle, pMatchCloudFileName1);
  }

  public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetMatchAnyTag(CSteamAPIContext.GetSteamUGC(), handle, bMatchAnyTag);
  }

  public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pSearchText1 = new InteropHelp.UTF8StringHandle(pSearchText))
      return NativeMethods.ISteamUGC_SetSearchText(CSteamAPIContext.GetSteamUGC(), handle, pSearchText1);
  }

  public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetRankedByTrendDays(CSteamAPIContext.GetSteamUGC(), handle, unDays);
  }

  public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pKey1 = new InteropHelp.UTF8StringHandle(pKey))
    {
      using (InteropHelp.UTF8StringHandle pValue1 = new InteropHelp.UTF8StringHandle(pValue))
        return NativeMethods.ISteamUGC_AddRequiredKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, pKey1, pValue1);
    }
  }

  public static SteamAPICall_t RequestUGCDetails(
    PublishedFileId_t nPublishedFileID,
    uint unMaxAgeSeconds)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_RequestUGCDetails(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, unMaxAgeSeconds);
  }

  public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_CreateItem(CSteamAPIContext.GetSteamUGC(), nConsumerAppId, eFileType);
  }

  public static UGCUpdateHandle_t StartItemUpdate(
    AppId_t nConsumerAppId,
    PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (UGCUpdateHandle_t) NativeMethods.ISteamUGC_StartItemUpdate(CSteamAPIContext.GetSteamUGC(), nConsumerAppId, nPublishedFileID);
  }

  public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchTitle1 = new InteropHelp.UTF8StringHandle(pchTitle))
      return NativeMethods.ISteamUGC_SetItemTitle(CSteamAPIContext.GetSteamUGC(), handle, pchTitle1);
  }

  public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchDescription1 = new InteropHelp.UTF8StringHandle(pchDescription))
      return NativeMethods.ISteamUGC_SetItemDescription(CSteamAPIContext.GetSteamUGC(), handle, pchDescription1);
  }

  public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchLanguage1 = new InteropHelp.UTF8StringHandle(pchLanguage))
      return NativeMethods.ISteamUGC_SetItemUpdateLanguage(CSteamAPIContext.GetSteamUGC(), handle, pchLanguage1);
  }

  public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchMetaData1 = new InteropHelp.UTF8StringHandle(pchMetaData))
      return NativeMethods.ISteamUGC_SetItemMetadata(CSteamAPIContext.GetSteamUGC(), handle, pchMetaData1);
  }

  public static bool SetItemVisibility(
    UGCUpdateHandle_t handle,
    ERemoteStoragePublishedFileVisibility eVisibility)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetItemVisibility(CSteamAPIContext.GetSteamUGC(), handle, eVisibility);
  }

  public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_SetItemTags(CSteamAPIContext.GetSteamUGC(), updateHandle, (IntPtr) new InteropHelp.SteamParamStringArray(pTags));
  }

  public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszContentFolder1 = new InteropHelp.UTF8StringHandle(pszContentFolder))
      return NativeMethods.ISteamUGC_SetItemContent(CSteamAPIContext.GetSteamUGC(), handle, pszContentFolder1);
  }

  public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszPreviewFile1 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
      return NativeMethods.ISteamUGC_SetItemPreview(CSteamAPIContext.GetSteamUGC(), handle, pszPreviewFile1);
  }

  public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      return NativeMethods.ISteamUGC_RemoveItemKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, pchKey1);
  }

  public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
    {
      using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
        return NativeMethods.ISteamUGC_AddItemKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, pchKey1, pchValue1);
    }
  }

  public static bool AddItemPreviewFile(
    UGCUpdateHandle_t handle,
    string pszPreviewFile,
    EItemPreviewType type)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszPreviewFile1 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
      return NativeMethods.ISteamUGC_AddItemPreviewFile(CSteamAPIContext.GetSteamUGC(), handle, pszPreviewFile1, type);
  }

  public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszVideoID1 = new InteropHelp.UTF8StringHandle(pszVideoID))
      return NativeMethods.ISteamUGC_AddItemPreviewVideo(CSteamAPIContext.GetSteamUGC(), handle, pszVideoID1);
  }

  public static bool UpdateItemPreviewFile(
    UGCUpdateHandle_t handle,
    uint index,
    string pszPreviewFile)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszPreviewFile1 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
      return NativeMethods.ISteamUGC_UpdateItemPreviewFile(CSteamAPIContext.GetSteamUGC(), handle, index, pszPreviewFile1);
  }

  public static bool UpdateItemPreviewVideo(
    UGCUpdateHandle_t handle,
    uint index,
    string pszVideoID)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszVideoID1 = new InteropHelp.UTF8StringHandle(pszVideoID))
      return NativeMethods.ISteamUGC_UpdateItemPreviewVideo(CSteamAPIContext.GetSteamUGC(), handle, index, pszVideoID1);
  }

  public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_RemoveItemPreview(CSteamAPIContext.GetSteamUGC(), handle, index);
  }

  public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchChangeNote1 = new InteropHelp.UTF8StringHandle(pchChangeNote))
      return (SteamAPICall_t) NativeMethods.ISteamUGC_SubmitItemUpdate(CSteamAPIContext.GetSteamUGC(), handle, pchChangeNote1);
  }

  public static EItemUpdateStatus GetItemUpdateProgress(
    UGCUpdateHandle_t handle,
    out ulong punBytesProcessed,
    out ulong punBytesTotal)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetItemUpdateProgress(CSteamAPIContext.GetSteamUGC(), handle, out punBytesProcessed, out punBytesTotal);
  }

  public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_SetUserItemVote(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, bVoteUp);
  }

  public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_GetUserItemVote(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
  }

  public static SteamAPICall_t AddItemToFavorites(
    AppId_t nAppId,
    PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_AddItemToFavorites(CSteamAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
  }

  public static SteamAPICall_t RemoveItemFromFavorites(
    AppId_t nAppId,
    PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_RemoveItemFromFavorites(CSteamAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
  }

  public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_SubscribeItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
  }

  public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_UnsubscribeItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
  }

  public static uint GetNumSubscribedItems()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetNumSubscribedItems(CSteamAPIContext.GetSteamUGC());
  }

  public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetSubscribedItems(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, cMaxEntries);
  }

  public static uint GetItemState(PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetItemState(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
  }

  public static bool GetItemInstallInfo(
    PublishedFileId_t nPublishedFileID,
    out ulong punSizeOnDisk,
    out string pchFolder,
    uint cchFolderSize,
    out uint punTimeStamp)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal((int) cchFolderSize);
    bool itemInstallInfo = NativeMethods.ISteamUGC_GetItemInstallInfo(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, out punSizeOnDisk, num, cchFolderSize, out punTimeStamp);
    pchFolder = itemInstallInfo ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return itemInstallInfo;
  }

  public static bool GetItemDownloadInfo(
    PublishedFileId_t nPublishedFileID,
    out ulong punBytesDownloaded,
    out ulong punBytesTotal)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_GetItemDownloadInfo(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
  }

  public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUGC_DownloadItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, bHighPriority);
  }

  public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pszFolder1 = new InteropHelp.UTF8StringHandle(pszFolder))
      return NativeMethods.ISteamUGC_BInitWorkshopForGameServer(CSteamAPIContext.GetSteamUGC(), unWorkshopDepotID, pszFolder1);
  }

  public static void SuspendDownloads(bool bSuspend)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUGC_SuspendDownloads(CSteamAPIContext.GetSteamUGC(), bSuspend);
  }

  public static SteamAPICall_t StartPlaytimeTracking(
    PublishedFileId_t[] pvecPublishedFileID,
    uint unNumPublishedFileIDs)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_StartPlaytimeTracking(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
  }

  public static SteamAPICall_t StopPlaytimeTracking(
    PublishedFileId_t[] pvecPublishedFileID,
    uint unNumPublishedFileIDs)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_StopPlaytimeTracking(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
  }

  public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems(CSteamAPIContext.GetSteamUGC());
  }

  public static SteamAPICall_t AddDependency(
    PublishedFileId_t nParentPublishedFileID,
    PublishedFileId_t nChildPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_AddDependency(CSteamAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
  }

  public static SteamAPICall_t RemoveDependency(
    PublishedFileId_t nParentPublishedFileID,
    PublishedFileId_t nChildPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_RemoveDependency(CSteamAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
  }

  public static SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_AddAppDependency(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
  }

  public static SteamAPICall_t RemoveAppDependency(
    PublishedFileId_t nPublishedFileID,
    AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_RemoveAppDependency(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
  }

  public static SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_GetAppDependencies(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
  }

  public static SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
  {
    InteropHelp.TestIfAvailableClient();
    return (SteamAPICall_t) NativeMethods.ISteamUGC_DeleteItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
  }
}
