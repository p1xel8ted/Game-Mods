// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerDessentingChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class Interaction_FollowerDessentingChoice : Interaction
{
  [SerializeField]
  public SkeletonAnimation followerSpine;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  [SerializeField]
  public Interaction_FollowerDessentingChoice.Question[] questions;
  public FollowerInfo followerInfo;
  public ParticleSystem recruitParticles;
  public string skin;
  public Villager_Info v_i;
  public WorshipperInfoManager wim;
  public FollowerSpineEventListener eventlistener;
  public string IdleAnimation;
  public string TalkAnimation;
  public EventInstance receiveLoop;

  public void Start()
  {
    this.ActivateDistance = 3f;
    this.followerSpine = this.GetComponentInChildren<SkeletonAnimation>();
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Dungeon1_4:
        this.IdleAnimation = "Dissenters/rescue2";
        this.TalkAnimation = "Dissenters/rescue-talk2";
        break;
      default:
        this.IdleAnimation = "Dissenters/rescue1";
        this.TalkAnimation = "Dissenters/rescue-talk1";
        break;
    }
    this.followerSpine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
    this.skin = DataManager.GetRandomLockedSkin();
    if (this.skin.IsNullOrEmpty())
      this.skin = DataManager.GetRandomSkin();
    this.v_i = Villager_Info.NewCharacter(this.skin);
    this.wim = this.GetComponent<WorshipperInfoManager>();
    this.wim.SetV_I(this.v_i);
    this.followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, this.skin);
    if (this.followerInfo.SkinName == "Giraffe")
      this.followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (this.followerInfo.SkinName == "Poppy")
      this.followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Poppy");
    if (this.followerInfo.SkinName == "Pudding")
      this.followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Pudding");
    this.followerInfo.Traits.Clear();
    this.followerInfo.Traits.Add(UnityEngine.Random.Range(0, 2) == 1 ? FollowerTrait.TraitType.Faithless : FollowerTrait.TraitType.Cynical);
    this.followerInfo.TraitsSet = true;
    this.eventlistener.SetPitchAndVibrator(this.followerInfo.follower_pitch, this.followerInfo.follower_vibrato, this.followerInfo.ID);
  }

  public override void GetLabel()
  {
    this.Label = this.Interactable ? ScriptLocalization.Interactions.Talk : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }

  public IEnumerator InteractIE()
  {
    Interaction_FollowerDessentingChoice dessentingChoice = this;
    dessentingChoice.playerFarming.GoToAndStop(dessentingChoice.transform.position + Vector3.left * 2f);
    while (dessentingChoice.playerFarming.GoToAndStopping)
      yield return (object) null;
    dessentingChoice.playerFarming.state.facingAngle = Utils.GetAngle(dessentingChoice.playerFarming.transform.position, dessentingChoice.transform.position);
    dessentingChoice.playerFarming.state.LookAngle = Utils.GetAngle(dessentingChoice.playerFarming.transform.position, dessentingChoice.transform.position);
    Interaction_FollowerDessentingChoice.Question question = dessentingChoice.GetRandomQuestion();
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(dessentingChoice.gameObject, question.Line1),
      new ConversationEntry(dessentingChoice.gameObject, question.Line2)
    }, new List<MMTools.Response>()
    {
      new MMTools.Response(question.AnswerA, (System.Action) (() => this.StartCoroutine((IEnumerator) this.ResponseIE(true, question))), question.AnswerA),
      new MMTools.Response(question.AnswerB, (System.Action) (() => this.StartCoroutine((IEnumerator) this.ResponseIE(false, question))), question.AnswerB)
    }, (System.Action) null), false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
  }

  public IEnumerator ResponseIE(
    bool responseWasA,
    Interaction_FollowerDessentingChoice.Question question)
  {
    Interaction_FollowerDessentingChoice dessentingChoice = this;
    yield return (object) null;
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(dessentingChoice.gameObject, responseWasA ? question.ResultA : question.ResultB)
    };
    Entries[0].CharacterName = dessentingChoice.followerInfo.Name;
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[0].vibratoValue = dessentingChoice.followerInfo.follower_vibrato;
    Entries[0].pitchValue = dessentingChoice.followerInfo.follower_pitch;
    if (responseWasA && question.CorrectAnswerIsA || !responseWasA && !question.CorrectAnswerIsA)
    {
      Entries[0].Animation = "Conversations/talk-nice1";
    }
    else
    {
      Entries[0].soundPath = "event:/dialogue/followers/talk_short_hate";
      Entries[0].Animation = dessentingChoice.TalkAnimation;
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    Debug.Log((object) ("responseWasA: " + responseWasA.ToString()));
    Debug.Log((object) ("question.CorrectAnswerIsA: " + question.CorrectAnswerIsA.ToString()));
    if (responseWasA && question.CorrectAnswerIsA || !responseWasA && !question.CorrectAnswerIsA)
    {
      Debug.Log((object) "AAA");
      dessentingChoice.StartCoroutine((IEnumerator) dessentingChoice.RecruitedFollower());
    }
    else
    {
      Debug.Log((object) "BBB");
      GameManager.GetInstance().OnConversationEnd();
      dessentingChoice.followerSpine.AnimationState.SetAnimation(0, dessentingChoice.IdleAnimation, true);
    }
  }

  public Interaction_FollowerDessentingChoice.Question GetRandomQuestion()
  {
    return this.questions[DataManager.Instance.DessentingFollowerChoiceQuestionIndex < this.questions.Length - 1 ? ++DataManager.Instance.DessentingFollowerChoiceQuestionIndex : UnityEngine.Random.Range(0, this.questions.Length)];
  }

  public IEnumerator RecruitedFollower()
  {
    Interaction_FollowerDessentingChoice dessentingChoice = this;
    dessentingChoice.Interactable = false;
    GameManager.GetInstance().OnConversationNext(dessentingChoice.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    AudioManager.Instance.PlayOneShot("event:/followers/rescue");
    dessentingChoice.recruitParticles.Play();
    dessentingChoice.followerSpine.AnimationState.SetAnimation(0, "convert-short", false);
    dessentingChoice.portalSpine.gameObject.SetActive(true);
    dessentingChoice.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", dessentingChoice.playerFarming.gameObject);
    dessentingChoice.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", dessentingChoice.playerFarming.gameObject, true);
    dessentingChoice.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration = dessentingChoice.playerFarming.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    FollowerManager.CreateNewRecruit(dessentingChoice.followerInfo, NotificationCentre.NotificationType.NewRecruit);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", dessentingChoice.playerFarming.gameObject);
    int num1 = (int) dessentingChoice.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    float num2 = UnityEngine.Random.value;
    Thought thought = Thought.None;
    if ((double) num2 < 0.699999988079071)
    {
      float num3 = UnityEngine.Random.value;
      if ((double) num3 <= 0.30000001192092896)
        thought = Thought.HappyConvert;
      else if ((double) num3 > 0.30000001192092896 && (double) num3 < 0.60000002384185791)
        thought = Thought.GratefulConvert;
      else if ((double) num3 >= 0.60000002384185791)
        thought = Thought.SkepticalConvert;
    }
    else
      thought = (double) UnityEngine.Random.value > 0.30000001192092896 || DataManager.Instance.Followers.Count <= 0 ? Thought.InstantBelieverConvert : Thought.ResentfulConvert;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    dessentingChoice.followerInfo.Thoughts.Add(data);
    RoomLockController.RoomCompleted();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    dessentingChoice.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) dessentingChoice.gameObject);
  }

  [Serializable]
  public struct Question
  {
    public string Line1;
    public string Line2;
    public string AnswerA;
    public string AnswerB;
    public string ResultA;
    public string ResultB;
    public bool CorrectAnswerIsA;
  }
}
