// Decompiled with JetBrains decompiler
// Type: HubMenuItemAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
public class HubMenuItemAttribute : Attribute
{
  public string icon_name;
  public string text;
  public string tooltip;

  public HubMenuItemAttribute(string icon_name, string text = "", string tooltip = "")
  {
    this.icon_name = icon_name;
    this.text = text;
    this.tooltip = tooltip;
  }
}
