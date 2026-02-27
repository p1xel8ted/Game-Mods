// Decompiled with JetBrains decompiler
// Type: FollowerTask_ClearRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_ClearRubble : FollowerTask_AssistPlayerBase
{
  private FollowerLocation _location;
  private new bool _helpingPlayer;
  private int _rubbleID;
  private Structures_Rubble rubble;
  private float _gameTimeSinceLastProgress;
  private float WaitTimer;

  public override FollowerTaskType Type => FollowerTaskType.ClearRubble;

  public override FollowerLocation Location => this.rubble.Data.Location;

  public override bool BlockTaskChanges => this._helpingPlayer;

  public override int UsingStructureID => this._rubbleID;

  public override float Priorty => !this.rubble.Data.Prioritised ? 1f : 5f;

  public int RubbleID => this._rubbleID;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    if (FollowerRole != FollowerRole.StoneMiner)
      return PriorityCategory.Low;
    return this.rubble.RockSize != 0 ? PriorityCategory.Medium : PriorityCategory.WorkPriority;
  }

  public FollowerTask_ClearRubble(int rubbleID)
  {
    this._helpingPlayer = false;
    this._rubbleID = rubbleID;
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    this._location = this.rubble.Data.Location;
  }

  private Structures_Rubble GetNextRubble()
  {
    this.ReleaseReservations();
    Structures_Rubble nextRubble = (Structures_Rubble) null;
    float num1 = float.MaxValue;
    float num2 = this._helpingPlayer ? this.AssistRange : float.MaxValue;
    PlayerFarming instance = PlayerFarming.Instance;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    List<Structures_Rubble> allAvailableRubble = StructureManager.GetAllAvailableRubble(this.Location);
    foreach (Structures_Rubble structuresRubble in allAvailableRubble)
    {
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      {
        nextRubble = structuresRubble;
        break;
      }
      float num3 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, structuresRubble.Data.Position);
      if (structuresRubble.RockSize == 0 && (double) num3 < (double) num2)
      {
        float num4 = num3 + (structuresRubble.Data.Prioritised ? 0.0f : 1000f);
        if ((double) num4 < (double) num1)
        {
          nextRubble = structuresRubble;
          num1 = num4;
        }
      }
    }
    if (nextRubble == null)
    {
      foreach (Structures_Rubble structuresRubble in allAvailableRubble)
      {
        if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
        {
          nextRubble = structuresRubble;
          break;
        }
        float num5 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, structuresRubble.Data.Position);
        if (structuresRubble.RockSize == 1 && (double) num5 < (double) num2)
        {
          float num6 = num5 + (structuresRubble.Data.Prioritised ? 0.0f : 1000f);
          if ((double) num6 < (double) num1)
          {
            nextRubble = structuresRubble;
            num1 = num6;
          }
        }
      }
    }
    return nextRubble;
  }

  public FollowerTask_ClearRubble(Rubble rubble)
  {
    this._helpingPlayer = true;
    this._rubbleID = rubble.StructureInfo.ID;
    Interaction_PlayerClearRubble.PlayerActivatingEnd += new Action<Rubble>(this.OnPlayerActivatingEnd);
  }

  public override void ClaimReservations()
  {
    if (this._helpingPlayer)
      return;
    Structures_Rubble structureById = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    if (structureById != null && structureById.AvailableSlotCount > 0)
      --structureById.AvailableSlotCount;
    else
      this.End();
  }

  public override void ReleaseReservations()
  {
    if (this._helpingPlayer)
      return;
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    if (this.rubble == null)
      return;
    ++this.rubble.AvailableSlotCount;
  }

  protected override void OnStart()
  {
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    if (this.rubble != null)
    {
      this.rubble.OnRemovalComplete += new System.Action(this.OnRemovalComplete);
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  protected override void OnComplete()
  {
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    if (this.rubble != null)
      this.rubble.OnRemovalComplete -= new System.Action(this.OnRemovalComplete);
    this._brain.GetXP(1f);
    Interaction_PlayerClearRubble.PlayerActivatingEnd -= new Action<Rubble>(this.OnPlayerActivatingEnd);
  }

  protected override void AssistPlayerTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Wait)
    {
      if ((double) (this.WaitTimer += deltaGameTime) <= 60.0 && PlayerFarming.Location != this._brain.Location)
        return;
      this.End();
    }
    else
    {
      if (this.State != FollowerTaskState.Doing)
        return;
      this._gameTimeSinceLastProgress += deltaGameTime;
      if (this._brain.Location == PlayerFarming.Location || (double) this._gameTimeSinceLastProgress <= (double) this.ConvertAnimTimeToGameTime(1.9f) / 2.0)
        return;
      this.ProgressTask();
    }
  }

  private float ConvertAnimTimeToGameTime(float duration) => duration * 2f;

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Chop"))
      return;
    this.ProgressTask();
  }

  public override void ProgressTask()
  {
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    if (this.rubble == null || this.rubble.ProgressFinished)
    {
      Debug.Log((object) "rubble is null so END");
      this.End();
    }
    else
    {
      this.rubble.RemovalProgress += this._gameTimeSinceLastProgress * 0.25f;
      if (this._brain != null && this._brain.Info != null && this.rubble != null)
        this.rubble.UpdateProgress(this._brain.Info.ID);
      this._gameTimeSinceLastProgress = 0.0f;
    }
  }

  private void OnRemovalComplete()
  {
    if (this._brain.Location != PlayerFarming.Location)
    {
      List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(this._brain.Location);
      if (structuresOfType.Count > 0 && this.rubble != null && this.rubble.Data != null)
        structuresOfType[0].AddItem(this.rubble.Data.LootToDrop, Mathf.RoundToInt((float) this.rubble.RubbleDropAmount * this._brain.ResourceHarvestingMultiplier));
      this.WaitTimer = 0.0f;
      this.SetState(FollowerTaskState.Wait);
    }
    else
    {
      if (this.rubble != null && this.rubble.Data != null)
        this.rubble.Data.FollowerID = this._brain.Info.ID;
      this.End();
    }
  }

  private void OnPlayerActivatingEnd(Rubble rubble)
  {
    if (rubble.StructureInfo.ID != this._rubbleID)
      return;
    this.End();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    return this.rubble.Data.Position + new Vector3(0.0f, (float) (this.rubble.Data.Bounds.y / 2)) + (Vector3) (UnityEngine.Random.insideUnitCircle * (float) this.rubble.Data.Bounds.x);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    if (this.rubble != null && !this.rubble.ProgressFinished)
      return;
    Structures_Rubble nextRubble = this.GetNextRubble();
    if (nextRubble == null)
    {
      this.End();
    }
    else
    {
      this.ReleaseReservations();
      this.ClearDestination();
      this._rubbleID = nextRubble.Data.ID;
      this._location = nextRubble.Data.Location;
      nextRubble.ReservedForTask = true;
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    this.rubble = StructureManager.GetStructureByID<Structures_Rubble>(this._rubbleID);
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.rubble.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("mining", true);
  }

  private Rubble FindRubble()
  {
    Rubble rubble1 = (Rubble) null;
    foreach (Rubble rubble2 in Rubble.Rubbles)
    {
      if (rubble2.StructureInfo.ID == this._rubbleID)
      {
        rubble1 = rubble2;
        break;
      }
    }
    return rubble1;
  }
}
