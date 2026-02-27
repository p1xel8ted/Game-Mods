// Decompiled with JetBrains decompiler
// Type: StructureAndTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class StructureAndTime
{
  public int StructureID;
  public float TimeReacted;
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

  private static StructureAndTime AddNewTime(
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
