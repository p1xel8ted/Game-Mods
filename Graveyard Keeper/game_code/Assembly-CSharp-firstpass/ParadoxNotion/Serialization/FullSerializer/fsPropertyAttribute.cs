// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsPropertyAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class fsPropertyAttribute : Attribute
{
  public string Name;
  public Type Converter;

  public fsPropertyAttribute()
    : this(string.Empty)
  {
  }

  public fsPropertyAttribute(string name) => this.Name = name;
}
