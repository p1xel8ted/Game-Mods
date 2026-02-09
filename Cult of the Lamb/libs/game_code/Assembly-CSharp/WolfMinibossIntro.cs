// Decompiled with JetBrains decompiler
// Type: WolfMinibossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WolfMinibossIntro : BossIntro
{
  [SerializeField]
  public GameObject follower;
  [SerializeField]
  public GameObject leader;
  [SerializeField]
  public GameObject bossPrefab;
  [SerializeField]
  public WolfCage cage;
  [TermsPopup("")]
  [SerializeField]
  public string bossName;
  [SerializeField]
  public SkeletonAnimation followerSpine;
  [SpineAnimation("", "", true, false, dataField = "followerSpine")]
  [SerializeField]
  public string followerIntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "followerSpine")]
  [SerializeField]
  public string followerMutationAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  [SerializeField]
  public string bossIntroAnimation;
  [SpineEvent("", "", true, false, false, dataField = "BossSpine")]
  [SerializeField]
  public string bossChompEvent;
  [SerializeField]
  public Interaction_SimpleConversation introConversation;
  [SerializeField]
  public Interaction_SimpleConversation introConversationFollower;
  [SerializeField]
  public Interaction_SimpleConversation followerMutationConversation;
  [SerializeField]
  public GameObject goopFloorParticle;
  [SerializeField]
  public LightingManagerVolume lightOverride;
  [SerializeField]
  public string MarchosiasEntersSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/intro_marchosias_enter";
  [SerializeField]
  public string MarchosiasAmbientLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/intro_marchosias_loop";
  [SerializeField]
  public string MarchosiasExitsSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/intro_marchosias_exit";
  [SerializeField]
  public string DogBreaksFreeSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/intro_break_free_start";
  public EventInstance marchosiasAmbientLoopInstance;
  public SkeletonAnimation _goopSkeletonAnimation;
  public SkeletonAnimation _leaderSpine;
  public SkeletonAnimation _spawnEffectSpine;
  public SkeletonAnimation _minibossSpine;
  public BossIntro _minibossIntro;
  public EnemyDogMiniboss _minibossAI;
  public Health _minibossHealth;
  public Health _followerHealth;
  public StateMachine _followerStateMachine;

  public Vector3 playerPos => this.transform.position + new Vector3(0.0f, -2.5f);

  public void Awake()
  {
    this._goopSkeletonAnimation = this.goopFloorParticle.GetComponent<SkeletonAnimation>();
    this._leaderSpine = this.leader.GetComponentInChildren<SkeletonAnimation>();
    this._minibossIntro = this.bossPrefab.GetComponent<BossIntro>();
    this._minibossSpine = this.bossPrefab.GetComponentInChildren<SkeletonAnimation>();
    this._minibossAI = this.bossPrefab.GetComponent<EnemyDogMiniboss>();
    this._minibossHealth = this.bossPrefab.GetComponent<Health>();
    this._followerStateMachine = this.follower.GetComponent<StateMachine>();
    this._followerHealth = this.follower.GetComponent<Health>();
    this._minibossAI.enabled = false;
    this._minibossHealth.untouchable = true;
    this._followerHealth.untouchable = true;
    this._minibossSpine.gameObject.SetActive(false);
  }

  public new void Start() => this.SetFollowerFace(this.playerPos);

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    WolfMinibossIntro wolfMinibossIntro = this;
    if (DataManager.Instance.BeatenYngya)
    {
      wolfMinibossIntro.leader.gameObject.SetActive(false);
      wolfMinibossIntro.follower.gameObject.SetActive(false);
      yield return (object) wolfMinibossIntro.WaitForPlayersToBeCloseBy();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(wolfMinibossIntro.leader, 12f);
      wolfMinibossIntro.RepositionPlayers();
      yield return (object) wolfMinibossIntro.cage.ExplodeRoutine(true);
      CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
      AudioManager.Instance.PlayOneShot(" event:/enemy/vocals/worm_large/warning");
      yield return (object) wolfMinibossIntro.StartFight();
    }
    else
    {
      yield return (object) wolfMinibossIntro.WaitForPlayersToBeCloseBy();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(wolfMinibossIntro.leader, 12f);
      wolfMinibossIntro.RepositionPlayers();
      wolfMinibossIntro.StartCoroutine((IEnumerator) wolfMinibossIntro.FollowerFaceSlowly(wolfMinibossIntro.leader.transform.position));
      yield return (object) wolfMinibossIntro.SpawnLeader();
      yield return (object) wolfMinibossIntro.PlayIntroConversation();
      yield return (object) wolfMinibossIntro.PlayIntroConversationFollower();
      yield return (object) wolfMinibossIntro.DespawnLeader();
      wolfMinibossIntro.StartCoroutine((IEnumerator) wolfMinibossIntro.FollowerFaceSlowly(wolfMinibossIntro.playerPos));
      yield return (object) wolfMinibossIntro.PlayMutationConversation();
      wolfMinibossIntro.StartCoroutine((IEnumerator) wolfMinibossIntro.FollowerFaceSlowly(wolfMinibossIntro.cage.transform.position));
      yield return (object) wolfMinibossIntro.SpawnBoss();
    }
    GameManager.GetInstance().OnConversationEnd();
    wolfMinibossIntro.Callback.Invoke();
    yield return (object) null;
  }

  public void DimLighting() => this.lightOverride.gameObject.SetActive(true);

  public void BrightenLighting() => this.lightOverride.gameObject.SetActive(false);

  public IEnumerator FollowerFaceSlowly(Vector3 target, float speed = 2f)
  {
    StateMachine followerStateMachine = this.follower.GetComponent<StateMachine>();
    float currentAngle = followerStateMachine.LookAngle;
    float targetAngle = Utils.GetAngle(this.follower.transform.position, target);
    while ((double) Mathf.Abs(currentAngle - targetAngle) > 0.10000000149011612)
    {
      currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed);
      followerStateMachine.LookAngle = currentAngle;
      followerStateMachine.facingAngle = currentAngle;
      yield return (object) null;
    }
  }

  public void SetFollowerFace(Vector3 target)
  {
    StateMachine component = this.follower.GetComponent<StateMachine>();
    float angle = Utils.GetAngle(this.follower.transform.position, target);
    component.LookAngle = angle;
    component.facingAngle = angle;
  }

  public IEnumerator SpawnLeader()
  {
    this.DimLighting();
    yield return (object) new WaitForSeconds(0.5f);
    this.ShowGoop();
    yield return (object) new WaitForSeconds(1.5f);
    this.leader.SetActive(true);
    this._leaderSpine.transform.parent.gameObject.SetActive(true);
    this._leaderSpine.AnimationState.SetAnimation(0, "enter", false);
    bool isLeaderSpawnAnimationComplete = false;
    this._leaderSpine.AnimationState.Complete += (Spine.AnimationState.TrackEntryDelegate) (t => isLeaderSpawnAnimationComplete = true);
    yield return (object) new WaitUntil((Func<bool>) (() => isLeaderSpawnAnimationComplete));
  }

  public IEnumerator DespawnLeader()
  {
    this.PlayLeaderDespawnAnimation();
    yield return (object) new WaitForSeconds(2f);
    if (!string.IsNullOrEmpty(this.MarchosiasAmbientLoopSFX))
      AudioManager.Instance.StopLoop(this.marchosiasAmbientLoopInstance);
    this.HideGoop();
  }

  public IEnumerator SpawnBoss()
  {
    GameManager.GetInstance().OnConversationNext(this.cage.gameObject, 6f);
    this.followerSpine.AnimationState.SetAnimation(0, this.followerMutationAnimation, true);
    yield return (object) new WaitForSeconds(1f);
    this.cage.StopAllCoroutines();
    Vector3 pos = this.cage.transform.position;
    if (!string.IsNullOrEmpty(this.DogBreaksFreeSFX))
      AudioManager.Instance.PlayOneShot(this.DogBreaksFreeSFX);
    yield return (object) this.cage.ExplodeRoutine();
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    AudioManager.Instance.PlayOneShot(" event:/enemy/vocals/worm_large/warning", pos);
    Vector3 vector3 = new Vector3(pos.x, pos.y, pos.z - 1f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(vector3);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(vector3, Vector3.one * 5f);
    RumbleManager.Instance.Rumble();
    yield return (object) this.StartFight();
  }

  public IEnumerator StartFight()
  {
    WolfMinibossIntro wolfMinibossIntro = this;
    wolfMinibossIntro.bossPrefab.SetActive(true);
    wolfMinibossIntro._minibossSpine.gameObject.SetActive(true);
    wolfMinibossIntro.bossPrefab.transform.position = Vector3.zero;
    wolfMinibossIntro.bossPrefab.transform.DOScale(Vector3.one, 0.5f).From<Vector3, Vector3, VectorOptions>(Vector3.one * 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(wolfMinibossIntro.CameraTarget, 5f);
    HUD_DisplayName.Play(wolfMinibossIntro.bossName, 2, HUD_DisplayName.Positions.Centre);
    yield return (object) new WaitForSeconds(0.25f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    wolfMinibossIntro._minibossSpine.gameObject.SetActive(true);
    if (!DataManager.Instance.BeatenYngya)
    {
      wolfMinibossIntro.BossSpine.AnimationState.SetAnimation(0, wolfMinibossIntro.bossIntroAnimation, false);
      wolfMinibossIntro.followerSpine.AnimationState.SetAnimation(0, wolfMinibossIntro.followerIntroAnimation, false);
      bool isBossIntroAnimationFinished = false;
      wolfMinibossIntro.BossSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(wolfMinibossIntro.OnBossAnimationEvent);
      wolfMinibossIntro.BossSpine.AnimationState.Complete += (Spine.AnimationState.TrackEntryDelegate) (t => isBossIntroAnimationFinished = true);
      yield return (object) new WaitUntil((Func<bool>) (() => isBossIntroAnimationFinished));
    }
    else
      yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
    wolfMinibossIntro._minibossAI.enabled = true;
    wolfMinibossIntro._minibossHealth.untouchable = false;
  }

  public void OnBossAnimationEvent(TrackEntry trackentry, Spine.Event e)
  {
    if (!(e.Data.Name == this.bossChompEvent) || !((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this._followerHealth.untouchable = false;
    this._followerHealth.DealDamage(999f, this.bossPrefab, this.bossPrefab.transform.position);
  }

  public void RepositionPlayers()
  {
    PlayerFarming.Instance.GoToAndStop(this.playerPos, this.gameObject);
  }

  public IEnumerator WaitForPlayersToBeCloseBy()
  {
    WolfMinibossIntro wolfMinibossIntro = this;
    while ((double) PlayerFarming.GetClosestPlayerDist(wolfMinibossIntro.transform.position) > 9.0)
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        PlayerFarming player = PlayerFarming.players[index];
        if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
          PlayerFarming.SetStateForAllPlayers();
      }
      wolfMinibossIntro.transform.position = Vector3.zero;
      yield return (object) null;
    }
  }

  public void PlayLeaderDespawnAnimation()
  {
    this._leaderSpine.AnimationState.SetAnimation(0, "exit2", false);
    if (string.IsNullOrEmpty(this.MarchosiasExitsSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.MarchosiasExitsSFX, this.goopFloorParticle.transform.position);
  }

  public IEnumerator PlayMutationConversation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    WolfMinibossIntro wolfMinibossIntro = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    wolfMinibossIntro.followerMutationConversation.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(wolfMinibossIntro.\u003CPlayMutationConversation\u003Eb__46_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PlayIntroConversation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    WolfMinibossIntro wolfMinibossIntro = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    wolfMinibossIntro.introConversation.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(wolfMinibossIntro.\u003CPlayIntroConversation\u003Eb__47_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PlayIntroConversationFollower()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    WolfMinibossIntro wolfMinibossIntro = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    wolfMinibossIntro.introConversationFollower.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(wolfMinibossIntro.\u003CPlayIntroConversationFollower\u003Eb__48_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void HideGoop()
  {
    this._goopSkeletonAnimation.AnimationState.SetAnimation(0, "leader-stop", false);
    this.BrightenLighting();
  }

  public void ShowGoop()
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    this.goopFloorParticle.gameObject.SetActive(true);
    this._goopSkeletonAnimation.AnimationState.AddAnimation(0, "leader-loop", true, 0.0f);
    if (!string.IsNullOrEmpty(this.MarchosiasEntersSFX))
      AudioManager.Instance.PlayOneShot(this.MarchosiasEntersSFX);
    if (!string.IsNullOrEmpty(this.MarchosiasAmbientLoopSFX))
      this.marchosiasAmbientLoopInstance = AudioManager.Instance.CreateLoop(this.MarchosiasAmbientLoopSFX, this.leader, true);
    this.DimLighting();
  }

  [CompilerGenerated]
  public bool \u003CPlayMutationConversation\u003Eb__46_0()
  {
    return this.followerMutationConversation.Finished;
  }

  [CompilerGenerated]
  public bool \u003CPlayIntroConversation\u003Eb__47_0() => this.introConversation.Finished;

  [CompilerGenerated]
  public bool \u003CPlayIntroConversationFollower\u003Eb__48_0()
  {
    return this.introConversationFollower.Finished;
  }
}
