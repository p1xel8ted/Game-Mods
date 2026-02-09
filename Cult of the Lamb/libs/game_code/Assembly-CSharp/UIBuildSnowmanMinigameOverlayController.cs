// Decompiled with JetBrains decompiler
// Type: UIBuildSnowmanMinigameOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Coffee.UIExtensions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIBuildSnowmanMinigameOverlayController : UIMenuBase
{
  public static int FillColorLerpFade = Shader.PropertyToID("_FillColorLerpFade");
  public static int FillColor = Shader.PropertyToID("_FillColor");
  public System.Action OnFail;
  public Action<int> OnSucceed;
  [Header("General")]
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public Transform snowballIndicator;
  [SerializeField]
  public Image snowballIndicatorImage;
  [SerializeField]
  public Image snowballIndicatorImageOutline;
  [SerializeField]
  public CanvasGroup snowballCanvasGroup;
  [SerializeField]
  public GameObject snowballDropIndicator;
  [SerializeField]
  public Transform overlayContainer;
  [SerializeField]
  public float perfectShotError = 30f;
  [SerializeField]
  public float speed;
  [SerializeField]
  public float gameWidth;
  [SerializeField]
  public GameObject button;
  [SerializeField]
  public Transform leftPos;
  [SerializeField]
  public Transform rightPos;
  [Header("Snowball")]
  [SerializeField]
  public Transform snowballContainer;
  [SerializeField]
  public GameObject snowballTemplate;
  [SerializeField]
  public Transform startPosition;
  [SerializeField]
  public Transform endPosition;
  [SerializeField]
  public float snowballSizeY = 150f;
  [SerializeField]
  public List<Sprite> snowballSprites = new List<Sprite>();
  [SerializeField]
  public AnimationCurve curve;
  [Header("Timer Bar")]
  [SerializeField]
  public Image timerBar;
  [SerializeField]
  public GameObject timerContainer;
  [Header("Snowball")]
  [SerializeField]
  public CanvasGroup accuracyCanvasGroup;
  [SerializeField]
  public TextMeshProUGUI accuracyAmountText;
  [Header("Effects")]
  [SerializeField]
  public UIParticle hitEffect;
  [SerializeField]
  public UIParticle perfectEffect;
  [SerializeField]
  public TextMeshProUGUI perfectText;
  [SerializeField]
  public TextMeshProUGUI failText;
  [SerializeField]
  public Material snowBallMaterial;
  [SerializeField]
  public Transform shakeContainer;
  public List<float> snowballPositionXList = new List<float>();
  public float indicatorTime;
  public bool isSnowballSequencePlaying;
  public int numberOfSnowballs;
  public int perfectShotsScore;
  public const int FAIL_OFFSET = 20;
  public int level;
  public float timer;
  public const float TOTAL_TIME = 15f;
  public float delay;
  public bool abortRequested;
  public bool accuracyVisible;
  public DG.Tweening.Sequence indicatorTween;
  public bool startSequencePlayed;
  public bool shownNegativeFeedback = true;
  public bool timerShaking;
  public bool canDrop;
  public bool failed;
  public int count;
  public List<GameObject> snowBallsList = new List<GameObject>();
  public int snowballCount;
  public int accuracySum;
  public int shots;
  [Header("Gizmos")]
  [SerializeField]
  public bool showGizmos = true;
  [SerializeField]
  public Color pathColor = new Color(0.0f, 0.8f, 1f, 1f);
  [SerializeField]
  public Color currentColor = new Color(1f, 0.85f, 0.0f, 1f);
  [SerializeField]
  [Tooltip("Sphere size relative to path length")]
  public float gizmoSizeFactor = 0.04f;

  public float normalisedTime => this.timer / 15f;

  public void Initialize()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
    this.abortRequested = false;
    this.canvasGroup.alpha = 0.0f;
    this.canvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.snowBallMaterial.SetFloat(UIBuildSnowmanMinigameOverlayController.FillColorLerpFade, 0.0f);
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UIManager>.Instance.UnloadBuildSnowmanMinigameAssets();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
    this.snowballTemplate.gameObject.SetActive(false);
    this.snowballCanvasGroup.alpha = 0.0f;
    this.button.GetComponent<CanvasGroup>().alpha = 0.0f;
    this.accuracyCanvasGroup.alpha = 0.0f;
    this.perfectText.alpha = 0.0f;
    this.failText.alpha = 0.0f;
    this.snowballDropIndicator.transform.localScale = Vector3.zero;
    this.snowballIndicatorImageOutline.gameObject.SetActive(false);
  }

  public override void OnShowCompleted()
  {
    Time.timeScale = 0.0f;
    base.OnShowCompleted();
    this.StartCoroutine((IEnumerator) this.TweenInSnowBall());
  }

  public IEnumerator TweenInSnowBall()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIBuildSnowmanMinigameOverlayController overlayController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      overlayController.snowballDropIndicator.gameObject.SetActive(true);
      overlayController.snowballDropIndicator.transform.localScale = Vector3.zero;
      overlayController.snowballDropIndicator.transform.DOScale(1f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      overlayController.snowballIndicator.transform.DOLocalMoveX(overlayController.leftPos.localPosition.x, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(overlayController.\u003CTweenInSnowBall\u003Eb__52_0));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    overlayController.snowballCanvasGroup.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController.snowballIndicatorImageOutline.gameObject.SetActive(true);
    overlayController.snowballIndicatorImageOutline.transform.localScale = Vector3.zero;
    overlayController.snowballIndicatorImageOutline.transform.DOScale(1f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    UIManager.PlayAudio("event:/dlc/env/snowman/minigame_spawn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(0.33f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void MoveSnowBall()
  {
    this.button.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnStart<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.startSequencePlayed = true));
    this.snowballIndicator.transform.localPosition = new Vector3(this.leftPos.localPosition.x, this.snowballIndicator.transform.localPosition.y, this.snowballIndicator.transform.localPosition.z);
    this.indicatorTween = DOTween.Sequence().Append((Tween) this.snowballIndicator.transform.DOLocalMoveX(this.rightPos.localPosition.x, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true)).AppendCallback((TweenCallback) (() =>
    {
      if (this.snowballPositionXList.Count >= 3)
        return;
      UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_bounce");
    })).Append((Tween) this.snowballIndicator.transform.DOLocalMoveX(this.leftPos.localPosition.x, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true)).AppendCallback((TweenCallback) (() =>
    {
      if (this.snowballPositionXList.Count >= 3)
        return;
      UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_bounce");
    })).SetLoops<DG.Tweening.Sequence>(-1, DG.Tweening.LoopType.Restart).SetUpdate<DG.Tweening.Sequence>(true);
  }

  public void Update()
  {
    if (this.IsShowing || !this.startSequencePlayed)
      return;
    if (!this.isSnowballSequencePlaying && this.snowballPositionXList.Count < 3)
    {
      this.indicatorTime += this.speed * Time.unscaledDeltaTime;
      this.canDrop = (double) Time.unscaledTime - (double) this.delay > 0.5;
      if (!this.failed)
        this.button.gameObject.SetActive(this.canDrop);
      else
        this.button.gameObject.SetActive(false);
      this.snowballIndicatorImage.color = new Color(this.snowballIndicatorImage.color.r, this.snowballIndicatorImage.color.g, this.snowballIndicatorImage.color.b, this.canDrop ? 1f : 0.5f);
      if (InputManager.Gameplay.GetInteractButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        if (this.canDrop)
        {
          this.button.transform.DOComplete();
          this.button.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.33f).SetUpdate<Tweener>(true);
          MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
          this.SpawnSnowball();
          if (this.numberOfSnowballs >= 2)
          {
            this.snowballIndicator.gameObject.SetActive(false);
            this.snowballDropIndicator.gameObject.SetActive(false);
            this.isSnowballSequencePlaying = true;
          }
          else
          {
            this.snowballIndicatorImageOutline.sprite = this.snowballSprites[this.numberOfSnowballs + 1];
            this.snowballIndicatorImage.sprite = this.snowballSprites[this.numberOfSnowballs + 1];
            this.snowballIndicatorImageOutline.transform.localScale = Vector3.zero;
            this.snowballIndicatorImageOutline.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
            this.snowballDropIndicator.transform.localScale = Vector3.zero;
            this.snowballDropIndicator.transform.DOScale(1f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1f);
          }
        }
      }
      else if (!this.shownNegativeFeedback)
      {
        this.button.transform.DOComplete();
        this.button.transform.DOPunchPosition(new Vector3(10f, 0.0f), 0.33f).SetUpdate<Tweener>(true);
        UIManager.PlayAudio("event:/ui/negative_feedback");
        MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
        this.shownNegativeFeedback = true;
      }
      if (InputManager.Gameplay.GetInteractButtonUp(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this.shownNegativeFeedback = false;
    }
    if ((double) this.timer == 3.4028234663852886E+38)
      return;
    this.timer += Time.unscaledDeltaTime;
    this.timerBar.fillAmount = 1f - this.normalisedTime;
    if ((double) this.timerBar.fillAmount < 0.20000000298023224)
    {
      this.timerBar.color = StaticColors.RedColor;
      if (!this.timerShaking)
      {
        this.timerContainer.transform.DOKill();
        this.timerContainer.transform.DOShakePosition(1f, fadeOut: false).SetUpdate<Tweener>(true).SetLoops<Tweener>(-1);
        this.timerShaking = true;
      }
    }
    else if ((double) this.timerBar.fillAmount < 0.40000000596046448)
      this.timerBar.color = StaticColors.OrangeColor;
    else
      this.timerBar.color = StaticColors.GreenColor;
    if ((double) this.normalisedTime < 1.0 && !this.abortRequested)
      return;
    this.failed = true;
    this.canDrop = false;
    this.overlayContainer.DOShakePosition(0.5f, new Vector3(25f, 0.0f, 0.0f), randomness: 0.0f).SetUpdate<Tweener>(true);
    this.button.gameObject.SetActive(false);
    this.indicatorTween.Kill();
    this.snowballIndicatorImageOutline.gameObject.SetActive(false);
    this.snowballIndicator.gameObject.SetActive(false);
    this.snowballDropIndicator.gameObject.SetActive(false);
    this.timerContainer.transform.DOComplete();
    this.timer = float.MaxValue;
    System.Action onFail = this.OnFail;
    if (onFail != null)
      onFail();
    this.shakeContainer.DOComplete();
    this.shakeContainer.DOShakePosition(0.5f, new Vector3(25f, 0.0f, 0.0f), randomness: 0.0f).SetUpdate<Tweener>(true);
    this.Close(true);
  }

  public void RequestAbort() => this.abortRequested = true;

  public void SpawnSnowball()
  {
    UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_drop");
    this.isSnowballSequencePlaying = true;
    GameObject snowball = UnityEngine.Object.Instantiate<GameObject>(this.snowballTemplate, this.snowballContainer);
    snowball.SetActive(true);
    snowball.transform.localPosition = (Vector3) new Vector2(this.snowballIndicator.transform.localPosition.x, this.startPosition.localPosition.y);
    snowball.GetComponent<Image>().sprite = this.snowballSprites[this.numberOfSnowballs];
    this.snowBallsList.Add(snowball);
    this.snowballPositionXList.Add(this.snowballIndicator.transform.localPosition.x);
    bool failed = this.CheckFail();
    float endValue1 = failed ? this.endPosition.localPosition.y : this.endPosition.localPosition.y + (float) this.numberOfSnowballs * this.snowballSizeY;
    float duration = 0.5f;
    Vector3 endValue2 = new Vector3(0.0f, 0.0f, (float) UnityEngine.Random.Range(-45, 45));
    snowball.transform.DORotate(endValue2, duration).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InCubic).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    snowball.transform.DOLocalMoveY(endValue1, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCubic).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
      bool flag = this.HandlePerfectShot(snowball.transform.position);
      if (this.count == 0 | failed || !flag)
        UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_land_level_01");
      else if (this.count == 1 & flag)
        UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_land_level_02");
      else if (this.count == 2 & flag)
        UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_land_level_03");
      ++this.count;
      this.shakeContainer.DOComplete();
      this.shakeContainer.DOShakePosition(0.33f, new Vector3(0.0f, 20f)).SetUpdate<Tweener>(true);
      this.hitEffect.transform.position = snowball.transform.position;
      this.hitEffect.Play();
      this.ShowOrUpdateAccuracyPersistent();
      ++this.numberOfSnowballs;
      if (failed)
      {
        this.indicatorTween.Kill();
        UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_land_level_01");
        this.overlayContainer.DOShakePosition(0.5f, new Vector3(25f, 0.0f, 0.0f), randomness: 0.0f).SetUpdate<Tweener>(true);
        this.snowballIndicator.gameObject.SetActive(false);
        this.Close(true);
      }
      else if (this.snowballPositionXList.Count >= 3)
      {
        this.Close(false);
      }
      else
      {
        this.isSnowballSequencePlaying = false;
        this.delay = Time.unscaledTime;
      }
    }));
  }

  public void Close(bool failed) => this.StartCoroutine((IEnumerator) this.CloseRoutine(failed));

  public IEnumerator CloseRoutine(bool failed)
  {
    UIBuildSnowmanMinigameOverlayController overlayController = this;
    overlayController.timerContainer.GetComponent<CanvasGroup>().DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController.button.gameObject.SetActive(false);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    float delay = 0.0f;
    if (failed)
    {
      overlayController.snowBallMaterial.SetColor(UIBuildSnowmanMinigameOverlayController.FillColor, StaticColors.RedColor);
      UIManager.PlayAudio("event:/dlc/env/snowman/minigame_fail");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
      overlayController.failText.alpha = 1f;
      overlayController.failText.transform.DOComplete();
      overlayController.failText.DOComplete();
      overlayController.failText.transform.DOPunchPosition(new Vector3(10f, 0.0f), 0.5f).SetUpdate<Tweener>(true);
      ShortcutExtensionsTMPText.DOFade(overlayController.failText, 0.0f, 1f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(1.25f);
    }
    else
      overlayController.snowBallMaterial.SetColor(UIBuildSnowmanMinigameOverlayController.FillColor, StaticColors.GreenColor);
    foreach (GameObject snowBalls in overlayController.snowBallsList)
    {
      DOVirtual.Float(overlayController.snowBallMaterial.GetFloat(UIBuildSnowmanMinigameOverlayController.FillColorLerpFade), 1f, 0.5f, new TweenCallback<float>(overlayController.\u003CCloseRoutine\u003Eb__66_1)).SetUpdate<Tweener>(true);
      snowBalls.transform.DOPunchScale(new Vector3(0.3f, 0.3f), 0.33f).SetUpdate<Tweener>(true).SetDelay<Tweener>(delay);
      delay += 0.25f;
    }
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOVirtual.Float(overlayController.snowBallMaterial.GetFloat(UIBuildSnowmanMinigameOverlayController.FillColorLerpFade), 0.0f, 0.5f, new TweenCallback<float>(overlayController.\u003CCloseRoutine\u003Eb__66_0)).SetUpdate<Tweener>(true);
    if (!failed)
    {
      overlayController.accuracyCanvasGroup.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      overlayController.accuracyAmountText.text = "0<size=30>%";
      float count;
      if (overlayController.ComputeAccuracyPercent() != 100)
      {
        overlayController.accuracyAmountText.DOText(overlayController.ComputeAccuracyPercent().ToString(), 1f, scrambleMode: ScrambleMode.Numerals).SetUpdate<TweenerCore<string, string, StringOptions>>(true);
        count = 0.0f;
        while ((double) count <= 1.0)
        {
          AudioManager.Instance.PlayOneShotAndSetParameterValue("event:/dlc/env/snowman/minigame_point_tick", "snowmanMinigamePointTick", count);
          count += 0.2f;
          yield return (object) new WaitForSecondsRealtime(0.2f);
        }
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
      else
      {
        overlayController.accuracyAmountText.DOText("99", 2f, scrambleMode: ScrambleMode.Numerals).SetUpdate<TweenerCore<string, string, StringOptions>>(true);
        count = 0.0f;
        while ((double) count <= 1.0)
        {
          AudioManager.Instance.PlayOneShotAndSetParameterValue("event:/dlc/env/snowman/minigame_point_tick", "snowmanMinigamePointTick", count);
          count += 0.2f;
          yield return (object) new WaitForSecondsRealtime(0.2f);
        }
        overlayController.accuracyAmountText.DOKill();
        overlayController.accuracyAmountText.transform.DOPunchPosition(new Vector3(0.0f, 10f), 0.33f).SetUpdate<Tweener>(true);
        overlayController.accuracyAmountText.DOText(overlayController.ComputeAccuracyPercent().ToString(), 0.0f).SetUpdate<TweenerCore<string, string, StringOptions>>(true);
        AudioManager.Instance.PlayOneShotAndSetParameterValue("event:/dlc/env/snowman/minigame_point_tick", "snowmanMinigamePointTick", 1f);
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
      overlayController.accuracyCanvasGroup.DOFade(0.0f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      if (overlayController.ComputeAccuracyPercent() == 100)
      {
        overlayController.perfectEffect.Play();
        overlayController.perfectText.alpha = 1f;
        overlayController.perfectText.transform.DOComplete();
        overlayController.perfectText.DOComplete();
        overlayController.perfectText.transform.DOPunchPosition(new Vector3(0.0f, 10f), 0.5f).SetUpdate<Tweener>(true);
        ShortcutExtensionsTMPText.DOFade(overlayController.perfectText, 1f, 0.33f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        UIManager.PlayAudio("event:/Stings/generic_positive");
        yield return (object) new WaitForSecondsRealtime(1f);
      }
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
    UIManager.PlayAudio("event:/ui/close_menu");
    Time.timeScale = 1f;
    overlayController.Hide();
    overlayController.timerBar.fillAmount = 1f - overlayController.normalisedTime;
    overlayController.timer = float.MaxValue;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    this.CheckResult();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public bool CheckFail()
  {
    return this.snowballPositionXList.Count >= 2 && (double) MathF.Abs(this.snowballPositionXList[this.snowballPositionXList.Count - 1] - this.snowballPositionXList[this.snowballPositionXList.Count - 2]) >= (double) this.snowballSizeY - 20.0;
  }

  public bool HandlePerfectShot(Vector3 perfectEffectPosition)
  {
    if (this.snowballPositionXList.Count == 1)
    {
      ++this.perfectShotsScore;
      return false;
    }
    if (this.snowballPositionXList.Count == 0)
      return false;
    if ((double) MathF.Abs(this.snowballPositionXList[this.snowballPositionXList.Count - 1] - this.snowballPositionXList[this.snowballPositionXList.Count - 2]) <= (double) this.perfectShotError)
    {
      --this.timer;
      this.perfectEffect.transform.position = perfectEffectPosition;
      this.perfectEffect.Play();
      if (this.snowballPositionXList.Count < 3 | Mathf.CeilToInt((float) this.ComputeAccuracyPercent() / 10f) > 9)
        this.perfectText.alpha = 1f;
      this.perfectText.transform.DOComplete();
      this.perfectText.DOComplete();
      this.perfectText.transform.DOPunchPosition(new Vector3(0.0f, 10f), 0.5f).SetUpdate<Tweener>(true);
      ShortcutExtensionsTMPText.DOFade(this.perfectText, 0.0f, 1f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      ++this.perfectShotsScore;
      return true;
    }
    ++this.timer;
    return false;
  }

  public void CheckResult()
  {
    if (this.snowballPositionXList.Count < 1)
      return;
    for (int index = 1; index < this.snowballPositionXList.Count; ++index)
    {
      if ((double) MathF.Abs(this.snowballPositionXList[index] - this.snowballPositionXList[index - 1]) >= (double) this.snowballSizeY - 20.0)
      {
        System.Action onFail = this.OnFail;
        if (onFail == null)
          return;
        onFail();
        return;
      }
    }
    int num = Mathf.CeilToInt((float) this.ComputeAccuracyPercent() / 10f);
    Action<int> onSucceed = this.OnSucceed;
    if (onSucceed != null)
      onSucceed(num);
    Debug.Log((object) $"Succeed with level: {this.level.ToString()}, perfect shots: {this.perfectShotsScore.ToString()}");
  }

  public float GetLastError()
  {
    if (this.snowballPositionXList.Count < 2)
      return 0.0f;
    int index = this.snowballPositionXList.Count - 1;
    return Mathf.Abs(this.snowballPositionXList[index] - this.snowballPositionXList[index - 1]);
  }

  public int ComputeAccuracyPercent()
  {
    int num1 = 34;
    int num2 = num1;
    float num3 = this.snowballPositionXList[0];
    for (int index = 1; index < this.snowballPositionXList.Count; ++index)
    {
      float num4 = 1f - Mathf.Clamp01(Mathf.Abs(this.snowballPositionXList[index] - num3) / (this.snowballSizeY - 20f));
      num2 += Mathf.RoundToInt((float) num1 * num4);
      num3 = this.snowballPositionXList[index];
    }
    return Mathf.Clamp(num2, 0, 100);
  }

  public void ShowOrUpdateAccuracyPersistent()
  {
    int accuracyPercent = this.ComputeAccuracyPercent();
    if (accuracyPercent >= 0)
    {
      this.accuracySum += accuracyPercent;
      ++this.shots;
    }
    if (this.shots > 0)
      Mathf.RoundToInt((float) this.accuracySum / (float) this.shots);
    if (accuracyPercent < 0)
      return;
    accuracyPercent.ToString();
  }

  public void OnDrawGizmos()
  {
    if (!this.showGizmos || (UnityEngine.Object) this.leftPos == (UnityEngine.Object) null || (UnityEngine.Object) this.rightPos == (UnityEngine.Object) null)
      return;
    Gizmos.color = this.pathColor;
    Gizmos.DrawLine(this.leftPos.position, this.rightPos.position);
    float num = Mathf.Max(0.02f, (float) ((double) Vector3.Distance(this.leftPos.position, this.rightPos.position) * (double) this.gizmoSizeFactor * 0.5));
    for (int index = 0; index <= 8; ++index)
      Gizmos.DrawWireSphere(Vector3.Lerp(this.leftPos.position, this.rightPos.position, (float) index / 8f), num * 0.6f);
    if (!((UnityEngine.Object) this.snowballIndicator != (UnityEngine.Object) null))
      return;
    Gizmos.color = this.currentColor;
    Gizmos.DrawSphere(this.snowballIndicator.position, Mathf.Max(0.02f, Vector3.Distance(this.leftPos.position, this.rightPos.position) * this.gizmoSizeFactor));
  }

  [CompilerGenerated]
  public void \u003CTweenInSnowBall\u003Eb__52_0()
  {
    UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_bounce");
    this.MoveSnowBall();
  }

  [CompilerGenerated]
  public void \u003CMoveSnowBall\u003Eb__54_0() => this.startSequencePlayed = true;

  [CompilerGenerated]
  public void \u003CMoveSnowBall\u003Eb__54_1()
  {
    if (this.snowballPositionXList.Count >= 3)
      return;
    UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_bounce");
  }

  [CompilerGenerated]
  public void \u003CMoveSnowBall\u003Eb__54_2()
  {
    if (this.snowballPositionXList.Count >= 3)
      return;
    UIManager.PlayAudio("event:/dlc/env/snowman/minigame_snowball_bounce");
  }

  [CompilerGenerated]
  public void \u003CCloseRoutine\u003Eb__66_1(float value)
  {
    this.snowBallMaterial.SetFloat(UIBuildSnowmanMinigameOverlayController.FillColorLerpFade, value);
  }

  [CompilerGenerated]
  public void \u003CCloseRoutine\u003Eb__66_0(float value)
  {
    this.snowBallMaterial.SetFloat(UIBuildSnowmanMinigameOverlayController.FillColorLerpFade, value);
  }
}
