// Decompiled with JetBrains decompiler
// Type: Data.Serialization.StoryObjectiveDataFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public class StoryObjectiveDataFormatter : 
  IMessagePackFormatter<StoryObjectiveData>,
  IMessagePackFormatter
{
  public void Serialize(
    ref MessagePackWriter writer,
    StoryObjectiveData value,
    MessagePackSerializerOptions options)
  {
    if ((Object) value == (Object) null)
      writer.WriteNil();
    else
      StoryObjectiveDataFormatter.WriteNode(ref writer, value, options, 0);
  }

  public StoryObjectiveData Deserialize(
    ref MessagePackReader reader,
    MessagePackSerializerOptions options)
  {
    if (reader.TryReadNil())
      return (StoryObjectiveData) null;
    if (reader.NextMessagePackType == MessagePackType.Array)
      return StoryObjectiveDataFormatter.ReadNode(ref reader, options);
    if (reader.NextMessagePackType != MessagePackType.String)
      return StoryObjectiveDataFormatter.CloneOrNew(Quests.GetStoryObjectiveDataByID(reader.ReadInt32()), (string) null);
    string str = reader.ReadString();
    return StoryObjectiveDataFormatter.CloneOrNew(Quests.GetStoryObjectiveDataByName(str), str);
  }

  public static void WriteNode(
    ref MessagePackWriter writer,
    StoryObjectiveData v,
    MessagePackSerializerOptions options,
    int depth)
  {
    if (depth > 64 /*0x40*/)
    {
      writer.Write(v.UniqueStoryID);
    }
    else
    {
      writer.WriteArrayHeader(9);
      writer.Write(v.UniqueStoryID);
      if (v.GiveQuestTerm == null)
        writer.WriteNil();
      else
        writer.Write(v.GiveQuestTerm);
      writer.Write(v.QuestIndex);
      writer.Write(v.Target1FollowerID);
      writer.Write(v.Target2FollowerID);
      writer.Write(v.DeadBodyFollowerID);
      writer.Write(v.QuestGiverRequiresID);
      if (string.IsNullOrEmpty(v.AssetName))
        writer.WriteNil();
      else
        writer.Write(v.AssetName);
      List<StoryObjectiveData> chilldStoryItems = v.ChilldStoryItems;
      if (chilldStoryItems == null)
      {
        writer.WriteNil();
      }
      else
      {
        writer.WriteArrayHeader(chilldStoryItems.Count);
        for (int index = 0; index < chilldStoryItems.Count; ++index)
        {
          StoryObjectiveData v1 = chilldStoryItems[index];
          if ((Object) v1 == (Object) null)
            writer.WriteNil();
          else
            StoryObjectiveDataFormatter.WriteNode(ref writer, v1, options, depth + 1);
        }
      }
    }
  }

  public static StoryObjectiveData ReadNode(
    ref MessagePackReader reader,
    MessagePackSerializerOptions options)
  {
    int num1 = reader.ReadArrayHeader();
    int id1 = 0;
    int num2 = 0;
    int num3 = -1;
    int num4 = -1;
    int num5 = -1;
    int num6 = -1;
    string str1 = (string) null;
    string str2 = (string) null;
    List<StoryObjectiveData> storyObjectiveDataList = (List<StoryObjectiveData>) null;
    if (num1 >= 1)
      id1 = reader.ReadInt32();
    if (num1 >= 2 && !reader.TryReadNil())
      str1 = reader.ReadString();
    if (num1 >= 3)
      num2 = reader.ReadInt32();
    if (num1 >= 4)
      num3 = reader.ReadInt32();
    if (num1 >= 5)
      num4 = reader.ReadInt32();
    if (num1 >= 6)
      num5 = reader.ReadInt32();
    if (num1 >= 7)
      num6 = reader.ReadInt32();
    if (num1 >= 8 && !reader.TryReadNil())
      str2 = reader.ReadString();
    if (num1 >= 9)
    {
      if (reader.TryReadNil())
      {
        storyObjectiveDataList = (List<StoryObjectiveData>) null;
      }
      else
      {
        int capacity = reader.ReadArrayHeader();
        storyObjectiveDataList = new List<StoryObjectiveData>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          if (reader.TryReadNil())
            storyObjectiveDataList.Add((StoryObjectiveData) null);
          else if (reader.NextMessagePackType == MessagePackType.Array)
            storyObjectiveDataList.Add(StoryObjectiveDataFormatter.ReadNode(ref reader, options));
          else if (reader.NextMessagePackType == MessagePackType.String)
          {
            string str3 = reader.ReadString();
            storyObjectiveDataList.Add(StoryObjectiveDataFormatter.CloneOrNew(Quests.GetStoryObjectiveDataByName(str3), str3));
          }
          else
          {
            int id2 = reader.ReadInt32();
            storyObjectiveDataList.Add(StoryObjectiveDataFormatter.CloneOrNew(Quests.GetStoryObjectiveDataByID(id2), (string) null));
          }
        }
      }
    }
    for (int index = 9; index < num1; ++index)
      reader.Skip();
    StoryObjectiveData storyObjectiveData = StoryObjectiveDataFormatter.CloneOrNew(!string.IsNullOrEmpty(str2) ? Quests.GetStoryObjectiveDataByName(str2) : Quests.GetStoryObjectiveDataByID(id1), str2);
    storyObjectiveData.UniqueStoryID = id1;
    storyObjectiveData.GiveQuestTerm = str1;
    storyObjectiveData.QuestIndex = num2;
    storyObjectiveData.Target1FollowerID = num3;
    storyObjectiveData.Target2FollowerID = num4;
    storyObjectiveData.DeadBodyFollowerID = num5;
    storyObjectiveData.QuestGiverRequiresID = num6;
    storyObjectiveData.ChilldStoryItems = storyObjectiveDataList;
    if (!string.IsNullOrEmpty(str2))
      storyObjectiveData.AssetName = str2;
    return storyObjectiveData;
  }

  public static StoryObjectiveData CloneOrNew(StoryObjectiveData src, string nameIfNew)
  {
    StoryObjectiveData storyObjectiveData;
    if ((Object) src != (Object) null)
    {
      storyObjectiveData = Object.Instantiate<StoryObjectiveData>(src);
    }
    else
    {
      storyObjectiveData = ScriptableObject.CreateInstance<StoryObjectiveData>();
      if (!string.IsNullOrEmpty(nameIfNew))
        storyObjectiveData.name = nameIfNew;
    }
    if (!string.IsNullOrEmpty(nameIfNew))
      storyObjectiveData.name = nameIfNew;
    return storyObjectiveData;
  }
}
