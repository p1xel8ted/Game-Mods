// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamHTMLSurface
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

public static class SteamHTMLSurface
{
  public static bool Init()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamHTMLSurface_Init(CSteamAPIContext.GetSteamHTMLSurface());
  }

  public static bool Shutdown()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamHTMLSurface_Shutdown(CSteamAPIContext.GetSteamHTMLSurface());
  }

  public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchUserAgent1 = new InteropHelp.UTF8StringHandle(pchUserAgent))
    {
      using (InteropHelp.UTF8StringHandle pchUserCSS1 = new InteropHelp.UTF8StringHandle(pchUserCSS))
        return (SteamAPICall_t) NativeMethods.ISteamHTMLSurface_CreateBrowser(CSteamAPIContext.GetSteamHTMLSurface(), pchUserAgent1, pchUserCSS1);
    }
  }

  public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_RemoveBrowser(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchURL1 = new InteropHelp.UTF8StringHandle(pchURL))
    {
      using (InteropHelp.UTF8StringHandle pchPostData1 = new InteropHelp.UTF8StringHandle(pchPostData))
        NativeMethods.ISteamHTMLSurface_LoadURL(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, pchURL1, pchPostData1);
    }
  }

  public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetSize(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, unWidth, unHeight);
  }

  public static void StopLoad(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_StopLoad(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void Reload(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_Reload(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void GoBack(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_GoBack(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void GoForward(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_GoForward(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
    {
      using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
        NativeMethods.ISteamHTMLSurface_AddHeader(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, pchKey1, pchValue1);
    }
  }

  public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchScript1 = new InteropHelp.UTF8StringHandle(pchScript))
      NativeMethods.ISteamHTMLSurface_ExecuteJavascript(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, pchScript1);
  }

  public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_MouseUp(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, eMouseButton);
  }

  public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_MouseDown(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, eMouseButton);
  }

  public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_MouseDoubleClick(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, eMouseButton);
  }

  public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_MouseMove(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, x, y);
  }

  public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_MouseWheel(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nDelta);
  }

  public static void KeyDown(
    HHTMLBrowser unBrowserHandle,
    uint nNativeKeyCode,
    EHTMLKeyModifiers eHTMLKeyModifiers)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_KeyDown(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
  }

  public static void KeyUp(
    HHTMLBrowser unBrowserHandle,
    uint nNativeKeyCode,
    EHTMLKeyModifiers eHTMLKeyModifiers)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_KeyUp(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
  }

  public static void KeyChar(
    HHTMLBrowser unBrowserHandle,
    uint cUnicodeChar,
    EHTMLKeyModifiers eHTMLKeyModifiers)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_KeyChar(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
  }

  public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nAbsolutePixelScroll);
  }

  public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetVerticalScroll(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nAbsolutePixelScroll);
  }

  public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetKeyFocus(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bHasKeyFocus);
  }

  public static void ViewSource(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_ViewSource(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_CopyToClipboard(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_PasteFromClipboard(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void Find(
    HHTMLBrowser unBrowserHandle,
    string pchSearchStr,
    bool bCurrentlyInFind,
    bool bReverse)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchSearchStr1 = new InteropHelp.UTF8StringHandle(pchSearchStr))
      NativeMethods.ISteamHTMLSurface_Find(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, pchSearchStr1, bCurrentlyInFind, bReverse);
  }

  public static void StopFind(HHTMLBrowser unBrowserHandle)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_StopFind(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
  }

  public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, x, y);
  }

  public static void SetCookie(
    string pchHostname,
    string pchKey,
    string pchValue,
    string pchPath = "/",
    uint nExpires = 0,
    bool bSecure = false,
    bool bHTTPOnly = false)
  {
    InteropHelp.TestIfAvailableClient();
    using (InteropHelp.UTF8StringHandle pchHostname1 = new InteropHelp.UTF8StringHandle(pchHostname))
    {
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      {
        using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
        {
          using (InteropHelp.UTF8StringHandle pchPath1 = new InteropHelp.UTF8StringHandle(pchPath))
            NativeMethods.ISteamHTMLSurface_SetCookie(CSteamAPIContext.GetSteamHTMLSurface(), pchHostname1, pchKey1, pchValue1, pchPath1, nExpires, bSecure, bHTTPOnly);
        }
      }
    }
  }

  public static void SetPageScaleFactor(
    HHTMLBrowser unBrowserHandle,
    float flZoom,
    int nPointX,
    int nPointY)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, flZoom, nPointX, nPointY);
  }

  public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetBackgroundMode(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bBackgroundMode);
  }

  public static void SetDPIScalingFactor(HHTMLBrowser unBrowserHandle, float flDPIScaling)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_SetDPIScalingFactor(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, flDPIScaling);
  }

  public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_AllowStartRequest(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bAllowed);
  }

  public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_JSDialogResponse(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bResult);
  }

  public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
  {
    InteropHelp.TestIfAvailableClient();
    NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, pchSelectedFiles);
  }
}
