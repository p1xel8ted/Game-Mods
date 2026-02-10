// Decompiled with JetBrains decompiler
// Type: FollowerTask_CollectDevotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_CollectDevotion : FollowerTask
{
  public int _shrineID = -1;
  public int _targetID = -1;
  public bool carryingDevotion;
  public int holdingDevotion;
  public Structures_Shrine_Disciple_Collection _shrine;

  public override FollowerTaskType Type => FollowerTaskType.CollectDevotion;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override float Priorty => 0.0f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return !brain.Info.IsDisciple ? PriorityCategory.Ignore : PriorityCategory.OverrideWorkPriority;
  }

  public FollowerTask_CollectDevotion(int shrineID)
  {
    this._shrineID = shrineID;
    this._shrine = StructureManager.GetStructureByID<Structures_Shrine_Disciple_Collection>(this._shrineID);
  }

  public override int GetSubTaskCode() => this._shrineID;

  public override void ClaimReservations()
  {
    if (this._shrine != null)
      this._shrine.ReservedForTask = true;
    if (this._targetID == -1)
      return;
    StructureBrain target = this.FindTarget();
    if (target == null)
      return;
    target.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this._shrine != null)
      this._shrine.ReservedForTask = false;
    if (this._targetID == -1)
      return;
    StructureBrain target = this.FindTarget();
    if (target == null)
      return;
    target.ReservedForTask = false;
  }

  public override void OnStart()
  {
    StructureBrain nextStructure = this._shrine.GetNextStructure();
    if (nextStructure != null)
    {
      this._targetID = nextStructure.Data.ID;
      nextStructure.ReservedForTask = true;
    }
    else
      this._targetID = -1;
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (!this.carryingDevotion)
      return;
    this.RefundDevotion();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!this.carryingDevotion)
      return;
    this.RefundDevotion();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.carryingDevotion || this._targetID == -1)
      return this._shrine.Data.Position;
    StructureBrain target = this.FindTarget();
    if (target != null)
      return target.Data.Position + Vector3.back;
    this.End();
    return Vector3.zero;
  }

  public void RefundDevotion()
  {
    if (this._targetID == -1 || this.holdingDevotion <= 0)
      return;
    StructureBrain target = this.FindTarget();
    if (target != null)
    {
      target.Data.SoulCount += this.holdingDevotion;
      this.holdingDevotion = 0;
      Action<int> onSoulsGained = target.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.holdingDevotion);
    }
    else
    {
      this._shrine.Data.SoulCount += this.holdingDevotion;
      this.holdingDevotion = 0;
      Action<int> onSoulsGained = this._shrine.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.holdingDevotion);
    }
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    if (!this.carryingDevotion && this._targetID == -1)
    {
      StructureBrain nextStructure = this._shrine.GetNextStructure();
      this._targetID = nextStructure != null ? nextStructure.Data.ID : -1;
      if (this._targetID == -1)
      {
        this.carryingDevotion = false;
        this.End();
      }
      else
      {
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
    else
    {
      StructureBrain target = this.FindTarget();
      if (this.carryingDevotion)
      {
        if (this._shrine == null)
        {
          this.End();
        }
        else
        {
          this._shrine.Data.SoulCount += this.holdingDevotion;
          this.holdingDevotion = 0;
          this._targetID = -1;
          this.carryingDevotion = false;
          Action<int> onSoulsGained = this._shrine.OnSoulsGained;
          if (onSoulsGained != null)
            onSoulsGained(this.holdingDevotion);
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
      }
      else if (target == null)
      {
        this.End();
      }
      else
      {
        int num = Mathf.Min(target.Data.SoulCount, this._shrine.SoulMax - this._shrine.SoulCount);
        this.holdingDevotion = num;
        target.Data.SoulCount -= num;
        this.carryingDevotion = true;
        Action<int> onSoulsGained = target.OnSoulsGained;
        if (onSoulsGained != null)
          onSoulsGained(num);
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (!this.carryingDevotion && this._targetID == -1)
    {
      StructureBrain nextStructure = this._shrine.GetNextStructure();
      this._targetID = nextStructure != null ? nextStructure.Data.ID : -1;
      if (this._targetID == -1)
      {
        this.End();
      }
      else
      {
        nextStructure.ReservedForTask = true;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
    else
    {
      StructureBrain target = this.FindTarget();
      Vector3 vector3 = this.carryingDevotion || target == null ? this._shrine.Data.Position : target.Data.Position;
      follower.FacePosition(vector3);
      if (this.carryingDevotion)
      {
        if (this._shrine == null)
        {
          this.End();
        }
        else
        {
          follower.StartCoroutine((IEnumerator) this.CollectDevotion(this.FindShrineStructure().gameObject, follower.transform.position, this.holdingDevotion, (StructureBrain) this._shrine));
          this.holdingDevotion = 0;
          this._targetID = -1;
          this.carryingDevotion = false;
          Action<int> onSoulsGained = this._shrine.OnSoulsGained;
          if (onSoulsGained != null)
            onSoulsGained(this.holdingDevotion);
          follower.TimedAnimation("devotion/devotion-collect", 2f, (System.Action) (() =>
          {
            this.ClearDestination();
            this.SetState(FollowerTaskState.GoingTo);
          }));
        }
      }
      else if (target == null)
      {
        this.End();
      }
      else
      {
        int amount = Mathf.Min(target.Data.SoulCount, this._shrine.SoulMax - this._shrine.SoulCount);
        follower.StartCoroutine((IEnumerator) this.CollectDevotion(follower.gameObject, vector3, amount, (StructureBrain) null));
        this.holdingDevotion = amount;
        target.Data.SoulCount -= amount;
        this.carryingDevotion = true;
        Action<int> onSoulsGained = target.OnSoulsGained;
        if (onSoulsGained != null)
          onSoulsGained(amount);
        target.ReservedForTask = false;
        follower.TimedAnimation("devotion/devotion-collect", 2f, (System.Action) (() =>
        {
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }));
      }
    }
  }

  public override void Cleanup(Follower follower) => base.Cleanup(follower);

  public StructureBrain FindTarget()
  {
    StructureBrain target = (StructureBrain) null;
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain != null && allBrain.Data.ID == this._targetID)
      {
        target = allBrain;
        break;
      }
    }
    return target;
  }

  public Structure FindShrineStructure()
  {
    Structure shrineStructure = (Structure) null;
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && structure.Brain.Data.ID == this._shrineID)
      {
        shrineStructure = structure;
        break;
      }
    }
    return shrineStructure;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this._shrine == null || this._shrine.SoulCount < this._shrine.SoulMax)
      return;
    this.Complete();
  }

  public IEnumerator CollectDevotion(
    GameObject target,
    Vector3 from,
    int amount,
    StructureBrain targetBrain)
  {
    float timeBetween = 1f / (float) amount;
    for (int i = 0; i < amount; ++i)
    {
      SoulCustomTarget.Create(target, from, Color.white, (System.Action) (() =>
      {
        if (targetBrain == null)
          return;
        ++targetBrain.SoulCount;
      }));
      yield return (object) new WaitForSeconds(timeBetween);
    }
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__28_0()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__28_1()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }
}
