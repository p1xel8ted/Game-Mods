// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector2Formatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Vector2Formatter : IMessagePackFormatter<Vector2>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Vector2 value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(2);
    writer.Write(value.x);
    writer.Write(value.y);
  }

  public Vector2 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    return new Vector2(reader.ReadSingle(), reader.ReadSingle());
  }
}
