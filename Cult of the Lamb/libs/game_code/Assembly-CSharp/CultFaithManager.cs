// Decompiled with JetBrains decompiler
// Type: CultFaithManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
  public CanvasGroup flash;
  [SerializeField]
  public CanvasGroup faithLock;
  public bool showing;
  public static System.Action OnPulse;
  [SerializeField]
  public Image faithImageHolder;
  [SerializeField]
  public Sprite faithImageWinterSprite;
  [SerializeField]
  public Sprite faithImageDefaultSprite;
  public const float STARTING_FAITH = 50f;
  public const float LOW_FAITH = 5f;
  public const float MIN_FAITH = 0.0f;
  public const float MAX_FAITH = 85f;
  public static Action<bool, NotificationData> OnUpdateFaith;
  public BarController BarController;
  public float delay = 0.835f;
  public static List<FollowerBrain> targetedFollowers = new List<FollowerBrain>();
  public bool activatedLock;
  public DG.Tweening.Sequence sequence;
  public DG.Tweening.Sequence PulseSequence;

  public event CultFaithManager.ThoughtEvent OnThoughtModified;

  public event CultFaithManager.ThoughtEvent OnThoughtAdded;

  [SerializeField]
  public static float makeDissenterDeltaTime => 3600f;

  public void Awake()
  {
    CultFaithManager.Instance = this;
    CultFaithManager.OnUpdateFaith += new Action<bool, NotificationData>(this.UpdateBar);
    this.OnThoughtModified += new CultFaithManager.ThoughtEvent(FollowerBrain.UpdateInactiveFollowersThought);
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
  }

  public void OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    if (newSeason == SeasonsManager.Season.Winter)
      this.faithImageHolder.sprite = this.faithImageWinterSprite;
    else
      this.faithImageHolder.sprite = this.faithImageDefaultSprite;
  }

  public void Pulse(float Duration = 1f, float Scale = 1.25f, float Alpha = 0.8f, Ease Ease = Ease.OutSine)
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * Scale, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease);
    this.WhitePulse.color = new Color(1f, 1f, 1f, Alpha);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, Duration).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease);
  }

  public void Flash()
  {
    this.flash.gameObject.SetActive(true);
    this.flash.alpha = 1f;
    this.flash.GetComponent<Image>().color = Color.white;
    DOTweenModuleUI.DOColor(this.flash.GetComponent<Image>(), StaticColors.RedColor, 0.33f);
    this.flash.DOFade(0.0f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.33f);
  }

  public void ReadThoughts()
  {
    Debug.Log((object) ("FAITH: " + CultFaithManager.CurrentFaith.ToString()));
    foreach (ThoughtData trackedThought in CultFaithManager.TrackedThoughts)
      Debug.Log((object) trackedThought.ThoughtType.ToString());
  }

  public static List<ThoughtData> TrackedThoughts
  {
    get => DataManager.Instance.Thoughts;
    set => DataManager.Instance.Thoughts = value;
  }

  public static bool HasThought(Thought thought)
  {
    foreach (ThoughtData trackedThought in CultFaithManager.TrackedThoughts)
    {
      if (trackedThought.ThoughtType == thought)
        return true;
    }
    return false;
  }

  public void OnEnable()
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

  public IEnumerator RevealRoutine()
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

  public void OnDestroy()
  {
    this.gameObject.transform.DOKill();
    this.faithLock.DOKill();
    this.WhitePulse.DOKill();
    this.flash.DOKill();
    this.CanvasGroup.DOKill();
    this.Container.transform.DOKill();
    if (this.sequence != null && this.sequence.active)
    {
      this.sequence.Kill();
      this.sequence = (DG.Tweening.Sequence) null;
    }
    if (this.PulseSequence != null && this.PulseSequence.active)
    {
      this.PulseSequence.Kill();
      this.PulseSequence = (DG.Tweening.Sequence) null;
    }
    CultFaithManager.OnUpdateFaith -= new Action<bool, NotificationData>(this.UpdateBar);
    this.OnThoughtModified -= new CultFaithManager.ThoughtEvent(FollowerBrain.UpdateInactiveFollowersThought);
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
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
    float num1 = 0.0f;
    if (FollowerBrainStats.BrainWashed)
    {
      Delta = 0.0f;
    }
    else
    {
      foreach (ThoughtData trackedThought in CultFaithManager.TrackedThoughts)
      {
        int num2 = -1;
        while (++num2 < trackedThought.Quantity)
          num1 += num2 <= 0 ? trackedThought.Modifier : (float) trackedThought.StackModifier;
      }
    }
    CultFaithManager.StaticFaith = Mathf.Clamp(CultFaithManager.StaticFaith + Delta, 0.0f, 85f);
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
    if (!DataManager.Instance.ShowCultFaith || DataManager.Instance.Followers.Count == 0 || DungeonSandboxManager.Active)
      return;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    data.FollowerID = FollowerID;
    data.Modifier *= faithMultiplier;
    if ((double) data.Warmth != 0.0)
    {
      if (data.ThoughtType == Thought.DancePit && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
        data.Warmth = 0.0f;
      else
        WarmthBar.ModifyWarmth(FollowerThoughts.GetNotificationOnLocalizationKey(data.ThoughtType) + "/Warmth", data.Warmth);
    }
    if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
    {
      CultFaithManager.ThoughtEvent onThoughtModified = CultFaithManager.Instance.OnThoughtModified;
      if (onThoughtModified != null)
        onThoughtModified(thought);
      CultFaithManager.ThoughtEvent onThoughtAdded = CultFaithManager.Instance.OnThoughtAdded;
      if (onThoughtAdded != null)
        onThoughtAdded(thought);
    }
    if (data.TrackThought)
    {
      int index = -1;
      while (++index < CultFaithManager.TrackedThoughts.Count)
      {
        ThoughtData trackedThought = CultFaithManager.TrackedThoughts[index];
        if (trackedThought.ThoughtGroup == data.ThoughtGroup && trackedThought.Stacking <= 0)
        {
          CultFaithManager.TrackedThoughts[index] = data;
          CultFaithManager.GetFaith(0.0f, trackedThought.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(trackedThought.ThoughtType), trackedThought.FollowerID, args);
          return;
        }
        if (trackedThought.ThoughtType == thought && trackedThought.Stacking > 0)
        {
          if (trackedThought.Quantity < trackedThought.Stacking)
          {
            ++trackedThought.Quantity;
          }
          else
          {
            trackedThought.CoolDowns.RemoveAt(0);
            trackedThought.TimeStarted.RemoveAt(0);
          }
          trackedThought.CoolDowns.Add(data.Duration);
          trackedThought.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
          CultFaithManager.GetFaith(0.0f, trackedThought.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(trackedThought.ThoughtType), trackedThought.FollowerID, args);
          return;
        }
      }
      CultFaithManager.TrackedThoughts.Add(data);
      CultFaithManager.GetFaith(0.0f, data.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(data.ThoughtType), data.FollowerID, args);
    }
    else
      CultFaithManager.GetFaith(data.Modifier, data.Modifier, true, CultFaithManager.GetNotificationFlair(thought), FollowerThoughts.GetNotificationOnLocalizationKey(data.ThoughtType), data.FollowerID, args);
  }

  public static NotificationBase.Flair GetNotificationFlair(Thought thought)
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
    foreach (ThoughtData trackedThought in CultFaithManager.TrackedThoughts)
    {
      if (trackedThought.ThoughtType == thought)
      {
        CultFaithManager.TrackedThoughts.Remove(trackedThought);
        if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
        {
          CultFaithManager.ThoughtEvent onThoughtModified = CultFaithManager.Instance.OnThoughtModified;
          if (onThoughtModified != null)
            onThoughtModified(trackedThought.ThoughtType);
        }
        GameManager.GetInstance().StartCoroutine((IEnumerator) CultFaithManager.DelayGetFaith(trackedThought));
        break;
      }
    }
  }

  public static IEnumerator DelayGetFaith(ThoughtData thought)
  {
    yield return (object) null;
    CultFaithManager.GetFaith(0.0f, 0.0f, true, CultFaithManager.GetNotificationFlair(thought.ThoughtType), FollowerThoughts.GetNotificationOffLocalizationKey(thought.ThoughtType));
  }

  public static void UpdateThoughts(float DeltaGameTime)
  {
    foreach (ThoughtData trackedThought in CultFaithManager.TrackedThoughts)
    {
      if ((double) trackedThought.Duration != -1.0 && (double) TimeManager.TotalElapsedGameTime - (double) trackedThought.TimeStarted[0] > (double) trackedThought.CoolDowns[0])
      {
        if (trackedThought.Quantity > 1)
        {
          --trackedThought.Quantity;
          trackedThought.CoolDowns.RemoveAt(0);
          trackedThought.TimeStarted.RemoveAt(0);
        }
        else
        {
          CultFaithManager.TrackedThoughts.Remove(trackedThought);
          if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
          {
            CultFaithManager.ThoughtEvent onThoughtModified = CultFaithManager.Instance.OnThoughtModified;
            if (onThoughtModified != null)
              onThoughtModified(trackedThought.ThoughtType);
          }
        }
        CultFaithManager.GetFaith(0.0f, 0.0f, true, CultFaithManager.GetNotificationFlair(trackedThought.ThoughtType), FollowerThoughts.GetNotificationOffLocalizationKey(trackedThought.ThoughtType));
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

  public void Start()
  {
    CultFaithManager.GetFaith(0.0f, 0.0f, false, NotificationBase.Flair.None);
    this.showing = DataManager.Instance.ShowCultFaith;
    this.OnSeasonChanged(SeasonsManager.CurrentSeason);
  }

  public static float CultFaithNormalised => CultFaithManager.CurrentFaith / 85f;

  public void UpdateBar(bool Animate, NotificationData NotificationData)
  {
    this.delay = 0.835f;
    if (SettingsManager.Settings.Game.PerformanceMode)
      Animate = false;
    this.BarController.SetBarSize(CultFaithManager.CultFaithNormalised, Animate, NotificationData: NotificationData);
  }

  public static void UpdateSimulation(float DeltaGameTime)
  {
    if (!DataManager.Instance.ShowCultFaith)
      return;
    CultFaithManager.UpdateThoughts(DeltaGameTime);
    if (!DataManager.Instance.OnboardedDissenter && PlayerFarming.Location != FollowerLocation.Base || TimeManager.IsNight || DataManager.Instance.Followers.Count == 0 || TimeManager.CurrentPhase == DayPhase.Dusk || (double) CultFaithManager.CurrentFaith / 85.0 >= 0.25 || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToStartDissenting < (double) CultFaithManager.makeDissenterDeltaTime)
      return;
    FollowerBrain.GetAvailableBrainsWithNecklaceTargeted(ref CultFaithManager.targetedFollowers);
    FollowerBrain followerBrain = FollowerBrain.RandomAvailableBrainNoCurseState(CultFaithManager.targetedFollowers.Count > 0 ? CultFaithManager.targetedFollowers : FollowerBrain.AllBrains);
    if (followerBrain != null && DataManager.Instance.OnboardedDissenter && followerBrain.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_Loyalty && !followerBrain.Info.HasTrait(FollowerTrait.TraitType.BishopOfCult))
    {
      followerBrain.MakeDissenter();
      DataManager.Instance.LastFollowerToStartDissenting = TimeManager.TotalElapsedGameTime;
      if (followerBrain.HasTrait(FollowerTrait.TraitType.Mutated))
        DataManager.Instance.LastFollowerToStartDissenting -= CultFaithManager.makeDissenterDeltaTime * 0.333f;
    }
    CultFaithManager.targetedFollowers.Clear();
  }

  public void TurnFaithLockOn()
  {
    this.activatedLock = true;
    this.faithLock.gameObject.SetActive(true);
    this.Flash();
    this.Pulse();
    this.faithLock.alpha = 1f;
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOShakePosition(2f, new Vector3(5f, 0.0f, 0.0f)).SetDelay<Tweener>(0.5f);
  }

  public void TurnFaithLockOff()
  {
    this.activatedLock = false;
    this.Flash();
    this.Pulse();
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOShakePosition(2f, new Vector3(5f, 0.0f, 0.0f));
    this.faithLock.DOFade(0.0f, 2f);
  }

  public void Update()
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
    if ((double) CultFaithManager.CurrentFaith / 85.0 < 0.25 && !SettingsManager.Settings.Game.PerformanceMode)
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
      if (this.sequence != null && this.sequence.active)
      {
        this.sequence.Kill();
        this.sequence = (DG.Tweening.Sequence) null;
      }
      if (this.PulseSequence == null || !this.PulseSequence.active)
        return;
      this.PulseSequence.Kill();
      this.PulseSequence = (DG.Tweening.Sequence) null;
    }
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__63_0()
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * 1.25f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.WhitePulse.color = new Color(1f, 1f, 1f, 0.8f);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
    System.Action onPulse = CultFaithManager.OnPulse;
    if (onPulse == null)
      return;
    onPulse();
  }

  public delegate void ThoughtEvent(Thought thought);
}
