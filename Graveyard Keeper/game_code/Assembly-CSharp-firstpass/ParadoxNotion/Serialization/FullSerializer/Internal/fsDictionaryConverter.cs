// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsDictionaryConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsDictionaryConverter : fsConverter
{
  public override bool CanProcess(Type type) => typeof (IDictionary).IsAssignableFrom(type);

  public override object CreateInstance(fsData data, Type storageType)
  {
    return fsMetaType.Get(this.Serializer.Config, storageType).CreateInstance();
  }

  public override fsResult TryDeserialize(fsData data, ref object instance_, Type storageType)
  {
    IDictionary dictionary = (IDictionary) instance_;
    fsResult fsResult1 = fsResult.Success;
    Type keyStorageType;
    Type valueStorageType;
    fsDictionaryConverter.GetKeyValueTypes(dictionary.GetType(), out keyStorageType, out valueStorageType);
    if (data.IsList)
    {
      List<fsData> asList = data.AsList;
      for (int index = 0; index < asList.Count; ++index)
      {
        fsData data1 = asList[index];
        fsResult fsResult2;
        fsResult fsResult3 = fsResult2 = fsResult1 + this.CheckType(data1, fsDataType.Object);
        if (fsResult3.Failed)
          return fsResult2;
        fsData subitem1;
        fsResult fsResult4;
        fsResult3 = fsResult4 = fsResult2 + this.CheckKey(data1, "Key", out subitem1);
        if (fsResult3.Failed)
          return fsResult4;
        fsData subitem2;
        fsResult fsResult5;
        fsResult3 = fsResult5 = fsResult4 + this.CheckKey(data1, "Value", out subitem2);
        if (fsResult3.Failed)
          return fsResult5;
        object result1 = (object) null;
        object result2 = (object) null;
        fsResult fsResult6;
        fsResult3 = fsResult6 = fsResult5 + this.Serializer.TryDeserialize(subitem1, keyStorageType, ref result1);
        if (fsResult3.Failed)
          return fsResult6;
        fsResult3 = fsResult1 = fsResult6 + this.Serializer.TryDeserialize(subitem2, valueStorageType, ref result2);
        if (fsResult3.Failed)
          return fsResult1;
        this.AddItemToDictionary(dictionary, result1, result2);
      }
    }
    else if (data.IsDictionary)
    {
      foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
      {
        if (!fsSerializer.IsReservedKeyword(keyValuePair.Key))
        {
          fsData data2 = new fsData(keyValuePair.Key);
          fsData data3 = keyValuePair.Value;
          object result3 = (object) null;
          object result4 = (object) null;
          if ((fsResult1 += this.Serializer.TryDeserialize(data2, keyStorageType, ref result3)).Failed || (fsResult1 += this.Serializer.TryDeserialize(data3, valueStorageType, ref result4)).Failed)
            return fsResult1;
          this.AddItemToDictionary(dictionary, result3, result4);
        }
      }
    }
    else
      return this.FailExpectedType(data, fsDataType.Array, fsDataType.Object);
    return fsResult1;
  }

  public override fsResult TrySerialize(object instance_, out fsData serialized, Type storageType)
  {
    serialized = fsData.Null;
    fsResult fsResult1 = fsResult.Success;
    IDictionary dictionary = (IDictionary) instance_;
    Type keyStorageType;
    Type valueStorageType;
    fsDictionaryConverter.GetKeyValueTypes(dictionary.GetType(), out keyStorageType, out valueStorageType);
    IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
    bool flag = true;
    List<fsData> fsDataList1 = new List<fsData>(dictionary.Count);
    List<fsData> fsDataList2 = new List<fsData>(dictionary.Count);
    while (enumerator.MoveNext())
    {
      fsData data1;
      fsResult fsResult2;
      if ((fsResult2 = fsResult1 + this.Serializer.TrySerialize(keyStorageType, enumerator.Key, out data1)).Failed)
        return fsResult2;
      fsData data2;
      if ((fsResult1 = fsResult2 + this.Serializer.TrySerialize(valueStorageType, enumerator.Value, out data2)).Failed)
        return fsResult1;
      fsDataList1.Add(data1);
      fsDataList2.Add(data2);
      flag &= data1.IsString;
    }
    if (flag)
    {
      serialized = fsData.CreateDictionary();
      Dictionary<string, fsData> asDictionary = serialized.AsDictionary;
      for (int index = 0; index < fsDataList1.Count; ++index)
      {
        fsData fsData1 = fsDataList1[index];
        fsData fsData2 = fsDataList2[index];
        asDictionary[fsData1.AsString] = fsData2;
      }
    }
    else
    {
      serialized = fsData.CreateList(fsDataList1.Count);
      List<fsData> asList = serialized.AsList;
      for (int index = 0; index < fsDataList1.Count; ++index)
      {
        fsData fsData3 = fsDataList1[index];
        fsData fsData4 = fsDataList2[index];
        asList.Add(new fsData(new Dictionary<string, fsData>()
        {
          ["Key"] = fsData3,
          ["Value"] = fsData4
        }));
      }
    }
    return fsResult1;
  }

  public fsResult AddItemToDictionary(IDictionary dictionary, object key, object value)
  {
    if (key == null || value == null)
    {
      Type type = fsReflectionUtility.GetInterface(dictionary.GetType(), typeof (ICollection<>));
      if (Type.op_Equality(type, (Type) null))
        return fsResult.Warn(dictionary.GetType()?.ToString() + " does not extend ICollection");
      object instance = Activator.CreateInstance(type.GetGenericArguments()[0], key, value);
      type.GetFlattenedMethod("Add").Invoke((object) dictionary, new object[1]
      {
        instance
      });
      return fsResult.Success;
    }
    dictionary[key] = value;
    return fsResult.Success;
  }

  public static void GetKeyValueTypes(
    Type dictionaryType,
    out Type keyStorageType,
    out Type valueStorageType)
  {
    Type type = fsReflectionUtility.GetInterface(dictionaryType, typeof (IDictionary<,>));
    if (Type.op_Inequality(type, (Type) null))
    {
      Type[] genericArguments = type.GetGenericArguments();
      keyStorageType = genericArguments[0];
      valueStorageType = genericArguments[1];
    }
    else
    {
      keyStorageType = typeof (object);
      valueStorageType = typeof (object);
    }
  }
}
