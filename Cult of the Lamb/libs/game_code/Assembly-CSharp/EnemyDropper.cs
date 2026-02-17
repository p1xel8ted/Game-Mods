// Decompiled with JetBrains decompiler
// Type: EnemyDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyDropper : MonoBehaviour
{
  public GameObject sprite;
  public Transform spriteRaisedHeight;
  public Transform spriteLoweredHeight;
  public Transform targetReticle;
  public Transform damageCollider;
  public EnemyDropper.DropperState dropperState;
  public Bouncer bouncer;
  public float distanceToAlert = 5f;
  public float floorTimeBase = 1f;
  public float floorTimeRange = 1f;
  public float liftTime = 0.5f;
  public float pauseTime = 1f;
  public float scanTimeBase = 2f;
  public float scanTimeRange = 2f;
  public float lockTime = 1f;
  public float dropTime = 0.5f;
  public float scanSpeed = 3f;
  public bool quickLiftOnFloorTouch = true;
  public float scanTime;
  public float floorTime;
  public Vector3 dropperShadowMinScale;
  public Vector3 dropperShadowMaxScale;
  public Transform shadowTransform;
  public ParticleSystem AOEParticles;
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string fallingAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string landAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string prepareAnimation;
  public float timePassed;
  public Vector3 pointToRiseOnComplete;
  public EventInstance loopedSound;
  public bool foundPlayer;
  public bool playedGoUpSfx;
  public bool playedGoDownSfx;

  public void Start()
  {
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.targetReticle.gameObject.SetActive(false);
    this.damageCollider.gameObject.SetActive(false);
    this.shadowTransform.localScale = this.dropperShadowMinScale;
    this.SetDropperState(EnemyDropper.DropperState.asleep);
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.ChestAppeared);
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.SpineEventHandler);
  }

  public void OnEnable()
  {
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.SpineEventHandler);
    this.StartCoroutine((IEnumerator) this.WaitForPlayerLoop());
  }

  public void SpineEventHandler(TrackEntry track, Spine.Event e)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/stomp_head/stomphead_close_teeth", this.gameObject);
  }

  public IEnumerator WaitForPlayerLoop()
  {
    EnemyDropper enemyDropper = this;
    while ((Object) PlayerFarming.Instance == (Object) null)
      yield return (object) null;
    enemyDropper.loopedSound = AudioManager.Instance.CreateLoop("event:/enemy/stomp_head/stomphead_loop", enemyDropper.gameObject, true);
    enemyDropper.foundPlayer = true;
  }

  public void FixedUpdate()
  {
    if (PlayerRelic.TimeFrozen || Health.isGlobalTimeFreeze)
      return;
    this.timePassed += Time.deltaTime;
    switch (this.dropperState)
    {
      case EnemyDropper.DropperState.asleep:
        this.UpdateAsleep();
        break;
      case EnemyDropper.DropperState.onFloor:
        this.UpdateOnFloor();
        break;
      case EnemyDropper.DropperState.lifting:
        this.UpdateLifting();
        break;
      case EnemyDropper.DropperState.pauseBeforeScan:
        this.UpdatePauseBeforeScan();
        break;
      case EnemyDropper.DropperState.scanning:
        this.UpdateScanning();
        break;
      case EnemyDropper.DropperState.locked:
        this.UpdateLocked();
        break;
      case EnemyDropper.DropperState.dropping:
        this.UpdateDropping();
        break;
      case EnemyDropper.DropperState.roomComplete:
        this.UpdateRoomComplete();
        break;
    }
  }

  public void SetDropperState(EnemyDropper.DropperState targetState)
  {
    if (targetState == EnemyDropper.DropperState.scanning)
    {
      AudioManager.Instance.PlayLoop(this.loopedSound);
    }
    else
    {
      int num = (int) this.loopedSound.stop(STOP_MODE.ALLOWFADEOUT);
    }
    this.timePassed = 0.0f;
    this.dropperState = targetState;
    this.bouncer.gameObject.SetActive(targetState == EnemyDropper.DropperState.onFloor || targetState == EnemyDropper.DropperState.asleep || targetState == EnemyDropper.DropperState.none);
  }

  public void UpdateAsleep(bool causeToWake = false)
  {
    if ((Object) PlayerFarming.Instance == (Object) null || !((double) PlayerFarming.GetClosestPlayerDist(this.transform.position) < (double) this.distanceToAlert | causeToWake))
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/stomp_head/stomphead_wakeup", this.gameObject);
    this.SetDropperState(EnemyDropper.DropperState.none);
    this.Spine.AnimationState.SetAnimation(0, this.prepareAnimation, true);
    this.transform.DOShakeScale(1f, 0.5f).OnComplete<Tweener>(new TweenCallback(this.WakeUp));
  }

  public void WakeUp()
  {
    this.ResetScale();
    this.SetDropperState(EnemyDropper.DropperState.lifting);
  }

  public void UpdateOnFloor()
  {
    if ((double) this.timePassed <= (double) this.floorTime)
      return;
    this.SetDropperState(EnemyDropper.DropperState.lifting);
  }

  public void UpdateLifting()
  {
    if (!this.playedGoUpSfx)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/stomp_head/stomphead_go_up", this.gameObject);
      this.playedGoUpSfx = true;
    }
    float t = 1f / this.liftTime * this.timePassed;
    this.sprite.transform.position = Vector3.Lerp(this.spriteLoweredHeight.transform.position, this.spriteRaisedHeight.transform.position, t);
    this.shadowTransform.localScale = Vector3.Lerp(this.dropperShadowMinScale, this.dropperShadowMaxScale, t);
    this.Spine.AnimationState.SetAnimation(0, this.fallingAnimation, true);
    if ((double) this.timePassed <= (double) this.liftTime)
      return;
    this.playedGoUpSfx = false;
    this.SetDropperState(EnemyDropper.DropperState.pauseBeforeScan);
  }

  public void UpdatePauseBeforeScan()
  {
    if ((double) this.timePassed <= (double) this.pauseTime)
      return;
    this.scanTime = this.scanTimeBase + this.scanTimeRange * Random.value;
    this.SetDropperState(EnemyDropper.DropperState.scanning);
  }

  public void UpdateScanning()
  {
    this.transform.position = Vector3.Lerp(this.transform.position, PlayerFarming.FindClosestPlayer(this.transform.position).transform.position, Time.deltaTime * this.scanSpeed);
    if ((double) this.timePassed <= (double) this.scanTime)
      return;
    this.PlayTargetReticle();
    this.SetDropperState(EnemyDropper.DropperState.locked);
  }

  public void PlayTargetReticle()
  {
    this.targetReticle.gameObject.SetActive(true);
    this.targetReticle.localScale = Vector3.zero;
    Quaternion rotation = this.targetReticle.rotation;
    this.targetReticle.Rotate(0.0f, 0.0f, -180f);
    float delay = this.lockTime / 6f;
    this.targetReticle.DOScale(2.5f, delay * 2f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay);
    this.targetReticle.DORotateQuaternion(rotation, delay * 2f).SetDelay<TweenerCore<Quaternion, Quaternion, NoOptions>>(delay);
  }

  public void HideTargetReticle()
  {
    this.targetReticle.DOKill();
    this.targetReticle.DOScale(0.0f, this.dropTime / 4f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(this.EndTargetReticleAnimation));
  }

  public void EndTargetReticleAnimation() => this.targetReticle.gameObject.SetActive(false);

  public void UpdateLocked()
  {
    if (!this.playedGoDownSfx)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/stomp_head/stomphead_go_down", this.gameObject);
      this.playedGoDownSfx = true;
    }
    if ((double) this.timePassed <= (double) this.lockTime)
      return;
    this.playedGoDownSfx = false;
    this.HideTargetReticle();
    this.SetDropperState(EnemyDropper.DropperState.dropping);
  }

  public void UpdateDropping()
  {
    float t = 1f / this.dropTime * this.timePassed;
    this.sprite.transform.position = Vector3.Lerp(this.spriteRaisedHeight.transform.position, this.spriteLoweredHeight.transform.position, t);
    this.shadowTransform.localScale = Vector3.Lerp(this.dropperShadowMaxScale, this.dropperShadowMinScale, t);
    this.Spine.AnimationState.SetAnimation(0, this.fallingAnimation, true);
    if ((double) this.timePassed < (double) this.dropTime)
      return;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/land", this.gameObject);
    this.Spine.AnimationState.SetAnimation(0, this.landAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    CameraManager.shakeCamera(2f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    this.AOEParticles.Play();
    this.ShowDamageCollider();
    this.floorTime = this.floorTimeBase + this.floorTimeRange * Random.value;
    this.SetDropperState(EnemyDropper.DropperState.onFloor);
  }

  public void ShowDamageCollider()
  {
    this.damageCollider.gameObject.SetActive(true);
    this.damageCollider.localScale = Vector3.zero;
    this.damageCollider.DOScale(1f, 0.2f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(this.RemoveDamageCollider));
  }

  public void RemoveDamageCollider() => this.damageCollider.gameObject.SetActive(false);

  public void ChestAppeared()
  {
    this.pointToRiseOnComplete = new Vector3(this.spriteRaisedHeight.position.x, this.spriteRaisedHeight.position.y, this.spriteRaisedHeight.position.z * 2f);
    this.SetDropperState(EnemyDropper.DropperState.roomComplete);
    Object.Destroy((Object) this.gameObject, 2f);
  }

  public void UpdateRoomComplete()
  {
    this.sprite.transform.position = Vector3.Lerp(this.sprite.transform.position, this.pointToRiseOnComplete, Time.deltaTime);
    this.sprite.transform.localScale = Vector3.Lerp(this.sprite.transform.localScale, Vector3.zero, Time.deltaTime);
    this.shadowTransform.localScale = Vector3.Lerp(this.shadowTransform.localScale, Vector3.zero, Time.deltaTime * 3f);
    this.HideTargetReticle();
  }

  public void OnDestroy()
  {
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.ChestAppeared);
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public void BounceUnit()
  {
    if (this.quickLiftOnFloorTouch && this.dropperState == EnemyDropper.DropperState.onFloor && (double) this.timePassed > (double) this.floorTime / 2.0)
      this.SetDropperState(EnemyDropper.DropperState.lifting);
    if (this.dropperState == EnemyDropper.DropperState.asleep)
      this.UpdateAsleep(true);
    this.transform.DOShakeScale(0.5f, 0.5f).OnComplete<Tweener>(new TweenCallback(this.ResetScale));
  }

  public void ResetScale() => this.transform.localScale = Vector3.one;

  public enum DropperState
  {
    none,
    asleep,
    onFloor,
    lifting,
    pauseBeforeScan,
    scanning,
    locked,
    dropping,
    roomComplete,
  }
}
