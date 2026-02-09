// Decompiled with JetBrains decompiler
// Type: SmartSerializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

#nullable disable
public static class SmartSerializer
{
  public const int VERSION = 1;
  public static System.Type TYPE_SERIALIZED_FIELD = typeof (SerializeField);
  public static System.Type TYPE_SMART_SERIALIZE = typeof (SmartSerialize);
  public static System.Type TYPE_SMART_DONT_SERIALIZE = typeof (SmartDontSerialize);
  public static System.Type TYPE_INTERFACE = typeof (ISmartCustomSerialize);
  public static System.Type TYPE_COMPONENT = typeof (Component);
  public static System.Type TYPE_MONOBEHAVIOUR = typeof (MonoBehaviour);
  public static Dictionary<System.Type, List<FieldInfo>> _fields_cache = new Dictionary<System.Type, List<FieldInfo>>();
  public static Dictionary<System.Type, Dictionary<int, FieldInfo>> _fields_hashes_cache = new Dictionary<System.Type, Dictionary<int, FieldInfo>>();
  public static Dictionary<string, int> _string_hashes = new Dictionary<string, int>();
  public static char[] _buffer = new char[10240];
  public static StringBuilder _sb = new StringBuilder();

  public static byte[] Serialize<T>(T o) where T : class
  {
    MemoryStream memoryStream;
    Stream stream = (Stream) (memoryStream = new MemoryStream());
    SmartSerializer.Serialize<T>(o, stream);
    return memoryStream.ToArray();
  }

  public static void Serialize<T>(T o, Stream stream) where T : class
  {
    BinaryWriter bw = new BinaryWriter(stream);
    bw.Write(0L);
    SmartSerializer.WriteHeader(bw, new SmartSerializer.Header());
    SmartSerializer.SerializerData sd = new SmartSerializer.SerializerData();
    SmartSerializer.SerializeInternal<T>(o, bw, sd);
    SmartSerializer.WriteSerializerData(bw, sd);
    bw.Close();
  }

  public static T Deserialize<T>(byte[] data) where T : new()
  {
    return SmartSerializer.Deserialize<T>((Stream) new MemoryStream(data));
  }

  public static T Deserialize<T>(Stream stream) where T : new()
  {
    SmartSerializer.ClearCache();
    stream.Seek(0L, SeekOrigin.Begin);
    BinaryReader br = new BinaryReader(stream);
    SmartSerializer.SerializerData sd = SmartSerializer.ReadSerializerData(br);
    SmartSerializer.ReadHeader(br);
    return SmartSerializer.DeserializeInternal<T>(br, sd);
  }

  public static void DeserializeInto<T>(T obj, byte[] data)
  {
    SmartSerializer.ClearCache();
    BinaryReader br = new BinaryReader((Stream) new MemoryStream(data));
    SmartSerializer.SerializerData sd = SmartSerializer.ReadSerializerData(br);
    SmartSerializer.ReadHeader(br);
    SmartSerializer.DeserializeIntoInternal((object) obj, typeof (T), br, sd);
  }

  public static void SerializeInternal<T>(T o, BinaryWriter bw, SmartSerializer.SerializerData sd) where T : class
  {
    SmartSerializer.ClearCache();
    SmartSerializer.SerializeInternal(typeof (T), (object) o, bw, sd);
  }

  public static void ClearCache()
  {
    SmartSerializer._fields_cache.Clear();
    SmartSerializer._fields_hashes_cache.Clear();
    SmartSerializer._string_hashes.Clear();
  }

  public static List<FieldInfo> GetSerializableFieldsForType(System.Type obj_type)
  {
    List<FieldInfo> serializableFieldsForType1;
    if (SmartSerializer._fields_cache.TryGetValue(obj_type, out serializableFieldsForType1))
      return serializableFieldsForType1;
    List<FieldInfo> serializableFieldsForType2 = new List<FieldInfo>();
    foreach (FieldInfo field in obj_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      if (((field.IsPublic && !field.IsNotSerialized || Attribute.IsDefined((MemberInfo) field, SmartSerializer.TYPE_SERIALIZED_FIELD, true) ? 1 : (Attribute.IsDefined((MemberInfo) field, SmartSerializer.TYPE_SMART_SERIALIZE, true) ? 1 : 0)) == 0 || Attribute.IsDefined((MemberInfo) field, SmartSerializer.TYPE_SMART_DONT_SERIALIZE) || obj_type.IsSubclassOf(SmartSerializer.TYPE_COMPONENT) ? 0 : (!obj_type.IsSubclassOf(SmartSerializer.TYPE_MONOBEHAVIOUR) ? 1 : 0)) != 0)
        serializableFieldsForType2.Add(field);
    }
    SmartSerializer._fields_cache.Add(obj_type, serializableFieldsForType2);
    return serializableFieldsForType2;
  }

