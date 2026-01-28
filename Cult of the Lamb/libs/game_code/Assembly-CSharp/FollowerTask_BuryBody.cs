// Decompiled with JetBrains decompiler
// Type: FollowerTask_BuryBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_BuryBody : FollowerTask
{
  public int _corpseID;
  public Structures_DeadWorshipper _corpse;
  public int _graveID;
  public Structures_Grave _grave;
  public bool _haveCorpse;

  public override FollowerTaskType Type => FollowerTaskType.BuryBody;

  public override FollowerLocation Location => this._grave.Data.Location;

  public override int UsingStructureID => this._graveID;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override float Priorty => 25f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Lumberjack:
      case FollowerRole.Farmer:
      case FollowerRole.Monk:
        return PriorityCategory.Low;
      case FollowerRole.Worker:
        return PriorityCategory.Low;
      default:
        return PriorityCategory.Low;
    }
  }

  public FollowerTask_BuryBody(int corpseID, int graveID)
  {
    this._corpseID = corpseID;
    this._corpse = StructureManager.GetStructureByID<Structures_DeadWorshipper>(this._corpseID);
    this._graveID = graveID;
    this._grave = StructureManager.GetStructureByID<Structures_Grave>(this._graveID);
  }

  public override int GetSubTaskCode() => this._corpseID * 1000 + this._graveID;

  public override void ClaimReservations()
  {
    if (this._corpse != null)
      this._corpse.ReservedForTask = true;
    if (this._grave == null)
      return;
    this._grave.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this._corpse != null)
      this._corpse.ReservedForTask = false;
    if (this._grave == null)
      return;
    this._grave.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void ProgressTask()
  {
    if (!this._haveCorpse)
    {
      this._haveCorpse = true;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
    {
      this._grave.Data.FollowerID = this._corpse.Data.FollowerID;
      StructureManager.RemoveStructure((StructureBrain) this._corpse);
      this._corpseID = 0;
      this._corpse = (Structures_DeadWorshipper) null;
      this.End();
    }
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this._haveCorpse ? StructureManager.GetStructureByID<Structures_Grave>(this._graveID).Data.Position : StructureManager.GetStructureByID<Structures_DeadWorshipper>(this._corpseID).Data.Position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.GoingTo || !this._haveCorpse)
      return;
    DeadWorshipper corpse = this.FindCorpse();
    corpse.WrapBody();
    corpse.HideBody();
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-corpse");
  }

  public override void OnDoingBegin(Follower follower)
  {
    this.UndoStateAnimationChanges(follower);
    if (!this._haveCorpse)
      follower.StartCoroutine((IEnumerator) this.PickUpBodyRoutine(follower));
    else
      follower.StartCoroutine((IEnumerator) this.BuryBodyRoutine(follower));
  }

  public IEnumerator PickUpBodyRoutine(Follower follower)
  {
    FollowerTask_BuryBody followerTaskBuryBody = this;
    DeadWorshipper corpse = followerTaskBuryBody.FindCorpse();
    if ((Object) corpse == (Object) null)
    {
      follower.State.CURRENT_STATE = StateMachine.State.CustomAction0;
      yield return (object) new WaitForSeconds(1f);
      followerTaskBuryBody.\u003C\u003En__0(follower);
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
      followerTaskBuryBody.End();
    }
    else
    {
      corpse.WrapBody();
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
      yield return (object) new WaitForSeconds(0.5f);
      corpse.HideBody();
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-corpse");
      followerTaskBuryBody.ProgressTask();
    }
  }

  public IEnumerator BuryBodyRoutine(Follower follower)
  {
    FollowerTask_BuryBody followerTaskBuryBody = this;
    DeadWorshipper corpse = followerTaskBuryBody.FindCorpse();
    corpse.StructureInfo.Position = follower.transform.position;
    corpse.transform.position = follower.transform.position;
    corpse.ShowBody();
    yield return (object) new WaitForSeconds(0.5f);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("dig", true);
    yield return (object) new WaitForSeconds(5f);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForSeconds(0.5f);
    followerTaskBuryBody.ProgressTask();
    followerTaskBuryBody.FindGrave().SetGameObjects();
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override void Cleanup(Follower follower)
  {
    DeadWorshipper corpse = this.FindCorpse();
    if ((Object) corpse != (Object) null)
    {
      corpse.StructureInfo.Position = follower.transform.position;
      corpse.transform.position = follower.transform.position;
      corpse.ShowBody();
    }
    this.UndoStateAnimationChanges(follower);
    base.Cleanup(follower);
  }

  public DeadWorshipper FindCorpse()
  {
    DeadWorshipper corpse = (DeadWorshipper) null;
    foreach (DeadWorshipper deadWorshipper in DeadWorshipper.DeadWorshippers)
    {
      if (deadWorshipper.StructureInfo.ID == this._corpseID)
      {
        corpse = deadWorshipper;
        break;
      }
    }
    return corpse;
  }

  public Grave FindGrave()
  {
    Grave grave1 = (Grave) null;
    foreach (Grave grave2 in Grave.Graves)
    {
      if (grave2.StructureInfo.ID == this._graveID)
      {
        grave1 = grave2;
        break;
      }
    }
    return grave1;
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(Follower follower) => base.Cleanup(follower);
}
