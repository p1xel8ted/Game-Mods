// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactGrave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactGrave : FollowerTask
{
  public int _graveID;
  public StructureBrain _grave;
  public int state;

  public override FollowerTaskType Type => FollowerTaskType.ReactGrave;

  public override FollowerLocation Location => this._grave.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._graveID;

  public FollowerTask_ReactGrave(int graveID)
  {
    this._graveID = graveID;
    this._grave = StructureManager.GetStructureByID<StructureBrain>(this._graveID);
  }

  public override int GetSubTaskCode() => this._graveID;

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
    if (this.state == 0)
      this._brain.AddThought(Thought.ReactGrave);
    else if (this.state == 1)
      this._brain.AddThought(Thought.ReactGraveAfterlife);
    else if (this.state == 2)
      this._brain.AddThought(Thought.ReactGraveLover);
    else if (this.state == 3)
      this._brain.AddThought(Thought.ReactGraveEnemy);
    CultFaithManager.AddThought(Thought.Cult_BuildingGoodGraves, this._brain.Info.ID);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this._graveID);
    float num = (float) UnityEngine.Random.Range(2, 3);
    float f = Utils.GetAngle(structureById.Data.Position, follower.transform.position) * ((float) Math.PI / 180f);
    return structureById.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    string animation = "Reactions/react-grieve-sad";
    this.state = 0;
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.DesensitisedToDeath))
    {
      animation = "Reactions/react-grieve-happy";
      this.state = 1;
    }
    foreach (IDAndRelationship relationship in follower.Brain.Info.Relationships)
    {
      if (relationship.ID == this._grave.Data.FollowerID)
      {
        if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
        {
          animation = "Reactions/react-cry";
          this.state = 2;
          break;
        }
        if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
        {
          animation = "Reactions/react-grieve-happy";
          this.state = 3;
          break;
        }
      }
    }
    follower.TimedAnimation(animation, 3.33f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_0() => this.ProgressTask();
}
