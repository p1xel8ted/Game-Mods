// Decompiled with JetBrains decompiler
// Type: BasicBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class BasicBossIntro : BossIntro
{
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IdleAnimation;
  public string RoarSfx = "event:/enemy/patrol_boss/miniboss_intro_roar";
  public bool useCustomIntroSFX;
  public string customIntroSFX = "";
  public bool useRoarSpineEvent;
  public bool WaitingForAnimationToComplete = true;
  public BasicBossIntro.EnemyType typeEnemy;
  public EventInstance roarInstance;
  public TrackEntry introTrackEntry;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    BasicBossIntro basicBossIntro = this;
    basicBossIntro.WaitingForAnimationToComplete = true;
    while (PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.CustomAnimation);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(basicBossIntro.CameraTarget, 5f);
    yield return (object) new WaitForSeconds(0.5f);
    basicBossIntro.introTrackEntry = basicBossIntro.BossSpine.AnimationState.SetAnimation(0, basicBossIntro.IntroAnimation, false);
    basicBossIntro.introTrackEntry.Complete += new Spine.AnimationState.TrackEntryDelegate(basicBossIntro.AnimationState_Complete);
    basicBossIntro.BossSpine.AnimationState.AddAnimation(0, basicBossIntro.IdleAnimation, true, 0.0f);
    float num = basicBossIntro.introTrackEntry.AnimationEnd - basicBossIntro.introTrackEntry.AnimationStart;
    if (!basicBossIntro.useCustomIntroSFX)
    {
      if (basicBossIntro.RoarSfx != null)
      {
        Debug.Log((object) ("Using roar spine event? " + basicBossIntro.useRoarSpineEvent.ToString()));
        if (basicBossIntro.useRoarSpineEvent)
        {
          basicBossIntro.BossSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(basicBossIntro.HandleEvent);
        }
        else
        {
          basicBossIntro.PlaySound();
          CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
          MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        }
      }
    }
    else if (!string.IsNullOrEmpty(basicBossIntro.customIntroSFX))
      basicBossIntro.PlayCustomIntroSound(basicBossIntro.customIntroSFX);
    float forceTime = Time.time + num;
    float minTime = Time.time + 2f;
    while (basicBossIntro.WaitingForAnimationToComplete && (double) Time.time < (double) forceTime || (double) Time.time < (double) minTime)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    basicBossIntro.Callback?.Invoke();
  }

  public void PlaySound()
  {
    AudioManager.Instance.PlayOneShotAndSetParameterValue(this.RoarSfx, "roar_layers", (float) this.typeEnemy, this.BossSpine.transform);
  }

  public void PlayCustomIntroSound(string sfx)
  {
    AudioManager.Instance.PlayOneShotAndSetParameterValue(sfx, "roar_layers", (float) this.typeEnemy, this.BossSpine.transform);
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

  public void Play() => this.StartCoroutine(this.PlayRoutine(false));

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
