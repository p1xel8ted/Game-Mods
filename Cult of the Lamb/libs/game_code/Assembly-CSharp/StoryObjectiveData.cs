// Decompiled with JetBrains decompiler
// Type: StoryObjectiveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Story Objective Data")]
[MessagePackObject(false)]
public class StoryObjectiveData : ScriptableObject
{
  [Key(0)]
  public int UniqueStoryID;
  [IgnoreMember]
  public bool IsEntryStory;
  [Key(1)]
  public string GiveQuestTerm;
  [IgnoreMember]
  public string CompleteQuestTerm;
  [Space]
  [Key(2)]
  public int QuestIndex;
  [IgnoreMember]
  public bool TargetQuestGiver = true;
  [IgnoreMember]
  public bool QuestGiverRequiresDisciple;
  [Header("Follower Targets")]
  [Header("Follower Target 1")]
  [IgnoreMember]
  public bool RequireTarget_1;
  [Key(3)]
  public int Target1FollowerID = -1;
  [IgnoreMember]
  public bool NewTarget1Follower;
  [Tooltip("Holds Target1FollowerID's ID for the next quest when the Target 1 is changed. If it is also set on the next quest files, data will be overriden!")]
  [IgnoreMember]
  public bool CacheTarget_1;
  [IgnoreMember]
  public bool UseCachedTarget_1;
  [IgnoreMember]
  public bool SwapTarget1AsQuestGiver;
  [Header("Follower Target 2")]
  [IgnoreMember]
  public bool RequireTarget_2;
  [Key(4)]
  public int Target2FollowerID = -1;
  [IgnoreMember]
  public bool NewTarget2Follower;
  [Tooltip("Holds Target1FollowerID's ID for the next quest when the Target 1 is changed. If it is also set on the next quest files, data will be overriden!")]
  [IgnoreMember]
  public bool CacheTarget_2;
  [IgnoreMember]
  public bool UseCachedTarget_2;
  [IgnoreMember]
  public bool SwapTarget2AsQuestGiver;
  [IgnoreMember]
  public bool RequireTarget_Deadbody;
  [Key(5)]
  public int DeadBodyFollowerID = -1;
  [Space]
  [Space]
  [Key(6)]
  public int QuestGiverRequiresID = -1;
  [IgnoreMember]
  public bool HasTimer = true;
  [IgnoreMember]
  public List<MMBiomeGeneration.BiomeGenerator.VariableAndCondition> ConditionalVariables;
  [Key(7)]
  public List<StoryObjectiveData> ChilldStoryItems;
  [Key(8)]
  public string AssetName;

  [OnDeserialized]
  public void OnJsonDeserialized(StreamingContext _)
  {
    if (!string.IsNullOrEmpty(this.AssetName))
      return;
    this.AssetName = this.name;
  }
}
