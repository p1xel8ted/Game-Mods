// Decompiled with JetBrains decompiler
// Type: NGTools.Network.Packet
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NGTools.Network;

public abstract class Packet
{
  public static Dictionary<System.Type, int> cachedPacketId = new Dictionary<System.Type, int>();
  public static Dictionary<System.Type, FieldInfo[]> cachedPacketFields = new Dictionary<System.Type, FieldInfo[]>();
  public int packetId;
  public bool isBatchable;

  public Packet()
  {
    if (Packet.cachedPacketId.TryGetValue(this.GetType(), out this.packetId))
      return;
    PacketLinkToAttribute[] customAttributes = this.GetType().GetCustomAttributes(typeof (PacketLinkToAttribute), true) as PacketLinkToAttribute[];
    this.packetId = customAttributes.Length == 1 ? customAttributes[0].packetId : throw new MissingComponentException("Missing attribute PacketLinkToAttribute on " + this.ToString());
    this.isBatchable = customAttributes[0].isBatchable;
    Packet.cachedPacketId.Add(this.GetType(), this.packetId);
  }

  public Packet(ByteBuffer buffer)
    : this()
  {
    try
    {
      this.In(buffer);
    }
    catch (Exception ex)
    {
      InternalNGDebug.LogFileException(this.GetType().ToString(), ex);
      throw;
    }
  }

