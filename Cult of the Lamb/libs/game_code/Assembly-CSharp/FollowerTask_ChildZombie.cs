// Decompiled with JetBrains decompiler
// Type: FollowerTask_ChildZombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ChildZombie : FollowerTask
{
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 20f;
  public float _gameTimeToNextStateUpdate;
  public float tryDoSomethingProgress;
  public int age;
  public bool busy;
  public Follower follower;
  public StructureBrain daycare;
  public EventInstance loopingSound;

  public override FollowerTaskType Type => FollowerTaskType.ChildZombie;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockThoughts => true;

  public override bool BlockTaskChanges => this.daycare != null;

  public FollowerTask_ChildZombie(int followerID) => this.AssignDaycare(followerID);

  public FollowerTask_ChildZombie(StructureBrain daycare) => this.daycare = daycare;

  public override int GetSubTaskCode() => 0;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public void AssignDaycare(int followerID)
  {
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.DAYCARE))
    {
      if (structureBrain.Data.MultipleFollowerIDs.Contains(followerID))
        this.daycare = structureBrain;
    }
    if (this.daycare == null || !((UnityEngine.Object) FollowerManager.FindFollowerByID(followerID) != (UnityEngine.Object) null))
      return;
    FollowerManager.FindFollowerByID(followerID).transform.position = this.daycare.Data.Position;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location != FollowerLocation.Base)
      AudioManager.Instance.StopLoop(this.loopingSound);
    if (this.busy)
      return;
    if (this.loopingSound.isValid())
    {
      int num = (int) this.loopingSound.setParameterByName("zombie_pitch", this.State == FollowerTaskState.GoingTo ? 1f : 0.0f);
    }
    this.tryDoSomethingProgress += deltaGameTime;
    if (this.Brain.Location == FollowerLocation.Base && DataManager.Instance.CurrentOnboardingFollowerID == this.Brain.Info.ID && !UIDrumMinigameOverlayController.IsPlaying && this.daycare == null)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (this.Brain.HasTrait(FollowerTrait.TraitType.Scared) || this.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred)) && (double) Vector3.Distance(this.Brain.LastPosition, PlayerFarming.Instance.transform.position) < 2.0)
      {
        this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_RunAwayFromLamb());
        return;
      }
      if (DataManager.Instance.CurrentOnboardingFollowerID == this.Brain.Info.ID && !UIDrumMinigameOverlayController.IsPlaying)
      {
        this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveOnboarding, false));
        return;
      }
    }
    if ((double) this.tryDoSomethingProgress > 35.0)
    {
      if (this.daycare == null)
        this.AssignDaycare(this._brain.Info.ID);
      this.tryDoSomethingProgress = (float) UnityEngine.Random.Range(-5, 5);
      if (UnityEngine.Random.Range(0, 100) < 50)
      {
        if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && this.follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving).Animation.Animation.Name == "Baby/Baby-zombie/baby-crawl-zombie")
          this.TryGetUp();
        else
          this.FallDown();
      }
      else if (this.daycare != null)
        this.daycare.DepositItem(InventoryItem.ITEM_TYPE.POOP);
      else
        this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_InstantPoop());
    }
    else
    {
      if (this.Brain.Info.Age >= 14 && this.daycare != null && PlayerFarming.Location == FollowerLocation.Base)
      {
        Interaction_Daycare.RemoveFromDaycare(this.Brain.Info.ID);
        this.daycare = (StructureBrain) null;
        this.follower.Interaction_FollowerInteraction.Interactable = true;
      }
      if (this._state == FollowerTaskState.Idle)
      {
        this._gameTimeToNextStateUpdate -= deltaGameTime;
        if ((double) this._gameTimeToNextStateUpdate <= 0.0)
        {
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
          this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(10f, 20f);
        }
      }
      if (this.Brain == null || this.Brain.Info == null)
        return;
      if (this.Brain.Info.Age != this.age && this.Brain.CurrentState != null && (UnityEngine.Object) this.follower != (UnityEngine.Object) null)
        this.Brain.CurrentState.SetStateAnimations(this.follower);
      this.age = this.Brain.Info.Age;
    }
  }

  public void TryGetUp()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.Idle);
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.busy = true;
    this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (this.follower.Brain.Info.Age >= 10)
    {
      double num = (double) this.follower.SetBodyAnimation("Baby/Baby-zombie/baby-get-up-zombie", false);
      this.follower.AddBodyAnimation("Baby/Baby-zombie/baby-idle-stand-zombie", true, 0.0f);
    }
    else
    {
      double num = (double) this.follower.SetBodyAnimation("Baby/Baby-zombie/baby-get-up-fall-zombie", false);
      this.follower.AddBodyAnimation("Baby/Baby-zombie/baby-idle-sit-zombie", true, 0.0f);
    }
    GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(4f, 5f), (System.Action) (() =>
    {
      if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      {
        this.follower.State.CURRENT_STATE = StateMachine.State.Idle;
        if (this.follower.Brain.CurrentState != null)
          this.follower.Brain.CurrentState.SetStateAnimations(this.follower);
      }
      this.busy = false;
      this.SetState(FollowerTaskState.GoingTo);
    }));
  }

  public void FallDown()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.Idle);
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.busy = true;
    this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) this.follower.SetBodyAnimation("Baby/Baby-zombie/baby-fall-zombie", false);
    this.follower.AddBodyAnimation("Baby/Baby-zombie/baby-idle-sit-zombie", false, 0.0f);
    this.follower.AddBodyAnimation("Baby/Baby-zombie/baby-get-up-fall-zombie", false, 0.0f);
    GameManager.GetInstance().WaitForSeconds(6.2f, (System.Action) (() =>
    {
      if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      {
        this.follower.State.CURRENT_STATE = StateMachine.State.Idle;
        if (this.follower.Brain.CurrentState != null)
          this.follower.Brain.CurrentState.SetStateAnimations(this.follower);
      }
      this.busy = false;
      this.SetState(FollowerTaskState.GoingTo);
    }));
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.SetOutfit(FollowerOutfitType.Child, false);
    if (follower.Brain.CurrentState != null)
      follower.Brain.CurrentState.SetStateAnimations(follower);
    if (this.daycare != null)
    {
      follower.Interaction_FollowerInteraction.Interactable = false;
      follower.HideAllFollowerIcons();
    }
    this.loopingSound = AudioManager.Instance.CreateLoop("event:/dialogue/followers/babies/zombie_baby_mumble", follower.gameObject);
    AudioManager.Instance.PlayLoop(this.loopingSound);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.ShowAllFollowerIcons();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.daycare == null)
      return TownCentre.RandomCircleFromTownCentre(16f);
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
    {
      if (daycare.Structure.Brain.Data.ID == this.daycare.Data.ID)
        return daycare.MiddlePosition + (Vector3) UnityEngine.Random.insideUnitCircle * ((Structures_Daycare) daycare.Structure.Brain).BoundariesRadius;
    }
    return this.daycare.Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle * ((Structures_Daycare) this.daycare).BoundariesRadius;
  }

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override float SocialChange(float deltaGameTime) => 0.0f;

  public override float VomitChange(float deltaGameTime) => 0.0f;

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        WorshipperBubble.SPEECH_TYPE Type = WorshipperBubble.SPEECH_TYPE.HELP;
        follower.WorshipperBubble.gameObject.SetActive(true);
        follower.WorshipperBubble.Play(Type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }

  [CompilerGenerated]
  public void \u003CTryGetUp\u003Eb__29_0()
  {
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      this.follower.State.CURRENT_STATE = StateMachine.State.Idle;
      if (this.follower.Brain.CurrentState != null)
        this.follower.Brain.CurrentState.SetStateAnimations(this.follower);
    }
    this.busy = false;
    this.SetState(FollowerTaskState.GoingTo);
  }

  [CompilerGenerated]
  public void \u003CFallDown\u003Eb__30_0()
  {
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      this.follower.State.CURRENT_STATE = StateMachine.State.Idle;
      if (this.follower.Brain.CurrentState != null)
        this.follower.Brain.CurrentState.SetStateAnimations(this.follower);
    }
    this.busy = false;
    this.SetState(FollowerTaskState.GoingTo);
  }
}
