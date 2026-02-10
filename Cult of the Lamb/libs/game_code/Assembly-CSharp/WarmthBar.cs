// Decompiled with JetBrains decompiler
// Type: WarmthBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class WarmthBar : MonoBehaviour
{
  public BarController BarController;
  public static WarmthBar Instance;
  public CanvasGroup CanvasGroup;
  public GameObject Container;
  public Image WhitePulse;
  [SerializeField]
  public CanvasGroup flash;
  [SerializeField]
  public CanvasGroup Lock;
  [SerializeField]
  public CanvasGroup IceOverlay;
  public bool iceVisible = true;
  public Tween iceTween;
  public UnityEvent<bool> ShowingChanged;
  public bool showing;
  public bool activatedLock;
  [CompilerGenerated]
  public static float \u003CWarmthNormalized_Uncalculated\u003Ek__BackingField = float.MaxValue;
  public DG.Tweening.Sequence sequence;
  public DG.Tweening.Sequence PulseSequence;
  public DG.Tweening.Sequence OutOfFuelSequence;
  public float OutOfFuelTimer;

  public static float MIN_WARMTH => 0.0f;

  public static float MAX_WARMTH => 100f;

  public static float WarmthNormalized
  {
    get
    {
      if (FollowerBrainStats.IsWarmthRitual || FollowerBrainStats.LockedWarmth)
        return 1f;
      if ((Object) Interaction_DLCFurnace.Instance != (Object) null && Interaction_DLCFurnace.Instance.Structure.Brain != null)
        return (float) Interaction_DLCFurnace.Instance.Structure.Brain.Data.Fuel / (float) Interaction_DLCFurnace.Instance.Structure.Brain.Data.MaxFuel;
      List<Structures_Furnace> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Furnace>();
      return structuresOfType.Count > 0 ? (float) structuresOfType[0].Data.Fuel / (float) structuresOfType[0].Data.MaxFuel : 0.0f;
    }
  }

  public static float WarmthNormalized_Uncalculated
  {
    get => WarmthBar.\u003CWarmthNormalized_Uncalculated\u003Ek__BackingField;
    set => WarmthBar.\u003CWarmthNormalized_Uncalculated\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.BarController.SetBarSize(WarmthBar.WarmthNormalized, false);
    this.showing = DataManager.Instance.ShowCultWarmth;
  }

  public void OnDestroy() => WarmthBar.WarmthNormalized_Uncalculated = float.MaxValue;

  public void OnEnable()
  {
    this.activatedLock = false;
    this.TurnLockOff();
    WarmthBar.Instance = this;
    this.Container.SetActive(DataManager.Instance.ShowCultWarmth);
    this.StartCoroutine((IEnumerator) this.UpdateRoutine());
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  public IEnumerator RevealRoutine()
  {
    WarmthBar warmthBar = this;
    while (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
    Vector3 LocalPosition = warmthBar.Container.transform.localPosition;
    warmthBar.Container.transform.parent = HUD_Manager.Instance.transform;
    warmthBar.Container.transform.localPosition = Vector3.zero;
    warmthBar.Container.transform.parent = warmthBar.transform;
    warmthBar.CanvasGroup.alpha = 0.0f;
    warmthBar.CanvasGroup.DOFade(1f, 1f);
    warmthBar.Container.SetActive(true);
    warmthBar.Container.transform.localScale = Vector3.one * 3f;
    warmthBar.Container.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.4f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.6f);
    warmthBar.Container.transform.DOLocalMove(LocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.8f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    warmthBar.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    warmthBar.showing = true;
    DataManager.Instance.ShowCultWarmth = true;
  }

  public void OnDisable()
  {
    if (this.sequence != null)
      this.sequence.Kill();
    if (this.PulseSequence != null)
      this.PulseSequence.Kill();
    this.Container.transform.DOKill();
    this.CanvasGroup.DOKill();
    this.WhitePulse.transform.DOKill();
    this.WhitePulse.DOKill();
    this.sequence = (DG.Tweening.Sequence) null;
    this.PulseSequence = (DG.Tweening.Sequence) null;
    if ((Object) WarmthBar.Instance == (Object) this)
      WarmthBar.Instance = (WarmthBar) null;
    this.StopAllCoroutines();
  }

  public void Update()
  {
    if (!DataManager.Instance.ShowCultWarmth)
      return;
    if (this.showing && (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || DataManager.Instance.Followers.Count < 1))
    {
      this.showing = false;
      this.ShowingChanged.Invoke(this.showing);
      this.CanvasGroup.DOFade(0.0f, 1f);
    }
    else if (!this.showing && DataManager.Instance.Followers.Count > 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      this.showing = true;
      this.ShowingChanged.Invoke(this.showing);
      this.CanvasGroup.DOFade(1f, 1f);
    }
    int num1 = SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter ? 1 : 0;
    bool isWarmthRitual = FollowerBrainStats.IsWarmthRitual;
    bool lockedWarmth = FollowerBrainStats.LockedWarmth;
    int num2 = (!((Object) Interaction_DLCFurnace.Instance != (Object) null) || !((Object) Interaction_DLCFurnace.Instance.Structure != (Object) null) ? 0 : (Interaction_DLCFurnace.Instance.Structure.Brain != null ? 1 : 0)) != 0 ? Interaction_DLCFurnace.Instance.Structure.Brain.Data.Fuel : int.MaxValue;
    bool flag = num1 != 0 && !isWarmthRitual && !lockedWarmth && num2 <= 0;
    if (flag)
    {
      this.OutOfFuelTimer -= Time.deltaTime;
      if ((double) this.OutOfFuelTimer < 0.0)
      {
        this.WhitePulse.DOKill();
        this.WhitePulse.transform.DOScale(Vector3.one * 1.5f, 1f).From<Vector3, Vector3, VectorOptions>(Vector3.one).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
        DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).From(0.8f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
        this.OutOfFuelTimer = 2.5f;
      }
    }
    if (flag && !this.iceVisible && this.iceTween == null)
    {
      Tween iceTween = this.iceTween;
      if (iceTween != null)
        iceTween.Kill();
      this.IceOverlay.gameObject.SetActive(true);
      this.IceOverlay.alpha = 0.0f;
      this.iceTween = (Tween) this.IceOverlay.DOFade(1f, 1f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.iceVisible = true;
        this.iceTween = (Tween) null;
      }));
    }
    else
    {
      if (flag || !this.iceVisible || this.iceTween != null)
        return;
      Tween iceTween = this.iceTween;
      if (iceTween != null)
        iceTween.Kill();
      this.iceTween = (Tween) this.IceOverlay.DOFade(0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.iceVisible = false;
        this.IceOverlay.gameObject.SetActive(false);
        this.iceTween = (Tween) null;
      }));
    }
  }

  public static void ModifyWarmth(string loc, float amount)
  {
  }

  public static float GetCount()
  {
    float count = 0.0f;
    foreach (ThoughtData trackedThought in CultFaithManager.TrackedThoughts)
    {
      int num = -1;
      while (++num < trackedThought.Quantity)
        count += trackedThought.Warmth;
    }
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (StructureManager.GetBuildingWarmth(allBrain) > 0)
        count += (float) StructureManager.GetBuildingWarmth(allBrain);
    }
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      count -= 20f;
    if (TimeManager.IsNight)
      count -= 10f;
    double num1 = (double) count / (double) WarmthBar.MAX_WARMTH;
    double normalizedUncalculated = (double) WarmthBar.WarmthNormalized_Uncalculated;
    WarmthBar.WarmthNormalized_Uncalculated = (float) num1;
    return count;
  }

  public static void ForceCountUpdate()
  {
    DataManager.Instance.WarmthBarCount = WarmthBar.GetCount();
  }

  public static void UpdateSimulation(float DeltaGameTime)
  {
    int num = DataManager.Instance.ShowCultFaith ? 1 : 0;
  }

  public IEnumerator UpdateRoutine()
  {
    WarmthBar warmthBar = this;
    yield return (object) new WaitForEndOfFrame();
    float previousSize = 0.0f;
    while (true)
    {
      while (!DataManager.Instance.ShowCultWarmth)
        yield return (object) null;
      if (FollowerBrainStats.LockedWarmth && !warmthBar.activatedLock)
        warmthBar.TurnLockOn();
      else if (!FollowerBrainStats.LockedWarmth && warmthBar.activatedLock)
        warmthBar.TurnLockOff();
      bool Animate = (double) WarmthBar.WarmthNormalized > (double) previousSize;
      if (SettingsManager.Settings.Game.PerformanceMode)
        Animate = false;
      warmthBar.BarController.SetBarSize(WarmthBar.WarmthNormalized, Animate);
      if ((double) WarmthBar.WarmthNormalized < 0.25 & Animate)
      {
        if (warmthBar.sequence == null)
        {
          warmthBar.sequence = DOTween.Sequence();
          warmthBar.sequence.Append((Tween) warmthBar.BarController.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f));
          warmthBar.sequence.SetLoops<DG.Tweening.Sequence>(-1);
          warmthBar.sequence.Play<DG.Tweening.Sequence>();
          warmthBar.PulseSequence = DOTween.Sequence();
          warmthBar.PulseSequence.AppendCallback(new TweenCallback(warmthBar.\u003CUpdateRoutine\u003Eb__36_0));
          warmthBar.PulseSequence.AppendInterval(0.5f);
          warmthBar.PulseSequence.SetLoops<DG.Tweening.Sequence>(-1);
          warmthBar.PulseSequence.Play<DG.Tweening.Sequence>();
        }
      }
      else
      {
        if (warmthBar.sequence != null && warmthBar.sequence.active)
        {
          warmthBar.sequence.Kill();
          warmthBar.sequence = (DG.Tweening.Sequence) null;
        }
        if (warmthBar.PulseSequence != null && warmthBar.PulseSequence.active)
        {
          warmthBar.PulseSequence.Kill();
          warmthBar.PulseSequence = (DG.Tweening.Sequence) null;
        }
      }
      previousSize = WarmthBar.WarmthNormalized;
      yield return (object) new WaitForSeconds(0.3f);
    }
  }

  public void DoPulse()
  {
    this.OutOfFuelSequence = DOTween.Sequence();
    this.OutOfFuelSequence.Append((Tween) this.WhitePulse.transform.DOScale(Vector3.one * 2f, 1f).From<Vector3, Vector3, VectorOptions>(Vector3.one).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine));
    this.OutOfFuelSequence.Join((Tween) DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).From(0.8f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine));
    this.OutOfFuelSequence.AppendInterval(3f);
    this.OutOfFuelSequence.SetLoops<DG.Tweening.Sequence>(-1).Play<DG.Tweening.Sequence>();
  }

  public void KillPulse()
  {
    this.OutOfFuelSequence.Kill();
    this.WhitePulse.color = this.WhitePulse.color with
    {
      a = 0.0f
    };
  }

  public void TurnLockOn()
  {
    this.activatedLock = true;
    this.Lock.gameObject.SetActive(true);
    this.Flash();
    this.Pulse();
    this.Lock.DOKill();
    this.Lock.alpha = 1f;
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOShakePosition(2f, new Vector3(5f, 0.0f, 0.0f)).SetDelay<Tweener>(0.5f);
  }

  public void TurnLockOff()
  {
    this.activatedLock = false;
    this.Flash();
    this.Pulse();
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOShakePosition(2f, new Vector3(5f, 0.0f, 0.0f));
    this.Lock.DOFade(0.0f, 2f);
  }

  public void Pulse(float Duration = 1f, float Scale = 1.5f, float Alpha = 0.8f, Ease Ease = Ease.OutSine)
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

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__29_0()
  {
    this.iceVisible = true;
    this.iceTween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__29_1()
  {
    this.iceVisible = false;
    this.IceOverlay.gameObject.SetActive(false);
    this.iceTween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CUpdateRoutine\u003Eb__36_0()
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.WhitePulse.color = new Color(1f, 1f, 1f, 0.8f);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
  }
}
