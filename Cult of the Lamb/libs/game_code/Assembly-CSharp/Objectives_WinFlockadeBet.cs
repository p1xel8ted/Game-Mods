// Decompiled with JetBrains decompiler
// Type: Objectives_WinFlockadeBet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using MessagePack;
using System;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_WinFlockadeBet : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public string OpponentTermId;
  [Key(17)]
  public int TargetWoolAmount;
  [Key(18)]
  public int Count;
  [Key(19)]
  public DataManager.Variables WoolCountVariable;

  public Objectives_WinFlockadeBet()
  {
  }

  public Objectives_WinFlockadeBet(
    string groupName,
    string opponentNameTermId,
    int minWoolAmount,
    DataManager.Variables woolCountVariable,
    float questExpireDuration = -1f)
    : base(groupName, questExpireDuration)
  {
    this.Type = Objectives.TYPES.WIN_FLOCKADE_BET;
    this.OpponentTermId = opponentNameTermId;
    this.TargetWoolAmount = minWoolAmount;
    this.WoolCountVariable = woolCountVariable;
    Debug.Log((object) $"Retrieving wool count from {this.WoolCountVariable}");
    this.Count = DataManager.Instance.GetVariableInt(this.WoolCountVariable);
  }

  public override string Text
  {
    get
    {
      return string.Format("Objectives/Flockade/WinBetFinal".Localized(), (object) this.TargetWoolAmount, (object) this.OpponentTermId.Localized(), (object) this.Count, (object) this.TargetWoolAmount);
    }
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    return (ObjectivesDataFinalized) new Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet(this.OpponentTermId, this.TargetWoolAmount);
  }

  public override void Init(bool initialAssigning)
  {
    FlockadeGameScreen.OnMatchCompleted -= new FlockadeGameScreen.MatchCompletionEvent(this.OnFlockadeMatchCompleted);
    FlockadeGameScreen.OnMatchCompleted += new FlockadeGameScreen.MatchCompletionEvent(this.OnFlockadeMatchCompleted);
    if (initialAssigning)
      this.Count = 0;
    base.Init(initialAssigning);
  }

  public override void Complete()
  {
    base.Complete();
    FlockadeGameScreen.OnMatchCompleted -= new FlockadeGameScreen.MatchCompletionEvent(this.OnFlockadeMatchCompleted);
  }

  public override void Failed()
  {
    base.Failed();
    FlockadeGameScreen.OnMatchCompleted -= new FlockadeGameScreen.MatchCompletionEvent(this.OnFlockadeMatchCompleted);
  }

  public void OnFlockadeMatchCompleted(
    string opponentName,
    int winnings,
    FlockadeUIController.Result result)
  {
    Debug.Log((object) $"WinFlockadeBet has detected a flockade game played {{Opponent={opponentName}, ObjectiveOpponent={this.OpponentTermId}, WoolWagered={winnings}, Result={result}}}");
    if (opponentName != this.OpponentTermId || winnings <= 0)
      return;
    this.Count += winnings;
    Debug.Log((object) $"Count is now {this.Count}, Target={this.TargetWoolAmount}");
  }

  public override bool CheckComplete()
  {
    bool flag = this.Count >= this.TargetWoolAmount;
    Debug.Log((object) $"WinFlockadeBet.CheckComplete: {{Count={this.Count}, Target={this.TargetWoolAmount}, isComplete={flag}}}");
    return flag;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_WinFlockadeBet : ObjectivesDataFinalized
  {
    [Key(3)]
    public string OpponentTermId;
    [Key(4)]
    public int WoolAmount;

    public FinalizedData_WinFlockadeBet()
    {
    }

    public FinalizedData_WinFlockadeBet(string opponentTermId, int woolAmount)
    {
      this.OpponentTermId = opponentTermId;
      this.WoolAmount = woolAmount;
    }

    public override string GetText()
    {
      return string.Format("Objectives/Flockade/WinBetFinal".Localized(), (object) this.WoolAmount, (object) this.OpponentTermId.Localized(), (object) this.WoolAmount, (object) this.WoolAmount);
    }
  }
}
