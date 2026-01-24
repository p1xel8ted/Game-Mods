// Decompiled with JetBrains decompiler
// Type: HUD_Time
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMTools;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Time : BaseMonoBehaviour
{
  public static HUD_Time Instance;
  public const float defaultFillAmount = 0.21f;
  public const float defaultZRotation = -51.895f;
  public const float logngNightZRotation = -14f;
  public Vector3 StartPos;
  public Vector3 MovePos;
  public TextMeshProUGUI DayLabel;
  public Transform Clockhand;
  [SerializeField]
  public TextMeshProUGUI _sun;
  [SerializeField]
  public TextMeshProUGUI _moon;
  [Header("Red Overlay")]
  [SerializeField]
  public Image _redOverlay;
  [SerializeField]
  public Color _nightTimeRed;
  [SerializeField]
  public Color _defaultRed;
  [SerializeField]
  public CanvasGroup _speedUpTime;
  [Header("Winter")]
  [SerializeField]
  public Image _outline;
  [SerializeField]
  public Sprite _winterActive;
  [SerializeField]
  public Sprite _default;
  [SerializeField]
  public Image _clockHand;
  [SerializeField]
  public HUD_Time.PhaseIconPosition[] _phaseIconPositions;
  [SerializeField]
  public HUD_Winter _winterTimer;
  public bool timescaleChanged;

  public void Awake() => HUD_Time.Instance = this;

  public void OnDestroy() => HUD_Time.Instance = (HUD_Time) null;

  public void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.Init);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
    this.DayLabel.text = $"{TimeManager.CurrentDay}";
    if (SaveAndLoad.Loaded)
      this.Init();
    this._speedUpTime.alpha = 0.0f;
  }

  public void Init()
  {
    this.OnNewDay();
    this.OnSeasonChanged(SeasonsManager.CurrentSeason);
    if (TimeManager.IsLongNight)
    {
      this._redOverlay.fillAmount *= 2f;
      this._redOverlay.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -14f);
    }
    else
    {
      this._redOverlay.fillAmount = 0.21f;
      this._redOverlay.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -51.895f);
    }
    foreach (HUD_Time.PhaseIconPosition phaseIconPosition in this._phaseIconPositions)
      ((RectTransform) phaseIconPosition.Icon.transform).anchoredPosition = (Vector2) (TimeManager.IsLongNight ? phaseIconPosition.LongNightPos : phaseIconPosition.NormalPos);
    this.OnNewPhaseStarted();
  }

  public void Update()
  {
    if (TimeManager.IsLongNight)
    {
      if (TimeManager.CurrentPhase == DayPhase.Night)
      {
        this.Clockhand.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(-260f, -410f, TimeManager.CurrentPhaseProgress));
      }
      else
      {
        float currentGameTime = TimeManager.CurrentGameTime;
        int num1 = 960;
        int num2 = 576;
        this.Clockhand.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(-50f, -260f, (currentGameTime - (float) (num2 / num1)) / (float) num1));
      }
    }
    else
      this.Clockhand.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(0.0f, -360f, TimeManager.CurrentDayProgress));
    if ((double) Time.timeScale > 1.0)
    {
      if (this.timescaleChanged)
        return;
      this._speedUpTime.DOFade(1f, 0.5f);
      this.timescaleChanged = true;
    }
    else
    {
      if (!this.timescaleChanged)
        return;
      this._speedUpTime.DOFade(0.0f, 0.5f);
      this.timescaleChanged = false;
    }
  }

  public void OnNewDay()
  {
    this.DayLabel.text = $"{TimeManager.CurrentDay}";
    this.DayLabel.transform.DOKill();
    this.DayLabel.transform.DOShakeScale(0.75f, 0.5f);
  }

  public void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.Init);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
  }

  public void OnNewPhaseStarted()
  {
    this._redOverlay.DOKill();
    if (TimeManager.CurrentPhase == DayPhase.Night)
    {
      DOTweenModuleUI.DOColor(this._redOverlay, this._nightTimeRed, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this._moon.DOComplete();
      ShortcutExtensionsTMPText.DOFade(this._moon, 0.5f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this._moon.DOScale(1f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._sun.DOComplete();
      ShortcutExtensionsTMPText.DOFade(this._sun, 1f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this._sun.DOScale(1.1f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    else
    {
      DOTweenModuleUI.DOColor(this._redOverlay, this._defaultRed, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this._moon.DOComplete();
      ShortcutExtensionsTMPText.DOFade(this._moon, 1f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this._moon.DOScale(1.1f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._sun.DOComplete();
      ShortcutExtensionsTMPText.DOFade(this._sun, 0.5f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this._sun.DOScale(1f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
  }

  public void OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    this._outline.sprite = newSeason == SeasonsManager.Season.Winter ? this._winterActive : this._default;
    if (newSeason == SeasonsManager.Season.Winter)
      return;
    this._redOverlay.DOFillAmount(0.21f, 0.0f);
    this._redOverlay.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, -51.895f), 0.0f);
  }

  public void LongNightSequence()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.LongNightIE());
  }

  public IEnumerator LongNightIE()
  {
    HUD_Time hudTime = this;
    yield return (object) new WaitForSeconds(2f);
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.Location != FollowerLocation.Base || PlayerFarming.AnyPlayerGotoAndStopping() || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || SimulationManager.IsPaused || MMConversation.isPlaying || LetterBox.IsPlaying || MMTransition.IsPlaying || HUD_Manager.Instance.Hidden || HUD_Manager.IsTransitioning)
      yield return (object) null;
    PlayerFarming.StopRidingOnAnimals();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    foreach (CanvasGroup canvasGroup1 in hudTime.GetComponentsInParent<CanvasGroup>())
      canvasGroup1.alpha = 1f;
    DataManager.Instance.OnboardedLongNights = true;
    DataManager.Instance.LongNightActive = true;
    RectTransform t = (RectTransform) hudTime.transform;
    Vector3 previousPos = t.position;
    t.position = new Vector3((float) Screen.width / 2f, (float) Screen.height / 2f);
    RectTransform rectTransform1 = t;
    rectTransform1.localScale = rectTransform1.localScale * 2f;
    hudTime._winterTimer.severityIconsContainer.alpha = 0.0f;
    CanvasGroup canvasGroup = hudTime.GetComponent<CanvasGroup>();
    canvasGroup.alpha = 0.0f;
    canvasGroup.DOFade(1f, 1f);
    yield return (object) new WaitForSeconds(1f);
    hudTime._redOverlay.DOFillAmount(0.42f, 3f);
    hudTime._redOverlay.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, -14f), 3f);
    foreach (HUD_Time.PhaseIconPosition phaseIconPosition in hudTime._phaseIconPositions)
      ((RectTransform) phaseIconPosition.Icon.transform).DOAnchorPos((Vector2) phaseIconPosition.LongNightPos, 3f);
    yield return (object) new WaitForSeconds(4f);
    canvasGroup.DOFade(0.0f, 1f);
    yield return (object) new WaitForSeconds(1f);
    hudTime._winterTimer.severityIconsContainer.alpha = 1f;
    canvasGroup.DOFade(1f, 1f);
    RectTransform rectTransform2 = t;
    rectTransform2.localScale = rectTransform2.localScale / 2f;
    t.position = previousPos;
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Midwinter))
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Midwinter, callback: (System.Action) (() => PlayerFarming.SetStateForAllPlayers()));
    else
      PlayerFarming.SetStateForAllPlayers();
  }

  [Serializable]
  public struct PhaseIconPosition
  {
    public GameObject Icon;
    public Vector3 NormalPos;
    public Vector3 LongNightPos;
  }
}
