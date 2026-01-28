// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector3Formatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Vector3Formatter : IMessagePackFormatter<Vector3>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Vector3 value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(3);
    writer.Write(value.x);
    writer.Write(value.y);
    writer.Write(value.z);
  }

  public Vector3 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    double x = (double) reader.ReadSingle();
    float num1 = reader.ReadSingle();
    float num2 = reader.ReadSingle();
    double y = (double) num1;
    double z = (double) num2;
    return new Vector3((float) x, (float) y, (float) z);
  }
}
