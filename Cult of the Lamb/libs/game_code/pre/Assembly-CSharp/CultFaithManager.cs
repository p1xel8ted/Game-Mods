// Decompiled with JetBrains decompiler
// Type: CultFaithManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CultFaithManager : MonoBehaviour
{
  public static CultFaithManager Instance;
  public GameObject Container;
  public CanvasGroup CanvasGroup;
  public Image WhitePulse;
  [SerializeField]
  private CanvasGroup flash;
  [SerializeField]
  private CanvasGroup faithLock;
  private bool showing;
  public static System.Action OnPulse;
  private const float STARTING_FAITH = 50f;
  public const float LOW_FAITH = 5f;
  public const float MIN_FAITH = 0.0f;
  public const float MAX_FAITH = 85f;
  private static Action<bool, NotificationData> OnUpdateFaith;
  public BarController BarController;
  private bool activatedLock;
  private DG.Tweening.Sequence sequence;
  private DG.Tweening.Sequence PulseSequence;

  public event CultFaithManager.ThoughtEvent OnThoughtModified;

  private void Awake()
  {
    CultFaithManager.Instance = this;
    CultFaithManager.OnUpdateFaith += new Action<bool, NotificationData>(this.UpdateBar);
  }

  private void Pulse(float Duration = 1f, float Scale = 1.25f, float Alpha = 0.8f, Ease Ease = Ease.OutSine)
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * Scale, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease);
    this.WhitePulse.color = new Color(1f, 1f, 1f, Alpha);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, Duration).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease);
  }

  private void Flash()
  {
    this.flash.gameObject.SetActive(true);
    this.flash.alpha = 1f;
    this.flash.GetComponent<Image>().color = Color.white;
    DOTweenModuleUI.DOColor(this.flash.GetComponent<Image>(), StaticColors.RedColor, 0.33f);
    this.flash.DOFade(0.0f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.33f);
  }

  private void ReadThoughts()
  {
    Debug.Log((object) ("FAITH: " + (object) CultFaithManager.CurrentFaith));
    foreach (ThoughtData thought in CultFaithManager.Thoughts)
      Debug.Log((object) thought.ThoughtType.ToString());
  }

  public static List<ThoughtData> Thoughts
  {
    get => DataManager.Instance.Thoughts;
    set => DataManager.Instance.Thoughts = value;
  }

  public static bool HasThought(Thought thought)
  {
    foreach (ThoughtData thought1 in CultFaithManager.Thoughts)
    {
      if (thought1.ThoughtType == thought)
        return true;
    }
    return false;
  }

  private void OnEnable()
  {
    this.faithLock.gameObject.SetActive(false);
    this.flash.gameObject.SetActive(false);
    this.activatedLock = false;
    this.UpdateBar(false, (NotificationData) null);
    this.Container.SetActive(DataManager.Instance.ShowCultFaith);
  }

  public void Reveal()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RevealRoutine());
  }

  private IEnumerator RevealRoutine()
  {
    CultFaithManager cultFaithManager = this;
    while (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden)
      yield return (object) null;
    while ((double) Time.timeScale < 1.0)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
    Vector3 LocalPosition = cultFaithManager.Container.transform.localPosition;
    cultFaithManager.Container.transform.parent = HUD_Manager.Instance.transform;
    cultFaithManager.Container.transform.localPosition = Vector3.zero;
    cultFaithManager.Container.transform.parent = cultFaithManager.transform;
    DataManager.Instance.ShowCultFaith = true;
    cultFaithManager.CanvasGroup.alpha = 0.0f;
    cultFaithManager.CanvasGroup.DOFade(1f, 1f);
    cultFaithManager.Container.SetActive(true);
    cultFaithManager.Container.transform.localScale = Vector3.one * 3f;
    cultFaithManager.Container.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.4f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.6f);
    cultFaithManager.Container.transform.DOLocalMove(LocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.8f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    cultFaithManager.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    cultFaithManager.showing = true;
  }

  private void OnDestroy()
  {
    this.gameObject.transform.DOKill();
    this.faithLock.DOKill();
    this.WhitePulse.DOKill();
    this.flash.DOKill();
    this.CanvasGroup.DOKill();
    this.Container.transform.DOKill();
    if (this.sequence != null)
    {
      this.sequence.Kill();
      this.sequence = (DG.Tweening.Sequence) null;
    }
    if (this.PulseSequence != null)
    {
      this.PulseSequence.Kill();
      this.PulseSequence = (DG.Tweening.Sequence) null;
    }
    CultFaithManager.OnUpdateFaith -= new Action<bool, NotificationData>(this.UpdateBar);
    if (!((UnityEngine.Object) CultFaithManager.Instance == (UnityEngine.Object) this))
      return;
    CultFaithManager.Instance = (CultFaithManager) null;
  }

  public static float StaticFaith
  {
    get => DataManager.Instance.StaticFaith;
    set => DataManager.Instance.StaticFaith = value;
  }

  public static float CurrentFaith
  {
    get => DataManager.Instance.CultFaith;
    set => DataManager.Instance.CultFaith = value;
  }

  public static void GetFaith(
    float Delta,
    float DeltaDisplay,
    bool Animate,
    NotificationBase.Flair Flair,
    string NotificationMessage = "",
    int FollowerID = -1,
    params string[] args)
  {
    if (FollowerBrainStats.BrainWashed)
    {
      Delta = 0.0f;
      DeltaDisplay = 0.0f;
    }
    CultFaithManager.StaticFaith = Mathf.Clamp(CultFaithManager.StaticFaith + Delta, 0.0f, 85f);
    float num1 = 0.0f;
    foreach (ThoughtData thought in CultFaithManager.Thoughts)
    {
      int num2 = -1;
      while (++num2 < thought.Quantity)
        num1 += num2 <= 0 ? thought.Modifier : (float) thought.StackModifier;
    }
    CultFaithManager.CurrentFaith = Mathf.Clamp(CultFaithManager.StaticFaith + num1, 0.0f, 85f);
    if ((double) CultFaithManager.CurrentFaith >= 42.5)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CrisisOfFaith);
    else if ((double) CultFaithManager.CurrentFaith <= 5.0 && (double) DataManager.Instance.TimeSinceFaithHitEmpty == -1.0)
      DataManager.Instance.TimeSinceFaithHitEmpty = TimeManager.TotalElapsedGameTime;
    else if ((double) CultFaithManager.CurrentFaith > 5.0 && (double) DataManager.Instance.TimeSinceFaithHitEmpty != -1.0)
      DataManager.Instance.TimeSinceFaithHitEmpty = -1f;
    if ((UnityEngine.Object) CultFaithManager.Instance == (UnityEngine.Object) null)
    {
      Action<bool, NotificationData> onUpdateFaith = CultFaithManager.OnUpdateFaith;
      if (onUpdateFaith != null)
        onUpdateFaith(Animate, (NotificationData) null);
      if (!(NotificationMessage != "") || !((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null))
        return;
      NotificationCentre.Instance.PlayFaithNotification(NotificationMessage, DeltaDisplay, Flair, FollowerID, args);
    }
    else
    {
      Action<bool, NotificationData> onUpdateFaith = CultFaithManager.OnUpdateFaith;
      if (onUpdateFaith == null)
        return;
      onUpdateFaith(Animate, new NotificationData(NotificationMessage, DeltaDisplay, FollowerID, Flair, args));
    }
  }

  public static void AddThought(
    Thought thought,
    int FollowerID = -1,
    float faithMultiplier = 1f,
    params string[] args)
  {
    if (!DataManager.Instance.ShowCultFaith || DataManager.Instance.Followers.Count == 0)
      return;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    data.FollowerID = FollowerID;
    data.Modifier *= faithMultiplier;
    if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
    {
      CultFaithManager.ThoughtEvent onThoughtModified = CultFaithManager.Instance.OnThoughtModified;
      if (onThoughtModified != null)
        onThoughtModified(thought);
    }
    if (data.TrackThought)
    {
      int index = -1;
      while (++index < CultFaithManager.Thoughts.Count)
      {
        ThoughtData thought1 = CultFaithManager.Thoughts[index];
        if (thought1.ThoughtGroup == data.ThoughtGroup && thought1.Stacking <= 0)
        {
          CultFaithManager.Thoughts[index] = data;
          CultFaithManager.GetFaith(0.0f, thought1.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(thought1.ThoughtType), thought1.FollowerID, args);
          return;
        }
        if (thought1.ThoughtType == thought && thought1.Stacking > 0)
        {
          if (thought1.Quantity < thought1.Stacking)
          {
            ++thought1.Quantity;
          }
          else
          {
            thought1.CoolDowns.RemoveAt(0);
            thought1.TimeStarted.RemoveAt(0);
          }
          thought1.CoolDowns.Add(data.Duration);
          thought1.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
          CultFaithManager.GetFaith(0.0f, thought1.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(thought1.ThoughtType), thought1.FollowerID, args);
          return;
        }
      }
      CultFaithManager.Thoughts.Add(data);
      CultFaithManager.GetFaith(0.0f, data.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(data.ThoughtType), data.FollowerID, args);
    }
    else
      CultFaithManager.GetFaith(data.Modifier, data.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(data.ThoughtType), data.FollowerID, args);
  }

  private static NotificationBase.Flair GetNotificationFlair(Thought thought)
  {
    switch (thought)
    {
      case Thought.Cult_FollowerDied:
      case Thought.Cult_FollowerDied_Trait:
      case Thought.Cult_FollowerDiedOfOldAge:
      case Thought.DiedFromIllness:
        return NotificationBase.Flair.Negative;
      case Thought.Cult_CompleteQuest:
        return NotificationBase.Flair.Positive;
      default:
        return NotificationBase.Flair.None;
    }
  }

  public static void RemoveThought(Thought thought)
  {
    foreach (ThoughtData thought1 in CultFaithManager.Thoughts)
    {
      if (thought1.ThoughtType == thought)
      {
        CultFaithManager.Thoughts.Remove(thought1);
        if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
        {
          CultFaithManager.ThoughtEvent onThoughtModified = CultFaithManager.Instance.OnThoughtModified;
          if (onThoughtModified != null)
            onThoughtModified(thought1.ThoughtType);
        }
        GameManager.GetInstance().StartCoroutine((IEnumerator) CultFaithManager.DelayGetFaith(thought1));
        break;
      }
    }
  }

  private static IEnumerator DelayGetFaith(ThoughtData thought)
  {
    yield return (object) null;
    CultFaithManager.GetFaith(0.0f, 0.0f, true, CultFaithManager.GetNotificationFlair(thought.ThoughtType), FollowerThoughts.GetNotificationOffLocalizationKey(thought.ThoughtType));
  }

  public static void UpdateThoughts(float DeltaGameTime)
  {
    foreach (ThoughtData thought in CultFaithManager.Thoughts)
    {
      if ((double) thought.Duration != -1.0 && (double) TimeManager.TotalElapsedGameTime - (double) thought.TimeStarted[0] > (double) thought.CoolDowns[0])
      {
        if (thought.Quantity > 1)
        {
          --thought.Quantity;
          thought.CoolDowns.RemoveAt(0);
          thought.TimeStarted.RemoveAt(0);
        }
        else
        {
          CultFaithManager.Thoughts.Remove(thought);
          if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
          {
            CultFaithManager.ThoughtEvent onThoughtModified = CultFaithManager.Instance.OnThoughtModified;
            if (onThoughtModified != null)
              onThoughtModified(thought.ThoughtType);
          }
        }
        CultFaithManager.GetFaith(0.0f, 0.0f, true, CultFaithManager.GetNotificationFlair(thought.ThoughtType), FollowerThoughts.GetNotificationOffLocalizationKey(thought.ThoughtType));
        return;
      }
    }
    if (CheatConsole.FaithType != CheatConsole.FaithTypes.DRIP || FollowerBrainStats.BrainWashed || !((UnityEngine.Object) CultFaithManager.Instance == (UnityEngine.Object) null) && (!((UnityEngine.Object) CultFaithManager.Instance != (UnityEngine.Object) null) || CultFaithManager.Instance.BarController.IsPlaying))
      return;
    if ((double) CultFaithManager.CurrentFaith > 0.34999999403953552)
      CultFaithManager.StaticFaith -= DeltaGameTime * (0.02f * DifficultyManager.GetDripMultiplier());
    else
      CultFaithManager.StaticFaith -= DeltaGameTime * (0.01f * DifficultyManager.GetDripMultiplier());
    CultFaithManager.GetFaith(0.0f, 0.0f, false, NotificationBase.Flair.None);
  }

  private void Start()
  {
    CultFaithManager.GetFaith(0.0f, 0.0f, false, NotificationBase.Flair.None);
    this.showing = DataManager.Instance.ShowCultFaith;
  }

  public static float CultFaithNormalised => CultFaithManager.CurrentFaith / 85f;

  private void UpdateBar(bool Animate, NotificationData NotificationData)
  {
    this.BarController.SetBarSize(CultFaithManager.CultFaithNormalised, Animate, NotificationData: NotificationData);
  }

  public static void UpdateSimulation(float DeltaGameTime)
  {
    if (!DataManager.Instance.ShowCultFaith)
      return;
    CultFaithManager.UpdateThoughts(DeltaGameTime);
    if (!DataManager.Instance.OnboardedDissenter && PlayerFarming.Location != FollowerLocation.Base || TimeManager.IsNight || DataManager.Instance.Followers.Count == 0 || TimeManager.CurrentPhase == DayPhase.Dusk || (double) CultFaithManager.CurrentFaith / 85.0 >= 0.25 || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToStartDissenting < 3600.0)
      return;
    FollowerBrain followerBrain = FollowerBrain.RandomAvailableBrainNoCurseState();
    if (followerBrain == null || !DataManager.Instance.OnboardedDissenter)
      return;
    followerBrain.MakeDissenter();
    DataManager.Instance.LastFollowerToStartDissenting = TimeManager.TotalElapsedGameTime;
  }

  private void TurnFaithLockOn()
  {
    this.activatedLock = true;
    this.faithLock.gameObject.SetActive(true);
    this.Flash();
    this.Pulse();
    this.faithLock.alpha = 1f;
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOShakePosition(2f, new Vector3(5f, 0.0f, 0.0f)).SetDelay<Tweener>(0.5f);
  }

  private void TurnFaithLockOff()
  {
    this.activatedLock = false;
    this.Flash();
    this.Pulse();
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOShakePosition(2f, new Vector3(5f, 0.0f, 0.0f));
    this.faithLock.DOFade(0.0f, 2f);
  }

  private void Update()
  {
    if (!DataManager.Instance.ShowCultFaith)
      return;
    if (this.showing && DataManager.Instance.Followers.Count <= 0)
    {
      this.showing = false;
      this.CanvasGroup.DOFade(0.0f, 1f);
    }
    else if (!this.showing && DataManager.Instance.Followers.Count > 0)
    {
      this.showing = true;
      this.CanvasGroup.DOFade(1f, 1f);
    }
    if (Time.frameCount % 5 != 0)
      return;
    if (FollowerBrainStats.BrainWashed && !this.activatedLock)
      this.TurnFaithLockOn();
    else if (!FollowerBrainStats.BrainWashed && this.activatedLock)
      this.TurnFaithLockOff();
    if ((double) CultFaithManager.CurrentFaith / 85.0 < 0.25)
    {
      if (this.sequence != null)
        return;
      this.sequence = DOTween.Sequence();
      this.sequence.Append((Tween) this.BarController.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f));
      this.sequence.AppendInterval(2f);
      this.sequence.SetLoops<DG.Tweening.Sequence>(-1);
      this.sequence.Play<DG.Tweening.Sequence>();
      this.PulseSequence = DOTween.Sequence();
      this.PulseSequence.AppendCallback((TweenCallback) (() =>
      {
        this.WhitePulse.transform.localScale = Vector3.one;
        this.WhitePulse.transform.DOScale(Vector3.one * 1.25f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
        this.WhitePulse.color = new Color(1f, 1f, 1f, 0.8f);
        DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
        System.Action onPulse = CultFaithManager.OnPulse;
        if (onPulse == null)
          return;
        onPulse();
      }));
      this.PulseSequence.AppendInterval(2.5f);
      this.PulseSequence.SetLoops<DG.Tweening.Sequence>(-1);
      this.PulseSequence.Play<DG.Tweening.Sequence>();
    }
    else
    {
      if (this.sequence == null)
        return;
      this.sequence.Kill();
      this.sequence = (DG.Tweening.Sequence) null;
      this.PulseSequence.Kill();
      this.PulseSequence = (DG.Tweening.Sequence) null;
    }
  }

  public delegate void ThoughtEvent(Thought thought);
}
