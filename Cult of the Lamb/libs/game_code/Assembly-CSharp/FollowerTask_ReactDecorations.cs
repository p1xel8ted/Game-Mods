// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactDecorations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactDecorations : FollowerTask
{
  public int _poopStructureID;
  public StructureBrain _poop;

  public override FollowerTaskType Type => FollowerTaskType.ReactDecoration;

  public override FollowerLocation Location => this._poop.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._poopStructureID;

  public FollowerTask_ReactDecorations(int poopID)
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
    if (this._brain.HasTrait(FollowerTrait.TraitType.FalseIdols))
      this._brain.AddThought(Thought.ReactDecorationFalseIdols);
    else
      this._brain.AddThought(Thought.ReactDecoration);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    float num = UnityEngine.Random.Range(0.5f, 1.5f);
    float f = (float) (((double) Utils.GetAngle(this._poop.Data.Position, follower.transform.position) + (double) UnityEngine.Random.Range(-45, 45)) * (Math.PI / 180.0));
    return this._poop.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    int num = UnityEngine.Random.Range(1, 4);
    follower.TimedAnimation("Reactions/react-admire" + num.ToString(), 2f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__17_0() => this.ProgressTask();
}