  public static Dictionary<int, FieldInfo> GetFieldsHashesForType(System.Type obj_type)
  {
    Dictionary<int, FieldInfo> fieldsHashesForType1;
    if (System.Type.op_Inequality(obj_type, (System.Type) null) && SmartSerializer._fields_hashes_cache.TryGetValue(obj_type, out fieldsHashesForType1))
      return fieldsHashesForType1;
    Dictionary<int, FieldInfo> fieldsHashesForType2 = new Dictionary<int, FieldInfo>();
    if (System.Type.op_Equality(obj_type, (System.Type) null))
      return fieldsHashesForType2;
    foreach (FieldInfo fieldInfo in SmartSerializer.GetSerializableFieldsForType(obj_type))
      fieldsHashesForType2.Add(fieldInfo.Name.GetStableHashCode(), fieldInfo);
    SmartSerializer._fields_hashes_cache.Add(obj_type, fieldsHashesForType2);
    return fieldsHashesForType2;
  }

  public static void SerializeInternal(
    System.Type obj_type,
    object o,
    BinaryWriter bw,
    SmartSerializer.SerializerData sd)
  {
    if (o == null)
    {
      bw.Write(-1);
    }
    else
    {
      List<FieldInfo> serializableFieldsForType = SmartSerializer.GetSerializableFieldsForType(obj_type);
      if (SmartSerializer.TYPE_INTERFACE.IsAssignableFrom(obj_type))
        obj_type.GetMethod("OnSmartPreSerialize").Invoke(o, (object[]) null);
      bw.Write(serializableFieldsForType.Count);
      foreach (FieldInfo fieldInfo in serializableFieldsForType)
      {
        System.Type fieldType = fieldInfo.FieldType;
        object o1 = fieldInfo.GetValue(o);
        int stableHashCode = fieldInfo.Name.GetStableHashCode();
        bw.Write(stableHashCode);
        if (!SmartSerializer.TrySerializeObject(fieldType, bw, o1, sd))
        {
          if (fieldType.IsPrimitive)
          {
            Debug.LogError((object) ("Unsupported serialization type: " + fieldType.Name));
          }
          else
          {
            bw.Write((byte) 250);
            SmartSerializer.SerializeInternal(fieldType, o1, bw, sd);
          }
        }
        bw.Flush();
      }
    }
  }

