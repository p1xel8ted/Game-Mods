// Decompiled with JetBrains decompiler
// Type: MMTools.MMConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using Rewired;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unify.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WebSocketSharp;

#nullable disable
namespace MMTools;

public class MMConversation : MonoBehaviour
{
  private EventInstance talkSoundInstance;
  public TMP_Text TitleText;
  public TextAnimatorPlayer TextPlayer;
  public SpeechBubble SpeechBubble;
  public DialogueWheel DialogueWheel;
  private static GameObject Instance;
  public static ConversationObject CURRENT_CONVERSATION;
  private Queue<string> dialogueLines = new Queue<string>();
  private static int Position;
  public static MMConversation mmConversation;
  private Tween currentCloseFadeTween;
  public Player player;
  public static bool isPlaying = false;
  private static bool CallOnConversationEnd = true;
  private static bool SetPlayerIdleOnComplete;
  public static bool ControlCamera = true;
  [Header("Next Arrow")]
  public GameObject NextArrowContainer;
  public RectTransform NextArrowRectTransform;
  public static bool isBark = false;
  private SkeletonAnimation SpeakerSpine;
  private string CachedAnimation = "";
  public List<MMConversation.TermAndColor> TermsAndColors = new List<MMConversation.TermAndColor>();

  public static event MMConversation.ConversationNew OnConversationNew;

  public static event MMConversation.ConversationNext OnConversationNext;

  public static event MMConversation.ConversationEnd OnConversationEnd;

  public static void Play(
    ConversationObject ConversationObject,
    bool CallOnConversationEnd = true,
    bool SetPlayerInactiveOnStart = true,
    bool SetPlayerIdleOnComplete = true,
    bool Translate = true,
    bool ShowLetterBox = true,
    bool SnapLetterBox = false,
    bool showControlPrompt = true)
  {
    if (PlayerFarming.Location != FollowerLocation.Base)
      SimulationManager.Pause();
    MMConversation.CURRENT_CONVERSATION = ConversationObject;
    MMConversation.isBark = false;
    MMConversation.ControlCamera = true;
    if ((UnityEngine.Object) MMConversation.Instance == (UnityEngine.Object) null)
    {
      MMConversation.Instance = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("MMConversation/Conversation")) as GameObject;
      MMConversation.mmConversation = MMConversation.Instance.GetComponent<MMConversation>();
      MMConversation.mmConversation.TextPlayer.StopShowingText();
      MMConversation.mmConversation.TextPlayer.ShowText("");
    }
    MMConversation.Instance.SetActive(true);
    MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(false);
    MMConversation.mmConversation.DialogueWheel.gameObject.SetActive(false);
    MMConversation.CallOnConversationEnd = CallOnConversationEnd;
    MMConversation.SetPlayerIdleOnComplete = SetPlayerIdleOnComplete;
    MMConversation.isPlaying = true;
    MMConversation.mmConversation.TextPlayer.textAnimator.timeScale = TextAnimator.TimeScale.Scaled;
    MMConversation.mmConversation.NextArrowContainer.SetActive(showControlPrompt);
    MMConversation.ConversationNew onConversationNew = MMConversation.OnConversationNew;
    if (onConversationNew != null)
      onConversationNew(SetPlayerInactiveOnStart, SnapLetterBox, ShowLetterBox);
    MMConversation.mmConversation.player = RewiredInputManager.MainPlayer;
    CanvasGroup canvasgGroup = MMConversation.mmConversation.GetComponent<CanvasGroup>();
    canvasgGroup.alpha = 0.0f;
    MMConversation.mmConversation.SpeakerSpine = (SkeletonAnimation) null;
    MMConversation.Position = 0;
    MMConversation.mmConversation.SpeechBubble.Reset(MMConversation.mmConversation.TextPlayer.GetComponent<TextMeshProUGUI>());
    MMConversation.mmConversation.TextPlayer.onTextShowed.AddListener(new UnityAction(MMConversation.mmConversation.OnPrintCompleted));
    MMConversation.mmConversation.DialogueWheel.gameObject.SetActive(false);
    MMConversation.mmConversation.ShowNextLine(false);
    MMConversation.Position = -1;
    SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(MMConversation.SceneManager_activeSceneChanged);
    GameManager.GetInstance().StartCoroutine((IEnumerator) MMConversation.WaitForCameraToStop((System.Action) (() =>
    {
      MMConversation.mmConversation.ShowNextLine(true);
      canvasgGroup.DOFade(1f, 0.75f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/ui/conversation_start");
    })));
  }

