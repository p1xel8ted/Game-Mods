// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsObjectAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class fsObjectAttribute : Attribute
{
  public Type[] PreviousModels;
  public string VersionString;
  public fsMemberSerialization MemberSerialization = fsMemberSerialization.Default;
  public Type Converter;
  public Type Processor;

  public fsObjectAttribute()
  {
  }

  public fsObjectAttribute(string versionString, params Type[] previousModels)
  {
    this.VersionString = versionString;
    this.PreviousModels = previousModels;
  }
}
