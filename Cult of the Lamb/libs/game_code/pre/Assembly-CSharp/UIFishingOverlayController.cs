// Decompiled with JetBrains decompiler
// Type: UIFishingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFishingOverlayController : UIMenuBase
{
  [SerializeField]
  private Vector3 offset;
  [SerializeField]
  private CanvasGroup castingCanvasGroup;
  [SerializeField]
  private CanvasGroup reelingCanvasGroup;
  [SerializeField]
  private Transform castingParent;
  [SerializeField]
  private Image castingStrengthBar;
  [SerializeField]
  private Transform reelingParent;
  [SerializeField]
  private RectTransform reelingNeedle;
  [SerializeField]
  private Image reelingNeedleIcon;
  [SerializeField]
  private Rigidbody2D needleRigidbody;
  [SerializeField]
  private Image reelingBarBG;
  [SerializeField]
  private Transform reelingBarParent;
  [SerializeField]
  private RectTransform targetSection;
  [Space]
  [SerializeField]
  private UIFishingOverlayController.Difficulty[] difficulties;
  [SerializeField]
  private RectTransform holdButtonAdjust;
  [SerializeField]
  private GameObject _reelButotnContainer;
  [SerializeField]
  private RectTransform holdButtonCast;
  [SerializeField]
  private GameObject _castButtonContainer;
  private StateMachine.State currentState;
  private GameObject lockPosition;
  private int difficulty;
  private Vector2 sectionMinVelocity;
  private Vector2 sectionMaxVelocity;
  private float sectionTimer;
  private float sectionDirection;
  private float sectionIncrease;
  private float sectionMin;
  private float sectionMax;
  private int rapidMode = -1;
  private float rapidModeChance;
  private float rapidModeTimer;
  private Vector2 rapidModeDuration = new Vector2(4f, 7.5f);
  private const float needleUpForce = 2000f;
  private const float needleDownForce = -50000f;
  private Vector3[] sectionWorldCorners = new Vector3[4];
  private Vector3[] needleWorldCorners = new Vector3[4];
  private EventInstance loopingSoundInstance;
  private Material _needleHighlightedMaterial;
  private bool cacheResult;
  private bool _changedUpState;
  private bool _changedDownState;

  private UIFishingOverlayController.Difficulty CurrentDifficulty
  {
    get => this.difficulties[this.difficulty];
  }

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

  protected override void OnShowStarted()
  {
    this.targetSection.anchorMin = new Vector2(this.targetSection.anchorMin.x, 0.4f);
    this.targetSection.anchorMax = new Vector2(this.targetSection.anchorMax.x, 0.5f);
    this.reelingNeedle.anchoredPosition = new Vector2(this.reelingNeedle.anchoredPosition.x, 0.0f);
    this._castButtonContainer.SetActive(!SettingsManager.Settings.Accessibility.AutoFish);
    this._reelButotnContainer.SetActive(!SettingsManager.Settings.Accessibility.AutoFish);
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

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

  private void BeginRapidMode()
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

  private void FixedUpdate()
  {
    if (this.currentState == StateMachine.State.Attacking)
    {
      if (InputManager.Gameplay.GetInteractButtonHeld())
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
          MMVibrate.RumbleContinuous(0.1f, 0.25f);
          AudioManager.Instance.PlayOneShot("event:/fishing/caught_something_tap", PlayerFarming.Instance.transform.position);
          this.reelingNeedleIcon.material = this._needleHighlightedMaterial;
          this._changedUpState = true;
        }
      }
      else
      {
        this._changedUpState = false;
        this.needleRigidbody.AddForce(Vector2.up * -50000f * Time.deltaTime);
        if (!this._changedDownState)
        {
          MMVibrate.StopRumble();
          MMVibrate.RumbleContinuous(0.0f, 0.025f);
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
