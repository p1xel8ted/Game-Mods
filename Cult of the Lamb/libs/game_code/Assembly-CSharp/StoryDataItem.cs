// Decompiled with JetBrains decompiler
// Type: StoryDataItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class StoryDataItem
{
  [Key(0)]
  public int QuestGiverFollowerID = -1;
  [Key(1)]
  public int TargetFollowerID_1 = -1;
  [Key(2)]
  public int TargetFollowerID_2 = -1;
  [Key(3)]
  public int DeadFollowerID = -1;
  [Key(4)]
  public int FollowerID = -1;
  [Key(5)]
  public int CachedTargetFollowerID_1 = -1;
  [Key(6)]
  public int CachedTargetFollowerID_2 = -1;
  [Key(7)]
  public bool QuestGiven;
  [Key(8)]
  public bool QuestLocked;
  [Key(9)]
  public bool QuestDeclined;
  [Key(10)]
  public StoryObjectiveData StoryObjectiveData;
  [Key(11)]
  public List<StoryDataItem> ChildStoryDataItems = new List<StoryDataItem>();
  [Key(12)]
  public ObjectivesData Objective;
}
