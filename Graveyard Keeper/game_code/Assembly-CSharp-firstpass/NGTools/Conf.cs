// Decompiled with JetBrains decompiler
// Type: NGTools.Conf
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NGTools;

public static class Conf
{
  public const string DebugModeKeyPref = "NGTools_DebugMode";
  public static Action DebugModeChanged;
  public static Conf.DebugModes debugMode;

  public static Conf.DebugModes DebugMode
  {
    get => Conf.debugMode;
    set
    {
      if (Conf.debugMode == value)
        return;
      Conf.debugMode = value;
      if (Conf.DebugModeChanged == null)
        return;
      Conf.DebugModeChanged();
    }
  }

  public enum DebugModes
  {
    None,
    Active,
    Verbose,
  }
}
