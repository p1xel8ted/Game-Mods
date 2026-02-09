// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUtils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class SteamUtils
{
  public static uint GetSecondsSinceAppActive()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetSecondsSinceAppActive(CSteamAPIContext.GetSteamUtils());
  }

  public static uint GetSecondsSinceComputerActive()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetSecondsSinceComputerActive(CSteamAPIContext.GetSteamUtils());
  }

  public static EUniverse GetConnectedUniverse()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetConnectedUniverse(CSteamAPIContext.GetSteamUtils());
  }

  public static uint GetServerRealTime()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetServerRealTime(CSteamAPIContext.GetSteamUtils());
  }

  public static string GetIPCountry()
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetIPCountry(CSteamAPIContext.GetSteamUtils()));
  }

  public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetImageSize(CSteamAPIContext.GetSteamUtils(), iImage, out pnWidth, out pnHeight);
  }

  public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetImageRGBA(CSteamAPIContext.GetSteamUtils(), iImage, pubDest, nDestBufferSize);
  }

  public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetCSERIPPort(CSteamAPIContext.GetSteamUtils(), out unIP, out usPort);
  }

  public static byte GetCurrentBatteryPower()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetCurrentBatteryPower(CSteamAPIContext.GetSteamUtils());
  }

  public static AppId_t GetAppID()
  {
    InteropHelp.TestIfAvailableClient();
    return (AppId_t) NativeMethods.ISteamUtils_GetAppID(CSteamAPIContext.GetSteamUtils());
  }

  public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUtils_SetOverlayNotificationPosition(CSteamAPIContext.GetSteamUtils(), eNotificationPosition);
  }

  public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_IsAPICallCompleted(CSteamAPIContext.GetSteamUtils(), hSteamAPICall, out pbFailed);
  }

  public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetAPICallFailureReason(CSteamAPIContext.GetSteamUtils(), hSteamAPICall);
  }

  public static bool GetAPICallResult(
    SteamAPICall_t hSteamAPICall,
    IntPtr pCallback,
    int cubCallback,
    int iCallbackExpected,
    out bool pbFailed)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetAPICallResult(CSteamAPIContext.GetSteamUtils(), hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
  }

  public static uint GetIPCCallCount()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetIPCCallCount(CSteamAPIContext.GetSteamUtils());
  }

  public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUtils_SetWarningMessageHook(CSteamAPIContext.GetSteamUtils(), pFunction);
  }

  public static bool IsOverlayEnabled()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_IsOverlayEnabled(CSteamAPIContext.GetSteamUtils());
  }

  public static bool BOverlayNeedsPresent()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_BOverlayNeedsPresent(CSteamAPIContext.GetSteamUtils());
  }

  public static SteamAPICall_t CheckFileSignature(string szFileName)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle szFileName1 = new InteropHelp.UTF8StringHandle(szFileName))
      return (SteamAPICall_t) NativeMethods.ISteamUtils_CheckFileSignature(CSteamAPIContext.GetSteamUtils(), szFileName1);
  }

  public static bool ShowGamepadTextInput(
    EGamepadTextInputMode eInputMode,
    EGamepadTextInputLineMode eLineInputMode,
    string pchDescription,
    uint unCharMax,
    string pchExistingText)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchDescription1 = new InteropHelp.UTF8StringHandle(pchDescription))
    {
      using (InteropHelp.UTF8StringHandle pchExistingText1 = new InteropHelp.UTF8StringHandle(pchExistingText))
        return NativeMethods.ISteamUtils_ShowGamepadTextInput(CSteamAPIContext.GetSteamUtils(), eInputMode, eLineInputMode, pchDescription1, unCharMax, pchExistingText1);
    }
  }

  public static uint GetEnteredGamepadTextLength()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_GetEnteredGamepadTextLength(CSteamAPIContext.GetSteamUtils());
  }

  public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
  {
    InteropHelp.TestIfAvailableClient();
    IntPtr num = Marshal.AllocHGlobal((int) cchText);
    bool gamepadTextInput = NativeMethods.ISteamUtils_GetEnteredGamepadTextInput(CSteamAPIContext.GetSteamUtils(), num, cchText);
    pchText = gamepadTextInput ? InteropHelp.PtrToStringUTF8(num) : (string) null;
    Marshal.FreeHGlobal(num);
    return gamepadTextInput;
  }

  public static string GetSteamUILanguage()
  {
    InteropHelp.TestIfAvailableClient();
    return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetSteamUILanguage(CSteamAPIContext.GetSteamUtils()));
  }

  public static bool IsSteamRunningInVR()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_IsSteamRunningInVR(CSteamAPIContext.GetSteamUtils());
  }

  public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUtils_SetOverlayNotificationInset(CSteamAPIContext.GetSteamUtils(), nHorizontalInset, nVerticalInset);
  }

  public static bool IsSteamInBigPictureMode()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_IsSteamInBigPictureMode(CSteamAPIContext.GetSteamUtils());
  }

  public static void StartVRDashboard()
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUtils_StartVRDashboard(CSteamAPIContext.GetSteamUtils());
  }

  public static bool IsVRHeadsetStreamingEnabled()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamUtils_IsVRHeadsetStreamingEnabled(CSteamAPIContext.GetSteamUtils());
  }

  public static void SetVRHeadsetStreamingEnabled(bool bEnabled)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamUtils_SetVRHeadsetStreamingEnabled(CSteamAPIContext.GetSteamUtils(), bEnabled);
  }
}
