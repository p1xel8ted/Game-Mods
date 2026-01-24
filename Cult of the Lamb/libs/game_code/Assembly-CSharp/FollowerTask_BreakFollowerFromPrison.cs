// Decompiled with JetBrains decompiler
// Type: FollowerTask_BreakFollowerFromPrison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_BreakFollowerFromPrison : FollowerTask
{
  public int _prisonID;
  public StructureBrain _prison;
  public int state;
  public bool sneaking;

  public override FollowerTaskType Type => FollowerTaskType.BreakFollowerFromPrison;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._prisonID;

  public FollowerTask_BreakFollowerFromPrison(int prisonID)
  {
    this._prisonID = prisonID;
    this._prison = StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
  }

  public override int GetSubTaskCode() => this._prisonID;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location != PlayerFarming.Location)
      this.End();
    if ((double) Vector3.Distance(this.Brain.LastPosition, this._prison.Data.Position) >= 5.0 || this.sneaking)
      return;
    this.sneaking = true;
    Follower followerById = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    followerById.SpeedMultiplier = 0.25f;
    followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Prison/Unlawful/sneak");
    followerById.gameObject.AddComponent<Interaction_BackToWork>().Init(followerById);
    this.SetState(FollowerTaskState.Idle);
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void ProgressTask() => this.End();

  public override void OnEnd() => base.OnEnd();

  public override Vector3 UpdateDestination(Follower follower)
  {
    return StructureManager.GetStructureByID<StructureBrain>(this._prisonID).Data.Position + Vector3.up / 2f + Vector3.right;
  }

  public override void OnDoingBegin(Follower follower)
  {
    string animation = "Prison/Unlawful/stocks-breaking-free";
    this.state = 0;
    Follower f = FollowerManager.FindFollowerByID(this._prison.Data.FollowerID);
    if ((UnityEngine.Object) f == (UnityEngine.Object) null)
    {
      this.ProgressTask();
    }
    else
    {
      follower.FacePosition(f.transform.position);
      follower.TimedAnimation(animation, 2.667f, (System.Action) (() =>
      {
        if ((UnityEngine.Object) f == (UnityEngine.Object) null)
          this.ProgressTask();
        else if (this._prison.Data.FollowerID != -1)
        {
          this._prison.Data.FollowerID = -1;
          DataManager.Instance.Followers_Imprisoned_IDs.Remove(f.Brain.Info.ID);
          NotificationCentre.Instance.PlayFaithNotification("Notifications/BrokeFollowerFromPrison", 0.0f, NotificationBase.Flair.None, f.Brain.Info.ID, follower.Brain.Info.Name, f.Brain.Info.Name);
          AudioManager.Instance.PlayOneShot("event:/material/wood_barrel_break", f.gameObject);
          BiomeConstants.Instance.EmitSmokeExplosionVFX(this._prison.Data.Position);
          this._prison.Collapse(refreshFollowerTasks: false);
          f.TimedAnimation("Prison/Unlawful/stocks-broke-free", 1f, (System.Action) (() => f.Brain.CompleteCurrentTask()), false);
          follower.TimedAnimation("Reactions/react-laugh", 3.33f, (System.Action) (() => this.ProgressTask()));
        }
        else
          this.End();
      }));
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>());
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower.SpeedMultiplier = 1f;
  }
}
