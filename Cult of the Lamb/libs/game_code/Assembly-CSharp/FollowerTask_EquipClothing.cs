// Decompiled with JetBrains decompiler
// Type: FollowerTask_EquipClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using UnityEngine;

#nullable disable
public class FollowerTask_EquipClothing : FollowerTask
{
  public int _tailorID;
  public Structures_Tailor _tailor;
  public StructuresData.ClothingStruct _outfit;

  public override FollowerTaskType Type => FollowerTaskType.EquipClothing;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.OverrideWorkPriority;
  }

  public FollowerTask_EquipClothing(int tailorID, StructuresData.ClothingStruct outfit)
  {
    this._tailorID = tailorID;
    this._tailor = StructureManager.GetStructureByID<Structures_Tailor>(this._tailorID);
    this._outfit = outfit;
  }

  public override int GetSubTaskCode() => this._tailorID;

  public override void ClaimReservations()
  {
    if (this._tailor == null)
      return;
    this._tailor.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this._tailor == null)
      return;
    this._tailor.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
  }

  public void Loop() => this.End();

  public Structure GetStructure(int ID)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && structure.Brain.Data.ID == ID)
        return structure;
    }
    return (Structure) null;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Interaction_Tailor tailor = this.FindTailor();
    if ((UnityEngine.Object) tailor != (UnityEngine.Object) null)
      return tailor.transform.position + Vector3.back + (Vector3) (UnityEngine.Random.insideUnitCircle / 2f);
    this.End();
    return Vector3.zero;
  }

  public override void OnArrive()
  {
    base.OnArrive();
    Follower follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    Structures_Tailor tailor;
    if (StructureBrain.TryFindBrainByID<Structures_Tailor>(in this._tailorID, out tailor) && this._brain.Info.Clothing != FollowerClothingType.None)
      tailor.Data.AllClothing.Add(new StructuresData.ClothingStruct(this._brain.Info.Clothing, this._brain.Info.ClothingVariant));
    this._brain.AssignClothing(this._outfit.ClothingType, this._outfit.Variant);
    if ((bool) (UnityEngine.Object) follower)
    {
      follower.TimedAnimation("action", 1.5f, (System.Action) (() =>
      {
        FollowerBrain.SetFollowerCostume(follower.Spine.skeleton, this._brain._directInfoAccess, forceUpdate: true);
        follower.TimedAnimation("Reactions/react-admire1", 2f, (System.Action) (() =>
        {
          tailor?.Data.ReservedClothing.Remove(this._outfit);
          this.End();
        }));
      }));
    }
    else
    {
      tailor?.Data.ReservedClothing.Remove(this._outfit);
      this.End();
    }
  }

  public override void OnAbort()
  {
    base.OnAbort();
    Structures_Tailor result;
    if (this._outfit.ClothingType == FollowerClothingType.None || this._brain.Info.Clothing == this._outfit.ClothingType || !StructureBrain.TryFindBrainByID<Structures_Tailor>(in this._tailorID, out result))
      return;
    result.Data.AllClothing.Add(this._outfit);
    result.Data.ReservedClothing.Remove(this._outfit);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    Structures_Tailor result;
    if (this._outfit.ClothingType == FollowerClothingType.None || this._brain.Info.Clothing == this._outfit.ClothingType || !StructureBrain.TryFindBrainByID<Structures_Tailor>(in this._tailorID, out result))
      return;
    result.Data.AllClothing.Add(this._outfit);
    result.Data.ReservedClothing.Remove(this._outfit);
  }

  public override void Cleanup(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    follower.SetOutfit(FollowerOutfitType.Follower, false);
    follower.Interaction_FollowerInteraction.enabled = true;
  }

  public Interaction_Tailor FindTailor() => Interaction_Tailor.Instance;

  public override void TaskTick(float deltaGameTime)
  {
  }
}
