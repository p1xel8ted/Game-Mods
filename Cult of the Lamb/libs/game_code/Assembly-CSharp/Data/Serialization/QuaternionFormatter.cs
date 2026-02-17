// Decompiled with JetBrains decompiler
// Type: Data.Serialization.QuaternionFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class QuaternionFormatter : IMessagePackFormatter<Quaternion>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Quaternion value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(4);
    writer.Write(value.x);
    writer.Write(value.y);
    writer.Write(value.z);
    writer.Write(value.w);
  }

  public Quaternion Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    double x = (double) reader.ReadSingle();
    float num1 = reader.ReadSingle();
    float num2 = reader.ReadSingle();
    float num3 = reader.ReadSingle();
    double y = (double) num1;
    double z = (double) num2;
    double w = (double) num3;
    return new Quaternion((float) x, (float) y, (float) z, (float) w);
  }
}
