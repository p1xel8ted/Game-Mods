// Decompiled with JetBrains decompiler
// Type: IntroDeathSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System.Collections;
using Unify;
using UnityEngine;

#nullable disable
public class IntroDeathSceneManager : BaseMonoBehaviour
{
  public IntroDeathSceneMusicController musicController;
  public Vector3 ActivateOffset;
  public float ActivateDistance;
  public Interaction_SimpleConversation Conversation;
  public Interaction_SimpleConversation ConversationResponse;
  public SkeletonAnimation DeathSpine;
  public GameObject DeathSpineCamera;
  public SkeletonAnimation DeathCat_0;
  public SkeletonAnimation DeathCat_1;
  public SimpleSetCamera SimpleSetCamera;
  public CameraFollowTarget Camera;
  public GameObject PlayerPrefab;
  public Material PlayerMaterial;
  public Transform StartPlayerPosition;
  public bool HideHUD;
  public BiomeConstantsVolume biomeVolume;
  public bool triggered;
  public float playerStartingMaterialValue_0;
  public float playerStartingMaterialValue_1;
  public bool wasPlayed;
  public GameObject Player;

  public void Start()
  {
    this.Conversation.enabled = false;
    this.ConversationResponse.enabled = false;
    this.StartCoroutine((IEnumerator) this.PlaceAndPositionPlayer());
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  public IEnumerator PlaceAndPositionPlayer()
  {
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
      this.Player = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPrefab, GameObject.FindGameObjectWithTag("Unit Layer").transform, true);
    StateMachine state = this.Player.GetComponent<StateMachine>();
    state.facingAngle = 85f;
    if ((UnityEngine.Object) this.StartPlayerPosition != (UnityEngine.Object) null)
      this.Player.transform.position = this.StartPlayerPosition.position;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew(false, true, true, (PlayerFarming) null);
    GameManager.GetInstance().CamFollowTarget.SetOffset(new Vector3(0.0f, 0.0f, -0.2f));
    GameManager.GetInstance().CamFollowTarget.CurrentOffset = new Vector3(0.0f, 0.0f, -0.2f);
    GameManager.GetInstance().CameraSetZoom(5f);
    PlayerPrisonerController component = this.Player.GetComponent<PlayerPrisonerController>();
    GameObject playerCameraBone = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? GameObject.FindWithTag("Player Camera Bone") : component.CameraBone;
    this.Camera.SnapTo(playerCameraBone.transform.position);
    state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    SimpleSpineAnimator componentInChildren = this.Player.GetComponentInChildren<SimpleSpineAnimator>();
    componentInChildren.Animate("intro/kneel-wake", 0, false);
    componentInChildren.AddAnimate("intro/idle-up", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(4.5f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddToCamera(playerCameraBone);
    state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator WaitForPlayer()
  {
    IntroDeathSceneManager deathSceneManager = this;
    while ((UnityEngine.Object) (deathSceneManager.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      yield return (object) null;
    while ((double) Vector3.Distance(deathSceneManager.Player.transform.position, deathSceneManager.transform.position + deathSceneManager.ActivateOffset) > (double) deathSceneManager.ActivateDistance)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    deathSceneManager.SimpleSetCamera.Play();
    deathSceneManager.DeathCat_0.AnimationState.SetAnimation(0, "snarling", true);
    deathSceneManager.DeathCat_1.AnimationState.SetAnimation(0, "snarling", true);
    AudioManager.Instance.PlayOneShot("event:/dialogue/death_cat_dogs/death_cat_dogs", deathSceneManager.DeathCat_0.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dialogue/death_cat_dogs/death_cat_dogs", deathSceneManager.DeathCat_1.gameObject);
    deathSceneManager.DeathSpine.AnimationState.SetAnimation(0, "crouch-down", false);
    deathSceneManager.DeathSpine.AnimationState.AddAnimation(0, "idle-crouched", true, 0.0f);
    UnityEngine.Object.FindObjectOfType<PlayerPrisonerController>().GoToAndStop(deathSceneManager.DeathSpine.transform.position + Vector3.down * 2f, StateMachine.State.InActive);
    yield return (object) new WaitForSeconds(1f);
    deathSceneManager.Conversation.enabled = true;
    deathSceneManager.Conversation.Play();
  }

  public void Respond()
  {
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("DEAL_WITH_THE_DEVIL"));
    Debug.Log((object) "RESPOND!");
    this.ConversationResponse.enabled = true;
    this.ConversationResponse.Play();
    LetterBox.Show(true);
  }

  public void GiveCrown() => this.StartCoroutine((IEnumerator) this.GiveCrownRoutine());

  public IEnumerator GiveCrownRoutine()
  {
    IntroDeathSceneManager deathSceneManager = this;
    yield return (object) new WaitForEndOfFrame();
    DataManager.Instance.HadInitialDeathCatConversation = true;
    deathSceneManager.Player.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.InActive;
    LetterBox.Show(true);
    deathSceneManager.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathSceneManager.DeathSpineCamera, 10f);
    deathSceneManager.DeathSpine.AnimationState.SetAnimation(0, "give-life", false);
    deathSceneManager.musicController.PlaySpawnCrown();
    float Progress = 0.0f;
    float Duration = 3.83333349f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
      yield return (object) null;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", new System.Action(deathSceneManager.PlayVideo));
  }

  public void PlayVideo()
  {
    this.biomeVolume.manualExitAndDeactive();
    this.musicController.StopAll();
    MMVideoPlayer.Play("Intro", new System.Action(this.VideoComplete), MMVideoPlayer.Options.DISABLE, MMVideoPlayer.Options.DISABLE, false);
    AudioManager.Instance.PlayOneShot("event:/music/intro/intro_video");
    UnityEngine.Object.FindObjectOfType<IntroManager>().DisableBoth();
    MMTransition.ResumePlay();
  }

  public void VideoComplete()
  {
    if (this.wasPlayed)
      return;
    MMTransition.IsPlaying = false;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", new System.Action(this.ChangeRoom));
    this.wasPlayed = true;
  }

  public void ChangeRoom()
  {
    if (this.triggered)
      return;
    this.triggered = true;
    MMTransition.IsPlaying = true;
    MMVideoPlayer.Hide();
    this.PlayerMaterial.SetFloat("_CloudOverlayIntensity", this.playerStartingMaterialValue_0);
    this.PlayerMaterial.SetFloat("_ShadowIntensity", this.playerStartingMaterialValue_1);
    UnityEngine.Object.FindObjectOfType<IntroManager>().ToggleGameScene();
    MMTransition.ResumePlay();
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.green);
  }
}
