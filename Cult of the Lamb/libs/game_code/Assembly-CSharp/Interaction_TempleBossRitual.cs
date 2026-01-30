// Decompiled with JetBrains decompiler
// Type: Interaction_TempleBossRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_TempleBossRitual : Interaction
{
  public RoomSwapManager RoomSwapManager;
  public bool Activating;

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

  public IEnumerator CentrePlayer()
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

  public IEnumerator InteractionRoutine()
  {
    Interaction_TempleBossRitual templeBossRitual = this;
    templeBossRitual.state.facingAngle = 90f;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(templeBossRitual.playerFarming.CameraBone, 8f);
    templeBossRitual.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    templeBossRitual.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    templeBossRitual.playerFarming.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetZoom((float) (8.0 - 3.0 * (double) Progress / (double) Duration));
      CameraManager.shakeCamera((float) (0.10000000149011612 + 0.60000002384185791 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
    templeBossRitual.playerFarming.simpleSpineAnimator.Animate("teleport-out", 0, false);
    yield return (object) new WaitForSeconds(1.16666663f);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.2f, "", new System.Action(templeBossRitual.ChangeRoom));
  }

  public void ChangeRoom()
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

  public IEnumerator PlayerStopAnimationRoutine()
  {
    Interaction_TempleBossRitual templeBossRitual = this;
    yield return (object) new WaitForSeconds(0.5f);
    templeBossRitual.playerFarming.simpleSpineAnimator.Animate("teleport-in", 0, false);
    templeBossRitual.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1.1f);
    GameManager.GetInstance().OnConversationEnd();
  }
}
