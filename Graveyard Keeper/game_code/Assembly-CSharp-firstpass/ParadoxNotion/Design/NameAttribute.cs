// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Design.NameAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Design;

public class NameAttribute : Attribute
{
  public string name;
  public int priority;

  public NameAttribute(string name, int priority = 0)
  {
    this.name = name;
    this.priority = priority;
  }
}
