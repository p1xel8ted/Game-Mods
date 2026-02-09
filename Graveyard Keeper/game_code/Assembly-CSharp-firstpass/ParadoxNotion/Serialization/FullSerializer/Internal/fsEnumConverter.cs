// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsEnumConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsEnumConverter : fsConverter
{
  public override bool CanProcess(Type type) => type.Resolve().IsEnum;

  public override bool RequestCycleSupport(Type storageType) => false;

  public override bool RequestInheritanceSupport(Type storageType) => false;

  public override object CreateInstance(fsData data, Type storageType)
  {
    return Enum.ToObject(storageType, (object) 0);
  }

  public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
  {
    if (this.Serializer.Config.SerializeEnumsAsInteger)
      serialized = new fsData(Convert.ToInt64(instance));
    else if (fsPortableReflection.GetAttribute<FlagsAttribute>((MemberInfo) storageType) != null)
    {
      long int64_1 = Convert.ToInt64(instance);
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (object obj in Enum.GetValues(storageType))
      {
        long int64_2 = Convert.ToInt64(obj);
        if ((int64_1 & int64_2) != 0L)
        {
          if (!flag)
            stringBuilder.Append(",");
          flag = false;
          stringBuilder.Append(obj.ToString());
        }
      }
      serialized = new fsData(stringBuilder.ToString());
    }
    else
      serialized = new fsData(Enum.GetName(storageType, instance));
    return fsResult.Success;
  }

  public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
  {
    if (data.IsString)
    {
      string[] strArray = data.AsString.Split(new char[1]
      {
        ','
      }, StringSplitOptions.RemoveEmptyEntries);
      long num1 = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = strArray[index];
        if (!fsEnumConverter.ArrayContains<string>(Enum.GetNames(storageType), str))
          return fsResult.Fail($"Cannot find enum name {str} on type {storageType?.ToString()}");
        long num2 = (long) Convert.ChangeType(Enum.Parse(storageType, str), typeof (long));
        num1 |= num2;
      }
      instance = Enum.ToObject(storageType, (object) num1);
      return fsResult.Success;
    }
    if (!data.IsInt64)
      return fsResult.Fail("EnumConverter encountered an unknown JSON data type");
    int asInt64 = (int) data.AsInt64;
    instance = Enum.ToObject(storageType, (object) asInt64);
    return fsResult.Success;
  }

  public static bool ArrayContains<T>(T[] values, T value)
  {
    for (int index = 0; index < values.Length; ++index)
    {
      if (EqualityComparer<T>.Default.Equals(values[index], value))
        return true;
    }
    return false;
  }
}
