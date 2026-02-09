// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsDuplicateVersionNameException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public sealed class fsDuplicateVersionNameException(Type typeA, Type typeB, string version) : 
  Exception($"{typeA?.ToString()} and {typeB?.ToString()} have the same version string ({version}); please change one of them.")
{
}
