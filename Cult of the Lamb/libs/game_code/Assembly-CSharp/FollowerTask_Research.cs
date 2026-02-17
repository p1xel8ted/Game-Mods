// Decompiled with JetBrains decompiler
// Type: FollowerTask_Research
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Research : FollowerTask
{
  public int _researchStationID;
  public Structures_Research _researchStation;
  public int _slotIndex = -1;
  public float _gameTimeSinceLastProgress;
  public const float HighPriorityValue = 15f;
  public const float LowPriorityValue = 8f;
  public float OverridePriority;

  public override FollowerTaskType Type => FollowerTaskType.Research;

  public override FollowerLocation Location => this._researchStation.Data.Location;

  public override int UsingStructureID => this._researchStationID;

  public override float Priorty => this.OverridePriority;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Monk:
        return PriorityCategory.Medium;
      case FollowerRole.Worker:
        return PriorityCategory.Medium;
      case FollowerRole.Lumberjack:
        return PriorityCategory.Medium;
      case FollowerRole.Farmer:
        return PriorityCategory.Medium;
      default:
        return PriorityCategory.Low;
    }
  }

  public FollowerTask_Research(int researchStationID, float Priority)
  {
    this.OverridePriority = Priority;
    this._researchStationID = researchStationID;
    this._researchStation = StructureManager.GetStructureByID<Structures_Research>(this._researchStationID);
  }

  public override int GetSubTaskCode() => this._researchStationID * 100 + this._slotIndex;

  public override void ClaimReservations()
  {
    StructureManager.GetStructureByID<Structures_Research>(this._researchStationID).TryClaimSlot(ref this._slotIndex);
    if (this._slotIndex < 0)
      return;
    Debug.Log((object) $"{this._brain.Info.Name} reserving Research {this._researchStationID}.{this._slotIndex}");
  }

  public override void ReleaseReservations()
  {
    if (this._slotIndex < 0)
      return;
    Debug.Log((object) $"{this._brain.Info.Name} releasing Research {this._researchStationID}.{this._slotIndex}");
    StructureManager.GetStructureByID<Structures_Research>(this._researchStationID).ReleaseSlot(this._slotIndex);
  }

  public override void OnStart()
  {
    if (this._slotIndex >= 0)
      this.SetState(FollowerTaskState.GoingTo);
    else
      this.End();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
    {
      this._gameTimeSinceLastProgress += deltaGameTime;
      for (int index = 0; index < DataManager.Instance.CurrentResearch.Count; ++index)
      {
        StructuresData.ResearchObject researchObject = DataManager.Instance.CurrentResearch[index];
        researchObject.Progress += this._gameTimeSinceLastProgress;
        if ((double) researchObject.Progress >= (double) researchObject.TargetProgress)
          StructuresData.CompleteResearch(researchObject.Type);
      }
      this._gameTimeSinceLastProgress = 0.0f;
    }
    if (StructuresData.GetAnyResearchExists())
      return;
    this.End();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return StructureManager.GetStructureByID<Structures_Research>(this._researchStationID).GetResearchPosition(this._slotIndex);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "astrologer");
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "astrologer");
  }
}
