// Decompiled with JetBrains decompiler
// Type: Indicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Indicator : MonoSingleton<Indicator>
{
  public Image BG;
  public Image ContainerImage;
  public TextMeshProUGUI text;
  public TextMeshProUGUI SecondaryText;
  public TextMeshProUGUI Thirdtext;
  public TextMeshProUGUI Fourthtext;
  private float TargetWidth;
  private float TargetWidthSpeed;
  private float width;
  private float height;
  public CanvasGroup canvasGroup;
  private bool closing;
  public RectTransform ControlPrompt;
  public RectTransform ControlPromptContainer;
  public RectTransform SecondaryControlPrompt;
  public RectTransform ThirdControlPrompt;
  public RectTransform FourthControlPrompt;
  public float Progress;
  [SerializeField]
  private RadialProgress _radialProgress;
  public float Margin = 120f;
  public float ControlPromptSpacing = 15f;
  private float PromptDist;
  public RectTransform CenterObject;
  public bool HoldToInteract;
  public bool Interactable = true;
  public bool HasSecondaryInteraction;
  public bool SecondaryInteractable = true;
  public bool HasThirdInteraction;
  public bool ThirdInteractable = true;
  public bool FourthInteractable = true;
  public bool HasFourthInteraction;
  public GameObject ControlPromptUI;
  public MMControlPrompt primaryControlPrompt;
  public MMControlPrompt secondaryControlPrompt;
  public MMControlPrompt thirdControlPrompt;
  public MMControlPrompt fourthControlPrompt;
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  public int Action = 9;
  public GameObject TopInfoContainer;
  public TMP_Text TopInfoText;
  public SimpleSFX sfx;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _container;
  private Vector2 _cachedPosition;
  private Vector2 _cachedContainerPosition;
  private Vector2 Easing = new Vector2(0.2f, 0.55f);
  private float ClosingAlphaSpeed = 5f;
  private Vector3 WorldPosition;
  private Vector2 Shake;
  private float ShakeSpeed;
  public bool UpdatePosition = true;
  public bool WorldPositionShake = true;
  public float ShakeAmount = 25f;
  public float bounce = 0.4f;
  public float easing = 0.8f;
  private bool hidden;
  private float cacheAlpha;
  private bool PlacementObjectEnabled;

  public RectTransform RectTransform => this._rectTransform;

  public Vector2 CachedPosition => this._cachedPosition;

  public override void Start()
  {
    if ((UnityEngine.Object) this.sfx == (UnityEngine.Object) null)
      this.sfx = this.GetComponent<SimpleSFX>();
    this._cachedPosition = this._rectTransform.anchoredPosition;
    this._cachedContainerPosition = this._container.anchoredPosition;
    this.HideTopInfo();
  }

  private void OnEnable()
  {
    this.closing = false;
    this.canvasGroup.alpha = 0.0f;
    this.TargetWidth = this.TargetWidthSpeed = 0.0f;
    Singleton<AccessibilityManager>.Instance.OnHoldActionToggleChanged += new System.Action<bool>(this.OnHoldActionToggleChanged);
    this.Reset();
  }

  private void OnDisable()
  {
    Singleton<AccessibilityManager>.Instance.OnHoldActionToggleChanged -= new System.Action<bool>(this.OnHoldActionToggleChanged);
  }

  private void OnHoldActionToggleChanged(bool value)
  {
    this._radialProgress.gameObject.SetActive(value);
  }

  public void Reset()
  {
    this.closing = false;
    this.ContainerImage.enabled = true;
    if (this.Interactable && this.text.text != "" && this.text.text != " ")
    {
      this.ControlPromptContainer.gameObject.SetActive(true);
      if (this.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions)
      {
        this.Progress = this._radialProgress.Progress = 0.0f;
        this._radialProgress.gameObject.SetActive(true);
      }
      else
      {
        this._radialProgress.gameObject.SetActive(false);
        this.Progress = this._radialProgress.Progress = 0.0f;
      }
    }
    else
    {
      this.ControlPromptContainer.gameObject.SetActive(false);
      this._radialProgress.gameObject.SetActive(false);
    }
    if (this.SecondaryInteractable && !string.IsNullOrEmpty(this.SecondaryText.text))
    {
      this.SecondaryControlPrompt.gameObject.SetActive(this.HasSecondaryInteraction);
    }
    else
    {
      this.SecondaryControlPrompt.gameObject.SetActive(false);
      this.SecondaryText.text = "";
    }
    if (this.ThirdInteractable && (UnityEngine.Object) this.Thirdtext != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.Thirdtext.text))
    {
      this.ThirdControlPrompt.gameObject.SetActive(this.HasThirdInteraction);
    }
    else
    {
      this.ThirdControlPrompt.gameObject.SetActive(false);
      this.Thirdtext.text = "";
    }
    if (this.FourthInteractable && (UnityEngine.Object) this.Fourthtext != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.Fourthtext.text))
    {
      this.FourthControlPrompt.gameObject.SetActive(this.HasFourthInteraction);
    }
    else
    {
      this.FourthControlPrompt.gameObject.SetActive(false);
      this.Fourthtext.text = "";
    }
    if (!string.IsNullOrEmpty(this.text.text) || !string.IsNullOrEmpty(this.SecondaryText.text) || !string.IsNullOrEmpty(this.Thirdtext.text) || !string.IsNullOrEmpty(this.Fourthtext.text))
      return;
    this.ContainerImage.enabled = false;
  }

  public void ShowTopInfo(string text)
  {
    if (!(bool) (UnityEngine.Object) this.TopInfoContainer)
      return;
    this.TopInfoContainer.SetActive(true);
    this.TopInfoText.text = text;
  }

  public void HideTopInfo()
  {
    if (!(bool) (UnityEngine.Object) this.TopInfoContainer)
      return;
    this.TopInfoContainer.SetActive(false);
  }

  public void Deactivate()
  {
    if (this.closing)
      return;
    this.closing = true;
  }

  public void Activate()
  {
    this.HideTopInfo();
    if (!(bool) (UnityEngine.Object) Interactor.CurrentInteraction)
      return;
    Interactor.CurrentInteraction.HasChanged = true;
  }

  private void SetSize()
  {
    this.PromptDist = (float) ((double) this.text.preferredWidth / 2.0 + (double) this.ControlPrompt.rect.width / 2.0) + this.ControlPromptSpacing;
    this.height = this.text.preferredHeight + 20f;
    this.BG.rectTransform.sizeDelta = new Vector2(this.width = this.text.preferredWidth + this.ControlPrompt.rect.width + this.Margin, this.height);
  }

  public void SetPosition(Vector3 WorldPosition) => this.WorldPosition = WorldPosition;

  private void LateUpdate()
  {
    if (!this.UpdatePosition)
      return;
    this._container.anchoredPosition = this._cachedContainerPosition + this.Shake;
  }

  public void PlayShake()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
    this.ShakeSpeed = this.ShakeAmount * ((double) UnityEngine.Random.value < 0.5 ? -1f : 1f);
  }

  private void Update()
  {
    if ((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null)
    {
      this.PlacementObjectEnabled = true;
      this.hidden = false;
      this.canvasGroup.alpha = 1f;
    }
    else
    {
      if (this.PlacementObjectEnabled)
      {
        this.Deactivate();
        this.PlacementObjectEnabled = false;
      }
      if ((double) Time.timeScale == 0.0)
      {
        this.hidden = true;
        this.cacheAlpha = this.canvasGroup.alpha;
        this.canvasGroup.alpha = 0.0f;
      }
      if (this.hidden)
      {
        if ((double) Time.timeScale <= 0.0)
          return;
        this.hidden = false;
        this.canvasGroup.alpha = this.cacheAlpha;
      }
      else if (!this.closing && (UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) null)
      {
        if ((double) this.canvasGroup.alpha < 1.0)
          this.canvasGroup.alpha += 20f * Time.deltaTime;
        this.PromptDist = (float) ((double) this.text.preferredWidth / 2.0 + (double) this.ControlPrompt.rect.width / 2.0) + this.ControlPromptSpacing;
        if (this._radialProgress.gameObject.activeSelf)
          this._radialProgress.Progress = this.Progress;
        this.width = this.text.preferredWidth + this.ControlPrompt.rect.width + this.Margin;
        this.TargetWidthSpeed += (this.width - this.TargetWidth) * this.Easing.x;
        this.TargetWidth += (this.TargetWidthSpeed *= this.Easing.y);
        this.height = this.text.preferredHeight + 20f;
        this.BG.rectTransform.sizeDelta = new Vector2(this.TargetWidth, this.height);
      }
      else
      {
        if ((double) this.canvasGroup.alpha > 0.0)
          this.canvasGroup.alpha -= this.ClosingAlphaSpeed * Time.deltaTime;
        if ((double) this.canvasGroup.alpha > 0.0)
          return;
        this.text.text = "";
        this.gameObject.SetActive(false);
      }
    }
  }

  private void FixedUpdate()
  {
    this.ShakeSpeed += (0.0f - this.Shake.x) * this.bounce;
    this.Shake += new Vector2(this.ShakeSpeed *= this.easing, 0.0f);
  }
}
