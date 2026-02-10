// Decompiled with JetBrains decompiler
// Type: Interaction_ConfessionBooth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_ConfessionBooth : Interaction
{
  public bool Activating;
  public Follower sacrificeFollower;
  public GameObject Position1;
  public GameObject Position2;
  public GameObject Position3;
  public GameObject CameraBone;
  public Structure Structure;
  public SpriteRenderer SpeechBubble;
  public List<Sprite> SpeechSprites = new List<Sprite>();
  public string SacrificeFollower;
  public string NotEnoughFollowers;
  public string NoFollowers;
  public string AlreadyHeardConfession;
  public FollowerTask_ManualControl Task;
  public bool givenOutfit;

  public void Start() => this.UpdateLocalisation();

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
    int num;
    bool flag = DataManager.Instance.DayPreviosulyUsedStructures.TryGetValue(this.Structure.Type, out num);
    if (!flag || flag && TimeManager.CurrentDay > num)
    {
      DataManager.Instance.DayPreviosulyUsedStructures.Remove(this.Structure.Type);
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
    this.state = state;
    this.StartCoroutine((IEnumerator) this.OnInteractIE(state));
  }

  public IEnumerator OnInteractIE(StateMachine state)
  {
    Interaction_ConfessionBooth interactionConfessionBooth = this;
    if (!interactionConfessionBooth.Activating)
    {
      interactionConfessionBooth.\u003C\u003En__0(state);
      interactionConfessionBooth.Activating = true;
      interactionConfessionBooth.Interactable = false;
      GameManager.GetInstance().OnConversationNew(false);
      GameManager.GetInstance().OnConversationNext(interactionConfessionBooth.playerFarming.CameraBone, 8f);
      bool waiting = true;
      interactionConfessionBooth.playerFarming.GoToAndStop(interactionConfessionBooth.Position1.transform.position, interactionConfessionBooth.gameObject, GoToCallback: (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
      if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0 || DataManager.Instance.Followers.Count <= 0)
      {
        GameManager.GetInstance().OnConversationEnd();
        interactionConfessionBooth.Activating = false;
      }
      else
      {
        interactionConfessionBooth.playerFarming.Spine.UseDeltaTime = false;
        interactionConfessionBooth.StartCoroutine((IEnumerator) interactionConfessionBooth.PositionCharacters(interactionConfessionBooth.playerFarming.gameObject, interactionConfessionBooth.Position1.transform.position));
        state.CURRENT_STATE = StateMachine.State.InActive;
        Time.timeScale = 0.0f;
        bool giveSin = false;
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.DoctrinalExtremist))
        {
          GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
          ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
          choice.Offset = new Vector3(0.0f, -500f);
          choice.Show(LocalizationManager.GetTranslation("UI/Loyalty"), LocalizationManager.GetTranslation("UI/Sin"), (System.Action) (() =>
          {
            g = (GameObject) null;
            giveSin = false;
          }), (System.Action) (() =>
          {
            g = (GameObject) null;
            giveSin = true;
          }), interactionConfessionBooth.transform.position);
          while ((UnityEngine.Object) g != (UnityEngine.Object) null)
          {
            choice.UpdatePosition(interactionConfessionBooth.transform.position);
            yield return (object) null;
          }
          choice = (ChoiceIndicator) null;
        }
        List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
        foreach (Follower follower in Follower.Followers)
        {
          if (follower.Brain._directInfoAccess.IsSnowman)
            followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
          else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
            followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
          else if (!DataManager.Instance.Followers_Recruit.Contains(follower.Brain._directInfoAccess))
            followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
        }
        followerSelectEntries.Sort((Comparison<FollowerSelectEntry>) ((a, b) => b.FollowerInfo.XPLevel.CompareTo(a.FollowerInfo.XPLevel)));
        UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
        followerSelectInstance.VotingType = TwitchVoting.VotingType.CONFESSION;
        followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
        UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
        selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
        {
          Time.timeScale = 1f;
          this.playerFarming.Spine.UseDeltaTime = true;
          this.sacrificeFollower = FollowerManager.FindFollowerByID(followerInfo.ID);
          if (TimeManager.IsNight && this.sacrificeFollower.Brain.CurrentTask != null && this.sacrificeFollower.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.sacrificeFollower.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.sacrificeFollower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
            CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.sacrificeFollower.Brain.Info.ID);
          this.sacrificeFollower.Brain.CurrentTask?.Abort();
          this.Task = new FollowerTask_ManualControl();
          this.sacrificeFollower.Brain.HardSwapToTask((FollowerTask) this.Task);
          GameManager.GetInstance().OnConversationNext(this.sacrificeFollower.gameObject);
          this.sacrificeFollower.transform.position = this.Position3.transform.position;
          this.StartCoroutine((IEnumerator) this.PositionCharacters(state.gameObject, this.Position1.transform.position));
          this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine(giveSin));
          SimulationManager.Pause();
        });
        UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
        selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
        {
          GameManager.GetInstance().OnConversationEnd();
          this.Activating = false;
          Time.timeScale = 1f;
          this.Activating = false;
          this.playerFarming.Spine.UseDeltaTime = true;
          this.playerFarming.GoToAndStop(this.Position3, IdleOnEnd: true);
        });
        UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
        selectMenuController3.OnShow = selectMenuController3.OnShow + (System.Action) (() =>
        {
          if (!giveSin)
            return;
          foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
            followerInfoBox.ShowPleasure(FollowerBrain.PleasureActions.ConfessionBooth);
        });
        UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
        selectMenuController4.OnShownCompleted = selectMenuController4.OnShownCompleted + (System.Action) (() =>
        {
          if (!giveSin)
            return;
          foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
            followerInfoBox.ShowPleasure(FollowerBrain.PleasureActions.ConfessionBooth);
        });
        UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance;
        selectMenuController5.OnShow = selectMenuController5.OnShow + (System.Action) (() =>
        {
          foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
            followerInfoBox.ShowFaithGain(FollowerBrain.AdorationsAndActions[FollowerBrain.AdorationActions.ConfessionBooth], followerInfoBox.followBrain.Stats.MAX_ADORATION);
        });
        UIFollowerSelectMenuController selectMenuController6 = followerSelectInstance;
        selectMenuController6.OnShownCompleted = selectMenuController6.OnShownCompleted + (System.Action) (() =>
        {
          foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
            followerInfoBox.ShowFaithGain(FollowerBrain.AdorationsAndActions[FollowerBrain.AdorationActions.ConfessionBooth], followerInfoBox.followBrain.Stats.MAX_ADORATION);
        });
        UIFollowerSelectMenuController selectMenuController7 = followerSelectInstance;
        selectMenuController7.OnHidden = selectMenuController7.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
      }
    }
  }

  public IEnumerator SacrificeFollowerRoutine(bool giveSin)
  {
    Interaction_ConfessionBooth interactionConfessionBooth = this;
    yield return (object) new WaitForSeconds(2f);
    bool isInBooth = false;
    interactionConfessionBooth.sacrificeFollower.FacePosition(interactionConfessionBooth.Position1.transform.position);
    interactionConfessionBooth.Task.GoToAndStop(interactionConfessionBooth.sacrificeFollower, interactionConfessionBooth.Position2.transform.position, (System.Action) (() => isInBooth = true));
    if (interactionConfessionBooth._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
    while (!isInBooth)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    interactionConfessionBooth.SpeechBubble.gameObject.SetActive(true);
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(0.0f, 0.0f);
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    interactionConfessionBooth.SpeechBubble.sprite = interactionConfessionBooth.SpeechSprites[UnityEngine.Random.Range(0, interactionConfessionBooth.SpeechSprites.Count - 1)];
    switch (UnityEngine.Random.Range(0, 4))
    {
      case 0:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/general_talk");
        break;
      case 1:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/talk_short_hate");
        break;
      case 2:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/talk_short_love");
        break;
      case 3:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/talk_short_nice");
        break;
    }
    yield return (object) new WaitForSeconds(2.2f);
    interactionConfessionBooth.SpeechBubble.sprite = interactionConfessionBooth.SpeechSprites[UnityEngine.Random.Range(0, interactionConfessionBooth.SpeechSprites.Count)];
    if (giveSin)
      interactionConfessionBooth.SpeechBubble.sprite = interactionConfessionBooth.SpeechSprites[interactionConfessionBooth.SpeechSprites.Count - 1];
    interactionConfessionBooth.SpeechBubble.gameObject.transform.localScale = Vector3.one * 1.2f;
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    switch (UnityEngine.Random.Range(0, 4))
    {
      case 0:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/general_talk");
        break;
      case 1:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/talk_short_hate");
        break;
      case 2:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/talk_short_love");
        break;
      case 3:
        interactionConfessionBooth.sacrificeFollower.Interaction_FollowerInteraction.eventListener.PlayFollowerVO("event:/dialogue/followers/talk_short_nice");
        break;
    }
    yield return (object) new WaitForSeconds(2.2f);
    interactionConfessionBooth.SpeechBubble.gameObject.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    yield return (object) new WaitForSeconds(0.5f);
    interactionConfessionBooth.SpeechBubble.gameObject.SetActive(false);
    if (interactionConfessionBooth._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
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
      if (giveSin)
        this.sacrificeFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.ConfessionBooth);
      else
        this.sacrificeFollower.Brain.AddAdoration(FollowerBrain.AdorationActions.ConfessionBooth, (System.Action) null);
      ++this.sacrificeFollower.Brain._directInfoAccess.TimesDoneConfessionBooth;
      if (this.sacrificeFollower.Brain._directInfoAccess.TimesDoneConfessionBooth < 5)
        return;
      this.CheckGiveOutfit();
    }));
    yield return (object) new WaitForSeconds(2f);
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("TAKE_CONFESSION"));
    yield return (object) new WaitForSeconds(2f);
    interactionConfessionBooth.StartCoroutine((IEnumerator) interactionConfessionBooth.DelayEnd());
  }

  public void CheckGiveOutfit()
  {
    if (!DataManager.Instance.TailorEnabled || DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_3) || this.givenOutfit)
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_3;
    this.givenOutfit = true;
  }

  public IEnumerator DelayEnd()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_ConfessionBooth interactionConfessionBooth = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().OnConversationEnd();
      interactionConfessionBooth.sacrificeFollower.Brain.CompleteCurrentTask();
      interactionConfessionBooth.Activating = false;
      DataManager.Instance.DayPreviosulyUsedStructures.Add(interactionConfessionBooth.Structure.Type, TimeManager.CurrentDay);
      interactionConfessionBooth.playerFarming.GoToAndStop(interactionConfessionBooth.Position3, IdleOnEnd: true);
      SimulationManager.UnPause();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PositionCharacters(GameObject Character, Vector3 TargetPosition)
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

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(StateMachine state) => base.OnInteract(state);
}
