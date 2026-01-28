// Decompiled with JetBrains decompiler
// Type: UIFishingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMTools;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFishingOverlayController : UIMenuBase
{
  [SerializeField]
  public Vector3 offset;
  [SerializeField]
  public CanvasGroup castingCanvasGroup;
  [SerializeField]
  public CanvasGroup reelingCanvasGroup;
  [SerializeField]
  public Transform castingParent;
  [SerializeField]
  public Image castingStrengthBar;
  [SerializeField]
  public RectTransform header;
  [SerializeField]
  public MMControlPrompt castingControlPrompt;
  [SerializeField]
  public Transform reelingParent;
  [SerializeField]
  public RectTransform reelingNeedle;
  [SerializeField]
  public Image reelingNeedleIcon;
  [SerializeField]
  public Rigidbody2D needleRigidbody;
  [SerializeField]
  public Image reelingBarBG;
  [SerializeField]
  public Transform reelingBarParent;
  [SerializeField]
  public RectTransform targetSection;
  [SerializeField]
  public MMControlPrompt reelingControlPrompt;
  [Space]
  [SerializeField]
  public UIFishingOverlayController.Difficulty[] difficulties;
  [SerializeField]
  public RectTransform holdButtonAdjust;
  [SerializeField]
  public GameObject _reelButotnContainer;
  [SerializeField]
  public RectTransform holdButtonCast;
  [SerializeField]
  public GameObject _castButtonContainer;
  [SerializeField]
  public CoopIndicatorIcon[] coopIndicators;
  public StateMachine.State currentState;
  public GameObject lockPosition;
  public int difficulty;
  [CompilerGenerated]
  public PlayerFarming \u003CPlayerFarming\u003Ek__BackingField;
  public Vector2 sectionMinVelocity;
  public Vector2 sectionMaxVelocity;
  public float sectionTimer;
  public float sectionDirection;
  public float sectionIncrease;
  public float sectionMin;
  public float sectionMax;
  public int rapidMode = -1;
  public float rapidModeChance;
  public float rapidModeTimer;
  public Vector2 rapidModeDuration = new Vector2(4f, 7.5f);
  public const float needleUpForce = 2000f;
  public const float needleDownForce = -1250f;
  public Vector3[] sectionWorldCorners = new Vector3[4];
  public Vector3[] needleWorldCorners = new Vector3[4];
  public EventInstance loopingSoundInstance;
  public Material _needleHighlightedMaterial;
  public bool cacheResult;
  public bool _changedUpState;
  public bool _changedDownState;

  public UIFishingOverlayController.Difficulty CurrentDifficulty
  {
    get => this.difficulties[this.difficulty];
  }

  public PlayerFarming PlayerFarming
  {
    get => this.\u003CPlayerFarming\u003Ek__BackingField;
    set => this.\u003CPlayerFarming\u003Ek__BackingField = value;
  }

  public override bool _addToActiveMenus => false;

  public void CastingButtonDown(bool down)
  {
    if (down)
    {
      Transform parent1 = this.holdButtonCast.transform.parent;
      parent1.DOKill();
      parent1.DOScale(1f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      Transform parent2 = this.holdButtonAdjust.transform.parent;
      parent2.DOKill();
      parent2.DOScale(1f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    else
    {
      Transform parent3 = this.holdButtonCast.transform.parent;
      parent3.DOKill();
      parent3.DOScale(0.0f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      Transform parent4 = this.holdButtonAdjust.transform.parent;
      parent4.DOKill();
      parent4.DOScale(0.0f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
  }

  public override void Awake()
  {
    base.Awake();
    this._needleHighlightedMaterial = this.reelingNeedleIcon.material;
  }

  public void ConfigurePrompt()
  {
    this.castingControlPrompt.playerFarming = this.PlayerFarming;
    this.castingControlPrompt.ForceUpdate();
    this.reelingControlPrompt.playerFarming = this.PlayerFarming;
    this.reelingControlPrompt.ForceUpdate();
    foreach (CoopIndicatorIcon coopIndicator in this.coopIndicators)
    {
      coopIndicator.gameObject.SetActive(PlayerFarming.playersCount >= 2);
      coopIndicator.SetIcon(this.PlayerFarming.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat);
    }
    if (PlayerFarming.playersCount < 2)
      return;
    RectTransform header = this.header;
    header.localPosition = header.localPosition + Vector3.right * 300f;
  }

  public override void OnShowStarted()
  {
    this.targetSection.anchorMin = new Vector2(this.targetSection.anchorMin.x, 0.4f);
    this.targetSection.anchorMax = new Vector2(this.targetSection.anchorMax.x, 0.5f);
    this.reelingNeedle.anchoredPosition = new Vector2(this.reelingNeedle.anchoredPosition.x, 0.0f);
    this._castButtonContainer.SetActive(!SettingsManager.Settings.Accessibility.AutoFish);
    this._reelButotnContainer.SetActive(!SettingsManager.Settings.Accessibility.AutoFish);
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void SetState(StateMachine.State state)
  {
    this.currentState = state;
    this.castingCanvasGroup.alpha = 0.0f;
    this.reelingCanvasGroup.alpha = 0.0f;
    if (state != StateMachine.State.Casting)
      this.castingCanvasGroup.DOFade(0.0f, 1f);
    else
      this.castingCanvasGroup.DOFade(1f, 1f);
    if (state != StateMachine.State.Attacking)
      this.reelingCanvasGroup.DOFade(0.0f, 1f);
    else
      this.reelingCanvasGroup.DOFade(1f, 1f);
    if (state != StateMachine.State.Attacking)
      return;
    this.rapidMode = -1;
  }

  public void UpdateCastingStrength(float strength)
  {
    this.castingStrengthBar.fillAmount = strength;
  }

  public void SetReelingDifficulty(int difficulty)
  {
    this.difficulty = difficulty;
    this.sectionDirection = (double) UnityEngine.Random.Range(0.0f, 1f) > 0.5 ? 1f : -1f;
    this.rapidModeChance = 0.5f;
    this.sectionMin = (float) (0.5 - (double) this.CurrentDifficulty.sectionMinGap / 2.0);
    this.sectionMax = (float) (0.5 + (double) this.CurrentDifficulty.sectionMaxGap / 2.0);
  }

  public void UpdateReelBar(float reeledAmount)
  {
    if ((double) reeledAmount <= 0.699999988079071 || this.rapidMode != -1)
      return;
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > (double) this.rapidModeChance)
      this.BeginRapidMode();
    else
      this.rapidMode = 0;
  }

  public void BeginRapidMode()
  {
    float duration = UnityEngine.Random.Range(this.rapidModeDuration.x, this.rapidModeDuration.y);
    this.rapidModeTimer = Time.time + duration;
    this.SetReelingDifficulty(this.difficulty + 1);
    this.rapidMode = 1;
    this.reelingBarParent.DOShakeScale(duration, 0.05f, fadeOut: false).SetEase<Tweener>(Ease.Linear);
  }

  public bool IsNeedleWithinSection()
  {
    this.targetSection.GetWorldCorners(this.sectionWorldCorners);
    this.reelingNeedle.GetWorldCorners(this.needleWorldCorners);
    Vector3 sectionWorldCorner1 = this.sectionWorldCorners[0];
    Vector3 sectionWorldCorner2 = this.sectionWorldCorners[1];
    Vector3 needleWorldCorner1 = this.needleWorldCorners[0];
    Vector3 needleWorldCorner2 = this.needleWorldCorners[1];
    bool flag = (double) needleWorldCorner1.y > (double) sectionWorldCorner1.y && (double) needleWorldCorner2.y < (double) sectionWorldCorner2.y || (double) needleWorldCorner1.y <= (double) sectionWorldCorner2.y && (double) needleWorldCorner2.y >= (double) sectionWorldCorner1.y || (double) needleWorldCorner2.y >= (double) sectionWorldCorner1.y && (double) needleWorldCorner2.y <= (double) sectionWorldCorner2.y;
    this.reelingBarBG.enabled = flag;
    return flag;
  }

  public void FixedUpdate()
  {
    if (this.currentState == StateMachine.State.Attacking && !this.IsHiding)
    {
      if (InputManager.Gameplay.GetInteractButtonHeld(this.PlayerFarming))
      {
        this._changedDownState = false;
        this.needleRigidbody.AddForce(Vector2.up * 2000f, ForceMode2D.Force);
        if (!this._changedUpState)
        {
          Transform parent1 = this.holdButtonCast.transform.parent;
          parent1.DOKill();
          parent1.DOScale(0.75f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
          Transform parent2 = this.holdButtonAdjust.transform.parent;
          parent2.DOKill();
          parent2.DOScale(0.75f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
          MMVibrate.StopRumble();
          MMVibrate.RumbleContinuous(0.1f, 0.25f, this.PlayerFarming);
          AudioManager.Instance.PlayOneShot("event:/fishing/caught_something_tap", PlayerFarming.Instance.transform.position);
          this.reelingNeedleIcon.material = this._needleHighlightedMaterial;
          this._changedUpState = true;
        }
      }
      else
      {
        this._changedUpState = false;
        this.needleRigidbody.AddForce(Vector2.up * -1250f, ForceMode2D.Force);
        if (!this._changedDownState)
        {
          MMVibrate.StopRumble();
          MMVibrate.RumbleContinuous(0.0f, 0.025f, this.PlayerFarming);
          Transform parent3 = this.holdButtonCast.transform.parent;
          parent3.DOKill();
          parent3.DOScale(1f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
          Transform parent4 = this.holdButtonAdjust.transform.parent;
          parent4.DOKill();
          parent4.DOScale(1f, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
          this.reelingNeedleIcon.material = (Material) null;
          this._changedDownState = true;
        }
      }
      if ((double) this.reelingNeedle.anchoredPosition.y >= 100.0 || (double) this.reelingNeedle.anchoredPosition.y <= -100.0)
      {
        this.needleRigidbody.velocity = Vector2.zero;
        this.reelingNeedle.anchoredPosition = new Vector2(this.reelingNeedle.anchoredPosition.x, Mathf.Clamp(this.reelingNeedle.anchoredPosition.y, -100f, 100f));
      }
      if ((double) GameManager.GetInstance().CurrentTime > (double) this.sectionTimer)
      {
        if ((double) UnityEngine.Random.Range(0.0f, 1f) > 0.5)
          this.sectionDirection *= -1f;
        this.sectionTimer = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.CurrentDifficulty.sectionRandomTimer.x, this.CurrentDifficulty.sectionRandomTimer.y);
        this.sectionIncrease = UnityEngine.Random.Range(this.CurrentDifficulty.sectionRange.x, this.CurrentDifficulty.sectionRange.y);
      }
      this.sectionMin = Mathf.Clamp(this.sectionMin + this.sectionIncrease * this.sectionDirection, Mathf.Clamp01(this.sectionMax - this.CurrentDifficulty.sectionMaxGap), Mathf.Clamp01(this.sectionMax - this.CurrentDifficulty.sectionMinGap));
      this.sectionMax = Mathf.Clamp(this.sectionMax + this.sectionIncrease * this.sectionDirection, Mathf.Clamp01(this.sectionMin + this.CurrentDifficulty.sectionMinGap), Mathf.Clamp01(this.sectionMin + this.CurrentDifficulty.sectionMaxGap));
      this.sectionIncrease = Mathf.Clamp01(this.sectionIncrease - Time.deltaTime);
      this.targetSection.anchorMin = Vector2.SmoothDamp(this.targetSection.anchorMin, new Vector2(this.targetSection.anchorMin.x, this.sectionMin), ref this.sectionMinVelocity, this.CurrentDifficulty.sectionMoveSpeed);
      this.targetSection.anchorMax = Vector2.SmoothDamp(this.targetSection.anchorMax, new Vector2(this.targetSection.anchorMax.x, this.sectionMax), ref this.sectionMaxVelocity, this.CurrentDifficulty.sectionMoveSpeed);
      if (SettingsManager.Settings.Accessibility.AutoFish)
      {
        this.needleRigidbody.velocity = Vector2.zero;
        this.reelingNeedle.position = this.targetSection.position;
      }
      if (this.rapidMode != 1 || (double) Time.time <= (double) this.rapidModeTimer)
        return;
      this.SetReelingDifficulty(this.difficulty - 1);
      this.rapidMode = 0;
    }
    else
      MMVibrate.StopRumble();
  }

  [Serializable]
  public class Difficulty
  {
    public float sectionMinGap;
    public float sectionMaxGap;
    public float sectionMoveSpeed;
    public Vector2 sectionRange;
    public Vector2 sectionRandomTimer;
  }
}
