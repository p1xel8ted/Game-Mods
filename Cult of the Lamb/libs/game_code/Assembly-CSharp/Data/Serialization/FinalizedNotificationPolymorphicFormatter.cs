// Decompiled with JetBrains decompiler
// Type: Data.Serialization.FinalizedNotificationPolymorphicFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MessagePack;
using MessagePack.Formatters;

#nullable disable
namespace Data.Serialization;

public sealed class FinalizedNotificationPolymorphicFormatter : 
  IMessagePackFormatter<FinalizedNotification>,
  IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    FinalizedNotification value,
    MessagePackSerializerOptions options)
  {
    if (value == null)
    {
      writer.WriteNil();
    }
    else
    {
      writer.WriteArrayHeader(2);
      if (!(value is FinalizedFaithNotification faithNotification))
      {
        if (!(value is FinalizedItemNotification itemNotification))
        {
          if (!(value is FinalizedFollowerNotification followerNotification))
          {
            if (!(value is FinalizedRelationshipNotification relationshipNotification))
            {
              if (!(value is FinalizedNotificationSimple notificationSimple))
                throw new MessagePackSerializationException("Unsupported FinalizedNotification subtype: " + value.GetType().FullName);
              writer.Write((byte) 0);
              MessagePackSerializer.Serialize<FinalizedNotificationSimple>(ref writer, notificationSimple, options);
            }
            else
            {
              writer.Write((byte) 4);
              MessagePackSerializer.Serialize<FinalizedRelationshipNotification>(ref writer, relationshipNotification, options);
            }
          }
          else
          {
            writer.Write((byte) 3);
            MessagePackSerializer.Serialize<FinalizedFollowerNotification>(ref writer, followerNotification, options);
          }
        }
        else
        {
          writer.Write((byte) 2);
          MessagePackSerializer.Serialize<FinalizedItemNotification>(ref writer, itemNotification, options);
        }
      }
      else
      {
        writer.Write((byte) 1);
        MessagePackSerializer.Serialize<FinalizedFaithNotification>(ref writer, faithNotification, options);
      }
    }
  }

  public FinalizedNotification Deserialize(
    ref MessagePackReader reader,
    MessagePackSerializerOptions options)
  {
    if (reader.TryReadNil())
      return (FinalizedNotification) null;
    if (reader.NextMessagePackType != MessagePackType.Array)
    {
      reader.Skip();
      return (FinalizedNotification) null;
    }
    int num = reader.ReadArrayHeader();
    if (num < 2)
      throw new MessagePackSerializationException("FinalizedNotification: expected array with >= 2 items.");
    FinalizedNotificationPolymorphicFormatter.Tag tag;
    switch (reader.NextMessagePackType)
    {
      case MessagePackType.Integer:
        tag = (FinalizedNotificationPolymorphicFormatter.Tag) reader.ReadInt32();
        break;
      case MessagePackType.String:
        if (!FinalizedNotificationPolymorphicFormatter.TryMapStringToTag(reader.ReadString(), out tag))
        {
          for (int index = 1; index < num; ++index)
            reader.Skip();
          return (FinalizedNotification) null;
        }
        break;
      default:
        for (int index = 1; index < num; ++index)
          reader.Skip();
        return (FinalizedNotification) null;
    }
    if (num >= 3)
      reader.Skip();
    FinalizedNotification finalizedNotification;
    switch (tag)
    {
      case FinalizedNotificationPolymorphicFormatter.Tag.Base:
        finalizedNotification = (FinalizedNotification) MessagePackSerializer.Deserialize<FinalizedNotificationSimple>(ref reader, options);
        break;
      case FinalizedNotificationPolymorphicFormatter.Tag.Faith:
        finalizedNotification = (FinalizedNotification) MessagePackSerializer.Deserialize<FinalizedFaithNotification>(ref reader, options);
        break;
      case FinalizedNotificationPolymorphicFormatter.Tag.Item:
        finalizedNotification = (FinalizedNotification) MessagePackSerializer.Deserialize<FinalizedItemNotification>(ref reader, options);
        break;
      case FinalizedNotificationPolymorphicFormatter.Tag.Follower:
        finalizedNotification = (FinalizedNotification) MessagePackSerializer.Deserialize<FinalizedFollowerNotification>(ref reader, options);
        break;
      case FinalizedNotificationPolymorphicFormatter.Tag.Relationship:
        finalizedNotification = (FinalizedNotification) MessagePackSerializer.Deserialize<FinalizedRelationshipNotification>(ref reader, options);
        break;
      default:
        reader.Skip();
        for (int index = 3; index < num; ++index)
          reader.Skip();
        return (FinalizedNotification) null;
    }
    for (int index = 3; index < num; ++index)
      reader.Skip();
    return finalizedNotification;
  }

  public static bool TryMapStringToTag(
    string s,
    out FinalizedNotificationPolymorphicFormatter.Tag tag)
  {
    int result;
    if (int.TryParse(s, out result) && result >= 0 && result <= 4)
    {
      tag = (FinalizedNotificationPolymorphicFormatter.Tag) result;
      return true;
    }
    string str1 = s.Trim();
    int num = str1.LastIndexOf('.');
    if (num >= 0 && num < str1.Length - 1)
      str1 = str1.Substring(num + 1);
    string str2 = str1.ToLowerInvariant();
    if (str2.StartsWith("finalized"))
      str2 = str2.Substring(9);
    if (str2.EndsWith("notification"))
      str2 = str2.Substring(0, str2.Length - 12);
    switch (str2)
    {
      case "faith":
        tag = FinalizedNotificationPolymorphicFormatter.Tag.Faith;
        return true;
      case "item":
        tag = FinalizedNotificationPolymorphicFormatter.Tag.Item;
        return true;
      case "follower":
        tag = FinalizedNotificationPolymorphicFormatter.Tag.Follower;
        return true;
      case "relationship":
        tag = FinalizedNotificationPolymorphicFormatter.Tag.Relationship;
        return true;
      case "base":
      case "simple":
        tag = FinalizedNotificationPolymorphicFormatter.Tag.Base;
        return true;
      default:
        tag = FinalizedNotificationPolymorphicFormatter.Tag.Base;
        return false;
    }
  }

  public enum Tag : byte
  {
    Base,
    Faith,
    Item,
    Follower,
    Relationship,
  }
}
