// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Color32Formatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Color32Formatter : IMessagePackFormatter<Color32>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Color32 value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(4);
    writer.Write(value.r);
    writer.Write(value.g);
    writer.Write(value.b);
    writer.Write(value.a);
  }

  public Color32 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    int r = (int) reader.ReadByte();
    byte num1 = reader.ReadByte();
    byte num2 = reader.ReadByte();
    byte num3 = reader.ReadByte();
    int g = (int) num1;
    int b = (int) num2;
    int a = (int) num3;
    return new Color32((byte) r, (byte) g, (byte) b, (byte) a);
  }
}
