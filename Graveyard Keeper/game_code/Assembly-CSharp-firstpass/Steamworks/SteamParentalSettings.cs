// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamParentalSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public static class SteamParentalSettings
{
  public static bool BIsParentalLockEnabled()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamParentalSettings_BIsParentalLockEnabled(CSteamAPIContext.GetSteamParentalSettings());
  }

  public static bool BIsParentalLockLocked()
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamParentalSettings_BIsParentalLockLocked(CSteamAPIContext.GetSteamParentalSettings());
  }

  public static bool BIsAppBlocked(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamParentalSettings_BIsAppBlocked(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
  }

  public static bool BIsAppInBlockList(AppId_t nAppID)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamParentalSettings_BIsAppInBlockList(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
  }

  public static bool BIsFeatureBlocked(EParentalFeature eFeature)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamParentalSettings_BIsFeatureBlocked(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
  }

  public static bool BIsFeatureInBlockList(EParentalFeature eFeature)
  {
    InteropHelp.TestIfAvailableClient();
    return NativeMethods.ISteamParentalSettings_BIsFeatureInBlockList(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
  }
}
