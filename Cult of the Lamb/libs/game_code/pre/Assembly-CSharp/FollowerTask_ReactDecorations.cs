// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactDecorations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactDecorations : FollowerTask
{
  private int _poopStructureID;
  private StructureBrain _poop;

  public override FollowerTaskType Type => FollowerTaskType.ReactDecoration;

  public override FollowerLocation Location => this._poop.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._poopStructureID;

  public FollowerTask_ReactDecorations(int poopID)
  {
    this._poopStructureID = poopID;
    this._poop = StructureManager.GetStructureByID<StructureBrain>(this._poopStructureID);
  }

  protected override int GetSubTaskCode() => this._poopStructureID;

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  protected override void OnEnd()
  {
    base.OnEnd();
    if (this._brain.HasTrait(FollowerTrait.TraitType.FalseIdols))
      this._brain.AddThought(Thought.ReactDecorationFalseIdols);
    else
      this._brain.AddThought(Thought.ReactDecoration);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    float num = UnityEngine.Random.Range(0.5f, 1.5f);
    float f = (float) (((double) Utils.GetAngle(this._poop.Data.Position, follower.transform.position) + (double) UnityEngine.Random.Range(-45, 45)) * (Math.PI / 180.0));
    return this._poop.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    int num = UnityEngine.Random.Range(1, 4);
    follower.TimedAnimation("Reactions/react-admire" + (object) num, 2f, (System.Action) (() => this.ProgressTask()));
  }
}
