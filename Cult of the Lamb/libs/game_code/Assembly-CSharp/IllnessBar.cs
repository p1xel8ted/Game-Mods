// Decompiled with JetBrains decompiler
// Type: IllnessBar
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
public class IllnessBar : MonoBehaviour
{
  public BarController BarController;
  public static IllnessBar Instance;
  public GameObject Container;
  public CanvasGroup CanvasGroup;
  public Image WhitePulse;
  public bool showing;
  public static float makeIllDeltaTime = 600f;
  public static List<FollowerBrain> targetedFollowers = new List<FollowerBrain>();
  public DG.Tweening.Sequence sequence;
  public DG.Tweening.Sequence PulseSequence;
  public static int frameCounter;

  public void Pulse(float Duration = 1f, float Scale = 1.5f, float Alpha = 0.8f, Ease Ease = Ease.OutSine)
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * Scale, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease);
    this.WhitePulse.color = new Color(1f, 1f, 1f, Alpha);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, Duration).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease);
  }

  public static float Max => Mathf.Max(10f, IllnessBar.DynamicMax);

  public static float DynamicMax
  {
    get => DataManager.Instance.IllnessBarDynamicMax;
    set => DataManager.Instance.IllnessBarDynamicMax = value;
  }

  public static float Count
  {
    get => DataManager.Instance.IllnessBarCount;
    set => DataManager.Instance.IllnessBarCount = value;
  }

  public static float IllnessNormalized
  {
    get => (float) (1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max);
  }

  public void Start()
  {
    this.BarController.SetBarSize((float) (1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max), false);
    TimeManager.OnNewPhaseStarted += new System.Action(this.DecreaseDynamicMax);
    this.showing = DataManager.Instance.ShowCultIllness;
  }

  public void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.DecreaseDynamicMax);

  public void OnEnable()
  {
    IllnessBar.Instance = this;
    this.Container.SetActive(DataManager.Instance.ShowCultIllness);
    this.StartCoroutine((IEnumerator) this.UpdateRoutine());
  }

  public void Update()
  {
    if (!DataManager.Instance.ShowCultIllness)
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

  public void DecreaseDynamicMax()
  {
    if ((double) IllnessBar.Count >= (double) IllnessBar.DynamicMax)
      return;
    IllnessBar.DynamicMax = Mathf.Clamp(IllnessBar.DynamicMax - 1f, 10f, IllnessBar.DynamicMax);
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  public IEnumerator RevealRoutine()
  {
    IllnessBar illnessBar = this;
    while (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden)
      yield return (object) null;
    if (!DataManager.Instance.ShowCultIllness)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
      Vector3 LocalPosition = illnessBar.Container.transform.localPosition;
      illnessBar.Container.transform.parent = HUD_Manager.Instance.transform;
      illnessBar.Container.transform.localPosition = Vector3.zero;
      illnessBar.Container.transform.parent = illnessBar.transform;
      DataManager.Instance.ShowCultIllness = true;
      illnessBar.CanvasGroup.alpha = 0.0f;
      illnessBar.CanvasGroup.DOFade(1f, 1f);
      illnessBar.Container.SetActive(true);
      illnessBar.Container.transform.localScale = Vector3.one * 3f;
      illnessBar.Container.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      yield return (object) new WaitForSeconds(0.4f);
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
      yield return (object) new WaitForSeconds(0.6f);
      illnessBar.Container.transform.DOLocalMove(LocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
      yield return (object) new WaitForSeconds(0.8f);
      AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
      illnessBar.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
      yield return (object) new WaitForSeconds(0.5f);
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    }
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
    if ((UnityEngine.Object) IllnessBar.Instance == (UnityEngine.Object) this)
      IllnessBar.Instance = (IllnessBar) null;
    this.StopAllCoroutines();
  }

  public static void UpdateSimulation(float DeltaGameTime)
  {
    if (!DataManager.Instance.ShowCultIllness)
      return;
    ++IllnessBar.frameCounter;
    if (IllnessBar.frameCounter < 30)
      return;
    IllnessBar.frameCounter = 0;
    double num = (double) IllnessBar.UpdateCount(DeltaGameTime);
    if (!DataManager.Instance.OnboardedSickFollower && PlayerFarming.Location != FollowerLocation.Base || TimeManager.IsNight || DataManager.Instance.Followers.Count == 0 || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToBecomeIll < (double) IllnessBar.makeIllDeltaTime / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier() || 1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max >= 0.25 || !DataManager.Instance.OnboardedSickFollower)
      return;
    FollowerBrain.GetAvailableBrainsWithNecklaceTargeted(ref IllnessBar.targetedFollowers);
    FollowerBrain followerBrain = FollowerBrain.RandomAvailableBrainNoCurseState(IllnessBar.targetedFollowers.Count > 0 ? IllnessBar.targetedFollowers : FollowerBrain.AllBrains);
    if (followerBrain != null)
    {
      DataManager.Instance.LastFollowerToBecomeIll = TimeManager.TotalElapsedGameTime;
      if (followerBrain.HasTrait(FollowerTrait.TraitType.Mutated))
        DataManager.Instance.LastFollowerToBecomeIll -= (float) ((double) IllnessBar.makeIllDeltaTime / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier() * 0.5);
      followerBrain.MakeSick();
    }
    IllnessBar.targetedFollowers.Clear();
  }

  public static float UpdateCount(float deltaGameTime)
  {
    if (DataManager.Instance == null || (UnityEngine.Object) BaseLocationManager.Instance == (UnityEngine.Object) null || !DataManager.Instance.ShowCultIllness || !BaseLocationManager.Instance.StructuresPlaced)
      return IllnessBar.Max;
    double count = (double) IllnessBar.Count;
    IllnessBar.Count = (float) StructureManager.GetWasteCount();
    if ((double) IllnessBar.Count > (double) IllnessBar.DynamicMax)
      IllnessBar.DynamicMax = IllnessBar.Count;
    return IllnessBar.Count;
  }

  public IEnumerator UpdateRoutine()
  {
    IllnessBar illnessBar = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while (!DataManager.Instance.ShowCultIllness)
        yield return (object) null;
      bool Animate = true;
      if (SettingsManager.Settings.Game.PerformanceMode)
        Animate = false;
      illnessBar.BarController.SetBarSize((float) (1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max), Animate);
      if (1.0 - (double) IllnessBar.Count / (double) IllnessBar.Max < 0.25 & Animate)
      {
        if (illnessBar.sequence == null)
        {
          illnessBar.sequence = DOTween.Sequence();
          illnessBar.sequence.Append((Tween) illnessBar.BarController.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f));
          illnessBar.sequence.AppendInterval(2f);
          illnessBar.sequence.SetLoops<DG.Tweening.Sequence>(-1);
          illnessBar.sequence.Play<DG.Tweening.Sequence>();
          illnessBar.PulseSequence = DOTween.Sequence();
          illnessBar.PulseSequence.AppendCallback(new TweenCallback(illnessBar.\u003CUpdateRoutine\u003Eb__32_0));
          illnessBar.PulseSequence.AppendInterval(2.5f);
          illnessBar.PulseSequence.SetLoops<DG.Tweening.Sequence>(-1);
          illnessBar.PulseSequence.Play<DG.Tweening.Sequence>();
        }
      }
      else
      {
        if (illnessBar.sequence != null && illnessBar.sequence.active)
        {
          illnessBar.sequence.Kill();
          illnessBar.sequence = (DG.Tweening.Sequence) null;
        }
        if (illnessBar.PulseSequence != null && illnessBar.PulseSequence.active)
        {
          illnessBar.PulseSequence.Kill();
          illnessBar.PulseSequence = (DG.Tweening.Sequence) null;
        }
      }
      yield return (object) new WaitForSeconds(0.835f);
    }
  }

  [CompilerGenerated]
  public void \u003CUpdateRoutine\u003Eb__32_0()
  {
    this.WhitePulse.transform.localScale = Vector3.one;
    this.WhitePulse.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.WhitePulse.color = new Color(1f, 1f, 1f, 0.8f);
    DOTweenModuleUI.DOFade(this.WhitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
  }
}
