// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.DeserializeFromAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization;

public class DeserializeFromAttribute : Attribute
{
  public string[] previousTypeNames;

  public DeserializeFromAttribute(params string[] previousTypeNames)
  {
    this.previousTypeNames = previousTypeNames;
  }
}
