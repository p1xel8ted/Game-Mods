// Decompiled with JetBrains decompiler
// Type: Data.Serialization.RectFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class RectFormatter : IMessagePackFormatter<Rect>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Rect value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(4);
    writer.Write(value.x);
    writer.Write(value.y);
    writer.Write(value.width);
    writer.Write(value.height);
  }

  public Rect Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    double x = (double) reader.ReadSingle();
    float num1 = reader.ReadSingle();
    float num2 = reader.ReadSingle();
    float num3 = reader.ReadSingle();
    double y = (double) num1;
    double width = (double) num2;
    double height = (double) num3;
    return new Rect((float) x, (float) y, (float) width, (float) height);
  }
}
