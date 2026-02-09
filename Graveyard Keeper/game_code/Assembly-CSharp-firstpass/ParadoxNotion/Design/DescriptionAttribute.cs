// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Design.DescriptionAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Design;

public class DescriptionAttribute : Attribute
{
  public string description;

  public DescriptionAttribute(string description) => this.description = description;
}
