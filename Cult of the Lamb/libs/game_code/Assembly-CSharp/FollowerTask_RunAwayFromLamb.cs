// Decompiled with JetBrains decompiler
// Type: FollowerTask_RunAwayFromLamb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_RunAwayFromLamb : FollowerTask
{
  public float updateDestination;
  public LayerMask layerMask;
  public bool animating;
  public static float timeSinceLastScream;

  public override FollowerTaskType Type => FollowerTaskType.RunFromPlayer;

  public override FollowerLocation Location => FollowerLocation.Base;

  public FollowerTask_RunAwayFromLamb()
  {
    this.layerMask = (LayerMask) ((int) this.layerMask | 1 << LayerMask.NameToLayer("Island"));
  }

  public override int GetSubTaskCode() => 0;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (follower.Brain.Info.CursedState == Thought.Child)
    {
      if (follower.Brain.Info.Age < 10)
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Baby/baby-crawl-scared");
      else
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Baby/baby-walk-scared");
    }
    else
    {
      if ((double) TimeManager.TotalElapsedGameTime <= (double) FollowerTask_RunAwayFromLamb.timeSinceLastScream)
        return;
      FollowerTask_RunAwayFromLamb.timeSinceLastScream = TimeManager.TotalElapsedGameTime + 120f;
      this.animating = true;
      follower.TimedAnimation("Scared/scared-scream", 0.6666667f);
      GameManager.GetInstance().WaitForSeconds(0.6666667f, (System.Action) (() =>
      {
        this.animating = false;
        this.RecalculateDestination();
      }));
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (follower.Brain.Info.CursedState != Thought.Child || follower.Brain.CurrentState == null)
      return;
    follower.Brain.CurrentState.SetStateAnimations(follower);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || (double) Vector3.Distance(this.Brain.LastPosition, PlayerFarming.Instance.transform.position) > 5.0)
    {
      this.End();
    }
    else
    {
      if (this.animating)
        return;
      this.updateDestination -= deltaGameTime;
      if ((double) this.updateDestination <= 0.0)
      {
        this.RecalculateDestination();
        this.updateDestination = 1.5f;
      }
      if ((double) UnityEngine.Random.value >= 0.004999999888241291)
        return;
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_InstantPoop());
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return Vector3.zero;
    float distance = 1.5f;
    Vector3 normalized = (follower.transform.position - PlayerFarming.Instance.transform.position).normalized;
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
  public void \u003CSetup\u003Eb__10_0()
  {
    this.animating = false;
    this.RecalculateDestination();
  }
}
