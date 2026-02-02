// Decompiled with JetBrains decompiler
// Type: GameplaySettingsUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
public class GameplaySettingsUtilities
{
  public static Action<bool> OnShowFollowerNamesChanged;

  public static void UpdateShowFollowerNamesSetting(bool value)
  {
    SettingsManager.Settings.Game.ShowFollowerNames = value;
    Action<bool> followerNamesChanged = GameplaySettingsUtilities.OnShowFollowerNamesChanged;
    if (followerNamesChanged == null)
      return;
    followerNamesChanged(value);
  }
}
