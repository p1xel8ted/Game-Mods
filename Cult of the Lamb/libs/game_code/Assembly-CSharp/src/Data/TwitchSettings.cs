// Decompiled with JetBrains decompiler
// Type: src.Data.TwitchSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Data;

[MessagePackObject(false)]
[Serializable]
public class TwitchSettings
{
  [Key(1)]
  public bool HelpHinderEnabled = true;
  [Key(2)]
  public float HelpHinderFrequency = 20f;
  [Key(3)]
  public bool TotemEnabled = true;
  [Key(4)]
  public bool FollowerNamesEnabled = true;
  [Key(5)]
  public bool TwitchMessagesEnabled = true;

  public TwitchSettings()
  {
  }

  public TwitchSettings(TwitchSettings twitchSettings)
  {
    this.HelpHinderEnabled = twitchSettings.HelpHinderEnabled;
    this.HelpHinderFrequency = twitchSettings.HelpHinderFrequency;
    this.TotemEnabled = twitchSettings.TotemEnabled;
    this.FollowerNamesEnabled = twitchSettings.FollowerNamesEnabled;
    this.TwitchMessagesEnabled = twitchSettings.TwitchMessagesEnabled;
  }
}
