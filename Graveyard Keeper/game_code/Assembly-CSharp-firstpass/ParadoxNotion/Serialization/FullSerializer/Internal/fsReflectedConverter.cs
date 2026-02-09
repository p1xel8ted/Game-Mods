// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsReflectedConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsReflectedConverter : fsConverter
{
  public override bool CanProcess(System.Type type)
  {
    return !type.Resolve().IsArray && !typeof (ICollection).IsAssignableFrom(type);
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, System.Type storageType)
  {
    serialized = fsData.CreateDictionary();
    fsResult success = fsResult.Success;
    fsMetaType fsMetaType = fsMetaType.Get(this.Serializer.Config, instance.GetType());
    fsMetaType.EmitAotData();
    object context = (object) null;
    if (!fsGlobalConfig.SerializeDefaultValues && (object) (instance as UnityEngine.Object) == null)
      context = fsMetaType.CreateInstance();
    for (int index = 0; index < fsMetaType.Properties.Length; ++index)
    {
      fsMetaProperty property = fsMetaType.Properties[index];
      if (property.CanRead && (fsGlobalConfig.SerializeDefaultValues || context == null || !object.Equals(property.Read(instance), property.Read(context))))
      {
        fsData data;
        fsResult result = this.Serializer.TrySerialize(property.StorageType, property.OverrideConverterType, property.Read(instance), out data);
        success.AddMessages(result);
        if (!result.Failed)
          serialized.AsDictionary[property.JsonName] = data;
      }
    }
    return success;
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, System.Type storageType)
  {
    fsResult fsResult;
    if ((fsResult = fsResult.Success + this.CheckType(data, fsDataType.Object)).Failed)
      return fsResult;
    if (data.AsDictionary.Count == 0)
      return fsResult.Success;
    fsMetaType fsMetaType = fsMetaType.Get(this.Serializer.Config, storageType);
    fsMetaType.EmitAotData();
    for (int index = 0; index < fsMetaType.Properties.Length; ++index)
    {
      fsMetaProperty property = fsMetaType.Properties[index];
      fsData data1;
      if (property.CanWrite && data.AsDictionary.TryGetValue(property.JsonName, out data1))
      {
        object result1 = (object) null;
        fsResult result2 = this.Serializer.TryDeserialize(data1, property.StorageType, property.OverrideConverterType, ref result1);
        fsResult.AddMessages(result2);
        if (!result2.Failed)
          property.Write(instance, result1);
      }
    }
    return fsResult;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return fsMetaType.Get(this.Serializer.Config, storageType).CreateInstance();
  }
}
