// Decompiled with JetBrains decompiler
// Type: FollowerTask_CleanWaste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_CleanWaste : FollowerTask_AssistPlayerBase
{
  public const float REMOVAL_DURATION_GAME_MINUTES = 4f;
  public const float REMOVAL_DURATION_GAME_MINUTES_PLAYER = 2f;
  private int _wasteID;
  private FollowerLocation _location;
  private float _removalProgress;
  private float _gameTimeSinceLastProgress;
  private Structures_Waste waste;

  public override FollowerTaskType Type => FollowerTaskType.CleanWaste;

  public override FollowerLocation Location => this._location;

  public override float Priorty => !this.waste.Data.Prioritised ? 3f : 6f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Lumberjack:
      case FollowerRole.Monk:
        return PriorityCategory.Low;
      case FollowerRole.Worker:
      case FollowerRole.Farmer:
        return PriorityCategory.Medium;
      default:
        return PriorityCategory.Low;
    }
  }

  public FollowerTask_CleanWaste(int wasteID)
  {
    this._wasteID = wasteID;
    this.waste = StructureManager.GetStructureByID<Structures_Waste>(this._wasteID);
    this._location = this.waste.Data.Location;
  }

  public override void ClaimReservations()
  {
    this.waste = StructureManager.GetStructureByID<Structures_Waste>(this._wasteID);
    if (this.waste == null)
      return;
    this.waste.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    this.waste = StructureManager.GetStructureByID<Structures_Waste>(this._wasteID);
    if (this.waste == null)
      return;
    this.waste.ReservedForTask = false;
  }

  protected override void OnStart()
  {
    if (this._wasteID == 0)
      this.Loop(true);
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void AssistPlayerTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.GoingTo)
    {
      if (LocationManager.GetLocationState(this._location) == LocationState.Active)
      {
        if ((UnityEngine.Object) this.FindWaste() == (UnityEngine.Object) null)
        {
          this.SetState(FollowerTaskState.Idle);
          this.Loop();
        }
      }
      else
      {
        this.waste = StructureManager.GetStructureByID<Structures_Waste>(this._wasteID);
        if (this.waste == null)
        {
          this.SetState(FollowerTaskState.Idle);
          this.Loop();
        }
      }
    }
    if (this.State != FollowerTaskState.Doing)
      return;
    float num = 1f;
    this._gameTimeSinceLastProgress += deltaGameTime * num;
  }

  public override void ProgressTask()
  {
    this.waste = StructureManager.GetStructureByID<Structures_Waste>(this._wasteID);
    if (this.waste == null)
    {
      this.End();
    }
    else
    {
      this._removalProgress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._removalProgress < 4.0)
        return;
      this._removalProgress = 0.0f;
      this.waste.Remove();
      this.Loop();
    }
  }

  private void Loop(bool force = false)
  {
    if (!force && this._helpingPlayer && this.EndIfPlayerIsDistant())
      return;
    Structures_Waste nextWaste = this.GetNextWaste();
    if (nextWaste == null)
    {
      this.End();
    }
    else
    {
      this.ClearDestination();
      this._wasteID = nextWaste.Data.ID;
      nextWaste.ReservedForTask = true;
      this.SetState(FollowerTaskState.GoingTo);
    }
  }

  private Structures_Waste GetNextWaste()
  {
    Structures_Waste nextWaste = (Structures_Waste) null;
    float num1 = float.MaxValue;
    float num2 = this._helpingPlayer ? this.AssistRange : float.MaxValue;
    PlayerFarming instance = PlayerFarming.Instance;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    foreach (Structures_Waste structuresWaste in StructureManager.GetAllAvailableWaste(this.Location))
    {
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      {
        nextWaste = structuresWaste;
        break;
      }
      float num3 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, structuresWaste.Data.Position);
      if ((double) num3 < (double) num2)
      {
        float num4 = num3 + (structuresWaste.Data.Prioritised ? 0.0f : 1000f);
        if ((double) num4 < (double) num1)
        {
          nextWaste = structuresWaste;
          num1 = num4;
        }
      }
    }
    return nextWaste;
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    this.waste = StructureManager.GetStructureByID<Structures_Waste>(this._wasteID);
    return this.waste.Data.Position + new Vector3(-0.2f, 0.0f, 0.0f);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this._wasteID == 0)
    {
      this.ProgressTask();
    }
    else
    {
      Waste waste = this.FindWaste();
      follower.FacePosition(waste.transform.position);
      follower.TimedAnimation("action", 3.5f, (System.Action) (() => this.ProgressTask()));
    }
  }

  private Waste FindWaste()
  {
    Waste waste1 = (Waste) null;
    foreach (Waste waste2 in Waste.Wastes)
    {
      if (waste2.StructureInfo.ID == this._wasteID)
      {
        waste1 = waste2;
        break;
      }
    }
    return waste1;
  }
}
