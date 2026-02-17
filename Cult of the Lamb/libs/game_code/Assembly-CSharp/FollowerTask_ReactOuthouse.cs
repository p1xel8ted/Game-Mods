// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactOuthouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactOuthouse : FollowerTask
{
  public int _poopStructureID;
  public StructureBrain _poop;

  public override FollowerTaskType Type => FollowerTaskType.ReactOuthouse;

  public override FollowerLocation Location => this._poop.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._poopStructureID;

  public FollowerTask_ReactOuthouse(int poopID)
  {
    this._poopStructureID = poopID;
    this._poop = StructureManager.GetStructureByID<StructureBrain>(this._poopStructureID);
  }

  public override int GetSubTaskCode() => this._poopStructureID;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  public override void OnEnd()
  {
    base.OnEnd();
    if (this._brain.HasTrait(FollowerTrait.TraitType.Germophobe))
      this._brain.AddThought(Thought.ReactToFullOuthouseGermophobeTrait);
    else if (this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
      this._brain.AddThought(Thought.ReactToFullOuthouseCoprophiliacTrait);
    else
      this._brain.AddThought(Thought.ReactToFullOuthouse);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    float num = (float) UnityEngine.Random.Range(2, 3);
    float f = Utils.GetAngle(this._poop.Data.Position, follower.transform.position) * ((float) Math.PI / 180f);
    return this._poop.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
      follower.TimedAnimation("Reactions/react-laugh", 3.3f, (System.Action) (() => this.ProgressTask()));
    else
      follower.TimedAnimation("Reactions/react-sick", 2.9666667f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__17_0() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__17_1() => this.ProgressTask();
}
