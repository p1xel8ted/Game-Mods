// Decompiled with JetBrains decompiler
// Type: FollowerTask_PetAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_PetAnimal : FollowerTask
{
  public const float SPAWN_EGG_CHANCE = 0.0f;
  public StructuresData.Ranchable_Animal animal;

  public override FollowerTaskType Type => FollowerTaskType.PetAnimal;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public FollowerTask_PetAnimal() => this.animal = this.GetTargetAnimal();

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    if (this.CanInteractWithAnimal(this.animal))
    {
      this.animal.State = Interaction_Ranchable.State.Animating;
      foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
      {
        if (ranchable.Animal == this.animal)
        {
          ranchable.ClearCurrentPath();
          break;
        }
      }
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public override void OnArrive()
  {
    if (LocationManager.GetLocationState(this.Location) == LocationState.Active)
      base.OnArrive();
    else
      this.End();
  }

  public override void OnComplete()
  {
    base.OnComplete();
    if (this.animal == null)
      return;
    this.animal.State = Interaction_Ranchable.State.Default;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (LocationManager.GetLocationState(this.Location) != LocationState.Active)
      return this.animal.LastPosition;
    Interaction_Ranchable animalInteraction = this.GetAnimalInteraction(this.animal.ID);
    return (UnityEngine.Object) animalInteraction != (UnityEngine.Object) null ? animalInteraction.transform.position : this.animal.LastPosition;
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (!this.CanInteractWithAnimal(this.animal, true))
      this.End();
    else
      this.animal.State = Interaction_Ranchable.State.Animating;
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    if (!this.CanInteractWithAnimal(this.animal, true))
    {
      this.End();
    }
    else
    {
      Interaction_Ranchable animalInteraction = this.GetAnimalInteraction(this.animal.ID);
      follower.FacePosition(animalInteraction.transform.position);
      animalInteraction.CurrentState = Interaction_Ranchable.State.Animating;
      animalInteraction.PlayPetVO();
      if (this._brain.HasTrait(FollowerTrait.TraitType.AnimalLover))
      {
        follower.TimedAnimation("animal-loving", 3.5f, (System.Action) (() =>
        {
          animalInteraction.AddAdoration(50f);
          this.End();
        }));
        DOVirtual.DelayedCall(1.5f, (TweenCallback) (() => BiomeConstants.Instance.EmitHeartPickUpVFX(this.Brain.LastPosition, 0.0f, "red", "burst_small")));
      }
      else
        follower.TimedAnimationWithDuration("Egg/egg-tending", (System.Action) (() => this.End()), false);
    }
  }

  public StructuresData.Ranchable_Animal GetTargetAnimal()
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.RANCH, StructureBrain.TYPES.RANCH_2);
    List<StructuresData.Ranchable_Animal> ts = new List<StructuresData.Ranchable_Animal>();
    for (int index = 0; index < structuresOfTypes.Count; ++index)
      ts.AddRange((IEnumerable<StructuresData.Ranchable_Animal>) structuresOfTypes[index].Data.Animals);
    ts.Shuffle<StructuresData.Ranchable_Animal>();
    foreach (StructuresData.Ranchable_Animal targetAnimal in ts)
    {
      if (this.CanInteractWithAnimal(targetAnimal))
        return targetAnimal;
    }
    return (StructuresData.Ranchable_Animal) null;
  }

  public Interaction_Ranchable GetAnimalInteraction(int animalID)
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if (ranchable.Animal.ID == animalID)
        return ranchable;
    }
    return (Interaction_Ranchable) null;
  }

  public StructuresData.EggData GetEggData()
  {
    List<FollowerTrait.TraitType> traitTypeList1 = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) this.Brain.Info.Traits);
    List<FollowerTrait.TraitType> traitTypeList2 = new List<FollowerTrait.TraitType>();
    for (int index = traitTypeList1.Count - 1; index >= 0; --index)
    {
      if (FollowerTrait.ExcludedFromMating.Contains(traitTypeList1[index]))
        traitTypeList1.RemoveAt(index);
    }
    if (traitTypeList1.Count > 0)
    {
      FollowerTrait.TraitType traitType1 = traitTypeList1[UnityEngine.Random.Range(0, traitTypeList1.Count)];
      traitTypeList2.Add(traitType1);
      traitTypeList1.Remove(traitType1);
      if (traitTypeList1.Count > 0)
      {
        FollowerTrait.TraitType traitType2 = traitTypeList1[UnityEngine.Random.Range(0, traitTypeList1.Count)];
        traitTypeList2.Add(traitType2);
        traitTypeList1.Remove(traitType2);
      }
    }
    StructuresData.EggData eggData = new StructuresData.EggData();
    eggData.Parent_1_ID = this.Brain.Info.ID;
    eggData.Parent_2_ID = this.Brain.Info.ID;
    eggData.Parent_1_SkinColor = this.Brain.Info.SkinColour;
    eggData.Parent_2_SkinColor = this.Brain.Info.SkinColour;
    eggData.Parent_1_SkinVariant = this.Brain.Info.SkinVariation;
    eggData.Parent_2_SkinVariant = this.Brain.Info.SkinVariation;
    eggData.Parent_1_SkinName = this.Brain.Info.SkinName;
    eggData.Parent_2_SkinName = this.Brain.Info.SkinName;
    eggData.Parent1Name = this.Brain.Info.Name;
    eggData.Parent2Name = "{...}";
    eggData.Traits = traitTypeList2;
    eggData.EggSeed = UnityEngine.Random.Range(1, int.MaxValue);
    if (eggData.Traits.Contains(FollowerTrait.TraitType.Mutated))
      eggData.Rotting = true;
    return eggData;
  }

  public bool CanInteractWithAnimal(
    StructuresData.Ranchable_Animal targetAnimal,
    bool excludeAnimatingState = false)
  {
    if (targetAnimal == null)
      return false;
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      Interaction_Ranchable animalInteraction = this.GetAnimalInteraction(targetAnimal.ID);
      if ((UnityEngine.Object) animalInteraction == (UnityEngine.Object) null || animalInteraction.ReservedByPlayer)
        return false;
    }
    return !targetAnimal.IsPlayersAnimal() && (targetAnimal.State == Interaction_Ranchable.State.Default || excludeAnimatingState || targetAnimal.State != Interaction_Ranchable.State.Animating);
  }
}
