// Decompiled with JetBrains decompiler
// Type: FollowerTask_Rancher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Rancher : FollowerTask
{
  public Structures_Ranch ranch;
  public int ranchID = -1;
  public int targetAnimalID = -1;
  public StructuresData.Ranchable_Animal targetAnimal;
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.Rancher;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public FollowerTask_Rancher(int structureID)
  {
    this.ranchID = structureID;
    this.ranch = StructureManager.GetStructureByID<Structures_Ranch>(structureID);
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    this.ranch.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    this.ranch.ReservedForTask = false;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Rancher ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public override void OnStart()
  {
    base.OnStart();
    if (this.ranch == null)
      this.TryRestoreRanch();
    if (this.ranch == null)
      this.End();
    else
      this.SetState(FollowerTaskState.GoingTo);
  }

  public override int GetSubTaskCode() => this.ranch == null ? this.ranchID : this.ranch.Data.ID;

  public override void OnArrive()
  {
    base.OnArrive();
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        followerById.SetHat(FollowerHatType.Ranch);
    }
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      this.follower = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    bool retargetAnimal = true;
    if (this.targetAnimalID != -1)
    {
      Interaction_Ranchable animal = this.GetAnimal(this.targetAnimalID);
      StructuresData.Ranchable_Animal a = this.ranch.GetAnimal(this.targetAnimalID);
      if (a == null || !a.IsAvailableForFollowerTask())
      {
        this.targetAnimalID = -1;
        retargetAnimal = true;
      }
      else if (a != null && a.Ailment == Interaction_Ranchable.Ailment.Stinky)
      {
        if ((UnityEngine.Object) animal != (UnityEngine.Object) null && !animal.ReservedByPlayer)
        {
          if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Base)
          {
            this.targetAnimal = a;
            animal.ClearCurrentPath();
            animal.CurrentState = Interaction_Ranchable.State.Animating;
            AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/clean_follower", this.follower.transform.position);
            this.follower.TimedAnimation("action", 3.4f, (System.Action) (() =>
            {
              if (!animal.ReservedByPlayer)
              {
                animal.CurrentState = Interaction_Ranchable.State.Default;
                animal.Clean(false);
              }
              else
                retargetAnimal = false;
              this.targetAnimalID = this.RetargetAnimal();
              this.ClearDestination();
              this.SetState(FollowerTaskState.GoingTo);
            }));
            this.targetAnimalID = -1;
            retargetAnimal = false;
            return;
          }
          animal.CurrentState = Interaction_Ranchable.State.Default;
          animal.Clean(false);
          this.targetAnimalID = this.RetargetAnimal();
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
        {
          if ((UnityEngine.Object) animal != (UnityEngine.Object) null && animal.ReservedByPlayer)
          {
            this.targetAnimalID = this.RetargetAnimal();
          }
          else
          {
            if (a.State == Interaction_Ranchable.State.Animating)
              a.State = Interaction_Ranchable.State.Default;
            a.Ailment = Interaction_Ranchable.Ailment.None;
            string str = InventoryItem.LocalizedName(a.Type);
            if (!string.IsNullOrEmpty(a.GivenName))
              str = a.GivenName;
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalNotStinky", str);
            a.Adoration += 20f;
            if ((double) a.Adoration >= 100.0)
            {
              ++a.Level;
              a.Adoration = 0.0f;
            }
            this.targetAnimalID = this.RetargetAnimal();
          }
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      else if (a != null && !a.WorkedToday)
      {
        if ((UnityEngine.Object) animal != (UnityEngine.Object) null && !animal.ReservedByPlayer)
        {
          if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Base)
          {
            this.targetAnimal = a;
            animal.ClearCurrentPath();
            animal.CurrentState = Interaction_Ranchable.State.Animating;
            List<InventoryItem> workLoot = Interaction_Ranchable.GetWorkLoot(animal.Animal);
            string suffix = "wool";
            if (workLoot[0].type == 1)
              suffix = "log";
            else if (workLoot[0].type == 89)
              suffix = "crystal";
            else if (workLoot[0].type == 90)
              suffix = "web";
            else if (InventoryItem.AllSeeds.Contains((InventoryItem.ITEM_TYPE) workLoot[0].type))
              suffix = "seed";
            else if (workLoot[0].type == 197)
              suffix = "milk";
            else if (workLoot[0].type == 172)
              suffix = "rotstone";
            AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/shear_follower", this.follower.transform.position);
            this.follower.TimedAnimation("Ranching/collect-" + suffix, 3.4f, (System.Action) (() =>
            {
              if (!a.WorkedToday && !animal.ReservedByPlayer)
              {
                string str = "Ranching/run-";
                if (workLoot[0].type == 1)
                {
                  str = "Buildings/run-";
                  suffix = "wood";
                }
                else if (workLoot[0].type == 172)
                {
                  str = "Buildings/run-";
                  suffix = "rotstone";
                }
                else if (InventoryItem.AllSeeds.Contains((InventoryItem.ITEM_TYPE) workLoot[0].type))
                  str = "Farming/run-";
                animal.CurrentState = Interaction_Ranchable.State.Default;
                this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, str + suffix);
                animal.Work(false);
                this.Brain._directInfoAccess.Inventory.AddRange((IEnumerable<InventoryItem>) workLoot);
              }
              else if (animal.ReservedByPlayer)
              {
                this.targetAnimalID = -1;
                retargetAnimal = false;
              }
              else
              {
                this.targetAnimalID = -1;
                retargetAnimal = false;
                a.State = Interaction_Ranchable.State.Default;
              }
              this.ClearDestination();
              this.SetState(FollowerTaskState.GoingTo);
            }));
            this.targetAnimalID = -1;
            retargetAnimal = false;
            return;
          }
          animal.Work(false);
          animal.CurrentState = Interaction_Ranchable.State.Default;
          this.Brain._directInfoAccess.Inventory.AddRange((IEnumerable<InventoryItem>) Interaction_Ranchable.GetWorkLoot(animal.Animal));
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
        {
          if ((UnityEngine.Object) animal != (UnityEngine.Object) null && animal.ReservedByPlayer)
          {
            this.targetAnimalID = -1;
            retargetAnimal = false;
          }
          else if (a != null)
          {
            a.WorkedToday = true;
            this.Brain._directInfoAccess.Inventory.AddRange((IEnumerable<InventoryItem>) Interaction_Ranchable.GetWorkLoot(a));
            if (a.State == Interaction_Ranchable.State.Animating)
              a.State = Interaction_Ranchable.State.Default;
          }
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      else if (a != null && !a.MilkedToday && a.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      {
        if ((UnityEngine.Object) animal != (UnityEngine.Object) null && !animal.ReservedByPlayer)
        {
          if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Base)
          {
            this.targetAnimal = a;
            animal.ClearCurrentPath();
            animal.CurrentState = Interaction_Ranchable.State.Animating;
            AudioManager.Instance.PlayOneShot("event:/dlc/animal/cow/milk_follower", animal.transform.position);
            this.follower.TimedAnimation("Ranching/collect-milk", 3.4f, (System.Action) (() =>
            {
              if (!a.MilkedToday && !animal.ReservedByPlayer)
              {
                animal.CurrentState = Interaction_Ranchable.State.Default;
                this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Ranching/run-milk");
                animal.MilkAnimal(false);
                this.Brain._directInfoAccess.Inventory.AddRange((IEnumerable<InventoryItem>) new List<InventoryItem>()
                {
                  new InventoryItem(InventoryItem.ITEM_TYPE.MILK)
                });
              }
              else if (animal.ReservedByPlayer)
              {
                this.targetAnimalID = -1;
                retargetAnimal = false;
              }
              else
              {
                this.targetAnimalID = -1;
                retargetAnimal = false;
                a.State = Interaction_Ranchable.State.Default;
              }
              this.ClearDestination();
              this.SetState(FollowerTaskState.GoingTo);
            }));
            this.targetAnimalID = -1;
            retargetAnimal = false;
            return;
          }
          animal.MilkAnimal(false);
          animal.CurrentState = Interaction_Ranchable.State.Default;
          this.Brain._directInfoAccess.Inventory.AddRange((IEnumerable<InventoryItem>) new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MILK)
          });
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
        {
          if ((UnityEngine.Object) animal != (UnityEngine.Object) null && animal.ReservedByPlayer)
          {
            this.targetAnimalID = -1;
            retargetAnimal = false;
          }
          else if (a != null)
          {
            a.MilkedToday = true;
            this.Brain._directInfoAccess.Inventory.AddRange((IEnumerable<InventoryItem>) new List<InventoryItem>()
            {
              new InventoryItem(InventoryItem.ITEM_TYPE.MILK)
            });
            if (a.State == Interaction_Ranchable.State.Animating)
              a.State = Interaction_Ranchable.State.Default;
          }
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      this.targetAnimalID = -1;
      retargetAnimal = false;
    }
    else if (this.Brain._directInfoAccess.Inventory.Count > 0 && this.ranch != null)
    {
      if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Base)
      {
        string animation = "Ranching/add-wool";
        if (this._brain._directInfoAccess.Inventory[0].type == 1)
          animation = "Buildings/add-wood";
        else if (this._brain._directInfoAccess.Inventory[0].type == 172)
          animation = "Buildings/add-rotstone";
        else if (this._brain._directInfoAccess.Inventory[0].type == 89)
          animation = "Ranching/add-crystal";
        else if (this._brain._directInfoAccess.Inventory[0].type == 90)
          animation = "Ranching/add-web";
        else if (this._brain._directInfoAccess.Inventory[0].type == 197)
          animation = "Ranching/add-milk";
        else if (InventoryItem.AllSeeds.Contains((InventoryItem.ITEM_TYPE) this._brain._directInfoAccess.Inventory[0].type))
          animation = "Farming/add-seed";
        this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
        this.follower.TimedAnimation(animation, 1.56666672f, (System.Action) (() =>
        {
          if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Base)
            AudioManager.Instance.PlayOneShot("event:/cooking/add_wood", this.follower.transform.position);
          foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
          {
            for (int index = 0; index < inventoryItem.quantity; ++index)
              this.ranch.DepositItemUnstacked((InventoryItem.ITEM_TYPE) inventoryItem.type);
          }
          this._brain._directInfoAccess.Inventory.Clear();
          this.ClearDestination();
          if (retargetAnimal)
          {
            this.targetAnimalID = this.RetargetAnimal();
            if (this.targetAnimalID == -1)
            {
              this.End();
              return;
            }
          }
          this.SetState(FollowerTaskState.GoingTo);
        }));
        return;
      }
      foreach (InventoryItem inventoryItem in this._brain._directInfoAccess.Inventory)
        this.ranch.DepositItem((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity);
      this._brain._directInfoAccess.Inventory.Clear();
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    if (retargetAnimal)
    {
      this.targetAnimalID = this.RetargetAnimal();
      if (this.targetAnimalID == -1)
      {
        this.End();
        return;
      }
    }
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public int RetargetAnimal()
  {
    int animalID = -1;
    List<StructuresData.Ranchable_Animal> ranchableAnimalList1 = new List<StructuresData.Ranchable_Animal>();
    List<StructuresData.Ranchable_Animal> ranchableAnimalList2 = new List<StructuresData.Ranchable_Animal>();
    foreach (StructuresData.Ranchable_Animal animal1 in this.ranch.Data.Animals)
    {
      if (animal1.IsAvailableForFollowerTask())
      {
        Interaction_Ranchable animal2 = this.GetAnimal(animal1.ID);
        if ((UnityEngine.Object) animal2 == (UnityEngine.Object) null || !animal2.ReservedByPlayer)
        {
          if (animal1.Ailment == Interaction_Ranchable.Ailment.Stinky)
            ranchableAnimalList1.Add(animal1);
          else
            ranchableAnimalList2.Add(animal1);
        }
      }
    }
    if (ranchableAnimalList1.Count > 0)
      animalID = ranchableAnimalList1[0].ID;
    else if (ranchableAnimalList2.Count > 0)
      animalID = ranchableAnimalList2[0].ID;
    if (animalID != -1)
      this.GetAnimal(animalID)?.ClearCurrentPath();
    return animalID;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower.ResetStateAnimations();
    follower.SetHat(FollowerHatType.None);
  }

  public void TryRestoreRanch()
  {
    if (this.ranchID != -1)
    {
      this.ranch = StructureManager.GetStructureByID<Structures_Ranch>(this.ranchID);
      if (this.ranch != null)
        return;
    }
    foreach (Structures_Ranch structuresRanch in StructureManager.GetAllStructuresOfType<Structures_Ranch>())
    {
      if (structuresRanch != null && structuresRanch.Data != null)
      {
        this.ranch = structuresRanch;
        this.ranchID = structuresRanch.Data.ID;
        break;
      }
    }
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.ranch == null)
      this.TryRestoreRanch();
    if (this.ranch == null)
      this.End();
    if (this.State != FollowerTaskState.Doing || PlayerFarming.Location == FollowerLocation.Base || this.ranch == null || this.targetAnimal == null || this.targetAnimal.State != Interaction_Ranchable.State.Animating)
      return;
    this.targetAnimal.State = Interaction_Ranchable.State.Default;
    this.targetAnimalID = this.RetargetAnimal();
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.targetAnimalID == -1 || this.Brain._directInfoAccess.Inventory.Count > 0)
      return this.ranch == null ? Vector3.zero : this.ranch.Data.Position;
    if (this.ranch != null)
    {
      Interaction_Ranchable animal = this.GetAnimal(this.targetAnimalID);
      if ((UnityEngine.Object) animal != (UnityEngine.Object) null)
        return animal.transform.position;
    }
    return Vector3.zero;
  }

  public Interaction_Ranchable GetAnimal(int animalID)
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if (ranchable.Animal.ID == animalID)
        return ranchable;
    }
    return (Interaction_Ranchable) null;
  }
}
