// Decompiled with JetBrains decompiler
// Type: CommandPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class CommandPosition
{
  public FollowerCommands FollowerCommand;
  public WheelPosition WheelPosition;
  public bool ShowNotification;

  public CommandPosition(FollowerCommands f, WheelPosition w, bool showNotification = false)
  {
    this.FollowerCommand = f;
    this.WheelPosition = w;
    this.ShowNotification = showNotification;
  }
}
