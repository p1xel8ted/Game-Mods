// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactPrisoner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactPrisoner : FollowerTask
{
  public int _prisonID;
  public StructureBrain _prison;
  public int state;

  public override FollowerTaskType Type => FollowerTaskType.ReactPrisoner;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._prisonID;

  public FollowerTask_ReactPrisoner(int prisonID)
  {
    this._prisonID = prisonID;
    this._prison = StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
  }

  public override int GetSubTaskCode() => this._prisonID;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  public override void OnEnd() => base.OnEnd();

  public override Vector3 UpdateDestination(Follower follower)
  {
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
    float num = (float) UnityEngine.Random.Range(2, 3);
    float f = Utils.GetAngle(structureById.Data.Position, follower.transform.position) * ((float) Math.PI / 180f);
    return structureById.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    string animation = "Reactions/react-laugh";
    this.state = 0;
    foreach (IDAndRelationship relationship in follower.Brain.Info.Relationships)
    {
      if (relationship.ID == this._prison.Data.FollowerID && (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers || relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends))
      {
        animation = "Reactions/react-cry";
        this.state = 1;
        break;
      }
    }
    follower.FacePosition(this._brain.LastPosition);
    follower.TimedAnimation(animation, 3.33f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_0() => this.ProgressTask();
}
