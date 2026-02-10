// Decompiled with JetBrains decompiler
// Type: FollowerTask_IceSculpture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_IceSculpture : FollowerTask
{
  public static List<StructureBrain.TYPES> SculptureTypes = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.ICE_SCULPTURE,
    StructureBrain.TYPES.ICE_SCULPTURE_1,
    StructureBrain.TYPES.ICE_SCULPTURE_2,
    StructureBrain.TYPES.ICE_SCULPTURE_3
  };
  public PlacementRegion.TileGridTile ClosestWasteTile;
  public Structures_IceSculpture targetSculpture;
  public Interaction_IceSculpture sculptureInteraction;
  public Follower sculptor;
  public Slot chiselSlot;
  public const float maxTargetSculptureAwaitTimer = 5f;
  public float targetsulptureAwaitTimer;
  public bool isFirstStageFinished;

  public override FollowerTaskType Type => FollowerTaskType.IceSculpture;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    this.targetsulptureAwaitTimer = 0.0f;
    this.SetTargetSculpture();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3_1 = Vector3.right * ((double) UnityEngine.Random.value < 0.5 ? 1f : -1f);
    Vector3 vector3_2;
    if (this.targetSculpture != null)
    {
      vector3_2 = this.targetSculpture.Data.Position + vector3_1;
    }
    else
    {
      this.ClosestWasteTile = StructureManager.GetCloseTile(this.Brain.LastPosition, this._brain.Location);
      if (this.ClosestWasteTile != null)
      {
        this.ClosestWasteTile.ReservedForWaste = true;
        vector3_2 = this.ClosestWasteTile.WorldPosition + vector3_1;
      }
      else
        vector3_2 = !((UnityEngine.Object) follower == (UnityEngine.Object) null) ? follower.transform.position : this._brain.LastPosition;
    }
    return vector3_2;
  }

  public List<PlacementRegion.TileGridTile> GetAvailableFollowerPositions(
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

  public override void TaskTick(float deltaGameTime)
  {
    if ((double) this.targetsulptureAwaitTimer >= 5.0)
    {
      this.targetsulptureAwaitTimer = 0.0f;
      this.End();
    }
    else if (this.targetSculpture == null)
    {
      this.SetTargetSculpture();
      this.targetsulptureAwaitTimer += deltaGameTime;
    }
    else if (!this.targetSculpture.IsFinished && !this.targetSculpture.Data.Destroyed)
    {
      this.targetSculpture.Data.Progress += deltaGameTime / this.targetSculpture.Duration;
      if ((UnityEngine.Object) this.sculptor != (UnityEngine.Object) null && (double) this.targetSculpture.Data.Progress >= 0.5 && !this.isFirstStageFinished)
      {
        this.sculptor.Spine.AnimationState.SetAnimation(1, "sculpt-ground", true);
        this.isFirstStageFinished = true;
      }
      if (!((UnityEngine.Object) this.sculptor != (UnityEngine.Object) null))
        return;
      this.sculptor.FacePosition(this.targetSculpture.Data.Position);
    }
    else
      this.End();
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    this.sculptor = follower;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.chiselSlot = follower.Spine.Skeleton.FindSlot("TOOL");
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    if (this.targetSculpture == null)
    {
      this.BuildSculptrue(follower);
      follower.Spine.AnimationState.SetAnimation(1, "sculpt", true);
    }
    else if ((double) this.targetSculpture.Data.Progress >= 0.5)
    {
      follower.Spine.AnimationState.SetAnimation(1, "sculpt-ground", true);
      this.isFirstStageFinished = true;
    }
    else
      follower.Spine.AnimationState.SetAnimation(1, "sculpt", true);
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.GetDestination((Follower) null);
    if (this.targetSculpture != null)
      return;
    this.BuildSculptrue();
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    if (this.targetSculpture != null)
    {
      if (this.targetSculpture.IsFinished)
        follower.TimedAnimation("Reactions/react-admire1", 2.1f, (System.Action) (() => this.Complete()));
      else if (this.targetSculpture.Data.Destroyed)
        follower.TimedAnimation("Reactions/react-cry", 4f, (System.Action) (() => this.Complete()));
      else
        this.Complete();
    }
    else
      this.Complete();
  }

  public override void OnComplete()
  {
    base.OnComplete();
    if (this.targetSculpture == null)
      return;
    this.targetSculpture.ReservedForTask = false;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void BuildSculptrue(Follower follower = null)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(FollowerTask_IceSculpture.SculptureTypes[UnityEngine.Random.Range(0, FollowerTask_IceSculpture.SculptureTypes.Count)], 0);
    infoByType.FollowerID = this._brain.Info.ID;
    Vector3 pos = this._brain.LastPosition;
    if (this.ClosestWasteTile != null)
    {
      if (this.ClosestWasteTile.CanPlaceStructure)
      {
        infoByType.GridTilePosition = this.ClosestWasteTile.Position;
        StructureManager.BuildStructure(this._brain.Location, infoByType, this.ClosestWasteTile.WorldPosition, Vector2Int.one);
        pos = this.ClosestWasteTile.WorldPosition;
      }
    }
    else
    {
      StructureManager.BuildStructure(this._brain.Location, infoByType, this._currentDestination.Value, Vector2Int.one);
      pos = this._currentDestination.Value;
    }
    AudioManager.Instance.PlayOneShot("event:/dlc/env/icesculpture/spawn", pos);
    if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
      return;
    follower.FacePosition(this.ClosestWasteTile.WorldPosition);
  }

  public override void ClaimReservations()
  {
    if (this.ClosestWasteTile == null)
      return;
    this.ClosestWasteTile.ReservedForWaste = true;
  }

  public override void ReleaseReservations()
  {
    if (this.ClosestWasteTile == null)
      return;
    this.ClosestWasteTile.ReservedForWaste = false;
  }

  public void SetTargetSculpture()
  {
    foreach (StructureBrain structuresOfType in StructureManager.GetAllStructuresOfTypes(FollowerTask_IceSculpture.SculptureTypes.ToArray()))
    {
      Structures_IceSculpture structuresIceSculpture = (Structures_IceSculpture) structuresOfType;
      if (structuresOfType.Data.FollowerID == this.Brain.Info.ID && !structuresOfType.ReservedForTask && !structuresIceSculpture.IsFinished && !structuresIceSculpture.Data.Destroyed)
      {
        this.targetSculpture = structuresIceSculpture;
        this.targetSculpture.ReservedForTask = true;
        using (List<Interaction_IceSculpture>.Enumerator enumerator = Interaction_IceSculpture.IceSculptures.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Interaction_IceSculpture current = enumerator.Current;
            if (current.structure.Brain == structuresOfType)
            {
              this.sculptureInteraction = current;
              break;
            }
          }
          break;
        }
      }
    }
  }

  public void HandleAnimationStateEvent(TrackEntry track, Spine.Event e)
  {
    if (!(e.Data.Name == "Build"))
      return;
    this.sculptureInteraction?.Hit(this.sculptor.Spine.transform.TransformPoint(new Vector3(this.chiselSlot.Bone.WorldX, this.chiselSlot.Bone.WorldY, 0.0f)));
  }

  [CompilerGenerated]
  public void \u003COnFinaliseBegin\u003Eb__27_0() => this.Complete();

  [CompilerGenerated]
  public void \u003COnFinaliseBegin\u003Eb__27_1() => this.Complete();
}
