// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactVomit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactVomit : FollowerTask
{
  private int _vomitStructureID;
  private StructureBrain _vomit;

  public override FollowerTaskType Type => FollowerTaskType.ReactVomit;

  public override FollowerLocation Location => this._vomit.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._vomitStructureID;

  public FollowerTask_ReactVomit(int vomitID)
  {
    this._vomitStructureID = vomitID;
    this._vomit = StructureManager.GetStructureByID<StructureBrain>(this._vomitStructureID);
  }

  protected override int GetSubTaskCode() => this._vomitStructureID;

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
    if (this._brain.HasTrait(FollowerTrait.TraitType.Germophobe))
    {
      this._brain.AddThought(Thought.ReactToVomitGermophobeTrait);
      if ((double) UnityEngine.Random.value >= 0.33000001311302185)
        return;
      GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerTask_ReactVomit.FrameDelay((System.Action) (() =>
      {
        if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
          return;
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
      })));
    }
    else if (this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
    {
      this._brain.AddThought(Thought.ReactToVomitCoprophiliacTrait);
    }
    else
    {
      this._brain.AddThought(Thought.ReactToVomit);
      if ((double) UnityEngine.Random.value >= 0.10000000149011612)
        return;
      GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerTask_ReactVomit.FrameDelay((System.Action) (() =>
      {
        if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
          return;
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
      })));
    }
  }

  private static IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this._vomitStructureID);
    float num = (float) UnityEngine.Random.Range(2, 3);
    float f = Utils.GetAngle(structureById.Data.Position, follower.transform.position) * ((float) Math.PI / 180f);
    return structureById.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
      follower.TimedAnimation("Reactions/react-laugh", 3.3f, (System.Action) (() => this.ProgressTask()));
    else
      follower.TimedAnimation("Reactions/react-sick", 2.9666667f, (System.Action) (() => this.ProgressTask()));
  }

  private Vomit FindVomit()
  {
    Vomit vomit1 = (Vomit) null;
    foreach (Vomit vomit2 in Vomit.Vomits)
    {
      if (vomit2.structure.Structure_Info.ID == this._vomitStructureID)
      {
        vomit1 = vomit2;
        break;
      }
    }
    return vomit1;
  }
}