  private static IEnumerator WaitForCameraToStop(System.Action callback)
  {
    while (MMConversation.isPlaying && GameManager.GetInstance().CamFollowTarget.IsMoving)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
  {
    MMConversation.Instance = (GameObject) null;
    MMConversation.mmConversation = (MMConversation) null;
    MMConversation.isPlaying = false;
    MMConversation.CURRENT_CONVERSATION = (ConversationObject) null;
  }

  public static void UseDeltaTime(bool Toggle)
  {
    if (!((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null))
      return;
    MMConversation.mmConversation.TextPlayer.textAnimator.timeScale = Toggle ? TextAnimator.TimeScale.Scaled : TextAnimator.TimeScale.Unscaled;
  }

  public static void PlayBark(ConversationObject ConversationObject, bool Translate = true)
  {
    MMConversation.CURRENT_CONVERSATION = ConversationObject;
    MMConversation.isBark = true;
    MMConversation.isPlaying = true;
    if ((UnityEngine.Object) MMConversation.Instance == (UnityEngine.Object) null)
    {
      MMConversation.Instance = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("MMConversation/Conversation")) as GameObject;
      MMConversation.mmConversation = MMConversation.Instance.GetComponent<MMConversation>();
      MMConversation.mmConversation.TextPlayer.StopShowingText();
      MMConversation.mmConversation.TextPlayer.ShowText("");
    }
    else
      MMConversation.Instance.SetActive(true);
    MMConversation.mmConversation.player = (Player) null;
    MMConversation.Position = -1;
    MMConversation.mmConversation.SpeechBubble.Reset(MMConversation.mmConversation.TextPlayer.GetComponent<TextMeshProUGUI>());
    MMConversation.mmConversation.TextPlayer.onTextShowed.AddListener(new UnityAction(MMConversation.mmConversation.OnPrintCompleted));
    MMConversation.mmConversation.DialogueWheel.gameObject.SetActive(false);
    MMConversation.mmConversation.ShowNextLine(true);
    MMConversation.mmConversation.NextArrowContainer.SetActive(false);
    MMConversation.mmConversation.GetComponent<CanvasGroup>().DOFade(1f, 0.25f);
  }

  private void OnPrintCompleted()
  {
    if (MMConversation.Position < MMConversation.CURRENT_CONVERSATION.Entries.Count - 1 && MMConversation.CURRENT_CONVERSATION.Entries.Count != 1)
      return;
    if (MMConversation.CURRENT_CONVERSATION.Responses != null && MMConversation.CURRENT_CONVERSATION.Responses.Count > 0)
    {
      this.DialogueWheel.gameObject.SetActive(true);
      this.DialogueWheel.OnGiveAnswer += new DialogueWheel.GiveAnswer(this.OnGiveAnswer);
      this.SpeechBubble.HidePrompt();
    }
    else
    {
      if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses == null || MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count <= 0)
        return;
      MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>().Show();
      this.SpeechBubble.HidePrompt();
    }
  }

  private void OnGiveAnswer(int Answer)
  {
    if (MMConversation.CURRENT_CONVERSATION == null || MMConversation.CURRENT_CONVERSATION.Responses == null)
      return;
    System.Action CallBack = (System.Action) null;
    UnityEvent EventCallBack = MMConversation.CURRENT_CONVERSATION.Responses.Count > Answer ? MMConversation.CURRENT_CONVERSATION.Responses[Answer]?.EventCallBack : (UnityEvent) null;
    if (MMConversation.CURRENT_CONVERSATION.Responses.Count > Answer && MMConversation.CURRENT_CONVERSATION.Responses[Answer] != null && MMConversation.CURRENT_CONVERSATION.Responses[Answer].ActionCallBack != null)
      CallBack = MMConversation.CURRENT_CONVERSATION.Responses[Answer].ActionCallBack;
    this.Close(CustomCallbacks: (System.Action) (() =>
    {
      if (CallBack != null)
        CallBack();
      UIManager.PlayAudio("event:/ui/conversation_change_page");
      EventCallBack?.Invoke();
    }));
  }

