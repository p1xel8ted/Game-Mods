// Decompiled with JetBrains decompiler
// Type: UITailorMinigameOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITailorMinigameOverlayController : MonoBehaviour
{
  public const float kPauseDuration = 0.3f;
  public const float kAccessibleCooldown = 0.5f;
  public Image Background;
  public RectTransform Tracker;
  public Image Indicator;
  public Image Flash;
  public Image Middle;
  public Transform ControlPromptUI;
  public Targets[] targets;
  public List<float> _beadMealRanges = new List<float>()
  {
    50f,
    50f
  };
  public List<float> _mediumMealRanges = new List<float>()
  {
    75f,
    75f
  };
  public List<float> _goodMealRanges = new List<float>()
  {
    100f,
    100f
  };
  public System.Action OnCook;
  public System.Action OnUnderCook;
  public System.Action OnBurn;
  public System.Action OnFinish;
  public TextMeshProUGUI CounterText;
  public TailorItem RecipeItem;
  public bool MiniGamePaused;
  public bool DidBurn;
  public EventInstance loopingSoundInstance;
  public float NormalisedPosition;
  public float _randomOffset;
  public Interaction_Tailor kitchen;
  public float _accessibleCooldown;
  public int successfulCraftedItems;
  public PlayerFarming interactingPlayer;
  public int targetsHit;
  public EventInstance LoopedSound;
  public int CurrentDifficulty;
  public int StartingClothes;
  public StructuresData StructureInfo;
  public RectTransform RecipeItemRT;
  public CanvasGroup RecipeItemCG;
  public CanvasGroup CanvasGroup;
  public DG.Tweening.Sequence BurnFadeSequence;

  public float UnderCookedRange
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

  public float OverCookedRange
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

  public void Initialise(
    StructuresData StructureInfo,
    Interaction_Tailor kitchen,
    PlayerFarming interactingPlayer)
  {
    this.StructureInfo = StructureInfo;
    this.kitchen = kitchen;
    this.interactingPlayer = interactingPlayer;
    this.LoopedSound = AudioManager.Instance.CreateLoop("event:/building/tailor/spinning_wheel_loop", true);
    AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, "spinning_speed", 0.0f);
    AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, "spinning_speed", 1f);
    this.StartingClothes = StructureInfo.QueuedClothings.Count;
    this.UpdateText();
    this.Flash.color = StaticColors.GreenColor;
    this.Flash.transform.localScale = Vector3.one;
    this.Middle.transform.localScale = Vector3.one;
    this.SetBarSizes();
    this.RecipeItem.GetComponent<MMButton>().enabled = false;
    this.transform.localScale = Vector3.one * 1.2f;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.CanvasGroup.alpha = 0.0f;
    this.CanvasGroup.DOFade(1f, 0.2f);
    string str = "asdasD" + "asdasd";
    this.OnAutoCookSettingChanged(SettingsManager.Settings.Accessibility.AutoCraft);
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
  }

  public void OnAutoCookSettingChanged(bool value)
  {
    this.ControlPromptUI.gameObject.SetActive(!value);
  }

  public float Speed => 250f;

  public Vector2 FillRange
  {
    get
    {
      switch (this.CurrentDifficulty)
      {
        case 1:
          return new Vector2(0.15f, 0.175f);
        case 2:
          return new Vector2(0.125f, 0.15f);
        case 3:
          return new Vector2(0.1f, 0.125f);
        default:
          return new Vector2(0.09f, 0.13f);
      }
    }
  }

  public void SetBarSizes()
  {
    this.CurrentDifficulty = TailorManager.GetClothingData(this.StructureInfo.QueuedClothings[0].ClothingType).IsSecret || TailorManager.GetClothingData(this.StructureInfo.QueuedClothings[0].ClothingType).SpecialClothing ? 3 : 1;
    Debug.Log((object) $"{"+===============================+".Colour(Color.yellow)}  {this.CurrentDifficulty.ToString()}");
    if (this.StructureInfo.QueuedClothings.Count != this.StartingClothes)
    {
      this._randomOffset = 0.0f;
      this._randomOffset = Mathf.Min(this.UnderCookedRange, this.OverCookedRange);
      this._randomOffset = UnityEngine.Random.Range(-this._randomOffset, this._randomOffset);
    }
    float num1 = (float) UnityEngine.Random.Range(0, 360);
    float maxInclusive = 360f / (float) this.targets.Length;
    for (int index = 0; index < this.targets.Length; ++index)
    {
      float z = Utils.Repeat(num1 + UnityEngine.Random.Range(maxInclusive / 2f, maxInclusive), 360f);
      this.targets[index].image.transform.rotation = Quaternion.Euler(0.0f, 0.0f, z);
      float num2 = UnityEngine.Random.Range(this.FillRange.x, this.FillRange.y);
      this.targets[index].outlineRightPivot.transform.rotation = Quaternion.Euler(0.0f, 0.0f, z - num2 * 360f);
      this.targets[index].image.fillAmount = num2;
      num1 = Utils.Repeat(num1 + maxInclusive, 360f);
      this.targets[index].image.gameObject.SetActive(true);
    }
  }

  public void UpdateText()
  {
    Debug.Log((object) "UpdateText()");
    if (this.StructureInfo.QueuedClothings.Count <= 0)
      return;
    this.CounterText.text = $"{(this.StartingClothes - this.StructureInfo.QueuedClothings.Count + 1).ToString()}/{this.StartingClothes.ToString()}";
    this.RecipeItemRT.DOKill();
    this.RecipeItemRT.localScale = Vector3.one;
    this.RecipeItemRT.localPosition = new Vector3(0.0f, -100f);
    this.RecipeItemRT.DOLocalMove(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.RecipeItemCG.alpha = 0.0f;
    this.RecipeItemCG.DOFade(1f, 0.25f);
    this.RecipeItem.Configure(this.StructureInfo.QueuedClothings[0].ClothingType, this.StructureInfo.QueuedClothings[0].Variant, true);
  }

  public void HideText(float Duration)
  {
    this.RecipeItemRT.DOScale(Vector3.zero, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
  }

  public void OnEnable()
  {
    Singleton<AccessibilityManager>.Instance.OnAutoCraftChanged += new Action<bool>(this.OnAutoCookSettingChanged);
  }

  public void OnDisable()
  {
    if (this.BurnFadeSequence != null)
    {
      this.BurnFadeSequence.Kill();
      this.BurnFadeSequence = (DG.Tweening.Sequence) null;
    }
    AudioManager.Instance.StopLoop(this.LoopedSound);
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    Singleton<AccessibilityManager>.Instance.OnAutoCraftChanged -= new Action<bool>(this.OnAutoCookSettingChanged);
  }

  public void OnDestroy() => MonoSingleton<UIManager>.Instance.UnloadTailorMinigameAssets();

  public void Close()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.transform.DOLocalMove(new Vector3(0.0f, -500f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    this.CanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.kitchen.HasChanged = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      allBrain.CheckChangeTask();
    this.kitchen.ForceShowAssignMenu = this.successfulCraftedItems > 0;
    this.kitchen.OnInteract(this.interactingPlayer.state);
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public void ShakeRecipeItem(float Duration)
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

  public void Update()
  {
    if (this.MiniGamePaused || MonoSingleton<UIManager>.Instance.MenusBlocked)
      return;
    Transform transform = this.Tracker.transform;
    Quaternion rotation = this.Tracker.transform.rotation;
    Quaternion quaternion = Quaternion.Euler(new Vector3(0.0f, 0.0f, (float) ((double) rotation.eulerAngles.z - (double) this.Speed * (double) Time.deltaTime)));
    transform.rotation = quaternion;
    rotation = this.Tracker.transform.rotation;
    this.NormalisedPosition = rotation.eulerAngles.z / 360f;
    bool flag = false;
    if (SettingsManager.Settings.Accessibility.AutoCraft)
    {
      if ((double) this._accessibleCooldown <= 0.0)
      {
        if (this.IsTargetHit())
        {
          flag = true;
          this._accessibleCooldown = 0.5f;
        }
      }
      else
        this._accessibleCooldown -= Time.deltaTime;
    }
    else
      flag = InputManager.Gameplay.GetInteractButtonDown(this.interactingPlayer);
    if (!flag)
      return;
    UIManager.PlayAudio("event:/ui/arrow_change_selection");
    this.Tracker.transform.DOKill();
    this.Indicator.rectTransform.localScale = Vector3.one;
    this.Indicator.rectTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
    this.Background.rectTransform.DOKill();
    if (this.IsTargetHit() || SettingsManager.Settings.Accessibility.AutoCraft)
    {
      UIManager.PlayAudio("event:/building/tailor/spinning_wheel_positive");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
      AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, "spinning_speed", 0.0f);
      GameManager.GetInstance().WaitForSeconds(0.2f, (System.Action) (() => AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, "spinning_speed", 1f)));
      ++this.targetsHit;
      if (this.targetsHit >= this.targets.Length)
      {
        ++DataManager.Instance.outfitsCreated;
        DataManager.Instance.Alerts.ClothingCustomiseAlerts.AddOnce(this.StructureInfo.QueuedClothings[0].ClothingType);
        DataManager.Instance.Alerts.ClothingAssignAlerts.AddOnce(this.StructureInfo.QueuedClothings[0].ClothingType);
        DataManager.Instance.clothesCrafted.Add(this.StructureInfo.QueuedClothings[0].ClothingType);
        if (DataManager.Instance._revealingOutfits.ContainsKey(this.StructureInfo.QueuedClothings[0].ClothingType))
          DataManager.Instance._revealingOutfits[this.StructureInfo.QueuedClothings[0].ClothingType]++;
        else
          DataManager.Instance._revealingOutfits.Add(this.StructureInfo.QueuedClothings[0].ClothingType, 1);
        Debug.Log((object) ("Added outfit: " + this.StructureInfo.QueuedClothings[0].ClothingType.ToString()));
        this.targetsHit = 0;
        UIManager.PlayAudio("event:/building/tailor/spinning_wheel_win");
        ++this.successfulCraftedItems;
        this.HideText(0.3f);
        System.Action onCook = this.OnCook;
        if (onCook != null)
          onCook();
        this.StartCoroutine((IEnumerator) this.InputPause());
      }
      this.ControlPromptUI.DOKill();
      this.ControlPromptUI.DOScale(0.75f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
      this.Flash.DOKill();
      this.Flash.transform.DOKill();
      this.Flash.color = StaticColors.GreenColor;
      this.Flash.transform.localScale = Vector3.one;
      this.Flash.transform.DOScale(1.3f, 0.5f);
      DOTweenModuleUI.DOFade(this.Flash, 0.0f, 0.5f);
      this.Middle.transform.DOKill();
      this.Middle.transform.localScale = Vector3.one;
      this.Middle.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1);
    }
    else
    {
      UIManager.PlayAudio("event:/building/tailor/spinning_wheel_negative");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
      this.targetsHit = 0;
      this.ControlPromptUI.DOKill();
      this.ControlPromptUI.DOScale(0.75f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this.ShakeRecipeItem(0.3f);
      this.Background.rectTransform.DOShakePosition(0.5f, new Vector3(25f, 0.0f, 0.0f), randomness: 0.0f);
      this.Flash.color = StaticColors.RedColor;
      System.Action onBurn = this.OnBurn;
      if (onBurn != null)
        onBurn();
      this.DidBurn = true;
      DOTweenModuleUI.DOFade(this.Flash, 0.0f, 0.5f);
      this.StartCoroutine((IEnumerator) this.InputPause());
    }
  }

  public bool IsTargetHit()
  {
    bool flag = false;
    foreach (Targets target in this.targets)
    {
      if (target.image.gameObject.activeSelf)
      {
        float z = target.image.transform.rotation.eulerAngles.z;
        float num1 = Utils.Repeat(target.image.transform.rotation.eulerAngles.z - target.image.fillAmount * 360f, 360f);
        float num2 = 360f * this.NormalisedPosition;
        if ((double) num1 > (double) z)
        {
          if (((double) num2 >= 0.0 && (double) num2 < (double) z || (double) num2 > (double) num1) && ((double) num2 <= 360.0 && (double) num2 > (double) num1 || (double) num2 < (double) z))
          {
            flag = true;
            target.image.gameObject.SetActive(false);
            break;
          }
        }
        else if ((double) num2 > (double) num1 && (double) num2 < (double) z)
        {
          flag = true;
          target.image.gameObject.SetActive(false);
          break;
        }
      }
    }
    return flag;
  }

  public IEnumerator InputPause()
  {
    this.MiniGamePaused = true;
    int count = this.StructureInfo.QueuedClothings.Count;
    yield return (object) new WaitForSecondsRealtime(0.3f);
    this.ControlPromptUI.DOKill();
    this.ControlPromptUI.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (count > 0)
    {
      this.SetBarSizes();
      this.UpdateText();
      this.MiniGamePaused = false;
    }
    else
    {
      System.Action onFinish = this.OnFinish;
      if (onFinish != null)
        onFinish();
      this.Close();
    }
  }

  [CompilerGenerated]
  public void \u003CClose\u003Eb__51_0()
  {
    this.kitchen.HasChanged = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__54_0()
  {
    AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, "spinning_speed", 1f);
  }
}
