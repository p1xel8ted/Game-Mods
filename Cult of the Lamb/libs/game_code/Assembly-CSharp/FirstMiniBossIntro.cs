// Decompiled with JetBrains decompiler
// Type: FirstMiniBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FirstMiniBossIntro : BossIntro
{
  public SkeletonAnimation LeshySpine;
  public SkeletonAnimation SpawnEffectSpine;
  public GameObject SpawnEffect;
  [SerializeField]
  public Health enemy;
  [SerializeField]
  public Transform playerPosition;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IdleAnimation;
  public List<SkeletonAnimation> BossSpines;
  [SerializeField]
  public Material leaderOldMaterial;
  [SerializeField]
  public Material leaderNewMaterial;
  [SerializeField]
  public GameObject goopFloorParticle;
  [SerializeField]
  public LightingManagerVolume lightOverride;
  [TermsPopup("")]
  public string DisplayName;
  [EventRef]
  public string RoarSfx;
  public GameObject Leshy;
  public Interaction_SimpleConversation Conversation;
  public bool WaitingForAnimationToComplete = true;
  public bool WaitingForConversationToComplete = true;
  public GameObject Follower;
  public SkeletonAnimation FollowerSpine;
  public SkeletonAnimation goopSkeletonAnimation;
  public bool triggered;
  public EventInstance LoopedSound;
  public EventInstance roarInstance;

  public void OnDisable() => AudioManager.Instance.StopLoop(this.LoopedSound);

  public void OnEnable()
  {
    if (DataManager.Instance.CheckKilledBosses(this.GetComponentInParent<MiniBossController>().name) || DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
    {
      this.Follower.gameObject.SetActive(false);
      foreach (SkeletonAnimation bossSpine in this.BossSpines)
      {
        bossSpine.gameObject.SetActive(true);
        bossSpine.AnimationState.SetAnimation(0, "animation", true);
      }
      this.BossSpine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
    }
    this.goopSkeletonAnimation = this.goopFloorParticle.GetComponent<SkeletonAnimation>();
  }

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    FirstMiniBossIntro firstMiniBossIntro = this;
    if (!DataManager.Instance.CheckKilledBosses(firstMiniBossIntro.GetComponentInParent<MiniBossController>().name) && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
    {
      while ((double) PlayerFarming.GetClosestPlayerDist(firstMiniBossIntro.transform.position) > 2.0)
      {
        for (int index = 0; index < PlayerFarming.playersCount; ++index)
        {
          PlayerFarming player = PlayerFarming.players[index];
          if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
            PlayerFarming.SetStateForAllPlayers();
        }
        firstMiniBossIntro.transform.position = Vector3.zero;
        yield return (object) null;
      }
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(firstMiniBossIntro.Conversation.gameObject);
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
        PlayerFarming.players[index].GoToAndStop(firstMiniBossIntro.playerPosition.position + new Vector3(0.0f, (float) index, 0.0f), firstMiniBossIntro.gameObject);
      firstMiniBossIntro.LeshySpine.CustomMaterialOverride.Add(firstMiniBossIntro.leaderOldMaterial, firstMiniBossIntro.leaderNewMaterial);
      yield return (object) new WaitForSeconds(0.5f);
      firstMiniBossIntro.goopFloorParticle.gameObject.SetActive(true);
      firstMiniBossIntro.goopSkeletonAnimation.AnimationState.AddAnimation(0, "leader-loop", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", firstMiniBossIntro.goopFloorParticle.transform.position);
      firstMiniBossIntro.lightOverride.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
      firstMiniBossIntro.LeshySpine.transform.parent.gameObject.SetActive(true);
      yield return (object) new WaitForEndOfFrame();
      firstMiniBossIntro.LeshySpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(2f);
      foreach (Component bossSpine in firstMiniBossIntro.BossSpines)
        bossSpine.gameObject.SetActive(false);
      foreach (SkeletonAnimation bossSpine in firstMiniBossIntro.BossSpines)
        bossSpine.AnimationState.SetAnimation(0, "hidden", false);
      firstMiniBossIntro.Leshy.SetActive(true);
      firstMiniBossIntro.Conversation.Play();
      while (firstMiniBossIntro.WaitingForConversationToComplete)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(firstMiniBossIntro.Follower, 6f);
      float Progress = 0.0f;
      float Duration = 5f;
      float StartingZoom = GameManager.GetInstance().CamFollowTarget.distance;
      firstMiniBossIntro.FollowerSpine.AnimationState.SetAnimation(0, "mutate", false);
      firstMiniBossIntro.SpawnEffect.SetActive(true);
      firstMiniBossIntro.SpawnEffectSpine.AnimationState.SetAnimation(0, "boss-transform", false);
      float f = 0.0f;
      DOTween.To((DOGetter<float>) (() => f), (DOSetter<float>) (x => f = x), 1f, 1.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/enemy/summoned")));
      while ((double) (Progress += Time.deltaTime) < (double) Duration - 0.5)
      {
        GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(StartingZoom, 4f, Progress / Duration));
        yield return (object) null;
      }
      CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
      foreach (SkeletonAnimation bossSpine in firstMiniBossIntro.BossSpines)
      {
        bossSpine.AnimationState.SetAnimation(0, "transform", false);
        bossSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      }
      Vector3 position = firstMiniBossIntro.Follower.transform.position;
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid/death", position);
      AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", position);
      AudioManager.Instance.PlayOneShot("event:/enemy/summon", position);
      AudioManager.Instance.PlayOneShot(" event:/enemy/vocals/worm_large/warning", position);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(position);
      RumbleManager.Instance.Rumble();
      foreach (Component bossSpine in firstMiniBossIntro.BossSpines)
        bossSpine.gameObject.SetActive(true);
      firstMiniBossIntro.Follower.SetActive(false);
      firstMiniBossIntro.SpawnEffect.SetActive(false);
      firstMiniBossIntro.LeshySpine.AnimationState.SetAnimation(0, "exit", false);
      AudioManager.Instance.StopLoop(firstMiniBossIntro.LoopedSound);
      firstMiniBossIntro.lightOverride.gameObject.SetActive(false);
      yield return (object) new WaitForSeconds(1.5f);
      firstMiniBossIntro.goopSkeletonAnimation.AnimationState.SetAnimation(0, "leader-stop", false);
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", firstMiniBossIntro.goopFloorParticle.transform.position);
    }
    else
    {
      firstMiniBossIntro.Follower.SetActive(false);
      firstMiniBossIntro.Leshy.SetActive(false);
      firstMiniBossIntro.BossSpine.AnimationState.SetAnimation(0, firstMiniBossIntro.IntroAnimation, false);
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(firstMiniBossIntro.CameraTarget, 5f);
    HUD_DisplayName.Play(firstMiniBossIntro.DisplayName, 2, HUD_DisplayName.Positions.Centre);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    firstMiniBossIntro.BossSpine.AnimationState.SetAnimation(0, firstMiniBossIntro.IntroAnimation, false);
    firstMiniBossIntro.BossSpine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(firstMiniBossIntro.AnimationState_Complete);
    firstMiniBossIntro.BossSpine.AnimationState.AddAnimation(0, firstMiniBossIntro.IdleAnimation, true, 0.0f);
    firstMiniBossIntro.roarInstance = AudioManager.Instance.CreateLoop(firstMiniBossIntro.RoarSfx, firstMiniBossIntro.BossSpine.gameObject);
    int num = (int) firstMiniBossIntro.roarInstance.setParameterByName("roar_layers", 1f);
    AudioManager.Instance.PlayLoop(firstMiniBossIntro.roarInstance);
    while (firstMiniBossIntro.WaitingForAnimationToComplete)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
  }

  public void ConversationComplete() => this.WaitingForConversationToComplete = false;

  public void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.WaitingForAnimationToComplete = false;
  }
}
