// Decompiled with JetBrains decompiler
// Type: Objectives_Story
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_Story : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public StoryDataItem ParentStoryDataItem;
  [Key(17)]
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

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData : ObjectivesDataFinalized
  {
    public override string GetText() => "";
  }
}
