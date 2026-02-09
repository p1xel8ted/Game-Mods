// Decompiled with JetBrains decompiler
// Type: GameplaySettingsUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
