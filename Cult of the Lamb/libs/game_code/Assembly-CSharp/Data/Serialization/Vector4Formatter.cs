// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector4Formatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Vector4Formatter : IMessagePackFormatter<Vector4>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Vector4 value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(4);
    writer.Write(value.x);
    writer.Write(value.y);
    writer.Write(value.z);
    writer.Write(value.w);
  }

  public Vector4 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    double x = (double) reader.ReadSingle();
    float num1 = reader.ReadSingle();
    float num2 = reader.ReadSingle();
    float num3 = reader.ReadSingle();
    double y = (double) num1;
    double z = (double) num2;
    double w = (double) num3;
    return new Vector4((float) x, (float) y, (float) z, (float) w);
  }
}
