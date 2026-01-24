// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactCorpse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactCorpse : FollowerTask
{
  public int _corpseStructureID;
  public Structures_DeadWorshipper _corpse;

  public override FollowerTaskType Type => FollowerTaskType.ReactCorpse;

  public override FollowerLocation Location => this._corpse.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._corpseStructureID;

  public FollowerTask_ReactCorpse(int corpseID)
  {
    this._corpseStructureID = corpseID;
    this._corpse = StructureManager.GetStructureByID<Structures_DeadWorshipper>(this._corpseStructureID);
  }

  public override int GetSubTaskCode() => this._corpseStructureID;

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
    if (this._brain.HasTrait(FollowerTrait.TraitType.DesensitisedToDeath))
      this._brain.AddThought(Thought.HappyToSeeDeadBody);
    else if (this._brain.HasTrait(FollowerTrait.TraitType.FearOfDeath))
    {
      if (this._corpse.Data.Rotten)
      {
        this._brain.AddThought(Thought.GrievedAtRottenUnburiedBodyFearOfDeathTrait);
        if ((double) UnityEngine.Random.value >= 0.75)
          return;
        GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerTask_ReactCorpse.FrameDelay((System.Action) (() =>
        {
          if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
            return;
          this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
        })));
      }
      else
      {
        this._brain.AddThought(Thought.GrievedAtUnburiedBodyFearOfDeathTrait);
        if ((double) UnityEngine.Random.value >= 0.33000001311302185)
          return;
        GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerTask_ReactCorpse.FrameDelay((System.Action) (() =>
        {
          if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
            return;
          this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
        })));
      }
    }
    else if (this._corpse.Data.Rotten)
    {
      if ((double) UnityEngine.Random.value >= 0.5)
        return;
      this._brain.AddThought(Thought.GrievedAtRottenUnburiedBody);
      GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerTask_ReactCorpse.FrameDelay((System.Action) (() =>
      {
        if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
          return;
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
      })));
    }
    else
    {
      this._brain.AddThought(Thought.GrievedAtUnburiedBody);
      if ((double) UnityEngine.Random.value >= 0.25)
        return;
      GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerTask_ReactCorpse.FrameDelay((System.Action) (() =>
      {
        if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
          return;
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
      })));
    }
  }

  public static IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Structures_DeadWorshipper structureById = StructureManager.GetStructureByID<Structures_DeadWorshipper>(this._corpseStructureID);
    float num = UnityEngine.Random.Range(0.5f, 1.5f);
    float f = (float) (((double) Utils.GetAngle(structureById.Data.Position, follower.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    return structureById.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    int num = this._corpse.Data.Rotten ? 1 : 0;
    Structures_DeadWorshipper structureById = StructureManager.GetStructureByID<Structures_DeadWorshipper>(this._corpseStructureID);
    follower.FacePosition(structureById.Data.Position);
    if (this._brain.HasTrait(FollowerTrait.TraitType.DesensitisedToDeath))
    {
      follower.TimedAnimation("Reactions/react-grieve-happy", 3.3f, (System.Action) (() => this.ProgressTask()));
    }
    else
    {
      IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._corpse.Data.FollowerID);
      if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
        follower.TimedAnimation("Reactions/react-laugh", 3.3f, (System.Action) (() => this.ProgressTask()));
      else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Strangers)
        follower.TimedAnimation("Reactions/react-sad", 3f, (System.Action) (() => this.ProgressTask()));
      else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
      {
        follower.TimedAnimation("Reactions/react-cry", 3f, (System.Action) (() => this.ProgressTask()));
      }
      else
      {
        if (relationship.CurrentRelationshipState != IDAndRelationship.RelationshipState.Lovers)
          return;
        follower.TimedAnimation("Reactions/react-cry", 3f, (System.Action) (() => this.ProgressTask()));
      }
    }
  }

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__15_0()
  {
    if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
      return;
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
  }

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__15_1()
  {
    if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
      return;
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
  }

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__15_2()
  {
    if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
      return;
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
  }

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__15_3()
  {
    if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
      return;
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_0() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_1() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_2() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_3() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_4() => this.ProgressTask();
}
