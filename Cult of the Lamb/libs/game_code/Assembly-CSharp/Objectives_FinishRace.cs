// Decompiled with JetBrains decompiler
// Type: Objectives_FinishRace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using System.Runtime.CompilerServices;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_FinishRace : ObjectivesData
{
  [CompilerGenerated]
  public float \u003CRaceTargetTime\u003Ek__BackingField;
  [IgnoreMember]
  public float raceTime = float.PositiveInfinity;

  [IgnoreMember]
  public override string Text
  {
    get
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/FinishRace"), (object) this.RaceTargetTime);
    }
  }

  [Key(16 /*0x10*/)]
  public float RaceTargetTime
  {
    get => this.\u003CRaceTargetTime\u003Ek__BackingField;
    set => this.\u003CRaceTargetTime\u003Ek__BackingField = value;
  }

  public Objectives_FinishRace()
  {
  }

  public Objectives_FinishRace(string groupId, float raceTargetTime = 10f, float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.FINISH_RACE;
    this.RaceTargetTime = raceTargetTime;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      Interaction_RacingGate.OnFinishRace += new Action<float>(this.OnFinishRace);
    base.Init(initialAssigning);
    if (!initialAssigning)
      return;
    this.raceTime = float.PositiveInfinity;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_FinishRace.FinalizedData_Objectives_FinishRace finalizedData = new Objectives_FinishRace.FinalizedData_Objectives_FinishRace();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.RaceTime = this.raceTime;
    finalizedData.RaceTargetTime = this.RaceTargetTime;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    return (double) this.raceTime != double.PositiveInfinity && base.CheckComplete();
  }

  public void OnFinishRace(float time)
  {
    this.raceTime = time;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.FINISH_RACE);
  }

  public override void Complete()
  {
    base.Complete();
    Interaction_RacingGate.OnFinishRace -= new Action<float>(this.OnFinishRace);
  }

  public override void Failed()
  {
    base.Failed();
    Interaction_RacingGate.OnFinishRace -= new Action<float>(this.OnFinishRace);
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_Objectives_FinishRace : ObjectivesDataFinalized
  {
    [Key(3)]
    public float RaceTime;
    [Key(4)]
    public float RaceTargetTime;

    public override string GetText()
    {
      string str = LocalizeIntegration.ReverseText(this.RaceTime.ToString());
      return string.Format(LocalizationManager.GetTranslation("Objectives/FinishRace"), (object) str);
    }
  }
}
