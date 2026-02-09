// Decompiled with JetBrains decompiler
// Type: Steamworks.EResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EResult
{
  k_EResultOK = 1,
  k_EResultFail = 2,
  k_EResultNoConnection = 3,
  k_EResultInvalidPassword = 5,
  k_EResultLoggedInElsewhere = 6,
  k_EResultInvalidProtocolVer = 7,
  k_EResultInvalidParam = 8,
  k_EResultFileNotFound = 9,
  k_EResultBusy = 10, // 0x0000000A
  k_EResultInvalidState = 11, // 0x0000000B
  k_EResultInvalidName = 12, // 0x0000000C
  k_EResultInvalidEmail = 13, // 0x0000000D
  k_EResultDuplicateName = 14, // 0x0000000E
  k_EResultAccessDenied = 15, // 0x0000000F
  k_EResultTimeout = 16, // 0x00000010
  k_EResultBanned = 17, // 0x00000011
  k_EResultAccountNotFound = 18, // 0x00000012
  k_EResultInvalidSteamID = 19, // 0x00000013
  k_EResultServiceUnavailable = 20, // 0x00000014
  k_EResultNotLoggedOn = 21, // 0x00000015
  k_EResultPending = 22, // 0x00000016
  k_EResultEncryptionFailure = 23, // 0x00000017
  k_EResultInsufficientPrivilege = 24, // 0x00000018
  k_EResultLimitExceeded = 25, // 0x00000019
  k_EResultRevoked = 26, // 0x0000001A
  k_EResultExpired = 27, // 0x0000001B
  k_EResultAlreadyRedeemed = 28, // 0x0000001C
  k_EResultDuplicateRequest = 29, // 0x0000001D
  k_EResultAlreadyOwned = 30, // 0x0000001E
  k_EResultIPNotFound = 31, // 0x0000001F
  k_EResultPersistFailed = 32, // 0x00000020
  k_EResultLockingFailed = 33, // 0x00000021
  k_EResultLogonSessionReplaced = 34, // 0x00000022
  k_EResultConnectFailed = 35, // 0x00000023
  k_EResultHandshakeFailed = 36, // 0x00000024
  k_EResultIOFailure = 37, // 0x00000025
  k_EResultRemoteDisconnect = 38, // 0x00000026
  k_EResultShoppingCartNotFound = 39, // 0x00000027
  k_EResultBlocked = 40, // 0x00000028
  k_EResultIgnored = 41, // 0x00000029
  k_EResultNoMatch = 42, // 0x0000002A
  k_EResultAccountDisabled = 43, // 0x0000002B
  k_EResultServiceReadOnly = 44, // 0x0000002C
  k_EResultAccountNotFeatured = 45, // 0x0000002D
  k_EResultAdministratorOK = 46, // 0x0000002E
  k_EResultContentVersion = 47, // 0x0000002F
  k_EResultTryAnotherCM = 48, // 0x00000030
  k_EResultPasswordRequiredToKickSession = 49, // 0x00000031
  k_EResultAlreadyLoggedInElsewhere = 50, // 0x00000032
  k_EResultSuspended = 51, // 0x00000033
  k_EResultCancelled = 52, // 0x00000034
  k_EResultDataCorruption = 53, // 0x00000035
  k_EResultDiskFull = 54, // 0x00000036
  k_EResultRemoteCallFailed = 55, // 0x00000037
  k_EResultPasswordUnset = 56, // 0x00000038
  k_EResultExternalAccountUnlinked = 57, // 0x00000039
  k_EResultPSNTicketInvalid = 58, // 0x0000003A
  k_EResultExternalAccountAlreadyLinked = 59, // 0x0000003B
  k_EResultRemoteFileConflict = 60, // 0x0000003C
  k_EResultIllegalPassword = 61, // 0x0000003D
  k_EResultSameAsPreviousValue = 62, // 0x0000003E
  k_EResultAccountLogonDenied = 63, // 0x0000003F
  k_EResultCannotUseOldPassword = 64, // 0x00000040
  k_EResultInvalidLoginAuthCode = 65, // 0x00000041
  k_EResultAccountLogonDeniedNoMail = 66, // 0x00000042
  k_EResultHardwareNotCapableOfIPT = 67, // 0x00000043
  k_EResultIPTInitError = 68, // 0x00000044
  k_EResultParentalControlRestricted = 69, // 0x00000045
  k_EResultFacebookQueryError = 70, // 0x00000046
  k_EResultExpiredLoginAuthCode = 71, // 0x00000047
  k_EResultIPLoginRestrictionFailed = 72, // 0x00000048
  k_EResultAccountLockedDown = 73, // 0x00000049
  k_EResultAccountLogonDeniedVerifiedEmailRequired = 74, // 0x0000004A
  k_EResultNoMatchingURL = 75, // 0x0000004B
  k_EResultBadResponse = 76, // 0x0000004C
  k_EResultRequirePasswordReEntry = 77, // 0x0000004D
  k_EResultValueOutOfRange = 78, // 0x0000004E
  k_EResultUnexpectedError = 79, // 0x0000004F
  k_EResultDisabled = 80, // 0x00000050
  k_EResultInvalidCEGSubmission = 81, // 0x00000051
  k_EResultRestrictedDevice = 82, // 0x00000052
  k_EResultRegionLocked = 83, // 0x00000053
  k_EResultRateLimitExceeded = 84, // 0x00000054
  k_EResultAccountLoginDeniedNeedTwoFactor = 85, // 0x00000055
  k_EResultItemDeleted = 86, // 0x00000056
  k_EResultAccountLoginDeniedThrottle = 87, // 0x00000057
  k_EResultTwoFactorCodeMismatch = 88, // 0x00000058
  k_EResultTwoFactorActivationCodeMismatch = 89, // 0x00000059
  k_EResultAccountAssociatedToMultiplePartners = 90, // 0x0000005A
  k_EResultNotModified = 91, // 0x0000005B
  k_EResultNoMobileDevice = 92, // 0x0000005C
  k_EResultTimeNotSynced = 93, // 0x0000005D
  k_EResultSmsCodeFailed = 94, // 0x0000005E
  k_EResultAccountLimitExceeded = 95, // 0x0000005F
  k_EResultAccountActivityLimitExceeded = 96, // 0x00000060
  k_EResultPhoneActivityLimitExceeded = 97, // 0x00000061
  k_EResultRefundToWallet = 98, // 0x00000062
  k_EResultEmailSendFailure = 99, // 0x00000063
  k_EResultNotSettled = 100, // 0x00000064
  k_EResultNeedCaptcha = 101, // 0x00000065
  k_EResultGSLTDenied = 102, // 0x00000066
  k_EResultGSOwnerDenied = 103, // 0x00000067
  k_EResultInvalidItemType = 104, // 0x00000068
  k_EResultIPBanned = 105, // 0x00000069
  k_EResultGSLTExpired = 106, // 0x0000006A
  k_EResultInsufficientFunds = 107, // 0x0000006B
  k_EResultTooManyPending = 108, // 0x0000006C
  k_EResultNoSiteLicensesFound = 109, // 0x0000006D
  k_EResultWGNetworkSendExceeded = 110, // 0x0000006E
  k_EResultAccountNotFriends = 111, // 0x0000006F
  k_EResultLimitedUserAccount = 112, // 0x00000070
}
