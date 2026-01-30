// Decompiled with JetBrains decompiler
// Type: FollowerTask_Zombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class FollowerTask_Zombie : FollowerTask
{
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 20f;
  public float _gameTimeToNextStateUpdate;
  public EventInstance loopingSound;

  public override FollowerTaskType Type => FollowerTaskType.Zombie;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override int GetSubTaskCode() => 0;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._state == FollowerTaskState.Idle)
    {
      this._gameTimeToNextStateUpdate -= deltaGameTime;
      if ((double) this._gameTimeToNextStateUpdate <= 0.0)
      {
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        this._gameTimeToNextStateUpdate = Random.Range(10f, 20f);
      }
    }
    if (this.loopingSound.isValid())
    {
      int num = (int) this.loopingSound.setParameterByName("zombie_pitch", this.State == FollowerTaskState.GoingTo ? 1f : 0.0f);
    }
    if (PlayerFarming.Location == FollowerLocation.Base)
      return;
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Zombie/zombie-idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, (double) Random.value < 0.5 ? "Zombie/zombie-walk-limp" : "Zombie/zombie-walk");
    follower.SetOutfit(FollowerOutfitType.Old, false);
    this.loopingSound = AudioManager.Instance.CreateLoop("event:/dialogue/followers/zombie_fol/zombie_mumble", follower.gameObject);
    AudioManager.Instance.PlayLoop(this.loopingSound);
  }

  public override void CleanupSetup()
  {
    base.CleanupSetup();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomCircleFromTownCentre(16f);
  }

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override float SocialChange(float deltaGameTime) => 0.0f;

  public override float VomitChange(float deltaGameTime) => 0.0f;
}
