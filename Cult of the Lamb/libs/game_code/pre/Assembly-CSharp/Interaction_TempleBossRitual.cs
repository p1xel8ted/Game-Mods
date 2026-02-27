// Decompiled with JetBrains decompiler
// Type: Interaction_TempleBossRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_TempleBossRitual : Interaction
{
  public RoomSwapManager RoomSwapManager;
  private bool Activating;

  public override void GetLabel()
  {
    this.Label = this.Activating ? "" : "Perform spooky boopy ritual!";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.InteractionRoutine());
    this.StartCoroutine((IEnumerator) this.CentrePlayer());
  }

  private IEnumerator CentrePlayer()
  {
    Interaction_TempleBossRitual templeBossRitual = this;
    float Progress = 0.0f;
    float Duration = 0.5f;
    Vector3 StartPosition = templeBossRitual.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      templeBossRitual.state.transform.position = Vector3.Lerp(StartPosition, templeBossRitual.transform.position + Vector3.down, Progress / Duration);
      yield return (object) null;
    }
  }

  private IEnumerator InteractionRoutine()
  {
    Interaction_TempleBossRitual templeBossRitual = this;
    templeBossRitual.state.facingAngle = 90f;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
    templeBossRitual.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetZoom((float) (8.0 - 3.0 * (double) Progress / (double) Duration));
      CameraManager.shakeCamera((float) (0.10000000149011612 + 0.60000002384185791 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
    PlayerFarming.Instance.simpleSpineAnimator.Animate("teleport-out", 0, false);
    yield return (object) new WaitForSeconds(1.16666663f);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.2f, "", new System.Action(templeBossRitual.ChangeRoom));
  }

  private void ChangeRoom()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask.Type == FollowerTaskType.AssistRitual)
      {
        allBrain.FollowingPlayer = false;
        allBrain.CompleteCurrentTask();
        allBrain.DesiredLocation = FollowerLocation.Base;
      }
    }
    this.RoomSwapManager.ToggleChurch();
    CameraManager.shakeCamera(0.5f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlayerStopAnimationRoutine());
  }

  private IEnumerator PlayerStopAnimationRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("teleport-in", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1.1f);
    GameManager.GetInstance().OnConversationEnd();
  }
}