  public static bool TrySerializeObject(
    System.Type type,
    BinaryWriter bw,
    object value,
    SmartSerializer.SerializerData sd)
  {
    if (value == null)
    {
      bw.Write((byte) 0);
      return true;
    }
    if (System.Type.op_Equality(type, typeof (string)))
    {
      string s = (string) value;
      if (s.Length == 0)
        bw.Write((byte) 11);
      else if (s.Length > 30)
      {
        bw.Write((byte) 9);
        SmartSerializer.WriteStringToStream(s, bw);
      }
      else
      {
        bw.Write((byte) 10);
        SmartSerializer.WriteIndexedStringToStream(s, bw, sd);
      }
      return true;
    }
    if (System.Type.op_Equality(type, typeof (int)))
    {
      int num = (int) value;
      switch (num)
      {
        case 0:
          bw.Write((byte) 16 /*0x10*/);
          break;
        case 1:
          bw.Write((byte) 17);
          break;
        default:
          bw.Write((byte) 3);
          bw.Write(num);
          break;
      }
      return true;
    }
    if (System.Type.op_Equality(type, typeof (long)))
    {
      bw.Write((byte) 4);
      bw.Write((long) value);
      return true;
    }
    if (System.Type.op_Equality(type, typeof (bool)))
    {
      bw.Write((bool) value ? (byte) 1 : (byte) 2);
      return true;
    }
    if (System.Type.op_Equality(type, typeof (float)))
    {
      float num = (float) value;
      if ((double) num == 0.0)
        bw.Write((byte) 18);
      else if ((double) num == 1.0)
      {
        bw.Write((byte) 19);
      }
      else
      {
        bw.Write((byte) 5);
        bw.Write(num);
      }
      return true;
    }
    if (System.Type.op_Equality(type, typeof (double)))
    {
      bw.Write((byte) 6);
      bw.Write((double) value);
      return true;
    }
    if (System.Type.op_Equality(type, typeof (byte)))
    {
      bw.Write((byte) 7);
      bw.Write((byte) value);
      return true;
    }
    if (System.Type.op_Equality(type, typeof (char)))
    {
      bw.Write((byte) 8);
      bw.Write((char) value);
      return true;
    }
    if (System.Type.op_Equality(type, typeof (Vector2)))
    {
      Vector2 vector2 = (Vector2) value;
      if ((double) vector2.x == 0.0 && (double) vector2.y == 0.0)
        bw.Write((byte) 20);
      else if ((double) vector2.x == 1.0 && (double) vector2.y == 1.0)
      {
        bw.Write((byte) 21);
      }
      else
      {
        bw.Write((byte) 13);
        bw.Write(vector2.x);
        bw.Write(vector2.y);
      }
      return true;
    }
    if (System.Type.op_Equality(type, typeof (Vector3)))
    {
      Vector3 vector3 = (Vector3) value;
      if ((double) vector3.x == 0.0 && (double) vector3.y == 0.0 && (double) vector3.z == 0.0)
        bw.Write((byte) 22);
      else if ((double) vector3.x == 1.0 && (double) vector3.y == 1.0 && (double) vector3.z == 1.0)
      {
        bw.Write((byte) 23);
      }
      else
      {
        bw.Write((byte) 14);
        bw.Write(vector3.x);
        bw.Write(vector3.y);
        bw.Write(vector3.z);
      }
      return true;
    }
    if (System.Type.op_Equality(type, typeof (Quaternion)))
    {
      Quaternion quaternion = (Quaternion) value;
      if ((double) quaternion.x == 0.0 && (double) quaternion.y == 0.0 && (double) quaternion.z == 0.0 && (double) quaternion.w == 1.0)
      {
        bw.Write((byte) 24);
      }
      else
      {
        bw.Write((byte) 15);
        bw.Write(quaternion.x);
        bw.Write(quaternion.y);
        bw.Write(quaternion.z);
        bw.Write(quaternion.w);
      }
      return true;
    }
    if (System.Type.op_Equality(type, typeof (byte[])))
    {
      bw.Write((byte) 102);
      byte[] buffer = (byte[]) value;
      bw.Write(buffer.Length);
      bw.Write(buffer);
      return true;
    }
    if (type.IsArray)
    {
      System.Type elementType = type.GetElementType();
      Array array = value as Array;
      bw.Write((byte) 101);
      bw.Write(array.Length);
      for (int index = 0; index < array.Length; ++index)
      {
        object o = array.GetValue(index);
        if (!SmartSerializer.TrySerializeObject(elementType, bw, o, sd))
        {
          bw.Write((byte) 250);
          SmartSerializer.SerializeInternal(elementType, o, bw, sd);
        }
      }
      return true;
    }
    if (!(value is IList) || !type.IsGenericType)
      return false;
    IList list = value as IList;
    bw.Write((byte) 100);
    bw.Write(list.Count);
    System.Type genericArgument = type.GetGenericArguments()[0];
    for (int index = 0; index < list.Count; ++index)
    {
      object o = list[index];
      if (!SmartSerializer.TrySerializeObject(genericArgument, bw, o, sd))
      {
        bw.Write((byte) 250);
        SmartSerializer.SerializeInternal(genericArgument, o, bw, sd);
      }
    }
    return true;
  }

  public static T DeserializeInternal<T>(BinaryReader br, SmartSerializer.SerializerData sd) where T : new()
  {
    return (T) SmartSerializer.DeserializeInternal(typeof (T), br, sd);
  }

