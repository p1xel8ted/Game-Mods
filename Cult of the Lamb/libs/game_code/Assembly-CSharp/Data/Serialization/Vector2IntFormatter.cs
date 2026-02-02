// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector2IntFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Vector2IntFormatter : IMessagePackFormatter<Vector2Int>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Vector2Int value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(2);
    writer.Write(value.x);
    writer.Write(value.y);
  }

  public Vector2Int Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    return new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
  }
}
