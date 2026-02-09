// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsVersionedType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public struct fsVersionedType
{
  public fsVersionedType[] Ancestors;
  public string VersionString;
  public Type ModelType;

  public object Migrate(object ancestorInstance)
  {
    return Activator.CreateInstance(this.ModelType, ancestorInstance);
  }

  public override string ToString()
  {
    return $"fsVersionedType [ModelType={this.ModelType?.ToString()}, VersionString={this.VersionString}, Ancestors.Length={this.Ancestors.Length.ToString()}]";
  }

  public static bool operator ==(fsVersionedType a, fsVersionedType b)
  {
    return Type.op_Equality(a.ModelType, b.ModelType);
  }

  public static bool operator !=(fsVersionedType a, fsVersionedType b)
  {
    return Type.op_Inequality(a.ModelType, b.ModelType);
  }

  public override bool Equals(object obj)
  {
    return obj is fsVersionedType fsVersionedType && Type.op_Equality(this.ModelType, fsVersionedType.ModelType);
  }

  public override int GetHashCode() => this.ModelType.GetHashCode();
}
