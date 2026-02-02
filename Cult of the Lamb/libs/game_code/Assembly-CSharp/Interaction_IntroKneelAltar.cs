// Decompiled with JetBrains decompiler
// Type: Interaction_IntroKneelAltar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_IntroKneelAltar : Interaction
{
  public Interaction_SimpleConversation Conversation;
  public IntroRoomMusicController musicController;
  public GameObject PlayerMoveToPosition;
  public UnitObject unitObject;
  public EnemyBrute Executioner;
  public SkeletonAnimation ExecutionerSpine;
  public SimpleSetCamera SimpleSetCamera;
  public SimpleSetCamera TurnOffSimpleSetCamera;
  public bool Activated;
  public string sInteraction;

  public void Awake() => this.ChangeLightingForPerformanceMode();

  public void Start()
  {
    this.HoldToInteract = true;
    this.UpdateLocalisation();
  }

  public void ChangeLightingForPerformanceMode()
  {
    if (SettingsManager.Settings == null || !SettingsManager.Settings.Game.PerformanceMode)
      return;
    GameObject gameObject = GameObject.Find("Intro Room 1(Clone)");
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    IEnumerator enumerator = (IEnumerator) gameObject.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = enumerator.Current as Transform;
        if ((UnityEngine.Object) current != (UnityEngine.Object) null)
        {
          if (current.name == "StencilLighting_DecalSprite_Fill")
            current.localScale = new Vector3(21.2f, 23.8f, 60f);
          else if (current.name == "StencilLighting_DecalSprite_Fill (2)")
            current.localScale = new Vector3(17.07f, 20.5f, 60f);
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
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

  public IEnumerator PanToLeader()
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

  public IEnumerator KneelRoutine()
  {
    Interaction_IntroKneelAltar interactionIntroKneelAltar = this;
    interactionIntroKneelAltar.musicController.PlayExecutionTrack();
    interactionIntroKneelAltar.Activated = true;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
    GameManager.GetInstance().OnConversationNext(interactionIntroKneelAltar.state.gameObject, 8f);
    yield return (object) new WaitForEndOfFrame();
    LetterBox.Show(true);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerPrisonerController component = interactionIntroKneelAltar.state.GetComponent<PlayerPrisonerController>();
    interactionIntroKneelAltar.unitObject = component.GoToAndStop(interactionIntroKneelAltar.PlayerMoveToPosition.transform.position);
    interactionIntroKneelAltar.unitObject.EndOfPath += new System.Action(interactionIntroKneelAltar.EndOfPath);
  }

  public void Continue() => this.StartCoroutine((IEnumerator) this.ContinueRoutine());

  public IEnumerator ContinueRoutine()
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
    MMVibrate.RumbleContinuous(0.01f, 0.01f, interactionIntroKneelAltar.playerFarming);
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
    MMVibrate.RumbleContinuous(0.02f, 0.02f, interactionIntroKneelAltar.playerFarming);
    yield return (object) new WaitForSeconds(0.5f);
    MMVibrate.RumbleContinuous(0.03f, 0.03f, interactionIntroKneelAltar.playerFarming);
    interactionIntroKneelAltar.Executioner.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionIntroKneelAltar.ExecutionerSpine.AnimationState.SetAnimation(0, "execute", false);
    interactionIntroKneelAltar.ExecutionerSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/miniboss_executioner/game_opening_warning_vo", interactionIntroKneelAltar.Executioner.transform.position);
    interactionIntroKneelAltar.ExecutionerSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionIntroKneelAltar.ExecuteEvent);
    MMVibrate.RumbleContinuous(0.5f, 0.5f, interactionIntroKneelAltar.playerFarming);
    yield return (object) new WaitForSeconds(1.8f);
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", AudioManager.Instance.Listener);
    MMVibrate.RumbleContinuous(1f, 1f, interactionIntroKneelAltar.playerFarming);
    yield return (object) new WaitForSeconds(0.3f);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/miniboss_executioner/game_opening_attack_vo", AudioManager.Instance.Listener);
    AudioManager.Instance.PlayOneShot("event:/weapon/melee_swing_heavy", AudioManager.Instance.Listener);
  }

  public void ExecuteEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "execute"))
      return;
    AudioManager.Instance.PlayOneShot("weapon/metal_heavy", PlayerPrisonerController.Instance.transform.position);
    CameraManager.shakeCamera(0.5f);
    MMVibrate.StopRumble();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.2f, "", new System.Action(this.ChangeRoom));
  }

  public void ChangeRoom()
  {
    MMVibrate.StopRumble();
    this.musicController.StopAll();
    UnityEngine.Object.FindObjectOfType<IntroManager>().ToggleDeathScene();
  }

  public void EndOfPath()
  {
    this.unitObject.EndOfPath -= new System.Action(this.EndOfPath);
    this.Continue();
  }
}