  public static object DeserializeInternal(
    System.Type obj_type,
    BinaryReader br,
    SmartSerializer.SerializerData sd)
  {
    return SmartSerializer.DeserializeIntoInternal(System.Type.op_Equality(obj_type, (System.Type) null) ? (object) null : Activator.CreateInstance(obj_type), obj_type, br, sd);
  }

  public static object DeserializeIntoInternal(
    object obj,
    System.Type obj_type,
    BinaryReader br,
    SmartSerializer.SerializerData sd)
  {
    Dictionary<int, FieldInfo> fieldsHashesForType = SmartSerializer.GetFieldsHashesForType(obj_type);
    int num = br.ReadInt32();
    if (num == -1)
      return (object) null;
    for (int index = 0; index < num; ++index)
    {
      int key = br.ReadInt32();
      FieldInfo fieldInfo = (FieldInfo) null;
      fieldsHashesForType.TryGetValue(key, out fieldInfo);
      if (FieldInfo.op_Equality(fieldInfo, (FieldInfo) null))
        Debug.LogWarning((object) ("Can't find a field to deserialize: " + key.ToString()));
      object obj1;
      try
      {
        obj1 = SmartSerializer.DeserializeObject(FieldInfo.op_Equality(fieldInfo, (FieldInfo) null) ? (System.Type) null : fieldInfo.FieldType, br, sd);
      }
      catch (MissingMethodException ex)
      {
        if (FieldInfo.op_Inequality(fieldInfo, (FieldInfo) null))
          Debug.LogError((object) $"Error deserializing object: {fieldInfo.FieldType?.ToString()}, name = {fieldInfo.Name}");
        throw;
      }
      if (FieldInfo.op_Inequality(fieldInfo, (FieldInfo) null))
      {
        if (obj != null)
          fieldInfo.SetValue(obj, obj1);
      }
      else
        Debug.LogError((object) $"Couldn't find a field to deserialize, obj_type = {obj_type?.ToString()}, field_type = {(obj1 == null ? "null" : obj1.GetType().ToString())}, hash_size = {fieldsHashesForType.Count.ToString()}");
    }
    if (System.Type.op_Inequality(obj_type, (System.Type) null) && SmartSerializer.TYPE_INTERFACE.IsAssignableFrom(obj_type))
      obj_type.GetMethod("OnSmartPostDeserialize").Invoke(obj, (object[]) null);
    return obj;
  }

