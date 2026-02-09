// Decompiled with JetBrains decompiler
// Type: HungerBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HungerBar : MonoBehaviour
{
  public BarController BarController;
  public static HungerBar Instance;
  public CanvasGroup CanvasGroup;
  public GameObject Container;
  public Image WhitePulse;
  [SerializeField]
  public CanvasGroup flash;
  [SerializeField]
  public CanvasGroup Lock;
  public bool showing;
  public bool activatedLock;
  public static float reservedSatiation = 0.0f;
  public bool Revealing;
  public static List<FollowerBrain> targetedFollowers = new List<FollowerBrain>();
  public static float LastCount = 0.0f;
  public static float CountInterval = 15f;
  public static float ElapsedGameTime;
  public DG.Tweening.Sequence sequence;
  public DG.Tweening.Sequence PulseSequence;

  public static float makeHungryDeltaTime => 600f;

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

  public static float MIN_HUNGER => 0.0f;

  public static float MAX_HUNGER => 100f;

  public static float HungerNormalized => HungerBar.Count / HungerBar.MAX_HUNGER;

  public static float Count
  {
    get => DataManager.Instance.HungerBarCount;
    set => DataManager.Instance.HungerBarCount = value;
  }

  public static float ReservedSatiation
  {
    get => HungerBar.reservedSatiation;
    set
    {
      HungerBar.reservedSatiation = Mathf.Clamp(value, HungerBar.MIN_HUNGER, HungerBar.MAX_HUNGER);
    }
  }

  public void Start()
  {
    this.BarController.SetBarSize(HungerBar.HungerNormalized, false);
    this.showing = DataManager.Instance.ShowCultHunger;
  }

  public void OnEnable()
  {
    this.Lock.gameObject.SetActive(false);
    this.flash.gameObject.SetActive(false);
    this.activatedLock = false;
    HungerBar.Instance = this;
    this.Container.SetActive(DataManager.Instance.ShowCultHunger);
    this.StartCoroutine((IEnumerator) this.UpdateRoutine());
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  public IEnumerator RevealRoutine()
  {
    HungerBar hungerBar = this;
    while (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden)
      yield return (object) null;
    HungerBar.Count = 0.0f;
    int num = 0;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        ++num;
        HungerBar.Count += allBrain.Stats.Satiation;
        HungerBar.Count -= allBrain.Stats.Starvation;
      }
    }
    HungerBar.Count /= (float) num;
    yield return (object) new WaitForSeconds(0.5f);
    hungerBar.Revealing = true;
    AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
    Vector3 LocalPosition = hungerBar.Container.transform.localPosition;
    hungerBar.Container.transform.parent = HUD_Manager.Instance.transform;
    hungerBar.Container.transform.localPosition = Vector3.zero;
    hungerBar.Container.transform.parent = hungerBar.transform;
    DataManager.Instance.ShowCultHunger = true;
    hungerBar.CanvasGroup.alpha = 0.0f;
    hungerBar.CanvasGroup.DOFade(1f, 1f);
    hungerBar.Container.SetActive(true);
    hungerBar.Container.transform.localScale = Vector3.one * 3f;
    hungerBar.Container.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.4f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.6f);
    hungerBar.Container.transform.DOLocalMove(LocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.8f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    hungerBar.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    hungerBar.Revealing = false;
    hungerBar.showing = true;
  }

  public void OnDisable()
  {
    if (this.sequence != null && this.sequence.active)
      this.sequence.Kill();
    if (this.PulseSequence != null && this.PulseSequence.active)
      this.PulseSequence.Kill();
    this.Container.transform.DOKill();
    this.CanvasGroup.DOKill();
    this.WhitePulse.transform.DOKill();
    this.WhitePulse.DOKill();
    this.sequence = (DG.Tweening.Sequence) null;
    this.PulseSequence = (DG.Tweening.Sequence) null;
    if ((Object) HungerBar.Instance == (Object) this)
      HungerBar.Instance = (HungerBar) null;
    this.StopAllCoroutines();
  }

  public void Update()
  {
    if (!DataManager.Instance.ShowCultHunger)
      return;
    if (this.showing && DataManager.Instance.Followers.Count <= 0)
    {
      this.showing = false;
      this.CanvasGroup.DOFade(0.0f, 1f);
    }
    else
    {
      if (this.showing || DataManager.Instance.Followers.Count <= 0)
        return;
      this.showing = true;
      this.CanvasGroup.DOFade(1f, 1f);
    }
  }

  public static void UpdateSimulation(float DeltaGameTime)
  {
    if (!DataManager.Instance.ShowCultHunger || (Object) HungerBar.Instance != (Object) null && HungerBar.Instance.Revealing)
      return;
    HungerBar.GetCount(DeltaGameTime);
    if (DataManager.Instance.Followers.Count <= 0 || (double) HungerBar.HungerNormalized >= 0.25 || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToStartStarving < (double) HungerBar.makeHungryDeltaTime)
      return;
    FollowerBrain.GetAvailableBrainsWithNecklaceTargeted(ref HungerBar.targetedFollowers);
    FollowerBrain followerBrain = FollowerBrain.RandomAvailableBrainNoCurseState(HungerBar.targetedFollowers.Count > 0 ? HungerBar.targetedFollowers : FollowerBrain.AllBrains);
    if (followerBrain != null)
    {
      followerBrain.MakeStarve();
      DataManager.Instance.LastFollowerToStartStarving = TimeManager.TotalElapsedGameTime;
      if (followerBrain.HasTrait(FollowerTrait.TraitType.Mutated))
        DataManager.Instance.LastFollowerToStartStarving -= HungerBar.makeHungryDeltaTime * 0.5f;
    }
    HungerBar.targetedFollowers.Clear();
  }

  public static void GetCount(float DeltaGameTime)
  {
    HungerBar.ElapsedGameTime += DeltaGameTime;
    if ((double) HungerBar.ElapsedGameTime <= (double) HungerBar.CountInterval)
      return;
    HungerBar.ElapsedGameTime = 0.0f;
    double count = (double) HungerBar.Count;
    HungerBar.Count = 0.0f;
    int num = 0;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID, true))
      {
        ++num;
        HungerBar.Count += allBrain.Stats.Satiation;
        HungerBar.Count -= allBrain.Stats.Starvation;
      }
    }
    HungerBar.Count /= (float) num;
  }

  public IEnumerator UpdateRoutine()
  {
    HungerBar hungerBar = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while (!DataManager.Instance.ShowCultHunger)
        yield return (object) null;
      HungerBar.GetCount(TimeManager.DeltaGameTime);
      if ((Object) HungerBar.Instance != (Object) null)
        HungerBar.Instance.BarController.SetBarSize(HungerBar.HungerNormalized, true);
      if (Time.frameCount % 5 == 0)
      {
        if (FollowerBrainStats.Fasting && !hungerBar.activatedLock)
          hungerBar.TurnLockOn();
        else if (!FollowerBrainStats.Fasting && hungerBar.activatedLock)
          hungerBar.TurnLockOff();
      }
      if ((double) HungerBar.HungerNormalized < 0.25 && !SettingsManager.Settings.Game.PerformanceMode)
      {
        if (hungerBar.sequence == null)
        {
          hungerBar.sequence = DOTween.Sequence();
          hungerBar.sequence.Append((Tween) hungerBar.BarController.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f));
          hungerBar.sequence.AppendInterval(2f);
          hungerBar.sequence.SetLoops<DG.Tweening.Sequence>(-1);
          hungerBar.sequence.Play<DG.Tweening.Sequence>();
          hungerBar.PulseSequence = DOTween.Sequence();
          hungerBar.PulseSequence.AppendCallback(new TweenCallback(hungerBar.\u003CUpdateRoutine\u003Eb__43_0));
          hungerBar.PulseSequence.AppendInterval(2.5f);
          hungerBar.PulseSequence.SetLoops<DG.Tweening.Sequence>(-1);
          hungerBar.PulseSequence.Play<DG.Tweening.Sequence>();
        }
      }
      else
      {
        if (hungerBar.sequence != null && hungerBar.sequence.active)
        {
          hungerBar.sequence.Kill();
          hungerBar.sequence = (DG.Tweening.Sequence) null;
        }
        if (hungerBar.PulseSequence != null && hungerBar.PulseSequence.active)
        {
          hungerBar.PulseSequence.Kill();
          hungerBar.PulseSequence = (DG.Tweening.Sequence) null;
        }
      }
      yield return (object) new WaitForSeconds(0.835f);
    }
  }

  [CompilerGenerated]
  public void \u003CUpdateRoutine\u003Eb__43_0()
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.WhitePulse.color = new Color(1f, 1f, 1f, 0.8f);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
  }
}
