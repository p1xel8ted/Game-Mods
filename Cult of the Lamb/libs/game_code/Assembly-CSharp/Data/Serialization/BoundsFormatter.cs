// Decompiled with JetBrains decompiler
// Type: Data.Serialization.BoundsFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class BoundsFormatter : IMessagePackFormatter<Bounds>, IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    Bounds value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(2);
    IMessagePackFormatter<Vector3> formatterWithVerify = options.Resolver.GetFormatterWithVerify<Vector3>();
    formatterWithVerify.Serialize(ref writer, value.center, options);
    formatterWithVerify.Serialize(ref writer, value.size, options);
  }

  public Bounds Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    IMessagePackFormatter<Vector3> formatterWithVerify = options.Resolver.GetFormatterWithVerify<Vector3>();
    return new Bounds(formatterWithVerify.Deserialize(ref reader, options), formatterWithVerify.Deserialize(ref reader, options));
  }
}
