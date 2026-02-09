// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsKeyValuePairConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsKeyValuePairConverter : fsConverter
{
  public override bool CanProcess(Type type)
  {
    return type.Resolve().IsGenericType && Type.op_Equality(type.GetGenericTypeDefinition(), typeof (KeyValuePair<,>));
  }

  public override bool RequestCycleSupport(Type storageType) => false;

  public override bool RequestInheritanceSupport(Type storageType) => false;

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    fsData subitem1;
    fsResult fsResult1;
    if ((fsResult1 = fsResult.Success + this.CheckKey(data, "Key", out subitem1)).Failed)
      return fsResult1;
    fsData subitem2;
    fsResult fsResult2;
    if ((fsResult2 = fsResult1 + this.CheckKey(data, "Value", out subitem2)).Failed)
      return fsResult2;
    Type[] genericArguments = storageType.GetGenericArguments();
    Type storageType1 = genericArguments[0];
    Type storageType2 = genericArguments[1];
    object result1 = (object) null;
    object result2 = (object) null;
    fsResult2.AddMessages(this.Serializer.TryDeserialize(subitem1, storageType1, ref result1));
    fsResult2.AddMessages(this.Serializer.TryDeserialize(subitem2, storageType2, ref result2));
    instance = Activator.CreateInstance(storageType, result1, result2);
    return fsResult2;
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    PropertyInfo declaredProperty1 = storageType.GetDeclaredProperty("Key");
    PropertyInfo declaredProperty2 = storageType.GetDeclaredProperty("Value");
    object obj = instance;
    object instance1 = declaredProperty1.GetValue(obj, (object[]) null);
    object instance2 = declaredProperty2.GetValue(instance, (object[]) null);
    Type[] genericArguments = storageType.GetGenericArguments();
    Type storageType1 = genericArguments[0];
    Type storageType2 = genericArguments[1];
    fsResult success = fsResult.Success;
    fsData data1;
    success.AddMessages(this.Serializer.TrySerialize(storageType1, instance1, out data1));
    fsData data2;
    success.AddMessages(this.Serializer.TrySerialize(storageType2, instance2, out data2));
    serialized = fsData.CreateDictionary();
    if (data1 != (fsData) null)
      serialized.AsDictionary["Key"] = data1;
    if (data2 != (fsData) null)
      serialized.AsDictionary["Value"] = data2;
    return success;
  }
}
