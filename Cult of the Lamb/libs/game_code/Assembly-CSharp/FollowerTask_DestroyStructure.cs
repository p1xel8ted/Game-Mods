// Decompiled with JetBrains decompiler
// Type: FollowerTask_DestroyStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_DestroyStructure : FollowerTask
{
  public StructureBrain _buildSite;
  public Structure structure;
  public float progress;
  public const float TIME_UNTIL_DESTROYED = 180f;
  public static List<FollowerBrain> FollowersDestroyingStructure = new List<FollowerBrain>();
  public static float timeSinceLastShake = 0.0f;

  public override FollowerTaskType Type => FollowerTaskType.DestroyStructure;

  public override FollowerLocation Location => this._buildSite.Data.Location;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int UsingStructureID => this._buildSite.Data.ID;

  public FollowerTask_DestroyStructure(int structureID)
  {
    this._buildSite = StructureManager.GetStructureByID<StructureBrain>(structureID);
    this.structure = this.GetStructure();
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    if (FollowerTask_DestroyStructure.FollowersDestroyingStructure.Contains(this.Brain))
      return;
    FollowerTask_DestroyStructure.FollowersDestroyingStructure.Add(this.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain != this._brain && allBrain.CurrentTaskType == FollowerTaskType.DestroyStructure)
        allBrain.CurrentTask.RecalculateDestination();
    }
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    if (!FollowerTask_DestroyStructure.FollowersDestroyingStructure.Contains(this.Brain))
      return;
    FollowerTask_DestroyStructure.FollowersDestroyingStructure.Remove(this.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain != this._brain && allBrain.CurrentTaskType == FollowerTaskType.DestroyStructure)
        allBrain.CurrentTask.RecalculateDestination();
    }
  }

  public override int GetSubTaskCode() => this._buildSite.Data.ID;

  public override void OnStart()
  {
    if (this._buildSite != null)
    {
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
    {
      this.progress += deltaGameTime;
      if (!this._buildSite.Data.IsCollapsed && ((double) this.progress >= 180.0 || PlayerFarming.Location != FollowerLocation.Base && PlayerFarming.Location != FollowerLocation.Church))
      {
        this._buildSite.Collapse();
        FollowerTask_DestroyStructure.FollowersDestroyingStructure.Clear();
      }
    }
    if (this._buildSite == null || this._buildSite.Data == null || !this._buildSite.Data.IsCollapsed && !this._buildSite.Data.IsSnowedUnder && StructureManager.IsCollapsible(this._buildSite.Data.Type))
      return;
    this.End();
    FollowerTask_DestroyStructure.FollowersDestroyingStructure.Clear();
  }

  public override void OnComplete()
  {
    base.OnComplete();
    if (this._buildSite == null || !this._buildSite.Data.IsCollapsed)
      return;
    this.Brain.AddPleasure(FollowerBrain.PleasureActions.DestroyStructure);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.GetCirclePosition(follower.Brain, this._buildSite.Data.Position + Vector3.up * ((float) this._buildSite.Data.Bounds.y / 2f), (float) this._buildSite.Data.Bounds.x / 2f, FollowerTask_DestroyStructure.FollowersDestroyingStructure);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Build") || (double) Time.time <= (double) FollowerTask_DestroyStructure.timeSinceLastShake)
      return;
    if ((Object) this.structure == (Object) null)
      this.structure = this.GetStructure();
    this.structure.transform.DOKill();
    this.structure.transform.localScale = new Vector3((float) this.structure.Brain.Data.Direction, 1f, 1f);
    this.structure.transform.rotation = Quaternion.identity;
    this.structure.transform.DOPunchRotation((Vector3) (Random.insideUnitCircle * (float) Random.Range(2, 10)), 1f).SetEase<Tweener>(Ease.InOutBounce);
    this.structure.transform.DOPunchScale(this.structure.transform.localScale * Random.Range(0.025f, 0.05f), 0.25f).SetEase<Tweener>(Ease.InOutBounce);
    FollowerTask_DestroyStructure.timeSinceLastShake = Time.time + 1f;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this._buildSite.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    switch (Random.Range(0, 3))
    {
      case 0:
        double num1 = (double) follower.SetBodyAnimation("Riot/riot-destroy-structure", true);
        break;
      case 1:
        double num2 = (double) follower.SetBodyAnimation("Riot/riot-destroy-structure2", true);
        break;
      default:
        double num3 = (double) follower.SetBodyAnimation("Riot/riot-destroy-structure3", true);
        break;
    }
  }

  public Structure GetStructure()
  {
    foreach (Structure structure in Structure.Structures)
    {
      if ((Object) structure != (Object) null && structure.Brain != null && structure.Brain.Data != null && this._buildSite != null && this._buildSite.Data != null && structure.Brain.Data.ID == this._buildSite.Data.ID)
        return structure;
    }
    return (Structure) null;
  }
}
