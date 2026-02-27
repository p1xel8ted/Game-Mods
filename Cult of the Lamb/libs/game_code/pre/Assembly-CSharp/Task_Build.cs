// Decompiled with JetBrains decompiler
// Type: Task_Build
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using System.Collections;
using UnityEngine;

#nullable disable
public class Task_Build : Task
{
  private Worshipper worshipper;
  private BuildSitePlot buildsitePlot;
  private FarmPlot TargetFarmPlot;
  private GameObject GoToMarker;
  private Vector3 RandomPosition;
  private Coroutine cGoToAndBuild;
  private float RepathTimer = 1f;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.worshipper = t.GetComponent<Worshipper>();
    this.buildsitePlot = TargetObject.GetComponent<BuildSitePlot>();
    this.Type = Task_Type.BUILDING;
    this.RandomPosition = this.buildsitePlot.transform.position + new Vector3(0.0f, (float) (this.buildsitePlot.Bounds.y / 2)) + (Vector3) (UnityEngine.Random.insideUnitCircle * (float) this.buildsitePlot.Bounds.x);
    this.GoToMarker = new GameObject();
    this.GoToMarker.transform.position = this.RandomPosition;
    this.worshipper.GoToAndStop(this.GoToMarker, new System.Action(this.GoToAndBuild), TargetObject, false);
  }

  public override void TaskUpdate()
  {
    if (!((UnityEngine.Object) this.buildsitePlot == (UnityEngine.Object) null))
      return;
    this.WorkCompleted();
  }

  private void GoToAndBuild()
  {
    this.cGoToAndBuild = this.t.StartCoroutine((IEnumerator) this.GoToAndBuildRoutine());
  }

  private IEnumerator GoToAndBuildRoutine()
  {
    Task_Build taskBuild = this;
    yield return (object) new WaitForSeconds(0.5f);
    taskBuild.state.facingAngle = Utils.GetAngle(taskBuild.t.transform.position, taskBuild.buildsitePlot.transform.position);
    taskBuild.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    taskBuild.worshipper.SetAnimation(taskBuild.worshipper.Motivated ? "build-fast-scared" : "build", true);
    taskBuild.worshipper.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(taskBuild.HandleAnimationStateEvent);
    taskBuild.worshipper.health.OnHit += new Health.HitAction(taskBuild.Health_OnHit);
    while ((UnityEngine.Object) taskBuild.buildsitePlot != (UnityEngine.Object) null)
      yield return (object) null;
    taskBuild.WorkCompleted();
  }

  private void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    Debug.Log((object) "HIT AND CHANGE ANIMATION!");
    this.worshipper.SetAnimation("build-fast-scared", true);
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
  }

  public void WorkCompleted()
  {
    this.t.StopAllCoroutines();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.ClearTask();
  }

  public override void ClearTask()
  {
    if ((UnityEngine.Object) this.GoToMarker != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.GoToMarker);
    if ((UnityEngine.Object) this.buildsitePlot != (UnityEngine.Object) null)
      this.buildsitePlot.Worshippers.Remove(this.worshipper);
    this.worshipper.health.OnHit -= new Health.HitAction(this.Health_OnHit);
    this.worshipper.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    base.ClearTask();
  }
}
