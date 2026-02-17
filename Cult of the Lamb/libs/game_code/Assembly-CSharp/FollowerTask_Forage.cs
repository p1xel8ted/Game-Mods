// Decompiled with JetBrains decompiler
// Type: FollowerTask_Forage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Forage : FollowerTask_AssistPlayerBase
{
  public float _removalProgress;
  public float _gameTimeSinceLastProgress;
  public Structures_BerryBush _berries;
  public int _berryID;
  public FollowerLocation _location;
  public float WaitTimer;
  public List<Structures_BerryBush> cachedBerryBushes = new List<Structures_BerryBush>();

  public override FollowerTaskType Type => FollowerTaskType.Forage;

  public override FollowerLocation Location => this._location;

  public override float Priorty => 3f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Forager || FollowerRole == FollowerRole.Berries ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Forage(int BerryID)
  {
    this._helpingPlayer = false;
    this._berryID = BerryID;
    this._berries = StructureManager.GetStructureByID<Structures_BerryBush>(BerryID);
    this._location = this._berries.Data.Location;
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    this._berries = StructureManager.GetStructureByID<Structures_BerryBush>(this._berryID);
    if (this._berries == null)
      return;
    this._berries.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    this._berries = StructureManager.GetStructureByID<Structures_BerryBush>(this._berryID);
    if (this._berries == null)
      return;
    this._berries.ReservedForTask = false;
  }

  public FollowerTask_Forage() => this._helpingPlayer = true;

  public override void OnStart()
  {
    this.ReleaseReservations();
    this.Loop(true);
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void AssistPlayerTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Wait)
    {
      if (PlayerFarming.Location == this._brain.Location)
      {
        this.Loop();
      }
      else
      {
        if ((double) (this.WaitTimer += deltaGameTime) <= 60.0)
          return;
        this.Loop();
      }
    }
    else
    {
      if (LocationManager.GetLocationState(this._location) == LocationState.Active)
      {
        Interaction_Berries bush = this.FindBush();
        if ((this.State == FollowerTaskState.GoingTo || this.State == FollowerTaskState.Doing) && ((Object) bush == (Object) null || this._berries.BerryPicked || this._berries.IsCrop))
        {
          this._berries = (Structures_BerryBush) null;
          this._berryID = -1;
          this.SetState(FollowerTaskState.Idle);
          this.Loop();
        }
        if (this._berries != null && (Object) bush != (Object) null && (bool) (Object) bush.GetComponentInParent<CropController>())
        {
          this._berries.IsCrop = true;
          this.End();
        }
      }
      else if (this._berries == null)
      {
        this.SetState(FollowerTaskState.Idle);
        this.Loop();
      }
      if (this.State != FollowerTaskState.Doing)
        return;
      this._gameTimeSinceLastProgress += deltaGameTime;
      this.ProgressTask();
    }
  }

  public override void ProgressTask()
  {
    if (this._berries == null || this._berries.Data.Picked)
    {
      this.End();
    }
    else
    {
      this._berries.PickBerries((float) ((double) this._gameTimeSinceLastProgress * (double) this._brain.Info.ProductivityMultiplier * 0.5));
      this._gameTimeSinceLastProgress = 0.0f;
      if (!this._berries.BerryPicked)
        return;
      if (this._brain.Location != PlayerFarming.Location)
      {
        this._berries.AddBerriesToChest(this._brain.Location);
        if (!this._berries.Data.CanRegrow)
          this._berries.Remove();
      }
      this._berries = (Structures_BerryBush) null;
      this._brain.GetXP(0.5f);
      if (this._brain.Location != PlayerFarming.Location)
      {
        this.WaitTimer = 0.0f;
        this.SetState(FollowerTaskState.Wait);
      }
      else
        this.Loop();
    }
  }

  public void Loop(bool force = false)
  {
    if (!force && this._helpingPlayer && this.EndIfPlayerIsDistant())
      return;
    Structures_BerryBush nextBush = this.GetNextBush();
    if (nextBush == null)
    {
      this.End();
    }
    else
    {
      this.ReleaseReservations();
      this._berryID = nextBush.Data.ID;
      this._berries = nextBush;
      this._location = nextBush.Data.Location;
      this._berries.ReservedForTask = true;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
  }

  public Structures_BerryBush GetNextBush()
  {
    this.ReleaseReservations();
    Structures_BerryBush nextBush = (Structures_BerryBush) null;
    float num1 = float.MaxValue;
    float num2 = this._helpingPlayer ? this.AssistRange : float.MaxValue;
    PlayerFarming instance = PlayerFarming.Instance;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    StructureManager.TryGetAllAvailableBushes(ref this.cachedBerryBushes, this.Location);
    foreach (Structures_BerryBush cachedBerryBush in this.cachedBerryBushes)
    {
      if ((Object) followerById == (Object) null)
      {
        nextBush = cachedBerryBush;
        break;
      }
      float num3 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, cachedBerryBush.Data.Position);
      if ((double) num3 < (double) num2)
      {
        float num4 = num3 + (cachedBerryBush.Data.Prioritised ? 0.0f : 1000f);
        if ((double) num4 < (double) num1)
        {
          nextBush = cachedBerryBush;
          num1 = num4;
        }
      }
    }
    this.cachedBerryBushes.Clear();
    return nextBush;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this._berries != null ? this._berries.Data.Position + new Vector3((double) Random.value < 0.5 ? -0.4f : 0.4f, -0.2f, 0.0f) : follower.transform.position;
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this._berryID == 0)
      return;
    if ((Object) this.FindBush() != (Object) null)
      follower.FacePosition(this._berries.Data.Position);
    follower.FacePosition(this._berries.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("action", true);
  }

  public Interaction_Berries FindBush()
  {
    Interaction_Berries bush = (Interaction_Berries) null;
    foreach (Interaction_Berries berry in Interaction_Berries.Berries)
    {
      if (berry.StructureInfo.ID == this._berryID)
      {
        bush = berry;
        break;
      }
    }
    return bush;
  }
}
