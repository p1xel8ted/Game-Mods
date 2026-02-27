// Decompiled with JetBrains decompiler
// Type: UICookingMinigameOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICookingMinigameOverlayController : MonoBehaviour
{
  private const float kPauseDuration = 0.3f;
  private const float kAccessibleCooldown = 0.5f;
  public Image Background;
  public Image SafeZone;
  public Image Tracker;
  public Image Flash;
  public float TrackerNormalisedPosition;
  public float Speed = 1f;
  public float GameWidth = 300f;
  public Transform ControlPromptUI;
  private readonly List<float> _beadMealRanges = new List<float>()
  {
    50f,
    50f
  };
  private readonly List<float> _mediumMealRanges = new List<float>()
  {
    75f,
    75f
  };
  private readonly List<float> _goodMealRanges = new List<float>()
  {
    100f,
    100f
  };
  public System.Action OnCook;
  public System.Action OnUnderCook;
  public System.Action OnBurn;
  public TextMeshProUGUI CounterText;
  public RecipeItem RecipeItem;
  private bool MiniGamePaused;
  private bool DidBurn;
  private EventInstance loopingSoundInstance;
  private float _randomOffset;
  private Interaction_Kitchen kitchen;
  private float _accessibleCooldown;
  private int CurrentDifficulty;
  private int StartingMeals;
  private StructuresData StructureInfo;
  public RectTransform RecipeItemRT;
  public CanvasGroup RecipeItemCG;
  public CanvasGroup CanvasGroup;
  private DG.Tweening.Sequence BurnFadeSequence;

  private float UnderCookedRange
  {
    get
    {
      switch (this.CurrentDifficulty)
      {
        case 1:
          return this._beadMealRanges[0] + this._randomOffset;
        case 2:
          return this._mediumMealRanges[0] + this._randomOffset;
        case 3:
          return this._goodMealRanges[0] + this._randomOffset;
        default:
          return 0.0f;
      }
    }
  }

  private float OverCookedRange
  {
    get
    {
      switch (this.CurrentDifficulty)
      {
        case 1:
          return this._beadMealRanges[1] - this._randomOffset;
        case 2:
          return this._mediumMealRanges[1] - this._randomOffset;
        case 3:
          return this._goodMealRanges[1] - this._randomOffset;
        default:
          return 0.0f;
      }
    }
  }

  public void Initialise(StructuresData StructureInfo, Interaction_Kitchen kitchen)
  {
    this.StructureInfo = StructureInfo;
    this.kitchen = kitchen;
    this.StartingMeals = StructureInfo.QueuedMeals.Count;
    this.UpdateText();
    this.Flash.color = StaticColors.OffWhiteColor;
    DOTweenModuleUI.DOFade(this.Flash, 0.0f, 0.5f);
    this.SetBarSizes();
    this.RecipeItem.GetComponent<MMButton>().enabled = false;
    this.transform.localScale = Vector3.one * 1.2f;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.CanvasGroup.alpha = 0.0f;
    this.CanvasGroup.DOFade(1f, 0.2f);
    string str = "asdasD" + "asdasd";
    this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/cooking/cooking_loop", kitchen.gameObject, true);
    this.OnAutoCookSettingChanged(SettingsManager.Settings.Accessibility.AutoCook);
  }

  private void OnAutoCookSettingChanged(bool value)
  {
    this.ControlPromptUI.gameObject.SetActive(!value);
  }

  private void SetBarSizes()
  {
    this.CurrentDifficulty = CookingData.GetSatationLevel(this.StructureInfo.QueuedMeals[0].MealType);
    if (this.StructureInfo.QueuedMeals.Count != this.StartingMeals)
    {
      this._randomOffset = 0.0f;
      this._randomOffset = Mathf.Min(this.UnderCookedRange, this.OverCookedRange);
      this._randomOffset = UnityEngine.Random.Range(-this._randomOffset, this._randomOffset);
    }
    this.SafeZone.rectTransform.SetRect(this.UnderCookedRange, 0.0f, this.OverCookedRange, 0.0f);
  }

  private void UpdateText()
  {
    if (this.StructureInfo.QueuedMeals.Count <= 0)
      return;
    this.CounterText.text = $"{(object) this.StructureInfo.QueuedMeals.Count}/{(object) this.StartingMeals}";
    this.RecipeItemRT.DOKill();
    this.RecipeItemRT.localScale = Vector3.one;
    this.RecipeItemRT.localPosition = new Vector3(0.0f, -100f);
    this.RecipeItemRT.DOLocalMove(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.RecipeItemCG.alpha = 0.0f;
    this.RecipeItemCG.DOFade(1f, 0.25f);
    this.RecipeItem.Configure(this.StructureInfo.QueuedMeals[0].MealType, false, false);
  }

  private void HideText(float Duration)
  {
    this.RecipeItemRT.DOScale(Vector3.zero, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
  }

  private void OnEnable()
  {
    Singleton<AccessibilityManager>.Instance.OnAutoCookChanged += new Action<bool>(this.OnAutoCookSettingChanged);
  }

  private void OnDisable()
  {
    if (this.BurnFadeSequence != null)
    {
      this.BurnFadeSequence.Kill();
      this.BurnFadeSequence = (DG.Tweening.Sequence) null;
    }
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    Singleton<AccessibilityManager>.Instance.OnAutoCookChanged -= new Action<bool>(this.OnAutoCookSettingChanged);
  }

  public void Close()
  {
    if (this.DidBurn && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.BurntFood))
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.BurntFood);
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.transform.DOLocalMove(new Vector3(0.0f, -500f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    this.CanvasGroup.DOFade(0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.kitchen.HasChanged = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
  }

  private void ShakeRecipeItem(float Duration)
  {
    this.RecipeItemRT.DOKill();
    this.RecipeItemRT.DOShakePosition(Duration, new Vector3(50f, 0.0f, 0.0f), randomness: 0.0f);
    if (this.BurnFadeSequence != null)
      this.BurnFadeSequence.Kill();
    this.BurnFadeSequence = DOTween.Sequence();
    this.BurnFadeSequence.AppendInterval(Duration - 0.15f);
    this.BurnFadeSequence.Append((Tween) this.RecipeItemCG.DOFade(0.0f, 0.15f));
    this.BurnFadeSequence.Play<DG.Tweening.Sequence>();
  }

  private void Update()
  {
    if (this.MiniGamePaused || MonoSingleton<UIManager>.Instance.MenusBlocked)
      return;
    this.TrackerNormalisedPosition += this.Speed * Time.deltaTime;
    this.Tracker.transform.localPosition = new Vector3((float) (-(double) this.GameWidth / 2.0 + (double) this.GameWidth * (double) Mathf.PingPong(this.TrackerNormalisedPosition, 1f)), 0.0f);
    bool flag = false;
    if (SettingsManager.Settings.Accessibility.AutoCook)
    {
      if ((double) this._accessibleCooldown <= 0.0)
      {
        if ((double) this.Tracker.rectTransform.anchoredPosition.x < (double) this.GameWidth / 2.0 - (double) this.OverCookedRange && (double) this.Tracker.rectTransform.anchoredPosition.x > -(double) this.GameWidth / 2.0 + (double) this.UnderCookedRange)
        {
          flag = true;
          this._accessibleCooldown = 0.5f;
        }
      }
      else
        this._accessibleCooldown -= Time.deltaTime;
    }
    else
      flag = InputManager.Gameplay.GetInteractButtonDown();
    if (!flag)
      return;
    UIManager.PlayAudio("event:/ui/arrow_change_selection");
    this.Tracker.transform.DOKill();
    this.Tracker.rectTransform.localScale = Vector3.one;
    this.Tracker.rectTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
    this.Background.rectTransform.DOKill();
    if ((double) this.Tracker.rectTransform.anchoredPosition.x < -(double) this.GameWidth / 2.0 + (double) this.UnderCookedRange)
    {
      this.ShakeRecipeItem(0.3f);
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
      this.Background.rectTransform.DOShakePosition(0.5f, new Vector3(25f, 0.0f, 0.0f), randomness: 0.0f);
      this.Flash.color = StaticColors.RedColor;
      System.Action onUnderCook = this.OnUnderCook;
      if (onUnderCook != null)
        onUnderCook();
      this.DidBurn = true;
    }
    else if ((double) this.Tracker.rectTransform.anchoredPosition.x > (double) this.GameWidth / 2.0 - (double) this.OverCookedRange)
    {
      this.ControlPromptUI.DOKill();
      this.ControlPromptUI.DOScale(0.75f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
      this.ShakeRecipeItem(0.3f);
      this.Background.rectTransform.DOShakePosition(0.5f, new Vector3(25f, 0.0f, 0.0f), randomness: 0.0f);
      this.Flash.color = StaticColors.RedColor;
      System.Action onBurn = this.OnBurn;
      if (onBurn != null)
        onBurn();
      this.DidBurn = true;
    }
    else
    {
      this.ControlPromptUI.DOKill();
      this.ControlPromptUI.DOScale(0.75f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
      this.HideText(0.3f);
      this.Flash.color = StaticColors.OffWhiteColor;
      System.Action onCook = this.OnCook;
      if (onCook != null)
        onCook();
    }
    DOTweenModuleUI.DOFade(this.Flash, 0.0f, 0.5f);
    this.StartCoroutine((IEnumerator) this.InputPause());
  }

  private IEnumerator InputPause()
  {
    this.MiniGamePaused = true;
    int count = this.StructureInfo.QueuedMeals.Count;
    yield return (object) new WaitForSeconds(0.3f);
    this.ControlPromptUI.DOKill();
    this.ControlPromptUI.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (count > 0)
    {
      this.SetBarSizes();
      this.UpdateText();
      this.MiniGamePaused = false;
    }
    else
      this.Close();
  }
}
