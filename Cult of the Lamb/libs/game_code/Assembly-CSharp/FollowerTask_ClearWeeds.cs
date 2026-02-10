// Decompiled with JetBrains decompiler
// Type: FollowerTask_ClearWeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_ClearWeeds : FollowerTask_AssistPlayerBase
{
  public const float REMOVAL_DURATION_GAME_MINUTES = 4f;
  public const float REMOVAL_DURATION_GAME_MINUTES_PLAYER = 2f;
  public int _weedID;
  public FollowerLocation _location;
  public float _removalProgress;
  public float _gameTimeSinceLastProgress;
  public Structures_Weeds _weed;
  public List<Structures_Weeds> cachedWeeds;

  public override FollowerTaskType Type => FollowerTaskType.ClearWeeds;

  public override FollowerLocation Location => this._location;

  public override float Priorty
  {
    get => this._weed != null && this._weed.Data.PrioritisedAsBuildingObstruction ? 100f : 2f;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return this._weed != null && this._weed.Data.PrioritisedAsBuildingObstruction ? PriorityCategory.OverrideWorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_ClearWeeds(int weedID)
  {
    this._helpingPlayer = false;
    this._weedID = weedID;
    this._weed = StructureManager.GetStructureByID<Structures_Weeds>(this._weedID);
    this._location = this._weed.Data.Location;
  }

  public FollowerTask_ClearWeeds(Interaction_Weed weed)
  {
    this._helpingPlayer = true;
    this._location = weed.StructureInfo.Location;
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    this._weed = StructureManager.GetStructureByID<Structures_Weeds>(this._weedID);
    if (this._weed == null)
      return;
    this._weed.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    this._weed = StructureManager.GetStructureByID<Structures_Weeds>(this._weedID);
    if (this._weed == null)
      return;
    this._weed.ReservedForTask = false;
  }

  public override void OnStart()
  {
    this.ReleaseReservations();
    this.Loop(true);
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void AssistPlayerTick(float deltaGameTime)
  {
    if (LocationManager.GetLocationState(this._location) == LocationState.Active)
    {
      Interaction_Weed weed = this.FindWeed();
      if ((Object) weed == (Object) null || weed.Activating)
      {
        this._weed = (Structures_Weeds) null;
        this._weedID = -1;
        this.SetState(FollowerTaskState.Idle);
        this.Loop();
      }
    }
    else if (this._weed == null)
    {
      this.SetState(FollowerTaskState.Idle);
      this.Loop();
    }
    if (this.State != FollowerTaskState.Doing)
      return;
    this._gameTimeSinceLastProgress += deltaGameTime;
    this._weed.PickWeeds(1f);
    this._removalProgress += this._gameTimeSinceLastProgress * this._brain.Info.ProductivityMultiplier;
    this._gameTimeSinceLastProgress = 0.0f;
    if (!this._weed.PickedWeeds)
      return;
    this.ProgressTask();
  }

  public override void ProgressTask()
  {
    this._weed = StructureManager.GetStructureByID<Structures_Weeds>(this._weedID);
    if (this._weed == null)
    {
      this.End();
    }
    else
    {
      if (this._brain.Location != PlayerFarming.Location && this._weed.DropWeed)
      {
        List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(this._brain.Location);
        if (structuresOfType.Count > 0)
          structuresOfType[0].AddItem(InventoryItem.ITEM_TYPE.GRASS, 1);
      }
      this._removalProgress = 0.0f;
      this._weed.Remove();
      this._brain.GetXP(0.01f);
      this.Loop();
    }
  }

  public void Loop(bool force = false)
  {
    if (!force && this._helpingPlayer && this.EndIfPlayerIsDistant())
      return;
    Structures_Weeds nextWeed = this.GetNextWeed();
    if (nextWeed == null)
    {
      this.End();
    }
    else
    {
      this.ReleaseReservations();
      this.ClearDestination();
      this._weedID = nextWeed.Data.ID;
      this._weed = nextWeed;
      this._location = nextWeed.Data.Location;
      nextWeed.ReservedForTask = true;
      this.SetState(FollowerTaskState.GoingTo);
    }
  }

  public Structures_Weeds GetNextWeed()
  {
    this.ReleaseReservations();
    Structures_Weeds nextWeed = (Structures_Weeds) null;
    float num1 = float.MaxValue;
    float num2 = this._helpingPlayer ? this.AssistRange : float.MaxValue;
    PlayerFarming instance = PlayerFarming.Instance;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    StructureManager.TryGetAllAvailableWeeds(ref this.cachedWeeds, this.Location);
    foreach (Structures_Weeds cachedWeed in this.cachedWeeds)
    {
      if ((Object) followerById == (Object) null)
      {
        nextWeed = cachedWeed;
        break;
      }
      if (cachedWeed.Data.PrioritisedAsBuildingObstruction)
      {
        float num3 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, cachedWeed.Data.Position);
        if ((double) num3 < (double) num2)
        {
          float num4 = num3 + (cachedWeed.Data.Prioritised ? 0.0f : 1000f);
          if ((double) num4 < (double) num1)
          {
            nextWeed = cachedWeed;
            num1 = num4;
          }
        }
      }
    }
    this.cachedWeeds.Clear();
    return nextWeed;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Structures_Weeds structureById = StructureManager.GetStructureByID<Structures_Weeds>(this._weedID);
    return structureById != null ? structureById.Data.Position + new Vector3(-0.2f, 0.0f, 0.0f) : Vector3.zero;
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this._weedID == 0)
    {
      this.ProgressTask();
    }
    else
    {
      Interaction_Weed weed = this.FindWeed();
      if ((Object) weed != (Object) null)
        follower.FacePosition(weed.transform.position);
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) follower.SetBodyAnimation("action", true);
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Chop"))
      return;
    this.ProgressTask();
  }

  public Interaction_Weed FindWeed()
  {
    Interaction_Weed weed1 = (Interaction_Weed) null;
    foreach (Interaction_Weed weed2 in Interaction_Weed.Weeds)
    {
      if (weed2.StructureInfo.ID == this._weedID)
      {
        weed1 = weed2;
        break;
      }
    }
    return weed1;
  }
}
