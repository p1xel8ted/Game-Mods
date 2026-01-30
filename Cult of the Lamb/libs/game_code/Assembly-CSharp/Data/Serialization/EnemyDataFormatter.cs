// Decompiled with JetBrains decompiler
// Type: Data.Serialization.EnemyDataFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;

#nullable disable
namespace Data.Serialization;

public sealed class EnemyDataFormatter : 
  IMessagePackFormatter<DataManager.EnemyData>,
  IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    DataManager.EnemyData value,
    MessagePackSerializerOptions options)
  {
    if (value == null)
    {
      writer.WriteNil();
    }
    else
    {
      writer.WriteArrayHeader(2);
      options.Resolver.GetFormatterWithVerify<Enemy>().Serialize(ref writer, value.EnemyType, options);
      writer.Write(value.AmountKilled);
    }
  }

  public DataManager.EnemyData Deserialize(
    ref MessagePackReader reader,
    MessagePackSerializerOptions options)
  {
    if (reader.TryReadNil())
      return (DataManager.EnemyData) null;
    options.Security.DepthStep(ref reader);
    IFormatterResolver resolver = options.Resolver;
    DataManager.EnemyData enemyData = new DataManager.EnemyData();
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
                enemyData.EnemyType = resolver.GetFormatterWithVerify<Enemy>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 1:
              if (!reader.TryReadNil())
              {
                enemyData.AmountKilled = reader.ReadInt32();
                break;
              }
              break;
            case 1312:
              if (!reader.TryReadNil())
              {
                enemyData.EnemyType = resolver.GetFormatterWithVerify<Enemy>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 1313:
              if (!reader.TryReadNil())
              {
                enemyData.AmountKilled = reader.ReadInt32();
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
            case 1312:
              if (!reader.TryReadNil())
              {
                enemyData.EnemyType = resolver.GetFormatterWithVerify<Enemy>().Deserialize(ref reader, options);
                break;
              }
              break;
            case 1:
            case 1313:
              if (!reader.TryReadNil())
              {
                enemyData.AmountKilled = reader.ReadInt32();
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
    return enemyData;
  }
}
