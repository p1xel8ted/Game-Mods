// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsForwardConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsForwardConverter : fsConverter
{
  public string _memberName;

  public fsForwardConverter(fsForwardAttribute attribute)
  {
    this._memberName = attribute.MemberName;
  }

  public override bool CanProcess(Type type)
  {
    throw new NotSupportedException("Please use the [fsForward(...)] attribute.");
  }

  public fsResult GetProperty(object instance, out fsMetaProperty property)
  {
    fsMetaProperty[] properties = fsMetaType.Get(this.Serializer.Config, instance.GetType()).Properties;
    for (int index = 0; index < properties.Length; ++index)
    {
      if (properties[index].MemberName == this._memberName)
      {
        property = properties[index];
        return fsResult.Success;
      }
    }
    property = (fsMetaProperty) null;
    return fsResult.Fail($"No property named \"{this._memberName}\" on {instance.GetType().CSharpName()}");
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    serialized = fsData.Null;
    fsMetaProperty property;
    fsResult fsResult;
    if ((fsResult = fsResult.Success + this.GetProperty(instance, out property)).Failed)
      return fsResult;
    object instance1 = property.Read(instance);
    return this.Serializer.TrySerialize(property.StorageType, instance1, out serialized);
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    fsMetaProperty property;
    fsResult fsResult1;
    if ((fsResult1 = fsResult.Success + this.GetProperty(instance, out property)).Failed)
      return fsResult1;
    object result = (object) null;
    fsResult fsResult2;
    if ((fsResult2 = fsResult1 + this.Serializer.TryDeserialize(data, property.StorageType, ref result)).Failed)
      return fsResult2;
    property.Write(instance, result);
    return fsResult2;
  }

  public override object CreateInstance(fsData data, Type storageType)
  {
    return fsMetaType.Get(this.Serializer.Config, storageType).CreateInstance();
  }
}
