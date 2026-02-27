// Decompiled with JetBrains decompiler
// Type: UIFishing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFishing : BaseMonoBehaviour
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
  private Image reelingNeedleUpIcon;
  [SerializeField]
  private Image reelingNeedleDownIcon;
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
  private float needleUpForce;
  [SerializeField]
  private float needleDownForce;
  [Space]
  [SerializeField]
  private UIFishing.Difficulty[] difficulties;
  [SerializeField]
  private RectTransform holdButtonAdjust;
  [SerializeField]
  private RectTransform holdButtonCast;
  private StateMachine.State currentState;
  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;
  private Canvas canvas;
  private GameObject lockPosition;
  private bool hiding;
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
  private Vector3[] sectionWorldCorners = new Vector3[4];
  private Vector3[] needleWorldCorners = new Vector3[4];
  private EventInstance loopingSoundInstance;
  private Material _needleHighlightedMaterial;
  private bool cacheResult;
  private bool _changedUpState;
  private bool _changedDownState;

  private UIFishing.Difficulty CurrentDifficulty => this.difficulties[this.difficulty];

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

  private void Awake()
  {
    this._needleHighlightedMaterial = this.reelingNeedleIcon.material;
    this.rectTransform = this.GetComponent<RectTransform>();
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvas = this.GetComponentInParent<Canvas>();
  }

  public void Play(GameObject LockPosition)
  {
    this.lockPosition = LockPosition;
    this.gameObject.SetActive(true);
    this.hiding = false;
    this.targetSection.anchorMin = new Vector2(this.targetSection.anchorMin.x, 0.4f);
    this.targetSection.anchorMax = new Vector2(this.targetSection.anchorMax.x, 0.5f);
    this.reelingNeedle.anchoredPosition = new Vector2(this.reelingNeedle.anchoredPosition.x, 0.0f);
  }

  public void SetLockPosition(GameObject g) => this.lockPosition = g;

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

  public void Hide() => this.hiding = true;

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

  private void Update()
  {
    if (this.currentState == StateMachine.State.Attacking)
    {
      if (InputManager.Gameplay.GetInteractButtonHeld())
      {
        this._changedDownState = false;
        this.needleRigidbody.AddForce(Vector2.up * this.needleUpForce * Time.deltaTime);
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
          this.reelingNeedleUpIcon.gameObject.SetActive(true);
          this.reelingNeedleDownIcon.gameObject.SetActive(false);
          this._changedUpState = true;
        }
      }
      else
      {
        this._changedUpState = false;
        this.needleRigidbody.AddForce(Vector2.up * this.needleDownForce * Time.deltaTime);
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
          this.reelingNeedleUpIcon.gameObject.SetActive(false);
          this.reelingNeedleDownIcon.gameObject.SetActive(true);
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
      if (this.rapidMode != 1 || (double) Time.time <= (double) this.rapidModeTimer)
        return;
      this.SetReelingDifficulty(this.difficulty - 1);
      this.rapidMode = 0;
    }
    else
      MMVibrate.StopRumble();
  }

  private void LateUpdate()
  {
    if (!this.hiding)
    {
      if ((double) this.canvasGroup.alpha >= 1.0)
        return;
      this.canvasGroup.alpha += Time.deltaTime * 5f;
    }
    else if ((double) this.canvasGroup.alpha > 0.0)
    {
      this.canvasGroup.alpha -= 5f * Time.deltaTime;
    }
    else
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
    }
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
