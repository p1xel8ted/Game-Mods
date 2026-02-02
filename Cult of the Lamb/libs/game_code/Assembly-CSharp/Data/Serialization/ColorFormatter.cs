// Decompiled with JetBrains decompiler
// Type: Data.Serialization.ColorFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class ColorFormatter : IMessagePackFormatter<Color>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Color value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(4);
    writer.Write(value.r);
    writer.Write(value.g);
    writer.Write(value.b);
    writer.Write(value.a);
  }

  public Color Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    double r = (double) reader.ReadSingle();
    float num1 = reader.ReadSingle();
    float num2 = reader.ReadSingle();
    float num3 = reader.ReadSingle();
    double g = (double) num1;
    double b = (double) num2;
    double a = (double) num3;
    return new Color((float) r, (float) g, (float) b, (float) a);
  }
}
