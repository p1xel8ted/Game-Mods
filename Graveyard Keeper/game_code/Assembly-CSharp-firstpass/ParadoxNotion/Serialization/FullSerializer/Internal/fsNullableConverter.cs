// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsNullableConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsNullableConverter : fsConverter
{
  public override bool CanProcess(Type type)
  {
    return type.Resolve().IsGenericType && Type.op_Equality(type.GetGenericTypeDefinition(), typeof (Nullable<>));
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    return this.Serializer.TrySerialize(Nullable.GetUnderlyingType(storageType), instance, out serialized);
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    return this.Serializer.TryDeserialize(data, Nullable.GetUnderlyingType(storageType), ref instance);
  }

  public override object CreateInstance(fsData data, Type storageType) => (object) storageType;
}
