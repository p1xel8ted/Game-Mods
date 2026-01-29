// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector3IntFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Vector3IntFormatter : IMessagePackFormatter<Vector3Int>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Vector3Int value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(3);
    writer.Write(value.x);
    writer.Write(value.y);
    writer.Write(value.z);
  }

  public Vector3Int Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    int x = reader.ReadInt32();
    int num1 = reader.ReadInt32();
    int num2 = reader.ReadInt32();
    int y = num1;
    int z = num2;
    return new Vector3Int(x, y, z);
  }
}
