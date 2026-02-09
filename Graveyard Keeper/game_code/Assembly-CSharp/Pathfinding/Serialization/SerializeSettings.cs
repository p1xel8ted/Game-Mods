// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.SerializeSettings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding.Serialization;

public class SerializeSettings
{
  public bool nodes = true;
  [Obsolete("There is no support for pretty printing the json anymore")]
  public bool prettyPrint;
  public bool editorSettings;

  public static SerializeSettings Settings
  {
    get => new SerializeSettings() { nodes = false };
  }
}
