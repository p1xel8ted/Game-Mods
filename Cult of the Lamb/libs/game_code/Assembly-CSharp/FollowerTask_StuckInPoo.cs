// Decompiled with JetBrains decompiler
// Type: FollowerTask_StuckInPoo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_StuckInPoo : FollowerTask
{
  public StructureBrain structure;

  public override FollowerTaskType Type => FollowerTaskType.StuckInPoo;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public FollowerTask_StuckInPoo(StructureBrain structure) => this.structure = structure;

  public override int GetSubTaskCode() => this.structure.Data.ID;

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    this.structure.Data.FollowerID = this.Brain.Info.ID;
    this.structure.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    if (this.structure.Data.FollowerID != this.Brain.Info.ID)
      return;
    this.structure.ReservedForTask = false;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.GoingTo)
      this.Arrive();
    this.Brain.LastPosition = this.structure.Data.Position;
    if (this.structure == null || this.structure.Data.Destroyed)
    {
      this.End();
    }
    else
    {
      if (this.structure.Data.FollowerID == this.Brain.Info.ID)
        return;
      this.Abort();
    }
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this.GetStuckInPoopAnim());
    follower.transform.position = this.structure.Data.Position;
    this.structure.Data.FollowerID = this._brain.Info.ID;
    follower.Interaction_FollowerInteraction.Interactable = false;
    follower.transform.parent = this.GetPoopStructure().transform;
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.transform.localScale = Vector3.one;
    follower.transform.parent = BaseLocationManager.Instance.UnitLayer;
    Spine.Animation animation = follower.Spine.Skeleton.Data.FindAnimation(this.GetFreeAnim());
    follower.TimedAnimation(animation.Name, animation.Duration, (System.Action) (() => this.\u003C\u003En__0(follower)));
  }

  public override Vector3 UpdateDestination(Follower follower) => this.structure.Data.Position;

  public Structure GetPoopStructure()
  {
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && structure.Brain.Data.ID == this.structure.Data.ID)
        return structure;
    }
    return (Structure) null;
  }

  public string GetStuckInPoopAnim()
  {
    return this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac) ? "Poop/stuck-in-poop-coprophiliac" : "Poop/stuck-in-poop";
  }

  public string GetFreeAnim()
  {
    if (this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
      return "Poop/free-from-poop-coprophiliac" + ((double) UnityEngine.Random.value > 0.5 ? (object) "" : (object) UnityEngine.Random.Range(2, 4))?.ToString();
    return this._brain.HasTrait(FollowerTrait.TraitType.Germophobe) ? "Poop/free-from-poop" : "Poop/free-from-poop3";
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(Follower follower) => base.OnFinaliseBegin(follower);
}
