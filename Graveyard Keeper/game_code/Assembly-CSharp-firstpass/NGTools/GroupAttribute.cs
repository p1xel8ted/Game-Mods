// Decompiled with JetBrains decompiler
// Type: NGTools.GroupAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NGTools;

public class GroupAttribute : PropertyAttribute
{
  public string group;
  public bool hide;

  public GroupAttribute(string group, bool hide = false)
  {
    this.group = group;
    this.hide = hide;
    this.order = -1;
  }
}
