// Decompiled with JetBrains decompiler
// Type: FollowerTask_RunAway
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_RunAway : FollowerTask
{
  public static float TimeSinceLastScream;
  public float updateDestination;
  public LayerMask layerMask;
  public bool animating;
  public bool autoComplete = true;
  public float updateTime = 1.5f;
  public GameObject target;
  public Vector3 targetPos = Vector3.zero;

  public override FollowerTaskType Type => FollowerTaskType.RunFromSomething;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSocial => true;

  public FollowerTask_RunAway(GameObject target)
  {
    this.target = target;
    this.layerMask = (LayerMask) ((int) this.layerMask | 1 << LayerMask.NameToLayer("Island"));
  }

  public FollowerTask_RunAway(FollowerBrain brain)
  {
    this.target = FollowerManager.FindFollowerByID(brain.Info.ID)?.gameObject;
    this.layerMask = (LayerMask) ((int) this.layerMask | 1 << LayerMask.NameToLayer("Island"));
  }

  public FollowerTask_RunAway(Vector3 targetPos)
  {
    this.targetPos = targetPos;
    this.updateTime = -1f;
    this.autoComplete = false;
    this.layerMask = (LayerMask) ((int) this.layerMask | 1 << LayerMask.NameToLayer("Island"));
  }

  public override int GetSubTaskCode() => 0;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Interaction_FollowerInteraction.Interactable = false;
    if ((double) TimeManager.TotalElapsedGameTime > (double) FollowerTask_RunAway.TimeSinceLastScream && follower.Brain.Info.CursedState != Thought.Child)
    {
      FollowerTask_RunAway.TimeSinceLastScream = TimeManager.TotalElapsedGameTime + 120f;
      this.animating = true;
      follower.TimedAnimation("Scared/scared-scream", 0.6666667f);
      GameManager.GetInstance().WaitForSeconds(0.6666667f, (System.Action) (() =>
      {
        this.animating = false;
        this.RecalculateDestination();
      }));
      follower.SetEmotionAnimation();
    }
    else
      follower.SetEmotionAnimation();
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Interaction_FollowerInteraction.Interactable = true;
    if (follower.Brain.Info.CursedState != Thought.Child || follower.Brain.CurrentState == null)
      return;
    follower.Brain.CurrentState.SetStateAnimations(follower);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (((UnityEngine.Object) this.target == (UnityEngine.Object) null || (double) Vector3.Distance(this.Brain.LastPosition, this.target.transform.position) > 5.0) && this.autoComplete)
    {
      this.End();
    }
    else
    {
      if (this.animating || (double) this.updateTime == -1.0)
        return;
      this.updateDestination -= deltaGameTime;
      if ((double) this.updateDestination <= 0.0)
      {
        this.RecalculateDestination();
        this.updateDestination = this.updateTime;
      }
      if ((double) UnityEngine.Random.value >= 0.004999999888241291)
        return;
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_InstantPoop());
    }
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (follower.Brain.Info.CursedState != Thought.Child)
      return;
    if (follower.Brain.Info.Age < 10)
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Baby/baby-crawl-chased");
    else
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Baby/baby-chased");
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (this.Brain.Info.CursedState != Thought.Child)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    this.Brain.CurrentState.SetStateAnimations(followerById, true);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return this.targetPos;
    if (follower.Brain.Info.CursedState == Thought.Child)
    {
      foreach (Structures_Daycare structuresDaycare in StructureManager.GetAllStructuresOfType<Structures_Daycare>())
      {
        if (structuresDaycare.Data.MultipleFollowerIDs.Contains(follower.Brain.Info.ID))
        {
          foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
          {
            if (daycare.Structure.Brain.Data.ID == structuresDaycare.Data.ID)
              return daycare.MiddlePosition + (Vector3) UnityEngine.Random.insideUnitCircle * ((Structures_Daycare) daycare.Structure.Brain).BoundariesRadius;
          }
        }
      }
    }
    float distance = 1.5f;
    Vector3 normalized = (follower.transform.position - this.target.transform.position).normalized;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) normalized, distance, (int) this.layerMask).collider == (UnityEngine.Object) null)
      return follower.transform.position + normalized * distance;
    if ((double) Mathf.Abs(normalized.y) > 0.5 && (double) Mathf.Abs(normalized.x) < 0.5)
    {
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.right, distance, (int) this.layerMask).collider == (UnityEngine.Object) null)
        return follower.transform.position + Vector3.right * distance;
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.left, distance, (int) this.layerMask).collider == (UnityEngine.Object) null)
        return follower.transform.position + Vector3.left * distance;
    }
    else
    {
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.up, distance, (int) this.layerMask).collider == (UnityEngine.Object) null)
        return follower.transform.position + Vector3.up * distance;
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) Vector3.down, distance, (int) this.layerMask).collider == (UnityEngine.Object) null)
        return follower.transform.position + Vector3.down * distance;
    }
    return follower.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 5f);
  }

  [CompilerGenerated]
  public void \u003CSetup\u003Eb__22_0()
  {
    this.animating = false;
    this.RecalculateDestination();
  }
}
