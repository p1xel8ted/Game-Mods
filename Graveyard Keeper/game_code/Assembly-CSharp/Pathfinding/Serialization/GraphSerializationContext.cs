// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.GraphSerializationContext
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

#nullable disable
namespace Pathfinding.Serialization;

public class GraphSerializationContext
{
  public GraphNode[] id2NodeMapping;
  public BinaryReader reader;
  public BinaryWriter writer;
  public uint graphIndex;
  public GraphMeta meta;

  public GraphSerializationContext(
    BinaryReader reader,
    GraphNode[] id2NodeMapping,
    uint graphIndex,
    GraphMeta meta)
  {
    this.reader = reader;
    this.id2NodeMapping = id2NodeMapping;
    this.graphIndex = graphIndex;
    this.meta = meta;
  }

  public GraphSerializationContext(BinaryWriter writer) => this.writer = writer;

  public void SerializeNodeReference(GraphNode node)
  {
    this.writer.Write(node == null ? -1 : node.NodeIndex);
  }

  public GraphNode DeserializeNodeReference()
  {
    int index = this.reader.ReadInt32();
    if (this.id2NodeMapping == null)
      throw new Exception("Calling DeserializeNodeReference when serializing");
    if (index == -1)
      return (GraphNode) null;
    return this.id2NodeMapping[index] ?? throw new Exception($"Invalid id ({index.ToString()})");
  }

  public void SerializeVector3(Vector3 v)
  {
    this.writer.Write(v.x);
    this.writer.Write(v.y);
    this.writer.Write(v.z);
  }

  public Vector3 DeserializeVector3()
  {
    return new Vector3(this.reader.ReadSingle(), this.reader.ReadSingle(), this.reader.ReadSingle());
  }

  public void SerializeInt3(Int3 v)
  {
    this.writer.Write(v.x);
    this.writer.Write(v.y);
    this.writer.Write(v.z);
  }

  public Int3 DeserializeInt3()
  {
    return new Int3(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
  }

  public int DeserializeInt(int defaultValue)
  {
    return this.reader.BaseStream.Position <= this.reader.BaseStream.Length - 4L ? this.reader.ReadInt32() : defaultValue;
  }

  public float DeserializeFloat(float defaultValue)
  {
    return this.reader.BaseStream.Position <= this.reader.BaseStream.Length - 4L ? this.reader.ReadSingle() : defaultValue;
  }

  public UnityEngine.Object DeserializeUnityObject()
  {
    if (this.reader.ReadInt32() == int.MaxValue)
      return (UnityEngine.Object) null;
    string path = this.reader.ReadString();
    string typeName = this.reader.ReadString();
    string str = this.reader.ReadString();
    System.Type type = System.Type.GetType(typeName);
    if (System.Type.op_Equality(type, (System.Type) null))
    {
      Debug.LogError((object) $"Could not find type '{typeName}'. Cannot deserialize Unity reference");
      return (UnityEngine.Object) null;
    }
    if (!string.IsNullOrEmpty(str))
    {
      UnityReferenceHelper[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (UnityReferenceHelper)) as UnityReferenceHelper[];
      for (int index = 0; index < objectsOfType.Length; ++index)
      {
        if (objectsOfType[index].GetGUID() == str)
          return System.Type.op_Equality(type, typeof (GameObject)) ? (UnityEngine.Object) objectsOfType[index].gameObject : (UnityEngine.Object) objectsOfType[index].GetComponent(type);
      }
    }
    UnityEngine.Object[] objectArray = Resources.LoadAll(path, type);
    for (int index = 0; index < objectArray.Length; ++index)
    {
      if (objectArray[index].name == path || objectArray.Length == 1)
        return objectArray[index];
    }
    return (UnityEngine.Object) null;
  }
}
