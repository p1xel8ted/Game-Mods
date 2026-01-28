// Decompiled with JetBrains decompiler
// Type: StructureAndTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class StructureAndTime
{
  [Key(0)]
  public int StructureID;
  [Key(1)]
  public float TimeReacted;
  [Key(2)]
  public StructureAndTime.IDTypes IDType;

  public static float GetOrAddTime(
    int StructureID,
    FollowerBrain Brain,
    StructureAndTime.IDTypes Type)
  {
    foreach (StructureAndTime structureAndTime in Brain.Stats.ReactionsAndTime)
    {
      if (structureAndTime.StructureID == StructureID && structureAndTime.IDType == Type)
        return structureAndTime.TimeReacted;
    }
    return StructureAndTime.AddNewTime(StructureID, Brain, float.MinValue, Type).TimeReacted;
  }

  public static StructureAndTime AddNewTime(
    int ID,
    FollowerBrain Brain,
    float Time,
    StructureAndTime.IDTypes Type)
  {
    StructureAndTime structureAndTime = new StructureAndTime()
    {
      TimeReacted = Time,
      StructureID = ID,
      IDType = Type
    };
    Brain.Stats.ReactionsAndTime.Add(structureAndTime);
    return structureAndTime;
  }

  public static void SetTime(int StructureID, FollowerBrain Brain, StructureAndTime.IDTypes Type)
  {
    foreach (StructureAndTime structureAndTime in Brain.Stats.ReactionsAndTime)
    {
      if (structureAndTime.StructureID == StructureID && structureAndTime.IDType == Type)
      {
        structureAndTime.TimeReacted = TimeManager.TotalElapsedGameTime;
        return;
      }
    }
    StructureAndTime.AddNewTime(StructureID, Brain, TimeManager.TotalElapsedGameTime, Type);
  }

  public enum IDTypes
  {
    Structure,
    Follower,
  }
}
