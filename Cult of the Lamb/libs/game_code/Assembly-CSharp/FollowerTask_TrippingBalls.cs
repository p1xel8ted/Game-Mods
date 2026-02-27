// Decompiled with JetBrains decompiler
// Type: FollowerTask_TrippingBalls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_TrippingBalls : FollowerTask
{
  public Follower _follower;
  public Structures_BerryBush targetShrooms;
  public float progress;
  public List<Structures_BerryBush> cachedBerryBushes = new List<Structures_BerryBush>();

  public override FollowerTaskType Type => FollowerTaskType.TrippingBalls;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override void OnStart()
  {
    this.targetShrooms = this.GetNextShroomCrop();
    if (this.targetShrooms != null)
      this.targetShrooms.ReservedForTask = true;
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    if (this.targetShrooms == null)
      return;
    this.targetShrooms.ReservedForTask = false;
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.GoingTo)
      return;
    this.progress += deltaGameTime;
    if ((double) this.progress <= 15.0)
      return;
    this.End();
    this.progress = 0.0f;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.targetShrooms != null)
      return this.targetShrooms.Data.Position;
    Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.0f, -1f));
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
    return (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * UnityEngine.Random.Range(2f, 4f));
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this._follower = follower;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance-mushroom");
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Audio/sozo_evil") || (double) UnityEngine.Random.value >= 0.20000000298023224)
      return;
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_sozo_evil", this._follower.gameObject);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this.targetShrooms != null)
    {
      follower.TimedAnimation("Food/feast-eat", 5f, (System.Action) (() =>
      {
        follower.Brain.AddThought((Thought) UnityEngine.Random.Range(377, 382));
        follower.Brain._directInfoAccess.SozoBrainshed = true;
        follower.Brain.Stats.Satiation += (float) CookingData.GetSatationAmount(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL);
        this.targetShrooms.PickBerries(float.MaxValue, false);
        this.ClearDestination();
        this.OnStart();
      }));
    }
    else
    {
      if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
        return;
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance-mushroom");
    }
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    if (this.targetShrooms == null || PlayerFarming.Location == FollowerLocation.Base)
      return;
    simFollower.Brain.AddThought((Thought) UnityEngine.Random.Range(377, 382));
    simFollower.Brain._directInfoAccess.SozoBrainshed = true;
    simFollower.Brain.Stats.Satiation += (float) CookingData.GetSatationAmount(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL);
    this.targetShrooms.PickBerries(float.MaxValue, false);
    this.ClearDestination();
    this.OnStart();
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    if (this.targetShrooms != null)
      this.targetShrooms.ReservedForTask = false;
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public Structures_BerryBush GetNextShroomCrop()
  {
    Structures_BerryBush nextShroomCrop = (Structures_BerryBush) null;
    float num1 = float.MaxValue;
    StructureManager.TryGetAllUnpickedPlots(ref this.cachedBerryBushes, FollowerLocation.Base);
    foreach (Structures_BerryBush cachedBerryBush in this.cachedBerryBushes)
    {
      if (cachedBerryBush.Data.Type == StructureBrain.TYPES.MUSHROOM_BUSH)
      {
        if (PlayerFarming.Location == FollowerLocation.Base)
        {
          bool flag1 = false;
          foreach (FarmPlot farmPlot in FarmPlot.FarmPlots)
          {
            if ((UnityEngine.Object) farmPlot._activeCropController != (UnityEngine.Object) null && farmPlot.StructureBrain.Data.ID == cachedBerryBush.CropID && (!farmPlot.StructureBrain.HasFertilized() || farmPlot.StructureBrain.GetFertilizer().type != 144 /*0x90*/) && farmPlot.StructureBrain.GetPlantedSeed().type != 160 /*0xA0*/)
            {
              bool flag2 = false;
              foreach (GameObject cropState in farmPlot._activeCropController.CropStates)
              {
                if (cropState.activeSelf)
                  flag2 = true;
              }
              if (farmPlot._activeCropController.BumperCropObject.activeSelf)
                flag2 = true;
              if (flag2)
              {
                flag1 = true;
                break;
              }
            }
          }
          if (!flag1)
            continue;
        }
        float num2 = Vector3.Distance(this._brain.LastPosition, cachedBerryBush.Data.Position);
        Vector3 position = cachedBerryBush.Data.Position;
        if ((double) num2 < (double) num1)
        {
          nextShroomCrop = cachedBerryBush;
          num1 = num2;
        }
      }
    }
    this.cachedBerryBushes.Clear();
    return nextShroomCrop;
  }
}
