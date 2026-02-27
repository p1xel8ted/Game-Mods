// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactPoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactPoop : FollowerTask
{
  public int _poopStructureID;
  public StructureBrain _poop;

  public override FollowerTaskType Type => FollowerTaskType.ReactPoop;

  public override FollowerLocation Location => this._poop.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._poopStructureID;

  public FollowerTask_ReactPoop(int poopID)
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
    {
      this._brain.AddThought(Thought.ReactToPoopGermophobeTrait);
      if ((double) UnityEngine.Random.value >= 0.25)
        return;
      GameManager.GetInstance().StartCoroutine(FollowerTask_ReactPoop.FrameDelay((System.Action) (() =>
      {
        if ((double) IllnessBar.IllnessNormalized <= 0.05000000074505806)
          return;
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
      })));
    }
    else if (this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
    {
      this._brain.AddThought(Thought.ReactToPoopCoprophiliacTrait);
    }
    else
    {
      this._brain.AddThought(Thought.ReactToPoop);
      if ((double) UnityEngine.Random.value >= 0.10000000149011612)
        return;
      GameManager.GetInstance().StartCoroutine(FollowerTask_ReactPoop.FrameDelay((System.Action) (() =>
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
    float num = (float) UnityEngine.Random.Range(2, 3);
    float f = Utils.GetAngle(this._poop.Data.Position, follower.transform.position) * ((float) Math.PI / 180f);
    return this._poop.Data.Position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this._poop.Data.Type != StructureBrain.TYPES.POOP)
      follower.TimedAnimation("Reactions/react-admire" + UnityEngine.Random.Range(1, 4).ToString(), 2.13333344f, (System.Action) (() => this.ProgressTask()));
    else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
      follower.TimedAnimation("Reactions/react-laugh", 3.3f, (System.Action) (() => this.ProgressTask()));
    else
      follower.TimedAnimation("Reactions/react-sick", 2.9666667f, (System.Action) (() => this.ProgressTask()));
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
  public void \u003COnDoingBegin\u003Eb__18_0() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_1() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__18_2() => this.ProgressTask();
}
