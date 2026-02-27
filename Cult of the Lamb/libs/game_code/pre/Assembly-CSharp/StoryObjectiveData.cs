// Decompiled with JetBrains decompiler
// Type: StoryObjectiveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Story Objective Data")]
public class StoryObjectiveData : ScriptableObject
{
  public int UniqueStoryID;
  public bool IsEntryStory;
  public string GiveQuestTerm;
  public string CompleteQuestTerm;
  [Space]
  public int QuestIndex;
  public bool TargetQuestGiver = true;
  public bool RequireTarget_1;
  public bool RequireTarget_2;
  public bool RequireTarget_Deadbody;
  public int QuestGiverRequiresID = -1;
  public List<BiomeGenerator.VariableAndCondition> ConditionalVariables;
  public List<StoryObjectiveData> ChilldStoryItems;
}
