// Decompiled with JetBrains decompiler
// Type: ThoughtData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class ThoughtData
{
  public static List<Thought> BishopAltThoughts = new List<Thought>()
  {
    Thought.CultHasNewRecruit,
    Thought.CultHasNewBuilding,
    Thought.CultHasNewBuildingConstructionEnthusiast,
    Thought.CultMemberWasHealed,
    Thought.DancedWithLeader,
    Thought.HappyToBeHealed,
    Thought.HelpedLeaderInResourceRoom,
    Thought.Intimidated,
    Thought.LeaderDidQuest,
    Thought.LeaderMurderedAFollower,
    Thought.NewEnemy,
    Thought.ReceivedGift,
    Thought.ReceivedNecklace,
    Thought.UpsetNoSermonYesterday,
    Thought.WatchedSermon,
    Thought.WatchedSermonDevotee,
    Thought.LeaderFailedQuest,
    Thought.ReactDecoration,
    Thought.ReactDecorationFalseIdols,
    Thought.TiredFromMissionaryHappy,
    Thought.DemonFailedRun,
    Thought.DemonSuccessfulRun,
    Thought.SpouseKiss
  };
  public static List<Thought> BishopExcludeThoughts = new List<Thought>()
  {
    Thought.CultMemberWasSacrificed,
    Thought.EnthusiasticNewRecruit,
    Thought.GaveConfessionEcstatic,
    Thought.GaveConfessionHonoured,
    Thought.GratefulConvert,
    Thought.GratefulRecued,
    Thought.HappyConvert,
    Thought.HappyNewRecruit,
    Thought.InAweOfLeaderChain,
    Thought.InstantBelieverConvert,
    Thought.InstantBelieverRescued
  };
  [Key(0)]
  public Thought ThoughtType;
  [Key(1)]
  public Thought ThoughtGroup;
  [Key(2)]
  public float Modifier;
  [Key(3)]
  public float StartingModifier;
  [Key(4)]
  public float Duration;
  [Key(5)]
  public int Quantity = 1;
  [Key(6)]
  public int Stacking;
  [Key(7)]
  public int StackModifier;
  [Key(8)]
  public int TotalCountDisplay;
  [Key(9)]
  public bool ReduceOverTime;
  [Key(10)]
  public List<float> CoolDowns = new List<float>();
  [Key(11)]
  public List<float> TimeStarted = new List<float>();
  [Key(12)]
  public int FollowerID = -1;
  [Key(13)]
  public float Warmth;
  [Key(14)]
  public bool TrackThought;

  public ThoughtData()
  {
  }

  public ThoughtData(Thought thought) => this.ThoughtType = this.ThoughtGroup = thought;

  public ThoughtData(Thought thought, Thought thoughtGroup)
  {
    this.ThoughtType = thought;
    this.ThoughtGroup = thoughtGroup;
  }

  public void Init()
  {
    this.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
    this.CoolDowns.Add(this.Duration);
  }

  public ThoughtData Clone()
  {
    return new ThoughtData(this.ThoughtType, this.ThoughtGroup)
    {
      TimeStarted = new List<float>((IEnumerable<float>) this.TimeStarted),
      CoolDowns = new List<float>((IEnumerable<float>) this.CoolDowns),
      Modifier = this.Modifier,
      Duration = this.Duration,
      Quantity = this.Quantity,
      Stacking = this.Stacking,
      StackModifier = this.StackModifier,
      TotalCountDisplay = this.TotalCountDisplay
    };
  }
}
