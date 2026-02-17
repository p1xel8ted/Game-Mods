// Decompiled with JetBrains decompiler
// Type: BasicBossIntroMouthMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BasicBossIntroMouthMiniboss : BossIntro
{
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IdleAnimation;
  [EventRef]
  public string BassIntroBigChompSFX = "event:/dlc/dungeon06/enemy/miniboss_hole/intro_wakeup";
  public string RoarSfx = "event:/enemy/patrol_boss/miniboss_intro_roar";
  public bool useRoarSpineEvent;
  public bool WaitingForAnimationToComplete = true;
  public BasicBossIntroMouthMiniboss.EnemyType typeEnemy;
  public EventInstance roarInstance;
  public TrackEntry introTrackEntry;
  public EnemyFleshSwarmer triggerEnemy0;
  public EnemyFleshSwarmer triggerEnemy1;
  public EnemyFleshSwarmer triggerEnemy2;
  public EnemyFleshSwarmer triggerEnemy3;
  public EnemyHole enemyHole;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    BasicBossIntroMouthMiniboss introMouthMiniboss = this;
    introMouthMiniboss.WaitingForAnimationToComplete = true;
    while (PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.CustomAnimation);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(introMouthMiniboss.CameraTarget, 7f);
    yield return (object) new WaitForSeconds(0.1f);
    introMouthMiniboss.triggerEnemy0.gameObject.SetActive(true);
    Vector3 localPosition1 = introMouthMiniboss.triggerEnemy0.Spine.transform.localPosition with
    {
      z = 0.0f
    };
    Vector3 vector3_1 = localPosition1 with { z = -10f };
    introMouthMiniboss.triggerEnemy0.Spine.transform.localPosition = vector3_1;
    introMouthMiniboss.triggerEnemy0.Spine.AnimationState.SetAnimation(0, introMouthMiniboss.triggerEnemy0.IdleAnimation, true);
    introMouthMiniboss.triggerEnemy0.Spine.Initialize(false);
    introMouthMiniboss.triggerEnemy0.Spine.Update(0.0f);
    DispatchThread.Instance?.Dequeue();
    introMouthMiniboss.triggerEnemy0.Spine.BaseLateUpdate();
    introMouthMiniboss.triggerEnemy0.Spine.UpdateInterval = 1;
    introMouthMiniboss.triggerEnemy0.Spine.transform.DOLocalMove(localPosition1, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(introMouthMiniboss.\u003CPlayRoutine\u003Eb__10_0));
    yield return (object) new WaitForSeconds(0.75f);
    introMouthMiniboss.triggerEnemy1.gameObject.SetActive(true);
    Vector3 localPosition2 = introMouthMiniboss.triggerEnemy1.Spine.transform.localPosition with
    {
      z = 0.0f
    };
    Vector3 vector3_2 = localPosition2 with { z = -10f };
    introMouthMiniboss.triggerEnemy1.Spine.transform.localPosition = vector3_2;
    introMouthMiniboss.triggerEnemy1.Spine.AnimationState.SetAnimation(0, introMouthMiniboss.triggerEnemy1.IdleAnimation, true);
    introMouthMiniboss.triggerEnemy1.Spine.Initialize(false);
    introMouthMiniboss.triggerEnemy1.Spine.Update(0.0f);
    DispatchThread.Instance?.Dequeue();
    introMouthMiniboss.triggerEnemy1.Spine.BaseLateUpdate();
    introMouthMiniboss.triggerEnemy1.Spine.UpdateInterval = 1;
    introMouthMiniboss.triggerEnemy1.Spine.transform.DOLocalMove(localPosition2, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(introMouthMiniboss.\u003CPlayRoutine\u003Eb__10_1));
    yield return (object) new WaitForSeconds(0.75f);
    introMouthMiniboss.triggerEnemy2.gameObject.SetActive(true);
    Vector3 localPosition3 = introMouthMiniboss.triggerEnemy2.Spine.transform.localPosition with
    {
      z = 0.0f
    };
    Vector3 vector3_3 = localPosition3 with { z = -10f };
    introMouthMiniboss.triggerEnemy2.Spine.transform.localPosition = vector3_3;
    introMouthMiniboss.triggerEnemy2.Spine.AnimationState.SetAnimation(0, introMouthMiniboss.triggerEnemy2.IdleAnimation, true);
    introMouthMiniboss.triggerEnemy2.Spine.Initialize(false);
    introMouthMiniboss.triggerEnemy2.Spine.Update(0.0f);
    DispatchThread.Instance?.Dequeue();
    introMouthMiniboss.triggerEnemy2.Spine.BaseLateUpdate();
    introMouthMiniboss.triggerEnemy2.Spine.UpdateInterval = 1;
    introMouthMiniboss.triggerEnemy2.Spine.transform.DOLocalMove(localPosition3, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(introMouthMiniboss.\u003CPlayRoutine\u003Eb__10_2));
    yield return (object) new WaitForSeconds(0.75f);
    introMouthMiniboss.triggerEnemy3.transform.position = introMouthMiniboss.enemyHole.transform.position;
    introMouthMiniboss.triggerEnemy3.gameObject.SetActive(true);
    Vector3 localPosition4 = introMouthMiniboss.triggerEnemy3.Spine.transform.localPosition with
    {
      z = 0.0f
    };
    Vector3 vector3_4 = localPosition4 with { z = -10f };
    introMouthMiniboss.triggerEnemy3.Spine.transform.localPosition = vector3_4;
    introMouthMiniboss.triggerEnemy3.Spine.AnimationState.SetAnimation(0, introMouthMiniboss.triggerEnemy3.IdleAnimation, true);
    introMouthMiniboss.triggerEnemy3.Spine.Initialize(false);
    introMouthMiniboss.triggerEnemy3.Spine.Update(0.0f);
    DispatchThread.Instance?.Dequeue();
    introMouthMiniboss.triggerEnemy3.Spine.BaseLateUpdate();
    introMouthMiniboss.triggerEnemy3.Spine.UpdateInterval = 1;
    introMouthMiniboss.triggerEnemy3.Spine.transform.DOLocalMove(localPosition4, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(UpdateType.Late).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(introMouthMiniboss.\u003CPlayRoutine\u003Eb__10_3));
    yield return (object) new WaitForSeconds(0.75f);
    GameManager.GetInstance().OnConversationNext(introMouthMiniboss.CameraTarget, 7f);
    introMouthMiniboss.StartCoroutine((IEnumerator) introMouthMiniboss.enemyHole.TriggerSurpriseAttack(introMouthMiniboss.BassIntroBigChompSFX));
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.shakeCamera(3f);
    Explosion.CreateExplosion(introMouthMiniboss.triggerEnemy0.transform.position, introMouthMiniboss.triggerEnemy0.health.team, introMouthMiniboss.triggerEnemy0.health, 1f);
    Explosion.CreateExplosion(introMouthMiniboss.triggerEnemy1.transform.position, introMouthMiniboss.triggerEnemy1.health.team, introMouthMiniboss.triggerEnemy1.health, 1f);
    Explosion.CreateExplosion(introMouthMiniboss.triggerEnemy2.transform.position, introMouthMiniboss.triggerEnemy2.health.team, introMouthMiniboss.triggerEnemy2.health, 1f);
    Explosion.CreateExplosion(introMouthMiniboss.triggerEnemy3.transform.position, introMouthMiniboss.triggerEnemy3.health.team, introMouthMiniboss.triggerEnemy3.health, 1f);
    Object.Destroy((Object) introMouthMiniboss.triggerEnemy0.gameObject);
    Object.Destroy((Object) introMouthMiniboss.triggerEnemy1.gameObject);
    Object.Destroy((Object) introMouthMiniboss.triggerEnemy2.gameObject);
    Object.Destroy((Object) introMouthMiniboss.triggerEnemy3.gameObject);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    introMouthMiniboss.Callback?.Invoke();
  }

  public void PlaySound()
  {
    AudioManager.Instance.PlayOneShotAndSetParameterValue(this.RoarSfx, "roar_layers", (float) this.typeEnemy, this.BossSpine.transform);
  }

  public void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.WaitingForAnimationToComplete = false;
    if (this.useRoarSpineEvent)
      this.BossSpine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.introTrackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "roar"))
      return;
    this.PlaySound();
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.PlayRoutine(false));

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__10_0()
  {
    AudioManager.Instance.PlayOneShot(this.triggerEnemy0.OnFallSFX, this.triggerEnemy0.Spine.transform.gameObject);
    this.triggerEnemy0.Spine.AnimationState.SetAnimation(0, this.triggerEnemy0.LandAnimation, false);
    this.triggerEnemy0.Spine.AnimationState.AddAnimation(0, this.triggerEnemy0.IdleAnimation, true, 0.0f);
  }

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__10_1()
  {
    AudioManager.Instance.PlayOneShot(this.triggerEnemy1.OnFallSFX, this.triggerEnemy1.Spine.transform.gameObject);
    this.triggerEnemy1.Spine.AnimationState.SetAnimation(0, this.triggerEnemy1.LandAnimation, false);
    this.triggerEnemy1.Spine.AnimationState.AddAnimation(0, this.triggerEnemy1.IdleAnimation, true, 0.0f);
  }

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__10_2()
  {
    AudioManager.Instance.PlayOneShot(this.triggerEnemy2.OnFallSFX, this.triggerEnemy2.Spine.transform.gameObject);
    this.triggerEnemy2.Spine.AnimationState.SetAnimation(0, this.triggerEnemy2.LandAnimation, false);
    this.triggerEnemy2.Spine.AnimationState.AddAnimation(0, this.triggerEnemy2.IdleAnimation, true, 0.0f);
  }

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__10_3()
  {
    AudioManager.Instance.PlayOneShot(this.triggerEnemy3.OnFallSFX, this.triggerEnemy3.Spine.transform.gameObject);
    this.triggerEnemy3.Spine.AnimationState.SetAnimation(0, this.triggerEnemy3.LandAnimation, false);
    this.triggerEnemy3.Spine.AnimationState.AddAnimation(0, this.triggerEnemy3.IdleAnimation, true, 0.0f);
  }

  public enum EnemyType
  {
    NoLayer,
    Worm,
    Frog,
    ScreechBat,
    JellyFish,
    Spider,
  }
}
