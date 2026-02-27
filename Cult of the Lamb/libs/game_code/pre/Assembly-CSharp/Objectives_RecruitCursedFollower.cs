// Decompiled with JetBrains decompiler
// Type: Objectives_RecruitCursedFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_RecruitCursedFollower : ObjectivesData
{
  public Thought CursedState;
  public int Target;
  public int Count;

  public override string Text
  {
    get
    {
      return string.Format(LocalizationManager.GetTranslation($"Objectives/RecruitCursedFollower/{this.CursedState}") + " ({0}/{1})", (object) this.Count, (object) this.Target);
    }
  }

  public Objectives_RecruitCursedFollower()
  {
  }

  public Objectives_RecruitCursedFollower(
    string groupId,
    Thought cursedState,
    int target = 1,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.RECRUIT_CURSED_FOLLOWER;
    this.CursedState = cursedState;
    this.Target = target;
    this.Count = 0;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      FollowerRecruit.OnFollowerRecruited += new FollowerRecruit.FollowerEventDelegate(this.OnFollowerRecruited);
    base.Init(initialAssigning);
    if (!initialAssigning)
      return;
    this.Count = 0;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower finalizedData = new Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.CursedState = this.CursedState;
    finalizedData.Target = this.Target;
    finalizedData.Count = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private void OnFollowerRecruited(FollowerInfo info)
  {
    if (info.CursedState == this.CursedState || info.StartingCursedState == this.CursedState)
      ++this.Count;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.RECRUIT_CURSED_FOLLOWER);
  }

  protected override bool CheckComplete() => this.Count >= this.Target;

  public override void Complete()
  {
    base.Complete();
    FollowerRecruit.OnFollowerRecruited -= new FollowerRecruit.FollowerEventDelegate(this.OnFollowerRecruited);
  }

  public override void Failed()
  {
    base.Failed();
    FollowerRecruit.OnFollowerRecruited -= new FollowerRecruit.FollowerEventDelegate(this.OnFollowerRecruited);
  }

  [Serializable]
  public class FinalizedData_RecruitCursedFollower : ObjectivesDataFinalized
  {
    public Thought CursedState;
    public int Target;
    public int Count;

    public override string GetText()
    {
      return $"{LocalizationManager.GetTranslation($"Objectives/RecruitCursedFollower/{this.CursedState}")} {this.Count}/{this.Target}";
    }
  }
}
