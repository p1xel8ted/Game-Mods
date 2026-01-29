// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactEgg : FollowerTask
{
  public Structures_Hatchery hatchery;

  public override FollowerTaskType Type => FollowerTaskType.ReactEgg;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this.hatchery.Data.ID;

  public FollowerTask_ReactEgg(Structures_Hatchery hatchery) => this.hatchery = hatchery;

  public override int GetSubTaskCode() => this.hatchery.Data.ID;

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
    Vector3 position = this.hatchery.Data.Position;
    if (Interaction_Hatchery.Hatcheries.Count > 0)
    {
      foreach (Interaction_Hatchery hatchery in Interaction_Hatchery.Hatcheries)
      {
        if (hatchery.Structure.Brain.Data.ID == this.hatchery.Data.ID)
          return hatchery.transform.position;
      }
    }
    return position;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.FacePosition(this.UpdateDestination(follower));
    follower.TimedAnimation("Egg/egg-tending", 1.83333337f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__16_0() => this.ProgressTask();
}
