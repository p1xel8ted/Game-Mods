// Decompiled with JetBrains decompiler
// Type: Data.Serialization.RanchableAnimalFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public sealed class RanchableAnimalFormatter : 
  IMessagePackFormatter<StructuresData.Ranchable_Animal>,
  IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    StructuresData.Ranchable_Animal value,
    MessagePackSerializerOptions options)
  {
    if (value == null)
    {
      writer.WriteNil();
    }
    else
    {
      IFormatterResolver resolver = options.Resolver;
      writer.WriteArrayHeader(34);
      resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.Type, options);
      writer.Write(value.Age);
      writer.Write(value.ID);
      writer.Write(value.Satiation);
      writer.Write(value.TimeSincePoop);
      writer.Write(value.MilkedToday);
      writer.Write(value.MilkedReady);
      writer.Write(value.WorkedToday);
      writer.Write(value.WorkedReady);
      writer.Write(value.EatenToday);
      writer.Write(value.PetToday);
      writer.Write(value.PlayerMadeHappyToday);
      writer.Write(value._adoration);
      writer.Write(value.Level);
      resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.FavouriteFood, options);
      writer.Write(value.IsFavouriteFoodRevealed);
      resolver.GetFormatterWithVerify<Interaction_Ranchable.State>().Serialize(ref writer, value.State, options);
      resolver.GetFormatterWithVerify<Interaction_Ranchable.Ailment>().Serialize(ref writer, value.Ailment, options);
      writer.Write(value.AilmentGameTime);
      writer.Write(value.HappyAmount);
      resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.LastPosition, options);
      writer.Write(value.GivenName);
      writer.Write(value.GrowthStage);
      resolver.GetFormatterWithVerify<Interaction_Ranchable.GrowthRate>().Serialize(ref writer, value.growthRate, options);
      writer.Write(value.Horns);
      writer.Write(value.Ears);
      writer.Write(value.Head);
      writer.Write(value.Colour);
      writer.Write(value.Speed);
      writer.Write(value.TimeSinceLastWash);
      writer.Write(value.BestFriend);
      writer.Write(value.RacedToday);
      writer.Write(value.Injured);
      writer.Write(value.FeralCalming);
    }
  }

  public StructuresData.Ranchable_Animal Deserialize(
    ref MessagePackReader reader,
    MessagePackSerializerOptions options)
  {
    if (reader.TryReadNil())
      return (StructuresData.Ranchable_Animal) null;
    options.Security.DepthStep(ref reader);
    IFormatterResolver resolver = options.Resolver;
    StructuresData.Ranchable_Animal ranchableAnimal = new StructuresData.Ranchable_Animal();
    switch (reader.NextMessagePackType)
    {
      case MessagePackType.Array:
        int num1 = reader.ReadArrayHeader();
        for (int index = 0; index < num1; ++index)
        {
          switch (index)
          {
            case 0:
              ranchableAnimal.Type = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 1:
              ranchableAnimal.Age = reader.ReadInt32();
              break;
            case 2:
              ranchableAnimal.ID = reader.ReadInt32();
              break;
            case 3:
              ranchableAnimal.Satiation = reader.ReadSingle();
              break;
            case 4:
              ranchableAnimal.TimeSincePoop = reader.ReadSingle();
              break;
            case 5:
              ranchableAnimal.MilkedToday = reader.ReadBoolean();
              break;
            case 6:
              ranchableAnimal.MilkedReady = reader.ReadBoolean();
              break;
            case 7:
              ranchableAnimal.WorkedToday = reader.ReadBoolean();
              break;
            case 8:
              ranchableAnimal.WorkedReady = reader.ReadBoolean();
              break;
            case 9:
              ranchableAnimal.EatenToday = reader.ReadBoolean();
              break;
            case 10:
              ranchableAnimal.PetToday = reader.ReadBoolean();
              break;
            case 11:
              ranchableAnimal.PlayerMadeHappyToday = reader.ReadBoolean();
              break;
            case 12:
              ranchableAnimal._adoration = reader.ReadSingle();
              break;
            case 13:
              ranchableAnimal.Level = reader.ReadInt32();
              break;
            case 14:
              ranchableAnimal.FavouriteFood = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 15:
              ranchableAnimal.IsFavouriteFoodRevealed = reader.ReadBoolean();
              break;
            case 16 /*0x10*/:
              ranchableAnimal.State = resolver.GetFormatterWithVerify<Interaction_Ranchable.State>().Deserialize(ref reader, options);
              break;
            case 17:
              ranchableAnimal.Ailment = resolver.GetFormatterWithVerify<Interaction_Ranchable.Ailment>().Deserialize(ref reader, options);
              break;
            case 18:
              ranchableAnimal.AilmentGameTime = reader.ReadSingle();
              break;
            case 19:
              ranchableAnimal.HappyAmount = reader.ReadInt32();
              break;
            case 20:
              ranchableAnimal.LastPosition = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
              break;
            case 21:
              ranchableAnimal.GivenName = reader.ReadString();
              break;
            case 22:
              ranchableAnimal.GrowthStage = reader.ReadInt32();
              break;
            case 23:
              ranchableAnimal.growthRate = resolver.GetFormatterWithVerify<Interaction_Ranchable.GrowthRate>().Deserialize(ref reader, options);
              break;
            case 24:
              ranchableAnimal.Horns = reader.ReadInt32();
              break;
            case 25:
              ranchableAnimal.Ears = reader.ReadInt32();
              break;
            case 26:
              ranchableAnimal.Head = reader.ReadInt32();
              break;
            case 27:
              ranchableAnimal.Colour = reader.ReadInt32();
              break;
            case 28:
              ranchableAnimal.Speed = reader.ReadSingle();
              break;
            case 29:
              ranchableAnimal.TimeSinceLastWash = reader.ReadSingle();
              break;
            case 30:
              ranchableAnimal.BestFriend = reader.ReadBoolean();
              break;
            case 31 /*0x1F*/:
              ranchableAnimal.RacedToday = reader.ReadBoolean();
              break;
            case 32 /*0x20*/:
              if (!reader.TryReadNil())
              {
                ranchableAnimal.Injured = reader.ReadSingle();
                break;
              }
              break;
            case 33:
              if (!reader.TryReadNil())
              {
                ranchableAnimal.FeralCalming = reader.ReadSingle();
                break;
              }
              break;
            case 122:
              if (!reader.TryReadNil())
              {
                ranchableAnimal.Injured = reader.ReadSingle();
                break;
              }
              break;
            case 123:
              if (!reader.TryReadNil())
              {
                ranchableAnimal.FeralCalming = reader.ReadSingle();
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
              ranchableAnimal.Type = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 1:
              ranchableAnimal.Age = reader.ReadInt32();
              break;
            case 2:
              ranchableAnimal.ID = reader.ReadInt32();
              break;
            case 3:
              ranchableAnimal.Satiation = reader.ReadSingle();
              break;
            case 4:
              ranchableAnimal.TimeSincePoop = reader.ReadSingle();
              break;
            case 5:
              ranchableAnimal.MilkedToday = reader.ReadBoolean();
              break;
            case 6:
              ranchableAnimal.MilkedReady = reader.ReadBoolean();
              break;
            case 7:
              ranchableAnimal.WorkedToday = reader.ReadBoolean();
              break;
            case 8:
              ranchableAnimal.WorkedReady = reader.ReadBoolean();
              break;
            case 9:
              ranchableAnimal.EatenToday = reader.ReadBoolean();
              break;
            case 10:
              ranchableAnimal.PetToday = reader.ReadBoolean();
              break;
            case 11:
              ranchableAnimal.PlayerMadeHappyToday = reader.ReadBoolean();
              break;
            case 12:
              ranchableAnimal._adoration = reader.ReadSingle();
              break;
            case 13:
              ranchableAnimal.Level = reader.ReadInt32();
              break;
            case 14:
              ranchableAnimal.FavouriteFood = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
              break;
            case 15:
              ranchableAnimal.IsFavouriteFoodRevealed = reader.ReadBoolean();
              break;
            case 16 /*0x10*/:
              ranchableAnimal.State = resolver.GetFormatterWithVerify<Interaction_Ranchable.State>().Deserialize(ref reader, options);
              break;
            case 17:
              ranchableAnimal.Ailment = resolver.GetFormatterWithVerify<Interaction_Ranchable.Ailment>().Deserialize(ref reader, options);
              break;
            case 18:
              ranchableAnimal.AilmentGameTime = reader.ReadSingle();
              break;
            case 19:
              ranchableAnimal.HappyAmount = reader.ReadInt32();
              break;
            case 20:
              ranchableAnimal.LastPosition = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
              break;
            case 21:
              ranchableAnimal.GivenName = reader.ReadString();
              break;
            case 22:
              ranchableAnimal.GrowthStage = reader.ReadInt32();
              break;
            case 23:
              ranchableAnimal.growthRate = resolver.GetFormatterWithVerify<Interaction_Ranchable.GrowthRate>().Deserialize(ref reader, options);
              break;
            case 24:
              ranchableAnimal.Horns = reader.ReadInt32();
              break;
            case 25:
              ranchableAnimal.Ears = reader.ReadInt32();
              break;
            case 26:
              ranchableAnimal.Head = reader.ReadInt32();
              break;
            case 27:
              ranchableAnimal.Colour = reader.ReadInt32();
              break;
            case 28:
              ranchableAnimal.Speed = reader.ReadSingle();
              break;
            case 29:
              ranchableAnimal.TimeSinceLastWash = reader.ReadSingle();
              break;
            case 30:
              ranchableAnimal.BestFriend = reader.ReadBoolean();
              break;
            case 31 /*0x1F*/:
              ranchableAnimal.RacedToday = reader.ReadBoolean();
              break;
            case 32 /*0x20*/:
            case 122:
              if (!reader.TryReadNil())
              {
                ranchableAnimal.Injured = reader.ReadSingle();
                break;
              }
              break;
            case 33:
            case 123:
              if (!reader.TryReadNil())
              {
                ranchableAnimal.FeralCalming = reader.ReadSingle();
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
    return ranchableAnimal;
  }
}