  public static object DeserializeObject(
    System.Type obj_type,
    BinaryReader br,
    SmartSerializer.SerializerData sd)
  {
    SmartSerializer.FieldType fieldType = (SmartSerializer.FieldType) br.ReadByte();
    switch (fieldType)
    {
      case SmartSerializer.FieldType.NullValue:
        return (object) null;
      case SmartSerializer.FieldType.Bool_True:
        return (object) true;
      case SmartSerializer.FieldType.Bool_False:
        return (object) false;
      case SmartSerializer.FieldType.Int32:
        return (object) br.ReadInt32();
      case SmartSerializer.FieldType.Int64:
        return (object) br.ReadInt64();
      case SmartSerializer.FieldType.Single:
        return (object) br.ReadSingle();
      case SmartSerializer.FieldType.Double:
        return (object) br.ReadDouble();
      case SmartSerializer.FieldType.Byte:
        return (object) br.ReadByte();
      case SmartSerializer.FieldType.Char:
        return (object) br.ReadChar();
      case SmartSerializer.FieldType.String:
        return (object) SmartSerializer.ReadStringFromStream(br);
      case SmartSerializer.FieldType.String_Indexed:
        return (object) SmartSerializer.ReadIndexedStringFromStream(br, sd);
      case SmartSerializer.FieldType.String_Empty:
        return (object) string.Empty;
      case SmartSerializer.FieldType.Vector2:
        return (object) new Vector2(br.ReadSingle(), br.ReadSingle());
      case SmartSerializer.FieldType.Vector3:
        return (object) new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
      case SmartSerializer.FieldType.Quaternion:
        return (object) new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
      case SmartSerializer.FieldType.Int32_0:
        return (object) 0;
      case SmartSerializer.FieldType.Int32_1:
        return (object) 1;
      case SmartSerializer.FieldType.Single_0:
        return (object) 0.0f;
      case SmartSerializer.FieldType.Single_1:
        return (object) 1f;
      case SmartSerializer.FieldType.Vector2_00:
        return (object) Vector2.zero;
      case SmartSerializer.FieldType.Vector2_11:
        return (object) Vector2.one;
      case SmartSerializer.FieldType.Vector3_000:
        return (object) Vector3.zero;
      case SmartSerializer.FieldType.Vector3_111:
        return (object) Vector3.one;
      case SmartSerializer.FieldType.Quaternion_0001:
        return (object) new Quaternion(0.0f, 0.0f, 0.0f, 1f);
      case SmartSerializer.FieldType.GenericList:
        int num = br.ReadInt32();
        System.Type obj_type1 = System.Type.op_Equality(obj_type, (System.Type) null) ? typeof (object) : obj_type.GetGenericArguments()[0];
        IList instance1 = (IList) Activator.CreateInstance(typeof (List<>).MakeGenericType(obj_type1));
        for (int index = 0; index < num; ++index)
        {
          object obj = SmartSerializer.DeserializeObject(obj_type1, br, sd);
          if (System.Type.op_Inequality(obj_type, (System.Type) null))
            instance1.Add(obj);
        }
        return (object) instance1;
      case SmartSerializer.FieldType.Array:
        int length = br.ReadInt32();
        System.Type type = System.Type.op_Equality(obj_type, (System.Type) null) ? typeof (object) : obj_type.GetElementType();
        Array instance2 = System.Type.op_Equality(type, (System.Type) null) ? (Array) null : Array.CreateInstance(type, length);
        for (int index = 0; index < length; ++index)
        {
          object obj = SmartSerializer.DeserializeObject(type, br, sd);
          instance2?.SetValue(obj, index);
        }
        return (object) instance2;
      case SmartSerializer.FieldType.ByteArray:
        int count = br.ReadInt32();
        return (object) br.ReadBytes(count);
      case SmartSerializer.FieldType.SmartSerialized:
        return SmartSerializer.DeserializeInternal(obj_type, br, sd);
      default:
        throw new NotImplementedException("Not implemented desearialization of a field type: " + fieldType.ToString());
    }
  }

  public static string ReadStringFromStream(BinaryReader br, bool encrypt = false)
  {
    int num1 = br.ReadInt32();
    if (num1 == -1)
      return (string) null;
    int num2 = num1;
    SmartSerializer._sb.Length = 0;
    while (num2 > 0)
    {
      int num3 = num2 <= SmartSerializer._buffer.Length ? num2 : SmartSerializer._buffer.Length;
      num2 -= num3;
      long num4 = (long) br.Read(SmartSerializer._buffer, 0, num3);
      if (encrypt)
      {
        for (int index = 0; (long) index < num4; ++index)
        {
          uint num5 = (uint) SmartSerializer._buffer[index];
          if (num5 <= (uint) byte.MaxValue && num5 != 0U && num5 != 109U)
          {
            uint num6 = num5 ^ 109U;
            SmartSerializer._buffer[index] = (char) num6;
          }
        }
      }
      SmartSerializer._sb.Append(SmartSerializer._buffer, 0, num3);
      if (num4 < (long) num3)
        throw new EndOfStreamException();
    }
    return SmartSerializer._sb.ToString();
  }

  public static string ReadIndexedStringFromStream(
    BinaryReader br,
    SmartSerializer.SerializerData sd)
  {
    int index = br.ReadInt32();
    return sd.strings[index];
  }

  public static void WriteIndexedStringToStream(
    string s,
    BinaryWriter bw,
    SmartSerializer.SerializerData sd)
  {
    int num = sd.strings.IndexOf(s);
    if (num == -1)
    {
      sd.strings.Add(s);
      num = sd.strings.Count - 1;
    }
    bw.Write(num);
  }