  private void ShowNextLine(bool playVOandAnim)
  {
    if (MMConversation.CURRENT_CONVERSATION == null || MMConversation.CURRENT_CONVERSATION.Entries == null)
      return;
    if (this.talkSoundInstance.isValid() && !MMConversation.isBark)
      AudioManager.Instance.StopLoop(this.talkSoundInstance);
    if (playVOandAnim)
    {
      UIManager.PlayAudio("event:/ui/conversation_change_page");
      ++MMConversation.Position;
    }
    if (MMConversation.Position >= MMConversation.CURRENT_CONVERSATION.Entries.Count)
    {
      this.ResetSpineAnimation();
      if (MMConversation.CURRENT_CONVERSATION.Responses != null && MMConversation.CURRENT_CONVERSATION.Responses.Count > 0 || MMConversation.CURRENT_CONVERSATION.DoctrineResponses != null && MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 0)
        return;
      this.SpeechBubble.ClearTarget();
      this.Close();
    }
    else
    {
      GameObject go = MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].SpeakerIsPlayer ? PlayerFarming.Instance.gameObject : MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Speaker;
      if (playVOandAnim)
      {
        this.UpdateText();
        if ((UnityEngine.Object) go != (UnityEngine.Object) null)
        {
          this.SetSpineAnimation((UnityEngine.Object) go.GetComponentInChildren<SkeletonAnimation>() != (UnityEngine.Object) null ? go : MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].SkeletonData?.gameObject, MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Animation, MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].LoopAnimation, MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].DefaultAnimation, playVOandAnim);
          if ((UnityEngine.Object) this.SpeakerSpine != (UnityEngine.Object) null)
            this.SpeechBubble.SetTarget(this.SpeakerSpine.transform, MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Offset);
          else
            this.SpeechBubble.SetTarget(go.transform, MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Offset);
        }
        string soundPath = !MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].soundPath.IsNullOrEmpty() || !((UnityEngine.Object) MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].SkeletonData != (UnityEngine.Object) null) ? MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].soundPath : this.GetFallBackVO(MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].SkeletonData);
        this.talkSoundInstance = !((UnityEngine.Object) go != (UnityEngine.Object) null) ? AudioManager.Instance.CreateLoop(soundPath, true) : AudioManager.Instance.CreateLoop(soundPath, go, true);
        if ((double) MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].pitchValue != -1.0)
        {
          int num1 = (int) this.talkSoundInstance.setParameterByName("follower_pitch", MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].pitchValue);
        }
        if ((double) MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].vibratoValue != -1.0)
        {
          int num2 = (int) this.talkSoundInstance.setParameterByName("follower_vibrato", MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].vibratoValue);
        }
      }
      if (MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Callback != null && MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Callback.GetPersistentEventCount() > 0)
        MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Callback.Invoke();
      if (MMConversation.OnConversationNext == null || MMConversation.isBark)
        return;
      MMConversation.OnConversationNext(MMConversation.ControlCamera ? go : (GameObject) null, MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].SetZoom ? MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].Zoom : UnityEngine.Random.Range(7.5f, 8.5f));
    }
  }

  private string GetFallBackVO(SkeletonAnimation spine)
  {
    Debug.Log((object) ("spine.skeletonDataAsset.name: " + spine.skeletonDataAsset.name.Colour(Color.red)));
    switch (spine.skeletonDataAsset.name)
    {
      case "DeathBig":
      case "DeathBig_SkeletonData":
      case "DeathCat_Boss":
      case "DeathCat_Boss_SkeletonData":
      case "DeathCat_Spirit":
      case "DeathCat_Spirit_SkeletonData":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/death_cat/standard_death";
      case "Follower":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/followers/general_talk";
      case "ForestCultLeader_SkeletonData":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/dun1_cult_leader_leshy/standard_leshy";
      case "Fox_SkeletonData":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/the_night/standard_the_night";
      case "FrogCultLeader":
      case "FrogCultLeader_SkeletonData":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/dun2_cult_leader_heket/standard_heket";
      case "Haro_SkeletonData":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/haro/standard_haro";
      case "JellyCultLeader":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/dun3_cult_leader_kallamar/standard_kallamar";
      case "MidasNPC":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/midas/standard_midas";
      case "MushroomTraveller_SkeletonData":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/sozo/sozo_standard";
      case "RatNPC":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/ratau/standard_ratau";
      case "Ratoo":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/ratoo/standard_ratoo";
      case "SpiderCultLeader":
        Debug.Log((object) ("Return fallback for: " + spine.skeletonDataAsset.name));
        return "event:/dialogue/dun4_cult_leader_shamura/standard_shamura";
      default:
        Debug.Log((object) ("NO fallback for: " + spine.skeletonDataAsset.name));
        return string.Empty;
    }
  }

  public void Update()
  {
    if (MonoSingleton<UIManager>.Instance.ForceBlockMenus || MMTransition.IsPlaying || GameManager.InMenu || UIMenuBase.ActiveMenus.Count > 0 || MonoSingleton<UIManager>.Instance.IsPaused)
    {
      if (!MMConversation.isBark)
        return;
      this.Close();
    }
    else
    {
      if (this.player == null)
        this.player = RewiredInputManager.MainPlayer;
      if (this.player != null && InputManager.Gameplay.GetAdvanceDialogueButtonDown() && (MMConversation.CURRENT_CONVERSATION.DoctrineResponses == null || MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count <= 0))
        this.HandleNextClicked();
      if (!((UnityEngine.Object) this.TextPlayer != (UnityEngine.Object) null) || !((UnityEngine.Object) this.TextPlayer.textAnimator != (UnityEngine.Object) null) || !((UnityEngine.Object) this.DialogueWheel != (UnityEngine.Object) null) || MMConversation.CURRENT_CONVERSATION == null || !this.TextPlayer.textAnimator.allLettersShown || this.DialogueWheel.gameObject.activeSelf || MMConversation.CURRENT_CONVERSATION.Responses == null || MMConversation.CURRENT_CONVERSATION.Responses.Count <= 0)
        return;
      this.OnPrintCompleted();
    }
  }

  private void HandleNextClicked()
  {
    if (this.DialogueWheel.gameObject.activeSelf || !this.SpeechBubble.gameObject.activeSelf)
      return;
    if (!this.TextPlayer.textAnimator.allLettersShown)
    {
      this.TextPlayer.SkipTypewriter();
    }
    else
    {
      this.TextPlayer.StopShowingText();
      this.ShowNextLine(true);
    }
    this.NextArrowRectTransform.DOKill();
    this.NextArrowRectTransform.anchoredPosition = new Vector2(0.0f, -10f);
    this.NextArrowRectTransform.DOAnchorPosY(0.0f, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack);
  }

  public static ConversationObject CreateConversation(
    List<ConversationEntry> Entries,
    List<Response> Responses,
    System.Action CallBack)
  {
    return new ConversationObject(Entries, Responses, CallBack);
  }

  public void Close(bool resetAnimation = true, System.Action CustomCallbacks = null, bool stopAudio = true)
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
      return;
    if (MMConversation.isBark)
    {
      if (stopAudio)
        AudioManager.Instance.StopLoop(this.talkSoundInstance);
      this.currentCloseFadeTween = (Tween) this.GetComponent<CanvasGroup>().DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.DoClose(resetAnimation, CustomCallbacks, true);
        this.currentCloseFadeTween = (Tween) null;
      }));
      this.currentCloseFadeTween.Play<Tween>();
    }
    else
      this.DoClose(resetAnimation, CustomCallbacks);
  }

  public void FinishCloseFadeTweenByForce()
  {
    if (this.currentCloseFadeTween == null || !MMConversation.isPlaying)
      return;
    this.currentCloseFadeTween.Complete();
  }

  private void DoClose(bool resetAnimation = true, System.Action CustomCallbacks = null, bool _isBark = false)
  {
    if (_isBark != MMConversation.isBark)
      return;
    if (MMConversation.isBark & resetAnimation)
      this.ResetSpineAnimation();
    this.TextPlayer.onTextShowed.RemoveAllListeners();
    this.DialogueWheel.OnGiveAnswer -= new DialogueWheel.GiveAnswer(this.OnGiveAnswer);
    if (MMConversation.CURRENT_CONVERSATION != null)
    {
      if (MMConversation.CURRENT_CONVERSATION.CallBack != null)
        MMConversation.CURRENT_CONVERSATION.CallBack();
      MMConversation.CURRENT_CONVERSATION?.Clear();
    }
    SimulationManager.UnPause();
    MMConversation.CURRENT_CONVERSATION = (ConversationObject) null;
    SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(MMConversation.SceneManager_activeSceneChanged);
    if (this.talkSoundInstance.isValid() && !_isBark)
      AudioManager.Instance.StopLoop(this.talkSoundInstance);
    if (!MMConversation.isBark)
      AudioManager.Instance.PlayOneShot("event:/ui/conversation_end");
    if (MMConversation.CallOnConversationEnd && MMConversation.OnConversationEnd != null && !_isBark)
      MMConversation.OnConversationEnd(MMConversation.SetPlayerIdleOnComplete);
    if ((UnityEngine.Object) MMConversation.Instance != (UnityEngine.Object) null)
      MMConversation.Instance.SetActive(false);
    if (CustomCallbacks != null)
      CustomCallbacks();
    MMConversation.isPlaying = false;
  }

  private void SetSpineAnimation(
    GameObject Speaker,
    string Animation,
    bool Loop,
    string DefaultAnimation,
    bool playAnimation)
  {
    this.ResetSpineAnimation();
    if ((UnityEngine.Object) Speaker == (UnityEngine.Object) null)
    {
      this.SpeakerSpine = (SkeletonAnimation) null;
    }
    else
    {
      this.SpeakerSpine = MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].SkeletonData;
      if ((UnityEngine.Object) this.SpeakerSpine == (UnityEngine.Object) null)
        this.SpeakerSpine = Speaker.GetComponentInChildren<SkeletonAnimation>();
      if (!((UnityEngine.Object) this.SpeakerSpine != (UnityEngine.Object) null) || this.SpeakerSpine.AnimationState == null || !playAnimation)
        return;
      this.CachedAnimation = DefaultAnimation == "" ? this.SpeakerSpine.AnimationName : DefaultAnimation;
      if (this.SpeakerSpine.AnimationState.Data.SkeletonData.FindAnimation(Animation) != null)
        this.SpeakerSpine.AnimationState.SetAnimation(0, Animation, Loop);
      if (!string.IsNullOrEmpty(DefaultAnimation))
        this.SpeakerSpine.AnimationState.AddAnimation(0, DefaultAnimation, true, 0.0f);
      else if (!Loop && this.SpeakerSpine.AnimationState.Data.SkeletonData.FindAnimation("idle") != null)
      {
        this.SpeakerSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      }
      else
      {
        if (Loop || this.SpeakerSpine.AnimationState.Data.SkeletonData.FindAnimation("animation") == null)
          return;
        this.SpeakerSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      }
    }
  }

  private void ResetSpineAnimation()
  {
    if (!((UnityEngine.Object) this.SpeakerSpine != (UnityEngine.Object) null) || this.SpeakerSpine.AnimationState == null || string.IsNullOrEmpty(this.CachedAnimation))
      return;
    this.SpeakerSpine.AnimationState.SetAnimation(0, this.CachedAnimation, true);
  }

  private void PlayAudioClip(AudioClip audioClip)
  {
  }

  private string AddCharacterName()
  {
    string characterName = MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].CharacterName;
    if (!(characterName != "-") || !(characterName != ""))
      return "";
    string translation = LocalizationManager.GetTranslation(characterName);
    return $"<size=35>{(translation == string.Empty ? characterName : translation).Bold()}</size> <size=5>\n\n</size>";
  }

  private string AddNameColors(string text)
  {
    foreach (MMConversation.TermAndColor termsAndColor in this.TermsAndColors)
    {
      if (termsAndColor.Name != "-" && termsAndColor.Name != "")
        text = text.Bold().Colour(termsAndColor.Color);
    }
    return text;
  }

  private void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateText);
  }

  private void OnDisable()
  {
    MMConversation.isPlaying = false;
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateText);
  }

  private void UpdateText()
  {
    string translation = LocalizationManager.GetTranslation(MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].TermToSpeak);
    string str = this.AddNameColors(this.AddCharacterName());
    if ((UnityEngine.Object) this.TitleText != (UnityEngine.Object) null)
      this.TitleText.text = str;
    this.TextPlayer.ShowText(translation);
  }

  public delegate void ConversationNew(
    bool SetPlayerInactive = true,
    bool SnapLetterBox = false,
    bool ShowLetterBox = true);

  public delegate void ConversationNext(GameObject Speaker, float Zoom = 8f);

  public delegate void ConversationEnd(bool SetPlayerToIdle = true, bool ShowHUD = true);

  [Serializable]
  public class TermAndColor
  {
    [TermsPopup("")]
    public string Name = "-";
    public Color Color;
  }
}
