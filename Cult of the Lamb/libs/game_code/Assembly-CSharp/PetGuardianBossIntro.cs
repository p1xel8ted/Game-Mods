// Decompiled with JetBrains decompiler
// Type: PetGuardianBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class PetGuardianBossIntro : BossIntro
{
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IdleAnimation;
  public string IntroSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/intro_windup";
  public bool WaitingForAnimationToComplete = true;
  public TrackEntry introTrackEntry;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    PetGuardianBossIntro guardianBossIntro = this;
    guardianBossIntro.WaitingForAnimationToComplete = true;
    while (PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.CustomAnimation);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(guardianBossIntro.CameraTarget, 5f);
    yield return (object) new WaitForSeconds(0.5f);
    guardianBossIntro.introTrackEntry = guardianBossIntro.BossSpine.AnimationState.SetAnimation(0, guardianBossIntro.IntroAnimation, false);
    guardianBossIntro.introTrackEntry.Complete += new Spine.AnimationState.TrackEntryDelegate(guardianBossIntro.AnimationState_Complete);
    guardianBossIntro.BossSpine.AnimationState.AddAnimation(0, guardianBossIntro.IdleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(guardianBossIntro.IntroSFX))
      AudioManager.Instance.PlayOneShot(guardianBossIntro.IntroSFX);
    float forceTime = Time.time + (guardianBossIntro.introTrackEntry.AnimationEnd - guardianBossIntro.introTrackEntry.AnimationStart);
    float minTime = Time.time + 2f;
    while (guardianBossIntro.WaitingForAnimationToComplete && (double) Time.time < (double) forceTime || (double) Time.time < (double) minTime)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    guardianBossIntro.Callback?.Invoke();
  }

  public void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.WaitingForAnimationToComplete = false;
    this.introTrackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
  }
}