  public static void WriteStringToStream(string s, BinaryWriter bw, bool encrypt = false)
  {
    if (s == null)
    {
      bw.Write(-1);
    }
    else
    {
      char[] charArray = s.ToCharArray();
      bw.Write(charArray.Length);
      if (encrypt)
      {
        for (int index = 0; index < charArray.Length; ++index)
        {
          uint num1 = (uint) charArray[index];
          if (num1 <= (uint) byte.MaxValue && num1 != 0U && num1 != 109U)
          {
            uint num2 = num1 ^ 109U;
            charArray[index] = (char) num2;
          }
        }
      }
      bw.Write(charArray);
    }
  }

  public static SmartSerializer.Header ReadHeader(BinaryReader br)
  {
    SmartSerializer.Header header = new SmartSerializer.Header()
    {
      sdata_offset = br.ReadInt64(),
      version = br.ReadInt32()
    };
    for (int index = 0; index < 15; ++index)
      br.ReadInt32();
    return header;
  }

  public static void WriteHeader(BinaryWriter bw, SmartSerializer.Header header)
  {
    bw.Write(header.sdata_offset);
    bw.Write(header.version);
    for (int index = 0; index < 15; ++index)
      bw.Write(0);
  }

  public static void WriteSerializerData(BinaryWriter bw, SmartSerializer.SerializerData sd)
  {
    long position = bw.BaseStream.Position;
    bw.BaseStream.Position = 0L;
    bw.Write(position);
    bw.BaseStream.Position = position;
    if (sd.strings == null)
    {
      bw.Write(0);
    }
    else
    {
      bw.Write(sd.strings.Count);
      foreach (string s in sd.strings)
        SmartSerializer.WriteStringToStream(s, bw, true);
    }
  }

  public static SmartSerializer.SerializerData ReadSerializerData(BinaryReader br)
  {
    long num1 = br.ReadInt64();
    long position = br.BaseStream.Position;
    br.BaseStream.Position = num1;
    SmartSerializer.SerializerData serializerData = new SmartSerializer.SerializerData();
    int num2 = br.ReadInt32();
    for (int index = 0; index < num2; ++index)
    {
      string str = SmartSerializer.ReadStringFromStream(br, true);
      serializerData.strings.Add(str);
    }
    br.BaseStream.Position = position;
    return serializerData;
  }

  public static int GetStableHashCode(this string str)
  {
    int stableHashCode1;
    if (SmartSerializer._string_hashes.TryGetValue(str, out stableHashCode1))
      return stableHashCode1;
    int num1 = 5381;
    int num2 = num1;
    for (int index = 0; index < str.Length && str[index] != char.MinValue; index += 2)
    {
      num1 = (num1 << 5) + num1 ^ (int) str[index];
      if (index != str.Length - 1 && str[index + 1] != char.MinValue)
        num2 = (num2 << 5) + num2 ^ (int) str[index + 1];
      else
        break;
    }
    int stableHashCode2 = num1 + num2 * 1566083941;
    SmartSerializer._string_hashes.Add(str, stableHashCode2);
    return stableHashCode2;
  }

  public enum FieldType
  {
    NullValue = 0,
    Bool_True = 1,
    Bool_False = 2,
    Int32 = 3,
    Int64 = 4,
    Single = 5,
    Double = 6,
    Byte = 7,
    Char = 8,
    String = 9,
    String_Indexed = 10, // 0x0000000A
    String_Empty = 11, // 0x0000000B
    Json = 12, // 0x0000000C
    Vector2 = 13, // 0x0000000D
    Vector3 = 14, // 0x0000000E
    Quaternion = 15, // 0x0000000F
    Int32_0 = 16, // 0x00000010
    Int32_1 = 17, // 0x00000011
    Single_0 = 18, // 0x00000012
    Single_1 = 19, // 0x00000013
    Vector2_00 = 20, // 0x00000014
    Vector2_11 = 21, // 0x00000015
    Vector3_000 = 22, // 0x00000016
    Vector3_111 = 23, // 0x00000017
    Quaternion_0001 = 24, // 0x00000018
    GenericList = 100, // 0x00000064
    Array = 101, // 0x00000065
    ByteArray = 102, // 0x00000066
    SmartSerialized = 250, // 0x000000FA
  }

  public class Header
  {
    public int version = 1;
    public long sdata_offset;
  }

  public class SerializerData
  {
    public List<string> strings = new List<string>();
  }
}
