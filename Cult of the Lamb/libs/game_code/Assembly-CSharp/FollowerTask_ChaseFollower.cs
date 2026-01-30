// Decompiled with JetBrains decompiler
// Type: FollowerTask_ChaseFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class FollowerTask_ChaseFollower : FollowerTask
{
  public FollowerTask_ChaseFollower OtherChatTask;
  public int _otherFollowerID;
  public bool IsLeader;
  public LayerMask layerMask;
  public float updateDestination;
  public float chaseForDuration;
  public float chaseProgress;
  public float maxDistance = float.MaxValue;
  public bool taskSet;
  public Follower follower;
  public Follower targetFollower;
  public float soundProgress;
  public bool isSozoScenario;
  public EventInstance loopingSound;

  public override FollowerTaskType Type => FollowerTaskType.ChaseFollower;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public FollowerTask_ChaseFollower(int followerID, bool leader, float maxDistance = 7f)
  {
    this.maxDistance = maxDistance;
    this._otherFollowerID = followerID;
    this.IsLeader = leader;
    this.layerMask = (LayerMask) ((int) this.layerMask | 1 << LayerMask.NameToLayer("Island"));
    this.chaseForDuration = UnityEngine.Random.Range(120f, 240f);
    this.targetFollower = FollowerManager.FindFollowerByID(this._otherFollowerID);
  }

  public override int GetSubTaskCode() => this._otherFollowerID;

  public override void OnStart()
  {
    this.isSozoScenario = this._otherFollowerID == 99996 || this._brain.Info.ID == 99996;
    this.follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (this.IsLeader)
    {
      this.OtherChatTask = new FollowerTask_ChaseFollower(this._brain.Info.ID, false, this.maxDistance);
      this.OtherChatTask.OtherChatTask = this;
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._otherFollowerID);
      if (brainById != null)
      {
        brainById.TransitionToTask((FollowerTask) this.OtherChatTask);
        this.SetState(FollowerTaskState.GoingTo);
      }
      else
        this.End();
    }
    else
      this.SetState(FollowerTaskState.Idle);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.taskSet)
      return;
    if (this.isSozoScenario && this.IsLeader)
    {
      this.soundProgress += deltaGameTime;
      if ((double) this.soundProgress > 5.0 && (double) Vector3.Distance(this.Brain.LastPosition, this.OtherChatTask.Brain.LastPosition) < 7.0)
      {
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_sozo_evil", this.follower.gameObject);
        this.soundProgress = 0.0f;
      }
    }
    this.updateDestination -= deltaGameTime;
    if ((double) this.updateDestination <= 0.0)
    {
      this.RecalculateDestination();
      this.updateDestination = 1.5f;
      if (!this.IsLeader && (double) UnityEngine.Random.value < 0.05000000074505806 && (double) this._brain._directInfoAccess.Exhaustion <= 0.0 && (double) this.chaseProgress > 60.0)
        this._brain.MakeExhausted();
    }
    this.chaseProgress += deltaGameTime;
    if (this.IsLeader && (double) this.chaseProgress > (double) this.chaseForDuration && this.State == FollowerTaskState.GoingTo || (double) Vector3.Distance(this.Brain.LastPosition, this.OtherChatTask.Brain.LastPosition) > (double) this.maxDistance)
      this.EndInAnger();
    if (this.IsLeader && (double) Vector3.Distance(this.Brain.LastPosition, this.OtherChatTask.Brain.LastPosition) < 0.5 && this.State == FollowerTaskState.GoingTo && !this.taskSet)
    {
      this.ClearDestination();
      this.End();
      this._brain.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(this.OtherChatTask.Brain.Info.ID, true));
      this.taskSet = true;
    }
    if (this.OtherChatTask != null && this.OtherChatTask.Brain.CurrentTaskType != FollowerTaskType.ChaseFollower && this.State == FollowerTaskState.GoingTo)
      this.EndInAnger();
    if (this.IsLeader || this.OtherChatTask == null || this.OtherChatTask.Brain.CurrentTaskType == FollowerTaskType.ChaseFollower)
      return;
    this.End();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.IsLeader)
      return (UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null ? this.targetFollower.transform.position + (this.targetFollower.transform.position - follower.transform.position).normalized : this.OtherChatTask.Brain.LastPosition;
    float distance = 5f;
    Vector3 normalized = (this.OtherChatTask.Brain.LastPosition - follower.transform.position).normalized;
    Vector3 vector3 = Vector3.zero;
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) -normalized, distance, (int) this.layerMask);
    if ((UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null)
      vector3 = follower.transform.position + -normalized * distance;
    else if ((double) Mathf.Abs(normalized.y) > 0.5 && (double) Mathf.Abs(normalized.x) < 0.5)
    {
      raycastHit2D = Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.right, distance, (int) this.layerMask);
      if ((UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null)
      {
        vector3 = follower.transform.position + Vector3.right * distance;
      }
      else
      {
        raycastHit2D = Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.left, distance, (int) this.layerMask);
        if ((UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null)
          vector3 = follower.transform.position + Vector3.left * distance;
      }
    }
    else
    {
      raycastHit2D = Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.up, distance, (int) this.layerMask);
      if ((UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null)
      {
        vector3 = follower.transform.position + Vector3.up * distance;
      }
      else
      {
        raycastHit2D = Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.down, distance, (int) this.layerMask);
        if ((UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null)
          vector3 = follower.transform.position + Vector3.down * distance;
      }
    }
    if ((double) Mathf.Abs(vector3.x) < 3.0 && (double) vector3.y < -3.0 && (double) Vector3.Dot(normalized, Vector3.up) > 0.0)
      vector3 = follower.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.0f, -1f));
    if ((double) vector3.y > 2.0)
      vector3 = TownCentre.Instance.Centre.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 5f);
    return vector3;
  }

  public void EndInAnger()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    this.SetState(FollowerTaskState.Doing);
    string animation = "Conversations/react-hate" + UnityEngine.Random.Range(1, 4).ToString();
    System.Action onComplete = new System.Action(((FollowerTask) this).End);
    followerById.TimedAnimation(animation, 2f, onComplete);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (!follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      return;
    this.loopingSound = AudioManager.Instance.CreateLoop("event:/dialogue/followers/zombie_fol/zombie_mumble", follower.gameObject);
    int num = (int) this.loopingSound.setParameterByName("zombie_pitch", 1f);
    AudioManager.Instance.PlayLoop(this.loopingSound);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    AudioManager.Instance.StopLoop(this.loopingSound);
  }
}
