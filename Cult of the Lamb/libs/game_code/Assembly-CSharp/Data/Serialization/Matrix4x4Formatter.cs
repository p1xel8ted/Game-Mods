// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Matrix4x4Formatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class Matrix4x4Formatter : IMessagePackFormatter<Matrix4x4>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Matrix4x4 value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      writer.Write(value[index]);
  }

  public Matrix4x4 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    Matrix4x4 matrix4x4 = new Matrix4x4();
    for (int index = 0; index < 16 /*0x10*/; ++index)
      matrix4x4[index] = reader.ReadSingle();
    return matrix4x4;
  }
}