  public virtual void Out(ByteBuffer buffer)
  {
    FieldInfo[] fields = this.GetFields(this.GetType());
    for (int index = 0; index < fields.Length; ++index)
    {
      if (System.Type.op_Equality(fields[index].FieldType, typeof (string)))
        buffer.AppendUnicodeString((string) fields[index].GetValue((object) this));
      else if (fields[index].FieldType.IsEnum())
        buffer.Append((int) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (bool)))
        buffer.Append((bool) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (byte)))
        buffer.Append((byte) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (sbyte)))
        buffer.Append((sbyte) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (char)))
        buffer.Append((char) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (float)))
        buffer.Append((float) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (double)))
        buffer.Append((double) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (short)))
        buffer.Append((short) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (int)))
        buffer.Append((int) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (long)))
        buffer.Append((long) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (ushort)))
        buffer.Append((ushort) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (uint)))
        buffer.Append((uint) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (ulong)))
        buffer.Append((ulong) fields[index].GetValue((object) this));
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (Vector2)))
      {
        Vector2 vector2 = (Vector2) fields[index].GetValue((object) this);
        buffer.Append(vector2.x);
        buffer.Append(vector2.y);
      }
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (Vector3)))
      {
        Vector3 vector3 = (Vector3) fields[index].GetValue((object) this);
        buffer.Append(vector3.x);
        buffer.Append(vector3.y);
        buffer.Append(vector3.z);
      }
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (Vector4)))
      {
        Vector4 vector4 = (Vector4) fields[index].GetValue((object) this);
        buffer.Append(vector4.x);
        buffer.Append(vector4.y);
        buffer.Append(vector4.z);
        buffer.Append(vector4.w);
      }
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (Rect)))
      {
        Rect rect = (Rect) fields[index].GetValue((object) this);
        buffer.Append(rect.x);
        buffer.Append(rect.y);
        buffer.Append(rect.width);
        buffer.Append(rect.height);
      }
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (Quaternion)))
      {
        Quaternion quaternion = (Quaternion) fields[index].GetValue((object) this);
        buffer.Append(quaternion.x);
        buffer.Append(quaternion.y);
        buffer.Append(quaternion.z);
        buffer.Append(quaternion.w);
      }
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (System.Type)))
        buffer.AppendUnicodeString(((System.Type) fields[index].GetValue((object) this)).GetShortAssemblyType());
      else if (System.Type.op_Equality(fields[index].FieldType, typeof (byte[])))
      {
        byte[] src = (byte[]) fields[index].GetValue((object) this);
        buffer.Append(src.Length);
        buffer.Append((Array) src);
      }
      else
      {
        if (!fields[index].FieldType.IsUnityArray())
          throw new NotSupportedException($"Type \"{fields[index].FieldType?.ToString()}\" is not supported.");
        ICollectionModifier collectionModifier = Utility.GetCollectionModifier(fields[index].GetValue((object) this));
        buffer.Append(collectionModifier.Size);
        this.AppendArrayToBuffer(collectionModifier, Utility.GetArraySubType(fields[index].FieldType), buffer);
      }
    }
  }

  public virtual void In(ByteBuffer buffer)
  {
    FieldInfo[] fields = this.GetFields(this.GetType());
    for (int index1 = 0; index1 < fields.Length; ++index1)
    {
      if (System.Type.op_Equality(fields[index1].FieldType, typeof (int)))
        fields[index1].SetValue((object) this, (object) buffer.ReadInt32());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (string)))
        fields[index1].SetValue((object) this, (object) buffer.ReadUnicodeString());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (float)))
        fields[index1].SetValue((object) this, (object) buffer.ReadSingle());
      else if (fields[index1].FieldType.IsEnum())
        fields[index1].SetValue((object) this, (object) buffer.ReadInt32());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (bool)))
        fields[index1].SetValue((object) this, (object) buffer.ReadBoolean());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (byte)))
        fields[index1].SetValue((object) this, (object) buffer.ReadByte());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (sbyte)))
        fields[index1].SetValue((object) this, (object) buffer.ReadSByte());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (char)))
        fields[index1].SetValue((object) this, (object) buffer.ReadChar());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (double)))
        fields[index1].SetValue((object) this, (object) buffer.ReadDouble());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (short)))
        fields[index1].SetValue((object) this, (object) buffer.ReadInt16());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (long)))
        fields[index1].SetValue((object) this, (object) buffer.ReadInt64());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (ushort)))
        fields[index1].SetValue((object) this, (object) buffer.ReadUInt16());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (uint)))
        fields[index1].SetValue((object) this, (object) buffer.ReadUInt32());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (ulong)))
        fields[index1].SetValue((object) this, (object) buffer.ReadUInt64());
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (Vector2)))
        fields[index1].SetValue((object) this, (object) new Vector2(buffer.ReadSingle(), buffer.ReadSingle()));
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (Vector3)))
        fields[index1].SetValue((object) this, (object) new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle()));
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (Vector4)))
        fields[index1].SetValue((object) this, (object) new Vector4(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle()));
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (Rect)))
        fields[index1].SetValue((object) this, (object) new Rect(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle()));
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (Quaternion)))
        fields[index1].SetValue((object) this, (object) new Quaternion(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle()));
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (System.Type)))
        fields[index1].SetValue((object) this, (object) System.Type.GetType(buffer.ReadUnicodeString()));
      else if (System.Type.op_Equality(fields[index1].FieldType, typeof (byte[])))
      {
        fields[index1].SetValue((object) this, (object) buffer.ReadBytes(buffer.ReadInt32()));
      }
      else
      {
        if (!fields[index1].FieldType.IsUnityArray())
          throw new NotSupportedException($"Type \"{fields[index1].FieldType?.ToString()}\" is not supported.");
        object instance1;
        if (fields[index1].FieldType.IsArray)
        {
          instance1 = (object) Array.CreateInstance(fields[index1].FieldType.GetElementType(), buffer.ReadInt32());
        }
        else
        {
          int num = buffer.ReadInt32();
          instance1 = Activator.CreateInstance(fields[index1].FieldType, (object) num);
          IList list = (IList) instance1;
          object instance2 = !Utility.GetArraySubType(fields[index1].FieldType).IsValueType() ? (object) null : Activator.CreateInstance(Utility.GetArraySubType(fields[index1].FieldType));
          for (int index2 = 0; index2 < num; ++index2)
            list.Add(instance2);
        }
        this.ReadArrayFromBuffer(Utility.GetCollectionModifier(instance1), Utility.GetArraySubType(fields[index1].FieldType), buffer);
        fields[index1].SetValue((object) this, instance1);
      }
    }
  }

  public virtual void OnGUI(IUnityData unityData) => GUILayout.Label(this.GetType().Name);

  public virtual bool AggregateInto(Packet x) => false;

  public void AppendArrayToBuffer(ICollectionModifier array, System.Type type, ByteBuffer buffer)
  {
    if (System.Type.op_Equality(type, typeof (string)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.AppendUnicodeString((string) array.Get(index));
    }
    else if (type.IsEnum())
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((int) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (bool)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((bool) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (byte)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((byte) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (sbyte)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((sbyte) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (char)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((char) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (float)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((float) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (double)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((double) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (short)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((short) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (int)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((int) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (long)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((long) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (ushort)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((ushort) array.Get(index));
    }
    else if (System.Type.op_Equality(type, typeof (uint)))
    {
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((uint) array.Get(index));
    }
    else
    {
      if (!System.Type.op_Equality(type, typeof (ulong)))
        return;
      for (int index = 0; index < array.Size; ++index)
        buffer.Append((ulong) array.Get(index));
    }
  }

  public void ReadArrayFromBuffer(ICollectionModifier array, System.Type type, ByteBuffer buffer)
  {
    if (System.Type.op_Equality(type, typeof (string)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadUnicodeString());
    }
    else if (type.IsEnum())
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadInt32());
    }
    else if (System.Type.op_Equality(type, typeof (bool)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadBoolean());
    }
    else if (System.Type.op_Equality(type, typeof (byte)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadByte());
    }
    else if (System.Type.op_Equality(type, typeof (sbyte)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadSByte());
    }
    else if (System.Type.op_Equality(type, typeof (char)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadChar());
    }
    else if (System.Type.op_Equality(type, typeof (float)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadSingle());
    }
    else if (System.Type.op_Equality(type, typeof (double)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadDouble());
    }
    else if (System.Type.op_Equality(type, typeof (short)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadInt16());
    }
    else if (System.Type.op_Equality(type, typeof (int)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadInt32());
    }
    else if (System.Type.op_Equality(type, typeof (long)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadInt64());
    }
    else if (System.Type.op_Equality(type, typeof (ushort)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadUInt16());
    }
    else if (System.Type.op_Equality(type, typeof (uint)))
    {
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadUInt32());
    }
    else
    {
      if (!System.Type.op_Equality(type, typeof (ulong)))
        return;
      for (int index = 0; index < array.Size; ++index)
        array.Set(index, (object) buffer.ReadUInt64());
    }
  }

  public FieldInfo[] GetFields(System.Type type)
  {
    FieldInfo[] array;
    if (!Packet.cachedPacketFields.TryGetValue(type, out array))
    {
      List<FieldInfo> fieldInfoList = new List<FieldInfo>((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public));
      for (int index = 0; index < fieldInfoList.Count; ++index)
      {
        if (fieldInfoList[index].IsDefined(typeof (StripFromNetworkAttribute), false))
        {
          fieldInfoList.RemoveAt(index);
          --index;
        }
      }
      array = fieldInfoList.ToArray();
      Packet.cachedPacketFields.Add(type, array);
    }
    return array;
  }
}
