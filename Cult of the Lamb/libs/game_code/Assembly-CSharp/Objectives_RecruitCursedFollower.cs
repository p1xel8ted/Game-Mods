// Decompiled with JetBrains decompiler
// Type: Objectives_RecruitCursedFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_RecruitCursedFollower : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public Thought CursedState;
  [Key(17)]
  public int Target;
  [Key(18)]
  public int Count;
  [Key(19)]
  public string FollowerName = "";
  [Key(20)]
  public int FollowerID = -1;
  [Key(21)]
  public string FollowerSkin = "";

  public override string Text
  {
    get
    {
      if (this.FollowerID != -1)
        return string.Format(LocalizationManager.GetTranslation("Objectives/RecruitCursedFollower/None"), (object) LocalizeIntegration.FixEnglishWord(this.FollowerName));
      string translation = LocalizationManager.GetTranslation($"Objectives/RecruitCursedFollower/{this.CursedState}");
      return LocalizeIntegration.IsArabic() ? string.Format(translation + " ){1}/{0}(", (object) LocalizeIntegration.ReverseText(this.Count.ToString()), (object) LocalizeIntegration.ReverseText(this.Target.ToString())) : string.Format(translation + " ({0}/{1})", (object) this.Count, (object) this.Target);
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
    finalizedData.FollowerID = this.FollowerID;
    finalizedData.FollowerName = this.FollowerName;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void OnFollowerRecruited(FollowerInfo info)
  {
    if (this.FollowerID != -1 && this.FollowerID != info.ID)
      return;
    if (info.CursedState == this.CursedState || info.StartingCursedState == this.CursedState)
      ++this.Count;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.RECRUIT_CURSED_FOLLOWER);
  }

  public override bool CheckComplete() => this.Count >= this.Target;

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

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_RecruitCursedFollower : ObjectivesDataFinalized
  {
    [Key(3)]
    public Thought CursedState;
    [Key(4)]
    public int Target;
    [Key(5)]
    public int Count;
    [Key(6)]
    public string FollowerName = "";
    [Key(7)]
    public int FollowerID = -1;

    public override string GetText()
    {
      if (this.FollowerID != -1)
        return string.Format(LocalizationManager.GetTranslation("Objectives/RecruitCursedFollower/None"), (object) LocalizeIntegration.FixEnglishWord(this.FollowerName));
      string translation = LocalizationManager.GetTranslation($"Objectives/RecruitCursedFollower/{this.CursedState}");
      return LocalizeIntegration.IsArabic() ? string.Format(translation + " ){1}/{0}(", (object) LocalizeIntegration.ReverseText(this.Count.ToString()), (object) LocalizeIntegration.ReverseText(this.Target.ToString())) : string.Format(translation + " ({0}/{1})", (object) this.Count, (object) this.Target);
    }
  }
}
