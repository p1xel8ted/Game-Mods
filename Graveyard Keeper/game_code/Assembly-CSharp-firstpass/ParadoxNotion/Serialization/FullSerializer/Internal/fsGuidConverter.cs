// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsGuidConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsGuidConverter : fsConverter
{
  public override bool CanProcess(Type type) => Type.op_Equality(type, typeof (Guid));

  public override bool RequestCycleSupport(Type storageType) => false;

  public override bool RequestInheritanceSupport(Type storageType) => false;

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    Guid guid = (Guid) instance;
    serialized = new fsData(guid.ToString());
    return fsResult.Success;
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    if (!data.IsString)
      return fsResult.Fail("fsGuidConverter encountered an unknown JSON data type");
    instance = (object) new Guid(data.AsString);
    return fsResult.Success;
  }

  public override object CreateInstance(fsData data, Type storageType) => (object) new Guid();
}
