// Decompiled with JetBrains decompiler
// Type: BasicBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private string RoarSfx = "event:/enemy/patrol_boss/miniboss_intro_roar";
  public bool useRoarSpineEvent;
  private bool WaitingForAnimationToComplete = true;
  public BasicBossIntro.EnemyType typeEnemy;
  private EventInstance roarInstance;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    BasicBossIntro basicBossIntro = this;
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(basicBossIntro.CameraTarget, 5f);
    yield return (object) new WaitForSeconds(0.5f);
    basicBossIntro.BossSpine.AnimationState.SetAnimation(0, basicBossIntro.IntroAnimation, false);
    basicBossIntro.BossSpine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(basicBossIntro.AnimationState_Complete);
    basicBossIntro.BossSpine.AnimationState.AddAnimation(0, basicBossIntro.IdleAnimation, true, 0.0f);
    if (basicBossIntro.RoarSfx != null)
    {
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
    while (basicBossIntro.WaitingForAnimationToComplete)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  private void PlaySound()
  {
    this.roarInstance = AudioManager.Instance.CreateLoop(this.RoarSfx, this.BossSpine.gameObject);
    int num = (int) this.roarInstance.setParameterByName("roar_layers", (float) this.typeEnemy);
    AudioManager.Instance.PlayLoop(this.roarInstance);
  }

  private void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.WaitingForAnimationToComplete = false;
    if (!this.useRoarSpineEvent)
      return;
    this.BossSpine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "roar"))
      return;
    this.PlaySound();
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
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
