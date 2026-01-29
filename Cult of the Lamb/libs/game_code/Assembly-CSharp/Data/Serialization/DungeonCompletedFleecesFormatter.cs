// Decompiled with JetBrains decompiler
// Type: Data.Serialization.DungeonCompletedFleecesFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using System.Collections.Generic;

#nullable disable
namespace Data.Serialization;

public sealed class DungeonCompletedFleecesFormatter : 
  IMessagePackFormatter<DataManager.DungeonCompletedFleeces>,
  IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    DataManager.DungeonCompletedFleeces value,
    MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(2);
    IFormatterResolver resolver = options.Resolver;
    resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
    resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Fleeces, options);
  }

  public DataManager.DungeonCompletedFleeces Deserialize(
    ref MessagePackReader reader,
    MessagePackSerializerOptions options)
  {
    if (reader.TryReadNil())
      return new DataManager.DungeonCompletedFleeces();
    options.Security.DepthStep(ref reader);
    IFormatterResolver resolver = options.Resolver;
    DataManager.DungeonCompletedFleeces completedFleeces = new DataManager.DungeonCompletedFleeces();
    switch (reader.NextMessagePackType)
    {
      case MessagePackType.Array:
        int num1 = reader.ReadArrayHeader();
        for (int index = 0; index < num1; ++index)
        {
          switch (index)
          {
            case 0:
              if (!reader.TryReadNil())
              {
                completedFleeces.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 1:
              if (!reader.TryReadNil())
              {
                completedFleeces.Fleeces = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 437:
              if (!reader.TryReadNil())
              {
                completedFleeces.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 438:
              if (!reader.TryReadNil())
              {
                completedFleeces.Fleeces = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
                break;
              }
              break;
            default:
              reader.Skip();
              break;
          }
        }
        break;
      case MessagePackType.Map:
        int num2 = reader.ReadMapHeader();
        for (int index = 0; index < num2; ++index)
        {
          switch (reader.ReadInt32())
          {
            case 0:
            case 437:
              if (!reader.TryReadNil())
              {
                completedFleeces.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 1:
            case 438:
              if (!reader.TryReadNil())
              {
                completedFleeces.Fleeces = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
                break;
              }
              break;
            default:
              reader.Skip();
              break;
          }
        }
        break;
      default:
        reader.Skip();
        break;
    }
    --reader.Depth;
    return completedFleeces;
  }
}
