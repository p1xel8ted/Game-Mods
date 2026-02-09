// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsTypeConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsTypeConverter : fsConverter
{
  public override bool CanProcess(Type type) => typeof (Type).IsAssignableFrom(type);

  public override bool RequestCycleSupport(Type type) => false;

  public override bool RequestInheritanceSupport(Type type) => false;

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    Type type = (Type) instance;
    serialized = new fsData(type.FullName);
    return fsResult.Success;
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    if (!data.IsString)
      return fsResult.Fail("Type converter requires a string");
    instance = (object) fsTypeCache.GetType(data.AsString);
    return instance == null ? fsResult.Fail("Unable to find type " + data.AsString) : fsResult.Success;
  }

  public override object CreateInstance(fsData data, Type storageType) => (object) storageType;
}
