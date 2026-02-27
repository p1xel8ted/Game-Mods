// Decompiled with JetBrains decompiler
// Type: Interaction_ConfessionBooth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ConfessionBooth : Interaction
{
  private bool Activating;
  private Follower sacrificeFollower;
  public GameObject Position1;
  public GameObject Position2;
  public GameObject Position3;
  public GameObject CameraBone;
  public Structure Structure;
  public SpriteRenderer SpeechBubble;
  public List<Sprite> SpeechSprites = new List<Sprite>();
  private string SacrificeFollower;
  private string NotEnoughFollowers;
  private string NoFollowers;
  private string AlreadyHeardConfession;
  private FollowerTask_ManualControl Task;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.SacrificeFollower = ScriptLocalization.Interactions.TakeConfession;
    this.NotEnoughFollowers = ScriptLocalization.Interactions.RequiresMoreFollowers;
    this.NoFollowers = ScriptLocalization.Interactions.NoFollowers;
    this.AlreadyHeardConfession = ScriptLocalization.Interactions.AlreadyTakenConfession;
  }

  public override void GetLabel()
  {
    if (TimeManager.CurrentDay > this.Structure.Structure_Info.DayPreviouslyUsed)
    {
      if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0)
      {
        this.Label = DataManager.Instance.Followers.Count <= 0 ? this.NoFollowers : this.NotEnoughFollowers;
        this.Interactable = false;
      }
      else
      {
        this.Interactable = true;
        this.Label = this.Activating ? "" : this.SacrificeFollower;
      }
    }
    else
    {
      this.Interactable = false;
      this.Label = this.AlreadyHeardConfession;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
    this.Interactable = false;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
    PlayerFarming.Instance.GoToAndStop(this.Position1.transform.position, this.gameObject, GoToCallback: (System.Action) (() =>
    {
      if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0 || DataManager.Instance.Followers.Count <= 0)
      {
        GameManager.GetInstance().OnConversationEnd();
        this.Activating = false;
      }
      else
      {
        PlayerFarming.Instance.Spine.UseDeltaTime = false;
        this.StartCoroutine((IEnumerator) this.PositionCharacters(state.gameObject, this.Position1.transform.position));
        state.CURRENT_STATE = StateMachine.State.InActive;
        Time.timeScale = 0.0f;
        List<Follower> followers = new List<Follower>();
        foreach (Follower follower in Follower.Followers)
        {
          if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
            followers.Add(follower);
        }
        UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
        followerSelectInstance.Show(followers, (List<Follower>) null, false, UpgradeSystem.Type.Count, true, true, true);
        UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
        selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
        {
          Time.timeScale = 1f;
          PlayerFarming.Instance.Spine.UseDeltaTime = true;
          this.sacrificeFollower = FollowerManager.FindFollowerByID(followerInfo.ID);
          if (TimeManager.IsNight && this.sacrificeFollower.Brain.CurrentTask != null && this.sacrificeFollower.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.sacrificeFollower.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.sacrificeFollower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
            CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.sacrificeFollower.Brain.Info.ID);
          this.sacrificeFollower.Brain.CurrentTask?.Abort();
          this.Task = new FollowerTask_ManualControl();
          this.sacrificeFollower.Brain.HardSwapToTask((FollowerTask) this.Task);
          GameManager.GetInstance().OnConversationNext(this.sacrificeFollower.gameObject);
          this.sacrificeFollower.transform.position = this.Position3.transform.position;
          this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
          SimulationManager.Pause();
        });
        UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
        selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
        {
          GameManager.GetInstance().OnConversationEnd();
          this.Activating = false;
          Time.timeScale = 1f;
          this.Activating = false;
          PlayerFarming.Instance.Spine.UseDeltaTime = true;
          PlayerFarming.Instance.GoToAndStop(this.Position3, IdleOnEnd: true);
        });
        UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
        selectMenuController3.OnHidden = selectMenuController3.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
      }
    }));
  }

  private IEnumerator SacrificeFollowerRoutine()
  {
    Interaction_ConfessionBooth interactionConfessionBooth = this;
    interactionConfessionBooth.Task.GoToAndStop(interactionConfessionBooth.sacrificeFollower, interactionConfessionBooth.Position2.transform.position, (System.Action) null);
    yield return (object) new WaitForSeconds(2f);
    interactionConfessionBooth.sacrificeFollower.FacePosition(interactionConfessionBooth.Position1.transform.position);
    interactionConfessionBooth.StartCoroutine((IEnumerator) interactionConfessionBooth.PositionCharacters(interactionConfessionBooth.sacrificeFollower.gameObject, interactionConfessionBooth.Position2.transform.position));
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    yield return (object) new WaitForSeconds(0.75f);
    interactionConfessionBooth.SpeechBubble.gameObject.SetActive(true);
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(0.0f, 0.0f);
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    interactionConfessionBooth.SpeechBubble.sprite = interactionConfessionBooth.SpeechSprites[UnityEngine.Random.Range(0, interactionConfessionBooth.SpeechSprites.Count)];
    switch (UnityEngine.Random.Range(0, 4))
    {
      case 0:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_talk", interactionConfessionBooth.gameObject);
        break;
      case 1:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/talk_short_hate", interactionConfessionBooth.gameObject);
        break;
      case 2:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/talk_short_love", interactionConfessionBooth.gameObject);
        break;
      case 3:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/talk_short_nice", interactionConfessionBooth.gameObject);
        break;
    }
    yield return (object) new WaitForSeconds(2.2f);
    interactionConfessionBooth.SpeechBubble.sprite = interactionConfessionBooth.SpeechSprites[UnityEngine.Random.Range(0, interactionConfessionBooth.SpeechSprites.Count)];
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOPunchScale(new Vector3(1f, 1f), 0.5f).SetEase<Tweener>(Ease.OutQuart);
    switch (UnityEngine.Random.Range(0, 4))
    {
      case 0:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_talk", interactionConfessionBooth.gameObject);
        break;
      case 1:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/talk_short_hate", interactionConfessionBooth.gameObject);
        break;
      case 2:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/talk_short_love", interactionConfessionBooth.gameObject);
        break;
      case 3:
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/talk_short_nice", interactionConfessionBooth.gameObject);
        break;
    }
    yield return (object) new WaitForSeconds(2.2f);
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    yield return (object) new WaitForSeconds(0.5f);
    interactionConfessionBooth.SpeechBubble.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(interactionConfessionBooth.CameraBone, 5f);
    GameManager.GetInstance().OnConversationNext(interactionConfessionBooth.sacrificeFollower.gameObject, 8f);
    string Animation = "";
    float Duration = 0.0f;
    Thought thought = Thought.None;
    interactionConfessionBooth.Task.GoToAndStop(interactionConfessionBooth.sacrificeFollower, interactionConfessionBooth.Position3.transform.position, (System.Action) (() =>
    {
      if ((double) UnityEngine.Random.value <= 0.10000000149011612)
      {
        float num = UnityEngine.Random.value;
        if ((double) num < 0.3333333432674408)
        {
          thought = Thought.GaveConfessionAnxious;
          Animation = "Reactions/react-feared";
          Duration = 2.8f;
        }
        else if (Utils.Between(num, 0.333333343f, 0.6666667f))
        {
          thought = Thought.GaveConfessionAnnoyed;
          Animation = "Reactions/react-sad";
          Duration = 2.9f;
        }
        else if ((double) num >= 0.66666668653488159)
        {
          thought = Thought.GaveConfessionDivine;
          Animation = "Reactions/react-happy1";
          Duration = 2.1f;
        }
      }
      else
      {
        float num = UnityEngine.Random.value;
        if ((double) num < 0.3333333432674408)
        {
          thought = Thought.GaveConfessionHappy;
          Animation = "Reactions/react-enlightened2";
          Duration = 2.1f;
        }
        else if (Utils.Between(num, 0.333333343f, 0.6666667f))
        {
          thought = Thought.GaveConfessionEcstatic;
          Animation = "Reactions/react-happy1";
          Duration = 2.1f;
        }
        else if ((double) num >= 0.66666668653488159)
        {
          thought = Thought.GaveConfessionHonoured;
          Animation = "Reactions/react-happy1";
          Duration = 2.1f;
        }
      }
      this.sacrificeFollower.Brain.AddThought(thought);
      double num1 = (double) this.sacrificeFollower.SetBodyAnimation(Animation, false);
      this.sacrificeFollower.AddBodyAnimation("idle", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
      this.sacrificeFollower.Brain.AddAdoration(FollowerBrain.AdorationActions.ConfessionBooth, (System.Action) null);
    }));
    yield return (object) new WaitForSeconds(2f);
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("TAKE_CONFESSION"));
    yield return (object) new WaitForSeconds(2f);
    interactionConfessionBooth.StartCoroutine((IEnumerator) interactionConfessionBooth.DelayEnd());
  }

  private IEnumerator DelayEnd()
  {
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    this.sacrificeFollower.Brain.CompleteCurrentTask();
    this.Activating = false;
    this.Structure.Structure_Info.DayPreviouslyUsed = TimeManager.CurrentDay;
    PlayerFarming.Instance.GoToAndStop(this.Position3, IdleOnEnd: true);
    SimulationManager.UnPause();
  }

  private IEnumerator PositionCharacters(GameObject Character, Vector3 TargetPosition)
  {
    float Progress = 0.0f;
    float Duration = 0.3f;
    Vector3 StartingPosition = Character.transform.position;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      Character.transform.position = Vector3.Lerp(StartingPosition, TargetPosition, Progress / Duration);
      yield return (object) null;
    }
    Character.transform.position = TargetPosition;
  }
}
