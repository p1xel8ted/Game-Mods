// Decompiled with JetBrains decompiler
// Type: Objectives_Story
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class Objectives_Story : ObjectivesData
{
  public StoryDataItem ParentStoryDataItem;
  public StoryDataItem StoryDataItem;

  public override string Text => "";

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_Story.FinalizedData finalizedData = new Objectives_Story.FinalizedData();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public Objectives_Story()
  {
  }

  public Objectives_Story(StoryDataItem storyDataItem, StoryDataItem parentStoryDataItem)
    : base("")
  {
    this.ParentStoryDataItem = parentStoryDataItem;
    this.StoryDataItem = storyDataItem;
  }

  [Serializable]
  public class FinalizedData : ObjectivesDataFinalized
  {
    public override string GetText() => "";
  }
}
