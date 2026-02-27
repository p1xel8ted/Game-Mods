// Decompiled with JetBrains decompiler
// Type: Interaction_IntroKneelAltar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_IntroKneelAltar : Interaction
{
  public Interaction_SimpleConversation Conversation;
  public IntroRoomMusicController musicController;
  public GameObject PlayerMoveToPosition;
  private UnitObject unitObject;
  public EnemyBrute Executioner;
  public SkeletonAnimation ExecutionerSpine;
  public SimpleSetCamera SimpleSetCamera;
  public SimpleSetCamera TurnOffSimpleSetCamera;
  private bool Activated;
  private string sInteraction;

  private void Start()
  {
    this.HoldToInteract = true;
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sInteraction = ScriptLocalization.Interactions_Intro.Kneel;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sInteraction;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    this.Activated = true;
    state.CURRENT_STATE = StateMachine.State.InActive;
    this.StartCoroutine((IEnumerator) this.PanToLeader());
  }

  private IEnumerator PanToLeader()
  {
    SimpleSetCamera.DisableAll();
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(this.Conversation.Entries[0].Speaker);
    PlayerPrisonerController.Instance.Speed = 0.0f;
    yield return (object) new WaitForSeconds(1f);
    this.Conversation.enabled = true;
  }

  public void Play()
  {
    SimpleSetCamera.EnableAll();
    this.StartCoroutine((IEnumerator) this.KneelRoutine());
  }

  private IEnumerator KneelRoutine()
  {
    Interaction_IntroKneelAltar interactionIntroKneelAltar = this;
    interactionIntroKneelAltar.musicController.PlayExecutionTrack();
    interactionIntroKneelAltar.Activated = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionIntroKneelAltar.state.gameObject, 8f);
    yield return (object) new WaitForEndOfFrame();
    LetterBox.Show(true);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerPrisonerController component = interactionIntroKneelAltar.state.GetComponent<PlayerPrisonerController>();
    interactionIntroKneelAltar.unitObject = component.GoToAndStop(interactionIntroKneelAltar.PlayerMoveToPosition.transform.position);
    interactionIntroKneelAltar.unitObject.EndOfPath += new System.Action(interactionIntroKneelAltar.EndOfPath);
  }

  private void Continue() => this.StartCoroutine((IEnumerator) this.ContinueRoutine());

  private IEnumerator ContinueRoutine()
  {
    Interaction_IntroKneelAltar interactionIntroKneelAltar = this;
    PlayerPrisonerController.Instance.GoToAndStopping = true;
    interactionIntroKneelAltar.state.facingAngle = Utils.GetAngle(interactionIntroKneelAltar.state.transform.position, interactionIntroKneelAltar.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    interactionIntroKneelAltar.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNext(interactionIntroKneelAltar.state.gameObject, 6f);
    SimpleSpineAnimator componentInChildren = interactionIntroKneelAltar.state.GetComponentInChildren<SimpleSpineAnimator>();
    componentInChildren.Animate("intro/kneel", 0, false);
    componentInChildren.AddAnimate("intro/kneel-loop", 0, true, 0.0f);
    interactionIntroKneelAltar.TurnOffSimpleSetCamera.enabled = false;
    interactionIntroKneelAltar.SimpleSetCamera.Play();
    yield return (object) new WaitForSeconds(1f);
    MMVibrate.RumbleContinuous(0.01f, 0.01f);
    interactionIntroKneelAltar.Executioner.state.CURRENT_STATE = StateMachine.State.Moving;
    Vector3 StartPosition = interactionIntroKneelAltar.Executioner.transform.position;
    Vector3 EndPosition = interactionIntroKneelAltar.state.transform.position + Vector3.right * 3f;
    float Progress = 0.0f;
    float Duration = 2f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      interactionIntroKneelAltar.Executioner.transform.position = Vector3.Lerp(StartPosition, EndPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    interactionIntroKneelAltar.Executioner.state.CURRENT_STATE = StateMachine.State.Idle;
    MMVibrate.RumbleContinuous(0.02f, 0.02f);
    yield return (object) new WaitForSeconds(0.5f);
    MMVibrate.RumbleContinuous(0.03f, 0.03f);
    interactionIntroKneelAltar.Executioner.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionIntroKneelAltar.ExecutionerSpine.AnimationState.SetAnimation(0, "execute", false);
    interactionIntroKneelAltar.ExecutionerSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/warning", interactionIntroKneelAltar.Executioner.transform.position);
    interactionIntroKneelAltar.ExecutionerSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionIntroKneelAltar.ExecuteEvent);
    MMVibrate.RumbleContinuous(0.5f, 0.5f);
    yield return (object) new WaitForSeconds(1.8f);
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", AudioManager.Instance.Listener);
    MMVibrate.RumbleContinuous(1f, 1f);
    yield return (object) new WaitForSeconds(0.3f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/attack", AudioManager.Instance.Listener);
    AudioManager.Instance.PlayOneShot("event:/weapon/melee_swing_heavy", AudioManager.Instance.Listener);
  }

  private void ExecuteEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "execute"))
      return;
    AudioManager.Instance.PlayOneShot("weapon/metal_heavy", PlayerPrisonerController.Instance.transform.position);
    CameraManager.shakeCamera(0.5f);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.2f, "", new System.Action(this.ChangeRoom));
  }

  private void ChangeRoom()
  {
    MMVibrate.StopRumble();
    this.musicController.StopAll();
    UnityEngine.Object.FindObjectOfType<IntroManager>().ToggleDeathScene();
  }

  private void EndOfPath()
  {
    this.unitObject.EndOfPath -= new System.Action(this.EndOfPath);
    this.Continue();
  }
}
