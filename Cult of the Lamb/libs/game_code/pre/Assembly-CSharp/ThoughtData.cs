// Decompiled with JetBrains decompiler
// Type: ThoughtData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ThoughtData
{
  public Thought ThoughtType;
  public Thought ThoughtGroup;
  public float Modifier;
  public float StartingModifier;
  public float Duration;
  public int Quantity = 1;
  public int Stacking;
  public int StackModifier;
  public int TotalCountDisplay;
  public bool ReduceOverTime;
  public List<float> CoolDowns = new List<float>();
  public List<float> TimeStarted = new List<float>();
  public int FollowerID = -1;
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
