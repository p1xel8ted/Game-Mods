// Decompiled with JetBrains decompiler
// Type: FollowerTask_Bathroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Bathroom : FollowerTask
{
  private int _toiletID;
  private Structures_Outhouse _toilet;
  private Coroutine _doorCoroutine;
  private int tryUseOuthouseCounter;
  private const int maxOuthouseTries = 3;
  private PlacementRegion.TileGridTile ClosestWasteTile;

  public override FollowerTaskType Type => FollowerTaskType.Bathroom;

  public override FollowerLocation Location
  {
    get => this._hasToilet ? this._toilet.Data.Location : this._brain.HomeLocation;
  }

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override int UsingStructureID
  {
    get
    {
      int usingStructureId = 0;
      if (this._hasToilet)
        usingStructureId = this._toiletID;
      return usingStructureId;
    }
  }

  private bool _hasToilet => this._toiletID != 0;

  public FollowerTask_Bathroom()
  {
  }

  public FollowerTask_Bathroom(int toiletID)
  {
    this._toiletID = toiletID;
    this._toilet = StructureManager.GetStructureByID<Structures_Outhouse>(this._toiletID);
    this.tryUseOuthouseCounter = 0;
  }

  protected override int GetSubTaskCode() => this.UsingStructureID;

  public override void ClaimReservations()
  {
    if (this._hasToilet && this._toilet != null && !this._toilet.ReservedForTask)
      this._toilet.ReservedForTask = true;
    if (this.ClosestWasteTile == null)
      return;
    this.ClosestWasteTile.ReservedForWaste = true;
  }

  public override void ReleaseReservations()
  {
    if (this._hasToilet)
      this._toilet.ReservedForTask = false;
    if (this.ClosestWasteTile == null)
      return;
    this.ClosestWasteTile.ReservedForWaste = false;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
  }

  public override void ProgressTask()
  {
    if (!this._hasToilet)
    {
      StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.POOP, 0);
      infoByType.FollowerID = this._brain.Info.ID;
      this._brain.AddThought(Thought.BathroomOutside);
      if (this.ClosestWasteTile != null)
      {
        infoByType.GridTilePosition = this.ClosestWasteTile.Position;
        StructureManager.BuildStructure(this._brain.Location, infoByType, this.ClosestWasteTile.WorldPosition, Vector2Int.one, false, new Action<GameObject>(this.LerpPoop));
      }
      else
        StructureManager.BuildStructure(this._brain.Location, infoByType, this._currentDestination.Value, Vector2Int.one, false, new Action<GameObject>(this.LerpPoop));
    }
    else
    {
      ++this._toilet.Data.TotalPoops;
      this._toilet.DepositItem(InventoryItem.ITEM_TYPE.POOP);
      switch (this._toilet.Data.Type)
      {
        case StructureBrain.TYPES.OUTHOUSE:
          this._brain.AddThought(Thought.BathroomOuthouse);
          break;
        case StructureBrain.TYPES.OUTHOUSE_2:
          this._brain.AddThought(Thought.BathroomOuthouse2);
          break;
      }
    }
    this._brain.Stats.TargetBathroom = 0.0f;
  }

  private void LerpPoop(GameObject poop)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      return;
    Vector3 position = poop.transform.position;
    poop.transform.position = followerById.transform.position;
    poop.transform.localScale = Vector3.zero;
    poop.transform.DOMove(position, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    poop.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    Interaction_Outhouse outhouse = this.FindOuthouse();
    Vector3 vector3;
    if (this._hasToilet && (UnityEngine.Object) follower != (UnityEngine.Object) null && (bool) (UnityEngine.Object) outhouse && !outhouse.IsFull)
    {
      vector3 = outhouse.StructureInfo.FollowerID != this._brain.Info.ID ? outhouse.WaitingFollowerPosition.position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.5f : outhouse.InsideFollowerPosition.position;
    }
    else
    {
      this.ClosestWasteTile = StructureManager.GetBestWasteTile(this._brain.Location);
      if (this.ClosestWasteTile != null)
      {
        this.ClosestWasteTile.ReservedForWaste = true;
        List<PlacementRegion.TileGridTile> followerPositions = this.GetAvailableFollowerPositions(this.ClosestWasteTile);
        vector3 = followerPositions.Count <= 0 ? this.ClosestWasteTile.WorldPosition : followerPositions[UnityEngine.Random.Range(0, followerPositions.Count)].WorldPosition;
      }
      else
        vector3 = !((UnityEngine.Object) follower == (UnityEngine.Object) null) ? follower.transform.position : this._brain.LastPosition;
    }
    return vector3;
  }

  private List<PlacementRegion.TileGridTile> GetAvailableFollowerPositions(
    PlacementRegion.TileGridTile poopPosition)
  {
    List<PlacementRegion.TileGridTile> followerPositions = new List<PlacementRegion.TileGridTile>();
    PlacementRegion.TileGridTile closeTile1 = StructureManager.GetCloseTile(poopPosition.WorldPosition + Vector3.up * 0.5f, FollowerLocation.Base);
    if (closeTile1 != null)
      followerPositions.Add(closeTile1);
    PlacementRegion.TileGridTile closeTile2 = StructureManager.GetCloseTile(poopPosition.WorldPosition - Vector3.up * 0.5f, FollowerLocation.Base);
    if (closeTile2 != null)
      followerPositions.Add(closeTile2);
    PlacementRegion.TileGridTile closeTile3 = StructureManager.GetCloseTile(poopPosition.WorldPosition + Vector3.right * 0.5f, FollowerLocation.Base);
    if (closeTile3 != null)
      followerPositions.Add(closeTile3);
    PlacementRegion.TileGridTile closeTile4 = StructureManager.GetCloseTile(poopPosition.WorldPosition - Vector3.right * 0.5f, FollowerLocation.Base);
    if (closeTile4 != null)
      followerPositions.Add(closeTile4);
    return followerPositions;
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (!this._hasToilet)
      return;
    Interaction_Outhouse outhouse = this.FindOuthouse();
    if (!((UnityEngine.Object) outhouse != (UnityEngine.Object) null) || outhouse.StructureInfo.FollowerID != this._brain.Info.ID)
      return;
    this._doorCoroutine = follower.StartCoroutine((IEnumerator) this.DoorCoroutine(follower, outhouse));
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (!this._hasToilet && (UnityEngine.Object) follower != (UnityEngine.Object) null)
    {
      this.GetDestination(follower);
      follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
      follower.TimedAnimation("poop", 1.5333333f, (System.Action) (() =>
      {
        follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
        follower.FacePosition(this._currentDestination.Value);
        if (this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
          follower.TimedAnimation("Reactions/react-laugh", 3.33333325f, new System.Action(((FollowerTask) this).End), false);
        else
          follower.TimedAnimation("Reactions/react-embarrassed", 3f, new System.Action(((FollowerTask) this).End), false);
      }));
    }
    else
    {
      Interaction_Outhouse outhouse = this.FindOuthouse();
      if ((UnityEngine.Object) outhouse != (UnityEngine.Object) null && outhouse.StructureInfo != null && outhouse.StructureInfo.FollowerID == this._brain.Info.ID)
        follower.TimedAnimation("poop-outhouse", 3f, (System.Action) (() =>
        {
          this.ProgressTask();
          this.End();
        }));
      else if ((UnityEngine.Object) outhouse != (UnityEngine.Object) null)
      {
        if (this._toilet != null && this._toilet.Data != null && this._toilet.Data.FollowerID == -1)
        {
          this._toilet.Data.FollowerID = this._brain.Info.ID;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else if (this.tryUseOuthouseCounter >= 3 || outhouse.IsFull)
        {
          this._toiletID = 0;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
          follower.TimedAnimation("waiting", 3.2f, (System.Action) (() =>
          {
            ++this.tryUseOuthouseCounter;
            this.SetState(FollowerTaskState.GoingTo);
          }));
      }
      else
        this.End();
    }
  }

  protected override void OnEnd()
  {
    base.OnEnd();
    Interaction_Outhouse outhouse = this.FindOuthouse();
    if (!((UnityEngine.Object) outhouse != (UnityEngine.Object) null))
      return;
    outhouse.StructureInfo.FollowerID = -1;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (this._doorCoroutine == null)
      return;
    follower.StopCoroutine(this._doorCoroutine);
    this._doorCoroutine = (Coroutine) null;
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Poop"))
      return;
    this.ProgressTask();
  }

  private Interaction_Outhouse FindOuthouse()
  {
    Interaction_Outhouse outhouse1 = (Interaction_Outhouse) null;
    foreach (Interaction_Outhouse outhouse2 in Interaction_Outhouse.Outhouses)
    {
      if (outhouse2.StructureInfo.ID == this._toiletID)
      {
        outhouse1 = outhouse2;
        break;
      }
    }
    return outhouse1;
  }

  protected override void OnAbort()
  {
    base.OnAbort();
    Interaction_Outhouse outhouse = this.FindOuthouse();
    if ((bool) (UnityEngine.Object) outhouse)
    {
      outhouse.DoorClosed.SetActive(true);
      outhouse.DoorOpen.SetActive(false);
    }
    if (this._toilet == null || this._toilet.Data == null || this._toilet.Data.FollowerID != this._brain.Info.ID)
      return;
    this._toilet.Data.FollowerID = -1;
  }

  private IEnumerator DoorCoroutine(Follower follower, Interaction_Outhouse outhouse)
  {
    FollowerTask_Bathroom followerTaskBathroom = this;
    while (followerTaskBathroom.State == FollowerTaskState.GoingTo && (double) Vector3.Distance(follower.transform.position, outhouse.InsideFollowerPosition.position) > 2.0)
      yield return (object) null;
    outhouse.DoorClosed.SetActive(false);
    outhouse.DoorOpen.SetActive(true);
    while (followerTaskBathroom.State == FollowerTaskState.GoingTo)
      yield return (object) null;
    outhouse.DoorClosed.SetActive(true);
    outhouse.DoorOpen.SetActive(false);
    while (followerTaskBathroom.State == FollowerTaskState.Doing)
      yield return (object) null;
    outhouse.DoorClosed.SetActive(false);
    outhouse.DoorOpen.SetActive(true);
    yield return (object) new WaitForSeconds(0.5f);
    outhouse.DoorClosed.SetActive(true);
    outhouse.DoorOpen.SetActive(false);
    followerTaskBathroom._doorCoroutine = (Coroutine) null;
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.GetDestination((Follower) null);
    this.End();
  }
}
