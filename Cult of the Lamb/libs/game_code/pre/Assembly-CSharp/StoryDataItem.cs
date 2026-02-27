// Decompiled with JetBrains decompiler
// Type: StoryDataItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class StoryDataItem
{
  public int QuestGiverFollowerID = -1;
  public int TargetFollowerID_1 = -1;
  public int TargetFollowerID_2 = -1;
  public int DeadFollowerID = -1;
  public int FollowerID = -1;
  public bool QuestGiven;
  public bool QuestLocked;
  public bool QuestDeclined;
  public StoryObjectiveData StoryObjectiveData;
  public List<StoryDataItem> ChildStoryDataItems = new List<StoryDataItem>();
  public ObjectivesData Objective;
}
